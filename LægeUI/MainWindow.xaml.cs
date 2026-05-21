using Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;

namespace LægeUI
{
    public partial class MainWindow : Window
    {
        //statisk for ikke at løbe tør for netværks-sockets
        private static readonly HttpClient _httpClient = new HttpClient();
        private Lægehus? _aktueltLægehus;
        private List<Ordination> _nyeOrdinationer = new List<Ordination>();

        public MainWindow()
        {
            InitializeComponent();

            _httpClient.BaseAddress = new Uri("https://localhost:7106/");
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string ydernummer = YdernummerInput.Text;
            LoginFejlBesked.Text = "";

            if (string.IsNullOrWhiteSpace(ydernummer))
            {
                LoginFejlBesked.Foreground = System.Windows.Media.Brushes.Red;
                LoginFejlBesked.Text = "Ydernummer må ikke være tomt.";
                return;
            }

            LoginFejlBesked.Foreground = System.Windows.Media.Brushes.Black;
            LoginFejlBesked.Text = "Logger ind, vent venligst...";
            if (sender is Button btn) btn.IsEnabled = false;

            try
            {
                // Await: programmet pauser og frigiver UI-tråden, indtil API'et svarer
                HttpResponseMessage response = await _httpClient.GetAsync($"api/lægehus/{ydernummer}");

                if (response.IsSuccessStatusCode)
                {
                    // læser lægehuset fra JSON-svaret
                    _aktueltLægehus = await response.Content.ReadFromJsonAsync<Lægehus>();

                    // skifter visning fra login
                    LoginPanel.Visibility = Visibility.Collapsed;
                    ArbejdsPanel.Visibility = Visibility.Visible;
                    if (_aktueltLægehus != null)
                    {
                        VelkomstTekst.Text = $"Logget ind som: {_aktueltLægehus.Navn} ({_aktueltLægehus.Ydernummer})";
                    }
                }
                else
                {
                    LoginFejlBesked.Foreground = System.Windows.Media.Brushes.Red;
                    LoginFejlBesked.Text = "Ugyldigt ydernummer. Lægehuset findes ikke.";
                }
            }
            catch (Exception)
            {
                LoginFejlBesked.Foreground = System.Windows.Media.Brushes.Red;
                LoginFejlBesked.Text = "Kunne ikke oprette forbindelse til API'et. Kører det?";
            }
            finally
            {
                if (sender is Button button) button.IsEnabled = true;
            }
        }

        private void TilfoejMedicin_Click(object sender, RoutedEventArgs e)
        {
            InputFejlBesked.Text = "";

            if (string.IsNullOrWhiteSpace(MedicinInput.Text) || string.IsNullOrWhiteSpace(DosisInput.Text) || !int.TryParse(UdleveringerInput.Text, out int antal))
            {
                InputFejlBesked.Text = "Udfyld venligst alle medicin-felter korrekt. Antal udleveringer skal være et tal.";
                return;
            }

            var nyOrdination = new Ordination
            {
                Lægemiddel = MedicinInput.Text,
                Dosering = DosisInput.Text,
                AntalUdleveringer = antal,
                AntalForetagneUdleveringer = 0
            };

            _nyeOrdinationer.Add(nyOrdination);
            MedicinListe.ItemsSource = null; //nulstil for at opdatere visningen
            MedicinListe.ItemsSource = _nyeOrdinationer;

            //ryd felterne så lægen kan indtaste mere medicin til samme recept
            MedicinInput.Clear();
            DosisInput.Clear();
            UdleveringerInput.Clear();
        }

        private async void OpretRecept_Click(object sender, RoutedEventArgs e)
        {
            OpretStatusBesked.Foreground = System.Windows.Media.Brushes.Black;
            OpretStatusBesked.Text = "Sender recept...";

            if (string.IsNullOrWhiteSpace(CprInput.Text) || _nyeOrdinationer.Count == 0)
            {
                OpretStatusBesked.Foreground = System.Windows.Media.Brushes.Red;
                OpretStatusBesked.Text = "Du skal indtaste et CPR-nummer og tilføje mindst én medicin.";
                return;
            }

            if (_aktueltLægehus == null)
            {
                OpretStatusBesked.Foreground = System.Windows.Media.Brushes.Red;
                OpretStatusBesked.Text = "Du skal være logget ind for at oprette en recept.";
                return;
            }

            //bygger selve recepten der skal sendes til API'et
            var nyRecept = new Recept
            {
                PatientCpr = CprInput.Text,
                LægehusYdernummer = _aktueltLægehus.Ydernummer,
                OprettetDato = DateTime.Now,
                ErLukket = false,
                Ordinationer = _nyeOrdinationer
            };

            try
            {
                // PostAsJsonAsync konverterer (serialiserer) automatisk C# recept til JSON og sender det afsted til controlleren
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/recepter", nyRecept);

                if (response.IsSuccessStatusCode)
                {
                    OpretStatusBesked.Foreground = System.Windows.Media.Brushes.Green;
                    OpretStatusBesked.Text = "Recepten er nu oprettet i databasen!";

                    // blanke felter
                    CprInput.Clear();
                    _nyeOrdinationer.Clear();
                    MedicinListe.ItemsSource = null;
                }
                else
                {
                    OpretStatusBesked.Foreground = System.Windows.Media.Brushes.Red;
                    OpretStatusBesked.Text = "Der skete en fejl. Recepten blev ikke oprettet.";
                }
            }
            catch (Exception)
            {
                OpretStatusBesked.Foreground = System.Windows.Media.Brushes.Red;
                OpretStatusBesked.Text = "Kunne ikke få forbindelse til serveren.";
            }
        }
    }
}