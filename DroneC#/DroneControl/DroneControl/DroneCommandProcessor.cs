using DroneControl.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace DroneControl {
    public class DroneCommandProcessor
    {

        private Queue<IDroneCommand> _commands;

        public DroneCommandProcessor() {
            this._commands = new Queue<IDroneCommand>();
        }

        public void AddCommand(IDroneCommand command) {
            this._commands.Enqueue(command);
        }

        public void AddListCommand(List<IDroneCommand> commands) {
            foreach(IDroneCommand command in commands) {
                this.AddCommand(command);
            }
        }
        
        public void Execute() {
            while (_commands.Count > 0) {
                _commands.Dequeue().Execute();
            }
        }

        public List<Command> commandList() {
            List<Command> commands = new List<Command>();

            foreach (IDroneCommand dc in _commands) {
                commands.Add(new Command { name = dc.GetName(), value = dc.GetValue() });
            }

            return commands;
        }
    }
}