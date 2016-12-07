using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
    public class ForwardCommand : IDroneCommand
    {
        private DroneController _controller { get; set; }
        private double _meters { get; set; }

        public ForwardCommand(DroneController controller, double meters) {
            _controller = controller;
            _meters = meters;
        }

        public void Execute()
        {
            _controller.Up((float)_meters);
        }

        public void Undo()
        {
            _controller.Down((float)_meters);
        }

        public string GetName() {
            return "Forward";
        }

        public double GetValue() {
            return _meters;
        }
    }
}