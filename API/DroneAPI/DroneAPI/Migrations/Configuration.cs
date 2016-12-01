namespace DroneAPI.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DroneAPI.DAL.DroneContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DroneAPI.DAL.DroneContext context)
        {
          /*  Warehouse warehouse = new Warehouse();
            warehouse.Name = "Default Warehouse";
            warehouse.Height = 1000;
            warehouse.Width = 1500;

            GraphNodeDal start = new GraphNodeDal { X = 800, Y = 100 };

            District district1 = new District { Name="District1", Orientation = 270, X= 200, Y = 800, }
            GraphNodeDal startDistrict1 = new GraphNodeDal { X = 100, Y = 900 };
            GraphNodeDal endDistrict1 = new GraphNodeDal { X = 100, Y = 300 };*/

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
