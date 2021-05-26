using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Linq;
using System.Web.UI.WebControls.Expressions;
using Microsoft.Ajax.Utilities;
using System.Globalization;

namespace WhereEver
{
    public partial class PrintSchedule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreateDataGrid();
                DgBikou.EditCommand +=
                        new DataGridCommandEventHandler(this.DgBikou_EditCommand);
                DgBikou.CancelCommand +=
                    new DataGridCommandEventHandler(this.DgBikou_CancelCommand);
                DgBikou.UpdateCommand +=
                    new DataGridCommandEventHandler(this.DgBikou_UpdateCommand);
                DgBikou.ItemCommand +=
                        new DataGridCommandEventHandler(this.DgBikou_ItemCommand);

                ////今週の週番号と来週の週番号を取得する
                //DateTime date = DateTime.Now;

                //int konsyu = cal.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                //int raisyu = konsyu + 1;


                ////週番号から日付を取得する
                //int kongetsu = cal.GetDayOfMonth(date);
                //int monday1 = cal.
                DateTime today = DateTime.Now;
                DayOfWeek dow = today.DayOfWeek;
                switch (dow)
                {
                    case DayOfWeek.Sunday:
                        DateTime monday1 = today.AddDays(-6);

                        for (int n = 0; n <= 13; n++)
                        {
                            if (n == 0)
                            {
                                DateTime a = monday1.AddDays(n);
                                Label1.Text = a.ToShortDateString();
                            }

                            if (n == 1)
                            {
                                DateTime a = monday1.AddDays(n);
                                Label15.Text = a.ToShortDateString();
                            }

                            if (n == 2)
                            {
                                DateTime a = monday1.AddDays(n);
                                Label29.Text = a.ToShortDateString();
                            }

                            if (n == 3)
                            {
                                DateTime a = monday1.AddDays(n);
                                Label43.Text = a.ToShortDateString();
                            }

                            if (n == 4)
                            {
                                DateTime a = monday1.AddDays(n);
                                Label57.Text = a.ToShortDateString();
                            }

                            if (n == 5)
                            {
                                DateTime a = monday1.AddDays(n);
                                Labelsat1.Text = a.ToShortDateString();
                            }

                            if (n == 6)
                            {
                                DateTime a = monday1.AddDays(n);
                                Labelsun1.Text = a.ToShortDateString();
                            }

                            if (n == 7)
                            {
                                DateTime a = monday1.AddDays(n);
                                Label99.Text = a.ToShortDateString();
                            }

                            if (n == 8)
                            {
                                DateTime a = monday1.AddDays(n);
                                Label113.Text = a.ToShortDateString();
                            }

                            if (n == 9)
                            {
                                DateTime a = monday1.AddDays(n);
                                Label127.Text = a.ToShortDateString();
                            }

                            if (n == 10)
                            {
                                DateTime a = monday1.AddDays(n);
                                Label141.Text = a.ToShortDateString();
                            }

                            if (n == 11)
                            {
                                DateTime a = monday1.AddDays(n);
                                Label155.Text = a.ToShortDateString();
                            }

                            if (n == 12)
                            {
                                DateTime a = monday1.AddDays(n);
                                Labelsat15.Text = a.ToShortDateString();
                            }

                            if (n == 13)
                            {
                                DateTime a = monday1.AddDays(n);
                                Labelsun15.Text = a.ToShortDateString();
                            }
                        }
                        break;

                    case DayOfWeek.Monday:
                        DateTime monday2 = today.AddDays(-7);
                        for (int n = 0; n <= 13; n++)
                        {
                            if (n == 0)
                            {
                                DateTime a = monday2.AddDays(n);
                                Label1.Text = a.ToShortDateString();
                            }

                            if (n == 1)
                            {
                                DateTime a = monday2.AddDays(n);
                                Label15.Text = a.ToShortDateString();
                            }

                            if (n == 2)
                            {
                                DateTime a = monday2.AddDays(n);
                                Label29.Text = a.ToShortDateString();
                            }

                            if (n == 3)
                            {
                                DateTime a = monday2.AddDays(n);
                                Label43.Text = a.ToShortDateString();
                            }

                            if (n == 4)
                            {
                                DateTime a = monday2.AddDays(n);
                                Label57.Text = a.ToShortDateString();
                            }

                            if (n == 5)
                            {
                                DateTime a = monday2.AddDays(n);
                                Labelsat1.Text = a.ToShortDateString();
                            }

                            if (n == 6)
                            {
                                DateTime a = monday2.AddDays(n);
                                Labelsun1.Text = a.ToShortDateString();
                            }

                            if (n == 7)
                            {
                                DateTime a = monday2.AddDays(n);
                                Label99.Text = a.ToShortDateString();
                            }

                            if (n == 8)
                            {
                                DateTime a = monday2.AddDays(n);
                                Label113.Text = a.ToShortDateString();
                            }

                            if (n == 9)
                            {
                                DateTime a = monday2.AddDays(n);
                                Label127.Text = a.ToShortDateString();
                            }

                            if (n == 10)
                            {
                                DateTime a = monday2.AddDays(n);
                                Label141.Text = a.ToShortDateString();
                            }

                            if (n == 11)
                            {
                                DateTime a = monday2.AddDays(n);
                                Label155.Text = a.ToShortDateString();
                            }

                            if (n == 12)
                            {
                                DateTime a = monday2.AddDays(n);
                                Labelsat15.Text = a.ToShortDateString();
                            }

                            if (n == 13)
                            {
                                DateTime a = monday2.AddDays(n);
                                Labelsun15.Text = a.ToShortDateString();
                            }
                        }
                        break;

                    case DayOfWeek.Tuesday:
                        DateTime monday3 = today.AddDays(-8);
                        for (int n = 0; n <= 13; n++)
                        {
                            if (n == 0)
                            {
                                DateTime a = monday3.AddDays(n);
                                Label1.Text = a.ToShortDateString();
                            }

                            if (n == 1)
                            {
                                DateTime a = monday3.AddDays(n);
                                Label15.Text = a.ToShortDateString();
                            }

                            if (n == 2)
                            {
                                DateTime a = monday3.AddDays(n);
                                Label29.Text = a.ToShortDateString();
                            }

                            if (n == 3)
                            {
                                DateTime a = monday3.AddDays(n);
                                Label43.Text = a.ToShortDateString();
                            }

                            if (n == 4)
                            {
                                DateTime a = monday3.AddDays(n);
                                Label57.Text = a.ToShortDateString();
                            }

                            if (n == 5)
                            {
                                DateTime a = monday3.AddDays(n);
                                Labelsat1.Text = a.ToShortDateString();
                            }

                            if (n == 6)
                            {
                                DateTime a = monday3.AddDays(n);
                                Labelsun1.Text = a.ToShortDateString();
                            }

                            if (n == 7)
                            {
                                DateTime a = monday3.AddDays(n);
                                Label99.Text = a.ToShortDateString();
                            }

                            if (n == 8)
                            {
                                DateTime a = monday3.AddDays(n);
                                Label113.Text = a.ToShortDateString();
                            }

                            if (n == 9)
                            {
                                DateTime a = monday3.AddDays(n);
                                Label127.Text = a.ToShortDateString();
                            }

                            if (n == 10)
                            {
                                DateTime a = monday3.AddDays(n);
                                Label141.Text = a.ToShortDateString();
                            }

                            if (n == 11)
                            {
                                DateTime a = monday3.AddDays(n);
                                Label155.Text = a.ToShortDateString();
                            }

                            if (n == 12)
                            {
                                DateTime a = monday3.AddDays(n);
                                Labelsat15.Text = a.ToShortDateString();
                            }

                            if (n == 13)
                            {
                                DateTime a = monday3.AddDays(n);
                                Labelsun15.Text = a.ToShortDateString();
                            }
                        }
                        break;

                    case DayOfWeek.Wednesday:
                        DateTime monday4 = today.AddDays(-9);
                        for (int n = 0; n <= 13; n++)
                        {
                            if (n == 0)
                            {
                                DateTime a = monday4.AddDays(n);
                                Label1.Text = a.ToShortDateString();
                            }

                            if (n == 1)
                            {
                                DateTime a = monday4.AddDays(n);
                                Label15.Text = a.ToShortDateString();
                            }

                            if (n == 2)
                            {
                                DateTime a = monday4.AddDays(n);
                                Label29.Text = a.ToShortDateString();
                            }

                            if (n == 3)
                            {
                                DateTime a = monday4.AddDays(n);
                                Label43.Text = a.ToShortDateString();
                            }

                            if (n == 4)
                            {
                                DateTime a = monday4.AddDays(n);
                                Label57.Text = a.ToShortDateString();
                            }

                            if (n == 5)
                            {
                                DateTime a = monday4.AddDays(n);
                                Labelsat1.Text = a.ToShortDateString();
                            }

                            if (n == 6)
                            {
                                DateTime a = monday4.AddDays(n);
                                Labelsun1.Text = a.ToShortDateString();
                            }

                            if (n == 7)
                            {
                                DateTime a = monday4.AddDays(n);
                                Label99.Text = a.ToShortDateString();
                            }

                            if (n == 8)
                            {
                                DateTime a = monday4.AddDays(n);
                                Label113.Text = a.ToShortDateString();
                            }

                            if (n == 9)
                            {
                                DateTime a = monday4.AddDays(n);
                                Label127.Text = a.ToShortDateString();
                            }

                            if (n == 10)
                            {
                                DateTime a = monday4.AddDays(n);
                                Label141.Text = a.ToShortDateString();
                            }

                            if (n == 11)
                            {
                                DateTime a = monday4.AddDays(n);
                                Label155.Text = a.ToShortDateString();
                            }

                            if (n == 12)
                            {
                                DateTime a = monday4.AddDays(n);
                                Labelsat15.Text = a.ToShortDateString();
                            }

                            if (n == 13)
                            {
                                DateTime a = monday4.AddDays(n);
                                Labelsun15.Text = a.ToShortDateString();
                            }
                        }
                        break;

                    case DayOfWeek.Thursday:
                        DateTime monday5 = today.AddDays(-10);
                        for (int n = 0; n <= 13; n++)
                        {
                            if (n == 0)
                            {
                                DateTime a = monday5.AddDays(n);
                                Label1.Text = a.ToShortDateString();
                            }

                            if (n == 1)
                            {
                                DateTime a = monday5.AddDays(n);
                                Label15.Text = a.ToShortDateString();
                            }

                            if (n == 2)
                            {
                                DateTime a = monday5.AddDays(n);
                                Label29.Text = a.ToShortDateString();
                            }

                            if (n == 3)
                            {
                                DateTime a = monday5.AddDays(n);
                                Label43.Text = a.ToShortDateString();
                            }

                            if (n == 4)
                            {
                                DateTime a = monday5.AddDays(n);
                                Label57.Text = a.ToShortDateString();
                            }

                            if (n == 5)
                            {
                                DateTime a = monday5.AddDays(n);
                                Labelsat1.Text = a.ToShortDateString();
                            }

                            if (n == 6)
                            {
                                DateTime a = monday5.AddDays(n);
                                Labelsun1.Text = a.ToShortDateString();
                            }

                            if (n == 7)
                            {
                                DateTime a = monday5.AddDays(n);
                                Label99.Text = a.ToShortDateString();
                            }

                            if (n == 8)
                            {
                                DateTime a = monday5.AddDays(n);
                                Label113.Text = a.ToShortDateString();
                            }

                            if (n == 9)
                            {
                                DateTime a = monday5.AddDays(n);
                                Label127.Text = a.ToShortDateString();
                            }

                            if (n == 10)
                            {
                                DateTime a = monday5.AddDays(n);
                                Label141.Text = a.ToShortDateString();
                            }

                            if (n == 11)
                            {
                                DateTime a = monday5.AddDays(n);
                                Label155.Text = a.ToShortDateString();
                            }

                            if (n == 12)
                            {
                                DateTime a = monday5.AddDays(n);
                                Labelsat15.Text = a.ToShortDateString();
                            }

                            if (n == 13)
                            {
                                DateTime a = monday5.AddDays(n);
                                Labelsun15.Text = a.ToShortDateString();
                            }
                        }
                        break;

                    case DayOfWeek.Friday:
                        DateTime monday6 = today.AddDays(-11);

                        for (int n = 0; n <= 13; n++)
                        {
                            if (n == 0)
                            {
                                DateTime a = monday6.AddDays(n);
                                Label1.Text = a.ToShortDateString();
                            }

                            if (n == 1)
                            {
                                DateTime a = monday6.AddDays(n);
                                Label15.Text = a.ToShortDateString();
                            }

                            if (n == 2)
                            {
                                DateTime a = monday6.AddDays(n);
                                Label29.Text = a.ToShortDateString();
                            }

                            if (n == 3)
                            {
                                DateTime a = monday6.AddDays(n);
                                Label43.Text = a.ToShortDateString();
                            }

                            if (n == 4)
                            {
                                DateTime a = monday6.AddDays(n);
                                Label57.Text = a.ToShortDateString();
                            }

                            if (n == 5)
                            {
                                DateTime a = monday6.AddDays(n);
                                Labelsat1.Text = a.ToShortDateString();
                            }

                            if (n == 6)
                            {
                                DateTime a = monday6.AddDays(n);
                                Labelsun1.Text = a.ToShortDateString();
                            }

                            if (n == 7)
                            {
                                DateTime a = monday6.AddDays(n);
                                Label99.Text = a.ToShortDateString();
                            }

                            if (n == 8)
                            {
                                DateTime a = monday6.AddDays(n);
                                Label113.Text = a.ToShortDateString();
                            }

                            if (n == 9)
                            {
                                DateTime a = monday6.AddDays(n);
                                Label127.Text = a.ToShortDateString();
                            }

                            if (n == 10)
                            {
                                DateTime a = monday6.AddDays(n);
                                Label141.Text = a.ToShortDateString();
                            }

                            if (n == 11)
                            {
                                DateTime a = monday6.AddDays(n);
                                Label155.Text = a.ToShortDateString();
                            }

                            if (n == 12)
                            {
                                DateTime a = monday6.AddDays(n);
                                Labelsat15.Text = a.ToShortDateString();
                            }

                            if (n == 13)
                            {
                                DateTime a = monday6.AddDays(n);
                                Labelsun15.Text = a.ToShortDateString();
                            }
                        }

                        break;

                    case DayOfWeek.Saturday:
                        DateTime monday7 = today.AddDays(-12);
                        for (int n = 0; n <= 13; n++)
                        {
                            if (n == 0)
                            {
                                DateTime a = monday7.AddDays(n);
                                Label1.Text = a.ToShortDateString();
                            }

                            if (n == 1)
                            {
                                DateTime a = monday7.AddDays(n);
                                Label15.Text = a.ToShortDateString();
                            }

                            if (n == 2)
                            {
                                DateTime a = monday7.AddDays(n);
                                Label29.Text = a.ToShortDateString();
                            }

                            if (n == 3)
                            {
                                DateTime a = monday7.AddDays(n);
                                Label43.Text = a.ToShortDateString();
                            }

                            if (n == 4)
                            {
                                DateTime a = monday7.AddDays(n);
                                Label57.Text = a.ToShortDateString();
                            }

                            if (n == 5)
                            {
                                DateTime a = monday7.AddDays(n);
                                Labelsat1.Text = a.ToShortDateString();
                            }

                            if (n == 6)
                            {
                                DateTime a = monday7.AddDays(n);
                                Labelsun1.Text = a.ToShortDateString();
                            }

                            if (n == 7)
                            {
                                DateTime a = monday7.AddDays(n);
                                Label99.Text = a.ToShortDateString();
                            }

                            if (n == 8)
                            {
                                DateTime a = monday7.AddDays(n);
                                Label113.Text = a.ToShortDateString();
                            }

                            if (n == 9)
                            {
                                DateTime a = monday7.AddDays(n);
                                Label127.Text = a.ToShortDateString();
                            }

                            if (n == 10)
                            {
                                DateTime a = monday7.AddDays(n);
                                Label141.Text = a.ToShortDateString();
                            }

                            if (n == 11)
                            {
                                DateTime a = monday7.AddDays(n);
                                Label155.Text = a.ToShortDateString();
                            }

                            if (n == 12)
                            {
                                DateTime a = monday7.AddDays(n);
                                Labelsat15.Text = a.ToShortDateString();
                            }

                            if (n == 13)
                            {
                                DateTime a = monday7.AddDays(n);
                                Labelsun15.Text = a.ToShortDateString();
                            }
                        }
                        break;
                }

                DATASET.DataSet.T_ScheduleDataTable dd = Class2.Insatsu1(Global.GetConnection());


                for (int j = 0; j < dd.Count; j++)

                {
                    DATASET.DataSet.T_ScheduleRow dr = dd.Rows[j] as DATASET.DataSet.T_ScheduleRow;

                    DateTime DT = DateTime.Parse(dr.date.ToString());
                    string week = DT.ToString("ddd");
                    // string.Format("HH:mm", dr.time);
                    string tm = dr.time.ToString();

                    while (week == "月")
                    {

                        if (Label3.Text == "")
                        {
                            Label3.Text = tm;
                            Label4.Text = dr.title;
                            Label5.Text = dr.name;
                            break;
                        }
                        if (Label6.Text == "")
                        {
                            Label6.Text = tm;
                            Label7.Text = dr.title;
                            Label8.Text = dr.name;
                            break;
                        }
                        if (Label9.Text == "")
                        {
                            Label9.Text = tm;
                            Label10.Text = dr.title;
                            Label11.Text = dr.name;
                            break;
                        }
                        if (Label12.Text == "")
                        {
                            Label12.Text = tm;
                            Label13.Text = dr.title;
                            Label14.Text = dr.name;
                            break;
                        }
                        break;
                    }

                    while (week == "火")
                    {

                        if (Label17.Text == "")
                        {
                            Label17.Text = tm;
                            Label18.Text = dr.title;
                            Label19.Text = dr.name;
                            break;
                        }
                        if (Label20.Text == "")
                        {
                            Label20.Text = tm;
                            Label21.Text = dr.title;
                            Label22.Text = dr.name;
                            break;
                        }
                        if (Label23.Text == "")
                        {
                            Label23.Text = tm;
                            Label24.Text = dr.title;
                            Label25.Text = dr.name;
                            break;
                        }
                        if (Label26.Text == "")
                        {
                            Label26.Text = tm;
                            Label27.Text = dr.title;
                            Label28.Text = dr.name;
                            break;
                        }
                        break;
                    }

                    while (week == "水")
                    {

                        if (Label31.Text == "")
                        {
                            Label31.Text = tm;
                            Label32.Text = dr.title;
                            Label33.Text = dr.name;
                            break;
                        }
                        if (Label34.Text == "")
                        {
                            Label34.Text = tm;
                            Label35.Text = dr.title;
                            Label36.Text = dr.name;
                            break;
                        }
                        if (Label37.Text == "")
                        {
                            Label37.Text = tm;
                            Label38.Text = dr.title;
                            Label39.Text = dr.name;
                            break;
                        }
                        if (Label40.Text == "")
                        {
                            Label40.Text = tm;
                            Label41.Text = dr.title;
                            Label42.Text = dr.name;
                            break;
                        }
                        break;
                    }

                    while (week == "木")
                    {

                        if (Label45.Text == "")
                        {
                            Label45.Text = tm;
                            Label46.Text = dr.title;
                            Label47.Text = dr.name;
                            break;
                        }
                        if (Label48.Text == "")
                        {
                            Label48.Text = tm;
                            Label49.Text = dr.title;
                            Label50.Text = dr.name;
                            break;
                        }
                        if (Label51.Text == "")
                        {
                            Label51.Text = tm;
                            Label52.Text = dr.title;
                            Label53.Text = dr.name;
                            break;
                        }
                        if (Label54.Text == "")
                        {
                            Label54.Text = tm;
                            Label55.Text = dr.title;
                            Label56.Text = dr.name;
                            break;
                        }
                        break;
                    }

                    while (week == "金")
                    {

                        if (Label59.Text == "")
                        {
                            Label59.Text = tm;
                            Label60.Text = dr.title;
                            Label61.Text = dr.name;
                            break;
                        }
                        if (Label62.Text == "")
                        {
                            Label62.Text = tm;
                            Label63.Text = dr.title;
                            Label64.Text = dr.name;
                            break;
                        }
                        if (Label65.Text == "")
                        {
                            Label65.Text = tm;
                            Label66.Text = dr.title;
                            Label67.Text = dr.name;
                            break;
                        }
                        if (Label68.Text == "")
                        {
                            Label68.Text = tm;
                            Label69.Text = dr.title;
                            Label70.Text = dr.name;
                            break;
                        }
                        break;
                    }
                    while (week == "土")
                    {

                        if (Labelsat3.Text == "")
                        {
                            Labelsat3.Text = tm;
                            Labelsat4.Text = dr.title;
                            Labelsat5.Text = dr.name;
                            break;
                        }
                        if (Labelsat6.Text == "")
                        {
                            Labelsat6.Text = tm;
                            Labelsat7.Text = dr.title;
                            Labelsat8.Text = dr.name;
                            break;
                        }
                        if (Labelsat9.Text == "")
                        {
                            Labelsat9.Text = tm;
                            Labelsat10.Text = dr.title;
                            Labelsat11.Text = dr.name;
                            break;
                        }
                        if (Labelsat12.Text == "")
                        {
                            Labelsat12.Text = tm;
                            Labelsat13.Text = dr.title;
                            Labelsat14.Text = dr.name;
                            break;
                        }
                        break;
                    }
                    while (week == "日")
                    {

                        if (Labelsun3.Text == "")
                        {
                            Labelsun3.Text = tm;
                            Labelsun4.Text = dr.title;
                            Labelsun5.Text = dr.name;
                            break;
                        }
                        if (Labelsun6.Text == "")
                        {
                            Labelsun6.Text = tm;
                            Labelsun7.Text = dr.title;
                            Labelsun8.Text = dr.name;
                            break;
                        }
                        if (Labelsun9.Text == "")
                        {
                            Labelsun9.Text = tm;
                            Labelsun10.Text = dr.title;
                            Labelsun11.Text = dr.name;
                            break;
                        }
                        if (Labelsun12.Text == "")
                        {
                            Labelsun12.Text = tm;
                            Labelsun13.Text = dr.title;
                            Labelsun14.Text = dr.name;
                            break;
                        }
                        break;
                    }
                }

                DATASET.DataSet.T_ScheduleDataTable dp = Class2.Insatsu2(Global.GetConnection());


                for (int j = 0; j < dp.Count; j++)

                {
                    DATASET.DataSet.T_ScheduleRow dr = dp.Rows[j] as DATASET.DataSet.T_ScheduleRow;

                    DateTime DT = DateTime.Parse(dr.date.ToString());
                    string week = DT.ToString("ddd");
                    // string.Format("HH:mm", dr.time);

                    string tm = dr.time.ToString();

                    while (week == "月")
                    {

                        if (Label101.Text == "")
                        {
                            Label101.Text = tm;
                            Label102.Text = dr.title;
                            Label103.Text = dr.name;
                            break;
                        }
                        if (Label104.Text == "")
                        {
                            Label104.Text = tm;
                            Label105.Text = dr.title;
                            Label106.Text = dr.name;
                            break;
                        }
                        if (Label107.Text == "")
                        {
                            Label107.Text = tm;
                            Label108.Text = dr.title;
                            Label109.Text = dr.name;
                            break;
                        }
                        if (Label110.Text == "")
                        {
                            Label110.Text = tm;
                            Label111.Text = dr.title;
                            Label112.Text = dr.name;
                            break;
                        }
                        break;
                    }

                    while (week == "火")
                    {

                        if (Label115.Text == "")
                        {
                            Label115.Text = tm;
                            Label116.Text = dr.title;
                            Label117.Text = dr.name;
                            break;
                        }
                        if (Label118.Text == "")
                        {
                            Label118.Text = tm;
                            Label119.Text = dr.title;
                            Label120.Text = dr.name;
                            break;
                        }
                        if (Label121.Text == "")
                        {
                            Label121.Text = tm;
                            Label122.Text = dr.title;
                            Label123.Text = dr.name;
                            break;
                        }
                        if (Label124.Text == "")
                        {
                            Label124.Text = tm;
                            Label125.Text = dr.title;
                            Label126.Text = dr.name;
                            break;
                        }
                        break;
                    }

                    while (week == "水")
                    {

                        if (Label129.Text == "")
                        {
                            Label129.Text = tm;
                            Label130.Text = dr.title;
                            Label131.Text = dr.name;
                            break;
                        }
                        if (Label132.Text == "")
                        {
                            Label132.Text = tm;
                            Label133.Text = dr.title;
                            Label134.Text = dr.name;
                            break;
                        }
                        if (Label135.Text == "")
                        {
                            Label135.Text = tm;
                            Label136.Text = dr.title;
                            Label137.Text = dr.name;
                            break;
                        }
                        if (Label138.Text == "")
                        {
                            Label138.Text = tm;
                            Label139.Text = dr.title;
                            Label140.Text = dr.name;
                            break;
                        }
                        break;
                    }

                    while (week == "木")
                    {

                        if (Label143.Text == "")
                        {
                            Label143.Text = tm;
                            Label144.Text = dr.title;
                            Label145.Text = dr.name;
                            break;
                        }
                        if (Label146.Text == "")
                        {
                            Label146.Text = tm;
                            Label147.Text = dr.title;
                            Label148.Text = dr.name;
                            break;
                        }
                        if (Label149.Text == "")
                        {
                            Label149.Text = tm;
                            Label150.Text = dr.title;
                            Label151.Text = dr.name;
                            break;
                        }
                        if (Label152.Text == "")
                        {
                            Label152.Text = tm;
                            Label153.Text = dr.title;
                            Label154.Text = dr.name;
                            break;
                        }
                        break;
                    }

                    while (week == "金")
                    {

                        if (Label157.Text == "")
                        {
                            Label157.Text = tm;
                            Label158.Text = dr.title;
                            Label159.Text = dr.name;
                            break;
                        }
                        if (Label160.Text == "")
                        {
                            Label160.Text = tm;
                            Label161.Text = dr.title;
                            Label162.Text = dr.name;
                            break;
                        }
                        if (Label163.Text == "")
                        {
                            Label163.Text = tm;
                            Label164.Text = dr.title;
                            Label165.Text = dr.name;
                            break;
                        }
                        if (Label166.Text == "")
                        {
                            Label166.Text = tm;
                            Label167.Text = dr.title;
                            Label168.Text = dr.name;
                            break;
                        }
                        break;
                    }
                    while (week == "土")
                    {

                        if (Labelsat17.Text == "")
                        {
                            Labelsat17.Text = tm;
                            Labelsat18.Text = dr.title;
                            Labelsat19.Text = dr.name;
                            break;
                        }
                        if (Labelsat20.Text == "")
                        {
                            Labelsat20.Text = tm;
                            Labelsat21.Text = dr.title;
                            Labelsat22.Text = dr.name;
                            break;
                        }
                        if (Labelsat23.Text == "")
                        {
                            Labelsat23.Text = tm;
                            Labelsat24.Text = dr.title;
                            Labelsat25.Text = dr.name;
                            break;
                        }
                        if (Labelsat26.Text == "")
                        {
                            Labelsat26.Text = tm;
                            Labelsat27.Text = dr.title;
                            Labelsat28.Text = dr.name;
                            break;
                        }
                        break;
                    }
                    while (week == "日")
                    {

                        if (Labelsun17.Text == "")
                        {
                            Labelsun17.Text = tm;
                            Labelsun18.Text = dr.title;
                            Labelsun19.Text = dr.name;
                            break;
                        }
                        if (Labelsun20.Text == "")
                        {
                            Labelsun20.Text = tm;
                            Labelsun21.Text = dr.title;
                            Labelsun22.Text = dr.name;
                            break;
                        }
                        if (Labelsun23.Text == "")
                        {
                            Labelsun23.Text = tm;
                            Labelsun24.Text = dr.title;
                            Labelsun25.Text = dr.name;
                            break;
                        }
                        if (Labelsun26.Text == "")
                        {
                            Labelsun26.Text = tm;
                            Labelsun27.Text = dr.title;
                            Labelsun28.Text = dr.name;
                            break;
                        }
                        break;

                    }
                }
            }
            
        }



        private Bitmap memoryImage;

        /// <summary>
        /// フォームのイメージを印刷する
        /// </summary>
        /// <param name="frm">イメージを印刷するフォーム</param>


        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdcDest,
             int nXDest, int nYDest, int nWidth, int nHeight,
             IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        private const int SRCCOPY = 0xCC0020;

        /// <summary>
        /// コントロールのイメージを取得する
        /// </summary>
        /// <param name="ctrl">キャプチャするコントロール</param>
        /// <returns>取得できたイメージ</returns>

        //PrintDocument1のPrintPageイベントハンドラ
        private void PrintDocument1_PrintPage(object sender,
             System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }

        //Button1のClickイベントハンドラ




        protected void Button1_Click(object sender, EventArgs e)
        {
            PrintForm(this);

        }

        private void PrintForm(PrintSchedule printSchedule)
        {
            memoryImage = CaptureControl(printSchedule);
            //フォームのイメージを印刷する
            System.Drawing.Printing.PrintDocument PrintDocument1 =
                 new System.Drawing.Printing.PrintDocument();
            PrintDocument1.PrintPage +=
                 new System.Drawing.Printing.PrintPageEventHandler(
                 PrintDocument1_PrintPage);
            PrintDocument1.Print();


            memoryImage.Dispose();
        }

        private Bitmap CaptureControl(PrintSchedule printSchedule)
        {
            Bitmap img = new Bitmap(842, 595);
            Graphics memg = Graphics.FromImage(img);
            IntPtr dc2 = memg.GetHdc();
            BitBlt(dc2, 0, 0, img.Width, img.Height, dc2, 0, 0, SRCCOPY);
            memg.ReleaseHdc(dc2);
            memg.Dispose();
            return img;

        }
        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, new PointF(0, 0));
        }

        protected void DgBikou_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DATASET.DataSet.T_PrintScheduleRow dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_PrintScheduleRow;
                e.Item.Cells[0].Text = dr.bikouid.ToString();
                e.Item.Cells[1].Text = dr.bikou.ToString();
                bikou.Text += dr.bikou.ToString() +"<br>";
            }
        }
        private void CreateDataGrid()
        {
            DATASET.DataSet.T_PrintScheduleDataTable dt = GetT_PrintScheduleDataTable(Global.GetConnection());

            DgBikou.DataSource = dt;
            DgBikou.DataBind();
        }
        public static DATASET.DataSet.T_PrintScheduleDataTable GetT_PrintScheduleDataTable(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_PrintSchedule";
            DATASET.DataSet.T_PrintScheduleDataTable dt = new DATASET.DataSet.T_PrintScheduleDataTable();
            da.Fill(dt);
            return dt;
        }

        protected void DgBikou_CancelCommand(object source, DataGridCommandEventArgs e)
        {

            bikou.Text = "";
            DgBikou.EditItemIndex = -1;
            CreateDataGrid();
        }

        internal static void Update(DATASET.DataSet.T_PrintScheduleRow dt, string id)
        {
            string cstr = System.Configuration.ConfigurationManager.ConnectionStrings["WhereverConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(cstr))
            {
                string sql = "update T_PrintSchedule set bikou = @bikou where bikouid = @bikouid";

                SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                da.SelectCommand.Parameters.AddWithValue("@bikou", dt.bikou);
                da.SelectCommand.Parameters.AddWithValue("@bikouid", id);
                connection.Open();
                int cnt = da.SelectCommand.ExecuteNonQuery();
                connection.Close();
            }
        }

        protected void DgBikou_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            bikou.Text = "";
            TextBox txtBikou = (TextBox)e.Item.Cells[1].Controls[0];
            DATASET.DataSet.T_PrintScheduleDataTable t_PrintScheduleRows = new DATASET.DataSet.T_PrintScheduleDataTable();
            DATASET.DataSet.T_PrintScheduleRow t_PrintScheduleRow = t_PrintScheduleRows.NewT_PrintScheduleRow();

            t_PrintScheduleRow[0] = txtBikou.Text;
            Update(t_PrintScheduleRow, e.Item.Cells[0].Text);
            DgBikou.EditItemIndex = -1;
            CreateDataGrid();
        }

        protected void DgBikou_EditCommand(object source, DataGridCommandEventArgs e)
        {

            bikou.Text = "";
            DgBikou.EditItemIndex = e.Item.ItemIndex;
            CreateDataGrid();
        }

        protected void DgBikou_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            string id = e.Item.Cells[0].Text;
            switch (((LinkButton)e.CommandSource).CommandName)
            {

                case "Delete":
                    DeleteBikou(id);
                    break;
                default:
                    break;

            }
            DgBikou.EditItemIndex = -1;
            DgBikou.DataSource = GetT_PrintScheduleDataTable(Global.GetConnection());
            DgBikou.DataBind();
        }
        internal static void DeleteBikou(string id)
        {
            string cstr = System.Configuration.ConfigurationManager.ConnectionStrings["WhereverConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(cstr))
            {
                string sql = "DELETE FROM T_PrintSchedule where bikouid = @id";
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);

                da.SelectCommand.Parameters.AddWithValue("@id", id);
                connection.Open();
                int cnt = da.SelectCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}



