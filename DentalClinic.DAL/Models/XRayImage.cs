using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Models
{
    public class XRayImage
    {
        public int Id {  get; set; }
        public string ImageName { get; set; }
        public int VisitId {  get; set; }
        public Visit Visit { get; set; }
    }
}
