using System.Collections.Generic;
using bl = Solitea_Mihalik.BL;

namespace Solitea_Mihalik.DAL
{
    public interface IAdresarRepozitar
    {
        Osoba NactiOsobuDB(int id);
        List<Osoba> NactiAdresarDB();
        int UlozOsobuDB(bl.Osoba osoba);
        int UlozAdresuDoDB(Adresa adresa);
        bool SmazOsobuDB(int id);
        bool SmazAdresuDB(int id);
    }
}