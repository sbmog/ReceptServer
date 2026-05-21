using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class ReceptManager
    {
        private readonly ReceptRepository _repository;

        public ReceptManager()
        {
            _repository = new ReceptRepository();
        }

        public List<Recept> HentRecepterPåCpr(string cpr)
        {
            if (string.IsNullOrWhiteSpace(cpr) || cpr.Length != 10)
            {
                throw new ArgumentException("CPR-nummer skal være præcis 10 tegn uden bindestreg.");
            }
            var recepter = _repository.HentRecepterPåCpr(cpr);
            foreach (var recept in recepter)
            {
                if (!recept.ErLukket && recept.OprettetDato <= DateTime.Now.AddYears(-2))
                {
                    recept.ErLukket = true;
                    _repository.OpdaterRecept(recept);

                }
            }
            return recepter.Where(r=>!r.ErLukket).ToList();
        }

        public void OpretRecept(Recept nyRecept)
        {
            if (string.IsNullOrWhiteSpace(nyRecept.LægehusYdernummer) || nyRecept.LægehusYdernummer.Length != 6)
            {
                throw new ArgumentException("Ydernummer skal være præcis 6 tegn.");
            }

            if (nyRecept.Ordinationer == null || nyRecept.Ordinationer.Count == 0)
            {
                throw new ArgumentException("En recept skal indeholde mindst én ordination.");
            }

            foreach (var ord in nyRecept.Ordinationer)
            {
                if (string.IsNullOrWhiteSpace(ord.Lægemiddel))
                {
                    throw new ArgumentException("Lægemiddelnavn må ikke være tomt.");
                }

                if (ord.AntalUdleveringer <= 0)
                {
                    throw new ArgumentException("Antal udleveringer skal være større end 0.");
                }
            }

            _repository.OpretRecept(nyRecept);
        }
        public Recept HentRecept(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id skal være større end 0.");
            }
            return _repository.HentRecept(id);
        }

        public void ForetagUdlevering(Ordination ordination)
        {
            if (ordination == null)
            {
                throw new ArgumentNullException(nameof(ordination), "Ordinationen findes ikke.");
            }
            if (ordination.ErFuldtUdleveret)
            {
                throw new Exception("Der kan ikke foretages flere udleveringer på denne ordination, da den allerede er fuldt udleveret.");
            }

            ordination.AntalForetagneUdleveringer++;
            _repository.OpdaterOrdination(ordination);

            var recept = _repository.HentRecept(ordination.ReceptId);
            if (recept != null)
            {
                if (recept.Ordinationer.All(o => o.ErFuldtUdleveret))
                {
                    recept.ErLukket = true;
                    _repository.OpdaterRecept(recept);
                }
            }
        }

        public Lægehus HentLægehus(string ydernummer)
        {
            if (string.IsNullOrWhiteSpace(ydernummer) || ydernummer.Length != 6)
            {
                throw new ArgumentException("Ydernummer skal være præcis 6 tegn.");
            }
            return _repository.HentLægehus(ydernummer);
        }
    }
}
