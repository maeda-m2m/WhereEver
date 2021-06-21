<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test1.aspx.cs" Inherits="WhereEver.スケジュール.test1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="https://unpkg.com/ress/dist/ress.min.css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta charset="UTF-8" />
    <title>m2m</title>
    <meta name="keywords" content="IT,クラウド" />
    <meta name="description" content="エム・ツー・エム株式会社の公式ホームページ" />
    <link rel="preconnect" href="https://fonts.gstatic.com" />
    <link href="https://fonts.googleapis.com/css2?family=Noto+Sans+JP&display=swap" rel="stylesheet" />

    <!-- <link rel="icon" type="image/png" href=""> -->
    <meta name="viewport" content="width=device-width,initial-scale=1" />

</head>
<body>

    <div>
        <asp:GridView runat="server" ID="TestGV" CssClass="Center" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="SdlNo" DataSourceID="Sql1" OnRowCreated="TestGV_RowCreated" ShowFooter="True">

            <Columns>

                <asp:TemplateField HeaderText="管理番号" SortExpression="SdlNo">


                    <EditItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("SdlNo") %>'></asp:Label>
                    </EditItemTemplate>

                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("SdlNo") %>'></asp:Label>
                    </ItemTemplate>

                </asp:TemplateField>

                <asp:TemplateField HeaderText="日付" SortExpression="date">

                    <EditItemTemplate>
                        <asp:TextBox ID="TextBoxA" runat="server" Text='<%# Bind("date") %>'></asp:TextBox>
                    </EditItemTemplate>

                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("date", "{0:yyyy年MM月dd日dddd}") %>'></asp:Label>
                    </ItemTemplate>

                </asp:TemplateField>

                <asp:TemplateField HeaderText="時間" SortExpression="time">

                    <EditItemTemplate>
                        <asp:DropDownList ID="Test1" runat="server" Text='<%# Bind("time") %>' SelectedValue='<%# Bind("time", "{0:HH時MM分}") %>' DataSourceID="Sql1" DataTextField="time" DataValueField="time"></asp:DropDownList>
                    </EditItemTemplate>

                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("time", "{0:HH時MM分}") %>'></asp:Label>
                    </ItemTemplate>

                </asp:TemplateField>

                <asp:TemplateField HeaderText="内容" SortExpression="title">

                    <EditItemTemplate>
                        <asp:TextBox ID="TextBoxB" runat="server" Text='<%# Bind("title") %>'></asp:TextBox>
                    </EditItemTemplate>

                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("title") %>'></asp:Label>
                    </ItemTemplate>

                </asp:TemplateField>

                <asp:TemplateField HeaderText="担当者" SortExpression="name">

                    <EditItemTemplate>
                        <asp:TextBox ID="TextBoxC" runat="server" Text='<%# Bind("name") %>'></asp:TextBox>
                    </EditItemTemplate>

                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                    </ItemTemplate>

                </asp:TemplateField>

                <asp:TemplateField HeaderText="選択">
                    <EditItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="編集/削除" ShowHeader="False">

                    <EditItemTemplate>
                        <asp:Button ID="Button1" runat="server" CausesValidation="True" CommandName="Update" Text="更新" />
                        &nbsp;<asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="Cancel" Text="キャンセル" />
                    </EditItemTemplate>

                    <ItemTemplate>
                        <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="Edit" Text="編集" />
                        &nbsp;<asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="Delete" Text="削除" OnClientClick="return confirm('本当に削除しますか？')" />
                    </ItemTemplate>

                </asp:TemplateField>

            </Columns>

        </asp:GridView>

        <asp:SqlDataSource ID="Sql1" runat="server" ConnectionString="<%$ ConnectionStrings:WhereverConnectionString %>" DeleteCommand="DELETE FROM [T_Schedule] WHERE [SdlNo] = @SdlNo" InsertCommand="INSERT INTO [T_Schedule] ([date], [time], [title], [name], [KanriFlag], [SdlNo]) VALUES (@date, @time, @title, @name, @KanriFlag, @SdlNo)" SelectCommand="SELECT * FROM [T_Schedule]" UpdateCommand="UPDATE [T_Schedule] SET [date] = @date, [time] = @time, [title] = @title, [name] = @name, [KanriFlag] = @KanriFlag WHERE [SdlNo] = @SdlNo">
            <DeleteParameters>
                <asp:Parameter Name="SdlNo" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="date" Type="DateTime" />
                <asp:Parameter Name="time" Type="String" />
                <asp:Parameter Name="title" Type="String" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="KanriFlag" Type="String" />
                <asp:Parameter Name="SdlNo" Type="Int32" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="date" Type="DateTime" />
                <asp:Parameter Name="time" Type="String" />
                <asp:Parameter Name="title" Type="String" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="KanriFlag" Type="String" />
                <asp:Parameter Name="SdlNo" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>

    </div>
</body>
</html>
