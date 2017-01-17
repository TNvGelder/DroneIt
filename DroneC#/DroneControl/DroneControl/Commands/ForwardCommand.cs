/**
 * @author: Gerhard Kroes
 * */
namespace DroneControl.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class ForwardCommand : IDroneCommand
    {
        private DroneController _controller { get; set; }
        private double _meters { get; set; }

        public ForwardCommand(DroneController controller, double meters) {
            _controller = controller;
            _meters = meters;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Execute()
        {
            // Send a status update to the api
            ApiConnection.Instance.UpdateQualityCheck("Go forward " + _meters + " meters");

            // Make some sound
            Sound.Instance.R2D2e();

            _controller.Forward((float)_meters);
            _controller.Hover();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            _controller.Backward((float)_meters);
            _controller.Hover();
        }
    }
}