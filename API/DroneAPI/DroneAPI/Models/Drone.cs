using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Models
{
    public class Drone
    {
        public string Name { get; set; } = "Thomas";
        public string NodeJsIp { get; set; } = "http://127.0.0.1:8000/";
        public bool Flying { get; set; } = false;
        public bool busy { get; set; }
    }
}