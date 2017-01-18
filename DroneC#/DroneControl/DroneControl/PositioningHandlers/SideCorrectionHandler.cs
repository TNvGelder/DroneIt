using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LineTrackingTest.Models;

namespace DroneControl.PositioningHandlers
{
    /// <summary>
    /// @author: Twan van Gelder
    /// Controls the drone in case the drone is left or right of a line.
    /// </summary>
    public class SideCorrectionHandler : PositioningHandler
    {

        public SideCorrectionHandler(DroneController controller) : base(controller) { }

        /// <summary>
        /// In case the drone is right or left or the line, the drone will fly in the opposite direction.
        /// Otherwise if theres a successor the successor will handle the positioning.
        /// </summary>
        /// <param name="newState"></param>
        /// <param name="oldState"></param>
        /// <param name="isForward"></param>
        /// <returns></returns>
        public override bool HandlePositioningChange(PositioningState newState, PositioningState oldState, bool isForward)
        {
            bool result = true;
            if (newState == PositioningState.Left || newState == PositioningState.Right)
            {
                float oldSpeed = controller.Speed;
                controller.Speed = 0.1F / 6;// Make the drone fly slower when it corrects itself
                                             //so that it will not go too far
                if (newState == PositioningState.Left)
                {
                    controller.Right();
                }
                else{
                    controller.Left();
                }
                controller.Speed = oldSpeed;
            }
            else if (successor != null)
            {
                result = successor.HandlePositioningChange(newState, oldState, isForward);
            }
            return result;
        }
    }
}
