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
    class LineProcessor
    {
        public static double minPosRatio = .45;
        public static PositioningState ProcessLine(Bitmap bitmap)
        {
            //Bgr orange1 = new Bgr(15, 58, 243);
            //Bgr orange2 = new Bgr(11, 83, 255);

            
            Image<Bgr, Byte> img = new Image<Bgr, Byte>(bitmap);

            CvInvoke.CvtColor(img, img, ColorConversion.Bgr2Hsv);

            int width = img.Size.Width;
            int regionHeight = 100;
            img.ROI = new Rectangle(0, img.Size.Height/2 - regionHeight/2, width, regionHeight);

            Image<Gray, Byte>[] channels = img.Split();
            Image<Gray, Byte> imgHue = channels[0];
            Image<Gray, Byte> imgSat = channels[1];


            //Image<Gray, byte> hueFilter = imgHue.InRange(new Gray(0), new Gray(10));
            //Image<Gray, byte> satFilter = imgSat.InRange(new Gray(150), new Gray(255));

            Image<Gray, byte> hueFilter = imgHue.InRange(new Gray(170), new Gray(180));
            Image<Gray, byte> satFilter = imgSat.InRange(new Gray(100), new Gray(200));

            hueFilter = hueFilter.And(satFilter);


            img.Save("../../TestImage/FilteredImage.png");
            CvInvoke.Dilate(hueFilter, hueFilter, null, new Point(), 1, BorderType.Default, default(MCvScalar));
            CvInvoke.Erode(hueFilter, hueFilter, null, new Point(),2, BorderType.Default, default(MCvScalar));
            CvInvoke.Blur(hueFilter, hueFilter, new Size(20, 20), new Point());
            img = hueFilter.Convert<Bgr, Byte>();

            LineSegment2D[] lines = hueFilter.HoughLines(0, 0, 5, 5, 10, 7, 1)[0];

            foreach (LineSegment2D line in lines)
            {
                img.Draw(line, new Bgr(Color.Green), 3);
            }

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
                if (leftPosRatio < minPosRatio)
                {
                    result = PositioningState.Left;
                }else if (rightPosRatio < minPosRatio)
                {
                    result = PositioningState.Right;
                }
                else
                {
                    result = PositioningState.Correct;
                }
                
            }
            


            //hueFilter.Save("../../TestImage/FilteredImage.png");
            img.Save("../../TestImage/OutputImage.png");
            return result;
        }
    }
}