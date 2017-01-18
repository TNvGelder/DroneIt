using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 * @author: Gerhard Kroes
 * */
namespace DroneControl {
    /// <summary>
    /// Command data and can transfor into a command
    /// </summary>
    public class Command {
        public string name { get; set; }
        public double value { get; set; }
    }
}
