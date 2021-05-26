using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WhereEver.スケジュール
{
    public partial class Wiki : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }
        }

        protected void Button1_Click(object sender, EventArgs e)//登録
        {
            var dt = Class.Wiki.GetT_WikiDataTable(Global.GetConnection());

            var dr = dt.NewT_WikiRow();

            var dl = Class.Wiki.Maxid(Global.GetConnection());

            int no = dl.id;



            DateTime datetime = DateTime.Now;
            datetime = DateTime.Parse(datetime.ToString());

            string name = "";

            string Title = TextBox2.Text;

            string Text1 = TextBox1.Text;

            dr.id = no + 1;
            dr.Date = datetime;
            dr.Name = name;
            dr.Title = Title;
            dr.Text = Text1;

            dt.AddT_WikiRow(dr);

            Class.Wiki.InsertT_Wiki(dt, Global.GetConnection());

            Response.Redirect("Wiki_Top");

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Wiki_Top");
        }
    }
}