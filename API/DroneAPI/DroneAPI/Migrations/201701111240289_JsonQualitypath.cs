namespace DroneAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JsonQualitypath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QualityCheck", "JSONPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.QualityCheck", "JSONPath");
        }
    }
}
