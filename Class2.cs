﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace WhereEver
{
    public class Class2
    {
        public static DataSet1.T_ChatDataTable GetChatDataTable(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Chat";
            DataSet1.T_ChatDataTable dt = new DataSet1.T_ChatDataTable();
            da.Fill(dt);
            return (dt);
        }

        public static DataSet1.T_ChatRow GetChatRow(SqlConnection schedule)
        {
            SqlDataAdapter da = new SqlDataAdapter("", schedule);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Chat";
            DataSet1.T_ChatDataTable dt = new DataSet1.T_ChatDataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
                return dt[0] as DataSet1.T_ChatRow;
            else
                return null;

        }

        public static DataSet1.T_ChatDataTable MaxNoDataTable(SqlConnection Sqlco)
        {
            SqlDataAdapter da = new SqlDataAdapter("", Sqlco);
            da.SelectCommand.CommandText =
                "SELECT MAX(No) AS No FROM T_Chat";
            DataSet1.T_ChatDataTable df = new DataSet1.T_ChatDataTable();
            da.Fill(df);
            return df;
        }

        //Noの最大値を持ってくる
        public static DataSet1.T_ChatRow MaxNoRow(SqlConnection schedule)
        {
            SqlDataAdapter da = new SqlDataAdapter("", schedule);
            da.SelectCommand.CommandText =
                "SELECT MAX(No) AS No FROM T_Chat";
            DataSet1.T_ChatDataTable dt = new DataSet1.T_ChatDataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
                return dt[0] as DataSet1.T_ChatRow;
            else
                return null;
        }

        public static void InsertList(DataSet1.T_ChatDataTable dt, SqlConnection sql)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Chat";
            da.InsertCommand = (new SqlCommandBuilder(da)).GetInsertCommand();

            SqlTransaction sqltra = null;

            try
            {
                sql.Open();
                sqltra = sql.BeginTransaction();

                da.SelectCommand.Transaction = da.InsertCommand.Transaction = sqltra;

                da.Update(dt);
                sqltra.Commit();
            }
            catch (Exception ex)
            {
                if (sqltra != null)
                    sqltra.Rollback();
            }
            finally
            {
                sql.Close();
            }

        }

        public static DataSet2.T_LoginListDataTable GetLoginListDataTable(SqlConnection sqlConnection)//LoginListの重複してある名前をまとめてreturn name
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT DISTINCT name FROM T_LoginList";
            DataSet2.T_LoginListDataTable dt = new DataSet2.T_LoginListDataTable();
            da.Fill(dt);
            return dt;
        }
        public static DataSet2.T_LogoutListDataTable GetLogoutListDataTable(SqlConnection sqlConnection)//LogoutListの重複してある名前をまとめてreturn name
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT DISTINCT name FROM T_LogoutList";
            DataSet2.T_LogoutListDataTable dt = new DataSet2.T_LogoutListDataTable();
            da.Fill(dt);
            return dt;
        }

        public static DataSet2.T_LoginListRow GetLoginListRow(SqlConnection schedule)//T_LoginListのすべて
        {
            SqlDataAdapter da = new SqlDataAdapter("", schedule);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_LoginList";
            DataSet2.T_LoginListDataTable dt = new DataSet2.T_LoginListDataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
                return dt[0] as DataSet2.T_LoginListRow;
            else
                return null;

        }

        public static void InsertLoginList(DataSet2.T_LoginListDataTable dt, SqlConnection sql)//ログインするたびにnameとDateをLoginListにInsert
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
        public static void InsertLogoutList(DataSet2.T_LoginListDataTable dt, SqlConnection sql)//ログアウトするたびにnameとDateをLogoutListにInsert
        {
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_LogoutList";
            da.InsertCommand = (new SqlCommandBuilder(da)).GetInsertCommand();

            sql.Open();
            SqlTransaction sqltra = sql.BeginTransaction();

            da.SelectCommand.Transaction = da.InsertCommand.Transaction = sqltra;

            da.Update(dt);
            sqltra.Commit();
            sql.Close();

        }


        internal static DataSet2.T_LoginListRow UserLoginMAXTime(SqlConnection sqlConnection,string name)//最新LoginDateの取得
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT MAX(Date) as date FROM T_LoginList where name like @i ORDER BY Date DESC ";
            da.SelectCommand.Parameters.AddWithValue("@i", name);
            DataSet2.T_LoginListDataTable dt = new DataSet2.T_LoginListDataTable();
            da.Fill(dt);
            return dt[0];
        }
        internal static DataSet2.T_LoginListRow UserLogoutMAXTime(SqlConnection sqlConnection, string name)//最新LogoutDateの取得
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT MAX(Date) as date FROM T_LogoutList where name like @i ORDER BY Date DESC ";
            da.SelectCommand.Parameters.AddWithValue("@i", name);
            DataSet2.T_LoginListDataTable dt = new DataSet2.T_LoginListDataTable();
            da.Fill(dt);
            return dt[0];
        }

        public static DataSet1.T_Schedule3DataTable Insatsu1(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule3 WHERE DATEPART(WEEK, date) = DATEPART(WEEK, GETDATE())-1 order by time";
            DataSet1.T_Schedule3DataTable dt = new DataSet1.T_Schedule3DataTable();
            da.Fill(dt);
            return (dt);
        }
        internal static DataSet1.T_LoginListRow Insatsu1Row(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule3 WHERE DATEPART(WEEK, date) = DATEPART(WEEK, GETDATE())-1 order by time";
            DataSet1.T_LoginListDataTable dt = new DataSet1.T_LoginListDataTable();
            da.Fill(dt);
            if (dt.Rows.Count >= 1)
                return dt[0] as DataSet1.T_LoginListRow;
            else
                return null;
        }


        public static DataSet1.T_Schedule3DataTable Insatsu2(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule3 WHERE DATEPART(WEEK, date) = DATEPART(WEEK, GETDATE()) order by date";
            DataSet1.T_Schedule3DataTable dt = new DataSet1.T_Schedule3DataTable();
            da.Fill(dt);
            return (dt);
        }

        internal static DataSet1.T_LoginListRow Insatsu2Row(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule3 WHERE DATEPART(WEEK, date) = DATEPART(WEEK, GETDATE()) order by date";
            DataSet1.T_LoginListDataTable dt = new DataSet1.T_LoginListDataTable();
            da.Fill(dt);
            if (dt.Rows.Count >= 1)
                return dt[0] as DataSet1.T_LoginListRow;
            else
                return null;
        }

        //public static DataSet1.T_FileDataTable GetT_Files(SqlConnection sql)
        //{
        //    //SqlCommand da = new SqlCommand("", sql);
        //    //da.CommandText =
        //    //   "INSERT INTO image_data(title,type,datum) VALUES(@title, @type, @datum)";
        //    //da.Parameters.AddWithValue("@title", Path.GetFileName(datum.PostedFile.FileName));
        //    //da.Parameters.Add("@type", datum.PostedFile.ContentType);

        //    //DataSet1.T_FileDataTable dt = new DataSet1.T_FileDataTable();
        //    //return (dt);
            
        //}
    }
}