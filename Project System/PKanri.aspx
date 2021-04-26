<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PKanri.aspx.cs" Inherits="WhereEver.Project_System.PKanri" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="PKanri.css" type="text/css" rel="stylesheet" />
    <title></title>
    
</head>
<body>
    <form id="form1" runat="server">
         <div>
            <table>
                <tr class="All">
                    <td>
                        <Menu:c_menu ID="m" runat="server"></Menu:c_menu>
                    </td>
                </tr>
            </table>
        </div>
        <div class="wbs">
            <table class="table">
                <tr>
                    <td colspan="3" class="auto-style1">
                        
                        <asp:Label ID="lblPBig" CssClass="txt" runat="server" Text="大項目登録"></asp:Label>
                        <asp:TextBox ID="txtPBig" CssClass="txt" runat="server"></asp:TextBox>
                        <asp:Button ID="btnToroku" CssClass="btn" runat="server" Text="大項目登録" OnClick="btnToroku_Click" ValidationGroup="Group01"/>
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
                    <td rowspan="2">
                        </td>
                    <td>
                        <asp:Label ID="lblStart" CssClass="txt" runat="server" Text="開始"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblOver" CssClass="txt" runat="server" Text="終了"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="date1" runat="server" type="date" min="2018-01-01"/></td>
                    <td>
                        <input id="date2" runat="server" type="date" min="2018-01-01"/></td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        </td>
                    <td colspan="2" class="auto-style2">
                        <asp:Button ID="btnPMiddle" CssClass="btn" runat="server" Text="中項目登録" OnClick="btnPMiddle_Click" ValidationGroup="Group02"/>
                        <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="クリア" CausesValidation="False" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td colspan="2" class="btn-wbs">
                        <asp:Button ID="btnWBS" CssClass="btn" runat="server" Text="WBS表一覧⇒" OnClick="btnWBS_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:DataGrid ID="DgPKanri" runat="server" 
                            AutoGenerateColumns="False" 
                            OnItemDataBound="DgPKanri_ItemDataBound" 
                            OnEditCommand="DgPKanri_EditCommand"
                            OnCancelCommand="DgPKanri_CancelCommand"
                            OnUpdateCommand="DgPKanri_UpdateCommand"
                            OnItemCommand="DgPKanri_ItemCommand"
                            Width="100%"
                            ItemStyle-CssClass="item">
                            <Columns>
                                <asp:BoundColumn DataField="PBigname" HeaderText="大項目" ReadOnly="True"/>
                                <asp:BoundColumn DataField="PMiddleid" HeaderText="中項目ID" ReadOnly="True"/>
                                <asp:BoundColumn DataField="PMiddlename" HeaderText="中項目"/>
                                <asp:BoundColumn DataField="PMiddlestart" HeaderText="開始"/>
                                <asp:BoundColumn DataField="PMiddleover" HeaderText="終了"/>
                                <asp:BoundColumn HeaderText="ステータス" ReadOnly="True"/>
                                <asp:BoundColumn DataField="PTorokutime" HeaderText="最新編集日付" ReadOnly="True"/>
                                <asp:BoundColumn DataField="PTorokusya" HeaderText="最新編集者" ReadOnly="True"/>
                                
                                <asp:EditCommandColumn EditText="変更" CancelText="キャンセル" UpdateText="保存" ></asp:EditCommandColumn>
                                <asp:ButtonColumn ButtonType="LinkButton" Text="削除" CommandName="Delete"/>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"/>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Height="30px"/>
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>
        </div>
        <div class="wbs">

                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>

                        <asp:DataGrid ID="wbs" runat="server" 
                            AutoGenerateColumns="False" 
                            Width="100%">
                            <Columns>
                                <asp:BoundColumn DataField="No" HeaderText="No."/>
                                <asp:BoundColumn DataField="Bigname" HeaderText="大項目"/>
                                <asp:BoundColumn DataField="Middlename" HeaderText="中項目"/>
                                
                            </Columns>
                            <HeaderStyle />
                            <ItemStyle/>
                        </asp:DataGrid>

        </div>
    </form>
</body>
</html>
