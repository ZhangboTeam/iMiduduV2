using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMidudu.Model
{
    public class WXUser
    {
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public string Pic { get; set; }
        public string Sex { get; set; }
        public string WXCity { get; set; }
        public string WXProvince { get; set; }
        public string WXCountry { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastActiveTime { get; set; }

    }
}
