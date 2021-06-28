using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WhereEver.ClassLibrary
{
    public class WorkRosterClass
    {


        //------------------------------------------------------------------------------------------------------------
        //UUID
        //------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// uuidをもとにDATASET.DataSet.T_WorkRosterDataTableを返します。
        /// 主にGridViewのGridRowCommandから参照して使います。
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="sqlConnection"></param>
        /// <returns>DATASET.DataSet.T_WorkRosterDataTable</returns>
        public static DATASET.DataSet.T_WorkRosterDataTable GetT_WorkRoster(SqlConnection sqlConnection, string uid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            //UserIDからNULL以外の列数を取得します。例えばUserIDがguestひとつなら1を返します。
            //LIKE @name%は0個以上の文字（前にも後ろにもつけられます）、@name_は「１」文字、
            //[@name]は[]内に指定した任意の文字です。 = @nameなら完全一致です。
            da.SelectCommand.CommandText =
                "SELECT * FROM [T_WorkRoster] WHERE uid = LTRIM(RTRIM(@uid))";

            //パラメータを取得
            da.SelectCommand.Parameters.AddWithValue("@uid", uid);

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_WorkRosterDataTable dt = new DATASET.DataSet.T_WorkRosterDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                if (dt.Count >= 1)
                {
                    //ダブりあり
                    return dt;

                }
                else
                {
                    //ダブりなし
                    return null;
                }

            }
            catch
            {
                //例えばguestなどの不正な値が入力された場合はtrueを返します。
                return null;
            }

        }




        //---------------------------------------------------------------------------------------------------------------------------



        /// <summary>
        /// WorkRoster（社員名簿）テーブルにインサートします。
        /// ※workPhoneNoはハイフンを含めた14文字以内
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="uid"></param>
        /// <param name="workCompanyName"></param>
        /// <param name="workThumbnail"></param>
        /// <param name="workUserName"></param>
        /// <param name="workPost"></param>
        /// <param name="workAssignment"></param>
        /// <param name="workBirthday"></param>
        /// <param name="workPhoneNo"></param>
        /// <param name="workMail"></param>
        /// <param name="isPublic"></param>
        public static void SetT_WorkRosterInsert(SqlConnection sqlConnection, string uid, string workCompanyName, byte[] workThumbnail, string workUserName, string workPost, string workAssignment, DateTime workBirthday, string workPhoneNo, string workMail, bool isPublic)
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
                    command.Parameters.Add(new SqlParameter("@editorId", System.Data.SqlDbType.NVarChar, 50, "editorId")).Value = SessionManager.User.M_User.id.Trim();
                    command.Parameters.Add(new SqlParameter("@uid", System.Data.SqlDbType.NVarChar, 50, "uid")).Value = uid.Trim();
                    command.Parameters.Add(new SqlParameter("@workCompanyName", System.Data.SqlDbType.NVarChar, 50, "workCompanyName")).Value = workCompanyName.Trim();
                    command.Parameters.Add(new SqlParameter("@workThumbnail", System.Data.SqlDbType.VarBinary, -1, "workThumbnail")).Value = workThumbnail;
                    command.Parameters.Add(new SqlParameter("@workUserName", System.Data.SqlDbType.NVarChar, 50, "workUserName")).Value = workUserName.Trim();
                    command.Parameters.Add(new SqlParameter("@workPost", System.Data.SqlDbType.NVarChar, 50, "workPost")).Value = workPost.Trim();
                    command.Parameters.Add(new SqlParameter("@workAssignment", System.Data.SqlDbType.NVarChar, 50, "workAssignment")).Value = workAssignment.Trim();
                    command.Parameters.Add(new SqlParameter("@workBirthday", System.Data.SqlDbType.Date, 3, "workBirthday")).Value = workBirthday.Date;
                    command.Parameters.Add(new SqlParameter("@workPhoneNo", System.Data.SqlDbType.NVarChar, 14, "workPhoneNo")).Value = workPhoneNo.Trim();
                    command.Parameters.Add(new SqlParameter("@workMail", System.Data.SqlDbType.NVarChar, 50, "workMail")).Value = workMail.Trim();
                    command.Parameters.Add(new SqlParameter("@isPublic", System.Data.SqlDbType.Bit, 1, "workMail")).Value = isPublic;

                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO [T_WorkRoster]([uid],[editorId], [workCompanyName], [workThumbnail], [workPost], [workUserName], [workAssignment], [workBirthday], [workPhoneNo], [workMail], [isPublic]) VALUES(LTRIM(RTRIM(@uid)), LTRIM(RTRIM(@editorId)), LTRIM(RTRIM(@workCompanyName)), CAST(LTRIM(RTRIM(@workThumbnail)) AS VarBinary(max)), LTRIM(RTRIM(@workPost)), LTRIM(RTRIM(@workUserName)), LTRIM(RTRIM(@workAssignment)), LTRIM(RTRIM(@workBirthday)), LTRIM(RTRIM(@workPhoneNo)), LTRIM(RTRIM(@workMail)), @isPublic)";

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