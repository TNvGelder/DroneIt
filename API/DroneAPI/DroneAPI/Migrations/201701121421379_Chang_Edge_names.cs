namespace DroneAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Chang_Edge_names : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Edge", name: "DestinationGraphNode_Id", newName: "EndGraphNode_Id");
            RenameColumn(table: "dbo.Edge", name: "GraphNodeDal_Id", newName: "StartGraphNode_Id");
            RenameIndex(table: "dbo.Edge", name: "IX_DestinationGraphNode_Id", newName: "IX_EndGraphNode_Id");
            RenameIndex(table: "dbo.Edge", name: "IX_GraphNodeDal_Id", newName: "IX_StartGraphNode_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Edge", name: "IX_StartGraphNode_Id", newName: "IX_GraphNodeDal_Id");
            RenameIndex(table: "dbo.Edge", name: "IX_EndGraphNode_Id", newName: "IX_DestinationGraphNode_Id");
            RenameColumn(table: "dbo.Edge", name: "StartGraphNode_Id", newName: "GraphNodeDal_Id");
            RenameColumn(table: "dbo.Edge", name: "EndGraphNode_Id", newName: "DestinationGraphNode_Id");
        }
    }
}
