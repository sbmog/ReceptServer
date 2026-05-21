using Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;

namespace LægeUI
{
    public partial class MainWindow : Window
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private Lægehus? _aktueltLægehus;
        private System.Collections.Generic.List<Ordination> _nyeOrdinationer = new System.Collections.Generic.List<Ordination>();

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
                HttpResponseMessage response = await _httpClient.GetAsync($"api/lægehus/{ydernummer}");

                if (response.IsSuccessStatusCode)
                {
                    // læser lægehuset fra JSON-svaret
                    _aktueltLægehus = await response.Content.ReadFromJsonAsync<Lægehus>();

                    // skifter visning fra login
                    LoginPanel.Visibility = Visibility.Collapsed;
                    ArbejdsPanel.Visibility = Visibility.Visible;
                    VelkomstTekst.Text = $"Logget ind som: {_aktueltLægehus.Navn} ({_aktueltLægehus.Ydernummer})";
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

            // Simpel validering
            if (string.IsNullOrWhiteSpace(MedicinInput.Text) || string.IsNullOrWhiteSpace(DosisInput.Text) || !int.TryParse(UdleveringerInput.Text, out int antal))
            {
                InputFejlBesked.Text = "Udfyld venligst alle medicin-felter korrekt. Antal udleveringer skal være et tal.";
                return;
            }

            // Opretter en ny ordination lokalt i WPF
            var nyOrdination = new Ordination
            {
                Lægemiddel = MedicinInput.Text,
                Dosering = DosisInput.Text,
                AntalUdleveringer = antal,
                AntalForetagneUdleveringer = 0
            };

            // Tilføjer til listen og opdaterer UI
            _nyeOrdinationer.Add(nyOrdination);
            MedicinListe.ItemsSource = null; // Nulstil for at opdatere visningen
            MedicinListe.ItemsSource = _nyeOrdinationer;

            // Ryd felterne så lægen kan indtaste mere medicin
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

            // Bygger selve recepten der skal sendes til API'et
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
                // Sender HTTP POST til vores API endpoint (RecepterController)
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/recepter", nyRecept);

                if (response.IsSuccessStatusCode)
                {
                    OpretStatusBesked.Foreground = System.Windows.Media.Brushes.Green;
                    OpretStatusBesked.Text = "Recepten er nu oprettet i databasen!";

                    // Ryd alt op, så systemet er klar til næste patient
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