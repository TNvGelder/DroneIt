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


namespace LineTrackingTest.Services
{
    class LineProcessor
    {

        public static void ProcessLine()
        {
            Bitmap myBmp = new Bitmap(Bitmap.FromFile("../../TestImage/StraightLineTestImage.png"));
            Image<Bgr, Byte> img = new Image<Bgr, Byte>(myBmp);

            //Emgu.CV.CvEnum.LoadImageType.AnyColor

            CvInvoke.CvtColor(img, img, ColorConversion.Bgr2Hsv);
            CvInvoke.Blur(img, img, new Size(50, 50), new Point());

            int regionHeight = 100;
            img.ROI = new Rectangle(0, img.Height/2 - regionHeight/2, img.Size.Width, regionHeight);

            Image<Gray, Byte>[] channels = img.Split();
            Image<Gray, Byte> imgHue = channels[0];
            Image<Gray, Byte> imgSat = channels[1];


            Image<Gray, byte> hueFilter = imgHue.InRange(new Gray(0), new Gray(10));
            Image<Gray, byte> satFilter = imgSat.InRange(new Gray(50), new Gray(255));

            hueFilter = hueFilter.And(satFilter);

            Bgr orange1 = new Bgr(15, 58, 243);
            Bgr orange2 = new Bgr(11, 83, 255);

            //Image<Bgr, byte> huefilter = img.InRange(orange1, orange2);

            img.Save("../../TestImage/FilteredImage.png");
            CvInvoke.Blur(hueFilter, hueFilter, new Size(10, 10), new Point());
            img = hueFilter.Convert<Bgr, Byte>();

            LineSegment2D[][] result = hueFilter.HoughLines(0, 0, 1, 1, 1, 10, 1);

            foreach (LineSegment2D line in result[0])
            {
                img.Draw(line, new Bgr(Color.Green), 3);
            }


            //hueFilter.Save("../../TestImage/FilteredImage.png");
            img.Save("../../TestImage/OutputImage.png");

        }
    }
}