using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using static System.Web.HttpUtility;
using System.Text.RegularExpressions;

namespace WhereEver.管理ページ
{
    public partial class Kanri : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblResult.Text = SessionManager.User.M_User.id;

            //SMTP設定
            if (DropDownList_SMTP.SelectedValue == "手動")
            {
                Panel_UserSMTP.Visible = true;
            }
            else
            {
                Panel_UserSMTP.Visible = false;
            }

            PreView();

            //Twetterボタン作成 test
            Label_Tweet.Text = "<p class=\"center\"><script src = \"../Scripts/setTwit.js\" type=\"text/javascript\" charset=\"utf-8\"></script>\r\f<button class=\"btn_loginlist\" type=\"button\" onclick=\"twitText('[m2m server]: tweet test.');\">tweet(test)</button></p>";

        }

        //GridViewのRowCommand属性に参照可能なメソッドを入力すると↓が自動生成されます。
        //GridViewが読み込まれたときに実行されます。
        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //好きなコードを入れて下さい。


        }

        protected void grid_RowUpdatedCommand(object sender, GridViewUpdatedEventArgs e)
        {

            GridView2.DataBind();

            //列
            int args = 0;

            //ロードのためにテーブルには用いるデータをバインドし、Visible=trueにしている必要がある。falseでも配列int[]は数える。
            // クリックされた[args]行の左から2番目の列[0-nで数える]のセルにある「テキスト」を取得
            //【重要】ReadOnly属性がついていないと読み込みできない。

            //idをロード（必要なら）
            //string isbn_name = GridView2.Rows[args].Cells[0].Text.Trim();

            //名前をロード
            string isbn_name1 = GridView2.Rows[args].Cells[2].Text.Trim();

            //passwordをロード
            string isbn_pw = GridView2.Rows[args].Cells[3].Text.Trim();

            if (isbn_name1 != null || isbn_pw != null) {
                //SessionManagerに代入
                SessionManager.User.M_User.name1 = isbn_name1;
                SessionManager.User.M_User.pw = isbn_pw;
            }
            return;
        }


        //-----------------------------------------------------------------------------------

        //メール送信システム

        protected void btnEditTop_Click(object sender, EventArgs e)
        {
            DATASET.DataSet.T_TopPageRow dr = Class.Toppage.GetT_TopPage(Global.GetConnection());
            if (dr != null)
            {
                TextBox_EditTop.Text = dr.TopPage;
            }
        }

        protected void btnReformTop_Click(object sender, EventArgs e)
        {
            //sql更新 ※全部Nullだと更新できません。DBのDateTimeには何か値を入れておいて下さい。

            //StringBuilder sb = new StringBuilder();
            //sb.Append(HtmlEncode(TextBox_EditTop.Text));
            //"<br />"タグのみ許可
            //sb.Replace("&lt;br /&gt;", "<br />");

            int smtpvalue = 1;
            if (DropDownList_SMTP.SelectedValue == "mail.m2m-asp.com")
            {
                smtpvalue = 1;
            }
            else if (DropDownList_SMTP.SelectedValue == "192.168.2.156")
            {
                smtpvalue = 2;
            }
            else if (DropDownList_SMTP.SelectedValue == "192.168.2.140")
            {
                smtpvalue = 3;
            }
            else if (DropDownList_SMTP.SelectedValue == "手動")
            {
                smtpvalue = 0;
            }

            string pattern = @"^[0-9]";
            Regex rx = new Regex(pattern);
            string port = rx.Replace(HtmlEncode(TextBox_Port.Text), pattern);
            //bool match = Regex.IsMatch(HtmlEncode(TextBox_Port.Text), pattern);

            if(port == "")
            {
                port = "0";
            }

            if (ClassLibrary.MailToClass.MailTo(HtmlEncode(TextBox_MailTo.Text), HtmlEncode(TextBox_CC.Text), HtmlEncode(TextBox_BCC.Text), HtmlEncode(TextBox_Title.Text), HtmlEncode(TextBox_EditTop.Text), HtmlEncode(TextBox_Host.Text), int.Parse(port), HtmlEncode(TextBox_UserName.Text), HtmlEncode(TextBox_UserPass.Text), smtpvalue, CheckBox_Annonimas.Checked))
            {
                //送信成功
                Label_MailTo_Result.Text = @"メールの送信に成功しました。";
            }
            else
            {
                //送信失敗
                Label_MailTo_Result.Text = @"メールの送信に失敗しました。";
            }

        }

        protected void btnReformTopDel_Click(object sender, EventArgs e)
        {
            TextBox_EditTop.Text = "";
        }

        protected void Radio_SMTP_changed(object sender, EventArgs e)
        {
            Response.Redirect("Kanri.aspx#edittop", false);
        }

        protected void btnVideo_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Video/Video.aspx", false);
        }

        protected void btnButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("../XHTML5Editor/XHTML5Editor.aspx", false);
        }

        protected void btnDP_Click(object sender, EventArgs e)
        {
            Response.Redirect("../ResearchAnalitics/ResearchAnalitics.aspx", false);
        }

        protected void MailToOpen_Click(object sender, EventArgs e)
        {
            if (Panel_MailTo.Visible)
            {
                Panel_MailTo.Visible = false;
            }
            else
            {
                Panel_MailTo.Visible = true;
            }
        }

        //-----------------------------------------------------------------------------------



        protected void Button_UpLoad(object sender, EventArgs e)
        {
            //ラベル初期化
            lblUpResult.Text = "";

            if (FileUpload_userfile.HasFile)
            {
                //パスを排除したファイル名を取得
                string fileName = FileUpload_userfile.PostedFile.FileName;

                //拡張子を取得
                string extension = Path.GetExtension(fileName);

                if(extension != "png" && extension != "jpg" && extension != "jpeg" && extension != "gif")
                {
                    lblUpResult.Text = "ファイル形式に対応していません！";
                }


                //File Upload to Database
                int result = ClassLibrary.FileShareClass.Set_Thumbnail_UpLoad(FileUpload_userfile.PostedFile, 20000);
                if (result == 0)
                {
                    lblUpResult.Text = "ファイル名が見つかりません！";
                }
                else if (result == -1)
                {
                    lblUpResult.Text = "アップロードに失敗しました。";
                }
                else if (result == 1)
                {
                    lblUpResult.Text = "アップロードファイルが完了しました！";
                    PreView();
                }

            }
            else
            {
                lblUpResult.Text = "ローカルからファイルを選択して下さい。";
            }
            return;
        }

        protected void PreView()
        {
                //ThumbnailDownLoad by DataBase
                string result = ClassLibrary.FileShareClass.Get_Thumbnail_DownLoad_src(Page.Response, SessionManager.User.M_User.id.Trim(), 20000);
                lblDLResult.Text = @result;
                return;
        }

        protected void Push_Delete(object sender, EventArgs e)
        {
            if(ClassLibrary.FileShareClass.DeleteT_ThumbnailRow(Global.GetConnection(), SessionManager.User.M_User.id.Trim()))
            {
                lblUpResult.Text = "サムネイルをリセットしました。";
                lblDLResult.Text = "Thumbnail_Deleted";
            }
            else
            {
                lblUpResult.Text = "サムネイルが見つかりませんでした。";
            }
        }

    }
}