using DroneAPI.Models;
using DroneAPI.Processors.DroneProcessors;
using DroneAPI.Processors.DroneProcessors.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneAPI.Factorys
{
    public abstract class CommandFactory
    {
        private DroneProcessor _droneProcessor;

        public CommandFactory(DroneProcessor _droneProcessor)
        {
            this._droneProcessor = _droneProcessor;
        }
    }
}
