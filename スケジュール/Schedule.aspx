<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="WhereEver.Schedule" EnableEventValidation="false" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Schedule.css" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../MenuControl.css" />
    <meta http-equiv="Content-Type" content="text/html" charset="utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <title>Schedule</title>

    <style>
    </style>
</head>

<body>
    <form runat="server">



        <table>

            <tr>
                <td id="menu">
                    <Menu:c_menu ID="m" runat="server"></Menu:c_menu>
                </td>
            </tr>
        </table>

        <asp:Button runat="server" ID="ButtonA" OnClick="ButtonA_Click" />
        <asp:Label runat="server" ID="LabelA" CssClass="Center_Color1"></asp:Label>


        <div class="Center">
            <asp:Panel ID="Panel2" runat="server" HorizontalAlign="center">
                <asp:Button ID="Button3" runat="server" Text="登録" class="btn-flat-border" OnClick="Button3_Click" />

                <asp:Button ID="Button1" runat="server" class="btn-flat-border" OnClick="Button1_Click" Text="印刷" />
                <asp:Button ID="Button4" runat="server" Text="前へ" class="btn-flat-border" OnClick="Button4_Click" />
                <asp:Button ID="Button6" runat="server" Text="今週" class="btn-flat-border" OnClick="Button6_Click" />
                <asp:Button ID="Button5" runat="server" Text="次へ" class="btn-flat-border" OnClick="Button5_Click" />
                <asp:Label runat="server" ID="Label_Timer" Text=""></asp:Label>
            </asp:Panel>
        </div>

        <br />

        <div>

            <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center">
                <%--登録--%>

                <table class="Center_Table">

                    <tr>
                        <th class="Center_Color">日付</th>
                        <td>
                            <asp:TextBox runat="server" ID="TextBox2" TextMode="Date"></asp:TextBox>

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
                            <asp:CheckBoxList ID="CheckBoxList1" runat="server" CssClass="CheckBoxList_Left">
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

                        <td colspan="2">

                            <asp:Button ID="Button2" runat="server" Text="登録" class="btn-flat-border" OnClick="Button2_Click" />

                            <asp:Button ID="Button7" runat="server" Text="戻る" class="btn-flat-border" OnClick="Button7_Click" OnClientClick="if (A() == false) return(false)" />

                        </td>

                    </tr>

                </table>

            </asp:Panel>

        </div>


        <div>
            <asp:DataGrid runat="server"
                ID="Scdl3"
                class="Center"
                AutoGenerateColumns="False"
                OnItemDataBound="Scdl3_ItemDataBound"
                OnSelectedIndexChanged="Scdl3_SelectedIndexChanged"
                BorderStyle="Solid"
                BorderWidth="1px"
                CellPadding="3"
                GridLines="Vertical"
                Width="1200px">

                <HeaderStyle Width="200px" BackColor=" #16BA00" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="15px"></HeaderStyle>

                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="15px" />

                <AlternatingItemStyle BackColor="#ccffcc" />

                <Columns>

                    <asp:TemplateColumn
                        HeaderText="曜日"
                        ItemStyle-Height="30px"
                        ItemStyle-Width="30px">
                        <HeaderStyle Wrap="true" />

                        <ItemStyle Wrap="true" />

                        <ItemTemplate>
                            <asp:Label ID="Jikan" runat="server" Text=""></asp:Label>
                            <input type="hidden" id="Jikan1" runat="server" />
                        </ItemTemplate>

                    </asp:TemplateColumn>

                    <asp:TemplateColumn
                        HeaderText="月"
                        ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />

                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" />

                        <ItemTemplate>

                            <asp:Label ID="MondayTitle" runat="server" Text=""></asp:Label>
                            <asp:Label ID="Label7" runat="server" Text=""></asp:Label>

                        </ItemTemplate>

                    </asp:TemplateColumn>

                    <asp:TemplateColumn
                        HeaderText="火"
                        ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />

                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" />

                        <ItemTemplate>

                            <asp:Label ID="TuesdayTitle" runat="server" Text=""></asp:Label>
                            <asp:Label ID="Label8" runat="server" Text=""></asp:Label>

                        </ItemTemplate>

                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="水" ItemStyle-Width="100px">

                        <HeaderStyle Wrap="true" />

                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" />

                        <ItemTemplate>

                            <asp:Label ID="WednesdayTitle" runat="server" Text=""></asp:Label>
                            <asp:Label ID="Label9" runat="server" Text=""></asp:Label>

                        </ItemTemplate>

                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="木" ItemStyle-Width="100px">

                        <HeaderStyle Wrap="true" />

                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" />

                        <ItemTemplate>
                            <asp:Label ID="ThursdayTitle" runat="server" Text=""></asp:Label>
                            <asp:Label ID="Label10" runat="server" Text=""></asp:Label>

                        </ItemTemplate>

                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="金" ItemStyle-Width="100px">

                        <HeaderStyle Wrap="true" />

                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" />

                        <ItemTemplate>
                            <asp:Label ID="FridayTitle" runat="server" Text=""></asp:Label>
                            <asp:Label ID="Label11" runat="server" Text=""></asp:Label>

                        </ItemTemplate>

                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="土" ItemStyle-Width="100px">

                        <HeaderStyle Wrap="true" />

                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" />

                        <ItemTemplate>

                            <asp:Label ID="Saturday" runat="server" Text=""></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="日" ItemStyle-Width="100px">

                        <HeaderStyle Wrap="true" />

                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" />

                        <ItemTemplate>

                            <asp:Label ID="Sunday" runat="server" Text=""></asp:Label>

                        </ItemTemplate>

                    </asp:TemplateColumn>

                </Columns>

            </asp:DataGrid>

        </div>

        <br />

        <div class="Center">

            <asp:Panel runat="server" ID="Panel4" CssClass="Center">

                <asp:Button ID="Button10" runat="server" Text="検索" class="btn-flat-border" OnClick="Button10_Click" />

                <asp:DropDownList runat="server" ID="Ddl" AutoPostBack="True" OnTextChanged="Create4">
                    <asp:ListItem>昇順</asp:ListItem>
                    <asp:ListItem>降順</asp:ListItem>

                </asp:DropDownList>



            </asp:Panel>

        </div>

        <div>
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
                            <asp:CheckBoxList ID="CheckBoxList2" runat="server" CssClass="CheckBoxList_Left">
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
                        <td colspan="2">
                            <asp:Button ID="Button8" runat="server" class="btn-flat-border" OnClick="Button8_Click" Text="検索" />
                            <asp:Button ID="Button9" runat="server" class="btn-flat-border" OnClick="Button7_Click" OnClientClick="if (A() == false) return(false)" Text="戻る" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>

        <br />

        <div>
            <asp:DataGrid
                runat="server"
                ID="ScdlList"
                CssClass="Center"
                AutoGenerateColumns="False"
                OnEditCommand="ScdlList_EditCommand"
                OnCancelCommand="ScdlList_CancelCommand"
                OnUpdateCommand="ScdlList_UpdateCommand"
                OnItemDataBound="ScdlList_ItemDataBound" OnItemCommand="ScdlList_ItemCommand"
                HeaderStyle-Width="200px"
                HorizontalAlign="Center" Width="1200px">

                <AlternatingItemStyle BackColor="#ccffcc" />
                <Columns>


                    <asp:BoundColumn DataField="date" HeaderText="日付" HeaderStyle-Width="" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" />
                    </asp:BoundColumn>

                    <asp:BoundColumn DataField="time" HeaderText="時間" ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" />
                    </asp:BoundColumn>

                    <asp:BoundColumn DataField="title" HeaderText="内容" ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" />
                    </asp:BoundColumn>

                    <asp:BoundColumn DataField="name" HeaderText="担当者名" ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" />
                    </asp:BoundColumn>




                    <asp:ButtonColumn
                        ItemStyle-Width="100px"
                        HeaderText="削除"
                        ButtonType="PushButton"
                        Text="削除"
                        CommandName="Delete">


                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:ButtonColumn>

                    <asp:EditCommandColumn
                        HeaderText="編集"
                        ItemStyle-Width="100px"
                        EditText="編集"
                        CancelText="キャンセル"
                        UpdateText="保存" ButtonType="PushButton">
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:EditCommandColumn>

                </Columns>

                <HeaderStyle Width="200px" BackColor="#16BA00" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="12px"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="15px" />
            </asp:DataGrid>
        </div>

        <br />

        <%--<div>
            <asp:GridView runat="server" ID="TestGV" CssClass="Center" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="SdlNo" DataSourceID="Sql1" OnRowCreated="TestGV_RowCreated" ShowFooter="True">

                <Columns>

                    <asp:TemplateField HeaderText="管理番号" SortExpression="SdlNo">

                        <EditItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("SdlNo") %>'></asp:Label>
                        </EditItemTemplate>

                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("SdlNo") %>'></asp:Label>
                        </ItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="日付" SortExpression="date">

                        <EditItemTemplate>
                            <asp:TextBox ID="TextBoxA" runat="server" Text='<%# Bind("date") %>'></asp:TextBox>
                        </EditItemTemplate>

                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("date", "{0:yyyy年MM月dd日dddd}") %>'></asp:Label>
                        </ItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="時間" SortExpression="time">

                        <EditItemTemplate>
                            <asp:DropDownList ID="Test1" runat="server" Text='<%# Bind("time") %>' SelectedValue='<%# Bind("time", "{0:HH時MM分}") %>' DataSourceID="Sql1" DataTextField="time" DataValueField="time"></asp:DropDownList>
                        </EditItemTemplate>

                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("time", "{0:HH時MM分}") %>'></asp:Label>
                        </ItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="内容" SortExpression="title">

                        <EditItemTemplate>
                            <asp:TextBox ID="TextBoxB" runat="server" Text='<%# Bind("title") %>'></asp:TextBox>
                        </EditItemTemplate>

                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("title") %>'></asp:Label>
                        </ItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="担当者" SortExpression="name">

                        <EditItemTemplate>
                            <asp:TextBox ID="TextBoxC" runat="server" Text='<%# Bind("name") %>'></asp:TextBox>
                        </EditItemTemplate>

                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                        </ItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="選択">
                        <EditItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="編集/削除" ShowHeader="False">

                        <EditItemTemplate>
                            <asp:Button ID="Button1" runat="server" CausesValidation="True" CommandName="Update" Text="更新" />
                            &nbsp;<asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="Cancel" Text="キャンセル" />
                        </EditItemTemplate>

                        <ItemTemplate>
                            <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="Edit" Text="編集" />
                            &nbsp;<asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="Delete" Text="削除" OnClientClick="return confirm('本当に削除しますか？')" />
                        </ItemTemplate>

                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

            <asp:SqlDataSource ID="Sql1" runat="server" ConnectionString="<%$ ConnectionStrings:WhereverConnectionString %>" DeleteCommand="DELETE FROM [T_Schedule] WHERE [SdlNo] = @SdlNo" InsertCommand="INSERT INTO [T_Schedule] ([date], [time], [title], [name], [KanriFlag], [SdlNo]) VALUES (@date, @time, @title, @name, @KanriFlag, @SdlNo)" SelectCommand="SELECT * FROM [T_Schedule]" UpdateCommand="UPDATE [T_Schedule] SET [date] = @date, [time] = @time, [title] = @title, [name] = @name, [KanriFlag] = @KanriFlag WHERE [SdlNo] = @SdlNo">
                <DeleteParameters>
                    <asp:Parameter Name="SdlNo" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="date" Type="DateTime" />
                    <asp:Parameter Name="time" Type="String" />
                    <asp:Parameter Name="title" Type="String" />
                    <asp:Parameter Name="name" Type="String" />
                    <asp:Parameter Name="KanriFlag" Type="String" />
                    <asp:Parameter Name="SdlNo" Type="Int32" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="date" Type="DateTime" />
                    <asp:Parameter Name="time" Type="String" />
                    <asp:Parameter Name="title" Type="String" />
                    <asp:Parameter Name="name" Type="String" />
                    <asp:Parameter Name="KanriFlag" Type="String" />
                    <asp:Parameter Name="SdlNo" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>

        </div>--%>

        <footer>
            <section class="Center">
                <p>最終アクセス:<span id="time"></span></p>
            </section>
        </footer>

        <script src="jquery-3.6.0.min.js"></script>
        <script src="Schedule.js"></script>

    </form>

</body>

</html>
