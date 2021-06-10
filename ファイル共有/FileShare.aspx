<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileShare.aspx.cs" Inherits="WhereEver.FileShare" %>
<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="Content-Type" content="application/json; charset=utf-8"/>
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../MenuControl.css" />
    <link rel="stylesheet" type="text/css" href="FileShare.css" />

    <title>ファイル共有</title>
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
       </div>

       <div id="Wrap">
      

           <p>
            <asp:Label ID="lblResult" runat="server" Text="ファイルをアップロードできます。ファイル名はUUIDに自動変換されます。タイムアウトしない限り、容量無制限でストリーミングアップロードできます。"></asp:Label>
           </p>

           <p class ="form-flat-border">
            <span class="f-notice">[アップロードファイル]</span>　<asp:FileUpload ID="FileUpload_userfile" runat="server" Width="485px"  CssClass="form-flat-border-inner" />
            <span class="f-notice">[パスワード]</span>　<asp:TextBox ID="TextBox_UploadPass" runat="server" CssClass="form-flat-border-inner" Width="100px" MaxLength="20" placeholder="未設定"></asp:TextBox>
            <asp:Button ID="Button_Upload" runat="server" Text="アップロード" OnClick="Button_UpLoad" CssClass="btn-flat-border" /><br />
            <span class="f-notice">[コメント]</span>　<asp:TextBox ID="TextBox_Upload_Comment" runat="server" CssClass="form-flat-border-inner" Width="896px" MaxLength="40" placeholder="ファイルの説明　なければ「無題」"></asp:TextBox>
            <asp:CheckBox ID="CheckBox_Annonimas" runat="server" Text="[匿名]" CssClass="f-notice" />
           </p>

           <%-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// --%>
           <hr />
           <%-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// --%>

           <p>
            <asp:Label ID="lblDLResult" runat="server" Text="ファイルをストリーミングダウンロードできます。" ValidateRequestMode="Disabled"></asp:Label>
           </p>

           <asp:Panel ID="Panel1" runat="server" DefaultButton="Button_Download">
            <p class ="form-flat-border">
            <span class="f-notice">[ダウンロードファイル]</span>　<asp:TextBox ID="TextBox_dl" runat="server" Width="485px" CssClass="form-flat-border-inner" placeholder="テーブルの「参照」ボタンを押して下さい"></asp:TextBox>
            <span class="f-notice">[パスワード]</span>　<asp:TextBox ID="TextBox_DownloadPass" runat="server" CssClass="form-flat-border-inner" Width="100px" MaxLength="20" placeholder="未設定"></asp:TextBox>
                <asp:Button ID="Button_Download" runat="server" Text="ダウンロード" OnClick="Button_DownLoad" CssClass="btn-flat-border" PostBackUrl="#" />
                <asp:Button ID="Button_Preview" runat="server" Text="プレビュー" OnClick="Button_PreView" CssClass="btn-flat-border" PostBackUrl="#" />
            </p>
           </asp:Panel>

           <hr />

            <p class ="form-flat-border">
                [一度の転送量]　<asp:RadioButton ID="RadioButton_Streaming_8000" runat="server" GroupName="Streaming" Checked="true" Text="8000Byte" /><asp:RadioButton ID="RadioButton_Streaming_20000" runat="server" GroupName="Streaming" Checked="false" Text="20000Byte" /><asp:RadioButton ID="RadioButton_Streaming_30000" runat="server" GroupName="Streaming" Checked="false" Text="30000Byte" /><asp:RadioButton ID="RadioButton_Streaming_40000" runat="server" GroupName="Streaming" Checked="false" Text="40000Byte" />　※ファイルサイズに応じて使い分けて下さい。
            </p>

           <hr />

           <p>～共有ファイル一覧～</p>

           <div class ="btn-wrap">
               <p><asp:Button ID="Button_DataBind" runat="server" Text="一覧更新" OnClick="Push_DataBind" CssClass="btn-flat-border" /></p>
           </div>

           <hr />

           <div>
              <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="id,FileName" DataSourceID="SqlDataSource1" CssClass="DGTable" OnRowCommand="grid_RowCommand" AllowPaging="True" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="id" ReadOnly="True" SortExpression="id" Visible="false" />
                    <asp:BoundField DataField="userName" HeaderText="投稿者" ReadOnly="True" SortExpression="userName" />
                    <asp:BoundField DataField="FileName" HeaderText="ファイル名" ReadOnly="True" SortExpression="FileName" />
                    <asp:BoundField DataField="Title" HeaderText="コメント" ReadOnly="True" SortExpression="Title" />
                    <asp:BoundField DataField="DateTime" HeaderText="アップロード日" ReadOnly="True" SortExpression="DateTime" />
                    <asp:BoundField DataField="size" HeaderText="サイズ" ReadOnly="True" SortExpression="size" />
                    <asp:BoundField DataField="isPass" HeaderText="パスワード" ReadOnly="True" SortExpression="isPass" />

                    <asp:ButtonField ButtonType="Button" Text="削除" ControlStyle-CssClass="btn-flat-border"  HeaderText="削除" CommandName="Remove" CausesValidation="False" >
                    <ControlStyle CssClass="btn-flat-border" />
                    </asp:ButtonField>

                    <asp:ButtonField ButtonType="Button" Text="参照" ControlStyle-CssClass="btn-flat-border"  HeaderText="ダウンロード" CommandName="DownLoad" CausesValidation="False" >
                    <ControlStyle CssClass="btn-flat-border" />
                    </asp:ButtonField>

                </Columns>
                <HeaderStyle BackColor="#1E1E1E" ForeColor="White" />
                  <RowStyle BackColor="Gray" ForeColor="White" />
            </asp:GridView>

             <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                ConnectionString="<%$ ConnectionStrings:WhereverConnectionString %>"
                SelectCommand="SELECT [id],[userName],[FileName],[Title],[DateTime],[size],[IsPass] FROM [T_FileShare] ORDER BY DateTime DESC">
                <SelectParameters>
                    <asp:ControlParameter ControlID="lblid" DefaultValue="null" Name="id" PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
           </div>

           <p>
            <asp:Label ID="lblid" runat="server" Text="null" Visible="False"></asp:Label>
           </p>
           <p>
            <asp:Label ID="lbluid" runat="server" Text="null" Visible="False"></asp:Label>
           </p>

           <hr />

        </div>
    </form>
</body>
</html>
