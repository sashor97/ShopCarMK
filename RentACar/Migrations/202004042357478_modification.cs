namespace RentACar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Voziloes", "Godina", c => c.Int(nullable: false));
            AddColumn("dbo.Voziloes", "Gorivo", c => c.String(nullable: false));
            AddColumn("dbo.Voziloes", "Menuvac", c => c.String(nullable: false));
            AddColumn("dbo.Voziloes", "Registracija", c => c.String(nullable: false));
            AddColumn("dbo.Voziloes", "Moknost", c => c.Int(nullable: false));
            AddColumn("dbo.Sopstveniks", "Telefon", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sopstveniks", "Telefon");
            DropColumn("dbo.Voziloes", "Moknost");
            DropColumn("dbo.Voziloes", "Registracija");
            DropColumn("dbo.Voziloes", "Menuvac");
            DropColumn("dbo.Voziloes", "Gorivo");
            DropColumn("dbo.Voziloes", "Godina");
        }
    }
}
