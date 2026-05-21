using Models;
using Xunit;
using System;
using System.Collections.Generic;

namespace BLL.Tests
{
    public class ReceptManagerTests
    {
        [Fact]
        public void Ordination_ErFuldtUdleveret_ReturnererTrue_NaarMaxErNaaet()
        {
            var ordination = new Ordination
            {
                AntalUdleveringer = 3,
                AntalForetagneUdleveringer = 3
            };

            bool resultat = ordination.ErFuldtUdleveret;

            Assert.True(resultat);
        }

        [Fact]
        public void Ordination_ErFuldtUdleveret_ReturnererFalse_NaarDerErMereTilbage()
        {
            var ordination = new Ordination
            {
                AntalUdleveringer = 4,
                AntalForetagneUdleveringer = 1
            };

            bool resultat = ordination.ErFuldtUdleveret;

            Assert.False(resultat);
        }

        [Fact]
        public void Recept_TilfoejelseAfMedicin_RegistreresKorrektIListe()
        {
            var recept = new Recept
            {
                Ordinationer = new List<Ordination>()
            };

            var medicin1 = new Ordination { Lægemiddel = "Pamol" };
            var medicin2 = new Ordination { Lægemiddel = "Ipren" };

            recept.Ordinationer.Add(medicin1);
            recept.Ordinationer.Add(medicin2);

            Assert.Equal(2, recept.Ordinationer.Count);
        }

        [Fact]
        public void ForetagUdlevering_KasterException_NaarFuldtUdleveret()
        {
            var manager = new ReceptManager();
            var ordination = new Ordination
            {
                AntalUdleveringer = 2,
                AntalForetagneUdleveringer = 2
            };

            Assert.Throws<Exception>(() => manager.ForetagUdlevering(ordination));
        }

        [Fact]
        public void HentRecepterPåCpr_KasterArgumentException_VedForKortCpr()
        {
            var manager = new ReceptManager();

            Assert.Throws<ArgumentException>(() => manager.HentRecepterPåCpr("1234"));
        }

        [Fact]
        public void OpretRecept_KasterArgumentException_HvisIngenOrdinationerFindes()
        {
            var manager = new ReceptManager();
            var receptUdenMedicin = new Recept
            {
                PatientCpr = "1212121212",
                LægehusYdernummer = "123456",
                Ordinationer = new List<Ordination>()
            };

            Assert.Throws<ArgumentException>(() => manager.OpretRecept(receptUdenMedicin));
        }
    }
}