﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginList.aspx.cs" Inherits="WhereEver.LoginList" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../MenuControl.css" type="text/css" rel="stylesheet" />
    <link href="LoginList.css" type="text/css" rel="stylesheet" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <title>トップページ</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id ="Wrap">


            <table>
                <tr>
                    <td id="menu">
                        <Menu:c_menu ID="m" runat="server"></Menu:c_menu>
                    </td>
                </tr>
            </table>

            <div id="Navigation">
                <div class="logo"><img src="../IMG/m2mlogo_r.png" alt="m2m_logo" /></div>
            </div>


            <div id="Main">

                <span class="hr"></span>

                <p class="index1">◆Welcome to m2m</p>
                <div class="article">
                    <p>--本日の予定--</p>
                    <asp:Label ID="Label_WhatNow" runat="server" Text="" ValidateRequestMode="Disabled"></asp:Label>
                </div>

           </div><%-- Main --%>


            <div id="Sub">

        <div class ="bg_test">
            <table class="bg_test-text">
                    <tr>
                        <td><asp:Button ID="btnOut" runat="server" CssClass="btn_loginlist" Text="ログアウト" OnClick="btnOut_Click" /></td>
                        <td><asp:Button ID="btnKanri" runat="server" CssClass="btn_loginlist" Text="ユーザー情報変更" OnClick="btnKanri_Click" /></td>
                    </tr>

                    <tr>
                        <td colspan="2">
                            <asp:DataGrid ID="DgTimeDetail" runat="server" AllowSorting="True" AutoGenerateColumns="false" HorizontalAlign="Left" OnItemDataBound="DgTimeDetail_ItemDataBound" CssClass="DgTD">
                            <Columns>
                                <asp:TemplateColumn>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblTitle" CssClass="All" runat="server" Text="M2M社内アクセス時間表 "></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server"></asp:Label>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl1" runat="server" Text="ログイン"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl2" runat="server" Text="ログアウト"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
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
                </table>           
              </div>

            </div><%-- Sub --%>

        </div><%-- Warp --%>
    </form>
</body>
</html>
