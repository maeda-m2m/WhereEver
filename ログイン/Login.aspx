<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WhereEver.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="Login.css" type="text/css" rel="stylesheet" />


    <title>m2m ログインページ</title>
    <style type="text/css">
        .auto-style2 {
            width: 102px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <div>
            &nbsp;<table class="auto-style1">
                <tr>
                    <td colspan="2">
            <img src="../ログイン/haikei-1.png" runat="server" id="ue" /></td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <asp:TextBox ID="TbxID" runat="server" MaxLength="30" placeholder="UserID"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TbxPW" runat="server" MaxLength="30" placeholder="Password" TextMode="Password"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/IMG/LoginBtn.png" OnClick="ImgBtn_Click" Width="30px" />
            <asp:Label ID="LblError" runat="server" ForeColor="Red"></asp:Label> </td>
                </tr>
                <tr>
                    <td colspan="2">
            <img src="..//ログイン/haikei-2.png" runat="server" id="sita" /></td>
                </tr>
            </table>
        </div>
        <div>
            
            <br />


        </div>
        <div>
            &nbsp;</div>
    </form>
</body>
</html>
