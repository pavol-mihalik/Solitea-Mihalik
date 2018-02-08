using System.Data.Entity;

namespace Solitea_Mihalik.DAL
{
    public class AdresarDBContext : DbContext
    {
        public AdresarDBContext() : base("AdresarDB")
        {
        }

        public virtual DbSet<Osoba> Osoby { get; set; }
        public virtual DbSet<Adresa> Adresy { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<AdresarDBContext>(null);
            modelBuilder.Entity<Osoba>().ToTable("Osoba");
            modelBuilder.Entity<Adresa>().ToTable("Adresa");
            base.OnModelCreating(modelBuilder);
        }
    }
}