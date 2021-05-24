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

            //初期化
            DATASET.DataSet.T_ChatRow ch3 = null;


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

                //誰かのチャット通知
                time = ch.Date;

                //返信通知
                DATASET.DataSet.T_ChatDataTable ch2 = Class.Chat_CH.GetT_Chat_Distinct(Global.GetConnection(), SessionManager.User.M_User.id);
                for(int i = 0; i < ch2.Count; i++)
                {
                    ch3 = Class.Chat_CH.GetT_Chat_Reply(Global.GetConnection(), SessionManager.User.M_User.id, ch2[i].No, time);
                    if (ch3 != null)
                    {
                        break;
                    }
                }
            }

            //誰かのチャット通知
            DATASET.DataSet.T_ChatDataTable cdt = Class.Chat.NewHensin(Global.GetConnection(), time);



            if (cdt != null)
            {
                if (ch3 != null)
                {
                    //返信通知
                    lblHensin.Text = Environment.NewLine + "<font color = blue>*";
                    lblHensin.Visible = true;
                }
                else
                {
                    //誰かのチャット通知
                    lblHensin.Text = Environment.NewLine + "<font color = red>*";
                    lblHensin.Visible = true;
                }

            }
            else
            {
                lblHensin.Visible = false;
            }
        }
    }
}