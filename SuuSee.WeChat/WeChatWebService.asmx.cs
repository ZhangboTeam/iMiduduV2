using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Services;

namespace SuuSee.WeChat
{
    /// <summary>
    /// Summary description for WeChatWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WeChatWebService : System.Web.Services.WebService
    {

        public   long Timestamp
        {
            get
            {

                return (DateTime.Now.Ticks / 100000000);
            }
        }
        public   string nonceStr
        {
            get
            {
                return Guid.NewGuid().ToString().ToLower().Replace("-", "");
            }
        }
         

        [WebMethod(EnableSession =true)]
        public string AccessToken()
        {
            return this.Token;
        }
        public   string Token
        {
            get
            {
                var cachedToken = System.Web.HttpContext.Current.Cache["token"];
                 
                if (cachedToken == null )
                {
                    CacheItemRemovedCallback("token", null, CacheItemRemovedReason.Expired);
                    cachedToken = System.Web.HttpContext.Current.Cache["token"];

                }
                return cachedToken.ToString();
            }
        }
        private  void CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            if (key == "token")
            {
                var tokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wx8cabe7121f5369a3&secret=6066d7e2e03fbb351a9a4602f07a3a94";
                //              https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wx8cabe7121f5369a3&secret=6066d7e2e03fbb351a9a4602f07a3a94
                var response = System.Net.WebRequest.Create(tokenUrl).GetResponse();
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                var newToken = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(responseFromServer);
                System.Web.HttpContext.Current.Cache.Insert("token", newToken.access_token, null, DateTime.Now.AddSeconds(newToken.expires_in), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, CacheItemRemovedCallback);
            }
        }


        public  string Ticket
        {
            get
            {
                var cachedTicket = System.Web.HttpContext.Current.Cache.Get("ticket");
                if (cachedTicket == null)
                {
                    TicketItemRemovedCallback("ticket", null, CacheItemRemovedReason.Expired);
                    cachedTicket = System.Web.HttpContext.Current.Cache.Get("ticket");

                }
                return cachedTicket.ToString();
            }
        }
        private  void TicketItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            if (key == "ticket")
            {
                var tokenUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + Token + "&type=jsapi";

                var response = System.Net.WebRequest.Create(tokenUrl).GetResponse();
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                var newTicket = Newtonsoft.Json.JsonConvert.DeserializeObject<TicketResponse>(responseFromServer);
                if (newTicket.errcode != 0)
                {
                    throw new Exception(newTicket.errmsg);
                }
                System.Web.HttpContext.Current.Cache.Add("ticket", newTicket.ticket, null, DateTime.Now.AddSeconds(newTicket.expires_in ), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, TicketItemRemovedCallback);
            }
        }

        [WebMethod(EnableSession =true)]
        public  OpenIdResponse getOpenId(string code)
        {

            var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                System.Web.Configuration.WebConfigurationManager.AppSettings["AppID"],
                System.Web.Configuration.WebConfigurationManager.AppSettings["AppSecret"],
                code);

            //  Adinnet.SEQ.interfaces.Log.Add(url);
            var response = System.Net.WebRequest.Create(url).GetResponse();
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<OpenIdResponse>(responseFromServer);
            //  Adinnet.SEQ.interfaces.Log.Add(responseFromServer);
            //  Adinnet.SEQ.interfaces.Log.Add("CODE:" +System.Web.HttpContext.Current.Request["code"]);
            return result;
        }

        [WebMethod(EnableSession = true)]
        public  UserInfo getUserInfo(OpenIdResponse openid)
        {
            // var openid = getOpenId();
            //https://api.weixin.qq.com/sns/userinfo?access_token=OezXcEiiBSKSxW0eoylIeKnhyo09fwXSJHCEJ1LNgisTLHUblD2OzRxOO0bdFyWf0cHcGYAEHp6SgtjStmnUQ4IYzA6Ht2IQzJBg9bGYKReodSh682YH6tSSy-0sj0AgiyCrLtHwXhR2XRmdt9IDcQ&openid=oo-nWs3meUO4Bu_zEWKoZYvpcr2g&lang=zh_CN
            //var url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", openid.access_token, openid.openid);
            var url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", openid.access_token, openid.openid);
            //  Adinnet.SEQ.interfaces.Log.Add(url);

            var response = System.Net.WebRequest.Create(url).GetResponse();
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfo>(responseFromServer);

            // Adinnet.SEQ.interfaces.Log.Add(responseFromServer);
            return result;
            //return getResponse<UserInfo>(url);
        }
        [WebMethod(EnableSession = true)]
        public  string newBillNo()
        {
            var rnd = new Random();
            return string.Format("{0}{1}{2}{3}", System.Web.Configuration.WebConfigurationManager.AppSettings["mch_id"], DateTime.Now.ToString("yyyyMMdd"), rnd.Next(10000, 99999), rnd.Next(10000, 99999));
        }
        public  T getResponse<T>(string url)
        {

            var response = System.Net.WebRequest.Create(url).GetResponse();
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            //  Adinnet.SEQ.interfaces.Log.Add(responseFromServer);
            T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseFromServer);
            return result;
        }
        public  string GetMd5(string inputStr)
        {
            byte[] md5Bytes = Encoding.UTF8.GetBytes(inputStr);

            // compute MD5 hash.
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] cryptString = md5.ComputeHash(md5Bytes);

            int len;
            string temp = String.Empty;

            len = cryptString.Length;

            for (int i = 0; i < len; i++)
            {
                temp += cryptString[i].ToString("X2");
            }
            return temp;
        }
        /// <summary>
        /// 创建签名
        /// </summary>
        /// <returns></returns>
        /// <remarks>必须保证所有传入的参数都有值，没有值的要过滤掉</remarks>
         string CreateSignString(Dictionary<string, string> parameters, string paySecret)
        {
            foreach (var key in parameters.Keys.Where(key => string.IsNullOrEmpty(parameters[key])))
            {
                parameters.Remove(key);
            }

            //第一步，设所有収送戒者接收到的数据为集合 M，将集合 M 内非空参数值的参数按照参数名ASCII码从小到大排序（字典序），
            //使用URL键值对的格式（即key1=value1&key2=value2…）拼接成字符串 stringA。
            //特别注意以下重要规则：
            //1.参数名 ASCII 码从小到大排序（字典序） ；
            //2.如果参数的值为空不参与签名；
            //3.参数名区分大小写；
            //4.验证调用返回戒微信主劢通知签名时，传送的 sign 参数不参与签名，将生成的签名与该 sign 值作校验。
            var keyArray = parameters.Keys.ToArray();
            Array.Sort(keyArray);
            var keyString = keyArray.Aggregate("", (current, t) => current + (t + "=" + parameters[t] + "&"));

            //第二步，在 stringA 最后拼接上 key=商户支付密钥得到 stringSignTemp 字符串，
            //并对 stringSignTemp 进行 MD5 运算，再将得到的字符串所有字符转换为大写，得到 sign的值 signValue
            keyString += "key=" + paySecret;

            return GetMd5(keyString).ToUpper();
        } 
        [WebMethod(EnableSession=true)]
        public string SendBounsToOpenId(string OpenId, int Money, string billNo,  out string paramstr, out string responseXML)
        {
         
            var actName = "perfetti";
            var ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            var nickname = "米嘟嘟";
            var tmpstr = "d56ace11210245d2aed5f0243f9b68e3";// nonceStr;
            var remark = "米嘟嘟";
            var sendername = "不凡帝";
            var wishing = "不凡帝小店活动红包";
            Money *= 100;

            var payKey = "d56ace11210245d2aed5f0243f9b68e3";

            var sb = new StringBuilder();
            sb.AppendFormat("act_name={0}", actName);
            sb.AppendFormat("&client_ip={0}", ip);
            //  sb.AppendFormat("&logo_imgurl={0}", "https://ss0.bd.com/5a21bjqh_Q23odCf//superplus/img/logo_white_ee663702.png");
            sb.AppendFormat("&max_value={0}", Money);
            sb.AppendFormat("&mch_billno={0}", billNo);
            sb.AppendFormat("&mch_id={0}", System.Web.Configuration.WebConfigurationManager.AppSettings["mch_id"]);
            sb.AppendFormat("&min_value={0}", Money);
            sb.AppendFormat("&nick_name={0}", nickname);
            sb.AppendFormat("&nonce_str={0}", tmpstr);
            sb.AppendFormat("&re_openid={0}", OpenId);
            sb.AppendFormat("&remark={0}", remark);
            sb.AppendFormat("&send_name={0}", sendername);
            //sb.AppendFormat("&share_content={0}",  "iMidudu");
            // sb.AppendFormat("&share_imgurl={0}", "http://img0.bd.com/img/image/shouye/bizhi0313.jpg");
            // sb.AppendFormat("&share_url={0}", "http://www.baidu.com/"  );
            // sb.AppendFormat("&sub_mch_id={0}",  );
            sb.AppendFormat("&total_amount={0}", Money);
            sb.AppendFormat("&total_num={0}", 1);
            sb.AppendFormat("&wishing={0}", wishing);
            sb.AppendFormat("&wxappid={0}", System.Web.Configuration.WebConfigurationManager.AppSettings["AppID"]);

            var param = sb.ToString() + "&key=" + payKey;


            //Dictionary<string, string> parameters = new Dictionary<string, string>();      

            //parameters.Add("act_name={0}", actName);
            //parameters.Add("client_ip={0}", ip);
            ////  parameters.Add("logo_imgurl={0}",null  );
            //parameters.Add("max_value={0}", Money.ToString());
            //parameters.Add("mch_billno={0}", billNo);
            //parameters.Add("mch_id={0}", System.Web.Configuration.WebConfigurationManager.AppSettings["mch_id"]);
            //parameters.Add("min_value={0}", Money.ToString());
            //parameters.Add("nick_name={0}", nickname);
            //parameters.Add("nonce_str={0}", tmpstr);
            //parameters.Add("re_openid={0}", OpenId);
            //parameters.Add("remark={0}", remark);
            //parameters.Add("send_name={0}", sendername);
            ////parameters.Add("share_content={0}", null);
            ////parameters.Add("share_imgurl={0}", null);
            ////parameters.Add("share_url={0}", null);
            ////parameters.Add("sub_mch_id={0}", null);
            //parameters.Add("total_amount={0}", Money.ToString());
            //parameters.Add("total_num={0}", "1");
            //parameters.Add("wishing={0}", wishing);
            //parameters.Add("wxappid={0}", System.Web.Configuration.WebConfigurationManager.AppSettings["AppID"]); 




            paramstr = param;
            var md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(paramstr, "MD5");
            //  md5 = GetMd5(paramstr).ToUpper();
            //  md5 = CreateSignString(parameters, System.Web.Configuration.WebConfigurationManager.AppSettings["AppID"]);
            // return md5;
            paramstr += "&sign=" + md5;
            var url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";

            var sb2 = new StringBuilder();
            sb2.Append("<xml>");
            sb2.AppendFormat("<act_name>{0}</act_name>", actName);
            sb2.AppendFormat("<client_ip>{0}</client_ip>", ip);
            sb2.AppendFormat("<max_value>{0}</max_value>", Money);
            sb2.AppendFormat("<mch_billno>{0}</mch_billno>", billNo);
            sb2.AppendFormat("<mch_id>{0}</mch_id>", System.Web.Configuration.WebConfigurationManager.AppSettings["mch_id"]);
            sb2.AppendFormat("<min_value>{0}</min_value>", Money);
            sb2.AppendFormat("<nick_name>{0}</nick_name>", nickname);
            sb2.AppendFormat("<nonce_str>{0}</nonce_str>", tmpstr);
            sb2.AppendFormat("<re_openid>{0}</re_openid>", OpenId);
            sb2.AppendFormat("<remark>{0}</remark>", remark);
            sb2.AppendFormat("<send_name>{0}</send_name>", sendername);
            sb2.AppendFormat("<total_amount>{0}</total_amount>", Money);
            sb2.AppendFormat("<total_num>{0}</total_num>", 1);
            sb2.AppendFormat("<wishing>{0}</wishing>", wishing);
            sb2.AppendFormat("<wxappid>{0}</wxappid>", System.Web.Configuration.WebConfigurationManager.AppSettings["AppID"]);
            sb2.AppendFormat("<key>{0}</key>", payKey);
            sb2.AppendFormat("<sign>{0}</sign>", md5);
            sb2.Append("</xml>");
            string responseString;
            getResponseCert<PayResult>(url, sb2.ToString(), out responseString);
            //responseString = PostPage(url, sb2.ToString());
            responseXML = responseString;
            return sb2.ToString();
        }


        private  string getResponseCert<T>(string url, string postData, out string responseString) where T : new()
        {

            //      //testuse
            //postData="<xml>"+
            //"<act_name>test</act_name>"+
            //"<client_ip>139.226.130.219</client_ip>"+
            //"<max_value>1000</max_value>"+
            //"<mch_billno>222</mch_billno>"+
            //"<mch_id>1230283802</mch_id>"+
            //"<min_value>1000</min_value>"+
            //"<nick_name>米嘟嘟</nick_name>"+
            //"<nonce_str>bdb76cfa032945f1a5cd2c26c83ca67d</nonce_str>" +
            //"<re_openid>oo-nWs3meUO4Bu_zEWKoZYvpcr2g</re_openid>" +
            //"<remark>米嘟嘟备注</remark>"+
            //"<send_name>米嘟嘟</send_name>"+
            //"<total_amount>1000</total_amount>"+
            //"<total_num>1</total_num>"+
            //"<wishing>米嘟嘟备注</wishing>"+
            //"<wxappid>wx8cabe7121f5369a3</wxappid>"+
            //"<key>wx8cabe7121f5369a3</key>" +
            //"<sign>B76AAC78A8BE1C04E1467C2F152B24F5</sign>" +
            //"</xml>";

            //  responseString = Send(postData, url);
            responseString = PostPage(url, postData);
            return responseString;

            // X509Certificate Cert = X509Certificate.CreateFromCertFile("C:\\网站证书\\iMidudu.cer"); //证书存放的绝对路径
            var password = "1230283802";

            X509Certificate2 Cert = new System.Security.Cryptography.X509Certificates.X509Certificate2("C:\\网站证书\\iMidudu.cer", password, X509KeyStorageFlags.MachineKeySet);


            // ServicePointManager.CertificatePolicy = new CertPolicy(); //处理来自证书服务器的错误信息
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(url);
            Request.ClientCertificates.Add(Cert);
            Request.UserAgent = "Mitchell Chu robot test"; // 使用的客户端，如果服务端没有要求可以随便填写
            Request.Method = "POST"; // 请求的方式：POST/GET 
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.
            Request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            Request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = Request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            var response = Request.GetResponse();
            Stream dataStreamres = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStreamres);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            responseString = responseFromServer;
            //  Adinnet.SEQ.interfaces.Log.Add(responseFromServer);
            try
            {

                T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseFromServer);
                //  return result;
            }
            catch (Exception ex)
            {
                //  return new T();
            }
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="data">发送拼接的参数</param>
        /// <param name="url">要发送到的链接地址</param>
        /// <returns>返回xml</returns>

        public  string Send(string data, string url)
        {
            return Send(Encoding.GetEncoding("UTF-8").GetBytes(data), url);
        }

        public  string Send(byte[] data, string url)
        {
            string cert = "D:\\网站证书\\rootca.pem";//证书存放的地址
            string password = "1230283802";//证书密码 即商户号
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            X509Certificate cer = new X509Certificate(cert, password);

            #region 该部分是关键，若没有该部分则在IIS下会报 CA证书出错
            X509Certificate2 certificate = new X509Certificate2(cert, password);
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            store.Remove(certificate);   //可省略
            store.Add(certificate);
            store.Close();

            #endregion


            Stream responseStream;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
            {
                throw new ApplicationException(string.Format("Invalid url string: {0}", url));
            }
            // request.UserAgent = sUserAgent;
            //  request.ContentType = sContentType;
            request.ClientCertificates.Add(cer);


            request.Method = "POST";
            request.ContentLength = data.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            try
            {
                responseStream = request.GetResponse().GetResponseStream();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            string str = string.Empty;
            using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
            {
                str = reader.ReadToEnd();
            }
            responseStream.Close();
            return str;
        }
        private  bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
        public  string PostPage(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...  
            try
            {
                //CerPath证书路径
                string certPath = "D:\\网站证书\\apiclient_cert.p12";
                //证书密码
                string password = "1230283802";
                X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(certPath, password, X509KeyStorageFlags.MachineKeySet);

                // 设置参数  
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.ContentLength = data.Length;
                request.ClientCertificates.Add(cert);
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据  
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求  
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码  
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;

            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }
        public class PayResult
        {
            public string return_code { get; set; }
            public string return_msg { get; set; }
        } 

        public  void FocusedUser(ref FocusedUserResponse init)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}", Token, init == null ? null : init.next_openid);
            var r = getResponse<FocusedUserResponse>(url);
            if (init == null)
            {
                init = r;
            }
            else
            {
                init.next_openid = r.next_openid;
                if (r.data != null)
                {
                    init.data.openid.AddRange(r.data.openid);
                }
            }
            if (!string.IsNullOrEmpty(init.next_openid))
            {
                FocusedUser(ref init);
            }
        }
        [WebMethod(EnableSession =true)]
        public  WXconfig Config(string Url)
        {
            //debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
            //            appId: 'wx8cabe7121f5369a3', // 必填，公众号的唯一标识
            //            timestamp: <%= WX.timestamp %>, // 必填，生成签名的时间戳
            //    nonceStr: '<%=WX.nonceStr%>', // 必填，生成签名的随机串
            //    signature: '',// 必填，签名，见附录1
            //    jsApiList: [] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
            var config = new WXconfig()
            {
                appId =
                System.Web.Configuration.WebConfigurationManager.AppSettings["AppID"],
                timestamp = Timestamp,
                debug = true,
                nonceStr = nonceStr,
                ticket = Ticket
            };

            config.jsApiList.Add("");



            String nonce_str = config.nonceStr;
            String timestamp = config.timestamp.ToString();
            String string1;

            //注意这里参数名必须全部小写，且必须有序
            string1 = "jsapi_ticket=" + config.ticket +
                      "&noncestr=" + nonce_str +
                      "&timestamp=" + timestamp +
                      "&url=" + Url;
            //string1 = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}",config.ticket,);
            ////建立SHA1对象
            //SHA1 sha = new SHA1CryptoServiceProvider();

            ////将mystr转换成byte[]
            //ASCIIEncoding enc = new ASCIIEncoding();
            //byte[] dataToHash = enc.GetBytes(string1);

            ////Hash运算
            //byte[] dataHashed = SHA1.Create().ComputeHash(dataToHash);

            ////将运算结果转换成string
            //string hash = BitConverter.ToString(dataHashed).Replace("-", "");
            config.signature = Sha1(string1);
            return config;
        }
        public  string Sha1(string orgStr, string encode = "UTF-8")
        {
            var sha1 = new SHA1Managed();
            var sha1bytes = System.Text.Encoding.GetEncoding(encode).GetBytes(orgStr);
            byte[] resultHash = sha1.ComputeHash(sha1bytes);
            string sha1String = BitConverter.ToString(resultHash).ToLower();
            sha1String = sha1String.Replace("-", "");
            return sha1String;
        }

    }


    public class WXconfig
    {

        public WXconfig()
        {
            this.jsApiList = new List<string>();
        }
        public string appId { get; set; }
        public long timestamp
        {
            get; set;
        }
        public string nonceStr { get; set; }

        public string signature { get; set; }
        public List<string> jsApiList { get; set; }
        public bool debug { get; set; }
        public string ticket { get; set; }

    }

    public class UserInfo
    {
        // public int subscribe { get; set; }
        public string openid { get; set; }
        public string nickname { get; set; }
        public int sex { get; set; }
        public string language { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        // public int subscribe_time { get; set; }
        public string unionid { get; set; }
        public List<string> privilege { get; set; }
    }

    public class TokenResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }

    public class TicketResponse
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string ticket { get; set; }
        public int expires_in { get; set; }
    }


    public class OpenIdResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
    }


    public class FocusedUserResponse
    {
        public int total { get; set; }
        public int count { get; set; }

        public FocuedUserData data { get; set; }
        public string next_openid { get; set; }
        public class FocuedUserData
        {
            public FocuedUserData()
            {
                this.openid = new List<string>();
            }
            public List<string> openid { get; set; }
        }
    }



}
