using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

/**
 * @author: Henk-Jan
 * Model containing product information
 * */
namespace DroneAPI.Models.Database
{

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProductLocation> ProductLocations { get; set; }
    }
}