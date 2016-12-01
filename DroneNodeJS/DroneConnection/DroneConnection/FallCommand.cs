using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneConnection {
    public class FallCommand : IDroneCommand {
        private DroneProcessor _processor { get; set; }
        private int _squares { get; set; }

        public FallCommand(DroneProcessor processor, int squares) {
            _processor = processor;
            _squares = squares;
        }

        public void Execute() {
            _processor.Fall(_squares);
        }

        public void Undo() {
            _processor.Rise(_squares);
        }
    }
}