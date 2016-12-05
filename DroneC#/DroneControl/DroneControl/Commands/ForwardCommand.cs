using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
    public class ForwardCommand : IDroneCommand
    {
        //private DroneProcessor _processor { get; set; }
        private double _squares { get; set; }

        public void Execute()
        {
            //_processor.Forwards(_squares);
        }

        public void Undo()
        {
            //_processor.Backwards(_squares);
        }

        public string GetName() {
            return "Forward";
        }

        public double GetValue() {
            return _squares;
        }

        public ForwardCommand(double squares)
        {
            //_processor = processor;
            _squares = squares;
        }
    }
}