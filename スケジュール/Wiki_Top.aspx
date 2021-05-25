<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Wiki_Top.aspx.cs" Inherits="WhereEver.スケジュール.Wiki_Top" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Wiki_Top</title>
    <style>
        .Center {
            margin: auto;
            text-align: center;
        }
    </style>

</head>

<body>

    <form id="form1" runat="server">

        <div class="Center">

            <asp:Button runat="server" ID="Button1" Text="登録" OnClick="Button1_Click" />
            <asp:Button runat="server" ID="Button2" Text="編集" OnClick="Button2_Click" />

        </div>

        <div class="Center">
            <asp:DataGrid runat="server" ID="dg1"></asp:DataGrid>
        </div>

    </form>

</body>

</html>
