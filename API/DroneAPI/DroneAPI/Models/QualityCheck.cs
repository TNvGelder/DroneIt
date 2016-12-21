using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneAPI.Models
{
    public class QualityCheck
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual ProductLocation ProductLocation { get; set; }
        public string Status { get; set; }
        public string PictureFolderUrl { get; set; }
    }
}