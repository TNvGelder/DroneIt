using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DroneAPI.Models;
/**
 * @author: Twan van Gelder
 * class that determines edge between 2 graphnode destinations
 * */
namespace DroneAPI.DataStructures
{
    //The Edge class provides data about a connected node and what the cost is to reach the node using that edge.
    public class Edge<T> : IComparable<Edge<T>>
    {
        public GraphNode<T> Destination { get; set; }
        public double Cost { get; set; }

        public Edge(GraphNode<T> destination, double cost)
        {
            Destination = destination;
            Cost = cost;
        }

        //This method compares the Edge with another Edge.
        public int CompareTo(Edge<T> otherEdge)
        {
            double otherCost = otherEdge.Cost;
            int result;
            if (Cost < otherCost)
            {
                result = -1;
            }
            else if (Cost > otherCost)
            {
                result = 1;
            }
            else
            {
                result = 0;
            }
            return result;
        }
    }
}