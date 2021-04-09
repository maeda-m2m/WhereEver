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
        //T_UserDB_v2
        //------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 申請ログをアップデートします。不要なデータにはnullではなく"なし"と入力してください。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="shinseiSyubetsu"></param>
        /// <param name="A_BuyItem"></param>
        /// <param name="A_BuyKind"></param>
        /// <param name="A_BuyHowMany"></param>
        /// <param name="A_BuyHowMach"></param>
        /// <param name="A_BuyPlace"></param>
        /// <param name="B_DiligenceClassification1"></param>
        /// <param name="B_DiligenceClassification2"></param>
        /// <param name="B_DiligenceDateA1"></param>
        /// <param name="B_DiligenceDateA2"></param>
        /// <param name="B_DiligenceDateB1"></param>
        /// <param name="B_DiligenceDateB2"></param>
        /// <param name="C_Tatekae_Result_Main"></param>
        /// <param name="C_Tatekae_TWaste"></param>
        /// <param name="C_Tatekae_PWaste"></param>
        /// <param name="C_Tatekae_Result1"></param>
        /// <param name="C_Tatekae_Result2"></param>
        /// <param name="C_Tatekae_Result3"></param>
        public static void SetT_ShinseiLogUpdate(SqlConnection sqlConnection, string id, string name1, string SinseiSyubetsu, string A_BuyItem, string A_BuyKind, string A_BuyHowMany, string A_BuyHowMach, string A_BuyPlace, string B_DiligenceClassification1, string B_DiligenceClassification2, string B_DiligenceDateA1, string B_DiligenceDateA2, string B_DiligenceDateB1, string B_DiligenceDateB2, string C_Tatekae_Result_Main, string C_Tatekae_TWaste, string C_Tatekae_PWaste, string C_Tatekae_Result1, string C_Tatekae_Result2, string C_Tatekae_Result3)
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
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NChar, 20, "id")).Value = id;
                    command.Parameters.Add(new SqlParameter("@name1", System.Data.SqlDbType.NVarChar, 50, "name1")).Value = name1;
                    command.Parameters.Add(new SqlParameter("@SinseiSyubetsu", System.Data.SqlDbType.NVarChar, -1, "SinseiSyubetsu")).Value = SinseiSyubetsu;
                    command.Parameters.Add(new SqlParameter("@LastUpdate", System.Data.SqlDbType.DateTime, 8, "LastUpdate")).Value = date;

                    command.Parameters.Add(new SqlParameter("@A_BuyItem", System.Data.SqlDbType.NVarChar, 20, "A_BuyItem")).Value = A_BuyItem;
                    command.Parameters.Add(new SqlParameter("@A_BuyKind", System.Data.SqlDbType.NVarChar, 20, "A_BuyKind")).Value = A_BuyKind;
                    command.Parameters.Add(new SqlParameter("@A_BuyHowMany", System.Data.SqlDbType.NVarChar, 20, "A_BuyHowMany")).Value = A_BuyHowMany;
                    command.Parameters.Add(new SqlParameter("@A_BuyHowMach", System.Data.SqlDbType.NVarChar, 20, "A_BuyHowMach")).Value = A_BuyHowMach;
                    command.Parameters.Add(new SqlParameter("@A_BuyPlace", System.Data.SqlDbType.NVarChar, 20, "A_BuyPlace")).Value = A_BuyPlace;

                    command.Parameters.Add(new SqlParameter("@B_DiligenceClassification1", System.Data.SqlDbType.NVarChar, 20, "B_DiligenceClassification1")).Value = B_DiligenceClassification1;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceClassification2", System.Data.SqlDbType.NVarChar, 20, "B_DiligenceClassification2")).Value = B_DiligenceClassification2;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceDateA1", System.Data.SqlDbType.NVarChar, 20, "B_DiligenceDateA1")).Value = B_DiligenceDateA1;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceDateA2", System.Data.SqlDbType.NVarChar, 20, "B_DiligenceDateA2")).Value = B_DiligenceDateA2;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceDateB1", System.Data.SqlDbType.NVarChar, 20, "B_DiligenceDateB1")).Value = B_DiligenceDateB1;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceDateB2", System.Data.SqlDbType.NVarChar, 20, "B_DiligenceDateB2")).Value = B_DiligenceDateB2;

                    command.Parameters.Add(new SqlParameter("@C_Tatekae_Result_Main", System.Data.SqlDbType.NVarChar, -1, "C_Tatekae_Result_Main")).Value = C_Tatekae_Result_Main;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_TWaste", System.Data.SqlDbType.NVarChar, 20, "C_Tatekae_TWaste")).Value = C_Tatekae_TWaste;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_PWaste", System.Data.SqlDbType.NVarChar, 20, "C_Tatekae_PWaste")).Value = C_Tatekae_PWaste;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_Result1", System.Data.SqlDbType.NVarChar, 20, "C_Tatekae_Result1")).Value = C_Tatekae_Result1;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_Result2", System.Data.SqlDbType.NVarChar, 20, "C_Tatekae_Result2")).Value = C_Tatekae_Result2;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_Result3", System.Data.SqlDbType.NVarChar, 20, "C_Tatekae_Result3")).Value = C_Tatekae_Result3;



                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "UPDATE [T_ShinseiLog] SET[LastUpdate] = @LastUpdate, [A_BuyItem]= @A_BuyItem, [A_BuyKind]= @A_BuyKind, [A_BuyHowMany]= @A_BuyHowMany, [A_BuyHowMach]= @A_BuyHowMach, [A_BuyPlace]= @A_BuyPlace, [B_DiligenceClassification1]= @B_DiligenceClassification1, [B_DiligenceClassification2]= @B_DiligenceClassification2, [B_DiligenceDateA1]= @B_DiligenceDateA1, [B_DiligenceDateA2]= @B_DiligenceDateA2, [B_DiligenceDateB1]= @B_DiligenceDateB1, [B_DiligenceDateB2]= @B_DiligenceDateB2, [C_Tatekae_Result_Main]= @C_Tatekae_Result_Main, [C_Tatekae_TWaste]= @C_Tatekae_TWaste, [C_Tatekae_PWaste]= @C_Tatekae_PWaste, [C_Tatekae_Result1]= @C_Tatekae_Result1, [C_Tatekae_Result2]= @C_Tatekae_Result2, [C_Tatekae_Result3]= @C_Tatekae_Result3 WHERE([id] = @id, [DateTime] = @DateTime)";

                 /*
                 UpdateCommand="UPDATE [T_ShinseiLog] 
                 SET [LastUpdate]=@LastUpdate, 
                 [A_BuyItem]=@A_BuyItem, 
                 [A_BuyKind]=@A_BuyKind, 
                 [A_BuyHowMany]=@A_BuyHowMany, 
                 [A_BuyHowMach]=@A_BuyHowMach, 
                 [A_BuyPlace]=@A_BuyPlace, 
                 [B_DiligenceClassification1]=@B_DiligenceClassification1, 
                 [B_DiligenceClassification2]=@B_DiligenceClassification2, 
                 [B_DiligenceDateA1]=@B_DiligenceDateA1, 
                 [B_DiligenceDateA2]=@B_DiligenceDateA2, 
                 [B_DiligenceDateB1]=@B_DiligenceDateB1, 
                 [B_DiligenceDateB2]=@B_DiligenceDateB2, 
                 [C_Tatekae_Result_Main]=@C_Tatekae_Result_Main, 
                 [C_Tatekae_TWaste]=@C_Tatekae_TWaste, 
                 [C_Tatekae_PWaste]=@C_Tatekae_PWaste, 
                 [C_Tatekae_Result1]=@C_Tatekae_Result1, 
                 [C_Tatekae_Result2]=@C_Tatekae_Result2, 
                 [C_Tatekae_Result3]=@C_Tatekae_Result3 
                 WHERE ([id] = @id, [DateTime] = @DateTime)">
                 */


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

            return;

        }

        /// <summary>
        /// 申請ログをアップデートします。不要なデータにはnullではなく"なし"と入力してください。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="shinseiSyubetsu"></param>
        /// <param name="A_BuyItem"></param>
        /// <param name="A_BuyKind"></param>
        /// <param name="A_BuyHowMany"></param>
        /// <param name="A_BuyHowMach"></param>
        /// <param name="A_BuyPlace"></param>
        /// <param name="B_DiligenceClassification1"></param>
        /// <param name="B_DiligenceClassification2"></param>
        /// <param name="B_DiligenceDateA1"></param>
        /// <param name="B_DiligenceDateA2"></param>
        /// <param name="B_DiligenceDateB1"></param>
        /// <param name="B_DiligenceDateB2"></param>
        /// <param name="C_Tatekae_Result_Main"></param>
        /// <param name="C_Tatekae_TWaste"></param>
        /// <param name="C_Tatekae_PWaste"></param>
        /// <param name="C_Tatekae_Result1"></param>
        /// <param name="C_Tatekae_Result2"></param>
        /// <param name="C_Tatekae_Result3"></param>
        public static void SetT_ShinseiLogInsert(SqlConnection sqlConnection, string id, string name1, string SinseiSyubetsu, string A_BuyItem, string A_BuyKind, string A_BuyHowMany, string A_BuyHowMach, string A_BuyPlace, string B_DiligenceClassification1, string B_DiligenceClassification2, string B_DiligenceDateA1, string B_DiligenceDateA2, string B_DiligenceDateB1, string B_DiligenceDateB2, string C_Tatekae_Result_Main, string C_Tatekae_TWaste, string C_Tatekae_PWaste, string C_Tatekae_Result1, string C_Tatekae_Result2, string C_Tatekae_Result3)
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

                DateTime LastUpdate = DateTime.Now; //DateTime取得

                try
                {

                    //Add the paramaters for the Updatecommand.必ずダブルクオーテーションで@変数の宣言を囲んでください。command.CommandTextで使用するものは、必ずすべて宣言してください。
                    //-------------------------------------------------------------------------------------------------------------------
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NChar, 20, "id")).Value = id;
                    command.Parameters.Add(new SqlParameter("@name1", System.Data.SqlDbType.NVarChar, 50, "name1")).Value = name1;
                    command.Parameters.Add(new SqlParameter("@SinseiSyubetsu", System.Data.SqlDbType.NVarChar, -1, "SinseiSyubetsu")).Value = SinseiSyubetsu;
                    command.Parameters.Add(new SqlParameter("@LastUpdate", System.Data.SqlDbType.DateTime, 8, "LastUpdate")).Value = LastUpdate;

                    command.Parameters.Add(new SqlParameter("@A_BuyItem", System.Data.SqlDbType.NVarChar, 20, "A_BuyItem")).Value = A_BuyItem;
                    command.Parameters.Add(new SqlParameter("@A_BuyKind", System.Data.SqlDbType.NVarChar, 20, "A_BuyKind")).Value = A_BuyKind;
                    command.Parameters.Add(new SqlParameter("@A_BuyHowMany", System.Data.SqlDbType.NVarChar, 20, "A_BuyHowMany")).Value = A_BuyHowMany;
                    command.Parameters.Add(new SqlParameter("@A_BuyHowMach", System.Data.SqlDbType.NVarChar, 20, "A_BuyHowMach")).Value = A_BuyHowMach;
                    command.Parameters.Add(new SqlParameter("@A_BuyPlace", System.Data.SqlDbType.NVarChar, 20, "A_BuyPlace")).Value = A_BuyPlace;

                    command.Parameters.Add(new SqlParameter("@B_DiligenceClassification1", System.Data.SqlDbType.NVarChar, 20, "B_DiligenceClassification1")).Value = B_DiligenceClassification1;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceClassification2", System.Data.SqlDbType.NVarChar, 20, "B_DiligenceClassification2")).Value = B_DiligenceClassification2;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceDateA1", System.Data.SqlDbType.NVarChar, 20, "B_DiligenceDateA1")).Value = B_DiligenceDateA1;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceDateA2", System.Data.SqlDbType.NVarChar, 20, "B_DiligenceDateA2")).Value = B_DiligenceDateA2;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceDateB1", System.Data.SqlDbType.NVarChar, 20, "B_DiligenceDateB1")).Value = B_DiligenceDateB1;
                    command.Parameters.Add(new SqlParameter("@B_DiligenceDateB2", System.Data.SqlDbType.NVarChar, 20, "B_DiligenceDateB2")).Value = B_DiligenceDateB2;

                    command.Parameters.Add(new SqlParameter("@C_Tatekae_Result_Main", System.Data.SqlDbType.NVarChar, -1, "C_Tatekae_Result_Main")).Value = C_Tatekae_Result_Main;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_TWaste", System.Data.SqlDbType.NVarChar, 20, "C_Tatekae_TWaste")).Value = C_Tatekae_TWaste;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_PWaste", System.Data.SqlDbType.NVarChar, 20, "C_Tatekae_PWaste")).Value = C_Tatekae_PWaste;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_Result1", System.Data.SqlDbType.NVarChar, 20, "C_Tatekae_Result1")).Value = C_Tatekae_Result1;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_Result2", System.Data.SqlDbType.NVarChar, 20, "C_Tatekae_Result2")).Value = C_Tatekae_Result2;
                    command.Parameters.Add(new SqlParameter("@C_Tatekae_Result3", System.Data.SqlDbType.NVarChar, 20, "C_Tatekae_Result3")).Value = C_Tatekae_Result3;


                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO T_ShinseiLog(id, name1, SinseiSyubetsu, LastUpdate, A_BuyItem, A_BuyKind, A_BuyHowMany, A_BuyHowMach, A_BuyPlace, B_DiligenceClassification1, B_DiligenceClassification2, B_DiligenceDateA1, B_DiligenceDateA2, B_DiligenceDateB1, B_DiligenceDateB2, C_Tatekae_Result_Main, C_Tatekae_TWaste, C_Tatekae_PWaste, C_Tatekae_Result1, C_Tatekae_Result2, C_Tatekae_Result3) VALUES(@id, @name1, @SinseiSyubetsu, @LastUpdate, @A_BuyItem, @A_BuyKind, @A_BuyHowMany, @A_BuyHowMach, @A_BuyPlace, @B_DiligenceClassification1, @B_DiligenceClassification2, @B_DiligenceDateA1, @B_DiligenceDateA2, @B_DiligenceDateB1, @B_DiligenceDateB2, @C_Tatekae_Result_Main, @C_Tatekae_TWaste, @C_Tatekae_PWaste, @C_Tatekae_Result1, @C_Tatekae_Result2, @C_Tatekae_Result3)";

                    //このメソッドでは、XmlCommandTypeプロパティおよびCommandTextプロパティを使用してSQL文またはコマンドを実行し、影響を受ける行数を戻します（必須）。
                    //ここでエラーが出る場合は、宣言やSql文が不正な場合があります。
                    command.ExecuteNonQuery();

                    //Attempt to commit the transaction.
                    da.InsertCommand = command;
                    transaction.Commit();


                    //Console.WriteLine("Insert Completed");
                }
                catch
                {
                    //catch文
                    //Console.WriteLine("Insert Failed");
                    transaction.Rollback();
                }

            } //sqlConnection.Close();
        }






    }
}