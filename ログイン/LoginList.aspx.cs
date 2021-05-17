using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;

namespace WhereEver
{
    public partial class LoginList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DATASET.DataSet.T_LoginListDataTable dtLogin = Class2.GetLoginListDataTable(Global.GetConnection());

                DgTimeDetail.DataSource = dtLogin;
                DgTimeDetail.DataBind();

            }

            //WhatNowをロード
            Label_WhatNow.Text = "";
            DATASET.DataSet.T_ScheduleDataTable dts = Class.Schedule.GetT_ScheduleDataTable(Global.GetConnection(), DateTime.Now.ToString());
            if(dts != null)
            {
                for(int i = 0;  i < dts.Count; i++)
                {                  
                    StringBuilder@sb = new StringBuilder();

                    if (!dts[i].IsNull("title"))
                    {
                        @sb.Append(dts[i].title);
                    }
                    if (!dts[i].IsNull("time"))
                    {
                        @sb.Append("　");
                        @sb.Append(dts[i].time);
                        @sb.Append("～");
                    }
                    if (!dts[i].IsNull("name"))
                    {
                        @sb.Append("　担当：");
                        @sb.Append(dts[i].name);
                    }

                    @sb.Append("<br />");
                    Label_WhatNow.Text += @sb.ToString();
                }
            }
            else
            {
                Label_WhatNow.Text = @"本日の予定はありません。";
            }


        }

        protected void DgTimeDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {

                DATASET.DataSet.T_LoginListRow dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_LoginListRow;
                Label lblName = e.Item.FindControl("lblName") as Label;
                Label lblLoginTime = e.Item.FindControl("lblLoginTime") as Label;
                Label lblLogoutTime = e.Item.FindControl("lblLogoutTime") as Label;
                DATASET.DataSet.T_LoginListRow dl = Class.Login.UserLoginMAXTime(Global.GetConnection(), dr.name.ToString());
                DATASET.DataSet.T_LogoutListRow dt = Class.Login.UserLogoutMAXTime(Global.GetConnection(), dr.name.ToString());

                DATASET.DataSet.M_UserRow ml = Class2.Getname1(Global.GetConnection(), dr.name.ToString());
                lblName.Text = ml.name1.ToString();
                lblLoginTime.Text = dl.Date.ToString();
                if(dt == null)
                {
                    lblLogoutTime.Text = "";
                }
                else
                {
                    lblLogoutTime.Text = dt.Date.ToString();
                }
                
            }
        }

        protected void btnOut_Click(object sender, EventArgs e)
        {
            Class.Logout.InsertLogoutList(Global.GetConnection());
            this.Response.Redirect("Login.aspx");
        }

        protected void btnKanri_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("../管理ページ/Kanri.aspx");
        }
    }
}