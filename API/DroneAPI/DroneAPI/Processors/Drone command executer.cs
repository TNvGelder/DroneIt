using DroneAPI.Models;
using DroneAPI.Processors.DroneProcessors;
using DroneAPI.Processors.DroneProcessors.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Processors
{
    public class Drone_command_executer
    {
        public Drone_command_executer(Drone drone)
        {
            DroneProcessor processor = new DroneProcessor(drone);

            IDroneCommand t1 = new TakeOffCommand(processor);
            IDroneCommand t2 = new TurnCommand(processor, 90);
            IDroneCommand t3 = new LandCommand(processor);
  
            t1.Execute();
            //while (drone.busy) ;
            t2.Execute();
           // while (drone.busy) ;
            t3.Execute();
        }
    }
}