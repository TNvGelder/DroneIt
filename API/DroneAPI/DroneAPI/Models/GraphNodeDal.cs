using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Models
{
    public class GraphNodeDal
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public virtual ICollection<EdgeDal> Edges { get; set; }
        [JsonIgnore]
        public virtual District District { get; set; }
        
    }
}