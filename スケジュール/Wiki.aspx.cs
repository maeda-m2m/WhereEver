using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Web.HttpUtility;

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

            string Title = TextBox1.Text;

            StringBuilder sb = new StringBuilder();
            sb.Append(HtmlEncode(TextBox2.Text));

            sb.Replace("\r\n", "<br>");


            sb.Replace("&lt;br /&gt;", "<br />");

            sb.Replace("&lt;p&gt;", "<p>");

            sb.Replace("&lt;/p&gt;", "</p>");

            sb.Replace("&lt;ol&gt;", "<ol>");
            sb.Replace("&lt;/ol&gt;", "</ol>");
            sb.Replace("&lt;ul&gt;", "<ul>");
            sb.Replace("&lt;/ul&gt;", "</ul>");
            sb.Replace("&lt;li&gt;", "<li>");
            sb.Replace("&lt;/li&gt;", "</li>");
            sb.Replace("&lt;a&gt;", "<a>");
            sb.Replace("&lt;/a&gt;", "</a>");

            //sb.Replace("&lt;a href=&gt;", "<a href=>");

            sb.Replace("&lt;strong&gt;", "<strong>");

            sb.Replace("&lt;/strong&gt;", "</strong>");
            sb.Replace("&lt;div&gt;", "<div>");
            sb.Replace("&lt;/div&gt;", "</div>");
            sb.Replace("&lt;select&gt;", "<select>");
            sb.Replace("&lt;/select&gt;", "</select>");
            sb.Replace("&lt;option&gt;", "<option>");
            sb.Replace("&lt;/option&gt;", "</option>");
            sb.Replace("&lt;style&gt;", "<style>");
            sb.Replace("&lt;/style&gt;", "</style>");

            sb.Replace("&lt;&gt;", "<>");
            sb.Replace("&lt;/&gt;", "</>");

            sb.Replace("&lt;html&gt;", "<html>");
            sb.Replace("&lt;/html&gt;", "</html>");

            sb.Replace("&lt;head&gt;", "<head>");
            sb.Replace("&lt;/head&gt;", "</head>");

            sb.Replace("&lt;form&gt;", "<form>");
            sb.Replace("&lt;/form&gt;", "</form>");

            sb.Replace("&lt;title&gt;", "<title>");
            sb.Replace("&lt;/title&gt;", "</title>");

            sb.Replace("&lt;section&gt;", "<section>");
            sb.Replace("&lt;/section&gt;", "</section>");

            sb.Replace("&lt;body&gt;", "<body>");
            sb.Replace("&lt;/body&gt;", "</body>");



            dr.id = no + 1;
            dr.Date = datetime;
            dr.Name = name;
            dr.Title = Title;
            dr.Text = sb.ToString();

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