using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneAPI.Processors.DroneProcessors.Commands
{
    public interface IDroneCommand
    {
        void Execute();
        void Undo();
    }
}
