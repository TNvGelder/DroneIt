using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DroneAPI.DataStructures.GraphStructure;
using DroneAPI.Models;

/**
 * @author: Albert David
 * Processor class for pathfinding
 * */
namespace DroneAPI.Services
{
    public class Pathfinder
    {
        private Graph<Position> _pathGraph;
        
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

        /*
        ///Returns all the positions from the shortest path from the startValue to endValue. The linkedlist will start with the startPosition.
        //Returns an empty linkedlist if there is no possible path.
        */
        public LinkedList<Position> GetPath(Position startPosition, Position endPosition)
        {
            return _pathGraph.GetPath(startPosition, endPosition);
        }

        // returns all the positions from the shortestpath from the startValue to endValue In List format
        public List<Position> GetPathList(Position startPosition, Position endPosition)
        {
            return _pathGraph.GetPathList(startPosition, endPosition);
        }
    }
}