﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Kanri.aspx.cs" Inherits="WhereEver.管理ページ.Kanri" %>
<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <link rel="stylesheet" type="text/css" href="../MenuControl.css" />
    <link rel="stylesheet" type="text/css" href="Kanri.css" />

    <title>管理ページ</title>

</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div>
            <table>
                <tr>
                    <td id="menu">
                        <menu:c_menu id="m" runat="server"></menu:c_menu>

                    </td>
                </tr>
            </table>

        <div id="Wrap">
           <p>ユーザー情報を変更できます。</p>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"  DataSourceID="SqlDataSource1" CssClass="DGTable" OnRowCommand="grid_RowCommand">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="id" SortExpression="id" ReadOnly="true" />
                    <asp:BoundField DataField="name" HeaderText="Name" SortExpression="name" Visible="False" ReadOnly="true" />
                    <asp:BoundField DataField="name1" HeaderText="お名前" SortExpression="name1" />
                    <asp:BoundField DataField="pw" HeaderText="パスワード" SortExpression="pw" />
                    <asp:CommandField ShowEditButton="True" ButtonType="Button" ControlStyle-CssClass="btn-flat-border" HeaderText="編集">
                    <ControlStyle CssClass="btn-flat-border"></ControlStyle>
                    </asp:CommandField>
                </Columns>
            </asp:GridView>



            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                ConnectionString="<%$ ConnectionStrings:WhereverConnectionString %>"
                SelectCommand="SELECT [id], [pw], [name], [name1] FROM [M_User] WHERE ([id] = @id)"
                UpdateCommand="UPDATE [M_User] SET [pw] = @pw, [name1] = @name1 WHERE ([id] = @id)">
                <UpdateParameters>
                       <asp:ControlParameter Name="id" ControlId="lblResult" PropertyName="Text"/>
                </UpdateParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="lblResult" DefaultValue="null" Name="id" PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>



           <p>
            <asp:Label ID="lblResult" runat="server" Text="null" Visible="False"></asp:Label>
           </p>

        </div><%-- Wrap --%>


        </div>
    </form>
</body>
</html>
