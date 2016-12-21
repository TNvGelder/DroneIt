using AR.Drone.Avionics;
using AR.Drone.Avionics.Objectives;
using AR.Drone.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using LineTrackingTest.Services;
using AR.Drone.Infrastructure;
using System.Windows.Forms;
using System.Drawing;
using DroneControl.Commands;

namespace DroneControl {
    class Program {
        static void Main(string[] args) {
            //Bitmap myBmp = new Bitmap(Bitmap.FromFile("../../TestImage/BottomCamPhotoTest.png"));
            //Bitmap myBmp = new Bitmap(Bitmap.FromFile("../../TestImage/LineLeftTest.png"));
            //Bitmap myBmp = new Bitmap(Bitmap.FromFile("../../TestImage/YellowColorTest.jpg"));
            Bitmap myBmp = new Bitmap(Bitmap.FromFile("../../TestImage/MultiColorTest.png"));
            LineProcessor.ProcessLine(new Bitmap(myBmp));

            switch (Environment.OSVersion.Platform) {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                    string ffmpegPath = string.Format(@"../../../FFmpeg.AutoGen/FFmpeg/bin/windows/{0}", Environment.Is64BitProcess ? "x64" : "x86");
                    InteropHelper.RegisterLibrariesSearchPath(ffmpegPath);
                    break;
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    string libraryPath = Environment.GetEnvironmentVariable(InteropHelper.LD_LIBRARY_PATH);
                    InteropHelper.RegisterLibrariesSearchPath(libraryPath);
                    break;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Connection sockect for incoming commands
            //ConnectionSocket cs = ConnectionSocket.Instance;
            //cs.Start();

            DroneController dc = DroneController.Instance;
            dc.Start();
            //dc.Takeoff();
            //FollowLineCommand cmd = new FollowLineCommand(dc);
            //cmd.Execute();

            //DroneController dc = DroneController.Instance;
            //dc.Start();
            Bitmap bm0 = dc.GetBitmapFromBottomCam();
            bm0.Save("0bottom.png");
            Bitmap bm1 = dc.GetBitmapFromFrontCam();
            bm1.Save("0front.png");

            Console.Read();
        }
    }
}
