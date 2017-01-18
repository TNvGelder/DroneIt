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
using System.Windows.Forms;
using AR.Drone.Client.Configuration;
using DroneControl.Models;

/**
 * @author: Gerhard Kroes and Henk-jan Leusink
 * */
namespace DroneControl {
    /// <summary>
    /// Enum for the camera's in the drone
    /// </summary>
    enum DroneCamera { Front, Bottom }

    /// <summary>
    /// Controller that controls the drone
    /// </summary>
    public class DroneController
    {
        private static volatile DroneController _instance;
        private static object syncRoot = new Object();
        private float _speed = 0.05f;
        private DroneClient _droneClient;
        private NavigationPacket _navigationPacket;
        private readonly VideoPacketDecoderWorker _videoPacketDecoderWorker;
        private VideoFrame _frame;
        private int _frameNumber;
        private string _frameTag;
        private Bitmap _frameBitmap;
        private Thread _th;
        public string DataPath { get; private set; }
        public string LivePath { get; private set; }
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        private DroneCamera _camera { get; set; }
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

        /// <summary>
        /// Connects with the drone
        /// </summary>
        private DroneController()
        {
            // Sets paths
            DataPath = "Data/";
            LivePath = DataPath + "Live/";

            // Starts the video stream
            _videoPacketDecoderWorker = new VideoPacketDecoderWorker(PixelFormat.BGR24, true, OnVideoPacketDecoded);
            _videoPacketDecoderWorker.Start();

            // Connects with the drone
            _droneClient = new DroneClient("192.168.1.1");
            
            _frameNumber = 0;
            DateTime dt = DateTime.Now;
            _frameTag = dt.Year.ToString() + dt.Month.ToString("00") + dt.Day.ToString("00") + dt.Hour.ToString("00") + dt.Minute.ToString("00") + "_";

            // Enable navigation data
            _droneClient.NavigationPacketAcquired += OnNavigationPacketAcquired;
            _droneClient.VideoPacketAcquired += OnVideoPacketAcquired;
            _droneClient.NavigationDataAcquired += data => _navigationData = data;
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
        /// Lets the drone fly forward
        /// </summary>
        public void Forward()
        {
            _droneClient.Progress(FlightMode.Progressive, pitch: -this.Speed);
        }

        /// <summary>
        /// Lets the drone fly forward and waits until it has flown the given amount of meters
        /// The speed if from a constant value
        /// </summary>
        /// <param name="meters"></param>
        public void Forward(double meters)
        {
            Console.WriteLine("For" + meters);
            double Time = meters / this.Speed * 200;
            Console.WriteLine(Time);
            Forward();
            System.Threading.Thread.Sleep((int)Time);
        }

        
        /// <summary>
        /// Lets the drone fly backwards.
        /// </summary>
        public void Backward()
        {
            _droneClient.Progress(FlightMode.Progressive, pitch: this.Speed);
        }

        /// <summary>
        /// Lets the drone fly backwards and waits until it has flown the given amount of meters
        /// The speed if from a constant value 
        /// </summary>
        /// <param name="meters"></param>
        public void Backward(double meters) {
            Console.WriteLine("Back");
            double Time = meters / this.Speed * 200;
            Backward();
            System.Threading.Thread.Sleep(Convert.ToInt16(Time));
        }

        /// <summary>
        /// Lets the drone fly to the left.
        /// </summary>
        public void Left()
        {
            _droneClient.Progress(FlightMode.Progressive, roll: -this.Speed);
        }

        /// <summary>
        /// Lets the drone fly to the left and waits until it has flown the given amount of meters
        /// The speed if from a constant value 
        /// </summary>
        /// <param name="meters"></param>
        public void Left(double meters) {
            Console.WriteLine("Left");

            double Time = meters / this.Speed * 200;
            Left();
            System.Threading.Thread.Sleep(Convert.ToInt16(Time));
        }

        /// <summary>
        /// Lets the drone fly to the right.
        /// </summary>
        public void Right()
        {
            _droneClient.Progress(FlightMode.Progressive, roll: this.Speed);
        }

        /// <summary>
        /// Lets the drone fly to the right and waits until it has flown the given amount of meters
        /// The speed if from a constant value 
        /// </summary>
        /// <param name="meters"></param>
        public void Right(double meters) {
            Console.WriteLine("Left");
            double Time = meters / this.Speed * 200;
            Right();
            System.Threading.Thread.Sleep(Convert.ToInt16(Time));
        }

        /// <summary>
        /// Turn to degrees
        /// </summary>
        /// <param name="turnTo"></param>
        public void Turn(int turnTo)
        {
            int CurrentDegrees = degreesConverter(Convert.ToInt16(_navigationData.Degrees));
            int DistanceRight = 0;
            int DistanceLeft = 0;

            Console.WriteLine("CurrentDegrees " + CurrentDegrees);
            Console.WriteLine("North " + North);
            Console.WriteLine("TurnTo " + turnTo);

            while (true)
            {
                CurrentDegrees = degreesConverter(Convert.ToInt16(_navigationData.Degrees));

                if (turnTo > CurrentDegrees) {
                    DistanceRight = turnTo - CurrentDegrees;
                    DistanceLeft = (360 - turnTo) + CurrentDegrees;
                } else if (CurrentDegrees > turnTo) {
                    DistanceRight = (360 - CurrentDegrees) + turnTo;
                    DistanceLeft = CurrentDegrees - turnTo;
                }

                if (DistanceLeft < DistanceRight) {
                    // Go left
                    _droneClient.Progress(FlightMode.CombinedYaw, yaw: -0.2f);
                } else {
                    // Go right
                    _droneClient.Progress(FlightMode.CombinedYaw, yaw: 0.2f);
                }

                if (CurrentDegrees == turnTo) {
                    // Finish
                    break;
                }
                System.Threading.Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Turn with north degrees
        /// </summary>
        /// <param name="degrees"></param>
        public void TurnToWorldDegrees(int degrees) {
            Console.WriteLine("Turn " + degrees);
            int turnTo = (Convert.ToInt16(degrees) + Convert.ToInt16((this.North)));
            if (turnTo >= 360) {
                turnTo -= 360;
            }
            Turn(turnTo);
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
            setNorth();
        }

        /// <summary>
        /// Set north degrees
        /// </summary>
        private void setNorth() {
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
        public void Rise(double meters)
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
        public void Fall(double meters)
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
        public void Hover(int Time = 3000)
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
            _frameNumber += 1;
            SaveImage();
        }

        /// <summary>
        /// Saves the image from the video decoder
        /// </summary>
        public void SaveImage()
        {
            if (_frame == null)
                return;

            if (_frameBitmap == null)
                _frameBitmap = VideoHelper.CreateBitmap(ref _frame);
            else
                VideoHelper.UpdateBitmap(ref _frameBitmap, ref _frame);
            
            if (!Directory.Exists(DataPath))
                System.IO.Directory.CreateDirectory(DataPath);
            
            _frameBitmap.Save(DataPath + _frameTag + _frameNumber.ToString("000000") + ".png");
            
            if (!Directory.Exists(LivePath))
                System.IO.Directory.CreateDirectory(LivePath);

            _frameBitmap.Save(LivePath + "live.png");
        }

        /// <summary>
        /// Convert the degrees with north degrees
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        private int degreesConverter(int degrees) {
            if (degrees < 0) {
                degrees = (degrees + 360);
            }
            return degrees;
        }

        /// <summary>
        /// Switch camera to
        /// </summary>
        /// <param name="vct"></param>
        private void switchCamera(VideoChannelType vct) {
            var configuration = new Settings();
            configuration.Video.Channel = vct;
            _droneClient.Send(configuration);
            System.Threading.Thread.Sleep(100);
        }

        /// <summary>
        /// Switch camera to
        /// </summary>
        /// <param name="camera"></param>
        private void setCameraTo(DroneCamera camera) {
            if (camera == DroneCamera.Front) {
                switchCamera(VideoChannelType.Horizontal);
            } else {
                switchCamera(VideoChannelType.Vertical);
            }
        }

        /// <summary>
        /// Gets a bitmap from bottom camera
        /// </summary>
        /// <returns>Bitmap</returns>
        public Bitmap GetBitmapFromBottomCam() {
            setCameraTo(DroneCamera.Bottom);

            int frameNumber = (int)_frameNumber + 5;
            Bitmap bm;
            
            while (true) {
                frameNumber = File.Exists("Data/" + _frameTag + frameNumber.ToString("000000") + ".png") ? frameNumber : frameNumber + 1;
                try {
                    bm = new Bitmap("Data/" + _frameTag + frameNumber.ToString("000000") + ".png");
                    break;
                } catch (Exception) {
                    System.Threading.Thread.Sleep(100);
                }
            }
            return bm;
        }

        /// <summary>
        /// Gets a bitmap from front camera
        /// </summary>
        /// <returns>Bitmap</returns>
        public Bitmap GetBitmapFromFrontCam() {
            setCameraTo(DroneCamera.Front);

            int frameNumber = (int)_frameNumber + 5;
            Bitmap bm;

            while (true) {
                try {
                    frameNumber = File.Exists("Data/" + _frameTag + frameNumber.ToString("000000") + ".png") ? frameNumber : frameNumber + 1;
                    bm = new Bitmap("Data/" + _frameTag + frameNumber.ToString("000000") + ".png");
                    break;
                } catch (Exception) {
                    System.Threading.Thread.Sleep(100);
                }
            }
            return bm;
        }
    }
}
