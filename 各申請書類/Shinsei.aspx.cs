using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WhereEver.ClassLibrary;
using System.ComponentModel.DataAnnotations;

namespace WhereEver
{
    public partial class Calender : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
            reg.buy_purpose = TextBox_purchaseName.Text.ToString();
            reg.classification = TextBox_classification.Text.ToString();
            reg.howMany = TextBox_howMany.Text.ToString();
            reg.howMach = TextBox_howMach.Text.ToString();
            reg.marketPlace = TextBox_marketPlace.Text.ToString();
            reg.buy_purpose = TextArea_buy_purpose.ToString();

            Reg(reg);
            //------------------------------------------------------------

            //購入品のテキストを印刷フォームに代入
            Konyu.Text = TextBox_purchaseName.Text;
            Syubetsu.Text = TextBox_classification.Text;
            Suryo.Text = TextBox_howMany.Text + "点";
            Kingaku.Text = "\\" + TextBox_howMach.Text + "-";
            KonyuSaki.Text = TextBox_marketPlace.Text;
            Riyuu.InnerText = TextArea_buy_purpose.InnerText;
            Bikou.InnerText = TextArea_ps.InnerText;
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
            reg.Notification_Purpose = TextArea_Notification_Purpose.ToString();

            Reg(reg);
            //------------------------------------------------------------


            //勤怠届の印刷フォームに名前と日付を代入
            lblDiligenceUser.Text = "氏名：" + SessionManager.User.M_User.name;
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
            TextArea5.InnerText = TextArea_Notification_Purpose.InnerText;
            TextArea6.InnerText = TextArea_Notification_ps.InnerText;

            //DateTime dateTime1 = Calendar1.SelectedDate;
            //DateTime dateTime2 = Calendar2.SelectedDate;

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
                //印刷ボタンパネル
                Panel_Print.Visible = false;

            }
            name1.Text = "氏名：" + SessionManager.User.M_User.name;
            DateTime dt = DateTime.Now;
            date.Text = dt.ToShortDateString();
            //ChangeValidate(true);

        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            //カレンダー１の値が変更されたときに実行されます
            lblSelectedDateA1.Text = Calendar1.SelectedDate.ToShortDateString();
            lblSelectedDateA2.Text = DropDownList_A_Time.SelectedValue;
        }

        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            //カレンダー２の値が変更されたときに実行されます
            lblSelectedDateB1.Text = Calendar2.SelectedDate.ToShortDateString();
            lblSelectedDateB2.Text = DropDownList_B_Time.SelectedValue;
        }

        protected void DropDownList_A_Time_SelectionChanged(object sender, EventArgs e)
        {
            //カレンダー１の時間の値が変更されたときに実行されます
            lblSelectedDateA1.Text = Calendar1.SelectedDate.ToShortDateString();
            lblSelectedDateA2.Text = DropDownList_A_Time.SelectedValue;
        }
        protected void DropDownList_B_Time_SelectionChanged(object sender, EventArgs e)
        {
            //カレンダー２の時間の値が変更されたときに実行されます
            lblSelectedDateB1.Text = Calendar2.SelectedDate.ToShortDateString();
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
            //印刷ボタンパネル
            Panel_Print.Visible = false;

        }
    }
}