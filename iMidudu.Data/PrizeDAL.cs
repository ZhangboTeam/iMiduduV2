using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMidudu.Data
{
    public class PrizeDAL
    {
        public void UpdatePrize(Guid PrizeId, int Quantity)
        {
            SuuSee.Data.SqlHelper.ExecteNonQueryText("update Prize set Quantity=@Quantity where PrizeId=@PrizeId",
                new System.Data.SqlClient.SqlParameter("@PrizeId", PrizeId),
                new System.Data.SqlClient.SqlParameter("@Quantity", Quantity)
                );
        }
        public iMidudu.Model.Prize[] SelectPrizeByQRCode(Guid QRCode) {
            
           var table= SuuSee.Data.SqlHelper.GetTableText("select * from Prize where QRCode = @QRCode order by PrizeName",
                new System.Data.SqlClient.SqlParameter("@QRCode", QRCode))[0];
           iMidudu.Model.Prize[] prize = new iMidudu.Model.Prize[table.Rows.Count];
           for (int i = 0; i < table.Rows.Count; i++) {
               prize[i].PrizeId = Guid.Parse(table.Rows[i]["PrizeId"].ToString());
               prize[i].QRCode = QRCode;
               prize[i].Quantity = (int)table.Rows[i]["Quantity"];
               prize[i].DayLimit = (int)table.Rows[i]["DayLimit"];
               prize[i].URL = table.Rows[i]["URL"].ToString();
               prize[i].PrizeName = table.Rows[i]["PrizeName"].ToString();
               prize[i].NeedValid = false;
           }
               return prize;
        }
    }
}
