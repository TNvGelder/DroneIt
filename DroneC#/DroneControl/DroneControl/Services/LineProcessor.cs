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
    /// @Author: Twan van Gelder
    /// This class is used for detecting the position of the drone relative to lines in given images.
    /// </summary>
    class LineProcessor
    {
        private static volatile LineProcessor _instance;
        private static object _syncRoot = new Object();
        public static LineProcessor Instance
        {
            get
            {
                if (_instance == null) {
                    lock (_syncRoot) {
                        if (_instance == null)
                            _instance = new LineProcessor();
                    }
                }
                return _instance;
            }
        }
        public double MaxPosRatio = .65;//Max allowed distance from side of picture to line

        private int count = 0;

        //Path for saving a log of the imageprocessing
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
            Image<Gray, Byte>[] channels = hsvImage.Split();
            Image<Gray, byte> grayImg = channels[2].InRange(new Gray(0), new Gray(30));

            //Decrease noise in the images
            CvInvoke.Dilate(grayImg, grayImg, null, new Point(), 1, BorderType.Default, default(MCvScalar));
            CvInvoke.Erode(grayImg, grayImg, null, new Point(), 2, BorderType.Default, default(MCvScalar));
            CvInvoke.Blur(grayImg, grayImg, new Size(20, 20), new Point());

            LineSegment2D[] lines = grayImg.HoughLines(0, 0, 5, 5, 10, 7, 1)[0];
            //Show lines in image for logging and testing purposes
            img = grayImg.Convert<Bgr, Byte>();
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

        /// <summary>
        /// Checks if the line is visible and in the center of the image.
        /// </summary>

        public bool IsLineVisible(Bitmap bitmap)
        {

            Image<Bgr, Byte> img = new Image<Bgr, Byte>(bitmap);
            return ProcessLine(bitmap) == PositioningState.Correct;
        }
    }
}