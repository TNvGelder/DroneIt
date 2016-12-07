using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
    public class LandCommand : IDroneCommand
    {
        private object _controller { get; set; }
        private int _degrees { get; set; }

        public LandCommand(object controller) {
            _controller = controller;
        }

        public void Execute()
        {
            //_processor.Land();
        }

        public void Undo()
        {
            //_processor.Start();
        }

        public string GetName() {
            return "Backward";
        }

        public double GetValue() {
            return _degrees;
        }
    }
}