using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using WhereEver.ClassLibrary;
using static System.Web.HttpUtility;


namespace WhereEver
{
    public partial class FileShare : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            //[注意！]まだ十分なテストが完了していません！

            if (!IsPostBack)
            {
                Session.Add("args", (string)"null");
                lblid.Text = SessionManager.User.M_User.id;
                lbluid.Text = "null";
            }


        }

        protected void Button_UpLoad(object sender, EventArgs e)
        {
            //ラベル初期化
            lblResult.Text = "";

            //inputの場合はnameで参照する
            //HttpPostedFile posted = Request.Files["userfile"];

            if (FileUpload_userfile.HasFile)
            {

                /*
                //アップロードするファイル名を設定（@"C\:temp"だけなどは危険なためデバッグ時以外使用禁止）
                //書き込み先ディレクトリがない場合はエラーになります。
                string path = @"c:\\UploadedFiles\\";

                if (!Directory.Exists(path))
                {
                    //ディレクトリが存在しません。このままだとエラーになるため、ディレクトリを生成します。
                    Directory.CreateDirectory(path);
                }
                */

                //パスを排除したファイル名を取得
                string fileName = Path.GetFileName(FileUpload_userfile.FileName);
                //拡張子を取得
                string extension = Path.GetExtension(FileUpload_userfile.FileName);
                //ファイル内容を取得
                HttpPostedFile posted = FileUpload_userfile.PostedFile;

                if (fileName != null && fileName != "")
                {

                    //不正なファイル名にならないようuuidに置き換え
                    fileName = Guid.NewGuid().ToString() + extension;

                    /*
                    //保存先パスを設定
                    string filePath = Path.Combine(path, fileName);

                    //同名ディレクトリが見つかったときは連番にします（上書き防止）。
                    int i = 1;
                    string newPath = filePath;
                    while (Directory.Exists(newPath))
                    {
                        newPath = $"{path} ({i++})";
                    }
                    */


                    //アップロードされたファイル内容を指定したパスに保存します。（いらない）
                    //posted.SaveAs(newPath);

                    string title = HtmlEncode(@TextBox_Upload_Comment.Text);
                    if (title == "")
                    {
                        title = "無題";
                    }
                    string pass = @TextBox_UploadPass.Text;
                    if (pass != "")
                    {
                        pass = pass.GetHashCode().ToString();
                    }

                    //HttpPostedFileクラス（System.Web名前空間）のInputStreamプロパティを介して、アップロード・ファイルをいったんbyte配列に格納しておく
                    //byte配列に格納してしまえば、後は通常のテキストと同様の要領でデータベースに格納できる。

                    //バイトを取得します。
                    Byte[] aryData = new Byte[FileUpload_userfile.PostedFile.ContentLength];
                    FileUpload_userfile.PostedFile.InputStream.Read(aryData, 0, FileUpload_userfile.PostedFile.ContentLength);
                    //MIMEタイプを取得します。
                    string type = FileUpload_userfile.PostedFile.ContentType;

                    //データベースにファイルを保存します。
                    if (!FileShareClass.SetT_FileShareInsert(Global.GetConnection(), SessionManager.User.M_User.id, SessionManager.User.M_User.name1, fileName, title, pass, type, aryData))
                    {
                        lblResult.Text = "アップロードに失敗しました。ファイル容量等を見直して下さい。";
                        return;
                    }

                    lblResult.Text = "アップロードファイルを保存しました！";

                    //データバインド
                    GridView1.DataBind();

                    return;
                }
                else
                {
                    lblResult.Text = "ファイル名が見つかりません！";
                    return;
                }

            }
            else
            {
                lblResult.Text = "ローカルからファイルを選択して下さい。";
                return;
            }

        }



        protected void Button_DownLoad(object sender, EventArgs e)
        {
            lblDLResult.Text = "";
            TextBox_dl.Text = HtmlEncode(TextBox_dl.Text);

            //仮Password
            string pass = TextBox_DownloadPass.Text;
            if (pass != "")
            {
                pass = pass.GetHashCode().ToString();
            }

            if (TextBox_dl.Text != null && TextBox_dl.Text != "")
            {
                /*
                //ダウンロードするファイル名を設定（@"C\:temp"だけなどは危険なためデバッグ時以外使用禁止）
                //書き込み先ディレクトリがない場合はエラーになります。
                string path = @"c:\\UploadedFiles\\";

                if (Directory.Exists(path))
                {
                    //ディレクトリが存在します。
                }
                else
                {
                    //ディレクトリが存在しません。
                    lblDLResult.Text = "ファイルディレクトリが存在しません。";
                    return;
                }

                //保存先パスを設定
                string filePath = Path.Combine(path, TextBox_dl.Text);

                //拡張子を取得
                string extension = Path.GetExtension(filePath);

                if (extension == string.Empty)
                {
                    //
                    lblDLResult.Text = "拡張子まで正確に入力してください。";
                    return;
                }


                //↓がローカルを参照してしまっているためおかしな挙動になっている
                if (!Directory.Exists(filePath))
                {
                    //ファイルが存在しません。
                    lblDLResult.Text = "指定したファイルが存在しません。";
                    return;
                }
                */

                    DATASET.DataSet.T_FileShareRow dr = FileShareClass.GetT_FileShareRow(Global.GetConnection(), TextBox_dl.Text, pass);
                    if(dr != null)
                    {

                        // HTTPレスポンスのヘッダ＆エンティティのクリア（初期化）
                        Response.Clear();

                        // 拡張子を取得 ".txt"など
                        //string extension = Path.GetExtension(TextBox_dl.Text);

                        // ダウンロード用ファイルの種別とデフォルトの名前を指定
                        this.Response.ContentType = @"application/octet-stream"; //デフォルト設定: @"application/octet-stream"
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + TextBox_dl.Text);

                        // MIME Typeを取得
                        Response.ContentType = (string)@dr.type;

                        // ダウンロード実行 binary HTMLページと一緒にロードされる
                        Response.BinaryWrite((Byte[])dr.datum);

                        // ダウンロード実行　その2　ストリームに書き出し　やることは同じ
                        //this.Response.OutputStream.Write((Byte[])dr.datum, 0, ((Byte[])dr.datum).Length);

                        // 書き出しテスト　やることは同じ
                        //string data = "abcd";
                        //byte[] databytes = System.Text.Encoding.GetEncoding("shift-JIS").GetBytes(data);
                        //this.Response.OutputStream.Write(databytes, 0, databytes.Length);

                        // HTTPレスポンス エンティティ設定の終了　スレッドの中止　Response.End();
                        //※ これを行わないと、当該画面のHTML出力がファイル出力の後に続いてしまったりします。
                        //※ Response.End();をする場合、Response.Flash()は最終的にはFlashされるため必須ではありません。
                        //※ Webではスレッドを中止するとエラーが出ます。そのときはResponse.Flush();にします。
                        Response.Flush();

                    }
                    else
                    {
                        //ファイルが存在しません。
                        lblDLResult.Text = "指定したファイルが存在しません。あるいは、パスワードが誤っています。";
                        return;
                    }

                    /*
                    //Response情報クリア
                    Response.ClearContent();

                    //バッファリング
                    Response.Buffer = true;

                    //HTTPヘッダー情報・MIMEタイプ設定
                    Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", lblDLResult.Text));
                    //好きなMIMEタイプを設定
                    Response.ContentType = DropDownList1.SelectedValue;

                    //ファイルを書き出し
                    Response.WriteFile(filePath);
                    Response.Flush();
                    Response.End();
                    */

                    lblDLResult.Text = "ダウンロードに成功しました。";
                    return;

            }
            else
            {
                lblDLResult.Text = "ダウンロードしたいファイル名を入力して下さい。";
                return;
            }

        }







        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //好きなコードを入れて下さい。

            // コマンド名が“Remove”の場合にのみ処理（独自の削除ボタン）
            if (e.CommandName == "Remove")
            {

                //コマンドの引数を取得
                int args = Int32.Parse(e.CommandArgument.ToString());

                //ロードのためにテーブルには用いるデータをバインドし、Visible=trueにしている必要がある。falseでも配列int[]は数える。
                //【重要】ReadOnly属性がついていないと読み込みできない。

                //セッション変数argsを初期化
                Session.Add("args", (string)"null");

                RemoveFileShareRow(args);

                // コマンド名が“DownLoad”の場合にのみ処理（選択ボタン）
            }
            else if (e.CommandName == "DownLoad")
            {
                //コマンドの引数を取得
                int args = Int32.Parse(e.CommandArgument.ToString());

                //ロードのためにテーブルには用いるデータをバインドし、Visible=trueにしている必要がある。falseでも配列int[]は数える。
                //【重要】ReadOnly属性がついていないと読み込みできない。

                DLFileShareRow(args);

            }
            return; //grid_RowCommand
        }



        protected void DLFileShareRow(int args)
        {
            if (Session["args"].ToString() != "null")
            {
                //GridView1の色を変えた色をもとに戻す
                int resetargs = int.Parse(Session["args"].ToString());
                GridView1.Rows[resetargs].BackColor = System.Drawing.Color.Empty;
            }

            //新たに色を変更する行を記憶
            Session.Add("args", args);

            //行の色変更（選択行を強調表示）
            GridView1.Rows[args].BackColor = System.Drawing.Color.Red;

            //idをロード
            //string isbn_name = GridView1.Rows[args].Cells[0].Text.Trim();

            // クリックされた[args]行の左から3番目の列[0-nで数える]のセルにある「テキスト」を取得
            //FileNameをロード
            string isbn_filename = GridView1.Rows[args].Cells[2].Text.Trim();

            lbluid.Text = isbn_filename;
            TextBox_dl.Text = isbn_filename;

            return;
        }


        protected void RemoveFileShareRow(int args)
        {

            //idをロード
            //String isbn_name = GridView1.Rows[args].Cells[0].Text.Trim();

            // クリックされた[args]行の左から3番目の列[0-nで数える]のセルにある「テキスト」を取得
            //FileNameをロード
            String isbn_filename = GridView1.Rows[args].Cells[2].Text.Trim();

            //個別テーブルからSQL文を用いて削除（未実装）

            if(FileShareClass.DeleteT_FileShareRow(Global.GetConnection(), SessionManager.User.M_User.id, isbn_filename))
            {
                //一覧から削除
                lbluid.Text = "null";
                lblDLResult.Text = "ファイルを１件削除しました！";
            }
            else
            {
                lblDLResult.Text = "他人のファイルは削除できません！";
            }


            //データバインド
            BindData();

        }


        /// <summary>
        /// データバインドを実行します。
        /// </summary>
        protected void BindData()
        {
            //データソースを手動で入れる場合
            //例：GridView1.DataSource = Session["TaskTable"];

            //データバインド
            GridView1.DataBind();

            if (lbluid.Text != "null")
            {

                //この方式では複数ページが取得できない
                int cnt = GridView1.Rows.Count;

                //ページを複数表示したい場合（仮実装）
                //int page = GridView1.PageCount;
                //cnt *= page;

                try
                {

                    //----------------------------------------------
                    for (int i = 0; i < cnt; i++)
                    {
                        //Rows[i]行の左から3番目の列[0-nで数える]のセルにある「テキスト」を取得
                        //uidをロード
                        string isbn_uid = GridView1.Rows[i].Cells[2].Text.Trim();

                        //uidは一致したか？
                        if (lbluid.Text == isbn_uid)
                        {
                            //GridView1におけるuidと一致する行の背景に色を付けて強調表示する
                            GridView1.Rows[i].BackColor = System.Drawing.Color.AliceBlue;
                            break;
                        }

                    }
                    //----------------------------------------------

                }
                catch
                {
                    //存在しないRowsを読み込んでいます！
                    //色変更を行わずに強制終了します。
                    return;
                }

            }


            //正常に終了
            return;
        }

        protected void Push_DataBind(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }
    }
}