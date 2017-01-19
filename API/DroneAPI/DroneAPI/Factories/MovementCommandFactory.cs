using DroneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DroneAPI.DroneController;
/**
 * @author: Albert David
 * Factory used to generate commands to fly to a district Graphnode
 * */
namespace DroneAPI.Factories
{
    public class MovementCommandFactory : IMovementCommandFactory
    {
    
        public MovementCommandFactory() {        }


        // Creates different dronecommands based on the given path. 
        public List<Command> GetMovementCommands(LinkedList<Position> path)
        {
            List<Command> result = new List<Command>();

            for (LinkedListNode<Position> currentNode = path.First;
                currentNode.Next != null;
                currentNode = currentNode.Next)
            {
                Position start = currentNode.Value;
                Position end = currentNode.Next.Value;
                // Turn command. This sets the drone in the right direction.
                result.Add(new Command { name = "Turn", value = Services.MathUtility.CalculateAngle(start, end) });
                // Forward command. The meters the drone should travel to reach the next position.
                result.Add(new Command { name = "Forward", value = 1 } );
            }
            return result;
        }
    }
}