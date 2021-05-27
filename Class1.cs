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
        public static DATASET.DataSet.T_PdbRow GetProjectRow(string id, SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Pdb WHERE Pid = @id";
            da.SelectCommand.Parameters.AddWithValue("@id", id);
            DATASET.DataSet.T_PdbDataTable dt = new DATASET.DataSet.T_PdbDataTable();
            da.Fill(dt);
            return dt[0];
        }

        public static DATASET.DataSet.T_ScheduleDataTable GetT_Schedule3DataTable(SqlConnection Sqlco)//スケジュールリスト用
        {
            var da = new SqlDataAdapter("", Sqlco);

            da.SelectCommand.CommandText =
              "SELECT * FROM T_Schedule WHERE date BETWEEN GETDATE() AND DATEADD(YYYY,10,GETDATE()) ORDER BY date ASC";

            var dt = new DATASET.DataSet.T_ScheduleDataTable();

            da.Fill(dt);

            return dt;

        }

        public static DATASET.DataSet.T_ScheduleDataTable GetT_Schedule3DataTableDesc(SqlConnection Sqlco)//スケジュールリスト用
        {
            var da = new SqlDataAdapter("", Sqlco);

            da.SelectCommand.CommandText =
              "SELECT * FROM T_Schedule WHERE date BETWEEN GETDATE() AND DATEADD(YYYY,10,GETDATE()) ORDER BY date desc";

            var dt = new DATASET.DataSet.T_ScheduleDataTable();

            da.Fill(dt);

            return dt;

        }


        public static DATASET.DataSet.T_EmptyTableDataTable GetSchedule3DataTable(SqlConnection Sqlco)//スケジュール表用
        {
            SqlDataAdapter da = new SqlDataAdapter("", Sqlco);

            da.SelectCommand.CommandText =
            "SELECT * FROM T_EmptyTable ORDER BY 曜日 ASC ";

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



        public static DATASET.DataSet.T_ScheduleRow MaxSdlNo(SqlConnection schedule)//SdlNoの最大値を持ってくる
        {
            SqlDataAdapter da = new SqlDataAdapter("", schedule);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule ORDER BY SdlNo desc";
            DATASET.DataSet.T_ScheduleDataTable dt = new DATASET.DataSet.T_ScheduleDataTable();
            da.Fill(dt);
            return dt[0];
        }




        public static void InsertList(DATASET.DataSet.T_ScheduleDataTable dt, SqlConnection sql) //スケジュールを追加する
        {
            SqlDataAdapter da = new SqlDataAdapter("", sql);

            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule";

            da.InsertCommand = new SqlCommandBuilder(da).GetInsertCommand();

            try
            {
                sql.Open();

                SqlTransaction sqltra = sql.BeginTransaction();

                da.SelectCommand.Transaction = da.InsertCommand.Transaction = sqltra;

                da.Update(dt);

                sqltra.Commit();
            }
            finally
            {
                sql.Close();
            }

        }


        internal static void DeleteList(int sdl, SqlConnection sql) //削除ボタン
        {
            var da = new SqlCommand("", sql);
            da.CommandText =
                "DELETE FROM T_Schedule where [SdlNo] = @k ";
            da.Parameters.AddWithValue("@k", sdl);

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

        public static DATASET.DataSet.T_ScheduleRow UpdateProject(DATASET.DataSet.T_ScheduleRow dr, SqlConnection sql)//編集コマンドの値を変更する用
        {
            {
                var a = new SqlCommand("", sql);

                a.CommandText = "UPDATE T_Schedule SET [date] = @date, [time] =@time, [title] = @title, [name] = @name where [SdlNo] = @SdlNo";

                a.Parameters.AddWithValue("@date", dr.date);
                a.Parameters.AddWithValue("@time", dr.time);
                a.Parameters.AddWithValue("@title", dr.title);
                a.Parameters.AddWithValue("@name", dr.name);
                a.Parameters.AddWithValue("@SdlNo", dr.SdlNo);

                try
                {
                    sql.Open();
                    SqlTransaction sqltra = sql.BeginTransaction();
                    a.Transaction = sqltra;

                    a.ExecuteNonQuery();

                    sqltra.Commit();

                }
                finally
                {
                    sql.Close();
                }

                return dr;

            }
        }

        public static DATASET.DataSet.T_ScheduleDataTable ScheduleSearch(string a, string b, string c, string d, SqlConnection Sqlco)//検索用
        {
            SqlDataAdapter da = new SqlDataAdapter("", Sqlco);

            da.SelectCommand.CommandText =
              "SELECT * FROM T_Schedule WHERE date LIKE @a AND time LIKE @b AND title LIKE @c AND name LIKE @d order by date asc";


            da.SelectCommand.Parameters.AddWithValue("@a", "%" + a + "%");
            da.SelectCommand.Parameters.AddWithValue("@b", "%" + b + "%");
            da.SelectCommand.Parameters.AddWithValue("@c", "%" + c + "%");
            da.SelectCommand.Parameters.AddWithValue("@d", "%" + d + "%");

            var dt = new DATASET.DataSet.T_ScheduleDataTable();

            da.Fill(dt);

            return dt;
        }


    }
}
