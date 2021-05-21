using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WhereEver.Class
{
    public class Toppage
    {


        public static void SetT_TopPageUpdate(SqlConnection sqlConnection, string text)
        {
            sqlConnection.Open();

            //Create the Update Command.
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //Sql Commandを作成します。
            SqlCommand command = sqlConnection.CreateCommand();

            //ファイルを書き込み可能なようにオープンしてSqlのデータをアップデートします。
            //Start a local transaction. usingブロックを抜けると自動でcloseされます。
            using (SqlTransaction transaction = sqlConnection.BeginTransaction())
            {

                //Must assign both transaction object and connection
                //to Command object for apending local transaction
                command.Connection = sqlConnection;
                command.Transaction = transaction;

                DateTime date = DateTime.Now; //DateTime取得

                try
                {
                    //Add the paramaters for the Updatecommand.必ずダブルクオーテーションで@変数の宣言を囲んでください。command.CommandTextで使用するものは、必ずすべて宣言してください。
                    //-------------------------------------------------------------------------------------------------------------------
                    command.Parameters.Add(new SqlParameter("@TopPage", System.Data.SqlDbType.NVarChar, -1, "TopPage")).Value = text;
                    command.Parameters.Add(new SqlParameter("@DateTime", System.Data.SqlDbType.DateTime, 8, "DateTime")).Value = date;


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "UPDATE TOP(1) [T_TopPage] SET[TopPage] = @TopPage, [DateTime]=@DateTime";

                    //このメソッドでは、XmlCommandTypeプロパティおよびCommandTextプロパティを使用してSQL文またはコマンドを実行し、影響を受ける行数を戻します（必須）。 
                    //ここでエラーが出る場合は、宣言やSql文が不正な場合があります。
                    command.ExecuteNonQuery();


                    //Attempt to commit the transaction.
                    da.UpdateCommand = command;
                    transaction.Commit();

                    //Console.WriteLine("Update Completed");

                }
                catch
                {
                    //catch文
                    //Console.WriteLine("Update Failed");
                    transaction.Rollback();
                }

            } //sqlConnection.Close();

            // データベースの接続終了
            sqlConnection.Close();
            return;

        }

        public static DATASET.DataSet.T_TopPageRow GetT_TopPage(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得
            //da.SelectCommand.Parameters.AddWithValue("@FileName", FileName);
            //da.SelectCommand.Parameters.AddWithValue("@pass", pass);

            da.SelectCommand.CommandText =
                "SELECT TOP(1) * FROM T_TopPage";

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_TopPageDataTable dt = new DATASET.DataSet.T_TopPageDataTable();

            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                if (dt.Count >= 1)
                {
                    //ファイルあり
                    // データベースの接続終了
                    sqlConnection.Close();
                    return dt[0];

                }
                else
                {
                    //ファイルなし
                    // データベースの接続終了
                    sqlConnection.Close();
                    return null;
                }
            }
            catch
            {
                //不正な処理
                // データベースの接続終了
                sqlConnection.Close();
                return null;
            }

        }
    }
}