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
    class CircleProcessor
    {
        private static volatile CircleProcessor _instance;
        private static object syncRoot = new Object();
        public static CircleProcessor Instance
        {
            get
            {
                if (_instance == null) {
                    lock (syncRoot) {
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
            imgGray = imgGray.InRange(new Gray(0), new Gray(20));

            CvInvoke.Dilate(imgGray, imgGray, null, new Point(), 1, BorderType.Default, default(MCvScalar));
            CvInvoke.Erode(imgGray, imgGray, null, new Point(), 2, BorderType.Default, default(MCvScalar));
            CvInvoke.Blur(imgGray, imgGray, new Size(20, 20), new Point());
            CircleF[] circles = imgGray.HoughCircles(new Gray(12), new Gray(100), 1, 200, 70, 400)[0];
            img = imgGray.Convert<Bgr, Byte>();
            foreach (CircleF circle in circles)
            {
                img.Draw(circle, new Bgr(Color.Green), 3);
            }
            img.Save("../../TestImage/CircleOutputImage.png");

            return circles.Length > 0;
        }
    }
}
