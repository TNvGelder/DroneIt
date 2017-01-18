using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LineTrackingTest.Models;

namespace DroneControl.PositioningHandlers
{
    /// <summary>
    /// @author: Twan van Gelder
    /// Positioninghandlers decide how to control the drone from given positioningstates.
    /// If a positioninghandler does not know how to deal with given input and there is a successor
    /// the successor will handle the positioning.
    /// </summary>
    public abstract class PositioningHandler
    {
        protected PositioningHandler successor;
        protected DroneController controller;

        public PositioningHandler Successor
        {
            get { return successor; }
            set { successor = value; }
        }

        public PositioningHandler(DroneController controller)
        {
            this.controller = controller;
        }

        /// <summary>
        /// Implementations should control the drone based on the change and should return false if they land.
        /// If the drone is still flying it should return true.
        /// </summary>
        /// <param name="newState"></param>
        /// <param name="oldState"></param>
        /// <param name="isForward"></param>
        /// <returns></returns>
        public abstract bool HandlePositioningChange(PositioningState newState, PositioningState oldState, bool isForward);
    }
}


