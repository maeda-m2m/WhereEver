using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WhereEver.ClassLibrary
{
    public class FileShareClass
    {


        public static DATASET.DataSet.T_FileShareRow GetT_FileShareRow(SqlConnection sqlConnection, string FileName, string pass)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得
            da.SelectCommand.Parameters.AddWithValue("@FileName", FileName);
            da.SelectCommand.Parameters.AddWithValue("@pass", pass);

            da.SelectCommand.CommandText =
                "SELECT * FROM T_FileShare WHERE [FileName] = LTRIM(RTRIM(@FileName)) AND [Password] = LTRIM(RTRIM(@pass))";

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_FileShareDataTable dt = new DATASET.DataSet.T_FileShareDataTable();

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


        //------------------------------------------------------------------------------------------------------------
        //T_FileShare   DELETE
        //------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 指定したT_FileShareのDataTableRowを削除します。
        /// 中身がない場合や入力値が不正な場合はfalseを返します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns>bool</returns>
        public static bool DeleteT_FileShareRow(SqlConnection sqlConnection, string id, string FileName)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得
            da.SelectCommand.Parameters.AddWithValue("@id", id);
            da.SelectCommand.Parameters.AddWithValue("@FileName", FileName);

            da.SelectCommand.CommandText =
                "DELETE FROM T_FileShare WHERE id = LTRIM(RTRIM(@id)) AND FileName = LTRIM(RTRIM(@FileName))";

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_FileShareDataTable dt = new DATASET.DataSet.T_FileShareDataTable();

            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);
                if (dt.Count == 0)
                {
                    return false;
                }
                return true;

            }
            catch
            {
                //不正な値が入力された場合やidが誤っている場合はnullを返します。
                return false;
            }

        }
        //------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// FileShareテーブルにインサートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id">主キー１：Session変数に保存されているUserIDです。</param>
        /// <param name="filename">主キー２：uuidによって構成されたファイルネームです。拡張子まで含まれています。</param>
        /// <param name="filepath">ファイルパスです。</param>
        public static bool SetT_FileShareInsert(SqlConnection sqlConnection, string id, string username, string filename, string title, string pass, string type, byte[] datum)
        {

            //結果の宣言と定義
            bool result = true;

            sqlConnection.Open();

            //Create the Update Command.
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //Sql Commandを作成します。
            SqlCommand command = sqlConnection.CreateCommand();

            //ファイルを書き込み可能なようにオープンしてSqlのデータをアップデートします。
            //Start a local transaction. usingブロックを抜けると自動でcloseされます。
            using (SqlTransaction transaction = sqlConnection.BeginTransaction())
            {

                //passwordの有無
                string ispass = "なし";
                if (pass != "")
                {
                    ispass = "あり";
                }

                //現在のDateTimeを取得
                DateTime date = DateTime.Now; 

                //Must assign both transaction object and connection
                //to Command object for apending local transaction
                command.Connection = sqlConnection;
                command.Transaction = transaction;


                    //Add the paramaters for the Updatecommand.必ずダブルクオーテーションで@変数の宣言を囲んでください。command.CommandTextで使用するものは、必ずすべて宣言してください。
                    //-------------------------------------------------------------------------------------------------------------------
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar, 100, "id")).Value = id;
                    command.Parameters.Add(new SqlParameter("@userName", System.Data.SqlDbType.NVarChar, 50, "userName")).Value = username;
                    command.Parameters.Add(new SqlParameter("@FileName", System.Data.SqlDbType.NVarChar, 100, "FileName")).Value = filename;
                    command.Parameters.Add(new SqlParameter("@Title", System.Data.SqlDbType.NVarChar, 100, "Title")).Value = title;
                    command.Parameters.Add(new SqlParameter("@Password", System.Data.SqlDbType.NVarChar, 100, "Password")).Value = pass;
                    command.Parameters.Add(new SqlParameter("@type", System.Data.SqlDbType.NVarChar, 50, "type")).Value = type;
                    command.Parameters.Add(new SqlParameter("@datum", System.Data.SqlDbType.VarBinary, -1, "datum")).Value = datum;
                    command.Parameters.Add(new SqlParameter("@DateTime", System.Data.SqlDbType.DateTime, 8, "DateTime")).Value = date;
                    command.Parameters.Add(new SqlParameter("@IsPass", System.Data.SqlDbType.Char, 2, "IsPass")).Value = ispass;

                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO T_FileShare([id], [userName], [filename], [title], [Password], [type], [datum], [DateTime], [IsPass]) VALUES(LTRIM(RTRIM(@id)), LTRIM(RTRIM(@userName)), LTRIM(RTRIM(@FileName)), LTRIM(RTRIM(@Title)), LTRIM(RTRIM(@Password)), LTRIM(RTRIM(@type)), CAST(@datum AS varbinary(max)), LTRIM(RTRIM(@DateTime)), LTRIM(RTRIM(@IsPass)))";

                try
                {

                    //このメソッドでは、XmlCommandTypeプロパティおよびCommandTextプロパティを使用してSQL文またはコマンドを実行し、影響を受ける行数を戻します（必須）。 
                    //ここでエラーが出る場合は、宣言やSql文が不正な場合があります。
                    command.ExecuteNonQuery();


                    //Attempt to commit the transaction.
                    da.UpdateCommand = command;
                    transaction.Commit();

                //Console.WriteLine("Insert Completed");

                }
                catch
                {
                    //catch文
                    //Console.WriteLine("Insert Failed");
                    transaction.Rollback();
                    result = false;
                }

            } //sqlConnection.Close();

            return result;

        }


    }
}