using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneConnection
{
    public class ForwardCommand : IDroneCommand
    {
        private DroneProcessor _processor { get; set; }
        private double _squares { get; set; }

        public void Execute()
        {
            _processor.Forwards(_squares);
        }

        public void Undo()
        {
            _processor.Backwards(_squares);
        }

        public ForwardCommand(DroneProcessor processor, double squares)
        {
            _processor = processor;
            _squares = squares;
        }
    }
}