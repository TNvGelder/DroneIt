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
            //_controller.Forward();
            bool nodeReached = false;
            PositioningState prevState = PositioningState.Correct;
            while (!nodeReached)
            {
                //getbitmap
                Bitmap bmp = new Bitmap("");
                PositioningState state = LineProcessor.ProcessLine(bmp);
                if (state != prevState)
                {
                    switch (state)
                    {
                        case PositioningState.Correct:
                            //_controller.Forward();
                            break;
                        case PositioningState.Lost:
                            _controller.Land();
                            break;
                        case PositioningState.Left:
                            //_controller.Right()
                            break;
                        case PositioningState.Right:
                            //_controller.Left()
                            break;
                    }
                }
                
                
            }
            
    }

        public void Undo()
        {
            _controller.Backward((float)_meters);
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
