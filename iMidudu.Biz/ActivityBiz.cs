using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMidudu.Biz
{
   public class ActivityBiz
    {
        public string GetUrlOfCode(Guid Code)
        {
            var model = new Data.ActivityDAL().SelectByCode(Code);
            if (model == null)
            {
                return string.Empty;
            }
            else
            {
                return model.URL;
            }
        }
    }
   
}
