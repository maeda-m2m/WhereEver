using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WhereEver.Class
{
    public class Wiki
    {
        public static DATASET.DataSet.T_WikiDataTable GetT_WikiDataTable(SqlConnection Sqlco)
        {
            var da = new SqlDataAdapter("", Sqlco);

            da.SelectCommand.CommandText =
              "SELECT * FROM T_Wiki";

            var dt = new DATASET.DataSet.T_WikiDataTable();

            da.Fill(dt);

            return dt;

        }

        public static void InsertT_Wiki(DATASET.DataSet.T_WikiDataTable dt, SqlConnection sql)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sql);

            da.SelectCommand.CommandText =
                "SELECT * FROM T_Wiki";

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
        public static DATASET.DataSet.T_ScheduleRow MaxSdlNo(SqlConnection schedule)//SdlNoの最大値を持ってくる
        {
            SqlDataAdapter da = new SqlDataAdapter("", schedule);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule ORDER BY SdlNo desc";
            DATASET.DataSet.T_ScheduleDataTable dt = new DATASET.DataSet.T_ScheduleDataTable();
            da.Fill(dt);
            return dt[0];
        }
    }
}