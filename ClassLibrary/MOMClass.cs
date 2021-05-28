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
                    int uriagesourieki = uriagedaka + uriagegenka;
                    command.Parameters.Add(new SqlParameter("@uriagesourieki", System.Data.SqlDbType.BigInt, 8, "uriagesourieki")).Value = uriagesourieki;

                    command.Parameters.Add(new SqlParameter("@hanbaikanrihi", System.Data.SqlDbType.BigInt, 8, "hanbaikanrihi")).Value = hanbaikanrihi;
                    command.Parameters.Add(new SqlParameter("@eigyourieki", System.Data.SqlDbType.BigInt, 8, "eigyourieki")).Value = eigyourieki;
                    command.Parameters.Add(new SqlParameter("@eigyougaihiyou", System.Data.SqlDbType.BigInt, 8, "eigyougaihiyou")).Value = eigyougaihiyou;
                    int keijyourieki = uriagesourieki + hanbaikanrihi + eigyourieki + eigyougaihiyou;
                    command.Parameters.Add(new SqlParameter("@keijyourieki", System.Data.SqlDbType.BigInt, 8, "keijyourieki")).Value = keijyourieki;

                    command.Parameters.Add(new SqlParameter("@tokubetsurieki", System.Data.SqlDbType.BigInt, 8, "tokubetsurieki")).Value = tokubetsurieki;
                    command.Parameters.Add(new SqlParameter("@tokubetsusonshitsu", System.Data.SqlDbType.BigInt, 8, "tokubetsusonshitsu")).Value = tokubetsusonshitsu;
                    int zeibikimaetoukijyunrieki = keijyourieki + tokubetsurieki + tokubetsusonshitsu;                 
                    command.Parameters.Add(new SqlParameter("@zeibikimaetoukijyunrieki", System.Data.SqlDbType.BigInt, 8, "zeibikimaetoukijyunrieki")).Value = zeibikimaetoukijyunrieki;

                    command.Parameters.Add(new SqlParameter("@houjinzeitou", System.Data.SqlDbType.BigInt, 8, "houjinzeitou")).Value = houjinzeitou;
                    int toukijyunrieki = zeibikimaetoukijyunrieki + houjinzeitou;
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
                    command.CommandText = "INSERT INTO T_PL(uuid, uriagedaka, uriagegenka, uriagesourieki, hanbaikanrihi, eigyourieki, eigyougaihiyou, keijyourieki, tokubetsurieki, tokubetsusonshitsu, zeibikimaetoukijyunrieki, houjinzeitou, toukijyunrieki, arari_r, eigyou_r, keijyou_r, Date_S, Date_G, UpDateTime) VALUES(LTRIM(RTRIM(@uuid)), @uriagedaka, @uriagegenka, @uriagesourieki, @hanbaikanrihi, @eigyourieki, @eigyougaihiyou, @keijyourieki, @tokubetsurieki, @tokubetsusonshitsu, @zeibikimaetoukijyunrieki, @houjinzeitou, @toukijyunrieki, @arari_r, @eigyou_r, @keijyou_r, CONVERT(Date, @Date_S), CONVERT(Date, @Date_G), @UpDateTime)";


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
                    int uriagesourieki = uriagedaka + uriagegenka;
                    command.Parameters.Add(new SqlParameter("@uriagesourieki", System.Data.SqlDbType.BigInt, 8, "uriagesourieki")).Value = uriagesourieki;

                    command.Parameters.Add(new SqlParameter("@hanbaikanrihi", System.Data.SqlDbType.BigInt, 8, "hanbaikanrihi")).Value = hanbaikanrihi;
                    command.Parameters.Add(new SqlParameter("@eigyourieki", System.Data.SqlDbType.BigInt, 8, "eigyourieki")).Value = eigyourieki;
                    command.Parameters.Add(new SqlParameter("@eigyougaihiyou", System.Data.SqlDbType.BigInt, 8, "eigyougaihiyou")).Value = eigyougaihiyou;
                    int keijyourieki = uriagesourieki + hanbaikanrihi + eigyourieki + eigyougaihiyou;
                    command.Parameters.Add(new SqlParameter("@keijyourieki", System.Data.SqlDbType.BigInt, 8, "keijyourieki")).Value = keijyourieki;

                    command.Parameters.Add(new SqlParameter("@tokubetsurieki", System.Data.SqlDbType.BigInt, 8, "tokubetsurieki")).Value = tokubetsurieki;
                    command.Parameters.Add(new SqlParameter("@tokubetsusonshitsu", System.Data.SqlDbType.BigInt, 8, "tokubetsusonshitsu")).Value = tokubetsusonshitsu;
                    int zeibikimaetoukijyunrieki = keijyourieki + tokubetsurieki + tokubetsusonshitsu;
                    command.Parameters.Add(new SqlParameter("@zeibikimaetoukijyunrieki", System.Data.SqlDbType.BigInt, 8, "zeibikimaetoukijyunrieki")).Value = zeibikimaetoukijyunrieki;

                    command.Parameters.Add(new SqlParameter("@houjinzeitou", System.Data.SqlDbType.BigInt, 8, "houjinzeitou")).Value = houjinzeitou;
                    int toukijyunrieki = zeibikimaetoukijyunrieki + houjinzeitou;
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
                    command.CommandText = "UPDATE [T_PL] SET [uuid]=@uuid, [uriagedaka]=@uriagedaka, [uriagegenka]=@uriagegenka, [uriagesourieki]=@uriagesourieki, [hanbaikanrihi]=@hanbaikanrihi, [eigyourieki]=@eigyourieki, [eigyougaihiyou]=@eigyougaihiyou, [keijyourieki]=@keijyourieki, [tokubetsurieki]=@tokubetsurieki, [tokubetsusonshitsu]=@tokubetsusonshitsu, [zeibikimaetoukijyunrieki]=@zeibikimaetoukijyunrieki, [houjinzeitou]=@houjinzeitou, [toukijyunrieki]=@toukijyunrieki, [arari_r]=@arari_r, [eigyou_r]=@eigyou_r, [keijyou_r]=@keijyou_r, [Date_S]= CONVERT(Date, @Date_S), [Date_G]= CONVERT(Date, @Date_G), [UpDateTime]=@UpDateTime WHERE [uuid] = @uuid";


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