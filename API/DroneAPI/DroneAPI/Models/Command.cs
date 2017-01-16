using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Models
{
    /**
     * @author: Albert David
     * Model containing command info for DroneControl
     * */
    public class Command
    {
        public string name { get; set; }
        public double value { get; set; }
    }
}