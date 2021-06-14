<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Wiki.aspx.cs" Inherits="WhereEver.スケジュール.Wiki" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="https://unpkg.com/ress/dist/ress.min.css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="preconnect" href="https://fonts.gstatic.com" />
    <link href="https://fonts.googleapis.com/css2?family=Noto+Sans+JP&display=swap" rel="stylesheet" />


    <title>Wiki</title>
    <style>
        /*------------------------------------------------------*/
        html {
            font-size: 100%;
        }

        body {
            font-family: 'Noto Sans JP', sans-serif;
        }
        /*------------------------------------------------------*/

        header {
            margin: 0 auto;
            text-align: center;
            margin-bottom: 50px;
        }

        #TextBox1 {
            border-style: solid;
            border-width: thin;
            border-color: black;
        }

        /*------------------------------------------------------*/

        main {
            margin: 0 auto;
            text-align: center;
        }



        #FileUpload1 {
            background: rgba(255,255,255,1);
        }
        /*------------------------------------------------------*/
        #TextBox2 {
            /*メインのテキストボックス*/
            border-style: solid;
            border-width: thin;
            border-color: black;
            width: 700px;
            height: 700px;
        }
        /*------------------------------------------------------*/
        /*下の登録、戻るボタンのCSS*/

        section {
            margin: 0 auto;
            text-align: center;
            margin-top: 50px;
        }

        #Button1 {
            border-style: solid;
            border-width: thin;
            border-color: black;
        }

        #Button2 {
            border-style: solid;
            border-width: thin;
            border-color: black;
        }
        /*------------------------------------------------------*/
    </style>
</head>

<body>
    <form id="form1" runat="server">

        <header>

            <asp:Label runat="server" ID="Label1" Text="タイトル"></asp:Label>
            <asp:TextBox runat="server" ID="TextBox1" Text=""></asp:TextBox>
            <asp:FileUpload runat="server" ID="FileUpload1" />

        </header>

        <main>

            <asp:TextBox runat="server" ID="TextBox2" Text="" TextMode="MultiLine" ValidateRequestMode="Disabled"></asp:TextBox>

        </main>

        <section>

            <asp:Button runat="server" ID="Button1" Text="登録" OnClick="Button1_Click" />
            <asp:Button runat="server" ID="Button2" Text="戻る" OnClick="Button2_Click" />

        </section>

    </form>

</body>

</html>
