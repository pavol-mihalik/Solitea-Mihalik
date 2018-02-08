using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solitea_Mihalik.BL;
using System.Collections.Generic;

namespace Solitea_Mihalik_Test
{
    [TestClass]
    public class AdresarTest
    {
        [TestMethod]
        public void UlozOsobuTest()
        {
            Osoba novaOsoba = new Osoba()
            {
                Jmeno = "Pavol",
                Prijmeni = "Mihálik",
                IC = "86554441",
                DIC = "CZ86554441"
            };
            var facade = new AdresarFacade();
            int idOsoby = facade.UlozOsobu(novaOsoba);
            Assert.AreNotEqual(0, idOsoby);
        }

        [TestMethod]
        public void UlozAdresuTest()
        {
            var facade = new AdresarFacade();

            Osoba novaOsoba = new Osoba()
            {
                Jmeno = "Pavol",
                Prijmeni = "Mihálik",
                IC = "86554441",
                DIC = "CZ86554441"
            };
            int idOsoby = facade.UlozOsobu(novaOsoba);

            Adresa novaAdresa = new Adresa()
            {
                Ulice = "Kabátníkova",
                Mesto = "Brno",
                PSC = "60600",
                OsobaID = idOsoby,
                Osoba = novaOsoba,
            };
            int idAdresy = facade.UlozAdresu(novaAdresa);
            Assert.AreNotEqual(0, idAdresy);
        }

        [TestMethod]
        public void NactiAdresarTest()
        {
            var facade = new AdresarFacade();
            List<Osoba> adresar = facade.NactiAdresar();
            Assert.IsTrue(adresar.Count > 0);
        }

        [TestMethod]
        public void NactiOsobuTest()
        {
            var facade = new AdresarFacade();
            Osoba osoba = facade.NactiOsobu(1);
            Assert.AreEqual("86554441", osoba.IC);
        }

        [TestMethod]
        public void SmazOsobuTest()
        {
            var facade = new AdresarFacade();
            var vysledek = facade.SmazOsobu(1);
            var nacteni = facade.NactiOsobu(1);
            Assert.IsTrue(vysledek);
            Assert.IsNull(nacteni);
        }
    }
}
