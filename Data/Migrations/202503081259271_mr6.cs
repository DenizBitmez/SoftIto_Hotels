namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mr6 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Orders", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Address", c => c.String());
        }
    }
}
