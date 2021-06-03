using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WhereEver.Class
{
    public class Wiki
    {
        public static DATASET.DataSet.T_WikiDataTable GetT_WikiDataTable(SqlConnection Sqlco)//データバインド用
        {
            var da = new SqlDataAdapter("", Sqlco);

            da.SelectCommand.CommandText =
              "SELECT * FROM T_Wiki";

            var dt = new DATASET.DataSet.T_WikiDataTable();

            da.Fill(dt);

            return dt;

        }

        public static void InsertT_Wiki(DATASET.DataSet.T_WikiDataTable dt, HttpPostedFile file, SqlConnection sql)//データベースに値を追加する時の処理
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

        public static DATASET.DataSet.T_WikiRow Maxid(SqlConnection schedule)//idの最大値をとってくる
        {
            SqlDataAdapter da = new SqlDataAdapter("", schedule);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Wiki ORDER BY id desc";
            DATASET.DataSet.T_WikiDataTable dt = new DATASET.DataSet.T_WikiDataTable();
            da.Fill(dt);
            return dt[0];
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

        public static DATASET.DataSet.T_WikiDataTable Search(string Search1,string Search2, SqlConnection Sqlco)//検索用
        {
            SqlDataAdapter da = new SqlDataAdapter("", Sqlco);

            da.SelectCommand.CommandText =
              "SELECT * FROM T_Wiki WHERE Text LIKE @Search1 OR Title LIKE @Search2 order by Date asc";


            da.SelectCommand.Parameters.AddWithValue("@Search1", "%" + Search1 + "%");
            da.SelectCommand.Parameters.AddWithValue("@Search2", "%" + Search2 + "%");

            var dt = new DATASET.DataSet.T_WikiDataTable();

            da.Fill(dt);

            return dt;
        }

    }
}