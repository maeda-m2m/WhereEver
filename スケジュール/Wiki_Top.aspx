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

        #Title1 {
            margin: auto;
            text-align: center;
            width: 500px;
        }
    </style>

</head>

<body>

    <form id="form1" runat="server">

        <h1 id="Title1">WhereEver　社内Wiki</h1>

        <div class="Center">

            <asp:Button runat="server" ID="Button1" Text="登録" OnClick="Button1_Click" />
            <asp:Button runat="server" ID="Button2" Text="編集" OnClick="Button2_Click" />

        </div>

        <div class="Center">
            <asp:DataGrid runat="server" ID="dg1" AutoGenerateColumns="False" OnSelectedIndexChanged="dg1_SelectedIndexChanged" CssClass="Center">
                <Columns>


                    <asp:BoundColumn DataField="Date" HeaderText="日付" HeaderStyle-Width="" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" />
                    </asp:BoundColumn>

                    <asp:BoundColumn DataField="Name" HeaderText="名前" ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" />
                    </asp:BoundColumn>


                    <asp:BoundColumn DataField="Title" HeaderText="タイトル" ItemStyle-Width="100px">
                        <HeaderStyle Wrap="true" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" />
                    </asp:BoundColumn>

                    

                </Columns>


            </asp:DataGrid>
        </div>

    </form>

</body>


</html>
