<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="WhereEver.Schedule" EnableEventValidation="false" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="https://unpkg.com/ress/dist/ress.min.css" />
    <link rel="stylesheet" href="Schedule.css" type="text/css" />

    <meta http-equiv="Content-Type" content="text/html" charset="utf-8" />

    <link rel="preconnect" href="https://fonts.gstatic.com" />
    <link href="https://fonts.googleapis.com/css2?family=Noto+Sans+JP&display=swap" rel="stylesheet" />

    <title>Schedule</title>


</head>

<body>
    <form runat="server">

        <header id="wrapper">
            <div id="header_title">

                <a href="~/ログイン/LoginList.aspx" runat="server" id="header_index">WhereEver</a>

            </div>

            <nav>

                <ul class="nav_main">
                    <li id="open_nav">
                        <img src="batu_01.png" alt="" class="img1 img2" /></li>

                    <li><a href="~/スケジュール/Schedule.aspx" runat="server" class="header2_menu">スケジュール</a></li>
                    <li><a href="~/スケジュール/Wiki_Top.aspx" runat="server" class="header2_menu">社内Wiki</a></li>
                    <li><a href="~/各申請書類/Shinsei.aspx" runat="server" class="header2_menu">各申請書類</a></li>
                    <li><a href="~/チャット/Chat.aspx" runat="server" class="header2_menu">チャット<asp:Label ID="Label1" runat="server" Visible="False"></asp:Label></a></li>
                    <li><a href="~/ファイル共有/FileShare.aspx" runat="server" class="header2_menu">ファイル共有</a></li>
                    <li><a href="~/Project System/PIchiran.aspx" runat="server" class="header2_menu">プロジェクト</a></li>
                    <li><a href="~/管理ページ/Kanri.aspx" runat="server" class="header2_menu">マイページ</a></li>

                </ul>

            </nav>
        </header>

        <aside id="aside">

            <h2 class="aside_h2">WhereEver</h2>
            <ul class="hidden">
                <li><a href="~/ログイン/LoginList.aspx" runat="server" class="header2_menu">トップページ</a></li>
                <li><a href="~/スケジュール/Schedule.aspx" runat="server" class="header2_menu">スケジュール</a></li>
                <li><a href="~/スケジュール/Wiki_Top.aspx" runat="server" class="header2_menu">社内Wiki</a></li>
                <li><a href="~/各申請書類/Shinsei.aspx" runat="server" class="header2_menu">各申請書類</a></li>
                <li><a href="~/チャット/Chat.aspx" runat="server" class="header2_menu">チャット<asp:Label ID="Label3" runat="server" Visible="False"></asp:Label></a></li>
                <li><a href="~/ファイル共有/FileShare.aspx" runat="server" class="header2_menu">ファイル共有</a></li>
                <li><a href="~/Project System/PIchiran.aspx" runat="server" class="header2_menu">プロジェクト</a></li>
                <li><a href="~/管理ページ/Kanri.aspx" runat="server" class="header2_menu">マイページ</a></li>
                <li id="Image1">テスト</li>

            </ul>
            <h2 class="aside_h2">MySchedule</h2>
            <asp:RadioButtonList runat="server" ID="rbl1" CssClass="hidden" RepeatColumns="2" AutoPostBack="true" OnSelectedIndexChanged="rbl1_SelectedIndexChanged">

                <asp:ListItem>石岡</asp:ListItem>
                <asp:ListItem>木村</asp:ListItem>
                <asp:ListItem>佐藤</asp:ListItem>
                <asp:ListItem>白井</asp:ListItem>
                <asp:ListItem>寺島</asp:ListItem>
                <asp:ListItem>前田</asp:ListItem>
                <asp:ListItem>三浦</asp:ListItem>
                <asp:ListItem>三津谷</asp:ListItem>
                <asp:ListItem>柳沢</asp:ListItem>
                <asp:ListItem>張</asp:ListItem>
                <asp:ListItem>鯉淵</asp:ListItem>
                <asp:ListItem>坂口</asp:ListItem>
                <asp:ListItem>坂田</asp:ListItem>
                <asp:ListItem>一番ヶ瀬</asp:ListItem>
                <asp:ListItem>髙栁社長</asp:ListItem>


            </asp:RadioButtonList>

            <h2 class="aside_h2">登録・印刷・検索</h2>
            <ul class="hidden">
                <li>
                    <asp:Button ID="Button11" runat="server" Text="登録" OnClick="Button3_Click" /></li>
                <li>
                    <asp:Button ID="Button12" runat="server" OnClick="Button1_Click" Text="印刷" /></li>
                <li>
                    <asp:Button ID="Button13" runat="server" Text="検索" OnClick="Button10_Click" /></li>
            </ul>

        </aside>


        <main>

            <br />
            <section class="section_btn">

                <asp:Panel ID="Panel2" runat="server">

                    <asp:Button ID="Button3" runat="server" Text="登録" class="btn-flat-border" OnClick="Button3_Click" />
                    <asp:Button ID="Button1" runat="server" class="btn-flat-border" OnClick="Button1_Click" Text="印刷" />

                    <asp:Button ID="Button4" runat="server" Text="前へ" class="btn-flat-border" OnClick="Button4_Click" />
                    <asp:Button ID="Button6" runat="server" Text="今週" class="btn-flat-border" OnClick="Button6_Click" />
                    <asp:Button ID="Button5" runat="server" Text="次へ" class="btn-flat-border" OnClick="Button5_Click" />
                    <label runat="server" id="label6">test</label>

                </asp:Panel>

            </section>

            <br />

            <section>

                <asp:Panel ID="Panel1" runat="server">
                    <%--登録--%>

                    <table class="Center_Table">

                        <tr>
                            <th class="Center_Color">日付</th>
                            <td>
                                <asp:TextBox runat="server" ID="TextBox2" TextMode="Date"></asp:TextBox>

                            </td>
                        </tr>
                        <tr>
                            <th class="Center_Color">定例会議</th>
                            <td>

                                <asp:CheckBox runat="server" ID="cb1" />
                                <asp:TextBox runat="server" ID="ddlist1" TextMode="Number"></asp:TextBox>
                                <label runat="server" id="label5">週間分</label>
                                <label runat="server" id="label4">チェックを入れ、数字を入力してください。</label>
                            </td>
                        </tr>

                        <tr>
                            <th class="Center_Color">時間</th>
                            <td>
                                <asp:DropDownList ID="DropDownList1" runat="server">
                                    <asp:ListItem>9:00</asp:ListItem>
                                    <asp:ListItem>9:15</asp:ListItem>
                                    <asp:ListItem>9:30</asp:ListItem>
                                    <asp:ListItem>9:45</asp:ListItem>
                                    <asp:ListItem>10:00</asp:ListItem>
                                    <asp:ListItem>10:15</asp:ListItem>
                                    <asp:ListItem>10:30</asp:ListItem>
                                    <asp:ListItem>10:45</asp:ListItem>
                                    <asp:ListItem>11:00</asp:ListItem>
                                    <asp:ListItem>11:15</asp:ListItem>
                                    <asp:ListItem>11:30</asp:ListItem>
                                    <asp:ListItem>11:45</asp:ListItem>
                                    <asp:ListItem>12:00</asp:ListItem>
                                    <asp:ListItem>12:15</asp:ListItem>
                                    <asp:ListItem>12:30</asp:ListItem>
                                    <asp:ListItem>12:45</asp:ListItem>
                                    <asp:ListItem>13:00</asp:ListItem>
                                    <asp:ListItem>13:15</asp:ListItem>
                                    <asp:ListItem>13:30</asp:ListItem>
                                    <asp:ListItem>13:45</asp:ListItem>
                                    <asp:ListItem>14:00</asp:ListItem>
                                    <asp:ListItem>14:15</asp:ListItem>
                                    <asp:ListItem>14:30</asp:ListItem>
                                    <asp:ListItem>14:45</asp:ListItem>
                                    <asp:ListItem>15:00</asp:ListItem>
                                    <asp:ListItem>15:15</asp:ListItem>
                                    <asp:ListItem>15:30</asp:ListItem>
                                    <asp:ListItem>15:45</asp:ListItem>
                                    <asp:ListItem>16:00</asp:ListItem>
                                    <asp:ListItem>16:15</asp:ListItem>
                                    <asp:ListItem>16:30</asp:ListItem>
                                    <asp:ListItem>16:45</asp:ListItem>
                                    <asp:ListItem>17:00</asp:ListItem>
                                    <asp:ListItem>17:15</asp:ListItem>
                                    <asp:ListItem>17:30</asp:ListItem>
                                    <asp:ListItem>17:45</asp:ListItem>
                                    <asp:ListItem>18:00</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr>
                            <th class="Center_Color">内容</th>
                            <td>
                                <asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged" Columns="40" placeholder="例:LIXIL　Web会議" TextMode="Search"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <th class="Center_Color">担当者</th>
                            <td>
                                <asp:CheckBoxList ID="CheckBoxList1" runat="server" CssClass="CheckBoxList_Left" RepeatDirection="Horizontal" RepeatColumns="2">
                                    <asp:ListItem>石岡</asp:ListItem>
                                    <asp:ListItem>木村</asp:ListItem>
                                    <asp:ListItem>佐藤</asp:ListItem>
                                    <asp:ListItem>白井</asp:ListItem>
                                    <asp:ListItem>寺島</asp:ListItem>
                                    <asp:ListItem>前田</asp:ListItem>
                                    <asp:ListItem>三浦</asp:ListItem>
                                    <asp:ListItem>三津谷</asp:ListItem>
                                    <asp:ListItem>柳沢</asp:ListItem>
                                    <asp:ListItem>張</asp:ListItem>
                                    <asp:ListItem>鯉淵</asp:ListItem>
                                    <asp:ListItem>坂口</asp:ListItem>
                                    <asp:ListItem>坂田</asp:ListItem>
                                    <asp:ListItem>一番ヶ瀬</asp:ListItem>
                                    <asp:ListItem>髙栁社長</asp:ListItem>
                                    <asp:ListItem>藤川</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <th class="Center_Color">備考</th>
                            <td>
                                <asp:TextBox runat="server" ID="tb1" TextMode="MultiLine" Wrap="true" BorderStyle="Solid"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th class="Center_Color">ボタン</th>
                            <td>

                                <asp:Button ID="Button2" runat="server" Text="登録" class="btn-flat-border" OnClick="Button2_Click" />

                                <asp:Button ID="Button7" runat="server" Text="戻る" class="btn-flat-border" OnClick="Button7_Click" />

                            </td>

                        </tr>

                    </table>

                </asp:Panel>

            </section>


            <section>
                <asp:DataGrid runat="server" ID="Scdl3" AutoGenerateColumns="False" OnItemDataBound="Scdl3_ItemDataBound" OnSelectedIndexChanged="Scdl3_SelectedIndexChanged" GridLines="Vertical">

                    <HeaderStyle Width="200px" BackColor=" #16BA00" ForeColor="White" Font-Size="15px" HorizontalAlign="Center"></HeaderStyle>

                    <AlternatingItemStyle BackColor="#ccffcc" />

                    <Columns>

                        <asp:TemplateColumn HeaderText="曜日" ItemStyle-Height="30px" ItemStyle-Width="30px">


                            <ItemTemplate>
                                <asp:Label ID="Jikan" runat="server" Text=""></asp:Label>
                                <input type="hidden" id="Jikan1" runat="server" />
                            </ItemTemplate>

                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="月" ItemStyle-Width="150px">

                            <ItemTemplate>

                                <asp:Label ID="MondayTitle" runat="server" Text=""></asp:Label>
                                <asp:Label ID="Label7" runat="server" Text=""></asp:Label>

                            </ItemTemplate>

                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="火" ItemStyle-Width="150px">




                            <ItemTemplate>

                                <asp:Label ID="TuesdayTitle" runat="server" Text=""></asp:Label>
                                <asp:Label ID="Label8" runat="server" Text=""></asp:Label>

                            </ItemTemplate>

                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="水" ItemStyle-Width="150px">

                            <ItemTemplate>

                                <asp:Label ID="WednesdayTitle" runat="server" Text=""></asp:Label>
                                <asp:Label ID="Label9" runat="server" Text=""></asp:Label>

                            </ItemTemplate>

                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="木" ItemStyle-Width="150px">

                            <ItemTemplate>
                                <asp:Label ID="ThursdayTitle" runat="server" Text=""></asp:Label>
                                <asp:Label ID="Label10" runat="server" Text=""></asp:Label>

                            </ItemTemplate>

                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="金" ItemStyle-Width="150px">

                            <ItemTemplate>
                                <asp:Label ID="FridayTitle" runat="server" Text=""></asp:Label>
                                <asp:Label ID="Label11" runat="server" Text=""></asp:Label>

                            </ItemTemplate>

                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="土" ItemStyle-Width="150px">


                            <ItemTemplate>

                                <asp:Label ID="Saturday" runat="server" Text=""></asp:Label>

                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="日" ItemStyle-Width="150px">

                            <ItemTemplate>

                                <asp:Label ID="Sunday" runat="server" Text=""></asp:Label>

                            </ItemTemplate>

                        </asp:TemplateColumn>

                    </Columns>

                </asp:DataGrid>

            </section>

            <br />

            <section class="Center">

                <asp:Panel runat="server" ID="Panel4">

                    <asp:Button ID="Button10" runat="server" Text="検索" class="btn-flat-border" OnClick="Button10_Click" />

                    <asp:DropDownList runat="server" ID="Ddl" AutoPostBack="True" OnTextChanged="Create4">
                        <asp:ListItem>昇順</asp:ListItem>
                        <asp:ListItem>降順</asp:ListItem>

                    </asp:DropDownList>



                </asp:Panel>

            </section>

            <section>
                <asp:Panel ID="Panel3" runat="server">
                    <%--検索--%>


                    <table class="Center_Table">

                        <tr>
                            <th class="Center_Color">日付</th>
                            <td>
                                <asp:TextBox ID="TextBox3" runat="server" Text="" placeholder="例:一か月単位「04」特定の日にち「04　01」など" Columns="40" TextMode="SingleLine"></asp:TextBox>

                            </td>
                        </tr>

                        <tr>
                            <th class="Center_Color">時間</th>
                            <td>
                                <asp:DropDownList ID="DropDownList2" runat="server">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem>9:00</asp:ListItem>
                                    <asp:ListItem>9:15</asp:ListItem>
                                    <asp:ListItem>9:30</asp:ListItem>
                                    <asp:ListItem>9:45</asp:ListItem>
                                    <asp:ListItem>10:00</asp:ListItem>
                                    <asp:ListItem>10:15</asp:ListItem>
                                    <asp:ListItem>10:30</asp:ListItem>
                                    <asp:ListItem>10:45</asp:ListItem>
                                    <asp:ListItem>11:00</asp:ListItem>
                                    <asp:ListItem>11:15</asp:ListItem>
                                    <asp:ListItem>11:30</asp:ListItem>
                                    <asp:ListItem>11:45</asp:ListItem>
                                    <asp:ListItem>12:00</asp:ListItem>
                                    <asp:ListItem>12:15</asp:ListItem>
                                    <asp:ListItem>12:30</asp:ListItem>
                                    <asp:ListItem>12:45</asp:ListItem>
                                    <asp:ListItem>13:00</asp:ListItem>
                                    <asp:ListItem>13:15</asp:ListItem>
                                    <asp:ListItem>13:30</asp:ListItem>
                                    <asp:ListItem>13:45</asp:ListItem>
                                    <asp:ListItem>14:00</asp:ListItem>
                                    <asp:ListItem>14:15</asp:ListItem>
                                    <asp:ListItem>14:30</asp:ListItem>
                                    <asp:ListItem>14:45</asp:ListItem>
                                    <asp:ListItem>15:00</asp:ListItem>
                                    <asp:ListItem>15:15</asp:ListItem>
                                    <asp:ListItem>15:30</asp:ListItem>
                                    <asp:ListItem>15:45</asp:ListItem>
                                    <asp:ListItem>16:00</asp:ListItem>
                                    <asp:ListItem>16:15</asp:ListItem>
                                    <asp:ListItem>16:30</asp:ListItem>
                                    <asp:ListItem>16:45</asp:ListItem>
                                    <asp:ListItem>17:00</asp:ListItem>
                                    <asp:ListItem>17:15</asp:ListItem>
                                    <asp:ListItem>17:30</asp:ListItem>
                                    <asp:ListItem>17:45</asp:ListItem>
                                    <asp:ListItem>18:00</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr>
                            <th class="Center_Color">内容</th>
                            <td>
                                <asp:TextBox ID="TextBox5" runat="server" Text="" Columns="40" placeholder="例:LIXIL" TextMode="Search"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <th class="Center_Color">担当者</th>
                            <td>
                                <asp:CheckBoxList ID="CheckBoxList2" runat="server" CssClass="CheckBoxList_Left" RepeatDirection="Horizontal" RepeatColumns="2">
                                    <asp:ListItem>石岡</asp:ListItem>
                                    <asp:ListItem>木村</asp:ListItem>
                                    <asp:ListItem>佐藤</asp:ListItem>
                                    <asp:ListItem>白井</asp:ListItem>
                                    <asp:ListItem>寺島</asp:ListItem>
                                    <asp:ListItem>前田</asp:ListItem>
                                    <asp:ListItem>三浦</asp:ListItem>
                                    <asp:ListItem>三津谷</asp:ListItem>
                                    <asp:ListItem>柳沢</asp:ListItem>
                                    <asp:ListItem>張</asp:ListItem>
                                    <asp:ListItem>鯉淵</asp:ListItem>
                                    <asp:ListItem>坂口</asp:ListItem>
                                    <asp:ListItem>坂田</asp:ListItem>
                                    <asp:ListItem>一番ヶ瀬</asp:ListItem>
                                    <asp:ListItem>髙栁社長</asp:ListItem>
                                    <asp:ListItem>藤川</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>

                        <tr>
                            <th class="Center_Color">ボタン</th>
                            <td>
                                <asp:Button ID="Button8" runat="server" class="btn-flat-border" OnClick="Button8_Click" Text="検索" />
                                <asp:Button ID="Button9" runat="server" class="btn-flat-border" OnClick="Button7_Click" OnClientClick="if (A() == false) return(false)" Text="戻る" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </section>

            <br />

            <section class="Center">
                <asp:Label runat="server" ID="Label2" Text=""></asp:Label>
                <asp:DataGrid runat="server" ID="ScdlList" AutoGenerateColumns="False" OnEditCommand="ScdlList_EditCommand" OnCancelCommand="ScdlList_CancelCommand" OnUpdateCommand="ScdlList_UpdateCommand" OnItemDataBound="ScdlList_ItemDataBound" OnItemCommand="ScdlList_ItemCommand">

                    <HeaderStyle Width="200px" Height="50px" BackColor="#16BA00" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"></HeaderStyle>


                    <AlternatingItemStyle BackColor="#ccffcc" />

                    <Columns>


                        <asp:TemplateColumn HeaderText="選択" ItemStyle-Width="70px">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="checkbox1" />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="詳細" ItemStyle-Width="70px">
                            <ItemTemplate>
                                <asp:Button runat="server" ID="btn9" Text="詳細" />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:BoundColumn DataField="date" HeaderText="日付" ItemStyle-Height="50px" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>

                        <asp:BoundColumn DataField="time" HeaderText="時間" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>

                        <asp:BoundColumn DataField="title" HeaderText="内容" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>

                        <asp:BoundColumn DataField="name" HeaderText="担当者名" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>

                        <asp:ButtonColumn ItemStyle-Width="150px" HeaderText="削除" ButtonType="PushButton" Text="削除" CommandName="Delete" ItemStyle-BorderColor="Black" ItemStyle-BorderStyle="Solid" ItemStyle-BorderWidth="1px"></asp:ButtonColumn>

                        <asp:EditCommandColumn ItemStyle-CssClass="edit_focus" HeaderText="編集" ItemStyle-Width="150px" EditText="編集" CancelText="キャンセル" UpdateText="保存" ButtonType="PushButton" ItemStyle-BorderColor="Black" ItemStyle-BorderStyle="Solid" ItemStyle-BorderWidth="1px"></asp:EditCommandColumn>

                    </Columns>

                </asp:DataGrid>
            </section>

            <br />
        </main>



        <footer>
            <section class="Center">
                <p>現在時刻:<span id="time"></span></p>
                <a href="#" runat="server">トップへ</a>

            </section>





        </footer>

        <script src="jquery-3.6.0.min.js"></script>
        <script src="Schedule.js"></script>



        <script>





</script>

    </form>

</body>

</html>
