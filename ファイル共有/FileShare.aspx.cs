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

            if (!IsPostBack)
            {
                Session.Add("args", (string)"null");
                lblid.Text = SessionManager.User.M_User.id;
                lbluid.Text = "null";
            }

            //初期化
            Response.SuppressContent = false;

        }

        protected void Button_UpLoad(object sender, EventArgs e)
        {
            //ラベル初期化
            lblResult.Text = "";

            if (FileUpload_userfile.HasFile)
            {
                //File Upload to Database
                int result = FileShareClass.Set_File_UpLoad(FileUpload_userfile.PostedFile, HtmlEncode(@TextBox_Upload_Comment.Text), @TextBox_UploadPass.Text, LoadByteLength(), CheckBox_Annonimas.Checked);
                if(result == 0)
                {
                    lblResult.Text = "ファイル名が見つかりません！";
                }
                else if (result == -1)
                {
                    lblResult.Text = "アップロードに失敗しました。";
                }
                else if (result == 1)
                {
                    lblResult.Text = "アップロードファイルを保存しました！";
                    //データバインド
                    GridView1.DataBind();
                }

            }
            else
            {
                lblResult.Text = "ローカルからファイルを選択して下さい。";
            }
            return;
        }


        /// <summary>
        /// 一度にロードするデータバイト長設定をロードします。
        /// </summary>
        /// <returns></returns>
        protected int LoadByteLength()
        {
            //一度にロードするデータバイト長
            int length = 8000;

            if (RadioButton_Streaming_8000.Checked)
            {
                length = 8000;
            }
            else if (RadioButton_Streaming_20000.Checked)
            {
                length = 2000;
            }
            else if (RadioButton_Streaming_30000.Checked)
            {
                length = 30000;
            }
            else if (RadioButton_Streaming_40000.Checked)
            {
                length = 40000;
            }

            return length;
        }

        protected void Button_DownLoad(object sender, EventArgs e)
        {
            lblDLResult.Text = "";
            if (TextBox_dl.Text != null && TextBox_dl.Text != "")
            {
                //FileDownLoad by DataBase
                if(!FileShareClass.Get_File_DownLoad(Page.Response, HtmlEncode(TextBox_dl.Text), TextBox_DownloadPass.Text, LoadByteLength()))
                {
                    //ファイルが存在しません。
                    lblDLResult.Text = "指定したファイルが存在しません。あるいは、パスワードが誤っています。";
                }
            }
            else
            {
                lblDLResult.Text = "ダウンロードしたいファイル名を入力して下さい。";
                return;
            }
            return;
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