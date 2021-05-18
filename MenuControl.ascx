<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuControl.ascx.cs" Inherits="WhereEver.MenuControl" %>

<link href="MenuControl.css" type="text/css" rel="stylesheet" />

<style type="text/css">
    .auto-style1 {
        width: 150px;
        height: 40px;
        position: relative;
        display: inline-block;
        font-weight: bold;
        padding: 0.3em 0.5em;
        text-decoration: none;
        color: #000000;
        background: #ECECEC;
        transition: .4s;
        font-size: 1em;
        text-align: center;
        margin-top: 0px;
    }
</style>

<table>
    <tr>
        <td class="All">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/ログイン/m2m-logo.png"/>
            <td>
                <h1>WhereEver</h1>
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
                    <a href="~/チャット/Chat.aspx" class="btn" runat="server">チャット<asp:Label ID="lblHensin" runat="server" Visible="False"></asp:Label>
                    </a>
               &nbsp;</td>
               <td>
                    <a href="~/ファイル共有/FileShare.aspx" class="btn" runat="server">ファイル共有</a>
               </td>
               <td>
                    <a href="~/Project System/PIchiran.aspx" class="btn" runat="server">プロジェクト</a>
               </td>
               <td>
                    <a href="~/管理ページ/Kanri.aspx" class="btn" runat="server">ユーザー情報変更</a>
               </td>
               <td>
                    <asp:Label ID="Label1" CssClass="label" runat="server" Text="Label"></asp:Label>
               </td>
    </tr>
</table>
