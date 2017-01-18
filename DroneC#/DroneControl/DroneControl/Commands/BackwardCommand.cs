using System;
using System.Collections.Generic;
using System.Linq;
/**
 * @author: Gerhard Kroes
 * */
namespace DroneControl.Commands
{
    /// <summary>
    /// Backward command moves the drone backward and forward
    /// </summary>
    public class BackwardCommand : IDroneCommand
    {
        private DroneController _controller { get; set; }
        private double _meters { get; set; }

        public BackwardCommand(DroneController controller, double meters) {
            _controller = controller;
            _meters = meters;
        }

        /// <summary>
        /// Go backwards x meters
        /// </summary>
        public void Execute()
        {
            // Send a status update to the api
            ApiConnection.Instance.UpdateQualityCheck("Go backward " + _meters + " meters");

            // Make some sound
            Sound.Instance.R2D2e();

            _controller.Backward((float)_meters);
            _controller.Hover();
        }

        /// <summary>
        /// Go forward x meters
        /// </summary>
        public void Undo()
        {
            _controller.Forward((float)_meters);
            _controller.Hover();
        }
    }
}