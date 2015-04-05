using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMidudu.Biz
{
   public class AcceptionBiz
    {
       public void SaveAcception(iMidudu.Model.Acception accept) 
       {
           var dal = new iMidudu.Data.AcceptionDAL();
           var exists = dal.ExistsByScanHistoryId(accept.ScanHistoryId);
           if (exists)
           {
               var oldaccept = dal.SelectById(accept.ScanHistoryId);
               oldaccept.Address = accept.Address;
               oldaccept.Mobile = accept.Mobile;
               oldaccept.UserName = accept.UserName;
               oldaccept.ValidCode = accept.ValidCode;
               oldaccept.Remark = accept.Remark;
               oldaccept.Status = accept.Status;
               dal.UpdateAcception(oldaccept);
           }
           else
           {
               dal.InsertAcception(accept);
           }
       }
    }
}
