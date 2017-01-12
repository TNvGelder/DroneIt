using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using LineTrackingTest.Models;


namespace LineTrackingTest.Services
{
    /// <summary>
    /// This class is used for detecting the position of the drone relative to lines in given images.
    /// </summary>
    class LineProcessor
    {
        private static volatile LineProcessor _instance;
        private static object syncRoot = new Object();
        public static LineProcessor Instance
        {
            get
            {
                if (_instance == null) {
                    lock (syncRoot) {
                        if (_instance == null)
                            _instance = new LineProcessor();
                    }
                }
                return _instance;
            }
        }
        public double MaxPosRatio = .65;//Max allowed distance from side of picture to line

        private int _regionHeight = 100;
        private int _hue1 = 160;//170, 0
        private int _hue2 = 180;//180, 10
        private int _sat1 = 100;//100, 150
        private int _sat2 = 200;//200, 255

        private int count = 0;

        private string path = "ImageProcessing/";

        private LineProcessor() { }

        /// <summary>
        /// Tries to find the line in the image and returns a group of lines at the edges of the detected line.
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private LineSegment2D[] filterLines(Image<Bgr, Byte> img)
        {
            if (!Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            Image<Hsv, Byte> hsvImage = img.Convert<Hsv, Byte>();
            int width = img.Size.Width;
            //hsvImage.ROI = new Rectangle(0, img.Size.Height / 2 - _regionHeight / 2, width, _regionHeight);
            //hsvImage.Save("../../TestImage/FilteredImage.png");

            Image<Gray, Byte>[] channels = hsvImage.Split();
            Image<Gray, Byte> imgHue = channels[0];
            Image<Gray, Byte> imgSat = channels[1];

            Image<Gray, byte> hueFilter = imgHue.InRange(new Gray(_hue1), new Gray(_hue2));
            Image<Gray, byte> satFilter = imgSat.InRange(new Gray(_sat1), new Gray(_sat2));
            //hueFilter.Save("../../TestImage/GrayImage.png");
            hueFilter = channels[2].InRange(new Gray(0), new Gray(30)); ;
            
            //hueFilter = hueFilter.And(satFilter);

            CvInvoke.Dilate(hueFilter, hueFilter, null, new Point(), 1, BorderType.Default, default(MCvScalar));
            CvInvoke.Erode(hueFilter, hueFilter, null, new Point(), 2, BorderType.Default, default(MCvScalar));
            CvInvoke.Blur(hueFilter, hueFilter, new Size(20, 20), new Point());

            LineSegment2D[] lines = hueFilter.HoughLines(0, 0, 5, 5, 10, 7, 1)[0];

            //Show lines in image for testing purposes
            img = hueFilter.Convert<Bgr, Byte>();
            foreach (LineSegment2D line in lines)
            {
                img.Draw(line, new Bgr(Color.Green), 3);
            }
            img.Save(path + "OutputImage"+count+".png");
            
            return lines;
        }

        /// <summary>
        /// Takes a bitmap of a line to check where the drone flies compared to the line
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns>PositioningState</returns>
        public PositioningState ProcessLine(Bitmap bitmap)
        {
            
            Image<Bgr, Byte> img = new Image<Bgr, Byte>(bitmap);
            int width = img.Size.Width;

            LineSegment2D[] lines = filterLines(img);

            PositioningState result;
            if (lines.Length == 0)
            {
                result = PositioningState.Lost;
            }
            else
            {
                LineSegment2D leftLine = lines.First();
                LineSegment2D rightLine = lines.Last();
                foreach (LineSegment2D line in lines)//Find the line thats closest to the left side and the line closest to right side
                {
                    if (leftLine.Side(line.P1) == -1)
                    {
                        leftLine = line;
                    }
                    else if (rightLine.Side(line.P1) == 1)
                    {
                        rightLine = line;
                    }
                }
                double leftPosRatio = (double)leftLine.P1.X / width;
                double rightPosRatio = (double)(width - rightLine.P1.X) / width;

                if (leftPosRatio > MaxPosRatio)
                {
                    result = PositioningState.Left;
                }
                else if (rightPosRatio > MaxPosRatio)
                {
                    result = PositioningState.Right;
                }
                else
                {
                    result = PositioningState.Correct;
                }

            }
            img.Save(path + "DroneInput" + count + result+ ".png");
            count+= 1;
            return result;
        }

        public bool IsLineVisible(Bitmap bitmap)
        {
            Image<Bgr, Byte> img = new Image<Bgr, Byte>(bitmap);
            LineSegment2D[] lines = filterLines(img);
            img.Save(path + "DroneInput" + count + ".png");
            count += 1;
            return (lines.Length > 0);
        }
    }
}