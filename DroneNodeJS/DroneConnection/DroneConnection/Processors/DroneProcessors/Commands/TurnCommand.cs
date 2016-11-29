using DroneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Processors.DroneProcessors.Commands
{
    public class TurnCommand : IDroneCommand
    {
        private DroneProcessor _processor { get; set; }
        private int _degrees { get; set; }

        public void Execute()
        {
            _processor.Turn(_degrees);
        }

        public void Undo()
        {
            // Doesn't work!
            _processor.Turn(_degrees);
        }

        public TurnCommand(DroneProcessor processor, int degrees)
        {
            _processor = processor;
            _degrees = degrees;   
        }
    }
}