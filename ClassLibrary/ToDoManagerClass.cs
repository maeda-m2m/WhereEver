using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using static System.Web.HttpUtility;

namespace WhereEver.ClassLibrary
{
    public static class ToDoManagerClass
    {


        /// <summary>
        /// T_PdbDataTableを参照します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <returns></returns>
        public static DATASET.DataSet.T_PdbDataTable GetT_Pdb_DataTable(SqlConnection sqlConnection, int Pid = -1)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);


            if (Pid < 0)
            {
                //パラメータを取得（必要のないパラメータを設定するとコンパイルエラーする）
                da.SelectCommand.Parameters.Add(new SqlParameter("@name1", System.Data.SqlDbType.NVarChar, 50, "Presponsible")).Value = SessionManager.User.M_User.name1.Trim();
                da.SelectCommand.CommandText =
                    "SELECT * FROM [T_Pdb] WHERE LTRIM(RTRIM(@name1)) LIKE '%' + [Presponsible] + '%' AND  CONVERT(DateTime,[Povertime]) >= CONVERT(DateTime,SYSDATETIME ()) OR  LTRIM(RTRIM(@name1)) LIKE '%' + [Presponsible] + '%' AND CONVERT(DateTime,[Povertime]) >= CONVERT(DateTime,SYSDATETIME ()) AND CONVERT(DateTime,[Povertime])= CONVERT(DateTime,'2100/01/01') ORDER BY [Pid] ASC";
            }
            else
            {
                //パラメータを取得（必要のないパラメータを設定するとコンパイルエラーする）
                da.SelectCommand.Parameters.Add(new SqlParameter("@Pid", System.Data.SqlDbType.Int, 4, "Pid")).Value = Pid;
                da.SelectCommand.CommandText =
                    "SELECT * FROM [T_Pdb] WHERE [Pid] = @Pid";
            }

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_PdbDataTable dt = new DATASET.DataSet.T_PdbDataTable();


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
                return null;
            }

        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// T_PdbDataRowを１件削除します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <returns></returns>
        public static bool DeleteT_Pdb_DataRow(SqlConnection sqlConnection, int Pid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得（必要のないパラメータを設定するとコンパイルエラーする）
            da.SelectCommand.Parameters.Add(new SqlParameter("@Pid", System.Data.SqlDbType.Int, 4, "Pid")).Value = Pid;

            da.SelectCommand.CommandText =
                "DELETE TOP(1) FROM [T_Pdb] WHERE [Pid]=@Pid";

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_PdbDataTable dt = new DATASET.DataSet.T_PdbDataTable();

            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                //成功（削除対象がない場合もtrue）
                return true;

            }
            catch
            {
                //失敗
                return false;
            }

        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// PidをキーにT_PdbをUpdateします。
        /// p1=(int)Pid
        /// p2=(bool)b false：承認、p2=true：完成。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="Pid"></param>
        /// <param name="b">承認ならfalse、完成ならtrue。</param>
        public static void SetT_Pdb_Update(SqlConnection sqlConnection, int Pid, bool b)
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
                    command.Parameters.Add(new SqlParameter("@Pid", System.Data.SqlDbType.Int, 4, "Pid")).Value = Pid;

                    if (b)
                    {
                        //true 完成 
                        //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                        command.CommandText = "UPDATE [T_Pdb] SET [Povertime] = CONVERT(DateTime,SYSDATETIME ()) WHERE([Pid] = @Pid)";
                    }
                    else
                    {
                        //false 承認
                        //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                        command.CommandText = "UPDATE [T_Pdb] SET [Pstarttime] = CONVERT(DateTime,SYSDATETIME ()) WHERE([Pid] = @Pid)";
                    }

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