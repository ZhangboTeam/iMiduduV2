using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMidudu.Biz
{
    public class PrizeBiz
    {
        public void UpdatePrize(Guid PrizeId, string Quantity)
        {
            //insert or update
            var dal = new iMidudu.Data.PrizeDAL();
            dal.UpdatePrize(PrizeId, Quantity);
        }
        public iMidudu.Model.Prize[] SelectPrizeByQRCode(Guid QRCode)
        {
            //insert or update
            var dal = new iMidudu.Data.PrizeDAL();
            return dal.SelectPrizeByQRCode(QRCode); ;
        }
    }
}

