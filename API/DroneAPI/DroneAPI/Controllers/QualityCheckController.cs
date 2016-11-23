using DroneAPI.Models;
using DroneAPI.Processors.DroneProcessors;
using DroneAPI.Processors.DroneProcessors.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

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

        // POST api/CheckProduct
        public void CheckProduct()
        {

        }
        
        // GET: api/QualityCheck
        // Dient als een soort van Main
        public string GetDirections()
        {
            List<Position> pList = new List<Position>();
            pList.Add(new Position { X = 0, Y = 0 });
            pList.Add(new Position { X = 3, Y = 1 });
            pList.Add(new Position { X = 8, Y = 5 });
            pList.Add(new Position { X = 7, Y = 8 });
            pList.Add(new Position { X = 9, Y = 9 });
            this.GenerateDirections(pList);
            return "worked";
        }

        // Generates directions as Commands for the drone
        public void GenerateDirections(List<Position> pList)
        { 
            // start command
            _cmdProcessor.AddCommand(new StartCommand(_droneProcessor));
            // directions commands
            for (int i=0; i<pList.Count()-1; i++)
            {
                // Turn command. This sets the drone in the right direction.
                _cmdProcessor.AddCommand(new TurnCommand(_droneProcessor, Services.MathUtility.CalculateAngle(pList[i], pList[i + 1])));
                // Forward command. The meters the drone should travel to reach the next position.
                _cmdProcessor.AddCommand(new ForwardCommand(_droneProcessor, Services.MathUtility.CalculateDistance(pList[i], pList[i + 1])));
            }
            // land command
            _cmdProcessor.AddCommand(new LandCommand(_droneProcessor));
        }
    }
}