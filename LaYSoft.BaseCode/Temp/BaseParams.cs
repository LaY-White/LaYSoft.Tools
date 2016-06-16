/***************************************************************************
 * 
 *       ���ܣ�     ����ʵ������࣬�ѹ��ԵĲ������Գ������
 *       ���ߣ�     ������
 *       ���ڣ�     2009/12/30
 * 
 *       �޸����ڣ�  
 *       �޸��� �� 
 *       �޸����ݣ�
 * *************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace DGDP.Qtone.BaseCore.Params
{
    public abstract class BaseParams
    {
        /// <summary>
        /// ��ȡ�������ԣ�����ʵ��
        /// </summary>
        public abstract string TableName
        {
            get;
            set;
        }

        private object _id;
        /// <summary>
        /// ����ID
        /// </summary>
        public object ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _top;
        /// <summary>
        /// Sql�������������
        /// </summary>
        public string Top
        {
            get
            {
                return _top;
            }
            set { _top = value; }
        }

        

        /// <summary>
        /// ��������
        /// </summary>
        private string _orderBy;
        /// <summary>
        /// ��������
        /// </summary>
        public string OrderBy
        {
            get
            {
                if (string.IsNullOrEmpty(_orderBy))
                {
                    return "1";
                }
                else
                {
                    return _orderBy;
                }
            }
            set { _orderBy = value; }
        }

        private string _beginRow;

        /// <summary>
        /// ��ʼ��
        /// </summary>
        public string BeginRow
        {
            get { return _beginRow; }
            set { _beginRow = value; }
        }

        private string _endRow;

        /// <summary>
        /// ������ 
        /// </summary>
        public string EndRow
        {
            get { return _endRow; }
            set { _endRow = value; }
        }

        private string _createUserID;
        /// <summary>
        /// ������ID
        /// </summary>
        public string CreateUserID
        {
            get { return _createUserID; }
            set { _createUserID = value; }
        }

        private string _createUserIDFrom;
        /// <summary>
        /// ������ID
        /// </summary>
        public string CreateUserIDFrom
        {
            get { return _createUserIDFrom; }
            set { _createUserIDFrom = value; }
        }

        private string _createUserIDTo;
        /// <summary>
        /// ������ID
        /// </summary>
        public string CreateUserIDTo
        {
            get { return _createUserIDTo; }
            set { _createUserIDTo = value; }
        }


        private DateTime? _createTime;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime? CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        private DateTime? _createTimeFrom;
        /// <summary>
        /// ����ʱ�俪ʼ
        /// </summary>
        public DateTime? CreateTimeFrom
        {
            get { return _createTimeFrom; }
            set { _createTimeFrom = value; }
        }

        private DateTime? _createTimeTo;
        /// <summary>
        /// ����ʱ�����
        /// </summary>
        public DateTime? CreateTimeTo
        {
            get
            {
                if(_createTimeTo.HasValue)
                {
                    _createTimeTo.Value.Date.AddDays(1).AddSeconds(-1);
                }

                return _createTimeTo;
            }
            set { _createTimeTo = value; }
        }

        private string _modifyUserID;
        /// <summary>
        /// ����޸���ID
        /// </summary>
        public string ModifyUserID
        {
            get { return _modifyUserID; }
            set { _modifyUserID = value; }
        }


        private string _modifyUserIDFrom;
        /// <summary>
        /// ������ID
        /// </summary>
        public string ModifyUserIDFrom
        {
            get { return _modifyUserIDFrom; }
            set { _modifyUserIDFrom = value; }
        }

        private string _modifyUserIDTo;
        /// <summary>
        /// ������ID
        /// </summary>
        public string ModifyUserIDTo
        {
            get { return _modifyUserIDTo; }
            set { _modifyUserIDTo = value; }
        }


        private DateTime? _modifyTime;
        /// <summary>
        /// ����޸�ʱ��
        /// </summary>
        public DateTime? ModifyTime
        {
            get { return _modifyTime; }
            set { _modifyTime = value; }
        }


        private DateTime? _modifyTimeFrom;
        /// <summary>
        /// ����޸�ʱ�俪ʼ
        /// </summary>
        public DateTime? ModifyTimeFrom
        {
            get { return _modifyTimeFrom; }
            set { _modifyTimeFrom = value; }
        }


        private DateTime? _modifyTimeTo;
        /// <summary>
        /// ����޸�ʱ�����
        /// </summary>
        public DateTime? ModifyTimeTo
        {
            get
            {
                if (_modifyTimeTo.HasValue)
                {
                    _modifyTimeTo.Value.Date.AddDays(1).AddSeconds(-1);
                }

                return _modifyTimeTo;
            }
            set { _modifyTimeTo = value; }
        }

        private string _sqlWhere;
        /// <summary>
        /// ��ѯ����
        /// </summary>
        public string SqlWhere
        {
            get { return _sqlWhere; }
            set { _sqlWhere = value; }
        }

        private string _DisablePropertys;
        /// <summary>
        /// ���õ��������У���Ӣ�Ķ��ŷָ�
        /// </summary>
        public string DisablePropertys
        {
            get { return _DisablePropertys; }
            set { _DisablePropertys = value; }
        }

        /// <summary>
        /// ��дToString
        /// </summary>
        /// <returns></returns>
        public object  Trim()
        {
             return ModelToTrim(this);
        }

        /// <summary>
        /// Modelת��Ϊ�ַ����ķ���
        /// </summary>
        /// <param name="model">Ҫת����Model</param>
        /// <returns></returns>
        private static object ModelToTrim(object model)
        {
            // TODO:
            // ��չ���迼��ʵ�����ְ�������ʵ��������
           

            object objValue = null;
            int i=0;
            if (model != null)
            {
                foreach (PropertyInfo property in model.GetType().GetProperties())
                {
                    objValue = property.GetValue(model, null);
                    if (objValue != null)
                    {

                        try
                        {
                           model.GetType().GetProperty(property.Name).SetValue(model,objValue.ToString().Trim(), null);
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

    }
}
