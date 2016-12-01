namespace DroneAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_warehouse_models : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EdgeDal", "GraphNodeDal_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EdgeDal", "GraphNodeDal_Id");
        }
    }
}
