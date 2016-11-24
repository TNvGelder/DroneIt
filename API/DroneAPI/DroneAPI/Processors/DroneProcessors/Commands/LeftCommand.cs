using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Processors.DroneProcessors.Commands {
    public class LeftCommand : IDroneCommand {
        private DroneProcessor _processor { get; set; }
        private int _squares { get; set; }

        public LeftCommand(DroneProcessor processor, int squares) {
            _processor = processor;
            _squares = squares;
        }

        public void Execute() {
            _processor.Left(_squares);
        }

        public void Undo() {
            _processor.Right(_squares);
        }
    }
}