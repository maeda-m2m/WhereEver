<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tatekae.ascx.cs" Inherits="WhereEver.Tatekae" %>


<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<div>
    <div>
        <table runat="server" id="MitumoriInput">
            <tr>
                <td class="InputTitle" runat="server">
                    <p>見積NO.</p>
                    <td>
                        <asp:Label ID="Label94" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td class="MiniTitle">
                        <p>カテゴリー</p>
                        <td>
                            <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="SqlDataSource3" DataTextField="CategoryName" DataValueField="CategoryName"></asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:MMC_TestConnectionString %>" SelectCommand="SELECT [CategoryName] FROM [M_Category]"></asp:SqlDataSource>
                            <td class="MiniTitle">
                                <p>部門</p>
                                <td>
                                    <asp:DropDownList ID="DropDownList3" runat="server" DataSourceID="SqlDataSource4" DataTextField="Busyo" DataValueField="Busyo" AutoPostBack="True"></asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:MMC_TestConnectionString %>" SelectCommand="SELECT [Busyo] FROM [M_Bumon]"></asp:SqlDataSource>
                                    <td class="MiniTitle">
                                        <p>担当者</p>
                                        <td>
                                            <asp:DropDownList ID="DropDownList4" runat="server" DataSourceID="SqlDataSource5" DataTextField="UserName" DataValueField="UserName" AutoPostBack="True"></asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:MMC_TestConnectionString %>" SelectCommand="SELECT [UserName] FROM [M_Tanto]"></asp:SqlDataSource>
                                            <td class="MiniTitle">
                                                <p>照会日付</p>
                                                <td>
                                                    <telerik:raddatepicker id="RadDatePicker1" runat="server"></telerik:raddatepicker>
                                                    <td class="MiniTitle">
                                                        <p>見積日付</p>
                                                        <td>
                                                            <telerik:raddatepicker id="RadDatePicker2" runat="server"></telerik:raddatepicker>
                                                            <td class="MiniTitle">
                                                                <p>リレー状況</p>
                                                                <td>
                                                                    <asp:Label ID="Label95" runat="server" Text="Label"></asp:Label>
                                                                </td>
                                                            </td>
                                                        </td>
                                                    </td>
                                                </td>
                                            </td>
                                        </td>
                                    </td>
                                </td>
                            </td>
                        </td>
                    </td>
                </td>
            </tr>
            <tr>
                <td class="InputTitle" runat="server">
                    <p>得意先</p>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="得意先詳細" CssClass="Btn2" />
                        <td colspan="4">kuuhaku
                        </td>
                        <td class="MiniTitle">
                            <p>仮宛先</p>
                            <asp:CheckBox ID="CheckBox1" runat="server" />
                            <td colspan="3">
                                <td class="MiniTitle">
                                    <p>掛け率</p>
                                    <td>
                                        <asp:Label ID="Label96" runat="server" Text="Label"></asp:Label>
                                        <td class="MiniTitle">
                                            <p>税区分</p>
                                            <td>
                                                <asp:DropDownList ID="DropDownList5" runat="server">
                                                    <asp:ListItem>税込</asp:ListItem>
                                                    <asp:ListItem>税抜</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </td>
                                    </td>
                                </td>
                            </td>
                        </td>
                    </td>
                </td>
            </tr>
            <tr>
                <td class="InputTitle" runat="server">
                    <p>請求先</p>
                    <td>
                        <asp:Button ID="Button2" runat="server" Text="請求先詳細" CssClass="Btn2" />
                        <td colspan="4">kuuhaku
                        </td>
                        <td class="MiniTitle">
                            <p>仮宛先</p>
                            <asp:CheckBox ID="CheckBox2" runat="server" />
                            <td colspan="3">
                                <td class="MiniTitle">
                                    <p>締め日</p>
                                    <td>
                                        <asp:Label ID="Label97" runat="server" Text="Label"></asp:Label>
                                        <td class="MiniTitle">
                                            <p>数量合計</p>
                                            <td class="sisan">
                                                <asp:Label ID="Label98" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        </td>
                                    </td>
                                </td>
                            </td>
                        </td>
                    </td>
                </td>
            </tr>
            <tr>
                <td class="InputTitle" runat="server">
                    <p>納品先</p>
                    <td>
                        <asp:Button ID="Button3" runat="server" Text="納品先詳細" CssClass="Btn2" />
                        <td colspan="4">kuuhaku
                        </td>
                        <td class="MiniTitle">
                            <p>仮宛先</p>
                            <asp:CheckBox ID="CheckBox3" runat="server" />
                            <td colspan="3">
                                <td class="MiniTitle">
                                    <p>売上計</p>
                                    <td class="sisan">
                                        <asp:Label ID="Label99" runat="server" Text="Label"></asp:Label>
                                        <td class="MiniTitle">
                                            <p>仕入計</p>
                                            <td class="sisan">
                                                <asp:Label ID="Label100" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        </td>
                                    </td>
                                </td>
                            </td>
                        </td>
                    </td>
                </td>
            </tr>
            <tr>
                <td class="InputTitle" runat="server">
                    <p>使用施設名</p>
                    <td>
                        <asp:Button ID="Button4" runat="server" Text="複数" CssClass="Btn2" />
                        <td colspan="2">kuuhaku
                        </td>
                        <td class="MiniTitle">
                            <p>使用期間</p>
                            <td>
                                <asp:Button ID="Button5" runat="server" Text="複数" CssClass="Btn2" />
                                <td class="MiniTitle">
                                    <p>開始</p>
                                    <td>
                                        <telerik:raddatepicker id="RadDatePicker3" runat="server"></telerik:raddatepicker>
                                        <td class="MiniTitle">
                                            <p>終了</p>
                                            <td>
                                                <telerik:raddatepicker id="RadDatePicker4" runat="server"></telerik:raddatepicker>
                                                <td class="MiniTitle">
                                                    <p>消費税額</p>
                                                    <td class="sisan">
                                                        <asp:Label ID="Label101" runat="server" Text="Label"></asp:Label>
                                                        <td class="MiniTitle">
                                                            <p>粗利計</p>
                                                            <td class="sisan">
                                                                <asp:Label ID="Label102" runat="server" Text="Label"></asp:Label>
                                                            </td>
                                                        </td>
                                                    </td>
                                                </td>
                                            </td>
                                        </td>
                                    </td>
                                </td>
                            </td>
                        </td>
                    </td>
                </td>
            </tr>
            <tr>
                <td class="InputTitle" runat="server">
                    <p>備考</p>
                    <td colspan="9">
                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                        <td class="MiniTitle">
                            <p>売上合計</p>
                            <td class="sisan">
                                <asp:Label ID="Label103" runat="server" Text="Label"></asp:Label>
                                <td class="MiniTitle">
                                    <p>利益率</p>
                                    <td class="sisan">
                                        <asp:Label ID="Label104" runat="server" Text="Label"></asp:Label>
                                    </td>
                                </td>
                            </td>
                        </td>
                    </td>
                </td>
            </tr>

        </table>
    </div>
