using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
    public class TurnCommand : IDroneCommand
    {
        private DroneController _controller { get; set; }
        private int _degrees { get; set; }

        public TurnCommand(DroneController controller, int degrees) {
            _controller = controller;
            _degrees = degrees;
        }

        public void Execute() {
            _controller.Turn(_degrees);
        }

        public void Undo() {
            // Doesn't work!
            _controller.Turn(_degrees);
        }

        public string GetName() {
            return "Turn";
        }

        public double GetValue() {
            return _degrees;
        }
    }
}