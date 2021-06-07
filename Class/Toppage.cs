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
                    command.CommandText = "UPDATE TOP(1) [T_TopPage] SET[TopPage] = CAST(@TopPage AS nvarchar(MAX)), [DateTime]=@DateTime";

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
                    return dt[0];

                }
                else
                {
                    //ファイルなし
                    return null;
                }
            }
            catch
            {
                //不正な処理
                return null;
            }

        }



        /// <summary>
        /// 現在の日付（PC依存）以降の天気データを取得します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <returns>DATASET.DataSet.T_WeatherDataTable</returns>
        public static DATASET.DataSet.T_WeatherDataTable GetT_Weather(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得
            //da.SelectCommand.Parameters.AddWithValue("@FileName", FileName);
            //da.SelectCommand.Parameters.AddWithValue("@pass", pass);
            da.SelectCommand.Parameters.AddWithValue("@Date", DateTime.Now.Date);

            da.SelectCommand.CommandText =
                "SELECT * FROM [T_Weather] WHERE CAST([Date] AS DATE) >= CAST(@Date AS DATE)";

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_WeatherDataTable dt = new DATASET.DataSet.T_WeatherDataTable();

            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                if (dt.Count >= 1)
                {
                    //ファイルあり
                    return dt;

                }
                else
                {
                    //ファイルなし
                    return null;
                }
            }
            catch
            {
                //不正な処理
                return null;
            }

        }

        /// <summary>
        /// 入力した日付の天気データを取得します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="date"></param>
        /// <returns>DATASET.DataSet.T_WeatherRow</returns>
        public static DATASET.DataSet.T_WeatherRow GetT_WeatherSelect(SqlConnection sqlConnection, DateTime date)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得
            //da.SelectCommand.Parameters.AddWithValue("@FileName", FileName);
            //da.SelectCommand.Parameters.AddWithValue("@pass", pass);
            da.SelectCommand.Parameters.AddWithValue("@Date", date.Date);

            da.SelectCommand.CommandText =
                "SELECT * FROM [T_Weather] WHERE CAST([Date] AS DATE) = CAST(@Date AS DATE)";

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_WeatherDataTable dt = new DATASET.DataSet.T_WeatherDataTable();

            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                if (dt.Count >= 1)
                {
                    //ファイルあり
                    return dt[0];

                }
                else
                {
                    //ファイルなし
                    return null;
                }
            }
            catch
            {
                //不正な処理
                return null;
            }

        }






        public static void SetT_WeatherUpdate(SqlConnection sqlConnection, DateTime date, string weather, int maxtemp, int mintemp, int rain_p)
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
                    command.Parameters.Add(new SqlParameter("@Date", System.Data.SqlDbType.Date, 3, "Date")).Value = date;
                    command.Parameters.Add(new SqlParameter("@Weather", System.Data.SqlDbType.NVarChar, 10, "Weather")).Value = weather;
                    command.Parameters.Add(new SqlParameter("@MaxTemp", System.Data.SqlDbType.Int, 4, "MaxTemp")).Value = maxtemp;
                    command.Parameters.Add(new SqlParameter("@MinTemp", System.Data.SqlDbType.Int, 4, "MinTemp")).Value = mintemp;
                    command.Parameters.Add(new SqlParameter("@Rain_p", System.Data.SqlDbType.Int, 4, "Rain_p")).Value = rain_p;

                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "UPDATE [T_Weather] SET [Date] = CAST(@Date AS DATE), [Weather]=@Weather, [MaxTemp]=@MaxTemp, [MinTemp]=@MinTemp, [Rain_p]=@Rain_p WHERE CAST([Date] AS DATE) = CAST(@Date AS DATE)";

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



        public static void SetT_WeatherInsert(SqlConnection sqlConnection, DateTime date, string weather, int maxtemp, int mintemp, int rain_p)
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
                    command.Parameters.Add(new SqlParameter("@Date", System.Data.SqlDbType.Date, 3, "Date")).Value = date;
                    command.Parameters.Add(new SqlParameter("@Weather", System.Data.SqlDbType.NVarChar, 10, "Weather")).Value = weather;
                    command.Parameters.Add(new SqlParameter("@MaxTemp", System.Data.SqlDbType.Int, 4, "MaxTemp")).Value = maxtemp;
                    command.Parameters.Add(new SqlParameter("@MinTemp", System.Data.SqlDbType.Int, 4, "MinTemp")).Value = mintemp;
                    command.Parameters.Add(new SqlParameter("@Rain_p", System.Data.SqlDbType.Int, 4, "Rain_p")).Value = rain_p;

                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO [T_Weather] ([Date], [Weather], [MaxTemp], [MinTemp],[Rain_p]) VALUES(CAST(@Date AS DATE), @Weather, @MaxTemp, @MinTemp, @Rain_p)";

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