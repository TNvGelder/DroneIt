using DroneControl.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DroneControl.Factories
{
    public interface ICommandFactory
    {
        IDroneCommand MakeCommand(string command, double value);
    }
}
