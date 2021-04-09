<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuControl.ascx.cs" Inherits="WhereEver.MenuControl" %>

<link href="MenuControl.css" type="text/css" rel="stylesheet" />


<table class="menu">
    <tr>
        <td id="logo">
            <td id="title">
                <h1 class="TitleClass">WhereEver</h1>
                <td>
                    <a href="ログイン/LoginList.aspx" class="btnmenu" runat="server">ログインリスト</a>
                </td>
                <td>
                    <a href="スケジュール/Schedule.aspx" class="btnmenu" runat="server">スケジュール</a>
                </td>
               <td>
                    <a href="~/各申請書類/Shinsei.aspx" class="btnmenu" runat="server">各申請書類</a>
               </td>
               <td>
                    <a href="チャット/Chat.aspx" class="btnmenu" runat="server">チャット</a>
               </td>
               <td>
                    <a href="ファイル共有/FileShare.aspx" class="btnmenu" runat="server">ファイル共有</a>
               </td>
               <td>
                    <a href="Project System/PIchiran.aspx" class="btnmenu" runat="server">プロジェクト</a>
               </td>
               <td>
                    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
               </td>
    </tr>
</table>
