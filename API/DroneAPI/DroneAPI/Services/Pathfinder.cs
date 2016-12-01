using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DroneAPI.DataStructures.GraphStructure;
using DroneAPI.Models;

namespace DroneAPI.Services
{


    public class Pathfinder
    {
        private Graph<Position> _pathGraph;

        //Creates
        public Pathfinder()
        {
            _pathGraph = new Graph<Position>();
        }

        //Adds a bidirectional path to the graph from startposition to endposition
        public void AddPath(Position startPosition, Position endPosition)
        {
            double cost = MathUtility.CalculateDistance(startPosition, endPosition);
            _pathGraph.AddEdge(startPosition, endPosition, cost);
            _pathGraph.AddEdge(endPosition, startPosition, cost);
        }

        /// <summary>
        ///Returns all the positions from the shortest path from the startValue to endValue. The linkedlist will start with the startPosition.
        //Returns an empty linkedlist if there is no possible path.
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        /// <returns></returns>
        public LinkedList<Position> GetPath(Position startPosition, Position endPosition)
        {
            return _pathGraph.GetPath(startPosition, endPosition);
        }
    }
}