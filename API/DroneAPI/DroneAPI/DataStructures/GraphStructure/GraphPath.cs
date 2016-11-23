using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DroneAPI.Models;

namespace DroneAPI.DataStructures.GraphStructure
{
    public class GraphPath<T> : IComparable<GraphPath<T>>
    {
        public GraphNode<T> Destination { get; set; }
        public double Cost { get; set; }

        public GraphPath(GraphNode<T> destination, double cost)
        {
            Destination = destination;
            Cost = cost;
        }

        public int CompareTo(GraphPath<T> otherPath)
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