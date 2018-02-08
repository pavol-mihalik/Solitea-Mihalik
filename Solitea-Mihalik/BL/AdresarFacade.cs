using AutoMapper;
using Solitea_Mihalik.DAL;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using bl = Solitea_Mihalik.BL;
using dal = Solitea_Mihalik.DAL;

namespace Solitea_Mihalik.BL
{
    public class AdresarFacade
    {
        private IAdresarRepozitar _adresarRepozitar;
        public IAdresarRepozitar AdresarRepozitar
        {
            get
            {
                if (_adresarRepozitar == null)
                {
                    _adresarRepozitar = new AdresarRepozitar(null);
                }
                return _adresarRepozitar;
            }
            set
            {
                _adresarRepozitar = value;
            }
        }
        private IMapper _mapper;

        public AdresarFacade()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<bl.Osoba, dal.Osoba>();
                cfg.CreateMap<dal.Osoba, bl.Osoba>();
                cfg.CreateMap<bl.Adresa, dal.Adresa>();
                cfg.CreateMap<dal.Adresa, bl.Adresa>();
            });
            _mapper = config.CreateMapper();
        }

        public List<Osoba> NactiAdresar()
        {
            return _mapper.Map<List<Osoba>>(this.AdresarRepozitar.NactiAdresarDB());
        }

        public Osoba NactiOsobu(int id)
        {
            return _mapper.Map<Osoba>(this.AdresarRepozitar.NactiOsobuDB(id));
        }

        /// <summary>
        /// Kontrola formátu IČ, ověření, jestli je dané IČ dostupné pře API Aresu.
        /// Kontrola vyplnění jména a příjmení osoby.
        /// </summary>
        /// <returns>ID právě uložené osoby, 0 pokud IČ nebo jméno není validní.</returns>
        public int UlozOsobu(Osoba osoba)
        {
            if (!OverIcFormat(osoba.IC) || !OverIcAres(osoba.IC) || 
                string.IsNullOrWhiteSpace(osoba.Jmeno) || string.IsNullOrWhiteSpace(osoba.Jmeno))
            {
                return 0;
            }

            //return this.AdresarRepozitar.UlozOsobuDoDB(_mapper.Map<dal.Osoba>(osoba));
            return this.AdresarRepozitar.UlozOsobuDB(osoba);
        }

        // Vrací ID právě uložené adresy osoby.
        public int UlozAdresu(Adresa adresa)
        {
            return this.AdresarRepozitar.UlozAdresuDoDB(_mapper.Map<dal.Adresa>(adresa));
        }

        // Vrací true, pokud se smazání osoby úspěšně povedlo.
        public bool SmazOsobu(int id)
        {
            return this.AdresarRepozitar.SmazOsobuDB(id);
        }

        // Vrací true, pokud se smazání adresy úspěšně povedlo.
        public bool SmazAdresu(int id)
        {
            return this.AdresarRepozitar.SmazAdresuDB(id);
        }

        private bool OverIcAres(string ic)
        {
            string aresApiUrl = "http://wwwinfo.mfcr.cz/cgi-bin/ares/darv_std.cgi?ico=";
            WebRequest request = WebRequest.Create(aresApiUrl + ic);
            XmlDocument document = new XmlDocument();
            using (Stream stream = request.GetResponse().GetResponseStream())
            {
                XDocument doc = XDocument.Load(stream);
                XNamespace ns = doc.Root.Name.Namespace;
                string pocetZaznamu = doc.Root.Descendants().Elements(ns + "Pocet_zaznamu").FirstOrDefault()?.Value;

                if (int.TryParse(pocetZaznamu, out int vysledek))
                {
                    return vysledek > 0;
                }
            }

            return false;
        }

        private bool OverIcFormat(string ic)
        {
            if (ic.Length != 8)
            {
                return false;
            }

            int soucet = 0;
            for (int i = 0; i < ic.Length - 1; i++)
            {
                if (!int.TryParse(ic.Substring(i, 1), out int cislice))
                {
                    return false;
                }
                soucet += cislice * (8 - i);
            }

            if (!int.TryParse(ic.Substring(ic.Length - 1, 1), out int kontrolniCislice))
            {
                return false;
            }

            if (kontrolniCislice != (11 - (soucet % 11)) % 10)
            {
                return false;
            }

            return true;
        }
    }
}