<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Money.aspx.cs" Inherits="WhereEver.Money.WebForm1" %>
<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../MenuControl.css" />
    <link rel="stylesheet" type="text/css" href="Money.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

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

                       <span class="hr"></span>

           <p class="index1">
               ◆損益計算書(P/L)
               <asp:Button ID="Button_PL" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_PL_test" CausesValidation="False" />
               動作テスト済　要ブラッシュアップ</p>

           <hr />

<asp:Panel ID="Panel_PL" runat="server" Visible="false">



                       <span class="hr"></span>

    <p class="center">損益計算書をDBに記録できます。経費は負の値で入力して下さい。</p>

                       <span class="hr"></span>

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
                    <asp:DropDownList ID="DropDownList_PL_year_s" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true"></asp:DropDownList>年
                    <asp:DropDownList ID="DropDownList_PL_month_s" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true"></asp:DropDownList>月
                    <asp:DropDownList ID="DropDownList_PL_day_s" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true"></asp:DropDownList>日
                    <br />
                    至
                    <asp:DropDownList ID="DropDownList_PL_year_g" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true"></asp:DropDownList>年
                    <asp:DropDownList ID="DropDownList_PL_month_g" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true"></asp:DropDownList>月
                    <asp:DropDownList ID="DropDownList_PL_day_g" runat="server" OnSelectedIndexChanged="Change_PL" AutoPostBack="true"></asp:DropDownList>日
                </th>
            </tr>
            <tr>
                <td rowspan="2" class="td_master">
                    売上高<br /><span class="minus">売上原価</span>
                </td>
                <td>
                    <asp:TextBox ID="TextBox_Uriage" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" AutoPostBack="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="TextBox_UriageGenka" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" AutoPostBack="true"></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_HanbaiKanrihi" runat="server" CssClass="textbox_pl"  ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" AutoPostBack="true"></asp:TextBox>
                </td>
             </tr>
             <tr>
                <td>
                    営業利益
                </td>
                <td>
                    <asp:TextBox ID="TextBox_EigyouRieki" runat="server" CssClass="textbox_pl"  ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" AutoPostBack="true"></asp:TextBox>
                </td>
             </tr>
             <tr>
                <td class="minus">
                    営業外費用
                </td>
                <td>
                    <asp:TextBox ID="TextBox_EigyougaiHiyou" runat="server" CssClass="textbox_pl"  ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" AutoPostBack="true"></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_TokubetsuRieki" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" AutoPostBack="true"></asp:TextBox>
                </td>
             </tr>
             <tr>
                <td>
                    <asp:TextBox ID="TextBox_TokubetsuSonshitsu" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" AutoPostBack="true"></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_Houjinzei" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_PL" AutoPostBack="true"></asp:TextBox>
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
                    <asp:Label ID="Label_ArariR" runat="server" Text="0" CssClass="lbl_pl"></asp:Label>
                </td>
             </tr>
             <tr>
                <td class="per">
                    売上高営業利益率
                </td>
                <td class="right">
                    <asp:Label ID="Label_EigyouR" runat="server" Text="0" CssClass="lbl_pl"></asp:Label>
                </td>
             </tr>
             <tr>
                <td class="per">
                    売上高経常利益率
                </td>
                <td class="right">
                    <asp:Label ID="Label_KeijyouR" runat="server" Text="0" CssClass="lbl_pl"></asp:Label>
                </td>
             </tr>
        </table>

        <p class="center">
            <asp:Button ID="Button_Check_PL" CssClass="btn-flat-border" runat="server" Text="新規保存" OnClick="Push_Check_PL" CausesValidation="False" />
            <asp:Button ID="Button_CheckAS_PL" CssClass="btn-flat-border" runat="server" Text="上書き保存" OnClick="Push_CheckAS_PL" CausesValidation="False" />
        </p>

</div>

                           <span class="hr"></span>


  <p class="index1">◆P/L一覧</p>
　<p class="right">（単位：円）</p>
        <asp:GridView ID="GridView_PL" runat="server" CssClass="DGTable" AutoGenerateColumns="False" DataKeyNames="uuid" DataSourceID="SqlDataSource_PL" AllowPaging="True" AllowSorting="True" OnRowCommand="grid_RowCommand">
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
                <asp:BoundField DataField="Date_S" HeaderText="自" SortExpression="Date_S" DataFormatString="{0:d}" />
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

</asp:Panel>


           <span class="hr"></span>

           <p class="index1">
               ◆貸借対照表(B/S)
               <asp:Button ID="Button_BS" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_BS_test" CausesValidation="False" />
               動作テスト済　P/Lとの連携機能なし
           </p>

           <hr />

<asp:Panel ID="Panel_BS" runat="server" Visible="false">

           <span class="hr"></span>
            <p class="center">B/Sを作成します。値はすべて正の値で入力して下さい。資産合計と負債・純資産合計が同じになるように作成して下さい。</p>
           <span class="hr"></span>

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
                    <asp:DropDownList ID="DropDownList_BS_year" runat="server" OnSelectedIndexChanged="Change_BS" ></asp:DropDownList>年
                    <asp:DropDownList ID="DropDownList_BS_month" runat="server" OnSelectedIndexChanged="Change_BS" ></asp:DropDownList>月
                    <asp:DropDownList ID="DropDownList_BS_day" runat="server" OnSelectedIndexChanged="Change_BS" ></asp:DropDownList>日
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
                    <asp:TextBox ID="TextBox_BS1" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
                <td>
                    買掛金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS29" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    受取手形
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS2" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
                <td>
                    短期借入金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS30" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    売掛金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS3" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
                <td>
                    未払金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS31" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    商品
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS4" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
                <td>
                    未払費用
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS32" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    部品
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS5" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
                <td>
                    未払法人税等
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS33" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    前払費用
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS6" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
                <td>
                    預り金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS34" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    繰延税金資産
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS7" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
                <td>
                    賞与引当金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS35" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    短期貸付金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS8" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
                <td>
                    製品保証引当金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS36" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    未収入金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS9" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
                <td>
                    その他
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS37" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    その他
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS10" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_BS11" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_BS38" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_BS39" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_BS40" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    建物
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS12" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_BS13" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_BS14" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_BS15" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_BS16" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
                <td>
                    株主資本
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS41" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    土地
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS17" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
                <td>
                    資本金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS42" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    建設仮勘定
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS18" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
                <td>
                    資本余剰金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS43" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_BS44" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_BS45" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    施設利用権
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS19" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_BS20" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
                <td>
                    利益余剰金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS46" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    その他
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS21" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
                <td>
                    その他利益剰余金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS47" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_BS48" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_BS22" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
                <td>
                    自己株式
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS49" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    関係会社株式
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS23" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_BS24" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
                <td>
                    評価・換算差額等
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS50" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    長期貸付金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS25" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
                <td>
                    その他有価証券評価差額金
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS51" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    長期前払費用
                </td>
                <td class="right">
                    <asp:TextBox ID="TextBox_BS26" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_BS27" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
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
                    <asp:TextBox ID="TextBox_BS28" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" OnTextChanged="Change_BS" ></asp:TextBox>
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

        <p class="center">
            <asp:Button ID="Button_Check_BS" CssClass="btn-flat-border" runat="server" Text="新規保存" OnClick="Push_Check_BS" CausesValidation="False" />
            <asp:Button ID="Button_CheckAS_BS" CssClass="btn-flat-border" runat="server" Text="上書き保存" OnClick="Push_CheckAS_BS" CausesValidation="False" />
            <asp:Button ID="Button_BS_SUM" CssClass="btn-flat-border" runat="server" Text="小計/合計" OnClick="Change_BS" CausesValidation="False" />
        </p>


            <asp:GridView ID="GridView_BS" runat="server" CssClass="DGTable" AutoGenerateColumns="False" DataKeyNames="uuid" DataSourceID="SqlDataSource_BS" AllowPaging="True" AllowSorting="True" OnRowCommand="grid_RowCommand">
                <Columns>
                    <asp:BoundField DataField="uuid" HeaderText="uuid" ReadOnly="True" SortExpression="uuid" />
                    <asp:BoundField DataField="shisan_a" HeaderText="資産合計" SortExpression="shisan_a" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="fusai_a" HeaderText="負債合計" SortExpression="fusai_a" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="jyunshisan_a" HeaderText="純資産" SortExpression="jyunshisan_a" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:d}" />
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



</asp:Panel>


           <span class="hr"></span>


           <p class="index1">
               ◆キャッシュフロー(C/F)
               <asp:Button ID="Button_CF" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_CF_test" CausesValidation="False" />
               工事予定地</p>

           <hr />

<asp:Panel ID="Panel_CF" runat="server" Visible="false">


           <span class="hr"></span>
            <p class="center">///工事中///</p>
           <span class="hr"></span>

</asp:Panel>


        </div>
    </form>
</body>
</html>
