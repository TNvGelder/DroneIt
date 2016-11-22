using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DroneAPI.Models;
namespace DroneAPI.Processors.DroneProcessors
{
    public class DroneProcessor
    {
        private Drone _drone { get; set; }
        private DroneConnectionProcessor _connectionProcessor { get; set; }

        public DroneProcessor(Drone drone)
        {
            _drone = drone;
            _connectionProcessor = new DroneConnectionProcessor(_drone);
            _connectionProcessor.Connect();
        }

        public void Forwards(int miliseconds)
        {
            if (_drone.Flying)
                _connectionProcessor.sendData("forward", miliseconds.ToString());
        }

        public void Backwards(int miliseconds)
        {
            if (_drone.Flying)
                _connectionProcessor.sendData("backward", miliseconds.ToString());
        }

        public void Turn(int degrees)
        {
            if (_drone.Flying)
                _connectionProcessor.sendData("turn", degrees.ToString());
        }

        public void TakeOff()
        {
            if (!_drone.Flying)
            {
                _connectionProcessor.sendData("takeoff", "");
                _drone.Flying = true;
            }

        }
        public void Land()
        {
            if (_drone.Flying)
            {
                _connectionProcessor.sendData("land", "");
                _drone.Flying = false;
            }
        }
    }
}