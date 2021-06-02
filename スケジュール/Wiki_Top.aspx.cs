using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Web.HttpUtility;


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

        protected void Button2_Click(object sender, EventArgs e)//編集ボタン
        {
            Response.Redirect("Wiki_Top.aspx");




        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string Search = TextBox1.Text;

            var dt = Class.Wiki.Search(Search, Global.GetConnection());

            dg1.DataSource = dt;

            dg1.DataBind();
        }


        public void Create()
        {
            var dt = Class.Wiki.GetT_WikiDataTable(Global.GetConnection());


            dg1.DataSource = dt;

            dg1.DataBind();
        }

        //protected void dg1_ItemDataBound(object sender, DataGridItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {

        //        var dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_WikiRow;

        //        e.Item.Cells[0].Text = dr.Date.ToString("yyyy年MMMMd日HH時mm分");
        //        e.Item.Cells[1].Text = dr.Name.ToString();
        //        e.Item.Cells[2].Text = dr.Title.ToString();



        //    }
        //}

        protected void dg1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        protected void dg1_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Read")
            {

                byte[] allbyte = new byte[0];

                int Read = e.Item.ItemIndex;

                var dt = Class.Wiki.GetT_WikiDataTable(Global.GetConnection());

                var dr = dt.Rows[Read] as DATASET.DataSet.T_WikiRow;

                if (!dr.IstypeNull())
                {
                    allbyte = allbyte.Concat(dr.datum).ToArray();

                    Label1.Text = dr.Title;

                    Label2.Text = dr.Text;

                    Label3.Text = "<img src=\"data:" + dr.type + ";base64," + HtmlEncode(Convert.ToBase64String(allbyte)) + "\" />";
                }
                else
                {
                    Label1.Text = dr.Title;

                    Label2.Text = dr.Text;

                    Label3.Text = "";
                }
            }
        }
    }
}
