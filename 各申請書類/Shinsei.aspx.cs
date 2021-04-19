using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WhereEver.ClassLibrary;
using System.ComponentModel.DataAnnotations;
using static System.Web.HttpUtility;
using System.Text;

namespace WhereEver
{
    public partial class Calender : System.Web.UI.Page
    {
        //文字入力最大値
        protected const int maxstr = 306;
        protected const int s_maxstr = 100;

        //立替金計算
        protected int waste1 = 0;
        protected int waste2 = 0;
        protected int wasteR1 = 0;
        protected int wasteR2 = 0;
        protected int wasteR3 = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                lblResult.Text = SessionManager.User.M_User.id;

                Session.Add("waste1", waste1);
                Session.Add("waste2", waste2);
                Session.Add("wasteR1", wasteR1);
                Session.Add("wasteR2", wasteR2);
                Session.Add("wasteR3", wasteR3);
                Session.Add("args", (string)"null");

                //GridViewパネル
                Panel0.Visible = true;
                Panel00.Visible = false;
                //削除確認パネル
                Panel_del_pop.Visible = false;
                //初期選択パネル
                Panel1.Visible = true;
                //物品購入申請書パネル
                Panel2.Visible = false;
                //勤怠パネル
                Panel3.Visible = false;
                //物品購入申請書印刷フォーム
                Panel4.Visible = false;
                //勤怠届印刷フォーム
                Panel5.Visible = false;
                //立替金明細表申請パネル
                Panel6.Visible = false;
                //立替金明細表印刷フォーム
                Panel7.Visible = false;

                //印刷ボタンパネル
                Panel_Print.Visible = false;

                //日付初期化
                if (lblSelectedDateA1.Text == "")
                {
                    lblSelectedDateA1.Text = DateTime.Now.ToShortDateString();
                }
                if (lblSelectedDateB1.Text == "")
                {
                    lblSelectedDateB1.Text = DateTime.Now.ToShortDateString();
                }


                //トップのテキスト表示を変更
                //この方式では複数ページが取得できない
                int cnt1 = GridView1.Rows.Count;
                if (cnt1 < 1)
                {
                    //GridViewパネル
                    Panel0.Visible = false;
                    Panel00.Visible = true;
                }


                // T_Shinsei_Config
                // デリート時の確認用ポップアップのコンフィグ設定をロードします
                DATASET.DataSet.T_Shinsei_ConfigDataTable conf = ShinseiLog.GetT_Shinsei_ConfigRow(Global.GetConnection(), SessionManager.User.ID);
                if (conf != null)
                {
                    //デリートコンフィグをロードします
                    CheckBox_is_del_pop.Checked = conf[0].flag_del_pop;
                }
                else
                {
                    //初期設定をインサートします
                    ShinseiLog.SetT_Shinsei_ConfigInsert(Global.GetConnection(), SessionManager.User.ID, CheckBox_is_del_pop.Checked);
                }


            }//!IsPostBack

            topchange();

        }

        /// <summary>
        /// トップのテキストを変更します。
        /// </summary>
        protected void topchange()
        {
            //トップのテキストを変更
            //この方式では複数ページが取得できない
            int cnt2 = GridView1.Rows.Count;
            if (cnt2 >= 1)
            {
                //GridView1.Rows.Countにデータが1以上存在する
                lblTop_0.Text = "各種申請書類を作成または管理できます。";
                lblTop_00.Text = lblTop_0.Text;
            }
            else
            {
                //GridView1.Rows.Countにデータが存在しない(0)
                lblTop_0.Text = "あなたが作成した申請書類はありません。新規作成したい申請書類を選択して下さい。";
                lblTop_00.Text = lblTop_0.Text;
            }
        }

        protected void SetDelPop(object sender, EventArgs e)
        {
            //コンフィグ設定をアップデートします
            ShinseiLog.SetT_Shinsei_ConfigUpdate(Global.GetConnection(), SessionManager.User.ID, CheckBox_is_del_pop.Checked);
        }

        /// <summary>
        /// Validationを確認します。入力に問題がある場合はイベントを停止します。
        /// Validationを個別で無効化する場合、ボタンなどのCausesValidation属性をfalseにして下さい。
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        protected void Reg(ShinseiClass reg)
        {
            ValidationContext context = new ValidationContext(reg, serviceProvider: null, items: null);
            List<ValidationResult> results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(reg, context, results, true);

            if (!isValid)
            {
                foreach (ValidationResult validationResult in results)
                {
                    //Varidationラベルにエラーメッセージを書き込みます
                    Response.Write(validationResult.ErrorMessage.ToString());
                }
            }
            return;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //物品購入書類の確定ボタンが押されたときの処理です。

            //物品購入申請書印刷フォーム
            Panel4.Visible = true;
            //印刷ボタンパネル
            Panel_Print.Visible = true;



            Set_Konyu_Data();

        }

        //購入品
        protected void Set_Konyu_Data()
        {

            name1.Text = "氏名：" + SessionManager.User.M_User.name1;
            DateTime dt = DateTime.Now;
            date.Text = dt.ToShortDateString();

            //------------------------------------------------------------
            //Varidation用のクラスをインスタンス化します。
            ShinseiClass reg = new ShinseiClass();

            //必須入力フォームをチェック
            reg.buy_purpose = HtmlEncode(TextBox_purchaseName.Text.ToString());
            reg.classification = HtmlEncode(TextBox_classification.Text.ToString());
            reg.howMany = HtmlEncode(TextBox_howMany.Text.ToString());
            reg.howMach = HtmlEncode(TextBox_howMach.Text.ToString());
            reg.marketPlace = HtmlEncode(TextBox_marketPlace.Text.ToString());
            reg.buy_purpose = HtmlEncode(TextBox_buy_purpose.ToString());

            Reg(reg);
            //------------------------------------------------------------

            //購入品のテキストを印刷フォームに代入

            int rleng = Math.Min(TextBox_purchaseName.Text.Length, s_maxstr);
            Konyu.Text = TextBox_purchaseName.Text.Substring(0, rleng);

            rleng = Math.Min(TextBox_classification.Text.Length, s_maxstr);
            Syubetsu.Text = TextBox_classification.Text.Substring(0, rleng);

            //string str = TextBox_howMany.Text;

            //-------------------------------------------------------------------

            StringBuilder sb = new StringBuilder(HtmlEncode(TextBox_howMany.Text).ToString());
            sb.Replace("点", "");
            sb.Replace("0", "０");
            sb.Replace("1", "１");
            sb.Replace("2", "２");
            sb.Replace("3", "３");
            sb.Replace("4", "４");
            sb.Replace("5", "５");
            sb.Replace("6", "６");
            sb.Replace("7", "７");
            sb.Replace("8", "８");
            sb.Replace("9", "９");
            string str = sb.ToString();
            //----------------------

            rleng = Math.Min(str.Length, maxstr - 1);
            Suryo.Text = str.Substring(0, rleng) + "点";

            //-------------------------------------------------------------------

            lblTatekae_Result2.Text = "\\" + wasteR2.ToString() + "-";


            rleng = Math.Min(TextBox_marketPlace.Text.Length, s_maxstr);
            KonyuSaki.Text = TextBox_marketPlace.Text.Substring(0, rleng);

            rleng = Math.Min(TextBox_buy_purpose.Text.Length, maxstr);
            Label_Riyuu.Text = TextBox_buy_purpose.Text.Substring(0, rleng);

            rleng = Math.Min(TextBox_ps.Text.Length, maxstr);
            Label_Bikou.Text = TextBox_ps.Text.Substring(0, rleng);

            //-------------------------------------------------------------------

            str = TextBox_howMach.Text;
            rleng = Math.Min(str.Length, s_maxstr - 1);
            //Kingaku.Text = "\\" + str.Substring(0, rleng) + "-";

            sb = new StringBuilder(HtmlEncode(TextBox_howMach.Text).ToString());
            sb.Replace("-", "");
            sb.Replace(",", "");
            sb.Replace("\\", "");

            try
            {
                //intに変換できるか？
                int.Parse(sb.ToString());
            }
            catch
            {
                //不正な文字を検出
                Kingaku.Text = "Error";
                return;
            }

            //カンマ区切りを追加
            int i = 0;
            int lp = 0;
            for (i = 3; i < sb.Length; i += 3)
            {
                if ((i + lp) >= sb.Length)
                {
                    break;
                }
                sb.Insert(sb.Length - (i + lp), ",");
                lp += 1;
            }

            rleng = Math.Min(sb.Length, s_maxstr - 1);
            str = sb.ToString();
            Kingaku.Text = "\\" + str.Substring(0, rleng) + "-";

            //-------------------------------------------------------------------

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //勤怠届の確定ボタンが押されたときの処理です。

            //勤怠届印刷フォーム
            Panel5.Visible = true;
            //印刷ボタンパネル
            Panel_Print.Visible = true;

            Set_Kintai_Data();

        }

        protected void Set_Kintai_Data()
        {

            //------------------------------------------------------------
            //Varidation用のクラスをインスタンス化します。
            ShinseiClass reg = new ShinseiClass();

            //必須入力フォームをチェック
            reg.Notification_Purpose = HtmlEncode(TextBox_Notification_Purpose.ToString());

            Reg(reg);
            //------------------------------------------------------------

            //勤怠届の印刷フォームに名前と日付を代入
            lblDiligenceUser.Text = "氏名：" + SessionManager.User.M_User.name1;
            DateTime dt = DateTime.Now;
            lblDiligenceDate.Text = dt.ToShortDateString();

            //string dt1 = Label16.Text;
            //string dt2 = Label18.Text;

            //勤怠届印刷フォームに届出内容を代入
            lblDiligenceClassification1.Text = DropDownList_DetailsOfNotification.SelectedValue;
            lblDiligenceClassification2.Text = DropDownList_DetailsOfNotification.SelectedValue;

            lblDiligenceDateA1.Text = lblSelectedDateA1.Text;
            lblDiligenceDateA2.Text = " " + DropDownList_A_Time.SelectedValue;
            lblDiligenceDateB1.Text = lblSelectedDateB1.Text;
            lblDiligenceDateB2.Text = " " + DropDownList_B_Time.SelectedValue;

            int rleng = Math.Min(TextBox_Notification_Purpose.Text.Length, maxstr);
            Label_Diligence_because.Text = TextBox_Notification_Purpose.Text.Substring(0, rleng);

            rleng = Math.Min(TextBox_Notification_ps.Text.Length, maxstr);
            Label_Diligence_ps.Text = TextBox_Notification_ps.Text.Substring(0, rleng);

            //DateTime dateTime1 = Calendar1.SelectedDate;
            //DateTime dateTime2 = Calendar2.SelectedDate;
        }

        /// <summary>
        /// Buppinの入力フォームを初期化するボタンです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Buppin_Reset_Click(object sender, EventArgs e)
        {
            //初期化
            TextBox_purchaseName.Text = "";
            TextBox_classification.Text = "";
            TextBox_howMany.Text = "";
            TextBox_howMach.Text = "";
            TextBox_marketPlace.Text = "";
            TextBox_buy_purpose.Text = "";
            TextBox_ps.Text = "なし";

            //物品購入申請書印刷フォーム
            Panel4.Visible = false;
            //印刷ボタンパネル
            Panel_Print.Visible = false;

            //選択中uidを初期化
            lbluid.Text = "null";

            //データバインド（強調表示を初期化）
            BindData();
            Session.Add("args", (string)"null");

        }

        /// <summary>
        /// Deligenceの入力フォームを初期化するボタンです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Deligence_Reset_Click(object sender, EventArgs e)
        {
            DropDownList_DetailsOfNotification.SelectedValue = "出社届";
            //日時初期化
            DropDownList_A_Time.SelectedValue = "9:00";
            DropDownList_B_Time.SelectedValue = "9:00";
            lblSelectedDateA1.Text = DateTime.Now.ToShortDateString();
            lblSelectedDateA2.Text = DropDownList_A_Time.SelectedValue;
            lblSelectedDateB1.Text = DateTime.Now.ToShortDateString();
            lblSelectedDateB2.Text = DropDownList_B_Time.SelectedValue;

            //テキストボックス初期化
            TextBox_Notification_Purpose.Text = "";
            TextBox_Notification_ps.Text = "なし";

            //勤怠届印刷フォーム
            Panel5.Visible = false;
            //印刷ボタンパネル
            Panel_Print.Visible = false;

            //選択中uidを初期化
            lbluid.Text = "null";

            //データバインド（強調表示を初期化）
            BindData();
            Session.Add("args", (string)"null");
        }



        protected void Button3_Click(object sender, EventArgs e)
        {
            //立替金明細表の挿入ボタンが押されたときの処理です。

            //立替金明細表印刷フォーム
            Panel7.Visible = true;
            //印刷ボタンパネル
            Panel_Print.Visible = true;

            lblTatekaeName.Text = "氏名：" + SessionManager.User.M_User.name;
            DateTime dt = DateTime.Now;
            lblTatekaeDate.Text = dt.ToShortDateString();

            Set_Tatekae_Data();

        }

        protected void Set_Tatekae_Data()
        {

            //------------------------------------------------------------
            //Varidation用のクラスをインスタンス化します。
            ShinseiClass reg = new ShinseiClass();

            //必須入力フォームをチェック TextBox_Tatekae_Date
            reg.T_Date = HtmlEncode(TextBox_Tatekae_Date.Text.ToString());
            reg.T_W_Place = HtmlEncode(TextBox_Tatekae_WPlace.Text.ToString());
            reg.T_Use = HtmlEncode(TextBox_Tatekae_TUse.Text.ToString());
            reg.T_In = HtmlEncode(TextBox_Tatekae_TIn.Text.ToString());
            reg.T_Out = HtmlEncode(TextBox_Tatekae_TOut.Text.ToString());
            reg.T_T_Waste = HtmlEncode(TextBox_Tatekae_TWaste.Text.ToString());
            reg.T_Place = HtmlEncode(TextBox_Tatekae_Place.ToString());
            reg.T_P_Waste = HtmlEncode(TextBox_Tatekae_PWaste.ToString());
            reg.T_ps = HtmlEncode(TextBox_Tatekae_ps.ToString());
            reg.T_Teiki = HtmlEncode(TextBox_Tatekae_Teiki.ToString());

            //CheckBox_Tatekae_Receipt  直接入力

            Reg(reg);
            //------------------------------------------------------------

            CreateTatekaeTableRow();
        }


        /// <summary>
        /// Tatekae_Tableを初期化するボタンです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button4_Click(object sender, EventArgs e)
        {
            resetTatekaeTable();
        }

        protected void resetTatekaeTable() {
            //初期化
            lblTatekaeResult.Text = "";
            waste1 = 0;
            waste2 = 0;
            wasteR1 = 0;

            //定期券代
            wasteR2 = 0;
            try
            {
                wasteR2 = int.Parse(HtmlEncode(TextBox_Tatekae_Teiki.Text.Replace(",", "")));
            }
            catch
            {
                //不正な値
                wasteR2 = 0;
            }

            wasteR3 = 0;
            lblTatekae_Koutuuhi.Text = "\\" + waste1.ToString() + "-";
            lblTatekae_Shukuhakuhi.Text = "\\" + waste2.ToString() + "-";
            lblTatekae_Result1.Text = "\\" + wasteR1.ToString() + "-";
            lblTatekae_Result2.Text = "\\" + wasteR2.ToString() + "-";
            lblTatekae_Result3.Text = "\\" + wasteR3.ToString() + "-";

            Session.Add("waste1", waste1);
            Session.Add("waste2", waste2);
            Session.Add("wasteR1", wasteR1);
            Session.Add("wasteR2", wasteR2);
            Session.Add("wasteR3", wasteR3);

            //選択中uidを初期化
            lbluid.Text = "null";

            //データバインド（強調表示を初期化）
            BindData();
            Session.Add("args", (string)"null");

        }


        protected void DropDownList_Master_SelectionChanged(object sender, EventArgs e)
        {
            //申請書をドロップダウンリストで選択し。使用するフォームを決定します。
            string ddl1 = DropDownList1.SelectedValue;
            if (ddl1 == "物品購入申請")
            {
                //初期選択パネル
                Panel1.Visible = true;
                //物品購入申請書パネル
                Panel2.Visible = true;
                //勤怠パネル
                Panel3.Visible = false;
                //物品購入申請書印刷フォーム
                Panel4.Visible = false;
                //勤怠届印刷フォーム
                Panel5.Visible = false;
                //立替金明細表申請パネル
                Panel6.Visible = false;
                //立替金明細表印刷フォーム
                Panel7.Visible = false;
                //印刷ボタンパネル
                Panel_Print.Visible = false;

                name1.Text = "氏名：" + SessionManager.User.M_User.name1;

            }

            if (ddl1 == "勤怠関連申請")
            {
                //初期選択パネル
                Panel1.Visible = true;
                //物品購入申請書パネル
                Panel2.Visible = false;
                //勤怠パネル
                Panel3.Visible = true;
                //物品購入申請書印刷フォーム
                Panel4.Visible = false;
                //勤怠届印刷フォーム
                Panel5.Visible = false;
                //立替金明細表申請パネル
                Panel6.Visible = false;
                //立替金明細表印刷フォーム
                Panel7.Visible = false;
                //印刷ボタンパネル
                Panel_Print.Visible = false;

                name1.Text = "氏名：" + SessionManager.User.M_User.name1;

            }
            if (ddl1 == "立替金明細表申請")
            {
                //初期選択パネル
                Panel1.Visible = true;
                //物品購入申請書パネル
                Panel2.Visible = false;
                //勤怠パネル
                Panel3.Visible = false;
                //物品購入申請書印刷フォーム
                Panel4.Visible = false;
                //勤怠届印刷フォーム
                Panel5.Visible = false;
                //立替金明細表申請パネル
                Panel6.Visible = true;
                //立替金明細表印刷フォーム
                Panel7.Visible = true;
                //印刷ボタンパネル
                Panel_Print.Visible = true;

                name1.Text = SessionManager.User.M_User.name1;

            }
            DateTime dt = DateTime.Now;
            date.Text = dt.ToShortDateString();
            //ChangeValidate(true);

        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            //カレンダー１の値が変更されたときに実行されます
            lblSelectedDateA1.Text = Calendar1.SelectedDate.ToShortDateString();
            //lblSelectedDateA2.Text = DropDownList_A_Time.SelectedValue;
        }

        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            //カレンダー２の値が変更されたときに実行されます
            lblSelectedDateB1.Text = Calendar2.SelectedDate.ToShortDateString();
            //lblSelectedDateB2.Text = DropDownList_B_Time.SelectedValue;
        }

        protected void DropDownList_A_Time_SelectionChanged(object sender, EventArgs e)
        {
            //カレンダー１の時間の値が変更されたときに実行されます
            //lblSelectedDateA1.Text = Calendar1.SelectedDate.ToShortDateString();
            lblSelectedDateA2.Text = DropDownList_A_Time.SelectedValue;
        }
        protected void DropDownList_B_Time_SelectionChanged(object sender, EventArgs e)
        {
            //カレンダー２の時間の値が変更されたときに実行されます
            //lblSelectedDateB1.Text = Calendar2.SelectedDate.ToShortDateString();
            lblSelectedDateB2.Text = DropDownList_B_Time.SelectedValue;
        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            //印刷フォームで戻るボタンが押されたときの処理です。
            //印刷フォームをまとめて消去します。

            //初期選択パネル
            Panel1.Visible = true;
            ////物品購入申請書印刷フォーム
            Panel4.Visible = false;
            ////勤怠届印刷フォーム
            Panel5.Visible = false;
            //立替金明細表印刷フォーム
            Panel7.Visible = false;
            //印刷ボタンパネル
            Panel_Print.Visible = false;

        }
        protected void ResetButton_Click(object sender, EventArgs e)
        {
            //入力フォームで戻るボタンが押されたときの処理です。
            //初期状態に戻します。

            //初期選択パネル
            Panel1.Visible = true;
            ////物品購入申請書パネル
            Panel2.Visible = false;
            ////勤怠パネル
            Panel3.Visible = false;
            ////物品購入申請書印刷フォーム
            Panel4.Visible = false;
            ////勤怠届印刷フォーム
            Panel5.Visible = false;
            //立替金明細表申請パネル
            Panel6.Visible = false;
            //立替金明細表印刷フォーム
            Panel7.Visible = false;
            //印刷ボタンパネル
            Panel_Print.Visible = false;

        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            //立替金明細表印刷フォームを開きます。 

            //初期選択パネル
            Panel1.Visible = true;
            ////物品購入申請書パネル
            Panel2.Visible = false;
            ////勤怠パネル
            Panel3.Visible = false;
            ////物品購入申請書印刷フォーム
            Panel4.Visible = false;
            ////勤怠届印刷フォーム
            Panel5.Visible = false;
            //立替金明細表申請パネル
            Panel6.Visible = true;
            //立替金明細表印刷フォーム
            Panel7.Visible = true;
            //印刷ボタンパネル
            Panel_Print.Visible = true;

        }

        /// <summary>
        /// 立替金明細表に行を挿入します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CreateTatekaeTableRow()
        {
            //立替金明細表のテキストを印刷フォームに代入

            lblTatekaeName.Text = SessionManager.User.M_User.name1;
            DateTime dt = DateTime.Now;
            lblTatekaeDate.Text = dt.ToShortDateString();

            //----------------------------------

            //初期化
            string str = "";
            waste1 = int.Parse(Session["waste1"].ToString());
            waste2 = int.Parse(Session["waste2"].ToString());
            wasteR1 = int.Parse(Session["wasteR1"].ToString());
            wasteR2 = int.Parse(Session["wasteR2"].ToString());
            wasteR3 = int.Parse(Session["wasteR3"].ToString());

            /*
            string str1 = HtmlEncode(TextBox_Tatekae_TWaste.Text.Replace(",", ""));
            try
            {
                waste1 += int.Parse(str1);
            }
            catch
            {
                //不正な値
                lblTatekae_Koutuuhi.Text = "Error";
                return;
            }

            string str2 = HtmlEncode(TextBox_Tatekae_PWaste.Text.Replace(",", ""));
            try
            {
                waste2 += int.Parse(str2);
            }
            catch
            {
                //不正な値
                lblTatekae_Koutuuhi.Text = "Error";
                return;
            }
            */


            try
            {
                StringBuilder str1 = new StringBuilder(HtmlEncode(TextBox_Tatekae_TWaste.Text).ToString());
                str1.Replace("-", "");
                str1.Replace(",", "");
                str1.Replace("\\", "");
                waste1 += int.Parse(str1.ToString());
            }
            catch
            {
                //不正な値
                lblTatekae_Koutuuhi.Text = "Error";
                return;
            }

            try
            {
                StringBuilder str2 = new StringBuilder(HtmlEncode(TextBox_Tatekae_PWaste.Text).ToString());
                str2.Replace("-", "");
                str2.Replace(",", "");
                str2.Replace("\\", "");
                waste2 += int.Parse(str2.ToString());
            }
            catch
            {
                //不正な値
                lblTatekae_Koutuuhi.Text = "Error";
                return;
            }



            //定期券代
            try
            {
                wasteR2 = int.Parse(HtmlEncode(TextBox_Tatekae_Teiki.Text.Replace(",", "")));
                lblTatekae_Result2.Text = "\\" + wasteR2.ToString() + "-";
            }
            catch
            {
                //不正な値
                lblTatekae_Result2.Text = "Error";
                return;
            }

            //交通費＋宿泊費
            wasteR1 = waste1 + waste2;
            //立替金総合計
            wasteR3 = wasteR1 + wasteR2;

            //初期化
            StringBuilder sb = new StringBuilder();
            int i = 0;
            int lp = 0;

            //建て替え日
            str = TextBox_Tatekae_Date.Text;
            str = HtmlEncode(str);
            //----------------------
            sb = new StringBuilder(str);
            sb.Replace("0", "０");
            sb.Replace("1", "１");
            sb.Replace("2", "２");
            sb.Replace("3", "３");
            sb.Replace("4", "４");
            sb.Replace("5", "５");
            sb.Replace("6", "６");
            sb.Replace("7", "７");
            sb.Replace("8", "８");
            sb.Replace("9", "９");
            sb.Replace("/", "月");
            sb.Append("日");
            sb.Replace("日日", "日");
            str = sb.ToString();
            //----------------------
            int rleng = Math.Min(str.Length, s_maxstr);
            string cell0 = str.Substring(0, rleng);

            str = TextBox_Tatekae_WPlace.Text;
            str = HtmlEncode(str);
            rleng = Math.Min(str.Length, s_maxstr);
            string cell1 = str.Substring(0, rleng);

            str = TextBox_Tatekae_TUse.Text;
            str = HtmlEncode(str);
            rleng = Math.Min(str.Length, s_maxstr);
            string cell2 = str.Substring(0, rleng);

            str = TextBox_Tatekae_TIn.Text;
            str = HtmlEncode(str);
            rleng = Math.Min(str.Length, s_maxstr);
            string cell3 = str.Substring(0, rleng);

            str = TextBox_Tatekae_TOut.Text;
            str = HtmlEncode(str);
            rleng = Math.Min(str.Length, s_maxstr);
            string cell4 = str.Substring(0, rleng);

            str = TextBox_Tatekae_TWaste.Text;
            str = HtmlEncode(str);
            //--------------------------
            //区切り記号の追加
            sb = new StringBuilder(str);
            sb.Replace(",","");
            sb.Replace("-","");
            sb.Replace("\\","");
            lp = 0;
            for (i = 3; i < sb.Length; i += 3)
            {
                if ((i + lp) >= sb.Length)
                {
                    break;
                }
                sb.Insert(sb.Length - (i + lp), ",");
                lp += 1;
            }
            sb.Append("-");
            //--------------------------
            rleng = Math.Min(sb.Length, s_maxstr - 1); //"\\"と"-"のぶんだけ-1
            string cell5 = "\\" + sb.ToString().Substring(0, rleng);

            str = TextBox_Tatekae_Place.Text;
            str = HtmlEncode(str);
            rleng = Math.Min(str.Length, s_maxstr);
            string cell6 = str.Substring(0, rleng);

            str = TextBox_Tatekae_PWaste.Text;
            str = HtmlEncode(str);
            //--------------------------
            //区切り記号の追加
            sb = new StringBuilder(str);
            sb.Replace(",", "");
            sb.Replace("-", "");
            sb.Replace("\\", "");
            lp = 0;
            for (i = 3; i < sb.Length; i += 3)
            {
                if ((i + lp) >= sb.Length)
                {
                    break;
                }
                sb.Insert(sb.Length - (i + lp), ",");
                    lp += 1;
            }
            //--------------------------
            sb.Append("-");
            rleng = Math.Min(sb.Length, s_maxstr - 1); //"\\"と"-"のぶんだけ-1
            string cell7 = "\\" + sb.ToString().Substring(0, rleng);

            string cell8 = "";
            if (CheckBox_Tatekae_Receipt.Checked)
            {
                cell8 = "〇";
            }
            else
            {
                cell8 = "―";
            }


            str = TextBox_Tatekae_ps.Text;
            str = HtmlEncode(str);
            rleng = Math.Min(str.Length, maxstr);
            string cell9 = str.Substring(0, rleng);

            //----------------------------------
            lblTatekaeResult.Text += "<tr class=\"naiyou-c\"><td class=\"naiyou-c\">" + cell0 + "</td><td>" + cell1 + "</td><td class=\"naiyou-c\">" + cell2 + "</td><td>" + cell3 + "</td><td class=\"naiyou-c\">" + cell4 + "</td><td class=\"naiyou-c\"><a name = \"w1\">" + cell5 + "</a><a name = \"endw1\"></a></td><td class=\"naiyou-c\">" + cell6 + "</td><td><a name = \"w2\">" + cell7 + "</a><a name = \"endw2\"></a></td><td class=\"naiyou-c\">" + cell8 + "</td><td class=\"naiyou-c\">" + cell9 + "</td></tr>";
            //----------------------------------


            //セッション変数に保存（wasteは自動で初期化されてしまうから）
            Session.Add("waste1", waste1);
            Session.Add("waste2", waste2);
            Session.Add("wasteR1", wasteR1);
            Session.Add("wasteR2", wasteR2);
            Session.Add("wasteR3", wasteR3);

            //文字列をラベルに代入
            SetTatekaeString();

        }




        protected void Change_Text_T_Teiki(object sender, EventArgs e)
        {

            SetTatekaeDate();

            //------------------------------------------------------------
            //Varidation用のクラスをインスタンス化します。
            ShinseiClass reg = new ShinseiClass();

            //必須入力フォームをチェック TextBox_Tatekae_Date
            reg.T_Teiki = HtmlEncode(TextBox_Tatekae_Teiki.ToString());

            //CheckBox_Tatekae_Receipt  直接入力

            Reg(reg);
            //------------------------------------------------------------

            //立替金明細表申請パネル
            Panel6.Visible = true;
            //立替金明細表印刷フォーム
            Panel7.Visible = true;
            //印刷ボタンパネル
            Panel_Print.Visible = true;

            //----------------------------------

            //初期化
            /*
            waste1 = int.Parse(Session["waste1"].ToString());
            waste2 = int.Parse(Session["waste2"].ToString());
            wasteR1 = int.Parse(Session["wasteR1"].ToString());
            wasteR2 = int.Parse(Session["wasteR2"].ToString());
            wasteR3 = int.Parse(Session["wasteR3"].ToString());
            */

            //印刷フォームからロード
            try
            {
                StringBuilder str1 = new StringBuilder(HtmlEncode(lblTatekae_Koutuuhi.Text).ToString());
                str1.Replace("-", "");
                str1.Replace(",", "");
                str1.Replace("\\", "");
                waste1 = int.Parse(str1.ToString());
            }
            catch
            {
                //不正な値
                lblTatekae_Koutuuhi.Text = "Error";
                return;
            }

            try
            {
                StringBuilder str2 = new StringBuilder(HtmlEncode(lblTatekae_Shukuhakuhi.Text).ToString());
                str2.Replace("-", "");
                str2.Replace(",", "");
                str2.Replace("\\", "");
                waste2 = int.Parse(str2.ToString());
            }
            catch
            {
                //不正な値
                lblTatekae_Koutuuhi.Text = "Error";
                return;
            }

            //定期券代を反映
            try
            {
                StringBuilder str = new StringBuilder(HtmlEncode(TextBox_Tatekae_Teiki.Text).ToString());
                str.Replace("-", "");
                str.Replace(",", "");
                str.Replace("\\", "");
                wasteR2 = int.Parse(str.ToString());
                lblTatekae_Result2.Text = "\\" + wasteR2.ToString() + "-";

                //--------------------------------------------------------------

                //交通費＋宿泊費
                wasteR1 = waste1 + waste2;
                //立替金総合計
                wasteR3 = wasteR1 + wasteR2;

                //セッション変数に保存（wasteは自動で初期化されてしまうから）
                Session.Add("waste1", waste1);
                Session.Add("waste2", waste2);
                Session.Add("wasteR1", wasteR1);
                Session.Add("wasteR2", wasteR2);
                Session.Add("wasteR3", wasteR3);

                SetTatekaeString();
            }
            catch
            {
                //不正な値
                lblTatekae_Result2.Text = "Error";
                return;
            }
        }

        protected void Button_Undo(object sender, EventArgs e)
        {

            //セッション変数からロード
            waste1 = int.Parse(Session["waste1"].ToString());
            waste2 = int.Parse(Session["waste2"].ToString());
            wasteR1 = int.Parse(Session["wasteR1"].ToString());
            wasteR2 = int.Parse(Session["wasteR2"].ToString());
            wasteR3 = int.Parse(Session["wasteR3"].ToString());

            string lines = lblTatekaeResult.Text;

            int iws1 = lines.LastIndexOf("<a name = \"w1\">");
            int iwe1 = lines.LastIndexOf("</a><a name = \"endw1\"></a>");
            int iws2 = lines.LastIndexOf("<a name = \"w2\">");
            int iwe2 = lines.LastIndexOf("</a><a name = \"endw2\"></a>");


            if (iws1 >= 0 && iws2 >= 0 && iwe1 >= 0 && iwe2 >= 0) {

                //行った先から手前を引く
                string rw1 = lines.Substring(iws1, iwe1 - iws1);
                string rw2 = lines.Substring(iws2, iwe2 - iws2);

                rw1 = rw1.Replace(("<a name = \"w1\">"), "");
                rw1 = rw1.Replace("-", "");
                rw1 = rw1.Replace(",", "");
                rw1 = rw1.Replace("\\", "");

                rw2 = rw2.Replace(("<a name = \"w2\">"), "");
                rw2 = rw2.Replace("-", "");
                rw2 = rw2.Replace(",", "");
                rw2 = rw2.Replace("\\", "");


                //ロールバック
                waste1 -= int.Parse(rw1);
                waste2 -= int.Parse(rw2);

                //交通費＋宿泊費
                wasteR1 = waste1 + waste2;
                //立替金総合計
                wasteR3 = wasteR1 + wasteR2;

                //セッション変数に代入
                Session.Add("waste1", waste1);
                Session.Add("waste2", waste2);
                Session.Add("wasteR1", wasteR1);
                Session.Add("wasteR2", wasteR2);
                Session.Add("wasteR3", wasteR3);

                //文字列をラベルに代入
                SetTatekaeString();


            }



            //最後にある<trから末尾まで切り出し
            int lasttr = lines.LastIndexOf("<tr");
            if (lasttr >= 0) {
                lines = lblTatekaeResult.Text.Remove(lasttr);
                lblTatekaeResult.Text = lines;

            }
            else
            {
                //対象文字列<trがありません！
            }
        }




        protected void SetTatekaeString()
        {

            //init
            int i = 0;
            int lp = 0;

            //セッション変数からロード
            waste1 = int.Parse(Session["waste1"].ToString());
            waste2 = int.Parse(Session["waste2"].ToString());
            wasteR1 = int.Parse(Session["wasteR1"].ToString());
            wasteR2 = int.Parse(Session["wasteR2"].ToString());
            wasteR3 = int.Parse(Session["wasteR3"].ToString());

            //using System.Text; 高速文字列処理　中身を直接書き換える
            StringBuilder sbw1 = new StringBuilder(waste1.ToString());
            StringBuilder sbw2 = new StringBuilder(waste2.ToString());
            StringBuilder sbwr1 = new StringBuilder(wasteR1.ToString());
            StringBuilder sbwr2 = new StringBuilder(wasteR2.ToString());
            StringBuilder sbwr3 = new StringBuilder(wasteR3.ToString());


            //カンマ区切りを追加
            lp = 0;
            for (i = 3; i < sbw1.Length; i += 3)
            {
                if ((i + lp) >= sbw1.Length)
                {
                    break;
                }
                sbw1.Insert(sbw1.Length - (i + lp), ",");
                lp += 1;
            }

            lp = 0;
            for (i = 3; i < sbw2.Length; i += 3)
            {
                if ((i + lp) >= sbw2.Length)
                {
                    break;
                }
                sbw2.Insert(sbw2.Length - (i + lp), ",");
                lp += 1;
            }


            lp = 0;
            for (i = 3; i < sbwr1.Length; i += 3)
            {
                if ((i + lp) >= sbwr1.Length)
                {
                    break;
                }
                sbwr1.Insert(sbwr1.Length - (i + lp), ",");
                lp += 1;
            }

            lp = 0;
            for (i = 3; i < sbwr2.Length; i += 3)
            {
                if ((i + lp) >= sbwr2.Length)
                {
                    break;
                }
                sbwr2.Insert(sbwr2.Length - (i + lp), ",");
                lp += 1;
            }

            lp = 0;
            for (i = 3; i < sbwr3.Length; i += 3)
            {
                if ((i + lp) >= sbwr3.Length)
                {
                    break;
                }
                sbwr3.Insert(sbwr3.Length - (i + lp), ",");
                lp += 1;
            }

            //文字列をラベルに代入
            lblTatekae_Koutuuhi.Text = "\\" + sbw1 + "-";
            lblTatekae_Shukuhakuhi.Text = "\\" + sbw2 + "-";
            lblTatekae_Result1.Text = "\\" + sbwr1 + "-";
            lblTatekae_Result2.Text = "\\" + sbwr2 + "-";
            lblTatekae_Result3.Text = "\\" + sbwr3 + "-";

        }

        protected void RemoveShinseiRow(int args)
        {

            //idをロード
            //String isbn_key = (String)GridView1.DataKeys[args].Value;
            String isbn_name = GridView1.Rows[args].Cells[0].Text.Trim();

            // クリックされた[args]行の左から2番目の列[0-nで数える]のセルにある「テキスト」を取得
            //uidをロード
            String isbn_uid = GridView1.Rows[args].Cells[1].Text.Trim();

            // クリックされた[args]行の左から3番目の列[0-nで数える]のセルにある「テキスト」を取得
            //申請種別をロード
            String isbn_kind = GridView1.Rows[args].Cells[3].Text.Trim();

            //個別テーブルから削除
            if (isbn_kind == "物品購入申請")
            {
                ShinseiLog.DeleteT_Shinsei_A_BuyRow(Global.GetConnection(), isbn_name, isbn_uid);
            }
            else if (isbn_kind == "勤怠関連申請")
            {
                ShinseiLog.DeleteT_Shinsei_B_Diligence(Global.GetConnection(), isbn_name, isbn_uid);
            }
            else if (isbn_kind == "立替金明細表申請")
            {
                ShinseiLog.DeleteT_Shinsei_C_Tatekae(Global.GetConnection(), isbn_name, isbn_uid);
            }

            //一覧から削除
            ShinseiLog.DeleteT_Shinsei_MainRow(Global.GetConnection(), isbn_name, isbn_uid);
            lbluid.Text = "null";

            //データバインド
            BindData();

            //レスポンスリダイレクト
            //Response.Redirect("Shinsei.aspx");
        }

        protected void ReformShinseiRow(int args)
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
            GridView1.Rows[args].BackColor = System.Drawing.Color.AliceBlue;

            //idをロード
            //String isbn_key = (String)GridView1.DataKeys[args].Value;
            string isbn_name = GridView1.Rows[args].Cells[0].Text.Trim();

            // クリックされた[args]行の左から2番目の列[0-nで数える]のセルにある「テキスト」を取得
            //uidをロード
            string isbn_uid = GridView1.Rows[args].Cells[1].Text.Trim();
            lbluid.Text = isbn_uid;

            // クリックされた[args]行の左から2番目の列[0-nで数える]のセルにある「テキスト」を取得
            //name1をロード（いらない）
            //string isbn_name1 = GridView1.Rows[args].Cells[2].Text.Trim();
            string isbn_name1 = SessionManager.User.M_User.name1;

            // クリックされた[args]行の左から3番目の列[0-nで数える]のセルにある「テキスト」を取得
            //申請種別をロード
            string isbn_kind = GridView1.Rows[args].Cells[3].Text.Trim();

            // クリックされた[args]行の左から4番目の列[0-nで数える]のセルにある「テキスト」を取得
            //作成日付をロード
            //String isbn_date = GridView1.Rows[args].Cells[4].Text.Trim();

            // クリックされた[args]行の左から4番目の列[0-nで数える]のセルにある「テキスト」を取得
            //最終更新日をロード
            string isbn_date = GridView1.Rows[args].Cells[5].Text.Trim();
            DateTime datetime = DateTime.Parse(isbn_date);
            isbn_date = datetime.ToShortDateString();

            if (isbn_kind == "物品購入申請")
            {
                name1.Text = isbn_name1;
                date.Text = isbn_date;

                //DataTableを参照
                DATASET.DataSet.T_Shinsei_A_BuyDataTable dt = ShinseiLog.GetT_Shinsei_A_BuyRow(Global.GetConnection(), isbn_name, isbn_uid);

                //入力フォームに代入
                TextBox_purchaseName.Text = dt[0].A_BuyItem;
                TextBox_classification.Text = dt[0].A_BuyItem;
                TextBox_howMany.Text = dt[0].A_BuyHowMany;
                TextBox_howMach.Text = dt[0].A_BuyHowMach;
                TextBox_marketPlace.Text = dt[0].A_BuyPlace;
                TextBox_buy_purpose.Text = dt[0].A_Buy_Because;
                TextBox_ps.Text = dt[0].A_Buy_ps;

                //書き換え
                StringBuilder str = new StringBuilder(HtmlEncode(TextBox_howMany.Text).ToString());
                str.Replace("点", "");
                TextBox_howMany.Text = str.ToString();

                str = new StringBuilder(HtmlEncode(TextBox_howMach.Text).ToString());
                str.Replace("-", "");
                str.Replace(",", "");
                str.Replace("\\", "");
                TextBox_howMach.Text = str.ToString();


                //印刷ビューに代入
                Konyu.Text = dt[0].A_BuyItem;
                Syubetsu.Text = dt[0].A_BuyItem;
                Suryo.Text = dt[0].A_BuyHowMany;
                Kingaku.Text = dt[0].A_BuyHowMach;
                KonyuSaki.Text = dt[0].A_BuyPlace;
                Label_Riyuu.Text = dt[0].A_Buy_Because;
                Label_Bikou.Text = dt[0].A_Buy_ps;


                //初期選択パネル
                Panel1.Visible = true;
                //物品購入申請書パネル
                Panel2.Visible = true;
                //勤怠パネル
                Panel3.Visible = false;
                //物品購入申請書印刷フォーム
                Panel4.Visible = true;
                //勤怠届印刷フォーム
                Panel5.Visible = false;
                //立替金明細表申請パネル
                Panel6.Visible = false;
                //立替金明細表印刷フォーム
                Panel7.Visible = false;
                //印刷ボタンパネル
                Panel_Print.Visible = true;

                //SaveMessageを消去
                lbl_SaveResult1.Text = "";

            }
            else if (isbn_kind == "勤怠関連申請")
            {
                lblDiligenceUser.Text = isbn_name1;
                lblDiligenceDate.Text = isbn_date;

                //DataTableを参照
                DATASET.DataSet.T_Shinsei_B_DiligenceDataTable dt = ShinseiLog.GetT_Shinsei_B_DiligenceRow(Global.GetConnection(), isbn_name, isbn_uid);

                //入力フォームに代入
                DropDownList_DetailsOfNotification.Text = dt[0].B_DiligenceClassification.Trim();
                lblSelectedDateA1.Text = dt[0].B_DiligenceDateA1.Trim();
                lblSelectedDateA2.Text = dt[0].B_DiligenceDateA2.Trim();
                lblSelectedDateB1.Text = dt[0].B_DiligenceDateB1.Trim();
                lblSelectedDateB2.Text = dt[0].B_DiligenceDateB2.Trim();
                TextBox_Notification_Purpose.Text = dt[0].B_Diligence_Because;
                TextBox_Notification_ps.Text = dt[0].B_Diligence_ps;

                //印刷ビューに代入
                lblDiligenceClassification1.Text = dt[0].B_DiligenceClassification.Trim();
                lblDiligenceClassification2.Text = dt[0].B_DiligenceClassification.Trim();
                lblDiligenceDateA1.Text = dt[0].B_DiligenceDateA1.Trim();
                lblDiligenceDateA2.Text = dt[0].B_DiligenceDateA2.Trim();
                lblDiligenceDateB1.Text = dt[0].B_DiligenceDateB1.Trim();
                lblDiligenceDateB2.Text = dt[0].B_DiligenceDateB2.Trim();
                Label_Diligence_because.Text = dt[0].B_Diligence_Because;
                Label_Diligence_ps.Text = dt[0].B_Diligence_ps;

                //初期選択パネル
                Panel1.Visible = true;
                //物品購入申請書パネル
                Panel2.Visible = false;
                //勤怠パネル
                Panel3.Visible = true;
                //物品購入申請書印刷フォーム
                Panel4.Visible = false;
                //勤怠届印刷フォーム
                Panel5.Visible = true;
                //立替金明細表申請パネル
                Panel6.Visible = false;
                //立替金明細表印刷フォーム
                Panel7.Visible = false;
                //印刷ボタンパネル
                Panel_Print.Visible = true;

                //SaveMessageを消去
                lbl_SaveResult2.Text = "";

            }
            else if (isbn_kind == "立替金明細表申請")
            {
                lblTatekaeName.Text = isbn_name1;
                lblTatekaeDate.Text = isbn_date;

                //DataTableを参照
                DATASET.DataSet.T_Shinsei_C_TatekaeDataTable dt = ShinseiLog.GetT_Shinsei_C_TatekaeRow(Global.GetConnection(), isbn_name, isbn_uid);

                //入力フォーム
                TextBox_Tatekae_Teiki.Text = dt[0].C_Tatekae_Result2;

                //書き換え
                StringBuilder str = new StringBuilder(HtmlEncode(TextBox_Tatekae_Teiki.Text).ToString());
                str.Replace("-", "");
                str.Replace(",", "");
                str.Replace("\\", "");
                TextBox_Tatekae_Teiki.Text = str.ToString();

                //印刷フォーム
                lblTatekaeResult.Text = dt[0].C_Tatekae_Result_Main.Trim();
                lblTatekae_Koutuuhi.Text = dt[0].C_Tatekae_TWaste.Trim();
                lblTatekae_Shukuhakuhi.Text = dt[0].C_Tatekae_PWaste.Trim();
                lblTatekae_Result1.Text = dt[0].C_Tatekae_Result1.Trim();
                lblTatekae_Result2.Text = dt[0].C_Tatekae_Result2.Trim();
                lblTatekae_Result3.Text = dt[0].C_Tatekae_Result3.Trim();

                //初期選択パネル
                Panel1.Visible = true;
                //物品購入申請書パネル
                Panel2.Visible = false;
                //勤怠パネル
                Panel3.Visible = false;
                //物品購入申請書印刷フォーム
                Panel4.Visible = false;
                //勤怠届印刷フォーム
                Panel5.Visible = false;
                //立替金明細表申請パネル
                Panel6.Visible = true;
                //立替金明細表印刷フォーム
                Panel7.Visible = true;
                //印刷ボタンパネル
                Panel_Print.Visible = true;

                //SaveMessageを消去
                lbl_SaveResult3.Text = "";

            }
        }


        //GridViewのRowCommand属性に参照可能なメソッドを入力すると↓が自動生成されます。
        //GridViewが読み込まれたときに実行されます。
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

                if (CheckBox_is_del_pop.Checked)
                {

                    //uidをロード
                    string isbn_uid = GridView1.Rows[args].Cells[1].Text.Trim();
                    lbldeluid.Text = isbn_uid;

                    //消去確認ポップアップ
                    Panel0.Visible = false;
                    Panel00.Visible = false;
                    //削除確認パネル
                    Panel_del_pop.Visible = true;

                    Session.Add("del_args", args);
                    return;
                }

                //セッション変数argsを初期化
                Session.Add("args", (string)"null");

                RemoveShinseiRow(args);

                // コマンド名が“Reform”の場合にのみ処理（修正ボタン）
            }
            else if(e.CommandName == "Reform")
            {
                //コマンドの引数を取得
                int args = Int32.Parse(e.CommandArgument.ToString());

                //ロードのためにテーブルには用いるデータをバインドし、Visible=trueにしている必要がある。falseでも配列int[]は数える。
                //【重要】ReadOnly属性がついていないと読み込みできない。

                ReformShinseiRow(args);

            }
            return; //grid_RowCommand
        }

        protected void SaveButton_Click_1(object sender, EventArgs e)
        {
            //Set 物品購入申請
            //印刷フォームのテキストをT_ShinseiDBに代入

            Set_Konyu_Data();


            //UUID生成
            string uid = Guid.NewGuid().ToString();
            //ダブりなしまで無限ループ
            for (; ; )
            {
                if (ShinseiLog.GetT_Shinsei_Main_isuid(Global.GetConnection(), uid))
                {
                    //true ダブりあり
                    //強制終了
                    lbl_SaveResult1.Text = "Save Failed E1: " + DateTime.Now;
                    return;

                }
                else
                {
                    //false ダブりなし
                    break;
                }

            }

            //Insert
            ShinseiLog.SetT_Shinsei_A_BuyInsert(Global.GetConnection(), SessionManager.User.M_User.id.Trim(), uid, Konyu.Text, Syubetsu.Text, Suryo.Text, Kingaku.Text, KonyuSaki.Text, Label_Riyuu.Text, Label_Bikou.Text);
            ShinseiLog.SetT_Shinsei_MainInsert(Global.GetConnection(), SessionManager.User.M_User.id.Trim(), uid, SessionManager.User.M_User.name1.Trim(), "物品購入申請");
            lbluid.Text = uid;
            lbl_SaveResult1.Text = "New Save: " + DateTime.Now;

            //データバインド
            BindData();

        }

        protected void SaveButton_Click_2(object sender, EventArgs e)
        {
            //Set 勤怠関連申請
            //印刷フォームのテキストをT_ShinseiDBに代入

            Set_Kintai_Data();


            //UUID生成
            string uid = Guid.NewGuid().ToString();
            //ダブりなしまで無限ループ
            for (; ; )
            {
                if (ShinseiLog.GetT_Shinsei_Main_isuid(Global.GetConnection(), uid))
                {
                    //true ダブりあり
                    //強制終了
                    lbl_SaveResult2.Text = "Save Failed E1: " + DateTime.Now;
                    return;

                }
                else
                {
                    //false ダブりなし
                    break;
                }

            }

            //Insert
            ShinseiLog.SetT_Shinsei_B_DiligenceInsert(Global.GetConnection(), SessionManager.User.M_User.id.Trim(), uid, lblDiligenceClassification1.Text, lblDiligenceDateA1.Text, lblDiligenceDateA2.Text, lblDiligenceDateB1.Text, lblDiligenceDateB2.Text, Label_Diligence_because.Text, Label_Diligence_ps.Text);
            ShinseiLog.SetT_Shinsei_MainInsert(Global.GetConnection(), SessionManager.User.M_User.id.Trim(), uid, SessionManager.User.M_User.name1.Trim(), "勤怠関連申請");
            lbluid.Text = uid;
            lbl_SaveResult2.Text = "New Save: " + DateTime.Now;

            //データバインド
            BindData();

        }

        protected void SaveButton_Click_3(object sender, EventArgs e)
        {
            //Set 立替金明細表申請
            SetTatekaeDate();

            //UUID生成
            string uid = Guid.NewGuid().ToString();
            //ダブりなしまで無限ループ
            for (; ; )
            {
                if (ShinseiLog.GetT_Shinsei_Main_isuid(Global.GetConnection(), uid))
                {
                    //true ダブりあり
                    //強制終了
                    lbl_SaveResult3.Text = "Save Failed E1: " + DateTime.Now;
                    return;

                }
                else
                {
                    //false ダブりなし
                    break;
                }

            }

            //Insert
            ShinseiLog.SetT_Shinsei_C_TatekaeInsert(Global.GetConnection(), SessionManager.User.M_User.id.Trim(), uid, lblTatekaeResult.Text, lblTatekae_Koutuuhi.Text, lblTatekae_Shukuhakuhi.Text, lblTatekae_Result1.Text, lblTatekae_Result2.Text, lblTatekae_Result3.Text);
            ShinseiLog.SetT_Shinsei_MainInsert(Global.GetConnection(), SessionManager.User.M_User.id.Trim(), uid, SessionManager.User.M_User.name1.Trim(), "立替金明細表申請");
            lbluid.Text = uid;
            lbl_SaveResult3.Text = "New Save: " + DateTime.Now;

            //データバインド
            BindData();

        }

        protected void SaveAsButton_Click_1(object sender, EventArgs e)
        {
            //Set 物品購入申請
            //印刷フォームのテキストをT_ShinseiDBに代入
            /*
            //name1.Text
            date.Text
            
            Konyu.Text
            Syubetsu.Text
            Suryo.Text
            Kingaku.Text
            KonyuSaki.Text
            Label_Riyuu.Text
            Label_Bikou.Text
            */
            Set_Konyu_Data();

            if (lbluid.Text == "null")
            {
                //UUID生成
                string uid = Guid.NewGuid().ToString();
                //ダブりなしまで無限ループ
                for (; ; )
                {
                    if (ShinseiLog.GetT_Shinsei_Main_isuid(Global.GetConnection(), uid))
                    {
                        //true ダブりあり
                        //強制終了
                        lbl_SaveResult1.Text = "Save Failed E1: " + DateTime.Now;
                        return;

                    }
                    else
                    {
                        //false ダブりなし
                        lbl_SaveResult1.Text = "New Save: " + DateTime.Now;
                        break;
                    }

                }

                //Insert
                ShinseiLog.SetT_Shinsei_A_BuyInsert(Global.GetConnection(), SessionManager.User.M_User.id.Trim(), uid, Konyu.Text, Syubetsu.Text, Suryo.Text, Kingaku.Text, KonyuSaki.Text, Label_Riyuu.Text, Label_Bikou.Text);
                ShinseiLog.SetT_Shinsei_MainInsert(Global.GetConnection(), SessionManager.User.M_User.id.Trim(), uid, SessionManager.User.M_User.name1.Trim(), "物品購入申請");
                lbluid.Text = uid;

                //データバインド
                BindData();

            }
            else {

                //UUID取得
                string uid = lbluid.Text;

                DATASET.DataSet.T_Shinsei_A_BuyDataTable dt = ShinseiLog.GetT_Shinsei_A_BuyRow(Global.GetConnection(), SessionManager.User.M_User.id, uid);
                if (dt != null)
                {
                    //Update
                    ShinseiLog.SetT_Shinsei_A_BuyUpdate(Global.GetConnection(), SessionManager.User.M_User.id, uid.Trim(), Konyu.Text, Syubetsu.Text, Suryo.Text, Kingaku.Text, KonyuSaki.Text, Label_Riyuu.Text, Label_Bikou.Text);
                    ShinseiLog.SetT_Shinsei_MainUpdate(Global.GetConnection(), SessionManager.User.M_User.id.Trim(), uid);
                    lbl_SaveResult1.Text = "Save As: " + DateTime.Now;

                    //データバインド
                    BindData();

                }
                else
                {
                    //UUIDエラー
                    lbl_SaveResult1.Text = "Save Failed E2: " + DateTime.Now;
                    lbluid.Text = "null";
                }



            }

        }

        protected void SaveAsButton_Click_2(object sender, EventArgs e)
        {
            //Set 勤怠関連申請
            //印刷フォームのテキストをT_ShinseiDBに代入
            /*
            //lblDiligenceUser.Text
            lblDiligenceDate.Text

            lblDiligenceClassification1.Text
            lblDiligenceClassification2.Text

            lblDiligenceDateA1.Text
            lblDiligenceDateA2.Text
            lblDiligenceDateB1.Text
            lblDiligenceDateB2.Text
            Label_Diligence_because.Text
            Label_Diligence_ps.Text
            */
            Set_Kintai_Data();


            if (lbluid.Text == "null")
            {
                //UUID生成
                string uid = Guid.NewGuid().ToString();
                //ダブりなしまで無限ループ
                for (; ; )
                {
                    if (ShinseiLog.GetT_Shinsei_Main_isuid(Global.GetConnection(), uid))
                    {
                        //true ダブりあり
                        //強制終了
                        lbl_SaveResult2.Text = "Save Failed E1: " + DateTime.Now;
                        return;

                    }
                    else
                    {
                        //false ダブりなし
                        lbl_SaveResult2.Text = "New Save: " + DateTime.Now;
                        break;
                    }

                }

                //Insert
                ShinseiLog.SetT_Shinsei_B_DiligenceInsert(Global.GetConnection(), SessionManager.User.M_User.id.Trim(), uid, lblDiligenceClassification1.Text, lblDiligenceDateA1.Text, lblDiligenceDateA2.Text, lblDiligenceDateB1.Text, lblDiligenceDateB2.Text, Label_Diligence_because.Text, Label_Diligence_ps.Text);
                ShinseiLog.SetT_Shinsei_MainInsert(Global.GetConnection(), SessionManager.User.M_User.id.Trim(), uid, SessionManager.User.M_User.name1.Trim(), "勤怠関連申請");
                lbluid.Text = uid;

                //データバインド
                BindData();

            }
            else {

                //UUID取得
                string uid = lbluid.Text;

                DATASET.DataSet.T_Shinsei_B_DiligenceDataTable dt = ShinseiLog.GetT_Shinsei_B_DiligenceRow(Global.GetConnection(), SessionManager.User.M_User.id, uid);
                if (dt != null)
                {
                    //Update
                    ShinseiLog.SetT_Shinsei_B_DiligenceUpdate(Global.GetConnection(), SessionManager.User.M_User.id.Trim(), uid, lblDiligenceClassification1.Text, lblDiligenceDateA1.Text, lblDiligenceDateA2.Text, lblDiligenceDateB1.Text, lblDiligenceDateB2.Text, Label_Diligence_because.Text, Label_Diligence_ps.Text);
                    ShinseiLog.SetT_Shinsei_MainUpdate(Global.GetConnection(), SessionManager.User.M_User.id.Trim(), uid);
                    lbl_SaveResult2.Text = "Save As: " + DateTime.Now;

                    //データバインド
                    BindData();

                }
                else
                {
                    //UUIDエラー
                    lbl_SaveResult2.Text = "Save Failed E2: " + DateTime.Now;
                    lbluid.Text = "null";
                }


            }

        }

        protected void SaveAsButton_Click_3(object sender, EventArgs e)
        {
            //Set 立替金明細表申請
            SetTatekaeDate();

            //印刷フォームのテキストをT_ShinseiDBに代入
            /*
            //lblTatekaeName.Text
            lblTatekaeDate.Text

            lblTatekaeResult.Text

            lblTatekae_Koutuuhi.Text
            lblTatekae_Shukuhakuhi.Text
            lblTatekae_Result1.Text
            lblTatekae_Result2.Text
            lblTatekae_Result3.Text
            */

            //SetTatekaeString();

            if (lbluid.Text == "null")
            {

                //UUID生成
                string uid = Guid.NewGuid().ToString();
                //ダブりなしまで無限ループ
                for (; ; )
                {
                    if (ShinseiLog.GetT_Shinsei_Main_isuid(Global.GetConnection(), uid))
                    {
                        //true ダブりあり
                        //強制終了
                        lbl_SaveResult3.Text = "Save Failed E1: " + DateTime.Now;
                        return;

                    }
                    else
                    {
                        //false ダブりなし
                        lbl_SaveResult3.Text = "New Save: " + DateTime.Now;
                        break;
                    }

                }

                //Insert
                ShinseiLog.SetT_Shinsei_C_TatekaeInsert(Global.GetConnection(), SessionManager.User.M_User.id.Trim(), uid, lblTatekaeResult.Text, lblTatekae_Koutuuhi.Text, lblTatekae_Shukuhakuhi.Text, lblTatekae_Result1.Text, lblTatekae_Result2.Text, lblTatekae_Result3.Text);
                ShinseiLog.SetT_Shinsei_MainInsert(Global.GetConnection(), SessionManager.User.M_User.id, uid.Trim(), SessionManager.User.M_User.name1.Trim(), "立替金明細表申請");
                lbluid.Text = uid;

                //データバインド
                BindData();

            }
            else {

                //UUID取得
                string uid = lbluid.Text;


                DATASET.DataSet.T_Shinsei_C_TatekaeDataTable dt = ShinseiLog.GetT_Shinsei_C_TatekaeRow(Global.GetConnection(), SessionManager.User.M_User.id, uid);
                if (dt != null) {

                    //Update
                    ShinseiLog.SetT_Shinsei_C_TatekaeUpdate(Global.GetConnection(), SessionManager.User.M_User.id.Trim(), uid, lblTatekaeResult.Text, lblTatekae_Koutuuhi.Text, lblTatekae_Shukuhakuhi.Text, lblTatekae_Result1.Text, lblTatekae_Result2.Text, lblTatekae_Result3.Text);
                    ShinseiLog.SetT_Shinsei_MainUpdate(Global.GetConnection(), SessionManager.User.M_User.id.Trim(), uid);
                    lbl_SaveResult3.Text = "Save As: " + DateTime.Now;

                    //データバインド
                    BindData();

                }
                else {

                    //UUIDエラー
                    lbl_SaveResult3.Text = "Save Failed E2: " + DateTime.Now;
                    lbluid.Text = "null";

                }


            }

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



            //データバインド後に再度色付けしようとすると新規行の追加でずれる。
            //データバインド前に行うと色が初期化されるため何の意味もない。
            //  ↓
            //★ShinseiClassからGet参照してuidが一致する列番号を取得し、int argsに代入すればできそう。

            //SQLで連番を作成し、Rowをロードできるようにする。

            //ROW_NUMBER() OVER(ORDER BY {並びかえる列} ASC/DESC)
            //{並びかえる列}を昇順・降順に並び替え、連番（順位）をふる。

            //ROW_NUMBER() OVER(PARTITION BY {グループ化する列} ORDER BY {並びかえる列} ASC/DESC)
            //{並びかえる列}を昇順・降順に並び替え、グループごとに連番（順位）をふる。


            //★もしくは、uidが一致するまで上から順番にfor文で検索する。
            //　項目が増えたときに重くなりそうだけれども、SQLなしでも簡単に実装できる。
            //　項目が増えることを前提にするなら実装しないほうが安定する。

            if (lbluid.Text != "null"){

                //この方式では複数ページが取得できない
                int cnt = GridView1.Rows.Count;

                //ページを複数表示したい場合（仮実装）
                //int page = GridView1.PageCount;
                //cnt *= page;

                try {

                    //----------------------------------------------
                    for (int i = 0; i < cnt; i++)
                    {
                        //Rows[i]行の左から2番目の列[0-nで数える]のセルにある「テキスト」を取得
                        //uidをロード
                        string isbn_uid = GridView1.Rows[i].Cells[1].Text.Trim();

                        //uidは一致したか？
                        if (lbluid.Text == isbn_uid)
                        {
                            //GridView1におけるuidと一致する行の背景に色を付けて強調表示する
                            GridView1.Rows[i].BackColor = System.Drawing.Color.AliceBlue;
                            break;
                        }

                    }
                    //----------------------------------------------

                    //消去確認ポップアップを解除
                    Panel0.Visible = true;
                    Panel00.Visible = false;    //追加や更新ができるということは必ず1つ以上のShinseiデータが存在する
                    //削除確認パネルを閉じる
                    Panel_del_pop.Visible = false;

                }
                catch
                {
                    //存在しないRowsを読み込んでいます！
                    //色変更を行わずに強制終了します。
                    return;
                }

            }


            //正常に終了
            topchange();
            return;
        }

        /// <summary>
        /// リスト更新ボタンが押されたときの処理です。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_reload_Click(object sender, EventArgs e)
        {
            //データバインド
            BindData();
        }

        protected void Button_Datalist_Open_Click(object sender, EventArgs e)
        {
            //データリストパネルを開く
            Panel0.Visible = true;
            Panel00.Visible = false;
        }

        protected void Button_Datalist_Close_Click(object sender, EventArgs e)
        {
            //データリストパネルを最小化
            Panel0.Visible = false;
            Panel00.Visible = true;
        }


        protected void SetTatekaeDate()
        {
            lblTatekaeName.Text = SessionManager.User.M_User.name1;
            DateTime date = DateTime.Now;
            lblTatekaeDate.Text = date.ToShortDateString();
        }

        protected void Button_del_pop_delete(object sender, EventArgs e)
        {
            //削除を許可

            //消去確認ポップアップを解除
            Panel0.Visible = true;
            Panel00.Visible = false;    //削除できるということは必ず1つ以上のShinseiデータが存在する
            //削除確認パネル
            Panel_del_pop.Visible = false;

            int del_args = int.Parse(Session["del_args"].ToString());
            RemoveShinseiRow(del_args);

        }

        protected void Button_del_pop_cancel(object sender, EventArgs e)
        {
            //削除を不許可

            //消去確認ポップアップを解除
            Panel0.Visible = true;
            Panel00.Visible = false;    //削除できるということは必ず1つ以上のShinseiデータが存在する
            //削除確認パネル
            Panel_del_pop.Visible = false;

        }
    }
}