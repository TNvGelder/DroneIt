using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands {
    public class FallCommand : IDroneCommand {
        private object _controller { get; set; }
        private double _meters { get; set; }

        public FallCommand(object controller, double meters) {
            _controller = controller;
            _meters = meters;
        }

        public void Execute() {
            //_processor.Fall(_squares);
        }

        public void Undo() {
            //_processor.Rise(_squares);
        }

        public string GetName() {
            return "Fall";
        }

        public double GetValue() {
            return _meters;
        }
    }
}