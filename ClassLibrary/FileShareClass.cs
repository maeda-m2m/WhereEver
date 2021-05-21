using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WhereEver.ClassLibrary
{
    public class FileShareClass
    {

        //------------------------------------------------------------------------------------------------------------
        //T_FileShare   SELECT
        //------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 共有ファイルをロードします。uuidと拡張子で構成されたFileNameとpassを入力して下さい。
        /// countは1～数えます。lengthは一度にロードするbyte数です。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="FileName"></param>
        /// <param name="pass"></param>
        /// <param name="count">処理は何回目か？</param>
        /// <param name="length"></param>
        /// <returns>DATASET.DataSet.T_FileShareRow</returns>
        public static DATASET.DataSet.T_FileShareRow GetT_FileShareRow(SqlConnection sqlConnection, string FileName, string pass ,int count,int length)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得
            da.SelectCommand.Parameters.AddWithValue("@FileName", FileName);
            da.SelectCommand.Parameters.AddWithValue("@pass", pass);
            da.SelectCommand.Parameters.AddWithValue("@count", count);
            da.SelectCommand.Parameters.AddWithValue("@length", length);

            //少しずつロード
            da.SelectCommand.CommandText =
                "SELECT [id], [userName], [FileName], [Title], [Password], [Ispass], [type], [datum]=SUBSTRING(CAST([datum] AS varbinary(max)),@count,@length), [size], [DateTime] FROM [T_FileShare] WHERE [FileName] = LTRIM(RTRIM(@FileName)) AND [Password] = LTRIM(RTRIM(@pass)) ORDER BY [DateTime] DESC";

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_FileShareDataTable dt = new DATASET.DataSet.T_FileShareDataTable();

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

            //sql接続開始
            sqlConnection.Open();

            //Sql Commandを作成します。
            SqlCommand command = sqlConnection.CreateCommand();

            //Must assign both transaction object and connection
            //to Command object for apending local transaction
            command.Connection = sqlConnection;

            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@FileName", FileName);

            command.CommandText =
                "DELETE FROM [T_FileShare] WHERE [id] = LTRIM(RTRIM(@id)) AND [FileName] = LTRIM(RTRIM(@FileName))";

            //特定のDataTableをインスタンス化
            //DATASET.DataSet.T_FileShareDataTable dt = new DATASET.DataSet.T_FileShareDataTable();

            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                if (command.ExecuteNonQuery() > 0)
                {
                    // データベースの接続終了
                    sqlConnection.Close();
                    return true;
                }
                else
                {
                    // データベースの接続終了
                    sqlConnection.Close();
                    return false;
                }

            }
            catch
            {
                //不正な値が入力された場合やidが誤っている場合はnullを返します。
                // データベースの接続終了
                sqlConnection.Close();
                return false;
            }
        }
        //------------------------------------------------------------------------------------------------------------



        /// <summary>
        /// FileShareテーブルにインサートやアップデートを実行します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id">主キー１：Session変数に保存されているUserIDです。</param>
        /// <param name="username">ユーザーの漢字名です。</param>
        /// <param name="filename">主キー２：uuidによって構成されたファイルネームです。拡張子まで含まれています。</param>
        /// <param name="title">ファイルに付属するタイトルないしコメントです。</param>
        /// <param name="pass">ファイルのパスワードです。空でも可。</param>
        /// <param name="type">ファイルのMIMEタイプです。</param>
        /// <param name="datum">データバイト配列です。</param>
        /// <param name="isinsert">trueでInsert、falseでUpdateします。初回は必ずtrueで実行してください。ストリーミング型で転送するときは２回目以降falseに設定してください。</param>
        /// <returns></returns>
        public static bool SetT_FileShareInsert(SqlConnection sqlConnection, string id, string username, string filename, string title, string pass, string type, byte[] datum, bool isinsert)
        {

            //結果の宣言と定義

            //最大サイズ　40MB = 40960KB
            //41943040f = 40MB * 1024 * 1024
            const float maxsize = 41943040f;    //byte

            //判定結果
            bool result = true;

            //sql接続開始
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

                //宣言
                float kiloFileSize = 0f;
                float megaFileSize = 0f;
                float gigaFileSize = 0f;

                // ファイルサイズをバイトで取得します。
                float size = (float)datum.Length;


                if (size > maxsize)
                {
                    //ファイルが大きすぎます！
                    result = false;
                    return result;
                }

                //ファイルのサイズを取得
                string printFileSize = string.Format("{0:f2} B", size);

                if (size >= 1024f)
                {
                    kiloFileSize = size / 1024f; // バイト→キロバイトに変換
                    printFileSize = string.Format("{0:f2} KB", kiloFileSize);
                }

                if (kiloFileSize >= 1024f)
                {
                    megaFileSize = kiloFileSize / 1024f; // キロバイト→メガバイトに変換
                    printFileSize = string.Format("{0:f2} MB", megaFileSize);
                }

                if (megaFileSize >= 1024f)
                {
                    gigaFileSize = megaFileSize / 1024f; // メガバイト→ギガバイトに変換
                    printFileSize = string.Format("{0:f2} GB", gigaFileSize);
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
                command.Parameters.Add(new SqlParameter("@FileName", System.Data.SqlDbType.NVarChar, 100, "FileName")).Value = filename;
                command.Parameters.Add(new SqlParameter("@datum", System.Data.SqlDbType.VarBinary, -1, "datum")).Value = datum;

                if (isinsert)
                {
                    //Add the paramaters for the Updatecommand.必ずダブルクオーテーションで@変数の宣言を囲んでください。command.CommandTextで使用するものは、必ずすべて宣言してください。
                    //-------------------------------------------------------------------------------------------------------------------
                    command.Parameters.Add(new SqlParameter("@userName", System.Data.SqlDbType.NVarChar, 50, "userName")).Value = username;
                    command.Parameters.Add(new SqlParameter("@Title", System.Data.SqlDbType.NVarChar, 100, "Title")).Value = title;
                    command.Parameters.Add(new SqlParameter("@Password", System.Data.SqlDbType.NVarChar, 100, "Password")).Value = pass;
                    command.Parameters.Add(new SqlParameter("@type", System.Data.SqlDbType.NVarChar, 50, "type")).Value = type;
                    command.Parameters.Add(new SqlParameter("@DateTime", System.Data.SqlDbType.DateTime, 8, "DateTime")).Value = date;
                    command.Parameters.Add(new SqlParameter("@size", System.Data.SqlDbType.NVarChar, 50, "size")).Value = printFileSize;
                    command.Parameters.Add(new SqlParameter("@IsPass", System.Data.SqlDbType.Char, 2, "IsPass")).Value = ispass;
                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO T_FileShare([id], [userName], [filename], [title], [Password], [type], [datum], [DateTime], [size], [IsPass]) VALUES(LTRIM(RTRIM(@id)), LTRIM(RTRIM(@userName)), LTRIM(RTRIM(@FileName)), LTRIM(RTRIM(@Title)), LTRIM(RTRIM(@Password)), LTRIM(RTRIM(@type)), CAST(@datum AS varbinary(max)), LTRIM(RTRIM(@DateTime)), LTRIM(RTRIM(@size)), LTRIM(RTRIM(@IsPass)))";
                }
                else
                {
                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "UPDATE [T_FileShare] SET [datum] += CAST(@datum AS varbinary(max)) WHERE id = LTRIM(RTRIM(@id)) AND FileName = LTRIM(RTRIM(@FileName))";
                }

                //ストリーミング型のときは下記のようにByteを追加すると実装できそう
                //UPDATE TOP (1) [T_FileShare] SET datum += (0x00);

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

            // データベースの接続終了
            sqlConnection.Close();
            return result;

        }


    }
}