using LaYSoft.BaseCode;
using LaYSoft.BaseCode.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LaYSoft.Tools.Model.System
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [DBTable("Sys_UserInfo")]
    public class UserInfo : BaseModel
    {
        #region 属性

        ///<sumary>
        /// 用户ID
        ///</sumary>
        [DBField(IsPrimaryKey = true, IsIdentity = true)]
        public int UserID { get; set; }

        ///<sumary>
        /// 用户编号
        ///</sumary>
        [DBField]
        public String UserCode { get; set; }

        ///<sumary>
        /// 姓名
        ///</sumary>
        [DBField]
        public String UserName { get; set; }

        ///<sumary>
        /// 密码
        ///</sumary>
        [DBField]
        public String Password { get; set; }

        #endregion
    }
}
