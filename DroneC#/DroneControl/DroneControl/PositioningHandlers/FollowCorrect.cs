using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LineTrackingTest.Models;

namespace DroneControl.PositioningHandlers
{

    /// <summary>
    /// @author: Twan van Gelder
    /// Controls the drone in case the positioning is correct.
    /// </summary>
    public class FollowCorrect : PositioningHandler
    {

        public FollowCorrect(DroneController controller) : base(controller){ }

        /// <summary>
        /// In case the position is correct the drone will fly forwards or backwards
        /// If it's not correct the successor will handle the positioning.
        /// </summary>
        /// <param name="newState"></param>
        /// <param name="oldState"></param>
        /// <param name="isForward"></param>
        /// <returns></returns>
        public override bool HandlePositioningChange(PositioningState newState, PositioningState oldState, bool isForward)
        {
            bool result = true;
            if (newState == PositioningState.Correct)
            {
                if (isForward)
                {
                    controller.Forward();
                }
                else
                {
                    controller.Backward();
                }
            }
            else if (successor != null)
            {
                result = successor.HandlePositioningChange(newState, oldState, isForward);
            }
            return result;
        }
    }
}
