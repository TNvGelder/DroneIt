using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Processors.DroneProcessors.Commands {
    public class TakePictureCommand : IDroneCommand {
        private DroneProcessor _processor { get; set; }

        public TakePictureCommand(DroneProcessor processor) {
            _processor = processor;
        }

        public void Execute() {
            
        }

        public void Undo() {
            
        }

        public string GetName() {
            return "TakePicture";
        }

        public double GetValue() {
            return 0;
        }
    }
}