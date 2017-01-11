using System;
using System.Collections.Generic;
using System.Drawing;
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
        public static double MaxPosRatio = .65;//Max allowed distance from side of picture to line

        private static int _regionHeight = 100;
        private static int _hue1 = 160;//170, 0
        private static int _hue2 = 180;//180, 10
        private static int _sat1 = 100;//100, 150
        private static int _sat2 = 200;//200, 255

        

        /// <summary>
        /// Tries to find the line in the image and returns a group of lines at the edges of the detected line.
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static LineSegment2D[] filterLines(Image<Bgr, Byte> img)
        {
            img.Save("../../TestImage/DroneInput.png");
            Image<Hsv, Byte> hsvImage = img.Convert<Hsv, Byte>();
            int width = img.Size.Width;
            //hsvImage.ROI = new Rectangle(0, img.Size.Height / 2 - _regionHeight / 2, width, _regionHeight);
            hsvImage.Save("../../TestImage/FilteredImage.png");

            Image<Gray, Byte>[] channels = hsvImage.Split();
            Image<Gray, Byte> imgHue = channels[0];
            Image<Gray, Byte> imgSat = channels[1];

            Image<Gray, byte> hueFilter = imgHue.InRange(new Gray(_hue1), new Gray(_hue2));
            Image<Gray, byte> satFilter = imgSat.InRange(new Gray(_sat1), new Gray(_sat2));
            hueFilter = hueFilter.And(satFilter);

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
            img.Save("../../TestImage/OutputImage.png");

            return lines;
        }

        /// <summary>
        /// Takes a bitmap of a line to check where the drone flies
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns>PositioningState</returns>
        public static PositioningState ProcessLine(Bitmap bitmap)
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

            return result;
        }

        public static bool IsCircleInCenter(Bitmap bitmap)
        {
            Image<Bgr, Byte> img = new Image<Bgr, Byte>(bitmap);

            img.Save("../../TestImage/DroneInput.png");
            Image<Hsv, Byte> hsvImage = img.Convert<Hsv, Byte>();

            hsvImage.Save("../../TestImage/FilteredImage.png");

            Image<Gray, Byte>[] channels = hsvImage.Split();
            Image<Gray, Byte> imgGray = channels[2];
            //Image<Gray, Byte> imgHue = channels[0];
            //Image<Gray, Byte> imgSat = channels[1];
            //Image<Gray, byte> hueFilter = imgHue.And(imgSat);
            //Image<Gray, byte> hueFilter = imgHue.InRange(new Gray(_hue1), new Gray(_hue2));
            //Image<Gray, byte> satFilter = imgSat.InRange(new Gray(_sat1), new Gray(_sat2));
            //hueFilter = hueFilter.And(satFilter);
            imgGray = imgGray.InRange(new Gray(0), new Gray(20));

            CvInvoke.Dilate(imgGray, imgGray, null, new Point(), 1, BorderType.Default, default(MCvScalar));
            CvInvoke.Erode(imgGray, imgGray, null, new Point(), 2, BorderType.Default, default(MCvScalar));
            CvInvoke.Blur(imgGray, imgGray, new Size(20, 20), new Point());
            CircleF[]circles = imgGray.HoughCircles(new Gray(12), new Gray(260), 1, 10,50,50)[0];
            img = imgGray.Convert<Bgr, Byte>();
            foreach (CircleF circle in circles)
            {
                img.Draw(circle, new Bgr(Color.Green), 3);
            }
            img.Save("../../TestImage/OutputImage.png");

            return true;
        }

        public static bool Test(Bitmap bitmap)
        {
            Image<Bgr, Byte> img = new Image<Bgr, Byte>(bitmap);
            img.Save("../../TestImage/DroneInput.png");
            Image<Hsv, Byte> hsvImage = img.Convert<Hsv, Byte>();

            hsvImage.Save("../../TestImage/FilteredImage.png");

            Image<Gray, Byte>[] channels = hsvImage.Split();
            Image<Gray, Byte> Img_Source_Gray = channels[0];
            Image<Bgr, byte> Img_Result_Bgr = new Image<Bgr, byte>(Img_Source_Gray.Width, Img_Source_Gray.Height);
            //CvInvoke.CvtColor();
            //CvInvoke.CvtColor(Img_Source_Gray.Ptr, Img_Result_Bgr.Ptr, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_GRAY2BGR);
            Gray cannyThreshold = new Gray(12);
            Gray circleAccumulatorThreshold = new Gray(26);
            double Resolution = 1;
            double MinDistance = 10.0;
            int MinRadius = 0;
            int MaxRadius = 0;

            CircleF[] HoughCircles = Img_Source_Gray.HoughCircles(
                                    cannyThreshold,
                                    circleAccumulatorThreshold,
                                    Resolution, //Resolution of the accumulator used to detect centers of the circles
                                    MinDistance, //min distance 
                                    MinRadius, //min radius
                                    MaxRadius //max radius
                                    )[0]; //Get the circles from the first channel
            #region draw circles
            foreach (CircleF circle in HoughCircles)
                Img_Result_Bgr.Draw(circle, new Bgr(Color.Red), 2);
            #endregion

            //imageBox1.Image = Img_Result_Bgr;
            return true;
        }

        public static bool IsLineVisible(Bitmap bitmap)
        {
            Image<Bgr, Byte> img = new Image<Bgr, Byte>(bitmap);
            LineSegment2D[] lines = filterLines(img);
            return (lines.Length > 0);
        }
    }
}