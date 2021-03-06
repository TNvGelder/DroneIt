﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DroneAPI.DataStructures;


/**
 * @author: Twan van Gelder
 * class containing Graphnode information
 * */
namespace DroneAPI.Models
{
    // This Graphnode Class provides a point in the GraphStructure. 
    // This Node a flyable point for the drone
    public class GraphNode<T>
    {
        public T Value { get; set; }
        public List<Edge<T>> Adjacent { get; set; }
        public GraphNode<T> Prev { get; set; }
        public double Distance { get; set; }
        private static double _unknownDist = double.MaxValue;
        public int Scratch { get; set; }

        public GraphNode(T value)
        {
            this.Value = value;
            Adjacent = new List<Edge<T>>();
        }

        public void Reset()
        {
            Distance = _unknownDist;
            Prev = null;
            Scratch = 0;
        }
    }
}