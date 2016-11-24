using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DroneAPI.Models;

namespace DroneAPI.DataStructures
{
    public class Edge<T>
    {
        public GraphNode<T> Destination { get; set; }
        public double Cost { get; set; }

        public Edge(GraphNode<T> destination, double cost)
        {
            Destination = destination;
            Cost = cost;
        }
    }
}