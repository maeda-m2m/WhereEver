using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing.Printing;

namespace WhereEver
{
    public partial class Schedule : System.Web.UI.Page
    {
        //ページがロードするとき
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Create();
                Panel1.Visible = false;
                Create3();
                Scdl3.Columns[0].ItemStyle.Wrap = true;
                Scdl3.Columns[1].ItemStyle.Wrap = true;
                Scdl3.Columns[2].ItemStyle.Wrap = true;
                Scdl3.Columns[3].ItemStyle.Wrap = true;
                Scdl3.Columns[4].ItemStyle.Wrap = true;
            }
        }

        //スケジュールリストにデータを格納　→　ScdlList_ItemDataBound　に移動
        private void Create()
        {
            DATASET.DataSet.T_ScheduleDataTable dt = Class1.GetT_Schedule3DataTable(Global.GetConnection());
            ScdlList.DataSource = dt;
            ScdlList.DataBind();
        }

        //スケジュール表にデータを格納　→　Scdl3_ItemDataBound に移動　→　Create2 に移動
        public void Create3()
        {
            DATASET.DataSet.T_EmptyTableDataTable dt = Class1.GetSchedule3DataTable(Global.GetConnection());

            Scdl3.DataSource = dt;
            Scdl3.DataBind();
            Create2();

        }

        //Button1を押したらPanel1が表示される
        protected void Button1_Click(object sender, EventArgs e)
        {
            Panel1.Visible = true;
        }

        //Calender1で日付をクリックしたらLabel1に表示される
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {

            Label1.Text = Calendar1.SelectedDate.ToString("yyyy/MM/dd");

            Panel1.Visible = true;
            Create();
            Create3();
        }

        //スケジュール登録ボタンを押したときの動き
        protected void Button2_Click(object sender, EventArgs e)
        {

            var dt = Class1.GetT_Schedule3DataTable(Global.GetConnection());
            var dr = dt.NewT_ScheduleRow();

            string t = DropDownList1.SelectedValue;
            //選択したリストの名前をtに入れる

            string f = (Label1.Text) + " " + (DropDownList1.SelectedValue);
            //fにLabel1の何月何日何時何分を入れる

            DateTime dd = DateTime.Parse(f);
            //fを日付型に変換しddに入れる

            dr.date = dd;
            //ddをdate列にいれる

            dr.time = t;
            //time列にtを入れる

            dr.title = TextBox1.Text;

            dr.name = DropDownList2.SelectedValue.ToString() + " " + DropDownList3.SelectedValue.ToString() + " " + DropDownList4.SelectedValue.ToString() + "\n\r";
            //選択した名前をname列に入れる

            DATASET.DataSet.T_ScheduleRow dl = Class1.MaxSdlNo(Global.GetConnection());
            //SdlNoの最大値を持ってくるためだけのコード（MaxSdlNo）をClass.1に作る

            int no = dl.SdlNo;

            dr.SdlNo = no + 1;
            //持ってきた最大値が10であれば＋１して11になる

            dt.AddT_ScheduleRow(dr);

            Class1.InsertList(dt, Global.GetConnection());
            //Class1のInsertListで行を追加

            Create();
            Panel1.Visible = false;
            Create3();
            Panel2.Visible = true;

        }

        private void Create2()
        //scdl3にスケジュールに登録した値を登録している
        {
            var dd = Class1.SwitchScdl3DataTable(Global.GetConnection());

            string E;

            for (int j = 0; j < dd.Count; j++)
            {
                var dl = dd.Rows[j] as DATASET.DataSet.T_ScheduleRow;

                DateTime DT = DateTime.Parse(dl.date.ToString());

                string week = DT.ToString("ddd");

                string tm = dl.time.Trim();

                string A;

                if (tm == "9:00" || tm == "9:15" || tm == "9:30" || tm == "9:45")
                {
                    if (week == "月")
                    {
                        string scdl = Scdl3.Items[0].Cells[1].Text;
                        if(scdl !="")
                        {
                            scdl += Environment.NewLine + dl.time + dl.title + dl.name + " " + Environment.NewLine;
                            Scdl3.Items[0].Cells[1].Text = scdl;
                        }
                        else
                        {
                            Scdl3.Items[0].Cells[1].Text = Environment.NewLine + dl.time + dl.title + dl.name + " " + Environment.NewLine;
                        }
                        //E = Environment.NewLine + dl.time + dl.title + dl.name + " " + Environment.NewLine;
                        //Scdl3.Items[0].Cells[1].Text += E;

                        //"\r\n" + Environment.NewLine
                    }

                    if (week == "火")
                        Scdl3.Items[0].Cells[2].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[0].Cells[3].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[0].Cells[4].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[0].Cells[5].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                }
                else if (tm == "10:00" || tm == "10:30")
                {
                    if (week == "月")
                        Scdl3.Items[1].Cells[1].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[1].Cells[2].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[1].Cells[3].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[1].Cells[4].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[1].Cells[5].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                }
                else if (tm == "11:00" || tm == "11:30")
                {
                    if (week == "月")
                        Scdl3.Items[2].Cells[1].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[2].Cells[2].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[2].Cells[3].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[2].Cells[4].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[2].Cells[5].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                }
                else if (tm == "12:00" || tm == "12:30")
                {
                    if (week == "月")
                        Scdl3.Items[3].Cells[1].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[3].Cells[2].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[3].Cells[3].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[3].Cells[4].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[3].Cells[5].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                }
                else if (tm == "13:00" || tm == "13:30")
                {
                    if (week == "月")
                        Scdl3.Items[4].Cells[1].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[4].Cells[2].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[4].Cells[3].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[4].Cells[4].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[4].Cells[5].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                }
                else if (tm == "14:00" || tm == "14:30")
                {
                    if (week == "月")
                        Scdl3.Items[5].Cells[1].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[5].Cells[2].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[5].Cells[3].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[5].Cells[4].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[5].Cells[5].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                }
                else if (tm == "15:00" || tm == "15:30")
                {
                    if (week == "月")
                        Scdl3.Items[6].Cells[1].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[6].Cells[2].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[6].Cells[3].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[6].Cells[4].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[6].Cells[5].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                }
                else if (tm == "16:00" || tm == "16:30")
                {
                    if (week == "月")
                        Scdl3.Items[7].Cells[1].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[7].Cells[3].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[7].Cells[4].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[7].Cells[5].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";

                }
                else if (tm == "17:00" || tm == "17:30")
                {
                    if (week == "月")
                        Scdl3.Items[8].Cells[1].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[8].Cells[2].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[8].Cells[3].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[8].Cells[4].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[8].Cells[5].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                }
                else if (tm == "18:00")
                {
                    if (week == "月")
                        Scdl3.Items[9].Cells[1].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[9].Cells[2].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[9].Cells[3].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[9].Cells[4].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[9].Cells[5].Text += dl.time + dl.title + "<font color=#16ba00>" + dl.name + "</font>";

                }
            }
        }



        protected void Scdl3_ItemDataBound(object sender, DataGridItemEventArgs e)
        //scdl3の表示の枠を作っている
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

                //必要なら使う
                //Label name1 = e.Item.FindControl("Label7") as Label;
                //Label name2 = e.Item.FindControl("Label8") as Label;
                //Label name3 = e.Item.FindControl("Label9") as Label;
                //Label name4 = e.Item.FindControl("Label10") as Label;
                //Label name5 = e.Item.FindControl("Label11") as Label;

                time.Text = dr.時間.ToString();

                if (!dr.Is月Null())
                    monday.Text = dr.月;

                if (!dr.Is火Null())
                    tuesday.Text = dr.火;


                if (!dr.Is水Null())
                    wednesday.Text = dr.水;


                if (!dr.Is木Null())
                    thursday.Text = dr.木;


                if (!dr.Is金Null())
                    friday.Text = dr.金;

            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Panel1.Visible = true;
            Panel2.Visible = false;
            Calendar1.DataBind();

            Create();
            Create3();
        }

        protected void ScdlList_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                var dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_ScheduleRow;

                Label date = e.Item.FindControl("hiduke") as Label;
                Label jikan = e.Item.FindControl("jikan") as Label;
                Label title = e.Item.FindControl("taitoru") as Label;
                Label name = e.Item.FindControl("namae") as Label;

                //Label No = e.Item.FindControl("nanba") as Label;

                if (!dr.IsdateNull())
                    date.Text = dr.date.ToString();

                if (!dr.IstimeNull())
                    jikan.Text = dr.time.ToString();

                if (!dr.IstitleNull())
                    title.Text = dr.title;

                if (!dr.IsnameNull())
                    name.Text = dr.name;

                //No.Text = dr.SdlNo.ToString();
                //値を隠している

            }
        }

        //削除ボタンの処理
        protected void ScdlList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            int a = e.Item.ItemIndex;
            var dt = Class1.GetT_Schedule3DataTable(Global.GetConnection());
            var dr = dt.Rows[a] as DATASET.DataSet.T_ScheduleRow;
            string.Format("if (!confirm('{0}')) return false;", "削除しますか。");
            int sdl = dr.SdlNo;

            if (e.CommandName == "Delete")
            {
                if (sdl > 0)
                    Class1.DeleteList(sdl, Global.GetConnection());
                ScdlList.Items[a].FindControl("No");
                Create();
                Create3();
            }
            else
            {

            }
        }

        protected void Button1_Click1(object sender, System.EventArgs e)
        {
            Response.Redirect("PrintSchedule.aspx");

        }

        protected void Scdl3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }


}
