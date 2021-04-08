<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PIchiran.aspx.cs" Inherits="WhereEver.Project_System.PIchiran" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="../MenuControl.css" type="text/css" rel="stylesheet" />

    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 25px;
        }
        .auto-style2 {
            height: 24px;
        }
        .auto-style3 {
            height: 23px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form">
            <table>
                <tr>
                    <td id="menu">
                        <Menu:c_menu ID="m" runat="server"></Menu:c_menu>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table class="auto-style1">
                <tr>
                    <td colspan="5">

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

                    <asp:TemplateColumn>
                        <ItemTemplate>
                            <asp:Button ID="Button1" runat="server" Text="編集" />
                            <asp:Button ID="btnDelete" runat="server" Text="削除" />
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
                    <td class="auto-style2">

                        <asp:Label ID="lblNewPName" runat="server" Text="プロジェクト名"></asp:Label>
                    </td>
                    <td class="auto-style2">

                        <asp:TextBox ID="txtNewPName" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style2">

                        <asp:Label ID="lblNewCustomer" runat="server" Text="顧客名"></asp:Label>
                    </td>
                    <td class="auto-style2">

                        <asp:TextBox ID="txtNewCustomer" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style2">

                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style3">

                        <asp:Label ID="lblNewResponsible" runat="server" Text="担当者"></asp:Label>
                    </td>
                    <td class="auto-style3">

                        <asp:DropDownList ID="ddlResponsible" runat="server">
                            <asp:ListItem>chou</asp:ListItem>
                            <asp:ListItem>yanagisawa</asp:ListItem>
                            <asp:ListItem>sakaguchi</asp:ListItem>
                            <asp:ListItem>koibuchi</asp:ListItem>
                            <asp:ListItem>maeda</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="auto-style3">

                        <asp:Label ID="lblNewCategory" runat="server" Text="カテゴリー(△)"></asp:Label>
                    </td>
                    <td class="auto-style3">

                        <asp:TextBox ID="txtNewCategory" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style3">

                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">

                        <asp:Label ID="lblNewStartTime" runat="server" Text="開始日"></asp:Label>
                    </td>
                    <td class="auto-style1">

                <asp:Calendar ID="Calendar1" runat="server" ></asp:Calendar>
                    </td>
                    <td class="auto-style1">

                        <asp:Label ID="lblNewOverTime" runat="server" Text="終了日"></asp:Label>
                    </td>
                    <td class="auto-style1">

                <asp:Calendar ID="Calendar2" runat="server"></asp:Calendar>
                    </td>
                    <td class="auto-style1">

                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="auto-style1">

                        <asp:Button ID="btnNewP" runat="server" OnClick="btnNewP_Click" Text="新規として保存" />
                    </td>
                    <td class="auto-style1">

                        <asp:Button ID="btnBack" runat="server" Text="戻る" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
