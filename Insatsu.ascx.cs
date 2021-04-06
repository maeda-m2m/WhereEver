using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WhereEver
{
    public partial class Insatsu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DATASET.DataSet.T_ScheduleDataTable dt = Class2.Insatsu1(Global.GetConnection());
            //for (int j = 0; j < dt.Count; j++)
            //{
            //    DataSet1.T_Schedule3Row dr = dt.Rows[j] as DataSet1.T_Schedule3Row;

            //    DateTime DT = DateTime.Parse(dr.date.ToString());
            //    string week = DT.ToString("ddd");

            //    string tm = dr.time.ToString();

            //    if (week == "月")
            //    {
            //        Label1.Text = tm;
            //        if (Label1.Text != null)
            //        {
            //            Label2.Text = tm;
            //            if (Label2.Text != null)
            //            {
            //                Label3.Text = tm;
            //            }
            //            if (Label3.Text != null)
            //            {
            //                Label4.Text = tm;
            //            }
            //        }

            //    }

            //}

            for (int j = 0; j < dt.Count; j++)
            {
                DataSet1.T_Schedule3Row dr = dt.Rows[j] as DataSet1.T_Schedule3Row;

                DateTime DT = DateTime.Parse(dr.date.ToString());
                string week = DT.ToString("ddd");

                string tm = dr.time.ToString();

                while (week == "月")
                {

                    if (Label1.Text != null)
                    {
                        Label1.Text = tm;
                        Label5.Text = dr.titile;
                        Label9.Text = dr.name;
                        continue;
                    }
                    if (Label2.Text != null)
                    {
                        Label2.Text = tm;
                        Label6.Text = dr.titile;
                        Label10.Text = dr.name;
                        continue;
                    }
                    if (Label3.Text != null)
                    {
                        Label3.Text = tm;
                        Label7.Text = dr.titile;
                        Label11.Text = dr.name;
                        continue;
                    }
                    if (Label3.Text != null)
                    {
                        Label4.Text = tm;
                        Label8.Text = dr.titile;
                        Label12.Text = dr.name;
                        continue;
                    }
                    break;
                }
            }

        }

        internal void Create(DataSet1.T_Schedule3DataTable dt)
        {
            throw new NotImplementedException();
        }
    }
}
