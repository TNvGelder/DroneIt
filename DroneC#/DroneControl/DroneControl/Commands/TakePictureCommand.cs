using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DroneControl.Commands {
    class TakePictureCommand : IDroneCommand {
        private DroneController _controller { get; set; }

        public TakePictureCommand(DroneController controller) {
            _controller = controller;
        }

        public void Execute() {
            
        }

        public void Undo() {
            
        }

        public string GetName() {
            return "Picture";
        }

        public double GetValue() {
            return 0;
        }
    }
}
