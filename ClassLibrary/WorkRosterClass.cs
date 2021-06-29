using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.IO;

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

        //------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// uuidをもとにDATASET.DataSet.T_WorkRosterRowを削除します。
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="sqlConnection"></param>
        /// <returns>bool</returns>
        public static bool DeleteT_WorkRosterRow(SqlConnection sqlConnection, string uid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            //UserIDからNULL以外の列数を取得します。例えばUserIDがguestひとつなら1を返します。
            //LIKE @name%は0個以上の文字（前にも後ろにもつけられます）、@name_は「１」文字、
            //[@name]は[]内に指定した任意の文字です。 = @nameなら完全一致です。
            da.SelectCommand.CommandText =
                "DELETE TOP(1) FROM [T_WorkRoster] WHERE uid = LTRIM(RTRIM(@uid))";

            //パラメータを取得
            da.SelectCommand.Parameters.AddWithValue("@uid", uid);

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_WorkRosterDataTable dt = new DATASET.DataSet.T_WorkRosterDataTable();


            try
            {
                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                return true;
            }
            catch
            {
                //例えばguestなどの不正な値が入力された場合はtrueを返します。
                return false;
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



        //-----------------------------------------------------------------------------------------------------------



        /// <summary>
        /// １つのファイルをDBのT_Thumbnailにアップロードするためのクラスです。
        /// p1 FileUploadなどから取得したPostedFileです。
        /// p2 一度に転送する容量です。
        /// return int 0:ファイル名が無効 -1:db入力失敗 1:成功
        /// </summary>
        /// <param name="posted">必須</param>
        /// <param name="userid">必須 uuid</param>
        /// <param name="separatesize">任意変更　初期設定は8000</param>
        /// <returns></returns>
        public static int Set_WorkThumbnail_UpLoad(HttpPostedFile posted, string userid, int separatesize = 8000)
        {

            //パスを排除したファイル名を取得
            string fileName = posted.FileName;

            //拡張子を取得
            string extension = Path.GetExtension(fileName);

            //ファイル内容を取得: HttpPostedFile posted;

            if (fileName != null && fileName != "")
            {

                //不正なファイル名にならないようuuidに置き換え
                fileName = Guid.NewGuid().ToString() + extension;

                //投稿者id（変更不可）
                //string userid = SessionManager.User.M_User.id; //M_User

                //HttpPostedFileクラス（System.Web名前空間）のInputStreamプロパティを介して、アップロード・ファイルをいったんbyte配列に格納しておく
                //byte配列に格納してしまえば、後は通常のテキストと同様の要領でデータベースに格納できる。

                //空のバイト配列を生成します。
                byte[] aryData = new Byte[posted.ContentLength];

                //ここでファイルをaryDataに読み込み
                //posted.InputStream.Read(aryData, 0, posted.ContentLength);
                posted.InputStream.Read(aryData, 0, aryData.Length);

                //AES暗号化 sortKeyはサムネイルと同じ
                aryData = FileShareClass.SetAESEncrypt(aryData, "thumbnail", "");

                //MIMEタイプを取得します。
                string type = posted.ContentType;
                //-----------------------------------
                //バイトを分割します。
                byte[] bsSource = aryData;
                //分割回数
                int cutcount = 0;

                //一度にロードするデータバイト長: int separatesize;

                //宣言と初期化
                int remain = bsSource.Length;
                int position = 0;
                int split = 0;

                //配列リストを宣言と初期化
                List<byte[]> list = new List<byte[]>();

                while (remain > 0)
                {
                    //定数byteごとに分割 xが真ならばaを返し、偽ならばbを返す。三項演算子だと、split = remain < separatesize ? remain : separatesize;
                    if (remain < separatesize)
                    {
                        split = remain;
                    }
                    else
                    {
                        split = separatesize;
                    }
                    remain -= split;
                    cutcount += 1;

                    //配列リストに分割した配列を加算　途中で送信失敗しても再開できる（ようにするための仮実装）
                    byte[] bs = new byte[split];
                    Array.Copy(bsSource, position, bs, 0, split);
                    list.Add(bs);

                    //切り出し位置をずらす
                    position += split;
                }
                //-----------------------------------

                //宣言と初期化　初回のみtrue Insert
                bool b = true;
                for (int i = 0; i < list.Count; i++)
                {
                    //データベースにファイルを保存します。
                    byte[] smallbyte = list[i];
                    if (!FileShareClass.SetT_ThumbnailInsert(Global.GetConnection(), userid, type, smallbyte, b))
                    {
                        return -1; //db save failed
                    }

                    //Updateに変更
                    b = false;
                }
                //-----------------------------------

                //データバインド
                //GridView1.DataBind();

                return 1;   //file saved

            }
            else
            {
                return 0;  //filename is invalid
            }

        }


    }
}