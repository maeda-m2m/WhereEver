using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WhereEver.スケジュール
{
    public partial class Wiki_Top : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }
        }

        protected void Button1_Click(object sender, EventArgs e)//登録ボタン
        {
            Response.Redirect("Wiki.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }


        public void Create()
        {
            var dt = Class1.GetT_Schedule3DataTable(Global.GetConnection());

            dg1.DataSource = dt;

            dg1.DataBind();
        }
    }
}