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
                GraphNodeDal start = new GraphNodeDal { X = 400, Y = 150, Edges = new List<EdgeDal>() };
                GraphNodeDal startDistrict1 = new GraphNodeDal { X = 300, Y = 1200, Edges = new List<EdgeDal>() };
                GraphNodeDal endDistrict1 = new GraphNodeDal { X = 300, Y = 300, Edges = new List<EdgeDal>() };
                GraphNodeDal startDistrict2 = new GraphNodeDal { X = 500, Y = 300, Edges = new List<EdgeDal>() };
                GraphNodeDal endDistrict2 = new GraphNodeDal { X = 500, Y = 1200, Edges = new List<EdgeDal>() };

                context.GraphNodes.AddOrUpdate(p => p.Id, start, startDistrict1, endDistrict1, startDistrict2, endDistrict2);
                context.SaveChanges();

                EdgeDal district1edge1 = new EdgeDal { DestinationGraphNode = endDistrict1 };
                EdgeDal district1edge2 = new EdgeDal { DestinationGraphNode = startDistrict1 };
                EdgeDal district2edge1 = new EdgeDal { DestinationGraphNode = endDistrict2 };
                EdgeDal district2edge2 = new EdgeDal { DestinationGraphNode = startDistrict2 };
                EdgeDal startedge1 = new EdgeDal { DestinationGraphNode = endDistrict1 };
                EdgeDal startedge2 = new EdgeDal { DestinationGraphNode = startDistrict2 };

                context.Edges.AddOrUpdate(p => p.Id, district1edge1, district1edge2, district2edge1, district2edge2, startedge1, startedge2);
                context.SaveChanges();

                startDistrict1.Edges.Add(district1edge1);
                startDistrict2.Edges.Add(district2edge1);
                endDistrict2.Edges.Add(district2edge2);
                endDistrict1.Edges.Add(district1edge2);
                start.Edges.Add(startedge1);
                start.Edges.Add(startedge2);

                context.GraphNodes.AddOrUpdate(p => p.Id, start, startDistrict1, endDistrict1, startDistrict2, endDistrict2);
                context.SaveChanges();

                // Warehouse
                Warehouse warehouse = new Warehouse();
                warehouse.Name = "Warehouse Demo";
                warehouse.Height = 1250;
                warehouse.Width = 1200;
                warehouse.StartNode = start;

                context.Warehouses.AddOrUpdate(p => p.Name, warehouse);
                context.SaveChanges();

                // Districts
                District district1 = new District { StartGraphNode = startDistrict1, EndGraphNode = endDistrict1, Name = "Left District", Orientation = 90, X = 100, Y = 1200, Columns = 9, Rows = 2, Warehouse = warehouse };
                District district2 = new District { StartGraphNode = startDistrict2, EndGraphNode = endDistrict2, Name = "Right District", Orientation = 270, X = 700, Y = 300, Columns = 9, Rows = 2, Warehouse = warehouse };
                context.Districts.AddOrUpdate(p => p.Name, district1, district2);
                context.SaveChanges();

                startDistrict1.District = district1;
                endDistrict1.District = district1;
                startDistrict2.District = district2;
                endDistrict2.District = district2;

                context.Edges.AddOrUpdate(p => p.Id, district1edge1, district1edge2, district2edge1, district2edge2, startedge1, startedge2);
                context.SaveChanges();

                // Add some products
                Product p1 = new Product() { Name = "Banaan" };
                Product p2 = new Product() { Name = "Fietsband" };
                Product p3 = new Product() { Name = "River Ice Tea" };
                Product p4 = new Product() { Name = "Asus Laptops" };
                Product p5 = new Product() { Name = "AR Drone" };
                Product p6 = new Product() { Name = "Coca cola Light" };
                Product p7 = new Product() { Name = "Logitech G35" };
                Product p8 = new Product() { Name = "Koffie" };

                context.Products.AddOrUpdate(p1, p2, p3, p4, p5, p6, p7, p8);
                context.SaveChanges();

                // Assign locations to the products
                ProductLocation pl1 = new ProductLocation() { Product = p1, Column = 2, Row = 2, District = district1 };
                ProductLocation pl2 = new ProductLocation() { Product = p2, Column = 6, Row = 1, District = district1 };
                ProductLocation pl3 = new ProductLocation() { Product = p3, Column = 4, Row = 1, District = district2 };
                ProductLocation pl4 = new ProductLocation() { Product = p4, Column = 1, Row = 2, District = district2 };
                ProductLocation pl5 = new ProductLocation() { Product = p5, Column = 5, Row = 2, District = district2 };

                context.Locations.AddOrUpdate(pl1, pl2, pl3, pl4, pl5);
                context.SaveChanges();
            }

            if (context.Warehouses.ToList().Count < 2)

            {
                // Instantiate Nodes
                GraphNodeDal start = new GraphNodeDal { X = 0, Y = 0, Edges = new List<EdgeDal>() };
                GraphNodeDal startDistrict1 = new GraphNodeDal { X = 100, Y = 900, Edges = new List<EdgeDal>() };
                GraphNodeDal endDistrict1 = new GraphNodeDal { X = 100, Y = 100, Edges = new List<EdgeDal>() };
                GraphNodeDal startDistrict2 = new GraphNodeDal { X = 400, Y = 100, Edges = new List<EdgeDal>() };
                GraphNodeDal endDistrict2 = new GraphNodeDal { X = 400, Y = 900, Edges = new List<EdgeDal>() };
                GraphNodeDal startDistrict3 = new GraphNodeDal { X = 500, Y = 1100, Edges = new List<EdgeDal>() };
                GraphNodeDal endDistrict3 = new GraphNodeDal { X = 500, Y = 100, Edges = new List<EdgeDal>() };

                context.GraphNodes.AddOrUpdate(p => p.Id, start, startDistrict1, endDistrict1, startDistrict2, endDistrict2, startDistrict3, endDistrict3);
                context.SaveChanges();

                EdgeDal district1edge1 = new EdgeDal { DestinationGraphNode = endDistrict1 };
                EdgeDal district1edge2 = new EdgeDal { DestinationGraphNode = startDistrict1 };
                EdgeDal district2edge1 = new EdgeDal { DestinationGraphNode = endDistrict2 };
                EdgeDal district2edge2 = new EdgeDal { DestinationGraphNode = startDistrict2 };
                EdgeDal district3edge1 = new EdgeDal { DestinationGraphNode = endDistrict3 };
                EdgeDal district3edge2 = new EdgeDal { DestinationGraphNode = startDistrict3 };
                EdgeDal startedge1 = new EdgeDal { DestinationGraphNode = endDistrict1 };
                EdgeDal startedge2 = new EdgeDal { DestinationGraphNode = startDistrict2 };
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

                // Warehouse
                Warehouse warehouse = new Warehouse();
                warehouse.Name = "Default Warehouse";
                warehouse.Height = 1200;
                warehouse.Width = 1500;
                warehouse.StartNode = start;

                context.Warehouses.AddOrUpdate(p => p.Name, warehouse);
                context.SaveChanges();

                // Districts
                District district1 = new District { StartGraphNode = startDistrict1, EndGraphNode = endDistrict1, Name = "District1", Orientation = 270, X = 300, Y = 200, Columns = 6, Rows = 3, Warehouse = warehouse };
                District district2 = new District { StartGraphNode = startDistrict2, EndGraphNode = endDistrict2, Name = "District2", Orientation = 90, X = 300, Y = 800, Columns = 6, Rows = 3, Warehouse = warehouse };
                District district3 = new District { StartGraphNode = startDistrict3, EndGraphNode = endDistrict3, Name = "District3", Orientation = 270, X = 700, Y = 200, Columns = 8, Rows = 3, Warehouse = warehouse };
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

                // Add some products
                Product p1 = new Product() { Name = "iMac 27 (2016)" };
                Product p2 = new Product() { Name = "iPod" };
                Product p3 = new Product() { Name = "iPhone 7" };
                Product p4 = new Product() { Name = "iPhone 6s" };
                Product p5 = new Product() { Name = "Apple TV" };
                Product p6 = new Product() { Name = "iMac 21 (2012)" };
                Product p7 = new Product() { Name = "iPad Air 2" };
                Product p8 = new Product() { Name = "MacBook Pro Retina (2015)" };

                context.Products.AddOrUpdate(p1, p2, p3, p4, p5, p6, p7, p8);
                context.SaveChanges();

                // Assign locations to the products
                ProductLocation pl1 = new ProductLocation() { Product = p1, Column = 2, Row = 2, District = district1 };
                ProductLocation pl2 = new ProductLocation() { Product = p2, Column = 6, Row = 1, District = district1 };
                ProductLocation pl3 = new ProductLocation() { Product = p3, Column = 4, Row = 1, District = district2 };
                ProductLocation pl4 = new ProductLocation() { Product = p4, Column = 1, Row = 2, District = district2 };
                ProductLocation pl5 = new ProductLocation() { Product = p5, Column = 5, Row = 2, District = district2 };
                ProductLocation pl6 = new ProductLocation() { Product = p6, Column = 4, Row = 3, District = district3 };
                ProductLocation pl7 = new ProductLocation() { Product = p7, Column = 2, Row = 1, District = district3 };
                ProductLocation pl8 = new ProductLocation() { Product = p8, Column = 8, Row = 3, District = district3 };

                context.Locations.AddOrUpdate(pl1, pl2, pl3, pl4, pl5, pl6, pl7, pl8);
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