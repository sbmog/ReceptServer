using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Lægehus
    {
        [Key]
        public string? Ydernummer { get; set; }
        public string? Navn { get; set; }
        public string? Adresse { get; set; }
        public List<Recept> Recepter { get; set; } = new List<Recept>();
    }
}
