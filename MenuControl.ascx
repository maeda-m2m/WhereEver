<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuControl.ascx.cs" Inherits="WhereEver.MenuControl" %>

<link href="MenuControl.css" type="text/css" rel="stylesheet" />


<table class="menu">
    <tr>
        <td id="logo">
            <td id="title">
                <h1 class="TitleClass">WhereEver</h1>
                <td id="TopBtn">
                    <a href="TOP.aspx" class="btn-flat-border" runat="server">ログインリスト</a>
                    <td class="Btnpadd">
                    <a href="スケジュール/Schedule.aspx" class="btn-flat-border" runat="server">スケジュール</a>
                        <td class="Btnpadd">
                    <a href="~/各申請書類/Shinsei.aspx" class="btn-flat-border" runat="server">各申請書類</a>
                            <td class="Btnpadd">
                    <a href="チャット/Chat.aspx" class="btn-flat-border" runat="server">チャット</a>
                                <td>
                    <a href="ファイル共有/FileShare.aspx" class="btn-flat-border" runat="server">ファイル共有</a>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                                        </td>
                            </td>
                       </td>
                    </td>
               </td>
             </td>
        </td>
    </tr>
</table>
