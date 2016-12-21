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
        public static double _minPosRatio = .40;

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
            Image<Hsv, Byte> hsvImage = img.Convert<Hsv, Byte>();
            int width = img.Size.Width;
            //hsvImage.ROI = new Rectangle(0, img.Size.Height / 2 - _regionHeight / 2, width, _regionHeight);
            hsvImage.Save("../../TestImage/FilteredImage.png");

            Image<Gray, Byte>[] channels = hsvImage.Split();
            Image<Gray, Byte> imgHue = channels[0];
            Image<Gray, Byte> imgSat = channels[1];

            Image<Gray, byte> hueFilter = imgHue.InRange(new Gray( _hue1), new Gray(_hue2));
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
                    }else if (rightLine.Side(line.P1) == 1)
                    {
                        rightLine = line;
                    }
                }
                double leftPosRatio = (double) leftLine.P1.X/width;
                double rightPosRatio = (double) (width - rightLine.P1.X)/width;
                if (leftPosRatio < _minPosRatio)
                {
                    result = PositioningState.Left;
                }else if (rightPosRatio < _minPosRatio)
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
    }
}