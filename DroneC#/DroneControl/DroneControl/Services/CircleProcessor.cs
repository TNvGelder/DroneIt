using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace DroneControl.Services
{
    /// <summary>
    /// @author: Twan van Gelder
    /// 
    /// </summary>
    class CircleProcessor
    {
        private static volatile CircleProcessor _instance;
        private static object _syncRoot = new Object();

        //Get an instance of circleprocessor 
        public static CircleProcessor Instance
        {
            get
            {
                if (_instance == null) {
                    lock (_syncRoot) {
                        if (_instance == null)
                            _instance = new CircleProcessor();
                    }
                }
                return _instance;
            }
        }

        private CircleProcessor() { }

        public bool IsCircleInCenter(Bitmap bitmap)
        {
            Image<Bgr, Byte> img = new Image<Bgr, Byte>(bitmap);
            Image<Hsv, Byte> hsvImage = img.Convert<Hsv, Byte>();

            Image<Gray, Byte>[] channels = hsvImage.Split();
            Image<Gray, Byte> imgGray = channels[2];
            imgGray = imgGray.InRange(new Gray(200), new Gray(255));

            CvInvoke.Dilate(imgGray, imgGray, null, new Point(), 1, BorderType.Default, default(MCvScalar));
            CvInvoke.Erode(imgGray, imgGray, null, new Point(), 2, BorderType.Default, default(MCvScalar));
            CvInvoke.Blur(imgGray, imgGray, new Size(20, 20), new Point());
            CircleF[] circles = imgGray.HoughCircles(new Gray(12), new Gray(30), 1, 200, 50, 100)[0];
            img = imgGray.Convert<Bgr, Byte>();
            foreach (CircleF circle in circles)
            {
                img.Draw(circle, new Bgr(Color.Green), 3);
            }
            img.Save("../../TestImage/CircleOutputImage.png");

            if (circles.Length > 0)
            {
                Console.WriteLine("Circle detected");
            }

            return circles.Length > 0;
        }
    }
}
