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

        /// <summary>
        /// 抽奖
        /// </summary>
        /// <param name="QRCode">QR二维码</param>
        /// <returns>抽奖结果</returns>
        [WebMethod]
        public iMidudu.Model.Prize PrizeLottery(Guid QRCode1)
        {
            //TODO:老杨去实现抽奖逻辑
            //下面hard code了
            var table = SuuSee.Data.SqlHelper.GetTableText("select * ,(select isnull(count(1),0)  from ScanHistory where ScanHistory.PrizeId = Prize.PrizeId) as UsedCount from Prize where QRCode = @QRCode order by PrizeName", 
                new System.Data.SqlClient.SqlParameter("@QRCode", QRCode1))[0];
            int[] totelCount = new int[table.Rows.Count];//总数
            int[] usedCount = new int[table.Rows.Count];//已用
            int[] remainCount = new int[table.Rows.Count];//剩余
            int[] limit = new int[table.Rows.Count];//每日限制
            string[] url = new string[table.Rows.Count];//URL
            string[] PrizeName = new string[table.Rows.Count];//奖项名称
            //int[] todayUsedCount = new int[table.Rows.Count];//今天已用数量
            int allPrizeCount=0;
            var cumulativeNum=0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                totelCount[i] = (int)table.Rows[i]["Quantity"];
                usedCount[i] = (int)table.Rows[i]["UsedCount"];
                limit[i] = (int)table.Rows[i]["DayLimit"];
                url[i] =table.Rows[i]["URL"].ToString();
                PrizeName[i] =table.Rows[i]["PrizeName"].ToString();                
                //todayUsedCount[i] = (int)table.Rows[i]["TodayUsedCount"];
                remainCount[i] = totelCount[i] - usedCount[i];
                allPrizeCount = allPrizeCount + remainCount[i];
            }
            int randomNum = new Random().Next(1, allPrizeCount + 1);
            int j = 0;
            for (j = 0; j < table.Rows.Count-1; j++) {
                cumulativeNum = cumulativeNum + remainCount[j];
                if (randomNum < cumulativeNum + 1)
                {
                    break;           
                }            
            }
            var PrizeId1 = table.Rows[j]["PrizeId"];
            var todayUsedCount = (int)SuuSee.Data.SqlHelper.ExecuteScalarText("select isnull(count(1),0)  from ScanHistory where PrizeId=@PrizeId and DATEPART(year,ScanDate) = @Year and DATEPART(month,ScanDate) = @Month and DATEPART(today,ScanDate) = @Today",
                new System.Data.SqlClient.SqlParameter("@Year", DateTime.Today.Year),
                new System.Data.SqlClient.SqlParameter("@Month", DateTime.Today.Month),
                new System.Data.SqlClient.SqlParameter("@Today", DateTime.Today.Day),
                new System.Data.SqlClient.SqlParameter("@PrizeId", PrizeId1));
            if (todayUsedCount < limit[j] + 1)
            {
                return new iMidudu.Model.Prize()
                {
                    PrizeId=Guid.Parse(PrizeId1.ToString()),
                    DayLimit=limit[j],
                    QRCode=QRCode1,
                    Quantity=totelCount[j].ToString(),
                    NeedValid = false,
                    PrizeName = PrizeName[j],
                    URL = url[j]
                };
            }
            else
            {
                return new iMidudu.Model.Prize()
                {
                    PrizeId=Guid.Parse(PrizeId1.ToString()),
                    DayLimit = limit[table.Rows.Count - 1],
                    QRCode=QRCode1,
                    Quantity = totelCount[table.Rows.Count - 1].ToString(),
                    NeedValid = false,
                    PrizeName = PrizeName[table.Rows.Count - 1],
                    URL = url[table.Rows.Count - 1]
                };
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
