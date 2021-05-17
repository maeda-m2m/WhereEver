using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WhereEver
{
    public partial class MenuControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = SessionManager.User.M_User.name1;

            DATASET.DataSet.T_LogoutListDataTable dt = Class.Chat.GetLastLogoutTime(Global.GetConnection(), SessionManager.User.M_User.name);
            string time = dt[0][2].ToString();

            DATASET.DataSet.T_ChatDataTable cdt = Class.Chat.NewHensin(Global.GetConnection(), time);

            if (cdt != null)
            {
                lblHensin.Text = Environment.NewLine+"<font color = red>*";
                lblHensin.Visible = true;
            }
            else
            {
                lblHensin.Visible = false;
            }
        }
    }
}