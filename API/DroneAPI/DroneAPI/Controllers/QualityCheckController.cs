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

/**
 * @author: Harmen Hilvers
 * Controller for handling quality check functionality
 * */
namespace DroneAPI.Controllers
{
    public class QualityCheckController : ApiController
    {
        private DroneCommandProcessor _droneCommandProcessor;
        private DroneContext _db = new DroneContext();

        // POST api/QualityCheck/PostQualityCheck
        [EnableCors("*", "*", "POST")]
        [ResponseType(typeof(ProductLocation))]
        public QualityCheck PostQualityCheck(ProductLocation product)
        {
            if (_db.QualityChecks.Any(d => d.EndDate == null))
            {
                return null;
            }

            // get correct object from database by id
            ProductLocation pr = _db.Locations.Find(product.Id);

            // initiate quality check object
            QualityCheck qualitycheck = new QualityCheck();
            qualitycheck.StartDate = DateTime.Now;
            qualitycheck.ProductLocation = pr;
            qualitycheck.EndDate = null;
            _db.QualityChecks.Add(qualitycheck);
            _db.SaveChanges();

            // create commands for quality check
            DroneCommandProcessor commands = createCommands(qualitycheck);

            // Execute quality check
            commands.Execute();

            return qualitycheck;
        }


        // PUT api/QualityCheck/PutQualityCheck
        [EnableCors("*", "*", "PUT")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutQualityCheck(QualityCheck qualitycheck)
        {
            // return error when qualitycheck is empty
            if (qualitycheck == null)
            {
                return InternalServerError();
            }

            // getcorrect object from database by id
            QualityCheck ck = _db.QualityChecks.Find(qualitycheck.Id);

            // return error when qualitycheck does not excist
            if (ck == null)
            {
                return InternalServerError();
            }

            // update status for qualitycheck
            if (qualitycheck.Status != null)
            {
                ck.Status = qualitycheck.Status;
                if (qualitycheck.Status.Equals("Done"))
                {
                    ck.EndDate = DateTime.Now;
                }
            }

            // update picture folder for qualitycheck
            if (qualitycheck.PictureFolderUrl != null)
            {
                ck.PictureFolderUrl = qualitycheck.PictureFolderUrl;
            }

            _db.Entry(ck).State = EntityState.Modified;
            _db.SaveChanges();

            return Ok();
        }

        // GET api/QualityCheck/GetQualityCheck
        [EnableCors("*", "*", "GET")]
        [HttpGet]
        public string GetQualityCheck()
        {
            // get current active quality check, return null when not found
            QualityCheck q = _db.QualityChecks.Where(d => d.EndDate == null).FirstOrDefault();
            if (q == null)
            {
                return "null";
            }

            // convert qualitycheck to JSON object string, to prevent Reference loop errors.
            var s = JsonConvert.SerializeObject(q, Formatting.Indented,
                   new JsonSerializerSettings
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   });

            return s;
        }

        // GET api/QualityCheck/GetQualityCheckID
        [EnableCors("*", "*", "GET")]
        [HttpGet]
        public string GetQualityCheckID()
        {
            // get current active quality check, return null when not found
            QualityCheck q = _db.QualityChecks.Where(d => d.EndDate == null).FirstOrDefault();
            if (q == null)
            {
                return "null";
            }

            // return qualitycheck id
            return q.Id.ToString();
        }

        //GET api/QualityCheck/GetQualityChecks
        [EnableCors("*", "*", "GET")]
        [HttpGet]
        public string GetQualityChecks()
        {
            // get all completed quality checks, return null when not found
            List<QualityCheck> q = _db.QualityChecks.Where(d => d.EndDate != null).OrderByDescending(z => z.EndDate).ToList();
            if (q == null)
            {
                return "null";
            }

            // convert qualitycheck to JSON object string, to prevent Reference loop errors.
            var s = JsonConvert.SerializeObject(q, Formatting.Indented,
                   new JsonSerializerSettings
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   });

            return s;
        }

        // Method to create commands for the drone to execute
        private DroneCommandProcessor createCommands(QualityCheck qc)
        {
            // Get productLocation to take picture from
            ProductLocation pl = qc.ProductLocation;

            // Initiate CommandProcessor 
            _droneCommandProcessor = new DroneCommandProcessor();

            // Get Graph datastructure in pathfinder generated from selected warehouse
            Pathfinder pathfinder = PathfinderFactory.GetPathfinderFromWarehouse(pl.District.Warehouse);

            // Initiate start position (start position is drone start location)
            Position startNode = new Position(pl.District.Warehouse.StartNode.X, pl.District.Warehouse.StartNode.Y);

            // Get path to take from startposition to nearest graphnode for the Productlocation by using dijkstra algoritm
            LinkedList<Position> path = pathfinder.GetPath(startNode, this.giveEndPosition(pl));

            // save path to qualitycheck for webpage view 
            List<Position> path2 = pathfinder.GetPathList(startNode, this.giveEndPosition(pl));
            var s = JsonConvert.SerializeObject(path2, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            qc.JSONPath = s;
            _db.Entry(qc).State = EntityState.Modified;
            _db.SaveChanges();

            // add start / takeoff command to the droneCommandProcessor
            _droneCommandProcessor.AddCommand(new Command { name = "Start" });

            // add commands to the droneCommandProcessor from path, this generates the commands needed to move to the nearest graphnode to the productlocation
            MovementCommandFactory mFactory = new MovementCommandFactory();
            _droneCommandProcessor.AddListCommand(mFactory.GetMovementCommands(path));

            // add commands to move from the nearest graphnode to the position in front of the productlocation
            DistrictCommandFactory dFactory = new DistrictCommandFactory();
            _droneCommandProcessor.AddListCommand(dFactory.GetCommands(giveEndPosition(pl), pl));

            // add command to take picture, this also needs the current quality check id, needed for saving the pictures to the correct location
            _droneCommandProcessor.AddCommand(new Command { name = "TakePicture", value = qc.Id });

            // add command to land the drone
            _droneCommandProcessor.AddCommand(new Command { name = "Land" });

            return _droneCommandProcessor;
        }

        // method to determine wich graphnode is nearest to the productlocation and return its position
        private Position giveEndPosition(ProductLocation pl)
        {
            int half = pl.District.Columns / 2;
            Position result = new Position(pl.District.StartGraphNode.X, pl.District.StartGraphNode.Y);
            if (pl.Column > half)
            {
                result = new Position(pl.District.EndGraphNode.X, pl.District.EndGraphNode.Y);
            }
            return result;
        }
    }
}