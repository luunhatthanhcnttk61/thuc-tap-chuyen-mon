namespace aspnet_mvc_real_estate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAddressToStudent1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.products", "Thumbnail", c => c.String());
            AddColumn("dbo.collections", "Thumbnail", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.products", "Thumbnail");
            DropColumn("dbo.collections", "Thumbnail");
        }
    }
}
