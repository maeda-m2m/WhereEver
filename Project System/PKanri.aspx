<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PKanri.aspx.cs" Inherits="WhereEver.Project_System.PKanri" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="PKanri.css" type="text/css" rel="stylesheet" />
    <title></title>
    
</head>
<body>
    <form id="form1" runat="server">
         <div>
            <table>
                <tr class="All">
                    <td>
                        <Menu:c_menu ID="m" runat="server"></Menu:c_menu>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table class="table">
                <tr>
                    <td colspan="3">
                        
                        <asp:Label ID="lblPBig" CssClass="txt" runat="server" Text="大項目登録"></asp:Label>
                        <asp:TextBox ID="txtPBig" CssClass="txt" runat="server"></asp:TextBox>
                        <asp:Button ID="btnToroku" CssClass="btn" runat="server" Text="大項目登録" OnClick="btnToroku_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="lblPBigList" CssClass="txt" runat="server" Text="大項目リスト"></asp:Label>
                        <asp:DropDownList ID="ddlPBigList" CssClass="txt" runat="server">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblPMiddle" CssClass="txt" runat="server" Text="中項目"></asp:Label>
                        <asp:TextBox ID="txtPMiddle" CssClass="txt" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="lblAisatu1" CssClass="txt" runat="server" Text="を選択してから、中項目入力をお願い致します。"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblTime" CssClass="txt" runat="server" Text="日付選択"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        &nbsp;</td>
                    <td>
                        <asp:Label ID="lblStart" CssClass="txt" runat="server" Text="開始"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblOver" CssClass="txt" runat="server" Text="終了"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        &nbsp;</td>
                    <td>
                        <input id="date1" runat="server" type="date" min="2018-01-01"/></td>
                    <td>
                        <input id="date2" runat="server" type="date" min="2018-01-01"/></td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        &nbsp;</td>
                    <td colspan="2">
                        <asp:Button ID="btnPMiddle" CssClass="btn" runat="server" Text="中項目登録" OnClick="btnPMiddle_Click" />
                        <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="クリア" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:DataGrid ID="DgPKanri" runat="server" 
                            AutoGenerateColumns="False" 
                            OnItemDataBound="DgPKanri_ItemDataBound" 
                            OnEditCommand="DgPKanri_EditCommand"
                            OnCancelCommand="DgPKanri_CancelCommand"
                            OnUpdateCommand="DgPKanri_UpdateCommand"
                            OnItemCommand="DgPKanri_ItemCommand"
                            Width="100%">
                            <Columns>
                                <asp:BoundColumn DataField="PBigname" HeaderText="大項目" />
                                <asp:BoundColumn DataField="PMiddlename" HeaderText="中項目" />
                                <asp:BoundColumn DataField="PMiddlestart" HeaderText="開始" />
                                <asp:BoundColumn DataField="PMiddleover" HeaderText="終了" />
                                <asp:BoundColumn HeaderText="ステータス" />
                                <asp:BoundColumn DataField="PTorokutime" HeaderText="登録日付" />
                                <asp:BoundColumn DataField="PTorokusya" HeaderText="登録者" />
                                <asp:EditCommandColumn EditText="変更" CancelText="キャンセル" UpdateText="保存"  ></asp:EditCommandColumn>
                                <asp:ButtonColumn ButtonType="LinkButton" Text="削除" CommandName="Delete"/>
                            </Columns>
                            <HeaderStyle Height="50px" HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                            <ItemStyle Height="30px" HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
