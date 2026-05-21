using Models;
using Xunit;

namespace BLL.Tests
{
    public class ReceptManagerTests
    {
        [Fact]
        public void Ordination_ErFuldtUdleveret_ReturnererTrue_NaarMaxErNaaet()
        {
            //Arrange
            var ordination = new Ordination
            {
                AntalUdleveringer = 3,
                AntalForetagneUdleveringer = 3
            };

            //Act
            bool resultat = ordination.ErFuldtUdleveret;

            //Assert
            Assert.True(resultat);
        }

        [Fact]
        public void Ordination_ErFuldtUdleveret_ReturnererFalse_NaarDerErMereTilbage()
        {
            //Arrange
            var ordination = new Ordination
            {
                AntalUdleveringer = 4,
                AntalForetagneUdleveringer = 1
            };

            //Act
            bool resultat = ordination.ErFuldtUdleveret;

            //Assert
            Assert.False(resultat);
        }

        [Fact]
        public void Recept_TilfoejelseAfMedicin_RegistreresKorrektIListe()
        {
            //Arrange
            var recept = new Recept
            {
                Ordinationer = new List<Ordination>()
            };

            var medicin1 = new Ordination { Lægemiddel = "Pamol" };
            var medicin2 = new Ordination { Lægemiddel = "Ipren" };

            //Act
            recept.Ordinationer.Add(medicin1);
            recept.Ordinationer.Add(medicin2);

            //Assert
            Assert.Equal(2, recept.Ordinationer.Count);
        }

        [Fact]
        public void ForetagUdlevering_KasterException_NaarFuldtUdleveret()
        {
            //Arrange
            var manager = new ReceptManager();
            var ordination = new Ordination
            {
                AntalUdleveringer = 2,
                AntalForetagneUdleveringer = 2
            };

            //Act & Assert
            Assert.Throws<Exception>(() => manager.ForetagUdlevering(ordination));
        }

        [Fact]
        public void HentRecepterPåCpr_KasterArgumentException_VedForKortCpr()
        {
            //Arrange
            var manager = new ReceptManager();

            //Act & Assert
            Assert.Throws<ArgumentException>(() => manager.HentRecepterPåCpr("1234"));
        }

        [Fact]
        public void OpretRecept_KasterArgumentException_HvisIngenOrdinationerFindes()
        {
            //Arrange
            var manager = new ReceptManager();
            var receptUdenMedicin = new Recept
            {
                PatientCpr = "1212121212",
                LægehusYdernummer = "123456",
                Ordinationer = new List<Ordination>()
            };

            //Act & Assert
            Assert.Throws<ArgumentException>(() => manager.OpretRecept(receptUdenMedicin));
        }
    }
}