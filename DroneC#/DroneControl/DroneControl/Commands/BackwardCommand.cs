using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
    public class BackwardCommand : IDroneCommand
    {
        private DroneController _controller { get; set; }
        private double _meters { get; set; }

        public BackwardCommand(DroneController controller, double meters) {
            _controller = controller;
            _meters = meters;
        }

        public void Execute()
        {
            _controller.Backward((float)_meters);
        }

        public void Undo()
        {
            _controller.Forward((float)_meters);
        }

        public string GetName() {
            return "Backward";
        }

        public double GetValue() {
            return _meters;
        }
    }
}