﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DroneAPI.Models
{
    /// <summary>
    /// Simple product
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProductLocation> ProductLocations { get; set; }
    }
}