﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileShare.aspx.cs" Inherits="WhereEver.FileShare" %>
<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <link rel="stylesheet" type="text/css" href="FileShare.css" />

    <title>ファイル共有</title>
</head>

<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div>
            <table>
                <tr>
                    <td id="menu">
                        <menu:c_menu id="m" runat="server"></menu:c_menu>
                    </td>
                </tr>
            </table>
       </div>


       <div id="Wrap">
      

           <p>
            <asp:Label ID="lblResult" runat="server" Text="ファイルをアップロードできます。ファイル名はUUIDに自動変換されます。"></asp:Label>
           </p>

           <p class ="form-flat-border">
            [アップロードするファイル]　<asp:FileUpload ID="FileUpload_userfile" runat="server" Width="685px"  CssClass="form-flat-border" />
            <asp:Button ID="Button_Upload" runat="server" Text="アップロード" OnClick="Button_UpLoad" CssClass="btn-flat-border" />
           </p>
           <p>
             保存先：c:\\UploadedFiles\\[UUID].(拡張子)
           </p>


           <%-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// --%>
           <hr />
           <%-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// --%>

           <p>
            <asp:Label ID="lblDLResult" runat="server" Text="ファイルをダウンロードできます。拡張子を忘れずにつけて下さい。"></asp:Label>
           </p>

           <p>ダウンロードするMIMEタイプを選択して下さい：
           <asp:DropDownList ID="DropDownList1" runat="server">
               <asp:ListItem Value="text/plain" Text="テキストファイル"></asp:ListItem>
               <asp:ListItem Value="text/csv">CSVファイル</asp:ListItem>
               <asp:ListItem Value="text/html">HTMLファイル</asp:ListItem>
               <asp:ListItem Value="text/css">CSSファイル</asp:ListItem>
               <asp:ListItem Value="text/javascript">JavaScript</asp:ListItem>
               <asp:ListItem Value="application/octet-stream">実行ファイル(.exeなど)</asp:ListItem>
               <asp:ListItem Value="application/json">jsonファイル</asp:ListItem>
               <asp:ListItem Value="application/pdf">PDF</asp:ListItem>
               <asp:ListItem Value="application/vnd.ms-excel">旧Excel(.xls)</asp:ListItem>
               <asp:ListItem Value="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet">Excel(.xlsx)</asp:ListItem>
               <asp:ListItem Value="application/vnd.ms-powerpoint">旧PowerPoint(.ppt)</asp:ListItem>
               <asp:ListItem Value="application/vnd.openxmlformats-officedocument.presentationml.presentation">PowerPoint(.pptx)</asp:ListItem>
               <asp:ListItem Value="application/msword">旧Word(.doc)</asp:ListItem>
               <asp:ListItem Value="application/vnd.openxmlformats-officedocument.wordprocessingml.document">Word(.docx)</asp:ListItem>
               <asp:ListItem Value="image/jpeg">JPEG(JPG)</asp:ListItem>
               <asp:ListItem Value="image/png">PNG</asp:ListItem>
               <asp:ListItem Value="image/gif">GIF</asp:ListItem>
               <asp:ListItem Value="image/bmp">BMP</asp:ListItem>
               <asp:ListItem Value="image/svg+xml">SVG</asp:ListItem>
               <asp:ListItem Value="application/zip">ZIP</asp:ListItem>
               <asp:ListItem Value="application/x-lzh">LZH</asp:ListItem>
               <asp:ListItem Value="application/x-tar">TAR/TAR&amp;ZZIP</asp:ListItem>
               <asp:ListItem Value="audio/mpeg">MPEG</asp:ListItem>
               <asp:ListItem Value="video/mp4">MP4</asp:ListItem>
               <asp:ListItem Value="video/mpeg">MPEG</asp:ListItem>
           </asp:DropDownList></p>

            <p class ="form-flat-border">
            [ダウンロードするファイル]　c:\\UploadedFiles\\<asp:TextBox ID="TextBox_dl" runat="server" Width="600px" CssClass="form-flat-border" ></asp:TextBox>　<asp:Button ID="Button_Download" runat="server" Text="ダウンロード" OnClick="Button_DownLoad" CssClass="btn-flat-border" />
            </p>
           <p>
             参照先：c:\\UploadedFiles\\[UUID].(拡張子)
           </p>


        </div>
    </form>
</body>
</html>
