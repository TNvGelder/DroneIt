using DroneAPI.Models;
using DroneAPI.Processors.DroneProcessors.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DroneAPI.Processors.DroneProcessors
{
    public class DroneCommandProcessor
    {
        private DroneProcessor _droneProcessor;

        private Queue<IDroneCommand> _commands;

        public DroneCommandProcessor(DroneProcessor droneProcessor)
        {
            this._droneProcessor = droneProcessor;

            this._commands = new Queue<IDroneCommand>();
        }

        public void AddCommand(IDroneCommand command)
        {
            this._commands.Enqueue(command);
        }

        public void AddListCommand(List<IDroneCommand> commands)
        {
            foreach(IDroneCommand command in commands)
            {
                this.AddCommand(command);
            }
        }
        
        public async void Execute()
        {
            await Task.Run(() => asyncExecute()).ConfigureAwait(false);
        }

        public async void asyncExecute() {
            while (true) {
                if (_droneProcessor.DroneIsBusy() == false) {
                    if (this._commands.Count > 0) {
                        IDroneCommand nextCommand = this._commands.Dequeue();
                        nextCommand.Execute();
                    } else {
                        break;
                    }
                } else {
                    await Task.Delay(25);
                }
            }
        }
    }
}