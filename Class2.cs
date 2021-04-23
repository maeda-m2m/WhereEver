using System;
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

        public static void InsertList(DATASET.DataSet.T_ChatDataTable dt, SqlConnection sql)
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
            finally
            {
                sql.Close();
            }

        }

        internal static DATASET.DataSet.M_UserRow Getname1(SqlConnection sqlConnection, string name)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT name1 FROM M_User where name = @i";
            da.SelectCommand.Parameters.AddWithValue("@i", name);
            DATASET.DataSet.M_UserDataTable dt = new DATASET.DataSet.M_UserDataTable();
            da.Fill(dt);
            return dt[0];
        }
        public static DATASET.DataSet.T_LoginListDataTable GetLoginListDataTable(SqlConnection sqlConnection)//LoginListの重複してある名前をまとめてreturn name
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT DISTINCT name FROM T_LoginList";
            DATASET.DataSet.T_LoginListDataTable dt = new DATASET.DataSet.T_LoginListDataTable();
            da.Fill(dt);
            return dt;
        }

        internal static DATASET.DataSet.T_ChatRow MaxNoRow(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "select MAX(No) as No from T_Chat";
            DATASET.DataSet.T_ChatDataTable dt = new DATASET.DataSet.T_ChatDataTable();
            da.Fill(dt);
            return dt[0];
        }

        public static DATASET.DataSet.T_LogoutListDataTable GetLogoutListDataTable(SqlConnection sqlConnection)//LogoutListの重複してある名前をまとめてreturn name
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT DISTINCT name FROM T_LogoutList";
            DATASET.DataSet.T_LogoutListDataTable dt = new DATASET.DataSet.T_LogoutListDataTable();
            da.Fill(dt);
            return dt;
        }

        public static DATASET.DataSet.T_LoginListRow GetLoginListRow(SqlConnection schedule)//T_LoginListのすべて
        {
            SqlDataAdapter da = new SqlDataAdapter("", schedule);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_LoginList";
            DATASET.DataSet.T_LoginListDataTable dt = new DATASET.DataSet.T_LoginListDataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
                return dt[0] as DATASET.DataSet.T_LoginListRow;
            else
                return null;

        }

        
        


        

        public static DATASET.DataSet.T_ScheduleDataTable Insatsu1(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule WHERE DATEPART(WEEK, date) = DATEPART(WEEK, GETDATE())-1 order by time";
            DATASET.DataSet.T_ScheduleDataTable dt = new DATASET.DataSet.T_ScheduleDataTable();
            da.Fill(dt);
            return (dt);
        }
        internal static DATASET.DataSet.T_LoginListRow Insatsu1Row(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule WHERE DATEPART(WEEK, date) = DATEPART(WEEK, GETDATE())-1 order by time";
            DATASET.DataSet.T_LoginListDataTable dt = new DATASET.DataSet.T_LoginListDataTable();
            da.Fill(dt);
            if (dt.Rows.Count >= 1)
                return dt[0] as DATASET.DataSet.T_LoginListRow;
            else
                return null;
        }


        public static DATASET.DataSet.T_ScheduleDataTable Insatsu2(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule WHERE DATEPART(WEEK, date) = DATEPART(WEEK, GETDATE()) order by date";
            DATASET.DataSet.T_ScheduleDataTable dt = new DATASET.DataSet.T_ScheduleDataTable();
            da.Fill(dt);
            return (dt);
        }

        internal static DATASET.DataSet.T_LoginListRow Insatsu2Row(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule WHERE DATEPART(WEEK, date) = DATEPART(WEEK, GETDATE()) order by date";
            DATASET.DataSet.T_LoginListDataTable dt = new DATASET.DataSet.T_LoginListDataTable();
            da.Fill(dt);
            if (dt.Rows.Count >= 1)
                return dt[0] as DATASET.DataSet.T_LoginListRow;
            else
                return null;
        }

        //public static DataSet1.T_FileDataTable GetT_Files(SqlConnection sql)
        //{
        //    SqlCommand da = new SqlCommand("", sql);
        //    da.CommandText =
        //       "INSERT INTO image_data(title,type,datum) VALUES(@title, @type, @datum)";
        //    da.Parameters.AddWithValue("@title", Path.GetFileName(datum.PostedFile.FileName));
        //    da.Parameters.Add("@type", datum.PostedFile.ContentType);

        //    DataSet1.T_FileDataTable dt = new DataSet1.T_FileDataTable();
        //    return (dt);

        //}
    }
}