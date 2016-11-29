using DroneAPI.Models;
using DroneAPI.Processors.DroneProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Factorys
{
    public class DroneFactory
    {
        public static DroneProcessor getDroneProcessor()
        {
            Drone drone = new Drone("Tomasz", "http://localhost:8000/");
            DroneConnectionProcessor connectionProcessor = new DroneConnectionProcessor(drone);
            DroneProcessor droneProcessor = new DroneProcessor(connectionProcessor);
            DroneCommandProcessor commandProcessor = new DroneCommandProcessor(droneProcessor);

            return droneProcessor;
        }
    }
}