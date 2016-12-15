using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
	public class RightCommand : IDroneCommand {
        private DroneController _controller { get; set; }
        private double _meters { get; set; }

        public RightCommand(DroneController controller, double meters) {
            _controller = controller;
            _meters = meters;
        }

        public void Execute() {
            _controller.Right((int)_meters);
            _controller.Hover();
        }

        public void Undo() {
            _controller.Left((int)_meters);
            _controller.Hover();
        }

        public string GetName() {
            return "Right";
        }

        public double GetValue() {
            return _meters;
        }
    }
}