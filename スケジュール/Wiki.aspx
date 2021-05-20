<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Wiki.aspx.cs" Inherits="WhereEver.スケジュール.Wiki" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

    <title>Wiki</title>

    <style>
        .Test1{
            margin:auto;
            text-align:center;
            height:500px;
            width:500px;
        }
    </style>

</head>

<body>

    <form id="form1" runat="server">

        <div class="Test1">

            <asp:TextBox runat="server" ID="TextBox1" Text="" TextMode="MultiLine" MaxLength="1000" Rows="10"></asp:TextBox>
         
        </div>

    </form>

</body>

</html>
