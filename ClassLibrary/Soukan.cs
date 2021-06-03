using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WhereEver.ClassLibrary
{
    public class Soukan
    {



        /// <summary>
        /// 申請コンフィグテーブルにインサートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="tableName"></param>
        /// <param name="item_A"></param>
        /// <param name="item_B"></param>
        public static void SetT_Soukan_Main(SqlConnection sqlConnection, string id, string tableName, float item_A, float item_B)
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
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar, 50, "id")).Value = id.Trim();
                    command.Parameters.Add(new SqlParameter("@uuid", System.Data.SqlDbType.NVarChar, 50, "uuid")).Value = Guid.NewGuid().ToString();
                    command.Parameters.Add(new SqlParameter("@a", System.Data.SqlDbType.Float, 8, "item_A")).Value = (float)item_A;
                    command.Parameters.Add(new SqlParameter("@b", System.Data.SqlDbType.Float, 8, "item_B")).Value = (float)item_B;
                    command.Parameters.Add(new SqlParameter("@table", System.Data.SqlDbType.NVarChar, 50, "TableName")).Value = tableName.Trim();
                    command.Parameters.Add(new SqlParameter("@date", System.Data.SqlDbType.DateTime, 8, "DateTime")).Value = DateTime.Now;

                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO T_Soukan_Main(id, uuid, TableName, item_A, item_B, DateTime) VALUES(LTRIM(RTRIM(@id)), @uuid, @table, @a, @b, @date)";


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