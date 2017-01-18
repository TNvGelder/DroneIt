using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LineTrackingTest.Models;

namespace DroneControl.PositioningHandlers
{
    //DefenceHandlers deal with the attacking player in case they are strong enough or they will let their successor deal with the handler.
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


