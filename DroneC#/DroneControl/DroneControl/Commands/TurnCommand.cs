/**
 * @author: Gerhard Kroes
 * */
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

        /// <summary>
        /// Turn to degrees and saves current degrees
        /// </summary>
        public void Execute() {
            // Send a status update to the api
            ApiConnection.Instance.UpdateQualityCheck("Turning to " + _degrees);

            // Make sound
            Sound.Instance.R2D2b();

            // Save current degrees
            _UndoDegrees = _controller.PointOfView;

            _controller.TurnToWorldDegrees(_degrees);
            _controller.Hover();
        }

        /// <summary>
        /// Turn back
        /// </summary>
        public void Undo() {
            _controller.Turn(_UndoDegrees);
            _controller.Hover();
        }
    }
}