<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkManager.aspx.cs" Inherits="WhereEver.管理ページ.WorkManager" %>
<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="application/json; charset=utf-8"/>
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../MenuControl.css" />
    <link rel="stylesheet" type="text/css" href="WorkManager.css" />
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
               ◆進捗管理（γ版）
               <asp:Button ID="Button_DD" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_DD" CausesValidation="False" />
               　ドラッグアンドドロップを用いた進捗管理システムです。</p>

           <hr />

<asp:Panel ID="Panel_DD" runat="server" Visible="false" DefaultButton="Button_GetLabelDD">

                    <div class="noprint">
                       <span class="hr"></span>
                        <p class="center">
                            アイテムをドラッグアンドドロップすると、ボックス間を移動させることができます。<br />
                            「送受信」を押すと現在の配置をもとにプロジェクト管理テーブルをアップデートし、新しいタスクがあればロードします。<br />
                            プロジェクトを却下や中止する場合は、タスクを未処理に入れた状態で「破棄」ボタンを押して下さい。《注意！》一度削除すると元には戻せません！<br />
                            ※現状の仕様：作業中と納期間近のいずれに配置しても、残り期限をもとに自動で振り分けられます。
                        </p>
                       <span class="hr"></span>
                    </div>

    <div class="center">

        <%-- <p>動的コントロールはTextからは抽出できないため、コントロールのIDから抽出する必要があります（調査済）。</p>
        <p>→HiddenFieldで取得可能</p>--%>

        <asp:HiddenField ID="Hidden_Label_item" runat="server" />

<p class="index1"><a id="DD">進捗管理(γ版)</a></p>
                <asp:Button ID="Button_NewProj" CssClass="btn-flat-border" runat="server" Text="プロジェクト新規作成" OnClick="Push_NewProj" CausesValidation="False"/>
<div class="flex_ul">
        <span class="flex_title">未処理（見積～受注可否決定まで）</span>
        <span class="flex_title">作業中（要件定義等～開発まで）</span>
        <span class="flex_title">納期間近（完成まで２週間以内）</span>
        <span class="flex_title">完成（本番稼働）</span>
</div>
<div class="flex_ul">
    <span>
        <asp:Label ID="Label_dropbox_black" runat="server" CssClass="dditems" ondragover="f_dragover(event)" ondrop="f_drop(event)" Text="">
        </asp:Label>
    </span>
    <span>
        <asp:Label ID="Label_dropbox_red" CssClass="dropbox_red" ondragover="f_dragover(event)" ondrop="f_drop1(event)" runat="server" Text="" ValidateRequestMode="Disabled">
        </asp:Label>
    </span>
    <span>
        <asp:Label ID="Label_dropbox_blue" CssClass="dropbox_blue" ondragover="f_dragover(event)" ondrop="f_drop2(event)" runat="server" Text="" ValidateRequestMode="Disabled">
        </asp:Label>
    </span>
    <span>
        <asp:Label ID="Label_dropbox_green" CssClass="dropbox_green" ondragover="f_dragover(event)" ondrop="f_drop3(event)" runat="server" Text="" ValidateRequestMode="Disabled">
        </asp:Label>
    </span>
</div>

       <span class="hr"></span>

        <asp:Button ID="Button_GetLabelDD" CssClass="btn-flat-border" runat="server" Text="送受信" OnClick="Push_GetLabelDD" CausesValidation="False" PostBackUrl="#DD" />
        <asp:Button ID="Button_DeleteBlackDD" CssClass="btn-flat-border" runat="server" Text="破棄" OnClick="Push_DeleteBlackDD" CausesValidation="False" PostBackUrl="#DD" />
        
                        <span class="hr"></span>
                        <p class="center">
                            開発担当者の名前で部分一致による参照をしています（プロジェクトのDBに主キーがないため）。<br />
                            複数人で開発する際は、複数の姓名を入力するとタスクの状況を共有できます。<br />
                            個別のタスク機能が欲しい等のご要望がありましたらチャットにてお知らせください。
                        </p>
                       <span class="hr"></span>

        <%-- 非表示欄（デバッグ専用） --%>
        <div class="none">
        <p class="index1">デバッグコンソール</p>
        <asp:TextBox ID="TextBox_LabelDDResult" runat="server" CssClass="textbox_Wide" ValidateRequestMode="Disabled" Text="Ready..." CausesValidation="false" TextMode="MultiLine" Style="resize: none" ReadOnly="true" ></asp:TextBox>
        <span class="hr"></span>
        </div>


    </div>
</asp:Panel>

           <span class="hr"></span>




           <p class="index1">
               ◆名簿管理
               <asp:Button ID="Button_Meibo" CssClass="btn-flat-border" runat="server" Text="パネル開閉" OnClick="Push_MeiboButton" CausesValidation="False" />
               　社員名簿を管理します（無印版）。</p>

           <hr />

<asp:Panel ID="Panel_Meibo" runat="server" Visible="false" DefaultButton="Button_MeiboCorrect">

                    <div class="noprint">
                       <span class="hr"></span>
                        <p class="center">社員名簿の登録や閲覧をします（無印版。リッチデータの一覧化を実装予定）。</p>
                       <span class="hr"></span>
                    </div>


    <div class="center">
          <table class="DGTable">
            <tr>
                <th class="th_master" colspan="2">
                    設定
                </th>
                <th class="th_master">
                    サムネイル
                </th>
            </tr>
            <tr>
                <td>
                UUID
                </td>
                <td>
                    <asp:TextBox ID="TextBox_Work_uuid" runat="server" CssClass="textbox_LBSW" ValidateRequestMode="Disabled" ToolTip="UUID" placeholder="ここにUUIDが表示されます。" Text="" MaxLength="50" ReadOnly="true"></asp:TextBox>
                    <asp:Button ID="Button_Work_uuid_reset" CssClass="btn-flat-border-textinnner" runat="server" Text="Reset" OnClick="Push_Reset_Work" CausesValidation="False" />
                </td>
                <td rowspan="10" class="th_master"> <%-- rowspan --%>
                <asp:Label ID="lblMeiboPictureResult" runat="server" Text="ここにサムネイルがプレビューされます。" CssClass="lbl_pl" ValidateRequestMode="Disabled"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                企業名
                </td>
                <td>
                    <asp:TextBox ID="TextBox_CompanyName" runat="server" CssClass="textbox_LBSW" ValidateRequestMode="Disabled" ToolTip="全角30文字以内" placeholder="例：株式会社エム・ツー・エム" Text="株式会社エム・ツー・エム" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    サムネイル
                </td>
                <td>
                    <asp:FileUpload ID="FileUpload_MeiboPicture" runat="server" Width="485px" CssClass="form-flat-border-inner" /><br />
                    <asp:Button ID="Button_MeiboUpload" runat="server" CssClass="btn-flat-border" Text="アップロード" OnClick="Button_MeiboUpLoad" />
                    <asp:Button ID="Button_MeiboDelete" runat="server" CssClass="btn-flat-border" Text="画像初期化" OnClick="Push_MeiboDelete" />
                </td>
            </tr>
<%--
            <tr>
                <td>
                顔写真横幅
                </td>
                <td>
                    <asp:TextBox ID="TextBox_MeiboWidth" runat="server" CssClass="textbox_mini" ValidateRequestMode="Disabled" ToolTip="半角3文字以内(Max 500px or 25em, Min n >= 0)　※1px=0.026cm" Text="354" MaxLength="8"></asp:TextBox>
                    px
                </td>
            </tr>
            <tr>
                <td>
                顔写真高さ
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_MeiboHeight" runat="server" CssClass="textbox_mini" ValidateRequestMode="Disabled" ToolTip="半角3文字以内(Max 500px or 25em, Min n >= 0)　※1px=0.026cm" Text="472" MaxLength="8"></asp:TextBox>
                    px
                </td>
            </tr>
--%>
            <tr>
                <td>
                役職
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_MeiboWork" runat="server" CssClass="textbox_LBSW" ValidateRequestMode="Disabled" ToolTip="全角30文字以内" placeholder="例：係長" Text="" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                氏名
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_MeiboName" runat="server" CssClass="textbox_LBSW" ValidateRequestMode="Disabled" ToolTip="全角30文字以内　姓と名の間は全角１文字スペースをあける" placeholder="例：山田　太郎" Text="" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                配属
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_MeiboArea" runat="server" CssClass="textbox_LBSW" ValidateRequestMode="Disabled" ToolTip="全角30文字以内" placeholder="例：〇〇グループ" Text="" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                生年月日
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_Meibo_year" runat="server" CssClass="textbox_mini" ValidateRequestMode="Disabled" ToolTip="半角4文字以内" placeholder="例：2000" Text="" MaxLength="4"></asp:TextBox>年
                    <asp:TextBox ID="TextBox_Meibo_month" runat="server" CssClass="textbox_mini" ValidateRequestMode="Disabled" ToolTip="半角2文字以内" placeholder="例：01" Text="" MaxLength="2"></asp:TextBox>月
                    <asp:TextBox ID="TextBox_Meibo_day" runat="server" CssClass="textbox_mini" ValidateRequestMode="Disabled" ToolTip="半角2文字以内" placeholder="例：01" Text="" MaxLength="2"></asp:TextBox>日 生</td>
            </tr>
            <tr>
                <td>
                    スマホ/携帯電話番号
                </td>
                <td> 
                    <asp:TextBox ID="TextBox_Meibo_Tel1" runat="server" CssClass="textbox_mini" ValidateRequestMode="Disabled" ToolTip="半角4文字以内" placeholder="例：0120" Text="" MaxLength="4" TextMode="Phone"></asp:TextBox>-
                    <asp:TextBox ID="TextBox_Meibo_Tel2" runat="server" CssClass="textbox_mini" ValidateRequestMode="Disabled" ToolTip="半角4文字以内" placeholder="例：0123" Text="" MaxLength="4" TextMode="Phone"></asp:TextBox>-
                    <asp:TextBox ID="TextBox_Meibo_Tel3" runat="server" CssClass="textbox_mini" ValidateRequestMode="Disabled" ToolTip="半角4文字以内" placeholder="例：4567" Text="" MaxLength="4" TextMode="Phone"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                社用メールアドレス
                </td>
                <td>                    
                    <asp:TextBox ID="TextBox_MeiboAddress" runat="server" CssClass="textbox_LBS" ValidateRequestMode="Disabled" ToolTip="全角30文字以内" placeholder="例：yamada@m2m-asp.com" Text="" MaxLength="30" TextMode="Email"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                <asp:CheckBox ID="CheckBox_MeiboPB" runat="server" Checked="true" Text="社内で公開" />
                </td>
            </tr>
            <tr>
                <td colspan="3" class="th_master">
                    <asp:Button ID="Button_MeiboCorrect" CssClass="btn-flat-border" runat="server" Text="登録/上書き" OnClick="Push_MeiboCorrect" CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td colspan="3">

        <asp:GridView ID="GridView_Meibo" runat="server" CssClass="DGTable" AutoGenerateColumns="False" DataSourceID="SqlDataSource_Meibo" AllowPaging="True" AllowSorting="True" OnRowCommand="Grid_RowCommand">
            <Columns>
                <asp:BoundField DataField="uid" HeaderText="uid" SortExpression="uid" HeaderStyle-CssClass="none" ItemStyle-CssClass="none" HeaderStyle-ForeColor="Red" />
                <asp:BoundField DataField="workCompanyName" HeaderText="会社名" SortExpression="workCompanyName" />
                <asp:BoundField DataField="workPost" HeaderText="役職" SortExpression="workPost" />
                <asp:BoundField DataField="workUserName" HeaderText="氏名" SortExpression="workUserName" />
                <asp:BoundField DataField="workAssignment" HeaderText="配属" SortExpression="workAssignment" />
                <asp:BoundField DataField="workBirthday" HeaderText="生年月日" DataFormatString="{0:d}" SortExpression="workBirthday" />
                <asp:BoundField DataField="workPhoneNo" HeaderText="スマホ/携帯電話番号" SortExpression="workPhoneNo" />
                <asp:BoundField DataField="workMail" HeaderText="社用メールアドレス" SortExpression="workMail" />

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

        <asp:SqlDataSource ID="SqlDataSource_Meibo" runat="server" ConnectionString="<%$ ConnectionStrings:WhereverConnectionString %>" SelectCommand="SELECT [uid], [workCompanyName], [workThumbnail], [workPost], [workUserName], [workAssignment], [workBirthday], [workPhoneNo], [workMail] FROM [T_WorkRoster] WHERE ([isPublic] = @isPublic OR [editorId] = @editorId) ORDER BY [workUserName]">
            <SelectParameters>
                <asp:Parameter DefaultValue="True" Name="isPublic" Type="Boolean" />
                <asp:ControlParameter ControlID="lbluid" DefaultValue="null" Name="editorId" PropertyName="Text" Type="String" />
            </SelectParameters>
                    </asp:SqlDataSource>

                    <%-- 非表示欄（デバッグ専用） --%>
                    <div class="none">
                    <asp:Label ID="lbluid" runat="server" Text="null" Visible="false"></asp:Label>
                    <asp:TextBox ID="TextBox_MeiboResult" runat="server" CssClass="textbox_Wide" ValidateRequestMode="Disabled" Text="デバッグコンソール" CausesValidation="false" TextMode="MultiLine" Style="resize: none" ReadOnly="true" ></asp:TextBox>
                    </div>
                </td>
            </tr>
          </table>
    </div>

</asp:Panel>
</div><%-- noprint --%>

        </div>
    </form>
</body>
</html>
