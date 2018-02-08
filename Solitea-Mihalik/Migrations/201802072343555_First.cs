namespace Solitea_Mihalik.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adresa",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Ulice = c.String(),
                        Mesto = c.String(),
                        PSC = c.String(),
                        OsobaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Osoba", t => t.OsobaId, cascadeDelete: true)
                .Index(t => t.OsobaId);
            
            CreateTable(
                "dbo.Osoba",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Jmeno = c.String(),
                        Prijmeni = c.String(),
                        IC = c.String(),
                        DIC = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Adresa", "OsobaId", "dbo.Osoba");
            DropIndex("dbo.Adresa", new[] { "OsobaId" });
            DropTable("dbo.Osoba");
            DropTable("dbo.Adresa");
        }
    }
}
