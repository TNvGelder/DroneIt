using System;
using DroneAPI.Models;

/*
* @author : harmen hilvers
*/
namespace DroneAPI.Processors.DroneProcessors
{
    public class DroneConnectionProcessor
    {
        public Drone Drone { get; private set; }

        public DroneConnectionProcessor(Drone drone)
        {
            Drone = drone;
            Drone.Busy = false;
        }

        public void Connect()
        {
            
        }

        public void sendData(string action, string data = "")
        {
            Drone.Busy = true;
            string[] arrayData = { action, data };
        }     

        public void Disconnect()
        {
            
        }
    }
}