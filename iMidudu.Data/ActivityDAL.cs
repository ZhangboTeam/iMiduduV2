using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMidudu.Data
{
    public class ActivityDAL
    {
        public Model.Activity SelectByCode(Guid Code)
        {
            var dr = SuuSee.Data.SqlHelper.ExecuteReaderStoredProcedure("Activity_SelectProcedure", new System.Data.SqlClient.SqlParameter("@QRCode", Code));
            Model.Activity model = null;
            while (dr.Read())
            {
                model = new Model.Activity()
                {
                    ActivityName = dr["ActivityName"].ToString(),
                    URL = dr["URL"].ToString(),
                    QRCode = (Guid)dr["QRCode"]
                };
            }
            return model;
        }
    }
}
