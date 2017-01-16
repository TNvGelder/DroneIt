using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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