<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Money.aspx.cs" Inherits="WhereEver.Money.WebForm1" %>
<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="application/json; charset=utf-8"/>
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../MenuControl.css" />
    <link rel="stylesheet" type="text/css" href="Money.css" />
    <title>勘定システム</title>
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
               ◆損益計算書(P/L)
               <asp:Button ID="Button_PL" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_PL_test" CausesValidation="False" />
               　手軽に黒字か赤字かを見極めることができます。</p>

           <hr />
</div>

<asp:Panel ID="Panel_PL" runat="server" Visible="false" DefaultButton="Button_PL_SUM">

<div class="noprint">
                       <span class="hr"></span>

    <p class="center">一般的なテンプレートを用いてP/Lを作成します。すべて正の値で入力して下さい。</p>

                       <span class="hr"></span>
</div>

<div class="center">
　<p class="right">（単位：円）</p>
        <table class="DGTable">
            <tr>
                <th colspan="2" class="th_master">
                    損益計算書(P/L)
                </th>
            </tr>
            <tr>
                <th colspan="2">
                    出
                    <asp:DropDownList ID="DropDownList_PL_year_s" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true" CssClass="ddl_date" ></asp:DropDownList>年
                    <asp:DropDownList ID="DropDownList_PL_month_s" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true" CssClass="ddl_date" ></asp:DropDownList>月
                    <asp:DropDownList ID="DropDownList_PL_day_s" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true" CssClass="ddl_date" ></asp:DropDownList>日
                    <br />
                    至
                    <asp:DropDownList ID="DropDownList_PL_year_g" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true" CssClass="ddl_date" ></asp:DropDownList>年
                    <asp:DropDownList ID="DropDownList_PL_month_g" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true" CssClass="ddl_date" ></asp:DropDownList>月
                    <asp:DropDownList ID="DropDownList_PL_day_g" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true" CssClass="ddl_date" ></asp:DropDownList>日
                </th>
            </tr>
            <tr>
                <td rowspan="2" class="td_master">
                    売上高<br /><span class="minus">売上原価</span>
                </td>
                <td>
                    <asp:TextBox ID="TextBox_Uriage" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="TextBox_UriageGenka" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="sum">
                    売上総利益
                </td>
                <td class="right">
                    <asp:Label ID="Label_UriageSourieki" runat="server" Text="0" CssClass="lbl_pl"></asp:Label>
                </td>
             </tr>
             <tr>
                <td class="minus">
                    販売管理費
                </td>
                <td>
                    <asp:TextBox ID="TextBox_HanbaiKanrihi" runat="server" CssClass="textbox_pl"  ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" MaxLength="50"></asp:TextBox>
                </td>
             </tr>
             <tr>
                <td>
                    営業利益
                </td>
                <td>
                    <asp:TextBox ID="TextBox_EigyouRieki" runat="server" CssClass="textbox_pl"  ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" MaxLength="50"></asp:TextBox>
                </td>
             </tr>
             <tr>
                <td class="minus">
                    営業外費用
                </td>
                <td>
                    <asp:TextBox ID="TextBox_EigyougaiHiyou" runat="server" CssClass="textbox_pl"  ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" MaxLength="50"></asp:TextBox>
                </td>
             </tr>
             <tr>
                <td class="sum">
                    経常利益
                </td>
                <td class="right">
                    <asp:Label ID="Label_KeijyouRieki" runat="server" Text="0" CssClass="lbl_pl"></asp:Label>
                </td>
             </tr>
             <tr>
                <td rowspan="2">
                    特別利益<br /><span class="minus">特別損失</span>
                </td>
                <td>
                    <asp:TextBox ID="TextBox_TokubetsuRieki" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" MaxLength="50"></asp:TextBox>
                </td>
             </tr>
             <tr>
                <td>
                    <asp:TextBox ID="TextBox_TokubetsuSonshitsu" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" MaxLength="50"></asp:TextBox>
                </td>
             </tr>
             <tr>
                <td class="sum">
                    税引前当期純利益
                </td>
                <td class="right">
                    <asp:Label ID="Label_Zeibikimae" runat="server" Text="0" CssClass="lbl_pl"></asp:Label>
                </td>
             </tr>
             <tr>
                <td>
                    <span class="minus">法人税等</span>
                </td>
                <td>
                    <asp:TextBox ID="TextBox_Houjinzei" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" MaxLength="50"></asp:TextBox>
                </td>
             </tr>
             <tr>
                <td class="sum">
                    当期純利益
                </td>
                <td class="right">
                    <asp:Label ID="Label_Jyunrieki" runat="server" Text="0" CssClass="lbl_pl"></asp:Label>
                </td>
             </tr>
             <tr>
                <td class="per">
                    売上総利益率（粗利率）
                </td>
                <td class="right">
                    <asp:Label ID="Label_ArariR" runat="server" Text="0.0%" CssClass="lbl_pl"></asp:Label>
                </td>
             </tr>
             <tr>
                <td class="per">
                    売上高営業利益率
                </td>
                <td class="right">
                    <asp:Label ID="Label_EigyouR" runat="server" Text="0.0%" CssClass="lbl_pl"></asp:Label>
                </td>
             </tr>
             <tr>
                <td class="per">
                    売上高経常利益率
                </td>
                <td class="right">
                    <asp:Label ID="Label_KeijyouR" runat="server" Text="0.0%" CssClass="lbl_pl"></asp:Label>
                </td>
             </tr>
        </table>
</div>




<div class="noprint">

        <p class="center">
            <asp:Button ID="Button_Check_PL" CssClass="btn-flat-border" runat="server" Text="新規保存" OnClick="Push_Check_PL" CausesValidation="False" />
            <asp:Button ID="Button_CheckAS_PL" CssClass="btn-flat-border" runat="server" Text="上書き保存" OnClick="Push_CheckAS_PL" CausesValidation="False" />
            <asp:Button ID="Button_PL_SUM" CssClass="btn-flat-border" runat="server" Text="小計/合計" OnClick="Change_PL" CausesValidation="False" />
            <input type="button" class="btn-flat-border" value="印刷" onclick="window.print();" />
        </p>

  <span class="hr"></span>

  <p class="index1">◆P/L一覧</p>
　<p class="right">（単位：円）</p>
        <asp:GridView ID="GridView_PL" runat="server" CssClass="DGTable" AutoGenerateColumns="False" DataKeyNames="uuid" DataSourceID="SqlDataSource_PL" AllowPaging="True" AllowSorting="True" OnRowCommand="Grid_RowCommand">
            <Columns>
                <asp:BoundField DataField="uuid" HeaderText="uuid" ReadOnly="True" SortExpression="uuid" HeaderStyle-CssClass="none" ItemStyle-CssClass="none" />
                <asp:BoundField DataField="uriagedaka" HeaderText="売上高" SortExpression="uriagedaka" DataFormatString="{0:C}" />
                <asp:BoundField DataField="uriagegenka" HeaderText="売上原価" SortExpression="uriagegenka" DataFormatString="{0:C}" HeaderStyle-ForeColor="Red" Visible="false" />
                <asp:BoundField DataField="uriagesourieki" HeaderText="売上総利益" SortExpression="uriagesourieki" DataFormatString="{0:C}" />
                <asp:BoundField DataField="hanbaikanrihi" HeaderText="販管費" SortExpression="hanbaikanrihi" DataFormatString="{0:C}" HeaderStyle-ForeColor="Red" Visible="false" />
                <asp:BoundField DataField="eigyourieki" HeaderText="営業利益" SortExpression="eigyourieki" DataFormatString="{0:C}" />
                <asp:BoundField DataField="eigyougaihiyou" HeaderText="営業外費用" SortExpression="eigyougaihiyou" DataFormatString="{0:C}" HeaderStyle-ForeColor="Red" Visible="false" />
                <asp:BoundField DataField="keijyourieki" HeaderText="経常利益" SortExpression="keijyourieki" DataFormatString="{0:C}" />
                <asp:BoundField DataField="tokubetsurieki" HeaderText="特利" SortExpression="tokubetsurieki" DataFormatString="{0:C}" Visible="false" />
                <asp:BoundField DataField="tokubetsusonshitsu" HeaderText="特損" SortExpression="tokubetsusonshitsu" DataFormatString="{0:C}" HeaderStyle-ForeColor="Red" Visible="false" />
                <asp:BoundField DataField="zeibikimaetoukijyunrieki" HeaderText="税引前" SortExpression="zeibikimaetoukijyunrieki" DataFormatString="{0:C}" />
                <asp:BoundField DataField="houjinzeitou" HeaderText="法人税等" SortExpression="houjinzeitou" DataFormatString="{0:C}" HeaderStyle-ForeColor="Red" Visible="false" />
                <asp:BoundField DataField="toukijyunrieki" HeaderText="当期純利益" SortExpression="toukijyunrieki" DataFormatString="{0:C}" />
                <asp:BoundField DataField="arari_r" HeaderText="粗利率" SortExpression="arari_r" DataFormatString="{0:0.0%}" HeaderStyle-ForeColor="LightGreen" />
                <asp:BoundField DataField="eigyou_r" HeaderText="営業利益率" SortExpression="eigyou_r" DataFormatString="{0:0.0%}" HeaderStyle-ForeColor="LightGreen" />
                <asp:BoundField DataField="keijyou_r" HeaderText="経常利益率" SortExpression="keijyou_r" DataFormatString="{0:0.0%}" HeaderStyle-ForeColor="LightGreen" />
                <asp:BoundField DataField="Date_S" HeaderText="出" SortExpression="Date_S" DataFormatString="{0:d}" />
                <asp:BoundField DataField="Date_G" HeaderText="至" SortExpression="Date_G" DataFormatString="{0:d}" />
                <asp:BoundField DataField="UpDateTime" HeaderText="最終更新日" SortExpression="UpDateTime" />

                    <asp:ButtonField ButtonType="Button" Text="削除" HeaderText="削除" CommandName="Remove" CausesValidation="False" >
                    <ControlStyle CssClass="btn-flat-border-mini" />
                    </asp:ButtonField>

                    <asp:ButtonField ButtonType="Button" Text="参照" HeaderText="編集" CommandName="DownLoad" CausesValidation="False" >
                    <ControlStyle CssClass="btn-flat-border-mini" />
                    </asp:ButtonField>

            </Columns>
        <HeaderStyle BackColor="Black" ForeColor="White" />
        <RowStyle BackColor="#1E1E1E" ForeColor="White" />
        </asp:GridView>

        <asp:SqlDataSource ID="SqlDataSource_PL" runat="server" ConnectionString="<%$ ConnectionStrings:WhereverConnectionString %>" SelectCommand="SELECT * FROM [T_PL] ORDER BY [UpDateTime] DESC"></asp:SqlDataSource>

</div>

</asp:Panel>

<div class="noprint">
           <span class="hr"></span>

           <p class="index1">
               ◆貸借対照表(B/S)
               <asp:Button ID="Button_BS" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_BS_test" CausesValidation="False" />
               &nbsp;　無駄遣いや自己資本比率が一目でわかります。</p>

           <hr />
</div>

<asp:Panel ID="Panel_BS" runat="server" Visible="false" DefaultButton="Button_BS_SUM">

    <div class="noprint">
           <span class="hr"></span>
            <p class="center">一般的なテンプレートを用いてB/Sを作成します。すべて正の値で入力して下さい。資産合計と負債・純資産合計が同じになるように作成して下さい。</p>
           <span class="hr"></span>
    </div>

    <div class="center"><a name="BS_TOP"></a>
　<p class="right">（単位：円）</p>
        <table class="DGTable">
            <tr>
                <th colspan="4" class="th_master">
                    貸借対照表(B/S)
                </th>
            </tr>
            <tr>
                <th colspan="4" class="th_master">
                    <asp:DropDownList ID="DropDownList_BS_year" runat="server" OnSelectedIndexChanged="Change_BS" CssClass="ddl_date" ></asp:DropDownList>年
                    <asp:DropDownList ID="DropDownList_BS_month" runat="server" OnSelectedIndexChanged="Change_BS" CssClass="ddl_date" ></asp:DropDownList>月
                    <asp:DropDownList ID="DropDownList_BS_day" runat="server" OnSelectedIndexChanged="Change_BS" CssClass="ddl_date" ></asp:DropDownList>日
                </th>
            </tr>
            <tr>
                <td class="td_master_q">
                    科目
                </td>
                <td class="td_master_q">
                    金額
                </td>
                <td class="td_master_q">
                    科目
                </td>
                <td class="td_master_q">
                    金額
                </td>
            </tr>
            <tr>
                <td>
                    （資産の部）
                </td>
                <td class="right">
                    <asp:Label ID="Label_BS_Shisan" runat="server" Text="0" CssClass="lbl_BS"></asp:Label>円
                </td>
                <td>
                    （負債の部）
                </td>
                <td class="right">
                    <asp:Label ID="Label_BS_Fusai" runat="server" Text="0" CssClass="lbl_BS"></asp:Label>円
                </td>
            </tr>
            <tr>
                <td>
                    流動資産
                </td>
                <td class="right">
                    <asp:Label ID="Label_BS_RyuudouShisan" runat="server" Text="0" CssClass="lbl_BS"></asp:Label>
                </td>
                <td>
                    流動負債
                </td>
                <td class="right">
                    <asp:Label ID="Label_BS_RyuudouFusai" runat="server" Text="0" CssClass="lbl_BS"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    現金及び預金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS1" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    買掛金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS29" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    受取手形
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS2" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    短期借入金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS30" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    売掛金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS3" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    未払金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS31" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    商品
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS4" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    未払費用
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS32" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    部品
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS5" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    未払法人税等
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS33" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    前払費用
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS6" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    預り金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS34" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    繰延税金資産
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS7" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    賞与引当金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS35" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    短期貸付金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS8" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    製品保証引当金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS36" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    未収入金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS9" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    その他
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS37" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    その他
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS10" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    
                </td>
                <td class="right">
                    
                </td>
            </tr>
            <tr>
                <td>
                    貸倒引当金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS11" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    固定負債
                </td>
                <td class="right">
                    <asp:Label ID="Label_BS_KoteiFusai" runat="server" Text="0" CssClass="lbl_BS"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    
                </td>
                <td class="right">
                    
                </td>
                <td>
                    退職給費引当金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS38" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    固定資産
                </td>
                <td class="right">
                    <asp:Label ID="Label_BS_KoteiShisan" runat="server" Text="0" CssClass="lbl_BS"></asp:Label>
                </td>
                <td>
                    繰延税金負債
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS39" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    有形固定資産
                </td>
                <td class="right">
                    <asp:Label ID="Label_BS_YuukeiKoteiShisan" runat="server" Text="0" CssClass="lbl_BS"></asp:Label>
                </td>
                <td>
                    その他
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS40" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    建物
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS12" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    
                </td>
                <td class="right">
                    
                </td>
            </tr>
            <tr>
                <td>
                    構築物
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS13" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    負債合計
                </td>
                <td class="right">
                    <asp:Label ID="Label_BS_FusaiGoukei" runat="server" Text="0" CssClass="lbl_BS"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    機械及び装置
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS14" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    
                </td>
                <td class="right">
                    
                </td>
            </tr>
            <tr>
                <td>
                    車両及び運搬具
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS15" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    （総資産の部）
                </td>
                <td class="right">
                    
                </td>
            </tr>
            <tr>
                <td>
                    工具、器具及び備品
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS16" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    株主資本
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS41" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    土地
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS17" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    資本金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS42" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    建設仮勘定
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS18" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    資本余剰金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS43" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    
                </td>
                <td class="right">
                    
                </td>
                <td>
                    資本準備金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS44" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    無形固定資産
                </td>
                <td class="right">
                    <asp:Label ID="Label_BS_MukeiKoteiShisan" runat="server" Text="0" CssClass="lbl_BS"></asp:Label>
                </td>
                <td>
                    その他資本剰余金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS45" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    施設利用権
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS19" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    
                </td>
                <td class="right">
                    
                </td>
            </tr>
            <tr>
                <td>
                    ソフトウェア
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS20" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    利益余剰金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS46" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    その他
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS21" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    その他利益剰余金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS47" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    
                </td>
                <td class="right">
                    
                </td>
                <td>
                    繰越利益剰余金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS48" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    投資その他の資産
                </td>
                <td class="right">
                    <asp:Label ID="Label_BS_ToushiSonotanoShisan" runat="server" Text="0" CssClass="lbl_BS"></asp:Label>
                </td>
                <td>
                    
                </td>
                <td class="right">
                    
                </td>
            </tr>
            <tr>
                <td>
                    投資有価証券
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS22" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    自己株式
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS49" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    関係会社株式
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS23" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    
                </td>
                <td class="right">
                    
                </td>
            </tr>
            <tr>
                <td>
                    関係会社出資金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS24" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    評価・換算差額等
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS50" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    長期貸付金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS25" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50" ></asp:TextBox>
                </td>
                <td>
                    その他有価証券評価差額金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS51" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    長期前払費用
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS26" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    
                </td>
                <td class="right">
                    
                </td>
            </tr>
            <tr>
                <td>
                    その他
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS27" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    純資産合計
                </td>
                <td class="right">
                    <asp:Label ID="Label_BS_JyunshisanGoukei" runat="server" Text="0" CssClass="lbl_BS"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    貸倒引当金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS28" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" MaxLength="50"></asp:TextBox>
                </td>
                <td>
                    
                </td>
                <td class="right">
                    
                </td>
            </tr>
            <tr>
                <td>
                    
                </td>
                <td class="right">
                    
                </td>
                <td>
                    
                </td>
                <td class="right">
                    
                </td>
            </tr>
            <tr>
                <td>
                    資産合計
                </td>
                <td class="right">
                    <asp:Label ID="Label_BS_ShisanGoukei" runat="server" Text="0" CssClass="lbl_BS"></asp:Label>
                </td>
                <td>
                    負債・純資産合計
                </td>
                <td class="right">
                    <asp:Label ID="Label_BS_Fusai_JyunshisanGoukei" runat="server" Text="0" CssClass="lbl_BS"></asp:Label>
                </td>
            </tr>
        </table>
    </div>


<div class="noprint">

        <p class="center">
            <asp:Button ID="Button_Check_BS" CssClass="btn-flat-border" runat="server" Text="新規保存" OnClick="Push_Check_BS" CausesValidation="False" />
            <asp:Button ID="Button_CheckAS_BS" CssClass="btn-flat-border" runat="server" Text="上書き保存" OnClick="Push_CheckAS_BS" CausesValidation="False" />
            <asp:Button ID="Button_BS_SUM" CssClass="btn-flat-border" runat="server" Text="小計/合計" OnClick="Change_BS" CausesValidation="False" />
            <input type="button" class="btn-flat-border" value="印刷" onclick="window.print();" />
        </p>


            <asp:GridView ID="GridView_BS" runat="server" CssClass="DGTable" AutoGenerateColumns="False" DataKeyNames="uuid" DataSourceID="SqlDataSource_BS" AllowPaging="True" AllowSorting="True" OnRowCommand="Grid_RowCommand">
                <Columns>
                    <asp:BoundField DataField="uuid" HeaderText="uuid" ReadOnly="True" SortExpression="uuid" />
                    <asp:BoundField DataField="shisan_a" HeaderText="資産合計" SortExpression="shisan_a" DataFormatString="{0:C}" HeaderStyle-ForeColor="LightGreen" />
                    <asp:BoundField DataField="fusai_a" HeaderText="負債合計" SortExpression="fusai_a" DataFormatString="{0:C}" HeaderStyle-ForeColor="Red" />
                    <asp:BoundField DataField="jyunshisan_a" HeaderText="純資産" SortExpression="jyunshisan_a" DataFormatString="{0:C}" HeaderStyle-ForeColor="LightBlue" />
                    <asp:BoundField DataField="Date" HeaderText="申告年月日" SortExpression="Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="UpDateTime" HeaderText="最終更新日" SortExpression="UpDateTime" />

                    <asp:ButtonField ButtonType="Button" Text="削除" HeaderText="削除" CommandName="BSRemove" CausesValidation="False" >
                    <ControlStyle CssClass="btn-flat-border-mini" />
                    </asp:ButtonField>

                    <asp:ButtonField ButtonType="Button" Text="参照" HeaderText="編集" CommandName="BSDownLoad" CausesValidation="False" >
                    <ControlStyle CssClass="btn-flat-border-mini" />
                    </asp:ButtonField>

            </Columns>
        <HeaderStyle BackColor="Black" ForeColor="White" />
        <RowStyle BackColor="#1E1E1E" ForeColor="White" />
            </asp:GridView>

           <asp:SqlDataSource ID="SqlDataSource_BS" runat="server" ConnectionString="<%$ ConnectionStrings:WhereverConnectionString %>" SelectCommand="SELECT [uuid], [shisan_a], [fusai_a], [jyunshisan_a], [Date], [UpDateTime] FROM [T_BS] ORDER BY [UpDateTime] DESC"></asp:SqlDataSource>

</div>

</asp:Panel>

<div class="noprint">
           <span class="hr"></span>


           <p class="index1">
               ◆キャッシュフロー(C/F)
               <asp:Button ID="Button_CF" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_CF_test" CausesValidation="False" />
               　P/Lでは見抜けない企業のお金の流れを見分けられます。</p>

           <hr />
</div>

<asp:Panel ID="Panel_CF" runat="server" Visible="false" DefaultButton="Button_CF_SUM">

<div class="noprint">
           <span class="hr"></span>
            <p class="center">一般的なテンプレートを用いてC/Fを作成します。正の値または負の値で入力して下さい。</p>
           <span class="hr"></span>
</div>



    <div class="center"><a name="BS_TOP"></a>
　<p class="right">（単位：円）</p>
        <table class="DGTable">
            <tr>
                <th colspan="2" class="th_master">
                    財務活動によるキャッシュフロー(C/F)
                </th>
            </tr>
            <tr>
                <th colspan="2" class="th_master">
                    <asp:DropDownList ID="DropDownList_CF_year" runat="server" OnSelectedIndexChanged="Change_CF" CssClass="ddl_date" ></asp:DropDownList>年
                    <asp:DropDownList ID="DropDownList_CF_month" runat="server" OnSelectedIndexChanged="Change_CF" CssClass="ddl_date" ></asp:DropDownList>月
                    <asp:DropDownList ID="DropDownList_CF_day" runat="server" OnSelectedIndexChanged="Change_CF" CssClass="ddl_date" ></asp:DropDownList>日
                </th>
            </tr>
            <tr>
                <th class="th_master">
                    区分
                </th>
                <th class="th_master">
                    金額
                </th>
            </tr>
            <tr>
                <td class="th_master">
                    (1)営業活動によるキャッシュフロー
                </td>
                <td class="th_master">
                </td>
            </tr>
            <tr>
                <td class="sum">
                    税引き前当期純利益　＋
                </td>
                <td>
                    <asp:TextBox ID="TextBox_CF1" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_CF" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="sum">
                    減価償却費　＋
                </td>
                <td class="td_master_q">
                    <asp:TextBox ID="TextBox_CF2" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_CF" MaxLength="50"></asp:TextBox>                  
                </td>
            </tr>
            <tr>
                <td class="minus">
                    売上債権の増加　－
                </td>
                <td>
                    <asp:TextBox ID="TextBox_CF3" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_CF" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="minus">
                    棚卸資産の増加　－
                </td>
                <td>
                    <asp:TextBox ID="TextBox_CF4" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_CF" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="sum">
                    社入債務の増加　＋
                </td>
                <td>
                    <asp:TextBox ID="TextBox_CF5" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_CF" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="minus">
                    法人税等の支払い　－
                </td>
                <td>
                    <asp:TextBox ID="TextBox_CF6" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_CF" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="per">
                    営業営業活動によるキャッシュ・フロー
                </td>
                <td class="right">
                    <asp:Label ID="Label_CF1" runat="server" Text="0" CssClass="lbl_BS"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="th_master">
                    (2)投資活動によるキャッシュフロー
                </td>
                <td class="th_master">
                </td>
            </tr>
            <tr>
                <td class="sum">
                    有形固定資産の購入　－
                </td>
                <td>
                    <asp:TextBox ID="TextBox_CF7" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_CF" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="minus">
                    有形固定資産の売却　＋
                </td>
                <td>
                    <asp:TextBox ID="TextBox_CF8" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_CF" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="sum">
                    有価証券の購入　－
                </td>
                <td>
                    <asp:TextBox ID="TextBox_CF9" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_CF" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="minus">
                    有価証券の売却及び満期償還　＋
                </td>
                <td>
                    <asp:TextBox ID="TextBox_CF10" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_CF" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="per">
                    投資営業活動によるキャッシュ・フロー
                </td>
                <td class="right">
                    <asp:Label ID="Label_CF2" runat="server" Text="0" CssClass="lbl_BS"></asp:Label>                   
                </td>
            </tr>
            <tr>
                <td class="th_master">
                    (3)財務活動によるキャッシュ・フロー
                </td>
                <td class="th_master">
                </td>
            </tr>
            <tr>
                <td class="even">
                    借入金の増加　＋
                </td>
                <td>
                    <asp:TextBox ID="TextBox_CF11" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_CF" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="even">
                    借入金の返済　－
                </td>
                <td>
                    <asp:TextBox ID="TextBox_CF12" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_CF" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="per">
                    財務営業活動によるキャッシュ・フロー
                </td>
                <td class="right">
                    <asp:Label ID="Label_CF3" runat="server" Text="0" CssClass="lbl_BS"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    (4)現金及び現金同等物等の増加額
                </td>
                <td class="right">
                    <asp:Label ID="Label_CF4" runat="server" Text="0" CssClass="lbl_BS"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    (5)現金及び現金同等物等期首残高
                </td>
                <td>
                    <asp:TextBox ID="TextBox_CF13" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_CF" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    (6)現金および現金同等物期末残高
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_CF14" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_CF" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="even">
                    売上高
                </td>
                <td>
                    <asp:TextBox ID="TextBox_CF15" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_CF" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="per">
                    キャッシュ・フローマージン
                </td>
                <td class="right">
                    <%-- 「営業活動によるキャッシュ・フロー」÷「売上高」一般的な目標は7% --%>
                    <asp:Label ID="Label_CF6" runat="server" Text="0.0%" CssClass="lbl_BS"></asp:Label>
                </td>
            </tr>
</table>
</div>



<div class="noprint">

        <p class="center">
            <asp:Button ID="Button_Check_CF" CssClass="btn-flat-border" runat="server" Text="新規保存" OnClick="Push_Check_CF" CausesValidation="False" />
            <asp:Button ID="Button_CheckAS_CF" CssClass="btn-flat-border" runat="server" Text="上書き保存" OnClick="Push_CheckAS_CF" CausesValidation="False" />
            <asp:Button ID="Button_CF_SUM" CssClass="btn-flat-border" runat="server" Text="小計/合計" OnClick="Change_CF" CausesValidation="False" />
            <input type="button" class="btn-flat-border" value="印刷" onclick="window.print();" />
        </p>


            <asp:GridView ID="GridView_CF" runat="server" CssClass="DGTable" AutoGenerateColumns="False" DataKeyNames="uuid" DataSourceID="SqlDataSource_CF" AllowPaging="True" AllowSorting="True" OnRowCommand="Grid_RowCommand">
                <Columns>
                    <asp:BoundField DataField="uuid" HeaderText="uuid" ReadOnly="True" SortExpression="uuid" />
                    <asp:BoundField DataField="ACL1" HeaderText="営業活動C/F" SortExpression="ACL1" HeaderStyle-ForeColor="LightBlue" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="ACL2" HeaderText="投資活動C/F" SortExpression="ACL2" HeaderStyle-ForeColor="LightBlue" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="ACL3" HeaderText="財務活動C/F" SortExpression="ACL3" HeaderStyle-ForeColor="LightBlue" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="ACL4" HeaderText="現金等増加額" SortExpression="ACL4" HeaderStyle-ForeColor="LightYellow" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="ACL5" HeaderText="現金等期首残高" SortExpression="ACL5" HeaderStyle-ForeColor="LightYellow" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="CF14" HeaderText="現金等期未残高" SortExpression="CF14" HeaderStyle-ForeColor="LightYellow" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="ACL6" HeaderText="C/Fマージン" SortExpression="ACL6" HeaderStyle-ForeColor="LightGreen" DataFormatString="{0:0.0%}" />
                    <asp:BoundField DataField="Date" HeaderText="申請年月日" SortExpression="Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="UpDateTime" HeaderText="最終更新日" SortExpression="UpDateTime" />

                    <asp:ButtonField ButtonType="Button" Text="削除" HeaderText="削除" CommandName="CFRemove" CausesValidation="False" >
                    <ControlStyle CssClass="btn-flat-border-mini" />
                    </asp:ButtonField>

                    <asp:ButtonField ButtonType="Button" Text="参照" HeaderText="編集" CommandName="CFDownLoad" CausesValidation="False" >
                    <ControlStyle CssClass="btn-flat-border-mini" />
                    </asp:ButtonField>

            </Columns>
        <HeaderStyle BackColor="Black" ForeColor="White" />
        <RowStyle BackColor="#1E1E1E" ForeColor="White" />
            </asp:GridView>

           <asp:SqlDataSource ID="SqlDataSource_CF" runat="server" ConnectionString="<%$ ConnectionStrings:WhereverConnectionString %>" SelectCommand="SELECT [uuid], [ACL1], [ACL2], [ACL3], [ACL4], [ACL5], [CF14], [ACL6], [Date], [UpDateTime] FROM [T_CF] ORDER BY [UpDateTime] DESC"></asp:SqlDataSource>

    <span class="hr"></span>
    <p class="index1">◆コラム：C/Fの見方</p>
            <ul>
                <li>キャッシュフロー(C/F)は企業のお金の流れを示す表です。上場企業以外には作成義務がありません。</li>
                <li>C/Fを見ると、例えば、P/Lでは黒字の企業でも、倒産の危機にある企業を見分けることができます。</li>
                <li>同様に、P/Lでは赤字の企業でも、将来有望な企業を見分けることができます。</li>
                <li>(1)営業活動によるキャッシュフローはプラスなほど、本業でしっかりと稼いでいる優秀な企業です。反対に、マイナスなほど本業が疎かです。</li>
                <li>(2)投資活動によるキャッシュフロー はマイナスなほど、多額の投資をしている将来有望な企業です。反対に、プラスなほど現状維持傾向にあります。</li>
                <li>(3)財務活動によるキャッシュ・フローとは、簡単にいえば、お金の貸し借りの流れのことです。</li>
                <li>借金返済額が多く、投資が多い企業は好景気です。◎</li>
                <li>借金返済額が多く、投資が少ない企業は内部留保傾向です。〇</li>
                <li>借入金が多く、投資が多い企業はベンチャー傾向にあります。〇</li>
                <li>借入金が多く、投資が少ない企業は倒産の危機にあります。×</li>
                <li>キャッシュフロー・マージンは一般的に7%や15%が目安です。業種により異なります。</li>
                <li>マージンが低い企業は、スタートアップ企業ではない場合、黒字倒産の危機にあります。</li>
                <li>キャッシュフロー・マージンは経年変化で見るとよいです。長期的にマージンが低下している企業は倒産の危機にあります。</li>
                <li>キャッシュフロー・マージンはが異常に高い企業は、現金の一括払いを疑ったほうがよいです。</li>
                <li>参考：</li>
                <li>「経理COMPASS」: https://advisors-freee.jp/article/category/cat-big-02/cat-small-04/7901/（2021年５月28日アクセス）.</li>
                <li>「プロが教える会計講座――会計ショップ」: https://kaikei-shop.net/contents122/（2021年５月28日アクセス）.</li>
            </ul>
    <span class="hr"></span>


</div>


</asp:Panel>



<div class="noprint">
           <span class="hr"></span>


           <p class="index1">
               ◆リース管理
               <asp:Button ID="Button_Renatal" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_Rental" CausesValidation="False" />
               　PCなどのリース品の発注/貸出/返却/延滞情報を一括管理します。</p>





           <hr />
</div>

<asp:Panel ID="Panel_Rental" runat="server" Visible="false" DefaultButton="Button_Rental_Correct">

<div class="noprint">
           <span class="hr"></span>
            <p class="center">リースを登録/変更します。</p>
           <span class="hr"></span>
</div>

    <div class="center"><a name="Rental_TOP"></a>
        <table class="DGTable">
            <tr>
                <th colspan="2" class="th_master">
                    リース登録/管理
                </th>
            </tr>
            <tr>
                <th class="th_master">
                    uuid
                </th>
                <th class="th_master">
                    <asp:TextBox ID="TextBox_order_uuid" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" Placeholder="Null" OnTextChanged="Change_Rental" MaxLength="50" ReadOnly="true"></asp:TextBox>
                    <asp:Button ID="Button_order_uuid_reset" CssClass="btn-flat-border-textinnner" runat="server" Text="Reset" OnClick="Push_Reset_Rental" CausesValidation="False" />
                </th>
            </tr>            <tr>
                <th class="th_master">
                    発注日
                </th>
                <th colspan="2" class="th_master">
                    <asp:DropDownList ID="DropDownList_order_year" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>年
                    <asp:DropDownList ID="DropDownList_order_month" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>月
                    <asp:DropDownList ID="DropDownList_order_day" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>日
                </th>
            </tr>
            <tr>
                <th class="th_master">
                    発注者
                </th>
                <th class="th_master">
                    <asp:TextBox ID="TextBox_order_name" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" Placeholder="例：山田太郎" OnTextChanged="Change_Rental" MaxLength="50"></asp:TextBox>
                </th>
            </tr>
            <tr>
                <th class="th_master">
                    要期
                </th>
                <th colspan="2" class="th_master">
                    <asp:DropDownList ID="DropDownList_youki_year" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>年
                    <asp:DropDownList ID="DropDownList_youki_month" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>月
                    <asp:DropDownList ID="DropDownList_youki_day" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>日
                </th>
            </tr>
            <tr>
                <th class="th_master">
                    請期
                </th>
                <th colspan="2" class="th_master">
                    <asp:DropDownList ID="DropDownList_seiki_year" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>年
                    <asp:DropDownList ID="DropDownList_seiki_month" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>月
                    <asp:DropDownList ID="DropDownList_seiki_day" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>日
                </th>
            </tr>
            <tr>
                <th class="th_master">
                    発注残
                </th>
                <th class="th_master">
                    <asp:TextBox ID="TextBox_order_rest" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="0" OnTextChanged="Change_Rental" MaxLength="50"></asp:TextBox>
                </th>
            </tr>
            <tr>
                <th class="th_master">
                    出荷日
                </th>
                <th colspan="2" class="th_master">
                    <asp:DropDownList ID="DropDownList_shipping_year" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>年
                    <asp:DropDownList ID="DropDownList_shipping_month" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>月
                    <asp:DropDownList ID="DropDownList_shipping_day" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>日
                </th>
            </tr>
            <tr>
                <th class="th_master">
                    受取完了日
                </th>
                <th colspan="2" class="th_master">
                    <asp:DropDownList ID="DropDownList_receive_year" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>年
                    <asp:DropDownList ID="DropDownList_receive_month" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>月
                    <asp:DropDownList ID="DropDownList_receive_day" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>日
                </th>
            </tr>
            <tr>
                <th class="th_master">
                    返却期限日
                </th>
                <th colspan="2" class="th_master">
                    <asp:DropDownList ID="DropDownList_send_d_year" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>年
                    <asp:DropDownList ID="DropDownList_send_d_month" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>月
                    <asp:DropDownList ID="DropDownList_send_d_day" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>日
                </th>
            </tr>
            <tr>
                <th class="th_master">
                    返却残
                </th>
                <th class="th_master">
                    <asp:TextBox ID="TextBox_send_rest" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="0" OnTextChanged="Change_Rental" MaxLength="50"></asp:TextBox>
                </th>
            </tr>
            <tr>
                <th class="th_master">
                    返却完了確認日
                </th>
                <th colspan="2" class="th_master">
                    <asp:DropDownList ID="DropDownList_send_year" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>年
                    <asp:DropDownList ID="DropDownList_send_month" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>月
                    <asp:DropDownList ID="DropDownList_send_day" runat="server" OnSelectedIndexChanged="Change_Rental" CssClass="ddl_date" AutoPostBack="true" ></asp:DropDownList>日
                </th>
            </tr>
            <tr>
                <th class="th_master">
                    リース品名
                </th>
                <th class="th_master">
                    <asp:TextBox ID="TextBox_rental_name" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" Placeholder="例：m2m_rental_pc" OnTextChanged="Change_Rental" MaxLength="50"></asp:TextBox>
                </th>
            </tr>
            <tr>
                <th class="th_master">
                    リース品種別
                </th>
                <th class="th_master">
                    <asp:TextBox ID="TextBox_rental_type" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" Placeholder="例：パソコン" OnTextChanged="Change_Rental" MaxLength="50"></asp:TextBox>
                </th>
            </tr>
            <tr>
                <th class="th_master">
                    ＠単価
                </th>
                <th class="th_master">
                    <asp:TextBox ID="TextBox_rental_tanka" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="0" OnTextChanged="Change_Rental" MaxLength="50" AutoPostBack="true"></asp:TextBox>
                </th>
            </tr>
            <tr>
                <th class="th_master">
                    数量
                </th>
                <th class="th_master">
                    <asp:TextBox ID="TextBox_rental_amount" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="0" OnTextChanged="Change_Rental" MaxLength="50" AutoPostBack="true"></asp:TextBox>
                </th>
            </tr>
            <tr>
                <th class="th_master">
                    合計
                </th>
                <th class="th_master">
                    <asp:TextBox ID="TextBox_rental_total_amount" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" MaxLength="50" ReadOnly="true"></asp:TextBox>
                </th>
            </tr>
        </table>
        <asp:Button ID="Button_Rental_Correct" CssClass="btn-flat-border" runat="server" Text="登録/上書き" OnClick="Push_Rental_Correct" CausesValidation="False" />






        <asp:GridView ID="GridView_Rental" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource_Rental" CssClass="DGTable" AllowPaging="True" AllowSorting="True" OnRowCommand="Grid_RowCommand">
            <Columns>
                <asp:BoundField DataField="uuid" HeaderText="uuid" SortExpression="uuid" HeaderStyle-ForeColor="White" ReadOnly="true" />
                <asp:BoundField DataField="order_date" HeaderText="発注日" SortExpression="order_date" HeaderStyle-ForeColor="White" DataFormatString="{0:yyyy/MM/dd}" />
                <asp:BoundField DataField="order_name" HeaderText="発注担当者" SortExpression="order_name" HeaderStyle-ForeColor="White" />
                <asp:BoundField DataField="receive_date" HeaderText="受取日" SortExpression="receive_date" HeaderStyle-ForeColor="White" DataFormatString="{0:yyyy/MM/dd}" />
                <asp:BoundField DataField="send_deadline" HeaderText="返却期限日" SortExpression="send_deadline" HeaderStyle-ForeColor="White" DataFormatString="{0:yyyy/MM/dd}" />
                <asp:BoundField DataField="rental_name" HeaderText="リース品名" SortExpression="rental_name" HeaderStyle-ForeColor="White" />
                <asp:BoundField DataField="rental_type" HeaderText="リース種別" SortExpression="rental_type" HeaderStyle-ForeColor="White" />
                <asp:BoundField DataField="rental_tanka" HeaderText="＠単価" SortExpression="rental_tanka" HeaderStyle-ForeColor="White" DataFormatString="{0:C}" />
                <asp:BoundField DataField="rental_amount" HeaderText="数量" SortExpression="rental_amount" HeaderStyle-ForeColor="White" DataFormatString="{0:#,0}" />
                <asp:BoundField DataField="rental_total_amount" HeaderText="合計" SortExpression="rental_total_amount" HeaderStyle-ForeColor="White" DataFormatString="{0:C}" />
                <asp:BoundField DataField="up_day" HeaderText="最終更新日" SortExpression="up_day" HeaderStyle-ForeColor="White" />
                    <asp:ButtonField ButtonType="Button" Text="削除" HeaderText="削除" CommandName="RNRemove" CausesValidation="False" >
                    <ControlStyle CssClass="btn-flat-border-mini" />
                    </asp:ButtonField>
                    <asp:ButtonField ButtonType="Button" Text="参照" HeaderText="編集" CommandName="RNDownLoad" CausesValidation="False" >
                    <ControlStyle CssClass="btn-flat-border-mini" />
                    </asp:ButtonField>
            </Columns>
        </asp:GridView>


        <asp:SqlDataSource ID="SqlDataSource_Rental" runat="server" ConnectionString="<%$ ConnectionStrings:WhereverConnectionString %>" SelectCommand="SELECT [uuid], [order_date], [order_name], [receive_date], [send_deadline], [rental_name], [rental_type], [rental_tanka], [rental_amount], [rental_total_amount], [up_day] FROM [T_Rental] ORDER BY [up_day] DESC">
        </asp:SqlDataSource>


</div>
</asp:Panel>




        </div>
    </form>
</body>
</html>
