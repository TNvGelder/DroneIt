using System;
using DroneAPI.Models.Database;
using DroneAPI.Models;
using System.Collections.Generic;
using System.Web.Http;
using DroneAPI.Processors.DroneProcessors;
using DroneAPI.Services;
using DroneAPI.Factories;
using DroneAPI.DAL;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Data;
using System.Data.Entity;
using Newtonsoft.Json;
using System.Linq;

namespace DroneAPI.Controllers
{
    public class QualityCheckController : ApiController
    {
        private DroneCommandProcessor _droneCommandProcessor;
        private DroneContext db = new DroneContext();

        // GET api/QualityCheck/5
        [EnableCors("*", "*", "POST")]
        [ResponseType(typeof(ProductLocation))]

        public QualityCheck PostQualityCheck(ProductLocation product)
        {
            if (db.QualityChecks.Any(d => d.EndDate == null)) return null;

            ProductLocation pr = db.Locations.Find(product.Id);
            QualityCheck qualitycheck = new QualityCheck();
            qualitycheck.StartDate = DateTime.Now;
            qualitycheck.ProductLocation = pr;
            qualitycheck.EndDate = null;

            db.QualityChecks.Add(qualitycheck);
            db.SaveChanges();
            //Check if product has any locations
            // DataRow[] result = db.Locations.Select("Product_Id = "+productId);
            CreateCommands(qualitycheck);

            return qualitycheck;
        }

        [EnableCors("*", "*", "PUT")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutQualityCheck(QualityCheck qualitycheck)
        {
            QualityCheck ck = db.QualityChecks.Find(qualitycheck.Id);
            
            if (qualitycheck.Status != null) {
                ck.Status = qualitycheck.Status;
                if (qualitycheck.Status.Equals("Done"))
                    ck.EndDate = DateTime.Now;
            }
            
            if (qualitycheck.PictureFolderUrl != null)
                ck.PictureFolderUrl = qualitycheck.PictureFolderUrl;

            db.Entry(ck).State = EntityState.Modified;
            
            db.SaveChanges();

            return Ok();
        }

        [EnableCors("*", "*", "GET")]
        [HttpGet]
        public string GetQualityCheck()
        {
            QualityCheck q = db.QualityChecks.Where(d => d.EndDate == null).FirstOrDefault();

            if (q == null) return "null";

            var s = JsonConvert.SerializeObject(q, Formatting.Indented,
                   new JsonSerializerSettings
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   });

            return s;
        }

        [EnableCors("*", "*", "GET")]
        [HttpGet]
        public string GetQualityCheckID() {
            QualityCheck q = db.QualityChecks.Where(d => d.EndDate == null).FirstOrDefault();

            if (q == null) return "null";

            return q.Id.ToString();
        }

        [EnableCors("*", "*", "GET")]
        [HttpGet]
        public string GetQualityChecks()
        {
            List<QualityCheck> q = db.QualityChecks.Where(d => d.EndDate != null).OrderByDescending(z=>z.EndDate).ToList();

            if (q == null) return "null";

            var s = JsonConvert.SerializeObject(q, Formatting.Indented,
                   new JsonSerializerSettings
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   });

            return s;
        }

        // Create Commands
        private void CreateCommands(QualityCheck qc)
        {
            ProductLocation pl = qc.ProductLocation;
            _droneCommandProcessor = new DroneCommandProcessor();
            Pathfinder pathfinder = PathfinderFactory.GetPathfinderFromWarehouse(pl.District.Warehouse);
            
            Position startNode = new Position(pl.District.Warehouse.StartNode.X, pl.District.Warehouse.StartNode.Y);
            LinkedList < Position > path = pathfinder.GetPath(startNode, this.GiveEndPosition(pl));
			
            // save path to qualitycheck for webpage view
            List<Position> path2 = pathfinder.GetPathList(startNode, this.GiveEndPosition(pl));
            var s = JsonConvert.SerializeObject(path2, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            qc.JSONPath = s;
            db.Entry(qc).State = EntityState.Modified;
            db.SaveChanges();

            // start command
            _droneCommandProcessor.AddCommand(new Command { name = "Start" });
            
            MovementCommandFactory mFactory = new MovementCommandFactory();
            _droneCommandProcessor.AddListCommand(mFactory.GetMovementCommands(path));

            DistrictCommandFactory dFactory = new DistrictCommandFactory();
            _droneCommandProcessor.AddListCommand(dFactory.GetCommands(GiveEndPosition(pl), pl));
            
            // take picture command
            _droneCommandProcessor.AddCommand(new Command { name = "TakePicture", value = qc.Id });

            // land command
            _droneCommandProcessor.AddCommand(new Command { name = "Land" });
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
    }

}