/***************************************************************************
 * 
 *       功能：     数据库库访问基类
 *       作者：     
 *       日期：     
 * 
 *       修改日期：2010-3-24
 *       修改人：  黄振标
 *       修改内容：实现了IbatisNet读取动态配置映射文件
 * 
 * *************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections;
using IBatisNet.Common;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.Scope;
using IBatisNet.DataMapper.MappedStatements;

using DGDP.Qtone.BaseCore.Model;
using DGDP.Qtone.BaseCore.IDAL;
using DGDP.Qtone.BaseCore.Params;
using DGDP.Qtone.BaseCore.Comm;
using IBatisNet.DataMapper.Configuration.ResultMapping;

namespace DGDP.Qtone.BaseCore.DAL
{
    public abstract class BaseDAO<T> : ITransaction,IBaseDAO<T> where T : BaseModel
    {
        public BaseDAO()
        {
            this._MyMapper = MapperHelper.Instance(BaseCoreConfigHelper.DefaultSqlMapFileName);
//                IBatisNet.DataMapper.Mapper.Instance();
        }

        #region MyMapper
        
       
        protected ISqlMapper _MyMapper = null;

        /// <summary>
        /// Ibatisnet的Mapper类
        /// </summary>
        public virtual ISqlMapper MyMapper
        {
            get { return _MyMapper; }
        }

        /// <summary>
        /// 创建事务
        /// </summary>
        /// <returns></returns>
        public bool BeginTransaction()
        {
            if (this.MyMapper.IsSessionStarted) { return false; }
            else
            {
                this.MyMapper.BeginTransaction();
                return true;
            }
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            this.MyMapper.CommitTransaction();
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBackTransaction()
        {
            this.MyMapper.RollBackTransaction();
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        protected void FlushCaches()
        {
            this.MyMapper.FlushCaches();
        }

        #endregion

        #region 公共基础方法
        
       
        /// <summary>
        /// 根据唯一属性得到BaseModel
        /// </summary>
        /// <remarks>根据唯一属性得到BaseModel</remarks>
        /// <param name="pModel">实体类</param>
        /// <returns>BaseModel</returns>
        public T GetObject(T pModel)
        {
            ///ToDo
            return OnGetObject(pModel);
        }

        /// <summary>
        /// 新增,返回值为BaseModel唯一属性的值
        /// </summary>
        /// <remarks>新增,返回值为BaseModel唯一属性的值</remarks>
        /// <param name="pModel">pModel</param>
        /// <returns>返回值为BaseModel唯一属性的值</returns>
        public object Add(T pModel)
        {
            ///ToDo
            
            return  OnAdd(pModel);
        }

        /// <summary>
        /// 修改BaseModel
        /// </summary>
        /// <remarks>修改BaseModel</remarks>
        /// <param name="pModel">实体对象</param>
        /// <returns></returns>
        public void Update(T pModel)
        {
            ///ToDo
            OnUpdate(pModel);
        }

        /// <summary>
        /// 删除BaseModel
        /// </summary>
        /// <remarks>删除BaseModel</remarks>
        /// <param name="pModel">实体对象</param>
        /// <returns></returns>
        public void Delete(T pModel)
        {
            ///ToDo
            OnDelete(pModel);
        }


        /// <summary>
        /// 根据查询参数返回BaseModel的记录
        /// </summary>
        /// <remarks>根据查询参数返回BaseModel的记录</remarks>
        /// <param name="pModel">实体基类</param>
        /// <param name="pBaseParams">查询参数实体类pBaseParams</param>
        /// <returns>符合条件的IList集合</returns>
        public IList GetList(BaseParams pBaseParams)
        {
            ///ToDo
            return OnGetList(pBaseParams);
        }

        public IList<T> GetListExt(BaseParams pBaseParams)
        {
            return OnGetListExt(pBaseParams);
        }

        /// <summary>
        /// 根据分页参数获取列表信息
        /// </summary>
        /// <param name="pParamsModel">参数对象</param>
        /// <param name="pTotalCount">返回总数</param>
        /// <returns>返回列表信息</returns>
        public IList GetListByPaged(BaseParams pParamsModel, out int pTotalCount)
        {
            return OnGetListByPaged(pParamsModel,out pTotalCount);
        }

        /// <summary>
        /// 根据分页参数获取列表信息
        /// </summary>
        /// <param name="pParamsModel">参数对象</param>
        /// <param name="pTotalCount">返回总数</param>
        /// <returns>返回列表信息</returns>
        public IList<T> GetListByPagedExt(BaseParams pParamsModel, out int pTotalCount)
        {
            return OnGetListByPagedExt(pParamsModel, out pTotalCount);
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


        /// <summary>
        /// 根据执行参数获取运行时的Sql
        /// </summary>
        /// <param name="pStatementID">StatentmentID</param>
        /// <param name="pParams">查询参数</param>
        /// <returns></returns>
        public string GetRuntimeSql(string pStatementID, object pParams)
        {
            string result = string.Empty;
            try
            {
                IMappedStatement iMappedStatement = MyMapper.GetMappedStatement(pStatementID);
                if (!MyMapper.IsSessionStarted)
                {
                    MyMapper.OpenConnection();
                }
                RequestScope scope = iMappedStatement.Statement.Sql.GetRequestScope(iMappedStatement, pParams, MyMapper.LocalSession);
                result = scope.PreparedStatement.PreparedSql;
            }
            catch (Exception ex)
            {
                result = "SqlMapper根据查询参数获取SQL语句出现异常: " + ex.Message;

                throw new Exception(result);
            }

            return result;
        }

        /// <summary>
        /// 获取 SqlCommand 
        /// </summary>
        /// <param name="pStatementID">pStatementID</param>
        /// <param name="pParams">查询参数</param>
        /// <param name="pCommandType">存储过程或者Sql</param>
        /// <returns></returns>
        public IDbCommand GetCommand(string pStatementID, object pParams, CommandType pCommandType)
        {
            IStatement statement = MyMapper.GetMappedStatement(pStatementID).Statement;
            IMappedStatement mapStatement = MyMapper.GetMappedStatement(pStatementID);

            if (!MyMapper.IsSessionStarted)
            {
                MyMapper.OpenConnection();
            }

            RequestScope request = statement.Sql.GetRequestScope(mapStatement, pParams, MyMapper.LocalSession);
            mapStatement.PreparedCommand.Create(request, MyMapper.LocalSession, statement, pParams);

            IDbCommand preCommand = request.IDbCommand;
            IDbCommand command = MyMapper.LocalSession.CreateCommand(pCommandType);
            command.CommandTimeout = 240;//默认为30秒
            command.CommandText = preCommand.CommandText;

            foreach (IDataParameter pa in preCommand.Parameters)
            {
                IDbDataParameter para = MyMapper.LocalSession.CreateDataParameter();
                para.ParameterName = pa.ParameterName;
                para.Value = pa.Value;
                command.Parameters.Add(para);
            } 


            return command;
        }

        /// <summary>
        /// 根据参数返回DataSet
        /// </summary>
        /// <param name="pSql">SQL</param>
        /// <returns></returns>
        public DataSet GetDataSet(string pSql)
        {
            DataSet ds = new DataSet();

            if (!MyMapper.IsSessionStarted)
            {
                MyMapper.OpenConnection();
            }

            IDbCommand command = GetCommand(pSql);
            IDbDataAdapter dataAdapter = MyMapper.LocalSession.CreateDataAdapter(command);
            dataAdapter.Fill(ds);

            return ds;
        }


        /// <summary>
        /// 根据参数返回DataSet
        /// </summary>
        /// <param name="pStatementID">StatementID</param>
        /// <param name="pParams">查询参数</param>
        /// <returns></returns>
        public DataSet GetDataSet(string pStatementID, object pParams)
        {
            DataSet ds = new DataSet();

            if (!MyMapper.IsSessionStarted)
            {
                MyMapper.OpenConnection();
            }

            IDbCommand command = GetCommand(pStatementID, pParams);
            IDbDataAdapter dataAdapter = MyMapper.LocalSession.CreateDataAdapter(command);
            dataAdapter.Fill(ds);

            return ds;
        }

        /// <summary>
        /// 根据参数返回IDbCommand对象
        /// </summary>
        /// <param name="pStatementID">StatementID</param>
        /// <param name="pParams">查询参数</param>
        /// <returns></returns>
        public IDbCommand GetCommand(string pStatementID, object pParams)
        {
            IDbCommand command = MyMapper.LocalSession.CreateCommand(CommandType.Text);
            command.CommandTimeout = 240;//默认为30秒
            command.CommandText = this.GetRuntimeSql(pStatementID, pParams);
            return command;
        }

        /// <summary>
        /// 根据参数返回IDbCommand对象
        /// </summary>
        /// <param name="pSql">SQL</param>
        /// <returns></returns>
        public IDbCommand GetCommand(string pSql)
        {
            IDbCommand command = MyMapper.LocalSession.CreateCommand(CommandType.Text);
            command.CommandTimeout = 240;//默认为30秒
            command.CommandText = pSql;
            return command;
        }

        /// <summary>
        /// 根据参数返回第一个DataTable
        /// </summary>
        /// <param name="pStatementID">StatementID</param>
        /// <param name="pParams">查询参数</param>
        /// <returns></returns>
        public DataTable GetDataTable(string pStatementID, object pParams)
        {
            DataSet ds = this.GetDataSet(pStatementID, pParams);

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        /// <summary>
        /// 根据参数返回第一个DataTable
        /// </summary>
        /// <param name="pSql">SQL</param>
        /// <returns></returns>
        public DataTable GetDataTable(string pSql)
        {
            DataSet ds = this.GetDataSet(pSql);

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        /// <summary>
        /// 根据参数返回第一个DataTable
        /// </summary>
        /// <param name="pStatementID">XML对应的SQL关键字ID</param>
        /// <param name="pParams">查询参数</param>
        /// <param name="pOutParams">输出参数</param>
        /// <returns></returns>
        public DataTable GetProcedureDataTable(string pStatementID, object pParams, ref Hashtable pOutParams)
        {
            DataSet ds = this.GetProcedureDataSet(pStatementID, pParams, ref pOutParams);

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        /// <summary>
        /// 根据参数返回DataSet
        /// </summary>
        /// <param name="pStatementID">XML对应的SQL关键字ID</param>
        /// <param name="pParams">查询参数</param>
        /// <param name="pOutParams">输出参数</param>
        /// <returns></returns>
        public DataSet GetProcedureDataSet(string pStatementID, object pParams, ref Hashtable pOutParams)
        {
            DataSet ds = new DataSet();

            if (MyMapper.IsSessionStarted)
            {
                MyMapper.OpenConnection();
            }

            IDbCommand cmd = GetCommand(pStatementID, pParams, CommandType.StoredProcedure);

            try
            {
                IDbDataAdapter adapter = MyMapper.LocalSession.CreateDataAdapter(cmd);
                adapter.Fill(ds);

                foreach (IDataParameter parameter in cmd.Parameters)
                {
                    if (parameter.Direction == ParameterDirection.Output)
                    {
                        pOutParams.Add(parameter.ParameterName, parameter.Value);
                    }
                }
            }
            catch
            {
            }

            return ds;
        }

        #endregion

        #region 提供子类重写

        /// <summary>
        /// 根据唯一属性得到BaseModel
        /// </summary>
        /// <remarks>根据唯一属性得到BaseModel</remarks>
        /// <param name="pModel">实体类</param>
        /// <returns>BaseModel</returns>
        protected virtual T OnGetObject(T pModel)
        {
            return (T)MyMapper.QueryForObject("SelectObject_" + pModel.TableName, pModel.ID);
        }

        /// <summary>
        /// 新增,返回值为BaseModel唯一属性的值
        /// </summary>
        /// <remarks>新增,返回值为BaseModel唯一属性的值</remarks>
        /// <param name="pModel">pModel</param>
        /// <returns>返回值为BaseModel唯一属性的值</returns>
        protected virtual object OnAdd(T pModel)
        {
            string aSqlMap = string.Format("Insert_{0}", pModel.TableName);
            pModel.ID =  MyMapper.Insert(aSqlMap, pModel);
            return pModel.ID;
        }

        /// <summary>
        /// 修改BaseModel
        /// </summary>
        /// <remarks>修改BaseModel</remarks>
        /// <param name="pModel">实体对象</param>
        /// <returns></returns>
        protected virtual void OnUpdate(T pModel)
        {
            string aSqlMap = string.Format("Update_{0}", pModel.TableName);
            MyMapper.Update(aSqlMap, pModel);
        }

        /// <summary>
        /// 删除BaseModel
        /// </summary>
        /// <remarks>删除BaseModel</remarks>
        /// <param name="pModel">实体对象</param>
        /// <returns></returns>
        protected virtual void OnDelete(T pModel)
        {
            string aSqlMap = string.Format("Delete_{0}", pModel.TableName);
            MyMapper.Delete(aSqlMap, pModel.ID);
        }


        /// <summary>
        /// 根据查询参数返回BaseModel的记录
        /// </summary>
        /// <remarks>根据查询参数返回BaseModel的记录</remarks>
        /// <param name="pModel">实体基类</param>
        /// <param name="pBaseParams">查询参数实体类pBaseParams</param>
        /// <returns>符合条件的IList集合</returns>
        protected virtual IList OnGetList(BaseParams pBaseParams)
        {
            string aSqlMap = string.Format("SelectByParameter_{0}", pBaseParams.TableName);
            string strSQL = GetRuntimeSql(aSqlMap, pBaseParams);
            if (!string.IsNullOrEmpty(pBaseParams.DisablePropertys))
            {
                string resultMapKey = string.Format("{0}.SelectResult_{0}", pBaseParams.TableName);                
                if (MyMapper.ResultMaps.Contains(resultMapKey))
                {
                    IResultMap resultMap = (IResultMap)MyMapper.ResultMaps[resultMapKey];
                    string[] strs = pBaseParams.DisablePropertys.Split(',');
                    IList<ResultProperty> disablePropertyList = new List<ResultProperty>();
                    foreach (string str in strs)
                    {
                        ResultProperty resultProperty = resultMap.Properties.FindByPropertyName(str);
                        if (resultProperty == null) continue;
                        disablePropertyList.Add(resultProperty);//加到备忘录，以便稍后恢复
                        resultMap.Properties.Remove(resultProperty);//暂时移除该结果属性
                    }
                    
                    IList resultList = MyMapper.QueryForList(aSqlMap, pBaseParams);
                    //查询完毕后恢复原来的ResultMap的属性设置
                    foreach (ResultProperty disableProperty in disablePropertyList)
                    {
                        resultMap.Properties.Add(disableProperty);
                    }
                    return resultList;
                }
                else
                {
                    return MyMapper.QueryForList(aSqlMap, pBaseParams);
                }
            }
            else
            {
                return MyMapper.QueryForList(aSqlMap, pBaseParams);
            }
        }

        /// <summary>
        /// 根据查询参数返回BaseModel的记录
        /// </summary>
        /// <remarks>根据查询参数返回BaseModel的记录</remarks>
        /// <param name="pModel">实体基类</param>
        /// <param name="pBaseParams">查询参数实体类pBaseParams</param>
        /// <returns>符合条件的IList集合</returns>
        protected virtual IList<T> OnGetListExt(BaseParams pBaseParams)
        {
            string aSqlMap = string.Format("SelectByParameter_{0}", pBaseParams.TableName);
            string strSQL = GetRuntimeSql(aSqlMap, pBaseParams);
            return MyMapper.QueryForList<T>(aSqlMap, pBaseParams);
        }

        /// <summary>
        /// 根据查询参数返回BaseModel的记录
        /// </summary>
        /// <param name="pBaseParams">参数 </param>
        /// <param name="pTotalCount">总数</param>
        /// <returns></returns>
        protected virtual IList OnGetListByPaged(BaseParams pBaseParams, out int pTotalCount)
        {

            IList alist = OnGetList(pBaseParams);
            if (string.IsNullOrEmpty(pBaseParams.Top))
            {
                pTotalCount = GetRecordCount(pBaseParams);
            }
            else
            {
                pTotalCount = alist.Count;
            }
            return alist;
        }

        /// <summary>
        /// 根据查询参数返回BaseModel的记录
        /// </summary>
        /// <param name="pBaseParams">参数 </param>
        /// <param name="pTotalCount">总数</param>
        /// <returns></returns>
        protected virtual IList<T> OnGetListByPagedExt(BaseParams pBaseParams, out int pTotalCount)
        {

            IList<T> alist = OnGetListExt(pBaseParams);
            if (string.IsNullOrEmpty(pBaseParams.Top))
            {
                pTotalCount = GetRecordCount(pBaseParams);
            }
            else
            {
                pTotalCount = alist.Count;
            }
            return alist;
        }

        /// <summary>
        /// 取得记录数
        /// </summary>
        /// <param name="pBaseParams">查询参数</param>
        /// <returns></returns>
        public int GetRecordCount(BaseParams pBaseParams)
        {
            string aSqlMap = string.Format("SelectByParameterCount_{0}", pBaseParams.TableName);
            object count = MyMapper.QueryForObject(aSqlMap, pBaseParams);
            return Int32.Parse(count == null ? "0" : count.ToString());
        }

        #endregion
    }
}
