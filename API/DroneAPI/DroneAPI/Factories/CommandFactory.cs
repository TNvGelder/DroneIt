using DroneAPI.Models;
using DroneAPI.DroneController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/**
 * @author: Albert David
 * abstract class for commandfactory
 * */
namespace DroneAPI.Factories
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
