using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DroneAPI.Models;

namespace DroneAPI.DataStructures.GraphStructure
{
    public class Graph
    {
        private Dictionary<Position, GraphNode> _nodeDictionary;
        

        public void Dijkstra(Position startPosition, Position endPosition)
        {
            //PriorityQueue<Path> pq = new PriorityQueue<Path>();
            if (!_nodeDictionary.ContainsKey(startPosition))
            {
                foreach (GraphNode node in _nodeDictionary.Values)
                {
                    node.Reset();
                }
            }

            int nodesSeen = 0;
            //var heap = new C5.IntervalHeap<int>();
            while (nodesSeen < _nodeDictionary.Count)
            {
                
            }
            
        }

        private GraphNode getNode(Position position)
        {
            if (_nodeDictionary.ContainsKey(position))
            {
                return _nodeDictionary[position];
            }
            else
            {
                GraphNode node = new GraphNode(position);
                _nodeDictionary[position] = node;
                return node;
            }
        }
    }
}