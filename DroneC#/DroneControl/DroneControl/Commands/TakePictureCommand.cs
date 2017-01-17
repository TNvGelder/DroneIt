using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

/**
 * @author: Gerhard Kroes
 * */
namespace DroneControl.Commands {
    /// <summary>
    /// Take picture command moves and saves images
    /// </summary>
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

        /// <summary>
        /// Make 5 images and moves in a square
        /// </summary>
        public void Execute() {
            ApiConnection.Instance.UpdateQualityCheck("Taking pictures");
            Console.WriteLine("Cheese <:-)");

            if (!Directory.Exists(_websitePath + _subDestPath))
                Directory.CreateDirectory(_websitePath + _subDestPath);

            // Center image
            _bitmap = _controller.GetBitmapFromFrontCam();
            _bitmap.Save(_websitePath + _subDestPath + "0.png");
            Sound.Instance.Lazer();

            // Bottom left image
            _controller.Left(0.25);
            _controller.Hover();
            _controller.Fall(0.25);
            _controller.Hover();
            _bitmap = _controller.GetBitmapFromFrontCam();
            _bitmap.Save(_websitePath + _subDestPath + "1.png");
            Sound.Instance.Lazer();

            // Top left image
            _controller.Rise(0.5);
            _controller.Hover();
            _bitmap = _controller.GetBitmapFromFrontCam();
            _bitmap.Save(_websitePath + _subDestPath + "2.png");
            Sound.Instance.Lazer();

            // Top right image
            _controller.Right(0.5);
            _controller.Hover();
            _bitmap = _controller.GetBitmapFromFrontCam();
            _bitmap.Save(_websitePath + _subDestPath + "3.png");
            Sound.Instance.Lazer();

            // Bottom right image
            _controller.Fall(0.5);
            _controller.Hover();
            _bitmap = _controller.GetBitmapFromFrontCam();
            _bitmap.Save(_websitePath + _subDestPath + "4.png");
            Sound.Instance.Lazer();

            // Go to center
            _controller.Left(0.25);
            _controller.Hover();
            _controller.Rise(0.25);
            _controller.Hover();

            // Send the image path to the api
            ApiConnection.Instance.UpdateQualityCheckImagePath(_id, _subDestPath);
        }

        /// <summary>
        /// Removes the folders and images
        /// </summary>
        public void Undo() {
            if (!Directory.Exists(_websitePath + _subDestPath))
                Directory.Delete(_websitePath + _subDestPath);
        }
    }
}
