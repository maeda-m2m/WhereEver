using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WhereEver.Class
{
    public class Login
    {

        public static DATASET.DataSet.M_UserRow GetM_UserRow(string id, string pw, SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM M_User WHERE (id = @id) and (pw = @pw)";
            da.SelectCommand.Parameters.AddWithValue("@id", id);
            da.SelectCommand.Parameters.AddWithValue("@pw", pw);
            DATASET.DataSet.M_UserDataTable dt = new DATASET.DataSet.M_UserDataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                return dt[0];
            }
            else
            {
                return null;
            }
        }
        public static DATASET.DataSet.M_UserRow GetM_UserRow(SqlConnection schedule)
        {
            SqlDataAdapter da = new SqlDataAdapter("", schedule);
            da.SelectCommand.CommandText =
                "SELECT * FROM M_User";
            DATASET.DataSet.M_UserDataTable dt = new DATASET.DataSet.M_UserDataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
                return dt[0] as DATASET.DataSet.M_UserRow;
            else
                return null;
        }

        public static DATASET.DataSet.M_UserDataTable GetM_UserDataTable(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM M_User";
            DATASET.DataSet.M_UserDataTable dt = new DATASET.DataSet.M_UserDataTable();
            da.Fill(dt);
            return (dt);
        }



        public static void InsertLoginList(DATASET.DataSet.T_LoginListDataTable dt, SqlConnection sql)//ログインするたびにnameとDateをLoginListにInsert
        {
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_LoginList";
            da.InsertCommand = (new SqlCommandBuilder(da)).GetInsertCommand();

            sql.Open();
            SqlTransaction sqltra = sql.BeginTransaction();

            da.SelectCommand.Transaction = da.InsertCommand.Transaction = sqltra;

            da.Update(dt);
            sqltra.Commit();
            sql.Close();

        }

        public static DATASET.DataSet.T_LoginListDataTable GetLoginListDataTable(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_LoginList";
            DATASET.DataSet.T_LoginListDataTable dt = new DATASET.DataSet.T_LoginListDataTable();
            da.Fill(dt);
            return dt;
        }

        public static DATASET.DataSet.T_LoginListRow GetT_LoginListRow(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_LoginList";
            DATASET.DataSet.T_LoginListDataTable dt = new DATASET.DataSet.T_LoginListDataTable();
            da.Fill(dt);
            if (dt.Rows.Count >= 1)
                return dt[0] as DATASET.DataSet.T_LoginListRow;
            else
                return null;
        }

        public static void UpdateLogin(DATASET.DataSet.M_UserDataTable dt, SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "Select * From M_User";
            da.UpdateCommand = (new SqlCommandBuilder(da)).GetUpdateCommand();

            sqlConnection.Open();
            SqlTransaction sql = sqlConnection.BeginTransaction();

            da.SelectCommand.Transaction = da.UpdateCommand.Transaction = sql;

            da.Update(dt);

            sql.Commit();
            sqlConnection.Close();
        }

        internal static DATASET.DataSet.T_LoginListRow UserLoginMAXTime(SqlConnection sqlConnection, string name)//最新LoginDateの取得
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT MAX(Date) as date FROM T_LoginList where name like @i ORDER BY Date DESC ";
            da.SelectCommand.Parameters.AddWithValue("@i", name);
            DATASET.DataSet.T_LoginListDataTable dt = new DATASET.DataSet.T_LoginListDataTable();
            da.Fill(dt);
            return dt[0];
        }
        internal static DATASET.DataSet.T_LogoutListRow UserLogoutMAXTime(SqlConnection sqlConnection, string name)//最新LogoutDateの取得
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT MAX(Date) as date FROM T_LogoutList where name like @i ORDER BY Date DESC ";
            da.SelectCommand.Parameters.AddWithValue("@i", name);
            DATASET.DataSet.T_LogoutListDataTable dt = new DATASET.DataSet.T_LogoutListDataTable();
            da.Fill(dt);
            return dt[0];
        }
    }
}