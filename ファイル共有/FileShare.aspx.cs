using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace WhereEver
{
    public partial class FileShare : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //SqlConnection objDb = new SqlConnection();
            //SqlCommand objCom = new SqlCommand("INSERT INTO image_data(title,type,datum) VALUES(@title,@type,@datum)", objDb);            
            //// title、typeフィールドには、それぞれアップロード・ファイルの
            //// ファイル名、コンテンツ・タイプをセットする
            //objCom.Parameters.Add("@title", Path.GetFileName(datum.PostedFile.FileName));
            //objCom.Parameters.Add("@type", datum.PostedFile.ContentType);
            //// アップロード・ファイルを入力ストリーム経由でbyte配列に読み込む
            //Byte[] aryData = new Byte[datum.PostedFile.ContentLength];
            //datum.PostedFile.InputStream.Read(aryData, 0, datum.PostedFile.ContentLength);
            //objCom.Parameters.Add("@datum", aryData);
            
            //objDb.Open();
            //// データの登録
            //objCom.ExecuteNonQuery();
            //objDb.Close();
        }
    }
}