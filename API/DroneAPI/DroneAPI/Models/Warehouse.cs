using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Models
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        
        public virtual GraphNodeDal StartNode { get; set; }
       
        public virtual ICollection<District> Districts { get; set; }
    }
}