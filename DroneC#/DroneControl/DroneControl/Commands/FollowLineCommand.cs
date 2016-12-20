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
            while (lineEndReached)
            {
                //getbitmap
                Bitmap bmp = _controller.GetBitmapFromBottomCam();
                PositioningState state = LineProcessor.ProcessLine(bmp);
                int startPointOfView = _controller.PointOfView;
                if (state != prevState)
                {
                    switch (state)
                    {
                        case PositioningState.Correct:
                            if (isForward)
                            {
                                _controller.Forward();
                            }
                            else
                            {
                                _controller.Backward();
                            }
                            break;
                        case PositioningState.Lost:
                            lineEndReached = true;
                            _controller.Land();
                            break;
                        case PositioningState.Left:
                            _controller.Turn(startPointOfView);
                            _controller.Right();
                            break;
                        case PositioningState.Right:
                            _controller.Turn(startPointOfView);
                            _controller.Left();
                            break;
                    }


                }

            }
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
