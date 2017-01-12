using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
    public class BackwardCommand : IDroneCommand
    {
        private DroneController _controller { get; set; }
        private double _meters { get; set; }

        public BackwardCommand(DroneController controller, double meters) {
            _controller = controller;
            _meters = meters;
        }

        public void Execute()
        {
            ApiConnection.Instance.UpdateQualityCheck("Go backward " + _meters + " meters");
            Sound.Instance.R2D2e();
            _controller.Backward((float)_meters);
            _controller.Hover();
        }

        public void Undo()
        {
            _controller.Forward((float)_meters);
            _controller.Hover();
        }
    }
}