
using System.Text.Json.Serialization;

namespace Models
{
    public class Ordination
    {
        public int Id { get; set; }//PK

        [JsonIgnore] //forhindrer uendelige løkker (cirkulære referencer), når API'et konverterer data til JSON.
        public Recept? Recept { get; set; }
        public int ReceptId { get; set; } //FK
        public string? Lægemiddel { get; set; }
        public string? Dosering { get; set; }
        public int AntalUdleveringer { get; set; }
        public int AntalForetagneUdleveringer { get; set; } = 0;

        public bool ErFuldtUdleveret => AntalForetagneUdleveringer >= AntalUdleveringer;
    }
}
