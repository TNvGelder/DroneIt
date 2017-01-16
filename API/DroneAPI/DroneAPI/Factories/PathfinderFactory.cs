using DroneAPI.Models;
using DroneAPI.Models.Database;
using DroneAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Factories
{
    public class PathfinderFactory
    {
        private static Pathfinder _pathfinder { get; set; }

        public static Pathfinder GetPathfinderFromWarehouse(Warehouse warehouse)
        {
            _pathfinder = new Pathfinder();
            List<GraphNode> GraphNodes = new List<GraphNode>();
            GraphNodes.Add(warehouse.StartNode);            

            foreach (District district in warehouse.Districts)
            {
                GraphNodes.Add(district.StartGraphNode);
                GraphNodes.Add(district.EndGraphNode);
            }

            foreach (GraphNode gn in GraphNodes)
            {
                Position start = new Position { X = gn.X, Y = gn.Y };
                foreach (Edge edge in gn.Edges)
                    _pathfinder.AddPath(start, new Position { X = edge.EndGraphNode.X, Y = edge.EndGraphNode.Y });
            }

            return _pathfinder;
        }
    }
}