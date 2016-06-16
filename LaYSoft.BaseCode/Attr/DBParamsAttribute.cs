using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaYSoft.BaseCode.Attr
{
    /// <summary>
    /// 数据字段属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DBParamsAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strFieldName">字段名</param>
        public DBParamsAttribute(string strSearchSQL)
        {
            this.SearchSQL = strSearchSQL;
        }

        #region 属性
        /// <summary>
        /// 查询语句
        /// </summary>
        public string SearchSQL { get; set; }

        #endregion
    }
}
