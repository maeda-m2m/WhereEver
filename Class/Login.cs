﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WhereEver.Class
{
    public class Login
    {
        internal static DATASET.DataSet.M_UserRow GetM_UserRow(string id, string pw, SqlConnection sqlConnection)
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





        public static DATASET.DataSet.T_LoginListDataTable GetLoginListDataTable(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_LoginList";
            DATASET.DataSet.T_LoginListDataTable dt = new DATASET.DataSet.T_LoginListDataTable();
            da.Fill(dt);
            return dt;
        }

        internal static DATASET.DataSet.T_LoginListRow GetT_LoginListRow(SqlConnection sqlConnection)
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

            SqlTransaction sql = null;

            try
            {
                sqlConnection.Open();
                sql = sqlConnection.BeginTransaction();

                da.SelectCommand.Transaction = da.UpdateCommand.Transaction = sql;

                da.Update(dt);

                sql.Commit();
            }
            catch (Exception ex)
            {
                if (sql != null)
                    sql.Rollback();
            }
            finally
            {
                sqlConnection.Close();
            }
        }


    }
}