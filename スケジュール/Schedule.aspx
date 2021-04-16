<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="WhereEver.Schedule" EnableEventValidation="false" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Schedule.css" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../MenuControl.css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>



</head>
<body>
    <form id="form1" runat="server">
        <table>
            <tr>
                <td id="menu">
                    <Menu:c_menu ID="m" runat="server"></Menu:c_menu>
                </td>
            </tr>
        </table>


        <asp:Label ID="Label6" runat="server" Text=""></asp:Label>
        <div>
            <asp:Panel ID="Panel1" runat="server">
                <asp:Calendar ID="Calendar1" runat="server" OnSelectionChanged="Calendar1_SelectionChanged" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="350px" OnInit="CalendarA" OnLoad="CalendarA">
                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                    <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                    <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                    <TodayDayStyle BackColor="#CCCCCC" />
                </asp:Calendar>
                <table id="tourokuform">
                    <tr>
                        <td runat="server" class="auto-style1">
                            <p>日付</p>
                            <td class="auto-style7">
                                <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td runat="server" class="auto-style1">
                            <p>時間</p>
                            <td class="auto-style7">
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
                        </td>
                    </tr>
                    <tr>
                        <td runat="server" class="Title">
                            <p>内容</p>
                            <td class="auto-style8">
                                <asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td runat="server" class="Title">

                            <p>担当者名</p>

                            <td class="auto-style8">

                                <asp:CheckBoxList ID="CheckBoxList1" runat="server">
                                    <asp:ListItem></asp:ListItem>
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
                                </asp:CheckBoxList>

                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="Button2" runat="server" Text="登録" class="btn-flat-border" OnClick="Button2_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>

        <div>
            <asp:DataGrid runat="server"
                ID="Scdl3"
                AutoGenerateColumns="False"
                OnItemDataBound="Scdl3_ItemDataBound"
                CssClass="scdl" HeaderStyle-Width="200px" OnSelectedIndexChanged="Scdl3_SelectedIndexChanged" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" Width="1200px" Style="margin-left: 0px">
                <AlternatingItemStyle BackColor="#ccffcc" />
                <Columns>
                    <asp:TemplateColumn HeaderText="時間" HeaderStyle-Width="" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
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
        <table id="btn" class="auto-style12">


            <td class="auto-style11">
                <asp:Panel ID="Panel2" runat="server" CssClass="auto-style9" Height="10px" Width="1205px">
                    <asp:Button ID="Button3" runat="server" Text="登録" class="btn-flat-border" OnClick="Button3_Click" />
                    &nbsp;<asp:Button ID="Button1" runat="server" class="btn-flat-border" OnClick="Button1_Click1" Text="印刷（未完成）" />
                    <input type="button" class="btn-flat-border" value="印刷" onclick="window.print();" />
                </asp:Panel>
            </td>


        </table>

        <div>

            <asp:DataGrid
                runat="server"
                ID="ScdlList"
                AutoGenerateColumns="false"
                OnEditCommand="ScdlList_EditCommand"
                OnCancelCommand="ScdlList_CancelCommand"
                OnUpdateCommand="ScdlList_UpdateCommand"
                OnItemDataBound="ScdlList_ItemDataBound"
                CssClass="scdl"
                HeaderStyle-Width="200px"
                OnItemCommand="ScdlList_ItemCommand"
                Style="margin-left: 0px"
                HorizontalAlign="Left" Width="1200px" OnSelectedIndexChanged="ScdlList_SelectedIndexChanged">

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
                        UpdateText="保存"></asp:EditCommandColumn>

                </Columns>

                <HeaderStyle Width="200px" BackColor="#16BA00" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="12px"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="15px" />
            </asp:DataGrid>
        </div>

        <br />
    </form>
</body>
</html>
