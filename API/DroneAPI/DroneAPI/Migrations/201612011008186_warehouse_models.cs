namespace DroneAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class warehouse_models : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.District",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Columns = c.Int(nullable: false),
                        Rows = c.Int(nullable: false),
                        Orientation = c.Int(nullable: false),
                        EndGraphNode_Id = c.Int(),
                        StartGraphNode_Id = c.Int(),
                        Warehouse_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GraphNodeDal", t => t.EndGraphNode_Id)
                .ForeignKey("dbo.GraphNodeDal", t => t.StartGraphNode_Id)
                .ForeignKey("dbo.Warehouse", t => t.Warehouse_Id)
                .Index(t => t.EndGraphNode_Id)
                .Index(t => t.StartGraphNode_Id)
                .Index(t => t.Warehouse_Id);
            
            CreateTable(
                "dbo.GraphNodeDal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        District_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.District", t => t.District_Id)
                .Index(t => t.District_Id);
            
            CreateTable(
                "dbo.Warehouse",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Product", "District_Id", c => c.Int());
            CreateIndex("dbo.Product", "District_Id");
            AddForeignKey("dbo.Product", "District_Id", "dbo.District", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.District", "Warehouse_Id", "dbo.Warehouse");
            DropForeignKey("dbo.District", "StartGraphNode_Id", "dbo.GraphNodeDal");
            DropForeignKey("dbo.Product", "District_Id", "dbo.District");
            DropForeignKey("dbo.District", "EndGraphNode_Id", "dbo.GraphNodeDal");
            DropForeignKey("dbo.GraphNodeDal", "District_Id", "dbo.District");
            DropIndex("dbo.GraphNodeDal", new[] { "District_Id" });
            DropIndex("dbo.District", new[] { "Warehouse_Id" });
            DropIndex("dbo.District", new[] { "StartGraphNode_Id" });
            DropIndex("dbo.District", new[] { "EndGraphNode_Id" });
            DropIndex("dbo.Product", new[] { "District_Id" });
            DropColumn("dbo.Product", "District_Id");
            DropTable("dbo.Warehouse");
            DropTable("dbo.GraphNodeDal");
            DropTable("dbo.District");
        }
    }
}
