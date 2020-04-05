namespace RentACar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rezervacijas", "KorisnikId", "dbo.Korisniks");
            DropForeignKey("dbo.Rezervacijas", "VoziloId", "dbo.Voziloes");
            DropIndex("dbo.Rezervacijas", new[] { "VoziloId" });
            DropIndex("dbo.Rezervacijas", new[] { "KorisnikId" });
            DropTable("dbo.Rezervacijas");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Rezervacijas",
                c => new
                    {
                        RezervacijaId = c.Int(nullable: false, identity: true),
                        denoviIznajmuvanje = c.Int(nullable: false),
                        uspesnost = c.Boolean(nullable: false),
                        plateno = c.Boolean(nullable: false),
                        total = c.Double(nullable: false),
                        VoziloId = c.Int(nullable: false),
                        KorisnikId = c.Int(nullable: false),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RezervacijaId);
            
            CreateIndex("dbo.Rezervacijas", "KorisnikId");
            CreateIndex("dbo.Rezervacijas", "VoziloId");
            AddForeignKey("dbo.Rezervacijas", "VoziloId", "dbo.Voziloes", "VoziloId", cascadeDelete: true);
            AddForeignKey("dbo.Rezervacijas", "KorisnikId", "dbo.Korisniks", "KorisnikId", cascadeDelete: true);
        }
    }
}
