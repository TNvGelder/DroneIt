using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
    public class TurnCommand : IDroneCommand
    {
        private object _controller { get; set; }
        private int _degrees { get; set; }

        public TurnCommand(object controller, int degrees) {
            _controller = controller;
            _degrees = degrees;
        }

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
    }
}