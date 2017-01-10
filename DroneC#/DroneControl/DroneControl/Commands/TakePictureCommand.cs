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
            _bitmap = _controller.GetBitmapFromFrontCam();
            
            if (!Directory.Exists(_websitePath + _subDestPath))
                Directory.CreateDirectory(_websitePath + _subDestPath);

            _bitmap.Save(_websitePath + _subDestPath + "0.png");

            ApiConnection.Instance.UpdateQualityCheck(_id, _subDestPath);
        }

        public void Undo() {
            if (!File.Exists(_websitePath + _subDestPath + "0.png"))
                File.Delete(_websitePath + _subDestPath + "0.png");
        }
    }
}
