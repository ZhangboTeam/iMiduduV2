using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMidudu.Data
{
   public class AcceptionDAL
    {
       public bool ExistsByScanHistoryId(Guid scanHistoryId)
       {

           var dr = SuuSee.Data.SqlHelper.ExecuteReaderStoredProcedure("Acception_SelectProcedure",
                 new System.Data.SqlClient.SqlParameter("@ScanHistoryId", scanHistoryId));
           while (dr.Read())
           {
               return true;
           }
           return false;
       }
       public void InsertAcception(iMidudu.Model.Acception accept)
       {
           SuuSee.Data.SqlHelper.ExecteNonQueryStoredProcedure("Acception_InsertProcedure",
               new System.Data.SqlClient.SqlParameter("@ScanHistoryId", accept.ScanHistoryId),
               new System.Data.SqlClient.SqlParameter("@Address", accept.Address),
               new System.Data.SqlClient.SqlParameter("@Mobile", accept.Mobile),
               new System.Data.SqlClient.SqlParameter("@UserName", accept.UserName),
               new System.Data.SqlClient.SqlParameter("@ValidCode", accept.ValidCode),
               new System.Data.SqlClient.SqlParameter("@Remark", accept.Remark),
               new System.Data.SqlClient.SqlParameter("@Status", accept.Status));
       }
       public void UpdateAcception(iMidudu.Model.Acception accept)
       {
           SuuSee.Data.SqlHelper.ExecteNonQueryStoredProcedure("Acception_UpdateProcedure",
               new System.Data.SqlClient.SqlParameter("@ScanHistoryId", accept.ScanHistoryId),
               new System.Data.SqlClient.SqlParameter("@Address", accept.Address),
               new System.Data.SqlClient.SqlParameter("@Mobile", accept.Mobile),
               new System.Data.SqlClient.SqlParameter("@UserName", accept.UserName),
               new System.Data.SqlClient.SqlParameter("@ValidCode", accept.ValidCode),
               new System.Data.SqlClient.SqlParameter("@Remark", accept.Remark),
               new System.Data.SqlClient.SqlParameter("@Status", accept.Status));
       }
       public Model.Acception SelectById(Guid ScanHistoryId)
       {
           var dr = SuuSee.Data.SqlHelper.ExecuteReaderStoredProcedure("Acception_SelectProcedure",
               new System.Data.SqlClient.SqlParameter("@scanHistoryId", ScanHistoryId));
           Model.Acception model = null;
           while (dr.Read())
           {
               model = new Model.Acception()
               {
                   ScanHistoryId = (Guid)dr["ScanHistoryId"],
                   Address = dr["Address"].ToString(),
                   Mobile = dr["Mobile"].ToString(),
                   UserName = dr["UserName"].ToString(),
                   ValidCode = dr["ValidCode"].ToString(),
                   Remark = dr["Remark"].ToString(),
                   Status = (int)dr["Status"]
               };
           }
           return model;
       }
   }
}
