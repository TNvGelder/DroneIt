namespace DroneAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productlocation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Product", "District_Id", "dbo.District");
            DropIndex("dbo.Product", new[] { "District_Id" });
            CreateTable(
                "dbo.ProductLocation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Column = c.Int(nullable: false),
                        Row = c.Int(nullable: false),
                        District_Id = c.Int(),
                        Product_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.District", t => t.District_Id)
                .ForeignKey("dbo.Product", t => t.Product_Id)
                .Index(t => t.District_Id)
                .Index(t => t.Product_Id);
            
            DropColumn("dbo.Product", "District_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Product", "District_Id", c => c.Int());
            DropForeignKey("dbo.ProductLocation", "Product_Id", "dbo.Product");
            DropForeignKey("dbo.ProductLocation", "District_Id", "dbo.District");
            DropIndex("dbo.ProductLocation", new[] { "Product_Id" });
            DropIndex("dbo.ProductLocation", new[] { "District_Id" });
            DropTable("dbo.ProductLocation");
            CreateIndex("dbo.Product", "District_Id");
            AddForeignKey("dbo.Product", "District_Id", "dbo.District", "Id");
        }
    }
}
