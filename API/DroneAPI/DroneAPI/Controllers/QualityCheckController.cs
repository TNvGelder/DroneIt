
using System;
using DroneAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DroneAPI.DataStructures;
using DroneAPI.Processors.DroneProcessors;
using DroneAPI.Processors.DroneProcessors.Commands;
using DroneAPI.Services;
using DroneAPI.Factorys;

namespace DroneAPI.Controllers
{
    public class QualityCheckController : ApiController
    {
        private DroneProcessor _droneProcessor;
        private DroneCommandProcessor _droneCommandProcessor;
        
        // POST api/CheckProduct/5
        public void CheckProduct(int productId)
        {

        }
        
        // GET: api/QualityCheck
        // Dient als een soort van Main
        public string GetShortestPath()
        {
            _droneProcessor = DroneFactory.getDroneProcessor();
            _droneCommandProcessor = new DroneCommandProcessor(_droneProcessor);
            Pathfinder pathfinder = new Pathfinder();
            Position a = new Position { X=0, Y=0 };
            Position b = new Position { X=1, Y=3 };
            Position c = new Position { X = 4, Y = 0 }; ;
            Position d = new Position { X = 3, Y = 1 }; ;
            Position e = new Position { X = 4, Y = 4 }; ;
            Position f = new Position { X = 3, Y = 7 }; ;
            Position g = new Position { X = 6, Y = 6 }; ;
            Position h = new Position { X = 8, Y = 5 }; ;
            Position i = new Position { X = 7, Y = 8 }; ;
            Position j = new Position { X = 9, Y = 9 }; ;

            pathfinder.AddPath(a, b);
            pathfinder.AddPath(a, c);
            pathfinder.AddPath(a, d);
            pathfinder.AddPath(b, e);
            pathfinder.AddPath(c, f);
            pathfinder.AddPath(d, h);
            pathfinder.AddPath(e, g);
            pathfinder.AddPath(d, e);
            pathfinder.AddPath(e, g);
            pathfinder.AddPath(g, h);
            pathfinder.AddPath(h, i);
            pathfinder.AddPath(f, i);
            pathfinder.AddPath(i, j);

            LinkedList < Position > path = pathfinder.GetPath(a, j);

            // start command
            _droneCommandProcessor.AddCommand(new StartCommand(_droneProcessor));

            /*MovementCommandFactory factory = new MovementCommandFactory(_droneProcessor);
            _droneCommandProcessor.AddListCommand(factory.GetMovementCommands(path));*/
            /*_droneCommandProcessor.AddCommand(new RiseCommand(_droneProcessor, 2));
            _droneCommandProcessor.AddCommand(new FallCommand(_droneProcessor, 2));*/
            //_droneCommandProcessor.AddCommand(new TurnCommand(_droneProcessor, 270));

            _droneCommandProcessor.AddCommand(new TakePictureCommand(_droneProcessor, 1));
            
            //_droneCommandProcessor.AddCommand(new ForwardCommand(_droneProcessor, 2));
            //_droneCommandProcessor.AddCommand(new TurnCommand(_droneProcessor, 180));

            //_droneCommandProcessor.AddCommand(new ForwardCommand(_droneProcessor, 2));
            //_droneCommandProcessor.AddCommand(new TurnCommand(_droneProcessor, 90));

            //_droneCommandProcessor.AddCommand(new ForwardCommand(_droneProcessor, 2));
            //_droneCommandProcessor.AddCommand(new TurnCommand(_droneProcessor, 0));


            // land command
            _droneCommandProcessor.AddCommand(new LandCommand(_droneProcessor));
            _droneCommandProcessor.Execute();
            return this.GenerateDirections(path);
        }

        // Generates directions as Commands for the drone
        public string GenerateDirections(LinkedList<Position> pList)
        {
            string text = "";
            if (pList.Count < 2)
            {
                throw new ArgumentException("The list of positions should have atleast two positions.");
            }
            for (LinkedListNode<Position> currentNode = pList.First;
                currentNode.Next != null;
                currentNode = currentNode.Next)
            {
                Position start = currentNode.Value;
                Position end = currentNode.Next.Value;
                text += "p1: " + start.ToString() + " p2: "+end.ToString()+" ,";
                text += Services.MathUtility.CalculateDistance(start, end) + " : " + Services.MathUtility.CalculateAngle(start, end) + "| ";
            }
            return text;
        }
    }

}