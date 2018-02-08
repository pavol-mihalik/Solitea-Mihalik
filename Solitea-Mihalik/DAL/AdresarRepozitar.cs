using AutoMapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using bl = Solitea_Mihalik.BL;

namespace Solitea_Mihalik.DAL
{
    public class AdresarRepozitar : IAdresarRepozitar
    {
        private AdresarDBContext _db;
        private IMapper _mapper;
        private string connString;

        public AdresarRepozitar(AdresarDBContext context)
        {
            if (context == null)
            {
                _db = new AdresarDBContext();
            }
            else
            {
                _db = context;
            }

            connString = ConfigurationManager.ConnectionStrings["AdresarDB"].ToString();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<bl.Osoba, Osoba>();
                cfg.CreateMap<bl.Adresa, Adresa>();
            });
            _mapper = config.CreateMapper();
        }

        public List<Osoba> NactiAdresarDB()
        {
            return _db.Osoby.ToList();
        }

        public Osoba NactiOsobuDB(int id)
        {
            return _db.Osoby.Where(osoba => osoba.ID == id).FirstOrDefault();
        }

        public int UlozOsobuDB(bl.Osoba novaOsoba)
        {
            var osobaDal = _db.Osoby.Where(osoba => osoba.ID == novaOsoba.ID).FirstOrDefault();

            if (osobaDal != null)
            {
                VyvtorMapovani(osobaDal, novaOsoba);
                _db.Entry(osobaDal).State = EntityState.Modified;
                _db.SaveChanges();
            }
            else
            {
                var result = _mapper.Map<Osoba>(novaOsoba);
                _db.Osoby.Add(result);
                _db.SaveChanges();
            }

            return novaOsoba.ID;
        }

        public int UlozAdresuDoDB(Adresa adresa)
        {
            _db.Adresy.Add(adresa);
            _db.SaveChanges();
            return adresa.ID;
        }

        public bool SmazOsobuDB(int id)
        {
            Osoba osobaNaSmazani = this.NactiOsobuDB(id);
            _db.Osoby.Remove(osobaNaSmazani);
            return _db.SaveChanges() > 0;
        }

        public bool SmazAdresuDB(int id)
        {
            Adresa adresaNaSmazani = _db.Adresy.Where(adresa => adresa.ID == id).FirstOrDefault();
            _db.Adresy.Remove(adresaNaSmazani);
            return _db.SaveChanges() > 0;
        }

        private void VyvtorMapovani(Osoba osobaDal, bl.Osoba novaOsoba)
        {
            osobaDal.Jmeno = novaOsoba.Jmeno;
            osobaDal.Prijmeni = novaOsoba.Prijmeni;
            osobaDal.IC = novaOsoba.IC;
            osobaDal.DIC = novaOsoba.DIC;

            var adresy = osobaDal.Adresy.ToList();
            var noveAdresy = novaOsoba.Adresy.ToList();

            for (int i = 0; i < noveAdresy.Count; i++)
            {
                var adresaDal = adresy.Where(adresa => adresa.ID == noveAdresy[i].ID).FirstOrDefault();
                adresaDal.Ulice = noveAdresy[i].Ulice;
                adresaDal.Mesto = noveAdresy[i].Mesto;
                adresaDal.PSC = noveAdresy[i].PSC;
            }
        }
    }
}