using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DroneAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
    }
}