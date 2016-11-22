using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DroneAPI.Models;

namespace DroneAPI.DataStructures
{
    public class Edge
    {
        public GraphNode Destination { get; set; }
        public double Cost { get; set; }

    }
}