using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneConnection
{
    public class DroneProcessor
    {
        private DroneConnectionProcessor _connectionProcessor { get; set; }

        public DroneProcessor(DroneConnectionProcessor connectionProcessor)
        {
            _connectionProcessor = connectionProcessor;
            _connectionProcessor.Connect();
        }

        public bool DroneIsBusy()
        {
            return _connectionProcessor._drone.busy;
        }

        public void Forwards(double squares)
        {
            if (_connectionProcessor._drone.Flying)
                _connectionProcessor.sendData("forward", squares.ToString());
        }

        public void Backwards(double squares)
        {
            if (_connectionProcessor._drone.Flying)
                _connectionProcessor.sendData("backward", squares.ToString());
        }

        public void Left(double squares) {
            if (_connectionProcessor._drone.Flying)
                _connectionProcessor.sendData("left", squares.ToString());
        }

        public void Right(double squares) {
            if (_connectionProcessor._drone.Flying)
                _connectionProcessor.sendData("right", squares.ToString());
        }

        public void Rise(double squares) {
            if (_connectionProcessor._drone.Flying)
                _connectionProcessor.sendData("rise", squares.ToString());
        }

        public void Fall(double squares) {
            if (_connectionProcessor._drone.Flying)
                _connectionProcessor.sendData("fall", squares.ToString());
        }

        public void Turn(int degrees)
        {
            if (_connectionProcessor._drone.Flying)
                _connectionProcessor.sendData("turn", degrees.ToString());
        }

        public void Start()
        {
            if (!_connectionProcessor._drone.Flying)
            {
                _connectionProcessor.sendData("start");
                _connectionProcessor._drone.Flying = true;
            }

        }
        public void Land()
        {
            if (_connectionProcessor._drone.Flying)
            {
                _connectionProcessor.sendData("land");
                _connectionProcessor._drone.Flying = false;
            }
        }
    }
}