<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PIchiran.aspx.cs" Inherits="WhereEver.Project_System.PIchiran" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    

    
    <style type="text/css">
        .auto-style1 {
            width: 34px;
        }
    </style>
    

    
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="auto-style1">
                <tr>
                    <td>
                        <asp:DataGrid ID="DgPIchiran" runat="server" AllowSorting="True" AutoGenerateColumns="false" HorizontalAlign="Left" OnItemDataBound="DgTimeDetail_ItemDataBound">
                            <Columns>
                                <asp:TemplateColumn>
                                    <HeaderTemplate>
                                        プロジェクト一覧
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table class="auto-style1">
                                            <tr>
                                                <td class="auto-style1">
                                                    <asp:LinkButton ID="lbPName" runat="server" >プロジェクト名</asp:LinkButton>
                                                </td>
                                                <td class="auto-style1">
                                                    <asp:Label ID="lblCustomer" runat="server">顧客名</asp:Label>
                                                </td>
                                                <td class="auto-style1">
                                                    <asp:LinkButton ID="lblResponsible" runat="server">担当者</asp:LinkButton>
                                                </td>
                                                <td class="auto-style1">
                                                    <asp:Label ID="lblCategory" runat="server">カテゴリー</asp:Label>
                                                </td>
                                                <td class="auto-style1">
                                                    <asp:Label ID="lblStartTime" runat="server">開始日</asp:Label>
                                                </td>
                                                <td class="auto-style1">
                                                    <asp:Label ID="lblOverTime" runat="server">終了日</asp:Label>
                                                </td>
                                                <td class="auto-style1">
                                                    <asp:Button ID="btnChange" runat="server" Text="編集" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="Button" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
