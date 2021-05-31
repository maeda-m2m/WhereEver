using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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




            Class.Wiki.InsertT_Wiki(dt, FileUpload1.PostedFile, Global.GetConnection());

            DisplayFileContents(FileUpload1.PostedFile);//Upload


            Response.Redirect("Wiki_Top");


        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Wiki_Top");
        }

        void DisplayFileContents(HttpPostedFile file)
        {
            var dl = Class.Wiki.Maxid(Global.GetConnection());

            int no = dl.id;

            SqlConnection sqlco = Global.GetConnection();

            var da = new SqlCommand("", sqlco);

            da.CommandText = "UPDATE T_Wiki SET [type] = @type, [datum] =@datum where [id] = @id";


            //da.Parameters.AddWithValue("@title", Path.GetFileName(FileUpload1.PostedFile.FileName));

            da.Parameters.AddWithValue("@type", FileUpload1.PostedFile.ContentType);

            Byte[] aryData = new Byte[FileUpload1.PostedFile.ContentLength];

            FileUpload1.PostedFile.InputStream.Read(aryData, 0, FileUpload1.PostedFile.ContentLength);

            da.Parameters.AddWithValue("@datum", aryData);

            da.Parameters.AddWithValue("@id", no);

            sqlco.Open();

            da.ExecuteNonQuery();

            sqlco.Close();

        }

        SqlDataReader objRs;

        //void Page_Load(Object sender, EventArgs e)////表示
        //{
        //    if (!Page.IsPostBack)
        //    {
        //        SqlConnection objDb = new SqlConnection("Data Source=(local);User ID=sa;Password=sa;Persist Security Info=True;Initial Catalog=dotnet");
        //        // image_dataテーブルから取り出したファイル情報を
        //        // DropDownListコントロールにバインド
        //        SqlCommand objCom = new SqlCommand("SELECT id,title FROM image_data", objDb);
        //        objDb.Open();
        //        objRs = objCom.ExecuteReader();
        //        Page.DataBind();
        //        objDb.Close();
        //    }
        //}
        //void objBtn_Click(Object sender, EventArgs e)
        //{
        //    SqlConnection objDb = new SqlConnection("Data Source=(local);User ID=sa;Password=sa;Persist Security Info=True;Initial Catalog=dotnet");
        //    // ドキュメントIDをキーに
        //    // DropDownListコントロールで指定されたファイルを取得
        //    SqlCommand objCom = new SqlCommand("SELECT type,datum FROM image_data WHERE id=@id", objDb);
        //    objCom.Parameters.Add("@id", objDdp.SelectedItem.Value);
        //    objDb.Open();
        //    objRs = objCom.ExecuteReader();
        //    // データベースから取得したファイルを出力する。
        //    // typeフィールドの値をContentTypeプロパティにセットし、
        //    // datumフィールドの値をBinaryWriteメソッドで出力する。
        //    // それぞれ取得したフィールド値はキャストする必要がある。

        //    if (objRs.Read())
        //    {
        //        Response.ContentType = (String)objRs[0];
        //        Response.BinaryWrite((Byte[])objRs[1]);
        //    }
        //    objDb.Close();

        //    Response.End();
        //}
    }
}