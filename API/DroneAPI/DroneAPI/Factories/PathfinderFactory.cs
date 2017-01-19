using DroneAPI.Models;
using DroneAPI.Models.Database;
using DroneAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/**
 * @author: Harmen Hilvers
 * Factory used to generate Pathfinder from a warehouse object
 * */

namespace DroneAPI.Factories
{
    public class PathfinderFactory : IPathfinderFactory {
        private static Pathfinder _pathfinder { get; set; }
        public PathfinderFactory() { }

        // turn Warehouse into pathfinder object
        public Pathfinder GetPathfinderFromWarehouse(Warehouse warehouse)
        {
            _pathfinder = new Pathfinder();
            List<GraphNode> GraphNodes = new List<GraphNode>();

            // add start graphnode
            GraphNodes.Add(warehouse.StartNode);            

            // loop through districts and add all graphnodes
            foreach (District district in warehouse.Districts)
            {
                GraphNodes.Add(district.StartGraphNode);
                GraphNodes.Add(district.EndGraphNode);
            }

            // loop through all graphnodes and add a path if there is one
            foreach (GraphNode gn in GraphNodes)
            {
                Position start = new Position { X = gn.X, Y = gn.Y };
                foreach (Edge edge in gn.Edges)
                {
                    _pathfinder.AddPath(start, new Position { X = edge.EndGraphNode.X, Y = edge.EndGraphNode.Y });
                }
            }

            return _pathfinder;
        }
    }
}