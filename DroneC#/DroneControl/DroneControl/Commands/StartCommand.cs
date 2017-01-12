using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
    public class StartCommand : IDroneCommand
    {
        private DroneController _controller { get; set; }
        private int _degrees { get; set; }

        public StartCommand(DroneController controller) {
            _controller = controller;
        }

        public void Execute() {
            ApiConnection.Instance.UpdateQualityCheck("Starting the engines");
            Sound.Instance.R2D2f();
            _controller.Start();
            _controller.Calibrate();
            _controller.Takeoff();
            _controller.Hover();
        }

        public void Undo() {
            _controller.Land();
        }
    }
}