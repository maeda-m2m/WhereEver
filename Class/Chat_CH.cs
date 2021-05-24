using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WhereEver.Class
{
    public class Chat_CH
    {

        public static DATASET.DataSet.T_Chat_CHRow GetT_Chat_CH(SqlConnection sqlConnection, string id)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            //UserIDからNULL以外の列数を取得します。例えばUserIDがguestひとつなら1を返します。
            da.SelectCommand.CommandText =
                "SELECT * FROM [T_Chat_CH] WHERE id = LTRIM(RTRIM(@userid))";

            //パラメータを取得
            da.SelectCommand.Parameters.AddWithValue("@userid", id.Trim());

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_Chat_CHDataTable dt = new DATASET.DataSet.T_Chat_CHDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                if (dt.Count >= 1)
                {
                    //リストにある
                    return dt[0];

                }
                else
                {
                    //リストにない
                    return null;
                }

            }
            catch
            {
                //不正な値が入力された場合はnullを返します。
                return null;
            }

        }

        //--------------------------------------------------------------------------------------------------------


        public static void SetT_Chat_CHUpdate(SqlConnection sqlConnection, string id)
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

                try
                {
                    //Add the paramaters for the Updatecommand.必ずダブルクオーテーションで@変数の宣言を囲んでください。command.CommandTextで使用するものは、必ずすべて宣言してください。
                    //-------------------------------------------------------------------------------------------------------------------
                    command.Parameters.Add(new SqlParameter("@userid", System.Data.SqlDbType.NVarChar, 40, "id")).Value = id.Trim();
                    command.Parameters.Add(new SqlParameter("@date", System.Data.SqlDbType.DateTime, 8, "Date")).Value = DateTime.Now;


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "UPDATE [T_Chat_CH] SET[Date] = @date WHERE([id] = LTRIM(RTRIM(@userid)))";

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

        //--------------------------------------------------------------------------------------------------------

        public static void SetT_Chat_CHInsert(SqlConnection sqlConnection, string id)
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

                try
                {
                    //Add the paramaters for the Updatecommand.必ずダブルクオーテーションで@変数の宣言を囲んでください。command.CommandTextで使用するものは、必ずすべて宣言してください。
                    //-------------------------------------------------------------------------------------------------------------------
                    command.Parameters.Add(new SqlParameter("@userid", System.Data.SqlDbType.NVarChar, 40, "id")).Value = id.Trim();
                    command.Parameters.Add(new SqlParameter("@date", System.Data.SqlDbType.DateTime, 8, "Date")).Value = DateTime.Now;


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO [T_Chat_CH]([id],[Date]) VALUES(LTRIM(RTRIM(@userid)), @date)";

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


        //--------------------------------------------------------------------------------------------------------


    }
}