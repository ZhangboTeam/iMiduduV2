using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMidudu.Model
{
   public class ScanHistory
    {
        public Guid ScanHistoryId { get; set; }
        public string OpenId { get; set; }
        public Guid? PrizeId { get; set; }
        public string IP { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string LineType { get; set; }
        public string Agent { get; set; }
        public string Os { get; set; }
        public DateTime ScanDate { get; set; }
        public string TicketUrl { get; set; }
        public string TicketNumber { get; set; }

    }
}
