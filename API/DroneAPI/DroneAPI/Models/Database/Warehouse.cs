using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/**
 * @author: Harmen Hilvers
 * Model containing position information
 * */

namespace DroneAPI.Models.Database
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        
        public virtual GraphNode StartNode { get; set; }
       
        public virtual ICollection<District> Districts { get; set; }
    }
}