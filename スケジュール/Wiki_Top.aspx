<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Wiki_Top.aspx.cs" Inherits="WhereEver.スケジュール.Wiki_Top" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="https://unpkg.com/ress/dist/ress.min.css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="preconnect" href="https://fonts.gstatic.com" />
    <link href="https://fonts.googleapis.com/css2?family=Noto+Sans+JP&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="wiki.css" />
    <title>Wiki_Top</title>

</head>

<body>
    <form id="form1" runat="server">
        <header>
            <table>
                <tr>
                    <td class="All">
                        <a href="~/ログイン/LoginList.aspx" runat="server">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/ログイン/m2m-logo.png" /></a>
                    </td>
                    <td>
                        <h1>WhereEver</h1>
                    </td>
                    <td>
                        <a href="~/ログイン/LoginList.aspx" class="btn" runat="server">トップページ</a>
                    </td>
                    <td>
                        <a href="~/スケジュール/Schedule.aspx" class="btn" runat="server">スケジュール</a>
                    </td>
                    <td>
                        <a href="~/スケジュール/Wiki_Top.aspx" class="btn" runat="server">社内Wiki</a>
                    </td>
                    <td>
                        <a href="~/各申請書類/Shinsei.aspx" class="btn" runat="server">各申請書類</a>
                    </td>
                    <td>
                        <a href="~/チャット/Chat.aspx" class="btn" runat="server">チャット<asp:Label ID="lblHensin" runat="server" Visible="False"></asp:Label></a>
                    </td>
                    <td>
                        <a href="~/ファイル共有/FileShare.aspx" class="btn" runat="server">ファイル共有</a>
                    </td>
                    <td>
                        <a href="~/Project System/PIchiran.aspx" class="btn" runat="server">プロジェクト</a>
                    </td>
                    <td>
                        <a href="~/管理ページ/Kanri.aspx" class="btn" runat="server">マイページ</a>
                    </td>
                    <td>
                        <a href="~/ログイン/Login.aspx" class="btn" runat="server">ログアウト</a>
                    </td>

                </tr>

            </table>
        </header>





        <div class="Center-Button">

            <asp:TextBox runat="server" ID="TextBox1" Text="" placeholder="タイトル、本文を検索できます" TextMode="Search"></asp:TextBox>
            <asp:Button runat="server" ID="Button3" Text="検索" OnClick="Button3_Click" />

            <asp:Button runat="server" ID="Button1" Text="登録" OnClick="Button1_Click" />
            <asp:Button runat="server" ID="Button2" Text="編集" OnClick="Button2_Click" />


        </div>

        <div class="display-flex">

            <div class="Center-dg">

                <asp:Label runat="server" ID="Label4" Text=""></asp:Label>

                <asp:DataGrid runat="server" ID="dg1" AutoGenerateColumns="False" OnSelectedIndexChanged="dg1_SelectedIndexChanged" OnItemCommand="dg1_ItemCommand">
                    <HeaderStyle BackColor="Black" ForeColor="White" Font-Size="30px" />
                    <Columns>

                        <asp:BoundColumn DataField="Date" HeaderText="日付" DataFormatString="{0:yyyy}年{0:MM}月{0:dd}日"></asp:BoundColumn>

                        <asp:BoundColumn DataField="Name" HeaderText="筆者"></asp:BoundColumn>

                        <asp:BoundColumn DataField="Title" HeaderText="タイトル"></asp:BoundColumn>

                        <asp:ButtonColumn HeaderText="本文" ButtonType="PushButton" Text="読む" CommandName="Read"></asp:ButtonColumn>


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
        <footer>
        </footer>
    </form>

</body>

</html>
