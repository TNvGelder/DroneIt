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
        private string _subDestPath;
        private string _websitePath;

        public TakePictureCommand(DroneController controller, int id) {
            _controller = controller;
            _id = id;
            _subDestPath = "images/" + _id + "/";
            _websitePath = "../../../../../Website/";
        }

        public void Execute() {
            ApiConnection.Instance.UpdateQualityCheck("Taking pictures");
            Console.WriteLine("Cheese <:-)");

            _controller.Start();

            if (!Directory.Exists(_websitePath + _subDestPath))
                Directory.CreateDirectory(_websitePath + _subDestPath);

            // Center image
            _bitmap = _controller.GetBitmapFromFrontCam();
            _bitmap.Save(_websitePath + _subDestPath + "0.png");

            // Bottom left image
            //_controller.Left(0.25);
            //_controller.Hover();
            //_controller.Fall(0.25);
            //_controller.Hover();
            _bitmap = _controller.GetBitmapFromFrontCam();
            _bitmap.Save(_websitePath + _subDestPath + "1.png");

            // Top left image
            //_controller.Rise(0.5);
            //_controller.Hover();
            _bitmap = _controller.GetBitmapFromFrontCam();
            _bitmap.Save(_websitePath + _subDestPath + "2.png");

            // Top right image
            //_controller.Right(0.5);
            //_controller.Hover();
            _bitmap = _controller.GetBitmapFromFrontCam();
            _bitmap.Save(_websitePath + _subDestPath + "3.png");

            // Bottom right image
            //_controller.Fall(0.5);
            //_controller.Hover();
            _bitmap = _controller.GetBitmapFromFrontCam();
            _bitmap.Save(_websitePath + _subDestPath + "4.png");

            // Go to center
            //_controller.Left(0.25);
            //_controller.Hover();
            //_controller.Rise(0.25);
            //_controller.Hover();

            ApiConnection.Instance.UpdateQualityCheckImagePath(_id, _subDestPath);
        }

        public void Undo() {
            if (!Directory.Exists(_websitePath + _subDestPath))
                Directory.Delete(_websitePath + _subDestPath);
        }
    }
}
