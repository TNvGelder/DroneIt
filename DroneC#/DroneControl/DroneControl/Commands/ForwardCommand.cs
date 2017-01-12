﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Commands
{
    public class ForwardCommand : IDroneCommand
    {
        private DroneController _controller { get; set; }
        private double _meters { get; set; }

        public ForwardCommand(DroneController controller, double meters) {
            _controller = controller;
            _meters = meters;
        }

        public void Execute()
        {
            ApiConnection.Instance.UpdateQualityCheck("Go forward " + _meters + " meters");
            Sound.Instance.R2D2e();
            _controller.Forward((float)_meters);
            _controller.Hover();
        }

        public void Undo()
        {
            _controller.Backward((float)_meters);
            _controller.Hover();
        }
    }
}