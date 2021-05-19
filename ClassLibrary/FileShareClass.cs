﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WhereEver.ClassLibrary
{
    public class FileShareClass
    {


        /// <summary>
        /// FileShareテーブルにインサートします。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id">主キー１：Session変数に保存されているUserIDです。</param>
        /// <param name="filename">主キー２：uuidによって構成されたファイルネームです。拡張子まで含まれています。</param>
        /// <param name="filepath">ファイルパスです。</param>
        public static void SetT_FileShareInsert(SqlConnection sqlConnection, string id, string filename, string title, string pass, string type, byte[] datum)
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

                //現在のDateTimeを取得
                DateTime date = DateTime.Now; 

                //Must assign both transaction object and connection
                //to Command object for apending local transaction
                command.Connection = sqlConnection;
                command.Transaction = transaction;

                try
                {
                    //Add the paramaters for the Updatecommand.必ずダブルクオーテーションで@変数の宣言を囲んでください。command.CommandTextで使用するものは、必ずすべて宣言してください。
                    //-------------------------------------------------------------------------------------------------------------------
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar, 100, "id")).Value = id;
                    command.Parameters.Add(new SqlParameter("@FileName", System.Data.SqlDbType.NVarChar, 100, "FileName")).Value = filename;
                    command.Parameters.Add(new SqlParameter("@Title", System.Data.SqlDbType.NVarChar, 100, "Title")).Value = title;
                    command.Parameters.Add(new SqlParameter("@Password", System.Data.SqlDbType.NVarChar, 100, "Password")).Value = pass;
                    command.Parameters.Add(new SqlParameter("@type", System.Data.SqlDbType.NVarChar, 50, "type")).Value = type;
                    command.Parameters.Add(new SqlParameter("@datum", System.Data.SqlDbType.Binary, 200, "datum")).Value = datum;
                    command.Parameters.Add(new SqlParameter("@DateTime", System.Data.SqlDbType.DateTime, 8, "DateTime")).Value = date;

                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO T_FileShare(id, filename) VALUES(LTRIM(RTRIM(@id)), LTRIM(RTRIM(@FileName)), LTRIM(RTRIM(@Title)), LTRIM(RTRIM(@Password)), LTRIM(RTRIM(@type)), LTRIM(RTRIM(@datum)), LTRIM(RTRIM(@DateTime)))";


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
                }

            } //sqlConnection.Close();

            return;

        }


    }
}