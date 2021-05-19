<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintSchedule.aspx.cs" Inherits="WhereEver.PrintSchedule" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />

    <title>Print Schedule</title>
    <style type="text/css">
        body {
            margin: 0;
            padding: 0;
        }

        #ImgDiv {
            position: absolute;
            height: 1175px;
            width: 721px;
        }

        #Scd {
            height: 1273px;
            width: 743px;
        }

        #LabelDiv {
            position: absolute;
        }


        #Scdl4 {
            position: absolute;
        }

        .Label {
            font-size: 12px;
            position: absolute;
            margin-left: 30px;
        }

        #Label3 {
            margin-top: 148px;
        }

        #Label4 {
            margin-top: 213px;
        }

        #Label5 {
            margin-top: 278px;
        }

        #Label12 {
            margin-top: 343px;
        }

        #Label13 {
            margin-top: 408px;
        }

        #Label14 {
            margin-top: 473px;
        }

        #Label15 {
            margin-top: 538px;
        }

        #Label16 {
            margin-top: 603px;
        }

        #Label17 {
            margin-top: 668px;
        }

        #Label18 {
            margin-top: 733px;
        }

        #Label19 {
            margin-top: 798px;
        }

        #Label20 {
            margin-top: 863px;
        }

        #Label21 {
            margin-top: 928px;
        }

        #SCDL {
            margin-left: 180px;
            margin-top: 61px;
        }

        #t {
            position: absolute;
            text-align: center;
            width: 152px;
            border: 1px solid #bababa;
            border-right: none;
        }

            #t p {
                font-size: 12px;
            }

        .Heijitsu {
            text-align: center;
            font-size: 10px;
            height: 62px;
            border-bottom: 3px double #bababa;
            border-right: 1px solid black;
            border-left: 1px solid black;
        }

        .Holiday {
            margin: 0;
            padding: 0;
            background-color: #bababa;
            height: 55px;
            font-size: 10px;
            width: 72px;
            text-align: center;
            border-right: 1px solid black;
            border-left: 1px solid black;
            border-bottom: 3px double #bababa;
        }

        .Head {
            width: 72px;
            text-align: center;
            font-size: 12px;
            line-height: 10px;
        }

        .sc {
            height: 67px;
            font-size: 10px;
            width: 72px;
            border: 1px solid #bababa;
            border-right: none;
        }

        #TextArea1 {
            margin-top: 940px;
            margin-left: 312px;
        }

        #SCDL2 {
            margin-left: 180px;
            margin-top: 112px;
        }

        #SCDL3 {
            margin-left: 180px;
            margin-top: 130px;
        }

        .SCdl {
            width: 72px;
            height: 63px;
            font-size: 12px;
        }

        .DL {
            width: 340px;
            font-size: 12px;
        }

        #pr1 {
            position: absolute;
            margin-left: 151px;
            font-size: 9px;
            border: 1px solid #bababa;
            border-left: none;
            height: 60px;
            line-height: 2px;
            width: 540px;
        }

        .pri {
            width: 72px;
            height: 10px;
        }

        .prin {
            width: 400px;
            height: 13px;
        }

        #pr2 {
            position: absolute;
            margin-top: 114px;
            margin-left: 150px;
            font-size: 9px;
            border: 1px solid #bababa;
            border-left: none;
            height: 59px;
            line-height: 2px;
            width: 540px;
        }

        #pr3 {
            position: absolute;
            margin-top: 186px;
            margin-left: 150px;
            font-size: 10px;
            border: 1px solid #bababa;
            border-left: none;
            height: 62px;
            line-height: 1.5px;
            width: 540px;
        }

        #pr4 {
            position: absolute;
            margin-top: 256px;
            margin-left: 150px;
            font-size: 10px;
            border: 1px solid #bababa;
            border-left: none;
            height: 62px;
            line-height: 1.5px;
            width: 540px;
        }

        #pr5 {
            position: absolute;
            margin-top: 322px;
            margin-left: 151px;
            font-size: 10px;
            border: 1px solid #bababa;
            height: 62px;
            line-height: 1.5px;
            width: 540px;
        }

        #pr6 {
            position: absolute;
            margin-top: 529px;
            margin-left: 151px;
            font-size: 10px;
            border: 1px solid #bababa;
            height: 62px;
            line-height: 1.5px;
            width: 540px;
        }

        #pr7 {
            position: absolute;
            margin-top: 600px;
            margin-left: 151px;
            font-size: 10px;
            border: 1px solid #bababa;
            height: 62px;
            line-height: 1.5px;
            width: 540px;
        }

        #pr8 {
            position: absolute;
            margin-top: 671px;
            margin-left: 151px;
            font-size: 10px;
            border: 1px solid #bababa;
            height: 62px;
            line-height: 1.5px;
            width: 540px;
        }

        #pr9 {
            position: absolute;
            margin-top: 742px;
            margin-left: 151px;
            font-size: 10px;
            border: 1px solid #bababa;
            height: 62px;
            line-height: 1.5px;
            width: 540px;
        }

        #pr10 {
            position: absolute;
            margin-top: 813px;
            margin-left: 151px;
            font-size: 10px;
            border: 1px solid #bababa;
            height: 62px;
            line-height: 1.5px;
            width: 540px;
        }

        #Btn {
            position: absolute;
            margin-top: 1150px;
            margin-left: 230px;
        }

        #newsdl {
            font-size: 10px;
            font-family: Meiryo;
            border: 1px solid black;
            margin:auto;
        }

        .bb {
            text-align: center;
            background-color: #272727;
            color: white;
        }

        .aa {
            background-color: #272727;
            color: white;
            text-align: center;
        }

        #text {
            margin: 0;
            padding: 0;
        }

        .under {
            border-bottom: 3px double #bababa;
        }

        .cc {
            margin: 0;
            padding: 0;
        }

        @media print {
            .noprint {
                display: none;
            }
        }
        .auto-style1 {
            background-color: #272727;
            color: white;
            text-align: center;
            width: 380px;
        }
        .auto-style3 {
            width: 380px;
            border-right: 1px dotted black;
            border-left: 1px dotted black;
        }
        .auto-style4 {
            border-bottom: 3px double #bababa;
            width: 380px;
            border-right: 1px dotted black;
            border-left: 1px dotted black;
        }
        .auto-style5 {
            width: 380px;
            height: 13px;
            border-right: 1px dotted black;
            border-left: 1px dotted black;

        }
        .auto-style6 {
            background-color: #272727;
            color: white;
            text-align: center;
            width: 130px;
        }
        .auto-style7 {
            width: 130px;
            height: 10px;
        }
        .auto-style8 {
            width: 130px;
        }
        .auto-style9 {
            border-bottom: 3px double #bababa;
            width: 130px;
        }
        .auto-style10 {
            text-align: center;
            background-color: #272727;
            color: white;
            width: 50px;
        }
        .auto-style11 {
            text-align: center;
            font-size: 10px;
            height: 62px;
            border-bottom: 3px double #bababa;
            width: 50px;
            border-right: 1px solid black;
        }
        .auto-style12 {
            margin: 0;
            padding: 0;
            background-color: #bababa;
            height: 55px;
            font-size: 10px;
            width: 50px;
            text-align: center;
            border-right: 1px solid black;
            border-bottom: 3px double #bababa;
        }

        .DgBikou{
            float: left;
            width:50%;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="DgBikou">
        <input type="button" id="Btn" value="印刷" onclick="window.print();" class="noprint" />

        <table id="newsdl">
            <asp:Label ID="lbltitle" runat="server" Text="スケジュール表"></asp:Label>
            <tr>
                <td class="bb">
                    <p>日付</p>
                </td>
                <td class="auto-style10">
                    <p>曜日</p>
                </td>
                <td class="aa">
                    <p>時間</p>
                </td>
                <td class="auto-style1">
                    <p>内容</p>
                </td>
                <td class="auto-style6">
                    <p>担当者</p>
                </td>
            </tr>
            <tr>
                <td rowspan="4" class="Heijitsu">
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                </td>
                <td rowspan="4" class="auto-style11">
                    <asp:Label ID="Label2" runat="server" Text="月"></asp:Label>
                </td>
                <td class="pri">
                    <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style5">
                    <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style7">
                    <asp:Label ID="Label5" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label7" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label8" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label9" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label10" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label11" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="under">
                    <asp:Label ID="Label12" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style4">
                    <asp:Label ID="Label13" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style9">
                    <asp:Label ID="Label14" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td rowspan="4" class="Heijitsu">
                    <asp:Label ID="Label15" runat="server" Text=""></asp:Label>
                </td>
                <td rowspan="4" class="auto-style11">
                    <asp:Label ID="Label16" runat="server" Text="火"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label17" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label18" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label19" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label20" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label21" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label22" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label23" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label24" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label25" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="under">
                    <asp:Label ID="Label26" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style4">
                    <asp:Label ID="Label27" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style9">
                    <asp:Label ID="Label28" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td rowspan="4" class="Heijitsu">
                    <asp:Label ID="Label29" runat="server" Text=""></asp:Label>
                </td>
                <td rowspan="4" class="auto-style11">
                    <asp:Label ID="Label30" runat="server" Text="水"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label31" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label32" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label33" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label34" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label35" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label36" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label37" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label38" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label39" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="under">
                    <asp:Label ID="Label40" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style4">
                    <asp:Label ID="Label41" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style9">
                    <asp:Label ID="Label42" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td rowspan="4" class="Heijitsu">
                    <asp:Label ID="Label43" runat="server" Text=""></asp:Label>
                </td>
                <td rowspan="4" class="auto-style11">
                    <asp:Label ID="Label44" runat="server" Text="木"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label45" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label46" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label47" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label48" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label49" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label50" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label51" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label52" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label53" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="under">
                    <asp:Label ID="Label54" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style4">
                    <asp:Label ID="Label55" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style9">
                    <asp:Label ID="Label56" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td rowspan="4" class="Heijitsu">
                    <asp:Label ID="Label57" runat="server" Text=""></asp:Label>
                </td>
                <td rowspan="4" class="auto-style11">
                    <asp:Label ID="Label58" runat="server" Text="金"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label59" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label60" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label61" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label62" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label63" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label64" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label65" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label66" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label67" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="under">
                    <asp:Label ID="Label68" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style4">
                    <asp:Label ID="Label69" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style9">
                    <asp:Label ID="Label70" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="dd">
                <td rowspan="4" class="Holiday">
                    <asp:Label ID="Labelsat1" runat="server" Text=""></asp:Label>
                </td>
                <td rowspan="4" class="auto-style12">
                    <asp:Label ID="Labelsat2" runat="server" Text="土"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Labelsat3" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Labelsat4" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Labelsat5" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Labelsat6" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Labelsat7" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Labelsat8" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Labelsat9" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Labelsat10" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Labelsat11" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="under">
                    <asp:Label ID="Labelsat12" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style4">
                    <asp:Label ID="Labelsat13" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style9">
                    <asp:Label ID="Labelsat14" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="dd">
                <td rowspan="4" class="Holiday">
                    <asp:Label ID="Labelsun1" runat="server" Text=""></asp:Label>
                </td>
                <td rowspan="4" class="auto-style12">
                    <asp:Label ID="Labelsun2" runat="server" Text="日"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Labelsun3" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Labelsun4" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Labelsun5" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Labelsun6" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Labelsun7" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Labelsun8" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Labelsun9" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Labelsun10" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Labelsun11" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="under">
                    <asp:Label ID="Labelsun12" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style4">
                    <asp:Label ID="Labelsun13" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style9">
                    <asp:Label ID="Labelsun14" runat="server" Text=""></asp:Label>
                </td>
            </tr>

            <tr>
                <td rowspan="4" class="Heijitsu">
                    <asp:Label ID="Label99" runat="server" Text=""></asp:Label>
                </td>
                <td rowspan="4" class="auto-style11">
                    <asp:Label ID="Label100" runat="server" Text="月"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label101" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label102" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label103" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label104" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label105" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label106" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label107" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label108" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label109" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="under">
                    <asp:Label ID="Label110" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style4">
                    <asp:Label ID="Label111" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style9">
                    <asp:Label ID="Label112" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td rowspan="4" class="Heijitsu">
                    <asp:Label ID="Label113" runat="server" Text=""></asp:Label>
                </td>
                <td rowspan="4" class="auto-style11">
                    <asp:Label ID="Label114" runat="server" Text="火"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label115" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label116" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label117" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label118" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label119" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label120" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label121" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label122" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label123" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="under">
                    <asp:Label ID="Label124" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style4">
                    <asp:Label ID="Label125" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style9">
                    <asp:Label ID="Label126" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td rowspan="4" class="Heijitsu">
                    <asp:Label ID="Label127" runat="server" Text=""></asp:Label>
                </td>
                <td rowspan="4" class="auto-style11">
                    <asp:Label ID="Label128" runat="server" Text="水"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label129" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label130" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label131" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label132" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label133" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label134" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label135" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label136" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label137" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="under">
                    <asp:Label ID="Label138" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style4">
                    <asp:Label ID="Label139" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style9">
                    <asp:Label ID="Label140" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td rowspan="4" class="Heijitsu">
                    <asp:Label ID="Label141" runat="server" Text=""></asp:Label>
                </td>
                <td rowspan="4" class="auto-style11">
                    <asp:Label ID="Label142" runat="server" Text="木"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label143" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label144" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label145" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label146" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label147" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label148" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label149" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label150" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label151" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="under">
                    <asp:Label ID="Label152" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style4">
                    <asp:Label ID="Label153" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style9">
                    <asp:Label ID="Label154" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td rowspan="4" class="Heijitsu">
                    <asp:Label ID="Label155" runat="server" Text=""></asp:Label>
                </td>
                <td rowspan="4" class="auto-style11">
                    <asp:Label ID="Label156" runat="server" Text="金"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label157" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label158" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label159" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label160" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label161" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label162" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label163" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Label164" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Label165" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="under">
                    <asp:Label ID="Label166" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style4">
                    <asp:Label ID="Label167" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style9">
                    <asp:Label ID="Label168" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td rowspan="4" class="Holiday">
                    <asp:Label ID="Labelsat15" runat="server" Text=""></asp:Label>
                </td>
                <td rowspan="4" class="auto-style12">
                    <asp:Label ID="Labelsat16" runat="server" Text="土"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Labelsat17" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Labelsat18" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Labelsat19" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Labelsat20" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Labelsat21" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Labelsat22" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Labelsat23" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Labelsat24" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Labelsat25" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="under">
                    <asp:Label ID="Labelsat26" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style4">
                    <asp:Label ID="Labelsat27" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style9">
                    <asp:Label ID="Labelsat28" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td rowspan="4" class="Holiday">
                    <asp:Label ID="Labelsun15" runat="server" Text=""></asp:Label>
                </td>
                <td rowspan="4" class="auto-style12">
                    <asp:Label ID="Labelsun16" runat="server" Text="日"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Labelsun17" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Labelsun18" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Labelsun19" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Labelsun20" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Labelsun21" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Labelsun22" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Labelsun23" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:Label ID="Labelsun24" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Labelsun25" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="under">
                    <asp:Label ID="Labelsun26" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style4">
                    <asp:Label ID="Labelsun27" runat="server" Text=""></asp:Label>
                </td>
                <td class="auto-style9">
                    <asp:Label ID="Labelsun28" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:Label ID="lblbikou" runat="server" Text="[備考欄]"></asp:Label>
                    <textarea id="" rows="3" style="width: 685px"></textarea>
                </td>
            </tr>
        </table>
        <input type="button" id="Back" value="戻る" onclick="history.back();" class="noprint" />
        <br />
 </div>
        <div class="DgBikou">
            <asp:DataGrid ID="DgBikou" runat="server" Width="100%"
                            AutoGenerateColumns="False" 
                            OnItemDataBound="DgBikou_ItemDataBound"
                            OnEditCommand="DgBikou_EditCommand"
                            OnCancelCommand="DgBikou_CancelCommand"
                            OnUpdateCommand="DgBikou_UpdateCommand"
                            OnItemCommand="DgBikou_ItemCommand">
                <Columns>
                    <asp:BoundColumn DataField="BikouID" HeaderText="備考ID" ReadOnly="True" />
                    <asp:BoundColumn DataField="Bikou" HeaderText="備考内容" />
                    <asp:EditCommandColumn EditText="変更" CancelText="キャンセル" UpdateText="保存"></asp:EditCommandColumn>
                    <asp:ButtonColumn ButtonType="LinkButton" Text="削除" CommandName="Delete"/>
                </Columns>
            </asp:DataGrid>
        </div>
    </form>

</body>
</html>
