<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Shinsei.aspx.cs" Inherits="WhereEver.Calender" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <link rel="stylesheet" type="text/css" href="Shinsei.css" />
    <link rel="stylesheet" type="text/css" href="../MenuControl.css" />


    <title>申請書類</title>

    <style type="text/css">
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
                            <td class="title">申請書類</td>
                            <td class="text">
                                <asp:DropDownList ID="DropDownList1" runat="server" OnTextChanged="DropDownList_Master_SelectionChanged" AutoPostBack="True">
                                    <asp:ListItem>【申請書類を選択】</asp:ListItem>
                                    <asp:ListItem>物品購入申請</asp:ListItem>
                                    <asp:ListItem>勤怠関連申請</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="Button_Master" CssClass="btn-flat-border" runat="server" Text="申請" OnClick="DropDownList_Master_SelectionChanged" CausesValidation="False" />
                            </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="Panel2" runat="server" CssClass="noprint">
                <table id="buppintouroku">
                    <tr>
                        <td class="title">
                            <p>購入品名*</p>
                        </td>
                        <td class="text">
                            <asp:TextBox ID="TextBox_purchaseName" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" ToolTip="全角306文字以内"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_purchaseName" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_purchaseName"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>種別*</p>
                        </td>
                        <td class="text">
                            <asp:TextBox ID="TextBox_classification" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" ToolTip="全角306文字以内"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_classification" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_classification"></asp:RequiredFieldValidator>
                        </td>

                    </tr>
                    <tr>
                        <td class="title">
                            <p>購入点数*</p>
                        </td>
                        <td class="text">
                            <asp:TextBox ID="TextBox_howMany" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" ToolTip="全角305文字以内"></asp:TextBox>点
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_howMany" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_howMany"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>合計金額*</p>
                        </td>
                        <td class="text">
                            \<asp:TextBox ID="TextBox_howMach" runat="server" CssClass="textbox" Width="128px" ValidateRequestMode="Disabled" ToolTip="全角305文字以内"></asp:TextBox>-
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_howMach" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_howMach"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>購入元*</p>
                        </td>
                        <td class="text">
                            <asp:TextBox ID="TextBox_marketPlace" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" ToolTip="全角306文字以内"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_marketPlace" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_marketPlace"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>購入目的*</p>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_buy_purpose" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" TextMode="MultiLine" Rows="3" MaxLength="196" Height="100px" Width="415px" Style="resize: none" ToolTip="全角306文字以内"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_buy_purpose" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_buy_purpose"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>備考）</p>
                        </td>
                            <td class="text">
                                <asp:TextBox ID="TextBox_ps" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" TextMode="MultiLine" Rows="3" MaxLength="196" Height="100px" Width="415px" Style="resize: none" ToolTip="全角306文字以内"></asp:TextBox>
                            </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="Button1" CssClass="btn-flat-border" runat="server" Text="確定" OnClick="Button1_Click" />
                            <asp:Button ID="BtnBackA1" CssClass="btn-flat-border" runat="server" Text="閉じる" OnClick="ResetButton_Click" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="Panel3" runat="server" CssClass="noprint">
                <table runat="server" id="kyuka">
                    <tr>
                        <td class="title">
                            <p>届出内容</p>
                        </td>
                            <td class="text">
                                <asp:DropDownList ID="DropDownList_DetailsOfNotification" runat="server">
                                    <asp:ListItem>出社届</asp:ListItem>
                                    <asp:ListItem>出張届</asp:ListItem>
                                    <asp:ListItem>休暇届</asp:ListItem>
                                    <asp:ListItem>半休届</asp:ListItem>
                                    <asp:ListItem>早退届</asp:ListItem>
                                    <asp:ListItem>遅刻届</asp:ListItem>
                                </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>日時*</p>
                        </td>
                            <td class="text">

                                 <table>
                                    <tr>
                                      <td>
                                        <asp:Calendar ID="Calendar1" runat="server" OnSelectionChanged="Calendar1_SelectionChanged"></asp:Calendar>
                                        <asp:DropDownList ID="DropDownList_A_Time" runat="server" OnTextChanged="DropDownList_A_Time_SelectionChanged" AutoPostBack="True">
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

                                <asp:Label ID="lblSelectedDateA1" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lblSelectedDateA2" runat="server" Text="9:00"></asp:Label>から
                                      </td>
                                      <td>
                                        <asp:Calendar ID="Calendar2" runat="server" OnSelectionChanged="Calendar2_SelectionChanged"></asp:Calendar>
                                        <asp:DropDownList ID="DropDownList_B_Time" runat="server" OnTextChanged="DropDownList_B_Time_SelectionChanged" AutoPostBack="True">
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
                                    <asp:Label ID="lblSelectedDateB1" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="lblSelectedDateB2" runat="server" Text="9:00"></asp:Label>まで
                                      </td>
                                    </tr>
                                 </table>

                                </td>
                                <td>
                                </td>
                    </tr>

                    <tr>
                        <td class="title" runat="server">
                            <p>理由*</p>
                        </td>
                        <td class="text">
                            <asp:TextBox ID="TextBox_Notification_Purpose" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" TextMode="MultiLine" Rows="3" MaxLength="196" Height="100px" Width="415px" Style="resize: none" ToolTip="全角306文字以内"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_Notification_Purpose" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_Notification_Purpose"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>備考</p>
                        </td>
                        <td class="text">
                           <asp:TextBox ID="TextBox_Notification_ps" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" TextMode="MultiLine" Rows="3" MaxLength="196" Height="100px" Width="415px" Style="resize: none" ToolTip="全角306文字以内"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="Button2" CssClass="btn-flat-border" runat="server" Text="確定" OnClick="Button2_Click" />
                            <asp:Button ID="btnBack_A2" CssClass="btn-flat-border" runat="server" Text="閉じる" OnClick="ResetButton_Click" CausesValidation="False" />
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
                                <td class="aa">
                                    <p>申請日</p>
                                </td>
                                <td>
                                  <asp:Label ID="date" runat="server" Text="Select Date"></asp:Label>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                    </tr>
                    <tr>
                        <td id="name">
                            <asp:Label ID="name1" runat="server" Text="No Name"></asp:Label>
                        </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                    </tr>

                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td id="Syacho">
                           <p style="width: 71px">社長</p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="hanko">
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td id="yohaku">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle">
                            <p>購入品名</p>
                        </td>
                        <td colspan="3" class="zone">
                                <asp:Label ID="Konyu" runat="server" Text="購入品を記載してください" ValidateRequestMode="Disabled"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle">
                            <p>種別</p>
                        </td>
                        <td colspan="3" class="zone">
                            <asp:Label ID="Syubetsu" runat="server" Text="種別を記載してください" ValidateRequestMode="Disabled"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle">
                            <p>購入点数</p>
                        </td>
                        <td colspan="3" class="zone">
                            <asp:Label ID="Suryo" runat="server" Text="購入点数を記載してください" ValidateRequestMode="Disabled"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle">
                            <p>合計金額</p>
                        </td>
                        <td colspan="3" class="zone">
                            <asp:Label ID="Kingaku" runat="server" Text="合計金額を記載してください" ValidateRequestMode="Disabled"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td class="TableTitle">
                            <p>購入元</p>
                        </td>
                        <td colspan="3" class="zone">
                            <asp:Label ID="KonyuSaki" runat="server" Text="購入元を記載してください" ValidateRequestMode="Disabled"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle">
                            <p>購入理由/目的</p>
                        </td>
                        <td colspan="3" class="area">
                           <asp:Label ID="Label_Riyuu" runat="server" Text="" ValidateRequestMode="Disabled" Height="100px" Width="415px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle">
                            <p>備考</p>
                        </td>
                        <td colspan="3" class="area">
                            <asp:Label ID="Label_Bikou" runat="server" Text="" ValidateRequestMode="Disabled" Height="100px" Width="415px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <p>上記の通り申請致します。</p>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <%-- lllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll --%>
            <asp:Panel ID="Panel5" runat="server">
                <table id="Todoke">
                    <tr>
                        <td colspan="4" id="top">
                            <asp:Label ID="lblDiligenceClassification1" runat="server" Text="届ける項目を選択してください" ValidateRequestMode="Disabled"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td id="date2" class="takasa">
                            <p>申請日</p>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblDiligenceDate" runat="server" Text="DiligenceDate"></asp:Label>
                        </td>
                    </tr>
                    <tr class="takasa">
                        <td colspan="2" id="name2">
                            <asp:Label ID="lblDiligenceUser" runat="server" Text=""></asp:Label><asp:Label ID="Label13" runat="server" Text="（印）"></asp:Label>
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td id="Syacho2">
                                  <p>社長</p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td id="yohaku5">
                        </td>
                        <td id="yohaku4">
                        </td>
                        <td class="hanko2">
                        </td>
                    </tr>
                    <tr>
                        <td id="yohaku3" colspan="4"></td>
                    </tr>
                    <tr>
                        <td class="naiyou">
                            <p>届出内容</p>
                        </td>
                        <td colspan="3" runat="server" class="nakami">
                            <asp:Label ID="lblDiligenceClassification2" runat="server" Text="項目を選択してください"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="naiyou">
                            <p>日時</p>
                        </td>
                        <td colspan="3" class="nakami">
                            <asp:Label ID="lblDiligenceDateA1" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lblDiligenceDateA2" runat="server" Text=""></asp:Label>から<br />
                            <asp:Label ID="lblDiligenceDateB1" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lblDiligenceDateB2" runat="server" Text=""></asp:Label>まで
                        </td>
                    </tr>
                    <tr>
                        <td class="naiyou">
                            <p>理由</p>
                        </td>
                        <td colspan="3" class="nakami">
                           <asp:Label ID="Label_Diligence_because" runat="server" Text="" ValidateRequestMode="Disabled" Height="100px" Width="415px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="naiyou">
                            <p>備考</p>
                        </td>
                        <td colspan="3" class="nakami">
                           <asp:Label ID="Label_Diligence_ps" runat="server" Text="" ValidateRequestMode="Disabled" Height="100px" Width="415px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td id="foot" colspan="2">
                            <p>
                                上記の通り、申請いたします。
                            </p>
                        </td>
                    </tr>
                </table>

            </asp:Panel>

<%--            <asp:Panel ID="Panel6" runat="server" CssClass="noprint">
                <table>
                    <tr>
                        <td style="background-color:green">
                            <p>日付</p>
                        </td>
                            <td>
                                <asp:TextBox ID="TextBox_TripMonth" runat="server"></asp:TextBox>月
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_TripDay" runat="server"></asp:TextBox>日
                            </td>

                    </tr>
                    <tr>
                        <td>
                            <p>出張先</p>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_TripArea" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>--%>

            <asp:Panel ID="Panel_Print" runat="server" CssClass="noprint">

                <div id="fotter">
                <p class="noprint">
                    <input type="button" class="btn-flat-border" value="印刷" onclick="window.print();" />
                    <asp:Button ID="Btn_BackMaster" CssClass="btn-flat-border" runat="server" Text="閉じる" OnClick="BackButton_Click" CausesValidation="False" />
                    　※印刷プレビューには申請書のデータが反映されます。
                </p>
                </div>

            </asp:Panel>

        </div>
    </form>
</body>
</html>
