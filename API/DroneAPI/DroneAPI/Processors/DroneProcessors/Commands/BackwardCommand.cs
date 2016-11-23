using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Processors.DroneProcessors.Commands
{
    public class BackwardCommand : IDroneCommand
    {
        private DroneProcessor _processor { get; set; }
        private int _squares { get; set; }

        public void Execute()
        {
            _processor.Backwards(_squares);
        }

        public void Undo()
        {
            _processor.Forwards(_squares);
        }

        public BackwardCommand(DroneProcessor processor, int squares)
        {
            _processor = processor;
            _squares = squares;
        }
    }
}