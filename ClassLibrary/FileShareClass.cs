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
    public class FileShareClass
    {

        //-----------------------------------------------------------------------------------------------------------



        /// <summary>
        /// １つのファイルをDBにアップロードするためのクラスです。
        /// p1 FileUploadなどから取得したPostedFileです。
        /// p2 fileのtitleです。ファイル名とは異なります。
        /// p3 一度に転送する容量です。
        /// p4 trueなら匿名になります。
        /// return int 0:ファイル名が無効 -1:db入力失敗 1:成功
        /// </summary>
        /// <param name="posted">必須</param>
        /// <param name="title">任意　なければ「無題」</param>
        /// <param name="pass">任意</param>
        /// <param name="separatesize">任意変更　初期設定は8000</param>
        /// <param name="annonimas">任意変更　初期設定は匿名(true)</param>
        /// <returns></returns>
        public static int Set_File_UpLoad(HttpPostedFile posted, string title="", string pass="", int separatesize=8000, bool annonimas=true)
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
                    string userid = SessionManager.User.M_User.id; //M_User

                    //投稿者名
                    string username = SessionManager.User.M_User.name1; //M_User
                    if (annonimas)
                    {
                        username = "Annonimas";
                    }

                    //タイトル
                    title = HtmlEncode(title);
                    if (title == "")
                    {
                        title = "無題";
                    }
                    //パスワード
                    if (pass != "")
                    {
                        pass = pass.GetHashCode().ToString();
                    }

                    //HttpPostedFileクラス（System.Web名前空間）のInputStreamプロパティを介して、アップロード・ファイルをいったんbyte配列に格納しておく
                    //byte配列に格納してしまえば、後は通常のテキストと同様の要領でデータベースに格納できる。

                    //バイトを取得します。
                    byte[] aryData = new Byte[posted.ContentLength];
                    posted.InputStream.Read(aryData, 0, posted.ContentLength);

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
                        if (!FileShareClass.SetT_FileShareInsert(Global.GetConnection(), userid, username, fileName, title, pass, type, smallbyte, b))
                        {
                            return -1; //db save filed
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



        //-----------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 1つのファイルをDBからロードするためのクラスです。
        /// p1 必須　ロードページのPage.Response
        /// p2 必須　ファイル名　uuid.xxx
        /// p3 任意変更　ファイルのパスワード
        /// p4 任意変更　一度の転送容量
        /// return bool true:成功（Flushされるため取得不可）　false:失敗
        /// </summary>
        /// <param name="response">必須　Page.Response</param>
        /// <param name="file">必須</param>
        /// <param name="pass">あれば</param>
        /// <param name="separatesize">任意変更　初期設定は8000</param>
        /// <returns>bool</returns>
        public static bool Get_File_DownLoad(HttpResponse response, string file, string pass = "", int separatesize = 8000)
        {
            //Password
            if (pass != "")
            {
                pass = pass.GetHashCode().ToString();
            }

            if (file != null && file != "")
            {

                //一度にロードするデータバイト長: int separatesize;
                //初期化
                byte[] allbyte = new Byte[0];
                string type = "";

                //無限ループ
                for (int i = 0; i < int.MaxValue; i++)
                {
                    int startindex = 1 + (separatesize * i); //1からはじまる長さ
                    DATASET.DataSet.T_FileShareRow dr = FileShareClass.GetT_FileShareRow(Global.GetConnection(), file, pass, startindex, separatesize);
                    if (dr != null)
                    {
                        if (i == 0)
                        {
                            //初回はタイプ取得
                            type = @dr.type;
                        }

                        //取得した一部データの残りの長さを取得
                        if (dr.datum.Length <= 0)
                        {
                            //終了
                            break;
                        }

                        //配列allbyteの末尾にコピー
                        allbyte = allbyte.Concat(dr.datum).ToArray();   //LINQ .NET Framework 3.5以上
                    }
                    else
                    {
                        //終了
                        break;
                    }
                }

                if (allbyte.Length > 0)
                {

                    // HTTPレスポンスのヘッダ＆エンティティのクリア（初期化）
                    response.Clear();

                    // ダウンロード用ファイルの種別とデフォルトの名前を指定
                    response.ContentType = @"application/octet-stream"; //デフォルト設定: @"application/octet-stream"
                    response.AppendHeader("Content-Disposition", "attachment; filename=" + file);

                    // MIME Typeを取得
                    response.ContentType = @type;

                    // ダウンロード実行 binary HTMLページと一緒にロードされる
                    response.BinaryWrite((Byte[])allbyte);

                    response.Flush();
                    response.SuppressContent = true;    //必ずResponse.Flush();の後！              
                    return true;    //実際はFlushされているためこの値は返らない。
                }
                else
                {
                    //指定したファイルが存在しません。あるいは、パスワードが誤っています。
                    return false;
                }

            }
            else
            {
                //"ダウンロードしたいファイル名を入力して下さい。
                return false;
            }

        }


        //-----------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 1つのファイルをDBからロードし、img srcにするためのクラスです。
        /// p1 必須　ロードページのPage.Response
        /// p2 必須　ファイル名　uuid.xxx
        /// p3 任意変更　ファイルのパスワード
        /// p4 任意変更　一度の転送容量
        /// return string <img src=\"data:image/png;base64,.......\" />
        /// </summary>
        /// <param name="response">必須　Page.Response</param>
        /// <param name="file">必須</param>
        /// <param name="pass">あれば</param>
        /// <param name="separatesize">任意変更　初期設定は8000</param>
        /// <returns>string</returns>
        public static string Get_File_DownLoad_src(HttpResponse response, string file, string pass = "", int separatesize = 8000)
        {
            const int maxsize = 41943040;


            //Password
            if (pass != "")
            {
                pass = pass.GetHashCode().ToString();
            }

            if (file != null && file != "")
            {

                //一度にロードするデータバイト長: int separatesize;
                //初期化
                byte[] allbyte = new Byte[0];
                string type = "";

                //無限ループ
                for (int i = 0; i < int.MaxValue; i++)
                {
                    int startindex = 1 + (separatesize * i); //1からはじまる長さ
                    DATASET.DataSet.T_FileShareRow dr = FileShareClass.GetT_FileShareRow(Global.GetConnection(), file, pass, startindex, separatesize);
                    if (dr != null)
                    {
                        if (i == 0)
                        {
                            //初回はタイプ取得
                            type = @dr.type;

                            //ファイルサイズ取得
                            string size = @dr.size;
                            float value = 0f;
                            int n = 1;
                            StringBuilder sb = new StringBuilder();
                            sb.Append(size);
                            if (size.LastIndexOf("GB") != -1)
                            {
                                sb.Replace("GB", "");
                                n = 3;
                            }
                            else if (size.LastIndexOf("MB") != -1)
                            {
                                sb.Replace("MB", "");
                                n = 2;
                            }
                            else if (size.LastIndexOf("KB") != -1)
                            {
                                sb.Replace("KB", "");
                                n = 1;
                            }
                            else if (size.LastIndexOf("B") != -1)
                            {
                                sb.Replace("B", "");
                                n = 0;
                            }

                            if(!float.TryParse(sb.ToString().Trim(), System.Globalization.NumberStyles.Currency, null, out value))
                            {
                                return "Size_TryParse_Failed";
                            }

                            value = value * (float)(1024 ^n);

                            if(value > maxsize / 100)
                            {
                                return "Content_Too_Large_Size";
                            }


                            if (type != "image/jpeg" && type != "image/png" && type != "image/gif" && type != "image/svg+xml" && type != "application/pdf" && type != "text/plain")
                            {
                                return "Content_MIME_Type_Not_Supported";
                            }

                        }

                        //取得した一部データの残りの長さを取得
                        if (dr.datum.Length <= 0)
                        {
                            //終了
                            break;
                        }

                        //配列allbyteの末尾にコピー
                        allbyte = allbyte.Concat(dr.datum).ToArray();   //LINQ .NET Framework 3.5以上
                    }
                    else
                    {
                        //終了
                        break;
                    }
                }

                if (allbyte.Length > 0)
                {
                    if (type == "image/jpeg" || type == "image/png" || type == "image/gif" || type == "image/svg+xml")
                    {
                        //貼り付け用タグを返す           
                        return "<img src=\"data:" + @type + ";base64," + HtmlEncode(Convert.ToBase64String(allbyte)) + "\" />";
                    }
                    else if (type == "application/pdf")
                    {
                        //貼り付け用タグを返す           
                        return "<embed width=\"100%\" height=\"100%\" type=\"" + @type + "\" src=\"data:" + @type + "; base64," + HtmlEncode(Convert.ToBase64String(allbyte)) + "\" />";
                    }
                    else if (type == "text/plain")
                    {
                        //貼り付け用タグを返す           
                        //return Convert.ToBase64String(allbyte);
                        Encoding enc = Encoding.GetEncoding("UTF-8");
                        return HtmlEncode(enc.GetString(Convert.FromBase64String(Convert.ToBase64String(allbyte))));
                    }
                    return "Content_MIME_Type_Not_Supported";
                }
                else
                {
                    //指定したファイルが存在しません。あるいは、パスワードが誤っています。
                    return "Not_Found";
                }

            }
            else
            {
                //"ダウンロードしたいファイル名を入力して下さい。
                return "Not_Found";
            }

        }



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