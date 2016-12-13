using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
    public class LandCommand : IDroneCommand
    {
        private DroneController _controller { get; set; }
        private int _degrees { get; set; }

        public LandCommand(DroneController controller) {
            _controller = controller;
        }

        public void Execute()
        {
            _controller.Land();
        }

        public void Undo()
        {
            _controller.Takeoff();
        }

        public string GetName() {
            return "Backward";
        }

        public double GetValue() {
            return _degrees;
        }
    }
}