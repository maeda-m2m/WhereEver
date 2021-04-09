<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PIchiran.aspx.cs" Inherits="WhereEver.Project_System.PIchiran" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="../MenuControl.css" type="text/css" rel="stylesheet" />

    <title></title>
    
</head>
<body>
    <form id="form1" runat="server">
        <div class="form">
            <table>
                <tr>
                    <td id="menu">
                        <Menu:c_menu ID="m" runat="server"></Menu:c_menu>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table class="auto-style1">
                <tr>
                    <td class="auto-style2" colspan="5">

            <asp:DataGrid runat="server" ID="DgPIchiran" OnItemDataBound="DgPIchiran_ItemDataBound"
                OnEditCommand="DgPIchiran_EditCommand"
           OnCancelCommand="DgPIchiran_CancelCommand"
           OnUpdateCommand="DgPIchiran_UpdateCommand"
           OnItemCommand="DgPIchiran_ItemCommand"
                
           AutoGenerateColumns="False"
                BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2">
                <Columns>
                    <asp:BoundColumn HeaderText="プロジェクト名"
                        DataField="Pname"/>
                    <asp:BoundColumn HeaderText="取引先" 
                        DataField="Pcustomer"/>
                    <asp:BoundColumn HeaderText="担当者" 
                        DataField="Presponsible"/>
                    <asp:BoundColumn HeaderText="カテゴリー" 
                        DataField="Pcategory"/>
                    <asp:BoundColumn HeaderText="開始日" 
                        DataField="PstartTime"/>
                    <asp:BoundColumn HeaderText="終了日" 
                        DataField="PoverTime"/>
                    <asp:EditCommandColumn
                         EditText="変更"
                         CancelText="キャンセル"
                        UpdateText="保存" >

                    </asp:EditCommandColumn>
                    <asp:ButtonColumn 
                         ButtonType="LinkButton" 
                 Text="削除" 
                 CommandName="Delete"/>
                </Columns>

                <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />

                <HeaderStyle Width="200px" BackColor="#A55129" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" BackColor="#FFF7E7" ForeColor="#8C4510" />
                <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" Mode="NumericPages" />
                <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
            </asp:DataGrid>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">

                        <asp:Label ID="lblNewPName" runat="server" Text="プロジェクト名"></asp:Label>
                    </td>
                    <td class="auto-style2">

                        <asp:TextBox ID="txtNewPName" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style2">

                        <asp:Label ID="lblNewCustomer" runat="server" Text="顧客名"></asp:Label>
                    </td>
                    <td class="auto-style2" colspan="2">

                        <asp:TextBox ID="txtNewCustomer" runat="server" CssClass="auto-style1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">

                        <asp:Label ID="lblNewResponsible" runat="server" Text="担当者"></asp:Label>
                    </td>
                    <td class="auto-style3">

                        <asp:DropDownList ID="ddlResponsible" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>chou</asp:ListItem>
                            <asp:ListItem>yanagisawa</asp:ListItem>
                            <asp:ListItem>sakaguchi</asp:ListItem>
                            <asp:ListItem>koibuchi</asp:ListItem>
                            <asp:ListItem>maeda</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="auto-style3">

                        <asp:Label ID="lblNewCategory" runat="server" Text="カテゴリー(△)"></asp:Label>
                    </td>
                    <td class="auto-style3" colspan="2">

                        <asp:TextBox ID="txtNewCategory" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">

                        <asp:Label ID="lblNewStartTime" runat="server" Text="開始日"></asp:Label>
                    </td>
                    <td class="auto-style2">

                <asp:Calendar ID="Calendar1" runat="server" ></asp:Calendar>
                    </td>
                    <td class="auto-style2">

                        <asp:Label ID="lblNewOverTime" runat="server" Text="終了日"></asp:Label>
                    </td>
                    <td class="auto-style2">

                <asp:Calendar ID="Calendar2" runat="server"></asp:Calendar>
                    </td>
                    <td class="auto-style2">

                        <asp:Button ID="btnClear" runat="server" Text="クリア" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="auto-style1">

                        <asp:Button ID="btnNewP" runat="server" Text="新規として保存" OnClick="btnNewP_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="auto-style1">

                        <asp:Label ID="lblAisatu" runat="server"></asp:Label>
                    </td>
                    <td colspan="3" class="auto-style1">

                        &nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
