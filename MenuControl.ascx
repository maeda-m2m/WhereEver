<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuControl.ascx.cs" Inherits="WhereEver.MenuControl" %>

<link href="MenuControl.css" type="text/css" rel="stylesheet" />
<link href="Scripts/lastModified.js" type="text/javascript" rel="javascript" />

<table>
    <tr>
        <td class="All">
            <a href="ログイン/LoginList.aspx" runat="server">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/ログイン/m2m-logo.png" /></a>
        </td>
        <td>
            <h1>WhereEver</h1>
        </td>
        <td>
            <a href="~/ログイン/LoginList.aspx" class="btn" runat="server">トップページ</a>
        </td>
        <td>
            <a href="~/スケジュール/Schedule.aspx" class="btn" runat="server">スケジュール</a>
        </td>
        <td>
            <a href="~/各申請書類/Shinsei.aspx" class="btn" runat="server">各申請書類</a>
        </td>
        <td>
            <a href="~/チャット/Chat.aspx" class="btn" runat="server">チャット<asp:Label ID="lblHensin" runat="server" Visible="False"></asp:Label></a>
        </td>
        <td>
            <a href="~/ファイル共有/FileShare.aspx" class="btn" runat="server">ファイル共有</a>
        </td>
        <td>
            <a href="~/Project System/PIchiran.aspx" class="btn" runat="server">プロジェクト</a>
        </td>
        <td>
            <a href="~/管理ページ/Kanri.aspx" class="btn" runat="server">マイページ</a>
        </td>
        <td>
            <a href="~/ログイン/Login.aspx" class="btn" runat="server">ログアウト</a>
        </td>
        <td>
            <asp:Label ID="Label1" CssClass="label" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="9">
            <asp:Menu id="sitemap_main" runat="server"
              DataSourceID="smd" ExpandDepth="2" ShowLines="True">
            </asp:Menu>
            <asp:SiteMapDataSource id="smd" runat="server" />
        </td>
        <td colspan="2">
            <p class="date"><script src="../Scripts/lastModified.js" type="text/javascript" charset="utf-8"></script></p>
        </td>
    </tr>
</table>

