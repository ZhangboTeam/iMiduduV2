using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMidudu.Biz
{
    public class WXUserBiz
    {
        public void SaveWXUser(iMidudu.Model.WXUser user)
        {
            //insert or update
            //存在吗?
            var dal = new iMidudu.Data.WXUserDAL();
            var exists = dal.ExistsByOpenid(user.OpenId);
            if (exists)
            {
                //update
                var olduser = dal.SelectById(user.OpenId);
                olduser.NickName = user.NickName;
                olduser.Pic = user.Pic;
                olduser.Sex = user.Sex;
                olduser.WXCity = user.WXCity;
                olduser.WXProvince = user.WXProvince;
                olduser.WXCountry = user.WXCountry;
                //olduser.RegisterDate = DateTime.Now ;
                olduser.LastActiveTime = DateTime.Now;
                dal.UpdateWXUser(olduser);
            }
            else
            {
                //insert
                user.RegisterDate = DateTime.Now;
                user.LastActiveTime = DateTime.Now;
                dal.InsertWXUser(user);
            }

        }
    }
}
