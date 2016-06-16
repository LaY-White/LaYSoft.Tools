using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaYSoft.BaseCode.Attr
{
    /// <summary>
    /// 数据表属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class DBTableAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strTableName">表名</param>
        public DBTableAttribute(string strTableName)
        {
            this.TableName = strTableName;
        }

        /// <summary>
        /// 构造函数，
        /// </summary>
        public DBTableAttribute() : this(null) { }

        #region 属性

        /// <summary>
        /// 数据库名
        /// </summary>
        public string DBName { get; set; }

        /// <summary>
        /// 数据库表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 实体类对应的资源ID
        /// </summary>
        public string ResourceID { get; set; }

        #endregion
    }
}
