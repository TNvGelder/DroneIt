using DroneAPI.Models;
using DroneAPI.Processors.DroneProcessors.Commands;
using System.Collections.Generic;

namespace DroneAPI.Factorys
{
    interface ICommandFactory
    {
        List<IDroneCommand> GetCommands(LinkedList<Position> path);
    }
}
