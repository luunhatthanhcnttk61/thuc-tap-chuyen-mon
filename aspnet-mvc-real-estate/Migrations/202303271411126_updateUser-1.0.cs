namespace aspnet_mvc_real_estate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUser10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String());
            AddColumn("dbo.AspNetUsers", "Adrress", c => c.String());
            AddColumn("dbo.AspNetUsers", "Gender", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "Ban", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Ban");
            DropColumn("dbo.AspNetUsers", "Gender");
            DropColumn("dbo.AspNetUsers", "Adrress");
            DropColumn("dbo.AspNetUsers", "FullName");
        }
    }
}
