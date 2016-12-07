using DroneAPI.Models;
using DroneAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Factorys
{
    public class PathfinderFactory
    {
        private static Pathfinder _pathfinder { get; set; }

        public static Pathfinder GetPathfinderFromWarehouse(Warehouse warehouse)
        {
            _pathfinder = new Pathfinder();
            List<GraphNodeDal> GraphNodes = new List<GraphNodeDal>();
            GraphNodes.Add(warehouse.StartNode);            

            foreach (District district in warehouse.Districts)
            {
                GraphNodes.Add(district.StartGraphNode);
                GraphNodes.Add(district.EndGraphNode);
            }

            foreach (GraphNodeDal gn in GraphNodes)
            {
                Position start = new Position { X = gn.X, Y = gn.Y };
                foreach (EdgeDal edge in gn.Edges)
                    _pathfinder.AddPath(start, new Position { X = gn.X, Y = gn.Y });
            }

            return _pathfinder;
        }
    }
}