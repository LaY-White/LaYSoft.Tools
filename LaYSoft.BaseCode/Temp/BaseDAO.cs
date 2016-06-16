/***************************************************************************
 * 
 *       ���ܣ�     ���ݿ����ʻ���
 *       ���ߣ�     
 *       ���ڣ�     
 * 
 *       �޸����ڣ�2010-3-24
 *       �޸��ˣ�  �����
 *       �޸����ݣ�ʵ����IbatisNet��ȡ��̬����ӳ���ļ�
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
        /// Ibatisnet��Mapper��
        /// </summary>
        public virtual ISqlMapper MyMapper
        {
            get { return _MyMapper; }
        }

        /// <summary>
        /// ��������
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
        /// �ύ����
        /// </summary>
        public void CommitTransaction()
        {
            this.MyMapper.CommitTransaction();
        }
        /// <summary>
        /// �ع�����
        /// </summary>
        public void RollBackTransaction()
        {
            this.MyMapper.RollBackTransaction();
        }
        /// <summary>
        /// �������
        /// </summary>
        protected void FlushCaches()
        {
            this.MyMapper.FlushCaches();
        }

        #endregion

        #region ������������
        
       
        /// <summary>
        /// ����Ψһ���Եõ�BaseModel
        /// </summary>
        /// <remarks>����Ψһ���Եõ�BaseModel</remarks>
        /// <param name="pModel">ʵ����</param>
        /// <returns>BaseModel</returns>
        public T GetObject(T pModel)
        {
            ///ToDo
            return OnGetObject(pModel);
        }

        /// <summary>
        /// ����,����ֵΪBaseModelΨһ���Ե�ֵ
        /// </summary>
        /// <remarks>����,����ֵΪBaseModelΨһ���Ե�ֵ</remarks>
        /// <param name="pModel">pModel</param>
        /// <returns>����ֵΪBaseModelΨһ���Ե�ֵ</returns>
        public object Add(T pModel)
        {
            ///ToDo
            
            return  OnAdd(pModel);
        }

        /// <summary>
        /// �޸�BaseModel
        /// </summary>
        /// <remarks>�޸�BaseModel</remarks>
        /// <param name="pModel">ʵ�����</param>
        /// <returns></returns>
        public void Update(T pModel)
        {
            ///ToDo
            OnUpdate(pModel);
        }

        /// <summary>
        /// ɾ��BaseModel
        /// </summary>
        /// <remarks>ɾ��BaseModel</remarks>
        /// <param name="pModel">ʵ�����</param>
        /// <returns></returns>
        public void Delete(T pModel)
        {
            ///ToDo
            OnDelete(pModel);
        }


        /// <summary>
        /// ���ݲ�ѯ��������BaseModel�ļ�¼
        /// </summary>
        /// <remarks>���ݲ�ѯ��������BaseModel�ļ�¼</remarks>
        /// <param name="pModel">ʵ�����</param>
        /// <param name="pBaseParams">��ѯ����ʵ����pBaseParams</param>
        /// <returns>����������IList����</returns>
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
        /// ���ݷ�ҳ������ȡ�б���Ϣ
        /// </summary>
        /// <param name="pParamsModel">��������</param>
        /// <param name="pTotalCount">��������</param>
        /// <returns>�����б���Ϣ</returns>
        public IList GetListByPaged(BaseParams pParamsModel, out int pTotalCount)
        {
            return OnGetListByPaged(pParamsModel,out pTotalCount);
        }

        /// <summary>
        /// ���ݷ�ҳ������ȡ�б���Ϣ
        /// </summary>
        /// <param name="pParamsModel">��������</param>
        /// <param name="pTotalCount">��������</param>
        /// <returns>�����б���Ϣ</returns>
        public IList<T> GetListByPagedExt(BaseParams pParamsModel, out int pTotalCount)
        {
            return OnGetListByPagedExt(pParamsModel, out pTotalCount);
        }

        /// <summary>
        /// �Ƿ���ڸö���
        /// </summary>
        /// <param name="pModel">ʵ����</param>
        /// <returns></returns>
        public virtual bool IsExists(T pModel)
        {
            //TODO:
            throw new NotImplementedException();
        }


        /// <summary>
        /// ����ִ�в�����ȡ����ʱ��Sql
        /// </summary>
        /// <param name="pStatementID">StatentmentID</param>
        /// <param name="pParams">��ѯ����</param>
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
                result = "SqlMapper���ݲ�ѯ������ȡSQL�������쳣: " + ex.Message;

                throw new Exception(result);
            }

            return result;
        }

        /// <summary>
        /// ��ȡ SqlCommand 
        /// </summary>
        /// <param name="pStatementID">pStatementID</param>
        /// <param name="pParams">��ѯ����</param>
        /// <param name="pCommandType">�洢���̻���Sql</param>
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
            command.CommandTimeout = 240;//Ĭ��Ϊ30��
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
        /// ���ݲ�������DataSet
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
        /// ���ݲ�������DataSet
        /// </summary>
        /// <param name="pStatementID">StatementID</param>
        /// <param name="pParams">��ѯ����</param>
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
        /// ���ݲ�������IDbCommand����
        /// </summary>
        /// <param name="pStatementID">StatementID</param>
        /// <param name="pParams">��ѯ����</param>
        /// <returns></returns>
        public IDbCommand GetCommand(string pStatementID, object pParams)
        {
            IDbCommand command = MyMapper.LocalSession.CreateCommand(CommandType.Text);
            command.CommandTimeout = 240;//Ĭ��Ϊ30��
            command.CommandText = this.GetRuntimeSql(pStatementID, pParams);
            return command;
        }

        /// <summary>
        /// ���ݲ�������IDbCommand����
        /// </summary>
        /// <param name="pSql">SQL</param>
        /// <returns></returns>
        public IDbCommand GetCommand(string pSql)
        {
            IDbCommand command = MyMapper.LocalSession.CreateCommand(CommandType.Text);
            command.CommandTimeout = 240;//Ĭ��Ϊ30��
            command.CommandText = pSql;
            return command;
        }

        /// <summary>
        /// ���ݲ������ص�һ��DataTable
        /// </summary>
        /// <param name="pStatementID">StatementID</param>
        /// <param name="pParams">��ѯ����</param>
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
        /// ���ݲ������ص�һ��DataTable
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
        /// ���ݲ������ص�һ��DataTable
        /// </summary>
        /// <param name="pStatementID">XML��Ӧ��SQL�ؼ���ID</param>
        /// <param name="pParams">��ѯ����</param>
        /// <param name="pOutParams">�������</param>
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
        /// ���ݲ�������DataSet
        /// </summary>
        /// <param name="pStatementID">XML��Ӧ��SQL�ؼ���ID</param>
        /// <param name="pParams">��ѯ����</param>
        /// <param name="pOutParams">�������</param>
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

        #region �ṩ������д

        /// <summary>
        /// ����Ψһ���Եõ�BaseModel
        /// </summary>
        /// <remarks>����Ψһ���Եõ�BaseModel</remarks>
        /// <param name="pModel">ʵ����</param>
        /// <returns>BaseModel</returns>
        protected virtual T OnGetObject(T pModel)
        {
            return (T)MyMapper.QueryForObject("SelectObject_" + pModel.TableName, pModel.ID);
        }

        /// <summary>
        /// ����,����ֵΪBaseModelΨһ���Ե�ֵ
        /// </summary>
        /// <remarks>����,����ֵΪBaseModelΨһ���Ե�ֵ</remarks>
        /// <param name="pModel">pModel</param>
        /// <returns>����ֵΪBaseModelΨһ���Ե�ֵ</returns>
        protected virtual object OnAdd(T pModel)
        {
            string aSqlMap = string.Format("Insert_{0}", pModel.TableName);
            pModel.ID =  MyMapper.Insert(aSqlMap, pModel);
            return pModel.ID;
        }

        /// <summary>
        /// �޸�BaseModel
        /// </summary>
        /// <remarks>�޸�BaseModel</remarks>
        /// <param name="pModel">ʵ�����</param>
        /// <returns></returns>
        protected virtual void OnUpdate(T pModel)
        {
            string aSqlMap = string.Format("Update_{0}", pModel.TableName);
            MyMapper.Update(aSqlMap, pModel);
        }

        /// <summary>
        /// ɾ��BaseModel
        /// </summary>
        /// <remarks>ɾ��BaseModel</remarks>
        /// <param name="pModel">ʵ�����</param>
        /// <returns></returns>
        protected virtual void OnDelete(T pModel)
        {
            string aSqlMap = string.Format("Delete_{0}", pModel.TableName);
            MyMapper.Delete(aSqlMap, pModel.ID);
        }


        /// <summary>
        /// ���ݲ�ѯ��������BaseModel�ļ�¼
        /// </summary>
        /// <remarks>���ݲ�ѯ��������BaseModel�ļ�¼</remarks>
        /// <param name="pModel">ʵ�����</param>
        /// <param name="pBaseParams">��ѯ����ʵ����pBaseParams</param>
        /// <returns>����������IList����</returns>
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
                        disablePropertyList.Add(resultProperty);//�ӵ�����¼���Ա��Ժ�ָ�
                        resultMap.Properties.Remove(resultProperty);//��ʱ�Ƴ��ý������
                    }
                    
                    IList resultList = MyMapper.QueryForList(aSqlMap, pBaseParams);
                    //��ѯ��Ϻ�ָ�ԭ����ResultMap����������
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
        /// ���ݲ�ѯ��������BaseModel�ļ�¼
        /// </summary>
        /// <remarks>���ݲ�ѯ��������BaseModel�ļ�¼</remarks>
        /// <param name="pModel">ʵ�����</param>
        /// <param name="pBaseParams">��ѯ����ʵ����pBaseParams</param>
        /// <returns>����������IList����</returns>
        protected virtual IList<T> OnGetListExt(BaseParams pBaseParams)
        {
            string aSqlMap = string.Format("SelectByParameter_{0}", pBaseParams.TableName);
            string strSQL = GetRuntimeSql(aSqlMap, pBaseParams);
            return MyMapper.QueryForList<T>(aSqlMap, pBaseParams);
        }

        /// <summary>
        /// ���ݲ�ѯ��������BaseModel�ļ�¼
        /// </summary>
        /// <param name="pBaseParams">���� </param>
        /// <param name="pTotalCount">����</param>
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
        /// ���ݲ�ѯ��������BaseModel�ļ�¼
        /// </summary>
        /// <param name="pBaseParams">���� </param>
        /// <param name="pTotalCount">����</param>
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
        /// ȡ�ü�¼��
        /// </summary>
        /// <param name="pBaseParams">��ѯ����</param>
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
