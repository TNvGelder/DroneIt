using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace DroneControl.Commands {
    class TakePictureCommand : IDroneCommand {
        private DroneController _controller { get; set; }
        private int _id { get; set; }
        private Bitmap _bitmap;
        private int _frameNumber;
        private string _subDestPath;

        public TakePictureCommand(DroneController controller, int id) {
            _controller = controller;
            _id = id;
            _subDestPath = "Images/" + _id + "/";
        }

        public void Execute() {
            ApiConnection.Instance.UpdateQualityCheck("Taking pictures");

            _controller.Start();
            _bitmap = _controller.GetBitmapFromFrontCam();
            _frameNumber = (int)_controller.FrameNumber;
            
            if (!Directory.Exists(_subDestPath))
                Directory.CreateDirectory(_subDestPath);

            _bitmap.Save(_subDestPath + _frameNumber + ".png");

            ApiConnection.Instance.UpdateQualityCheck(_id, "DroneC#/DroneControl/DroneControl/bin/Debug/Data/" + _subDestPath);
        }

        public void Undo() {
            if (!File.Exists(_subDestPath + _frameNumber + ".png"))
                File.Delete(_subDestPath + _frameNumber + ".png");
        }
    }
}
