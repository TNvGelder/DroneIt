using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Models
{
    public class GraphNode
    {
        public Position Position { get; set; }
        public List<GraphNode> Adjacent { get; set; }
        public GraphNode Prev { get; set; }
        public double Distance { get; set; }
        private static double _unknownDist = -1;

        public double Dist { get; set; }

        public GraphNode(Position position)
        {
            this.Position = position;
        }

        public void Reset()
        {
            Distance = _unknownDist;
            Prev = null;

        }
    }
}