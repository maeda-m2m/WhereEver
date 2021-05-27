<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginList.aspx.cs" Inherits="WhereEver.LoginList" %>

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
               <div id="Innner">
                <span class="hr"></span>

                <p class="index1">◆Welcome to m2m</p>
                <div class="article">
                    <p>--本日の予定--</p>
                    <asp:Label ID="Label_WhatNow" runat="server" Text="" ValidateRequestMode="Disabled"></asp:Label><br />
                </div>
               </div>


                <div id="Edit_btn_space">
                  <asp:Button ID="Button_EditTop" runat="server" CssClass="btn_loginlist" Text="お知らせ編集" OnClick="btnEditTop_Click" PostBackUrl="#edittop" />
                </div>

                  <asp:Panel ID="Panel_EditTop" runat="server" Visible="false">

                    <div id="Edit">
                      <span class="hr"></span>
                       <a name="edittop"><p class="index1"> ◆お知らせ編集（&lt;br /&gt;&lt;p&gt;&lt;/p&gt;&lt;ol&gt;&lt;/ol&gt;&lt;ul&gt;&lt;/ul&gt;&lt;li&gt;&lt;/li&gt;タグを使用できます）</p></a>
                        <div class="article">
                           <p>文字数制限なし　最新情報を日付とともにいちばん上に書くことを推奨</p>
                    <asp:TextBox ID="TextBox_EditTop" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" TextMode="MultiLine" Rows="3" Height="100px" Width="1160px" Style="resize: none" Text="" placeholder="トップページに書く内容　文字数無制限" CausesValidation="false"></asp:TextBox>
                        </div>

                    <div class="center">
                        <asp:Button ID="Button_ReformTop" runat="server" CssClass="btn_loginlist" Text="更新" OnClick="btnReformTop_Click" />
                        <asp:Button ID="Button_ReformTopEnd" runat="server" CssClass="btn_loginlist" Text="閉じる" OnClick="btnReformTopEnd_Click" />
                        <asp:Button ID="Button_ReformDelete" runat="server" CssClass="btn_loginlist" Text="全消去" OnClick="btnReformTopDel_Click" PostBackUrl="#edittop" />
                        <asp:Button ID="Button_ReformReload" runat="server" CssClass="btn_loginlist" Text="全取り消し" OnClick="btnReformTopReload_Click" PostBackUrl="#edittop" />
                    </div>

                   </div><%-- Edit --%>
                  </asp:Panel>


           </div><%-- Main --%>






            <div id="Sub">

        <div class ="bg_test">
            <table class="bg_test-text">
                    <tr>
                        <td><asp:Button ID="btnOut" runat="server" CssClass="btn_loginlist" Text="ログアウト" OnClick="btnOut_Click" /></td>
                        <td><asp:Button ID="btnKanri" runat="server" CssClass="btn_loginlist" Text="マイページ" OnClick="btnKanri_Click" /></td>
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
