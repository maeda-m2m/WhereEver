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
                Create2();
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

        //スケジュール表にデータを格納　→　Scdl3_ItemDataBound　に移動　→　Create2 に移動
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
            // DataSet1.T_Schedule3Row dl = Class1.SwitchScdl3Row(Global.GetConnection());

            Label1.Text = Calendar1.SelectedDate.ToString("yyyy/MM/dd");
            // string s = Label1.Text;
            Panel1.Visible = true;
        }

        //スケジュール登録ボタンを押したときの動き
        protected void Button2_Click(object sender, EventArgs e)
        {

            var dt = Class1.GetT_Schedule3DataTable(Global.GetConnection());
            var dr = dt.NewT_ScheduleRow();

            string t = DropDownList1.SelectedValue;　　　　　　　　　　　　　//選択したリストのValue（名前？）tに入れる
            DateTime.Parse(t);　　　　　　　　　　　　　　　　　　　　　　　//tを日付型に変換①
            string f = Label1.Text;　　　　　　　　　　　　　　　　　　　　//fにLabel1の文字を入れる
            DateTime dd = DateTime.Parse(f);　　　　　　　　　　　　　　　//fを日付型に変換しddに入れる
            dr.date = dd;　　　　　　　　　　　　　　　　　　　　　　　　//ddをdate列にいれる
            dr.time = t;　　　　　　　　　　　　　　　　//time列にtを入れる（①がいらない説）
            dr.title = TextBox1.Text;
            dr.name = DropDownList2.SelectedValue.ToString();　　　　 //選択した名前をname列に入れる


            DATASET.DataSet.T_ScheduleRow dl = Class1.MaxSdlNo(Global.GetConnection());　　
            //SdlNoの最大値を持ってくるためだけのコード（MaxSdlNo）をClass.1に作る

            int no = int.Parse(dl.SdlNo);

            dr.SdlNo = (no + 1).ToString();　　
            //持ってきた最大値が10であれば＋１して11になる

            dt.AddT_ScheduleRow(dr);

            Class1.InsertList(dt, Global.GetConnection());           
            //Class1のInsertListで行を追加

            Create();
            Panel1.Visible = false;
        }

        private void Create2()
        {

            DATASET.DataSet.T_ScheduleDataTable dd = Class1.SwitchScdl3DataTable(Global.GetConnection());

            for (int j = 0; j < dd.Count; j++)
            {
                DATASET.DataSet.T_ScheduleRow dl = dd.Rows[j] as DATASET.DataSet.T_ScheduleRow;

                DateTime DT = DateTime.Parse(dl.date.ToString());
                string week = DT.ToString("ddd");

                string tm = dl.time.Trim();

                if (tm == "9:00")
                {
                    if (week == "月")
                        Scdl3.Items[0].Cells[1].Text = dl.title + Environment.NewLine + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[0].Cells[2].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[0].Cells[3].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[0].Cells[4].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[0].Cells[5].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                }
                else if (tm == "10:00")
                {
                    if (week == "月")
                        Scdl3.Items[1].Cells[1].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[1].Cells[2].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[1].Cells[3].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[1].Cells[4].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[1].Cells[5].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                }
                else if (tm == "11:00")
                {
                    if (week == "月")
                        Scdl3.Items[2].Cells[1].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[2].Cells[2].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[2].Cells[3].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[2].Cells[4].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[2].Cells[5].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                }
                else if (tm == "12:00")
                {
                    if (week == "月")
                        Scdl3.Items[3].Cells[1].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[3].Cells[2].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[3].Cells[3].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[3].Cells[4].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[3].Cells[5].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                }
                else if (tm == "13:00")
                {
                    if (week == "月")
                        Scdl3.Items[4].Cells[1].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[4].Cells[2].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[4].Cells[3].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[4].Cells[4].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[4].Cells[5].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                }
                else if (tm == "14:00")
                {
                    if (week == "月")
                        Scdl3.Items[5].Cells[1].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[5].Cells[2].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[5].Cells[3].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[5].Cells[4].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[5].Cells[5].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                }
                else if (tm == "15:00")
                {
                    if (week == "月")
                        Scdl3.Items[6].Cells[1].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[6].Cells[2].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[6].Cells[3].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[6].Cells[4].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[6].Cells[5].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                }
                else if (tm == "16:00")
                {
                    if (week == "月")
                        Scdl3.Items[7].Cells[1].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[7].Cells[2].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[7].Cells[3].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[7].Cells[4].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[7].Cells[5].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";

                }
                else if (tm == "17:00")
                {
                    if (week == "月")
                        Scdl3.Items[8].Cells[1].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[8].Cells[2].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[8].Cells[3].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[8].Cells[4].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[8].Cells[5].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                }
                else if (tm == "18:00")
                {
                    if (week == "月")
                        Scdl3.Items[9].Cells[1].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "火")
                        Scdl3.Items[9].Cells[2].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "水")
                        Scdl3.Items[9].Cells[3].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "木")
                        Scdl3.Items[9].Cells[4].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";
                    if (week == "金")
                        Scdl3.Items[9].Cells[5].Text = dl.title + "\r\n" + "<font color=#16ba00>" + dl.name + "</font>";

                }
            }
        }



        protected void Scdl3_ItemDataBound(object sender, DataGridItemEventArgs e)
        {　　//未使用
            //DATASET.DataSet.T_ScheduleDataTable dd = Class1.GetT_Schedule3DataTable(Global.GetConnection());
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                var dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_EmptyTableRow;

                Label time = e.Item.FindControl("Jikan") as Label;
                Label monday = e.Item.FindControl("MondayTitle") as Label;
                Label name1 = e.Item.FindControl("Label7") as Label;
                Label tuesday = e.Item.FindControl("TuesdayTitle") as Label;
                Label name2 = e.Item.FindControl("Label8") as Label;
                Label wednesday = e.Item.FindControl("WednesdayTitle") as Label;
                Label name3 = e.Item.FindControl("Label9") as Label;
                Label thursday = e.Item.FindControl("ThursdayTitle") as Label;
                Label name4 = e.Item.FindControl("Label10") as Label;
                Label friday = e.Item.FindControl("FridayTitle") as Label;
                Label name5 = e.Item.FindControl("Label11") as Label;

                //time.Text = dr.time.ToString();

                //if (!dr.IstitleNull())
                //    monday.Text = dr.title;

                //if (!dr.IsnameNull())
                //    name1.Text = dr.title;

                //if (!dr.IstitleNull())
                //    tuesday.Text = dr.title;

                //if (!dr.IsnameNull())
                //    name2.Text = dr.name;

                //if (!dr.IstitleNull())
                //    wednesday.Text = dr.title;

                //if (!dr.IsnameNull())
                //    name3.Text = dr.title;

                //if (!dr.IstitleNull())
                //    thursday.Text = dr.title;

                //if (!dr.IsnameNull())
                //    name4.Text = dr.name;

                //if (!dr.IstitleNull())
                //    friday.Text = dr.title;

                //if (!dr.IsnameNull())
                //    name5.Text = dr.name;
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Panel1.Visible = true;
            //未使用
            //DATASET.DataSet.T_ScheduleDataTable dt = Class1.SwitchScdl3DataTable(Global.GetConnection());

            Calendar1.DataBind();

        }

        protected void ScdlList_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                var dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_EmptyTableRow;

                Label date = e.Item.FindControl("hiduke") as Label;
                Label jikan = e.Item.FindControl("jikan") as Label;
                Label title = e.Item.FindControl("taitoru") as Label;
                Label name = e.Item.FindControl("namae") as Label;
                Label No = e.Item.FindControl("nanba") as Label;

                //if (!dr.IsdateNull())
                //    date.Text = dr.date.ToString();

                //if (!dr.IstimeNull())
                //    jikan.Text = dr.time.ToString();

                //if (!dr.IstitleNull())
                //    title.Text = dr.title;

                //if (!dr.IsnameNull())
                //    name.Text = dr.name;

                //No.Text = dr.SdlNo.ToString();
            }
        }

        protected void ScdlList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            int a = e.Item.ItemIndex;
            DATASET.DataSet.T_ScheduleDataTable dt = Class1.SwitchScdl3DataTable(Global.GetConnection());
            DATASET.DataSet.T_ScheduleRow dr = dt.Rows[a] as DATASET.DataSet.T_ScheduleRow;//T_Schedule3のa行目を取って来る
            string.Format("if (!confirm('{0}')) return false;", "削除しますか。");
            int sdl = int.Parse(dr.SdlNo);//とってきたSdlNoをsdlに入れる

            if (sdl > 0)//sdlが0より大きい場合
                Class1.DeleteList(sdl, Global.GetConnection());
            ScdlList.Items[a].FindControl("No");
            Create();
        }

        protected void Button1_Click1(object sender, System.EventArgs e)
        {
            Response.Redirect("PrintSchedule.aspx");

        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
