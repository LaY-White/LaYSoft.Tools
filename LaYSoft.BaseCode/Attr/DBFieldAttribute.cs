using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LaYSoft.BaseCode.Attr
{
    /// <summary>
    /// 数据字段属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DBFieldAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strFieldName">字段名</param>
        public DBFieldAttribute(string strFieldName)
        {
            this.FieldName = strFieldName;
            this.DBType = SqlDbType.NVarChar;
            this.IsPrimaryKey = false;
            this.IsForeignKey = false;
            this.IsUniqueness = false;
            this.IsIdentity = false;
        }

        public DBFieldAttribute() : this(null) { }

        #region 属性
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public SqlDbType DBType { get; set; }

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 是否外键(用于树结构删除)
        /// </summary>
        public bool IsForeignKey { get; set; }

        /// <summary>
        /// 是否唯一
        /// </summary>
        public bool IsUniqueness { get; set; }

        /// <summary>
        /// 是否自增
        /// </summary>
        public bool IsIdentity { get; set; } 
        #endregion
    }
}
