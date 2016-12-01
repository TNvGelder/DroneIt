using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Models
{
    public class EdgeDal
    {
        public int Id { get; set; }
        public int GraphNodeDal_Id { get; set; }
        public virtual GraphNodeDal DestinationGraphNode { get; set; }

    }
}