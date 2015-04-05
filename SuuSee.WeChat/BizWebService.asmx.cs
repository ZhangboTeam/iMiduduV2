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
            return new iMidudu.Model.Prize()
            {
                NeedValid = false,
                PrizeName = "3等奖",
                URL = "/LotteryResult/Activity1/Result3.aspx"
            };
        }
    }
}
