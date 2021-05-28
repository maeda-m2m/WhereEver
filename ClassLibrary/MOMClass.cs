using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using static System.Web.HttpUtility;

namespace WhereEver.ClassLibrary
{
    public class MOMClass
    {

        //------------------------------------------------------------------------------------------------------------
        //T_PL
        //------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// T_PLのDataTableを返します。
        /// 引数はDATASET.DataSet.T_PLRow型で参照して下さい。
        /// 中身がない場合や入力値が不正な場合はnullを返します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="uuid"></param>
        /// <returns>DATASET.DataSet.T_PLRow</returns>
        public static DATASET.DataSet.T_PLRow GetT_PLRow(SqlConnection sqlConnection, string uuid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得（必要のないパラメータを設定するとコンパイルエラーする）
            da.SelectCommand.Parameters.Add(new SqlParameter("@uuid", System.Data.SqlDbType.NVarChar, 50, "uuid")).Value = uuid;

            //idとuidからNULL以外の列数を取得します。
            da.SelectCommand.CommandText =
                "SELECT * FROM [T_PL] WHERE [uuid] = LTRIM(RTRIM(@uuid))";

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_PLDataTable dt = new DATASET.DataSet.T_PLDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                if (dt.Count >= 1)
                {
                    //中身あり
                    return dt[0];  //dt[0]の中にflag_del_popなどが入っています。

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
        /// T_PLから指定したuuidのRowをDeleteします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="uuid"></param>
        public static void DeleteT_PLRow(SqlConnection sqlConnection, string uuid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得（必要のないパラメータを設定するとコンパイルエラーする）
            da.SelectCommand.Parameters.Add(new SqlParameter("@uuid", System.Data.SqlDbType.NVarChar, 50, "uuid")).Value = uuid;

            //idとuidからNULL以外の列数を取得します。
            da.SelectCommand.CommandText =
                "DELETE FROM [T_PL] WHERE [uuid] = LTRIM(RTRIM(@uuid))";

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_PLDataTable dt = new DATASET.DataSet.T_PLDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);
                return;
            }
            catch
            {
                //不正な値が入力された場合はnullを返します。
                return;
            }

        }


        /// <summary>
        /// P/Lテーブルにインサートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="uriagedaka"></param>
        /// <param name="uriagegenka"></param>
        /// <param name="hanbaikanrihi"></param>
        /// <param name="eigyourieki"></param>
        /// <param name="eigyougaihiyou"></param>
        /// <param name="tokubetsurieki"></param>
        /// <param name="tokubetsusonshitsu"></param>
        /// <param name="houjinzeitou"></param>
        /// <param name="Date_S"></param>
        /// <param name="Date_G"></param>
        public static void SetT_PLInsert(SqlConnection sqlConnection, int uriagedaka, int uriagegenka, int hanbaikanrihi, int eigyourieki, int eigyougaihiyou, int tokubetsurieki, int tokubetsusonshitsu, int houjinzeitou, DateTime Date_S, DateTime Date_G)
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
                    command.Parameters.Add(new SqlParameter("@uuid", System.Data.SqlDbType.NVarChar, 50, "uuid")).Value = Guid.NewGuid().ToString();
                    command.Parameters.Add(new SqlParameter("@uriagedaka", System.Data.SqlDbType.BigInt, 8, "uriagedaka")).Value = uriagedaka;
                    command.Parameters.Add(new SqlParameter("@uriagegenka", System.Data.SqlDbType.BigInt, 8, "uriagegenka")).Value = uriagegenka;
                    int uriagesourieki = uriagedaka - uriagegenka;
                    command.Parameters.Add(new SqlParameter("@uriagesourieki", System.Data.SqlDbType.BigInt, 8, "uriagesourieki")).Value = uriagesourieki;

                    command.Parameters.Add(new SqlParameter("@hanbaikanrihi", System.Data.SqlDbType.BigInt, 8, "hanbaikanrihi")).Value = hanbaikanrihi;
                    command.Parameters.Add(new SqlParameter("@eigyourieki", System.Data.SqlDbType.BigInt, 8, "eigyourieki")).Value = eigyourieki;
                    command.Parameters.Add(new SqlParameter("@eigyougaihiyou", System.Data.SqlDbType.BigInt, 8, "eigyougaihiyou")).Value = eigyougaihiyou;
                    int keijyourieki = uriagesourieki - hanbaikanrihi + eigyourieki - eigyougaihiyou;
                    command.Parameters.Add(new SqlParameter("@keijyourieki", System.Data.SqlDbType.BigInt, 8, "keijyourieki")).Value = keijyourieki;

                    command.Parameters.Add(new SqlParameter("@tokubetsurieki", System.Data.SqlDbType.BigInt, 8, "tokubetsurieki")).Value = tokubetsurieki;
                    command.Parameters.Add(new SqlParameter("@tokubetsusonshitsu", System.Data.SqlDbType.BigInt, 8, "tokubetsusonshitsu")).Value = tokubetsusonshitsu;
                    int zeibikimaetoukijyunrieki = keijyourieki + tokubetsurieki - tokubetsusonshitsu;                 
                    command.Parameters.Add(new SqlParameter("@zeibikimaetoukijyunrieki", System.Data.SqlDbType.BigInt, 8, "zeibikimaetoukijyunrieki")).Value = zeibikimaetoukijyunrieki;

                    command.Parameters.Add(new SqlParameter("@houjinzeitou", System.Data.SqlDbType.BigInt, 8, "houjinzeitou")).Value = houjinzeitou;
                    int toukijyunrieki = zeibikimaetoukijyunrieki - houjinzeitou;
                    command.Parameters.Add(new SqlParameter("@toukijyunrieki", System.Data.SqlDbType.BigInt, 8, "toukijyunrieki")).Value = toukijyunrieki;

                    float arari_r;
                    float eigyou_r;
                    float keijyou_r;

                    if ((float)uriagedaka != 0)
                    {
                        arari_r = (float)uriagesourieki / (float)uriagedaka;
                    }
                    else
                    {
                        arari_r = 0f;
                    }

                    command.Parameters.Add(new SqlParameter("@arari_r", System.Data.SqlDbType.Float, 8, "arari_r")).Value = arari_r;


                    if ((float)uriagedaka != 0)
                    {
                        eigyou_r = (float)eigyourieki / (float)uriagedaka;
                    }
                    else
                    {
                        eigyou_r = 0f;
                    }

                    command.Parameters.Add(new SqlParameter("@eigyou_r", System.Data.SqlDbType.Float, 8, "eigyou_r")).Value = eigyou_r;


                    if ((float)uriagedaka != 0)
                    {
                        keijyou_r = (float)keijyourieki / (float)uriagedaka;
                    }
                    else
                    {
                        keijyou_r = 0f;
                    }

                    command.Parameters.Add(new SqlParameter("@keijyou_r", System.Data.SqlDbType.Float, 8, "keijyou_r")).Value = keijyou_r;

                    command.Parameters.Add(new SqlParameter("@Date_S", System.Data.SqlDbType.Date, 3, "Date_S")).Value = Date_S;
                    command.Parameters.Add(new SqlParameter("@Date_G", System.Data.SqlDbType.Date, 3, "Date_G")).Value = Date_G;
                    command.Parameters.Add(new SqlParameter("@UpDateTime", System.Data.SqlDbType.DateTime, 8, "UpDateTime")).Value = DateTime.Now;


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO [T_PL](uuid, uriagedaka, uriagegenka, uriagesourieki, hanbaikanrihi, eigyourieki, eigyougaihiyou, keijyourieki, tokubetsurieki, tokubetsusonshitsu, zeibikimaetoukijyunrieki, houjinzeitou, toukijyunrieki, arari_r, eigyou_r, keijyou_r, Date_S, Date_G, UpDateTime) VALUES(LTRIM(RTRIM(@uuid)), @uriagedaka, @uriagegenka, @uriagesourieki, @hanbaikanrihi, @eigyourieki, @eigyougaihiyou, @keijyourieki, @tokubetsurieki, @tokubetsusonshitsu, @zeibikimaetoukijyunrieki, @houjinzeitou, @toukijyunrieki, @arari_r, @eigyou_r, @keijyou_r, CONVERT(Date, @Date_S), CONVERT(Date, @Date_G), CONVERT(DateTime, @UpDateTime))";


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
        /// P/Lテーブルにuuidをキーにアップデートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="uuid"></param>
        /// <param name="uriagedaka"></param>
        /// <param name="uriagegenka"></param>
        /// <param name="hanbaikanrihi"></param>
        /// <param name="eigyourieki"></param>
        /// <param name="eigyougaihiyou"></param>
        /// <param name="tokubetsurieki"></param>
        /// <param name="tokubetsusonshitsu"></param>
        /// <param name="houjinzeitou"></param>
        /// <param name="Date_S"></param>
        /// <param name="Date_G"></param>
        public static void SetT_PLUpdate(SqlConnection sqlConnection, string uuid, int uriagedaka, int uriagegenka, int hanbaikanrihi, int eigyourieki, int eigyougaihiyou, int tokubetsurieki, int tokubetsusonshitsu, int houjinzeitou, DateTime Date_S, DateTime Date_G)
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
                    command.Parameters.Add(new SqlParameter("@uuid", System.Data.SqlDbType.NVarChar, 50, "uuid")).Value = uuid;
                    command.Parameters.Add(new SqlParameter("@uriagedaka", System.Data.SqlDbType.BigInt, 8, "uriagedaka")).Value = uriagedaka;
                    command.Parameters.Add(new SqlParameter("@uriagegenka", System.Data.SqlDbType.BigInt, 8, "uriagegenka")).Value = uriagegenka;
                    int uriagesourieki = uriagedaka - uriagegenka;
                    command.Parameters.Add(new SqlParameter("@uriagesourieki", System.Data.SqlDbType.BigInt, 8, "uriagesourieki")).Value = uriagesourieki;

                    command.Parameters.Add(new SqlParameter("@hanbaikanrihi", System.Data.SqlDbType.BigInt, 8, "hanbaikanrihi")).Value = hanbaikanrihi;
                    command.Parameters.Add(new SqlParameter("@eigyourieki", System.Data.SqlDbType.BigInt, 8, "eigyourieki")).Value = eigyourieki;
                    command.Parameters.Add(new SqlParameter("@eigyougaihiyou", System.Data.SqlDbType.BigInt, 8, "eigyougaihiyou")).Value = eigyougaihiyou;
                    int keijyourieki = uriagesourieki - hanbaikanrihi + eigyourieki - eigyougaihiyou;
                    command.Parameters.Add(new SqlParameter("@keijyourieki", System.Data.SqlDbType.BigInt, 8, "keijyourieki")).Value = keijyourieki;

                    command.Parameters.Add(new SqlParameter("@tokubetsurieki", System.Data.SqlDbType.BigInt, 8, "tokubetsurieki")).Value = tokubetsurieki;
                    command.Parameters.Add(new SqlParameter("@tokubetsusonshitsu", System.Data.SqlDbType.BigInt, 8, "tokubetsusonshitsu")).Value = tokubetsusonshitsu;
                    int zeibikimaetoukijyunrieki = keijyourieki + tokubetsurieki - tokubetsusonshitsu;
                    command.Parameters.Add(new SqlParameter("@zeibikimaetoukijyunrieki", System.Data.SqlDbType.BigInt, 8, "zeibikimaetoukijyunrieki")).Value = zeibikimaetoukijyunrieki;

                    command.Parameters.Add(new SqlParameter("@houjinzeitou", System.Data.SqlDbType.BigInt, 8, "houjinzeitou")).Value = houjinzeitou;
                    int toukijyunrieki = zeibikimaetoukijyunrieki - houjinzeitou;
                    command.Parameters.Add(new SqlParameter("@toukijyunrieki", System.Data.SqlDbType.BigInt, 8, "toukijyunrieki")).Value = toukijyunrieki;

                    float arari_r;
                    float eigyou_r;
                    float keijyou_r;

                    if ((float)uriagedaka != 0)
                    {
                        arari_r = (float)uriagesourieki / (float)uriagedaka;
                    }
                    else
                    {
                        arari_r = 0f;
                    }

                    command.Parameters.Add(new SqlParameter("@arari_r", System.Data.SqlDbType.Float, 8, "arari_r")).Value = arari_r;


                    if ((float)uriagedaka != 0)
                    {
                        eigyou_r = (float)eigyourieki / (float)uriagedaka;
                    }
                    else
                    {
                        eigyou_r = 0f;
                    }

                    command.Parameters.Add(new SqlParameter("@eigyou_r", System.Data.SqlDbType.Float, 8, "eigyou_r")).Value = eigyou_r;


                    if ((float)uriagedaka != 0)
                    {
                        keijyou_r = (float)keijyourieki / (float)uriagedaka;
                    }
                    else
                    {
                        keijyou_r = 0f;
                    }

                    command.Parameters.Add(new SqlParameter("@keijyou_r", System.Data.SqlDbType.Float, 8, "keijyou_r")).Value = keijyou_r;

                    command.Parameters.Add(new SqlParameter("@Date_S", System.Data.SqlDbType.Date, 3, "Date_S")).Value = Date_S;
                    command.Parameters.Add(new SqlParameter("@Date_G", System.Data.SqlDbType.Date, 3, "Date_G")).Value = Date_G;
                    command.Parameters.Add(new SqlParameter("@UpDateTime", System.Data.SqlDbType.DateTime, 8, "UpDateTime")).Value = DateTime.Now;


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "UPDATE [T_PL] SET [uuid]=@uuid, [uriagedaka]=@uriagedaka, [uriagegenka]=@uriagegenka, [uriagesourieki]=@uriagesourieki, [hanbaikanrihi]=@hanbaikanrihi, [eigyourieki]=@eigyourieki, [eigyougaihiyou]=@eigyougaihiyou, [keijyourieki]=@keijyourieki, [tokubetsurieki]=@tokubetsurieki, [tokubetsusonshitsu]=@tokubetsusonshitsu, [zeibikimaetoukijyunrieki]=@zeibikimaetoukijyunrieki, [houjinzeitou]=@houjinzeitou, [toukijyunrieki]=@toukijyunrieki, [arari_r]=@arari_r, [eigyou_r]=@eigyou_r, [keijyou_r]=@keijyou_r, [Date_S]= CONVERT(Date, @Date_S), [Date_G]= CONVERT(Date, @Date_G), [UpDateTime]=CONVERT(DateTime, @UpDateTime) WHERE [uuid] = LTRIM(RTRIM(@uuid))";


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
        //T_BS
        //------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// T_BSのDataTableを返します。
        /// 引数はDATASET.DataSet.T_BSRow型で参照して下さい。
        /// 中身がない場合や入力値が不正な場合はnullを返します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="uuid"></param>
        /// <returns>DATASET.DataSet.T_BSRow</returns>
        public static DATASET.DataSet.T_BSRow GetT_BSRow(SqlConnection sqlConnection, string uuid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得（必要のないパラメータを設定するとコンパイルエラーする）
            da.SelectCommand.Parameters.Add(new SqlParameter("@uuid", System.Data.SqlDbType.NVarChar, 50, "uuid")).Value = uuid;

            //idとuidからNULL以外の列数を取得します。
            da.SelectCommand.CommandText =
                "SELECT * FROM [T_BS] WHERE [uuid] = LTRIM(RTRIM(@uuid))";

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_BSDataTable dt = new DATASET.DataSet.T_BSDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                if (dt.Count >= 1)
                {
                    //中身あり
                    return dt[0];  //dt[0]の中にflag_del_popなどが入っています。

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
        /// T_BSから指定したuuidのRowをDeleteします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="uuid"></param>
        public static void DeleteT_BSRow(SqlConnection sqlConnection, string uuid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得（必要のないパラメータを設定するとコンパイルエラーする）
            da.SelectCommand.Parameters.Add(new SqlParameter("@uuid", System.Data.SqlDbType.NVarChar, 50, "uuid")).Value = uuid;

            //idとuidからNULL以外の列数を取得します。
            da.SelectCommand.CommandText =
                "DELETE FROM [T_BS] WHERE [uuid] = LTRIM(RTRIM(@uuid))";

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_BSDataTable dt = new DATASET.DataSet.T_BSDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);
                return;
            }
            catch
            {
                //不正な値が入力された場合はnullを返します。
                return;
            }

        }


        /// <summary>
        /// B/Sテーブルにインサートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="v4"></param>
        /// <param name="v5"></param>
        /// <param name="v6"></param>
        /// <param name="v7"></param>
        /// <param name="v8"></param>
        /// <param name="v9"></param>
        /// <param name="v10"></param>
        /// <param name="v11"></param>
        /// <param name="v12"></param>
        /// <param name="v13"></param>
        /// <param name="v14"></param>
        /// <param name="v15"></param>
        /// <param name="v16"></param>
        /// <param name="v17"></param>
        /// <param name="v18"></param>
        /// <param name="v19"></param>
        /// <param name="v20"></param>
        /// <param name="v21"></param>
        /// <param name="v22"></param>
        /// <param name="v23"></param>
        /// <param name="v24"></param>
        /// <param name="v25"></param>
        /// <param name="v26"></param>
        /// <param name="v27"></param>
        /// <param name="v28"></param>
        /// <param name="v29"></param>
        /// <param name="v30"></param>
        /// <param name="v31"></param>
        /// <param name="v32"></param>
        /// <param name="v33"></param>
        /// <param name="v34"></param>
        /// <param name="v35"></param>
        /// <param name="v36"></param>
        /// <param name="v37"></param>
        /// <param name="v38"></param>
        /// <param name="v39"></param>
        /// <param name="v40"></param>
        /// <param name="v41"></param>
        /// <param name="v42"></param>
        /// <param name="v43"></param>
        /// <param name="v44"></param>
        /// <param name="v45"></param>
        /// <param name="v46"></param>
        /// <param name="v47"></param>
        /// <param name="v48"></param>
        /// <param name="v49"></param>
        /// <param name="v50"></param>
        /// <param name="v51"></param>
        /// <param name="Date"></param>
        public static void SetT_BSInsert(SqlConnection sqlConnection, int v1, int v2, int v3, int v4, int v5, int v6, int v7, int v8, int v9, int v10, int v11, int v12, int v13, int v14, int v15, int v16, int v17, int v18, int v19, int v20, int v21, int v22, int v23, int v24, int v25, int v26, int v27, int v28, int v29, int v30, int v31, int v32, int v33, int v34, int v35, int v36, int v37, int v38, int v39, int v40, int v41, int v42, int v43, int v44, int v45, int v46, int v47, int v48, int v49, int v50, int v51, DateTime Date)
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
                    command.Parameters.Add(new SqlParameter("@uuid", System.Data.SqlDbType.NVarChar, 50, "uuid")).Value = Guid.NewGuid().ToString();

                    command.Parameters.Add(new SqlParameter("@v1", System.Data.SqlDbType.BigInt, 8, "BS1")).Value = v1;
                    command.Parameters.Add(new SqlParameter("@v2", System.Data.SqlDbType.BigInt, 8, "BS2")).Value = v2;
                    command.Parameters.Add(new SqlParameter("@v3", System.Data.SqlDbType.BigInt, 8, "BS3")).Value = v3;
                    command.Parameters.Add(new SqlParameter("@v4", System.Data.SqlDbType.BigInt, 8, "BS4")).Value = v4;
                    command.Parameters.Add(new SqlParameter("@v5", System.Data.SqlDbType.BigInt, 8, "BS5")).Value = v5;
                    command.Parameters.Add(new SqlParameter("@v6", System.Data.SqlDbType.BigInt, 8, "BS6")).Value = v6;
                    command.Parameters.Add(new SqlParameter("@v7", System.Data.SqlDbType.BigInt, 8, "BS7")).Value = v7;
                    command.Parameters.Add(new SqlParameter("@v8", System.Data.SqlDbType.BigInt, 8, "BS8")).Value = v8;
                    command.Parameters.Add(new SqlParameter("@v9", System.Data.SqlDbType.BigInt, 8, "BS9")).Value = v9;
                    command.Parameters.Add(new SqlParameter("@v10", System.Data.SqlDbType.BigInt, 8, "BS10")).Value = v10;
                    command.Parameters.Add(new SqlParameter("@v11", System.Data.SqlDbType.BigInt, 8, "BS11")).Value = v11;
                    command.Parameters.Add(new SqlParameter("@v12", System.Data.SqlDbType.BigInt, 8, "BS12")).Value = v12;
                    command.Parameters.Add(new SqlParameter("@v13", System.Data.SqlDbType.BigInt, 8, "BS13")).Value = v13;
                    command.Parameters.Add(new SqlParameter("@v14", System.Data.SqlDbType.BigInt, 8, "BS14")).Value = v14;
                    command.Parameters.Add(new SqlParameter("@v15", System.Data.SqlDbType.BigInt, 8, "BS15")).Value = v15;
                    command.Parameters.Add(new SqlParameter("@v16", System.Data.SqlDbType.BigInt, 8, "BS16")).Value = v16;
                    command.Parameters.Add(new SqlParameter("@v17", System.Data.SqlDbType.BigInt, 8, "BS17")).Value = v17;
                    command.Parameters.Add(new SqlParameter("@v18", System.Data.SqlDbType.BigInt, 8, "BS18")).Value = v18;
                    command.Parameters.Add(new SqlParameter("@v19", System.Data.SqlDbType.BigInt, 8, "BS19")).Value = v19;
                    command.Parameters.Add(new SqlParameter("@v20", System.Data.SqlDbType.BigInt, 8, "BS20")).Value = v20;
                    command.Parameters.Add(new SqlParameter("@v21", System.Data.SqlDbType.BigInt, 8, "BS21")).Value = v21;
                    command.Parameters.Add(new SqlParameter("@v22", System.Data.SqlDbType.BigInt, 8, "BS22")).Value = v22;
                    command.Parameters.Add(new SqlParameter("@v23", System.Data.SqlDbType.BigInt, 8, "BS23")).Value = v23;
                    command.Parameters.Add(new SqlParameter("@v24", System.Data.SqlDbType.BigInt, 8, "BS24")).Value = v24;
                    command.Parameters.Add(new SqlParameter("@v25", System.Data.SqlDbType.BigInt, 8, "BS25")).Value = v25;
                    command.Parameters.Add(new SqlParameter("@v26", System.Data.SqlDbType.BigInt, 8, "BS26")).Value = v26;
                    command.Parameters.Add(new SqlParameter("@v27", System.Data.SqlDbType.BigInt, 8, "BS27")).Value = v27;
                    command.Parameters.Add(new SqlParameter("@v28", System.Data.SqlDbType.BigInt, 8, "BS28")).Value = v28;
                    command.Parameters.Add(new SqlParameter("@v29", System.Data.SqlDbType.BigInt, 8, "BS29")).Value = v29;
                    command.Parameters.Add(new SqlParameter("@v30", System.Data.SqlDbType.BigInt, 8, "BS30")).Value = v30;
                    command.Parameters.Add(new SqlParameter("@v31", System.Data.SqlDbType.BigInt, 8, "BS31")).Value = v31;
                    command.Parameters.Add(new SqlParameter("@v32", System.Data.SqlDbType.BigInt, 8, "BS32")).Value = v32;
                    command.Parameters.Add(new SqlParameter("@v33", System.Data.SqlDbType.BigInt, 8, "BS33")).Value = v33;
                    command.Parameters.Add(new SqlParameter("@v34", System.Data.SqlDbType.BigInt, 8, "BS34")).Value = v34;
                    command.Parameters.Add(new SqlParameter("@v35", System.Data.SqlDbType.BigInt, 8, "BS35")).Value = v35;
                    command.Parameters.Add(new SqlParameter("@v36", System.Data.SqlDbType.BigInt, 8, "BS36")).Value = v36;
                    command.Parameters.Add(new SqlParameter("@v37", System.Data.SqlDbType.BigInt, 8, "BS37")).Value = v37;
                    command.Parameters.Add(new SqlParameter("@v38", System.Data.SqlDbType.BigInt, 8, "BS38")).Value = v38;
                    command.Parameters.Add(new SqlParameter("@v39", System.Data.SqlDbType.BigInt, 8, "BS39")).Value = v39;
                    command.Parameters.Add(new SqlParameter("@v40", System.Data.SqlDbType.BigInt, 8, "BS40")).Value = v40;
                    command.Parameters.Add(new SqlParameter("@v41", System.Data.SqlDbType.BigInt, 8, "BS41")).Value = v41;
                    command.Parameters.Add(new SqlParameter("@v42", System.Data.SqlDbType.BigInt, 8, "BS42")).Value = v42;
                    command.Parameters.Add(new SqlParameter("@v43", System.Data.SqlDbType.BigInt, 8, "BS43")).Value = v43;
                    command.Parameters.Add(new SqlParameter("@v44", System.Data.SqlDbType.BigInt, 8, "BS44")).Value = v44;
                    command.Parameters.Add(new SqlParameter("@v45", System.Data.SqlDbType.BigInt, 8, "BS45")).Value = v45;
                    command.Parameters.Add(new SqlParameter("@v46", System.Data.SqlDbType.BigInt, 8, "BS46")).Value = v46;
                    command.Parameters.Add(new SqlParameter("@v47", System.Data.SqlDbType.BigInt, 8, "BS47")).Value = v47;
                    command.Parameters.Add(new SqlParameter("@v48", System.Data.SqlDbType.BigInt, 8, "BS48")).Value = v48;
                    command.Parameters.Add(new SqlParameter("@v49", System.Data.SqlDbType.BigInt, 8, "BS49")).Value = v49;
                    command.Parameters.Add(new SqlParameter("@v50", System.Data.SqlDbType.BigInt, 8, "BS50")).Value = v50;
                    command.Parameters.Add(new SqlParameter("@v51", System.Data.SqlDbType.BigInt, 8, "BS51")).Value = v51;
                    command.Parameters.Add(new SqlParameter("@Date", System.Data.SqlDbType.Date, 3, "Date")).Value = Date;
                    command.Parameters.Add(new SqlParameter("@UpDateTime", System.Data.SqlDbType.DateTime, 8, "UpDateTime")).Value = DateTime.Now;


                    //--------------------------            
                    //BS1-11　流動資産
                    int ryuudoushisan = v1 + v2 + v3 + v4 + v5 + v6 + v7 + v8 + v9 + v10 + v11;
                    command.Parameters.Add(new SqlParameter("@a1", System.Data.SqlDbType.BigInt, 8, "ryuudoushisan")).Value = ryuudoushisan;
                    //--------------------------            
                    //BS12-18　有形固定資産
                    int yuukeikoteishisan = v12 + v13 + v14 + v15 + v16 + v17 + v18;
                    command.Parameters.Add(new SqlParameter("@a2", System.Data.SqlDbType.BigInt, 8, "yuukeikoteishisan")).Value = yuukeikoteishisan;
                    //BS19-21　無形固定資産
                    int mukeikoteishisan = v19 + v20 + v21;
                    command.Parameters.Add(new SqlParameter("@a3", System.Data.SqlDbType.BigInt, 8, "mukeikoteishisan")).Value = mukeikoteishisan;
                    //BS22-28　投資その他の資産
                    int toushisonotanoshisan = v22 + v23 + v24 + v25 + v26 + v27 + v28;
                    command.Parameters.Add(new SqlParameter("@a4", System.Data.SqlDbType.BigInt, 8, "touhshisonotanoshisan")).Value = toushisonotanoshisan;
                    //BS12-28　固定資産(SUM)
                    int koteishisan = yuukeikoteishisan + mukeikoteishisan + toushisonotanoshisan;
                    command.Parameters.Add(new SqlParameter("@a5", System.Data.SqlDbType.BigInt, 8, "koteishisan")).Value = koteishisan;
                    //--------------------------            
                    //BS1-29　資産合計
                    int shisan_a = ryuudoushisan + koteishisan;
                    command.Parameters.Add(new SqlParameter("@ta1", System.Data.SqlDbType.BigInt, 8, "shisan_a")).Value = shisan_a;
                    //--------------------------
                    //BS29-37　流動負債
                    int ryuudoufusai = v29 + v30 + v31 + v32 + v33 + v34 + v35 + v36 + v37;
                    command.Parameters.Add(new SqlParameter("@a6", System.Data.SqlDbType.BigInt, 8, "ryuudoufusai")).Value = ryuudoufusai;
                    //BS38-40　固定負債
                    int koteifusai = v38 + v39 + v40;
                    command.Parameters.Add(new SqlParameter("@a7", System.Data.SqlDbType.BigInt, 8, "koteifusai")).Value = koteifusai;
                    //--------------------------
                    //BS29-40　負債合計
                    int fusai_a = ryuudoufusai + koteifusai;
                    command.Parameters.Add(new SqlParameter("@ta2", System.Data.SqlDbType.BigInt, 8, "fusai_a")).Value = fusai_a;
                    //--------------------------
                    //BS41-51　純資産
                    int jyunshisan_a = v41 + v42 + v43 + v44 + v45 + v46 + v47 + v48 + v49 + v50 + v51;
                    command.Parameters.Add(new SqlParameter("@ta3", System.Data.SqlDbType.BigInt, 8, "jyunshisan_a")).Value = jyunshisan_a;
                    //--------------------------


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO [T_BS](uuid, BS1, BS2, BS3, BS4, BS5, BS6, BS7, BS8, BS9, BS10, BS11, BS12, BS13, BS14, BS15, BS16, BS17, BS18, BS19, BS20, BS21, BS22, BS23, BS24, BS25, BS26, BS27, BS28, BS29, BS30, BS31, BS32, BS33, BS34, BS35, BS36, BS37, BS38, BS39, BS40, BS41, BS42, BS43, BS44, BS45, BS46, BS47, BS48, BS49, BS50, BS51, ryuudoushisan, yuukeikoteishisan, mukeikoteishisan, toushisonotanoshisan, koteishisan, ryuudoufusai, koteifusai, shisan_a, fusai_a, jyunshisan_a, Date, UpDateTime) VALUES(LTRIM(RTRIM(@uuid)), @v1, @v2, @v3, @v4, @v5, @v6, @v7, @v8, @v9, @v10, @v11, @v12, @v13, @v14, @v15, @v16, @v17, @v18, @v19, @v20, @v21, @v22, @v23, @v24, @v25, @v26, @v27, @v28, @v29, @v30, @v31, @v32, @v33, @v34, @v35, @v36, @v37, @v38, @v39, @v40, @v41, @v42, @v43, @v44, @v45, @v46, @v47, @v48, @v49, @v50, @v51, @a1, @a2, @a3, @a4, @a5, @a6, @a7, @ta1, @ta2, @ta3, CONVERT(Date, @Date), CONVERT(DateTime, @UpDateTime))";


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
        /// P/Lテーブルにuuidをキーにアップデートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="uuid"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="v4"></param>
        /// <param name="v5"></param>
        /// <param name="v6"></param>
        /// <param name="v7"></param>
        /// <param name="v8"></param>
        /// <param name="v9"></param>
        /// <param name="v10"></param>
        /// <param name="v11"></param>
        /// <param name="v12"></param>
        /// <param name="v13"></param>
        /// <param name="v14"></param>
        /// <param name="v15"></param>
        /// <param name="v16"></param>
        /// <param name="v17"></param>
        /// <param name="v18"></param>
        /// <param name="v19"></param>
        /// <param name="v20"></param>
        /// <param name="v21"></param>
        /// <param name="v22"></param>
        /// <param name="v23"></param>
        /// <param name="v24"></param>
        /// <param name="v25"></param>
        /// <param name="v26"></param>
        /// <param name="v27"></param>
        /// <param name="v28"></param>
        /// <param name="v29"></param>
        /// <param name="v30"></param>
        /// <param name="v31"></param>
        /// <param name="v32"></param>
        /// <param name="v33"></param>
        /// <param name="v34"></param>
        /// <param name="v35"></param>
        /// <param name="v36"></param>
        /// <param name="v37"></param>
        /// <param name="v38"></param>
        /// <param name="v39"></param>
        /// <param name="v40"></param>
        /// <param name="v41"></param>
        /// <param name="v42"></param>
        /// <param name="v43"></param>
        /// <param name="v44"></param>
        /// <param name="v45"></param>
        /// <param name="v46"></param>
        /// <param name="v47"></param>
        /// <param name="v48"></param>
        /// <param name="v49"></param>
        /// <param name="v50"></param>
        /// <param name="v51"></param>
        /// <param name="Date"></param>
        public static void SetT_BSUpdate(SqlConnection sqlConnection, string uuid, int v1, int v2, int v3, int v4, int v5, int v6, int v7, int v8, int v9, int v10, int v11, int v12, int v13, int v14, int v15, int v16, int v17, int v18, int v19, int v20, int v21, int v22, int v23, int v24, int v25, int v26, int v27, int v28, int v29, int v30, int v31, int v32, int v33, int v34, int v35, int v36, int v37, int v38, int v39, int v40, int v41, int v42, int v43, int v44, int v45, int v46, int v47, int v48, int v49, int v50, int v51, DateTime Date)
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
                    command.Parameters.Add(new SqlParameter("@uuid", System.Data.SqlDbType.NVarChar, 50, "uuid")).Value = uuid;

                    command.Parameters.Add(new SqlParameter("@v1", System.Data.SqlDbType.BigInt, 8, "BS1")).Value = v1;
                    command.Parameters.Add(new SqlParameter("@v2", System.Data.SqlDbType.BigInt, 8, "BS2")).Value = v2;
                    command.Parameters.Add(new SqlParameter("@v3", System.Data.SqlDbType.BigInt, 8, "BS3")).Value = v3;
                    command.Parameters.Add(new SqlParameter("@v4", System.Data.SqlDbType.BigInt, 8, "BS4")).Value = v4;
                    command.Parameters.Add(new SqlParameter("@v5", System.Data.SqlDbType.BigInt, 8, "BS5")).Value = v5;
                    command.Parameters.Add(new SqlParameter("@v6", System.Data.SqlDbType.BigInt, 8, "BS6")).Value = v6;
                    command.Parameters.Add(new SqlParameter("@v7", System.Data.SqlDbType.BigInt, 8, "BS7")).Value = v7;
                    command.Parameters.Add(new SqlParameter("@v8", System.Data.SqlDbType.BigInt, 8, "BS8")).Value = v8;
                    command.Parameters.Add(new SqlParameter("@v9", System.Data.SqlDbType.BigInt, 8, "BS9")).Value = v9;
                    command.Parameters.Add(new SqlParameter("@v10", System.Data.SqlDbType.BigInt, 8, "BS10")).Value = v10;
                    command.Parameters.Add(new SqlParameter("@v11", System.Data.SqlDbType.BigInt, 8, "BS11")).Value = v11;
                    command.Parameters.Add(new SqlParameter("@v12", System.Data.SqlDbType.BigInt, 8, "BS12")).Value = v12;
                    command.Parameters.Add(new SqlParameter("@v13", System.Data.SqlDbType.BigInt, 8, "BS13")).Value = v13;
                    command.Parameters.Add(new SqlParameter("@v14", System.Data.SqlDbType.BigInt, 8, "BS14")).Value = v14;
                    command.Parameters.Add(new SqlParameter("@v15", System.Data.SqlDbType.BigInt, 8, "BS15")).Value = v15;
                    command.Parameters.Add(new SqlParameter("@v16", System.Data.SqlDbType.BigInt, 8, "BS16")).Value = v16;
                    command.Parameters.Add(new SqlParameter("@v17", System.Data.SqlDbType.BigInt, 8, "BS17")).Value = v17;
                    command.Parameters.Add(new SqlParameter("@v18", System.Data.SqlDbType.BigInt, 8, "BS18")).Value = v18;
                    command.Parameters.Add(new SqlParameter("@v19", System.Data.SqlDbType.BigInt, 8, "BS19")).Value = v19;
                    command.Parameters.Add(new SqlParameter("@v20", System.Data.SqlDbType.BigInt, 8, "BS20")).Value = v20;
                    command.Parameters.Add(new SqlParameter("@v21", System.Data.SqlDbType.BigInt, 8, "BS21")).Value = v21;
                    command.Parameters.Add(new SqlParameter("@v22", System.Data.SqlDbType.BigInt, 8, "BS22")).Value = v22;
                    command.Parameters.Add(new SqlParameter("@v23", System.Data.SqlDbType.BigInt, 8, "BS23")).Value = v23;
                    command.Parameters.Add(new SqlParameter("@v24", System.Data.SqlDbType.BigInt, 8, "BS24")).Value = v24;
                    command.Parameters.Add(new SqlParameter("@v25", System.Data.SqlDbType.BigInt, 8, "BS25")).Value = v25;
                    command.Parameters.Add(new SqlParameter("@v26", System.Data.SqlDbType.BigInt, 8, "BS26")).Value = v26;
                    command.Parameters.Add(new SqlParameter("@v27", System.Data.SqlDbType.BigInt, 8, "BS27")).Value = v27;
                    command.Parameters.Add(new SqlParameter("@v28", System.Data.SqlDbType.BigInt, 8, "BS28")).Value = v28;
                    command.Parameters.Add(new SqlParameter("@v29", System.Data.SqlDbType.BigInt, 8, "BS29")).Value = v29;
                    command.Parameters.Add(new SqlParameter("@v30", System.Data.SqlDbType.BigInt, 8, "BS30")).Value = v30;
                    command.Parameters.Add(new SqlParameter("@v31", System.Data.SqlDbType.BigInt, 8, "BS31")).Value = v31;
                    command.Parameters.Add(new SqlParameter("@v32", System.Data.SqlDbType.BigInt, 8, "BS32")).Value = v32;
                    command.Parameters.Add(new SqlParameter("@v33", System.Data.SqlDbType.BigInt, 8, "BS33")).Value = v33;
                    command.Parameters.Add(new SqlParameter("@v34", System.Data.SqlDbType.BigInt, 8, "BS34")).Value = v34;
                    command.Parameters.Add(new SqlParameter("@v35", System.Data.SqlDbType.BigInt, 8, "BS35")).Value = v35;
                    command.Parameters.Add(new SqlParameter("@v36", System.Data.SqlDbType.BigInt, 8, "BS36")).Value = v36;
                    command.Parameters.Add(new SqlParameter("@v37", System.Data.SqlDbType.BigInt, 8, "BS37")).Value = v37;
                    command.Parameters.Add(new SqlParameter("@v38", System.Data.SqlDbType.BigInt, 8, "BS38")).Value = v38;
                    command.Parameters.Add(new SqlParameter("@v39", System.Data.SqlDbType.BigInt, 8, "BS39")).Value = v39;
                    command.Parameters.Add(new SqlParameter("@v40", System.Data.SqlDbType.BigInt, 8, "BS40")).Value = v40;
                    command.Parameters.Add(new SqlParameter("@v41", System.Data.SqlDbType.BigInt, 8, "BS41")).Value = v41;
                    command.Parameters.Add(new SqlParameter("@v42", System.Data.SqlDbType.BigInt, 8, "BS42")).Value = v42;
                    command.Parameters.Add(new SqlParameter("@v43", System.Data.SqlDbType.BigInt, 8, "BS43")).Value = v43;
                    command.Parameters.Add(new SqlParameter("@v44", System.Data.SqlDbType.BigInt, 8, "BS44")).Value = v44;
                    command.Parameters.Add(new SqlParameter("@v45", System.Data.SqlDbType.BigInt, 8, "BS45")).Value = v45;
                    command.Parameters.Add(new SqlParameter("@v46", System.Data.SqlDbType.BigInt, 8, "BS46")).Value = v46;
                    command.Parameters.Add(new SqlParameter("@v47", System.Data.SqlDbType.BigInt, 8, "BS47")).Value = v47;
                    command.Parameters.Add(new SqlParameter("@v48", System.Data.SqlDbType.BigInt, 8, "BS48")).Value = v48;
                    command.Parameters.Add(new SqlParameter("@v49", System.Data.SqlDbType.BigInt, 8, "BS49")).Value = v49;
                    command.Parameters.Add(new SqlParameter("@v50", System.Data.SqlDbType.BigInt, 8, "BS50")).Value = v50;
                    command.Parameters.Add(new SqlParameter("@v51", System.Data.SqlDbType.BigInt, 8, "BS51")).Value = v51;
                    command.Parameters.Add(new SqlParameter("@Date", System.Data.SqlDbType.Date, 3, "Date")).Value = Date;
                    command.Parameters.Add(new SqlParameter("@UpDateTime", System.Data.SqlDbType.DateTime, 8, "UpDateTime")).Value = DateTime.Now;


                    //--------------------------            
                    //BS1-11　流動資産
                    int ryuudoushisan = v1 + v2 + v3 + v4 + v5 + v6 + v7 + v8 + v9 + v10 + v11;
                    command.Parameters.Add(new SqlParameter("@a1", System.Data.SqlDbType.BigInt, 8, "ryuudoushisan")).Value = ryuudoushisan;
                    //--------------------------            
                    //BS12-18　有形固定資産
                    int yuukeikoteishisan = v12 + v13 + v14 + v15 + v16 + v17 + v18;
                    command.Parameters.Add(new SqlParameter("@a2", System.Data.SqlDbType.BigInt, 8, "yuukeikoteishisan")).Value = yuukeikoteishisan;
                    //BS19-21　無形固定資産
                    int mukeikoteishisan = v19 + v20 + v21;
                    command.Parameters.Add(new SqlParameter("@a3", System.Data.SqlDbType.BigInt, 8, "mukeikoteishisan")).Value = mukeikoteishisan;
                    //BS22-28　投資その他の資産
                    int toushisonotanoshisan = v22 + v23 + v24 + v25 + v26 + v27 + v28;
                    command.Parameters.Add(new SqlParameter("@a4", System.Data.SqlDbType.BigInt, 8, "touhshisonotanoshisan")).Value = toushisonotanoshisan;
                    //BS12-28　固定資産(SUM)
                    int koteishisan = yuukeikoteishisan + mukeikoteishisan + toushisonotanoshisan;
                    command.Parameters.Add(new SqlParameter("@a5", System.Data.SqlDbType.BigInt, 8, "koteishisan")).Value = koteishisan;
                    //--------------------------            
                    //BS1-29　資産合計
                    int shisan_a = ryuudoushisan + koteishisan;
                    command.Parameters.Add(new SqlParameter("@ta1", System.Data.SqlDbType.BigInt, 8, "shisan_a")).Value = shisan_a;
                    //--------------------------
                    //BS29-37　流動負債
                    int ryuudoufusai = v29 + v30 + v31 + v32 + v33 + v34 + v35 + v36 + v37;
                    command.Parameters.Add(new SqlParameter("@a6", System.Data.SqlDbType.BigInt, 8, "ryuudoufusai")).Value = ryuudoufusai;
                    //BS38-40　固定負債
                    int koteifusai = v38 + v39 + v40;
                    command.Parameters.Add(new SqlParameter("@a7", System.Data.SqlDbType.BigInt, 8, "koteifusai")).Value = koteifusai;
                    //--------------------------
                    //BS29-40　負債合計
                    int fusai_a = ryuudoufusai + koteifusai;
                    command.Parameters.Add(new SqlParameter("@ta2", System.Data.SqlDbType.BigInt, 8, "fusai_a")).Value = fusai_a;
                    //--------------------------
                    //BS41-51　純資産
                    int jyunshisan_a = v41 + v42 + v43 + v44 + v45 + v46 + v47 + v48 + v49 + v50 + v51;
                    command.Parameters.Add(new SqlParameter("@ta3", System.Data.SqlDbType.BigInt, 8, "jyunshisan_a")).Value = jyunshisan_a;
                    //--------------------------


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "UPDATE [T_BS] SET [uuid]=@uuid, [BS1]=@v1, [BS2]=@v2, [BS3]=@v3, [BS4]=@v4, [BS5]=@v5, [BS6]=@v6, [BS7]=@v7, [BS8]=@v8, [BS9]=@v9, [BS10]=@v10, [BS11]=@v11, [BS12]=@v12, [BS13]=@v13, [BS14]=@v14, [BS15]=@v15, [BS16]=@v16, [BS17]=@v17, [BS18]=@v18, [BS19]=@v19, [BS20]=@v20, [BS21]=@v21, [BS22]=@v22, [BS23]=@v23, [BS24]=@v24, [BS25]=@v25, [BS26]=@v26, [BS27]=@v27, [BS28]=@v28, [BS29]=@v29, [BS30]=@v30, [BS31]=@v31, [BS32]=@v32, [BS33]=@v33, [BS34]=@v34, [BS35]=@v35, [BS36]=@v36, [BS37]=@v37, [BS38]=@v38, [BS39]=@v39, [BS40]=@v40, [BS41]=@v41, [BS42]=@v41, [BS43]=@v43, [BS44]=@v44, [BS45]=@v45, [BS46]=@v46, [BS47]=@v47, [BS48]=@v48, [BS49]=@v49, [BS50]=@v50, [BS51]=@v51, [ryuudoushisan]=@a1, [yuukeikoteishisan]=@a2, [mukeikoteishisan]=@a3, [toushisonotanoshisan]=@a4, [koteishisan]=@a5, [ryuudoufusai]=@a6, [koteifusai]=@a7, [shisan_a]=@ta1, [fusai_a]=@ta2, [jyunshisan_a]=@ta3, [Date]=CONVERT(Date, @Date), [UpDateTime]=CONVERT(DateTime, @UpDateTime) WHERE [uuid]=LTRIM(RTRIM(@uuid))";

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
        //T_CF
        //------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// T_CFのDataTableを返します。
        /// 引数はDATASET.DataSet.T_CFRow型で参照して下さい。
        /// 中身がない場合や入力値が不正な場合はnullを返します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="uuid"></param>
        /// <returns>DATASET.DataSet.T_CFRow</returns>
        public static DATASET.DataSet.T_CFRow GetT_CFRow(SqlConnection sqlConnection, string uuid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得（必要のないパラメータを設定するとコンパイルエラーする）
            da.SelectCommand.Parameters.Add(new SqlParameter("@uuid", System.Data.SqlDbType.NVarChar, 50, "uuid")).Value = uuid;

            //idとuidからNULL以外の列数を取得します。
            da.SelectCommand.CommandText =
                "SELECT * FROM [T_CF] WHERE [uuid] = LTRIM(RTRIM(@uuid))";

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_CFDataTable dt = new DATASET.DataSet.T_CFDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                if (dt.Count >= 1)
                {
                    //中身あり
                    return dt[0];  //dt[0]の中にflag_del_popなどが入っています。

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
        /// T_BSから指定したuuidのRowをDeleteします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="uuid"></param>
        public static void DeleteT_CFRow(SqlConnection sqlConnection, string uuid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得（必要のないパラメータを設定するとコンパイルエラーする）
            da.SelectCommand.Parameters.Add(new SqlParameter("@uuid", System.Data.SqlDbType.NVarChar, 50, "uuid")).Value = uuid;

            //idとuidからNULL以外の列数を取得します。
            da.SelectCommand.CommandText =
                "DELETE FROM [T_CF] WHERE [uuid] = LTRIM(RTRIM(@uuid))";

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_CFDataTable dt = new DATASET.DataSet.T_CFDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);
                return;
            }
            catch
            {
                //不正な値が入力された場合はnullを返します。
                return;
            }

        }


        /// <summary>
        /// C/Fテーブルにインサートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="v4"></param>
        /// <param name="v5"></param>
        /// <param name="v6"></param>
        /// <param name="v7"></param>
        /// <param name="v8"></param>
        /// <param name="v9"></param>
        /// <param name="v10"></param>
        /// <param name="v11"></param>
        /// <param name="v12"></param>
        /// <param name="v13"></param>
        /// <param name="v14"></param>
        /// <param name="v15"></param>
        /// <param name="Date"></param>
        public static void SetT_CFInsert(SqlConnection sqlConnection, int v1, int v2, int v3, int v4, int v5, int v6, int v7, int v8, int v9, int v10, int v11, int v12, int v13, int v14, int v15, DateTime Date)
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
                    command.Parameters.Add(new SqlParameter("@uuid", System.Data.SqlDbType.NVarChar, 50, "uuid")).Value = Guid.NewGuid().ToString();

                    command.Parameters.Add(new SqlParameter("@v1", System.Data.SqlDbType.BigInt, 8, "CF1")).Value = v1;
                    command.Parameters.Add(new SqlParameter("@v2", System.Data.SqlDbType.BigInt, 8, "CF2")).Value = v2;
                    command.Parameters.Add(new SqlParameter("@v3", System.Data.SqlDbType.BigInt, 8, "CF3")).Value = v3;
                    command.Parameters.Add(new SqlParameter("@v4", System.Data.SqlDbType.BigInt, 8, "CF4")).Value = v4;
                    command.Parameters.Add(new SqlParameter("@v5", System.Data.SqlDbType.BigInt, 8, "CF5")).Value = v5;
                    command.Parameters.Add(new SqlParameter("@v6", System.Data.SqlDbType.BigInt, 8, "CF6")).Value = v6;
                    command.Parameters.Add(new SqlParameter("@v7", System.Data.SqlDbType.BigInt, 8, "CF7")).Value = v7;
                    command.Parameters.Add(new SqlParameter("@v8", System.Data.SqlDbType.BigInt, 8, "CF8")).Value = v8;
                    command.Parameters.Add(new SqlParameter("@v9", System.Data.SqlDbType.BigInt, 8, "CF9")).Value = v9;
                    command.Parameters.Add(new SqlParameter("@v10", System.Data.SqlDbType.BigInt, 8, "CF10")).Value = v10;
                    command.Parameters.Add(new SqlParameter("@v11", System.Data.SqlDbType.BigInt, 8, "CF11")).Value = v11;
                    command.Parameters.Add(new SqlParameter("@v12", System.Data.SqlDbType.BigInt, 8, "CF12")).Value = v12;
                    command.Parameters.Add(new SqlParameter("@v13", System.Data.SqlDbType.BigInt, 8, "CF13")).Value = v13;

                    command.Parameters.Add(new SqlParameter("@v14", System.Data.SqlDbType.BigInt, 8, "CF14")).Value = v15;
                    command.Parameters.Add(new SqlParameter("@Date", System.Data.SqlDbType.Date, 3, "Date")).Value = Date;
                    command.Parameters.Add(new SqlParameter("@UpDateTime", System.Data.SqlDbType.DateTime, 8, "UpDateTime")).Value = DateTime.Now;

                    // C/F 内部計算
                    int cf1 = v1 + v2 - v3 - v4 + v5 - v6;
                    int cf2 = -v7 + v8 - v9 + v10;
                    int cf3 = v11 - v12;
                    int cf4 = cf1 + cf2 + cf3;
                    //int cf5 = v14;

                    //キャッシュ・フローマージン＝「営業活動によるキャッシュ・フロー」÷「売上高」
                    float ri_r;
                    if ((float)v15 != 0)
                    {
                        ri_r = (float)cf1 / (float)v15;
                    }
                    else
                    {
                        ri_r = 0f;
                    }

                    command.Parameters.Add(new SqlParameter("@a1", System.Data.SqlDbType.BigInt, 8, "ACL1")).Value = cf1;
                    command.Parameters.Add(new SqlParameter("@a2", System.Data.SqlDbType.BigInt, 8, "ACL2")).Value = cf2;
                    command.Parameters.Add(new SqlParameter("@a3", System.Data.SqlDbType.BigInt, 8, "ACL3")).Value = cf3;
                    command.Parameters.Add(new SqlParameter("@a4", System.Data.SqlDbType.BigInt, 8, "ACL4")).Value = cf4;
                    command.Parameters.Add(new SqlParameter("@a5", System.Data.SqlDbType.BigInt, 8, "ACL5")).Value = v14;
                    command.Parameters.Add(new SqlParameter("@a6", System.Data.SqlDbType.Float, 8, "ACL6")).Value = ri_r;


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO [T_CF]([uuid], [CF1], [CF2], [CF3], [CF4], [CF5], [CF6], [CF7], [CF8], [CF9], [CF10], [CF11], [CF12], [CF13], [CF14], [ACL1], [ACL2], [ACL3], [ACL4], [ACL5], [ACL6], [Date], [UpDateTime]) VALUES(LTRIM(RTRIM(@uuid)), @v1, @v2, @v3, @v4, @v5, @v6, @v7, @v8, @v9, @v10, @v11, @v12, @v13, @v14, @a1, @a2, @a3, @a4, @a5, @a6, CONVERT(Date, @Date), CONVERT(DateTime, @UpDateTime))";


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
        /// C/Fテーブルにuuidをキーにアップデートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="uuid"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="v4"></param>
        /// <param name="v5"></param>
        /// <param name="v6"></param>
        /// <param name="v7"></param>
        /// <param name="v8"></param>
        /// <param name="v9"></param>
        /// <param name="v10"></param>
        /// <param name="v11"></param>
        /// <param name="v12"></param>
        /// <param name="v13"></param>
        /// <param name="v14"></param>
        /// <param name="v15"></param>
        /// <param name="Date"></param>
        public static void SetT_CFUpdate(SqlConnection sqlConnection, string uuid, int v1, int v2, int v3, int v4, int v5, int v6, int v7, int v8, int v9, int v10, int v11, int v12, int v13, int v14, int v15, DateTime Date)
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
                    command.Parameters.Add(new SqlParameter("@uuid", System.Data.SqlDbType.NVarChar, 50, "uuid")).Value = uuid;

                    command.Parameters.Add(new SqlParameter("@v1", System.Data.SqlDbType.BigInt, 8, "CF1")).Value = v1;
                    command.Parameters.Add(new SqlParameter("@v2", System.Data.SqlDbType.BigInt, 8, "CF2")).Value = v2;
                    command.Parameters.Add(new SqlParameter("@v3", System.Data.SqlDbType.BigInt, 8, "CF3")).Value = v3;
                    command.Parameters.Add(new SqlParameter("@v4", System.Data.SqlDbType.BigInt, 8, "CF4")).Value = v4;
                    command.Parameters.Add(new SqlParameter("@v5", System.Data.SqlDbType.BigInt, 8, "CF5")).Value = v5;
                    command.Parameters.Add(new SqlParameter("@v6", System.Data.SqlDbType.BigInt, 8, "CF6")).Value = v6;
                    command.Parameters.Add(new SqlParameter("@v7", System.Data.SqlDbType.BigInt, 8, "CF7")).Value = v7;
                    command.Parameters.Add(new SqlParameter("@v8", System.Data.SqlDbType.BigInt, 8, "CF8")).Value = v8;
                    command.Parameters.Add(new SqlParameter("@v9", System.Data.SqlDbType.BigInt, 8, "CF9")).Value = v9;
                    command.Parameters.Add(new SqlParameter("@v10", System.Data.SqlDbType.BigInt, 8, "CF10")).Value = v10;
                    command.Parameters.Add(new SqlParameter("@v11", System.Data.SqlDbType.BigInt, 8, "CF11")).Value = v11;
                    command.Parameters.Add(new SqlParameter("@v12", System.Data.SqlDbType.BigInt, 8, "CF12")).Value = v12;
                    command.Parameters.Add(new SqlParameter("@v13", System.Data.SqlDbType.BigInt, 8, "CF13")).Value = v13;

                    command.Parameters.Add(new SqlParameter("@v14", System.Data.SqlDbType.BigInt, 8, "CF14")).Value = v15;
                    command.Parameters.Add(new SqlParameter("@Date", System.Data.SqlDbType.Date, 3, "Date")).Value = Date;
                    command.Parameters.Add(new SqlParameter("@UpDateTime", System.Data.SqlDbType.DateTime, 8, "UpDateTime")).Value = DateTime.Now;

                    // C/F 内部計算
                    int cf1 = v1 + v2 - v3 - v4 + v5 - v6;
                    int cf2 = -v7 + v8 - v9 + v10;
                    int cf3 = v11 - v12;
                    int cf4 = cf1 + cf2 + cf3;
                    //int cf5 = v14;

                    //キャッシュ・フローマージン＝「営業活動によるキャッシュ・フロー」÷「売上高」
                    float ri_r;
                    if ((float)v15 != 0)
                    {
                        ri_r = (float)cf1 / (float)v15;
                    }
                    else
                    {
                        ri_r = 0f;
                    }

                    command.Parameters.Add(new SqlParameter("@a1", System.Data.SqlDbType.BigInt, 8, "ACL1")).Value = cf1;
                    command.Parameters.Add(new SqlParameter("@a2", System.Data.SqlDbType.BigInt, 8, "ACL2")).Value = cf2;
                    command.Parameters.Add(new SqlParameter("@a3", System.Data.SqlDbType.BigInt, 8, "ACL3")).Value = cf3;
                    command.Parameters.Add(new SqlParameter("@a4", System.Data.SqlDbType.BigInt, 8, "ACL4")).Value = cf4;
                    command.Parameters.Add(new SqlParameter("@a5", System.Data.SqlDbType.BigInt, 8, "ACL5")).Value = v14;
                    command.Parameters.Add(new SqlParameter("@a6", System.Data.SqlDbType.Float, 8, "ACL6")).Value = ri_r;


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "UPDATE [T_CF] SET [uuid]=@uuid, [CF1]=@v1, [CF2]=@v2, [CF3]=@v3, [CF4]=@v4, [CF5]=@v5, [CF6]=@v6, [CF7]=@v7, [CF8]=@v8, [CF9]=@v9, [CF10]=@v10, [CF11]=@v11, [CF12]=@v12, [CF13]=@v13, [CF14]=@v14, [CF15]=@v15, [ACF1]=@a1, [ACF2]=@a2, [ACF3]=@a3, [ACF4]=@a4, [ACF5]=@a5, [ACF6]=@a6, [Date]=CONVERT(Date, @Date), [UpDateTime]=CONVERT(DateTime, @UpDateTime) WHERE [uuid]=LTRIM(RTRIM(@uuid))";

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