using System;
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
                    DataSet2.T_LoginListDataTable dt = Class2.GetLoginListDataTable(Global.GetConnection());

                    DgTimeDetail.DataSource = dt;

                    DgTimeDetail.DataBind();

                }
            }

        protected void DgTimeDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {

                DataSet2.T_LoginListRow dr = (e.Item.DataItem as DataRowView).Row as DataSet2.T_LoginListRow;
                Label lblName = e.Item.FindControl("lblName") as Label;
                Label lblLoginTime = e.Item.FindControl("lblLoginTime") as Label;
                Label lblLogoutTime = e.Item.FindControl("lblLogoutTime") as Label;
                DataSet2.T_LoginListRow dl = Class2.UserLoginMAXTime(Global.GetConnection(), dr.name.ToString());
                DataSet2.T_LoginListRow dt = Class2.UserLogoutMAXTime(Global.GetConnection(), dr.name.ToString());

                lblName.Text = dr.name.ToString();
                lblLoginTime.Text = dl.Date.ToString();
                lblLogoutTime.Text = dt.Date.ToString();
            }
        }

        protected void btnOut_Click(object sender, EventArgs e)
        {
            //DataSet2.M_UserRow dr = Class1.GetM_UserRow(ID, PW, Global.GetConnection());


        }
    }
}