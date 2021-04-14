<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileShare.aspx.cs" Inherits="WhereEver.FileShare" %>
<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="Content-Style-Type" content="text/css" />
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
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="id,uid" DataSourceID="SqlDataSource1">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="id" ReadOnly="True" SortExpression="id" />
                    <asp:BoundField DataField="uid" HeaderText="uid" ReadOnly="True" SortExpression="uid" />
                    <asp:BoundField DataField="name1" HeaderText="name1" SortExpression="name1" />
                    <asp:BoundField DataField="ShinseiSyubetsu" HeaderText="ShinseiSyubetsu" SortExpression="ShinseiSyubetsu" />
                    <asp:BoundField DataField="DateTime" HeaderText="DateTime" SortExpression="DateTime" />
                    <asp:BoundField DataField="LastUpdate" HeaderText="LastUpdate" SortExpression="LastUpdate" />
                    <asp:CommandField ShowEditButton="True" />
                </Columns>
            </asp:GridView>

            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:WhereverConnectionString %>" SelectCommand="SELECT * FROM [T_Shinsei_Main]"></asp:SqlDataSource>

           <p class ="form-flat-border">
            [アップロードするファイル(.zip)]　<asp:FileUpload ID="FileUpload_userfile" runat="server" Width="685px"  CssClass="form-flat-border" />
            <asp:Button ID="Button_Upload" runat="server" Text="アップロード" OnClick="Button_UpLoad" CssClass="btn-flat-border" />
           </p>
           <p>
             保存先：c:\\UploadedFiles\\[UUID].zip
           </p>
           <p>
            <asp:Label ID="lblResult" runat="server" Text="ファイルをアップロードできます。ファイル名はUUIDに自動変換されます。"></asp:Label>
           </p>

            <p class ="form-flat-border">
            [ダウンロードするファイル(.zip)]　c:\\UploadedFiles\\<asp:TextBox ID="TextBox_dl" runat="server" Width="600px" CssClass="form-flat-border" ></asp:TextBox>.zip　<asp:Button ID="Button_Download" runat="server" Text="ダウンロード" OnClick="Button_DownLoad" CssClass="btn-flat-border" />
            </p>
           <p>
             参照先：c:\\UploadedFiles\\[UUID].zip
           </p>
           <p>
            <asp:Label ID="lblDLResult" runat="server" Text="ファイルをダウンロードできます。"></asp:Label>
           </p>

        </div>
    </form>
</body>
</html>
