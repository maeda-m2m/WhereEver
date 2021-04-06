﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WhereEver
{
    public partial class LoginList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DATASET.DataSet.T_LoginListDataTable dt = Class2.GetLoginListDataTable(Global.GetConnection());

                DgTimeDetail.DataSource = dt;
                DgTimeDetail.DataBind();

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
                DATASET.DataSet.T_LoginListRow dl = Class2.UserLoginMAXTime(Global.GetConnection(), dr.name.ToString());
                DATASET.DataSet.T_LoginListRow dt = Class2.UserLogoutMAXTime(Global.GetConnection(), dr.name.ToString());

                lblName.Text = dr.name.ToString();
                lblLoginTime.Text = dl.Date.ToString();
                lblLogoutTime.Text = dt.Date.ToString();
            }
        }

        protected void btnOut_Click(object sender, EventArgs e)
        {
            Class.Logout.InsertLogoutList(Global.GetConnection());
        }
    }
}