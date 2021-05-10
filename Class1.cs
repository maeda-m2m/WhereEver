using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WhereEver
{
    public class Class1
    {

        //internal static DATASET.DataSet.M_UserRow LogoutM_UserRow(SqlConnection sqlConnection)
        //{
        //    SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
        //    da.SelectCommand.CommandText =
        //        "SELECT * FROM M_User WHERE LogoutTime IS NULL";
        //    DATASET.DataSet.M_UserDataTable dt = new DATASET.DataSet.M_UserDataTable();
        //    da.Fill(dt);
        //    if (dt.Rows.Count == 1)
        //        return dt[0] as DATASET.DataSet.M_UserRow;
        //    else
        //        return null;

        //}

        //internal static DATASET.DataSet.M_UserRow InsertLogoutTime(SqlConnection Sql)
        //{
        //    SqlDataAdapter da = new SqlDataAdapter("", Sql);
        //    da.InsertCommand.CommandText =
        //        "SELECT * FROM M_User WHERE LogoutTime IS NULL";
        //    DATASET.DataSet.M_UserDataTable dt = new DATASET.DataSet.M_UserDataTable();
        //    da.Fill(dt);
        //    if (dt.Rows.Count == 1)
        //        return dt[0] as DATASET.DataSet.M_UserRow;
        //    else
        //        return null;

        //}

        //public static DATASET.DataSet.T_ScheduleDataTable GetTDataTable(SqlConnection Sqlco)
        //{
        //    SqlDataAdapter da = new SqlDataAdapter("", Sqlco);
        //    da.SelectCommand.CommandText =
        //        "SELECT * FROM T_Schedule";
        //    var df = new DATASET.DataSet.T_ScheduleDataTable();
        //    da.Fill(df);
        //    return df;
        //}

        //public static DATASET.DataSet.T_ScheduleRow GetT_ScheduleRow(SqlConnection schedule)
        //{
        //    SqlDataAdapter da = new SqlDataAdapter("", schedule);
        //    da.SelectCommand.CommandText =
        //        "SELECT * FROM T_Schedule";
        //    DATASET.DataSet.T_ScheduleDataTable dt = new DATASET.DataSet.T_ScheduleDataTable();
        //    da.Fill(dt);
        //    if (dt.Rows.Count == 1)
        //        return dt[0] as DATASET.DataSet.T_ScheduleRow;
        //    else
        //        return null;

        //}

        //public static DATASET.DataSet.T_ScheduleRow UpdateT_ScheduleRow(SqlConnection update)
        //{
        //    SqlDataAdapter da = new SqlDataAdapter("", update);
        //    da.SelectCommand.CommandText =
        //        "UPDATE FROM T_schedule ";
        //    DATASET.DataSet.T_ScheduleDataTable dt = new DATASET.DataSet.T_ScheduleDataTable();
        //    da.Fill(dt);
        //    if (dt.Rows.Count == 1)
        //        return dt[0] as DATASET.DataSet.T_ScheduleRow;
        //    else
        //        return null;


        //}


        //public static bool DeleteT_Schedule(int time, SqlConnection sqlconnection)
        //{
        //    SqlCommand cmdDelSave = new SqlCommand("", sqlconnection);
        //    cmdDelSave.CommandText =
        //        "DELETE FROM T_schedule WHERE time=@c";
        //    cmdDelSave.Parameters.AddWithValue("@c", time);
        //    SqlTransaction sql = null;
        //    try
        //    {
        //        sqlconnection.Open();
        //        sql = sqlconnection.BeginTransaction();
        //        cmdDelSave.Transaction = sql;

        //        cmdDelSave.ExecuteNonQuery();

        //        sql.Commit();
        //        return true;
        //    }
        //    finally { sqlconnection.Close(); }

        //}

        //public static void InsertSchedule(DATASET.DataSet.T_ScheduleDataTable dt, SqlConnection sqlConnection)
        //{
        //    SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
        //    da.SelectCommand.CommandText =
        //        "SELECT * FROM T_Schedule";
        //    da.InsertCommand = (new SqlCommandBuilder(da)).GetInsertCommand();

        //    SqlTransaction sql = null;

        //    try
        //    {
        //        sqlConnection.Open();
        //        sql = sqlConnection.BeginTransaction();

        //        da.SelectCommand.Transaction = da.InsertCommand.Transaction = sql;

        //        da.Update(dt);

        //        sql.Commit();
        //    }
        //    finally
        //    {
        //        sqlConnection.Close();
        //    }
        //}



        public static DATASET.DataSet.T_ScheduleDataTable GetT_Schedule3DataTable(SqlConnection Sqlco)
        {
            var da = new SqlDataAdapter("", Sqlco);
            da.SelectCommand.CommandText =
               "SELECT * FROM T_Schedule order by date asc";
            var dt = new DATASET.DataSet.T_ScheduleDataTable();
            da.Fill(dt);
            return dt;
        }


        public static DATASET.DataSet.T_EmptyTableDataTable GetSchedule3DataTable(SqlConnection Sqlco)
        {
            SqlDataAdapter da = new SqlDataAdapter("", Sqlco);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_EmptyTable order by 曜日 asc";
            var dt = new DATASET.DataSet.T_EmptyTableDataTable();
            da.Fill(dt);
            return dt;
        }

        public static DATASET.DataSet.T_ScheduleDataTable SwitchScdl3DataTable(SqlConnection Sqlco)//今週
        {
            SqlDataAdapter da = new SqlDataAdapter("", Sqlco);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule WHERE DATEPART(WEEK,date) = DATEPART(WEEK,GETDATE()) order by date asc";
            var dt = new DATASET.DataSet.T_ScheduleDataTable();
            da.Fill(dt);
            return dt;
        }

        public static DATASET.DataSet.T_ScheduleDataTable SwitchNextScdl3DataTable(object Count, SqlConnection Sqlco)//先週
        {
            SqlDataAdapter da = new SqlDataAdapter("", Sqlco);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule WHERE DATEPART(WEEK,date) = DATEPART(WEEK,GETDATE() + @Count) order by date asc";
            da.SelectCommand.Parameters.AddWithValue("@Count", Count);
            var dt = new DATASET.DataSet.T_ScheduleDataTable();
            da.Fill(dt);
            return dt;
        }

        public static DATASET.DataSet.T_ScheduleDataTable SwitchNext2Scdl3DataTable(object Count, SqlConnection Sqlco)//来週
        {
            SqlDataAdapter da = new SqlDataAdapter("", Sqlco);
            da.SelectCommand.CommandText =
                "select * from T_Schedule where DATEPART(WEEK,date) = DATEPART(WEEK,GETDATE()+@Count) order by date asc";
            da.SelectCommand.Parameters.AddWithValue("@Count", Count);
            var dt = new DATASET.DataSet.T_ScheduleDataTable();
            da.Fill(dt);
            return dt;
        }

        //SdlNoがある列を持ってくる
        public static DATASET.DataSet.T_ScheduleRow SwitchScdl3Row(SqlConnection schedule)
        {
            SqlDataAdapter da = new SqlDataAdapter("", schedule);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule WHERE DATEPART(WEEK,date) = DATEPART(WEEK,GETDATE())";
            DATASET.DataSet.T_ScheduleDataTable dt = new DATASET.DataSet.T_ScheduleDataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
                return dt[0] as DATASET.DataSet.T_ScheduleRow;
            else
                return null;

        }

        //SdlNoの最大値を持ってくる
        public static DATASET.DataSet.T_ScheduleDataTable MaxSdlDataTable(SqlConnection Sqlco)
        {
            SqlDataAdapter da = new SqlDataAdapter("", Sqlco);
            da.SelectCommand.CommandText =
                "SELECT MAX(SdlNo) AS SdlNo FROM T_Schedule";
            DATASET.DataSet.T_ScheduleDataTable df = new DATASET.DataSet.T_ScheduleDataTable();
            da.Fill(df);
            return df;
        }

        //SdlNoの最大値を持ってくる
        public static DATASET.DataSet.T_ScheduleRow MaxSdlNo(SqlConnection schedule)
        {
            SqlDataAdapter da = new SqlDataAdapter("", schedule);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule ORDER BY SdlNo desc";
            DATASET.DataSet.T_ScheduleDataTable dt = new DATASET.DataSet.T_ScheduleDataTable();
            da.Fill(dt);
            return dt[0];
        }



        //スケジュールを追加する
        public static void InsertList(DATASET.DataSet.T_ScheduleDataTable dt, SqlConnection sql)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule";
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

        //削除ボタン
        internal static void DeleteList(int sdl, SqlConnection sql)
        {
            var da = new SqlCommand("", sql);
            da.CommandText =
                "DELETE FROM T_Schedule where [SdlNo] = @k ";
            da.Parameters.AddWithValue("@k", sdl);
            // Class1.DeleteList(sdl, Global.GetConnection()); のsdl
            SqlTransaction sqltra = null;


            try
            {
                sql.Open();
                sqltra = sql.BeginTransaction();

                da.Transaction = sqltra;

                da.ExecuteNonQuery();

                sqltra.Commit();

            }
            finally
            {
                sql.Close();
            }
        }


    }
}
