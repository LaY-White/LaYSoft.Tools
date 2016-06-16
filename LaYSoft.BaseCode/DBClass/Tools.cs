using LaYSoft.BaseCode.DBClass.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LaYSoft.BaseCode.DBClass
{
    public static class DBClassTools
    {
        public static string GetConnString(DBTypeEnum DBType, ConnectionArg Arg)
        {
            switch (DBType)
            {
                case DBTypeEnum.Access:
                    //IP = "database\\Database.mdb";
                    return "Provider=Microsoft.Jet.OLEDB.4.0;Password=" + Arg.Password + ";User ID=" + Arg.UserName +
                        ";Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory + Arg.IP +
                        ";Mode=Share Deny None";
                case DBTypeEnum.SQLite:
                    //IP = "database\\Database.db";
                    return "Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory + Arg.IP +
                        ";Pooling = true;FailIfMissing = false";
                case DBTypeEnum.MySql:
                    return "Host=" + Arg.IP + ";DataBase=" + Arg.DBName + ";Protocol=TCP;Port=" + Arg.Port +
                        ";Pooling=true;Connection Lifetime=0;User id=" + Arg.UserName +
                        ";Password=" + Arg.Password + ";charset=gbk";
                case DBTypeEnum.MSSql:
                    string Server = Arg.IP;
                    if (Arg.Port != null && Arg.Port != "")
                    { Server += "," + Arg.Port; }
                    return "Server=" + Server + ";User id=" + Arg.UserName +
                        ";Pwd=" + Arg.Password + ";Database=" + Arg.DBName + "";
                case DBTypeEnum.Oracle:
                    return "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + Arg.IP +
                        ")(PORT=" + Arg.Port + "))(CONNECT_DATA=(SERVICE_NAME=" + Arg.DBName +
                        ")));User Id=" + Arg.UserName + ";Password=" + Arg.Password + "";
                default:
                    throw new Exception("请先设置DBType");
            }
        }

        #region DBTypeEnum
        public static DBTypeEnum GetDBType(string as_DBType)
        {
            switch (as_DBType.ToLower())
            {
                case "mssql": return DBTypeEnum.MSSql;
                case "mysql": return DBTypeEnum.MySql;
                case "oracle": return DBTypeEnum.Oracle;
                case "access": return DBTypeEnum.Access;
                case "sqlite": return DBTypeEnum.SQLite;
                case "s": return DBTypeEnum.MSSql;
                case "m": return DBTypeEnum.MySql;
                case "o": return DBTypeEnum.Oracle;
                case "a": return DBTypeEnum.Access;
                case "l": return DBTypeEnum.SQLite;
                default: return DBTypeEnum.None;
            }
        }

        public static string GetDBType(DBTypeEnum DBType)
        {
            switch (DBType)
            {
                case DBTypeEnum.MSSql: return "MSSql";
                case DBTypeEnum.MySql: return "MySql";
                case DBTypeEnum.Oracle: return "Oracle";
                case DBTypeEnum.Access: return "Access";
                case DBTypeEnum.SQLite: return "SQLite";
                default: return "";
            }
        }
        #endregion

        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteSqlLog(string Action, string strSql, string ErrorText)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Sql\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileFullPath = path + DateTime.Now.ToString("yyyy-MM-dd") + "_Sql.txt";
            StringBuilder str = new StringBuilder();
            str.Append("时间:    " + DateTime.Now.ToString() + "\r\n");
            str.Append("Action:  " + Action + " \r\n");
            str.Append("Sql: " + strSql + "\r\n");
            str.Append("Error: " + ErrorText + "\r\n");
            str.Append("-----------------------------------------------------------" + "\r\n  \r\n ");
            StreamWriter sw = default(StreamWriter);
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }

    }
}
