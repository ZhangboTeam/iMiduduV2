using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMidudu.Biz
{
    public class PrizeBiz
    {
        //public void InsertPrize(iMidudu.Model.Prize prize)
        //{
        //    //insert or update
        //    var dal = new iMidudu.Data.PrizeDAL();
        //    dal.InsertPrize(prize);
        //}
        public void UpdatePrize(Guid PrizeId, string Quantity)
        {
            //insert or update
            var dal = new iMidudu.Data.PrizeDAL();
            dal.UpdatePrize(PrizeId, Quantity);
        }
    }
}

