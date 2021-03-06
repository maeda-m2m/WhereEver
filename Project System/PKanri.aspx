<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PKanri.aspx.cs" Inherits="WhereEver.Project_System.PKanri" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <link href="PKanri.css" type="text/css" rel="stylesheet" />
    <link href="../MenuControl.css" type="text/css" rel="stylesheet" />
    <title>プロジェクト管理(WBS)</title>

</head>
<body>
    <form id="form1" runat="server">
        <div id="Wrap">
            <table>
                <tr class="All">
                    <td>
                        <Menu:c_menu ID="m" runat="server"></Menu:c_menu>
                    </td>
                </tr>
            </table>
        </div>
        <div class="xxx">
            <table class="table">

                <tr>
                    <td class="auto-style2" colspan="3">
                        <asp:DataGrid ID="DgPKanri" runat="server"
                            AutoGenerateColumns="False"
                            OnItemDataBound="DgPKanri_ItemDataBound"
                            OnEditCommand="DgPKanri_EditCommand"
                            OnCancelCommand="DgPKanri_CancelCommand"
                            OnUpdateCommand="DgPKanri_UpdateCommand"
                            OnItemCommand="DgPKanri_ItemCommand"
                            Width="100%" CssClass="DgPKanri" Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                            <Columns>
                                <asp:BoundColumn DataField="PBigname" HeaderText="大項目" ReadOnly="True" HeaderStyle-Width="200px" />
                                <asp:TemplateColumn HeaderText="" HeaderStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:Button ID="uebig" Text="↑" runat="server" CommandName="uebig" CssClass="visibility_hidden" /><br>
                                        <asp:Button ID="sitabig" Text="↓" runat="server" CommandName="sitabig" CssClass="visibility_hidden" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="PMiddleid" HeaderText="中項目ID" ReadOnly="True" HeaderStyle-Width="10px" HeaderStyle-Font-Size="Small" ItemStyle-Width="10px" />
                                
                                <asp:TemplateColumn  HeaderText="中項目" HeaderStyle-Width="120px">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                </asp:TemplateColumn>

                                <asp:TemplateColumn HeaderText="進捗率" HeaderStyle-Width="120px">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddpShintyokuChange" runat="server" CssClass="ShintyokuChange">
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
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="" HeaderStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:Button ID="uemiddle" Text="↑" runat="server" CommandName="uemiddle" CssClass="visibility_hidden" /><br>
                                        <asp:Button ID="sitamiddle" Text="↓" runat="server" CommandName="sitamiddle" CssClass="visibility_hidden" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="開始" HeaderStyle-Width="130px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="StartYear" runat="server" CssClass="time" Width="30px"></asp:TextBox>-
                                            <asp:TextBox ID="StartMonth" runat="server" CssClass="time" Width="15px"></asp:TextBox>-
                                            <asp:TextBox ID="StartDay" runat="server" CssClass="time" Width="15px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateColumn>

                                <asp:TemplateColumn HeaderText="終了" HeaderStyle-Width="130px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="OverYear" runat="server" CssClass="time" Width="30px"></asp:TextBox>-
                                            <asp:TextBox ID="OverMonth" runat="server" CssClass="time" Width="15px"></asp:TextBox>-
                                            <asp:TextBox ID="OverDay" runat="server" CssClass="time" Width="15px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn ReadOnly="True" HeaderStyle-Width="40px" />
                                <asp:BoundColumn DataField="PTorokutime" HeaderText="最新編集日付" ReadOnly="True" HeaderStyle-Width="200px" />
                                <asp:BoundColumn DataField="PTorokusya" HeaderText="最新編集者" ReadOnly="True" HeaderStyle-Width="100px" />

                                <asp:EditCommandColumn EditText="<font color = red>編集" CancelText="<font color = red>取消" UpdateText="<font color = red>保存" HeaderStyle-Width="100px" HeaderText="編集用ボタン"></asp:EditCommandColumn>
                                <asp:ButtonColumn ButtonType="LinkButton" Text="<font color = red>削除" CommandName="Delete" HeaderStyle-Width="50px" />
                            </Columns>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:DataGrid>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">

                        <asp:Label ID="lblPBig" CssClass="txt" runat="server" Text="大項目登録"></asp:Label>
                        <asp:TextBox ID="txtPBig" CssClass="txt" runat="server"></asp:TextBox>
                        <asp:Button ID="btnToroku" CssClass="btn" runat="server" Text="大項目登録" OnClick="btnToroku_Click" ValidationGroup="Group01" />
                        <asp:RequiredFieldValidator ID="rfvBig" runat="server" ControlToValidate="txtPBig" Display="Dynamic" ErrorMessage="大項目名を入力してください" ForeColor="Red" ValidationGroup="Group01"></asp:RequiredFieldValidator>
                        <%--                        <asp:CustomValidator ID="cvBig" runat="server" ControlToValidate="txtPBig" Display="None" ErrorMessage="すでに登録済みの大項目名です" ForeColor="Red"></asp:CustomValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPBigList" CssClass="txt" runat="server" Text="大項目リスト"></asp:Label>
                        <asp:DropDownList ID="ddlPBigList" CssClass="txt" runat="server">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Button ID="btnDeleteBig" runat="server" OnClick="btnDeleteBig_Click" Text="大項目削除" />
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblPMiddle" CssClass="txt" runat="server" Text="中項目"></asp:Label>
                        <asp:TextBox ID="txtPMiddle" CssClass="txt" runat="server"></asp:TextBox>
                        <%--<asp:CustomValidator ID="cvMiddle" runat="server" ControlToValidate="txtPMiddle" Display="None" ErrorMessage="すでに登録済みの中項目名です" ForeColor="Red"></asp:CustomValidator>--%>
                        <asp:RequiredFieldValidator ID="rfvMiddle" runat="server" ControlToValidate="txtPMiddle" ErrorMessage="中項目名を入力してください" ForeColor="Red" ValidationGroup="Group02"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblAisatu1" CssClass="txt" runat="server" Text="を選択してから、中項目入力をお願い致します。"></asp:Label>
                    </td>
                    <td colspan="2" class="auto-style3">
                        <asp:Label ID="lblTime" CssClass="txt" runat="server" Text="日付選択"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td rowspan="2">&nbsp;</td>
                    <td>
                        <asp:Label ID="lblStart" CssClass="txt" runat="server" Text="開始"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblOver" CssClass="txt" runat="server" Text="終了"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="date1" runat="server" type="date" min="2018-01-01" /></td>
                    <td>
                        <input id="date2" runat="server" type="date" min="2018-01-01" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td colspan="2" class="auto-style2">
                        <asp:Label ID="lblCalendarError" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td colspan="2" class="auto-style2">
                        <asp:Label ID="lblShintyoku" runat="server" Text="進捗率"></asp:Label>
                        <asp:DropDownList ID="ddpShintyoku" runat="server">
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
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <asp:Label ID="lblJyunbanError" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                    <td colspan="2" class="auto-style2">
                        <asp:Button ID="btnPMiddle" CssClass="btn" runat="server" Text="中項目登録" OnClick="btnPMiddle_Click" ValidationGroup="Group02" Font-Overline="False" />
                        <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="クリア" CausesValidation="False" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="btn-wbs">
                        <asp:Button ID="btnWBS" CssClass="btn" runat="server" Text="WBS表一覧↓" OnClick="btnWBS_Click" />
                    </td>
                    <td colspan="2"></td>
                </tr>
            </table>
        </div>

        <div>

            <asp:DataGrid ID="wbs" runat="server"
                AutoGenerateColumns="False"
                OnItemDataBound="wbs_ItemDataBound" Width="100%">
                <Columns>
                    <asp:BoundColumn DataField="PBigname" HeaderText="大項目" />
                    <asp:BoundColumn DataField="PMiddlename" HeaderText="中項目" />
                </Columns>

                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="lblMonth" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="lblMonth" />
            </asp:DataGrid>

        </div>

    </form>
</body>
</html>
