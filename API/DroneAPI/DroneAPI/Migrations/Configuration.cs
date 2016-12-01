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
            if (!context.Warehouses.Any())
            {
                GraphNodeDal start = new GraphNodeDal { X = 800, Y = 100 };
                GraphNodeDal startDistrict1 = new GraphNodeDal { X = 100, Y = 300 };
                GraphNodeDal endDistrict1 = new GraphNodeDal { X = 100, Y = 900 };
                GraphNodeDal startDistrict2 = new GraphNodeDal { X = 500, Y = 900 };
                GraphNodeDal endDistrict2 = new GraphNodeDal { X = 500, Y = 300 };

                context.GraphNodes.AddOrUpdate(p => p.Id, start, startDistrict1, endDistrict1, startDistrict2, endDistrict2);
                context.SaveChanges();

                EdgeDal district1edge1 = new EdgeDal { DestinationGraphNode = endDistrict1 , GraphNodeDal_Id = startDistrict1.Id};
                EdgeDal district1edge2 = new EdgeDal { DestinationGraphNode = startDistrict1 , GraphNodeDal_Id = endDistrict1.Id};
                EdgeDal district2edge1 = new EdgeDal { DestinationGraphNode = endDistrict2 , GraphNodeDal_Id = startDistrict2.Id};
                EdgeDal district2edge2 = new EdgeDal { DestinationGraphNode = startDistrict2, GraphNodeDal_Id = endDistrict2.Id };
                EdgeDal startedge1 = new EdgeDal { DestinationGraphNode = startDistrict1 };
                EdgeDal startedge2 = new EdgeDal { DestinationGraphNode = endDistrict2 };

                context.Edges.AddOrUpdate(p => p.Id, district1edge1, district1edge2, district2edge1, district2edge2, startedge1, startedge2);
                context.SaveChanges();

                District district1 = new District { StartGraphNode = startDistrict1, EndGraphNode = endDistrict1, Name = "District1", Orientation = 270, X = 300, Y = 200, Columns = 4, Rows = 3 };
                District district2 = new District { StartGraphNode = startDistrict2, EndGraphNode = endDistrict2, Name = "District2", Orientation = 90, X = 300, Y = 800, Columns = 4, Rows = 3 };
                context.Districts.AddOrUpdate(p => p.Name, district1, district2);
                context.SaveChanges();

                startDistrict1.District = district1;
                endDistrict1.District = district1;
                startDistrict2.District = district2;
                endDistrict2.District = district2;

                context.Edges.AddOrUpdate(p => p.Id, district1edge1, district1edge2, district2edge1, district2edge2, startedge1, startedge2);
                context.SaveChanges();

                Warehouse warehouse = new Warehouse();
                warehouse.Name = "Default Warehouse";
                warehouse.Height = 1000;
                warehouse.Width = 1500;
                warehouse.StartNode = start;

                context.Warehouses.AddOrUpdate(p => p.Name, warehouse);
                context.SaveChanges();

                district1.Warehouse = warehouse;
                district2.Warehouse = warehouse;
                context.Districts.AddOrUpdate(p => p.Name, district1, district2);

                context.SaveChanges();
            }

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

            /*    District district3= new District { Name = "District3", Orientation = 270, X = 800, Y = 200, Columns = 4, Rows = 3 };
    GraphNodeDal startDistrict3 = new GraphNodeDal { X = 600, Y = 300 };
    GraphNodeDal endDistrict3 = new GraphNodeDal { X = 600, Y = 900 };

    District district4 = new District { Name = "District4", Orientation = 90, X = 500, Y = 800, Columns = 4, Rows = 3 };
    GraphNodeDal startDistrict4 = new GraphNodeDal { X = 1000, Y = 900 };
    GraphNodeDal endDistrict4 = new GraphNodeDal { X = 1000, Y = 300 };*/
        }
    }
}
