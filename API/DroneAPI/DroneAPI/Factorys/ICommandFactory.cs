using DroneAPI.Models;
using System.Collections.Generic;

namespace DroneAPI.Factorys
{
    interface ICommandFactory
    {
        List<Command> GetCommands(LinkedList<Position> path);
    }
}
