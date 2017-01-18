/**
 * @author: Gerhard Kroes
 * */
namespace DroneControl.Commands
{
    /// <summary>
    /// Moves the drone right and left
    /// </summary>
	public class RightCommand : IDroneCommand {
        private DroneController _controller { get; set; }
        private double _meters { get; set; }

        public RightCommand(DroneController controller, double meters) {
            _controller = controller;
            _meters = meters;
        }

        /// <summary>
        /// Let the drone right for x meters
        /// </summary>
        public void Execute() {
            // Send a status update to the api
            ApiConnection.Instance.UpdateQualityCheck("Go right " + _meters + " meters");

            // Make some sound
            Sound.Instance.R2D2e();

            _controller.Right((int)_meters);
            _controller.Hover();
        }

        /// <summary>
        /// Let the drone left for x meters
        /// </summary>
        public void Undo() {
            _controller.Left((int)_meters);
            _controller.Hover();
        }
    }
}