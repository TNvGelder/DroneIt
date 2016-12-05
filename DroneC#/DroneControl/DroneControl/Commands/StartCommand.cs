using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
    public class StartCommand : IDroneCommand
    {
        //private DroneProcessor _processor { get; set; }
        private int _degrees { get; set; }

        public void Execute()
        {
            //_processor.Start();
        }

        public void Undo()
        {
            //_processor.Land();
        }

        public StartCommand()
        {
            //_processor = processor;
        }

        public string GetName() {
            return "Start";
        }

        public double GetValue() {
            return _degrees;
        }
    }
}