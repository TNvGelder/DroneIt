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

        private static CommandFactory _factory;
        public static CommandFactory Factory

        {
            get { return _factory; }
        }
    }
}
