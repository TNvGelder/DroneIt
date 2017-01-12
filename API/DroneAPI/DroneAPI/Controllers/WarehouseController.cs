using DroneAPI.Models.Database;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DroneAPI.DAL;
using System.Web.Http.Cors;
using Newtonsoft.Json;

namespace DroneAPI.Controllers
{
    public class WarehouseController : ApiController
    {

        private DroneContext db = new DroneContext();

        // GET api/Warehouse/GetWarehouses
        [EnableCors("*", "*", "GET")]
        [HttpGet]
        public string GetWarehouses()
        {
            List<Warehouse> warehouses = db.Warehouses.ToList();

          // format warehouses to json, this is used because not all properties are needed
            var s = JsonConvert.SerializeObject(warehouses, Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            });
            return s;
        }

        [EnableCors("*", "*", "GET")]
        [HttpGet]
        public string GetWarehouse(int id)
        {
            Warehouse warehouses = db.Warehouses.Find(id);

            // format warehouses to json, this is used because not all properties are needed
            var s = JsonConvert.SerializeObject(warehouses, Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            });
            return s;
        }


    }
}