using LaYSoft.BaseCode.DBClass.Instance;
using LaYSoft.BaseCode.DBClass.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace LaYSoft.BaseCode.DBClass
{
    public class SqlHelper
    {
        private static string _CONN_STR;
        /// <summary>
        /// 数据连接字段
        /// 首先 CONN_STR 的设置,
        /// 最后 ConnectionStrings["ConnectionString"] 的数据,
        /// </summary>
        public static string CONN_STR
        {
            set { _CONN_STR = value; }
            get
            {
                //如果已经设置CONN_STR,则直接跳过
                if (String.IsNullOrEmpty(_CONN_STR))
                {
                    return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                }

                return _CONN_STR;
            }
        }

        private static DBTypeEnum _Dbtype = DBTypeEnum.None;
        /// <summary>
        /// SqlHelper的连接类型,
        /// 首先 ConnectionStrings["dbtype"] 的数据,
        /// 最后 默认None
        /// </summary>
        public static DBTypeEnum Dbtype
        {
            set { _Dbtype = value; }
            get
            {
                if (_Dbtype != DBTypeEnum.None)
                { return _Dbtype; }

                return DBClassTools.GetDBType(ConfigurationManager.ConnectionStrings["DBType"].ToString());
            }
        }

        /// <summary>
        /// 获取一个数据连接的实例
        /// </summary>
        /// <param name="DBType">数据连接类型</param>
        public static DBHelper GetDBHelper(DBTypeEnum DBType)
        {
            switch (DBType)
            {
                case DBTypeEnum.MSSql:
                    return new SqlServerHelper();
                case DBTypeEnum.MySql:
                    return new MySqlHelper();
                case DBTypeEnum.Oracle:
                    return new OracleHelper();
                case DBTypeEnum.Access:
                    return new AccseeHelper();
                case DBTypeEnum.SQLite:
                    return new SQLiteHelper();
                default:
                    throw new Exception("请选择正确的DBType");
            }
        }

        //执行Sql语句，返回Int 
        public static int ExecuteNonQuery(string connString, CommandType cmdType, string strSql,ref List<LaYSoftParameter> Pa)
        {
            if (string.IsNullOrEmpty(connString))
            { connString = CONN_STR; }

            DBHelper lnv_Helper = GetDBHelper(Dbtype);

            lnv_Helper.ConnString = connString;
            int li_rel = lnv_Helper.ExecuteNonQuery(cmdType, strSql, ref Pa);

            lnv_Helper.Close();

            if (!String.IsNullOrEmpty(lnv_Helper.ErrStr))
                throw new Exception(lnv_Helper.ErrStr);

            return li_rel;
        }
        public static int ExecuteNonQuery(CommandType cmdType, string strSql, ref  List<LaYSoftParameter> Pa)
        {
            return ExecuteNonQuery(null, cmdType, strSql, ref Pa);
        }
        public static int ExecuteNonQuery(CommandType cmdType, string strSql)
        {
            List<LaYSoftParameter> Pa = null;
            return ExecuteNonQuery(null, cmdType, strSql, ref  Pa);
        }
        public static int ExecuteNonQuery(string strSql, ref List<LaYSoftParameter> Pa)
        {
            return ExecuteNonQuery(null, CommandType.Text, strSql, ref Pa);
        }
        public static int ExecuteNonQuery(string strSql)
        {
            return ExecuteNonQuery( CommandType.Text, strSql);
        }

        //执行Sql，返回DataTable
        public static DataTable ExecuteDataTable(string connString, CommandType cmdType, string strSql, ref  List<LaYSoftParameter> Pa)
        {
            if (string.IsNullOrEmpty(connString))
            { connString = CONN_STR; }

            DBHelper lnv_Helper = GetDBHelper(Dbtype);
            lnv_Helper.ConnString = connString;
            DataTable ldt_rel = lnv_Helper.ExecuteDataTable(cmdType, strSql, ref Pa);
            lnv_Helper.Close();

            if (!String.IsNullOrEmpty(lnv_Helper.ErrStr))
                throw new Exception(lnv_Helper.ErrStr);

            return ldt_rel;
        }
        public static DataTable ExecuteDataTable(CommandType cmdType, string strSql, ref  List<LaYSoftParameter> Pa)
        {
            return ExecuteDataTable(null, cmdType, strSql, ref Pa);
        }
        public static DataTable ExecuteDataTable(CommandType cmdType, string strSql)
        {
            List<LaYSoftParameter> Pa = null;
            return ExecuteDataTable(null, cmdType, strSql, ref  Pa);
        }
        public static DataTable ExecuteDataTable(string strSql, ref  List<LaYSoftParameter> Pa)
        {
            return ExecuteDataTable(null, CommandType.Text, strSql, ref Pa);
        }
        public static DataTable ExecuteDataTable(string strSql)
        {
            return ExecuteDataTable( CommandType.Text, strSql);
        }

        //执行Sql，返回string
        public static string ExecuteScalar(string connString, CommandType cmdType, string strSql, ref  List<LaYSoftParameter> Pa)
        {
            if (string.IsNullOrEmpty(connString))
            { connString = CONN_STR; }

            DBHelper lnv_Helper = GetDBHelper(Dbtype);
            lnv_Helper.ConnString = connString;
            string ls_rel = lnv_Helper.ExecuteScalar(cmdType, strSql, ref Pa);
            lnv_Helper.Close();

            if (!String.IsNullOrEmpty(lnv_Helper.ErrStr))
                throw new Exception(lnv_Helper.ErrStr);

            return ls_rel;
        }
        public static string ExecuteScalar(CommandType cmdType, string strSql, ref  List<LaYSoftParameter> Pa)
        {
            return ExecuteScalar(null, cmdType, strSql, ref Pa);
        }
        public static string ExecuteScalar(CommandType cmdType, string strSql)
        {
            List<LaYSoftParameter> Pa = null;
            return ExecuteScalar(null, cmdType, strSql, ref  Pa);
        }
        public static string ExecuteScalar(string strSql, ref  List<LaYSoftParameter> Pa)
        {
            return ExecuteScalar(null, CommandType.Text, strSql, ref Pa);
        }
        public static string ExecuteScalar(string strSql)
        {
            return ExecuteScalar(CommandType.Text, strSql);
        }

        //执行Sql，返回int
        public static int ExecuteScalarNum(string connString, CommandType cmdType, string strSql, ref  List<LaYSoftParameter> Pa)
        {
            string ls_return = ExecuteScalar(connString, cmdType, strSql, ref Pa);

            try
            { return Convert.ToInt32(ls_return); }
            catch
            { return 0; }
        }
        public static int ExecuteScalarNum(CommandType cmdType, string strSql, ref  List<LaYSoftParameter> Pa)
        {
            return ExecuteScalarNum(null, cmdType, strSql, ref  Pa);
        }
        public static int ExecuteScalarNum(CommandType cmdType, string strSql)
        {
            List<LaYSoftParameter> Pa = null;
            return ExecuteScalarNum(null, cmdType, strSql, ref Pa);
        }
        public static int ExecuteScalarNum(string strSql, ref  List<LaYSoftParameter> Pa)
        {
            return ExecuteScalarNum(null, CommandType.Text, strSql, ref  Pa);
        }
        public static int ExecuteScalarNum(string strSql)
        {
            return ExecuteScalarNum( CommandType.Text, strSql);
        }
    }
}
