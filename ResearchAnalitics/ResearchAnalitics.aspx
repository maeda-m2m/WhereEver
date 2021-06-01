<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResearchAnalitics.aspx.cs" Inherits="WhereEver.ResearchAnalitics.ResearchAnalitics" %>
<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../MenuControl.css" />
    <link rel="stylesheet" type="text/css" href="ResearchAnalitics.css" />
    <title>分析</title>
</head>
<body>
    <form id="form1" runat="server">

        <div class="noprint">
            <table>
                <tr>
                    <td id="menu">
                        <menu:c_menu id="m" runat="server"></menu:c_menu>
                    </td>
                </tr>
            </table>
       </div>

        <div id="Wrap">

            <div class="noprint">
           <span class="hr"></span>

           <p class="index1">
               ◆ディープラーニング
               <asp:Button ID="Button_DP" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_DP" CausesValidation="False" />
               　手軽にディープラーニングを試すことができます。</p>

           <hr />
</div>

<asp:Panel ID="Panel_DP" runat="server" Visible="false" DefaultButton="Button_SetDeepRun">

<div class="noprint">
                       <span class="hr"></span>

    <p class="center">最小二乗法による誤差逆伝播法（バックプロパゲーション）を同期処理します（本来はAsyncで実行したほうが効率的です）。</p>

                       <span class="hr"></span>
</div>

<div class="center">
　<p class="right">（テスト版）</p>
        <table class="DGTable">
            <tr>
                <th colspan="2" class="th_master">
                    誤差逆伝播法設定パネル
                </th>
            </tr>
            <tr>
                <td>
                    学習回数(int>0)
                </td>
                <td>
                    <asp:TextBox ID="TextBox_Learn" runat="server" CssClass="textbox_math"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="30" MaxLength="6"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    学習率(double>0)
                </td>
                <td>
                    <asp:TextBox ID="TextBox_Rate" runat="server" CssClass="textbox_math"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="0.1" MaxLength="6"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    誤差逆伝播法
                </td>
                <td>
                    <asp:CheckBox ID="CheckBox_deep" runat="server" Text="使用する" Checked="true" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="TextBox_DeepResult" runat="server" CssClass="textbox_Wide" ValidateRequestMode="Disabled" Text="Ready..." CausesValidation="false" TextMode="MultiLine" Style="resize: none" ReadOnly="true" ></asp:TextBox>
                </td>
            </tr>
             <tr>
                <td colspan="2">
                    <asp:Button ID="Button_SetDeepRun" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_Deep" CausesValidation="False" />
                </td>
            </tr>
        </table>
</div>
</asp:Panel>

        </div>
    </form>
</body>
</html>
