using System.Collections.Generic;

namespace Solitea_Mihalik.Models
{
    public class OsobaViewModel
    {
        public int ID { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string IC { get; set; }
        public string DIC { get; set; }

        public List<AdresaViewModel> Adresy { get; set; }
    }
}
