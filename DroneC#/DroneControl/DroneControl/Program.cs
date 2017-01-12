using System;
using AR.Drone.Infrastructure;
using System.Windows.Forms;

namespace DroneControl {
    class Program {
        static void Main(string[] args) {
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
            ConnectionSocket cs = ConnectionSocket.Instance;
            cs.Start();

            //DroneController dc = DroneController.Instance;
            //dc.Start();
            //System.Threading.Thread.Sleep(1000);
            /*dc.Calibrate();
            System.Threading.Thread.Sleep(1000);
            dc.Takeoff();
            System.Threading.Thread.Sleep(1000);
            dc.Turn(45);
            System.Threading.Thread.Sleep(1000);
            dc.Land();*/
            //StartCommand start = new StartCommand(dc);
            //FollowLineCommand cmd = new FollowLineCommand(dc);
            //LandCommand land = new LandCommand(dc);
            //start.Execute();
            //land.Execute();

            //cmd.Execute();

            //dc.Stop(10);
            //DroneController dc = DroneController.Instance;
            //dc.Start();
            //Bitmap bm0 = dc.GetBitmapFromBottomCam();
            //bm0.Save("0bottom.png");
            //Bitmap bm1 = dc.GetBitmapFromFrontCam();
            //bm1.Save("0front.png");
        }
    }
}
