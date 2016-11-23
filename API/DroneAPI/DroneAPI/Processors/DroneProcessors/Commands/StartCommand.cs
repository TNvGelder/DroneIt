using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Processors.DroneProcessors.Commands
{
    public class StartCommand : IDroneCommand
    {
        private DroneProcessor _processor { get; set; }
        private int _degrees { get; set; }

        public void Execute()
        {
            _processor.Start();
        }

        public void Undo()
        {
            _processor.Land();
        }

        public StartCommand(DroneProcessor processor)
        {
            _processor = processor;
        }
    }
}