using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WhereEver.ClassLibrary;
using System.ComponentModel.DataAnnotations;
using static System.Web.HttpUtility;

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

                Session.Add("waste1", waste1);
                Session.Add("waste2", waste2);
                Session.Add("wasteR1", wasteR1);
                Session.Add("wasteR2", wasteR2);
                Session.Add("wasteR3", wasteR3);

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
            Kingaku.Text = "\\" + str.Substring(0, rleng) + "-";

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
                wasteR2 = int.Parse(HtmlEncode(TextBox_Tatekae_Teiki.Text));
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
            }
            name1.Text = "氏名：" + SessionManager.User.M_User.name1;
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

            lblTatekaeName.Text = "氏名：" + SessionManager.User.M_User.name1;
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


            string str1 = HtmlEncode(TextBox_Tatekae_TWaste.Text);
            str1 = HtmlEncode(str1);
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

            string str2 = HtmlEncode(TextBox_Tatekae_PWaste.Text);
            str2 = HtmlEncode(str2);
            try
            {
                waste2 += int.Parse(str2);
            }
            catch
            {
                //不正な値
                lblTatekae_Koutuuhi.Text = "Error";
                //Rollback
                waste1 -= int.Parse(str1);
                return;
            }

            //定期券代
            try
            {
                wasteR2 = int.Parse(HtmlEncode(TextBox_Tatekae_Teiki.Text));
            }
            catch
            {
                //不正な値
                lblTatekae_Result2.Text = "Error";
                //Rollback
                waste1 -= int.Parse(str1);
                waste2 -= int.Parse(str2);
                return;
            }


            //交通費＋宿泊費
            wasteR1 = waste1 + waste2;
            //定期券代
            wasteR2 = 0;
            //立替金総合計
            wasteR3 = wasteR1 + wasteR2; 
 


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
            rleng = Math.Min(str.Length, maxstr - 1);
            string cell5 = "\\" + str.Substring(0, rleng) + "-";

            str = TextBox_Tatekae_Place.Text;
            str = HtmlEncode(str);
            rleng = Math.Min(str.Length, maxstr);
            string cell6 = str.Substring(0, rleng);

            str = TextBox_Tatekae_PWaste.Text;
            str = HtmlEncode(str);
            rleng = Math.Min(str.Length, maxstr - 1);
            string cell7 = "\\" + str.Substring(0, rleng) + "-";

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
            lblTatekaeResult.Text += "<tr class=\"naiyou\"><td>" + cell0 + "</td><td>" + cell1 + "</td><td>" + cell2 + "</td><td>" + cell3 + "</td><td>" + cell4 + "</td><td>" + cell5 + "</td><td>" + cell6 + "</td><td>" + cell7 + "</td><td>" + cell8 + "</td><td>" + cell9 + "</td></tr>";
            //----------------------------------

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
                wasteR2 = int.Parse(HtmlEncode(TextBox_Tatekae_Teiki.Text));
                lblTatekae_Result2.Text = "\\" + wasteR2.ToString() + "-";
                Session.Add("wasteR2", wasteR2);
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
        //using System.Linq;

        string lines = lblTatekaeResult.Text;
        //最後の1行を削除する?
            string[] del = { "rn" };
            string[] arr = lblTatekaeResult.Text.Split(del, StringSplitOptions.None);
            lblTatekaeResult.Text = arr.ToString();
            //最初の1行を削除するなら、次のようにする
            //lblTatekaeResult.Text = lines.Skip(1).ToArray();
            //lines = lines.Skip(1).ToArray();


        }
    }
}