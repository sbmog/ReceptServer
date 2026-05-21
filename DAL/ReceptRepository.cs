using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class ReceptRepository
    {
        private readonly ReceptContext _context;

        public ReceptRepository()
        {
            _context = new ReceptContext();
        }

        public List<Recept> HentRecepterPåCpr(string cpr)
        {
            return _context.Recepter
                .Include(r => r.Ordinationer) //finder tilknyttet medicin
                .Include(r => r.LægehusYdernummer) //finder tilknyttet lægehus)
                    .ThenInclude(l => l.Navn) //finder tilknyttet læge
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
    }
}
