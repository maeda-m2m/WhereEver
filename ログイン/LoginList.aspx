<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginList.aspx.cs" Inherits="WhereEver.LoginList" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="LoginList.css" rel="stylesheet" type="text/css" />
    <title>ログインリスト</title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            width: 148px;
        }
        .auto-style3 {
            width: 148px;
            height: 31px;
        }
        .auto-style4 {
            margin-bottom: 0px;
        }
    </style>
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
        <div runat="server">
            <table class="auto-style1">
                <tr>
                    <td>
                        <br />
                    </td>
                    <td>
                        <asp:DataGrid ID="DgTimeDetail" runat="server" AllowSorting="True" AutoGenerateColumns="false" HorizontalAlign="Left" OnItemDataBound="DgTimeDetail_ItemDataBound">
                            <Columns>
                                <asp:TemplateColumn>
                                    <HeaderTemplate>
                                        M2M社内アクセス時間表
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" CssClass="none"></asp:Label>
                                        <table class="auto-style1">
                                            <tr>
                                                <td class="auto-style3">
                                                    <asp:Label ID="lblDetail" runat="server" Font-Bold="True" Font-Size="Large" Text="ログイン"></asp:Label>
                                                </td>
                                                <td class="auto-style3">
                                                    <asp:Label ID="lblDetail0" runat="server" Font-Bold="True" Font-Size="Large" Text="ログアウト"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style2">
                                                    <asp:Label ID="lblLoginTime" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblLogoutTime" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnOut" runat="server" CssClass="auto-style4" Text="ログアウト" OnClick="btnOut_Click" style="height: 21px" />
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
