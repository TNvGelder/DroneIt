using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Emgu.CV;
using LineTrackingTest.Models;
using LineTrackingTest.Services;

namespace DroneControl.Commands
{
    class FollowLineCommand : IDroneCommand
    {
        private DroneController _controller { get; set; }
        private double _meters { get; set; }

        public FollowLineCommand(DroneController controller)
        {
            _controller = controller;
        }

        public void Execute()
        {
            followLine(true);
            
        }

        public void Undo()
        {
            followLine(false);
        }

        //private void 

        private void followLine(bool isForward)
        {
            //_controller.Forward();
            bool lineEndReached = false;
            PositioningState prevState = PositioningState.Correct;
            Console.WriteLine("StartCamDetect");
            while (!lineEndReached)
            {
                //getbitmap
                Bitmap bmp = _controller.GetBitmapFromBottomCam();
                PositioningState state = LineProcessor.ProcessLine(bmp);
                int startPointOfView = _controller.PointOfView;
                Console.WriteLine(state);
                if (state != prevState)
                {
                    switch (state)
                    {
                        case PositioningState.Correct:
                            //if (isForward)
                            //{
                            //    _controller.Forward();
                            //}
                            //else
                            //{
                            //    _controller.Backward();
                            //}
                            Console.WriteLine("Forward");
                            break;
                        case PositioningState.Lost:
                            lineEndReached = true;
                            Console.WriteLine("Land");
                            //_controller.Land();
                            break;
                        case PositioningState.Left:
                            Console.WriteLine("Turn + left");
                            //_controller.Turn(startPointOfView);
                            //_controller.Right();
                            break;
                        case PositioningState.Right:
                            Console.WriteLine("Turn + right");
                            //_controller.Turn(startPointOfView);
                            //_controller.Left();
                            break;
                    }


                }
                

            }
            Console.WriteLine("Done");
        }

        public string GetName()
        {
            return "Forward";
        }

        public double GetValue()
        {
            return _meters;
        }
    }
}
