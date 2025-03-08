namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mr5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CustomerId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "CustomerId");
        }
    }
}
