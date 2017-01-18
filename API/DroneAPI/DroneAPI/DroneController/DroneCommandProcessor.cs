using DroneAPI.Models;
using System;
using System.Collections.Generic;

/**
 * @author: Harmen Hilvers, Henk-Jan Leusink
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

        //Methode for adding a single command to the Queue
        public void AddCommand(Command command)
        {
            _commands.Enqueue(command);
        }

        //Method for adding a List to the Queue
        public void AddListCommand(List<Command> commands)
        {
            foreach(Command command in commands)
            {
                AddCommand(command);
            }
        }
        
        //Sends the list with commands to the DroneController
        public void Execute()
        {
            DroneCommandSender.Instance.SendData(_commands);
        }
    }
}