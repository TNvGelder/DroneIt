using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Processors.DroneProcessors.Commands {
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

        public string GetName() {
            return "Fall";
        }

        public double GetValue() {
            return _squares;
        }
    }
}