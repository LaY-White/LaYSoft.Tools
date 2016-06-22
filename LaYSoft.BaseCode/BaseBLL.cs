using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LaYSoft.BaseCode
{
    public abstract class BaseBLL<T> where T : BaseModel, new()
    {
        protected abstract BaseDAO<T> BaseDaoInstance
        {
            get;
        }

        #region 提供公共方法重写

        /// <summary>
        /// 新增BaseModel，返回操作影响的记录条数
        /// </summary>
        public virtual object Add(T pModel)
        {
            try
            {
                pModel.CreateTime = DateTime.Now;
                pModel.ModifyTime = DateTime.Now;

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
        /// 修改BaseModel，返回操作影响的记录条数
        /// </summary>
        public virtual void Update(T pModel)
        {
            try
            {
                pModel.ModifyTime = DateTime.Now;

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
        /// 删除BaseModel，返回操作影响的记录条数
        /// </summary>
        public virtual void Delete(T pModel)
        {
            BaseDaoInstance.Delete(pModel);
        }

        /// <summary>
        /// 根据唯一属性得到BaseModel
        /// </summary>
        /// <remarks>根据唯一属性得到BaseModel</remarks>
        /// <param name="pModel">实体类</param>
        /// <returns>BaseModel</returns>
        public virtual T GetObject(T pModel)
        {
            return BaseDaoInstance.GetObject(pModel);
        }


        /// <summary>
        /// 根据查询参数返回BaseModel的记录
        /// </summary>
        /// <remarks>根据查询参数返回BaseModel的记录</remarks>
        /// <param name="pModel">实体基类</param>
        /// <param name="pBaseParams">查询参数实体类pBaseParams</param>
        /// <returns>符合条件的IList集合</returns>
        public virtual IList<T> GetList(BaseParams pParamsModel)
        {
            return BaseDaoInstance.GetList(pParamsModel);
        }

        /// <summary>
        /// 根据查询参数返回BaseModel的记录
        /// </summary>
        /// <param name="pBaseParams">参数 </param>
        /// <param name="pTotalCount">总数</param>
        /// <returns></returns>
        public virtual IList<T> GetListByPaged(BaseParams pParamsModel, out int pTotalCount)
        {
            return BaseDaoInstance.GetListByPaged(pParamsModel, out pTotalCount);
        }

        #endregion

        #region 公共基础方法
        /// <summary>
        /// 是否存在该对象
        /// </summary>
        /// <param name="pModel">实体类</param>
        /// <returns></returns>
        public bool IsExists(T pModel)
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
    }
}
