<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WhereEver.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="Login.css" type="text/css" rel="stylesheet" />


    <title>m2m ログインページ</title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
</head>
<body>
    <form runat="server">
        <div class ="bg_test">
            <table class="bg_test-text">
                <tr>
                    <td>
                        <table class="auto-style1">
                            <tr>
                                <td>
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/ログイン/m2m-logo.png" />
                                </td>
                            </tr>
                        </table>
                        <asp:Label ID="lblID" runat="server" Text="社員ID" Width="200px"></asp:Label>
                        <asp:TextBox ID="TbxID" runat="server"></asp:TextBox>

                        </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPassword" runat="server" Text="パスワード" Width="200px"></asp:Label>
                        <asp:TextBox ID="TbxPW" runat="server" TextMode="Password"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnLogin" CssClass="btn" runat="server" Text="ログイン" OnClick="btnLogin_Click" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
