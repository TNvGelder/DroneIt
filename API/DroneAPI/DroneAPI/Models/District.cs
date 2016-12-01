using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Models
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

        public virtual GraphNodeDal StartGraphNode {get;set;}
        public virtual GraphNodeDal EndGraphNode {get;set;}

        public virtual ICollection<Product> Products { get; set; }



    }
}