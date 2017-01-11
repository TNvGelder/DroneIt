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
        public static void FindLine(DroneController controller, float maxMeters, FlyDirection direction)
        {
            float speed = .1F/3;
            float time = maxMeters/speed*200;
            float oldSpeed = controller.Speed;
            controller.Speed = speed;
            controller.Move(direction);
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
        }
    }
}
