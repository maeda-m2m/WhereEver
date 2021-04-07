using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WhereEver.Project_System
{
    public partial class PIchiran : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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

                lblName.Text = dr.name.ToString();
                lblLoginTime.Text = dl.Date.ToString();
                if (dt == null)
                {
                    lblLogoutTime.Text = "";
                }
                else
                {
                    lblLogoutTime.Text = dt.Date.ToString();
                }

            }
        }
    }
}