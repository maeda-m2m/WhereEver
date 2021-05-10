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
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Globalization;

namespace WhereEver
{
    public partial class PrintSchedule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
                            Label71.Text = a.ToShortDateString();
                        }

                        if (n == 6)
                        {
                            DateTime a = monday1.AddDays(n);
                            Label73.Text = a.ToShortDateString();
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
                            Label75.Text = a.ToShortDateString();
                        }

                        if (n == 13)
                        {
                            DateTime a = monday1.AddDays(n);
                            Label77.Text = a.ToShortDateString();
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
                            Label71.Text = a.ToShortDateString();
                        }

                        if (n == 6)
                        {
                            DateTime a = monday2.AddDays(n);
                            Label73.Text = a.ToShortDateString();
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
                            Label75.Text = a.ToShortDateString();
                        }

                        if (n == 13)
                        {
                            DateTime a = monday2.AddDays(n);
                            Label77.Text = a.ToShortDateString();
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
                            Label71.Text = a.ToShortDateString();
                        }

                        if (n == 6)
                        {
                            DateTime a = monday3.AddDays(n);
                            Label73.Text = a.ToShortDateString();
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
                            Label75.Text = a.ToShortDateString();
                        }

                        if (n == 13)
                        {
                            DateTime a = monday3.AddDays(n);
                            Label77.Text = a.ToShortDateString();
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
                            Label71.Text = a.ToShortDateString();
                        }

                        if (n == 6)
                        {
                            DateTime a = monday4.AddDays(n);
                            Label73.Text = a.ToShortDateString();
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
                            Label75.Text = a.ToShortDateString();
                        }

                        if (n == 13)
                        {
                            DateTime a = monday4.AddDays(n);
                            Label77.Text = a.ToShortDateString();
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
                            Label71.Text = a.ToShortDateString();
                        }

                        if (n == 6)
                        {
                            DateTime a = monday5.AddDays(n);
                            Label73.Text = a.ToShortDateString();
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
                            Label75.Text = a.ToShortDateString();
                        }

                        if (n == 13)
                        {
                            DateTime a = monday5.AddDays(n);
                            Label77.Text = a.ToShortDateString();
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
                            Label71.Text = a.ToShortDateString();
                        }

                        if (n == 6)
                        {
                            DateTime a = monday6.AddDays(n);
                            Label73.Text = a.ToShortDateString();
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
                            Label75.Text = a.ToShortDateString();
                        }

                        if (n == 13)
                        {
                            DateTime a = monday6.AddDays(n);
                            Label77.Text = a.ToShortDateString();
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
                            Label71.Text = a.ToShortDateString();
                        }

                        if (n == 6)
                        {
                            DateTime a = monday7.AddDays(n);
                            Label73.Text = a.ToShortDateString();
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
                            Label75.Text = a.ToShortDateString();
                        }

                        if (n == 13)
                        {
                            DateTime a = monday7.AddDays(n);
                            Label77.Text = a.ToShortDateString();
                        }
                    }
                    break;
            }

            DATASET.DataSet.T_ScheduleDataTable dd = Class2.Insatsu1(Global.GetConnection());


<<<<<<< HEAD
            //for (int j = 0; j < dd.Count; j++)

            //{
            //    DataSet.T_Schedule dr = dd.Rows[j] as DataSet.T_Schedule;

            //    DateTime DT = DateTime.Parse(dr.date.ToString());
            //    string week = DT.ToString("ddd");
            //   // string.Format("HH:mm", dr.time);
            //    string tm = dr.time.ToString();
            //    string tm1 = tm.Substring(0, 5);

            //    while (week == "月")
            //    {

            //        if (Label3.Text == "")
            //        {
            //            Label3.Text = tm1;
            //            Label4.Text = dr.titile;
            //            Label5.Text = dr.name;
            //            break;
            //        }
            //        if (Label6.Text == "")
            //        {
            //            Label6.Text = tm1;
            //            Label7.Text = dr.titile;
            //            Label8.Text = dr.name;
            //            break;
            //        }
            //        if (Label9.Text == "")
            //        {
            //            Label9.Text = tm1;
            //            Label10.Text = dr.titile;
            //            Label11.Text = dr.name;
            //            break;
            //        }
            //        if (Label12.Text == "")
            //        {
            //            Label12.Text = tm1;
            //            Label13.Text = dr.titile;
            //            Label14.Text = dr.name;
            //            break;
            //        }
            //        break;
            //    }

            //    while (week == "火")
            //    {

            //        if (Label17.Text == "")
            //        {
            //            Label17.Text = tm1;
            //            Label18.Text = dr.titile;
            //            Label19.Text = dr.name;
            //            break;
            //        }
            //        if (Label20.Text == "")
            //        {
            //            Label20.Text = tm1;
            //            Label21.Text = dr.titile;
            //            Label22.Text = dr.name;
            //            break;
            //        }
            //        if (Label23.Text == "")
            //        {
            //            Label23.Text = tm1;
            //            Label24.Text = dr.titile;
            //            Label25.Text = dr.name;
            //            break;
            //        }
            //        if (Label26.Text == "")
            //        {
            //            Label26.Text = tm1;
            //            Label27.Text = dr.titile;
            //            Label28.Text = dr.name;
            //            break;
            //        }
            //        break;
            //    }

            //    while (week == "水")
            //    {

            //        if (Label31.Text == "")
            //        {
            //            Label31.Text = tm1;
            //            Label32.Text = dr.titile;
            //            Label33.Text = dr.name;
            //            break;
            //        }
            //        if (Label34.Text == "")
            //        {
            //            Label34.Text = tm1;
            //            Label35.Text = dr.titile;
            //            Label36.Text = dr.name;
            //            break;
            //        }
            //        if (Label37.Text == "")
            //        {
            //            Label37.Text = tm1;
            //            Label38.Text = dr.titile;
            //            Label39.Text = dr.name;
            //            break;
            //        }
            //        if (Label40.Text == "")
            //        {
            //            Label40.Text = tm1;
            //            Label41.Text = dr.titile;
            //            Label42.Text = dr.name;
            //            break;
            //        }
            //        break;
            //    }

            //    while (week == "木")
            //    {

            //        if (Label45.Text == "")
            //        {
            //            Label45.Text = tm1;
            //            Label46.Text = dr.titile;
            //            Label47.Text = dr.name;
            //            break;
            //        }
            //        if (Label48.Text == "")
            //        {
            //            Label48.Text = tm1;
            //            Label49.Text = dr.titile;
            //            Label50.Text = dr.name;
            //            break;
            //        }
            //        if (Label51.Text == "")
            //        {
            //            Label51.Text = tm1;
            //            Label52.Text = dr.titile;
            //            Label53.Text = dr.name;
            //            break;
            //        }
            //        if (Label54.Text == "")
            //        {
            //            Label54.Text = tm1;
            //            Label55.Text = dr.titile;
            //            Label56.Text = dr.name;
            //            break;
            //        }
            //        break;
            //    }

            //    while (week == "金")
            //    {

            //        if (Label59.Text == "")
            //        {
            //            Label59.Text = tm1;
            //            Label60.Text = dr.titile;
            //            Label61.Text = dr.name;
            //            break;
            //        }
            //        if (Label62.Text == "")
            //        {
            //            Label62.Text = tm1;
            //            Label63.Text = dr.titile;
            //            Label64.Text = dr.name;
            //            break;
            //        }
            //        if (Label65.Text == "")
            //        {
            //            Label65.Text = tm1;
            //            Label66.Text = dr.titile;
            //            Label67.Text = dr.name;
            //            break;
            //        }
            //        if (Label68.Text == "")
            //        {
            //            Label68.Text = tm1;
            //            Label69.Text = dr.titile;
            //            Label70.Text = dr.name;
            //            break;
            //        }
            //        break;
            //    }



            //}

            //DATASET.DataSet.T_ScheduleDataTable dp = Class2.Insatsu2(Global.GetConnection());


            //for (int j = 0; j < dp.Count; j++)

            //{
            //    DataSet.T_ScheduleRow dr = dp.Rows[j] as DataSet.T_ScheduleRow;

            //    DateTime DT = DateTime.Parse(dr.date.ToString());
            //    string week = DT.ToString("ddd");
            //   // string.Format("HH:mm", dr.time);
               
            //    string tm = dr.time.ToString();
            //    string tm1 = tm.Substring(0, 5);

            //    while (week == "月")
            //    {

            //        if (Label101.Text == "")
            //        {
            //            Label101.Text = tm1;
            //            Label102.Text = dr.titile;
            //            Label103.Text = dr.name;
            //            break;
            //        }
            //        if (Label104.Text == "")
            //        {
            //            Label104.Text = tm1;
            //            Label105.Text = dr.titile;
            //            Label106.Text = dr.name;
            //            break;
            //        }
            //        if (Label107.Text == "")
            //        {
            //            Label107.Text = tm1;
            //            Label108.Text = dr.titile;
            //            Label109.Text = dr.name;
            //            break;
            //        }
            //        if (Label110.Text == "")
            //        {
            //            Label110.Text = tm1;
            //            Label111.Text = dr.titile;
            //            Label112.Text = dr.name;
            //            break;
            //        }
            //        break;
            //    }

            //    while (week == "火")
            //    {

            //        if (Label115.Text == "")
            //        {
            //            Label115.Text = tm1;
            //            Label116.Text = dr.titile;
            //            Label117.Text = dr.name;
            //            break;
            //        }
            //        if (Label118.Text == "")
            //        {
            //            Label118.Text = tm1;
            //            Label119.Text = dr.titile;
            //            Label120.Text = dr.name;
            //            break;
            //        }
            //        if (Label121.Text == "")
            //        {
            //            Label121.Text = tm1;
            //            Label122.Text = dr.titile;
            //            Label123.Text = dr.name;
            //            break;
            //        }
            //        if (Label124.Text == "")
            //        {
            //            Label124.Text = tm1;
            //            Label125.Text = dr.titile;
            //            Label126.Text = dr.name;
            //            break;
            //        }
            //        break;
            //    }

            //    while (week == "水")
            //    {

            //        if (Label129.Text == "")
            //        {
            //            Label129.Text = tm1;
            //            Label130.Text = dr.titile;
            //            Label131.Text = dr.name;
            //            break;
            //        }
            //        if (Label132.Text == "")
            //        {
            //            Label132.Text = tm1;
            //            Label133.Text = dr.titile;
            //            Label134.Text = dr.name;
            //            break;
            //        }
            //        if (Label135.Text == "")
            //        {
            //            Label135.Text = tm1;
            //            Label136.Text = dr.titile;
            //            Label137.Text = dr.name;
            //            break;
            //        }
            //        if (Label138.Text == "")
            //        {
            //            Label138.Text = tm1;
            //            Label139.Text = dr.titile;
            //            Label140.Text = dr.name;
            //            break;
            //        }
            //        break;
            //    }

            //    while (week == "木")
            //    {

            //        if (Label143.Text == "")
            //        {
            //            Label143.Text = tm1;
            //            Label144.Text = dr.titile;
            //            Label145.Text = dr.name;
            //            break;
            //        }
            //        if (Label146.Text == "")
            //        {
            //            Label146.Text = tm1;
            //            Label147.Text = dr.titile;
            //            Label148.Text = dr.name;
            //            break;
            //        }
            //        if (Label149.Text == "")
            //        {
            //            Label149.Text = tm1;
            //            Label150.Text = dr.titile;
            //            Label151.Text = dr.name;
            //            break;
            //        }
            //        if (Label152.Text == "")
            //        { 
            //            Label152.Text = tm1;
            //            Label153.Text = dr.titile;
            //            Label154.Text = dr.name;
            //            break;
            //        }
            //        break;
            //    }

            //    while (week == "金")
            //    {

            //        if (Label157.Text == "")
            //        {
            //            Label157.Text = tm1;
            //            Label158.Text = dr.titile;
            //            Label159.Text = dr.name;
            //            break;
            //        }
            //        if (Label160.Text == "")
            //        {
            //            Label160.Text = tm1;
            //            Label161.Text = dr.titile;
            //            Label162.Text = dr.name;
            //            break;
            //        }
            //        if (Label163.Text == "")
            //        { 
            //            Label163.Text = tm1;
            //            Label164.Text = dr.titile;
            //            Label165.Text = dr.name;
            //            break;
            //        }
            //        if (Label166.Text == "")
            //        {
            //            Label166.Text = tm1;
            //            Label167.Text = dr.titile;
            //            Label168.Text = dr.name;
            //            break;
            //        }
            //        break;
            //    }



            //}
=======
            for (int j = 0; j < dd.Count; j++)

            {
               DATASET. DataSet.T_ScheduleRow dr = dd.Rows[j] as DATASET. DataSet.T_ScheduleRow;

                DateTime DT = DateTime.Parse(dr.date.ToString());
                string week = DT.ToString("ddd");
               // string.Format("HH:mm", dr.time);
                string tm = dr.time.ToString();
                string tm1 = tm.Substring(0, 5);

                while (week == "月")
                {

                    if (Label3.Text == "")
                    {
                        Label3.Text = tm1;
                        Label4.Text = dr.title;
                        Label5.Text = dr.name;
                        break;
                    }
                    if (Label6.Text == "")
                    {
                        Label6.Text = tm1;
                        Label7.Text = dr.title;
                        Label8.Text = dr.name;
                        break;
                    }
                    if (Label9.Text == "")
                    {
                        Label9.Text = tm1;
                        Label10.Text = dr.title;
                        Label11.Text = dr.name;
                        break;
                    }
                    if (Label12.Text == "")
                    {
                        Label12.Text = tm1;
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
                        Label17.Text = tm1;
                        Label18.Text = dr.title;
                        Label19.Text = dr.name;
                        break;
                    }
                    if (Label20.Text == "")
                    {
                        Label20.Text = tm1;
                        Label21.Text = dr.title;
                        Label22.Text = dr.name;
                        break;
                    }
                    if (Label23.Text == "")
                    {
                        Label23.Text = tm1;
                        Label24.Text = dr.title;
                        Label25.Text = dr.name;
                        break;
                    }
                    if (Label26.Text == "")
                    {
                        Label26.Text = tm1;
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
                        Label31.Text = tm1;
                        Label32.Text = dr.title;
                        Label33.Text = dr.name;
                        break;
                    }
                    if (Label34.Text == "")
                    {
                        Label34.Text = tm1;
                        Label35.Text = dr.title;
                        Label36.Text = dr.name;
                        break;
                    }
                    if (Label37.Text == "")
                    {
                        Label37.Text = tm1;
                        Label38.Text = dr.title;
                        Label39.Text = dr.name;
                        break;
                    }
                    if (Label40.Text == "")
                    {
                        Label40.Text = tm1;
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
                        Label45.Text = tm1;
                        Label46.Text = dr.title;
                        Label47.Text = dr.name;
                        break;
                    }
                    if (Label48.Text == "")
                    {
                        Label48.Text = tm1;
                        Label49.Text = dr.title;
                        Label50.Text = dr.name;
                        break;
                    }
                    if (Label51.Text == "")
                    {
                        Label51.Text = tm1;
                        Label52.Text = dr.title;
                        Label53.Text = dr.name;
                        break;
                    }
                    if (Label54.Text == "")
                    {
                        Label54.Text = tm1;
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
                        Label59.Text = tm1;
                        Label60.Text = dr.title;
                        Label61.Text = dr.name;
                        break;
                    }
                    if (Label62.Text == "")
                    {
                        Label62.Text = tm1;
                        Label63.Text = dr.title;
                        Label64.Text = dr.name;
                        break;
                    }
                    if (Label65.Text == "")
                    {
                        Label65.Text = tm1;
                        Label66.Text = dr.title;
                        Label67.Text = dr.name;
                        break;
                    }
                    if (Label68.Text == "")
                    {
                        Label68.Text = tm1;
                        Label69.Text = dr.title;
                        Label70.Text = dr.name;
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
                string tm1 = tm.Substring(0, 5);

                while (week == "月")
                {

                    if (Label101.Text == "")
                    {
                        Label101.Text = tm1;
                        Label102.Text = dr.title;
                        Label103.Text = dr.name;
                        break;
                    }
                    if (Label104.Text == "")
                    {
                        Label104.Text = tm1;
                        Label105.Text = dr.title;
                        Label106.Text = dr.name;
                        break;
                    }
                    if (Label107.Text == "")
                    {
                        Label107.Text = tm1;
                        Label108.Text = dr.title;
                        Label109.Text = dr.name;
                        break;
                    }
                    if (Label110.Text == "")
                    {
                        Label110.Text = tm1;
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
                        Label115.Text = tm1;
                        Label116.Text = dr.title;
                        Label117.Text = dr.name;
                        break;
                    }
                    if (Label118.Text == "")
                    {
                        Label118.Text = tm1;
                        Label119.Text = dr.title;
                        Label120.Text = dr.name;
                        break;
                    }
                    if (Label121.Text == "")
                    {
                        Label121.Text = tm1;
                        Label122.Text = dr.title;
                        Label123.Text = dr.name;
                        break;
                    }
                    if (Label124.Text == "")
                    {
                        Label124.Text = tm1;
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
                        Label129.Text = tm1;
                        Label130.Text = dr.title;
                        Label131.Text = dr.name;
                        break;
                    }
                    if (Label132.Text == "")
                    {
                        Label132.Text = tm1;
                        Label133.Text = dr.title;
                        Label134.Text = dr.name;
                        break;
                    }
                    if (Label135.Text == "")
                    {
                        Label135.Text = tm1;
                        Label136.Text = dr.title;
                        Label137.Text = dr.name;
                        break;
                    }
                    if (Label138.Text == "")
                    {
                        Label138.Text = tm1;
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
                        Label143.Text = tm1;
                        Label144.Text = dr.title;
                        Label145.Text = dr.name;
                        break;
                    }
                    if (Label146.Text == "")
                    {
                        Label146.Text = tm1;
                        Label147.Text = dr.title;
                        Label148.Text = dr.name;
                        break;
                    }
                    if (Label149.Text == "")
                    {
                        Label149.Text = tm1;
                        Label150.Text = dr.title;
                        Label151.Text = dr.name;
                        break;
                    }
                    if (Label152.Text == "")
                    { 
                        Label152.Text = tm1;
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
                        Label157.Text = tm1;
                        Label158.Text = dr.title;
                        Label159.Text = dr.name;
                        break;
                    }
                    if (Label160.Text == "")
                    {
                        Label160.Text = tm1;
                        Label161.Text = dr.title;
                        Label162.Text = dr.name;
                        break;
                    }
                    if (Label163.Text == "")
                    { 
                        Label163.Text = tm1;
                        Label164.Text = dr.title;
                        Label165.Text = dr.name;
                        break;
                    }
                    if (Label166.Text == "")
                    {
                        Label166.Text = tm1;
                        Label167.Text = dr.title;
                        Label168.Text = dr.name;
                        break;
                    }
                    break;
                }



            }
>>>>>>> 668c9645970e8861149031e0e399d17acbbea6d0

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

        protected void Button2_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            pd.Print();
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, new PointF(0, 0));
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            Response.Redirect("Schedule.aspx");
        }
    }
}



