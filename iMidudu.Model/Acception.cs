using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMidudu.Model
{
   public class Acception
    {
       public Guid ScanHistoryId { get; set; }
       public string Address { get; set; }
       public string Mobile { get; set; }
       public string ValidCode { get; set; }
       public string Remark { get; set; }
       public int Status { get; set; }
    }
}
