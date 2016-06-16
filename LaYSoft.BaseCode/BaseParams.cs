using LaYSoft.BaseCode.Attr;
using LaYSoft.BaseCode.DBClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LaYSoft.BaseCode
{
    public abstract class BaseParams
    {
        #region 属性
        /// <summary>
        /// Sql里面的最顶层的数据
        /// </summary>
        public Int32? Top { get; set; }

        /// <summary>
        /// 排序条件
        /// </summary>
        public String OrderBy { get; set; }

        /// <summary>
        /// 开始行
        /// </summary>
        public Int32? BeginRow { get; set; }

        /// <summary>
        /// 结束行 
        /// </summary>
        public Int32? EndRow { get; set; }

        /// <summary>
        /// 禁止查看的属性序列
        /// </summary>
        public List<String> DisablePropertys { get; set; }

        /// <summary>
        /// 自定义查询条件
        /// </summary>
        public String SqlWhere { get; set; }

        /// <summary>
        /// 自定义查询条件的参数
        /// </summary>
        public List<LaYSoftParameter> SqlWherePa { get; set; }


        /// <summary>
        /// 创建人ID
        /// </summary>
        [DBParams("CreateUserID = @CreateUserID")]
        public String CreateUserID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DBParams("CreateTime = @CreateTime")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建时间开始
        /// </summary>
        [DBParams("CreateTime >= @CreateTimeFrom")]
        public DateTime? CreateTimeFrom { get; set; }

        /// <summary>
        /// 创建时间结束
        /// </summary>
        [DBParams("CreateTime <= @CreateTimeTo")]
        public DateTime? CreateTimeTo { get; set; }


        /// <summary>
        /// 最后修改人ID
        /// </summary>
        [DBParams("ModifyUserID = @ModifyUserID")]
        public String ModifyUserID { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [DBParams("ModifyTime = @ModifyTime")]
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 最后修改时间开始
        /// </summary>
        [DBParams("ModifyTime >= @ModifyTimeFrom")]
        public DateTime? ModifyTimeFrom { get; set; }

        /// <summary>
        /// 最后修改时间结束
        /// </summary>
        [DBParams("ModifyTime <= @ModifyTimeTo")]
        public DateTime? ModifyTimeTo { get; set; }

        #endregion

        #region 方法

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns></returns>
        public Object Trim()
        {
            return ModelToTrim(this);
        }

        /// <summary>
        /// Model转化为字符串的方法
        /// </summary>
        /// <param name="model">要转化的Model</param>
        /// <returns></returns>
        private static Object ModelToTrim(Object model)
        {
            // TODO:
            // 扩展：需考虑实体中又包含其他实体的情况？


            object objValue = null;
            int i = 0;
            if (model != null)
            {
                foreach (PropertyInfo property in model.GetType().GetProperties())
                {
                    objValue = property.GetValue(model, null);
                    if (objValue != null)
                    {

                        try
                        {
                            model.GetType().GetProperty(property.Name).SetValue(model, objValue.ToString().Trim(), null);
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

        #endregion
    }
}
