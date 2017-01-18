using DroneControl.Commands;
using System.Collections.Generic;

/**
 * @author: Gerhard Kroes
 * */
namespace DroneControl {
    /// <summary>
    /// Drone command processor it adds and execute commands
    /// </summary>
    public class DroneCommandProcessor
    {
        private Queue<IDroneCommand> _commands;
        private Stack<IDroneCommand> _commandsUndo;

        public DroneCommandProcessor() {
            _commands = new Queue<IDroneCommand>();
            _commandsUndo = new Stack<IDroneCommand>();
        }

        /// <summary>
        /// Add a command to the list
        /// </summary>
        /// <param name="command"></param>
        public void AddCommand(IDroneCommand command) {
            _commands.Enqueue(command);
            _commandsUndo.Push(command);
        }

        /// <summary>
        /// Adds commands to the list
        /// </summary>
        /// <param name="commands"></param>
        public void AddListCommand(List<IDroneCommand> commands) {
            foreach(IDroneCommand command in commands) {
                AddCommand(command);
            }
        }

        /// <summary>
        /// Execute command for command and remove the command in the undo list
        /// </summary>
        public void Execute() {
            while (_commands.Count > 0) {
                _commands.Dequeue().Execute();
            }
        }

        /// <summary>
        /// Undo command for command and remove the command in the undo list
        /// </summary>
        public void Undo() {
            while (_commandsUndo.Count > 0) {
                _commandsUndo.Pop().Undo();
            }
        }
    }
}