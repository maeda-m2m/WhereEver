<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="XHTML5Editor.aspx.cs" Inherits="WhereEver.XHTML5Editor.WebForm1" ValidateRequest="false" %>
<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="application/json; charset=utf-8"/>
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../MenuControl.css" />
    <link rel="stylesheet" type="text/css" href="XHTML5Editor.css" />
    <title>XHTML5エディター</title>
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
               ◆フラットボタンエディター
               <asp:Button ID="Button_Button" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_Button" CausesValidation="False" />
               　手軽にフラットボタンをつくることができます。</p>

           <hr />

<asp:Panel ID="Panel_BT" runat="server" Visible="false" DefaultButton="Button_Correct">

                    <div class="noprint">
                       <span class="hr"></span>
                        <p class="center">汎用のフラットボタンを作ります。生成ボタンを押すとコードが生成されます。</p>
                       <span class="hr"></span>
                    </div>


    <div class="center">
          <table class="DGTable">
            <tr>
                <th class="th_master" colspan="2">
                    設定
                </th>
                <th class="th_master">
                結果ビュー
                </th>
            </tr>
            <tr>
                <td>
                ボタンテキスト
                </td>
                <td>
                    <asp:TextBox ID="TextBox_BTNText" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角30文字以内" Text="Button" MaxLength="30"></asp:TextBox>
                </td>
                <td rowspan="12" class="th_master"> <%-- rowspan --%>
                <asp:Label ID="Label_ButtonResult" runat="server" Text="ここにボタンがプレビューされます。" CssClass="lbl_pl" ValidateRequestMode="Disabled"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                フォントファミリー
                </td>
                <td>
                    <asp:TextBox ID="TextBox_FontFamily" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角/半角30文字以内" Text="Meiryo" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                横幅
                </td>
                <td>
                    <asp:TextBox ID="TextBox_Width" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="半角8文字以内(Max 300px or 25em, Min n >= 0)" Text="200" MaxLength="8"></asp:TextBox>
                    <asp:DropDownList ID="DropDownList_Width" runat="server" CssClass="ddl_date">
                        <asp:ListItem Value="px"></asp:ListItem>
                        <asp:ListItem Value="em"></asp:ListItem>
                        <asp:ListItem Value="auto"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                高さ
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_Height" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="半角8文字以内(Max 300px or 25em, Min n >= 0)" Text="50" MaxLength="8"></asp:TextBox>
                    <asp:DropDownList ID="DropDownList_Height" runat="server" CssClass="ddl_date">
                        <asp:ListItem Value="px"></asp:ListItem>
                        <asp:ListItem Value="em"></asp:ListItem>
                        <asp:ListItem Value="auto"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                線の太さ
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_Border" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="半角8文字以内(Max 300px or 25em, Min n >= 0)" Text="2" MaxLength="8"></asp:TextBox>
                    <asp:DropDownList ID="DropDownList_Border" runat="server" CssClass="ddl_date">
                        <asp:ListItem Value="px"></asp:ListItem>
                        <asp:ListItem Value="em"></asp:ListItem>
                        <asp:ListItem Value="auto"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                線の丸み
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_Radius" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="半角8文字以内(Max 3000px or 250em, Min n >= 0)" Text="4" MaxLength="8"></asp:TextBox>
                    <asp:DropDownList ID="DropDownList_Radius" runat="server" CssClass="ddl_date">
                        <asp:ListItem Value="px"></asp:ListItem>
                        <asp:ListItem Value="em"></asp:ListItem>
                        <asp:ListItem Value="auto"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                線の色名/色コード
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_BorderColor" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="半角30文字以内" Text="white" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                文字色名/色コード
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_Color" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="半角30文字以内" Text="white" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                背景色名/色コード
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_BColor" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="半角30文字以内" Text="#1e1e1e" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                選択中文字色名/色コード
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_HColor" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="半角30文字以内" Text="white" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                選択中背景色名/色コード
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_HBColor" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="半角30文字以内" Text="#808080" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                <asp:CheckBox ID="CheckBox_Pointer" runat="server" Checked="true" Text="指マークに変化" />
                </td>
            </tr>
            <tr>
                <td colspan="3" class="th_master">
                    <asp:Button ID="Button_Correct" CssClass="btn-flat-border" runat="server" Text="生成" OnClick="Push_Correct" CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:TextBox ID="TextBox_ButtonCodeResult" runat="server" CssClass="textbox_Wide" ValidateRequestMode="Disabled" Text="ここにコードが自動生成されます。" CausesValidation="false" TextMode="MultiLine" Style="resize: none" ReadOnly="true" ></asp:TextBox>
                </td>
            </tr>
          </table>
    </div>

</asp:Panel>


<div class="noprint">
 <span class="hr"></span>


           <p class="index1">
               ◆QRコード生成
               <asp:Button ID="Button_QRCode" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_QRCode" CausesValidation="False" />　QRコードを生成する機能です。
           </p>

 <hr />
</div>

<asp:Panel ID="Panel_QRCode" runat="server" Visible="false" DefaultButton="Button_CreateQRCode">

<div class="noprint">

<span class="hr"></span>

<div class="center"><a name="QRCode"></a>
    <p class="index1">QRコードにしたい文字列を入力して下さい。生成後は、右クリック→「名前を付けて保存」で画像を保存できます。</p>

    <div class="index1">
        <p>▼QRCodeにする文字列▼</p>
    </div>
    <div>
        <asp:TextBox ID="TextBox_QR_Text" runat="server" CssClass="textbox_Wide"  ValidateRequestMode="Disabled" Text="http://test.m2m-asp.com/WhereEver/ログイン/Login.aspx" MaxLength="200" TextMode="MultiLine" Style="resize: none"  AutoPostBack="True" placeholder="QRコードにしたい文字列を入力して下さい"></asp:TextBox>
    </div>

        <p>
            Width = <asp:TextBox ID="TextBox_QRCode_Width" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角4文字以内(1000 >= n >= 100)" placeholder="150" Text="150" MaxLength="4"></asp:TextBox>px
        </p>
        <p>
            Height = <asp:TextBox ID="TextBox_QRCode_Height" runat="server" CssClass="textbox_pl" ValidateRequestMode="Disabled" ToolTip="全角4文字以内(1000 >= n >= 100)" placeholder="150" Text="150" MaxLength="4"></asp:TextBox>px
        </p>

        <div>
            <p class="f-notice">[QR画像から文字列を読込]</p><asp:FileUpload ID="FileUpload_userfile" runat="server" Width="485px"  CssClass="form-flat-border-inner" />
        </div>

    <p>
        <asp:CheckBox ID="CheckBox_EAN" runat="server" Text="EAN-13バーコードを発行する（発行のみ対応）" Visible="false" CssClass="none" />
    </p>
    <p>
        <asp:Button ID="Button_CreateQRCode" CssClass="btn-flat-border" runat="server" Text="発行" OnClick="Push_QR_Create" CausesValidation="False" />
        <asp:Button ID="Button_EncodeQRCode" CssClass="btn-flat-border" runat="server" Text="読込" OnClick="Push_QR_Encode" CausesValidation="False" />
    </p>

    <p>
        <span class="hr"></span>
        <asp:Label ID="Label_QRCode_Area" runat="server" ValidateRequestMode="Disabled" Text="※このスペースにQRコードの画像が生成されます。"></asp:Label>
        <span class="hr"></span>
    </p>

    <div>
       <asp:TextBox ID="TextBox_QRCode_Result" runat="server" CssClass="textbox_Wide" ValidateRequestMode="Disabled" Text="Ready..." CausesValidation="false" TextMode="MultiLine" Style="resize: none" ReadOnly="true"></asp:TextBox>
    </div>
</div>


    <span class="hr"></span>
        <p class="index1">～概要～</p>
               <p>入力された文字列からQRコードを生成します（QR画像から文字列の読み取ることもできるようになりました）。</p>
               <p>本来はBitmapで生成されますが、ASP.NetでSystem.Drawing出力をそのまま使うことは非推奨のため、imgタグのPNG画像に変換しています。</p>
               <p>※環境によって、QRコード生成後に再度ボタンを押すと、動作しないときがあります。その際は、もう１度ボタンを押すと動作します。</p>
    <hr />
               <p>This script includes the work that is 'ZXing 0.16.6 (created by Michael Jahn)' distributed in the Apache License 2.0.</p>
               <p>このスクリプトは、 Apache 2.0ライセンスで配布されている製作物「ZXing 0.16.6 (製作者：Michael Jahn)」が含まれています。</p>
               <p>http://www.apache.org/licenses/LICENSE-2.0</p>

    <hr />

               <p>QRCodeClassのスクリプトの参照元：「【ZXing.Net】C#でQRコードの読取」@satorimon</p>
               <p>https://qiita.com/satorimon/items/7b7b70410398ee6fd1a4（2021年6月11日アクセス）.</p>
    <span class="hr"></span>

</div>
</asp:Panel>

           <span class="hr"></span>

           <p class="index1">
               ◆お手軽検索ボックス
               <asp:Button ID="Button_IndexBox" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_IndexBox" CausesValidation="False" />
               　文字列からキーワードを検索する機能です。</p>

           <hr />

<asp:Panel ID="Panel_IndexBox" runat="server" Visible="false" DefaultButton="Button_IBCorrect">

                    <div class="noprint">
                       <span class="hr"></span>
                        <p class="center">汎用の検索ボックスを作ります。上から、検索ボックス、検索対象、検索結果です。<br />
                            HTMLページを読み込んでのサイト内検索は、m2mのプロキシが不明なためできません。<br />
                            少なくともlocalhost時はできませんでした。<br />
                            ロード失敗時はHTMLページをそのまま画面に貼り付けますので、検索対象のボックスに丸ごとコピペして下さい。<br />
                            ※キーワードに改行コードは使用できません。
                        </p>
                       <span class="hr"></span>
                    </div>


    <div class="center">
          <table class="DGTable">
            <tr>
                <th class="th_master">
                    検索ボックス
                </th>
            </tr>
            <tr>
                <td>
                    <div class="ib_field">
                    <asp:TextBox ID="TextBox_IB" runat="server" CssClass="textbox_IB" ValidateRequestMode="Disabled" ToolTip="全角200文字以内" Text="" MaxLength="200" placeholder="検索キーワード" AutoCompleteType="Search" TextMode="Search" ></asp:TextBox>
                    <asp:Button ID="Button_IBCorrect" CssClass="btn-flat-border-ib" runat="server" Text="検索" OnClick="Push_IBCorrect" CausesValidation="False" />
                    <asp:Button ID="Button_IBPageCorrect" CssClass="btn-flat-border-ib" runat="server" Text="ページ検索" OnClick="Push_IBPageCorrect" CausesValidation="False" />
                    </div>
                </td>
            </tr>
            <tr>
                <th class="th_master">
                    でたらめ応答文章生成（Wikipediaの文章とユーザー入力を辞書に使用）
                </th>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button_SetIndexBook" CssClass="btn-flat-border" runat="server" Text="↓検索対象を辞書登録" OnClick="Push_SetIndexBook" CausesValidation="False" />
                    <asp:Button ID="Button_GetIndexBook" CssClass="btn-flat-border" runat="server" Text="↓検索対象から文章生成" OnClick="Push_GetIndexBook" CausesValidation="False" />
                    （※検索対象は、必ず「、,。,！,？,！？」のいずれかで区切られた日本語の文章を入力して下さい！）</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="TextBox_IBText" runat="server" CssClass="textbox_Wide" ValidateRequestMode="Disabled" Text="検索したい文字列を入力して下さい。" CausesValidation="false" TextMode="MultiLine" Style="resize: none" placeholder="ここに検索したい文字列を入力して下さい。" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="TextBox_IBResult" runat="server" CssClass="textbox_Wide" ValidateRequestMode="Disabled" Text="ここに検索結果が自動生成されます。" CausesValidation="false" TextMode="MultiLine" Style="resize: none" ReadOnly="true" ></asp:TextBox>
                </td>
            </tr>
          </table>
    </div>

</asp:Panel>


           <span class="hr"></span>

           <p class="index1">
               ◆遺伝的アルゴリズム
               <asp:Button ID="Button_DeepBit" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_DeepBit" CausesValidation="False" />
               　遺伝的アルゴリズムのテストを実行します。組み合わせ、順位、経路探索などに応用できます。</p>

           <hr />

<asp:Panel ID="Panel_DeepBit" runat="server" Visible="false" DefaultButton="Button_SetBit">

                    <div class="noprint">
                       <span class="hr"></span>
                        <p class="center">
                            演算開始ボタンを押すと一様交差処理のテストを行います。<br />
            				ランダム演算は一様交差処理とルーレット選択による遺伝的アルゴリズムのテストを実行します。<br />
                            ランダム演算の解は固定で0b00110111000100010000111100001111(923864847)です。<br />
                        </p>
                       <span class="hr"></span>
                    </div>


    <div class="center">
          <table class="DGTable">
            <tr>
                <th class="th_master">
                    Bit演算（仮）
                </th>
            </tr>
            <tr>
                <td>                    
                    [交配先Int] 0b<asp:TextBox ID="TextBox_Bit_a" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角64文字以内" Text="0000111100001111" MaxLength="64"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>                    
                    [交配者Int] 0b<asp:TextBox ID="TextBox_Bit_b" runat="server" CssClass="textbox_BS" ValidateRequestMode="Disabled" ToolTip="全角64文字以内" Text="1011101101100110" MaxLength="64"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button_SetBit" CssClass="btn-flat-border" runat="server" Text="演算開始" OnClick="Push_SetBit" CausesValidation="False" />
                    <asp:Button ID="Button_SetRandomBit" CssClass="btn-flat-border" runat="server" Text="ランダム演算開始" OnClick="Push_SetRandomBit" CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="TextBox_BitResult" runat="server" CssClass="textbox_Wide" ValidateRequestMode="Disabled" Text="ここに検索結果が自動生成されます。" CausesValidation="false" TextMode="MultiLine" Style="resize: none" ReadOnly="true" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <p>
                        解説：<br />
                        遺伝的アルゴリズムは、至極簡単にいえば、条件となる変数をもとに、生物の遺伝子のように数値を交配させて、<br />
                        環境に適応したエリート遺伝子を生み出す理論です。すなわち、ある一定の条件下における最適解を導き出す理論です。<br />
                        遺伝的アルゴリズムは0と1の計算に向いています。例えば車のハンドルを右に切るか左に切るかの２通りがあるものとします。<br />
                        ハンドルを右に切るときは0、左に切るときは1とします。また、アクセルが1、ブレーキが0です。<br />
                        ハンドルとアクセル/ブレーキは別々の評価をするため、別々に交配させた上で総合得点を合算して評価するなどします。<br />
                        ハンドルとアクセル/ブレーキはそれぞれ紐づいているため、評価は紐づいた状態で行う必要があります。<br />
                        評価には「賞」と「罰」の概念を用いることができるでしょう。<br />
                        例えば、目的地に到着する時間が短いほど賞を与えます。また、障害物にぶつかるほど罰を与えます。<br />
                        もし、安全運転を必要とするなら、速度超過した場合に賞よりも大きい罰を与える必要があります。<br />
                        何度も試行することで、素早くぶつからずに移動する最適解が得られるでしょう。<br />
                        遺伝的アルゴリズムは、静的環境を前提とします。動的環境にはDeep Q-Learningのような深層学習による強化学習を用います。<br />
                        強化学習は生産管理の効率化などに役立ちます。内部にコードサンプルを置いておきましたが、ASP.Netで動かすのは非推奨です（重い）。<br />
                        機能性や汎用性を考慮して、GPUを積んだデスクトップPCでPythonやUnity(C#)を用いるとよいでしょう。<br />
                    </p>
                </td>
            </tr>
          </table>
    </div>

</asp:Panel>



<div class="noprint">
 <span class="hr"></span>


           <p class="index1">
               ◆Bitmap CNN
               <asp:Button ID="Button1" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_CNNPanel" CausesValidation="False" />　画像認識(CNN)のテストです。
           </p>

 <hr />
</div>

<asp:Panel ID="Panel_CNN" runat="server" Visible="false" DefaultButton="Button_CNN_Correct">

<div class="noprint">

<span class="hr"></span>

<div class="center"><a name="CNN"></a>
    <p class="index1">認識してほしい画像(150px*150px程度)を入力して下さい。アラビア数字１文字を読み取ります。(Ver.0.8α)</p>

        <div>
            <p class="f-notice">[画像から文字列を判別]</p><asp:FileUpload ID="FileUpload_CNN" runat="server" Width="485px"  CssClass="form-flat-border-inner" />
        </div>
    <p>
        <asp:Button ID="Button_CNN_Correct" CssClass="btn-flat-border" runat="server" Text="判定" OnClick="Push_CNN_Create" CausesValidation="False" />
    </p>

    <p>
        <span class="hr"></span>
        <asp:Label ID="Label_CNN_Result" runat="server" ValidateRequestMode="Disabled" Text="※下記にCNN解析結果が生成されます。"></asp:Label>
        <span class="hr"></span>
    </p>

     <div>
       <asp:TextBox ID="TextBox_CNN_Result" runat="server" CssClass="textbox_Wide" ValidateRequestMode="Disabled" Text="Ready..." CausesValidation="false" TextMode="MultiLine" Style="resize: none" ReadOnly="true"></asp:TextBox>
     </div>
    </div>
</div>
</asp:Panel>


</div><%-- noprint --%>

    </div>       
    </form>


<script src="../Scripts/d_drop.js"></script>

</body>
</html>
