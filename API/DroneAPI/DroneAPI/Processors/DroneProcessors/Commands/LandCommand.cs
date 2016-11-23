using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Processors.DroneProcessors.Commands
{
    public class LandCommand : IDroneCommand
    {
        private DroneProcessor _processor { get; set; }
        private int _degrees { get; set; }

        public void Execute()
        {
            _processor.Land();
        }

        public void Undo()
        {
            _processor.Start();
        }

        public LandCommand(DroneProcessor processor)
        {
            _processor = processor;
        }
    }
}