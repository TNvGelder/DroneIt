/**
 * @author: Gerhard Kroes
 * */
namespace DroneControl.Commands {
    /// <summary>
    /// Moves the drone up and down
    /// </summary>
    public class RiseCommand : IDroneCommand {
        private DroneController _controller { get; set; }
        private double _meters { get; set; }

        public RiseCommand(DroneController controller, double meters) {
            _controller = controller;
            _meters = meters;
        }

        /// <summary>
        /// Let the drone rise for x meters
        /// </summary>
        public void Execute() {
            // Send a status update to the api
            ApiConnection.Instance.UpdateQualityCheck("Rise " + _meters + " meters");

            // Make some sound
            Sound.Instance.R2D2c();

            _controller.Rise((float)_meters);
            _controller.Hover();
        }

        /// <summary>
        /// Let the drone fall for x meters
        /// </summary>
        public void Undo() {
            _controller.Fall((float)_meters);
            _controller.Hover();
        }
    }
}