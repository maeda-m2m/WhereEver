using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WhereEver.ClassLibrary;

namespace WhereEver
{
    public partial class Calender : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Panel1.Visible = true;
            Panel2.Visible = false;
            Panel3.Visible = false;
            Panel4.Visible = false;
            Panel5.Visible = false;


        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            name1.Text = "氏名：" + SessionManager.User.M_User.name;
            DateTime dt = DateTime.Now;
            date.Text = dt.ToShortDateString();

            Konyu.Text = TextBox1.Text;
            Syubetsu.Text = TextBox2.Text;
            Suryo.Text = TextBox3.Text;
            KonyuSaki.Text = TextBox4.Text;
            Riyuu.InnerText = TextArea1.InnerText;
            Bikou.InnerText = TextArea2.InnerText;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Panel3.Visible = true;
            Panel5.Visible = true;
            Label4.Text = "氏名：" + SessionManager.User.M_User.name;
            DateTime dt = DateTime.Now;
            Label3.Text = dt.ToShortDateString();

            string tm = DropDownList3.SelectedValue;
            string tm2 = DropDownList4.SelectedValue;

            string dt1 = Label16.Text;
            string dt2 = Label18.Text;




            Label2.Text = DropDownList2.SelectedValue;
            Label5.Text = DropDownList2.SelectedValue;
            Label6.Text = Label16.Text;
            Label7.Text = " " + tm;
            Label8.Text = Label18.Text;
            Label9.Text = " " + tm2;
            TextArea5.InnerText = TextArea3.InnerText;
            TextArea6.InnerText = TextArea4.InnerText;

            DateTime dy = Calendar1.SelectedDate;
            DateTime dy1 = Calendar2.SelectedDate;

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string ddl1 = DropDownList1.SelectedValue;
            if (ddl1 == "物品購入申請")
            {
                Panel1.Visible = true;
                Panel2.Visible = true;
                Panel3.Visible = false;
                Panel4.Visible = true;

            }

            if (ddl1 == "休暇・早退・出社・遅刻届")
            {
                Panel1.Visible = true;
                Panel2.Visible = false;
                Panel3.Visible = true;
                Panel4.Visible = false;
                Panel5.Visible = true;

            }
            name1.Text = "氏名：" + SessionManager.User.M_User.name;
            DateTime dt = DateTime.Now;
            date.Text = dt.ToShortDateString();

        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            Label16.Text = Calendar1.SelectedDate.ToShortDateString();
            Panel1.Visible = true;
            Panel3.Visible = true;

        }

        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            Label18.Text = Calendar2.SelectedDate.ToShortDateString();
            Panel1.Visible = true;
            Panel3.Visible = true;

        }
    }
}