using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands {
    public class RiseCommand : IDroneCommand {
        private object _controller { get; set; }
        private double _meters { get; set; }

        public RiseCommand(object controller, double meters) {
            _controller = controller;
            _meters = meters;
        }

        public void Execute() {
            //_processor.Rise(_squares);
        }

        public void Undo() {
            //_processor.Fall(_squares);
        }

        public string GetName() {
            return "Rise";
        }

        public double GetValue() {
            return _meters;
        }
    }
}