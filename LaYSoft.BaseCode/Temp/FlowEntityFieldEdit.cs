using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;
using TinyStack.Data.DB;
using TinyStack.Data.Utility;


namespace TinyStack.Modules.Sample.DataModels
{

    [TinyStack.Data.DBTable("FlowEntityFieldEdit")]
    public class FlowEntityFieldEdit : TinyStack.Data.DataModel
    {
        #region 属性
        /// <summary>
        /// 主键ID
        /// </summary>
        [TinyStack.Data.DBField(IsPrimaryKey = true)]
        public string ID { get; set; }

        /// <summary>
        /// 实体编号，关联EntityInfo表
        /// </summary>
        [TinyStack.Data.DBField]
        public string EntityID { get; set; }

        /// <summary>
        /// 字段标识
        /// </summary>
        [TinyStack.Data.DBField]
        public string Name { get; set; }

        /// <summary>
        /// 字段中文名称
        /// </summary>
        [TinyStack.Data.DBField]
        public string CnName { get; set; }

        /// <summary>
        /// 数据类型   
        /// 1 - 字符串
        /// 2 - 整数
        /// 3 - 小数
        /// 4 - 日期
        /// 5 - 时间
        /// 6- 枚举
        /// 7 -布尔值
        /// </summary>
        [TinyStack.Data.DBField]
        public int DataType { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        [TinyStack.Data.DBField]
        public string MaxValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        [TinyStack.Data.DBField]
        public string MinValue { get; set; }

        /// <summary>
        /// 枚举类型
        /// </summary>
        [TinyStack.Data.DBField]
        public int EnumType { get; set; }

        /// <summary>
        /// 枚举关联编号
        /// </summary>
        [TinyStack.Data.DBField]
        public string EnumID { get; set; }

        /// <summary>
        /// 枚举关联字段编号
        /// </summary>
        [TinyStack.Data.DBField]
        public string EnumFieldID { get; set; }

        /// <summary>
        /// 当数据类型为字符串时
        ///0表示单行
        ///1表示双行
        /// </summary>
        [TinyStack.Data.DBField]
        public bool IsMultiply { get; set; }

        /// <summary>
        /// 是否唯一
        /// </summary>
        [TinyStack.Data.DBField]
        public bool IsUnique { get; set; }

        /// <summary>
        /// 是否为查询条件
        /// </summary>
        [TinyStack.Data.DBField]
        public bool IsCondictionOfSelect { get; set; }
        /// <summary>
        /// 是否可空
        /// </summary>
        [TinyStack.Data.DBField]
        public bool CanNull { get; set; }
        /// <summary>
        ///查询列表显示
        /// </summary>
        [TinyStack.Data.DBField]
        public bool IsList { get; set; }
        /// <summary>
        /// 支持排序
        /// </summary>
        [TinyStack.Data.DBField]
        public bool CanSort { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        [TinyStack.Data.DBField]
        public string DefaultValue { get; set; }

        /// <summary>
        /// 有效性验证规则
        /// </summary>
        [TinyStack.Data.DBField]
        public string ValidationRule { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [TinyStack.Data.DBField]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 排序值，默认为0，值越大，越靠前
        /// </summary>
        [TinyStack.Data.DBField]
        public int SortValue { get; set; }

        /// <summary>
        /// 最大精度值
        /// </summary>
        [TinyStack.Data.DBField]
        public int MaxPrecision { get; set; }

        /// <summary>
        /// 加入时间
        /// </summary>
        [TinyStack.Data.DBField]
        public DateTime CreateTime { get; set; }


        #endregion

        #region 查询

        /// <summary>
        /// 根据EntityID外键获取FlowEntityFieldEdit
        /// </summary>
        /// <param name="EntityID">实体ID</param>
        /// <returns></returns>
        public static List<FlowEntityFieldEdit> GetEntityFieldList(string EntityID,bool allFlag = false)
        {
            List<FlowEntityFieldEdit> list = new List<FlowEntityFieldEdit>();
            using (SqlCommand cmd = new SqlCommand())
            {

                string sql = "select * from FlowEntityFieldEdit where EntityID=@EntityID " + (allFlag ? "" : " and IsDeleted =0 ") + " order by IsDeleted asc,SortValue desc,CreateTime asc";
                
                cmd.Parameters.Add("@EntityID", SqlDbType.VarChar).Value = EntityID;
                cmd.CommandText = sql;
                DataSet ds = DBManager.MainDB.ExecuteDataSet(cmd);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        list.Add(item.ToObject<FlowEntityFieldEdit>());
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// 逻辑删除实体字段
        /// </summary>
        /// <param name="EnumID"></param>
        /// <returns></returns>
        public static bool DeleteByEntityID(string EntityID)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                //获取与枚举关联的枚举值
                string sql = "UPDATE FlowEntityFieldEdit SET IsDeleted=1 where EntityID=@EntityID";
                cmd.Parameters.Add("@EntityID", SqlDbType.VarChar).Value = EntityID;
                cmd.CommandText = sql;
                return DBManager.MainDB.ExecuteNonQuery(cmd) > 0;
            }
        }


 
        #endregion
    }
}
