namespace DroneAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class warehouse_models_edges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EdgeDal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DestinationGraphNode_Id = c.Int(),
                        SourceGraphNode_Id = c.Int(),
                        GraphNodeDal_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GraphNodeDal", t => t.DestinationGraphNode_Id)
                .ForeignKey("dbo.GraphNodeDal", t => t.SourceGraphNode_Id)
                .ForeignKey("dbo.GraphNodeDal", t => t.GraphNodeDal_Id)
                .Index(t => t.DestinationGraphNode_Id)
                .Index(t => t.SourceGraphNode_Id)
                .Index(t => t.GraphNodeDal_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EdgeDal", "GraphNodeDal_Id", "dbo.GraphNodeDal");
            DropForeignKey("dbo.EdgeDal", "SourceGraphNode_Id", "dbo.GraphNodeDal");
            DropForeignKey("dbo.EdgeDal", "DestinationGraphNode_Id", "dbo.GraphNodeDal");
            DropIndex("dbo.EdgeDal", new[] { "GraphNodeDal_Id" });
            DropIndex("dbo.EdgeDal", new[] { "SourceGraphNode_Id" });
            DropIndex("dbo.EdgeDal", new[] { "DestinationGraphNode_Id" });
            DropTable("dbo.EdgeDal");
        }
    }
}
