namespace DroneAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qualitycheckmodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QualityCheck",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        ProductLocation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductLocation", t => t.ProductLocation_Id)
                .Index(t => t.ProductLocation_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QualityCheck", "ProductLocation_Id", "dbo.ProductLocation");
            DropIndex("dbo.QualityCheck", new[] { "ProductLocation_Id" });
            DropTable("dbo.QualityCheck");
        }
    }
}
