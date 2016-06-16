using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using DGDP.Qtone.BaseCore.IBLL;
using DGDP.Qtone.BaseCore.Model;
using DGDP.Qtone.BaseCore.Params;
using DGDP.Qtone.BaseCore.IDAL;
using System.Data.SqlClient;

namespace DGDP.Qtone.BaseCore.BLL
{
    public abstract class BaseBLL<T> : IBaseBLL<T> where T : BaseModel
    {
        protected abstract IBaseDAO<T> BaseDaoInstance
        {
            get;
        }

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
            pModel.CreateTime = DateTime.Now;
            pModel.ModifyTime = DateTime.Now;
            
            return OnAdd(pModel);
        }

        /// <summary>
        /// �޸�BaseModel
        /// </summary>
        /// <remarks>�޸�BaseModel</remarks>
        /// <param name="pModel">ʵ�����</param>
        public void Update(T pModel)
        {
            pModel.ModifyTime = DateTime.Now;
            OnUpdate(pModel);
        }

        /// <summary>
        /// ɾ��BaseModel
        /// </summary>
        /// <remarks>ɾ��BaseModel</remarks>
        /// <param name="pModel">ʵ�����</param>
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
        /// <param name="pParamsModel">��ѯ����ʵ����pBaseParams</param>
        /// <returns>����������IList����</returns>
        public IList GetList( BaseParams pParamsModel)
        {
            ///ToDo
            return OnGetList(pParamsModel);
        }

        public IList GetListByPaged(BaseParams pParamsModel, out int pTotalCount)
        {
            return OnGetListByPaged(pParamsModel,out pTotalCount);
        }

        /// <summary>
        /// ���ݲ�ѯ��������BaseModel�ļ�¼
        /// </summary>
        /// <remarks>���ݲ�ѯ��������BaseModel�ļ�¼</remarks>
        /// <param name="pModel">ʵ�����</param>
        /// <param name="pParamsModel">��ѯ����ʵ����pBaseParams</param>
        /// <returns>����������IList����</returns>
        public IList<T> GetListExt(BaseParams pParamsModel)
        {
            ///ToDo
            return OnGetListExt(pParamsModel);
        }

        public IList<T> GetListByPagedExt(BaseParams pParamsModel, out int pTotalCount)
        {
            return OnGetListByPagedExt(pParamsModel, out pTotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public  virtual bool IsExists(T pModel)
        {
            return BaseDaoInstance.IsExists(pModel);
        }


        /// <summary>
        /// ȡ�ü�¼��
        /// </summary>
        /// <param name="pBaseParams">��ѯ����</param>
        /// <returns></returns>
        public int GetRecordCount(BaseParams pBaseParams)
        {
            return BaseDaoInstance.GetRecordCount(pBaseParams);
        }
        #endregion

        #region �ṩ������д
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pModel"></param>
        /// <returns></returns>
        protected virtual  T OnGetObject(T pModel)
        {
            return BaseDaoInstance.GetObject(pModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pModel"></param>
        /// <returns></returns>
        protected virtual object OnAdd(T pModel)
        {
            try
            {
                return BaseDaoInstance.Add(pModel);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    throw new Exception("��ܰ��ʾ������Ϣ�Ѿ����ڣ�", ex);
                }
                else if (ex.Number == 2601)
                {
                    throw new Exception("��ܰ��ʾ������Ϣ�Ѿ����ڣ�", ex);
                }
                else
                {
                    throw new Exception("��ܰ��ʾ�����ʧ�ܣ�ʧ��ԭ��" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pModel"></param>
        protected virtual void OnUpdate(T pModel)
        {
            try
            {
                BaseDaoInstance.Update(pModel);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    throw new Exception("��ܰ��ʾ������Ϣ�Ѿ����ڣ�", ex);
                }
                else if (ex.Number == 2601)
                {
                    throw new Exception("��ܰ��ʾ������Ϣ�Ѿ����ڣ�", ex);
                }
                else
                {
                    throw new Exception("��ܰ��ʾ�����ʧ�ܣ�ʧ��ԭ��" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pModel"></param>
        protected virtual void OnDelete(T pModel)
        {
            BaseDaoInstance.Delete(pModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pModel"></param>
        /// <param name="pParamsModel"></param>
        /// <returns></returns>
        protected virtual IList OnGetList(BaseParams pParamsModel)
        {
            return BaseDaoInstance.GetList(pParamsModel);
        }

        protected virtual IList OnGetListByPaged(BaseParams pParamsModel, out int pTotalCount)
        {
            return BaseDaoInstance.GetListByPaged(pParamsModel,out pTotalCount);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pModel"></param>
        /// <param name="pParamsModel"></param>
        /// <returns></returns>
        protected virtual IList<T> OnGetListExt(BaseParams pParamsModel)
        {
            return BaseDaoInstance.GetListExt(pParamsModel);
        }

        protected virtual IList<T> OnGetListByPagedExt(BaseParams pParamsModel, out int pTotalCount)
        {
            return BaseDaoInstance.GetListByPagedExt(pParamsModel, out pTotalCount);
        }

        #endregion
    }
}
