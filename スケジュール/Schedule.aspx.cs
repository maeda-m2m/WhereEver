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
        public DataGridCommandEventHandler Scdl_CancelCommand { get; private set; }


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
            //ScdlList.EditCommand +=
            //new DataGridCommandEventHandler(this.ScdlList_EditCommand);
            //ScdlList.CancelCommand +=
            //    new DataGridCommandEventHandler(this.ScdlList_CancelCommand);
            //ScdlList.UpdateCommand +=
            //    new DataGridCommandEventHandler(this.ScdlList_UpdateCommand);
            //ScdlList.ItemCommand +=
            //    new DataGridCommandEventHandler(this.ScdlList_ItemCommand);
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
        }

        //Calender1で日付をクリックしたらLabel1に表示される
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {

            Label1.Text = Calendar1.SelectedDate.ToString("yyyy/MM/dd");

            Panel1.Visible = true;
            Create();
            Create3();
            Create2();
        }


        //Calendarに予定を表示するためのクラス
        protected void CalendarA(object sender, EventArgs e)
        {
            Create();
            Create3();
            Create2();
        }


        //スケジュール登録ボタンを押したときの動き
        protected void Button2_Click(object sender, EventArgs e)
        {

            var dt = Class1.GetT_Schedule3DataTable(Global.GetConnection());

            var dr = dt.NewT_ScheduleRow();

            string t = DropDownList1.SelectedValue;

            string f = (Label1.Text) + " " + (DropDownList1.SelectedValue);

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

            DATASET.DataSet.T_ScheduleRow dl = Class1.MaxSdlNo(Global.GetConnection());

            int no = dl.SdlNo;

            dr.SdlNo = no + 1;

            dt.AddT_ScheduleRow(dr);

            Class1.InsertList(dt, Global.GetConnection());

            Create();
            Panel1.Visible = false;
            Create3();
            Panel2.Visible = true;
            Create2();

        }

        private void Create2()//scdl3にスケジュールに登録した値を登録している（今週分）
        {
            var dd = Class1.SwitchScdl3DataTable(Global.GetConnection());

            for (int j = 0; j < dd.Count; j++)
            {
                var dl = dd.Rows[j] as DATASET.DataSet.T_ScheduleRow;

                DateTime DT = DateTime.Parse(dl.date.ToString());

                string week = DT.ToString("ddd");

                string tm = dl.time.Trim();

                if (tm == "9:00" || tm == "9:15" || tm == "9:30" || tm == "9:45")
                {
                    if (week == "月")
                    {
                        string A1 = Scdl3.Items[0].Cells[1].Text;
                        A1 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[0].Cells[1].Text = A1.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A2 = Scdl3.Items[0].Cells[2].Text;
                        A2 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[0].Cells[2].Text = A2.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A3 = Scdl3.Items[0].Cells[3].Text;
                        A3 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[0].Cells[3].Text = A3.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A4 = Scdl3.Items[0].Cells[4].Text;
                        A4 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[0].Cells[4].Text = A4.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A5 = Scdl3.Items[0].Cells[5].Text;
                        A5 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[0].Cells[5].Text = A5.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "10:00" || tm == "10:15" || tm == "10:30" || tm == "10:45")
                {
                    if (week == "月")
                    {
                        string A6 = Scdl3.Items[1].Cells[1].Text;
                        A6 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[1].Text = A6.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A7 = Scdl3.Items[1].Cells[2].Text;
                        A7 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[2].Text = A7.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A8 = Scdl3.Items[1].Cells[3].Text;
                        A8 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[3].Text = A8.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A9 = Scdl3.Items[1].Cells[4].Text;
                        A9 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[4].Text = A9.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A10 = Scdl3.Items[1].Cells[5].Text;
                        A10 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[5].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "11:00" || tm == "11:15" || tm == "11:30" || tm == "11:45")
                {
                    if (week == "月")
                    {
                        string A11 = Scdl3.Items[2].Cells[1].Text;
                        A11 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[1].Text = A11.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A12 = Scdl3.Items[2].Cells[2].Text;
                        A12 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[2].Text = A12.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A13 = Scdl3.Items[2].Cells[3].Text;
                        A13 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[3].Text = A13.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A14 = Scdl3.Items[2].Cells[4].Text;
                        A14 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[4].Text = A14.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A15 = Scdl3.Items[2].Cells[5].Text;
                        A15 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[5].Text = A15.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "12:00" || tm == "12:15" || tm == "12:30" || tm == "12:45")
                {
                    if (week == "月")
                    {
                        string A16 = Scdl3.Items[3].Cells[1].Text;
                        A16 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[1].Text = A16.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A17 = Scdl3.Items[3].Cells[2].Text;
                        A17 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[2].Text = A17.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A18 = Scdl3.Items[3].Cells[3].Text;
                        A18 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[3].Text = A18.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A19 = Scdl3.Items[3].Cells[4].Text;
                        A19 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[4].Text = A19.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A20 = Scdl3.Items[3].Cells[5].Text;
                        A20 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[5].Text = A20.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "13:00" || tm == "13:15" || tm == "13:30" || tm == "13:45")
                {
                    if (week == "月")
                    {
                        string A21 = Scdl3.Items[4].Cells[1].Text;
                        A21 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[1].Text = A21.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A22 = Scdl3.Items[4].Cells[2].Text;
                        A22 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[2].Text = A22.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A23 = Scdl3.Items[4].Cells[3].Text;
                        A23 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[3].Text = A23.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A24 = Scdl3.Items[4].Cells[4].Text;
                        A24 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[4].Text = A24.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A25 = Scdl3.Items[4].Cells[5].Text;
                        A25 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[5].Text = A25.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "14:00" || tm == "14:15" || tm == "14:30" || tm == "14:45")
                {
                    if (week == "月")
                    {
                        string A26 = Scdl3.Items[5].Cells[1].Text;
                        A26 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[1].Text = A26.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A27 = Scdl3.Items[5].Cells[2].Text;
                        A27 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[2].Text = A27.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A28 = Scdl3.Items[5].Cells[3].Text;
                        A28 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[3].Text = A28.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A29 = Scdl3.Items[5].Cells[4].Text;
                        A29 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[4].Text = A29.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A30 = Scdl3.Items[5].Cells[5].Text;
                        A30 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[5].Text = A30.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "15:00" || tm == "15:15" || tm == "15:30" || tm == "15:45")
                {
                    if (week == "月")
                    {
                        string A31 = Scdl3.Items[6].Cells[1].Text;
                        A31 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[1].Text = A31.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A32 = Scdl3.Items[6].Cells[2].Text;
                        A32 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[2].Text = A32.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A33 = Scdl3.Items[6].Cells[3].Text;
                        A33 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[3].Text = A33.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A34 = Scdl3.Items[6].Cells[4].Text;
                        A34 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[4].Text = A34.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A35 = Scdl3.Items[6].Cells[5].Text;
                        A35 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[5].Text = A35.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "16:00" || tm == "16:15" || tm == "16:30" || tm == "16:45")
                {
                    if (week == "月")
                    {
                        string A36 = Scdl3.Items[7].Cells[1].Text;
                        A36 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[1].Text = A36.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A37 = Scdl3.Items[7].Cells[2].Text;
                        A37 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[2].Text = A37.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A38 = Scdl3.Items[7].Cells[3].Text;
                        A38 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[3].Text = A38.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A39 = Scdl3.Items[7].Cells[4].Text;
                        A39 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[4].Text = A39.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A40 = Scdl3.Items[7].Cells[5].Text;
                        A40 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[5].Text = A40.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "17:00" || tm == "17:15" || tm == "17:30" || tm == "17:45")
                {
                    if (week == "月")
                    {
                        string A41 = Scdl3.Items[8].Cells[1].Text;
                        A41 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[1].Text = A41.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A42 = Scdl3.Items[8].Cells[2].Text;
                        A42 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[2].Text = A42.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A43 = Scdl3.Items[8].Cells[3].Text;
                        A43 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[3].Text = A43.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A44 = Scdl3.Items[8].Cells[4].Text;
                        A44 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[4].Text = A44.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A45 = Scdl3.Items[8].Cells[5].Text;
                        A45 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[5].Text = A45.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "18:00")
                {
                    if (week == "月")
                    {
                        string A46 = Scdl3.Items[9].Cells[1].Text;
                        A46 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[1].Text = A46.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A47 = Scdl3.Items[9].Cells[2].Text;
                        A47 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[2].Text = A47.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A48 = Scdl3.Items[9].Cells[3].Text;
                        A48 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[3].Text = A48.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A49 = Scdl3.Items[9].Cells[4].Text;
                        A49 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[4].Text = A49.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A50 = Scdl3.Items[9].Cells[5].Text;
                        A50 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[5].Text = A50.Replace("\r\n", "<br>");
                    }

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

                //var dd = Class1.SwitchScdl3DataTable(Global.GetConnection());

                //for (int j = 0; j < dd.Count; j++)
                //{
                //    var dl = dd.Rows[j] as DATASET.DataSet.T_ScheduleRow;

                //    DateTime DT = DateTime.Parse(dl.date.ToString());

                //    string week = DT.ToString("MM/dd");
                //}


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
            Create2();
        }

        protected void ScdlList_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                var dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_ScheduleRow;

                //Label date = e.Item.FindControl("hiduke") as Label;
                //Label jikan = e.Item.FindControl("jikan") as Label;
                //Label title = e.Item.FindControl("taitoru") as Label;
                //Label name = e.Item.FindControl("namae") as Label;
                //Label No = e.Item.FindControl("nanba") as Label;

                //if (!dr.IsdateNull())
                //    date.Text = dr.date.ToString();

                //if (!dr.IstimeNull())
                //    jikan.Text = dr.time.ToString();

                //if (!dr.IstitleNull())
                //    title.Text = dr.title;

                //if (!dr.IsnameNull())
                //    name.Text = dr.name;

                //No.Text = dr.SdlNo.ToString();



                e.Item.Cells[0].Text = dr.date.ToString("yyyy/MM/dd") + " " + dr.date.ToString("dddd");
                e.Item.Cells[1].Text = dr.time.ToString();
                e.Item.Cells[2].Text = dr.title.ToString();
                e.Item.Cells[3].Text = dr.name.ToString();
                e.Item.Cells[4].Text = dr.SdlNo.ToString();


            }
        }

        //削除ボタンの処理
        protected void ScdlList_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            int a = e.Item.ItemIndex;
            var dt = Class1.GetT_Schedule3DataTable(Global.GetConnection());
            var dr = dt.Rows[a] as DATASET.DataSet.T_ScheduleRow;
            int sdl = dr.SdlNo;

            if (e.CommandName == "Delete")
            {
                if (sdl > 0)
                    Class1.DeleteList(sdl, Global.GetConnection());
                ScdlList.Items[a].FindControl("No");
                Create();
                Create3();
                Create2();
            }
            else
            {

            }
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            Response.Redirect("PrintSchedule.aspx");
        }

        protected void Scdl3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        protected void ScdlList_EditCommand(object sender, DataGridCommandEventArgs e)
        {

            ScdlList.EditItemIndex = e.Item.ItemIndex;
            var dt = Class1.GetT_Schedule3DataTable(Global.GetConnection());
            ScdlList.DataSource = dt;
            ScdlList.DataBind();

            Create();
            Create3();
            Create2();
        }


        protected void ScdlList_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            ScdlList.EditItemIndex = -1;
            ScdlList.DataSource = Class1.GetT_Schedule3DataTable(Global.GetConnection());
            ScdlList.DataBind();

            Create();
            Create3();
            Create2();
        }

        protected void ScdlList_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            TextBox a1 = (TextBox)e.Item.Cells[0].Controls[0];
            TextBox a2 = (TextBox)e.Item.Cells[1].Controls[0];
            TextBox a3 = (TextBox)e.Item.Cells[2].Controls[0];
            TextBox a4 = (TextBox)e.Item.Cells[3].Controls[0];
            TextBox a5 = (TextBox)e.Item.Cells[4].Controls[0];

            string b1 = a1.Text.Trim();
            string b2 = a2.Text.Trim();
            string b3 = a3.Text.Trim();
            string b4 = a4.Text.Trim();
            string b5 = a5.Text.Trim();

            var dt = Class1.GetT_Schedule3DataTable(Global.GetConnection());
            int a = e.Item.ItemIndex;
            var dr = dt.Rows[a] as DATASET.DataSet.T_ScheduleRow;

            dr[0] = b1.Trim();
            dr[1] = b2.Trim();
            dr[2] = b3.Trim();
            dr[3] = b4.Trim();
            dr[4] = b5.Trim();

            UpdateProject(dr, Global.GetConnection());

            ScdlList.EditItemIndex = -1;
            ScdlList.DataSource = Class1.GetT_Schedule3DataTable(Global.GetConnection());
            ScdlList.DataBind();

            Create();
            Create3();
            Create2();

        }

        private void UpdateProject(DATASET.DataSet.T_ScheduleRow dr, SqlConnection sql)
        {
            {
                var a = new SqlCommand("", sql);

                a.CommandText = "UPDATE T_Schedule SET [date] = @date, [time] =@time, [title] = @title, [name] = @name where [SdlNo] = @SdlNo";

                a.Parameters.AddWithValue("@date", dr.date);
                a.Parameters.AddWithValue("@time", dr.time);
                a.Parameters.AddWithValue("@title", dr.title);
                a.Parameters.AddWithValue("@name", dr.name);
                a.Parameters.AddWithValue("@SdlNo", dr.SdlNo);

                SqlTransaction sqltra = null;

                try
                {
                    sql.Open();
                    sqltra = sql.BeginTransaction();

                    a.Transaction = sqltra;

                    a.ExecuteNonQuery();

                    sqltra.Commit();

                }
                finally
                {
                    sql.Close();
                }
                sql.Open();
                a.ExecuteNonQuery();
                sql.Close();
            }
        }


        protected void ScdlList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            Create3();
            var dd = Class1.SwitchNextScdl3DataTable(Global.GetConnection());

            for (int j = 0; j < dd.Count; j++)
            {
                var dl = dd.Rows[j] as DATASET.DataSet.T_ScheduleRow;

                DateTime DT = DateTime.Parse(dl.date.ToString());

                string week = DT.ToString("ddd");

                string tm = dl.time.Trim();

                if (tm == "9:00" || tm == "9:15" || tm == "9:30" || tm == "9:45")
                {
                    if (week == "月")
                    {
                        string A1 = Scdl3.Items[0].Cells[1].Text;
                        A1 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[0].Cells[1].Text = A1.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A2 = Scdl3.Items[0].Cells[2].Text;
                        A2 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[0].Cells[2].Text = A2.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A3 = Scdl3.Items[0].Cells[3].Text;
                        A3 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[0].Cells[3].Text = A3.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A4 = Scdl3.Items[0].Cells[4].Text;
                        A4 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[0].Cells[4].Text = A4.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A5 = Scdl3.Items[0].Cells[5].Text;
                        A5 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[0].Cells[5].Text = A5.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "10:00" || tm == "10:15" || tm == "10:30" || tm == "10:45")
                {
                    if (week == "月")
                    {
                        string A6 = Scdl3.Items[1].Cells[1].Text;
                        A6 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[1].Text = A6.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A7 = Scdl3.Items[1].Cells[2].Text;
                        A7 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[2].Text = A7.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A8 = Scdl3.Items[1].Cells[3].Text;
                        A8 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[3].Text = A8.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A9 = Scdl3.Items[1].Cells[4].Text;
                        A9 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[4].Text = A9.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A10 = Scdl3.Items[1].Cells[5].Text;
                        A10 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[5].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "11:00" || tm == "11:15" || tm == "11:30" || tm == "11:45")
                {
                    if (week == "月")
                    {
                        string A11 = Scdl3.Items[2].Cells[1].Text;
                        A11 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[1].Text = A11.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A12 = Scdl3.Items[2].Cells[2].Text;
                        A12 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[2].Text = A12.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A13 = Scdl3.Items[2].Cells[3].Text;
                        A13 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[3].Text = A13.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A14 = Scdl3.Items[2].Cells[4].Text;
                        A14 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[4].Text = A14.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A15 = Scdl3.Items[2].Cells[5].Text;
                        A15 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[5].Text = A15.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "12:00" || tm == "12:15" || tm == "12:30" || tm == "12:45")
                {
                    if (week == "月")
                    {
                        string A16 = Scdl3.Items[3].Cells[1].Text;
                        A16 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[1].Text = A16.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A17 = Scdl3.Items[3].Cells[2].Text;
                        A17 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[2].Text = A17.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A18 = Scdl3.Items[3].Cells[3].Text;
                        A18 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[3].Text = A18.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A19 = Scdl3.Items[3].Cells[4].Text;
                        A19 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[4].Text = A19.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A20 = Scdl3.Items[3].Cells[5].Text;
                        A20 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[5].Text = A20.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "13:00" || tm == "13:15" || tm == "13:30" || tm == "13:45")
                {
                    if (week == "月")
                    {
                        string A21 = Scdl3.Items[4].Cells[1].Text;
                        A21 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[1].Text = A21.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A22 = Scdl3.Items[4].Cells[2].Text;
                        A22 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[2].Text = A22.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A23 = Scdl3.Items[4].Cells[3].Text;
                        A23 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[3].Text = A23.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A24 = Scdl3.Items[4].Cells[4].Text;
                        A24 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[4].Text = A24.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A25 = Scdl3.Items[4].Cells[5].Text;
                        A25 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[5].Text = A25.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "14:00" || tm == "14:15" || tm == "14:30" || tm == "14:45")
                {
                    if (week == "月")
                    {
                        string A26 = Scdl3.Items[5].Cells[1].Text;
                        A26 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[1].Text = A26.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A27 = Scdl3.Items[5].Cells[2].Text;
                        A27 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[2].Text = A27.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A28 = Scdl3.Items[5].Cells[3].Text;
                        A28 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[3].Text = A28.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A29 = Scdl3.Items[5].Cells[4].Text;
                        A29 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[4].Text = A29.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A30 = Scdl3.Items[5].Cells[5].Text;
                        A30 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[5].Text = A30.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "15:00" || tm == "15:15" || tm == "15:30" || tm == "15:45")
                {
                    if (week == "月")
                    {
                        string A31 = Scdl3.Items[6].Cells[1].Text;
                        A31 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[1].Text = A31.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A32 = Scdl3.Items[6].Cells[2].Text;
                        A32 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[2].Text = A32.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A33 = Scdl3.Items[6].Cells[3].Text;
                        A33 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[3].Text = A33.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A34 = Scdl3.Items[6].Cells[4].Text;
                        A34 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[4].Text = A34.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A35 = Scdl3.Items[6].Cells[5].Text;
                        A35 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[5].Text = A35.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "16:00" || tm == "16:15" || tm == "16:30" || tm == "16:45")
                {
                    if (week == "月")
                    {
                        string A36 = Scdl3.Items[7].Cells[1].Text;
                        A36 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[1].Text = A36.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A37 = Scdl3.Items[7].Cells[2].Text;
                        A37 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[2].Text = A37.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A38 = Scdl3.Items[7].Cells[3].Text;
                        A38 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[3].Text = A38.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A39 = Scdl3.Items[7].Cells[4].Text;
                        A39 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[4].Text = A39.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A40 = Scdl3.Items[7].Cells[5].Text;
                        A40 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[5].Text = A40.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "17:00" || tm == "17:15" || tm == "17:30" || tm == "17:45")
                {
                    if (week == "月")
                    {
                        string A41 = Scdl3.Items[8].Cells[1].Text;
                        A41 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[1].Text = A41.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A42 = Scdl3.Items[8].Cells[2].Text;
                        A42 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[2].Text = A42.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A43 = Scdl3.Items[8].Cells[3].Text;
                        A43 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[3].Text = A43.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A44 = Scdl3.Items[8].Cells[4].Text;
                        A44 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[4].Text = A44.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A45 = Scdl3.Items[8].Cells[5].Text;
                        A45 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[5].Text = A45.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "18:00")
                {
                    if (week == "月")
                    {
                        string A46 = Scdl3.Items[9].Cells[1].Text;
                        A46 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[1].Text = A46.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A47 = Scdl3.Items[9].Cells[2].Text;
                        A47 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[2].Text = A47.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A48 = Scdl3.Items[9].Cells[3].Text;
                        A48 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[3].Text = A48.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A49 = Scdl3.Items[9].Cells[4].Text;
                        A49 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[4].Text = A49.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A50 = Scdl3.Items[9].Cells[5].Text;
                        A50 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[5].Text = A50.Replace("\r\n", "<br>");
                    }

                }

            }
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            Create3();
            var dd = Class1.SwitchNext2Scdl3DataTable(Global.GetConnection());

            for (int j = 0; j < dd.Count; j++)
            {
                var dl = dd.Rows[j] as DATASET.DataSet.T_ScheduleRow;

                DateTime DT = DateTime.Parse(dl.date.ToString());

                string week = DT.ToString("ddd");

                string tm = dl.time.Trim();

                if (tm == "9:00" || tm == "9:15" || tm == "9:30" || tm == "9:45")
                {
                    if (week == "月")
                    {
                        string A1 = Scdl3.Items[0].Cells[1].Text;
                        A1 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[0].Cells[1].Text = A1.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A2 = Scdl3.Items[0].Cells[2].Text;
                        A2 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[0].Cells[2].Text = A2.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A3 = Scdl3.Items[0].Cells[3].Text;
                        A3 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[0].Cells[3].Text = A3.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A4 = Scdl3.Items[0].Cells[4].Text;
                        A4 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[0].Cells[4].Text = A4.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A5 = Scdl3.Items[0].Cells[5].Text;
                        A5 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[0].Cells[5].Text = A5.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "10:00" || tm == "10:15" || tm == "10:30" || tm == "10:45")
                {
                    if (week == "月")
                    {
                        string A6 = Scdl3.Items[1].Cells[1].Text;
                        A6 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[1].Text = A6.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A7 = Scdl3.Items[1].Cells[2].Text;
                        A7 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[2].Text = A7.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A8 = Scdl3.Items[1].Cells[3].Text;
                        A8 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[3].Text = A8.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A9 = Scdl3.Items[1].Cells[4].Text;
                        A9 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[4].Text = A9.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A10 = Scdl3.Items[1].Cells[5].Text;
                        A10 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[1].Cells[5].Text = A10.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "11:00" || tm == "11:15" || tm == "11:30" || tm == "11:45")
                {
                    if (week == "月")
                    {
                        string A11 = Scdl3.Items[2].Cells[1].Text;
                        A11 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[1].Text = A11.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A12 = Scdl3.Items[2].Cells[2].Text;
                        A12 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[2].Text = A12.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A13 = Scdl3.Items[2].Cells[3].Text;
                        A13 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[3].Text = A13.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A14 = Scdl3.Items[2].Cells[4].Text;
                        A14 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[4].Text = A14.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A15 = Scdl3.Items[2].Cells[5].Text;
                        A15 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[2].Cells[5].Text = A15.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "12:00" || tm == "12:15" || tm == "12:30" || tm == "12:45")
                {
                    if (week == "月")
                    {
                        string A16 = Scdl3.Items[3].Cells[1].Text;
                        A16 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[1].Text = A16.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A17 = Scdl3.Items[3].Cells[2].Text;
                        A17 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[2].Text = A17.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A18 = Scdl3.Items[3].Cells[3].Text;
                        A18 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[3].Text = A18.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A19 = Scdl3.Items[3].Cells[4].Text;
                        A19 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[4].Text = A19.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A20 = Scdl3.Items[3].Cells[5].Text;
                        A20 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[3].Cells[5].Text = A20.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "13:00" || tm == "13:15" || tm == "13:30" || tm == "13:45")
                {
                    if (week == "月")
                    {
                        string A21 = Scdl3.Items[4].Cells[1].Text;
                        A21 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[1].Text = A21.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A22 = Scdl3.Items[4].Cells[2].Text;
                        A22 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[2].Text = A22.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A23 = Scdl3.Items[4].Cells[3].Text;
                        A23 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[3].Text = A23.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A24 = Scdl3.Items[4].Cells[4].Text;
                        A24 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[4].Text = A24.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A25 = Scdl3.Items[4].Cells[5].Text;
                        A25 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[4].Cells[5].Text = A25.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "14:00" || tm == "14:15" || tm == "14:30" || tm == "14:45")
                {
                    if (week == "月")
                    {
                        string A26 = Scdl3.Items[5].Cells[1].Text;
                        A26 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[1].Text = A26.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A27 = Scdl3.Items[5].Cells[2].Text;
                        A27 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[2].Text = A27.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A28 = Scdl3.Items[5].Cells[3].Text;
                        A28 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[3].Text = A28.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A29 = Scdl3.Items[5].Cells[4].Text;
                        A29 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[4].Text = A29.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A30 = Scdl3.Items[5].Cells[5].Text;
                        A30 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[5].Cells[5].Text = A30.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "15:00" || tm == "15:15" || tm == "15:30" || tm == "15:45")
                {
                    if (week == "月")
                    {
                        string A31 = Scdl3.Items[6].Cells[1].Text;
                        A31 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[1].Text = A31.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A32 = Scdl3.Items[6].Cells[2].Text;
                        A32 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[2].Text = A32.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A33 = Scdl3.Items[6].Cells[3].Text;
                        A33 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[3].Text = A33.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A34 = Scdl3.Items[6].Cells[4].Text;
                        A34 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[4].Text = A34.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A35 = Scdl3.Items[6].Cells[5].Text;
                        A35 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[6].Cells[5].Text = A35.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "16:00" || tm == "16:15" || tm == "16:30" || tm == "16:45")
                {
                    if (week == "月")
                    {
                        string A36 = Scdl3.Items[7].Cells[1].Text;
                        A36 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[1].Text = A36.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A37 = Scdl3.Items[7].Cells[2].Text;
                        A37 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[2].Text = A37.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A38 = Scdl3.Items[7].Cells[3].Text;
                        A38 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[3].Text = A38.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A39 = Scdl3.Items[7].Cells[4].Text;
                        A39 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[4].Text = A39.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A40 = Scdl3.Items[7].Cells[5].Text;
                        A40 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[7].Cells[5].Text = A40.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "17:00" || tm == "17:15" || tm == "17:30" || tm == "17:45")
                {
                    if (week == "月")
                    {
                        string A41 = Scdl3.Items[8].Cells[1].Text;
                        A41 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[1].Text = A41.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A42 = Scdl3.Items[8].Cells[2].Text;
                        A42 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[2].Text = A42.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A43 = Scdl3.Items[8].Cells[3].Text;
                        A43 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[3].Text = A43.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A44 = Scdl3.Items[8].Cells[4].Text;
                        A44 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[4].Text = A44.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A45 = Scdl3.Items[8].Cells[5].Text;
                        A45 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[8].Cells[5].Text = A45.Replace("\r\n", "<br>");
                    }
                }
                else if (tm == "18:00")
                {
                    if (week == "月")
                    {
                        string A46 = Scdl3.Items[9].Cells[1].Text;
                        A46 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[1].Text = A46.Replace("\r\n", "<br>");
                    }
                    if (week == "火")
                    {
                        string A47 = Scdl3.Items[9].Cells[2].Text;
                        A47 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[2].Text = A47.Replace("\r\n", "<br>");
                    }
                    if (week == "水")
                    {
                        string A48 = Scdl3.Items[9].Cells[3].Text;
                        A48 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[3].Text = A48.Replace("\r\n", "<br>");
                    }
                    if (week == "木")
                    {
                        string A49 = Scdl3.Items[9].Cells[4].Text;
                        A49 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[4].Text = A49.Replace("\r\n", "<br>");
                    }
                    if (week == "金")
                    {
                        string A50 = Scdl3.Items[9].Cells[5].Text;
                        A50 += dl.time + dl.title + "<font color=#17a404>" + dl.name + "</font color>" + "\r\n";
                        Scdl3.Items[9].Cells[5].Text = A50.Replace("\r\n", "<br>");
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

        protected void Button8_Click(object sender, EventArgs e)
        {

            string a = TextBox2.Text;

            var dd = A(a, Global.GetConnection());

            ScdlList.DataSource = dd;

            ScdlList.DataBind();



            for (int j = 0; j < dd.Count; j++)
            {

                var dl = dd.Rows[j] as DATASET.DataSet.T_ScheduleRow;

                ScdlList.Items[j].Cells[0].Text = dl.date.ToString("yyyy/MM/dd") + " " + dl.date.ToString("dddd");
                ScdlList.Items[j].Cells[1].Text = dl.time.ToString();
                ScdlList.Items[j].Cells[2].Text = dl.title.ToString();
                ScdlList.Items[j].Cells[3].Text = dl.name.ToString();
                ScdlList.Items[j].Cells[4].Text = dl.SdlNo.ToString();
            }

            Create3();
            Create2();
        }

        public static DATASET.DataSet.T_ScheduleDataTable A(string a, SqlConnection Sqlco)
        {
            SqlDataAdapter da = new SqlDataAdapter("", Sqlco);

            da.SelectCommand.CommandText =
              "SELECT * FROM T_Schedule WHERE title LIKE @a order by date asc";


            da.SelectCommand.Parameters.AddWithValue("@a", "%" + a + "%");

            var dt = new DATASET.DataSet.T_ScheduleDataTable();

            da.Fill(dt);

            return dt;
        }

        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}


