<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chat.aspx.cs" Inherits="WhereEver.Chat" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../MenuControl.css" type="text/css" rel="stylesheet" />
    <link href="Chat.css" type="text/css" rel="stylesheet" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
                            <asp:Label ID="Name" runat="server" Text="" ForeColor="#000080"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="200px">
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
                 <asp:TextBox ID="TextBox1" runat="server" Width="180px"></asp:TextBox>
                 &nbsp;&nbsp;<asp:Label ID="lblHenshin" runat="server" Text="返信モード"></asp:Label>
                    &nbsp;<asp:TextBox ID="txtHenshin" runat="server"></asp:TextBox>
                 <asp:Button ID="Send" runat="server" Text="⤴" CssClass="btn-flat-border" OnClick="Send_Click" />
                 <asp:Button ID="btnHenshin" runat="server" Text="⤴" CssClass="btn-flat-border" OnClick="btnHenshin_Click" />
                    <asp:Label ID="Label2" runat="server" Text="" ForeColor="Red" ></asp:Label>
                </p>
                <p>
                    <asp:TextBox ID="txtHozon" runat="server"></asp:TextBox>
                    <asp:Label ID="lbl" runat="server" Text="Label" Visible="False"></asp:Label>
                </p>
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
