using DroneAPI.Models;
using DroneAPI.Processors.DroneProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneAPI.Factorys
{
    public abstract class CommandFactory
    {
        private static MovementCommandFactory _factory;
        public static MovementCommandFactory Factory
        {
            get { return _factory; }
        }
    }
}
