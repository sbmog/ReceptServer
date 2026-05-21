
using System.Text.Json.Serialization;

namespace Models
{
    public class Recept
    {
        public int Id { get; set; }
        public string? PatientCpr { get; set; }
        [JsonIgnore] // Forhindrer uendelige løkker
        public Lægehus? Lægehus { get; set; }
        public string? LægehusYdernummer { get; set; }
        public DateTime OprettetDato { get; set; } = DateTime.Now;
        public bool ErLukket { get; set; } = false;
        public List<Ordination> Ordinationer { get; set; } = new List<Ordination>();
    }
}
