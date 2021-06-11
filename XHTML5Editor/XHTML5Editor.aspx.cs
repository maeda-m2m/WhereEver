using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using static System.Web.HttpUtility;

namespace WhereEver.XHTML5Editor
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
            TextBox_QRCode_Result.Text = HtmlEncode("/*[生成されたコード（エンコード後のコードです）]*/\r\f" + Label_QRCode_Area.Text);
        }

        protected void Push_QR_Encode(object sender, EventArgs e)
        {
            if (FileUpload_userfile.HasFile)
            {
                HttpPostedFile filename = FileUpload_userfile.PostedFile;
                TextBox_QRCode_Result.Text = HtmlEncode("/*[読み込まれた内容（エンコード後のコードです）]*/\r\f" + ClassLibrary.QRCodeClass.SetQRCode(filename));
            }
            else
            {
                TextBox_QRCode_Result.Text = "QRコードから読み込みたいファイルを選択して下さい。";
                return;
            }
        }
    }
}