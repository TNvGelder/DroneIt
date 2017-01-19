using DroneAPI.Models;
using DroneAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneAPI.Factories {
    interface IDistrictCommandFactory {

        List<Command> GetCommands(Position p, ProductLocation pl);
        LinkedListNode<Position> GetCurrentNode(LinkedList<Position> path);

    }
}
