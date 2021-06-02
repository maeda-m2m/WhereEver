<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Wiki_Top.aspx.cs" Inherits="WhereEver.スケジュール.Wiki_Top" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Wiki_Top</title>
    <style>
        body {
            background-color: gainsboro;
        }

        .Center {
            margin: auto;
            text-align: center;
            background-color: white;
        }

        .Center-dg {
            margin: auto;
            text-align: center;
            background-color: white;
            width: 1000px;
        }

        .Center-Button {
            margin: auto;
            text-align: center;
            background-color: white;
            width: 300px;
        }

        .Center-Label {
            margin: auto;
            background-color: white;
            width: 800px;
            height: auto;
            font-size: 15px;
            padding: 10px 10px 10px 10px;
        }

        #Label1 {
            margin: auto;
            text-align: center;
            font-size: 30px;
        }

        #Label2 {
            margin: auto;
            text-align: left;
            font-size: 20px;
        }

        #TextBox_Output {
            margin: auto;
            text-align: left;
            font-size: 20px;
        }

        #Title1 {
            margin: auto;
            text-align: center;
            width: 500px;
        }

        .Center_1 {
            display: flex;
            justify-content: center;
            height: 200px;
        }
    </style>

</head>

<body>

    <form id="form1" runat="server">

        <h1 id="Title1">WhereEver　社内Wiki</h1>

        <div class="Center-Button">

            <asp:Button runat="server" ID="Button1" Text="登録" OnClick="Button1_Click" />
            <asp:Button runat="server" ID="Button2" Text="編集" OnClick="Button2_Click" />

        </div>

        <div class="Center_1">

            <div class="Center-dg">

                <asp:TextBox runat="server" ID="TextBox1" Text="" TextMode="Search"></asp:TextBox>
                <asp:Button runat="server" ID="Button3" Text="検索" OnClick="Button3_Click" />

                <asp:DataGrid runat="server" ID="dg1" AutoGenerateColumns="False" OnSelectedIndexChanged="dg1_SelectedIndexChanged" CssClass="Center" OnItemCommand="dg1_ItemCommand">
                    <Columns>


                        <asp:BoundColumn DataField="Date" HeaderText="日付" HeaderStyle-Width="" ItemStyle-Height="50px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px" DataFormatString="{0:yyyy}年{0:MM}月{0:dd}日">
                            <HeaderStyle Wrap="true" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left" />
                        </asp:BoundColumn>

                        <asp:BoundColumn DataField="Name" HeaderText="筆者" ItemStyle-Width="100px">
                            <HeaderStyle Wrap="true" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" />
                        </asp:BoundColumn>


                        <asp:BoundColumn DataField="Title" HeaderText="タイトル" ItemStyle-Width="100px">
                            <HeaderStyle Wrap="true" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px" />
                        </asp:BoundColumn>

                        <asp:ButtonColumn ItemStyle-Width="100px" HeaderText="本文" ButtonType="PushButton" Text="読む" CommandName="Read">
                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:ButtonColumn>


                    </Columns>


                </asp:DataGrid>
            </div>

            <div class="Center-Label">

                <asp:Label runat="server" ID="Label1" Text=""></asp:Label>

                <br />


                <asp:Label runat="server" ID="Label2" Text=""></asp:Label>

                <br />


                <asp:Label runat="server" ID="Label3" Text=""></asp:Label>

            </div>

        </div>

    </form>


</body>


</html>
