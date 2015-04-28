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
        public void UpdatePrize(Guid PrizeId, int Quantity)
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
            var table = SuuSee.Data.SqlHelper.GetTableText("select * ,(select isnull(count(1),0)  from ScanHistory where ScanHistory.PrizeId = Prize.PrizeId) as UsedCount from Prize where QRCode = @QRCode order by PrizeId",
                new System.Data.SqlClient.SqlParameter("@QRCode", QRCode))[0];
            iMidudu.Model.Prize prize = new iMidudu.Model.Prize();
            int[] totelCount = new int[table.Rows.Count];//总数
            int[] usedCount = new int[table.Rows.Count];//已用
            int[] remainCount = new int[table.Rows.Count];//剩余
            int[] limit = new int[table.Rows.Count];//每日限制
            string[] url = new string[table.Rows.Count];//URL
            string[] PrizeName = new string[table.Rows.Count];//奖项名称
            int allPrizeCount = 0;
            var cumulativeNum = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                totelCount[i] = (int)table.Rows[i]["Quantity"];
                usedCount[i] = (int)table.Rows[i]["UsedCount"];
                limit[i] = (int)table.Rows[i]["DayLimit"];
                url[i] = table.Rows[i]["URL"].ToString();
                PrizeName[i] = table.Rows[i]["PrizeName"].ToString();
                remainCount[i] = totelCount[i] - usedCount[i];
                if (remainCount[i]<0)
                {
                    remainCount[i] = 0;
                }
                allPrizeCount = allPrizeCount + remainCount[i];
            }
            int randomNum = new Random().Next(1, allPrizeCount + 1);
            int j = 0;
            for (j = 0; j < table.Rows.Count; j++)
            {
                cumulativeNum = cumulativeNum + remainCount[j];
                if (randomNum < cumulativeNum + 1)
                {
                    break;
                }
            }
            var prizeId = table.Rows[j]["PrizeId"].ToString();
            var prizeId2 = table.Rows[table.Rows.Count - 1]["PrizeId"].ToString();
            if (prizeId != table.Rows[0]["PrizeId"].ToString())
            {
                var todayUsedCount = (int)SuuSee.Data.SqlHelper.ExecuteScalarText("select isnull(count(1),0)  from ScanHistory where PrizeId=@PrizeId and DATEPART(year,ScanDate) = @Year and DATEPART(month,ScanDate) = @Month and DATEPART(day,ScanDate) = @Today and DATEPART(hour,ScanDate) = @Hour",
                    new System.Data.SqlClient.SqlParameter("@PrizeId", prizeId),
                    new System.Data.SqlClient.SqlParameter("@Year", DateTime.Today.Year),
                    new System.Data.SqlClient.SqlParameter("@Month", DateTime.Today.Month),
                    new System.Data.SqlClient.SqlParameter("@Today", DateTime.Today.Day),
                    new System.Data.SqlClient.SqlParameter("@Hour", DateTime.Today.Hour));
                if (todayUsedCount < limit[j])
                {
                    prize.PrizeId = Guid.Parse(prizeId);
                    prize.DayLimit = limit[j];
                    prize.QRCode = QRCode;
                    prize.Quantity = totelCount[j];
                    prize.NeedValid = false;
                    prize.PrizeName = PrizeName[j];
                    prize.URL = url[j];
                    return prize;
                }
                else
                {
                    prize.PrizeId = Guid.Parse(prizeId2);
                    prize.DayLimit = limit[table.Rows.Count - 1];
                    prize.QRCode = QRCode;
                    prize.Quantity = totelCount[table.Rows.Count - 1];
                    prize.NeedValid = false;
                    prize.PrizeName = PrizeName[table.Rows.Count - 1];
                    prize.URL = url[table.Rows.Count - 1];
                    return prize;
                }
            }
            else
            {
                var todayUsedCount = (int)SuuSee.Data.SqlHelper.ExecuteScalarText("select isnull(count(1),0)  from ScanHistory where PrizeId=@PrizeId and DATEPART(year,ScanDate) = @Year and DATEPART(month,ScanDate) = @Month and DATEPART(day,ScanDate) = @Today",
                    new System.Data.SqlClient.SqlParameter("@PrizeId", prizeId),
                    new System.Data.SqlClient.SqlParameter("@Year", DateTime.Today.Year),
                    new System.Data.SqlClient.SqlParameter("@Month", DateTime.Today.Month),
                    new System.Data.SqlClient.SqlParameter("@Today", DateTime.Today.Day));
                if (todayUsedCount < limit[0])
                {
                    prize.PrizeId = Guid.Parse(prizeId);
                    prize.DayLimit = limit[0];
                    prize.QRCode = QRCode;
                    prize.Quantity = totelCount[0];
                    prize.NeedValid = false;
                    prize.PrizeName = PrizeName[0];
                    prize.URL = url[0];
                    return prize;
                }
                else
                {
                    prize.PrizeId = Guid.Parse(prizeId2);
                    prize.DayLimit = limit[table.Rows.Count - 1];
                    prize.QRCode = QRCode;
                    prize.Quantity = totelCount[table.Rows.Count - 1];
                    prize.NeedValid = false;
                    prize.PrizeName = PrizeName[table.Rows.Count - 1];
                    prize.URL = url[table.Rows.Count - 1];
                    return prize;
                }
            }
        }

        [WebMethod]
        public iMidudu.Model.WXUser  SelectWXUserByScanHistoryId(Guid ScanHistoryId)
        {
            
            iMidudu.Model.WXUser user = new  iMidudu.Biz.ScanHistoryBiz().SelectWXUserByScanHistoryId(ScanHistoryId);
            return user;

        }
    }
}
