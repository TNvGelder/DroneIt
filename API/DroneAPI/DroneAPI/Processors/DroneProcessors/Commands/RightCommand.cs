using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Processors.DroneProcessors.Commands
{
	public class RightCommand : IDroneCommand {
        private DroneProcessor _processor { get; set; }
        private double _squares { get; set; }

        public RightCommand(DroneProcessor processor, double squares) {
            _processor = processor;
            _squares = squares;
        }

        public void Execute() {
            _processor.Right(_squares);
        }

        public void Undo() {
            _processor.Left(_squares);
        }
    }
}