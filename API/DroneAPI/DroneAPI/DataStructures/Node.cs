using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Models
{
    public class Node
    {
        public int x;
        public int y;
        public List<Node> adj;
        public Node prev;
        public double dist;

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}