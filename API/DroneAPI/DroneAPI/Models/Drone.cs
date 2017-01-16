using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/**
 * @author: Harmen Hilvers
 * Model containing Drone info
 * */

namespace DroneAPI.Models
{
    public class Drone
    {
        public string Name { get; private set; }
        public string NodeJsIp { get; private set; }
        public bool Flying { get; set; } = false;
        public bool Busy { get; set; } = false;

        public Drone(string name, string ip)
        {
            this.NodeJsIp = ip;
            this.Name = name;
        }
    }
}