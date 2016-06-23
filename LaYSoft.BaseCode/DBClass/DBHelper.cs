using LaYSoft.BaseCode.DBClass.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LaYSoft.BaseCode.DBClass
{
    public abstract class DBHelper
    {
        #region 参数

        public abstract string ConnString { set; get; }

        public abstract bool is_Trans { get; }

        public ConnectionArg ConnectionArgModel { get; set; }

        public abstract char PaSqlSign { get; }            //获取语句参数符号
        public abstract char PaNameSign { get; }           //获取传入参数符号

        private int _ErrCode = 0;
        public int ErrCode { set { _ErrCode = value; } get { return _ErrCode; } }

        private string _ErrSQL = "";
        public string ErrSQL { set { _ErrSQL = value; } get { return _ErrSQL; } }

        private string _ErrStr = "";
        public string ErrStr { set { _ErrStr = value; } get { return _ErrStr; } }

        #endregion

        /// <summary>
        /// 清空错误内容
        /// </summary>
        protected void ClearErr()
        {
            ErrCode = 0;
            ErrSQL = "";
            ErrStr = "";
        }

        public abstract void Open();                    //打开数据库连接
        public abstract void Close();                   //关闭数据库连接
        public abstract void BeginTrans();              //开始一个事务	 
        public abstract void CommitTrans();             //提交一个事务
        public abstract void RollbackTrans();           //回滚一个事务

        //执行Sql语句，返回Int 
        public abstract int ExecuteNonQuery(CommandType cmdType, string strSql, ref List<LaYSoftParameter> Pa);
        public abstract int ExecuteNonQuery(CommandType cmdType, string strSql);
        public abstract int ExecuteNonQuery(string strSql, ref  List<LaYSoftParameter> Pa);
        public abstract int ExecuteNonQuery(string strSql);

        //执行Sql，返回DataTable
        public abstract DataTable ExecuteDataTable(CommandType cmdType, string strSql, ref  List<LaYSoftParameter> Pa);
        public abstract DataTable ExecuteDataTable(CommandType cmdType, string strSql);
        public abstract DataTable ExecuteDataTable(string strSql, ref  List<LaYSoftParameter> Pa);
        public abstract DataTable ExecuteDataTable(string strSql);

        //执行Sql，返回string
        public abstract string ExecuteScalar(CommandType cmdType, string strSql, ref  List<LaYSoftParameter> Pa);
        public abstract string ExecuteScalar(CommandType cmdType, string strSql);
        public abstract string ExecuteScalar(string strSql, ref  List<LaYSoftParameter> Pa);
        public abstract string ExecuteScalar(string strSql);

        //执行Sql，返回int
        public abstract int ExecuteScalarNum(CommandType cmdType, string strSql, ref  List<LaYSoftParameter> Pa);
        public abstract int ExecuteScalarNum(CommandType cmdType, string strSql);
        public abstract int ExecuteScalarNum(string strSql, ref  List<LaYSoftParameter> Pa);
        public abstract int ExecuteScalarNum(string strSql);
    }
}
