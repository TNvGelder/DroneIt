using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands {
    public class RiseCommand : IDroneCommand {
        //private DroneProcessor _processor { get; set; }
        private double _squares { get; set; }

        public RiseCommand(double squares) {
            //_processor = processor;
            _squares = squares;
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
            return _squares;
        }
    }
}