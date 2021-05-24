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

            DATASET.DataSet.T_Chat_CHRow ch = Class.Chat_CH.GetT_Chat_CH(Global.GetConnection(), SessionManager.User.M_User.id);

            DateTime time = DateTime.Now;
            if(ch == null)
            {
                DATASET.DataSet.T_LogoutListDataTable dt = Class.Chat.GetLastLogoutTime(Global.GetConnection(), SessionManager.User.M_User.name);
                if (dt != null)
                {
                    time = DateTime.Parse(dt[0][2].ToString());
                }
            }
            else
            {
                time = ch.Date;
            }
            DATASET.DataSet.T_ChatDataTable cdt = Class.Chat.NewHensin(Global.GetConnection(), time);

            //test
            if (ch != null)
            {
                Class.Chat_CH.SetT_Chat_CHUpdate(Global.GetConnection(), SessionManager.User.M_User.id);
            }
            else
            {
                Class.Chat_CH.SetT_Chat_CHInsert(Global.GetConnection(), SessionManager.User.M_User.id);
            }

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