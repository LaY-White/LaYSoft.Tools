using LaYSoft.BaseCode.Attr;
using LaYSoft.BaseCode.DBClass;
using LaYSoft.BaseCode.DBClass.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LaYSoft.BaseCode
{
    public abstract class BaseDAO<T> where T : BaseModel, new()
    {
        public BaseDAO() { }

        #region 参数

        //表名
        private String _TableName;
        /// <summary>
        /// 表名
        /// </summary>
        private String TableName
        {
            get
            {
                if (String.IsNullOrEmpty(_TableName))
                {
                    Object[] Objs = typeof(T).GetCustomAttributes(typeof(DBTableAttribute), true);
                    if (Objs == null || Objs.Length == 0)
                        throw new Exception(String.Format("{0}类未定义DBTableAttribute属性", typeof(T).Name));

                    DBTableAttribute TableAttr = Objs[0] as DBTableAttribute;
                    _TableName = String.IsNullOrEmpty(TableAttr.TableName) ? typeof(T).Name : TableAttr.TableName;
                }

                return _TableName;
            }
        }

        //表字段
        private List<DBFieldAttribute> _Fields;
        /// <summary>
        /// 表字段
        /// </summary>
        private List<DBFieldAttribute> Fields
        {
            get
            {
                if (_Fields == null || _Fields.Count == 0)
                {
                    _Fields = new List<DBFieldAttribute>();
                    PropertyInfo[] oPropertyInfos = typeof(T).GetProperties();
                    foreach (PropertyInfo proInfo in oPropertyInfos)
                    {
                        Object[] Objs = proInfo.GetCustomAttributes(typeof(DBFieldAttribute), true);
                        if (Objs != null && Objs.Length > 0 && (Objs[0] as DBFieldAttribute) != null)
                        {
                            DBFieldAttribute FielAttr = Objs[0] as DBFieldAttribute;
                            if (String.IsNullOrEmpty(FielAttr.FieldName))
                                FielAttr.FieldName = proInfo.Name;

                            _Fields.Add(FielAttr);
                        }
                    }

                    if (_Fields.Count == 0)
                        throw new Exception(String.Format("{0}类未定义DBField", typeof(T).Name));
                }

                return _Fields;
            }
        }

        #endregion

        #region 提供公共方法重写

        /// <summary>
        /// 新增BaseModel，返回操作影响的记录条数
        /// </summary>
        public virtual int Add(T pModel)
        {
            List<String> Field = new List<String>();
            List<String> FieldValue = new List<String>();
            List<LaYSoftParameter> Pa = new List<LaYSoftParameter>();
            PropertyInfo IdentityField = null;

            #region 获取字段与值
            PropertyInfo[] oPropertyInfos = pModel.GetType().GetProperties();
            foreach (PropertyInfo proInfo in oPropertyInfos)
            {
                Object[] objs = proInfo.GetCustomAttributes(typeof(DBFieldAttribute), true);
                if (objs != null && objs.Length > 0)
                {
                    DBFieldAttribute DBFiel = objs[0] as DBFieldAttribute;
                    if (String.IsNullOrEmpty(DBFiel.FieldName))
                        DBFiel.FieldName = proInfo.Name;

                    object Value = proInfo.GetValue(pModel, null);
                    if (!DBFiel.IsIdentity && Value != null)
                    {
                        Field.Add("[" + DBFiel.FieldName + "]");
                        FieldValue.Add("@" + DBFiel.FieldName);
                        Pa.Add(new LaYSoftParameter("@" + DBFiel.FieldName, Value));
                    }
                    else if (DBFiel.IsIdentity)
                    {
                        IdentityField = proInfo;
                    }
                }
            }
            #endregion

            if (IdentityField != null)
                Pa.Add(new LaYSoftParameter("@NewID", null, ParameterDirection.Output));

            int rel = SqlHelper.ExecuteNonQuery(String.Format("INSERT INTO {0}({1}) VALUES({2});{3}",
                        this.TableName,
                        String.Join(",", Field.ToArray()),
                        String.Join(",", FieldValue.ToArray()),
                        IdentityField == null ? "" : "select @NewID = @@Identity;")
                    , Pa);

            if (IdentityField != null)
            {
                LaYSoftParameter IdentityValue = Pa.FirstOrDefault(P => P.PaName == "@NewID");
                if (IdentityValue != null && IdentityValue.Value != null)
                    IdentityField.SetValue(pModel, IdentityValue.Value, null);
            }

            return rel;
        }

        /// <summary>
        /// 修改BaseModel，返回操作影响的记录条数
        /// </summary>
        public virtual int Update(T pModel)
        {
            List<LaYSoftParameter> Pa = new List<LaYSoftParameter>();
            List<String> FieldUpdate = new List<String>();
            String strPrimaryKeyWhere = "";

            PropertyInfo[] oPropertyInfos = pModel.GetType().GetProperties();
            foreach (PropertyInfo proInfo in oPropertyInfos)
            {
                Object[] objs = proInfo.GetCustomAttributes(typeof(DBFieldAttribute), true);
                if (objs != null && objs.Length > 0)
                {
                    DBFieldAttribute DBFiel = objs[0] as DBFieldAttribute;
                    if (String.IsNullOrEmpty(DBFiel.FieldName))
                        DBFiel.FieldName = proInfo.Name;

                    object Value = proInfo.GetValue(pModel, null);
                    if (DBFiel.IsPrimaryKey)
                    {
                        if (Value == null) throw new Exception(String.Format("主键{0}不能为空", DBFiel.FieldName));
                        strPrimaryKeyWhere = String.Format(" AND {0}=@{0} ", DBFiel.FieldName);
                        Pa.Add(new LaYSoftParameter("@" + DBFiel.FieldName, Value));
                    }
                    else
                    {
                        if (Value == null) Value = DBNull.Value;
                        FieldUpdate.Add(String.Format("{0}=@{0}", DBFiel.FieldName));
                        Pa.Add(new LaYSoftParameter("@" + DBFiel.FieldName, Value));
                    }
                }
            }

            if (String.IsNullOrEmpty(strPrimaryKeyWhere))
                throw new Exception(String.Format("{0}尚未设置主键", pModel.GetType().Name));

            return SqlHelper.ExecuteNonQuery(
                String.Format("UPDATE {0} SET {1} WHERE 1=1 {2}", this.TableName,
                    String.Join(",", FieldUpdate.ToArray()), strPrimaryKeyWhere),
                Pa);
        }

        /// <summary>
        /// 删除BaseModel，返回操作影响的记录条数
        /// </summary>
        public virtual int Delete(T pModel)
        {
            List<LaYSoftParameter> Pa = new List<LaYSoftParameter>();
            String strPrimaryKeyWhere = "";

            PropertyInfo[] oPropertyInfos = pModel.GetType().GetProperties();
            foreach (PropertyInfo proInfo in oPropertyInfos)
            {
                Object[] objs = proInfo.GetCustomAttributes(typeof(DBFieldAttribute), true);
                if (objs != null && objs.Length > 0)
                {
                    DBFieldAttribute DBFiel = objs[0] as DBFieldAttribute;
                    if (String.IsNullOrEmpty(DBFiel.FieldName))
                        DBFiel.FieldName = proInfo.Name;

                    object Value = proInfo.GetValue(pModel, null);
                    if (DBFiel.IsPrimaryKey)
                    {
                        if (Value == null) throw new Exception(String.Format("主键{0}不能为空", DBFiel.FieldName));
                        strPrimaryKeyWhere = String.Format(" AND {0}=@{0} ", DBFiel.FieldName);
                        Pa.Add(new LaYSoftParameter("@" + DBFiel.FieldName, Value));
                    }
                }
            }

            if (String.IsNullOrEmpty(strPrimaryKeyWhere))
                throw new Exception(String.Format("{0}尚未设置主键", pModel.GetType().Name));

            return SqlHelper.ExecuteNonQuery(
                String.Format("DELETE {0} WHERE 1=1 {2}", this.TableName, strPrimaryKeyWhere),
                Pa);
        }


        /// <summary>
        /// 根据唯一属性得到BaseModel
        /// </summary>
        /// <remarks>根据唯一属性得到BaseModel</remarks>
        /// <param name="pModel">实体类</param>
        /// <returns>BaseModel</returns>
        public virtual T GetObject(T pModel)
        {
            String strPrimaryKeyWhere = "";
            List<LaYSoftParameter> Pa = new List<LaYSoftParameter>();

            //查询字段
            String strField = String.Join(",", this.Fields.Select(F => "[" + F.FieldName + "]").ToArray());

            PropertyInfo[] oPropertyInfos = pModel.GetType().GetProperties();
            foreach (PropertyInfo proInfo in oPropertyInfos)
            {
                Object[] objs = proInfo.GetCustomAttributes(typeof(DBFieldAttribute), true);
                if (objs != null && objs.Length > 0)
                {
                    DBFieldAttribute DBFiel = objs[0] as DBFieldAttribute;
                    if (String.IsNullOrEmpty(DBFiel.FieldName))
                        DBFiel.FieldName = proInfo.Name;

                    object Value = proInfo.GetValue(pModel, null);
                    if (DBFiel.IsPrimaryKey)
                    {
                        if (Value == null) throw new Exception(String.Format("主键{0}不能为空", DBFiel.FieldName));
                        strPrimaryKeyWhere = String.Format(" AND {0}=@{0} ", DBFiel.FieldName);
                        Pa.Add(new LaYSoftParameter("@" + DBFiel.FieldName, Value));
                    }
                }
            }

            if (String.IsNullOrEmpty(strPrimaryKeyWhere))
                throw new Exception(String.Format("{0}尚未设置主键", pModel.GetType().Name));

            DataTable Dt = SqlHelper.ExecuteDataTable(String.Format("SELECT TOP 1 {0} FROM {1} WHERE 1=1 {2}", strField, TableName, strPrimaryKeyWhere)
                  , Pa);

            IList<T> ListT = ConvertToModel(Dt);
            if (ListT.Count == 0)
                return null;

            return ListT[0];
        }

        /// <summary>
        /// 根据查询参数返回BaseModel的记录
        /// </summary>
        /// <remarks>根据查询参数返回BaseModel的记录</remarks>
        /// <param name="pModel">实体基类</param>
        /// <param name="pBaseParams">查询参数实体类pBaseParams</param>
        /// <returns>符合条件的IList集合</returns>
        public virtual IList<T> GetList(BaseParams pBaseParams)
        {
            if (String.IsNullOrEmpty(pBaseParams.OrderBy))
                throw new Exception(String.Format("{0}类未设置OrderBy", pBaseParams.GetType().Name));

            List<LaYSoftParameter> Pa = new List<LaYSoftParameter>();

            //查询字段，并判断是否禁止查看
            String strField = String.Join(",", this.Fields.Where(F => !pBaseParams.DisablePropertys.Contains(F.FieldName))
                .Select(F => "[" + F.FieldName + "]").ToArray());

            String StrSqlWhere = GetSqlWhere(pBaseParams, ref Pa);

            DataTable Dt = SqlHelper.ExecuteDataTable(String.Format(@"select {0} * From (Select 
                            ROW_NUMBER() OVER (ORDER BY {1}) AS RowNumber,{2} 
                            FROM [{3}] WHERE 1=1 {4} ) AS [{3}] 
                            WHERE 1=1 {5} {6} ",
                          pBaseParams.Top != null ? String.Format("TOP {0} ", pBaseParams.Top) : "",
                          pBaseParams.OrderBy, strField, this.TableName, StrSqlWhere,
                          pBaseParams.BeginRow != null ? String.Format(" AND RowNumber >= {0} ", pBaseParams.BeginRow) : "",
                          pBaseParams.EndRow != null ? String.Format(" AND {0} >= RowNumber ", pBaseParams.EndRow) : "")
                    , Pa);

            return this.ConvertToModel(Dt);
        }

        /// <summary>
        /// 根据查询参数返回BaseModel的记录
        /// </summary>
        /// <param name="pBaseParams">参数 </param>
        /// <param name="pTotalCount">总数</param>
        /// <returns></returns>
        public virtual IList<T> GetListByPaged(BaseParams pBaseParams, out int pTotalCount)
        {
            IList<T> alist = GetList(pBaseParams);
            if (pBaseParams.Top == null && pBaseParams.BeginRow == null && pBaseParams.EndRow == null)
                pTotalCount = alist.Count;
            else
                pTotalCount = GetRecordCount(pBaseParams);

            return alist;
        }

        /// <summary>
        /// 取得记录数
        /// </summary>
        /// <param name="pBaseParams">查询参数</param>
        /// <returns></returns>
        public virtual int GetRecordCount(BaseParams pBaseParams)
        {
            List<LaYSoftParameter> Pa = new List<LaYSoftParameter>();
            String StrSqlWhere = GetSqlWhere(pBaseParams, ref Pa);

            return SqlHelper.ExecuteScalarNum(String.Format("SELECT Count(*) FROM [{0}] WHERE 1=1 {1}", this.TableName, StrSqlWhere)
                , Pa);
        }


        /// <summary>
        /// 是否存在该对象
        /// </summary>
        /// <param name="pModel">实体类</param>
        /// <returns></returns>
        public virtual bool IsExists(T pModel)
        {
            //TODO:
            throw new NotImplementedException();
        }
        #endregion

        #region 私有方式

        /// <summary>
        /// 获取SqlWhere
        /// </summary>
        /// <returns></returns>
        private string GetSqlWhere(BaseParams pBaseParams, ref List<LaYSoftParameter> Pa)
        {
            StringBuilder StrSqlWhere = new StringBuilder();

            PropertyInfo[] oPropertyInfos = pBaseParams.GetType().GetProperties();
            if (oPropertyInfos.Length > 0)
            {
                foreach (PropertyInfo proInfo in oPropertyInfos)
                {
                    object PaValue = proInfo.GetValue(pBaseParams, null);
                    if (PaValue != null)
                    {
                        object[] objs = proInfo.GetCustomAttributes(typeof(DBParamsAttribute), true);
                        if (objs != null && objs.Length > 0)
                        {
                            DBParamsAttribute oDBPa = objs[0] as DBParamsAttribute;
                            if (String.IsNullOrEmpty(oDBPa.SearchSQL))
                                throw new Exception(String.Format("{0}.{1}未设置SearchSQL", pBaseParams.GetType().Name, proInfo.Name));

                            StrSqlWhere.Append(" AND " + oDBPa.SearchSQL);
                            Pa.Add(new LaYSoftParameter("@" + proInfo.Name, PaValue));
                        }
                    }
                }
            }

            //合并SqlWhere数据
            if (!String.IsNullOrEmpty(pBaseParams.SqlWhere))
                StrSqlWhere.Append(pBaseParams.SqlWhere);
            if (pBaseParams.SqlWherePa != null && pBaseParams.SqlWherePa.Count > 0)
                Pa = Pa.Concat(pBaseParams.SqlWherePa).ToList();

            return StrSqlWhere.ToString();
        }

        /// <summary>
        /// 转换DataTable
        /// </summary>
        private IList<T> ConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<T> ts = new List<T>();

            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列    

                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }
        #endregion

    }
}
