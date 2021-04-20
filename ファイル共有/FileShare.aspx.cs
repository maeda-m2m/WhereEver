using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using WhereEver.ClassLibrary;

namespace WhereEver
{
    public partial class FileShare : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            //[注意！]まだ十分なテストが完了していません！


        }

        protected void Button_UpLoad(object sender, EventArgs e)
        {
            //ラベル初期化
            lblResult.Text = "";

            //inputの場合はnameで参照する
            //HttpPostedFile posted = Request.Files["userfile"];

            if (FileUpload_userfile.HasFile)
            {
                //アップロードするファイル名を設定（@"C\:temp"だけなどは危険なためデバッグ時以外使用禁止）
                //書き込み先ディレクトリがない場合はエラーになります。
                string path = @"c:\\UploadedFiles\\";

                if (!Directory.Exists(path))
                {                
                    //ディレクトリが存在しません。このままだとエラーになるため、ディレクトリを生成します。
                    Directory.CreateDirectory(path);
                }


                //パスを排除したファイル名を取得
                string fileName = Path.GetFileName(FileUpload_userfile.FileName);
                //拡張子を取得
                string extension = Path.GetExtension(FileUpload_userfile.FileName);


                /*
                //MimeTypeの設定に合っているかどうか？
                if (extension != ".zip")
                {
                    lblResult.Text = "zipファイルのみアップロードできます。";
                    return;
                }
                */


                if (fileName != null && fileName != "")
                {

                    //不正なファイル名にならないようuuidに置き換え
                    fileName = Guid.NewGuid().ToString() + extension;

                    //保存先パスを設定
                    string filePath = Path.Combine(path, fileName);

                    //同名ディレクトリが見つかったときは連番にします（上書き防止）。
                    int i = 1;
                    string newPath = filePath;
                    while (Directory.Exists(newPath))
                    {
                        newPath = $"{path} ({i++})";
                    }

                    try
                    {

                        //アップロードファイルを指定したパスに保存します。
                        FileUpload_userfile.SaveAs(newPath);

                        //データベースにファイル履歴を保存します。
                        FileShareClass.SetT_FileShareInsert(Global.GetConnection(), SessionManager.User.M_User.id, fileName, newPath);

                    }
                    catch (Exception ex)
                    {
                        lblResult.Text = ex + "により、アップロードに失敗しました。";
                        return;
                    }

                    lblResult.Text = "アップロードファイルを以下のディレクトリに保存しました： " + newPath;
                    return;
                }
                else
                {
                    lblResult.Text = "ファイル名が見つかりません！";
                    return;
                }

            } else
            {
                lblResult.Text = "ローカルからファイルを選択して下さい。";
                return;
            }

        }



        protected void Button_DownLoad(object sender, EventArgs e)
        {
            lblDLResult.Text = "";

            if (TextBox_dl.Text != null && TextBox_dl.Text != "")
            {
                //アップロードするファイル名を設定（@"C\:temp"だけなどは危険なためデバッグ時以外使用禁止）
                //書き込み先ディレクトリがない場合はエラーになります。
                string path = @"c:\\UploadedFiles\\";

                if (Directory.Exists(path))
                {
                    //ディレクトリが存在します。
                }
                else
                {
                    //ディレクトリが存在しません。
                    lblDLResult.Text = "ファイルディレクトリが存在しません。";
                    return;
                }

                //保存先パスを設定
                string filePath = Path.Combine(path, TextBox_dl.Text + ".zip");

                if (!Directory.Exists(filePath))
                {
                    //ファイルが存在しません。
                    lblDLResult.Text = "指定したファイルが存在しません。";
                    return;
                }

                try
                {


                    //Response情報クリア
                    Response.ClearContent();

                    //バッファリング
                    Response.Buffer = true;

                    //HTTPヘッダー情報・MIMEタイプ設定
                    Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", lblDLResult.Text));
                    //好きなMIMEタイプを設定
                      Response.ContentType = DropDownList1.SelectedValue;

                    //ファイルを書き出し
                    Response.WriteFile(filePath);
                    Response.Flush();
                    Response.End();

                    lblDLResult.Text = "ダウンロードに成功しました。";
                    return;
                }
                catch (Exception ex)
                {
                    lblDLResult.Text = ex + "によりダウンロードに失敗しました。";
                    return;
                }

            }
            else
            {
                lblDLResult.Text = "ダウンロードファイルを選択して下さい。";
                return;
            }

        }


    }
}