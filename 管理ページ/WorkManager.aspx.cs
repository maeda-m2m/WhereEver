using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using static System.Web.HttpUtility;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace WhereEver.管理ページ
{
    public partial class WorkManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //user id 取得
                lbluid.Text = HtmlEncode(SessionManager.User.M_User.id.Trim());

                //セッション変数argsを初期化
                Session.Add("args", (string)"null");
                Session.Add("uuid", (string)"null");

                //Label生成（初期化）
                ResetItems();

            }



            // 編集中UUID確認フォームを用いるなら利用可能
            //消去すると１段ずれることがあるため、選択行かどうか１段ずつ検索し直す。
            for (int i = 0; i < GridView_Meibo.Rows.Count; i++)
            {
                if (GridView_Meibo.Rows[i].Cells[0].Text == TextBox_Work_uuid.Text)
                {
                    Session.Add("args", i);
                    Session.Add("uuid", GridView_Meibo.Rows[i].Cells[0].Text);
                    //GridView_Meiboの選択行を赤色にする
                    //int resetargs = int.Parse(Session["args"].ToString());
                    GridView_Meibo.Rows[i].BackColor = System.Drawing.Color.Red;
                    break;
                }
            }

            ChangeItemsColor();
            PreView();
        }

        protected void ResetItems()
        {
            //初期化
            Label_dropbox_black.Text = "";
            Label_dropbox_red.Text = "";
            Label_dropbox_blue.Text = "";
            Label_dropbox_green.Text = "";
        }

        protected bool SetItems(string item_id, string taskname, string color)
        {

            if (taskname == "")
            {
                //SQLからユーザーに関連する作業一覧を読み込み
                //GET
                string dtid = item_id.Replace("Label_item", "");
                DATASET.DataSet.T_PdbDataTable dt = ClassLibrary.ToDoManagerClass.GetT_Pdb_DataTable(Global.GetConnection(), int.Parse(dtid));  //Pidで取得
                if (dt != null)
                {
                    if (!dt[0].IsNull("Pname"))
                    {
                        if (!dt[0].IsNull("Povertime"))
                        {
                            DateTime Povertime = dt[0].Povertime;
                            if (Povertime != DateTime.Parse("2100/01/01"))
                            {
                                //Poverttimeが有効
                                taskname = HtmlEncode(dt[0].Pname.Trim() + "(" + Povertime.ToString("yyyy年MM月dd日") + "〆)");
                            }
                            else
                            {
                                //Povertimeが無効
                                taskname = HtmlEncode(dt[0].Pname.Trim() + "(〆未定)");
                            }
                        }
                        else
                        {
                            //PovertimeがNull（普通はここに来ない）
                            taskname = HtmlEncode(dt[0].Pname.Trim() + "(〆未定)");
                        }
                    }
                }
            }

            //ラベルに書く文章をここに入力(SQLからの入力でも可)
            string content = taskname;  //"タスク("+ item_id + color + ")"; //テスト専用

            if (content == null || content == "")
            {
                //未処理がなければfalseを返す
                return false;
            }

            Label lbl = (Label)FindControl("Label_dropbox_" + color);
            //Label_transparent.Text = "";

            string inst = "<span id=\"" + item_id + "\" runat=\"server\" class=\"item\" draggable=\"true\" ondragstart=\"f_dragstart(event)\" style=\"background-color: " + color + ";\" >" + content + "<br /><br /></span>";

            if (Label_dropbox_black.Text.Contains(item_id) || Label_dropbox_red.Text.Contains(item_id) || Label_dropbox_blue.Text.Contains(item_id) || Label_dropbox_green.Text.Contains(item_id))
            {
                Label_dropbox_black.Text = Label_dropbox_black.Text.Replace("black", color).Replace(inst, "");
                Label_dropbox_red.Text = Label_dropbox_red.Text.Replace("red", color).Replace(inst, "");
                Label_dropbox_blue.Text = Label_dropbox_blue.Text.Replace("blue", color).Replace(inst, "");
                Label_dropbox_green.Text = Label_dropbox_green.Text.Replace("green", color).Replace(inst, "");
            }

            //リフレクション
            lbl.Text += inst;
            return true;
        }

        protected void ChangeItemsColor()
        {
            // 宣言と定義

            //初期化
            ResetItems();

            // HiddenからViewStateのValue（文字列）を読み出し
            //string sp [p1:アイテム名], [p2:カラー]; 文字配列数は2nに等しくなる。
            string text = HtmlEncode(Hidden_Label_item.Value);//管理者ツールで弄れるため必ずエンコードする

            //SQLからユーザーに関連する作業一覧を読み込み
            DATASET.DataSet.T_PdbDataTable dt = ClassLibrary.ToDoManagerClass.GetT_Pdb_DataTable(Global.GetConnection());

            if (dt != null)
            {
                //とりあえず10件まで表示
                for (int k = 0; k < Math.Min(10, dt.Count); k++)
                {
                    //init
                    string color = "black";

                    //Label_item_id{n}, color
                    if (!dt[k].IsNull("Pname") && !dt[k].IsNull("Pid") && !text.Contains("Label_item" + k))
                    {

                        if (!dt[k].IsNull("Pstarttime"))
                        {
                            if (DateTime.Now >= dt[k].Pstarttime)
                            {
                                //スタートが決まっている
                                color = "red";

                                if (DateTime.Now.AddDays(-14) >= dt[k].Povertime)
                                {
                                    //完成２週間前
                                    color = "blue";
                                }

                            }
                        }

                        DateTime Povertime = dt[k].Povertime;
                        if (!dt[k].IsNull("Povertime"))
                        {
                            if (DateTime.Now >= dt[k].Povertime)
                            {
                                //すでに完了している（実際に表示されるのは==のみ）
                                color = "green";
                            }

                            DateTime Povertime2 = dt[k].Povertime;
                            if (Povertime2 != DateTime.Parse("2100/01/01"))
                            {
                                //Poverttimeが有効
                                SetItems("Label_item" + dt[k].Pid, HtmlEncode(dt[k].Pname.Trim() + "(" + Povertime2.ToString("yyyy年MM月dd日") + "〆)"), color);
                            }
                            else
                            {
                                //Povertimeが無効
                                SetItems("Label_item" + dt[k].Pid, HtmlEncode(dt[k].Pname.Trim() + "(〆未定)"), color);
                            }
                        }
                        else
                        {
                            SetItems("Label_item" + dt[k].Pid, HtmlEncode(dt[k].Pname.Trim() + "(〆未定)"), color);
                        }

                    }
                    else
                    {
                        //警告：中身なし
                    }
                }

            }



            string[] sp = text.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);//空白文字をトリム 

            //デバッグ用
            TextBox_LabelDDResult.Text = text;
            //初期化
            Hidden_Label_item.Value = "";

            // 総アイテム数
            int maxvalue = sp.Length;

            for (int i = 0; i < maxvalue; i++)
            {

                // ViewStateからリフレクションで読み出し
                //Label lbl = (Label)FindControl("Label_item" + i);
                Label lbl = new Label();

                if (sp[i].Contains("Label_item"))
                {
                    //Label生成（これを先にやらないとLabelが存在しない）
                    SetItems(sp[i], "", sp[i + 1]); //Label_item_id{n}, color

                    //[p1:アイテム名]を取得し一時保存する
                    //lbl = (Label)FindControl(sp[i]);
                    Hidden_Label_item.Value += sp[i] + "," + sp[i + 1] + ",";
                }

            }
        }


        protected void Push_DD(object sender, EventArgs e)
        {
            if (Panel_DD.Visible)
            {
                Panel_DD.Visible = false;
            }
            else
            {
                Panel_DD.Visible = true;
            }
        }

        protected void Push_GetLabelDD(object sender, EventArgs e)
        {
            //完成を反映して除去（ポストバック）
            //Label_dropbox_black.Text = "";
            DeleteHiddenFieldDD("green");
            Label_dropbox_green.Text = "";

            //承認
            UpdateHiddenFiledDD("red");
            UpdateHiddenFiledDD("blue");
        }

        protected void Push_DeleteBlackDD(object sender, EventArgs e)
        {
            //未処理を破棄（SQL未実装）
            DeleteHiddenFieldDD("black");
            Label_dropbox_black.Text = "";
        }


        protected void UpdateHiddenFiledDD(string color = "red")
        {
            string str = HtmlEncode(Hidden_Label_item.Value);
            StringBuilder sb = new StringBuilder();
            sb.Append(str);

            MatchCollection matche = Regex.Matches(str, "Label_item[0-9]*," + color + ",");
            foreach (Match m in matche)
            {
                string dtid = m.ToString().Replace("Label_item", "");
                dtid = dtid.Replace("," + color + ",", "");
                if (color == "red" || color == "blue")
                {

                    DATASET.DataSet.T_PdbDataTable dt = ClassLibrary.ToDoManagerClass.GetT_Pdb_DataTable(Global.GetConnection(), int.Parse(dtid));

                    //Pstarttimeは必須。開始日が未来なら本日の日付でアップデートする。
                    if (!dt[0].IsNull("Pstarttime"))
                    {
                        if (dt[0].Pstarttime > DateTime.Now)
                        {
                            //承認 UPDATE p2=false
                            ClassLibrary.ToDoManagerClass.SetT_Pdb_Update(Global.GetConnection(), int.Parse(dtid), false);
                        }
                    }

                }

            }
        }

        /// <summary>
        /// HiddenFieldの指定したValueを破棄します。
        /// 自動PostBack時の未処理反映を妨害します（ラベルに投入されたアイテムは残ります）。
        /// </summary>
        protected void DeleteHiddenFieldDD(string color = "black")
        {
            string str = HtmlEncode(Hidden_Label_item.Value);
            StringBuilder sb = new StringBuilder();
            sb.Append(str);

            MatchCollection matche = Regex.Matches(str, "Label_item[0-9]*," + color + ",");
            foreach (Match m in matche)
            {
                string dtid = m.ToString().Replace("Label_item", "");
                dtid = dtid.Replace("," + color + ",", "");
                if (color == "black")
                {
                    //DELETE
                    ClassLibrary.ToDoManagerClass.DeleteT_Pdb_DataRow(Global.GetConnection(), int.Parse(dtid));
                }
                if (color == "green")
                {
                    //完成　UPDATE p2=true
                    ClassLibrary.ToDoManagerClass.SetT_Pdb_Update(Global.GetConnection(), int.Parse(dtid), true);
                }

                sb.Replace(m.ToString(), "");
                //Regex reg = new Regex(m.ToString());
                //str = reg.Replace(str, "");
            }
            Hidden_Label_item.Value = sb.ToString();
        }

        /// <summary>
        /// プロジェクト新規作成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Push_NewProj(object sender, EventArgs e)
        {
            //プロジェクト管理にジャンプ
            this.Response.Redirect("/Project System/PIchiran.aspx", false);
        }




        /// <summary>
        /// 現在のuuidを返します。
        /// </summary>
        /// <returns></returns>
        protected string GETSetUUID()
        {
            if (TextBox_Work_uuid.Text == "")
            {
                TextBox_Work_uuid.Text = Guid.NewGuid().ToString();
            }
            return HtmlEncode(TextBox_Work_uuid.Text);
        }

        protected void Button_MeiboUpLoad(object sender, EventArgs e)
        {
            //ラベル初期化
            lblMeiboPictureResult.Text = "";

            if (FileUpload_MeiboPicture.HasFile)
            {
                //パスを排除したファイル名を取得
                string fileName = FileUpload_MeiboPicture.PostedFile.FileName;

                //拡張子を取得
                string extension = Path.GetExtension(fileName);

                if (extension != "png" && extension != "jpg" && extension != "jpeg" && extension != "gif")
                {
                    lblMeiboPictureResult.Text = "ファイル形式に対応していません！";
                }



                //File Upload to Database
                int result = ClassLibrary.WorkRosterClass.Set_WorkThumbnail_UpLoad(FileUpload_MeiboPicture.PostedFile, GETSetUUID(), 20000);
                if (result == 0)
                {
                    lblMeiboPictureResult.Text = "ファイル名が見つかりません！";
                }
                else if (result == -1)
                {
                    lblMeiboPictureResult.Text = "アップロードに失敗しました。";
                }
                else if (result == 1)
                {
                    lblMeiboPictureResult.Text = "アップロードが完了しました！";
                    PreView();
                }

            }
            else
            {
                lblMeiboPictureResult.Text = "ローカルからファイルを選択して下さい。";
            }
            return;
        }

        protected void PreView()
        {
            //ThumbnailDownLoad by DataBase
            string uuid = GETSetUUID();
            if (uuid != "")
            {
                lblMeiboPictureResult.Text = ClassLibrary.FileShareClass.Get_Thumbnail_DownLoad_src(Page.Response, uuid, 20000);
            }
            else
            {
                lblMeiboPictureResult.Text = "ここにサムネイルがプレビューされます。";
            }
            return;
        }


        protected void Push_MeiboDelete(object sender, EventArgs e)
        {
            DeleteWorkThumbnail();
        }

        protected void DeleteWorkThumbnail()
        {
            if (ClassLibrary.FileShareClass.DeleteT_ThumbnailRow(Global.GetConnection(), GETSetUUID()))
            {
                lblMeiboPictureResult.Text = "サムネイルをリセットしました。";
                lblMeiboPictureResult.Text = "Thumbnail_Deleted";
            }
            else
            {
                lblMeiboPictureResult.Text = "サムネイルが見つかりませんでした。";
            }
        }



        protected void Push_MeiboCorrect(object sender, EventArgs e)
        {
            // 1px = 0.026cm;
            // すなわち、
            // 1cm = 任意のpx / 0.026cm;
            // 証明写真は30mm*40mm ≒ 354px * 472px

            StringBuilder sb = new StringBuilder();

            //NVarChar
            sb.Append(HtmlEncode(TextBox_CompanyName.Text).Trim());
            sb.Append("\r\f");
            sb.Append(HtmlEncode(TextBox_MeiboWork.Text).Trim());
            sb.Append("\r\f");
            sb.Append(HtmlEncode(TextBox_MeiboName.Text).Trim());
            sb.Append("\r\f");
            sb.Append(HtmlEncode(TextBox_MeiboArea.Text).Trim());
            sb.Append("\r\f");


            //Date split('/')で分割可能
            StringBuilder sb2 = new StringBuilder();
            sb2.Append(HtmlEncode(TextBox_Meibo_year.Text).Trim());
            sb2.Append("/");
            sb2.Append(HtmlEncode(TextBox_Meibo_month.Text).Trim());
            sb2.Append("/");
            sb2.Append(HtmlEncode(TextBox_Meibo_day.Text).Trim());
            sb.Append(sb2.ToString());
            sb.Append("\r\f");

            //TextBox_Meibo_TelX split('-')で分割可能
            StringBuilder sb3 = new StringBuilder();
            sb3.Append(HtmlEncode(TextBox_Meibo_Tel1.Text).Trim());
            sb3.Append("-");
            sb3.Append(HtmlEncode(TextBox_Meibo_Tel2.Text).Trim());
            sb3.Append("-");
            sb3.Append(HtmlEncode(TextBox_Meibo_Tel3.Text).Trim());
            sb.Append(sb3.ToString());
            sb.Append("\r\f");

            //TextBox_MeiboAddress
            sb.Append(HtmlEncode(TextBox_MeiboAddress.Text).Trim());
            sb.Append("\r\f");

            //デバッグコンソールに追加
            TextBox_MeiboResult.Text = sb.ToString();

            //サムネイル
            //公開用（非暗号化埋め込みコードをDBにアップロード）
            string s_datum = HtmlEncode(ClassLibrary.FileShareClass.Get_Thumbnail_DownLoad_src(Page.Response, SessionManager.User.M_User.id.Trim(), 20000));
            byte[] datum = Encoding.GetEncoding("UTF-8").GetBytes(s_datum);

            //データベースに追加(Insert)　実はInsertでも上書きUpdateできる。
            ClassLibrary.WorkRosterClass.SetT_WorkRosterInsert(Global.GetConnection(), GETSetUUID(), HtmlEncode(TextBox_CompanyName.Text).Trim(), datum, HtmlEncode(TextBox_MeiboName.Text).Trim(), HtmlEncode(TextBox_MeiboWork.Text).Trim(), HtmlEncode(TextBox_MeiboArea.Text).Trim(), DateTime.Parse(sb2.ToString()), sb3.ToString(), HtmlEncode(TextBox_MeiboAddress.Text).Trim(), CheckBox_MeiboPB.Checked);
            GridView_Meibo.DataBind();
        }

        protected void Push_MeiboButton(object sender, EventArgs e)
        {
            if (Panel_Meibo.Visible)
            {
                Panel_Meibo.Visible = false;
            }
            else
            {
                Panel_Meibo.Visible = true;
            }
        }








        //-------------------------------------------------------------------------------------------------------------------------------------------------------------



        protected void Grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //好きなコードを入れて下さい。

            // コマンド名が“Remove”の場合にのみ処理（独自の削除ボタン）
            if (e.CommandName == "Remove")
            {

                //コマンドの引数を取得
                int args = Int32.Parse(e.CommandArgument.ToString());

                //ロードのためにテーブルには用いるデータをバインドし、Visible=trueにしている必要がある。falseでも配列int[]は数える。
                //【重要】ReadOnly属性がついていないと読み込みできない。

                string uuid = GridView_Meibo.Rows[args].Cells[0].Text;
                ClassLibrary.WorkRosterClass.DeleteT_WorkRosterRow(Global.GetConnection(), uuid);
                DeleteWorkThumbnail();

                if (TextBox_Work_uuid.Text == uuid)
                {
                    //セッション変数argsを初期化
                    Session.Add("args", (string)"null");
                    Session.Add("uuid", (string)"null");
                    TextBox_Work_uuid.Text = "";    //Reset
                }

                GridView_Meibo.DataBind();

                // 編集中UUID確認フォームを用いるなら利用可能
                //消去すると１段ずれることがあるため、選択行かどうか１段ずつ検索し直す。
                for (int i = 0; i < GridView_Meibo.Rows.Count; i++)
                {
                    if (GridView_Meibo.Rows[i].Cells[0].Text == TextBox_Work_uuid.Text)
                    {
                        Session.Add("args", i);
                        Session.Add("uuid", GridView_Meibo.Rows[i].Cells[0].Text);
                        //GridView_Meiboの選択行を赤色にする
                        //int resetargs = int.Parse(Session["args"].ToString());
                        GridView_Meibo.Rows[i].BackColor = System.Drawing.Color.Red;
                        break;
                    }
                }


                // コマンド名が“DownLoad”の場合にのみ処理（選択ボタン）
            }
            else if (e.CommandName == "DownLoad")
            {
                //コマンドの引数を取得
                int args = Int32.Parse(e.CommandArgument.ToString());

                //ロードのためにテーブルには用いるデータをバインドし、Visible=trueにしている必要がある。falseでも配列int[]は数える。
                //【重要】ReadOnly属性がついていないと読み込みできない。

                //ResetMeibo();

                //主キー呼び出しのため、参照されるDataTableは必ずRow=1（ロード前に他アカウントで削除されていたら0）となる。
                string uuid = GridView_Meibo.Rows[args].Cells[0].Text;
                DATASET.DataSet.T_WorkRosterDataTable dt = ClassLibrary.WorkRosterClass.GetT_WorkRoster(Global.GetConnection(), uuid);


                if (Session["args"].ToString() != "null")
                {
                    //GridView1の色を変えた色をもとに戻す
                    int resetargs = int.Parse(Session["args"].ToString());
                    GridView_Meibo.Rows[resetargs].BackColor = System.Drawing.Color.Empty;
                }

                if (dt == null)
                {
                    //Fatal Error
                    return;
                }

                Session.Add("uuid", uuid);

                //編集テーブルに代入
                TextBox_Work_uuid.Text = uuid;
                TextBox_CompanyName.Text = dt[0].workCompanyName.ToString().Trim();
                TextBox_MeiboWork.Text = dt[0].workPost.ToString().Trim();
                TextBox_MeiboName.Text = dt[0].workUserName.ToString().Trim();
                TextBox_MeiboArea.Text = dt[0].workAssignment.ToString().Trim();
                TextBox_Meibo_year.Text = dt[0].workBirthday.Year.ToString().Trim();
                TextBox_Meibo_month.Text = dt[0].workBirthday.Month.ToString().Trim();
                TextBox_Meibo_day.Text = dt[0].workBirthday.Day.ToString().Trim();

                string[] phone = dt[0].workPhoneNo.ToString().Trim().Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);//空白文字をトリム
                //3つなら直接代入したほうが早い かつ 安全
                if (phone.Length >= 1)
                {
                    TextBox_Meibo_Tel1.Text = phone[0];
                }
                if (phone.Length >= 2)
                {
                    TextBox_Meibo_Tel2.Text = phone[1];
                }
                if (phone.Length >= 3)
                {
                    TextBox_Meibo_Tel3.Text = phone[2];
                }

                TextBox_MeiboAddress.Text = dt[0].workMail.ToString().Trim();

                GridView_Meibo.DataBind();
                PreView();

                //新たに色を変更する行を記憶
                Session.Add("args", args);

                //行の色変更（選択行を強調表示）
                GridView_Meibo.Rows[args].BackColor = System.Drawing.Color.Red;

            }
        }

        protected void Push_Reset_Work(object sender, EventArgs e)
        {
            TextBox_Work_uuid.Text = "";
            if (Session["args"].ToString() != "null")
            {
                //GridView1の色を変えた色をもとに戻す
                int resetargs = int.Parse(Session["args"].ToString());
                GridView_Meibo.Rows[resetargs].BackColor = System.Drawing.Color.Empty;
            }
            //init
            Session.Add("uuid", "null");
            Session.Add("args", "null");
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

    }
}