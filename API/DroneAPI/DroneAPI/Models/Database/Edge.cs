using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/**
 * @author: Harmen Hilvers
 * Model containing Edge from Graphnode to graphnode 
 * */

namespace DroneAPI.Models.Database
{
    public class Edge
    {
        public int Id { get; set; }
        [JsonIgnore]
        public GraphNode StartGraphNode { get; set; }
        public virtual GraphNode EndGraphNode { get; set; }

    }
}