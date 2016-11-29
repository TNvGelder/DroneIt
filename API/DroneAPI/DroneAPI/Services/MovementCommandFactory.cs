using DroneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DroneAPI.Processors.DroneProcessors;
using DroneAPI.Processors.DroneProcessors.Commands;

namespace DroneAPI.DataStructures
{


    public class MovementCommandFactory
    {
        private static MovementCommandFactory _factory;
        public static MovementCommandFactory Factory
        {
            get { return _factory; }
        }
        private DroneProcessor _droneProcessor;

        public MovementCommandFactory(DroneProcessor _droneProcessor) {
            this._droneProcessor = _droneProcessor;
        }

        /// <summary>
        /// Creates different dronecommands based on the given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<IDroneCommand> GetMovementCommands(LinkedList<Position> path)
        {
            List<IDroneCommand> result = new List<IDroneCommand>();

            for (LinkedListNode<Position> currentNode = path.First;
                currentNode.Next != null;
                currentNode = currentNode.Next)
            {
                Position start = currentNode.Value;
                Position end = currentNode.Next.Value;
                // Turn command. This sets the drone in the right direction.
                result.Add(new TurnCommand(_droneProcessor, Services.MathUtility.CalculateAngle(start, end)));
                // Forward command. The meters the drone should travel to reach the next position.
                result.Add(new ForwardCommand(_droneProcessor, Services.MathUtility.CalculateDistance(start, end)));
            }
            return result;
        }
    }
}