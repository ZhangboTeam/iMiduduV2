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
        public iMidudu.Model.Prize PrizeLottery(Guid QRCode)
        {
            //TODO:老杨去实现抽奖逻辑
            //下面hard code了
            var table = SuuSee.Data.SqlHelper.GetTableText("select * ,(select isnull(count(1),0)  from ScanHistory where ScanHistory.PrizeId = Prize.PrizeId) as UsedCount from Prize where QRCode = @QRCode order by PrizeName", new System.Data.SqlClient.SqlParameter("@QRCode", QRCode))[0];
            int[,] a = new int[table.Rows.Count, 1];
            int[] b = new int[table.Rows.Count];//剩余
            int[] d = new int[table.Rows.Count];//每日限制
            string[] url = new string[table.Rows.Count];//URL
            string[] PrizeName = new string[table.Rows.Count];
            int c=0;
            var m=0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                a[i, 0] = (int)table.Rows[i]["Quantity"];
                a[i, 1] = (int)table.Rows[i]["UsedCount"];
                d[i] = (int)table.Rows[i]["DayLimit"];
                url[i] =table.Rows[i]["URL"].ToString();
                PrizeName[i] =table.Rows[i]["PrizeName"].ToString();
                b[i] = a[i, 0] - a[i, 1];
                c = c + b[i];
            }
            int num = new Random().Next(1,c+1);
            for (int j = 0; j < table.Rows.Count; j++) { 
                m=m+b[j];
                if (num < m+1) {
                    if (num < d[j] + 1)
                    {
                        return new iMidudu.Model.Prize()
                        {
                            NeedValid = false,
                            PrizeName = PrizeName[j],
                            URL = url[j]
                        };
                    }
                    else {
                        return new iMidudu.Model.Prize()
                        {
                            NeedValid = false,
                            PrizeName = PrizeName[table.Rows.Count-1],
                            URL = url[table.Rows.Count-1]
                        };                
                    }
            
                }            
            }
            return new iMidudu.Model.Prize()
            {
                NeedValid = false,
                PrizeName = PrizeName[table.Rows.Count - 1],
                URL = url[table.Rows.Count - 1]
             };
        }
    }
}
