using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WhereEver.ClassLibrary
{
    public class ShinseiLog
    {



        //------------------------------------------------------------------------------------------------------------
        //UUID
        //------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// UUIDがダブらないようにするためのクラスです。
        /// UserIDをもとに列情報を取得します。
        /// trueならダブりあり、falseならダブりなしです。例外時はtrueを返します。
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="sqlConnection"></param>
        /// <returns></returns>
        public static bool GetT_Shinsei_Main_isuid(SqlConnection sqlConnection, string uid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            //UserIDからNULL以外の列数を取得します。例えばUserIDがguestひとつなら1を返します。
            //LIKE @name%は0個以上の文字（前にも後ろにもつけられます）、@name_は「１」文字、
            //[@name]は[]内に指定した任意の文字です。 = @nameなら完全一致です。
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Shinsei_Main WHERE uid = LTRIM(RTRIM(@uid))";

            //パラメータを取得
            da.SelectCommand.Parameters.AddWithValue("@uid", uid);

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_Shinsei_MainDataTable dt = new DATASET.DataSet.T_Shinsei_MainDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                if (dt.Count >= 1)
                {
                    //ダブりあり
                    // データベースの接続終了
                    sqlConnection.Close();
                    return true;

                }
                else
                {
                    //ダブりなし
                    // データベースの接続終了
                    sqlConnection.Close();
                    return false;
                }

            }
            catch
            {
                //例えばguestなどの不正な値が入力された場合はtrueを返します。
                // データベースの接続終了
                sqlConnection.Close();
                return true;
            }

        }


        //------------------------------------------------------------------------------------------------------------
        //T_Shinsei_Config
        //------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// T_Shinsei_ConfigのDataTableを返します。
        /// 引数はDATASET.DataSet.T_Shinsei_ConfigDataTable型で参照して下さい。
        /// 中身がない場合や入力値が不正な場合はnullを返します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <returns>DATASET.DataSet.T_Shinsei_ConfigDataTable</returns>
        public static DATASET.DataSet.T_Shinsei_ConfigDataTable GetT_Shinsei_ConfigRow(SqlConnection sqlConnection, string id)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            //idとuidからNULL以外の列数を取得します。
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Shinsei_Config WHERE id = LTRIM(RTRIM(@id))";

            //パラメータを取得（必要のないパラメータを設定するとコンパイルエラーする）
            da.SelectCommand.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar, 100, "id")).Value = id;

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_Shinsei_ConfigDataTable dt = new DATASET.DataSet.T_Shinsei_ConfigDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                if (dt.Count >= 1)
                {
                    //中身あり
                    // データベースの接続終了
                    sqlConnection.Close();
                    return dt;  //dt[0]の中にflag_del_popなどが入っています。

                }
                else
                {
                    //中身なし
                    // データベースの接続終了
                    sqlConnection.Close();
                    return null;
                }

            }
            catch
            {
                //不正な値が入力された場合はnullを返します。
                // データベースの接続終了
                sqlConnection.Close();
                return null;
            }

        }

        /// <summary>
        /// 申請コンフィグテーブルのフラグをアップデートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="flag_del_pop"></param>
        public static void SetT_Shinsei_ConfigUpdate(SqlConnection sqlConnection, string id, bool flag_del_pop)
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
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar, 100, "id")).Value = id;
                    command.Parameters.Add(new SqlParameter("@flag_del_pop", System.Data.SqlDbType.Bit, 20, "flag_del_pop")).Value = flag_del_pop;


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "UPDATE [T_Shinsei_Config] SET[flag_del_pop] = @flag_del_pop WHERE([id] = LTRIM(RTRIM(@id)))";

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
        /// 申請コンフィグテーブルにインサートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="flag_del_pop">消去時にポップアップするかの判定です</param>
        public static void SetT_Shinsei_ConfigInsert(SqlConnection sqlConnection, string id, bool flag_del_pop)
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
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar, 100, "id")).Value = id;
                    command.Parameters.Add(new SqlParameter("@flag_del_pop", System.Data.SqlDbType.Bit, 20, "flag_del_pop")).Value = flag_del_pop;

                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO T_Shinsei_Config(id, flag_del_pop) VALUES(LTRIM(RTRIM(@id)), @flag_del_pop)";


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


        //------------------------------------------------------------------------------------------------------------
        //T_Shinsei   DELETE
        //------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 指定したT_Shinsei_MainのDataTableRowを削除します。
        /// 中身がない場合や入力値が不正な場合はfalseを返します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns>bool</returns>
        public static bool DeleteT_Shinsei_MainRow(SqlConnection sqlConnection, string id, string uid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            //idとuidからNULL以外の列数を取得します。
            da.SelectCommand.CommandText =
                "DELETE FROM T_Shinsei_Main WHERE id = LTRIM(RTRIM(@id)) AND uid = LTRIM(RTRIM(@uid))";

            //パラメータを取得
            da.SelectCommand.Parameters.AddWithValue("@id", id);
            da.SelectCommand.Parameters.AddWithValue("@uid", uid);

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_Shinsei_MainDataTable dt = new DATASET.DataSet.T_Shinsei_MainDataTable();

            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);
                // データベースの接続終了
                sqlConnection.Close();
                return true;

            }
            catch
            {
                //不正な値が入力された場合はnullを返します。
                // データベースの接続終了
                sqlConnection.Close();
                return false;
            }

        }

        /// <summary>
        /// 指定したT_Shinsei_A_BuyのDataTableRowを削除します。
        /// 中身がない場合や入力値が不正な場合はfalseを返します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns>bool</returns>
        public static bool DeleteT_Shinsei_A_BuyRow(SqlConnection sqlConnection, string id, string uid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            //idとuidからNULL以外の列数を取得します。
            da.SelectCommand.CommandText =
                "DELETE FROM T_Shinsei_A_Buy WHERE id = LTRIM(RTRIM(@id)) AND uid = LTRIM(RTRIM(@uid))";

            //パラメータを取得
            da.SelectCommand.Parameters.AddWithValue("@id", id);
            da.SelectCommand.Parameters.AddWithValue("@uid", uid);

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_Shinsei_MainDataTable dt = new DATASET.DataSet.T_Shinsei_MainDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);
                // データベースの接続終了
                sqlConnection.Close();
                return true;

                //test
            }
            catch
            {
                //不正な値が入力された場合はnullを返します。
                // データベースの接続終了
                sqlConnection.Close();
                return false;
            }

        }

        /// <summary>
        /// 指定したT_Shinsei_B_DiligenceのDataTableRowを削除します。
        /// 中身がない場合や入力値が不正な場合はfalseを返します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns>bool</returns>
        public static bool DeleteT_Shinsei_B_Diligence(SqlConnection sqlConnection, string id, string uid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            //idとuidからNULL以外の列数を取得します。
            da.SelectCommand.CommandText =
                "DELETE FROM T_Shinsei_B_Diligence WHERE id = LTRIM(RTRIM(@id)) AND uid = LTRIM(RTRIM(@uid))";

            //パラメータを取得
            da.SelectCommand.Parameters.AddWithValue("@id", id);
            da.SelectCommand.Parameters.AddWithValue("@uid", uid);

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_Shinsei_MainDataTable dt = new DATASET.DataSet.T_Shinsei_MainDataTable();


            try
            {

                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);
                // データベースの接続終了
                sqlConnection.Close();
                return true;

            }
            catch
            {
                //不正な値が入力された場合はnullを返します。
                // データベースの接続終了
                sqlConnection.Close();
                return false;
            }

        }


        /// <summary>
        /// 指定したT_Shinsei_C_TatekaeのDataTableRowを削除します。
        /// 中身がない場合や入力値が不正な場合はfalseを返します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns>bool</returns>
        public static bool DeleteT_Shinsei_C_Tatekae(SqlConnection sqlConnection, string id, string uid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            //idとuidからNULL以外の列数を取得します。
            da.SelectCommand.CommandText =
                "DELETE FROM T_Shinsei_C_Tatekae WHERE id = LTRIM(RTRIM(@id)) AND uid = LTRIM(RTRIM(@uid))";

            //パラメータを取得
            da.SelectCommand.Parameters.AddWithValue("@id", id);
            da.SelectCommand.Parameters.AddWithValue("@uid", uid);

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_Shinsei_MainDataTable dt = new DATASET.DataSet.T_Shinsei_MainDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);
                // データベースの接続終了
                sqlConnection.Close();
                return true;

            }
            catch
            {
                //不正な値が入力された場合はnullを返します。
                // データベースの接続終了
                sqlConnection.Close();
                return false;
            }

        }

        //------------------------------------------------------------------------------------------------------------
        //T_Shinsei_Main
        //------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// T_Shinsei_MainのDataTableを返します。
        /// 引数はDATASET.DataSet.T_Shinsei_MainDataTable型で参照して下さい。
        /// 中身がない場合や入力値が不正な場合はnullを返します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns>DATASET.DataSet.T_Shinsei_MainDataTable</returns>
        public static DATASET.DataSet.T_Shinsei_MainDataTable GetT_Shinsei_MainRow(SqlConnection sqlConnection, string id, string uid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            //idとuidからNULL以外の列数を取得します。
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Shinsei_Main WHERE id = LTRIM(RTRIM(@id)) AND uid = LTRIM(RTRIM(@uid))";

            //パラメータを取得
            da.SelectCommand.Parameters.AddWithValue("@id", id);
            da.SelectCommand.Parameters.AddWithValue("@uid", uid);

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_Shinsei_MainDataTable dt = new DATASET.DataSet.T_Shinsei_MainDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                if (dt.Count >= 1)
                {
                    //中身あり
                    // データベースの接続終了
                    sqlConnection.Close();
                    return dt;

                }
                else
                {
                    //中身なし
                    // データベースの接続終了
                    sqlConnection.Close();
                    return null;
                }

            }
            catch
            {
                //不正な値が入力された場合はnullを返します。
                // データベースの接続終了
                sqlConnection.Close();
                return null;
            }

        }

        /// <summary>
        /// 申請サブマスターのLastUpdateをアップデートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        public static void SetT_Shinsei_MainUpdate(SqlConnection sqlConnection, string id, string uid)
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
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar, 100, "id")).Value = id;
                    command.Parameters.Add(new SqlParameter("@uid", System.Data.SqlDbType.NVarChar, 100, "uid")).Value = uid;
                    command.Parameters.Add(new SqlParameter("@LastUpdate", System.Data.SqlDbType.DateTime, 8, "LastUpdate")).Value = date;


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "UPDATE [T_Shinsei_Main] SET[LastUpdate] = @LastUpdate WHERE([id] = LTRIM(RTRIM(@id)) AND [uid] = LTRIM(RTRIM(@uid)))";

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
        /// 申請サブマスターにインサートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <param name="name1"></param>
        /// <param name="ShinseiSyubetsu"></param>
        public static void SetT_Shinsei_MainInsert(SqlConnection sqlConnection, string id, string uid, string name1, string ShinseiSyubetsu)
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
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar, 100, "id")).Value = id;
                    command.Parameters.Add(new SqlParameter("@uid", System.Data.SqlDbType.NVarChar, 100, "uid")).Value = uid;
                    command.Parameters.Add(new SqlParameter("@name1", System.Data.SqlDbType.NVarChar, 100, "name1")).Value = name1;
                    command.Parameters.Add(new SqlParameter("@ShinseiSyubetsu", System.Data.SqlDbType.NChar, 20, "ShinseiSyubetsu")).Value = ShinseiSyubetsu;
                    command.Parameters.Add(new SqlParameter("@DateTime", System.Data.SqlDbType.DateTime, 8, "DateTime")).Value = date;
                    command.Parameters.Add(new SqlParameter("@LastUpdate", System.Data.SqlDbType.DateTime, 8, "LastUpdate")).Value = date;


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO T_Shinsei_Main(id, uid, name1, ShinseiSyubetsu, DateTime, LastUpdate) VALUES(LTRIM(RTRIM(@id)), LTRIM(RTRIM(@uid)), @name1, @ShinseiSyubetsu, @DateTime, @LastUpdate)";


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



        //------------------------------------------------------------------------------------------------------------
        //T_Shinsei_A_Buy
        //------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// T_Shinsei_A_BuyのDataTableを返します。
        /// 引数はDATASET.DataSet.T_Shinsei_A_BuyDataTable型で参照して下さい。
        /// 中身がない場合や入力値が不正な場合はnullを返します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns>DATASET.DataSet.T_Shinsei_A_BuyDataTable</returns>
        public static DATASET.DataSet.T_Shinsei_A_BuyDataTable GetT_Shinsei_A_BuyRow(SqlConnection sqlConnection, string id, string uid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            //idとuidからNULL以外の列数を取得します。
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Shinsei_A_Buy WHERE id = LTRIM(RTRIM(@id)) AND uid = LTRIM(RTRIM(@uid))";

            //パラメータを取得
            da.SelectCommand.Parameters.AddWithValue("@id", id);
            da.SelectCommand.Parameters.AddWithValue("@uid", uid);

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_Shinsei_A_BuyDataTable dt = new DATASET.DataSet.T_Shinsei_A_BuyDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                if (dt.Count >= 1)
                {
                    //中身あり
                    // データベースの接続終了
                    sqlConnection.Close();
                    return dt;

                }
                else
                {
                    //中身なし
                    // データベースの接続終了
                    sqlConnection.Close();
                    return null;
                }

            }
            catch
            {
                //不正な値が入力された場合はnullを返します。
                // データベースの接続終了
                sqlConnection.Close();
                return null;
            }

        }

        /// <summary>
        /// 申請A_Buyをアップデートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <param name="A_BuyItem"></param>
        /// <param name="A_BuyKind"></param>
        /// <param name="A_BuyHowMany"></param>
        /// <param name="A_BuyHowMach"></param>
        /// <param name="A_BuyPlace"></param>
        /// <param name="A_Buy_Because"></param>
        /// <param name="A_Buy_ps"></param>
            public static void SetT_Shinsei_A_BuyUpdate(SqlConnection sqlConnection, string id, string uid, string A_BuyItem, string A_BuyKind, string A_BuyHowMany, string A_BuyHowMach, string A_BuyPlace, string A_Buy_Because, string A_Buy_ps)
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
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar, 100, "id")).Value = id;
                    command.Parameters.Add(new SqlParameter("@uid", System.Data.SqlDbType.NVarChar, 100, "uid")).Value = uid;

                    command.Parameters.Add(new SqlParameter("@A_BuyItem", System.Data.SqlDbType.NVarChar, 100, "A_BuyItem")).Value = A_BuyItem;
                    command.Parameters.Add(new SqlParameter("@A_BuyKind", System.Data.SqlDbType.NVarChar, 100, "A_BuyKind")).Value = A_BuyKind;
                    command.Parameters.Add(new SqlParameter("@A_BuyHowMany", System.Data.SqlDbType.NVarChar, 100, "A_BuyHowMany")).Value = A_BuyHowMany;
                    command.Parameters.Add(new SqlParameter("@A_BuyHowMach", System.Data.SqlDbType.NVarChar, 100, "A_BuyHowMach")).Value = A_BuyHowMach;
                    command.Parameters.Add(new SqlParameter("@A_BuyPlace", System.Data.SqlDbType.NVarChar, 100, "A_BuyPlace")).Value = A_BuyPlace;
                    command.Parameters.Add(new SqlParameter("@A_Buy_Because", System.Data.SqlDbType.NVarChar, -1, "A_Buy_Because")).Value = A_Buy_Because;
                    command.Parameters.Add(new SqlParameter("@A_Buy_ps", System.Data.SqlDbType.NVarChar, -1, "A_Buy_ps")).Value = A_Buy_ps;


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "UPDATE [T_Shinsei_A_Buy] SET [A_BuyItem]= @A_BuyItem, [A_BuyKind]= @A_BuyKind, [A_BuyHowMany]= @A_BuyHowMany, [A_BuyHowMach]= @A_BuyHowMach, [A_BuyPlace]= @A_BuyPlace, [A_Buy_Because]= @A_Buy_Because, [A_Buy_ps]= @A_Buy_ps WHERE([id] = LTRIM(RTRIM(@id)) AND [uid] = LTRIM(RTRIM(@uid)))";

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
        /// 申請A_Buyにインサートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <param name="A_BuyItem"></param>
        /// <param name="A_BuyKind"></param>
        /// <param name="A_BuyHowMany"></param>
        /// <param name="A_BuyHowMach"></param>
        /// <param name="A_BuyPlace"></param>
        /// <param name="A_Buy_Because"></param>
        /// <param name="A_Buy_ps"></param>
        public static void SetT_Shinsei_A_BuyInsert(SqlConnection sqlConnection, string id, string uid, string A_BuyItem, string A_BuyKind, string A_BuyHowMany, string A_BuyHowMach, string A_BuyPlace, string A_Buy_Because, string A_Buy_ps)
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

                //Null Reference対策
                if (A_Buy_Because == "")
                {
                    A_Buy_Because = "なし";
                }
                if (A_Buy_ps == "")
                {
                    A_Buy_ps = "なし";
                }

                try
                {
                    //Add the paramaters for the Updatecommand.必ずダブルクオーテーションで@変数の宣言を囲んでください。command.CommandTextで使用するものは、必ずすべて宣言してください。
                    //-------------------------------------------------------------------------------------------------------------------
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar, 100, "id")).Value = id;
                    command.Parameters.Add(new SqlParameter("@uid", System.Data.SqlDbType.NVarChar, 100, "uid")).Value = uid;

                    command.Parameters.Add(new SqlParameter("@A_BuyItem", System.Data.SqlDbType.NVarChar, 100, "A_BuyItem")).Value = A_BuyItem;
                    command.Parameters.Add(new SqlParameter("@A_BuyKind", System.Data.SqlDbType.NVarChar, 100, "A_BuyKind")).Value = A_BuyKind;
                    command.Parameters.Add(new SqlParameter("@A_BuyHowMany", System.Data.SqlDbType.NVarChar, 100, "A_BuyHowMany")).Value = A_BuyHowMany;
                    command.Parameters.Add(new SqlParameter("@A_BuyHowMach", System.Data.SqlDbType.NVarChar, 100, "A_BuyHowMach")).Value = A_BuyHowMach;
                    command.Parameters.Add(new SqlParameter("@A_BuyPlace", System.Data.SqlDbType.NVarChar, 100, "A_BuyPlace")).Value = A_BuyPlace;
                    command.Parameters.Add(new SqlParameter("@A_Buy_Because", System.Data.SqlDbType.NVarChar, -1, "A_Buy_Because")).Value = A_Buy_Because;
                    command.Parameters.Add(new SqlParameter("@A_Buy_ps", System.Data.SqlDbType.NVarChar, -1, "A_Buy_ps")).Value = A_Buy_ps;


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO T_Shinsei_A_Buy(id, uid, A_BuyItem, A_BuyKind, A_BuyHowMany, A_BuyHowMach, A_BuyPlace, A_Buy_Because, A_Buy_ps) VALUES(LTRIM(RTRIM(@id)), LTRIM(RTRIM(@uid)), @A_BuyItem, @A_BuyKind, @A_BuyHowMany, @A_BuyHowMach, @A_BuyPlace, @A_Buy_Because, @A_Buy_ps)";


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


        //------------------------------------------------------------------------------------------------------------
        //T_Shinsei_B_Diligence
        //------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// T_Shinsei_B_DiligenceのDataTableを返します。
        /// 引数はDATASET.DataSet.T_Shinsei_B_DiligenceDataTable型で参照して下さい。
        /// 中身がない場合や入力値が不正な場合はnullを返します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns>DATASET.DataSet.T_Shinsei_B_DiligenceDataTable</returns>
        public static DATASET.DataSet.T_Shinsei_B_DiligenceDataTable GetT_Shinsei_B_DiligenceRow(SqlConnection sqlConnection, string id, string uid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            //idとuidからNULL以外の列数を取得します。
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Shinsei_B_Diligence WHERE id = LTRIM(RTRIM(@id)) AND uid = LTRIM(RTRIM(@uid))";

            //パラメータを取得
            da.SelectCommand.Parameters.AddWithValue("@id", id);
            da.SelectCommand.Parameters.AddWithValue("@uid", uid);

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_Shinsei_B_DiligenceDataTable dt = new DATASET.DataSet.T_Shinsei_B_DiligenceDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                if (dt.Count >= 1)
                {
                    //中身あり
                    // データベースの接続終了
                    sqlConnection.Close();
                    return dt;

                }
                else
                {
                    //中身なし
                    // データベースの接続終了
                    sqlConnection.Close();
                    return null;
                }

            }
            catch
            {
                //不正な値が入力された場合はnullを返します。
                // データベースの接続終了
                sqlConnection.Close();
                return null;
            }

        }

        /// <summary>
        /// 申請B_Diligenceをアップデートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <param name="B_DiligenceClassification"></param>
        /// <param name="B_DiligenceDateA1"></param>
        /// <param name="B_DiligenceDateA2"></param>
        /// <param name="B_DiligenceDateB1"></param>
        /// <param name="B_DiligenceDateB2"></param>
        /// <param name="B_Diligence_Because"></param>
        /// <param name="B_Diligence_ps"></param>
        public static void SetT_Shinsei_B_DiligenceUpdate(SqlConnection sqlConnection, string id, string uid, string B_DiligenceClassification, string B_DiligenceDateA1, string B_DiligenceDateA2, string B_DiligenceDateB1, string B_DiligenceDateB2, string B_Diligence_Because, string B_Diligence_ps)
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
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar, 100, "id")).Value = id;
                    command.Parameters.Add(new SqlParameter("@uid", System.Data.SqlDbType.NVarChar, 100, "uid")).Value = uid;

                    command.Parameters.Add(new SqlParameter("@B_DiligenceClassification", System.Data.SqlDbType.NChar, 20, "B_DiligenceClassification")).Value = B_DiligenceClassification;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceDateA1", System.Data.SqlDbType.NChar, 20, "B_DiligenceDateA1")).Value = B_DiligenceDateA1;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceDateA2", System.Data.SqlDbType.NChar, 20, "B_DiligenceDateA2")).Value = B_DiligenceDateA2;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceDateB1", System.Data.SqlDbType.NChar, 20, "B_DiligenceDateB1")).Value = B_DiligenceDateB1;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceDateB2", System.Data.SqlDbType.NChar, 20, "B_DiligenceDateB2")).Value = B_DiligenceDateB2;
                    command.Parameters.Add(new SqlParameter("@B_Diligence_Because", System.Data.SqlDbType.NVarChar, -1, "B_Diligence_Because")).Value = B_Diligence_Because;
                    command.Parameters.Add(new SqlParameter("@B_Diligence_ps", System.Data.SqlDbType.NVarChar, -1, "B_Diligence_ps")).Value = B_Diligence_ps;


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "UPDATE [T_Shinsei_B_Diligence] SET [B_DiligenceClassification]= @B_DiligenceClassification, [B_DiligenceDateA1]= @B_DiligenceDateA1, [B_DiligenceDateA2]= @B_DiligenceDateA2, [B_DiligenceDateB1]= @B_DiligenceDateB1, [B_DiligenceDateB2]= @B_DiligenceDateB2 WHERE([id] = LTRIM(RTRIM(@id)) AND [uid] = LTRIM(RTRIM(@uid)))";

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
        /// 申請B_Diligenceにインサートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <param name="B_DiligenceClassification"></param>
        /// <param name="B_DiligenceDateA1"></param>
        /// <param name="B_DiligenceDateA2"></param>
        /// <param name="B_DiligenceDateB1"></param>
        /// <param name="B_DiligenceDateB2"></param>
        /// <param name="B_Diligence_Because"></param>
        /// <param name="B_Diligence_ps"></param>
        public static void SetT_Shinsei_B_DiligenceInsert(SqlConnection sqlConnection, string id, string uid, string B_DiligenceClassification, string B_DiligenceDateA1, string B_DiligenceDateA2, string B_DiligenceDateB1, string B_DiligenceDateB2, string B_Diligence_Because, string B_Diligence_ps)
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

                //Null Reference対策
                if (B_Diligence_Because == "")
                {
                    B_Diligence_Because = "なし";
                }
                if (B_Diligence_ps == "")
                {
                    B_Diligence_ps = "なし";
                }

                try
                {
                    //Add the paramaters for the Updatecommand.必ずダブルクオーテーションで@変数の宣言を囲んでください。command.CommandTextで使用するものは、必ずすべて宣言してください。
                    //-------------------------------------------------------------------------------------------------------------------
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar, 100, "id")).Value = id;
                    command.Parameters.Add(new SqlParameter("@uid", System.Data.SqlDbType.NVarChar, 100, "uid")).Value = uid;

                    command.Parameters.Add(new SqlParameter("@B_DiligenceClassification", System.Data.SqlDbType.NChar, 20, "B_DiligenceClassification")).Value = B_DiligenceClassification;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceDateA1", System.Data.SqlDbType.NChar, 20, "B_DiligenceDateA1")).Value = B_DiligenceDateA1;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceDateA2", System.Data.SqlDbType.NChar, 20, "B_DiligenceDateA2")).Value = B_DiligenceDateA2;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceDateB1", System.Data.SqlDbType.NChar, 20, "B_DiligenceDateB1")).Value = B_DiligenceDateB1;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceDateB2", System.Data.SqlDbType.NChar, 20, "B_DiligenceDateB2")).Value = B_DiligenceDateB2;
                    command.Parameters.Add(new SqlParameter("@B_Diligence_Because", System.Data.SqlDbType.NVarChar, -1, "B_Diligence_Because")).Value = B_Diligence_Because;
                    command.Parameters.Add(new SqlParameter("@B_Diligence_ps", System.Data.SqlDbType.NVarChar, -1, "B_Diligence_ps")).Value = B_Diligence_ps;


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO T_Shinsei_B_Diligence(id, uid,  B_DiligenceClassification, B_DiligenceDateA1, B_DiligenceDateA2, B_DiligenceDateB1, B_DiligenceDateB2, B_Diligence_Because, B_Diligence_ps) VALUES(LTRIM(RTRIM(@id)), LTRIM(RTRIM(@uid)), @B_DiligenceClassification, @B_DiligenceDateA1, @B_DiligenceDateA2, @B_DiligenceDateB1, @B_DiligenceDateB2, @B_Diligence_Because, @B_Diligence_ps)";


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


        //------------------------------------------------------------------------------------------------------------
        //T_Shinsei_C_Tatekae
        //------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// T_Shinsei_B_DiligenceのDataTableを返します。
        /// 引数はDATASET.DataSet.T_Shinsei_B_DiligenceDataTable型で参照して下さい。
        /// 中身がない場合や入力値が不正な場合はnullを返します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns>DATASET.DataSet.T_Shinsei_C_TatekaeDataTable</returns>
        public static DATASET.DataSet.T_Shinsei_C_TatekaeDataTable GetT_Shinsei_C_TatekaeRow(SqlConnection sqlConnection, string id, string uid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            //idとuidからNULL以外の列数を取得します。
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Shinsei_C_Tatekae WHERE id = LTRIM(RTRIM(@id)) AND uid = LTRIM(RTRIM(@uid))";

            //パラメータを取得
            da.SelectCommand.Parameters.AddWithValue("@id", id);
            da.SelectCommand.Parameters.AddWithValue("@uid", uid);

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_Shinsei_C_TatekaeDataTable dt = new DATASET.DataSet.T_Shinsei_C_TatekaeDataTable();


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
                // データベースの接続終了
                sqlConnection.Close();
                return null;
            }

        }

        /// <summary>
        /// 申請C_Tatekaeをアップデートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <param name="C_Tatekae_Result_Main"></param>
        /// <param name="C_Tatekae_TWaste"></param>
        /// <param name="C_Tatekae_PWaste"></param>
        /// <param name="C_Tatekae_Result1"></param>
        /// <param name="C_Tatekae_Result2"></param>
        /// <param name="C_Tatekae_Result3"></param>
        public static void SetT_Shinsei_C_TatekaeUpdate(SqlConnection sqlConnection, string id, string uid, string C_Tatekae_Result_Main, string C_Tatekae_TWaste, string C_Tatekae_PWaste, string C_Tatekae_Result1, string C_Tatekae_Result2, string C_Tatekae_Result3)
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
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar, 100, "id")).Value = id;
                    command.Parameters.Add(new SqlParameter("@uid", System.Data.SqlDbType.NVarChar, 100, "uid")).Value = uid;

                    command.Parameters.Add(new SqlParameter("@C_Tatekae_Result_Main", System.Data.SqlDbType.NVarChar, -1, "C_Tatekae_Result_Main")).Value = C_Tatekae_Result_Main;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_TWaste", System.Data.SqlDbType.NVarChar, 100, "C_Tatekae_TWaste")).Value = C_Tatekae_TWaste;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_PWaste", System.Data.SqlDbType.NVarChar, 100, "C_Tatekae_PWaste")).Value = C_Tatekae_PWaste;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_Result1", System.Data.SqlDbType.NVarChar, 100, "C_Tatekae_Result1")).Value = C_Tatekae_Result1;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_Result2", System.Data.SqlDbType.NVarChar, 100, "C_Tatekae_Result2")).Value = C_Tatekae_Result2;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_Result3", System.Data.SqlDbType.NVarChar, 100, "C_Tatekae_Result3")).Value = C_Tatekae_Result3;

                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "UPDATE [T_Shinsei_C_Tatekae] SET [C_Tatekae_Result_Main]= @C_Tatekae_Result_Main, [C_Tatekae_TWaste]= @C_Tatekae_TWaste, [C_Tatekae_PWaste]= @C_Tatekae_PWaste, [C_Tatekae_Result1]= @C_Tatekae_Result1, [C_Tatekae_Result2]= @C_Tatekae_Result2, [C_Tatekae_Result3]= @C_Tatekae_Result3 WHERE([id] = LTRIM(RTRIM(@id)) AND [uid] = LTRIM(RTRIM(@uid)))";

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
        /// 申請C_Tatekaeにインサートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <param name="C_Tatekae_Result_Main"></param>
        /// <param name="C_Tatekae_TWaste"></param>
        /// <param name="C_Tatekae_PWaste"></param>
        /// <param name="C_Tatekae_Result1"></param>
        /// <param name="C_Tatekae_Result2"></param>
        /// <param name="C_Tatekae_Result3"></param>
        public static void SetT_Shinsei_C_TatekaeInsert(SqlConnection sqlConnection, string id, string uid, string C_Tatekae_Result_Main, string C_Tatekae_TWaste, string C_Tatekae_PWaste, string C_Tatekae_Result1, string C_Tatekae_Result2, string C_Tatekae_Result3)
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
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar, 100, "id")).Value = id;
                    command.Parameters.Add(new SqlParameter("@uid", System.Data.SqlDbType.NVarChar, 100, "uid")).Value = uid;

                    command.Parameters.Add(new SqlParameter("@C_Tatekae_Result_Main", System.Data.SqlDbType.NVarChar, -1, "C_Tatekae_Result_Main")).Value = C_Tatekae_Result_Main;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_TWaste", System.Data.SqlDbType.NVarChar, 100, "C_Tatekae_TWaste")).Value = C_Tatekae_TWaste;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_PWaste", System.Data.SqlDbType.NVarChar, 100, "C_Tatekae_PWaste")).Value = C_Tatekae_PWaste;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_Result1", System.Data.SqlDbType.NVarChar, 100, "C_Tatekae_Result1")).Value = C_Tatekae_Result1;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_Result2", System.Data.SqlDbType.NVarChar, 100, "C_Tatekae_Result2")).Value = C_Tatekae_Result2;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_Result3", System.Data.SqlDbType.NVarChar, 100, "C_Tatekae_Result3")).Value = C_Tatekae_Result3;


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO T_Shinsei_C_Tatekae(id, uid,  C_Tatekae_Result_Main, C_Tatekae_TWaste, C_Tatekae_PWaste, C_Tatekae_Result1, C_Tatekae_Result2, C_Tatekae_Result3) VALUES(LTRIM(RTRIM(@id)), LTRIM(RTRIM(@uid)), @C_Tatekae_Result_Main, @C_Tatekae_TWaste, @C_Tatekae_PWaste, @C_Tatekae_Result1, @C_Tatekae_Result2, @C_Tatekae_Result3)";


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