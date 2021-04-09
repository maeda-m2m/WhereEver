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

                //GridViewパネル
                Panel0.Visible = true;
                //初期選択パネル
                Panel1.Visible = false;
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

            }


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

            name1.Text = "氏名：" + SessionManager.User.M_User.name;
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

            int rleng = Math.Min(TextBox_purchaseName.Text.Length, maxstr);
            Konyu.Text = TextBox_purchaseName.Text.Substring(0, rleng);

            rleng = Math.Min(TextBox_classification.Text.Length, maxstr);
            Syubetsu.Text = TextBox_classification.Text.Substring(0, rleng);

            string str = TextBox_howMany.Text;
            rleng = Math.Min(str.Length, maxstr - 1);
            Suryo.Text = str.Substring(0, rleng) + "点";

            str = TextBox_howMach.Text;
            rleng = Math.Min(str.Length, maxstr - 1);
            //Kingaku.Text = "\\" + str.Substring(0, rleng) + "-";

            StringBuilder sb = new StringBuilder(HtmlEncode(TextBox_howMach.Text).ToString());
            sb.Replace("-", "");
            sb.Replace(",", "");
            sb.Replace("\\", "");

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

            rleng = Math.Min(sb.Length, maxstr - 1);
            str = sb.ToString();
            Kingaku.Text = "\\" + str.Substring(0, rleng) + "-";

            lblTatekae_Result2.Text = "\\" + wasteR2.ToString() + "-";


            rleng = Math.Min(TextBox_marketPlace.Text.Length, maxstr);
            KonyuSaki.Text = TextBox_marketPlace.Text.Substring(0, rleng);

            rleng = Math.Min(TextBox_buy_purpose.Text.Length, maxstr);
            Label_Riyuu.Text = TextBox_buy_purpose.Text.Substring(0, rleng);

            rleng = Math.Min(TextBox_ps.Text.Length, maxstr);
            Label_Bikou.Text = TextBox_ps.Text.Substring(0, rleng);

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //勤怠届の確定ボタンが押されたときの処理です。

            //勤怠届印刷フォーム
            Panel5.Visible = true;
            //印刷ボタンパネル
            Panel_Print.Visible = true;


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


            str = TextBox_Tatekae_Date.Text;
            str = HtmlEncode(str);
            int rleng = Math.Min(str.Length, maxstr);
            string cell0 = str.Substring(0, rleng);

            str = TextBox_Tatekae_WPlace.Text;
            str = HtmlEncode(str);
            rleng = Math.Min(str.Length, maxstr);
            string cell1 = str.Substring(0, rleng);

            str = TextBox_Tatekae_TUse.Text;
            str = HtmlEncode(str);
            rleng = Math.Min(str.Length, maxstr);
            string cell2 = str.Substring(0, rleng);

            str = TextBox_Tatekae_TIn.Text;
            str = HtmlEncode(str);
            rleng = Math.Min(str.Length, maxstr);
            string cell3 = str.Substring(0, rleng);

            str = TextBox_Tatekae_TOut.Text;
            str = HtmlEncode(str);
            rleng = Math.Min(str.Length, maxstr);
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
            rleng = Math.Min(sb.Length, maxstr - 1); //"\\"と"-"のぶんだけ-1
            string cell5 = "\\" + sb.ToString().Substring(0, rleng);

            str = TextBox_Tatekae_Place.Text;
            str = HtmlEncode(str);
            rleng = Math.Min(str.Length, maxstr);
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
            rleng = Math.Min(sb.Length, maxstr - 1); //"\\"と"-"のぶんだけ-1
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

            //定期券代
            try
            {
                StringBuilder str = new StringBuilder(HtmlEncode(TextBox_Tatekae_Teiki.Text).ToString());
                str.Replace("-", "");
                str.Replace(",", "");
                str.Replace("\\", "");
                wasteR2 = int.Parse(str.ToString());
                lblTatekae_Result2.Text = "\\" + wasteR2.ToString() + "-";
                Session.Add("wasteR2", wasteR2);
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


        //GridViewのRowCommand属性に参照可能なメソッドを入力すると↓が自動生成されます。
        //GridViewが読み込まれたときに実行されます。
        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //好きなコードを入れて下さい。

            // コマンド名が“Reform”の場合にのみ処理（修正ボタン）
            if (e.CommandName == "Reform")
            {
                //コマンドの引数を取得
                int args = Int32.Parse((String)e.CommandArgument);

                //ロードのためにテーブルには用いるデータをバインドしている必要がある。Panelなどで消しているぶんには問題なし。

                // 主キー（isbn列）の値を取得
                //ユーザー名をロード（いらない）
                String isbn_key = (String)GridView1.DataKeys[args].Value;

                // クリックされた[args]行の左から3番目の列[0-nで数える]のセルにある「テキスト」を取得
                //申請種別をロード
                String isbn_kind = GridView1.Rows[args].Cells[2].Text; ;

                // クリックされた[args]行の左から4番目の列[0-nで数える]のセルにある「テキスト」を取得
                //日付をロード
                String isbn_date = GridView1.Rows[args].Cells[3].Text; ;

                // クリックされた[args]行の左から4番目の列[0-nで数える]のセルにある「テキスト」を取得
                //最終更新日をロード
                String isbn_finaldate = GridView1.Rows[args].Cells[4].Text; ;


                if (isbn_kind == "物品購入申請")
                {
                    // クリックされた[args]行の左から6番目の列[0-nで数える]のセルにある「テキスト」を取得
                    //申請内容を１つずつロードしていく……
                    name1.Text = isbn_key;
                    date.Text = isbn_date;
                    Konyu.Text = GridView1.Rows[args].Cells[5].Text; ;
                    Syubetsu.Text = GridView1.Rows[args].Cells[6].Text; ;
                    Suryo.Text = GridView1.Rows[args].Cells[7].Text; ;
                    Kingaku.Text = GridView1.Rows[args].Cells[8].Text; ;
                    KonyuSaki.Text = GridView1.Rows[args].Cells[9].Text; ;
                    Label_Riyuu.Text = GridView1.Rows[args].Cells[10].Text; ;
                    Label_Bikou.Text = GridView1.Rows[args].Cells[11].Text; ;
                }
                else if(isbn_kind == "勤怠関連申請")
                {
                    // クリックされた[args]行の左から13番目の列[0-nで数える]のセルにある「テキスト」を取得
                    //申請内容を１つずつロードしていく……
                    lblDiligenceUser.Text = isbn_date;                  
                    lblDiligenceDate.Text = isbn_date;
                    lblDiligenceClassification1.Text = GridView1.Rows[args].Cells[12].Text; ;
                    lblDiligenceClassification2.Text = GridView1.Rows[args].Cells[13].Text; ;
                    lblDiligenceDateA1.Text = GridView1.Rows[args].Cells[14].Text; ;
                    lblDiligenceDateA2.Text = GridView1.Rows[args].Cells[15].Text; ;
                    lblDiligenceDateB1.Text = GridView1.Rows[args].Cells[16].Text; ;
                    lblDiligenceDateB2.Text = GridView1.Rows[args].Cells[17].Text; ;
                }
                else if (isbn_kind == "立替金明細表申請")
                {
                    // クリックされた[args]行の左から19番目の列[0-nで数える]のセルにある「テキスト」を取得
                    //申請内容を１つずつロードしていく……
                    lblTatekaeName.Text = isbn_date;
                    lblTatekaeDate.Text = isbn_date;
                    lblTatekaeResult.Text = GridView1.Rows[args].Cells[18].Text; ;
                    lblTatekae_Koutuuhi.Text = GridView1.Rows[args].Cells[19].Text; ;
                    lblTatekae_Shukuhakuhi.Text = GridView1.Rows[args].Cells[20].Text; ;
                    lblTatekae_Result1.Text = GridView1.Rows[args].Cells[21].Text; ;
                    lblTatekae_Result2.Text = GridView1.Rows[args].Cells[22].Text; ;
                    lblTatekae_Result3.Text = GridView1.Rows[args].Cells[23].Text; ;
                }

            }

            return; //grid_RowCommand
        }

        protected void SaveButton_Click_1(object sender, EventArgs e)
        {
            //Set 物品購入申請書
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
        }

        protected void SaveButton_Click_2(object sender, EventArgs e)
        {
            //Set 勤怠届
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
        }

        protected void SaveButton_Click_3(object sender, EventArgs e)
        {
            //Set 立替金明細表

            //初期化
            resetTatekaeTable();

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

            SetTatekaeString();

        }


    }
}