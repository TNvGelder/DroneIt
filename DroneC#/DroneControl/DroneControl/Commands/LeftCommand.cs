using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands {
    public class LeftCommand : IDroneCommand {
        private object _controller { get; set; }
        private double _meters { get; set; }

        public LeftCommand(object controller, double meters) {
            _controller = controller;
            _meters = meters;
        }

        public void Execute() {
            //_processor.Left(_squares);
        }

        public void Undo() {
            //_processor.Right(_squares);
        }

        public string GetName() {
            return "Left";
        }

        public double GetValue() {
            return _meters;
        }
    }
}