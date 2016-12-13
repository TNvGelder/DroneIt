using AR.Drone.Client;
using AR.Drone.Client.Command;
using AR.Drone.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Drone.Data;
using AR.Drone.Data.Navigation;
using System.Drawing;
using AR.Drone.Media;
using AR.Drone.WinApp;
using System.IO;
using System.Threading;

namespace DroneControl
{
    public class DroneController
    {
        public readonly float Speed = 0.25F;
        private DroneClient _droneClient;
        public bool isBusy
        {
            get
            {
                return _droneClient.NavigationData.State != NavigationState.Hovering;
            }
        }
        
        public NavigationData _navigationData { get; private set; }
        private NavigationPacket _navigationPacket;
        public int North { get; private set; }

        private PacketRecorder _packetRecorderWorker;
        private FileStream _recorderStream;
        private readonly VideoPacketDecoderWorker _videoPacketDecoderWorker;
        private VideoFrame _frame;
        private Bitmap _frameBitmap;
        private uint _frameNumber;

        public DroneController()
        {
            _videoPacketDecoderWorker = new VideoPacketDecoderWorker(PixelFormat.BGR24, true, OnVideoPacketDecoded);
            _videoPacketDecoderWorker.Start();

            _droneClient = new DroneClient("192.168.1.1");

            _droneClient.NavigationPacketAcquired += OnNavigationPacketAcquired;
            _droneClient.VideoPacketAcquired += OnVideoPacketAcquired;
            _droneClient.NavigationDataAcquired += data => _navigationData = data;
            StartRecording();

            Thread th = new Thread(takePicture);
            th.Start();
        }

        private void takePicture() {
            while (true) {
                System.Threading.Thread.Sleep(100);
                this.SaveImage();
                Console.WriteLine("Save Image");
            }
        }

        private void StopRecording()
        {
            if (_packetRecorderWorker != null)
            {
                _packetRecorderWorker.Stop();
                _packetRecorderWorker.Join();
                _packetRecorderWorker = null;
            }

            if (_recorderStream != null)
            {
                _recorderStream.Dispose();
                _recorderStream = null;
            }
        }

        private void StartRecording()
        {
            StopRecording();

            _recorderStream = new FileStream("test.bmp", FileMode.OpenOrCreate);
            _packetRecorderWorker = new PacketRecorder(_recorderStream);
            _packetRecorderWorker.Start();
        }

        /// <summary>
        /// This method will start the drone
        /// </summary>
        /// <param name="time"></param>
        public void Start(int time = 1000)
        {
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
            float Time = meters / this.Speed * 1000;
            _droneClient.Progress(FlightMode.Progressive, pitch: this.Speed);
            System.Threading.Thread.Sleep(Convert.ToInt16(Time));
            this.Hover();
        }

        public void Backward(float meters) {
            float Time = meters / this.Speed * 1000;
            _droneClient.Progress(FlightMode.Progressive, pitch: -this.Speed);
            System.Threading.Thread.Sleep(Convert.ToInt16(Time));
            this.Hover();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        public void FlatTrim(int time)
        {
            _droneClient.FlatTrim();
            System.Threading.Thread.Sleep(time);
            this.Hover();
        }


        public void Turn(int degrees)
        {
            int TurnTo = 0;
            if (North >= 0)
            {
                TurnTo = (Convert.ToInt16(degrees) + Convert.ToInt16((this.North)));
            }
            else if (North < 0)
            {
                TurnTo = (Convert.ToInt16(degrees) - Convert.ToInt16((this.North) * -1));
            }

            if (North > 180)
            {
                TurnTo = (Convert.ToInt16(degrees) - 360);
            }

            while (true)
            {
                int CurrentDegrees = Convert.ToInt16(_navigationData.Degrees);
                Console.WriteLine(" ------------- ");
                Console.WriteLine(" ------------- ");
                Console.WriteLine(" ------------- ");
                Console.WriteLine("North: " + North.ToString());
                Console.WriteLine("Current: " + CurrentDegrees.ToString());
                Console.WriteLine("Turn To: " + TurnTo.ToString());

                if (CurrentDegrees > TurnTo && CurrentDegrees > (TurnTo - 180))
                {
                    _droneClient.Progress(FlightMode.Progressive, roll: -0.05f);
                    Console.WriteLine("Go Left");
                }
                else if (CurrentDegrees < TurnTo && CurrentDegrees < (TurnTo + 180))
                {
                    _droneClient.Progress(FlightMode.Progressive, roll: 0.05f);
                    Console.WriteLine("Go Right");
                }
                else
                {
                    Console.WriteLine("FINISH");

                    break;
                }

                System.Threading.Thread.Sleep(20);
            }
        }

        /// <summary>--
        /// This method will takeoff the drone. 
        /// The parameter time is the Threading time for wait.
        /// </summary>
        /// <param name="time"></param>
        public void Takeoff(int time = 15000)
        {
            _droneClient.Takeoff();
            System.Threading.Thread.Sleep(time);
            North = Convert.ToInt16(_navigationData.Degrees);
            this.Hover();
        }

        /// <summary>
        /// This method will stop the drone
        /// The given time is the threading wait time
        /// </summary>
        /// <param name="time"></param>
        public void Stop(int time)
        {
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
            int Time = Convert.ToInt16((meters / Speed) * 1000);
            _droneClient.Progress(FlightMode.Progressive, gaz: this.Speed);
            System.Threading.Thread.Sleep(Time);
            this.Hover();
        }

        /// <summary>
        /// This method let the drone go down.
        /// The speed is het speed of the propellors and the time is the duration of the action.
        /// </summary>
        /// <param name="meters"></param>
        public void Fall(float meters)
        {
            int Time = Convert.ToInt16((meters / Speed) * 1000);
            _droneClient.Progress(FlightMode.Progressive, gaz: -Speed);
            System.Threading.Thread.Sleep(Time);
            this.Hover();
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
        public void Hover(int Time = 2000)
        {
            _droneClient.Hover();
            System.Threading.Thread.Sleep(Time);
        }

        /// <summary>
        /// This method will handle every packet that's coming from the drone.
        /// </summary>
        /// <param name="packet"></param>
        private void OnNavigationPacketAcquired(NavigationPacket packet)
        {
            if (_packetRecorderWorker != null && _packetRecorderWorker.IsAlive)
                _packetRecorderWorker.EnqueuePacket(packet);

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
                    //Console.WriteLine(_navigationData.Degrees.ToString());
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
        private void OnVideoPacketAcquired(VideoPacket packet)
        {
            if (_packetRecorderWorker != null && _packetRecorderWorker.IsAlive)
                _packetRecorderWorker.EnqueuePacket(packet);
            if (_videoPacketDecoderWorker.IsAlive)
                _videoPacketDecoderWorker.EnqueuePacket(packet);
        }

        /// <summary>
        /// This method will update the current frame by the given VideoFrame
        /// </summary>
        /// <param name="frame"></param>
        private void OnVideoPacketDecoded(VideoFrame frame)
        {
            Console.WriteLine("OnVideoPacketDecoded");
            _frame = frame;
            SaveImage();
        }

        /// <summary>
        /// Saves the image from the video decoder
        /// </summary>
        public void SaveImage()
        {
            if (_frame == null || _frameNumber == _frame.Number)
                return;
            _frameNumber = _frame.Number;

            if (_frameBitmap == null)
                _frameBitmap = VideoHelper.CreateBitmap(ref _frame);
            else
                VideoHelper.UpdateBitmap(ref _frameBitmap, ref _frame);
            Console.WriteLine("Save image");

            string subPath = "Data";
            bool exists = System.IO.Directory.Exists(subPath);
            if (!exists)
                System.IO.Directory.CreateDirectory(subPath);

            _frameBitmap.Save("Data/Test.jpg");
        }
    }
}
