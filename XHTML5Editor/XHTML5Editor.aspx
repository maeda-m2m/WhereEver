<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="XHTML5Editor.aspx.cs" Inherits="WhereEver.XHTML5Editor.WebForm1" %>
<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="application/json; charset=utf-8"/>
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../MenuControl.css" />
    <link rel="stylesheet" type="text/css" href="XHTML5Editor.css" />
    <title>XHTML5エディター</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="noprint">
            <table>
                <tr>
                    <td id="menu">
                        <menu:c_menu id="m" runat="server"></menu:c_menu>
                    </td>
                </tr>
            </table>
       </div>

        <div id="Wrap">


            <div class="noprint">
           <span class="hr"></span>

           <p class="index1">
               ◆フラットボタンエディター
               <asp:Button ID="Button_Button" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_Button" CausesValidation="False" />
               　手軽にフラットボタンをつくることができます。</p>

           <hr />

<asp:Panel ID="Panel_BT" runat="server" Visible="false" DefaultButton="Button_Correct">

                    <div class="noprint">
                       <span class="hr"></span>
                        <p class="center">汎用のフラットボタンを作ります。生成ボタンを押すとコードが生成されます。</p>
                       <span class="hr"></span>
                    </div>


    <div class="center">
          <table class="DGTable">
            <tr>
                <th class="th_master" colspan="2">
                    設定
                </th>
                <th class="th_master">
                結果ビュー
                </th>
            </tr>
            <tr>
                <td>
                ボタンテキスト
                </td>
                <td>
                    <asp:TextBox ID="TextBox_BTNText" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角30文字以内" Text="Button" MaxLength="30"></asp:TextBox>
                </td>
                <td rowspan="12" class="th_master"> <%-- rowspan --%>
                <asp:Label ID="Label_ButtonResult" runat="server" Text="ここにボタンがプレビューされます。" CssClass="lbl_pl" ValidateRequestMode="Disabled"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                フォントファミリー
                </td>
                <td>
                    <asp:TextBox ID="TextBox_FontFamily" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角30文字以内" Text="Meiryo" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                横幅
                </td>
                <td>
                    <asp:TextBox ID="TextBox_Width" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角8文字以内(Max 300px or 25em, Min n >= 0)" Text="200" MaxLength="8"></asp:TextBox>
                    <asp:DropDownList ID="DropDownList_Width" runat="server" CssClass="ddl_date">
                        <asp:ListItem Value="px"></asp:ListItem>
                        <asp:ListItem Value="em"></asp:ListItem>
                        <asp:ListItem Value="auto"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                高さ
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_Height" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角8文字以内(Max 300px or 25em, Min n >= 0)" Text="50" MaxLength="8"></asp:TextBox>
                    <asp:DropDownList ID="DropDownList_Height" runat="server" CssClass="ddl_date">
                        <asp:ListItem Value="px"></asp:ListItem>
                        <asp:ListItem Value="em"></asp:ListItem>
                        <asp:ListItem Value="auto"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                線の太さ
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_Border" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角8文字以内(Max 300px or 25em, Min n >= 0)" Text="2" MaxLength="8"></asp:TextBox>
                    <asp:DropDownList ID="DropDownList_Border" runat="server" CssClass="ddl_date">
                        <asp:ListItem Value="px"></asp:ListItem>
                        <asp:ListItem Value="em"></asp:ListItem>
                        <asp:ListItem Value="auto"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                線の丸み
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_Radius" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角8文字以内(Max 3000px or 250em, Min n >= 0)" Text="4" MaxLength="8"></asp:TextBox>
                    <asp:DropDownList ID="DropDownList_Radius" runat="server" CssClass="ddl_date">
                        <asp:ListItem Value="px"></asp:ListItem>
                        <asp:ListItem Value="em"></asp:ListItem>
                        <asp:ListItem Value="auto"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                線の色名/色コード
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_BorderColor" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角30文字以内" Text="white" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                文字色名/色コード
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_Color" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角30文字以内" Text="white" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                背景色名/色コード
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_BColor" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角30文字以内" Text="#1e1e1e" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                選択中文字色名/色コード
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_HColor" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角30文字以内" Text="white" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                選択中背景色名/色コード
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_HBColor" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角30文字以内" Text="#808080" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                <asp:CheckBox ID="CheckBox_Pointer" runat="server" Checked="true" Text="指マークに変化" />
                </td>
            </tr>
            <tr>
                <td colspan="3" class="th_master">
                    <asp:Button ID="Button_Correct" CssClass="btn-flat-border" runat="server" Text="生成" OnClick="Push_Correct" CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:TextBox ID="TextBox_ButtonCodeResult" runat="server" CssClass="textbox_Wide" ValidateRequestMode="Disabled" Text="ここにコードが自動生成されます。" CausesValidation="false" TextMode="MultiLine" Style="resize: none" ReadOnly="true" ></asp:TextBox>
                </td>
            </tr>
          </table>
    </div>

</asp:Panel>


</div>



        </div>
    </form>
</body>
</html>
