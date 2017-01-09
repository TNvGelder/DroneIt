namespace DroneAPI.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
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
                // Instantiate Nodes
                GraphNodeDal start = new GraphNodeDal { X = 0, Y = 0, Edges = new List<EdgeDal>() };
                GraphNodeDal startDistrict1 = new GraphNodeDal { X = 100, Y = 100, Edges = new List<EdgeDal>() };
                GraphNodeDal endDistrict1 = new GraphNodeDal { X = 100, Y = 900, Edges = new List<EdgeDal>() };
                GraphNodeDal startDistrict2 = new GraphNodeDal { X = 400, Y = 900, Edges = new List<EdgeDal>() };
                GraphNodeDal endDistrict2 = new GraphNodeDal { X = 400, Y = 100, Edges = new List<EdgeDal>() };
                GraphNodeDal startDistrict3 = new GraphNodeDal { X = 500, Y = 100, Edges = new List<EdgeDal>() };
                GraphNodeDal endDistrict3 = new GraphNodeDal { X = 500, Y = 1100, Edges = new List<EdgeDal>() };

                context.GraphNodes.AddOrUpdate(p => p.Id, start, startDistrict1, endDistrict1, startDistrict2, endDistrict2, startDistrict3, endDistrict3);
                context.SaveChanges();

                EdgeDal district1edge1 = new EdgeDal { DestinationGraphNode = endDistrict1 };
                EdgeDal district1edge2 = new EdgeDal { DestinationGraphNode = startDistrict1 };
                EdgeDal district2edge1 = new EdgeDal { DestinationGraphNode = endDistrict2 };
                EdgeDal district2edge2 = new EdgeDal { DestinationGraphNode = startDistrict2 };
                EdgeDal district3edge1 = new EdgeDal { DestinationGraphNode = endDistrict3 };
                EdgeDal district3edge2 = new EdgeDal { DestinationGraphNode = startDistrict3 };
                EdgeDal startedge1 = new EdgeDal { DestinationGraphNode = startDistrict1 };
                EdgeDal startedge2 = new EdgeDal { DestinationGraphNode = endDistrict2 };
                EdgeDal startedge3 = new EdgeDal { DestinationGraphNode = endDistrict3 };

                context.Edges.AddOrUpdate(p => p.Id, district1edge1, district1edge2, district2edge1, district2edge2, district3edge1, district3edge2, startedge1, startedge2, startedge3);
                context.SaveChanges();

                startDistrict1.Edges.Add(district1edge1);
                startDistrict2.Edges.Add(district2edge1);
                startDistrict3.Edges.Add(district3edge1);
                endDistrict3.Edges.Add(district3edge2);
                endDistrict2.Edges.Add(district2edge2);
                endDistrict1.Edges.Add(district1edge2);
                start.Edges.Add(startedge1);
                start.Edges.Add(startedge2);
                start.Edges.Add(startedge3);

                context.GraphNodes.AddOrUpdate(p => p.Id, start, startDistrict1, endDistrict1, startDistrict2, endDistrict2, startDistrict3, endDistrict3);
                context.SaveChanges();

                // Districts
                District district1 = new District { StartGraphNode = startDistrict1, EndGraphNode = endDistrict1, Name = "District1", Orientation = 270, X = 300, Y = 200, Columns = 6, Rows = 3 };
                District district2 = new District { StartGraphNode = startDistrict2, EndGraphNode = endDistrict2, Name = "District2", Orientation = 90, X = 300, Y = 800, Columns = 6, Rows = 3 };
                District district3 = new District { StartGraphNode = startDistrict3, EndGraphNode = endDistrict3, Name = "District3", Orientation = 270, X = 700, Y = 200, Columns = 8, Rows = 3 };
                context.Districts.AddOrUpdate(p => p.Name, district1, district2, district3);
                context.SaveChanges();

                startDistrict1.District = district1;
                endDistrict1.District = district1;
                startDistrict2.District = district2;
                endDistrict2.District = district2;
                startDistrict3.District = district3;
                endDistrict3.District = district3;

                context.Edges.AddOrUpdate(p => p.Id, district1edge1, district1edge2, district2edge1, district2edge2, district3edge1, district3edge2, startedge1, startedge2, startedge3);
                context.SaveChanges();

                // Warehouse
                Warehouse warehouse = new Warehouse();
                warehouse.Name = "Default Warehouse";
                warehouse.Height = 1200;
                warehouse.Width = 1500;
                warehouse.StartNode = start;

                context.Warehouses.AddOrUpdate(p => p.Name, warehouse);
                context.SaveChanges();

                district1.Warehouse = warehouse;
                district2.Warehouse = warehouse;
                district3.Warehouse = warehouse;
                context.Districts.AddOrUpdate(p => p.Name, district1, district2, district3);

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

            
        }
    }
}
