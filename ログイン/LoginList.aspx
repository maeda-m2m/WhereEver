<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginList.aspx.cs" Inherits="WhereEver.LoginList" ValidateRequest="false" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="application/json; charset=utf-8" />
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
                    <p class="index1">--本日（<asp:Label ID="Label_Today" runat="server" Text="Label"></asp:Label>）の予定--</p>
                    <asp:Label ID="Label_WhatNow" runat="server" Text="" ValidateRequestMode="Disabled"></asp:Label><br />
                    <p class="index1">--天気予報--</p>
                    <p><asp:Label ID="Label_WeatherComment" runat="server" Text="" ValidateRequestMode="Disabled"></asp:Label></p>
                    <asp:Label ID="Label_Weather2" runat="server" Text="" ValidateRequestMode="Disabled"></asp:Label><br />



                    <asp:Chart runat="server" ID="Chart_Weather" DataSourceID="SqlDataSource_Weather" Width="1170px" AlternateText="図表１　気温と降水確率の推移(No_Image)" >
                        <Legends>
                            <asp:Legend IsDockedInsideChartArea="False" Name="Legend1" DockedToChartArea="ChartArea" Alignment="Center" Docking="Top">
                            </asp:Legend>
                        </Legends>
                        <series>
                            <asp:Series Name="降水確率（%）" ChartType="Column" XValueMember="Date" YValueMembers="Rain_p" IsValueShownAsLabel="true" LabelFormat="0\%" CustomProperties="LabelStyle=Bottom" ></asp:Series>
                            <asp:Series Name="最低気温（℃）" ChartType="Line" XValueMember="Date" YValueMembers="MinTemp" MarkerStyle="Diamond" MarkerSize="10" IsValueShownAsLabel="true" LabelFormat="0℃" LabelForeColor="Blue" Color="Blue" ></asp:Series>
                            <asp:Series Name="最高気温（℃）" ChartType="Line" XValueMember="Date" YValueMembers="MaxTemp" MarkerStyle="Circle" MarkerSize="10" IsValueShownAsLabel="true" LabelFormat="0℃" LabelForeColor="Red" Color="Red" ></asp:Series>
                        </series>
                        <chartareas>
                            <asp:ChartArea Name="ChartArea" BackColor="White"></asp:ChartArea>
                        </chartareas>
                        <Titles>
                            <asp:Title DockedToChartArea="ChartArea" Docking="Bottom" IsDockedInsideChartArea="False" Name="Title_Weather" Text="図表１　気温と降水確率の推移">
                            </asp:Title>
                        </Titles>
                    </asp:Chart>

                    <asp:SqlDataSource ID="SqlDataSource_Weather" runat="server" ConnectionString="<%$ ConnectionStrings:WhereverConnectionString %>" SelectCommand="SELECT [Date], [MaxTemp], [MinTemp], [Rain_p] FROM [T_Weather] WHERE (CAST(DATE AS [Date]) &gt;= @Date) ORDER BY CAST(DATE AS [Date])">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="Label_WeatherDate" DbType="Date" DefaultValue="2021/01/01" Name="Date" PropertyName="Text" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:Label ID="Label_WeatherDate" runat="server" Text="2021/01/01" Visible="false" ></asp:Label>

                    <p class="index1">--お知らせ--</p>
                    <asp:Label ID="Label_Yotei" runat="server" Text="" ValidateRequestMode="Disabled"></asp:Label><br />
                </div>
               </div>
              </asp:Panel>


                <div class="Edit_btn_space">
                  <asp:Button ID="Button_EditTop" runat="server" CssClass="btn_loginlist" Text="お知らせ編集" OnClick="BtnEditTop_Click" PostBackUrl="#" />
                  <asp:Button ID="Button_EditWeather" runat="server" CssClass="btn_loginlist" Text="天気予報編集" OnClick="BtnEditWeather_Click" PostBackUrl="#" />
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
                        <div class="Edit_btn_space">
                        <asp:Button ID="Button_ReformTop" runat="server" CssClass="btn_loginlist" Text="更新" OnClick="BtnReformTop_Click" />
                        <asp:Button ID="Button_ReformTopEnd" runat="server" CssClass="btn_loginlist" Text="閉じる" OnClick="BtnReformTopEnd_Click" />
                        <asp:Button ID="Button_ReformDelete" runat="server" CssClass="btn_loginlist" Text="全消去" OnClick="BtnReformTopDel_Click" PostBackUrl="#edittop" />
                        <asp:Button ID="Button_ReformReload" runat="server" CssClass="btn_loginlist" Text="全取り消し" OnClick="BtnReformTopReload_Click" PostBackUrl="#edittop" />
                        </div>
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


                        </div><%-- article --%>
                            <div class="center">
                                <div class="Edit_btn_space">
                                <p><asp:Label ID="Label_WeatherResult" runat="server" Text=""></asp:Label></p>
                                <asp:Button ID="Button_ReformWeather" runat="server" CssClass="btn_loginlist" Text="追加/更新" OnClick="BtnReformWeather_Click" />
                                <asp:Button ID="Button_ReformWeatherEnd" runat="server" CssClass="btn_loginlist" Text="閉じる" OnClick="BtnReformWeatherEnd_Click" />
                                </div>
                            </div><%-- center --%>
                   </div><%-- Edit --%>
                  </asp:Panel>




           </div><%-- Main --%>






            <div id="Sub">

        <div class ="bg_test">
            <table class="bg_test-text">
                    <tr>
                        <td><asp:Button ID="btnOut" runat="server" CssClass="btn_loginlist" Text="ログアウト" OnClick="BtnOut_Click" /></td>
                        <td><asp:Button ID="btnKanri" runat="server" CssClass="btn_loginlist" Text="マイページ" OnClick="BtnKanri_Click" /></td>
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
                                        <span class="hr"></span>                                       
                                        <p class="log"><asp:Label ID="lblName" runat="server"></asp:Label></p>
                                        <span class="hr"></span>
                                        <div class="icon"><asp:Label ID="lblIcon" runat="server" ValidateRequestMode="Disabled"></asp:Label></div>
                                        <span class="hr"></span>
                                        <table class="log">
                                            <tr>
                                                <td>
                                                    <p><asp:Label ID="lbl1" runat="server" Text="ログイン"></asp:Label></p>
                                                </td>
                                                <td>
                                                    <p><asp:Label ID="lbl2" runat="server" Text="ログアウト"></asp:Label></p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <p><asp:Label ID="lblLoginTime" runat="server"></asp:Label></p>
                                                </td>
                                                <td>
                                                    <p><asp:Label ID="lblLogoutTime" runat="server"></asp:Label></p>
                                                </td>
                                            </tr>
                                        </table>
                                        <span class="hr"></span>
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
