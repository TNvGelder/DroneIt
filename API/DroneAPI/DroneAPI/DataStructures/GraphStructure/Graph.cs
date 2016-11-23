using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DroneAPI.DataStructures.PriorityQueue;
using DroneAPI.Exceptions;
using DroneAPI.Models;
using DroneAPI.Services;

namespace DroneAPI.DataStructures.GraphStructure
{
    public class Graph
    {
        private Dictionary<Position, GraphNode> _nodeDictionary;

        public void addEdge(Position start, Position destination)
        {
            GraphNode startNode = getNode(start);
            GraphNode destNode = getNode(destination);
            double cost = MathUtility.CalculateDistance(start, destination);
            startNode.Adjacent.Add(new Edge(destNode, cost));
            destNode.Adjacent.Add(new Edge(startNode, cost));
        }

        public void Dijkstra(Position startPosition, Position endPosition)
        {
            
            BinaryHeap<Path> priorityQueue = new BinaryHeap<Path>();

            if (!_nodeDictionary.ContainsKey(startPosition))
            {
                throw new NoSuchElementException();
            }
            foreach (GraphNode node in _nodeDictionary.Values)
            {
                node.Reset();
            }
            
            GraphNode start = _nodeDictionary[startPosition];
            priorityQueue.Add(new Path(start, 0));
            int nodesSeen = 0;
            while (!priorityQueue.IsEmpty && nodesSeen < _nodeDictionary.Count)
            {
                Path minPath = priorityQueue.DeleteMin();
                GraphNode node = minPath.Destination;

                if (node.Scratch == 0)//Not yet processed node
                {
                    node.Scratch = 1;
                    nodesSeen++;
                    if (node.Position.Equals(endPosition))
                    {
                        
                    }
                    foreach (Edge e in node.Adjacent)
                    {
                        GraphNode adjacentNode = e.Destination;
                        double edgeCost = e.Cost;

                        if (adjacentNode.Distance > node.Distance + edgeCost)
                        {
                            adjacentNode.Distance = node.Distance + edgeCost;
                            adjacentNode.Prev = node;
                            priorityQueue.Add(new Path(adjacentNode, adjacentNode.Distance));
                        }
                    }
                }
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