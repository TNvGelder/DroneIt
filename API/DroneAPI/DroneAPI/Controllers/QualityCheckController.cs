using DroneAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DroneAPI.Controllers
{
    public class QualityCheckController : ApiController
    {
        // POST api/CheckProduct/5
        public void CheckProduct(int productId)
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
            return this.GenerateDirections(pList);
        }

        // Generates directions as Commands for the drone
        public string GenerateDirections(List<Position> pList)
        {
            string text = "";
            for(int i=0; i<pList.Count()-1; i++)
            {
                text += Services.MathUtility.CalculateDistance(pList[i], pList[i+1]) + " : " + Services.MathUtility.CalculateAngle(pList[i],pList[i+1])+"| ";
            }
            return text;
        }
    }
}