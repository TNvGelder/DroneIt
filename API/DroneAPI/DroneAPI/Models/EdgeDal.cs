using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Models
{
    public class EdgeDal
    {
        public int Id { get; set; }
        public GraphNodeDal GraphNodeDal { get; set; }
        public virtual GraphNodeDal DestinationGraphNode { get; set; }

    }
}