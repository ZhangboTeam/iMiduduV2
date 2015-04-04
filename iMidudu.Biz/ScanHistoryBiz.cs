using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMidudu.Biz
{
     public class ScanHistoryBiz
    {
        public void InsertScanHistory(iMidudu.Model.ScanHistory History)
        {
            //insert or update
            var dal = new iMidudu.Data.ScanHistoryDAL();
            dal.InsertScanHistory(History);
        }
        public void UpdateScanHistory(Guid ScanHistoryId, Guid PrizeId)
        {
            //insert or update
            var dal = new iMidudu.Data.ScanHistoryDAL();
            dal.UpdateScanHistory(ScanHistoryId,PrizeId);
        }

    }
}
