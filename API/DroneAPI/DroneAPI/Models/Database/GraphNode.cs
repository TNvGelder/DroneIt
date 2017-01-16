using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


/**
 * @author: Harmen Hilvers
 * Model containing Graphnode 
 * */
namespace DroneAPI.Models.Database
{
    public class GraphNode
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public virtual ICollection<Edge> Edges { get; set; }
        [JsonIgnore]
        public virtual District District { get; set; }
        
    }
}