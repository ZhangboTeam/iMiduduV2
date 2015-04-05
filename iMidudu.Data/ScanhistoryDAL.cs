using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMidudu.Data
{
    public class ScanHistoryDAL
    {
        public void InsertScanHistory(iMidudu.Model.ScanHistory history)
        {
            //insert
            
            var ps = new System.Data.SqlClient.SqlParameter[] {  new System.Data.SqlClient.SqlParameter("@ScanHistoryId", history.ScanHistoryId),
                new System.Data.SqlClient.SqlParameter("@OpenId", history.OpenId),
                new System.Data.SqlClient.SqlParameter("@PrizeId", history.PrizeId == null ? (object) DBNull.Value : history.PrizeId),
                new System.Data.SqlClient.SqlParameter("@IP", history.IP),
                new System.Data.SqlClient.SqlParameter("@Country", history.Country),
                new System.Data.SqlClient.SqlParameter("@Province", history.Province),
                new System.Data.SqlClient.SqlParameter("@Area", history.Area),
                new System.Data.SqlClient.SqlParameter("@City", history.City),
                new System.Data.SqlClient.SqlParameter("@District", history.District),
                new System.Data.SqlClient.SqlParameter("@LineType", history.LineType),
                new System.Data.SqlClient.SqlParameter("@Agent", history.Agent),
                new System.Data.SqlClient.SqlParameter("@Os", history.Os),
                new System.Data.SqlClient.SqlParameter("@ScanDate", DateTime.Now),
                new System.Data.SqlClient.SqlParameter("@TicketUrl", history.TicketUrl),
                new System.Data.SqlClient.SqlParameter("@TicketNumber", history.TicketNumber)};
            foreach (var item in ps)
            {
                if (item.Value==null||string.IsNullOrEmpty (item.Value.ToString()))
                {
                    item.Value = DBNull.Value;
                }
            }
            SuuSee.Data.SqlHelper.ExecteNonQueryStoredProcedure("ScanHistory_InsertProcedure",ps
               );

        }
        public void UpdateScanHistory(Guid ScanHistoryId,Guid PrizeId)
        {
            SuuSee.Data.SqlHelper.ExecteNonQueryText("update ScanHistory set PrizeId=@PrizeId where ScanHistoryId=@ScanHistoryId",
                new System.Data.SqlClient.SqlParameter("@PrizeId", PrizeId),
                new System.Data.SqlClient.SqlParameter("@ScanHistoryId", ScanHistoryId));
        }
      
    }

}
