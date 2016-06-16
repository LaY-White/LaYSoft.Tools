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
            pModel.CreateTime = DateTime.Now;
            pModel.ModifyTime = DateTime.Now;
            
            return OnAdd(pModel);
        }

        /// <summary>
        /// 修改BaseModel
        /// </summary>
        /// <remarks>修改BaseModel</remarks>
        /// <param name="pModel">实体对象</param>
        public void Update(T pModel)
        {
            pModel.ModifyTime = DateTime.Now;
            OnUpdate(pModel);
        }

        /// <summary>
        /// 删除BaseModel
        /// </summary>
        /// <remarks>删除BaseModel</remarks>
        /// <param name="pModel">实体对象</param>
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
        /// <param name="pParamsModel">查询参数实体类pBaseParams</param>
        /// <returns>符合条件的IList集合</returns>
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
        /// 根据查询参数返回BaseModel的记录
        /// </summary>
        /// <remarks>根据查询参数返回BaseModel的记录</remarks>
        /// <param name="pModel">实体基类</param>
        /// <param name="pParamsModel">查询参数实体类pBaseParams</param>
        /// <returns>符合条件的IList集合</returns>
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
        /// 取得记录数
        /// </summary>
        /// <param name="pBaseParams">查询参数</param>
        /// <returns></returns>
        public int GetRecordCount(BaseParams pBaseParams)
        {
            return BaseDaoInstance.GetRecordCount(pBaseParams);
        }
        #endregion

        #region 提供子类重写
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
                    throw new Exception("温馨提示：该信息已经存在！", ex);
                }
                else if (ex.Number == 2601)
                {
                    throw new Exception("温馨提示：该信息已经存在！", ex);
                }
                else
                {
                    throw new Exception("温馨提示：添加失败，失败原因：" + ex.Message);
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
                    throw new Exception("温馨提示：该信息已经存在！", ex);
                }
                else if (ex.Number == 2601)
                {
                    throw new Exception("温馨提示：该信息已经存在！", ex);
                }
                else
                {
                    throw new Exception("温馨提示：添加失败，失败原因：" + ex.Message);
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
