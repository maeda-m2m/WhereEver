﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PIchiran.aspx.cs" Inherits="WhereEver.Project_System.PIchiran" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="PIchiran.css" type="text/css" rel="stylesheet" />
    <title></title>
    
    </head>
<body>
    <form id="All" runat="server">
        <div class="form">
            <table>
                <tr class="All">
                    <td>
                        <Menu:c_menu ID="m" runat="server"></Menu:c_menu>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table>
                <tr>
                    <td class="datagrid" colspan="4">

            <asp:DataGrid runat="server" ID="DgPIchiran" Width="100%"
                OnItemDataBound="DgPIchiran_ItemDataBound"
                OnEditCommand="DgPIchiran_EditCommand"
           OnCancelCommand="DgPIchiran_CancelCommand"
           OnUpdateCommand="DgPIchiran_UpdateCommand"
           OnItemCommand="DgPIchiran_ItemCommand"
                
           AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundColumn HeaderText="プロジェクトID(変更✖)" 
                        DataField="Pid"/>
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

                        <asp:TextBox ID="txtNewPName" CssClass="txt" runat="server"></asp:TextBox>

                    </td>
                    <td class="cell">

                        <asp:Label ID="lblNewCustomer" CssClass="lbl" runat="server" Text="顧客名"></asp:Label>

                    </td>
                    <td class="cell">

                        <asp:TextBox ID="txtNewCustomer" CssClass="txt" runat="server"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td class="cell">

                        <asp:Label ID="lblNewResponsible" CssClass="lbl" runat="server" Text="担当者"></asp:Label>

                    </td>
                    <td class="cell">

                        <asp:DropDownList ID="ddlResponsible" CssClass="txt" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>張</asp:ListItem>
                            <asp:ListItem>柳沢</asp:ListItem>
                            <asp:ListItem>坂口</asp:ListItem>
                            <asp:ListItem>鯉淵</asp:ListItem>
                            <asp:ListItem>前田</asp:ListItem>
                        </asp:DropDownList>

                    </td>
                    <td class="cell">

                        <asp:Label ID="lblNewCategory" CssClass="lbl" runat="server" Text="カテゴリー(△)"></asp:Label>

                    </td>
                    <td class="cell">

                        <asp:TextBox ID="txtNewCategory" CssClass="txt" runat="server"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td class="cell" colspan="2">

                        <asp:Label ID="lblNewStartTime" runat="server" Text="開始日"></asp:Label>

                <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="350px" >
                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                    <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                    <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                    <TodayDayStyle BackColor="#CCCCCC" />
                        </asp:Calendar>

                    </td>
                    <td class="cell" colspan="2">

                        <asp:Label ID="lblNewOverTime" runat="server" Text="終了日"></asp:Label>

                <asp:Calendar ID="Calendar2" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="350px">
                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                    <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                    <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                    <TodayDayStyle BackColor="#CCCCCC" />
                        </asp:Calendar>

                        未定の場合はそのまま保存してください。</td>
                    <td class="cell">

                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="cell" colspan="4">

                        <asp:Button ID="btnNewP" CssClass="btn" runat="server" Text="新規として保存" OnClick="btnNewP_Click"/>

                        <asp:Label ID="lblAisatu" CssClass="label" runat="server"></asp:Label>

                        <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="クリア" OnClick="btnClear_Click" />

                    </td>
                </tr>
                </table>
        </div>
    </form>
</body>
</html>
