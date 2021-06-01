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
    <p class="center">勾配∂E/∂wの情報をもとにパラメータwを更新。損失Eを最小にしたい。τ回目のパラメータの値を w^(τ)としたとき、w^(τ+1)=w^(τ)−η(∂E/∂w)(w^(τ))</p>

                       <span class="hr"></span>
</div>

<div class="center">
　<p class="right">（Ver.0.8β）</p>
        <table class="DGTable">
            <tr>
                <th colspan="2" class="th_master">
                    誤差逆伝播法設定パネル
                </th>
            </tr>
            <tr>
                <td>
                    学習回数τ(int>0)
                </td>
                <td>
                    <asp:TextBox ID="TextBox_Learn" runat="server" CssClass="textbox_math"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="1000" MaxLength="6"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    学習率η(double>0.0)
                </td>
                <td>
                    <asp:TextBox ID="TextBox_Rate" runat="server" CssClass="textbox_math"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="0.1" MaxLength="6"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    教師信号T(double>0.0)
                </td>
                <td>
                    {0,0,<asp:TextBox ID="TextBox_T" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="0.9" MaxLength="6"></asp:TextBox>,0}
                </td>
            </tr>
            <tr>
                <td>
                    入力値X(double)
                </td>
                <td>
                    {<asp:TextBox ID="TextBox_X1" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="6.3" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_X2" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="10.4" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_X3" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="11.1" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_X4" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="16.4" MaxLength="6"></asp:TextBox>}
                </td>
            </tr>
            <tr>
                <td>
                    入力値W1_1(double)
                </td>
                <td>
                    {<asp:TextBox ID="TextBox_W1_1_1" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="4.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W1_1_2" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="2.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W1_1_3" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="6.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W1_1_4" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="1.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W1_1_5" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="5.0" MaxLength="6"></asp:TextBox>}
                </td>
            </tr>
            <tr>
                <td>
                    入力値W1_2(double)
                </td>
                <td>
                    {<asp:TextBox ID="TextBox_W1_2_1" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="1.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W1_2_2" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="3.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W1_2_3" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="7.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W1_2_4" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="2.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W1_2_5" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="1.0" MaxLength="6"></asp:TextBox>}
                </td>
            </tr>
            <tr>
                <td>
                    入力値W1_3(double)
                </td>
                <td>
                    {<asp:TextBox ID="TextBox_W1_3_1" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="4.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W1_3_2" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="2.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W1_3_3" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="6.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W1_3_4" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="1.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W1_3_5" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="5.0" MaxLength="6"></asp:TextBox>}
                </td>
            </tr>
            <tr>
                <td>
                    入力値W1_4(double)
                </td>
                <td>
                    {<asp:TextBox ID="TextBox_W1_4_1" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="1.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W1_4_2" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="3.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W1_4_3" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="7.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W1_4_4" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="2.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W1_4_5" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="1.0" MaxLength="6"></asp:TextBox>}
                </td>
            </tr>

            <tr>
                <td>
                    入力値B1(double)
                </td>
                <td>
                    {<asp:TextBox ID="TextBox_B1_1" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="2.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_B1_2" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="6.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_B1_3" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="4.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_B1_4" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="4.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_B1_5" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="1.0" MaxLength="6"></asp:TextBox>}
                </td>
            </tr>

            <tr>
                <td>
                    入力値W2_1(double)
                </td>
                <td>
                    {<asp:TextBox ID="TextBox_W2_1_1" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="3.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W2_1_2" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="5.0" MaxLength="6"></asp:TextBox>}
                </td>
            </tr>
            <tr>
                <td>
                    入力値W2_2(double)
                </td>
                <td>
                    {<asp:TextBox ID="TextBox_W2_2_1" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="6.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W2_2_2" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="3.0" MaxLength="6"></asp:TextBox>}
                </td>
            </tr>
            <tr>
                <td>
                    入力値W2_3(double)
                </td>
                <td>
                    {<asp:TextBox ID="TextBox_W2_3_1" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="1.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W2_3_2" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="5.0" MaxLength="6"></asp:TextBox>}
                </td>
            </tr>
            <tr>
                <td>
                    入力値W2_4(double)
                </td>
                <td>
                    {<asp:TextBox ID="TextBox_W2_4_1" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="3.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W2_4_2" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="5.0" MaxLength="6"></asp:TextBox>}
                </td>
            </tr>
            <tr>
                <td>
                    入力値W2_5(double)
                </td>
                <td>
                    {<asp:TextBox ID="TextBox_W2_5_1" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="6.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W2_5_2" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="3.0" MaxLength="6"></asp:TextBox>}
                </td>
            </tr>


            <tr>
                <td>
                    入力値B2(double)
                </td>
                <td>
                    {<asp:TextBox ID="TextBox_B2_1" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="6.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_B2_2" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="3.0" MaxLength="6"></asp:TextBox>}
                </td>
            </tr>

            <tr>
                <td>
                    入力値W3_1(double)
                </td>
                <td>
                    {<asp:TextBox ID="TextBox_W3_1_1" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="7.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W3_1_2" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="5.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W3_1_3" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="7.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W3_1_4" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="6.0" MaxLength="6"></asp:TextBox>}
                </td>
            </tr>

            <tr>
                <td>
                    入力値W3_2(double)
                </td>
                <td>
                    {<asp:TextBox ID="TextBox_W3_2_1" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="4.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W3_2_2" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="2.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W3_2_3" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="4.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_W3_2_4" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="2.0" MaxLength="6"></asp:TextBox>}
                </td>
            </tr>

            <tr>
                <td>
                    入力値B3(double)
                </td>
                <td>
                    {<asp:TextBox ID="TextBox_B3_1" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="4.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_B3_2" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="6.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_B3_3" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="5.0" MaxLength="6"></asp:TextBox>
                    <asp:TextBox ID="TextBox_B3_4" runat="server" CssClass="textbox_math_s"  ValidateRequestMode="Disabled" ToolTip="全角6文字以内" Text="4.0" MaxLength="6"></asp:TextBox>}
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <asp:TextBox ID="TextBox_DeepResult" runat="server" CssClass="textbox_Wide" ValidateRequestMode="Disabled" Text="Ready..." CausesValidation="false" TextMode="MultiLine" Style="resize: none" ReadOnly="true" ></asp:TextBox>
                </td>
            </tr>
             <tr>
                <td colspan="2">
                    <asp:Button ID="Button_SetDeepRun" CssClass="btn-flat-border" runat="server" Text="実行" OnClick="Push_Deep" CausesValidation="False" />
                </td>
            </tr>
        </table>
</div>
</asp:Panel>

        </div>
    </form>
</body>
</html>
