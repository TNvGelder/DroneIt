using DroneAPI.DAL;
using DroneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
        public string GetDirections()
        {
            List<Position> pList = new List<Position>();
            pList.Add(new Position { X = 0, Y = 0 });
            pList.Add(new Position { X = 1, Y = 3 });
            pList.Add(new Position { X = 0, Y = 4 });
            pList.Add(new Position { X = 3, Y = 1 });
            return this.GenerateDirections(pList);
        }

        public string GenerateDirections(List<Position> pList)
        {
            string text = "";
            for(int i=0; i<pList.Count()-1; i++)
            {
                text += CalculateDistance(pList[i], pList[i+1])+" : "+this.CalculateAngle(pList[i],pList[i+1])+"| ";
            }
            return text;
        }

        double CalculateDistance(Position a, Position b)
        {
            return Math.Sqrt(Math.Pow((a.X-b.X), 2.0) + Math.Pow(a.Y - b.Y, 2.0));
        }

        int CalculateAngle(Position origin, Position destination)
        {
            var n = 270 - (Math.Atan2(origin.Y - destination.Y, origin.X - destination.X)) * 180 / Math.PI;
            return Convert.ToInt16(n) % 360;
        }
    }
}