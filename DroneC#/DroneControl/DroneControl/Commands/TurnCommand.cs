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
        private int _UndoDegrees { get; set; }

        public TurnCommand(DroneController controller, int degrees) {
            _controller = controller;
            _degrees = degrees;
        }

        public void Execute() {
            ApiConnection.Instance.UpdateQualityCheck("Turning to " + _degrees);

            _UndoDegrees = _controller.PointOfView;
            _controller.TurnToWorldDegrees(_degrees);
            _controller.Hover();
        }

        public void Undo() {
            _controller.Turn(_UndoDegrees);
            _controller.Hover();
        }
    }
}