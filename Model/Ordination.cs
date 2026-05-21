
using System.Text.Json.Serialization;

namespace Models
{
    public class Ordination
    {
        public int Id { get; set; }
        [JsonIgnore] // Forhindrer uendelige løkker når Recept og Ordination peger på hinanden
        public Recept? Recept { get; set; }
        public int ReceptId { get; set; }
        public string? Lægemiddel { get; set; }
        public string? Dosering { get; set; }
        public int AntalUdleveringer { get; set; }
        public int AntalForetagneUdleveringer { get; set; } = 0;

        public bool ErFuldtUdleveret => AntalForetagneUdleveringer >= AntalUdleveringer;
    }
}
