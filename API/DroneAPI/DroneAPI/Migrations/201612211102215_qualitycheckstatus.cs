namespace DroneAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qualitycheckstatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QualityCheck", "Status", c => c.String());
            AddColumn("dbo.QualityCheck", "PictureFolderUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.QualityCheck", "PictureFolderUrl");
            DropColumn("dbo.QualityCheck", "Status");
        }
    }
}
