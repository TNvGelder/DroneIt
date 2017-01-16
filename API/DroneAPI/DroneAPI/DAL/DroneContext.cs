using DroneAPI.Models.Database;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

/**
 * @author: Harmen Hilvers
 * contains context 
 * */

namespace DroneAPI.DAL
{
    public class DroneContext : DbContext
    {
        public DroneContext() : base("DroneDatabase") { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductLocation> Locations { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Edge> Edges { get; set; }
        public DbSet<GraphNode> GraphNodes { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<QualityCheck> QualityChecks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}