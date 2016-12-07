using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
    public class StartCommand : IDroneCommand
    {
        private object _controller { get; set; }
        private int _degrees { get; set; }

        public StartCommand(object controller) {
            _controller = controller;
        }

        public void Execute()
        {
            //_processor.Start();
        }

        public void Undo()
        {
            //_processor.Land();
        }

        public string GetName() {
            return "Start";
        }

        public double GetValue() {
            return _degrees;
        }
    }
}