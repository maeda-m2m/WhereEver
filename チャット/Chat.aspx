<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chat.aspx.cs" Inherits="WhereEver.Chat" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../MenuControl.css" type="text/css" rel="stylesheet" />
    <link href="Chat.css" type="text/css" rel="stylesheet" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <title>Mちゃんねる</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td id="menu">
                        <Menu:c_menu ID="m" runat="server"></Menu:c_menu>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <h2>Mちゃんねる</h2>
        </div>
        <div>
        <table>
            <asp:DataGrid runat="server" ID="ChatArea" AutoGenerateColumns="false" 
                OnItemDataBound="Chat_ItemDataBound"
                OnItemCommand="ChatArea_ItemCommand" >
                
                <Columns>

                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="50px">
                        <HeaderStyle Wrap="true"/>
                        <ItemTemplate>
                            <asp:Label ID="No" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                     <asp:TemplateColumn HeaderText="" ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />
                        <ItemTemplate>
                            <asp:Label ID="Id" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />
                        <ItemTemplate>
                            <asp:Label ID="Name" runat="server" Text="" ForeColor="Navy"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="500px">
                        <HeaderStyle Wrap="true" />
                        <ItemTemplate>
                            <asp:Label ID="Naiyou" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="100px" >
                        <HeaderStyle Wrap="true" />
                        <ItemTemplate>
                            <asp:Label ID="Date" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:ButtonColumn 
                         ButtonType="LinkButton" Text="削除"  CommandName="Delete" />
                    <asp:ButtonColumn 
                         ButtonType="LinkButton" Text="返信"  CommandName="Reply" />
                    
                </Columns>
            </asp:DataGrid>
            </table>
        </div>
        <div>

            <div id ="chatbox">
                <p>
                 <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                 <asp:TextBox ID="TextBox1" runat="server" Width="250px" Height="100px" TextMode="MultiLine" ValidationGroup="send"></asp:TextBox>
                 &nbsp;&nbsp;<asp:Label ID="lblHenshin" runat="server" Text="返信モード"></asp:Label>
                    &nbsp;<asp:TextBox ID="txtHenshin" runat="server" Width="250px" Height="100px" TextMode="MultiLine" ValidationGroup="hensin"></asp:TextBox>
                 <asp:Button ID="Send" runat="server" Text="⤴" CssClass="btn-flat-border" OnClick="Send_Click" ValidationGroup="send" />
                 <asp:Button ID="btnHenshin" runat="server" Text="⤴" CssClass="btn-flat-border" OnClick="btnHenshin_Click" ValidationGroup="hensin" />
                    <asp:Label ID="Label2" runat="server" Text="" ForeColor="Red" ></asp:Label>
                </p>
                <p>
                    <asp:TextBox ID="txtHozon" runat="server"></asp:TextBox>
                    <asp:Label ID="lbl" runat="server" Text="Label" Visible="False"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1" Display="None" ErrorMessage="内容を入力してください" ValidationGroup="send"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtHenshin" Display="None" ErrorMessage="返信内容を入力してください" ValidationGroup="hensin"></asp:RequiredFieldValidator>
                </p>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ForeColor="Red" ValidationGroup="send" />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ForeColor="Red" ValidationGroup="hensin" />
                <p>
                    <asp:Button ID="Update" runat="server" Text="更新" CssClass="Button-style" OnClick="Update_Click"/>
                &nbsp;&nbsp;
                    <asp:Button ID="Return" runat="server" Text="戻る" CssClass="Button-style" OnClick="Return_Click"/>
                </p>
           </div>
        </div>
    </form>
</body>
</html>
