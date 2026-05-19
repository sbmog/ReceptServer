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
            return _repository.HentRecepterPåCpr(cpr);
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
        }
    }
}
