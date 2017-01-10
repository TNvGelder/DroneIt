using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
    public class LandCommand : IDroneCommand
    {
        private DroneController _controller { get; set; }
        private int _degrees { get; set; }

        public LandCommand(DroneController controller) {
            _controller = controller;
        }

        public void Execute()
        {
            ApiConnection.Instance.UpdateQualityCheck("Landing");

            _controller.Land();
            _controller.Hover();
        }

        public void Undo()
        {
            _controller.Takeoff();
            _controller.Hover();
        }
    }
}