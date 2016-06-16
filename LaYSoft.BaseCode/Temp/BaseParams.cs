/***************************************************************************
 * 
 *       功能：     参数实体类基类，把共性的参数属性抽象出来
 *       作者：     李松添
 *       日期：     2009/12/30
 * 
 *       修改日期：  
 *       修改人 ： 
 *       修改内容：
 * *************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace DGDP.Qtone.BaseCore.Params
{
    public abstract class BaseParams
    {
        /// <summary>
        /// 抽取表名属性，必须实现
        /// </summary>
        public abstract string TableName
        {
            get;
            set;
        }

        private object _id;
        /// <summary>
        /// 主键ID
        /// </summary>
        public object ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _top;
        /// <summary>
        /// Sql里面的最顶层的数据
        /// </summary>
        public string Top
        {
            get
            {
                return _top;
            }
            set { _top = value; }
        }

        

        /// <summary>
        /// 排序条件
        /// </summary>
        private string _orderBy;
        /// <summary>
        /// 排序条件
        /// </summary>
        public string OrderBy
        {
            get
            {
                if (string.IsNullOrEmpty(_orderBy))
                {
                    return "1";
                }
                else
                {
                    return _orderBy;
                }
            }
            set { _orderBy = value; }
        }

        private string _beginRow;

        /// <summary>
        /// 开始行
        /// </summary>
        public string BeginRow
        {
            get { return _beginRow; }
            set { _beginRow = value; }
        }

        private string _endRow;

        /// <summary>
        /// 结束行 
        /// </summary>
        public string EndRow
        {
            get { return _endRow; }
            set { _endRow = value; }
        }

        private string _createUserID;
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string CreateUserID
        {
            get { return _createUserID; }
            set { _createUserID = value; }
        }

        private string _createUserIDFrom;
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string CreateUserIDFrom
        {
            get { return _createUserIDFrom; }
            set { _createUserIDFrom = value; }
        }

        private string _createUserIDTo;
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string CreateUserIDTo
        {
            get { return _createUserIDTo; }
            set { _createUserIDTo = value; }
        }


        private DateTime? _createTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        private DateTime? _createTimeFrom;
        /// <summary>
        /// 创建时间开始
        /// </summary>
        public DateTime? CreateTimeFrom
        {
            get { return _createTimeFrom; }
            set { _createTimeFrom = value; }
        }

        private DateTime? _createTimeTo;
        /// <summary>
        /// 创建时间结束
        /// </summary>
        public DateTime? CreateTimeTo
        {
            get
            {
                if(_createTimeTo.HasValue)
                {
                    _createTimeTo.Value.Date.AddDays(1).AddSeconds(-1);
                }

                return _createTimeTo;
            }
            set { _createTimeTo = value; }
        }

        private string _modifyUserID;
        /// <summary>
        /// 最后修改人ID
        /// </summary>
        public string ModifyUserID
        {
            get { return _modifyUserID; }
            set { _modifyUserID = value; }
        }


        private string _modifyUserIDFrom;
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string ModifyUserIDFrom
        {
            get { return _modifyUserIDFrom; }
            set { _modifyUserIDFrom = value; }
        }

        private string _modifyUserIDTo;
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string ModifyUserIDTo
        {
            get { return _modifyUserIDTo; }
            set { _modifyUserIDTo = value; }
        }


        private DateTime? _modifyTime;
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? ModifyTime
        {
            get { return _modifyTime; }
            set { _modifyTime = value; }
        }


        private DateTime? _modifyTimeFrom;
        /// <summary>
        /// 最后修改时间开始
        /// </summary>
        public DateTime? ModifyTimeFrom
        {
            get { return _modifyTimeFrom; }
            set { _modifyTimeFrom = value; }
        }


        private DateTime? _modifyTimeTo;
        /// <summary>
        /// 最后修改时间结束
        /// </summary>
        public DateTime? ModifyTimeTo
        {
            get
            {
                if (_modifyTimeTo.HasValue)
                {
                    _modifyTimeTo.Value.Date.AddDays(1).AddSeconds(-1);
                }

                return _modifyTimeTo;
            }
            set { _modifyTimeTo = value; }
        }

        private string _sqlWhere;
        /// <summary>
        /// 查询条件
        /// </summary>
        public string SqlWhere
        {
            get { return _sqlWhere; }
            set { _sqlWhere = value; }
        }

        private string _DisablePropertys;
        /// <summary>
        /// 禁用的属性序列，以英文逗号分隔
        /// </summary>
        public string DisablePropertys
        {
            get { return _DisablePropertys; }
            set { _DisablePropertys = value; }
        }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns></returns>
        public object  Trim()
        {
             return ModelToTrim(this);
        }

        /// <summary>
        /// Model转化为字符串的方法
        /// </summary>
        /// <param name="model">要转化的Model</param>
        /// <returns></returns>
        private static object ModelToTrim(object model)
        {
            // TODO:
            // 扩展：需考虑实体中又包含其他实体的情况？
           

            object objValue = null;
            int i=0;
            if (model != null)
            {
                foreach (PropertyInfo property in model.GetType().GetProperties())
                {
                    objValue = property.GetValue(model, null);
                    if (objValue != null)
                    {

                        try
                        {
                           model.GetType().GetProperty(property.Name).SetValue(model,objValue.ToString().Trim(), null);
                        }
                        catch
                        {
                        }
                      
                    }
                    i++;
                }
            }

            return model;
        }

    }
}
