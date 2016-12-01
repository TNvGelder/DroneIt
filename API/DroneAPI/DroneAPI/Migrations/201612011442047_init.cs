namespace DroneAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
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
                "dbo.EdgeDal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DestinationGraphNode_Id = c.Int(),
                        GraphNodeDal_Id = c.Int(),
                        GraphNodeDal_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GraphNodeDal", t => t.DestinationGraphNode_Id)
                .ForeignKey("dbo.GraphNodeDal", t => t.GraphNodeDal_Id)
                .ForeignKey("dbo.GraphNodeDal", t => t.GraphNodeDal_Id1)
                .Index(t => t.DestinationGraphNode_Id)
                .Index(t => t.GraphNodeDal_Id)
                .Index(t => t.GraphNodeDal_Id1);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
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
                        StartNode_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GraphNodeDal", t => t.StartNode_Id)
                .Index(t => t.StartNode_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Warehouse", "StartNode_Id", "dbo.GraphNodeDal");
            DropForeignKey("dbo.District", "Warehouse_Id", "dbo.Warehouse");
            DropForeignKey("dbo.District", "StartGraphNode_Id", "dbo.GraphNodeDal");
            DropForeignKey("dbo.Product", "District_Id", "dbo.District");
            DropForeignKey("dbo.District", "EndGraphNode_Id", "dbo.GraphNodeDal");
            DropForeignKey("dbo.EdgeDal", "GraphNodeDal_Id1", "dbo.GraphNodeDal");
            DropForeignKey("dbo.EdgeDal", "GraphNodeDal_Id", "dbo.GraphNodeDal");
            DropForeignKey("dbo.EdgeDal", "DestinationGraphNode_Id", "dbo.GraphNodeDal");
            DropForeignKey("dbo.GraphNodeDal", "District_Id", "dbo.District");
            DropIndex("dbo.Warehouse", new[] { "StartNode_Id" });
            DropIndex("dbo.Product", new[] { "District_Id" });
            DropIndex("dbo.EdgeDal", new[] { "GraphNodeDal_Id1" });
            DropIndex("dbo.EdgeDal", new[] { "GraphNodeDal_Id" });
            DropIndex("dbo.EdgeDal", new[] { "DestinationGraphNode_Id" });
            DropIndex("dbo.GraphNodeDal", new[] { "District_Id" });
            DropIndex("dbo.District", new[] { "Warehouse_Id" });
            DropIndex("dbo.District", new[] { "StartGraphNode_Id" });
            DropIndex("dbo.District", new[] { "EndGraphNode_Id" });
            DropTable("dbo.Warehouse");
            DropTable("dbo.Product");
            DropTable("dbo.EdgeDal");
            DropTable("dbo.GraphNodeDal");
            DropTable("dbo.District");
        }
    }
}
