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

        public bool DroneIsBusy()
        {
            return _drone.busy;
        }

        public void Forwards(int squares)
        {
            if (_drone.Flying)
                _connectionProcessor.sendData("forward", squares.ToString());
        }

        public void Backwards(int squares)
        {
            if (_drone.Flying)
                _connectionProcessor.sendData("backward", squares.ToString());
        }

        public void Left(int squares) {
            if (_drone.Flying)
                _connectionProcessor.sendData("left", squares.ToString());
        }

        public void Right(int squares) {
            if (_drone.Flying)
                _connectionProcessor.sendData("right", squares.ToString());
        }

        public void Rise(int squares) {
            if (_drone.Flying)
                _connectionProcessor.sendData("rise", squares.ToString());
        }

        public void Fall(int squares) {
            if (_drone.Flying)
                _connectionProcessor.sendData("fall", squares.ToString());
        }

        public void Turn(int degrees)
        {
            if (_drone.Flying)
                _connectionProcessor.sendData("turn", degrees.ToString());
        }

        public void Start()
        {
            if (!_drone.Flying)
            {
                _connectionProcessor.sendData("start");
                _drone.Flying = true;
            }

        }
        public void Land()
        {
            if (_drone.Flying)
            {
                _connectionProcessor.sendData("land");
                _drone.Flying = false;
            }
        }
    }
}