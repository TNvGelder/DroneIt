using DroneControl.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/**
 * @author: Gerhard Kroes
 * */
namespace DroneControl
{
    /// <summary>
    /// Command factory the makes commands
    /// </summary>
    public class CommandFactory
    {
        private DroneController _droneController { get; set; }

        public CommandFactory(DroneController droneProcessor) {
            _droneController = droneProcessor;
        }

        /// <summary>
        /// Makes from a string and value a command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="value"></param>
        /// <returns>Command</returns>
        public IDroneCommand makeCommand(string command, double value = 0) {
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
            } else if (command.Equals("FollowLine")) {
                droneCommand = new FollowLineCommand(_droneController);
            }

            return droneCommand;
        }
    }
}