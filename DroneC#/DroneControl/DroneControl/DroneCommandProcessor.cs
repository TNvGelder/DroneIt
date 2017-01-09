using DroneControl.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace DroneControl {
    public class DroneCommandProcessor
    {
        private Queue<IDroneCommand> _commands;
        private Thread _th { get; set; }

        public DroneCommandProcessor() {
            _commands = new Queue<IDroneCommand>();
            _th = new Thread(Executing);
        }

        public void AddCommand(IDroneCommand command) {
            this._commands.Enqueue(command);
        }

        public void AddListCommand(List<IDroneCommand> commands) {
            foreach(IDroneCommand command in commands) {
                AddCommand(command);
            }
        }
        
        public void Execute() {
            _th.Start();
        }

        public void Executing() {
            while (_commands.Count > 0) {
                _commands.Dequeue().Execute();
            }
        }
    }
}