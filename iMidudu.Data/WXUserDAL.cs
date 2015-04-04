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
    }
}
