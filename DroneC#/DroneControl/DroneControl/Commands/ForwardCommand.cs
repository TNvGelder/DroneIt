using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
    public class ForwardCommand : IDroneCommand
    {
        private object _controller { get; set; }
        private double _meters { get; set; }

        public ForwardCommand(object controller, double meters) {
            _controller = controller;
            _meters = meters;
        }

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
            return _meters;
        }
    }
}