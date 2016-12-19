using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands {
    public class RiseCommand : IDroneCommand {
        private DroneController _controller { get; set; }
        private double _meters { get; set; }

        public RiseCommand(DroneController controller, double meters) {
            _controller = controller;
            _meters = meters;
        }

        public void Execute() {
            _controller.Rise((float)_meters);
            _controller.Hover();
        }

        public void Undo() {
            _controller.Fall((float)_meters);
            _controller.Hover();
        }

        public string GetName() {
            return "Rise";
        }

        public double GetValue() {
            return _meters;
        }
    }
}