namespace aspnet_mvc_real_estate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUser11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            AddColumn("dbo.AspNetUsers", "isBan", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "isAdmin", c => c.Boolean(nullable: false));
            DropColumn("dbo.AspNetUsers", "Adrress");
            DropColumn("dbo.AspNetUsers", "Ban");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Ban", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Adrress", c => c.String());
            DropColumn("dbo.AspNetUsers", "isAdmin");
            DropColumn("dbo.AspNetUsers", "isBan");
            DropColumn("dbo.AspNetUsers", "Address");
        }
    }
}
