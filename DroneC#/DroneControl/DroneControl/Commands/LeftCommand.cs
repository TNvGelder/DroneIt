using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands {
    public class LeftCommand : IDroneCommand {
        //private DroneProcessor _processor { get; set; }
        private double _squares { get; set; }

        public LeftCommand(double squares) {
            //_processor = processor;
            _squares = squares;
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
            return _squares;
        }
    }
}