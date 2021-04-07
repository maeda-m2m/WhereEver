<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PIchiran.aspx.cs" Inherits="WhereEver.Project_System.PIchiran" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="Pichiran.css" rel="stylesheet" type="text/css" />

    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="auto-style1">
                <tr>
                    <td colspan="2">

            <asp:DataGrid runat="server" ID="DgPIchiran" AutoGenerateColumns="False" OnItemDataBound="DgPIchiran_ItemDataBound">
                <AlternatingItemStyle BackColor="#CCFFCC" />
                <Columns>
                    <asp:TemplateColumn HeaderText="プロジェクト名">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbPName" runat="server">プロジェクト名</asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle Wrap="True" />
                        <ItemStyle Height="50px" HorizontalAlign="Left" Width="100px" Wrap="True" />
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="顧客名">
                        <ItemTemplate>
                            <asp:Label ID="lblCustomer" runat="server">顧客名</asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Wrap="True" />
                        <ItemStyle CssClass="scdl" HorizontalAlign="Left" Width="150px" Wrap="True" />
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="担当者">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbResponsible" runat="server">担当者</asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle Wrap="True" />
                        <ItemStyle CssClass="scdl" HorizontalAlign="Left" Width="150px" Wrap="True" />
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="カテゴリー">
                        <ItemTemplate>
                            <asp:Label ID="lblCategory" runat="server">カテゴリー</asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Wrap="True" />
                        <ItemStyle CssClass="scdl" HorizontalAlign="Left" Width="150px" Wrap="True" />
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="開始日">
                        <ItemTemplate>
                            <asp:Label ID="lblStartTime" runat="server">開始日</asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Wrap="True" />
                        <ItemStyle CssClass="scdl" HorizontalAlign="Left" Width="150px" Wrap="True" />
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="終了日">
                        <ItemTemplate>
                            <asp:Label ID="lblOverTime" runat="server">終了日</asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Wrap="True" />
                        <ItemStyle CssClass="scdl" HorizontalAlign="Left" Width="150px" Wrap="True" />
                    </asp:TemplateColumn>
                </Columns>

                <HeaderStyle Width="200px" BackColor="#16BA00" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="12px"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="10px" />
            </asp:DataGrid>
                    </td>
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
