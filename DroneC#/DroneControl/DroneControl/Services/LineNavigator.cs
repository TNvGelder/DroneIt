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
        private static void move(DroneController controller, FlyDirection direction)
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

        public static bool FindLine(DroneController controller, float maxMeters, FlyDirection direction)
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
                lineDetected = LineProcessor.IsLineVisible(bmp);
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
