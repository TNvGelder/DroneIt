using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DroneAPI.Processors.DroneProcessors;
using DroneAPI.Models;

namespace DroneAPI.Factorys
{
    /// <summary>
    /// Creates the commands for flying within a district
    /// </summary>
    public class DistrictCommandFactory : CommandFactory
    {

        public List<Command> GetCommands(Position p, ProductLocation pl)
        {
            List<Command> result = new List<Command>();
            District d = pl.District;
            int aisleDirection;
            double length;
            int photoDirection = (d.Orientation - 180 + 360)%360;
            int height = pl.Row;

            // Create commands depending on which side of the district the drone is.
            if (p.X == d.StartGraphNode.X && p.Y == d.StartGraphNode.Y)
            {
                // calulate values for from start
                aisleDirection = (d.Orientation + 90 + 360)%360;
                length = pl.Column + 0.5;
            }
            else
            {
                // calulate values for from end
                aisleDirection = (d.Orientation - 90 + 360)%360;
                length = d.Columns - pl.Column + 0.5;
            }

            // Turn command. This sets the drone in the right direction.
            result.Add(new Command() { name = "Turn", value = aisleDirection });
            // Forward command. The locations the drone should travel to reach the next position.
            result.Add(new Command() { name = "Forward", value = length });
            // Turn command. This turn the drone in the direction of the district.
            result.Add(new Command() { name = "Turn", value = photoDirection });
            // Rise command. This lets the drone rise to the right row
            result.Add(new Command() { name = "Rise", value = height });

            return result;
        }

        private LinkedListNode<Position> GetCurrentNode(LinkedList<Position> path)
        {
            LinkedListNode<Position> currentNode = path.First;

            // Define current position of drone
            //if (path.Count < 2) { throw new ArgumentException("The list of positions should have atleast two positions."); }
            while (currentNode.Next != null)
            {
                currentNode = currentNode.Next;
            }
            return currentNode;
        }
    }
}