namespace Solitea_Mihalik.DAL
{
    public class Adresa
    {
        public int ID { get; set; }
        public string Ulice { get; set; }
        public string Mesto { get; set; }
        public string PSC { get; set; }
        public int OsobaID { get; set; }

        public virtual Osoba Osoba { get; set; }
    }
}