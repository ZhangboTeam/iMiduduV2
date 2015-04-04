using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMidudu.Data
{
    public class WXUserDAL
    {
        public bool ExistsByOpenid(string openid)
        {
            
          var dr = SuuSee.Data.SqlHelper.ExecuteReaderStoredProcedure("WXUser_SelectProcedure",
                new System.Data.SqlClient.SqlParameter("@OpenId", openid));
          while (dr.Read())
          {
              return true;
          }
          return false;
        }

        public Model.WXUser SelectById(string OpenId)
        {
            var dr = SuuSee.Data.SqlHelper.ExecuteReaderStoredProcedure("WXUser_SelectProcedure",
                new System.Data.SqlClient.SqlParameter("@OpenId", OpenId));
            Model.WXUser model = null;
            while (dr.Read())
            {
                model = new Model.WXUser()
                { OpenId = dr["OpenId"].ToString(),
                 LastActiveTime= (DateTime)dr["LastActiveTime"],
                  NickName= dr["NickName"].ToString(),
                   Pic= dr["Pic"].ToString(),
                    RegisterDate= (DateTime)dr["RegisterDate"],
                     Sex=(bool)dr["Sex"],
                      WXCity= dr["WXCity"].ToString(),
                       WXCountry= dr["WXCountry"].ToString(),
                        WXProvince = dr["WXProvince"].ToString()
                };
            }
            return model;
        }

        public void InsertWXUser(iMidudu.Model.WXUser user)
        {
            //insert
            SuuSee.Data.SqlHelper.ExecteNonQueryStoredProcedure("WXUser_InsertProcedure",
                new System.Data.SqlClient.SqlParameter("@OpenId", user.OpenId),
                new System.Data.SqlClient.SqlParameter("@NickName", user.NickName),
                new System.Data.SqlClient.SqlParameter("@Pic", user.Pic),
                new System.Data.SqlClient.SqlParameter("@Sex", user.Sex),
                new System.Data.SqlClient.SqlParameter("@WXCity", user.WXCity),
                new System.Data.SqlClient.SqlParameter("@WXProvince", user.WXProvince),
                new System.Data.SqlClient.SqlParameter("@WXCountry", user.WXCountry),
                new System.Data.SqlClient.SqlParameter("@RegisterDate", user.RegisterDate),
                new System.Data.SqlClient.SqlParameter("@LastActiveTime", user.LastActiveTime));

        }
        public void UpdateWXUser(iMidudu.Model.WXUser user)
        {
            //insert
            SuuSee.Data.SqlHelper.ExecteNonQueryStoredProcedure("WXUser_UpdateProcedure",
                new System.Data.SqlClient.SqlParameter("@OpenId", user.OpenId),
                new System.Data.SqlClient.SqlParameter("@NickName", user.NickName),
                new System.Data.SqlClient.SqlParameter("@Pic", user.Pic),
                new System.Data.SqlClient.SqlParameter("@Sex", user.Sex),
                new System.Data.SqlClient.SqlParameter("@WXCity", user.WXCity),
                new System.Data.SqlClient.SqlParameter("@WXProvince", user.WXProvince),
                new System.Data.SqlClient.SqlParameter("@WXCountry", user.WXCountry),
                new System.Data.SqlClient.SqlParameter("@RegisterDate",user.RegisterDate),
                new System.Data.SqlClient.SqlParameter("@LastActiveTime", user.LastActiveTime));

        }
    }
}
