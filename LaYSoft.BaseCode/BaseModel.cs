using LaYSoft.BaseCode.Attr;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LaYSoft.BaseCode
{
    [Serializable]
    public abstract class BaseModel
    {
        #region 属性

        ///<sumary>
        /// 创建人用户ID
        ///</sumary>
        [Description("创建人用户ID")]
        [DBField("CreateUserID")]
        public String CreateUserID { get; set; }

        ///<sumary>
        /// 修改人用户ID
        ///</sumary>
        [Description("修改人用户ID")]
        [DBField("ModifyUserID")]
        public String ModifyUserID { get; set; }

        ///<sumary>
        /// 创建时间
        ///</sumary>
        [Description("创建时间")]
        [DBField("CreateTime")]
        public DateTime CreateTime { get; set; }

        ///<sumary>
        /// 修改时间
        ///</sumary>
        [Description("修改时间")]
        [DBField("ModifyTime")]
        public DateTime ModifyTime { get; set; }

        #endregion

        #region 方法

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return ModelToString(this);
        }

        /// <summary>
        /// Model转化为字符串的方法
        /// </summary>
        /// <param name="model">要转化的Model</param>
        /// <returns></returns>
        private static String ModelToString(object model)
        {
            // TODO:
            // 扩展：需考虑实体中又包含其他实体的情况？
            StringBuilder sb = new StringBuilder();

            object objValue = null;

            if (model != null)
            {
                foreach (PropertyInfo property in model.GetType().GetProperties())
                {
                    objValue = property.GetValue(model, null);
                    if (objValue != null)
                    {
                        sb.AppendFormat("{0}【{1}】；", property.Name as String, objValue ?? string.Empty);
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            return sb.ToString();
        }

        #endregion

        
    }
}
