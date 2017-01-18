using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DroneControl.Models;
using DroneControl.PositioningHandlers;
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
    public class FollowLineCommand : IDroneCommand
    {
        private DroneController _controller;
        private double _meters;
        private PositioningHandler _positioningHandler;

        public FollowLineCommand(DroneController controller)
        {
            _controller = controller;
            PositioningHandler correct = new FollowCorrect(controller);
            PositioningHandler lost = new LostHandler(controller);
            PositioningHandler side = new SideCorrectionHandler(controller);
            correct.Successor = lost;
            lost.Successor = side;
            _positioningHandler = correct;
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
        /// Goes forward over the line until an endpoint. It will try to correct the position if it loses position.
        /// </summary>
        /// <param name="isForward"></param>
        private void followLine(bool isForward)
        {
            PositioningState prevState = PositioningState.Init;
            bool isFlying =true;
            Bitmap bmp = _controller.GetBitmapFromBottomCam();
            while (!CircleProcessor.Instance.IsCircleInCenter(bmp) && isFlying)//Go forward and correct until endpoint is found
            {
                PositioningState state = LineProcessor.Instance.ProcessLine(bmp);
                if (state != prevState)
                {
                    prevState = state;
                    isFlying = _positioningHandler.HandlePositioningChange(state, prevState, isForward);
                }
                System.Threading.Thread.Sleep(10);
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
