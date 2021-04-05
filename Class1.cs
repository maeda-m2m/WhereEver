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
        public static DataSet2.M_UserDataTable GetM_UserDataTable(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM M_User";
            DataSet2.M_UserDataTable dt = new DataSet2.M_UserDataTable();
            da.Fill(dt);
            return (dt);
        }
        internal static DataSet2.M_UserRow GetM_UserRow(string id, string pw, SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM M_User WHERE (id = @id) and (pw = @pw)";
            da.SelectCommand.Parameters.AddWithValue("@id", id);
            da.SelectCommand.Parameters.AddWithValue("@pw", pw);
            DataSet2.M_UserDataTable dt = new DataSet2.M_UserDataTable();
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

        public static DataSet2.M_UserRow GetM_UserRow(SqlConnection schedule)
        {
            SqlDataAdapter da = new SqlDataAdapter("", schedule);
            da.SelectCommand.CommandText =
                "SELECT * FROM M_User";
            DataSet2.M_UserDataTable dt = new DataSet2.M_UserDataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
                return dt[0] as DataSet2.M_UserRow;
            else
                return null;
        }


        public static DataSet2.T_LoginListDataTable GetLoginListDataTable(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_LoginList";
            DataSet2.T_LoginListDataTable dt = new DataSet2.T_LoginListDataTable();
            da.Fill(dt);
            return dt;
        }

        internal static DataSet2.T_LoginListRow GetT_LoginListRow(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_LoginList";
            DataSet2.T_LoginListDataTable dt = new DataSet2.T_LoginListDataTable();
            da.Fill(dt);
            if (dt.Rows.Count >= 1)
                return dt[0] as DataSet2.T_LoginListRow;
            else
                return null;
        }

        public static void UpdateLogin(DataSet2.M_UserDataTable dt, SqlConnection sqlConnection)
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


        internal static DataSet2.M_UserRow LogoutM_UserRow(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM M_User WHERE LogoutTime IS NULL";
            DataSet2.M_UserDataTable dt = new DataSet2.M_UserDataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
                return dt[0] as DataSet2.M_UserRow;
            else
                return null;

        }

        internal static DataSet2.M_UserRow InsertLogoutTime(SqlConnection Sql)
        {
            SqlDataAdapter da = new SqlDataAdapter("", Sql);
            da.InsertCommand.CommandText =
                "SELECT * FROM M_User WHERE LogoutTime IS NULL";
            DataSet2.M_UserDataTable dt = new DataSet2.M_UserDataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
                return dt[0] as DataSet2.M_UserRow;
            else
                return null;

        }

        public static DataSet2.T_ScheduleDataTable GetTDataTable(SqlConnection Sqlco)
        {
            SqlDataAdapter da = new SqlDataAdapter("", Sqlco);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule";
            DataSet2.T_ScheduleDataTable df = new DataSet2.T_ScheduleDataTable();
            da.Fill(df);
            return df;
        }

        public static DataSet2.T_ScheduleRow GetT_ScheduleRow(SqlConnection schedule)
        {
            SqlDataAdapter da = new SqlDataAdapter("", schedule);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule";
            DataSet2.T_ScheduleDataTable dt = new DataSet2.T_ScheduleDataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
                return dt[0] as DataSet2.T_ScheduleRow;
            else
                return null;

        }

        public static DataSet2.T_ScheduleRow UpdateT_ScheduleRow(SqlConnection update)
        {
            SqlDataAdapter da = new SqlDataAdapter("", update);
            da.SelectCommand.CommandText =
                "UPDATE FROM T_schedule ";
            DataSet2.T_ScheduleDataTable dt = new DataSet2.T_ScheduleDataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
                return dt[0] as DataSet2.T_ScheduleRow;
            else
                return null;


        }

        public static bool DeleteT_Schedule(int time, SqlConnection sqlconnection)
        {
            SqlCommand cmdDelSave = new SqlCommand("", sqlconnection);
            cmdDelSave.CommandText =
                "DELETE FROM T_schedule WHERE time=@c";
            cmdDelSave.Parameters.AddWithValue("@c", time);
            SqlTransaction sql = null;
            try
            {
                sqlconnection.Open();
                sql = sqlconnection.BeginTransaction();
                cmdDelSave.Transaction = sql;

                cmdDelSave.ExecuteNonQuery();

                sql.Commit();
                return true;
            }
            catch (Exception e)
            {
                if (sql != null)
                    sql.Rollback();
                return false;

            }
            finally { sqlconnection.Close(); }

        }

        public static void InsertSchedule(DataSet2.T_ScheduleDataTable dt, SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule";
            da.InsertCommand = (new SqlCommandBuilder(da)).GetInsertCommand();

            SqlTransaction sql = null;

            try
            {
                sqlConnection.Open();
                sql = sqlConnection.BeginTransaction();

                da.SelectCommand.Transaction = da.InsertCommand.Transaction = sql;

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



        public static DataSet2.T_ScheduleDataTable GetT_Schedule3DataTable(SqlConnection Sqlco)
        {
            SqlDataAdapter da = new SqlDataAdapter("", Sqlco);
            da.SelectCommand.CommandText =
               "SELECT * FROM T_Schedule";
            //"SELECT [time], [title], [name], [KanriFlag],[SdlNo] FROM T_Schedule WHERE KanriFlag IS NOT NULL"
            DataSet2.T_ScheduleDataTable df = new DataSet2.T_ScheduleDataTable();
            da.Fill(df);
            return df;
        }


        public static DataSet2.T_EmptyTableDataTable GetSchedule3DataTable(SqlConnection Sqlco)
        {
            SqlDataAdapter da = new SqlDataAdapter("", Sqlco);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_EmptyTable";
            //T_Schedule WHERE KanriFlag IS NULL
            var df = new DataSet2.T_EmptyTableDataTable();
            da.Fill(df);
            return df;
        }

        public static DataSet2.T_ScheduleDataTable SwitchScdl3DataTable(SqlConnection Sqlco)
        {
            SqlDataAdapter da = new SqlDataAdapter("", Sqlco);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule WHERE DATEPART(WEEK,date) = DATEPART(WEEK,GETDATE())";
            DataSet2.T_ScheduleDataTable df = new DataSet2.T_ScheduleDataTable();
            da.Fill(df);
            return df;
        }

        //SdlNoがある列を持ってくる
        public static DataSet2.T_ScheduleRow SwitchScdl3Row(SqlConnection schedule)
        {
            SqlDataAdapter da = new SqlDataAdapter("", schedule);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule WHERE DATEPART(WEEK,date) = DATEPART(WEEK,GETDATE())";
            DataSet2.T_ScheduleDataTable dt = new DataSet2.T_ScheduleDataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
                return dt[0] as DataSet2.T_ScheduleRow;
            else
                return null;

        }

        //SdlNoの最大値を持ってくる
        public static DataSet2.T_ScheduleDataTable MaxSdlDataTable(SqlConnection Sqlco)
        {
            SqlDataAdapter da = new SqlDataAdapter("", Sqlco);
            da.SelectCommand.CommandText =
                "SELECT MAX(SdlNo) AS SdlNo FROM T_Schedule";
            DataSet2.T_ScheduleDataTable df = new DataSet2.T_ScheduleDataTable();
            da.Fill(df);
            return df;
        }

        //SdlNoの最大値を持ってくる
        public static DataSet2.T_ScheduleRow MaxSdlNo(SqlConnection schedule)
        {
            SqlDataAdapter da = new SqlDataAdapter("", schedule);
            da.SelectCommand.CommandText =
                "SELECT MAX(SdlNo) AS SdlNo FROM T_Schedule";
            DataSet2.T_ScheduleDataTable dt = new DataSet2.T_ScheduleDataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
                return dt[0] as DataSet2.T_ScheduleRow;
            else
                return null;
        }



        //スケジュールを追加する
        public static void InsertList(DataSet2.T_ScheduleDataTable dt, SqlConnection sql)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule WHERE SdlNo IS NOT NULL";
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

        //削除ボタン
        internal static void DeleteList(int sdl, SqlConnection sql)
        {
            SqlCommand da = new SqlCommand("", sql);
            da.CommandText =
                "DELETE FROM T_Schedule where [SdlNo] = @k ";//SdlNoを取ってくる
            da.Parameters.AddWithValue("@k", sdl);// Class1.DeleteList(sdl, Global.GetConnection()); のsdl
            SqlTransaction sqltra = null;


            try
            {
                sql.Open();
                sqltra = sql.BeginTransaction();

                da.Transaction = sqltra;

                da.ExecuteNonQuery();

                sqltra.Commit();

            }
            catch (Exception et)
            {
                if (sqltra != null)
                    sqltra.Rollback();

            }
            finally
            {
                sql.Close();
            }
        }
    }
}
