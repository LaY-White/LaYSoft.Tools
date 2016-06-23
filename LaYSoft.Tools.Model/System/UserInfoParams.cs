using LaYSoft.BaseCode;
using LaYSoft.BaseCode.Attr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaYSoft.Tools.Model.System
{
    public class UserInfoParams : BaseParams
    {
        public UserInfoParams()
        {
            this.OrderBy = "UserID DESC";
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DBParams("UserID = @UserID")]
        public int? UserID { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [DBParams("UserCode like '%' + @UserCode + '%'")]
        public String UserCode { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DBParams("UserName like '%' + @UserName + '%'")]
        public String UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DBParams("Password = @Password")]
        public String Password { get; set; }
    }
}
