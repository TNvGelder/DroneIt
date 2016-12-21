using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Models
{
    /// <summary>
    /// District where Products are stored
    /// </summary>
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

        public virtual GraphNodeDal StartGraphNode {get;set;}
        public virtual GraphNodeDal EndGraphNode {get;set;}
        public virtual ICollection<ProductLocation> ProductLocations { get; set; }
    }
}