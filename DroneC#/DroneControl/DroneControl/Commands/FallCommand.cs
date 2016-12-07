using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands {
    public class FallCommand : IDroneCommand {
        private DroneController _controller { get; set; }
        private double _meters { get; set; }

        public FallCommand(DroneController controller, double meters) {
            _controller = controller;
            _meters = meters;
        }

        public void Execute() {
            _controller.Down((float)_meters);
        }

        public void Undo() {
            _controller.Up((float)_meters);
        }

        public string GetName() {
            return "Fall";
        }

        public double GetValue() {
            return _meters;
        }
    }
}