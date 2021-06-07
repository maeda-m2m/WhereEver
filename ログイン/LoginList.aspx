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


          <asp:Panel ID="Panel_Navigation" runat="server" Visible="true">

            <div id="Navigation">
                <div class="logo"><img src="../IMG/m2mlogo_r.png" alt="m2m_logo" /></div>
            </div>

            <div id="NewsFeed">
            <p class="index1">-Weather News-</p>
                <div class="flowtext">
                    <asp:Label ID="Label_Weather" runat="server" ValidateRequestMode="Disabled"></asp:Label>
                </div>
                <span class="hr"></span>
            </div>

           </asp:Panel>



            <div id="Main">


              <asp:Panel ID="Panel_Main" runat="server" Visible="true">

               <div id="Innner">
                <span class="hr"></span>

                <p class="index1">◆Welcome to m2m</p>
                <div class="article">
                    <p class="index1">--本日の予定--</p>
                    <asp:Label ID="Label_WhatNow" runat="server" Text="" ValidateRequestMode="Disabled"></asp:Label><br />
                    <p class="index1">--天気予報--</p>
                    <asp:Label ID="Label_Weather2" runat="server" Text="" ValidateRequestMode="Disabled"></asp:Label><br />
                    <p class="index1">--お知らせ--</p>
                    <asp:Label ID="Label_Yotei" runat="server" Text="" ValidateRequestMode="Disabled"></asp:Label><br />
                </div>
               </div>
              </asp:Panel>


                <div id="Edit_btn_space">
                  <asp:Button ID="Button_EditTop" runat="server" CssClass="btn_loginlist" Text="お知らせ編集" OnClick="btnEditTop_Click" PostBackUrl="#" />
                  <asp:Button ID="Button_EditWeather" runat="server" CssClass="btn_loginlist" Text="天気予報編集" OnClick="btnEditWeather_Click" PostBackUrl="#" />
                </div>

                  <asp:Panel ID="Panel_EditTop" runat="server" Visible="false">

                    <div class="Edit">
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


                   <asp:Panel ID="Panel_EditWeather" runat="server" Visible="false" DefaultButton="Button_ReformWeather">
                    <div class="Edit">
                      <span class="hr"></span>
                       <a name="editweather"><p class="index1"> ◆天気予報編集</p></a>
                        <div class="article">
                           <p>気象庁発表の天気予報を入力して下さい（有料APIを導入する場合は不要です）。</p>
                           <p class="linkbox"><a href="https://tenki.jp/forecast/3/16/4410/13104/" target="_blank" >気象庁公式ホームページ|新宿区10日間天気予報</a></p>

                          <div id="weather"></div><%-- Scroll Target --%>

                            <p>
                                <asp:DropDownList ID="DropDownList_CF_year" runat="server" AutoPostBack="true" CssClass="textbox" OnSelectedIndexChanged="Postback_Weather" ></asp:DropDownList>年
                                <asp:DropDownList ID="DropDownList_CF_month" runat="server" AutoPostBack="true" CssClass="textbox" OnSelectedIndexChanged="Postback_Weather" ></asp:DropDownList>月
                                <asp:DropDownList ID="DropDownList_CF_day" runat="server" AutoPostBack="true" CssClass="textbox" OnSelectedIndexChanged="Postback_Weather" ></asp:DropDownList>日の天気 ：</p>


                            <asp:DropDownList ID="DropDownList_Weather1" runat="server" CssClass="textbox" >
                                <asp:ListItem Value="">--</asp:ListItem>
                                <asp:ListItem Value="晴"></asp:ListItem>
                                <asp:ListItem Value="曇"></asp:ListItem>
                                <asp:ListItem Value="雨"></asp:ListItem>
                                <asp:ListItem Value="雷"></asp:ListItem>
                                <asp:ListItem Value="雷雨"></asp:ListItem>
                                <asp:ListItem Value="雪"></asp:ListItem>
                            </asp:DropDownList>

                            <asp:DropDownList ID="DropDownList_WeatherN" runat="server" CssClass="textbox" >
                                <asp:ListItem Value="">--</asp:ListItem>
                                <asp:ListItem Value="のち"></asp:ListItem>
                                <asp:ListItem Value="時々"></asp:ListItem>
                            </asp:DropDownList>

                            <asp:DropDownList ID="DropDownList_Weather2" runat="server" CssClass="textbox" >
                                <asp:ListItem Value="">--</asp:ListItem>
                                <asp:ListItem Value="晴"></asp:ListItem>
                                <asp:ListItem Value="曇"></asp:ListItem>
                                <asp:ListItem Value="雨"></asp:ListItem>
                                <asp:ListItem Value="雷"></asp:ListItem>
                                <asp:ListItem Value="雷雨"></asp:ListItem>
                                <asp:ListItem Value="雪"></asp:ListItem>
                            </asp:DropDownList>
                            
                                最高気温<asp:DropDownList ID="DropDownList_MaxTemp" runat="server" CssClass="textbox" ></asp:DropDownList>℃
                                最低気温<asp:DropDownList ID="DropDownList_MinTemp" runat="server" CssClass="textbox" ></asp:DropDownList>℃

                            
                            降水確率<asp:DropDownList ID="DropDownList_Rain_p" runat="server" CssClass="textbox" >
                                <asp:ListItem Value="0">0%</asp:ListItem>
                                <asp:ListItem Value="10">10%</asp:ListItem>
                                <asp:ListItem Value="20">20%</asp:ListItem>
                                <asp:ListItem Value="30">30%</asp:ListItem>
                                <asp:ListItem Value="40">40%</asp:ListItem>
                                <asp:ListItem Value="50">50%</asp:ListItem>
                                <asp:ListItem Value="60">60%</asp:ListItem>
                                <asp:ListItem Value="70">70%</asp:ListItem>
                                <asp:ListItem Value="80">80%</asp:ListItem>
                                <asp:ListItem Value="90">90%</asp:ListItem>
                                <asp:ListItem Value="100">100%</asp:ListItem>
                            </asp:DropDownList>

                        </div>

                    <div class="center">
                        <p><asp:Label ID="Label_WeatherResult" runat="server" Text=""></asp:Label></p>
                        <asp:Button ID="Button_ReformWeather" runat="server" CssClass="btn_loginlist" Text="追加/更新" OnClick="btnReformWeather_Click" />
                        <asp:Button ID="Button_ReformWeatherEnd" runat="server" CssClass="btn_loginlist" Text="閉じる" OnClick="btnReformWeatherEnd_Click" />
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


        <asp:Literal ID="Literal_js" runat="server" ValidateRequestMode="Disabled" ></asp:Literal>

    </form>
</body>
</html>
