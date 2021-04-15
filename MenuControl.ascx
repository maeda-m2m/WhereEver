<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuControl.ascx.cs" Inherits="WhereEver.MenuControl" %>

<link href="MenuControl.css" type="text/css" rel="stylesheet" />

<table>
    <tr>
        <td class="All">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/ログイン/m2m-logo.png"/>
            <td>
                <h1>WhereEver</h1>
                <td>
                    <a href="~/ログイン/LoginList.aspx" class="btn" runat="server">ログインリスト</a>
                </td>
                <td>
                    <a href="~/スケジュール/Schedule.aspx" class="btn" runat="server">スケジュール</a>
                </td>
               <td>
                    <a href="~/各申請書類/Shinsei.aspx" class="btn" runat="server">各申請書類</a>
               </td>
               <td>
                    <a href="~/チャット/Chat.aspx" class="btn" runat="server">チャット</a>
               </td>
               <td>
                    <a href="~/ファイル共有/FileShare.aspx" class="btn" runat="server">ファイル共有</a>
               </td>
               <td>
                    <a href="~/Project System/PIchiran.aspx" class="btn" runat="server">プロジェクト</a>
               </td>
               <td>
                    <a href="~/管理ページ/Kanri.aspx" class="btn" runat="server">ユーザー管理</a>
               </td>
               <td>
                    <asp:Label ID="Label1" CssClass="label" runat="server" Text="Label"></asp:Label>
               </td>
    </tr>
</table>
