using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


/**
 * @author: Harmen Hilvers
 * Model containing link from product to a location
 * */
namespace DroneAPI.Models.Database
{

    public class ProductLocation
    {
        public int Id { get; set; }
        public virtual Product Product { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
   
        
        public virtual District District { get; set; }
      
    }
}