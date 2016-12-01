namespace DroneAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class warehouse_StartNode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Warehouse", "StartNode_Id", c => c.Int());
            CreateIndex("dbo.Warehouse", "StartNode_Id");
            AddForeignKey("dbo.Warehouse", "StartNode_Id", "dbo.GraphNodeDal", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Warehouse", "StartNode_Id", "dbo.GraphNodeDal");
            DropIndex("dbo.Warehouse", new[] { "StartNode_Id" });
            DropColumn("dbo.Warehouse", "StartNode_Id");
        }
    }
}
