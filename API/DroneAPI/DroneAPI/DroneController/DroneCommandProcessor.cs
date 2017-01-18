using DroneAPI.Models;
using System;
using System.Collections.Generic;

/**
 * @author: Harmen Hilvers
 * Class containing methods for Command Handling, Commands contain behaviour the drone needs to execute
 * */

namespace DroneAPI.DroneController
{
    public class DroneCommandProcessor
    {
        private Queue<Command> _commands;

        public DroneCommandProcessor()
        {
            _commands = new Queue<Command>();
        }

        public void AddCommand(Command command)
        {
            _commands.Enqueue(command);
        }

        public void AddListCommand(List<Command> commands)
        {
            foreach(Command command in commands)
            {
                AddCommand(command);
            }
        }
        
        public void Execute()
        {
            DroneCommandSender.Instance.SendData(_commands);
        }
    }
}