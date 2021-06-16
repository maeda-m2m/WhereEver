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
    <style>
        @charset "UTF-8";
        /*------------------------------------------------------*/
        html {
            font-size: 100%;
            list-style: none;
        }

        body {
            font-family: 'Noto Sans JP', sans-serif;
        }

        /*------------------------------------------------------*/
        /*ヘッダーのCSS*/
        .header_fb {
            width: 100%;
            text-decoration: none;
            display: flex;
            background-color: black;
        }

            .header_fb a {
                text-decoration: none;
                color: white;
            }

        header_title {
            font-size: 30px;
        }

        .div1 {
        }

        .div2 {
            text-align: center;
        }




        .header_ul {
            display: flex;
            justify-content: center;
            list-style: none;
            margin-top: 50px;
        }

            .header_ul li {
                margin-left: 200px;
                font-size: 20px;
            }

                .header_ul li:hover {
                    border-width: 1px;
                    border-style: solid;
                    border-color: white;
                }
        /*------------------------------------------------------*/
        /*フッターのCSS*/
        .footer_fb {
            display: flex;
            justify-content: center;
            width: 100%;
            height: 500px;
            background-color: black;
        }

            .footer_fb a {
                text-decoration: none;
                color: white;
            }

            .footer_fb div {
                margin-left: 100px;
            }

            .footer_fb ul {
                list-style: none;
            }


        /*------------------------------------------------------*/
        .main1 {
            width: 1280px;
            height: 780px;
            background-color: aqua;
        }

        .center {
            margin: 0 auto;
            text-align: center;
        }
        /*------------------------------------------------------*/
        /* @media(){} */
    </style>
</head>
<body>

    <header class="header_fb">
        <div class="div1">
            <a href="#">
                <img src="../ログイン/m2m-logo.png" alt="" /></a>

        </div>
        <div class="div2">
            <h1 id="header_title"><a href="#">株式会社エム・ツー・エム　　m2mConsulting&Services</a></h1>


            <ul class="header_ul">
                <li><a href="#">株式会社エム・ツー・エムについて</a></li>
                <li><a href="Schedule.aspx">製品/サービス</a> </li>

                <li><a href="#">採用情報</a></li>
                <li><a href="#">お問い合わせ</a></li>


            </ul>
        </div>
    </header>

    <main>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />


    </main>

    <footer class="footer_fb">
        <div>
            <h3><a href="#">株式会社エム・ツー・エムについて</a></h3>

            <ul>
                <li><a href="#">経営理念</a></li>
                <li><a href="#">会社概要</a></li>
                <li><a href="#">社長メッセージ</a></li>
                <li><a href="#">沿革・歴史</a></li>

            </ul>
        </div>
        <div>
            <h3><a href="#">製品/サービス</a></h3>
            <ul>
                <li><a href="http://www.m2m-asp.com/prod_01.html">Web資材調達システム</a></li>
                <li><a href="http://www.m2m-asp.com/prod_02.html">Web受注システム</a></li>
                <li><a href="http://www.m2m-asp.com/prod_04.html">Web会計・販売管理システム</a></li>
                <li><a href="http://www.m2m-asp.com/prod_03.html">Web見積依頼システム</a></li>
                <li><a href="http://www.m2m-asp.com/prod_04.html">Web会計・販売管理システム</a></li>
                <li><a href="http://www.m2m-asp.com/prod_04.html">Web会計・販売管理システム</a></li>
                <li><a href="http://www.m2m-asp.com/prod_04.html">Web会計・販売管理システム</a></li>
                <li><a href="http://www.m2m-asp.com/prod_04.html">Web会計・販売管理システム</a></li>
                <li><a href="http://www.m2m-asp.com/prod_04.html">Web会計・販売管理システム</a></li>
                <li><a href="http://www.m2m-asp.com/prod_04.html">Web会計・販売管理システム</a></li>
                <li><a href="http://www.m2m-asp.com/prod_04.html">Web会計・販売管理システム</a></li>
                <li><a href="http://www.m2m-asp.com/prod_04.html">Web会計・販売管理システム</a></li>

            </ul>
        </div>

        <div>
            <h3><a href="#">採用情報</a></h3>
            <a href="https://job.rikunabi.com/2022/company/r951891034/">リクナビ2022</a>
        </div>
        <div>
            <h3><a href="#">お問い合わせ</a></h3>

        </div>
    </footer>

    <footer>
        <p class="center">プライバシーポリシーなど</p>
    </footer>

    <footer>
        <p class="center"><small>&copy;2021 株式会社エム・ツー・エム　　m2mConsulting&Services</small></p>
    </footer>



</body>
</html>
