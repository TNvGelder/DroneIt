using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace DroneControl.Commands {
    class TakePictureCommand : IDroneCommand {
        private DroneController _controller { get; set; }
        private int _frameNumber;
        private string _subSourcePath;
        private string _subDestPath;

        public TakePictureCommand(DroneController controller) {
            _controller = controller;
            _subSourcePath = "Data/";
            _subDestPath = "Images/";
        }

        public void Execute() {
            _controller.Start();
            _frameNumber = (int)_controller.FrameNumber;
            
            if (!Directory.Exists(_subDestPath))
                Directory.CreateDirectory(_subDestPath);

            if (!File.Exists(_subSourcePath + _frameNumber + ".png"))
                File.Copy(_subSourcePath + _frameNumber + ".png", _subDestPath + _frameNumber + ".png");
        }

        public void Undo() {
            if (!File.Exists(_subDestPath + _frameNumber + ".png"))
                File.Delete(_subDestPath + _frameNumber + ".png");
        }
    }
}
