using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneConnection.Processors.DroneProcessors.Commands
{
    public class BackwardCommand : IDroneCommand
    {
        private DroneProcessor _processor { get; set; }
        private double _squares { get; set; }

        public void Execute()
        {
            _processor.Backwards(_squares);
        }

        public void Undo()
        {
            _processor.Forwards(_squares);
        }

        public BackwardCommand(DroneProcessor processor, double squares)
        {
            _processor = processor;
            _squares = squares;
        }
    }
}