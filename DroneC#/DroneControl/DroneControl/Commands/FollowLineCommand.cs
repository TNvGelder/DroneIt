using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DroneControl.Models;
using DroneControl.Services;
using Emgu.CV;
using LineTrackingTest.Models;
using LineTrackingTest.Services;

namespace DroneControl.Commands
{

    /// <summary>
    /// @author: Twan van Gelder
    /// Command for navigating over a line to an endpoint.
    /// </summary>
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
            ApiConnection.Instance.UpdateQualityCheck("Follow line");

            followLine(true);
        }

        public void Undo()
        {
            followLine(false);
        }

        /// <summary>
        /// Converts a positioningstate to a corresponding direction
        /// </summary>
        /// <param name="state"></param>
        /// <param name="isForward"></param>
        /// <returns></returns>
        private FlyDirection getDirection(PositioningState state, bool isForward)
        {
            if (state == PositioningState.Left)
            {
                return FlyDirection.Right;
            }
            else if (state == PositioningState.Right)
            {
                return FlyDirection.Left;
            }
            else
            {
                return FlyDirection.None;
            }
        }

        /// <summary>
        /// Handles the controls when the drone is lost.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="isForward"></param>
        /// <returns></returns>
        private bool onLost(PositioningState state, bool isForward)
        {
            FlyDirection direction = getDirection(state, isForward);
            return !LineNavigator.Instance.FindLine(_controller, 2, direction); //Try to find back the line 
        }

        /// <summary>
        /// Goes forward over the line until an endpoint. It will try to correct the position if it loses position.
        /// </summary>
        /// <param name="isForward"></param>
        private void followLine(bool isForward)
        {
            float oldSpeed = _controller.Speed;
            PositioningState prevState = PositioningState.Init;
            bool landed = false;
            Bitmap bmp = _controller.GetBitmapFromBottomCam();
            while (!CircleProcessor.Instance.IsCircleInCenter(bmp) && !landed)//Go forward and correct until endpoint is found
            {
                PositioningState state = LineProcessor.Instance.ProcessLine(bmp);
                
                System.Threading.Thread.Sleep(10);
                if (state != prevState)
                {
                    Console.WriteLine(state);
                    if ( state == PositioningState.Correct) { 
                            if (isForward)
                            {
                                _controller.Forward();
                            }
                            else
                            {
                                _controller.Backward();
                            }
                    }
                    else if (state == PositioningState.Lost)
                    {
                        landed = onLost(prevState, isForward);
                    }
                    else
                    {//Correct drone position
                        _controller.Speed = 0.1F/6;// Make the drone fly slower when it corrects itself
                        //so that it will not go too far
                        if (state == PositioningState.Left)
                        {
                            _controller.Right();
                        }
                        else if (state == PositioningState.Right)
                        {
                            _controller.Left();
                        }
                        _controller.Speed = oldSpeed;
                    }
                   
                    prevState = state;

                }

                bmp = _controller.GetBitmapFromBottomCam();
            }
            _controller.Hover();
            
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
