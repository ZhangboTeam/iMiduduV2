using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMidudu.Data
{
    public class PrizeDAL
    {
        //public void InsertPrize(iMidudu.Model.Prize prize)
        //{
        //    //insert

        //    var ps = new System.Data.SqlClient.SqlParameter[] {  new System.Data.SqlClient.SqlParameter("@Prize_InsertProcedure", prize.PrizeId),
        //        new System.Data.SqlClient.SqlParameter("@PrizeId", prize.PrizeId),
        //        new System.Data.SqlClient.SqlParameter("@QRCode", prize.QRCode),
        //        new System.Data.SqlClient.SqlParameter("@Quantity", prize.Quantity),
        //        new System.Data.SqlClient.SqlParameter("@NeedValid", prize.NeedValid),
        //        new System.Data.SqlClient.SqlParameter("@DayLimit", prize.DayLimit),
        //        new System.Data.SqlClient.SqlParameter("@URL", prize.URL),
        //        new System.Data.SqlClient.SqlParameter("@PrizeName", prize.PrizeName)};
        //    foreach (var item in ps)
        //    {
        //        if (item.Value == null || string.IsNullOrEmpty(item.Value.ToString()))
        //        {
        //            item.Value = DBNull.Value;
        //        }
        //    }
        //    SuuSee.Data.SqlHelper.ExecteNonQueryStoredProcedure("ScanHistory_InsertProcedure", ps
        //       );
        //}
        public void UpdatePrize(Guid PrizeId, string Quantity)
        {
            SuuSee.Data.SqlHelper.ExecteNonQueryText("update ScanHistory set Quantity=@Quantity where PrizeId=@PrizeId",
                new System.Data.SqlClient.SqlParameter("@PrizeId", PrizeId),
                new System.Data.SqlClient.SqlParameter("@Quantity", Quantity)
                );
        }
    }
}
