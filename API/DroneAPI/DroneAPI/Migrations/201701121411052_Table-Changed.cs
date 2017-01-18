namespace DroneAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableChanged : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.GraphNodeDal", newName: "GraphNode");
            RenameTable(name: "dbo.EdgeDal", newName: "Edge");
            RenameColumn(table: "dbo.Edge", name: "GraphNodeDal_Id1", newName: "GraphNode_Id");
            RenameIndex(table: "dbo.Edge", name: "IX_GraphNodeDal_Id1", newName: "IX_GraphNode_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Edge", name: "IX_GraphNode_Id", newName: "IX_GraphNodeDal_Id1");
            RenameColumn(table: "dbo.Edge", name: "GraphNode_Id", newName: "GraphNodeDal_Id1");
            RenameTable(name: "dbo.Edge", newName: "EdgeDal");
            RenameTable(name: "dbo.GraphNode", newName: "GraphNodeDal");
        }
    }
}
