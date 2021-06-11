<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PIchiran.aspx.cs" Inherits="WhereEver.Project_System.PIchiran" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="../MenuControl.css" type="text/css" rel="stylesheet" />
    <link href="PIchiran.css" type="text/css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <title>プロジェクト一覧</title>
    
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

            <div>
                <table class="xxx">
                    <tr>
                        <td class="datagrid" colspan="4">

                            <asp:DataGrid runat="server" ID="DgPIchiran" 
                                OnItemDataBound="DgPIchiran_ItemDataBound"
                                OnEditCommand="DgPIchiran_EditCommand"
                                OnCancelCommand="DgPIchiran_CancelCommand"
                                OnUpdateCommand="DgPIchiran_UpdateCommand"
                                OnItemCommand="DgPIchiran_ItemCommand"
                                AutoGenerateColumns="False" Width="100%" CssClass="DgPIchiran">
                                    <Columns>
                    
                                        <asp:BoundColumn HeaderText="プロジェクトID(変更✖)" DataField="Pid" ReadOnly="True"/>
                                        <asp:BoundColumn HeaderText="プロジェクト名" DataField="Pname"/>
                                        <asp:BoundColumn HeaderText="取引先" DataField="Pcustomer"/>
                                        <asp:BoundColumn HeaderText="担当者" DataField="Presponsible"/>
                                        <asp:BoundColumn HeaderText="カテゴリー" DataField="Pcategory"/>
                                        <asp:BoundColumn HeaderText="開始日" DataField="PstartTime"/>
                                        <asp:BoundColumn HeaderText="終了日" DataField="PoverTime"/>
                                        <asp:EditCommandColumn EditText="<font color = red>変更" CancelText="<font color = red>キャンセル" UpdateText="<font color = red>保存"></asp:EditCommandColumn>
                                        <asp:ButtonColumn ButtonType="LinkButton" Text="<font color = red>削除" CommandName="Delete"/>
                                        <asp:ButtonColumn HeaderText="詳細" ButtonType="LinkButton" Text="<font color = red>編集(WBS図)" CommandName="wbs"/>

                                    </Columns>
                                <HeaderStyle Width="200px" Height="50px" HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Width="200px" Height="30px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td class="cell">

                            <asp:Label ID="lblNewPName" CssClass="lbl" runat ="server" Text="プロジェクト名"></asp:Label>

                        </td>
                        <td class="cell">

                            <asp:TextBox ID="txtNewPName" CssClass="txt" runat="server" ValidationGroup="new"></asp:TextBox>

                            <asp:RequiredFieldValidator ID="rfvProject" runat="server" ControlToValidate="txtNewPName" Display="None" ErrorMessage="プロジェクト名を入力してください" ValidationGroup="new"></asp:RequiredFieldValidator>

                        </td>
                        <td class="cell">

                            <asp:Label ID="lblNewCustomer" CssClass="lbl" runat="server" Text="取引先名"></asp:Label>

                        </td>
                        <td class="cell">

                            <asp:TextBox ID="txtNewCustomer" CssClass="txt" runat="server" ValidationGroup="new"></asp:TextBox>

                            <asp:RequiredFieldValidator ID="rfvNewCustomer" runat="server" ControlToValidate="txtNewCustomer" Display="None" ErrorMessage="取引先名を入力してください" ValidationGroup="new"></asp:RequiredFieldValidator>

                        </td>
                    </tr>
                    <tr>
                        <td class="cell">

                            <asp:Label ID="lblNewResponsible" CssClass="lbl" runat="server" Text="担当者"></asp:Label>

                        </td>
                        <td class="cell">

                            <asp:DropDownList ID="ddlResponsible" CssClass="txt" runat="server" Font-Size="Larger" ValidationGroup="new">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>

                            <asp:RequiredFieldValidator ID="rfvResponsible" runat="server" ControlToValidate="ddlResponsible" Display="None" EnableViewState="False" ErrorMessage="担当者を選択してください" ValidationGroup="new"></asp:RequiredFieldValidator>

                        </td>
                        <td class="cell">

                            <asp:Label ID="lblNewCategory" CssClass="lbl" runat="server" Text="カテゴリー"></asp:Label>

                        </td>
                        <td class="cell">

                            <asp:TextBox ID="txtNewCategory" CssClass="txt" runat="server" ValidationGroup="new"></asp:TextBox>

                            <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="txtNewCategory" Display="None" ErrorMessage="カテゴリーを入力してください" ValidationGroup="new"></asp:RequiredFieldValidator>

                        </td>
                    </tr>
                    <tr>
                    <td class="cell">

                        <asp:Label ID="lblNewStartTime" runat="server" Text="開始日"></asp:Label>

                        </td>
                    <td class="cell">

                        <input id="date1" runat="server" min="2018-01-01" type="date" class="txt"/></td>
                    <td class="cell">

                        <asp:Label ID="lblNewOverTime" runat="server" Text="終了日"></asp:Label>

                        </td>
                    <td class="cell">

                        <input id="date2" runat="server" type="date" min="2018-01-01" class="txt"/></td>
                    <td class="cell">

                        &nbsp;</td>
                </tr>
                    <tr>
                    <td class="cell">

                        <asp:Label ID="lblStartM" runat="server" ForeColor="Black" Text="（必須）"></asp:Label>
                        </td>
                    <td class="cell">

                        &nbsp;</td>
                    <td class="cell" colspan="2">
                        &nbsp;終了日未定の場合はそのまま保存してください。</td>
                    <td class="cell">

                        &nbsp;</td>
                </tr>
                    <tr>
                    <td class="cell" colspan="4">

                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ForeColor="Red" ValidationGroup="new" />

                    </td>
                </tr>
                <tr>
                    <td class="cell" colspan="4">

                        <asp:Button ID="btnNewP" CssClass="btn" runat="server" Text="新規として保存" OnClick="btnNewP_Click" ValidationGroup="new"/>

                        <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="クリア" OnClick="btnClear_Click" ValidationGroup="clear" />

                    </td>
                </tr>
                </table>
        </div>
            </div>
    </form>
</body>
</html>
