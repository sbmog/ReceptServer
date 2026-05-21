using Microsoft.EntityFrameworkCore;
using Models;

namespace DAL
{
    public class ReceptRepository
    {
        private readonly ReceptContext _context;

        public ReceptRepository()
        {
            //forbindelse til DB
            _context = new ReceptContext();
        }

        public List<Recept> HentRecepterPåCpr(string cpr)
        {
            return _context.Recepter
                .Include(r => r.Ordinationer) // Eager Loading: henter Ordination og lægehuset med i samme SQL-kald (JOIN)
                .Include(r => r.Lægehus) 
                .Where(r => r.PatientCpr == cpr && !r.ErLukket)
                .ToList();
        }

        public void OpretRecept(Recept nyRecept)
        {
            _context.Recepter.Add(nyRecept);
            _context.SaveChanges();
        }

        public Recept? HentRecept(int id)
        {
            return _context.Recepter
                .Include(r => r.Ordinationer)
                .Include(r => r.Lægehus)
                .FirstOrDefault(r => r.Id == id);
        }

        public void OpdaterOrdination (Ordination ordination)
        {
            _context.Ordinationer.Update(ordination);
            _context.SaveChanges();
        }

        public Lægehus? HentLægehus(string ydernummer)
        {
            return _context.Lægehuse.FirstOrDefault(l => l.Ydernummer == ydernummer);
        }

        public void OpdaterRecept(Recept recept)
        {
            _context.Recepter.Update(recept);
            _context.SaveChanges();
        }
    }
}
