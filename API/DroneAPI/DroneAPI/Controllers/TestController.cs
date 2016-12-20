using DroneAPI.DataStructures;
using DroneAPI.Factorys;
using DroneAPI.Models;
using DroneAPI.Processors.DroneProcessors;
using DroneAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DroneAPI.Controllers
{
    public class TestController : ApiController
    {
        private DroneCommandProcessor _droneCommandProcessor;
        
        [EnableCors("*", "*", "GET")]
        [ResponseType(typeof(string))]
        public string GetProduct(int id)
        {
            _droneCommandProcessor = new DroneCommandProcessor();
            Pathfinder pathfinder = new Pathfinder();
            LinkedList<Position> path = new LinkedList<Position>();

            switch (id)
            {
                case 1:
                    Position a = new Position { X = 0, Y = 0 };
                    Position b = new Position { X = 0, Y = 1 };
            
                    pathfinder.AddPath(a, b);
                    path = pathfinder.GetPath(a, b);

                    break;
                case 2:
                    Position a1 = new Position { X = 0, Y = 0 };
                    Position b1 = new Position { X = 0, Y = 3 };
                    Position c1 = new Position { X = 0, Y = 3 };
                    Position d1 = new Position { X = 0, Y = 3 };

                    pathfinder.AddPath(a1, b1);
                    pathfinder.AddPath(b1, c1);
                    pathfinder.AddPath(c1, d1);
                    pathfinder.AddPath(a1, d1);
                    path = pathfinder.GetPath(a1, c1);

                    break;
            }

            // start command
            _droneCommandProcessor.AddCommand(new Command { name = "Start" });

            MovementCommandFactory factory = new MovementCommandFactory();
            _droneCommandProcessor.AddListCommand(factory.GetMovementCommands(path));

            // land command
            _droneCommandProcessor.AddCommand(new Command { name = "Land" });
            _droneCommandProcessor.Execute();
            return "true";
        }
    }
}
