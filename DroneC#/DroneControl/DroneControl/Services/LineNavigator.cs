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
    /// 
    /// </summary>
    class LineNavigator
    {
        private static volatile LineNavigator _instance;
        private static object syncRoot = new Object();
        public static LineNavigator Instance
        {
            get
            {
                if (_instance == null) {
                    lock (syncRoot) {
                        if (_instance == null)
                            _instance = new LineNavigator();
                    }
                }
                return _instance;
            }
        }

        private LineNavigator() { }

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

        public bool FindLine(DroneController controller, float maxMeters, FlyDirection direction)
        {
            if (direction == FlyDirection.None)
            {
                controller.Land();
                return false;
            }
            float speed = .1F/3;
            float time = maxMeters/speed*200;
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
            if (lineDetected)
            {
                controller.Hover();
            }
            else
            {
                controller.Land();
            }
            controller.Speed = oldSpeed;
            s.Stop();
            return lineDetected;
        }
    }
}
