<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileShare.aspx.cs" Inherits="WhereEver.FileShare" %>
<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td id="menu">
                        <menu:c_menu id="m" runat="server"></menu:c_menu>
                    </td>
                </tr>
            </table>

            <table>
                <tr>
                    <td>
                        ファイル：<input type="file" id="datum" runat="server" />
                        <td>
                            <asp:Button ID="Button1" runat="server" Text="アップロード" OnClick="Button1_Click" />
                        </td>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
