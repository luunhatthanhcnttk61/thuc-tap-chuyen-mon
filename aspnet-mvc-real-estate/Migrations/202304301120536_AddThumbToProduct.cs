namespace aspnet_mvc_real_estate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddThumbToProduct : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.products", "Thumbnail");
            AddColumn("dbo.products", "Thumbnail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.products", "Thumbnail");
        }
    }
}
