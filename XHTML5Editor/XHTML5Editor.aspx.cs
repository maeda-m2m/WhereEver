using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using static System.Web.HttpUtility;
using System.Net;
using System.Net.Http;

namespace WhereEver.XHTML5Editor
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SessionManager.User.M_User.id == null || SessionManager.User.M_User.id.Trim() == "")
            {
                //不正ログイン防止
                this.Response.Redirect("../ログイン/Login.aspx", false);
                return;
            }
        }

        protected void Push_Correct(object sender, EventArgs e)
        {
            string width = HtmlEncode(TextBox_Width.Text);
            string height = HtmlEncode(TextBox_Height.Text);
            string color = HtmlEncode(TextBox_Color.Text);
            string bcolor = HtmlEncode(TextBox_BColor.Text);
            string hcolor = HtmlEncode(TextBox_HColor.Text);
            string hbcolor = HtmlEncode(TextBox_HBColor.Text);
            string border = HtmlEncode(TextBox_Border.Text);
            string bordercolor = HtmlEncode(TextBox_BorderColor.Text);
            string radius = HtmlEncode(TextBox_Radius.Text);
            string btntext = HtmlEncode(TextBox_BTNText.Text);
            string fontfamily = HtmlEncode(TextBox_FontFamily.Text);

            const int maxpx = 300;
            const int maxem = 25;

            //---------------------------------
            StringBuilder sb = new StringBuilder();
            //---------------------------------
            sb.Append("<style>");
            sb.Append("\r\n");
            sb.Append(".userstyle { ");
            sb.Append("\r\n");
            sb.Append("-webkit-appearance: none;");//スマホ対策
            sb.Append("\r\n");
            sb.Append(" width: "); 
            if(DropDownList_Width.SelectedValue != "auto")
            {
                if (DropDownList_Width.SelectedValue == "px")
                {
                    int.TryParse(width, System.Globalization.NumberStyles.Currency, null, out int value);
                    width = Math.Min(value, maxpx).ToString();
                    width = Math.Max(value, 0).ToString();
                }
                else if (DropDownList_Width.SelectedValue == "em")
                {
                    int.TryParse(width, System.Globalization.NumberStyles.Currency, null, out int value);
                    width = Math.Min(value, maxem).ToString();
                    width = Math.Max(value, 0).ToString();
                }
                sb.Append(width);
            }
            sb.Append(DropDownList_Width.Text);
            sb.Append("; ");
            sb.Append("\r\n");
            sb.Append(" height: ");
            if (DropDownList_Height.SelectedValue != "auto")
            {
                if (DropDownList_Height.SelectedValue == "px")
                {
                    int.TryParse(height, System.Globalization.NumberStyles.Currency, null, out int value);
                    height = Math.Min(value, maxpx).ToString();
                    height = Math.Max(value, 0).ToString();
                }
                else if (DropDownList_Height.SelectedValue == "em")
                {
                    int.TryParse(height, System.Globalization.NumberStyles.Currency, null, out int value);
                    height = Math.Min(value, maxem).ToString();
                    height = Math.Max(value, 0).ToString();
                }
                sb.Append(height);
            }
            sb.Append(DropDownList_Height.Text);
            sb.Append("; ");
            sb.Append("\r\n");
            sb.Append(" color: ");
            sb.Append(color);
            sb.Append("; ");
            sb.Append("\r\n");
            sb.Append(" background-color: ");
            sb.Append(bcolor);
            sb.Append("; ");
            sb.Append("\r\n");
            sb.Append(" display: inline-block; ");
            sb.Append("\r\n");
            sb.Append(" padding: 0.15em 1em; ");
            sb.Append("\r\n");
            sb.Append(" text-decoration: none; ");
            sb.Append("\r\n");
            sb.Append(" border: solid ");
            if (DropDownList_Border.SelectedValue != "auto")
            {
                if (DropDownList_Border.SelectedValue == "px")
                {
                    int.TryParse(border, System.Globalization.NumberStyles.Currency, null, out int value);
                    border = Math.Min(value, maxpx).ToString();
                    border = Math.Max(value, 0).ToString();
                }
                else if (DropDownList_Border.SelectedValue == "em")
                {
                    int.TryParse(border, System.Globalization.NumberStyles.Currency, null, out int value);
                    border = Math.Min(value, maxem).ToString();
                    border = Math.Max(value, 0).ToString();
                }
                sb.Append(border);
            }
            sb.Append(DropDownList_Border.Text);
            sb.Append(" ");
            sb.Append(bordercolor);
            sb.Append("; ");
            sb.Append("\r\n");
            sb.Append(" border-radius: ");
            if (DropDownList_Radius.SelectedValue != "auto")
            {
                if (DropDownList_Radius.SelectedValue == "px")
                {
                    int.TryParse(radius, System.Globalization.NumberStyles.Currency, null, out int value);
                    radius = Math.Min(value, maxpx * 10).ToString();
                    radius = Math.Max(value, 0).ToString();
                }
                else if (DropDownList_Radius.SelectedValue == "em")
                {
                    int.TryParse(radius, System.Globalization.NumberStyles.Currency, null, out int value);
                    radius = Math.Min(value, maxem * 10).ToString();
                    radius = Math.Max(value, 0).ToString();
                }
                sb.Append(radius);
            }
            sb.Append(DropDownList_Radius.Text);
            sb.Append("; ");
            sb.Append("\r\n");
            sb.Append(" transition: .4s; ");
            sb.Append("\r\n");
            sb.Append(" font-family: ");
            sb.Append(fontfamily);
            sb.Append("; ");
            sb.Append("\r\n");
            sb.Append("}");
            sb.Append("\r\n");
            //---------------------------------
            sb.Append("\r\n");
            sb.Append(".userstyle:hover { ");
            sb.Append("\r\n");
            sb.Append(" color: ");
            sb.Append(hcolor);
            sb.Append("; ");
            sb.Append("\r\n");
            sb.Append(" background-color: ");
            sb.Append(hbcolor);
            sb.Append("; ");
            sb.Append("\r\n");
            if (CheckBox_Pointer.Checked)
            {
                sb.Append(" cursor: pointer;");
                sb.Append("\r\n");
            }
            sb.Append("}");
            sb.Append("\r\n");
            sb.Append("</style>");
            sb.Append("\r\n");
            //---------------------------------
            sb.Append("\r\n");
            sb.Append("<button name=\"userbutton\" class=\"userstyle\" type=\"button\">");
            sb.Append("\r\n");
            sb.Append(btntext);
            sb.Append("\r\n");
            sb.Append("</button>");
            sb.Append("\r\n");

            //sb.Append("<asp:Button ID=\"Button_Correct\" CssClass=\"userstyle\" runat=\"server\" Text=\"Button\" />");
            Label_ButtonResult.Text = sb.ToString();
            TextBox_ButtonCodeResult.Text = sb.ToString();

        }

        protected void Push_Button(object sender, EventArgs e)
        {
            if (Panel_BT.Visible)
            {
                Panel_BT.Visible = false;
            }
            else
            {
                Panel_BT.Visible = true;
            }
        }

        protected void Push_QRCode(object sender, EventArgs e)
        {
            if (Panel_QRCode.Visible)
            {
                Panel_QRCode.Visible = false;
            }
            else
            {
                Panel_QRCode.Visible = true;
            }
        }

        protected void Push_IndexBox(object sender, EventArgs e)
        {
            if (Panel_IndexBox.Visible)
            {
                Panel_IndexBox.Visible = false;
            }
            else
            {
                Panel_IndexBox.Visible = true;
            }
        }

        protected void Push_QR_Create(object sender, EventArgs e)
        {
            if(TextBox_QR_Text.Text == "")
            {
                TextBox_QRCode_Result.Text = "QRコードに変換したいテキストを入力して下さい。";
                return;
            }

            int.TryParse(HtmlEncode(TextBox_QRCode_Width.Text), System.Globalization.NumberStyles.Currency, null, out int width);
            int.TryParse(HtmlEncode(TextBox_QRCode_Height.Text), System.Globalization.NumberStyles.Currency, null, out int height);

            //上限
            width = Math.Min(1000, width);
            height = Math.Min(1000, height);

            //下限
            width = Math.Max(100, width);
            height = Math.Max(100, height);

            Label_QRCode_Area.Text = ClassLibrary.QRCodeClass.GetQRCode(HtmlEncode(TextBox_QR_Text.Text), width, height);
            TextBox_QRCode_Result.Text = "/*[生成されたコード]*/\r\f" + Label_QRCode_Area.Text;
        }

        protected void Push_QR_Encode(object sender, EventArgs e)
        {
            if (FileUpload_userfile.HasFile)
            {
                HttpPostedFile filename = FileUpload_userfile.PostedFile;
                TextBox_QRCode_Result.Text = "/*[読み込まれた内容]*/\r\f" + ClassLibrary.QRCodeClass.SetQRCode(filename);
            }
            else
            {
                TextBox_QRCode_Result.Text = "QRコードから読み込みたいファイルを選択して下さい。";
                return;
            }
        }


        protected string GetWebString(string url)
        {
            WebClient wc = new WebClient();
            //HttpClient hc = new HttpClient();
            try
            {
                //1 WebClientの場合
                wc.Encoding = Encoding.UTF8;//utf-8エンコードを指定しないと文字化けする。

                if (url == "" || url == null)
                {
                    url = Request.Url.AbsoluteUri + ".aspx";
                }
                

                //Request.Url.AbsoluteUriが使用できない？　→　Proxy経由の問題

                //方法１　お手軽　コードイン　失敗
                IWebProxy wp = WebRequest.DefaultWebProxy;
                wp.Credentials = CredentialCache.DefaultCredentials;
                wc.Proxy = wp;

#if DEBUG
                //方法２　方法１の亜種
                /*
                 *   WebProxy wp = new WebProxy(" proxy server url here");
                 *   wc.Proxy = wp;
                 *   string str = client.DownloadString("http://www.example.com");
                 */

                //方法３　リリース向け　おすすめ
                /* Web.configに下記を追加
                 * <configuration>
                 * <system.net>
                 * <defaultProxy>
                 *   <proxy useDefaultCredentials="True" />
                 *   
                 *   ※↑でダメなら↓
                 *   <proxy 
                 *     usesystemdefault = "false"
                 *     proxyaddress="http://proxyserver"
                 *     bypassonlocal="true"
                 *      />
                 *   
                 * </defaultProxy>
                 * </system.net> 
                 * <configuration>
                 */
#endif
                //uif-8を指定
                wc.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows XP)");
                return wc.DownloadString(url);    //ここでデータベース等をもとに複数のURLを入れ込んでLIKE文で検索するとよい。
            }
            catch (WebException ex)
            {
                TextBox_IBResult.Text = ex.Message;
                return ex.Message;
            }

        }

        protected void Set_IBDataBase()
        {
            //init
            TextBox_IBResult.Text = "";

            string ib = HtmlEncode(TextBox_IB.Text).Trim(); //空白スペースでsplitしてstring[]にしてもよい。

            //登録用文字列 HtmlEncodeあり
            string text = HtmlEncode(TextBox_IBText.Text);
            if (text == "")
            {
                //登録不可能
                TextBox_IBResult.Text = "対象ボックスに辞書データが入力されていません。";
                return;
            }

            //textの中身が検索対象の文字列。キーワードが検索対象に部分一致するかどうか調べる。

            text = text.Replace("、", "、；").Replace("。", "。；").Replace("！？", "！？；").Replace("！", "！；").Replace("？", "？；").Replace("\r", "；").Replace("\n", "；");//改行コードをchar'；'に置き換え　、と。を消えないように修正。
            string[] sp = text.Split("；".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);//空白文字をトリム              


            StringBuilder sb = new StringBuilder();
            sb.Append("/* 辞書登録結果一覧 */\r\f");
            sb.Append("センテンス(");
            sb.Append(sp.Length);
            sb.Append("件)\r\f");

            for (int i = 0; i < sp.Length; i++)
            {
                string str = sp[i].ToString();
                if (!str.Contains("、") && !str.Contains("。") && !str.Contains("！？") && !str.Contains("！") && !str.Contains("？"))
                {
                    //センテンスではない可能性がある。
                    //出力用メモ
                    sb.Append("[センテンスではない？(Caution)]");
                    sb.Append(str);
                    sb.Append("\r\f");
                    continue;
                }

                //int len = Math.Min(str.Length, 10);１行の文字数に制限をかける場合　辞書データが精確なら不要

                if (!ClassLibrary.SentenceAIClass.GetIsT_SentenceAI(Global.GetConnection(), str))
                {
                    //string strを辞書(DB)に登録する
                    ClassLibrary.SentenceAIClass.SetT_SentenceAI_Insert(Global.GetConnection(), str.Trim(), SessionManager.User.M_User.name1.Trim());

                    //出力用メモ
                    sb.Append(str);
                    sb.Append("\r\f");
                }
                else
                {
                    //アップデート
                    ClassLibrary.SentenceAIClass.SetT_SentenceAI_Update(Global.GetConnection(), str.Trim());

                    //出力用メモ
                    sb.Append("[重複(DateTime_Update)]");
                    sb.Append(str);
                    sb.Append("\r\f");
                }
            }
            TextBox_IBResult.Text = sb.ToString();
        }

        protected void Push_IBCorrect(object sender, EventArgs e)
        {
            //init
            TextBox_IBResult.Text = "";

            string ib = HtmlEncode(TextBox_IB.Text).Trim(); //空白スペースでsplitしてstring[]にしてもよい。
            if (ib == "")
            {
                //検索不可能
                TextBox_IBResult.Text = "検索ボックスにキーワードが入力されていません。";
                return;
            }


            /*
            //WebPageの情報をそのまま持ってくる m2mのプロキシ設定をWeb.configに直接記入しないと機能しない
            string url = Request.Url.AbsoluteUri + ".aspx";
            string text = GetWebString(url);
            */


            //実験用 HtmlEncodeあり　空でも可（出力なし）
            string text = HtmlEncode(TextBox_IBText.Text);




            //textの中身が検索対象の文字列。キーワードが検索対象に部分一致するかどうか調べる。

            StringBuilder sb = new StringBuilder();
            sb.Append("/* \"" + ib + "\" の検索結果 */\r\f");
            if (text.Contains(ib))
            {
                //SplitはStringではなくCharとして扱うため、Stringを条件にしても１文字ずつ検証されてしまう。
                //これを防ぐためには、元の文をibから独自の予約文字１字に置き換えるとよい。
                //例えば、改行コードを除外した後で、改行コードに変換する。

                text = text.Replace("\r", "").Replace("\n", "");
                text = text.Replace(ib, "\r\f");

                string[] sp = text.Split("\r\f".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);//空白文字をトリム
                sb.Append("部分一致(");
                sb.Append(sp.Length - 1);
                sb.Append("件)\r\f");



                for (int i = 0; i < sp.Length; i++)
                {
                    if (i + 1 >= sp.Length)
                    {
                        break;
                    }


                    string str = sp[i].ToString();
                    int len = Math.Min(str.Length, 10);

                    // ????????? + ib
                    //len文字から末尾までとる（後ろからlen文字とる）
                    //文字数が少ない場合は全部とる
                    sb.Append("index: ");
                    sb.Append(i);
                    sb.Append("(Length: ");
                    if (i + 1 < sp.Length)
                    {
                        sb.Append(str.Length + sp[i + 1].ToString().Length + ib.Length);
                    }
                    else
                    {
                        //文末
                        sb.Append(str.Length + ib.Length);
                    }
                    sb.Append(")");
                    //-----------------
                    sb.Append("\r\f[...]");
                    sb.Append(str.Substring(str.Length - len));
                    sb.Append(" ");
                    sb.Append(ib);
                    sb.Append(" ");


                    // ib + ?????????
                    //前からlen文字とる
                    //文字数が少ない場合は全部とる
                    str = sp[i + 1].ToString();
                    len = Math.Min(str.Length, 10);
                    sb.Append(str.Substring(0, len));
                    sb.Append("[...]\r\f");

                }

            }
            else
            {
                sb.Append("不一致(0件)\r\f");
            }

            //全文
            //sb.Append(text);

            TextBox_IBResult.Text = sb.ToString();
        }

        protected void Push_SetIndexBook(object sender, EventArgs e)
        {
            Set_IBDataBase();
        }

        protected void Push_GetIndexBook(object sender, EventArgs e)
        {
            Get_IndexBook();
        }



        protected void Get_IndexBook()
        {
            //init
            TextBox_IBResult.Text = "";

            string ib = HtmlEncode(TextBox_IB.Text).Trim(); //空白スペースでsplitしてstring[]にしてもよい。

            //登録用文字列 HtmlEncodeあり
            string text = HtmlEncode(TextBox_IBText.Text);
            if (text == "")
            {
                //登録不可能
                TextBox_IBResult.Text = "対象ボックスに辞書データが入力されていません。";
                return;
            }

            //textの中身が検索対象の文字列。キーワードが検索対象DBに部分一致するかどうか調べる。

            text = text.Replace("、", "、；").Replace("。", "。；").Replace("！？", "！？；").Replace("！", "！；").Replace("？", "？；").Replace("\r", "；").Replace("\n", "；");//改行コードをchar'；'に置き換え　、と。を消えないように修正。
            string[] sp = text.Split("；".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);//空白文字をトリム              


            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            sb.Append("/* 辞書登録結果一覧 */\r\f");
            sb.Append("センテンス(");
            sb.Append(sp.Length);
            sb.Append("件)\r\f");

            for (int i = 0; i < sp.Length; i++)
            {
                string str = sp[i].ToString();

                if(!str.Contains("、") && !str.Contains("。") && !str.Contains("！？") && !str.Contains("！") && !str.Contains("？"))
                {
                    //センテンスではない可能性がある。
                    //出力用メモ
                    sb.Append("[センテンスではない？(Caution)]");
                    sb.Append(str);
                    sb.Append("\r\f");
                    continue;
                }

                //int len = Math.Min(str.Length, 10);１行の文字数に制限をかける場合　辞書データが精確なら不要

                //キーワードをセンテンスに区切る。対象のセンテンスは含まれているか？
                if (!ClassLibrary.SentenceAIClass.GetIsT_SentenceAI(Global.GetConnection(), str))
                {
                    //string strを辞書(DB)に登録する
                    ClassLibrary.SentenceAIClass.SetT_SentenceAI_Insert(Global.GetConnection(), str.Trim(), SessionManager.User.M_User.name1.Trim());


                    //部分一致を検索
                    DATASET.DataSet.T_SentenceAIDataTable dt = ClassLibrary.SentenceAIClass.GetT_SentenceAIDataTable(Global.GetConnection(), str.Trim().Substring(0, str.Length / 10));
                    if (dt != null)
                    {
                        Random rand = new Random();
                        sb2.Append(dt[rand.Next(0, dt.Count-1)].sentence);
                    }
                    else
                    {
                        //部分一致を検索
                        DATASET.DataSet.T_SentenceAIDataTable dt2 = ClassLibrary.SentenceAIClass.GetT_SentenceAIDataTable(Global.GetConnection(), str.Trim().Substring(0,str.Length/100));
                        if (dt2 != null)
                        {
                            Random rand = new Random();
                            sb2.Append(dt2[rand.Next(0, dt2.Count - 1)].sentence);
                        }

                    }

                    //出力用メモ
                    sb.Append(str);
                    sb.Append("\r\f");
                }
                else
                {
                    //完全一致
                    //アップデート
                    ClassLibrary.SentenceAIClass.SetT_SentenceAI_Update(Global.GetConnection(), str.Trim());

                    //部分一致を検索
                    DATASET.DataSet.T_SentenceAIDataTable dt = ClassLibrary.SentenceAIClass.GetT_SentenceAIDataTable(Global.GetConnection(), str.Trim().Substring(0, str.Length / 10));
                    if (dt != null)
                    {
                        Random rand = new Random();
                        sb2.Append(dt[rand.Next(0, dt.Count - 1)].sentence);
                    }
                    else
                    {
                        //部分一致を検索
                        DATASET.DataSet.T_SentenceAIDataTable dt2 = ClassLibrary.SentenceAIClass.GetT_SentenceAIDataTable(Global.GetConnection(), str.Trim().Substring(0, str.Length / 100));
                        if (dt2 != null)
                        {
                            Random rand = new Random();
                            sb2.Append(dt2[rand.Next(0, dt2.Count - 1)].sentence);
                        }

                    }

                    //出力用メモ
                    sb.Append("[重複(DateTime_Update)]");
                    sb.Append(str);
                    sb.Append("\r\f");
                }
            }
            //応答
            TextBox_IBResult.Text = sb2.ToString();
            TextBox_IBResult.Text += "\r\f--------------------------------\r\f";
            //ログ
            TextBox_IBResult.Text += sb.ToString();
        }



    }
}