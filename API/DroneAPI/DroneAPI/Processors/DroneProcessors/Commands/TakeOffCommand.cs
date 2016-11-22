using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Processors.DroneProcessors.Commands
{
    public class TakeOffCommand : IDroneCommand
    {
        private DroneProcessor _processor { get; set; }
        private int _degrees { get; set; }

        public void Execute()
        {
            _processor.TakeOff();
        }

        public void Undo()
        {
            _processor.Land();
        }

        public TakeOffCommand(DroneProcessor processor)
        {
            _processor = processor;
        }
    }
}