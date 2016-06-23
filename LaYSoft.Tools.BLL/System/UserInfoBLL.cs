using LaYSoft.BaseCode;
using LaYSoft.Tools.DAL.System;
using LaYSoft.Tools.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaYSoft.Tools.BLL.System
{
    public class UserInfoBLL : BaseBLL<UserInfo>
    {
        /// <summary>
        /// [不推荐](请使用CurDaoInstance)
        /// </summary>
        protected override BaseDAO<UserInfo> BaseDaoInstance
        {
            get
            {
                return MyFactory<UserInfoDAL>.CreateCurSystemClass();
            }
        }
    }
}
