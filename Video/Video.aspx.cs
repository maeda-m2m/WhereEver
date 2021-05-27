using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using static System.Web.HttpUtility;

namespace WhereEver.Video
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void SetYT()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("<script>");
            //sb.Append("// Load the IFrame Player API code asynchronously.");
            sb.Append("var tag = document.createElement('script');");
            sb.Append("tag.src = 'https://www.youtube.com/player_api';");
            sb.Append("var firstScriptTag = document.getElementsByTagName('script')[0];");
            sb.Append("firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);");
            //sb.Append("// Replace the 'ytplayer' element with an <iframe> and");
            //sb.Append("// YouTube player after the API code downloads.");
            sb.Append("var player;");
            sb.Append("function onYouTubePlayerAPIReady()");
            sb.Append("{");
            sb.Append("player = new YT.Player('ytplayer', {");
            sb.Append("height: '360',");
            sb.Append("width: '640',");
            sb.Append("videoId: '");
            sb.Append(HtmlEncode(TextBox_yt.Text));
            sb.Append("'");
            sb.Append("});");
            sb.Append("}");
            sb.Append("</script>");
            Literal_yt.Text = sb.ToString();

        }

        protected void Push_yt(object sender, EventArgs e)
        {
            SetYT();
        }

        protected void Push_YT_test(object sender, EventArgs e)
        {
            if (Panel_YT.Visible)
            {
                Panel_YT.Visible = false;
            }
            else
            {
                Panel_YT.Visible = true;
            }
        }

        protected void Push_Video_test(object sender, EventArgs e)
        {
            if (Panel_Video.Visible)
            {
                Panel_Video.Visible = false;
            }
            else
            {
                Panel_Video.Visible = true;
            }

        }


    }
}