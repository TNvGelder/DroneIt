/**
 * @author: Gerhard Kroes
 * */
namespace DroneControl.Commands
{
    /// <summary>
    /// Drone lands or takeoff
    /// </summary>
    public class LandCommand : IDroneCommand
    {
        private DroneController _controller { get; set; }
        private int _degrees { get; set; }

        public LandCommand(DroneController controller) {
            _controller = controller;
        }

        /// <summary>
        /// Lets the drone land
        /// </summary>
        public void Execute() {
            // Send a status update to the api
            ApiConnection.Instance.UpdateQualityCheck("Landing");

            // Make some sound
            Sound.Instance.R2D2f();

            _controller.Land();
        }

        /// <summary>
        /// Lets the drone takeoff
        /// </summary>
        public void Undo() {
            _controller.Takeoff();
            _controller.Hover();
        }
    }
}