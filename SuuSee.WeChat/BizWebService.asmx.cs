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
            var dr = SuuSee.Data.SqlHelper.GetTableText("select * ,(select isnull(count(1),0)  from ScanHistory where ScanHistory.PrizeId = Prize.PrizeId) as 已领数量 from Prize where QRCode = @QRCode order by PrizeName", new System.Data.SqlClient.SqlParameter("@QRCode", QRCode));
            int[,] a=new int[6,1];
            int[] b=new int[6];//剩余
            int[] d=new int[6];//每日限制
            string[] url=new string[6];//URL
            int c;
            for (int i = 0; i < 7; i++) {
                a[i, 0] = dr.;
                a[i, 1] = 1;
                d[i] = 1;
                url[i] = 1;
            }
            b[0] = a[0, 0] - a[0, 1];
            b[1] = a[1, 0] - a[1, 1];
            b[2] = a[2, 0] - a[2, 1];
            b[3] = a[3, 0] - a[3, 1];
            b[4] = a[4, 0] - a[4, 1];
            b[5] = a[5, 0] - a[5, 1];
            b[6] = a[6, 0] - a[6, 1];
            c = b[0] + b[1] + b[2] + b[3] + b[4] + b[5] + b[6];
            int num = new Random().Next(1,c+1);
            //判断是是否是大奖
            if (num < b[0]+1) {
                if (num < d[0] + 1)
                {
                    return new iMidudu.Model.Prize()
                    {
                        NeedValid = false,
                        PrizeName = "大奖",
                        URL = url[0]//"/LotteryResult/Activity1/Result1.aspx"
                    };
                }
                else {
                    return new iMidudu.Model.Prize()
                    {
                        NeedValid = false,
                        PrizeName = "谢谢参与",
                        URL = url[6]//"/LotteryResult/Activity1/Result1.aspx"
                    };                
                }
            
            }
            //判断是是否是小奖1
            else if (num < b[0]+b[1] + 1)
            {
                if (num < d[1] + 1)
                {
                    return new iMidudu.Model.Prize()
                    {
                        NeedValid = false,
                        PrizeName = "小奖1",
                        URL = url[1]//"/LotteryResult/Activity1/Result1.aspx"
                    };
                }
                else
                {
                    return new iMidudu.Model.Prize()
                    {
                        NeedValid = false,
                        PrizeName = "谢谢参与",
                        URL = url[6]//"/LotteryResult/Activity1/Result1.aspx"
                    };
                }

            }
            //判断是是否是小奖2
            else if (num < b[0] + b[1] +b[2]+ 1)
            {
                if (num < d[2] + 1)
                {
                    return new iMidudu.Model.Prize()
                    {
                        NeedValid = false,
                        PrizeName = "小奖2",
                        URL = url[2]//"/LotteryResult/Activity1/Result1.aspx"
                    };
                }
                else
                {
                    return new iMidudu.Model.Prize()
                    {
                        NeedValid = false,
                        PrizeName = "谢谢参与",
                        URL = url[6]//"/LotteryResult/Activity1/Result1.aspx"
                    };
                }

            }
            //判断是是否是小奖3
            else if (num < b[0] + b[1] +b[2]+b[3]+ 1)
            {
                if (num < d[3] + 1)
                {
                    return new iMidudu.Model.Prize()
                    {
                        NeedValid = false,
                        PrizeName = "小奖3",
                        URL = url[3]//"/LotteryResult/Activity1/Result1.aspx"
                    };
                }
                else
                {
                    return new iMidudu.Model.Prize()
                    {
                        NeedValid = false,
                        PrizeName = "谢谢参与",
                        URL = url[6]//"/LotteryResult/Activity1/Result1.aspx"
                    };
                }

            }
            //判断是是否是小奖4
            else if (num < b[0] + b[1] + b[2] + b[3] + b[4] + 1)
            {
                if (num < d[4] + 1)
                {
                    return new iMidudu.Model.Prize()
                    {
                        NeedValid = false,
                        PrizeName = "小奖4",
                        URL = url[4]//"/LotteryResult/Activity1/Result1.aspx"
                    };
                }
                else
                {
                    return new iMidudu.Model.Prize()
                    {
                        NeedValid = false,
                        PrizeName = "谢谢参与",
                        URL = url[6]//"/LotteryResult/Activity1/Result1.aspx"
                    };
                }

            }
            //判断是是否是小奖5
            else if (num < b[0] + b[1] + b[2] + b[3] + b[4] + b[5] + 1)
            {
                if (num < d[5] + 1)
                {
                    return new iMidudu.Model.Prize()
                    {
                        NeedValid = false,
                        PrizeName = "小奖5",
                        URL = url[5]//"/LotteryResult/Activity1/Result1.aspx"
                    };
                }
                else
                {
                    return new iMidudu.Model.Prize()
                    {
                        NeedValid = false,
                        PrizeName = "谢谢参与",
                        URL = url[6]//"/LotteryResult/Activity1/Result1.aspx"
                    };
                }

            }
            //判断是是否是谢谢参与
            else 
            {
   
                    return new iMidudu.Model.Prize()
                    {
                        NeedValid = false,
                        PrizeName = "谢谢参与",
                        URL = url[6]//"/LotteryResult/Activity1/Result1.aspx"
                    };


            }
        }
    }
}
