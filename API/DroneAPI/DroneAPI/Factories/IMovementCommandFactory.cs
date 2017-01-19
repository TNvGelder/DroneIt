using DroneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneAPI.Factories {
    interface IMovementCommandFactory {
        List<Command> GetMovementCommands(LinkedList<Position> path);

    }
}
