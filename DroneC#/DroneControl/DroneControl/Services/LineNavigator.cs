using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using DroneControl.Models;
using LineTrackingTest.Services;

namespace DroneControl.Services
{
    /// <summary>
    /// This class is used for finding the line. 
    /// It's not required that the line is visible for the drone for the navigation in this class.
    /// </summary>
    class LineNavigator
    {
        private static volatile LineNavigator _instance;
        private static object _syncRoot = new Object();

        //Gets or creates an instance of line navigator.
        public static LineNavigator Instance
        {
            get
            {
                if (_instance == null) {
                    lock (_syncRoot) {
                        if (_instance == null)
                            _instance = new LineNavigator();
                    }
                }
                return _instance;
            }
        }

        private LineNavigator() { }

        
        
        /// <summary>
        /// Makes the drone fly in a direction for a maximum amount of meters until the line is found.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="maxMeters"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool FindLine(DroneController controller, float maxMeters, FlyDirection direction)
        {
            if (direction == FlyDirection.None)
            {
                controller.Land();
                return false;
            }
            bool lineDetected = findLine(controller, maxMeters, direction);
            if (lineDetected)
            {
                controller.Hover();
            }
            else
            {
                controller.Land();
            }
            return lineDetected;
        }

        /// <summary>
        /// Checks if the drone can detect the line when flying in the given direction for the given amount of maximum meters.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="maxMeters"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private bool findLine(DroneController controller, float maxMeters, FlyDirection direction)
        {
            float speed = .1F / 3;
            float time = maxMeters / speed * 200;
            float oldSpeed = controller.Speed;
            controller.Speed = speed;
            move(controller, direction);
            Stopwatch s = new Stopwatch();
            s.Start();
            bool lineDetected = false;
            while (!lineDetected && s.Elapsed < TimeSpan.FromMilliseconds(time))
            {
                Bitmap bmp = controller.GetBitmapFromBottomCam();
                lineDetected = LineProcessor.Instance.IsLineVisible(bmp);
            }
            controller.Speed = oldSpeed;
            s.Stop();
            return lineDetected;
        }

        //Moves the drone in a direction.
        private void move(DroneController controller, FlyDirection direction)
        {
            if (direction == FlyDirection.Forward)
            {
                controller.Forward();
            }
            else if (direction == FlyDirection.Backward)
            {
                controller.Backward();
            }
            else if (direction == FlyDirection.Left)
            {
                controller.Left();
            }
            else if (direction == FlyDirection.Right)
            {
                controller.Right();
            }
        }
    }
}
