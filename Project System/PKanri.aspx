<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PKanri.aspx.cs" Inherits="WhereEver.Project_System.PKanri" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="auto-style1">
                <tr>
                    <td>
                        <asp:DataGrid ID="DgPDetail" runat="server" AutoGenerateColumns="False" OnCancelCommand="DgPIchiran_CancelCommand" OnEditCommand="DgPIchiran_EditCommand" OnItemCommand="DgPIchiran_ItemCommand" OnItemDataBound="DgPIchiran_ItemDataBound" OnUpdateCommand="DgPIchiran_UpdateCommand" Width="100%">
                            <Columns>
                                <asp:BoundColumn DataField="Pid" HeaderText="プロジェクトID(変更✖)" />
                                <asp:BoundColumn DataField="Pname" HeaderText="プロジェクト名" />
                                <asp:BoundColumn DataField="Pcustomer" HeaderText="取引先" />
                                <asp:BoundColumn DataField="Presponsible" HeaderText="担当者" />
                                <asp:BoundColumn DataField="Pcategory" HeaderText="カテゴリー" />
                                <asp:BoundColumn DataField="PstartTime" HeaderText="開始日" />
                                <asp:BoundColumn DataField="PoverTime" HeaderText="終了日" />
                                <asp:EditCommandColumn CancelText="キャンセル" EditText="変更" UpdateText="保存"></asp:EditCommandColumn>
                                <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="削除" />
                            </Columns>
                            <HeaderStyle Height="50px" HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                            <ItemStyle Height="30px" HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                        </asp:DataGrid>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
