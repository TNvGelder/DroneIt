using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Processors.DroneProcessors.Commands {
    public class TakePictureCommand : IDroneCommand {
        private DroneProcessor _processor { get; set; }
        private int _id { get; set; }

        public TakePictureCommand(DroneProcessor processor, int id) {
            _processor = processor;
            _id = id;
        }

        public void Execute() {
            
        }

        public void Undo() {
            
        }

        public string GetName() {
            return "TakePicture";
        }

        public double GetValue() {
            return _id;
        }
    }
}