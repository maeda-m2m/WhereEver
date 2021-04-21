<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PKanri.aspx.cs" Inherits="WhereEver.Project_System.PKanri" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblPBig" runat="server" Text="大項目登録"></asp:Label>
                        <asp:TextBox ID="txtPBig" runat="server"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        <asp:Button ID="btnToroku" runat="server" Text="大項目登録" OnClick="btnToroku_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPBigList" runat="server" Text="大項目リスト"></asp:Label>
                        <asp:DropDownList ID="ddlPBigList" runat="server">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblPMiddle" runat="server" Text="中項目"></asp:Label>
                        <asp:TextBox ID="txtPMiddle" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td colspan="2">
                        <asp:Label ID="lblTime" runat="server" Text="日付選択"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Label ID="lblStart" runat="server" Text="開始"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblOver" runat="server" Text="終了"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
                    </td>
                    <td>
                        <asp:Calendar ID="Calendar2" runat="server"></asp:Calendar>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td colspan="2">
                        <asp:Button ID="btnPMiddle" runat="server" Text="中項目登録" />
                        <asp:Button ID="btnClear" runat="server" Text="クリア" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:DataGrid ID="DgPKanri" runat="server" 
                            AutoGenerateColumns="False" 
                            OnItemDataBound="DgPKanri_ItemDataBound">
                            <Columns>
                                <asp:BoundColumn DataField="PBigname" HeaderText="大項目" />
                                <asp:BoundColumn DataField="PMiddlename" HeaderText="中項目" />
                                <asp:BoundColumn DataField="PMiddlestart" HeaderText="開始" />
                                <asp:BoundColumn DataField="PMiddleover" HeaderText="終了" />
                                <asp:BoundColumn DataField="PTorokutime" HeaderText="登録日付" />
                                <asp:BoundColumn DataField="PTorokusya" HeaderText="登録者" />
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
