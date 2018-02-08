using System.Collections.Generic;

namespace Solitea_Mihalik.DAL
{
    public class Osoba
    {
        public int ID { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string IC { get; set; }
        public string DIC { get; set; }

        public virtual ICollection<Adresa> Adresy { get; set; }
    }
}