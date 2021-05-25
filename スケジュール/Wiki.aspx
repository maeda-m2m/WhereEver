<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Wiki.aspx.cs" Inherits="WhereEver.スケジュール.Wiki" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title>Wiki</title>

    <style>
        .Center {
            margin: auto;
            text-align: center;
        }

        .Test1 {
            margin: auto;
            text-align: center;
        }
    </style>

</head>

<body>

    <form id="form1" runat="server">

        <div class="Center">

            <asp:Label runat="server" ID="Label1" Text="タイトル" Font-Size="Large"></asp:Label>
            <asp:TextBox runat="server" ID="TextBox2" Text=""></asp:TextBox>

        </div>

        <div class="Test1">
            <asp:Menu runat="server" ID="Menu"></asp:Menu>
            <asp:TextBox runat="server" ID="TextBox1" Text="" TextMode="MultiLine" MaxLength="1000" Rows="100" Height="1000" Width="1000"></asp:TextBox>

        </div>

        <div class="Center">

            <asp:Button runat="server" ID="Button1" Text="登録" OnClick="Button1_Click" />

        </div>

    </form>

</body>

</html>
