using DroneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DroneAPI.Processors.DroneProcessors.Commands;

namespace DroneAPI.DataStructures
{
    public class MovementCommandFactory
    {
        public void GetMovementCommands(LinkedList<Position> path)
        {
            List<IDroneCommand> result = new List<IDroneCommand>();
            for (LinkedListNode<Position> currentNode = path.First;
                currentNode.Next != null;
                currentNode = currentNode.Next)
            {
                Position start = currentNode.Value;
                Position end = currentNode.Next.Value;
                // Turn command. This sets the drone in the right direction.
                new TurnCommand(_droneProcessor, Services.MathUtility.CalculateAngle(start, end));
                // Forward command. The meters the drone should travel to reach the next position.
                _cmdProcessor.AddCommand(new ForwardCommand(_droneProcessor, Services.MathUtility.CalculateDistance(start, end)));
            }
        }
    }
}