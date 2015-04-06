using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SuuSee.WeChat
{
    /// <summary>
    /// Summary description for BizWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class BizWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string ActivityByCode(Guid Code)
        {
            return new iMidudu.Biz.ActivityBiz().GetUrlOfCode(Code);
        }
        [WebMethod]
        public void SaveWXUser(iMidudu.Model.WXUser user)
        {
            //insert or update
            new iMidudu.Biz.WXUserBiz().SaveWXUser(user);

        }
        [WebMethod]
        public void InsertScanHistory(iMidudu.Model.ScanHistory History)
        {
            //insert or update
            new iMidudu.Biz.ScanHistoryBiz().InsertScanHistory(History);

        }
        [WebMethod]
        public void UpdateScanHistory(Guid ScanHistoryId, Guid PrizeId)
        {
            //insert or update
            new iMidudu.Biz.ScanHistoryBiz().UpdateScanHistory(ScanHistoryId,PrizeId);

        }

        [WebMethod]
        public void SaveAcception(iMidudu.Model.Acception accept)
        {
            //insert or update
            new iMidudu.Biz.AcceptionBiz().SaveAcception(accept);
        }
        [WebMethod]
        public void UpdatePrize(Guid PrizeId, string Quantity)
        {
            //insert or update
            new iMidudu.Biz.PrizeBiz().UpdatePrize(PrizeId, Quantity);

        }
        /// <summary>
        /// 抽奖
        /// </summary>
        /// <param name="QRCode">QR二维码</param>
        /// <returns>抽奖结果</returns>
        [WebMethod]
        public iMidudu.Model.Prize PrizeLottery(Guid QRCode)
        {
            iMidudu.Model.Prize prize = new iMidudu.Model.Prize() ;
            //return new iMidudu.Model.Prize()
            //{
            var PrizeId1 = "4643c9ac-7b8a-42a5-a6db-2ae2e627477d";
            var Quantity1 = 30;
                prize.PrizeId = Guid.Parse(PrizeId1.ToString());
                prize.DayLimit = 3;
                prize.QRCode = QRCode;
                prize.Quantity = Quantity1.ToString();
                prize.NeedValid = false;
                prize.PrizeName = "124";
                prize.URL = "/LotteryResult/Activity1/Result1.aspx";
                // };
                return prize;
            



            //TODO:老杨去实现抽奖逻辑
            //下面hard code了
            //var table = SuuSee.Data.SqlHelper.GetTableText("select * ,(select isnull(count(1),0)  from ScanHistory where ScanHistory.PrizeId = Prize.PrizeId) as UsedCount from Prize where QRCode = @QRCode order by PrizeName", 
            //    new System.Data.SqlClient.SqlParameter("@QRCode", QRCode1))[0];
            //int[] totelCount = new int[table.Rows.Count];//总数
            //int[] usedCount = new int[table.Rows.Count];//已用
            //int[] remainCount = new int[table.Rows.Count];//剩余
            //int[] limit = new int[table.Rows.Count];//每日限制
            //string[] url = new string[table.Rows.Count];//URL
            //string[] PrizeName = new string[table.Rows.Count];//奖项名称
            ////int[] todayUsedCount = new int[table.Rows.Count];//今天已用数量
            //int allPrizeCount=0;
            //var cumulativeNum=0;
            //for (int i = 0; i < table.Rows.Count; i++)
            //{
            //    totelCount[i] = (int)table.Rows[i]["Quantity"];
            //    usedCount[i] = (int)table.Rows[i]["UsedCount"];
            //    limit[i] = (int)table.Rows[i]["DayLimit"];
            //    url[i] =table.Rows[i]["URL"].ToString();
            //    PrizeName[i] =table.Rows[i]["PrizeName"].ToString();                
            //    //todayUsedCount[i] = (int)table.Rows[i]["TodayUsedCount"];
            //    remainCount[i] = totelCount[i] - usedCount[i];
            //    allPrizeCount = allPrizeCount + remainCount[i];
            //}
            //int randomNum = new Random().Next(1, allPrizeCount + 1);
            //int j = 0;
            //for (j = 0; j < table.Rows.Count; j++) {
            //    cumulativeNum = cumulativeNum + remainCount[j];
            //    if (randomNum < cumulativeNum + 1)
            //    {
            //        break;           
            //    }            
            //}
            //var PrizeId1 = table.Rows[j]["PrizeId"];
            //var todayUsedCount = (int)SuuSee.Data.SqlHelper.ExecuteScalarText("select isnull(count(1),0)  from ScanHistory where PrizeId=@PrizeId and DATEPART(year,ScanDate) = @Year and DATEPART(month,ScanDate) = @Month and DATEPART(today,ScanDate) = @Today",
            //    new System.Data.SqlClient.SqlParameter("@Year", DateTime.Today.Year),
            //    new System.Data.SqlClient.SqlParameter("@Month", DateTime.Today.Month),
            //    new System.Data.SqlClient.SqlParameter("@Today", DateTime.Today.Day),
            //    new System.Data.SqlClient.SqlParameter("@PrizeId", PrizeId1));
            //iMidudu.Model.Prize prize = null;
            //if (todayUsedCount < limit[j] + 1)
            //{                
            //    //return new iMidudu.Model.Prize()
            //    //{
            //        prize.PrizeId=Guid.Parse(PrizeId1.ToString());
            //        prize.DayLimit=limit[j];
            //        prize.QRCode=QRCode1;
            //        prize.Quantity=totelCount[j].ToString();
            //        prize.NeedValid = false;
            //        prize.PrizeName = PrizeName[j];
            //        prize.URL = url[j];
            //   // };
            //        return prize;
            //}
            //else
            //{
            //   // return new iMidudu.Model.Prize()
            //    //{
            //    prize.PrizeId = Guid.Parse(PrizeId1.ToString());
            //    prize.DayLimit = limit[table.Rows.Count - 1];
            //    prize.QRCode = QRCode1;
            //    prize.Quantity = totelCount[table.Rows.Count - 1].ToString();
            //    prize.NeedValid = false;
            //    prize.PrizeName = PrizeName[table.Rows.Count - 1];
            //    prize.URL = url[table.Rows.Count - 1];
            //   // };
            //    return prize;
            //}
        }

        [WebMethod]
        public iMidudu.Model.WXUser  SelectWXUserByScanHistoryId(Guid ScanHistoryId)
        {
            
            iMidudu.Model.WXUser user = new  iMidudu.Biz.ScanHistoryBiz().SelectWXUserByScanHistoryId(ScanHistoryId);
            return user;

        }
    }
}
