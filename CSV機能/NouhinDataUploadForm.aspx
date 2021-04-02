<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NouhinDataUploadForm.aspx.cs" Inherits="Mizuno.OrderS.NouhinDataUploadForm" %>
<%@ Register Src="../CtlMenu.ascx" TagName="CtlMenu" TagPrefix="uc5" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="../MainStyle.css" rel="stylesheet" type="text/css" />
    
    <telerik:RadScriptBlock ID="RSM" runat="server">
    <script type="text/javascript">
        var _objDenpyou = null;

        function ShowDenpyou(arg) {
            if (null != _objDenpyou) _objDenpyou.close();
            _objDenpyou = window.open("ShukkaDenpyou.aspx?" + arg, "", "width=800px,height=600px,location=no,resizable=yes,scrollbars=yes,left=0,top=0");
        }
        
        function DenpyouHyouji(arg) {
            if ("" == arg) {
                if (null != _objDenpyou) _objDenpyou.close();
                return;
            }
            _objDenpyou = window.open("ShukkaDenpyou.aspx?" + arg, "", "width=800px,height=600px,location=no,resizable=yes,scrollbars=yes,left=0,top=0");
            if (null == _objDenpyou) {
                alert("表示に失敗しました。");
            }
            else {
                _objDenpyou.location.href = "../OrderS/ShukkaDenpyou.aspx?" + arg;
                _objDenpyou.resizeTo(800, 600);
                _objDenpyou.moveTo(0, 0);
                _objDenpyou.focus();
            }
        }
        function DenpyouHyouji0(arg) {
            if (null != _objDenpyou) _objDenpyou.close();
            _objDenpyou = open("../OrderS/ShukkaDenpyou.aspx?" + arg, "", "width=800px,height=600px,location=no,resizable=yes,scrollbars=yes");
        }

		function GetRadWindow()
		{
			var oWindow = null;
			if (window.radWindow) oWindow = window.radWindow;
			else if (window.frameElement && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
			return oWindow;
		}
		
		function Onload()
		{
		    var oWindow = GetRadWindow();
		    var hid = document.getElementById('<%=HidReturnData.ClientID%>');

		    if ('' != hid.value) {
		        oWindow.callback(hid.value);
		        oWindow.Close();
			}
		}
		
		function Close()
		{
			var oWindow = GetRadWindow();
					
			if (null != oWindow) {
				oWindow.Close();
			}
			else {
				window.close();
			}

        }
		</script>	  
		</telerik:RadScriptBlock>
</head>
<body onload="Onload();">
    <form id="form1" runat="server">
    
    <uc5:CtlMenu ID="M" runat="server" />
    
    
    <table><tr><td>
    <asp:Label ID="Label1" runat="server" Text="アップロードする発注品の種類を選択してください"></asp:Label>
    </td></tr>
    <tr>
    <td>
        <asp:DropDownList ID="DdlUploadShurui" runat="server" onselectedindexchanged="DdlUploadShurui_SelectedIndexChanged" AutoPostBack=true>
        
        <asp:ListItem Value="0" Selected="True">予算品・別注</asp:ListItem>
        <asp:ListItem Value="1">材料</asp:ListItem>
       
        </asp:DropDownList>
    </td>
    </tr>
    <tr>
    <td>
    
    <table align="center" cellpadding="2" cellspacing="0">
            <tr>
                <td>
 
        <table id="TblUpload" runat="server" border="1" bordercolor="#000000" 
        class="def9 np bg2" align="center" cellpadding="4" cellspacing="0" 
        style="border-collapse: collapse">
            <tr>
            <td width=120><center>予算品　別注品</center></td>
                <td class="bg1 tc">
                    テキストまたはCSVファイルを選択して下さい</td>
            </tr>
            <tr>
                <td class="nw" align="center" colspan=2>
                    <table cellpadding="2" cellspacing="0">
                        <tr>
                            <td align="center">
                    <input id="File" runat="server" type="file" size="50" /></td>
                        </tr>
                        <tr>
                            <td>
                                <table align="center" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Button ID="BtnUpload" runat="server" onclick="BtnUpload_Click" Text="登録" 
                                                Width="80" OnClientClick="if(window.document.readyState != null && window.document.readyState != 'complete'){return false;}else{return true;}"   />
                                        </td>
                                        
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        </table></td>
            </tr>
        </table>
       
       <table id="TblZUpload" runat="server" border="1" bordercolor="#000000" 
        class="def9 np bg2" align="center" cellpadding="4" cellspacing="0" 
        style="border-collapse: collapse">
            <tr>
            
            <td width=120><center>材料</center></td>
                <td class="bg1 tc">
                    テキストまたはCSVファイルを選択して下さい</td>
                    
            </tr>
            <tr>
                <td class="nw" align="center" colspan=2>
                    <table cellpadding="2" cellspacing="0">
                        <tr>
                            <td align="center">
                    <input id="ZFile" runat="server" type="file" size="50" /></td>
                        </tr>
                        <tr>
                            <td>
                                <table align="center" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Button ID="BtnZUpload" runat="server" Text="登録" 
                                                Width="80" onclick="BtnZUpload_Click" OnClientClick="if(window.document.readyState != null && window.document.readyState != 'complete'){return false;}else{return true;}" />
                                        </td>
                                        
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        </table></td>
            </tr>
        </table>
       
       
       
                </td>
            </tr>
            <tr>
                <td align="center">
                                <asp:Label ID="LblMsg" runat="server" CssClass="def9"></asp:Label>
                            </td>
            </tr>
            <tr>
                <td align="center">
                                <asp:TextBox ID="TbxError" runat="server" ForeColor="Red" Rows="8" 
                                    TextMode="MultiLine" Width="480px" CssClass="def9"></asp:TextBox>
                            </td>
            </tr>
        </table>
       
    
    
    </td>
    </tr>
    </table>
    
    <asp:HiddenField ID="HidReturnData" runat="server" />
    <telerik:RadAjaxLoadingPanel ID="LP" runat="server" Skin="Web20">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="R" runat="server" RequestQueueSize="10" UpdatePanelsRenderMode="Inline">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="BtnKensaku">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LblMsg" />
                    <telerik:AjaxUpdatedControl ControlID="L" LoadingPanelID="LP" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="BtnUploadEnd">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="DdlNengetu" />
                    <telerik:AjaxUpdatedControl ControlID="LblMsg" />
                    <telerik:AjaxUpdatedControl ControlID="L" LoadingPanelID="LP" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="BtnDownload">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="BtnDownload" LoadingPanelID="LP" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="D">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LblMsg" />
                    <telerik:AjaxUpdatedControl ControlID="D" LoadingPanelID="LP" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="BtnSaihakkouJikkou">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="BtnSaihakkouJikkou" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    </form>
</body>
</html>
