using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using LaYSoft.BaseCode.DBClass.Model;

namespace LaYSoft.BaseCode.DBClass.Instance
{
    internal class MySqlHelper : DBHelper
    {
        private String _ConnString;
        public override string ConnString
        {
            set { _ConnString = value; }
            get
            {
                if (_ConnString == null || _ConnString.Trim() == "")
                {
                    return DBClassTools.GetConnString(DBTypeEnum.MySql, ConnectionArgModel);
                }
                return _ConnString;
            }
        }

        public override bool is_Trans
        {
            get
            {
                if (MyTran != null && MyTran.Connection != null)
                {
                    return true;
                }
                return false;
            }
        }

        public override char PaSqlSign { get { return '@'; } }
        public override char PaNameSign { get { return '@'; } }

        private MySqlCommand MyCmd;
        private MySqlConnection MyConn;
        private MySqlTransaction MyTran;
        private MySqlDataAdapter MyAdp;

        public MySqlHelper() { }

        /// <summary>
        /// 判读Conn是否已经打开
        /// </summary>
        /// <returns></returns>
        private bool SuerConnOpen()
        {
            if (MyConn == null)
            { return false; }
            if (MyConn.State == System.Data.ConnectionState.Open)
            { return true; }

            return false;

        }

        public override void Open()
        {
            try
            {
                if (!SuerConnOpen())
                {
                    MyConn = new MySqlConnection(ConnString);
                    MyConn.Open();

                    MyCmd = new MySqlCommand();
                    MyCmd.CommandTimeout = 600;
                    MyCmd.Connection = MyConn;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
        public override void Close()
        {
            if (SuerConnOpen())
            {
                MyConn.Close();
            }
        }
        public override void BeginTrans()
        {
            if (is_Trans == false)
            {
                MyTran = MyConn.BeginTransaction();
                MyCmd.Transaction = MyTran;
            }
        }
        public override void CommitTrans()
        {
            if (is_Trans == true)
            {
                MyTran.Commit();
            }
        }
        public override void RollbackTrans()
        {
            if (is_Trans == true)
            {
                MyTran.Rollback();
            }
        }

        //执行Sql语句，返回Int 
        public override int ExecuteNonQuery(CommandType cmdType, string strSql, List<LaYSoftParameter> Pa)
        {
            MySqlParameter[] cmdParameters = LaYSoftParameter.GetMySqlParameter(Pa);
            if (!SuerConnOpen())
            { Open(); }

            MyCmd.CommandText = strSql;
            MyCmd.CommandType = cmdType;
            MyCmd.Parameters.Clear();
            if ((cmdParameters != null))
            {
                foreach (MySqlParameter parm in cmdParameters)
                {
                    MyCmd.Parameters.Add(parm);
                }
            }

            try
            {
                int val = MyCmd.ExecuteNonQuery();
                Pa = LaYSoftParameter.GetLaYSoftParameter(MyCmd.Parameters);

                MyCmd.Parameters.Clear();
                ClearErr();
                return val;
            }
            catch (MySqlException ex)
            {
                ErrCode = ex.ErrorCode;
                ErrSQL = strSql;
                ErrStr = ex.Message;
                //写入日志
                DBClassTools.WriteSqlLog("MySql", strSql, ex.Message);

                return -1;
            }
        }
        public override int ExecuteNonQuery(CommandType cmdType, string strSql)
        {
            return ExecuteNonQuery(CommandType.Text, strSql, null);
        }
        public override int ExecuteNonQuery(string strSql, List<LaYSoftParameter> Pa)
        {
            return ExecuteNonQuery(CommandType.Text, strSql, Pa);
        }
        public override int ExecuteNonQuery(string strSql)
        {
            return ExecuteNonQuery(CommandType.Text, strSql, null);
        }

        //执行Sql，返回DataTable
        public override DataTable ExecuteDataTable(CommandType cmdType, string strSql, List<LaYSoftParameter> Pa)
        {
            if (!SuerConnOpen())
            { Open(); }

            MySqlParameter[] cmdParameters = LaYSoftParameter.GetMySqlParameter(Pa);

            MyCmd.CommandText = strSql;
            MyCmd.CommandType = cmdType;
            MyCmd.Parameters.Clear();
            if ((cmdParameters != null))
            {
                foreach (MySqlParameter parm in cmdParameters)
                {
                    MyCmd.Parameters.Add(parm);
                }
            }

            DataTable dt = new DataTable();

            try
            {
                MyAdp = new MySqlDataAdapter(MyCmd);
                MyAdp.Fill(dt);
                Pa = LaYSoftParameter.GetLaYSoftParameter(MyCmd.Parameters);

                MyCmd.Parameters.Clear();
                ClearErr();
            }
            catch (MySqlException ex)
            {
                ErrCode = ex.ErrorCode;
                ErrSQL = strSql;
                ErrStr = ex.Message;
                //写入日志
                DBClassTools.WriteSqlLog("MySql", strSql, ex.Message);
            }
            finally
            {
                MyAdp.Dispose();
            }

            return dt;
        }
        public override DataTable ExecuteDataTable(CommandType cmdType, string strSql)
        {
            return ExecuteDataTable(cmdType, strSql, null);
        }
        public override DataTable ExecuteDataTable(string strSql, List<LaYSoftParameter> Pa)
        {
            return ExecuteDataTable(CommandType.Text, strSql, Pa);
        }
        public override DataTable ExecuteDataTable(string strSql)
        {
            return ExecuteDataTable(CommandType.Text, strSql, null);
        }

        //执行Sql，返回string
        public override string ExecuteScalar(CommandType cmdType, string strSql, List<LaYSoftParameter> Pa)
        {
            if (!SuerConnOpen())
            { Open(); }

            MySqlParameter[] cmdParameters = LaYSoftParameter.GetMySqlParameter(Pa);

            MyCmd.CommandText = strSql;
            MyCmd.CommandType = cmdType;
            MyCmd.Parameters.Clear();
            if ((cmdParameters != null))
            {
                foreach (MySqlParameter parm in cmdParameters)
                {
                    MyCmd.Parameters.Add(parm);
                }
            }

            try
            {
                object val = MyCmd.ExecuteScalar();
                Pa = LaYSoftParameter.GetLaYSoftParameter(MyCmd.Parameters);

                MyCmd.Parameters.Clear();
                ClearErr();
                if (val == null)
                {
                    return "";
                }
                return val.ToString();
            }
            catch (MySqlException ex)
            {
                ErrCode = ex.ErrorCode;
                ErrSQL = strSql;
                ErrStr = ex.Message;
                //写入日志
                DBClassTools.WriteSqlLog("MySql", strSql, ex.Message);
                return "";
            }
        }
        public override string ExecuteScalar(CommandType cmdType, string strSql)
        {
            return ExecuteScalar(cmdType, strSql, null);
        }
        public override string ExecuteScalar(string strSql, List<LaYSoftParameter> Pa)
        {
            return ExecuteScalar(CommandType.Text, strSql, Pa);
        }
        public override string ExecuteScalar(string strSql)
        {
            return ExecuteScalar(CommandType.Text, strSql, null);
        }

        //执行Sql，返回int
        public override int ExecuteScalarNum(CommandType cmdType, string strSql, List<LaYSoftParameter> Pa)
        {
            string ls_return = ExecuteScalar(cmdType, strSql, Pa);

            try
            { return Convert.ToInt32(ls_return); }
            catch
            { return 0; }
        }
        public override int ExecuteScalarNum(CommandType cmdType, string strSql)
        {
            return ExecuteScalarNum(cmdType, strSql, null);
        }
        public override int ExecuteScalarNum(string strSql, List<LaYSoftParameter> Pa)
        {
            return ExecuteScalarNum(CommandType.Text, strSql, Pa);
        }
        public override int ExecuteScalarNum(string strSql)
        {
            return ExecuteScalarNum(CommandType.Text, strSql, null);
        }
    }
}
