using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WhereEver
{
    public partial class Schedule : System.Web.UI.Page
    {



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Create();
                Create3();
                Create2();

                Panel1.Visible = false;
                Panel3.Visible = false;

                ViewState["Count"] = 0;
                ViewState["Serch"] = 0;
                ViewState["Count2"] = 0;

                //Panel1、登録
                //Panel2、登録メニュー
                //Panel3、検索
                //panel4、検索メニュー

                //TextBox1,2,3,5 used




            }
        }


        public void Create()//スケジュールリストにデータを格納(日付が古い順)
        {
            var dt = Class1.GetT_Schedule3DataTable(Global.GetConnection());

            ScdlList.DataSource = dt;

            ScdlList.DataBind();
        }

        public void Create_Desc()//スケジュールリストにデータを格納(日付が新しい順)
        {
            var dt = Class1.GetT_Schedule3DataTableDesc(Global.GetConnection());

            ScdlList.DataSource = dt;

            ScdlList.DataBind();
        }

        protected void Create4(object sender, EventArgs e)//リストを並び変える用
        {

            ViewState["Count2"] = int.Parse(ViewState["Count2"].ToString()) + 1;
            var a = Ddl.SelectedValue;

            var b = Ddl.SelectedValue;

            if (a == "降順")
            {
                Create_Desc();
                Create3();
                Create2();
                Ddl.Focus();


            }
            else if (b == "昇順")
            {
                Create();
                Create3();
                Create2();
                Ddl.Focus();

            }
        }

        public void Create2()//今週
        {
            ViewState["count"] = 0;

            var dd = Class1.SwitchScdl3DataTable(Global.GetConnection());

            for (int j = 0; j < dd.Count; j++)
            {
                var dl = dd.Rows[j] as DATASET.DataSet.T_ScheduleRow;

                DateTime DT = DateTime.Parse(dl.date.ToString());

                string week = DT.ToString("ddd");

                string tm = dl.time.Trim();

                DateTime dt = DateTime.Now;

                Scdl3.Items[0].Cells[1].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[2].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[3].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[4].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[5].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[6].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[7].Text = dt.ToString("ddd");


                string Z0 = Scdl3.Items[0].Cells[1].Text;//月
                string Z1 = Scdl3.Items[0].Cells[2].Text;//火
                string Z2 = Scdl3.Items[0].Cells[3].Text;//水
                string Z3 = Scdl3.Items[0].Cells[4].Text;//木
                string Z4 = Scdl3.Items[0].Cells[5].Text;//金
                string Z5 = Scdl3.Items[0].Cells[6].Text;//土
                string Z6 = Scdl3.Items[0].Cells[7].Text;//日

                Scdl3.Items[0].Cells[0].Text = "日付";
                Scdl3.Items[1].Cells[0].Text = "9:00";
                Scdl3.Items[2].Cells[0].Text = "10:00";
                Scdl3.Items[3].Cells[0].Text = "11:00";
                Scdl3.Items[4].Cells[0].Text = "12:00";
                Scdl3.Items[5].Cells[0].Text = "13:00";
                Scdl3.Items[6].Cells[0].Text = "14:00";
                Scdl3.Items[7].Cells[0].Text = "15:00";
                Scdl3.Items[8].Cells[0].Text = "16:00";
                Scdl3.Items[9].Cells[0].Text = "17:00";
                Scdl3.Items[10].Cells[0].Text = "18:00";

                Scdl3.Items[0].Cells[0].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[1].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[2].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[3].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[4].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[5].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[6].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[7].CssClass = "Center_cs";

                if (Z0 == "月")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(0).ToString("MMMMd日");//mon
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(+1).ToString("MMMMd日");//tue
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(+2).ToString("MMMMd日");//wed
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(+3).ToString("MMMMd日");//thu
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(+4).ToString("MMMMd日");//fri
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(+5).ToString("MMMMd日");//sat
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(+6).ToString("MMMMd日");//sun

                    Scdl3.Items[2].Cells[1].CssClass = "Center_cs_Color";
                    Scdl3.Items[4].Cells[1].CssClass = "Center_cs_Color";
                    Scdl3.Items[6].Cells[1].CssClass = "Center_cs_Color";
                    Scdl3.Items[8].Cells[1].CssClass = "Center_cs_Color";
                    Scdl3.Items[10].Cells[1].CssClass = "Center_cs_Color_Bottom";//セルの終わり

                    Scdl3.Items[1].Cells[1].CssClass = "Center_cs_Color";
                    Scdl3.Items[3].Cells[1].CssClass = "Center_cs_Color";
                    Scdl3.Items[5].Cells[1].CssClass = "Center_cs_Color";
                    Scdl3.Items[7].Cells[1].CssClass = "Center_cs_Color";
                    Scdl3.Items[9].Cells[1].CssClass = "Center_cs_Color";

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";
                }
                else if (Z1 == "火")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(-1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(0).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(+1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(+2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(+3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(+4).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(+6).ToString("MMMMd日");

                    Scdl3.Items[2].Cells[2].CssClass = "Center_cs_Color";
                    Scdl3.Items[4].Cells[2].CssClass = "Center_cs_Color";
                    Scdl3.Items[6].Cells[2].CssClass = "Center_cs_Color";
                    Scdl3.Items[8].Cells[2].CssClass = "Center_cs_Color";
                    Scdl3.Items[10].Cells[2].CssClass = "Center_cs_Color_Bottom";//セルの終わり

                    Scdl3.Items[1].Cells[2].CssClass = "Center_cs_Color";
                    Scdl3.Items[3].Cells[2].CssClass = "Center_cs_Color";
                    Scdl3.Items[5].Cells[2].CssClass = "Center_cs_Color";
                    Scdl3.Items[7].Cells[2].CssClass = "Center_cs_Color";
                    Scdl3.Items[9].Cells[2].CssClass = "Center_cs_Color";

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";
                }
                else if (Z2 == "水")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(-2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(-1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(0).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(+1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(+2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(+3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(+4).ToString("MMMMd日");

                    Scdl3.Items[2].Cells[3].CssClass = "Center_cs_Color";
                    Scdl3.Items[4].Cells[3].CssClass = "Center_cs_Color";
                    Scdl3.Items[6].Cells[3].CssClass = "Center_cs_Color";
                    Scdl3.Items[8].Cells[3].CssClass = "Center_cs_Color";
                    Scdl3.Items[10].Cells[3].CssClass = "Center_cs_Color_Bottom";//セルの終わり

                    Scdl3.Items[1].Cells[3].CssClass = "Center_cs_Color";
                    Scdl3.Items[3].Cells[3].CssClass = "Center_cs_Color";
                    Scdl3.Items[5].Cells[3].CssClass = "Center_cs_Color";
                    Scdl3.Items[7].Cells[3].CssClass = "Center_cs_Color";
                    Scdl3.Items[9].Cells[3].CssClass = "Center_cs_Color";

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";
                }
                else if (Z3 == "木")
                {
                    DateTime a = DateTime.Now;


                    Scdl3.Items[0].Cells[1].Text = a.AddDays(-3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(-2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(-1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(0).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(+1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(+2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(+3).ToString("MMMMd日");

                    Scdl3.Items[2].Cells[4].CssClass = "Center_cs_Color";
                    Scdl3.Items[4].Cells[4].CssClass = "Center_cs_Color";
                    Scdl3.Items[6].Cells[4].CssClass = "Center_cs_Color";
                    Scdl3.Items[8].Cells[4].CssClass = "Center_cs_Color";
                    Scdl3.Items[10].Cells[4].CssClass = "Center_cs_Color_Bottom";//セルの終わり

                    Scdl3.Items[1].Cells[4].CssClass = "Center_cs_Color";
                    Scdl3.Items[3].Cells[4].CssClass = "Center_cs_Color";
                    Scdl3.Items[5].Cells[4].CssClass = "Center_cs_Color";
                    Scdl3.Items[7].Cells[4].CssClass = "Center_cs_Color";
                    Scdl3.Items[9].Cells[4].CssClass = "Center_cs_Color";

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";
                }
                else if (Z4 == "金")
                {
                    DateTime a = DateTime.Now;


                    Scdl3.Items[0].Cells[1].Text = a.AddDays(-4).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(-3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(-2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(-1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(0).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(+1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(+2).ToString("MMMMd日");

                    Scdl3.Items[2].Cells[5].CssClass = "Center_cs_Color";
                    Scdl3.Items[4].Cells[5].CssClass = "Center_cs_Color";
                    Scdl3.Items[6].Cells[5].CssClass = "Center_cs_Color";
                    Scdl3.Items[8].Cells[5].CssClass = "Center_cs_Color";
                    Scdl3.Items[10].Cells[5].CssClass = "Center_cs_Color_Bottom";//セルの終わり

                    Scdl3.Items[1].Cells[5].CssClass = "Center_cs_Color";
                    Scdl3.Items[3].Cells[5].CssClass = "Center_cs_Color";
                    Scdl3.Items[5].Cells[5].CssClass = "Center_cs_Color";
                    Scdl3.Items[7].Cells[5].CssClass = "Center_cs_Color";
                    Scdl3.Items[9].Cells[5].CssClass = "Center_cs_Color";

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";
                }
                else if (Z5 == "土")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(-5).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(-4).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(-3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(-2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(-1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(0).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(+1).ToString("MMMMd日");

                    Scdl3.Items[2].Cells[6].CssClass = "Center_cs_Color";
                    Scdl3.Items[4].Cells[6].CssClass = "Center_cs_Color";
                    Scdl3.Items[6].Cells[6].CssClass = "Center_cs_Color";
                    Scdl3.Items[8].Cells[6].CssClass = "Center_cs_Color";
                    Scdl3.Items[10].Cells[6].CssClass = "Center_cs_Color_Bottom";//セルの終わり

                    Scdl3.Items[1].Cells[6].CssClass = "Center_cs_Color";
                    Scdl3.Items[3].Cells[6].CssClass = "Center_cs_Color";
                    Scdl3.Items[5].Cells[6].CssClass = "Center_cs_Color";
                    Scdl3.Items[7].Cells[6].CssClass = "Center_cs_Color";
                    Scdl3.Items[9].Cells[6].CssClass = "Center_cs_Color";

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";

                }
                else if (Z6 == "日")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(-6).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(-5).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(-4).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(-3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(-2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(-1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(0).ToString("MMMMd日");

                    Scdl3.Items[2].Cells[7].CssClass = "Center_cs_Color";
                    Scdl3.Items[4].Cells[7].CssClass = "Center_cs_Color";
                    Scdl3.Items[6].Cells[7].CssClass = "Center_cs_Color";
                    Scdl3.Items[8].Cells[7].CssClass = "Center_cs_Color";
                    Scdl3.Items[10].Cells[7].CssClass = "Center_cs_Color_Bottom";//セルの終わり

                    Scdl3.Items[1].Cells[7].CssClass = "Center_cs_Color";
                    Scdl3.Items[3].Cells[7].CssClass = "Center_cs_Color";
                    Scdl3.Items[5].Cells[7].CssClass = "Center_cs_Color";
                    Scdl3.Items[7].Cells[7].CssClass = "Center_cs_Color";
                    Scdl3.Items[9].Cells[7].CssClass = "Center_cs_Color";

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";

                }

                if (tm == "9:00" || tm == "9:15" || tm == "9:30" || tm == "9:45")
                {
                    if (week == "月")
                    {
                        string A1 = Scdl3.Items[1].Cells[1].Text;
                        A1 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[1].Text = A1.Replace("\r\n", "<br>");

                    }
                    if (week == "火")
                    {
                        string A2 = Scdl3.Items[1].Cells[2].Text;
                        A2 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[2].Text = A2.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A3 = Scdl3.Items[1].Cells[3].Text;
                        A3 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[3].Text = A3.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A4 = Scdl3.Items[1].Cells[4].Text;
                        A4 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[4].Text = A4.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A5 = Scdl3.Items[1].Cells[5].Text;
                        A5 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[5].Text = A5.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A5 = Scdl3.Items[1].Cells[6].Text;
                        A5 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[6].Text = A5.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A5 = Scdl3.Items[1].Cells[7].Text;
                        A5 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[6].Text = A5.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "10:00" || tm == "10:15" || tm == "10:30" || tm == "10:45")
                {
                    if (week == "月")
                    {
                        string A6 = Scdl3.Items[2].Cells[1].Text;
                        A6 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[1].Text = A6.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A7 = Scdl3.Items[2].Cells[2].Text;
                        A7 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[2].Text = A7.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A8 = Scdl3.Items[2].Cells[3].Text;
                        A8 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[3].Text = A8.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A9 = Scdl3.Items[2].Cells[4].Text;
                        A9 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[4].Text = A9.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A10 = Scdl3.Items[2].Cells[5].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[5].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[2].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[2].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "11:00" || tm == "11:15" || tm == "11:30" || tm == "11:45")
                {
                    if (week == "月")
                    {
                        string A11 = Scdl3.Items[3].Cells[1].Text;
                        A11 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[1].Text = A11.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A12 = Scdl3.Items[3].Cells[2].Text;
                        A12 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[2].Text = A12.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A13 = Scdl3.Items[3].Cells[3].Text;
                        A13 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[3].Text = A13.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A14 = Scdl3.Items[3].Cells[4].Text;
                        A14 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[4].Text = A14.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A15 = Scdl3.Items[3].Cells[5].Text;
                        A15 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[5].Text = A15.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[3].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[3].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "12:00" || tm == "12:15" || tm == "12:30" || tm == "12:45")
                {
                    if (week == "月")
                    {
                        string A16 = Scdl3.Items[4].Cells[1].Text;
                        A16 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[1].Text = A16.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A17 = Scdl3.Items[4].Cells[2].Text;
                        A17 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[2].Text = A17.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A18 = Scdl3.Items[4].Cells[3].Text;
                        A18 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[3].Text = A18.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A19 = Scdl3.Items[4].Cells[4].Text;
                        A19 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[4].Text = A19.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A20 = Scdl3.Items[4].Cells[5].Text;
                        A20 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[5].Text = A20.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[4].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[4].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "13:00" || tm == "13:15" || tm == "13:30" || tm == "13:45")
                {
                    if (week == "月")
                    {
                        string A21 = Scdl3.Items[5].Cells[1].Text;
                        A21 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[1].Text = A21.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A22 = Scdl3.Items[5].Cells[2].Text;
                        A22 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[2].Text = A22.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A23 = Scdl3.Items[5].Cells[3].Text;
                        A23 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[3].Text = A23.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A24 = Scdl3.Items[5].Cells[4].Text;
                        A24 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[4].Text = A24.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A25 = Scdl3.Items[5].Cells[5].Text;
                        A25 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[5].Text = A25.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[5].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[5].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "14:00" || tm == "14:15" || tm == "14:30" || tm == "14:45")
                {
                    if (week == "月")
                    {
                        string A26 = Scdl3.Items[6].Cells[1].Text;
                        A26 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[1].Text = A26.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A27 = Scdl3.Items[6].Cells[2].Text;
                        A27 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[2].Text = A27.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A28 = Scdl3.Items[6].Cells[3].Text;
                        A28 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[3].Text = A28.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A29 = Scdl3.Items[6].Cells[4].Text;
                        A29 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[4].Text = A29.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A30 = Scdl3.Items[6].Cells[5].Text;
                        A30 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[5].Text = A30.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[6].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[6].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "15:00" || tm == "15:15" || tm == "15:30" || tm == "15:45")
                {
                    if (week == "月")
                    {
                        string A31 = Scdl3.Items[7].Cells[1].Text;
                        A31 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[1].Text = A31.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A32 = Scdl3.Items[7].Cells[2].Text;
                        A32 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[2].Text = A32.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A33 = Scdl3.Items[7].Cells[3].Text;
                        A33 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[3].Text = A33.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A34 = Scdl3.Items[7].Cells[4].Text;
                        A34 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[4].Text = A34.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A35 = Scdl3.Items[7].Cells[5].Text;
                        A35 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[5].Text = A35.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[7].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[7].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "16:00" || tm == "16:15" || tm == "16:30" || tm == "16:45")
                {
                    if (week == "月")
                    {
                        string A36 = Scdl3.Items[8].Cells[1].Text;
                        A36 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[1].Text = A36.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A37 = Scdl3.Items[8].Cells[2].Text;
                        A37 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[2].Text = A37.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A38 = Scdl3.Items[8].Cells[3].Text;
                        A38 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[3].Text = A38.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A39 = Scdl3.Items[8].Cells[4].Text;
                        A39 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[4].Text = A39.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A40 = Scdl3.Items[8].Cells[5].Text;
                        A40 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[5].Text = A40.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[8].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[8].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "17:00" || tm == "17:15" || tm == "17:30" || tm == "17:45")
                {
                    if (week == "月")
                    {
                        string A41 = Scdl3.Items[9].Cells[1].Text;
                        A41 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[1].Text = A41.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A42 = Scdl3.Items[9].Cells[2].Text;
                        A42 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[2].Text = A42.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A43 = Scdl3.Items[9].Cells[3].Text;
                        A43 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[3].Text = A43.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A44 = Scdl3.Items[9].Cells[4].Text;
                        A44 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[4].Text = A44.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A45 = Scdl3.Items[9].Cells[5].Text;
                        A45 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[5].Text = A45.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[9].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[9].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "18:00")
                {
                    if (week == "月")
                    {
                        string A46 = Scdl3.Items[10].Cells[1].Text;
                        A46 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[1].Text = A46.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A47 = Scdl3.Items[10].Cells[2].Text;
                        A47 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[2].Text = A47.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A48 = Scdl3.Items[10].Cells[3].Text;
                        A48 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[3].Text = A48.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A49 = Scdl3.Items[10].Cells[4].Text;
                        A49 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[4].Text = A49.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A50 = Scdl3.Items[10].Cells[5].Text;
                        A50 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[5].Text = A50.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[10].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[10].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }

                }
            }
        }


        public void Create3()//スケジュール帳にデータを格納
        {
            var dt = Class1.GetSchedule3DataTable(Global.GetConnection());

            Scdl3.DataSource = dt;

            Scdl3.DataBind();
        }



        protected void Button1_Click(object sender, EventArgs e)//印刷
        {
            Response.Redirect("PrintSchedule.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)//スケジュールを登録
        {

            var dt = Class1.GetT_Schedule3DataTable(Global.GetConnection());

            var dr = dt.NewT_ScheduleRow();

            string t = DropDownList1.SelectedValue;

            string f = TextBox2.Text + " " + DropDownList1.SelectedValue;

            DateTime dd = DateTime.Parse(f);

            dr.date = dd;

            dr.time = t;

            dr.title = TextBox1.Text;

            dr.name = "";

            foreach (ListItem item in CheckBoxList1.Items)
            {
                if (item.Selected)
                {
                    dr.name += item.Value + " ";
                }
            }

            var dl = Class1.MaxSdlNo(Global.GetConnection());

            int no = dl.SdlNo;

            dr.SdlNo = no + 1;

            dt.AddT_ScheduleRow(dr);

            Class1.InsertList(dt, Global.GetConnection());

            Create();
            Create3();
            Create2();

            Panel1.Visible = false;
            Panel2.Visible = true;
            Panel3.Visible = false;
        }

        protected void Button3_Click(object sender, EventArgs e)//登録パネル
        {
            Panel1.Visible = true;
            Panel2.Visible = true;
            Panel3.Visible = false;
            Panel4.Visible = true;

            Create();
            Create3();
            Create2();
        }

        protected void Button10_Click(object sender, EventArgs e)//検索パネル
        {
            Panel1.Visible = false;
            Panel2.Visible = true;
            Panel3.Visible = true;
            Panel4.Visible = false;

            Create();
            Create3();
            Create2();

            Button8.Focus();


        }

        protected void Scdl3_ItemDataBound(object sender, DataGridItemEventArgs e)//scdl3の表示の枠を作っている
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                var dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_EmptyTableRow;


                Label time = e.Item.FindControl("Jikan") as Label;
                Label monday = e.Item.FindControl("MondayTitle") as Label;
                Label tuesday = e.Item.FindControl("TuesdayTitle") as Label;
                Label wednesday = e.Item.FindControl("WednesdayTitle") as Label;
                Label thursday = e.Item.FindControl("ThursdayTitle") as Label;
                Label friday = e.Item.FindControl("FridayTitle") as Label;
                Label Saturday = e.Item.FindControl("Saturday") as Label;
                Label Sunday = e.Item.FindControl("Sunday") as Label;

                time.Text = dr.曜日.ToString();

                if (!dr.Is月Null())
                    monday.Text = dr.月; ;

                if (!dr.Is火Null())
                    tuesday.Text = dr.火;

                if (!dr.Is水Null())
                    wednesday.Text = dr.水;

                if (!dr.Is木Null())
                    thursday.Text = dr.木;

                if (!dr.Is金Null())
                    friday.Text = dr.金;


                if (!dr.Is土Null())
                    Saturday.Text = dr.土;

                if (!dr.Is日Null())
                    Sunday.Text = dr.日;
            }
        }


        protected void ScdlList_ItemDataBound(object sender, DataGridItemEventArgs e)//スケジュールリストを表示している
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                var dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_ScheduleRow;

                e.Item.Cells[0].Text = dr.date.ToString("yyyy年MMMMd日") + " " + dr.date.ToString("dddd");
                e.Item.Cells[1].Text = dr.time.ToString();
                e.Item.Cells[2].Text = dr.title.ToString();
                e.Item.Cells[3].Text = dr.name.ToString();
                //e.Item.Cells[4].Text = dr.SdlNo.ToString();


            }
        }


        protected void ScdlList_ItemCommand(object sender, DataGridCommandEventArgs e)//削除ボタンの処理
        {
            int Count = 1;
            int Serch = int.Parse(ViewState["Serch"].ToString());
            int Count2 = int.Parse(ViewState["Count2"].ToString());

            var a = TextBox3.Text;//date

            var b = DropDownList2.SelectedValue;//time

            var c = TextBox5.Text;//title

            string d;//name

            d = "";

            foreach (ListItem item in CheckBoxList2.Items)
            {
                if (item.Selected)
                {
                    d += item.Value + " ";
                }
            }

            if (Serch == 0 & Count2 >= 1)
            {
                int Delete = e.Item.ItemIndex;
                var dt = Class1.GetT_Schedule3DataTableDesc(Global.GetConnection());
                var dr = dt.Rows[Delete] as DATASET.DataSet.T_ScheduleRow;
                int sdl = dr.SdlNo;

                if (e.CommandName == "Delete")
                {
                    if (sdl > 0)
                        Class1.DeleteList(sdl, Global.GetConnection());
                    ScdlList.Items[Delete].FindControl("No");

                    Create_Desc();
                    Create3();
                    Create2();
                }
            }
            else if (Count <= Serch)
            {
                int Delete = e.Item.ItemIndex;
                var dt = Class1.ScheduleSearch(a, b, c, d, Global.GetConnection());
                var dr = dt.Rows[Delete] as DATASET.DataSet.T_ScheduleRow;
                int sdl = dr.SdlNo;

                if (e.CommandName == "Delete")
                {
                    if (sdl > 0)
                        Class1.DeleteList(sdl, Global.GetConnection());
                    ScdlList.Items[Delete].FindControl("No");

                    var dd = Class1.ScheduleSearch(a, b, c, d, Global.GetConnection());
                    ScdlList.DataSource = dd;
                    ScdlList.DataBind();

                    Create3();
                    Create2();
                }

            }
            else
            {
                int Delete = e.Item.ItemIndex;
                var dt = Class1.GetT_Schedule3DataTable(Global.GetConnection());
                var dr = dt.Rows[Delete] as DATASET.DataSet.T_ScheduleRow;
                int sdl = dr.SdlNo;

                if (e.CommandName == "Delete")
                {
                    if (sdl > 0)
                        Class1.DeleteList(sdl, Global.GetConnection());
                    ScdlList.Items[Delete].FindControl("No");

                    Create();
                    Create3();
                    Create2();
                }

            }

        }

        protected void ScdlList_EditCommand(object sender, DataGridCommandEventArgs e)//編集ボタンの処理
        {
            int Count = 1;
            int Serch = int.Parse(ViewState["Serch"].ToString());
            int Count2 = int.Parse(ViewState["Count2"].ToString());

            var a = TextBox3.Text;//date

            var b = DropDownList2.SelectedValue;//time

            var c = TextBox5.Text;//title

            string d;//name

            d = "";

            foreach (ListItem item in CheckBoxList2.Items)
            {
                if (item.Selected)
                {
                    d += item.Value + " ";
                }
            }

            if (Serch == 0 & Count2 >= 1)
            {
                ScdlList.EditItemIndex = e.Item.ItemIndex;

                var dt = Class1.GetT_Schedule3DataTableDesc(Global.GetConnection());
                ScdlList.DataSource = dt;
                ScdlList.DataBind();

                Create_Desc();
                Create3();
                Create2();
            }

            else if (Count <= Serch)
            {
                ScdlList.EditItemIndex = e.Item.ItemIndex;

                var dt = Class1.ScheduleSearch(a, b, c, d, Global.GetConnection());
                ScdlList.DataSource = dt;
                ScdlList.DataBind();

                Create3();
                Create2();
            }
            else
            {
                ScdlList.EditItemIndex = e.Item.ItemIndex;

                Create();
                Create3();
                Create2();
            }


        }


        protected void ScdlList_CancelCommand(object sender, DataGridCommandEventArgs e)//編集ボタンをキャンセルした時の処理
        {
            ScdlList.EditItemIndex = -1;
            ScdlList.DataSource = Class1.GetT_Schedule3DataTable(Global.GetConnection());
            ScdlList.DataBind();

            Create();
            Create3();
            Create2();
        }

        public void ScdlList_UpdateCommand(object sender, DataGridCommandEventArgs e)//編集ボタンを押して編集を保存した時の処理
        {

            int Count = 1;
            int Serch = int.Parse(ViewState["Serch"].ToString());
            int Count2 = int.Parse(ViewState["Count2"].ToString());

            var a = TextBox3.Text;//date

            var b = DropDownList2.SelectedValue;//time

            var c = TextBox5.Text;//title

            string d;//name

            d = "";

            foreach (ListItem item in CheckBoxList2.Items)
            {
                if (item.Selected)
                {
                    d += item.Value + " ";
                }
            }
            if (Serch == 0 & Count2 >= 1)
            {
                TextBox a1 = (TextBox)e.Item.Cells[0].Controls[0];
                TextBox a2 = (TextBox)e.Item.Cells[1].Controls[0];
                TextBox a3 = (TextBox)e.Item.Cells[2].Controls[0];
                TextBox a4 = (TextBox)e.Item.Cells[3].Controls[0];
                //TextBox a5 = (TextBox)e.Item.Cells[4].Controls[0];

                string b1 = a1.Text.Trim();
                string b2 = a2.Text.Trim();
                string b3 = a3.Text.Trim();
                string b4 = a4.Text.Trim();
                //string b5 = a5.Text.Trim();

                var dt = Class1.GetT_Schedule3DataTableDesc(Global.GetConnection());
                int ItemRow = e.Item.ItemIndex;
                var dr = dt.Rows[ItemRow] as DATASET.DataSet.T_ScheduleRow;

                dr[0] = b1.Trim();
                dr[1] = b2.Trim();
                dr[2] = b3.Trim();
                dr[3] = b4.Trim();
                //dr[4] = b5.Trim();

                Class1.UpdateProject(dr, Global.GetConnection());

                ScdlList.EditItemIndex = -1;


                Create_Desc();
                Create3();
                Create2();
            }
            else if (Count <= Serch)
            {
                TextBox a1 = (TextBox)e.Item.Cells[0].Controls[0];
                TextBox a2 = (TextBox)e.Item.Cells[1].Controls[0];
                TextBox a3 = (TextBox)e.Item.Cells[2].Controls[0];
                TextBox a4 = (TextBox)e.Item.Cells[3].Controls[0];
                //TextBox a5 = (TextBox)e.Item.Cells[4].Controls[0];

                string b1 = a1.Text.Trim();
                string b2 = a2.Text.Trim();
                string b3 = a3.Text.Trim();
                string b4 = a4.Text.Trim();
                //string b5 = a5.Text.Trim();

                var dt = Class1.ScheduleSearch(a, b, c, d, Global.GetConnection());
                int A1 = e.Item.ItemIndex;
                var dr = dt.Rows[A1] as DATASET.DataSet.T_ScheduleRow;

                dr[0] = b1.Trim();
                dr[1] = b2.Trim();
                dr[2] = b3.Trim();
                dr[3] = b4.Trim();
                //dr[4] = b5.Trim();

                Class1.UpdateProject(dr, Global.GetConnection());

                ScdlList.EditItemIndex = -1;

                ScdlList.DataSource = Class1.ScheduleSearch(a, b, c, d, Global.GetConnection());
                ScdlList.DataBind();


                Create3();
                Create2();

            }
            else
            {
                TextBox a1 = (TextBox)e.Item.Cells[0].Controls[0];
                TextBox a2 = (TextBox)e.Item.Cells[1].Controls[0];
                TextBox a3 = (TextBox)e.Item.Cells[2].Controls[0];
                TextBox a4 = (TextBox)e.Item.Cells[3].Controls[0];
                //TextBox a5 = (TextBox)e.Item.Cells[4].Controls[0];

                string b1 = a1.Text.Trim();
                string b2 = a2.Text.Trim();
                string b3 = a3.Text.Trim();
                string b4 = a4.Text.Trim();
                //string b5 = a5.Text.Trim();

                var dt = Class1.GetT_Schedule3DataTable(Global.GetConnection());
                int A1 = e.Item.ItemIndex;
                var dr = dt.Rows[A1] as DATASET.DataSet.T_ScheduleRow;

                dr[0] = b1.Trim();
                dr[1] = b2.Trim();
                dr[2] = b3.Trim();
                dr[3] = b4.Trim();
                //dr[4] = b5.Trim();

                Class1.UpdateProject(dr, Global.GetConnection());

                ScdlList.EditItemIndex = -1;

                Create();
                Create3();
                Create2();
            }

        }



        protected void Button4_Click(object sender, EventArgs e)//前へボタンの処理
        {
            int Count_Week;
            Count_Week = int.Parse(ViewState["count"].ToString()) - 7;

            ViewState["count"] =
            int.Parse(ViewState["count"].ToString()) - 7;

            var Count = ViewState["count"];

            Create3();

            var dd = Class1.SwitchNextScdl3DataTable(Count, Global.GetConnection());//先週

            for (int j = 0; j < dd.Count; j++)
            {
                var dl = dd.Rows[j] as DATASET.DataSet.T_ScheduleRow;

                DateTime DT = DateTime.Parse(dl.date.ToString());

                string week = DT.ToString("ddd");

                string tm = dl.time.Trim();

                DateTime dt = DateTime.Now;

                Scdl3.Items[0].Cells[1].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[2].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[3].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[4].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[5].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[6].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[7].Text = dt.ToString("ddd");


                string Z0 = Scdl3.Items[0].Cells[1].Text;
                string Z1 = Scdl3.Items[0].Cells[2].Text;
                string Z2 = Scdl3.Items[0].Cells[3].Text;
                string Z3 = Scdl3.Items[0].Cells[4].Text;
                string Z4 = Scdl3.Items[0].Cells[5].Text;
                string Z5 = Scdl3.Items[0].Cells[6].Text;
                string Z6 = Scdl3.Items[0].Cells[7].Text;

                Scdl3.Items[0].Cells[0].Text = "日付";
                Scdl3.Items[1].Cells[0].Text = "9:00";
                Scdl3.Items[2].Cells[0].Text = "10:00";
                Scdl3.Items[3].Cells[0].Text = "11:00";
                Scdl3.Items[4].Cells[0].Text = "12:00";
                Scdl3.Items[5].Cells[0].Text = "13:00";
                Scdl3.Items[6].Cells[0].Text = "14:00";
                Scdl3.Items[7].Cells[0].Text = "15:00";
                Scdl3.Items[8].Cells[0].Text = "16:00";
                Scdl3.Items[9].Cells[0].Text = "17:00";
                Scdl3.Items[10].Cells[0].Text = "18:00";

                Scdl3.Items[0].Cells[0].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[1].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[2].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[3].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[4].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[5].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[6].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[7].CssClass = "Center_cs";


                if (Z0 == "月")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(Count_Week).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(Count_Week + 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(Count_Week + 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(Count_Week + 3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(Count_Week + 4).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(Count_Week + 5).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(Count_Week + 6).ToString("MMMMd日");

          

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";

                }
                else if (Z1 == "火")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(Count_Week - 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(Count_Week).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(Count_Week + 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(Count_Week + 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(Count_Week + 3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(Count_Week + 4).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(Count_Week + 5).ToString("MMMMd日");

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";
                }
                else if (Z2 == "水")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(Count_Week - 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(Count_Week - 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(Count_Week).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(Count_Week + 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(Count_Week + 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(Count_Week + 3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(Count_Week + 4).ToString("MMMMd日");

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";
                }
                else if (Z3 == "木")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(Count_Week - 3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(Count_Week - 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(Count_Week - 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(Count_Week).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(Count_Week + 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(Count_Week + 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(Count_Week + 3).ToString("MMMMd日");

            

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";
                }
                else if (Z4 == "金")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(Count_Week - 4).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(Count_Week - 3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(Count_Week - 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(Count_Week - 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(Count_Week).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(Count_Week + 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(Count_Week + 2).ToString("MMMMd日");

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";
                }
                else if (Z5 == "土")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(Count_Week - 5).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(Count_Week - 4).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(Count_Week - 3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(Count_Week - 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(Count_Week - 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(Count_Week).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(Count_Week + 1).ToString("MMMMd日");

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";
                }
                else if (Z6 == "日")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(Count_Week - 6).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(Count_Week - 5).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(Count_Week - 4).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(Count_Week - 3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(Count_Week - 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(Count_Week - 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(Count_Week).ToString("MMMMd日");

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";
                }




                if (tm == "9:00" || tm == "9:15" || tm == "9:30" || tm == "9:45")
                {
                    if (week == "月")
                    {
                        string A1 = Scdl3.Items[1].Cells[1].Text;
                        A1 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[1].Text = A1.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A2 = Scdl3.Items[1].Cells[2].Text;
                        A2 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[2].Text = A2.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A3 = Scdl3.Items[1].Cells[3].Text;
                        A3 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[3].Text = A3.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A4 = Scdl3.Items[1].Cells[4].Text;
                        A4 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[4].Text = A4.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A5 = Scdl3.Items[1].Cells[5].Text;
                        A5 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[5].Text = A5.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A5 = Scdl3.Items[1].Cells[6].Text;
                        A5 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[6].Text = A5.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A5 = Scdl3.Items[1].Cells[7].Text;
                        A5 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";

                        Scdl3.Items[1].Cells[6].Text = A5.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "10:00" || tm == "10:15" || tm == "10:30" || tm == "10:45")
                {
                    if (week == "月")
                    {
                        string A6 = Scdl3.Items[2].Cells[1].Text;
                        A6 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[1].Text = A6.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A7 = Scdl3.Items[2].Cells[2].Text;
                        A7 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[2].Text = A7.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A8 = Scdl3.Items[2].Cells[3].Text;
                        A8 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[3].Text = A8.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A9 = Scdl3.Items[2].Cells[4].Text;
                        A9 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[4].Text = A9.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A10 = Scdl3.Items[2].Cells[5].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[5].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[2].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[2].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "11:00" || tm == "11:15" || tm == "11:30" || tm == "11:45")
                {
                    if (week == "月")
                    {
                        string A11 = Scdl3.Items[3].Cells[1].Text;
                        A11 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[1].Text = A11.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A12 = Scdl3.Items[3].Cells[2].Text;
                        A12 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[2].Text = A12.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A13 = Scdl3.Items[3].Cells[3].Text;
                        A13 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[3].Text = A13.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A14 = Scdl3.Items[3].Cells[4].Text;
                        A14 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[4].Text = A14.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A15 = Scdl3.Items[3].Cells[5].Text;
                        A15 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[5].Text = A15.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[3].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[3].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "12:00" || tm == "12:15" || tm == "12:30" || tm == "12:45")
                {
                    if (week == "月")
                    {
                        string A16 = Scdl3.Items[4].Cells[1].Text;
                        A16 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[1].Text = A16.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A17 = Scdl3.Items[4].Cells[2].Text;
                        A17 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[2].Text = A17.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A18 = Scdl3.Items[4].Cells[3].Text;
                        A18 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[3].Text = A18.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A19 = Scdl3.Items[4].Cells[4].Text;
                        A19 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[4].Text = A19.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A20 = Scdl3.Items[4].Cells[5].Text;
                        A20 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[5].Text = A20.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[4].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[4].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "13:00" || tm == "13:15" || tm == "13:30" || tm == "13:45")
                {
                    if (week == "月")
                    {
                        string A21 = Scdl3.Items[5].Cells[1].Text;
                        A21 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[1].Text = A21.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A22 = Scdl3.Items[5].Cells[2].Text;
                        A22 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[2].Text = A22.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A23 = Scdl3.Items[5].Cells[3].Text;
                        A23 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[3].Text = A23.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A24 = Scdl3.Items[5].Cells[4].Text;
                        A24 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[4].Text = A24.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A25 = Scdl3.Items[5].Cells[5].Text;
                        A25 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[5].Text = A25.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[5].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[5].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "14:00" || tm == "14:15" || tm == "14:30" || tm == "14:45")
                {
                    if (week == "月")
                    {
                        string A26 = Scdl3.Items[6].Cells[1].Text;
                        A26 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[1].Text = A26.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A27 = Scdl3.Items[6].Cells[2].Text;
                        A27 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[2].Text = A27.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A28 = Scdl3.Items[6].Cells[3].Text;
                        A28 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[3].Text = A28.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A29 = Scdl3.Items[6].Cells[4].Text;
                        A29 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[4].Text = A29.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A30 = Scdl3.Items[6].Cells[5].Text;
                        A30 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[5].Text = A30.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[6].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[6].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "15:00" || tm == "15:15" || tm == "15:30" || tm == "15:45")
                {
                    if (week == "月")
                    {
                        string A31 = Scdl3.Items[7].Cells[1].Text;
                        A31 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[1].Text = A31.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A32 = Scdl3.Items[7].Cells[2].Text;
                        A32 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[2].Text = A32.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A33 = Scdl3.Items[7].Cells[3].Text;
                        A33 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[3].Text = A33.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A34 = Scdl3.Items[7].Cells[4].Text;
                        A34 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[4].Text = A34.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A35 = Scdl3.Items[7].Cells[5].Text;
                        A35 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[5].Text = A35.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[7].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[7].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "16:00" || tm == "16:15" || tm == "16:30" || tm == "16:45")
                {
                    if (week == "月")
                    {
                        string A36 = Scdl3.Items[8].Cells[1].Text;
                        A36 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[1].Text = A36.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A37 = Scdl3.Items[8].Cells[2].Text;
                        A37 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[2].Text = A37.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A38 = Scdl3.Items[8].Cells[3].Text;
                        A38 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[3].Text = A38.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A39 = Scdl3.Items[8].Cells[4].Text;
                        A39 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[4].Text = A39.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A40 = Scdl3.Items[8].Cells[5].Text;
                        A40 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[5].Text = A40.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[8].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[8].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "17:00" || tm == "17:15" || tm == "17:30" || tm == "17:45")
                {
                    if (week == "月")
                    {
                        string A41 = Scdl3.Items[9].Cells[1].Text;
                        A41 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[1].Text = A41.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A42 = Scdl3.Items[9].Cells[2].Text;
                        A42 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[2].Text = A42.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A43 = Scdl3.Items[9].Cells[3].Text;
                        A43 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[3].Text = A43.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A44 = Scdl3.Items[9].Cells[4].Text;
                        A44 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[4].Text = A44.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A45 = Scdl3.Items[9].Cells[5].Text;
                        A45 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[5].Text = A45.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[9].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[9].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "18:00")
                {
                    if (week == "月")
                    {
                        string A46 = Scdl3.Items[10].Cells[1].Text;
                        A46 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[1].Text = A46.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A47 = Scdl3.Items[10].Cells[2].Text;
                        A47 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[2].Text = A47.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A48 = Scdl3.Items[10].Cells[3].Text;
                        A48 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[3].Text = A48.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A49 = Scdl3.Items[10].Cells[4].Text;
                        A49 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[4].Text = A49.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A50 = Scdl3.Items[10].Cells[5].Text;
                        A50 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[5].Text = A50.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[10].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[10].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }

                }
            }
        }



        protected void Button5_Click(object sender, EventArgs e)//次へボタンの処理
        {
            int Count_Week;
            Count_Week = int.Parse(ViewState["count"].ToString()) + 7;

            ViewState["count"] =
           int.Parse(ViewState["count"].ToString()) + 7;

            var Count = ViewState["count"];

            Create3();
            var dd = Class1.SwitchNext2Scdl3DataTable(Count, Global.GetConnection());//次の週

            for (int j = 0; j < dd.Count; j++)
            {
                var dl = dd.Rows[j] as DATASET.DataSet.T_ScheduleRow;

                DateTime DT = DateTime.Parse(dl.date.ToString());

                string week = DT.ToString("ddd");

                string tm = dl.time.Trim();

                DateTime dt = DateTime.Now;


                Scdl3.Items[0].Cells[1].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[2].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[3].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[4].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[5].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[6].Text = dt.ToString("ddd");
                Scdl3.Items[0].Cells[7].Text = dt.ToString("ddd");


                string Z0 = Scdl3.Items[0].Cells[1].Text;
                string Z1 = Scdl3.Items[0].Cells[2].Text;
                string Z2 = Scdl3.Items[0].Cells[3].Text;
                string Z3 = Scdl3.Items[0].Cells[4].Text;
                string Z4 = Scdl3.Items[0].Cells[5].Text;
                string Z5 = Scdl3.Items[0].Cells[6].Text;
                string Z6 = Scdl3.Items[0].Cells[7].Text;

                Scdl3.Items[0].Cells[0].Text = "日付";
                Scdl3.Items[1].Cells[0].Text = "9:00";
                Scdl3.Items[2].Cells[0].Text = "10:00";
                Scdl3.Items[3].Cells[0].Text = "11:00";
                Scdl3.Items[4].Cells[0].Text = "12:00";
                Scdl3.Items[5].Cells[0].Text = "13:00";
                Scdl3.Items[6].Cells[0].Text = "14:00";
                Scdl3.Items[7].Cells[0].Text = "15:00";
                Scdl3.Items[8].Cells[0].Text = "16:00";
                Scdl3.Items[9].Cells[0].Text = "17:00";
                Scdl3.Items[10].Cells[0].Text = "18:00";

                Scdl3.Items[0].Cells[0].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[1].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[2].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[3].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[4].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[5].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[6].CssClass = "Center_cs";
                Scdl3.Items[0].Cells[7].CssClass = "Center_cs";


                if (Z0 == "月")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(Count_Week).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(Count_Week + 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(Count_Week + 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(Count_Week + 3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(Count_Week + 4).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(Count_Week + 5).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(Count_Week + 6).ToString("MMMMd日");

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";

                }
                else if (Z1 == "火")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(Count_Week - 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(Count_Week).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(Count_Week + 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(Count_Week + 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(Count_Week + 3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(Count_Week + 4).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(Count_Week + 5).ToString("MMMMd日");

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";
                }
                else if (Z2 == "水")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(Count_Week - 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(Count_Week - 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(Count_Week).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(Count_Week + 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(Count_Week + 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(Count_Week + 3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(Count_Week + 4).ToString("MMMMd日");

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";
                }
                else if (Z3 == "木")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(Count_Week - 3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(Count_Week - 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(Count_Week - 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(Count_Week).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(Count_Week + 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(Count_Week + 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(Count_Week + 3).ToString("MMMMd日");

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";
                }
                else if (Z4 == "金")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(Count_Week - 4).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(Count_Week - 3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(Count_Week - 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(Count_Week - 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(Count_Week).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(Count_Week + 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(Count_Week + 2).ToString("MMMMd日");

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";
                }
                else if (Z5 == "土")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(Count_Week - 5).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(Count_Week - 4).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(Count_Week - 3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(Count_Week - 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(Count_Week - 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(Count_Week).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(Count_Week + 1).ToString("MMMMd日");

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";
                }
                else if (Z6 == "日")
                {
                    DateTime a = DateTime.Now;

                    Scdl3.Items[0].Cells[1].Text = a.AddDays(Count_Week - 6).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[2].Text = a.AddDays(Count_Week - 5).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[3].Text = a.AddDays(Count_Week - 4).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[4].Text = a.AddDays(Count_Week - 3).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[5].Text = a.AddDays(Count_Week - 2).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[6].Text = a.AddDays(Count_Week - 1).ToString("MMMMd日");
                    Scdl3.Items[0].Cells[7].Text = a.AddDays(Count_Week).ToString("MMMMd日");

                    Scdl3.Items[10].Cells[1].Text = "Test 定例会議";
                }

                if (tm == "9:00" || tm == "9:15" || tm == "9:30" || tm == "9:45")
                {
                    if (week == "月")
                    {
                        string A1 = Scdl3.Items[1].Cells[1].Text;
                        A1 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[1].Text = A1.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A2 = Scdl3.Items[1].Cells[2].Text;
                        A2 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[2].Text = A2.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A3 = Scdl3.Items[1].Cells[3].Text;
                        A3 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[3].Text = A3.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A4 = Scdl3.Items[1].Cells[4].Text;
                        A4 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[4].Text = A4.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A5 = Scdl3.Items[1].Cells[5].Text;
                        A5 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[5].Text = A5.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A5 = Scdl3.Items[1].Cells[6].Text;
                        A5 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[6].Text = A5.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A5 = Scdl3.Items[1].Cells[7].Text;
                        A5 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";

                        Scdl3.Items[1].Cells[6].Text = A5.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "10:00" || tm == "10:15" || tm == "10:30" || tm == "10:45")
                {
                    if (week == "月")
                    {
                        string A6 = Scdl3.Items[2].Cells[1].Text;
                        A6 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[1].Text = A6.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A7 = Scdl3.Items[2].Cells[2].Text;
                        A7 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[2].Text = A7.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A8 = Scdl3.Items[2].Cells[3].Text;
                        A8 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[3].Text = A8.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A9 = Scdl3.Items[2].Cells[4].Text;
                        A9 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[4].Text = A9.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A10 = Scdl3.Items[2].Cells[5].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[5].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[2].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[2].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "11:00" || tm == "11:15" || tm == "11:30" || tm == "11:45")
                {
                    if (week == "月")
                    {
                        string A11 = Scdl3.Items[3].Cells[1].Text;
                        A11 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[1].Text = A11.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A12 = Scdl3.Items[3].Cells[2].Text;
                        A12 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[2].Text = A12.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A13 = Scdl3.Items[3].Cells[3].Text;
                        A13 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[3].Text = A13.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A14 = Scdl3.Items[3].Cells[4].Text;
                        A14 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[4].Text = A14.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A15 = Scdl3.Items[3].Cells[5].Text;
                        A15 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[5].Text = A15.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[3].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[3].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "12:00" || tm == "12:15" || tm == "12:30" || tm == "12:45")
                {
                    if (week == "月")
                    {
                        string A16 = Scdl3.Items[4].Cells[1].Text;
                        A16 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[1].Text = A16.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A17 = Scdl3.Items[4].Cells[2].Text;
                        A17 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[2].Text = A17.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A18 = Scdl3.Items[4].Cells[3].Text;
                        A18 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[3].Text = A18.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A19 = Scdl3.Items[4].Cells[4].Text;
                        A19 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[4].Text = A19.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A20 = Scdl3.Items[4].Cells[5].Text;
                        A20 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[5].Text = A20.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[4].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[4].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "13:00" || tm == "13:15" || tm == "13:30" || tm == "13:45")
                {
                    if (week == "月")
                    {
                        string A21 = Scdl3.Items[5].Cells[1].Text;
                        A21 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[1].Text = A21.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A22 = Scdl3.Items[5].Cells[2].Text;
                        A22 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[2].Text = A22.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A23 = Scdl3.Items[5].Cells[3].Text;
                        A23 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[3].Text = A23.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A24 = Scdl3.Items[5].Cells[4].Text;
                        A24 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[4].Text = A24.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A25 = Scdl3.Items[5].Cells[5].Text;
                        A25 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[5].Text = A25.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[5].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[5].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "14:00" || tm == "14:15" || tm == "14:30" || tm == "14:45")
                {
                    if (week == "月")
                    {
                        string A26 = Scdl3.Items[6].Cells[1].Text;
                        A26 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[1].Text = A26.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A27 = Scdl3.Items[6].Cells[2].Text;
                        A27 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[2].Text = A27.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A28 = Scdl3.Items[6].Cells[3].Text;
                        A28 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[3].Text = A28.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A29 = Scdl3.Items[6].Cells[4].Text;
                        A29 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[4].Text = A29.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A30 = Scdl3.Items[6].Cells[5].Text;
                        A30 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[5].Text = A30.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[6].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[6].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "15:00" || tm == "15:15" || tm == "15:30" || tm == "15:45")
                {
                    if (week == "月")
                    {
                        string A31 = Scdl3.Items[7].Cells[1].Text;
                        A31 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[1].Text = A31.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A32 = Scdl3.Items[7].Cells[2].Text;
                        A32 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[2].Text = A32.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A33 = Scdl3.Items[7].Cells[3].Text;
                        A33 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[3].Text = A33.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A34 = Scdl3.Items[7].Cells[4].Text;
                        A34 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[4].Text = A34.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A35 = Scdl3.Items[7].Cells[5].Text;
                        A35 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[5].Text = A35.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[7].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[7].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "16:00" || tm == "16:15" || tm == "16:30" || tm == "16:45")
                {
                    if (week == "月")
                    {
                        string A36 = Scdl3.Items[8].Cells[1].Text;
                        A36 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[1].Text = A36.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A37 = Scdl3.Items[8].Cells[2].Text;
                        A37 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[2].Text = A37.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A38 = Scdl3.Items[8].Cells[3].Text;
                        A38 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[3].Text = A38.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A39 = Scdl3.Items[8].Cells[4].Text;
                        A39 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[4].Text = A39.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A40 = Scdl3.Items[8].Cells[5].Text;
                        A40 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[5].Text = A40.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[8].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[8].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "17:00" || tm == "17:15" || tm == "17:30" || tm == "17:45")
                {
                    if (week == "月")
                    {
                        string A41 = Scdl3.Items[9].Cells[1].Text;
                        A41 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[1].Text = A41.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A42 = Scdl3.Items[9].Cells[2].Text;
                        A42 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[2].Text = A42.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A43 = Scdl3.Items[9].Cells[3].Text;
                        A43 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[3].Text = A43.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A44 = Scdl3.Items[9].Cells[4].Text;
                        A44 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[4].Text = A44.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A45 = Scdl3.Items[9].Cells[5].Text;
                        A45 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[5].Text = A45.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[9].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[9].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "18:00")
                {
                    if (week == "月")
                    {
                        string A46 = Scdl3.Items[10].Cells[1].Text;
                        A46 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[1].Text = A46.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A47 = Scdl3.Items[10].Cells[2].Text;
                        A47 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[2].Text = A47.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A48 = Scdl3.Items[10].Cells[3].Text;
                        A48 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[3].Text = A48.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A49 = Scdl3.Items[10].Cells[4].Text;
                        A49 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[4].Text = A49.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A50 = Scdl3.Items[10].Cells[5].Text;
                        A50 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[5].Text = A50.Replace("\r\n", "<br>");
                    }
                    if (week == "土")
                    {
                        string A10 = Scdl3.Items[10].Cells[6].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[6].Text = A10.Replace("\r\n", "<br>");
                    }
                    if (week == "日")
                    {
                        string A10 = Scdl3.Items[10].Cells[7].Text;
                        A10 += dl.time + "\r\n" + dl.title + "<font color=#17a404>" + "\r\n" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[10].Cells[7].Text = A10.Replace("\r\n", "<br>");
                    }

                }
            }
        }


        protected void Button6_Click(object sender, EventArgs e)
        {
            Create3();
            Create2();
        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            Response.Redirect("Schedule.aspx");
        }

        protected void Button8_Click(object sender, EventArgs e)//検索用
        {
            ViewState["Serch"] = int.Parse(ViewState["Serch"].ToString()) + 1;

            ViewState["Count2"] = 0;

            var a = TextBox3.Text;//date

            var b = DropDownList2.SelectedValue;//time

            var c = TextBox5.Text;//title

            string d;//name

            d = "";

            foreach (ListItem item in CheckBoxList2.Items)
            {
                if (item.Selected)
                {
                    d += item.Value + " ";
                }
            }


            var dd = Class1.ScheduleSearch(a, b, c, d, Global.GetConnection());

            ScdlList.DataSource = dd;

            ScdlList.DataBind();

            Create3();
            Create2();

        }

        protected void TestGV_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    var ltr = new Literal();

            //    ltr.Text = $"総ページ数:{TestGV.PageCount}";

            //    e.Row.Cells[e.Row.Cells.Count - 1].Wrap = false;
            //    e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(ltr);


            //}
        }

        protected void Scdl3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }
        protected void ScdlList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ButtonA_Click(object sender, EventArgs e)
        {
            Response.Redirect("Wiki_Top.aspx");

        }
    }
}


