/**
 * @author: Gerhard Kroes
 * */
namespace DroneControl.Commands
{
    /// <summary>
    /// Drone takeoff or lands
    /// </summary>
    public class StartCommand : IDroneCommand
    {
        private DroneController _controller { get; set; }
        private int _degrees { get; set; }

        public StartCommand(DroneController controller) {
            _controller = controller;
        }

        /// <summary>
        /// Starts the connection, clibrate and takeoff
        /// </summary>
        public void Execute() {
            // Send a status update to the api
            ApiConnection.Instance.UpdateQualityCheck("Starting the engines");

            // Make some sound
            Sound.Instance.R2D2f();

            _controller.Start();
            _controller.Calibrate();
            _controller.Takeoff();
            _controller.Hover();
        }

        /// <summary>
        /// Land
        /// </summary>
        public void Undo() {
            _controller.Land();
        }
    }
}