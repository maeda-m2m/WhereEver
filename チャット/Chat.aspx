﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chat.aspx.cs" Inherits="WhereEver.Chat" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Chat.css" type="text/css" rel="stylesheet" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Mちゃんねる</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td id="menu">
                        <Menu:c_menu ID="m" runat="server"></Menu:c_menu>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <h1>Mちゃんねる</h1>
        </div>
        <div>
            <asp:DataGrid runat="server" ID="ChatArea" AutoGenerateColumns="False" OnItemDataBound="Chat_ItemDataBound" BorderStyle="None" >
                <AlternatingItemStyle BackColor="White" BorderColor="Black" BorderStyle="None" />
                <Columns>
                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="50px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="50px" CssClass="scdl" />
                        <ItemTemplate>
                            <asp:Label ID="No" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="50px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="50px" CssClass="scdl" />
                        <ItemTemplate>
                            <asp:Label ID="ID" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="200px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="500px" CssClass="scdl" />
                        <ItemTemplate>
                            <asp:Label ID="Naiyou" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="50px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="50px" CssClass="scdl" />
                        <ItemTemplate>
                            <asp:Label ID="Date" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                </Columns>
                <EditItemStyle BorderStyle="None" />
                <FooterStyle BorderStyle="None" />
                <HeaderStyle BorderStyle="None" HorizontalAlign="Left" />
                <ItemStyle BorderStyle="None" HorizontalAlign="Left" />
                <PagerStyle BorderStyle="None" />
                <SelectedItemStyle BorderStyle="None" />
            </asp:DataGrid>
        </div>
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                        </td>
                    </td>
                </tr>
                <td colspan="2">
                    <asp:Button ID="Send" runat="server" Text="⤴" CssClass="btn-flat-border" OnClick="Send_Click" />
                </td>
            </table>
        </div>
    </form>
</body>
</html>
