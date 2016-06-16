using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;
using System.Collections.Specialized;
using System.Web;
using TinyStack.Data.DB;
using TinyStack.Data.Utility;

namespace TinyStack.Data
{
    /// <summary>
    /// 数据模型
    /// </summary>
    public class DataModel
    {
        #region 属性
        /// <summary>
        /// 数据库名（数据库连接字符串中配置的名称）
        /// </summary>
        public string DBName
        {
            get
            {
                return this.GetDBName();
            }
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        public string TableName
        {
            get
            {
                return this.GetTabelName();
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取数据库名（数据库连接字符串中配置的名称）
        /// </summary>
        /// <returns></returns>
        private string GetDBName()
        {
            DBTableAttribute oDBField = this.GetDBTableAttribute();
            if (!string.IsNullOrEmpty(oDBField.DBName))
                return oDBField.DBName;
            else
                return "MainDbContext";
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <returns></returns>
        private string GetTabelName()
        {
            DBTableAttribute oDBField = this.GetDBTableAttribute();
            if (!string.IsNullOrEmpty(oDBField.TableName))
                return oDBField.TableName;
            else
                throw new Exception("未设置对像\"DBTable\"特性对应的表名，请在特性\"DBTable\"特性中设置表名！");
        }

        /// <summary>
        /// 获取DBTable特性
        /// </summary>
        /// <returns></returns>
        private DBTableAttribute GetDBTableAttribute()
        {
            object[] objs = this.GetType().GetCustomAttributes(typeof(DBTableAttribute), true);
            if (objs.Length > 0)
                return objs[0] as DBTableAttribute;
            else
                throw new Exception("未设置对象的\"DBTable\"特性，请在声明类的上方添加\"DBTable\"特性！");
        }
        #endregion

        #region 数据操作
        /// <summary>
        /// 加载数据
        /// </summary>
        public virtual void Load()
        {
            DataSet ds = DBManager.DBSettings[this.DBName].ExecuteDataSet(this.GetSelectSqlCommand());
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                ds.Tables[0].Rows[0].ToObject<DataModel>(this);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <returns></returns>
        public virtual bool Insert(string strResource = "")
        {
            string strNameFieldValue = null; //名称字段值
            bool bIsSucceed = DBManager.DBSettings[this.DBName].ExecuteNonQuery(this.GetInsertSqlCommand(strResource, out strNameFieldValue)) > 0;
            return bIsSucceed;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <returns></returns>
        public virtual bool Update(string strResource = "")
        {
            string strNameFieldValue = null; //名称字段值
            bool bIsSucceed = DBManager.DBSettings[this.DBName].ExecuteNonQuery(this.GetUpdateSqlCommand(strResource, out strNameFieldValue)) > 0;
            return bIsSucceed;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public virtual bool Delete(string strResource = "")
        {
            string strNameFieldValue = null; //名称字段值
            bool bIsSucceed = DBManager.DBSettings[this.DBName].ExecuteNonQuery(this.GetDeleteSqlCommand(strResource, out strNameFieldValue)) > 0;
            return bIsSucceed;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="strResource">资源</param>
        /// <param name="strPrimaryKey">主键名(多个用逗号分隔)</param>
        /// <param name="dcValues">字典(key是跟主键名的位置对应,多个用逗号分隔)</param>
        /// <returns></returns>
        public bool BatchDelete(string strResource, string strPrimaryKey, Dictionary<string, string> dcValues)
        {
            List<DataModel> lModels = this.GetModelList(strPrimaryKey, dcValues);
            //执行批量删除
            string strRecords = "";
            bool bIsSucceed = lModels.BatchDelete(out strRecords);
            return bIsSucceed;
        }


        /// <summary>
        /// 获取实体类列表
        /// </summary>
        /// <param name="strPrimaryKey"></param>
        /// <param name="dcValues"></param>
        /// <returns></returns>
        private List<DataModel> GetModelList(string strPrimaryKey, Dictionary<string, string> dcValues)
        {
            List<DataModel> lModels = new List<DataModel>();
            string[] aPrimarykeys = strPrimaryKey.Split(','); //分解主键字段名
            int nPKIndex = -1; //查找主键所在的位置
            foreach (string strPkValues in dcValues.Keys)
            {
                //动态创建对像
                DataModel oModel = Activator.CreateInstance(this.GetType()) as DataModel;
                string[] aPKValues = strPkValues.Split(',');
                //对象赋值
                PropertyInfo[] oPropertyInfos = this.GetType().GetProperties();
                int nSetPropertyValueCount = 0; //记录设置值的次数(以减少循环次数)
                bool bSetNameValue = false;
                foreach (PropertyInfo oInfo in oPropertyInfos)
                {
                    nPKIndex = -1; //初始化开始值
                    object[] objs = oInfo.GetCustomAttributes(typeof(DBFieldAttribute), true);
                    if (objs.Length > 0)
                    {
                        //属性定义了特性,查找主键位置
                        DBFieldAttribute oDBField = objs[0] as DBFieldAttribute;
                        //设置名称字段值
                        if (oDBField.IsLogField)
                        {
                            oInfo.SetValue(oModel, Utility.ExtensionMethods.ConvertToPropertyType(dcValues[strPkValues], oInfo.PropertyType), null);
                            bSetNameValue = true;
                        }
                        else
                        {
                            string strFieldName = !string.IsNullOrEmpty(oDBField.FieldName) ? oDBField.FieldName : oInfo.Name;
                            //查找主键的位置
                            for (int j = 0; j < aPrimarykeys.Length; j++)
                            {
                                if (aPrimarykeys[j].Trim().ToLower() == strFieldName.Trim().ToLower())
                                {
                                    nPKIndex = j;
                                    break;
                                }
                            }

                            //找到匹配的主键
                            if (nPKIndex > -1)
                            {
                                oInfo.SetValue(oModel, Utility.ExtensionMethods.ConvertToPropertyType(aPKValues[nPKIndex], oInfo.PropertyType), null);
                                nSetPropertyValueCount++;
                            }
                        }
                    }

                    //已设置全部的主键值和名称值,就跳出循环,减少循环次数
                    if (nSetPropertyValueCount >= aPrimarykeys.Length && bSetNameValue)
                        break;
                }

                lModels.Add(oModel);
            }

            return lModels;
        }
        #endregion

        #region 获取查询SqlCommand
        /// <summary>
        /// 获取查询SqlCommand
        /// </summary>
        /// <returns></returns>
        private SqlCommand GetSelectSqlCommand()
        {
            StringBuilder sbField = new StringBuilder(); //查询字段
            StringBuilder sbFilter = new StringBuilder(); //过滤条件
            SqlCommand cmd = new SqlCommand();

            PropertyInfo[] oPropertyInfos = this.GetType().GetProperties();
            if (oPropertyInfos.Length > 0)
            {
                foreach (PropertyInfo proInfo in oPropertyInfos)
                {
                    object[] objs = proInfo.GetCustomAttributes(typeof(DBFieldAttribute), true);
                    if (objs != null && objs.Length > 0)
                    {
                        DBFieldAttribute oDBField = objs[0] as DBFieldAttribute;
                        string strFieldName = !string.IsNullOrEmpty(oDBField.FieldName) ? oDBField.FieldName : proInfo.Name;
                        if (oDBField.IsPrimaryKey == true)
                        {
                            //主键,做为过滤条件
                            if (sbFilter.Length > 0)
                                sbFilter.Append(" AND ");
                            sbFilter.AppendFormat("[{0}]=@{0}", strFieldName);
                            cmd.Parameters.Add(string.Format("@{0}", strFieldName), oDBField.DBType).Value = proInfo.GetValue(this, null);
                        }
                        else
                        {
                            //查询字段
                            if (sbField.Length > 0)
                                sbField.Append(", ");
                            sbField.AppendFormat("[{0}]", strFieldName);
                        }
                    }
                }
            }
            if (sbFilter.Length == 0)
                throw new Exception("对象未设置主键！");

            cmd.CommandText = string.Format("SELECT {0} FROM {1} WHERE {2};", sbField.ToString(), this.TableName, sbFilter.ToString());
            return cmd;
        }
        #endregion

        #region 获取查询Sql
        /// <summary>
        /// 获取查询Sql
        /// </summary>
        /// <returns></returns>
        public string GetSelectSql()
        {
            StringBuilder sbField = new StringBuilder(); //查询字段
            StringBuilder sbFilter = new StringBuilder(); //过滤条件

            PropertyInfo[] oPropertyInfos = this.GetType().GetProperties();
            if (oPropertyInfos.Length > 0)
            {
                foreach (PropertyInfo proInfo in oPropertyInfos)
                {
                    object[] objs = proInfo.GetCustomAttributes(typeof(DBFieldAttribute), true);
                    if (objs != null && objs.Length > 0)
                    {
                        DBFieldAttribute oDBField = objs[0] as DBFieldAttribute;
                        string strFieldName = !string.IsNullOrEmpty(oDBField.FieldName) ? oDBField.FieldName : proInfo.Name;
                        if (oDBField.IsPrimaryKey == true)
                        {
                            //主键,做为过滤条件
                            if (sbFilter.Length > 0)
                                sbFilter.Append(" AND ");
                            sbFilter.AppendFormat("[{0}]='{1}'", strFieldName, proInfo.GetValue(this, null));
                        }
                        else
                        {
                            //查询字段
                            if (sbField.Length > 0)
                                sbField.Append(", ");
                            sbField.AppendFormat("[{0}]", strFieldName);
                        }
                    }
                }
            }
            if (sbFilter.Length == 0)
                throw new Exception("对象未设置主键！");

            return string.Format("SELECT {0} FROM {1} WHERE {2};", sbField.ToString(), this.TableName, sbFilter.ToString());
        }
        #endregion

        #region 获取插入SqlCommand
        /// <summary>
        /// 获取插入SqlCommand
        /// </summary>
        /// <param name="strResource">资源</param>
        /// <param name="strNameFieldValue">记录</param>
        /// <returns></returns>
        private SqlCommand GetInsertSqlCommand(string strResource, out string strNameFieldValue)
        {
            StringBuilder sbField = new StringBuilder(); //查询字段
            StringBuilder sbValue = new StringBuilder(); //值参数
            strNameFieldValue = null; //名称字段值
            SqlCommand cmd = new SqlCommand();
            //唯一值验证
            bool bCheckValueUnique = false;
            StringBuilder sbCheckUnique = new StringBuilder();


            PropertyInfo[] oPropertyInfos = this.GetType().GetProperties();
            if (oPropertyInfos.Length > 0)
            {
                foreach (PropertyInfo proInfo in oPropertyInfos)
                {
                    object[] objs = proInfo.GetCustomAttributes(typeof(DBFieldAttribute), true);
                    if (objs != null && objs.Length > 0)
                    {
                        DBFieldAttribute oDBField = objs[0] as DBFieldAttribute;
                        string strFieldName = !string.IsNullOrEmpty(oDBField.FieldName) ? oDBField.FieldName : proInfo.Name;
                        object oValue = proInfo.GetValue(this, null);
                        //记录名称字段值
                        if (oDBField.IsLogField)
                            strNameFieldValue = Convert.ToString(oValue);

                        //检查值的唯一性
                        if (oDBField.IsPrimaryKey || oDBField.CheckValueUnique)
                        {
                            if (sbCheckUnique.Length > 0) sbCheckUnique.Append(" AND ");
                            if (oDBField.IsPrimaryKey)
                                sbCheckUnique.AppendFormat("[{0}] <> @CVU{0}", strFieldName);
                            else
                            {
                                bCheckValueUnique = true;
                                sbCheckUnique.AppendFormat("[{0}] = @CVU{0}", strFieldName);
                            }
                            cmd.Parameters.Add(string.Format("@CVU{0}", strFieldName), oDBField.DBType).Value = oValue;
                        }

                        //只插入不为NULL的字段
                        if (oValue != null)
                        {
                            //查询字段, 值参数
                            if (sbField.Length > 0)
                            {
                                sbField.Append(", ");
                                sbValue.Append(", ");
                            }

                            sbField.AppendFormat("[{0}]", strFieldName);
                            sbValue.Append(string.Format("@{0}", strFieldName));
                            cmd.Parameters.Add(string.Format("@{0}", strFieldName), oDBField.DBType).Value = oValue;
                        }
                    }
                }
            }
            if (bCheckValueUnique)
                cmd.CommandText = string.Format(@"IF(NOT EXISTS(SELECT * FROM {0} WHERE {3}))
                    INSERT INTO {0} ({1}) VALUES ({2});", this.TableName, sbField.ToString(), sbValue.ToString(), sbCheckUnique.ToString());
            else
                cmd.CommandText = string.Format("INSERT INTO {0} ({1}) VALUES ({2});", this.TableName, sbField.ToString(), sbValue.ToString());
            return cmd;
        }
        #endregion

        #region 获取插入Sql
        /// <summary>
        /// 获取插入Sql
        /// </summary>
        /// <returns></returns>
        public string GetInsertSql()
        {
            StringBuilder sbField = new StringBuilder(); //查询字段
            StringBuilder sbValue = new StringBuilder(); //值参数
            //唯一值验证
            bool bCheckValueUnique = false;
            StringBuilder sbCheckUnique = new StringBuilder();

            PropertyInfo[] oPropertyInfos = this.GetType().GetProperties();
            if (oPropertyInfos.Length > 0)
            {
                foreach (PropertyInfo proInfo in oPropertyInfos)
                {
                    object[] objs = proInfo.GetCustomAttributes(typeof(DBFieldAttribute), true);
                    if (objs != null && objs.Length > 0)
                    {
                        DBFieldAttribute oDBField = objs[0] as DBFieldAttribute;
                        string strFieldName = !string.IsNullOrEmpty(oDBField.FieldName) ? oDBField.FieldName : proInfo.Name;
                        object oValue = proInfo.GetValue(this, null);

                        //检查值的唯一性
                        if (oDBField.IsPrimaryKey || oDBField.CheckValueUnique)
                        {
                            if (sbCheckUnique.Length > 0) sbCheckUnique.Append(" AND ");
                            if (oDBField.IsPrimaryKey)
                                sbCheckUnique.AppendFormat("[{0}] <> '{1}'", strFieldName, oValue);
                            else
                            {
                                bCheckValueUnique = true;
                                sbCheckUnique.AppendFormat("[{0}] = '{1}'", strFieldName, oValue);
                            }
                        }

                        //只插入不为NULL的字段
                        if (oValue != null)
                        {
                            //查询字段, 值参数
                            if (sbField.Length > 0)
                            {
                                sbField.Append(", ");
                                sbValue.Append(", ");
                            }

                            sbField.AppendFormat("[{0}]", strFieldName);
                            sbValue.Append(string.Format("'{0}'", oValue));
                        }
                    }
                }
            }

            if (bCheckValueUnique)
                return string.Format(@"IF(NOT EXISTS(SELECT * FROM {0} WHERE {3}))
                    INSERT INTO {0} ({1}) VALUES ({2});", this.TableName, sbField.ToString(), sbValue.ToString(), sbCheckUnique.ToString());
            else
                return string.Format("INSERT INTO {0} ({1}) VALUES ({2});", this.TableName, sbField.ToString(), sbValue.ToString());
        }
        #endregion

        #region 获取更新SqlCommand
        /// <summary>
        /// 获取更新SqlCommand
        /// </summary>
        /// <param name="strResource">资源</param>
        /// <param name="strNameFieldValue">记录</param>
        /// <returns></returns>
        private SqlCommand GetUpdateSqlCommand(string strResource, out string strNameFieldValue)
        {
            StringBuilder sbField = new StringBuilder(); //更新字段
            StringBuilder sbFilter = new StringBuilder(); //过滤条件
            strNameFieldValue = null; //名称字段值
            SqlCommand cmd = new SqlCommand();
            //唯一值验证
            bool bCheckValueUnique = false;
            StringBuilder sbCheckUnique = new StringBuilder();

            PropertyInfo[] oPropertyInfos = this.GetType().GetProperties();
            if (oPropertyInfos.Length > 0)
            {
                foreach (PropertyInfo proInfo in oPropertyInfos)
                {
                    object[] objs = proInfo.GetCustomAttributes(typeof(DBFieldAttribute), true);
                    if (objs != null && objs.Length > 0)
                    {
                        DBFieldAttribute oDBField = objs[0] as DBFieldAttribute;
                        string strFieldName = !string.IsNullOrEmpty(oDBField.FieldName) ? oDBField.FieldName : proInfo.Name;
                        object oValue = proInfo.GetValue(this, null);
                        if (oDBField.IsPrimaryKey)
                        {
                            //主键,做为过滤条件
                            if (sbFilter.Length > 0)
                                sbFilter.Append(" AND ");
                            sbFilter.AppendFormat("[{0}]=@{0}", strFieldName);
                            cmd.Parameters.Add(string.Format("@{0}", strFieldName), oDBField.DBType).Value = oValue;

                            //唯一验证
                            if (sbCheckUnique.Length > 0) sbCheckUnique.Append(" AND ");
                            sbCheckUnique.AppendFormat("[{0}] <> @CVU{0}", strFieldName);
                            cmd.Parameters.Add(string.Format("@CVU{0}", strFieldName), oDBField.DBType).Value = oValue;
                        }
                        else
                        {
                            //只更新不为NULL的字段
                            if (oValue != null)
                            {
                                if (sbField.Length > 0)
                                    sbField.Append(", ");
                                sbField.Append(string.Format("[{0}] = @{0}", strFieldName));
                                cmd.Parameters.Add(string.Format("@{0}", strFieldName), oDBField.DBType).Value = oValue;
                            }

                            //唯一验证
                            if (oDBField.CheckValueUnique)
                            {
                                bCheckValueUnique = true;
                                if (sbCheckUnique.Length > 0) sbCheckUnique.Append(" AND ");
                                sbCheckUnique.AppendFormat("[{0}] = @CVU{0}", strFieldName);
                                cmd.Parameters.Add(string.Format("@CVU{0}", strFieldName), oDBField.DBType).Value = oValue;
                            }
                        }

                        //记录名称字段值
                        if (oDBField.IsLogField)
                            strNameFieldValue = Convert.ToString(oValue);
                    }
                }
            }
            if (sbFilter.Length == 0)
                throw new Exception("对象未设置主键！");

            if (bCheckValueUnique)
                cmd.CommandText = string.Format(@"IF(NOT EXISTS(SELECT * FROM {0} WHERE {3}))
                    UPDATE {0} SET {1} WHERE {2};", this.TableName, sbField.ToString(), sbFilter.ToString(), sbCheckUnique.ToString());
            else
                cmd.CommandText = string.Format("UPDATE {0} SET {1} WHERE {2};", this.TableName, sbField.ToString(), sbFilter.ToString());
            return cmd;
        }
        #endregion

        #region 获取更新Sql
        /// <summary>
        /// 获取更新Sql
        /// </summary>
        /// <returns></returns>
        public string GetUpdateSql()
        {
            StringBuilder sbField = new StringBuilder(); //更新字段
            StringBuilder sbFilter = new StringBuilder(); //过滤条件
            //唯一值验证
            bool bCheckValueUnique = false;
            StringBuilder sbCheckUnique = new StringBuilder();

            PropertyInfo[] oPropertyInfos = this.GetType().GetProperties();
            if (oPropertyInfos.Length > 0)
            {
                foreach (PropertyInfo proInfo in oPropertyInfos)
                {
                    object[] objs = proInfo.GetCustomAttributes(typeof(DBFieldAttribute), true);
                    if (objs != null && objs.Length > 0)
                    {
                        DBFieldAttribute oDBField = objs[0] as DBFieldAttribute;
                        string strFieldName = !string.IsNullOrEmpty(oDBField.FieldName) ? oDBField.FieldName : proInfo.Name;
                        object oValue = proInfo.GetValue(this, null);
                        if (oDBField.IsPrimaryKey)
                        {
                            //主键,做为过滤条件
                            if (sbFilter.Length > 0)
                                sbFilter.Append(" AND ");
                            sbFilter.AppendFormat("[{0}]='{1}'", strFieldName, oValue);

                            //唯一验证
                            if (sbCheckUnique.Length > 0) sbCheckUnique.Append(" AND ");
                            sbCheckUnique.AppendFormat("[{0}] <> '{1}'", strFieldName, oValue);
                        }
                        else
                        {
                            //只更新不为NULL的字段
                            if (oValue != null)
                            {
                                if (sbField.Length > 0)
                                    sbField.Append(", ");
                                sbField.Append(string.Format("[{0}] = '{1}'", strFieldName, oValue));
                            }

                            //唯一验证
                            if (oDBField.CheckValueUnique)
                            {
                                bCheckValueUnique = true;
                                if (sbCheckUnique.Length > 0) sbCheckUnique.Append(" AND ");
                                sbCheckUnique.AppendFormat("[{0}] = '{1}'", strFieldName, oValue);
                            }
                        }
                    }
                }
            }
            if (sbFilter.Length == 0)
                throw new Exception("对象未设置主键！");

            if (bCheckValueUnique)
                return string.Format(@"IF(NOT EXISTS(SELECT * FROM {0} WHERE {3}))
                    UPDATE {0} SET {1} WHERE {2};", this.TableName, sbField.ToString(), sbFilter.ToString(), sbCheckUnique.ToString());
            else
                return string.Format("UPDATE {0} SET {1} WHERE {2};", this.TableName, sbField.ToString(), sbFilter.ToString());
        }
        #endregion

        #region 获取删除SqlCommand
        /// <summary>
        /// 获取删除SqlCommand
        /// </summary>
        /// <param name="strResource">资源</param>
        /// <param name="strNameFieldValue">记录</param>
        /// <returns></returns>
        private SqlCommand GetDeleteSqlCommand(string strResource, out string strNameFieldValue)
        {
            StringBuilder sbFilter = new StringBuilder(); //过滤条件
            strNameFieldValue = null; //名称字段值
            SqlCommand cmd = new SqlCommand();

            PropertyInfo[] oPropertyInfos = this.GetType().GetProperties();
            if (oPropertyInfos.Length > 0)
            {
                foreach (PropertyInfo proInfo in oPropertyInfos)
                {
                    object[] objs = proInfo.GetCustomAttributes(typeof(DBFieldAttribute), true);
                    if (objs != null && objs.Length > 0)
                    {
                        DBFieldAttribute oDBField = objs[0] as DBFieldAttribute;
                        string strFieldName = !string.IsNullOrEmpty(oDBField.FieldName) ? oDBField.FieldName : proInfo.Name;
                        object oValue = proInfo.GetValue(this, null);
                        if (oDBField.IsPrimaryKey)
                        {
                            //主键,做为过滤条件
                            if (sbFilter.Length > 0)
                                sbFilter.Append(" AND ");
                            sbFilter.AppendFormat("[{0}] = @{0}", strFieldName);
                            cmd.Parameters.Add(string.Format("@{0}", strFieldName), oDBField.DBType).Value = oValue;
                        }
                        //记录名称字段值
                        if (oDBField.IsLogField)
                            strNameFieldValue = Convert.ToString(oValue);
                    }
                }
            }

            if (sbFilter.Length == 0)
                throw new Exception("对象未设置主键！");

            cmd.CommandText = string.Format("DELETE {0} WHERE {1};", this.TableName, sbFilter.ToString());
            return cmd;
        }
        #endregion

        #region 获取删除Sql
        /// <summary>
        /// 获取删除Sql
        /// </summary>
        /// <returns></returns>
        public string GetDeleteSql()
        {
            StringBuilder sbFilter = new StringBuilder(); //过滤条件

            PropertyInfo[] oPropertyInfos = this.GetType().GetProperties();
            if (oPropertyInfos.Length > 0)
            {
                foreach (PropertyInfo proInfo in oPropertyInfos)
                {
                    object[] objs = proInfo.GetCustomAttributes(typeof(DBFieldAttribute), true);
                    if (objs != null && objs.Length > 0)
                    {
                        DBFieldAttribute oDBField = objs[0] as DBFieldAttribute;
                        string strFieldName = !string.IsNullOrEmpty(oDBField.FieldName) ? oDBField.FieldName : proInfo.Name;
                        object oValue = proInfo.GetValue(this, null);
                        if (oDBField.IsPrimaryKey)
                        {
                            //主键,做为过滤条件
                            if (sbFilter.Length > 0)
                                sbFilter.Append(" AND ");
                            sbFilter.AppendFormat("[{0}] = '{1}'", strFieldName, oValue);
                        }
                    }
                }
            }

            if (sbFilter.Length == 0)
                throw new Exception("对象未设置主键！");

            return string.Format("DELETE {0} WHERE {1};", this.TableName, sbFilter.ToString());
        }
        #endregion



    }
}
