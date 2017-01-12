using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
	public class RightCommand : IDroneCommand {
        private DroneController _controller { get; set; }
        private double _meters { get; set; }

        public RightCommand(DroneController controller, double meters) {
            _controller = controller;
            _meters = meters;
        }

        public void Execute() {
            ApiConnection.Instance.UpdateQualityCheck("Go right " + _meters + " meters");
            Sound.Instance.R2D2e();
            _controller.Right((int)_meters);
            _controller.Hover();
        }

        public void Undo() {
            _controller.Left((int)_meters);
            _controller.Hover();
        }
    }
}