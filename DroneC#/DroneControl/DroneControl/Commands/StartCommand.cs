using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
    public class StartCommand : IDroneCommand
    {
        private DroneController _controller { get; set; }
        private int _degrees { get; set; }

        public StartCommand(DroneController controller) {
            _controller = controller;
        }

        public void Execute() {
            _controller.Start();
            _controller.Calibrate();
            _controller.Takeoff();
            _controller.Hover();
        }

        public void Undo() {
            _controller.Land();
        }

        public string GetName() {
            return "Start";
        }

        public double GetValue() {
            return _degrees;
        }
    }
}