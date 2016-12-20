
using System;
using DroneAPI.Models;
using System.Collections.Generic;
using System.Web.Http;
using DroneAPI.Processors.DroneProcessors;
using DroneAPI.Processors.DroneProcessors.Commands;
using DroneAPI.Services;
using DroneAPI.Factorys;
using DroneAPI.DAL;
using System.Web.Http.Cors;
using System.Linq;

namespace DroneAPI.Controllers
{
    public class QualityCheckController : ApiController
    {
        private DroneProcessor _droneProcessor;
        private DroneCommandProcessor _droneCommandProcessor;
        private DroneContext db = new DroneContext();

        // GET api/QualityCheck/5
        [EnableCors("*", "*", "GET")]
        [HttpGet]
        public IHttpActionResult GetQualityCheck(int id)
        {
            ProductLocation pl = db.Locations.Find(id);
            this.CreateCommands(pl);
            // logic to start the whole process
            
            return Ok();
        }

        // Create Commands
        private void CreateCommands(ProductLocation pl)
        {
            _droneProcessor = DroneFactory.getDroneProcessor();
            _droneCommandProcessor = new DroneCommandProcessor(_droneProcessor);
            Pathfinder pathfinder = PathfinderFactory.GetPathfinderFromWarehouse(pl.District.Warehouse);

            Position startNode = new Position(pl.District.Warehouse.StartNode.X, pl.District.Warehouse.StartNode.Y);
            LinkedList < Position > path = pathfinder.GetPath(startNode, this.GiveEndPosition(pl));

            //start command
            _droneCommandProcessor.AddCommand(new StartCommand(_droneProcessor));

            MovementCommandFactory mFactory = new MovementCommandFactory(_droneProcessor);
            _droneCommandProcessor.AddListCommand(mFactory.GetMovementCommands(path));

            DistrictCommandFactory dFactory = new DistrictCommandFactory(_droneProcessor);
            //_droneCommandProcessor.AddListCommand(dFactory.GetCommands(path, pl));

            // land command
            _droneCommandProcessor.AddCommand(new LandCommand(_droneProcessor));
            _droneCommandProcessor.Execute();
        }

        private Position GiveEndPosition(ProductLocation pl) {
            int half = pl.District.Columns / 2;
            Position result = new Position(pl.District.StartGraphNode.X, pl.District.StartGraphNode.Y);
            if (pl.Column>half)
            {
                result = new Position(pl.District.EndGraphNode.X, pl.District.EndGraphNode.Y);
            }
            return result;
        }

        // Generates directions as Commands for the drone TEst method
        private string GenerateDirections(LinkedList<Position> pList)
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
            // return text
            return text;
        }
    }

}