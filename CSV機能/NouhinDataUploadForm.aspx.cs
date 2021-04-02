using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MizunoDAL;
using System.Threading;

namespace Mizuno.OrderS
{
    public partial class NouhinDataUploadForm : Common.MizunoPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LblMsg.Text = "";
            this.TbxError.Text = "";

            if (DdlUploadShurui.SelectedValue == "0")
            {
                TblZUpload.Visible = false;
                TblUpload.Visible = true;
            }

            else if (DdlUploadShurui.SelectedValue == "1")
            {
                TblZUpload.Visible = true;
                TblUpload.Visible = false;
            }


            //this.TblZUpload.Visible = false;
            //this.BtnUpCancel0.Visible = false;
            //this.BtnZUpCancel0.Visible = false;
        }

        protected void BtnUpload_Click(object sender, EventArgs e)
        {

            if (null == this.File.PostedFile || 0 == this.File.PostedFile.ContentLength)
            {
                this.ShowMsg("ファイルを指定してください。", true);
                return;
            }

            List<string> lstError = new List<string>();
            List<NouhinDataClass.DenpyouKey> lst = new List<NouhinDataClass.DenpyouKey>();
            List<NouhinDataClass.DenpyouKey> lstShukkaAri = new List<NouhinDataClass.DenpyouKey>();

            Core.Error ret = NouhinDataClass.UploadNouhnData(SessionManager.User.M_ShiiresakiRow.ShiiresakiCode,
                                                            this.File.PostedFile.InputStream,
                                                            System.Text.Encoding.GetEncoding(932),
                                                            Global.GetConnection(),
                                                            out lstError,
                                                            out lst,
                                                            out lstShukkaAri,
                                                            System.Configuration.ConfigurationManager.AppSettings["ChokusouJigyoushoCode"],
                                                            System.Configuration.ConfigurationManager.AppSettings["ChokusouHokanBasho"]
                                                            );

            
            this.TbxError.Text = "";
            if (0 < lstError.Count)
            {
                this.TbxError.Text = string.Join("\r\n", lstError.ToArray());
                if (null == ret) ret = new Core.Error("登録に失敗しました。");
            }

            if (null != ret)
            {
                ShowMsg(ret.Message, true);
                return;
            }

            if (null == lstShukkaAri) lstShukkaAri = new List<NouhinDataClass.DenpyouKey>();

            // クライアントに返すのは出荷数=0＆完了の伝票ではなく、出荷ありの伝票のデータのみとする。
            //this.HidReturnData.Value = SessionManager.User.Encode(new string[] { Core.IO.ObjectSerializer.ToXml(lstShukkaAri) });

            if (null != lstShukkaAri && 0 < lstShukkaAri.Count)
                R.ResponseScripts.Add(string.Format("DenpyouHyouji('{0}');", ShukkaDenpyou.GetQuery(lstShukkaAri, false, false)));
            else
            {
                // 数量=0&完了フラグ=9の納品データのみが登録されたので、伝票表示でなく、メッセージのみ表示
                R.Alert("納品データの作成が完了しました。");
            }

        }

        // メッセージ表示
        private void ShowMsg(string strMsg, bool bError)
        {
            LblMsg.Text = strMsg;
            LblMsg.ForeColor = (bError) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.TbxError.Visible = ("" != this.TbxError.Text);
        }

        protected void DdlUploadShurui_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DdlUploadShurui.SelectedValue == "0")
            {
                TblZUpload.Visible = false;
                TblUpload.Visible = true;
            }

            else if (DdlUploadShurui.SelectedValue == "1")
            {
                TblZUpload.Visible = true;
                TblUpload.Visible = false;
            }
        }

        //材料データアップロードロジック
        protected void BtnZUpload_Click(object sender, EventArgs e)
        {
            if (null == this.ZFile.PostedFile || 0 == this.ZFile.PostedFile.ContentLength)
            {
                this.ShowMsg("ファイルを指定してください。", true);
                return;
            }

            List<string> lstError = new List<string>();
            List<NouhinDataClass.DenpyouKey> lst = new List<NouhinDataClass.DenpyouKey>();
            List<NouhinDataClass.DenpyouKey> lstShukkaAri = new List<NouhinDataClass.DenpyouKey>();

            Core.Error ret = NouhinDataClass.ZUploadNouhnData2(
                SessionManager.User.M_ShiiresakiRow.ShiiresakiCode,
                this.ZFile.PostedFile.InputStream, System.Text.Encoding.GetEncoding(932), Global.GetConnection(), out lstError, out lst, out lstShukkaAri);

            this.TbxError.Text = "";
            if (0 < lstError.Count)
            {
                this.TbxError.Text = string.Join("\r\n", lstError.ToArray());
                if (null == ret) ret = new Core.Error("登録に失敗しました。");
            }

            if (null != ret)
            {
                ShowMsg(ret.Message, true);
                return;
            }

            if (null == lstShukkaAri) lstShukkaAri = new List<NouhinDataClass.DenpyouKey>();

            // クライアントに返すのは出荷数=0＆完了の伝票ではなく、出荷ありの伝票のデータのみとする。
            //this.HidReturnData.Value = SessionManager.User.Encode(new string[] { Core.IO.ObjectSerializer.ToXml(lstShukkaAri) });

            if (null != lstShukkaAri && 0 < lstShukkaAri.Count)
                R.ResponseScripts.Add(string.Format("DenpyouHyouji('{0}');", ShukkaDenpyou.GetQuery(lstShukkaAri, false, false)));
            else
            {
                // 数量=0&完了フラグ=9の納品データのみが登録されたので、伝票表示でなく、メッセージのみ表示
                R.Alert("納品データの作成が完了しました。");
            }
        }
    }
}
