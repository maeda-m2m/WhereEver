using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WhereEver.ClassLibrary
{
    public static class RentalClass
    {

        /// <summary>
        /// T_Rentalを参照します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public static DATASET.DataSet.T_RentalRow GetT_Rental(SqlConnection sqlConnection, string uuid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得
            da.SelectCommand.Parameters.AddWithValue("@uuid", uuid);

            da.SelectCommand.CommandText =
                "SELECT * FROM [T_Rental] WHERE uuid = LTRIM(RTRIM(@uuid))";


            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_RentalDataTable dt = new DATASET.DataSet.T_RentalDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                if (dt.Count >= 1)
                {
                    //該当あり
                    return dt[0];

                }
                else
                {
                    //該当なし
                    return null;
                }

            }
            catch
            {
                //不正な値
                return null;
            }

        }

        public static bool SetT_RentalDelete(SqlConnection sqlConnection, string uuid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得
            da.SelectCommand.Parameters.AddWithValue("@uuid", uuid);

            da.SelectCommand.CommandText =
                "DELETE TOP(1) FROM [T_Rental] WHERE uuid = LTRIM(RTRIM(@uuid))";


            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_RentalDataTable dt = new DATASET.DataSet.T_RentalDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);
                return true;
            }
            catch
            {
                //不正な値
                return false;
            }

        }

        /// <summary>
        /// T_Rentalを更新します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="uuid"></param>
        /// <param name="order_date"></param>
        /// <param name="order_name"></param>
        /// <param name="youki"></param>
        /// <param name="seiki"></param>
        /// <param name="order_rest"></param>
        /// <param name="order_shipping_date"></param>
        /// <param name="receive_date"></param>
        /// <param name="send_d"></param>
        /// <param name="send_rest"></param>
        /// <param name="send"></param>
        /// <param name="rental_name"></param>
        /// <param name="rental_type"></param>
        /// <param name="rental_tanka"></param>
        /// <param name="rental_amount"></param>
        /// <param name="rental_total_amount"></param>
        public static void SetT_RentalUpdate(SqlConnection sqlConnection, string uuid, DateTime? order_date, string order_name, DateTime? youki, DateTime? seiki, int order_rest, DateTime? order_shipping_date, DateTime? receive_date, DateTime? send_d, int send_rest, DateTime? send, string rental_name, string rental_type, int rental_tanka, int rental_amount, int rental_total_amount)
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
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar, 20, "id")).Value = SessionManager.User.M_User.id;
                    command.Parameters.Add(new SqlParameter("@uuid", System.Data.SqlDbType.NVarChar, 50, "uuid")).Value = uuid;
                    command.Parameters.Add(new SqlParameter("@order_date", System.Data.SqlDbType.Date, 3, "order_date")).Value = order_date;
                    command.Parameters.Add(new SqlParameter("@order_name", System.Data.SqlDbType.NVarChar, 50, "order_name")).Value = order_name;
                    command.Parameters.Add(new SqlParameter("@order_youki", System.Data.SqlDbType.Date, 3, "order_youki")).Value = youki;
                    command.Parameters.Add(new SqlParameter("@order_seiki", System.Data.SqlDbType.Date, 3, "order_seiki")).Value = seiki;
                    command.Parameters.Add(new SqlParameter("@order_rest", System.Data.SqlDbType.BigInt, 8, "order_rest")).Value = order_rest;
                    command.Parameters.Add(new SqlParameter("@order_shipping_date", System.Data.SqlDbType.Date, 3, "order_shipping_date")).Value = order_shipping_date;
                    command.Parameters.Add(new SqlParameter("@receive_date", System.Data.SqlDbType.Date, 3, "receive_date")).Value = receive_date;
                    command.Parameters.Add(new SqlParameter("@send_deadline", System.Data.SqlDbType.Date, 3, "send_deadline")).Value = send_d;
                    command.Parameters.Add(new SqlParameter("@send_rest", System.Data.SqlDbType.BigInt, 8, "send_rest")).Value = send_rest;
                    command.Parameters.Add(new SqlParameter("@send_date", System.Data.SqlDbType.Date, 3, "send_date")).Value = send;
                    command.Parameters.Add(new SqlParameter("@rental_name", System.Data.SqlDbType.NVarChar, 50, "rental_name")).Value = rental_name;
                    command.Parameters.Add(new SqlParameter("@rental_type", System.Data.SqlDbType.NVarChar, 50, "rental_type")).Value = rental_type;
                    command.Parameters.Add(new SqlParameter("@rental_tanka", System.Data.SqlDbType.BigInt, 8, "rental_tanka")).Value = rental_tanka;
                    command.Parameters.Add(new SqlParameter("@rental_amount", System.Data.SqlDbType.BigInt, 8, "rental_amount")).Value = rental_amount;
                    command.Parameters.Add(new SqlParameter("@rental_total_amount", System.Data.SqlDbType.BigInt, 8, "rental_total_amount")).Value = rental_total_amount;
                    command.Parameters.Add(new SqlParameter("@up_day", System.Data.SqlDbType.DateTime, 8, "up_day")).Value = DateTime.Now;

                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "UPDATE [T_Rental] SET[id] = @id, [uuid]=@uuid, [order_date]=@order_date, [order_name]=@order_name, [order_youki]=@order_youki, [order_seiki]=@order_seiki, [order_rest]=@order_rest, [order_shipping_date]=@order_shipping_date, [receive_date]=@receive_date, [send_deadline]=@send_deadline, [send_rest]=@send_rest, [send_date]=@send_date, [rental_name]=@rental_name, [rental_type]=@rental_type, [rental_tanka]=@rental_tanka, [rental_amount]=@rental_amount, [rental_total_amount]=@rental_total_amount, [up_day]=@up_day WHERE([uuid] = LTRIM(RTRIM(@uuid)))";

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
        /// T_Rentalに代入します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="uuid"></param>
        /// <param name="order_date"></param>
        /// <param name="order_name"></param>
        /// <param name="youki"></param>
        /// <param name="seiki"></param>
        /// <param name="order_rest"></param>
        /// <param name="order_shipping_date"></param>
        /// <param name="receive_date"></param>
        /// <param name="send_d"></param>
        /// <param name="send_rest"></param>
        /// <param name="send"></param>
        /// <param name="rental_name"></param>
        /// <param name="rental_type"></param>
        /// <param name="rental_tanka"></param>
        /// <param name="rental_amount"></param>
        /// <param name="rental_total_amount"></param>
        public static void SetT_RentalInsert(SqlConnection sqlConnection, string uuid, DateTime? order_date, string order_name, DateTime? youki, DateTime? seiki, int order_rest, DateTime? order_shipping_date, DateTime? receive_date, DateTime? send_d, int send_rest, DateTime? send, string rental_name, string rental_type, int rental_tanka, int rental_amount, int rental_total_amount)
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
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar, 20, "id")).Value = SessionManager.User.M_User.id;
                    command.Parameters.Add(new SqlParameter("@uuid", System.Data.SqlDbType.NVarChar, 50, "uuid")).Value = uuid;
                    command.Parameters.Add(new SqlParameter("@order_date", System.Data.SqlDbType.Date, 3, "order_date")).Value = order_date;
                    command.Parameters.Add(new SqlParameter("@order_name", System.Data.SqlDbType.NVarChar, 50, "order_name")).Value = order_name;
                    command.Parameters.Add(new SqlParameter("@order_youki", System.Data.SqlDbType.Date, 3, "order_youki")).Value = youki;
                    command.Parameters.Add(new SqlParameter("@order_seiki", System.Data.SqlDbType.Date, 3, "order_seiki")).Value = seiki;
                    command.Parameters.Add(new SqlParameter("@order_rest", System.Data.SqlDbType.BigInt, 8, "order_rest")).Value = order_rest;
                    command.Parameters.Add(new SqlParameter("@order_shipping_date", System.Data.SqlDbType.Date, 3, "order_shipping_date")).Value = order_shipping_date;
                    command.Parameters.Add(new SqlParameter("@receive_date", System.Data.SqlDbType.Date, 3, "receive_date")).Value = receive_date;
                    command.Parameters.Add(new SqlParameter("@send_deadline", System.Data.SqlDbType.Date, 3, "send_deadline")).Value = send_d;
                    command.Parameters.Add(new SqlParameter("@send_rest", System.Data.SqlDbType.BigInt, 8, "send_rest")).Value = send_rest;
                    command.Parameters.Add(new SqlParameter("@send_date", System.Data.SqlDbType.Date, 3, "send_date")).Value = send;
                    command.Parameters.Add(new SqlParameter("@rental_name", System.Data.SqlDbType.NVarChar, 50, "rental_name")).Value = rental_name;
                    command.Parameters.Add(new SqlParameter("@rental_type", System.Data.SqlDbType.NVarChar, 50, "rental_type")).Value = rental_type;
                    command.Parameters.Add(new SqlParameter("@rental_tanka", System.Data.SqlDbType.BigInt, 8, "rental_tanka")).Value = rental_tanka;
                    command.Parameters.Add(new SqlParameter("@rental_amount", System.Data.SqlDbType.BigInt, 8, "rental_amount")).Value = rental_amount;
                    command.Parameters.Add(new SqlParameter("@rental_total_amount", System.Data.SqlDbType.BigInt, 8, "rental_total_amount")).Value = rental_total_amount;
                    command.Parameters.Add(new SqlParameter("@up_day", System.Data.SqlDbType.DateTime, 8, "up_day")).Value = DateTime.Now;

                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO [T_Rental] ([id], [uuid], [order_date], [order_name], [order_youki], [order_seiki], [order_rest], [order_shipping_date], [receive_date], [send_deadline], [send_rest], [send_date], [rental_name], [rental_type], [rental_tanka], [rental_amount], [rental_total_amount], [up_day]) VALUES(@id, @uuid, @order_date, @order_name, @order_youki, @order_seiki, @order_rest, @order_shipping_date, @receive_date, @send_deadline, @send_rest, @send_date, @rental_name, @rental_type, @rental_tanka, @rental_amount, @rental_total_amount, @up_day)";

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