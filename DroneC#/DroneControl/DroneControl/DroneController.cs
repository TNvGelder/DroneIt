using AR.Drone.Client;
using AR.Drone.Client.Command;
using AR.Drone.Video;
using System;
using AR.Drone.Data;
using AR.Drone.Data.Navigation;
using System.Drawing;
using AR.Drone.Media;
using AR.Drone.WinApp;
using System.IO;
using System.Threading;
using AR.Drone.Client.Configuration;

namespace DroneControl {
    enum DroneCamera { Front, Bottom }

    public class DroneController
    {
        private static volatile DroneController _instance;
        private static object syncRoot = new Object();
        public readonly float Speed = 0.1F;
        private DroneClient _droneClient;
        private NavigationPacket _navigationPacket;
        private readonly VideoPacketDecoderWorker _videoPacketDecoderWorker;
        private VideoFrame _frame;
        private Bitmap _frameBitmap;
        public uint FrameNumber { get; private set; }
        private Thread _th;
        public NavigationData _navigationData { get; private set; }
        public int North { get; private set; }
        public static DroneController Instance
        {
            get
            {
                if (_instance == null) {
                    lock (syncRoot) {
                        if (_instance == null)
                            _instance = new DroneController();
                    }
                }
                return _instance;
            }
        }
        public bool isBusy
        {
            get
            {
                return _droneClient.NavigationData.State != NavigationState.Hovering;
            }
        }
        public int PointOfView
        {
            get
            {
                return degreesConverter(Convert.ToInt16(_navigationData.Degrees));
            }
        }

        private DroneController()
        {
            _videoPacketDecoderWorker = new VideoPacketDecoderWorker(PixelFormat.BGR24, true, OnVideoPacketDecoded);
            _videoPacketDecoderWorker.Start();

            _droneClient = new DroneClient("192.168.1.1");

            _droneClient.NavigationPacketAcquired += OnNavigationPacketAcquired;
            _droneClient.VideoPacketAcquired += OnVideoPacketAcquired;
            _droneClient.NavigationDataAcquired += data => _navigationData = data;

            _th = new Thread(takePicture);
            _th.Start();
        }

        private void takePicture() {
            while (true) {
                this.SaveImage();
                System.Threading.Thread.Sleep(100);
            }
        }

        /// <summary>
        /// This method will start the drone
        /// </summary>
        /// <param name="time"></param>
        public void Start(int time = 1000) {
            Console.WriteLine("Start");
            _droneClient.Start();
            System.Threading.Thread.Sleep(time);
        }

        /// <summary>
        /// Let the drone fly for a give amount of meters. 
        /// The speed if from a constant value
        /// </summary>
        /// <param name="meters"></param>
        public void Forward(float meters)
        {
            Console.WriteLine("For" + meters);
            float Time = meters / this.Speed * 200;
            Console.WriteLine(Time);
            _droneClient.Progress(FlightMode.Progressive, pitch: -this.Speed);
            System.Threading.Thread.Sleep((int)Time);
        }

        public void Backward(float meters) {
            Console.WriteLine("Back");
            float Time = meters / this.Speed * 200;
            _droneClient.Progress(FlightMode.Progressive, pitch: this.Speed);
            System.Threading.Thread.Sleep(Convert.ToInt16(Time));
        }

        public void Left(float meters) {
            Console.WriteLine("Left");
            float Time = meters / this.Speed * 200;
            _droneClient.Progress(FlightMode.Progressive, roll: this.Speed);
            System.Threading.Thread.Sleep(Convert.ToInt16(Time));
        }

        public void Right(float meters) {
            Console.WriteLine("Left");
            float Time = meters / this.Speed * 200;
            _droneClient.Progress(FlightMode.Progressive, roll: -this.Speed);
            System.Threading.Thread.Sleep(Convert.ToInt16(Time));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        public void FlatTrim(int time)
        {
            _droneClient.FlatTrim();
            System.Threading.Thread.Sleep(time);
        }

        public void Turn(int degrees)
        {
            Console.WriteLine("Turn " + degrees);
            int TurnTo = (Convert.ToInt16(degrees) + Convert.ToInt16((this.North)));
            if (TurnTo >= 360) {
                TurnTo -= 360;
            }

            while (true)
            {
                int CurrentDegrees = degreesConverter(Convert.ToInt16(_navigationData.Degrees));
                Console.WriteLine(" ------------- ");
                Console.WriteLine(" ------------- ");
                Console.WriteLine(" ------------- ");
                Console.WriteLine("North: " + North.ToString());
                Console.WriteLine("Current: " + CurrentDegrees.ToString());
                Console.WriteLine("Turn To: " + TurnTo.ToString());

                int DistanceRight = 0;
                int DistanceLeft = 0;

                if (TurnTo > CurrentDegrees) {
                    DistanceRight = TurnTo - CurrentDegrees;
                    DistanceLeft = (360 - TurnTo) + CurrentDegrees;
                } else if (CurrentDegrees > TurnTo) {
                    DistanceRight = (360 - CurrentDegrees) + TurnTo;
                    DistanceLeft = CurrentDegrees - TurnTo;
                }

                if (DistanceLeft < DistanceRight) {
                    _droneClient.Progress(FlightMode.Progressive, yaw: -0.1f);
                    Console.WriteLine("Go Left");
                } else {
                    _droneClient.Progress(FlightMode.Progressive, yaw: 0.1f);
                    Console.WriteLine("Go Right");
                }
                if (CurrentDegrees == TurnTo) {
                    Console.WriteLine("FINISH");
                    break;
                }
            }
        }

        /// <summary>--
        /// This method will takeoff the drone. 
        /// The parameter time is the Threading time for wait.
        /// </summary>
        /// <param name="time"></param>
        public void Takeoff(int time = 15000)
        {
            Console.WriteLine("Takeoff");
            _droneClient.Takeoff();
            System.Threading.Thread.Sleep(time);
            North = Convert.ToInt16(_navigationData.Degrees);
            if (North < 0) {
                North = (Convert.ToInt16(North) + 360);
            }
        }

        /// <summary>
        /// This method will stop the drone
        /// The given time is the threading wait time
        /// </summary>
        /// <param name="time"></param>
        public void Stop(int time)
        {
            Console.WriteLine("Stop");
            _droneClient.Stop();
            System.Threading.Thread.Sleep(time);
        }

        /// <summary>
        /// This method will let the drone land. 
        /// The given time is the threading wait time
        /// </summary>
        /// <param name="time"></param>
        public void Land(int time = 3000)
        {
            Console.WriteLine("Land");
            _droneClient.Land();
            System.Threading.Thread.Sleep(time);
        }

        /// <summary>
        /// This method will the drone let go up. 
        /// The speed is the speed of the propellors and the time is the duration of the action.
        /// </summary>
        /// <param name="meters"></param>
        public void Rise(float meters)
        {
            Console.WriteLine("Rise");
            int Time = Convert.ToInt16((meters / Speed) * 200);
            _droneClient.Progress(FlightMode.Progressive, gaz: this.Speed);
            System.Threading.Thread.Sleep(Time);
        }

        /// <summary>
        /// This method let the drone go down.
        /// The speed is het speed of the propellors and the time is the duration of the action.
        /// </summary>
        /// <param name="meters"></param>
        public void Fall(float meters)
        {
            int Time = Convert.ToInt16((meters / Speed) * 200);
            _droneClient.Progress(FlightMode.Progressive, gaz: -Speed);
            System.Threading.Thread.Sleep(Time);
        }

        /// <summary>
        /// This method let the drone calibrate. 
        /// Must be happen every flight for an accurate flight.
        /// </summary>
        public void Calibrate()
        {
            _droneClient.Send(CalibrateCommand.Magnetometer);
        }

        /// <summary>
        /// This method let hover the drone.
        /// </summary>
        /// <param name="Time"></param>
        public void Hover(int Time = 5000)
        {
            Console.WriteLine("Hover");
            _droneClient.Progress(FlightMode.Hover);
            System.Threading.Thread.Sleep(Time);
        }

        /// <summary>
        /// This method will handle every packet that's coming from the drone.
        /// </summary>
        /// <param name="packet"></param>
        private void OnNavigationPacketAcquired(NavigationPacket packet)
        {
            _navigationPacket = packet;

            if (_navigationData != null)
            {
                if (!File.Exists("log.txt"))
                {
                    File.Create("log.txt");
                    Console.WriteLine("No file");
                }
                else
                {
                    File.AppendAllText("log.txt", _navigationData.ToString());
                }
            }
            else
            {
                Console.WriteLine("No navigation data");
            }
        }

        ////////////////////////////////////////////////////
        ////////////////////////////////////////////////////
        ////////////////////// VIDEO ///////////////////////
        ////////////////////////////////////////////////////
        ////////////////////////////////////////////////////
        private unsafe void OnVideoPacketAcquired(VideoPacket packet)
        {
            /*if (_packetRecorderWorker != null && _packetRecorderWorker.IsAlive)
                _packetRecorderWorker.EnqueuePacket(packet);*/
            if (_videoPacketDecoderWorker.IsAlive)
                _videoPacketDecoderWorker.EnqueuePacket(packet);
        }

        /// <summary>
        /// This method will update the current frame by the given VideoFrame
        /// </summary>
        /// <param name="frame"></param>
        private void OnVideoPacketDecoded(VideoFrame frame)
        {
            _frame = frame;
            SaveImage();
        }

        /// <summary>
        /// Saves the image from the video decoder
        /// </summary>
        public void SaveImage()
        {
            if (_frame == null || FrameNumber == _frame.Number)
                return;
            FrameNumber = _frame.Number;

            if (_frameBitmap == null)
                _frameBitmap = VideoHelper.CreateBitmap(ref _frame);
            else
                VideoHelper.UpdateBitmap(ref _frameBitmap, ref _frame);

            string subPath = "Data/";
            if (!Directory.Exists(subPath))
                System.IO.Directory.CreateDirectory(subPath);

            _frameBitmap.Save(subPath + FrameNumber + ".png");

            string livePath = "Live/";
            if (!Directory.Exists(livePath))
                System.IO.Directory.CreateDirectory(livePath);

            _frameBitmap.Save(subPath + livePath + "live.png");
        }

        private int degreesConverter(int degrees) {
            if (degrees < 0) {
                degrees = (degrees + 360);
            }
            return degrees;
        }

        private void switchCamera() {
            var configuration = new Settings();
            configuration.Video.Channel = VideoChannelType.Next;
            _droneClient.Send(configuration);
        }

        private void checkCameraTo(DroneCamera camera) {
            if (camera == DroneCamera.Front && _frameBitmap.Width != 1280) {
                switchCamera();
            } else if (camera == DroneCamera.Bottom && _frameBitmap.Width != 640) {
                switchCamera();
            }
        }

        public Bitmap getBitmapFromBottomCam() {
            checkCameraTo(DroneCamera.Bottom);

            int frameNumber = (int)FrameNumber + 5;
            Bitmap bm;

            while (true) {
                try {
                    bm = new Bitmap("Data/" + frameNumber + ".png");
                    break;
                } catch (IOException) { }
            }
            return bm;
        }

        public Bitmap getBitmapFromFrontCam() {
            checkCameraTo(DroneCamera.Front);

            int frameNumber = (int)FrameNumber + 5;
            Bitmap bm;

            while (true) {
                try {
                    bm = new Bitmap("Data/" + frameNumber + ".png");
                    break;
                } catch (IOException) { }
            }
            return bm;
        }
    }
}
