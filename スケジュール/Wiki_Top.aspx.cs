using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using WhereEver.ClassLibrary;

namespace WhereEver.スケジュール
{
    public partial class Wiki_Top : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Create();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)//登録ボタン
        {
            Response.Redirect("Wiki.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Wiki_Top.aspx");
        }


        public void Create()
        {
            var dt = Class.Wiki.GetT_WikiDataTable(Global.GetConnection());


            dg1.DataSource = dt;

            dg1.DataBind();
        }

        protected void dg1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                var dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_WikiRow;

                e.Item.Cells[0].Text = dr.Date.ToString("yyyy年MMMMd日");
                e.Item.Cells[1].Text = dr.Name.ToString();
                e.Item.Cells[2].Text = dr.Title.ToString();



            }
        }

        protected void dg1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        protected void dg1_ItemCommand(object sender, DataGridCommandEventArgs e)
        {


            //int sdl = dr.id;
            if (e.CommandName == "Read")
            {

                int Read = e.Item.ItemIndex;

                var dt = Class.Wiki.GetT_WikiDataTable(Global.GetConnection());

                var dr = dt.Rows[Read] as DATASET.DataSet.T_WikiRow;

                Label1.Text = dr.Title;

                Label2.Text = dr.Text;

                //if (sdl > 0)
                //    Class1.DeleteList(sdl, Global.GetConnection());
                //dg1.Items[Read].FindControl("No");


                //Label1.Text = "";
                //string result = FileShareClass.Get_File_DownLoad_src(Page.Response, HtmlEncode(TextBox_dl.Text), TextBox_DownloadPass.Text, LoadByteLength());
                //Label1.Text = @result;
            }
        }

    }
}
