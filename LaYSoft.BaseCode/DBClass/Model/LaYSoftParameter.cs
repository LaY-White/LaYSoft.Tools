using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace LaYSoft.BaseCode.DBClass.Model
{
    public class LaYSoftParameter
    {
        public LaYSoftParameter() { }

        public LaYSoftParameter(string as_PaName, object Obj_Value, ParameterDirection ae_Direction)
        {
            PaName = as_PaName;
            Value = Obj_Value;
            Direction = ae_Direction;
        }

        public LaYSoftParameter(string as_PaName, object Obj_Value)
        {
            PaName = as_PaName;
            Value = Obj_Value;
            Direction = ParameterDirection.Input;
        }

        #region 属性

        public DbType DbType { get; set; }

        /// <summary>
        /// 是否接受 null 值。
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 最大大小（以字节为单位）。
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 源列的名称
        /// </summary>
        public string SourceColumn { get; set; }

        /// <summary>
        /// 源列是否可为 null
        /// </summary>
        public bool SourceColumnNullMapping { get; set; }

        public DataRowVersion SourceVersion { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string PaName { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 参数的类型
        /// </summary>
        public ParameterDirection Direction { get; set; }
        #endregion 

        #region 转换LaYSoftParameter
        /// <summary>
        /// 转换为SqlParameter[]
        /// </summary>
        public static SqlParameter[] GetSqlParameter(List<LaYSoftParameter> as_Pa)
        {
            if (as_Pa == null) return null;
            SqlParameter[] Arr_Pa = new SqlParameter[as_Pa.Count];
            int Num = 0;
            foreach (LaYSoftParameter For_Pa in as_Pa)
            {
                if (For_Pa.Value == null)
                { For_Pa.Value = DBNull.Value; }

                Arr_Pa[Num] = new SqlParameter(For_Pa.PaName, For_Pa.Value);
                Arr_Pa[Num].DbType = For_Pa.DbType;
                Arr_Pa[Num].Direction = For_Pa.Direction;
                Arr_Pa[Num].IsNullable = For_Pa.IsNullable;
                Arr_Pa[Num].Size = For_Pa.Size;
                Arr_Pa[Num].SourceColumn = For_Pa.SourceColumn;
                Arr_Pa[Num].SourceColumnNullMapping = For_Pa.SourceColumnNullMapping;
                Arr_Pa[Num].SourceVersion = For_Pa.SourceVersion;

                Num++;
            }

            return Arr_Pa;
        }

        /// <summary>
        /// 转换为MySqlParameter[]
        /// </summary>
        public static MySqlParameter[] GetMySqlParameter(List<LaYSoftParameter> as_Pa)
        {
            if (as_Pa == null) return null;
            MySqlParameter[] Arr_Pa = new MySqlParameter[as_Pa.Count];
            int Num = 0;
            foreach (LaYSoftParameter For_Pa in as_Pa)
            {
                if (For_Pa.Value == null)
                { For_Pa.Value = DBNull.Value; }

                Arr_Pa[Num] = new MySqlParameter(For_Pa.PaName, For_Pa.Value);
                Arr_Pa[Num].DbType = For_Pa.DbType;
                Arr_Pa[Num].Direction = For_Pa.Direction;
                Arr_Pa[Num].IsNullable = For_Pa.IsNullable;
                Arr_Pa[Num].Size = For_Pa.Size;
                Arr_Pa[Num].SourceColumn = For_Pa.SourceColumn;
                Arr_Pa[Num].SourceColumnNullMapping = For_Pa.SourceColumnNullMapping;
                Arr_Pa[Num].SourceVersion = For_Pa.SourceVersion;

                Num++;
            }
            return Arr_Pa;
        }

        /// <summary>
        /// 转换为OracleParameter[]
        /// </summary>
        public static OracleParameter[] GetOracleParameter(List<LaYSoftParameter> as_Pa)
        {
            if (as_Pa == null) return null;
            OracleParameter[] Arr_Pa = new OracleParameter[as_Pa.Count];
            int Num = 0;
            foreach (LaYSoftParameter For_Pa in as_Pa)
            {
                if (For_Pa.Value == null)
                { For_Pa.Value = DBNull.Value; }

                Arr_Pa[Num] = new OracleParameter(For_Pa.PaName, For_Pa.Value);
                Arr_Pa[Num].DbType = For_Pa.DbType;
                Arr_Pa[Num].Direction = For_Pa.Direction;
                Arr_Pa[Num].IsNullable = For_Pa.IsNullable;
                Arr_Pa[Num].Size = For_Pa.Size;
                Arr_Pa[Num].SourceColumn = For_Pa.SourceColumn;
                Arr_Pa[Num].SourceColumnNullMapping = For_Pa.SourceColumnNullMapping;
                Arr_Pa[Num].SourceVersion = For_Pa.SourceVersion;

                Num++;
            }
            return Arr_Pa;
        }

        /// <summary>
        /// 转换为OleDbParameter[]
        /// </summary>
        public static OleDbParameter[] GetOleDbParameter(List<LaYSoftParameter> as_Pa)
        {
            if (as_Pa == null) return null;
            OleDbParameter[] Arr_Pa = new OleDbParameter[as_Pa.Count];
            int Num = 0;
            foreach (LaYSoftParameter For_Pa in as_Pa)
            {
                if (For_Pa.Value == null)
                { For_Pa.Value = DBNull.Value; }

                Arr_Pa[Num] = new OleDbParameter(For_Pa.PaName, For_Pa.Value);
                Arr_Pa[Num].DbType = For_Pa.DbType;
                Arr_Pa[Num].Direction = For_Pa.Direction;
                Arr_Pa[Num].IsNullable = For_Pa.IsNullable;
                Arr_Pa[Num].Size = For_Pa.Size;
                Arr_Pa[Num].SourceColumn = For_Pa.SourceColumn;
                Arr_Pa[Num].SourceColumnNullMapping = For_Pa.SourceColumnNullMapping;
                Arr_Pa[Num].SourceVersion = For_Pa.SourceVersion;

                Num++;
            }
            return Arr_Pa;
        }

        /// <summary>
        /// 转换为SQLiteParameter[]
        /// </summary>
        public static SQLiteParameter[] GetSQLiteParameter(List<LaYSoftParameter> as_Pa)
        {
            if (as_Pa == null) return null;
            SQLiteParameter[] Arr_Pa = new SQLiteParameter[as_Pa.Count];
            int Num = 0;
            foreach (LaYSoftParameter For_Pa in as_Pa)
            {
                if (For_Pa.Value == null)
                { For_Pa.Value = DBNull.Value; }

                Arr_Pa[Num] = new SQLiteParameter(For_Pa.PaName, For_Pa.Value);
                Arr_Pa[Num].DbType = For_Pa.DbType;
                Arr_Pa[Num].Direction = For_Pa.Direction;
                Arr_Pa[Num].IsNullable = For_Pa.IsNullable;
                Arr_Pa[Num].Size = For_Pa.Size;
                Arr_Pa[Num].SourceColumn = For_Pa.SourceColumn;
                Arr_Pa[Num].SourceColumnNullMapping = For_Pa.SourceColumnNullMapping;
                Arr_Pa[Num].SourceVersion = For_Pa.SourceVersion;
                Num++;
            }
            return Arr_Pa;
        }

        /// <summary>
        /// 转换为List<LaYSoftParameter>
        /// </summary>
        public static List<LaYSoftParameter> GetLaYSoftParameter(DbParameterCollection as_Pa)
        {
            List<LaYSoftParameter> List_Pa = new List<LaYSoftParameter>();
            foreach (DbParameter For_Pa in as_Pa)
            {
                LaYSoftParameter Pa = new LaYSoftParameter();
                Pa.PaName = For_Pa.ParameterName;
                Pa.Value = For_Pa.Value;

                Pa.DbType = For_Pa.DbType;
                Pa.Direction = For_Pa.Direction;
                Pa.IsNullable = For_Pa.IsNullable;
                Pa.Size = For_Pa.Size;
                Pa.SourceColumn = For_Pa.SourceColumn;
                Pa.SourceColumnNullMapping = For_Pa.SourceColumnNullMapping;
                Pa.SourceVersion = For_Pa.SourceVersion;
                List_Pa.Add(Pa);
            }

            if (List_Pa.Count == 0)
                return null;
            else
                return List_Pa;
        }

    
        #endregion 
    }
}
