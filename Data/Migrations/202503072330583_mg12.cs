namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mg12 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "OrderId", "dbo.Orders");
            DropIndex("dbo.Customers", new[] { "OrderId" });
            AddColumn("dbo.Customers", "Order_OrderId", c => c.Int());
            AddColumn("dbo.Customers", "Order_OrderId1", c => c.Int());
            AddColumn("dbo.Orders", "Customer_CustomerId", c => c.Int());
            CreateIndex("dbo.Customers", "Order_OrderId");
            CreateIndex("dbo.Customers", "Order_OrderId1");
            CreateIndex("dbo.Orders", "Customer_CustomerId");
            AddForeignKey("dbo.Orders", "Customer_CustomerId", "dbo.Customers", "CustomerId");
            AddForeignKey("dbo.Customers", "Order_OrderId1", "dbo.Orders", "OrderId");
            AddForeignKey("dbo.Customers", "Order_OrderId", "dbo.Orders", "OrderId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "Order_OrderId", "dbo.Orders");
            DropForeignKey("dbo.Customers", "Order_OrderId1", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Customer_CustomerId", "dbo.Customers");
            DropIndex("dbo.Orders", new[] { "Customer_CustomerId" });
            DropIndex("dbo.Customers", new[] { "Order_OrderId1" });
            DropIndex("dbo.Customers", new[] { "Order_OrderId" });
            DropColumn("dbo.Orders", "Customer_CustomerId");
            DropColumn("dbo.Customers", "Order_OrderId1");
            DropColumn("dbo.Customers", "Order_OrderId");
            CreateIndex("dbo.Customers", "OrderId");
            AddForeignKey("dbo.Customers", "OrderId", "dbo.Orders", "OrderId", cascadeDelete: true);
        }
    }
}
