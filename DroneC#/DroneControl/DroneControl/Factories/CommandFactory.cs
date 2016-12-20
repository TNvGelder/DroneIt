using DroneControl.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneControl.Factories
{
    /// <summary>
    /// This Factory is used for returning command object.
    /// </summary>
    public class CommandFactory : ICommandFactory
    {
        private DroneController _droneController { get; set; }

        public CommandFactory(DroneController droneProcessor) {
            _droneController = droneProcessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IDroneCommand MakeCommand(string command, double value = 0) {
            IDroneCommand droneCommand = null;

            if (command.Equals("Start")) {
                droneCommand = new StartCommand(_droneController);
            } else if (command.Equals("Land")) {
                droneCommand = new LandCommand(_droneController);
            } else if (command.Equals("Rise")) {
                droneCommand = new RiseCommand(_droneController, value);
            } else if (command.Equals("Fall")) {
                droneCommand = new FallCommand(_droneController, value);
            } else if (command.Equals("Right")) {
                droneCommand = new RightCommand(_droneController, value);
            } else if (command.Equals("Left")) {
                droneCommand = new LeftCommand(_droneController, value);
            } else if (command.Equals("Forward")) {
                droneCommand = new ForwardCommand(_droneController, value);
            } else if (command.Equals("Backward")) {
                droneCommand = new BackwardCommand(_droneController, value);
            } else if (command.Equals("Turn")) {
                droneCommand = new TurnCommand(_droneController, (int)value);
            } else if (command.Equals("TakePicture")) {
                droneCommand = new TakePictureCommand(_droneController, (int)value);
            }

            return droneCommand;
        }
    }
}