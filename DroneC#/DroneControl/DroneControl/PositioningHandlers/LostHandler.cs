using DroneControl.Models;
using DroneControl.Services;
using LineTrackingTest.Models;

namespace DroneControl.PositioningHandlers
{
    /// <summary>
    /// @author: Twan van Gelder
    /// Handles the positioning in case the drone is lost.
    /// </summary>
    class LostHandler : PositioningHandler
    {

        public LostHandler(DroneController controller) : base(controller) { }

        /// <summary>
        /// In case the positioningstate is lost, the drone will try to fly back in the opposite direction from
        /// where it came. If it's not lost and theres a successor, the successor will handle the positioning.
        /// </summary>
        /// <param name="newState"></param>
        /// <param name="oldState"></param>
        /// <param name="isForward"></param>
        /// <returns></returns>
        public override bool HandlePositioningChange(PositioningState newState, PositioningState oldState, bool isForward)
        {
            bool result = true;
            if (newState == PositioningState.Lost)
            {
                FlyDirection direction = getDirection(oldState);
                result = LineNavigator.Instance.FindLine(controller, 2, direction); //Try to find back the line 
            }else if (successor != null)
            {
                result =  successor.HandlePositioningChange(newState, oldState, isForward);
            }
            return result;
        }

        /// <summary>
        /// Converts a positioningstate to a corresponding direction
        /// </summary>
        /// <param name="state"></param>
        /// <param name="isForward"></param>
        /// <returns></returns>
        private FlyDirection getDirection(PositioningState state)
        {
            if (state == PositioningState.Left)
            {
                return FlyDirection.Right;
            }
            else if (state == PositioningState.Right)
            {
                return FlyDirection.Left;
            }
            else
            {
                return FlyDirection.None;
            }
        }
    }
}