
using System;
using DroneAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DroneAPI.Processors.DroneProcessors;
using DroneAPI.Processors.DroneProcessors.Commands;
using DroneAPI.Services;

namespace DroneAPI.Controllers
{
    public class QualityCheckController : ApiController
    {
		private Drone _drone;
        private DroneProcessor _droneProcessor;
        private DroneCommandProcessor _cmdProcessor;

        public QualityCheckController()
        {
            this._drone = new Drone();
            this._droneProcessor = new DroneProcessor(_drone);
            this._cmdProcessor = new DroneCommandProcessor(_drone);
        }

        // POST api/CheckProduct/5
        public void CheckProduct(int productId)
        {

        }
        
        // GET: api/QualityCheck
        // Dient als een soort van Main
        public string GetShortestPath()
        {
            Pathfinder pathfinder = new Pathfinder();
            Position a = new Position {X=0, Y=0};
            Position b = new Position { X=1, Y=3};
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
            _cmdProcessor.AddCommand(new StartCommand(_droneProcessor));
            for (LinkedListNode<Position> currentNode = path.First;
                currentNode.Next != null;
                currentNode = currentNode.Next)
            {
                Position start = currentNode.Value;
                Position end = currentNode.Next.Value;
                // Turn command. This sets the drone in the right direction.
                _cmdProcessor.AddCommand(new TurnCommand(_droneProcessor, Services.MathUtility.CalculateAngle(start, end)));
                // Forward command. The meters the drone should travel to reach the next position.
                _cmdProcessor.AddCommand(new ForwardCommand(_droneProcessor, Services.MathUtility.CalculateDistance(start, end)));
            }
            
            // land command
            _cmdProcessor.AddCommand(new LandCommand(_droneProcessor));   
            _cmdProcessor.Execute();
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