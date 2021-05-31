using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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

        protected void Button2_Click(object sender, EventArgs e)//編集ボタン
        {
            Response.Redirect("Wiki_Top.aspx");
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


            //int sdl = dr.id;
            if (e.CommandName == "Read")
            {

                int Read = e.Item.ItemIndex;

                var dt = Class.Wiki.GetT_WikiDataTable(Global.GetConnection());

                var dr = dt.Rows[Read] as DATASET.DataSet.T_WikiRow;

                Label1.Text = dr.Title;

                Label2.Text = dr.Text;

            }
        }

        //private void test()
        //{
        //      byte datum = fileupload.text;

        //    SqlConnection sqlco = Global.GetConnection();

        //    var da = new SqlDataAdapter("", sqlco);

        //    da.SelectCommand.CommandText = "INSERT INTO image_data(title,type,datum) VALUES(@title,@type,@datum)";


        //    da.SelectCommand.Parameters.AddWithValue("@title", Path.GetFileName(datum.PostedFile.FileName));

        //    da.SelectCommand.Parameters.AddWithValue("@type", datum.PostedFile.ContentType);

        //    // アップロード・ファイルを入力ストリーム経由でbyte配列に読み込む

        //    Byte[] aryData = new Byte[datum.PostedFile.ContentLength];

        //    datum.PostedFile.InputStream.Read(aryData, 0, datum.PostedFile.ContentLength);

        //    da.SelectCommand.Parameters.AddWithValue("@datum", aryData);

        //    sqlco.Open();


        //    da.ExecuteNonQuery();

        //}

        //SqlDataReader objRs;
        //void Page_Load(Object sender, EventArgs e)
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

        //    if(objRs.Read()) {
        //        Response.ContentType = (String)objRs[0];
        //        Response.BinaryWrite((Byte[])objRs[1]);
        //    }
        //    objDb.Close();

        //    Response.End();
        //}

    }
}
