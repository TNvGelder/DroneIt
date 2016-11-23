using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DroneAPI.Models;

namespace DroneAPI.DataStructures.GraphStructure
{
    public class Path : IComparable<Path>
    {
        public GraphNode Destination { get; set; }
        public double Cost { get; set; }

        public Path(GraphNode destination, double cost)
        {
            Destination = destination;
            Cost = cost;
        }

        public int CompareTo(Path otherPath)
        {
            double otherCost = otherPath.Cost;
            int result;
            if (Cost < otherCost)
            {
                result = -1;
            }else if (Cost > otherCost)
            {
                result = 1;
            }
            else
            {
                result =0;
            }
            return result;
        }
    }
}