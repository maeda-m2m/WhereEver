<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Kanri.aspx.cs" Inherits="WhereEver.管理ページ.Kanri" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
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


           <p>ユーザー情報を変更できます。</p>




           <p>
            <asp:Label ID="lblResult" runat="server" Text="ready..."></asp:Label>
           </p>


        </div>
    </form>
</body>
</html>
