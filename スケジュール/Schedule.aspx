<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="WhereEver.Schedule" EnableEventValidation="false" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Schedule.css" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../MenuControl.css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Schedule</title>

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

        <br />




        <div class="Center">
            <asp:Panel ID="Panel2" runat="server" HorizontalAlign="center">
                <asp:Button ID="Button3" runat="server" Text="登録" class="btn-flat-border" OnClick="Button3_Click" />
                <asp:Button ID="Button10" runat="server" Text="検索" class="btn-flat-border" OnClick="Button10_Click" />
                <asp:Button ID="Button1" runat="server" class="btn-flat-border" OnClick="Button1_Click1" Text="印刷（作成中）" />
                <input type="button" class="btn-flat-border" value="印刷" onclick="window.print();" />
                <asp:Button ID="Button4" runat="server" Text="前の週" class="btn-flat-border" OnClick="Button4_Click" />
                <asp:Button ID="Button6" runat="server" Text="今週" class="btn-flat-border" OnClick="Button6_Click" />
                <asp:Button ID="Button5" runat="server" Text="次の週" class="btn-flat-border" OnClick="Button5_Click" />
            </asp:Panel>
        </div>

        <br />

        <div class="Test">

            <div>

                <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center">

                    <table class="Center">

                        <tr>
                            <th class="Center_Color">日付</th>
                            <td>
                                <input type="date" runat="server" name="Calendar10" id="Calendar10" />
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
                                <asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
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
                <asp:Panel ID="Panel3" runat="server">


                    <table class="Center">

                        <tr>
                            <th class="Center_Color">日付</th>
                            <td>
                                <asp:TextBox ID="TextBox3" runat="server" Text=""></asp:TextBox>

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
                                <asp:TextBox ID="TextBox2" runat="server" Text=""></asp:TextBox>
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




        </div>




        <div>
            <asp:DataGrid runat="server"
                ID="Scdl3"
                CssClass="Center"
                AutoGenerateColumns="False"
                OnItemDataBound="Scdl3_ItemDataBound"
                HeaderStyle-Width="200px"
                OnSelectedIndexChanged="Scdl3_SelectedIndexChanged"
                BackColor="White"
                BorderColor="#999999"
                BorderStyle="Solid"
                BorderWidth="1px"
                CellPadding="3"
                ForeColor="Black"
                GridLines="Vertical"
                Width="1200px">


                <AlternatingItemStyle BackColor="#ccffcc" />
                <Columns>
                    <asp:TemplateColumn HeaderText="曜日" HeaderStyle-Width="" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="Jikan" runat="server" Text=""></asp:Label>
                            <input type="hidden" id="Jikan1" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="月" ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" CssClass="scdl" />

                        <ItemTemplate>
                            <asp:Label ID="MondayTitle" runat="server" Text=""></asp:Label>
                            <asp:Label ID="Label7" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="火" ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" CssClass="scdl" />
                        <ItemTemplate>
                            <asp:Label ID="TuesdayTitle" runat="server" Text=""></asp:Label>
                            <asp:Label ID="Label8" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="水" ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" CssClass="scdl" />
                        <ItemTemplate>
                            <asp:Label ID="WednesdayTitle" runat="server" Text=""></asp:Label>
                            <asp:Label ID="Label9" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="木" ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" CssClass="scdl" />
                        <ItemTemplate>
                            <asp:Label ID="ThursdayTitle" runat="server" Text=""></asp:Label>
                            <asp:Label ID="Label10" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="金" ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" CssClass="scdl" />
                        <ItemTemplate>
                            <asp:Label ID="FridayTitle" runat="server" Text=""></asp:Label>
                            <asp:Label ID="Label11" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                </Columns>


                <FooterStyle BackColor="#CCCCCC" />


                <HeaderStyle Width="200px" BackColor="#16BA00" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="12px"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="15px" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <SelectedItemStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            </asp:DataGrid>

        </div>





        <div>
            <asp:DataGrid
                runat="server"
                ID="ScdlList"
                CssClass="Center"
                AutoGenerateColumns="false"
                OnEditCommand="ScdlList_EditCommand"
                OnCancelCommand="ScdlList_CancelCommand"
                OnUpdateCommand="ScdlList_UpdateCommand"
                OnItemDataBound="ScdlList_ItemDataBound" OnItemCommand="ScdlList_ItemCommand"
                HeaderStyle-Width="200px"
                HorizontalAlign="center" Width="1200px">

                <AlternatingItemStyle BackColor="#ccffcc" />
                <Columns>
                    <asp:BoundColumn DataField="date" HeaderText="日付" HeaderStyle-Width="" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" />
                    </asp:BoundColumn>

                    <asp:BoundColumn DataField="time" HeaderText="時間" ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" CssClass="scdl" />
                    </asp:BoundColumn>

                    <asp:BoundColumn DataField="title" HeaderText="内容" ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" CssClass="scdl" />
                    </asp:BoundColumn>

                    <asp:BoundColumn DataField="name" HeaderText="担当者名" ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" CssClass="scdl" />
                    </asp:BoundColumn>

                    <asp:BoundColumn DataField="SdlNo" HeaderText="管理番号" ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" CssClass="scdl" />
                    </asp:BoundColumn>

                    <asp:ButtonColumn
                        ItemStyle-Width="100px"
                        HeaderText="削除"
                        ButtonType="LinkButton"
                        Text="削除"
                        CommandName="Delete" />

                    <asp:EditCommandColumn
                        HeaderText="編集"
                        ItemStyle-Width="100px"
                        EditText="編集"
                        CancelText="キャンセル"
                        UpdateText="保存" ButtonType="LinkButton"></asp:EditCommandColumn>

                </Columns>

                <HeaderStyle Width="200px" BackColor="#16BA00" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="12px"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="15px" />
            </asp:DataGrid>
        </div>

        <br />




        <script>

            function A() {

                return confirm('本当に実行しますか？');

            }

        </script>
    </form>
</body>
</html>
