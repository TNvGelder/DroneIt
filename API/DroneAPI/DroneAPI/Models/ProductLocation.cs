using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Models
{
    /// <summary>
    /// Links Product to a Location
    /// </summary>
    public class ProductLocation
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }

        public virtual District District { get; set; }
    }
}