using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/**
 * @author: Harmen Hilvers
 * Model containing district used to strore products in warehouse
 * */
namespace DroneAPI.Models.Database
{
    public class District
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public int Orientation { get; set; }
       
        public virtual Warehouse Warehouse { get; set; }

        public virtual GraphNode StartGraphNode {get;set;}
        public virtual GraphNode EndGraphNode {get;set;}
        public virtual ICollection<ProductLocation> ProductLocations { get; set; }
    }
}