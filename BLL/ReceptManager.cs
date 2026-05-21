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
            _repository.OpretRecept(nyRecept);
        }
        public Recept HentRecept(int id)
        {
            return _repository.HentRecept(id);
        }

        public void ForetagUdlevering(Ordination ordination)
        {
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
            return _repository.HentLægehus(ydernummer);
        }
    }
}
