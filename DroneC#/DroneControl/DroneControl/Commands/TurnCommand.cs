using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
    public class TurnCommand : IDroneCommand
    {
        //private DroneProcessor _processor { get; set; }
        private int _degrees { get; set; }

        public void Execute()
        {
            //_processor.Turn(_degrees);
        }

        public void Undo()
        {
            // Doesn't work!
            //_processor.Turn(_degrees);
        }

        public string GetName() {
            return "Turn";
        }

        public double GetValue() {
            return _degrees;
        }

        public TurnCommand(int degrees)
        {
            //_processor = processor;
            _degrees = degrees;   
        }
    }
}