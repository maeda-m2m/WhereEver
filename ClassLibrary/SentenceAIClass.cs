using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using static System.Web.HttpUtility;

namespace WhereEver.ClassLibrary
{
    public class SentenceAIClass
    {



        /// <summary>
        /// DATASET.DataSet.T_SentenceAIDataTableを返します。
        /// 引数はDATASET.DataSet.T_SentenceAIDataTable型で参照して下さい。
        /// 中身がない場合や入力値が不正な場合はnullを返します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="str"></param>
        /// <returns>DATASET.DataSet.T_SentenceAIDataTable</returns>
        public static DATASET.DataSet.T_SentenceAIDataTable GetT_SentenceAIDataTable(SqlConnection sqlConnection, string str)
        {
            //エンコード
            str = HtmlEncode(str);

            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得（必要のないパラメータを設定するとコンパイルエラーする）
            //da.SelectCommand.Parameters.Add(new SqlParameter("@sentence", System.Data.SqlDbType.NVarChar, -1, "sentence")).Value = str;
            da.SelectCommand.Parameters.AddWithValue("@str", str);
            //-------------------------------------------------------------------------------------------------------------------

            da.SelectCommand.CommandText =
                "SELECT * FROM [T_SentenceAI] WHERE [sentence] LIKE CAST(('%' + @str + '%') AS NVarChar(max))";

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_SentenceAIDataTable dt = new DATASET.DataSet.T_SentenceAIDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                if (dt.Count >= 1)
                {
                    //中身あり
                    return dt;

                }
                else
                {
                    //中身なし
                    return null;
                }

            }
            catch
            {
                //不正な値が入力された場合はnullを返します。
                return null;
            }

        }


        /// <summary>
        /// DATASET.DataSet.T_SentenceAIRowを返します。
        /// 引数はboolで参照して下さい。重複を✓します。
        /// 中身がない場合や入力値が不正な場合はnullを返します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="str"></param>
        /// <returns>bool</returns>
        public static bool GetIsT_SentenceAI(SqlConnection sqlConnection, string str)
        {
            //エンコード
            str = HtmlEncode(str);

            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得（必要のないパラメータを設定するとコンパイルエラーする）
            da.SelectCommand.Parameters.AddWithValue("@str", str);
            //-------------------------------------------------------------------------------------------------------------------

            da.SelectCommand.CommandText =
                "SELECT * FROM [T_SentenceAI] WHERE [sentence] = CAST(@str AS NVarChar(max))";

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_SentenceAIDataTable dt = new DATASET.DataSet.T_SentenceAIDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                if (dt.Count >= 1)
                {
                    //ダブりあり
                    return true;

                }
                else
                {
                    //ダブりなし
                    return false;
                }

            }
            catch
            {
                //不正な値が入力された場合はtrueを返します（ダブりありと同じ処理）。
                return true;
            }

        }


        /// <summary>
        /// センテンスAIテーブルにインサートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="str"></param>
        /// <param name="keywords"></param>
        public static void SetT_SentenceAI_Insert(SqlConnection sqlConnection, string str, string keywords)
        {

            //エンコード
            str = HtmlEncode(str);
            keywords = HtmlEncode(keywords);

            sqlConnection.Open();

            //GUID自動生成
            string uid = Guid.NewGuid().ToString();

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
                    command.Parameters.Add(new SqlParameter("@uid", System.Data.SqlDbType.NVarChar, 50, "uid")).Value = uid.Trim();
                    command.Parameters.Add(new SqlParameter("@sentence", System.Data.SqlDbType.NVarChar, -1, "sentence")).Value = str.Trim();
                    command.Parameters.Add(new SqlParameter("@keywords", System.Data.SqlDbType.NVarChar, -1, "keywords")).Value = keywords.Trim();
                    command.Parameters.Add(new SqlParameter("@up_day", System.Data.SqlDbType.DateTime, 8, "up_day")).Value = DateTime.Now;

                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO [T_SentenceAI]([uid], [sentence], [keywords], [up_day]) VALUES(LTRIM(RTRIM(@uid)), CAST(LTRIM(RTRIM(@sentence)) AS NVarChar(max)), CAST(LTRIM(RTRIM(@keywords)) AS NVarChar(max)), @up_day)";


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


        /// <summary>
        /// センテンスAIテーブルをアップデートします。uidで参照して下さい。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="uid"></param>
        /// <param name="str"></param>
        /// <param name="keywords"></param>
        public static void SetT_SentenceAI_Update(SqlConnection sqlConnection, string str)
        {

            //エンコード
            str = HtmlEncode(str);

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
                    command.Parameters.Add(new SqlParameter("@sentence", System.Data.SqlDbType.NVarChar, -1, "sentence")).Value = str.Trim();
                    command.Parameters.Add(new SqlParameter("@up_day", System.Data.SqlDbType.DateTime, 8, "up_day")).Value = DateTime.Now;

                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "UPDATE [T_SentenceAI] SET [sentence]=CAST(LTRIM(RTRIM(@sentence)) AS NVarChar(max)), [up_day]=@up_day WHERE([sentence] = LTRIM(RTRIM(@sentence)))";


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


    }
}