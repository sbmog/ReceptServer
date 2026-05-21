
using System.Text.Json.Serialization;

namespace Models
{
    public class Recept
    {
        public int Id { get; set; } //PK
        public string? PatientCpr { get; set; }
        [JsonIgnore] //forhindrer uendelige løkker (cirkulære referencer), når API'et konverterer data til JSON.
        public Lægehus? Lægehus { get; set; }
        public string? LægehusYdernummer { get; set; } //FK
        public DateTime OprettetDato { get; set; } = DateTime.Now;
        public bool ErLukket { get; set; } = false;
        public List<Ordination> Ordinationer { get; set; } = new List<Ordination>();
    }
}
