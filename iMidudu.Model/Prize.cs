using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMidudu.Model
{
    public class Prize
    {
        public Guid PrizeId { get; set; }
        public Guid QRCode { get; set; }
        public string PrizeName { get; set; } 
        public int Quantity { get; set; }
        public bool NeedValid { get; set; }
        public string URL { get; set; }
        public int DayLimit  { get; set; }

    }
}
