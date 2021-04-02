<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Shinsei.aspx.cs" Inherits="WhereEver.Calender" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="Shinsei.css" type="text/css" rel="stylesheet" />
    <link href="../MenuControl.css" type="text/css" rel="stylesheet" />

    <title></title>
    <style type="text/css">
        #Riyuu {
            width: 418px;
        }

        #Bikou {
            width: 418px;
        }

        #name2 {
            font-size: 12px;
            height: 50px;
        }

        #Syacho2 {
            text-align: center;
            width: 50px;
            border: 1px solid #000000;
            height: 20px;
            font-size: 12px;
            margin-left: 10px;
        }

        #yohaku2 {
            width: 380px;
        }

        #yohaku3 {
            height: 10px;
        }

        #date2 {
            width: 100px;
            font-size: 12px;
            height: 50px;
        }


        .naiyou {
            border: 1px solid #000000;
            width: 50px;
            height: 40px;
            text-align: center;
            font-size: 12px;
        }

        .nakami {
            border: 1px solid #000000;
            height: 30px;
            font-size: 12px;
        }

        #top {
            height: 70px;
            text-align: center;
        }

        #foot {
            height: 30px;
            font-size: 12px;
        }

        #yohaku4 {
            width: 200px;
            height: 60px;
        }

        #yohaku5 {
            width: 100px;
        }

        .hanko2 {
            border: 1px solid #000000;
            width: px;
            height: 40px;
        }

        #Todoke {
            font-family: 'MS Mincho';
            margin-left: 100px;
        }

        #Btn2 {
            text-align: center;
            width: 120px;
            display: inline-block;
            padding: 0.3em 1em;
            text-decoration: none;
            color: #16ba00;
            border: solid 2px #16ba00;
            border-radius: 3px;
            transition: .4s;
            font-family: Meiryo;
            border-radius: 3px;
        }

            #Btn2:hover {
                background: #16ba00;
                color: white;
            }

        #sinsei {
            margin-left: 100px;
        }

        #buppintouroku {
            margin-left: 100px;
        }

        #kyuka {
            margin-left: 100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="noprint">
                <tr>
                    <td id="menu">
                        <Menu:c_menu ID="c" runat="server"></Menu:c_menu>
                    </td>
                </tr>
            </table>


            <asp:Panel ID="Panel1" runat="server" CssClass="noprint">
                <table id="sinsei">
                    <tr>
                        <td class="title">
                            <p>申請書類</p>
                            <td class="text">
                                <asp:DropDownList ID="DropDownList1" runat="server">
                                    <asp:ListItem>物品購入申請</asp:ListItem>
                                    <asp:ListItem>休暇・早退・出社・遅刻届</asp:ListItem>
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="Button3" runat="server" Text="確定" CssClass="btn-flat-border" OnClick="Button3_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="Panel2" runat="server" CssClass="noprint">
                <table id="buppintouroku">
                    <tr>
                        <td class="title">
                            <p>購入品名</p>
                            <td class="text">
                                <asp:TextBox ID="TextBox1" runat="server" CssClass="textbox"></asp:TextBox>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>種別</p>
                            <td class="text">
                                <asp:TextBox ID="TextBox2" runat="server" CssClass="textbox"></asp:TextBox>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>数量</p>
                            <td class="text">
                                <asp:TextBox ID="TextBox3" runat="server" CssClass="textbox"></asp:TextBox>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>金額</p>
                            <td class="text">
                                <asp:TextBox ID="TextBox4" runat="server" CssClass="textbox"></asp:TextBox>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>購入先</p>
                            <td class="text">
                                <asp:TextBox ID="TextBox5" runat="server" CssClass="textbox"></asp:TextBox>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>購入目的</p>
                            <td>
                                <textarea id="TextArea1" cols="27" rows="3" cssclass="textbox" runat="server"></textarea>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>備考</p>
                            <td class="text">
                                <textarea id="TextArea2" cols="27" rows="3" cssclass="textbox" runat="server"></textarea>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="Button1" runat="server" Text="確定" CssClass="btn-flat-border" OnClick="Button1_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="Panel3" runat="server" CssClass="noprint">
                <table runat="server" id="kyuka">
                    <tr>
                        <td class="title">
                            <p>届出内容</p>
                            <td class="text">
                                <asp:DropDownList ID="DropDownList2" runat="server">
                                    <asp:ListItem>休暇届</asp:ListItem>
                                    <asp:ListItem>半休届</asp:ListItem>
                                    <asp:ListItem>早退届</asp:ListItem>
                                    <asp:ListItem>遅刻届</asp:ListItem>
                                    <asp:ListItem>出社届</asp:ListItem>
                                </asp:DropDownList>

                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>日時</p>
                            <td class="text">
                                <asp:Calendar ID="Calendar1" runat="server" OnSelectionChanged="Calendar1_SelectionChanged"></asp:Calendar>
                                <asp:DropDownList ID="DropDownList3" runat="server">
                                    <asp:ListItem>9:00</asp:ListItem>
                                    <asp:ListItem>9:30</asp:ListItem>
                                    <asp:ListItem>10:00</asp:ListItem>
                                    <asp:ListItem>10:00</asp:ListItem>
                                    <asp:ListItem>10:30</asp:ListItem>
                                    <asp:ListItem>11:00</asp:ListItem>
                                    <asp:ListItem>11:30</asp:ListItem>
                                    <asp:ListItem>12:00</asp:ListItem>
                                    <asp:ListItem>13:00</asp:ListItem>
                                    <asp:ListItem>13:30</asp:ListItem>
                                    <asp:ListItem>14:00</asp:ListItem>
                                    <asp:ListItem>14:30</asp:ListItem>
                                    <asp:ListItem>15:00</asp:ListItem>
                                    <asp:ListItem>15:30</asp:ListItem>
                                    <asp:ListItem>16:00</asp:ListItem>
                                    <asp:ListItem>16:30</asp:ListItem>
                                    <asp:ListItem>17:00</asp:ListItem>
                                    <asp:ListItem>17:30</asp:ListItem>
                                    <asp:ListItem>18:00</asp:ListItem>
                                </asp:DropDownList>

                                <br />

                                <asp:Label ID="Label16" runat="server" Text="Label"></asp:Label>
                                <asp:Label ID="Label17" runat="server" Text="Label"></asp:Label>から
                                <td>
                                    <asp:Calendar ID="Calendar2" runat="server" OnSelectionChanged="Calendar2_SelectionChanged"></asp:Calendar>
                                    <asp:DropDownList ID="DropDownList4" runat="server">
                                        <asp:ListItem>9:00</asp:ListItem>
                                        <asp:ListItem>9:30</asp:ListItem>
                                        <asp:ListItem>10:00</asp:ListItem>
                                        <asp:ListItem>10:00</asp:ListItem>
                                        <asp:ListItem>10:30</asp:ListItem>
                                        <asp:ListItem>11:00</asp:ListItem>
                                        <asp:ListItem>11:30</asp:ListItem>
                                        <asp:ListItem>12:00</asp:ListItem>
                                        <asp:ListItem>13:00</asp:ListItem>
                                        <asp:ListItem>13:30</asp:ListItem>
                                        <asp:ListItem>14:00</asp:ListItem>
                                        <asp:ListItem>14:30</asp:ListItem>
                                        <asp:ListItem>15:00</asp:ListItem>
                                        <asp:ListItem>15:30</asp:ListItem>
                                        <asp:ListItem>16:00</asp:ListItem>
                                        <asp:ListItem>16:30</asp:ListItem>
                                        <asp:ListItem>17:00</asp:ListItem>
                                        <asp:ListItem>17:30</asp:ListItem>
                                        <asp:ListItem>18:00</asp:ListItem>
                                    </asp:DropDownList><br />
                                    <asp:Label ID="Label18" runat="server" Text="Label"></asp:Label>
                                    <asp:Label ID="Label19" runat="server" Text="Label"></asp:Label>まで

                                </td>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>理由</p>
                            <td class="text">
                                <textarea id="TextArea3" cols="27" rows="3" cssclass="textbox" runat="server"></textarea>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>備考</p>
                            <td class="text">
                                <textarea id="TextArea4" cols="27" rows="3" cssclass="textbox" runat="server"></textarea>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="Button2" runat="server" Text="確定" CssClass="btn-flat-border" OnClick="Button2_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="Panel4" runat="server">
                <table id="buppin">
                    <tr>
                        <td colspan="4" id="head">
                            <h1>物品購入申請書</h1>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <td class="aa">
                                <td>
                                    <td>
                                        <asp:Label ID="date" runat="server" Text="Select Date"></asp:Label>
                                    </td>
                                </td>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td id="name">
                            <asp:Label ID="name1" runat="server" Text="No Name"></asp:Label>
                            <td>
                                <td>
                                    <td></td>
                                </td>
                            </td>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <td>
                                <td>
                                    <td id="Syacho">
                                        <p style="width: 71px">社長</p>
                                    </td>
                                </td>
                            </td>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <td>
                                <td>
                                    <td class="hanko"></td>
                                </td>
                            </td>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <td id="yohaku">
                                <td>
                                    <td></td>
                                </td>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle">
                            <p>購入品名</p>
                            <td colspan="3" class="zone">
                                <asp:Label ID="Konyu" runat="server" Text="購入品を記載してください"></asp:Label>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle">
                            <p>種別</p>
                            <td colspan="3" class="zone">
                                <asp:Label ID="Syubetsu" runat="server" Text="種別を記載してください"></asp:Label>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle">
                            <p>数量</p>
                            <td colspan="3" class="zone">
                                <asp:Label ID="Suryo" runat="server" Text="数量を記載してください"></asp:Label>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle">
                            <p>金額</p>
                            <td colspan="3" class="zone">
                                <asp:Label ID="Label1" runat="server" Text="金額を記載してください"></asp:Label>
                            </td>
                        </td>
                    </tr>

                    <tr>
                        <td class="TableTitle">
                            <p>購入先</p>
                            <td colspan="3" class="zone">
                                <asp:Label ID="KonyuSaki" runat="server" Text="購入先を記載してください"></asp:Label>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle">
                            <p>購入理由/目的</p>
                            <td colspan="3" class="area">
                                <textarea id="Riyuu" rows="7" runat="server"></textarea>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle">
                            <p>備考</p>
                            <td colspan="3" class="area">
                                <textarea id="Bikou" rows="7" runat="server"></textarea>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <p>上記の通り申請致します。</p>
                        </td>
                    </tr>
                </table>
                <input type="button" id="Btn" value="印刷" onclick="window.print();" class="noprint" />
            </asp:Panel>
            <%-- lllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll --%>
            <asp:Panel ID="Panel5" runat="server">
                <table id="Todoke">
                    <tr>
                        <td colspan="4" id="top">
                            <asp:Label ID="Label2" runat="server" Text="届ける項目を選択してください"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td id="date2" class="takasa">
                            <asp:Label ID="Label3" runat="server" Text="date"></asp:Label>
                            <td colspan="3"></td>
                        </td>
                    </tr>
                    <tr class="takasa">
                        <td colspan="2" id="name2">
                            <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label><asp:Label ID="Label13" runat="server" Text="（印）"></asp:Label>

                            <td colspan="2"></td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <td>
                                <td>
                                    <td id="Syacho2">
                                        <p>社長</p>
                                    </td>

                                </td>
                            </td>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <td id="yohaku5">
                                <td id="yohaku4">
                                    <td class="hanko2"></td>
                                </td>
                            </td>
                        </td>
                        </td>
                    </tr>
                    <tr>
                        <td id="yohaku3" colspan="4"></td>
                    </tr>
                    <tr>
                        <td class="naiyou">
                            <p>届出内容</p>
                            <td colspan="3" runat="server" class="nakami">
                                <asp:Label ID="Label5" runat="server" Text="項目を選択してください"></asp:Label>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="naiyou">
                            <p>日時</p>
                            <td colspan="3" class="nakami">
                                <asp:Label ID="Label6" runat="server" Text="Label"></asp:Label>
                                <asp:Label ID="Label7" runat="server" Text="Label"></asp:Label>から<br />
                                <asp:Label ID="Label8" runat="server" Text="Label"></asp:Label><asp:Label ID="Label9" runat="server" Text="Label"></asp:Label>まで
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="naiyou">
                            <p>理由</p>
                            <td colspan="3" class="nakami">
                                <textarea id="TextArea5" cols="55" rows="5" runat="server"></textarea>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="naiyou">
                            <p>備考</p>
                            <td colspan="3" class="nakami">
                                <textarea id="TextArea6" cols="55" rows="5" runat="server"></textarea>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td id="foot" colspan="2">
                            <p>
                                上記の通り、申請いたします。
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <input type="button" id="Btn2" value="印刷" onclick="window.print();" class="noprint" />
                        </td>
                    </tr>
                </table>

            </asp:Panel>

            <asp:Panel runat="server">
                <table>
                    <tr>
                        <td>
                            <p>日付</p>
                            <td>
                                <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>月
                                <td>
                                    <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>日
                                </td>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <p>出張先</p>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
                        </td>

                    </tr>

                </table>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
