﻿using System;
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
    /// 
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

        //private void 

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

        private bool onLost(PositioningState state, bool isForward)
        {
            FlyDirection direction = getDirection(state, isForward);
            return !LineNavigator.Instance.FindLine(_controller, 2, direction); //Try to find back the line
           
        }

        private void followLine(bool isForward)
        {
            float oldSpeed = _controller.Speed;
            PositioningState prevState = PositioningState.Init;
            int startPointOfView = _controller.PointOfView;
            bool landed = false;
            Bitmap bmp = _controller.GetBitmapFromBottomCam();
            while (!CircleProcessor.Instance.IsCircleInCenter(bmp) && !landed)
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
                            Console.WriteLine("Forward");
                    }
                    else if (state == PositioningState.Lost)
                    {
                        Sound.Instance.R2D2a();
                        landed = onLost(prevState, isForward);
                        
                    }
                    else
                    {
                        //Sound.Instance.R2D2e();
                        _controller.Speed = 0.1F/6;
                        if (state == PositioningState.Left)
                        {
                            Console.WriteLine("'To the right");
                            _controller.Right();
                        }
                        else if (state == PositioningState.Right)
                        {
                            Console.WriteLine("To the left");
                            _controller.Left();
                        }
                        _controller.Speed = oldSpeed;
                    }
                   
                    prevState = state;

                }

                bmp = _controller.GetBitmapFromBottomCam();
            }
            _controller.Hover();
            
            
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
