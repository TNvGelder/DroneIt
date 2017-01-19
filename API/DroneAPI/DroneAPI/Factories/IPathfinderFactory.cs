using DroneAPI.Models.Database;
using DroneAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneAPI.Factories {
    interface IPathfinderFactory {

        Pathfinder GetPathfinderFromWarehouse(Warehouse warehouse);
    }
}
