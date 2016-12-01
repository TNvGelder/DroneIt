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
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DroneAPI.DAL.DroneContext context)
        {
            Warehouse warehouse = new Warehouse();
            warehouse.Name = "Default Warehouse";
            warehouse.Height = 1000;
            warehouse.Width = 1500;

            GraphNodeDal start = new GraphNodeDal { X = 800, Y = 100 };

            District district1 = new District { Name = "District1", Orientation = 270, X = 300, Y = 200, Columns = 4, Rows = 3 };
            GraphNodeDal startDistrict1 = new GraphNodeDal { X = 100, Y = 300 };
            GraphNodeDal endDistrict1 = new GraphNodeDal { X = 100, Y = 900 };

            District district2 = new District { Name = "District2", Orientation = 90, X = 300, Y = 800, Columns = 4, Rows = 3 };
            GraphNodeDal startDistrict2 = new GraphNodeDal { X = 500, Y = 900 };
            GraphNodeDal endDistrict2 = new GraphNodeDal { X = 500, Y = 300 };

            District district3= new District { Name = "District3", Orientation = 270, X = 800, Y = 200, Columns = 4, Rows = 3 };
            GraphNodeDal startDistrict3 = new GraphNodeDal { X = 600, Y = 300 };
            GraphNodeDal endDistrict3 = new GraphNodeDal { X = 600, Y = 900 };

            District district4 = new District { Name = "District4", Orientation = 90, X = 500, Y = 800, Columns = 4, Rows = 3 };
            GraphNodeDal startDistrict4 = new GraphNodeDal { X = 1000, Y = 900 };
            GraphNodeDal endDistrict4 = new GraphNodeDal { X = 1000, Y = 300 };

            context.Warehouses.AddOrUpdate(p => p.Name, warehouse);
            context.SaveChanges();
            

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
