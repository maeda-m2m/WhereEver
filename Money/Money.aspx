<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Money.aspx.cs" Inherits="WhereEver.Money.WebForm1" %>
<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../MenuControl.css" />
    <link rel="stylesheet" type="text/css" href="Money.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <div>
            <table>
                <tr>
                    <td id="menu">
                        <menu:c_menu id="m" runat="server"></menu:c_menu>
                    </td>
                </tr>
            </table>
       </div>

        <div id="Wrap">

                       <span class="hr"></span>

           <p class="index1">
               ◆損益計算書(PL)
               <asp:Button ID="Button_PL" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_PL_test" CausesValidation="False" />
           </p>

           <hr />

<asp:Panel ID="Panel_PL" runat="server" Visible="false">

    <div class="center">

        <table class="DGTable">
            <tr>
                <th colspan="2" id="th_pl_master">
                    P/L
                </th>
            </tr>
            <tr>
                <th colspan="2">
                    出
                    <asp:DropDownList ID="DropDownList_PL_year_s" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true"></asp:DropDownList>
                    <asp:DropDownList ID="DropDownList_PL_month_s" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true"></asp:DropDownList>
                    <asp:DropDownList ID="DropDownList_PL_day_s" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true"></asp:DropDownList>
                    <br />
                    至
                    <asp:DropDownList ID="DropDownList_PL_year_g" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true"></asp:DropDownList>
                    <asp:DropDownList ID="DropDownList_PL_month_g" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true"></asp:DropDownList>
                    <asp:DropDownList ID="DropDownList_PL_day_g" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true"></asp:DropDownList>
                </th>
            </tr>
            <tr>
                <td rowspan="2" id="td_pl_master">
                    売上高<br /><span class="minus">▲売上原価</span>
                </td>
                <td>
                    <asp:TextBox ID="TextBox_Uriage" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" AutoPostBack="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="TextBox_UriageGenka" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" AutoPostBack="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    [売上総利益]
                </td>
                <td class="right">
                    <asp:Label ID="Label_UriageSourieki" runat="server" Text="0" CssClass="textbox_pl"></asp:Label>
                </td>
             </tr>
             <tr>
                <td class="minus">
                    ▲販売管理費
                </td>
                <td>
                    <asp:TextBox ID="TextBox_HanbaiKanrihi" runat="server" CssClass="textbox_pl"  ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" AutoPostBack="true"></asp:TextBox>
                </td>
             </tr>
             <tr>
                <td>
                    営業利益
                </td>
                <td>
                    <asp:TextBox ID="TextBox_EigyouRieki" runat="server" CssClass="textbox_pl"  ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" AutoPostBack="true"></asp:TextBox>
                </td>
             </tr>
             <tr>
                <td class="minus">
                    ▲営業外費用
                </td>
                <td>
                    <asp:TextBox ID="TextBox_EigyougaiHiyou" runat="server" CssClass="textbox_pl"  ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" AutoPostBack="true"></asp:TextBox>
                </td>
             </tr>
             <tr>
                <td>
                    [経常利益]
                </td>
                <td class="right">
                    <asp:Label ID="Label_KeijyouRieki" runat="server" Text="0" CssClass="textbox_pl"></asp:Label>
                </td>
             </tr>
             <tr>
                <td rowspan="2">
                    特別利益<br /><span class="minus">▲特別損失</span>
                </td>
                <td>
                    <asp:TextBox ID="TextBox_TokubetsuRieki" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" AutoPostBack="true"></asp:TextBox>
                </td>
             </tr>
             <tr>
                <td>
                    <asp:TextBox ID="TextBox_TokubetsuSonshitsu" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" AutoPostBack="true"></asp:TextBox>
                </td>
             </tr>
             <tr>
                <td>
                    [税引前当期純利益]
                </td>
                <td class="right">
                    <asp:Label ID="Label_Zeibikimae" runat="server" Text="0" CssClass="textbox_pl"></asp:Label>
                </td>
             </tr>
             <tr>
                <td>
                    <span class="minus">▲法人税等</span>
                </td>
                <td>
                    <asp:TextBox ID="TextBox_Houjinzei" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" AutoPostBack="true"></asp:TextBox>
                </td>
             </tr>
             <tr>
                <td>
                    [当期純利益]
                </td>
                <td class="right">
                    <asp:Label ID="Label_Jyunrieki" runat="server" Text="0" CssClass="textbox_pl"></asp:Label>
                </td>
             </tr>
             <tr>
                <td>
                    [売上総利益率（粗利率）]
                </td>
                <td class="right">
                    <asp:Label ID="Label_ArariR" runat="server" Text="0" CssClass="textbox_pl"></asp:Label>
                </td>
             </tr>
             <tr>
                <td>
                    [売上高営業利益率]
                </td>
                <td class="right">
                    <asp:Label ID="Label_EigyouR" runat="server" Text="0" CssClass="textbox_pl"></asp:Label>
                </td>
             </tr>
             <tr>
                <td>
                    [売上高経常利益率]
                </td>
                <td class="right">
                    <asp:Label ID="Label_KeijyouR" runat="server" Text="0" CssClass="textbox_pl"></asp:Label>
                </td>
             </tr>
        </table>

        <p class="center">
            <asp:Button ID="Button_Check_PL" CssClass="btn-flat-border" runat="server" Text="確定" OnClick="Push_Check_PL" CausesValidation="False" />
        </p>

        <p>予定：確認/確定ボタンを押す→Validation→入力値をGridViewに通貨単位で代入→TryParseした値をSqlに保存/表の印刷をできるようにする。</p>

        <asp:GridView ID="GridView_PL" runat="server" CssClass="DGTable" Visible="false">
        <HeaderStyle BackColor="Black" ForeColor="White" />
        <RowStyle BackColor="#1E1E1E" ForeColor="White" />
        </asp:GridView>

    </div>

</asp:Panel>


        </div>
    </form>
</body>
</html>
