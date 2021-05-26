<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Kanri.aspx.cs" Inherits="WhereEver.管理ページ.Kanri" %>
<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../MenuControl.css" />
    <link rel="stylesheet" type="text/css" href="Kanri.css" />

    <title>管理ページ</title>

</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div>
            <table>
                <tr>
                    <td id="menu">
                        <menu:c_menu id="m" runat="server"></menu:c_menu>

                    </td>
                </tr>
            </table>

        <div id="Wrap">
          <span class="hr"></span>
           <p class="index1">◆ユーザー情報を変更できます。</p>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSource1" CssClass="DGTable" OnRowUpdated="grid_RowUpdatedCommand" OnRowCommand="grid_RowCommand">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="id" SortExpression="id" ReadOnly="true" />
                    <asp:BoundField DataField="name" HeaderText="Name" SortExpression="name" Visible="False" ReadOnly="true" />
                    <asp:BoundField DataField="name1" HeaderText="お名前（全角20文字まで）" SortExpression="name1" />
                    <asp:BoundField DataField="pw" HeaderText="パスワード（半角英数10文字まで）" SortExpression="pw" />
                    <asp:CommandField ShowEditButton="True" ButtonType="Button" ControlStyle-CssClass="btn-flat-border" HeaderText="編集">
                    <ControlStyle CssClass="btn-flat-border"></ControlStyle>
                    </asp:CommandField>
                </Columns>
                <HeaderStyle BackColor="#1E1E1E" ForeColor="White" />
                <RowStyle BackColor="Gray" ForeColor="White" />
            </asp:GridView>



            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSource1" CssClass="DGTable" Visible="False">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="id" SortExpression="id" ReadOnly="true" />
                    <asp:BoundField DataField="name" HeaderText="name" SortExpression="name" Visible="False" ReadOnly="true" />
                    <asp:BoundField DataField="name1" HeaderText="name1" SortExpression="name1"  ReadOnly="true" />
                    <asp:BoundField DataField="pw" HeaderText="pw" SortExpression="pw"  ReadOnly="true" />
                </Columns>
            </asp:GridView>



            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                ConnectionString="<%$ ConnectionStrings:WhereverConnectionString %>"
                SelectCommand="SELECT [id], [name], [name1], [pw] FROM [M_User] WHERE ([id] = @id)"
                UpdateCommand="UPDATE [M_User] SET [pw] = @pw, [name1] = @name1 WHERE ([id] = @id)">
                <UpdateParameters>
                       <asp:ControlParameter Name="id" ControlId="lblResult" PropertyName="Text"/>
                </UpdateParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="lblResult" DefaultValue="null" Name="id" PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>



           <p>
            <asp:Label ID="lblResult" runat="server" Text="null" Visible="False"></asp:Label>
           </p>

            
                  <div id="Edit">
                      <span class="hr"></span>
                       <a name="edittop"><p class="index1"> ◆メール送信システム</p></a>
                        <div class="article">
                           <p>本文の文字数制限なし（HTMLタグは使用できません。空白と改行は反映されます）</p>
                    [To:]<br />
                    <asp:TextBox ID="TextBox_MailTo" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" Height="16px" Width="100%" Style="resize: none" Text="" placeholder="xxxxxx@m2m-asp.com　入力必須" CausesValidation="false"></asp:TextBox><br />
                    [CC:]<br />
                    <asp:TextBox ID="TextBox_CC" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" Height="16px" Width="100%" Style="resize: none" Text="" placeholder="xxxxxx@m2m-asp.com　省略可" CausesValidation="false"></asp:TextBox><br />
                    [BCC:]<br />
                    <asp:TextBox ID="TextBox_BCC" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" Height="16px" Width="100%" Style="resize: none" Text="" placeholder="xxxxxx@m2m-asp.com　省略可" CausesValidation="false"></asp:TextBox><br />
                    [件名:]<br />
                    <asp:TextBox ID="TextBox_Title" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" Height="16px" Width="100%" Style="resize: none" Text="" placeholder="件名　入力推奨" CausesValidation="false"></asp:TextBox><br />
                    [本文]<br />
                    <asp:TextBox ID="TextBox_EditTop" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" TextMode="MultiLine" Rows="3" Height="300px" Width="100%" Style="resize: none" Text="" placeholder="メール本文　文字数無制限　空入力可" CausesValidation="false"></asp:TextBox>
                    [SMTP]<br />
                            <asp:DropDownList ID="DropDownList_SMTP" runat="server" AutoPostBack="true" Width="50%">
                                <asp:ListItem>mail.m2m-asp.com</asp:ListItem>
                                <asp:ListItem>192.168.2.156</asp:ListItem>
                                <asp:ListItem>手動</asp:ListItem>
                            </asp:DropDownList>

                    <asp:Panel ID="Panel_UserSMTP" runat="server" Visible="false">
                    [SMTP HOST]<br />
                    <asp:TextBox ID="TextBox_Host" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" Height="16px" Width="50%" Style="resize: none" Text="" placeholder="SMTP HOST" CausesValidation="false"></asp:TextBox><br />
                    [SMTP PORT]<br />
                    <asp:TextBox ID="TextBox_Port" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" Height="16px" Width="50%" Style="resize: none" Text="" placeholder="SMTP Port" CausesValidation="false" TextMode="Number"></asp:TextBox><br />
                    [ユーザー名（ユーザー認証）]<br />
                    <asp:TextBox ID="TextBox_UserName" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" Height="16px" Width="50%" Style="resize: none" Text="" placeholder="任意" CausesValidation="false"></asp:TextBox><br />
                    [パスワード（ユーザー認証）]<br />
                    <asp:TextBox ID="TextBox_UserPass" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" Height="16px" Width="50%" Style="resize: none" Text="" placeholder="任意" CausesValidation="false"></asp:TextBox><br />
                    </asp:Panel>
                            <asp:CheckBox ID="CheckBox_Annonimas" runat="server" Text="匿名" />

                                                 <p><asp:Label ID="Label_MailTo_Result" runat="server" Text="Ready..."></asp:Label></p>

                        </div>

                    <div class="center">
                        <asp:Button ID="Button_ReformTop" runat="server" CssClass="btn_loginlist" Text="送信" OnClick="btnReformTop_Click" />
                        <asp:Button ID="Button_ReformDelete" runat="server" CssClass="btn_loginlist" Text="全消去" OnClick="btnReformTopDel_Click" PostBackUrl="#edittop" />
                    </div>
                  </div>

           <div id="TestSystem">
            <span class="hr"></span>
            <p class="index1"> ◆開発または研究中の機能</p>
            <div class="center">
               <asp:Button ID="Button_Video" runat="server" CssClass="btn_loginlist" Text="試作機能を試す" OnClick="btnVideo_Click" />
            </div>
           </div>

        </div><%-- Wrap --%>


        </div>
    </form>
</body>
</html>
