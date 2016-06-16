using LaYSoft.BaseCode.Attr;
using LaYSoft.BaseCode.DBClass.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LaYSoft.BaseCode
{
    public abstract class BaseDAO<T> where T : BaseModel
    {
        public BaseDAO() { }

        #region 提供子类重写

        /// <summary>
        /// 根据唯一属性得到BaseModel
        /// </summary>
        /// <remarks>根据唯一属性得到BaseModel</remarks>
        /// <param name="pModel">实体类</param>
        /// <returns>BaseModel</returns>
        protected virtual T GetObject(T pModel)
        {
            //return (T)MyMapper.QueryForObject("SelectObject_" + pModel.TableName, pModel.ID);
            return null;
        }

        /// <summary>
        /// 新增,返回值为BaseModel唯一属性的值
        /// </summary>
        /// <remarks>新增,返回值为BaseModel唯一属性的值</remarks>
        /// <param name="pModel">pModel</param>
        /// <returns>返回值为BaseModel唯一属性的值</returns>
        protected virtual object Add(T pModel)
        {
            //string aSqlMap = string.Format("Insert_{0}", pModel.TableName);
            //pModel.ID = MyMapper.Insert(aSqlMap, pModel);
            //return pModel.ID;
            return null;
        }

        /// <summary>
        /// 修改BaseModel
        /// </summary>
        /// <remarks>修改BaseModel</remarks>
        /// <param name="pModel">实体对象</param>
        /// <returns></returns>
        protected virtual void Update(T pModel)
        {
            //string aSqlMap = string.Format("Update_{0}", pModel.TableName);
            //MyMapper.Update(aSqlMap, pModel);
        }

        /// <summary>
        /// 删除BaseModel
        /// </summary>
        /// <remarks>删除BaseModel</remarks>
        /// <param name="pModel">实体对象</param>
        /// <returns></returns>
        protected virtual void Delete(T pModel)
        {
            //string aSqlMap = string.Format("Delete_{0}", pModel.TableName);
            //MyMapper.Delete(aSqlMap, pModel.ID);
        }


        /// <summary>
        /// 根据查询参数返回BaseModel的记录
        /// </summary>
        /// <remarks>根据查询参数返回BaseModel的记录</remarks>
        /// <param name="pModel">实体基类</param>
        /// <param name="pBaseParams">查询参数实体类pBaseParams</param>
        /// <returns>符合条件的IList集合</returns>
        protected virtual IList<T> GetList(BaseParams pBaseParams)
        {
            //string aSqlMap = string.Format("SelectByParameter_{0}", pBaseParams.TableName);
            //string strSQL = GetRuntimeSql(aSqlMap, pBaseParams);
            //if (!string.IsNullOrEmpty(pBaseParams.DisablePropertys))
            //{
            //    string resultMapKey = string.Format("{0}.SelectResult_{0}", pBaseParams.TableName);
            //    if (MyMapper.ResultMaps.Contains(resultMapKey))
            //    {
            //        IResultMap resultMap = (IResultMap)MyMapper.ResultMaps[resultMapKey];
            //        string[] strs = pBaseParams.DisablePropertys.Split(',');
            //        IList<ResultProperty> disablePropertyList = new List<ResultProperty>();
            //        foreach (string str in strs)
            //        {
            //            ResultProperty resultProperty = resultMap.Properties.FindByPropertyName(str);
            //            if (resultProperty == null) continue;
            //            disablePropertyList.Add(resultProperty);//加到备忘录，以便稍后恢复
            //            resultMap.Properties.Remove(resultProperty);//暂时移除该结果属性
            //        }

            //        IList resultList = MyMapper.QueryForList(aSqlMap, pBaseParams);
            //        //查询完毕后恢复原来的ResultMap的属性设置
            //        foreach (ResultProperty disableProperty in disablePropertyList)
            //        {
            //            resultMap.Properties.Add(disableProperty);
            //        }
            //        return resultList;
            //    }
            //    else
            //    {
            //        return MyMapper.QueryForList(aSqlMap, pBaseParams);
            //    }
            //}
            //else
            //{
            //    return MyMapper.QueryForList(aSqlMap, pBaseParams);
            //}
            return null;
        }

        /// <summary>
        /// 根据查询参数返回BaseModel的记录
        /// </summary>
        /// <param name="pBaseParams">参数 </param>
        /// <param name="pTotalCount">总数</param>
        /// <returns></returns>
        protected virtual IList<T> GetListByPaged(BaseParams pBaseParams, out int pTotalCount)
        {

            //IList alist = OnGetList(pBaseParams);
            //if (string.IsNullOrEmpty(pBaseParams.Top))
            //{
            //    pTotalCount = GetRecordCount(pBaseParams);
            //}
            //else
            //{
            //    pTotalCount = alist.Count;
            //}
            //return alist;
            pTotalCount = 0;
            return null;
        }

        /// <summary>
        /// 取得记录数
        /// </summary>
        /// <param name="pBaseParams">查询参数</param>
        /// <returns></returns>
        protected virtual int GetRecordCount(BaseParams pBaseParams)
        {
            //string aSqlMap = string.Format("SelectByParameterCount_{0}", pBaseParams.TableName);
            //object count = MyMapper.QueryForObject(aSqlMap, pBaseParams);
            //return Int32.Parse(count == null ? "0" : count.ToString());

            return 0;
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

        #region 私有基础方法

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

        /// <summary>
        /// 获取查询Sql
        /// </summary>
        /// <returns></returns>
        private string GetSelectSql(BaseParams pBaseParams, out List<LaYSoftParameter> Pa)
        {
            if (String.IsNullOrEmpty(pBaseParams.OrderBy))
                throw new Exception(String.Format("{0}类未设置OrderBy", pBaseParams.GetType().Name));

            Pa = new List<LaYSoftParameter>();

            //查询字段，并判断是否禁止查看
            String strField = String.Join(",", this.Fields.Where(F => !pBaseParams.DisablePropertys.Contains(F.FieldName))
                .Select(F => "[" + F.FieldName + "]").ToArray());

            StringBuilder sbFilter = new StringBuilder();
            #region 过滤条件
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

                            sbFilter.Append(" AND " + oDBPa.SearchSQL);
                            Pa.Add(new LaYSoftParameter("@" + proInfo.Name, PaValue));
                        }
                    }
                }
            }

            #endregion

            //合并SqlWhere数据
            if (!String.IsNullOrEmpty(pBaseParams.SqlWhere))
                sbFilter.Append(pBaseParams.SqlWhere);
            if (pBaseParams.SqlWherePa != null && pBaseParams.SqlWherePa.Count > 0)
                Pa = Pa.Concat(pBaseParams.SqlWherePa).ToList();


            return String.Format(@"select {0} * From (Select 
                            ROW_NUMBER() OVER (ORDER BY {1}) AS RowNumber,{2} 
                            FROM [{3}] WHERE 1=1 {4} ) AS [{3}] 
                            WHERE 1=1 {5} {6} ",
                  pBaseParams.Top != null ? String.Format("TOP {0} ", pBaseParams.Top) : "",
                  pBaseParams.OrderBy, strField, this.TableName, sbFilter.ToString(),
                  pBaseParams.BeginRow != null ? String.Format(" AND RowNumber >= {0} ", pBaseParams.BeginRow) : "",
                  pBaseParams.EndRow != null ? String.Format(" AND {0} >= RowNumber ", pBaseParams.EndRow) : "");
        }

        /// <summary>
        /// 获取插入Sql
        /// </summary>
        /// <returns></returns>
        private string GetInsertSql(T pModel, out List<LaYSoftParameter> Pa)
        {
            List<String> Field = new List<String>();
            List<String> FieldValue = new List<String>();
            Pa = new List<LaYSoftParameter>();

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
                }
            }

            return String.Format("INSERT INTO {0}({1}) VALUES({2})", this.TableName,
                String.Join(",", Field.ToArray()),
                String.Join(",", FieldValue.ToArray()));
        }

        /// <summary>
        /// 获取更新Sql
        /// </summary>
        /// <returns></returns>
        private string GetUpdateSql(T pModel, out List<LaYSoftParameter> Pa)
        {
            Pa = new List<LaYSoftParameter>();
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

            return String.Format("UPDATE {0} SET {1} WHERE 1=1 {2}", this.TableName,
                String.Join(",", FieldUpdate.ToArray()), strPrimaryKeyWhere);
        }

        /// <summary>
        /// 获取删除Sql
        /// </summary>
        /// <returns></returns>
        private string GetDeleteSql(T pModel, out List<LaYSoftParameter> Pa)
        {
            Pa = new List<LaYSoftParameter>();
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


            return String.Format("DELETE {0} WHERE 1=1 {2}", this.TableName, strPrimaryKeyWhere);
        }

        #endregion

    }
}
