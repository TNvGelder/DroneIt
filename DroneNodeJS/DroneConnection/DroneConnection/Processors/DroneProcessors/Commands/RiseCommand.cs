using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Processors.DroneProcessors.Commands {
    public class RiseCommand : IDroneCommand {
        private DroneProcessor _processor { get; set; }
        private double _squares { get; set; }

        public RiseCommand(DroneProcessor processor, double squares) {
            _processor = processor;
            _squares = squares;
        }

        public void Execute() {
            _processor.Rise(_squares);
        }

        public void Undo() {
            _processor.Fall(_squares);
        }
    }
}