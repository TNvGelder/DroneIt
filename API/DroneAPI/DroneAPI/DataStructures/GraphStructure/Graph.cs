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
    public class Graph<T>
    {
        private Dictionary<T, GraphNode<T>> _nodeDictionary = new Dictionary<T, GraphNode<T>>();

        public void AddEdge(T start, T destination, double cost)
        {
            GraphNode<T> startNode = getNode(start);
            GraphNode<T> destNode = getNode(destination);
            startNode.Adjacent.Add(new Edge<T>(destNode, cost));
        }

        private void Dijkstra(T startValue, T endValue)
        {
            
            BinaryHeap<GraphPath<T>> priorityQueue = new BinaryHeap<GraphPath<T>>();

            if (!_nodeDictionary.ContainsKey(startValue) || !_nodeDictionary.ContainsKey(endValue))
            {
                throw new NoSuchElementException();
            }
            foreach (GraphNode<T> node in _nodeDictionary.Values)
            {
                node.Reset();
            }
            
            GraphNode<T> start = _nodeDictionary[startValue];
            priorityQueue.Add(new GraphPath<T>(start, 0));
            start.Distance = 0;
            int nodesSeen = 0;
            while (!priorityQueue.IsEmpty && nodesSeen < _nodeDictionary.Count)
            {
                GraphPath<T> minPath = priorityQueue.DeleteMin();
                GraphNode<T> node = minPath.Destination;

                if (node.Scratch == 0)//Not yet processed node
                {
                    node.Scratch = 1;
                    nodesSeen++;
                    if (node.Value.Equals(endValue))
                    {
                        return;
                    }
                    foreach (Edge<T> e in node.Adjacent)
                    {
                        GraphNode<T> adjacentNode = e.Destination;
                        double edgeCost = e.Cost;

                        if (adjacentNode.Distance > node.Distance + edgeCost)
                        {
                            adjacentNode.Distance = node.Distance + edgeCost;
                            adjacentNode.Prev = node;
                            priorityQueue.Add(new GraphPath<T>(adjacentNode, adjacentNode.Distance));
                        }
                    }
                }
            }

            
        }

        //Returns all the values from the shortest path from the startValue to endValue. The linkedlist will start with the startValue.
        //Returns an empty linkedlist if there is no possible path.
        public LinkedList<T> GetPath(T startValue, T endValue)
        {
            Dijkstra(startValue, endValue);
            LinkedList<T> result = new LinkedList<T>();
            GraphNode<T> currentNode = getNode(endValue);
            if (currentNode.Prev != null)//There is a path between startValue and endValue
            {
                result.AddFirst(currentNode.Value);
                while (currentNode.Prev != null)
                {
                    currentNode = currentNode.Prev;
                    result.AddFirst(currentNode.Value);
                }
            }
            
            return result;
        }

        private GraphNode<T> getNode(T value)
        {
            if (_nodeDictionary.ContainsKey(value))
            {
                return _nodeDictionary[value];
            }
            else
            {
                GraphNode<T> node = new GraphNode<T>(value);
                _nodeDictionary[value] = node;
                return node;
            }
        }
    }
}