<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Shinsei.aspx.cs" Inherits="WhereEver.Calender" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <link rel="stylesheet" type="text/css" href="../MenuControl.css" />
    <link rel="stylesheet" type="text/css" href="Shinsei.css" />



    <title>申請書類</title>

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
        </div>

        <div id="Wrap" class="noprint">

         <asp:Panel ID="Panel00" runat="server" CssClass="noprint">
          <p><asp:Button ID="Button_list_open" CssClass="btn-flat-border" runat="server" Text="リストを開く" OnClick="Button_Datalist_Open_Click" CausesValidation="False" /></p>
          <p><asp:Label ID="lblTop_00" runat="server" Text="各種申請書類を作成または管理できます。"></asp:Label></p>
         </asp:Panel>

          <asp:Panel ID="Panel0" runat="server" CssClass="noprint">

                       <p><asp:Button ID="Button_list_close" CssClass="btn-flat-border" runat="server" Text="リストを閉じる" OnClick="Button_Datalist_Close_Click" CausesValidation="False" />
                       <asp:Button ID="Button_reload" CssClass="btn-flat-border" runat="server" Text="リスト手動更新" OnClick="Button_reload_Click" CausesValidation="False" /></p>

                       <p><asp:Label ID="lblTop_0" runat="server" Text="各種申請書類を作成または管理できます。"></asp:Label></p>

            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="id,uid" DataSourceID="SqlDataSource1" CssClass="form-flat-border" OnRowCommand="grid_RowCommand">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="id" ReadOnly="True" SortExpression="id" />
                    <asp:BoundField DataField="uid" HeaderText="uid" ReadOnly="True" SortExpression="uid" />
                    <asp:BoundField DataField="name1" HeaderText="お名前" SortExpression="name1" Visible="False" />
                    <asp:BoundField DataField="ShinseiSyubetsu" HeaderText="申請種別" SortExpression="ShinseiSyubetsu" />
                    <asp:BoundField DataField="DateTime" HeaderText="発行日" SortExpression="DateTime" />
                    <asp:BoundField DataField="LastUpdate" HeaderText="最終更新日" ReadOnly="True" SortExpression="LastUpdate" />

                    <asp:ButtonField ButtonType="Button" Text="削除" ControlStyle-CssClass="btn-flat-border"  HeaderText="削除" CommandName="Remove" CausesValidation="False" >
                    <ControlStyle CssClass="btn-flat-border" />
                    </asp:ButtonField>

                    <asp:ButtonField ButtonType="Button" Text="編集/閲覧" ControlStyle-CssClass="btn-flat-border"  HeaderText="編集/閲覧" CommandName="Reform" CausesValidation="False" >
                    <ControlStyle CssClass="btn-flat-border" />
                    </asp:ButtonField>

                </Columns>
            </asp:GridView>

             <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                ConnectionString="<%$ ConnectionStrings:WhereverConnectionString %>"
                SelectCommand="SELECT * FROM [T_Shinsei_Main]  WHERE ([id] = @id) ORDER BY ShinseiSyubetsu ASC, LastUpdate DESC"
                UpdateCommand="UPDATE [T_Shinsei_Main] SET[LastUpdate] = @LastUpdate WHERE([id] = @id, [uid] = @uid)"
                DeleteCommand="DELETE FROM [T_Shinsei_Main]  WHERE ([id] = @id, [uid] = @uid)">
                <UpdateParameters>
                       <asp:ControlParameter Name="id" ControlId="lblResult" PropertyName="Text"/>
                </UpdateParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="lblResult" DefaultValue="null" Name="id" PropertyName="Text" Type="String" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:ControlParameter ControlID="lblResult" DefaultValue="null" Name="id" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="lbluid" DefaultValue="null" Name="uid" PropertyName="Text" Type="String" />
                </DeleteParameters>
            </asp:SqlDataSource>



                       <p><asp:Button ID="Button_list_close_2" CssClass="btn-flat-border" runat="server" Text="リストを閉じる" OnClick="Button_Datalist_Close_Click" CausesValidation="False" />
                       <asp:Button ID="Button_reload_2" CssClass="btn-flat-border" runat="server" Text="リスト手動更新" OnClick="Button_reload_Click" CausesValidation="False" /></p>

            </asp:Panel>


            <%-- 共通 --%>
           <p>
            <asp:Label ID="lblResult" runat="server" Text="null" Visible="False"></asp:Label>
           </p>
           <p>
            選択中uid: <asp:Label ID="lbluid" runat="server" Text="null"></asp:Label>
           </p>
            <%-- 共通 --%>


            <asp:Panel ID="Panel1" runat="server" CssClass="noprint">
                <table id="sinsei">
                    <tr>
                            <td class="title">
                                申請書類                             
                            </td>
                            <td class="text">
                                <asp:DropDownList ID="DropDownList1" runat="server" OnTextChanged="DropDownList_Master_SelectionChanged" AutoPostBack="True" CausesValidation="False" >
                                    <asp:ListItem>【申請書類を選択】</asp:ListItem>
                                    <asp:ListItem>物品購入申請</asp:ListItem>
                                    <asp:ListItem>勤怠関連申請</asp:ListItem>
                                    <asp:ListItem>立替金明細表申請</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="Button_Master" CssClass="btn-flat-border" runat="server" Text="申請" OnClick="DropDownList_Master_SelectionChanged" CausesValidation="False" />
                            </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="Panel2" runat="server" CssClass="noprint"><a name="Buppin_Button"></a>
                <table id="buppintouroku">
                    <tr>
                        <td class="title">
                            <p>購入品名*</p>
                        </td>
                        <td class="text">
                            <asp:TextBox ID="TextBox_purchaseName" runat="server" CssClass="textbox" Width="415px" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" placeholder="例：『やさしいm2mの本』"></asp:TextBox>
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
                            <asp:TextBox ID="TextBox_classification" runat="server" CssClass="textbox" Width="415px" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" placeholder="例：本"></asp:TextBox>
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
                            <asp:TextBox ID="TextBox_howMany" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" ToolTip="全角49文字以内" placeholder="例：１"></asp:TextBox>点
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_howMany" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_howMany"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>合計金額（税込）*</p>
                        </td>
                        <td class="text">
                            \<asp:TextBox ID="TextBox_howMach" runat="server" CssClass="textbox" Width="128px" ValidateRequestMode="Disabled" ToolTip="全角49文字以内" placeholder="例：1200"></asp:TextBox>-
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
                            <asp:TextBox ID="TextBox_marketPlace" runat="server" CssClass="textbox" Width="415px" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" placeholder="例：m2m書店"></asp:TextBox>
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
                            <asp:TextBox ID="TextBox_buy_purpose" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" TextMode="MultiLine" Rows="3" MaxLength="196" Height="100px" Width="415px" Style="resize: none" ToolTip="全角306文字以内" placeholder="例：テキストとして使用するため。"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_buy_purpose" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_buy_purpose"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>備考</p>
                        </td>
                            <td class="text">
                                <asp:TextBox ID="TextBox_ps" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" TextMode="MultiLine" Rows="3" MaxLength="196" Height="100px" Width="415px" Style="resize: none" ToolTip="全角306文字以内" Text="なし"></asp:TextBox>
                            </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="Button1" CssClass="btn-flat-border" runat="server" Text="印刷ビュー" OnClick="Button1_Click" PostBackUrl="#Buppin_Button" />
                            <asp:Button ID="Button_Buppin_Reset" CssClass="btn-flat-border" runat="server" Text="初期化" OnClick="Button_Buppin_Reset_Click" CausesValidation="False" PostBackUrl="#Buppin_Button" />
                            <asp:Button ID="Button_Save1" CssClass="btn-flat-border" runat="server" Text="DBに新規保存" OnClick="SaveButton_Click_1" PostBackUrl="#Buppin_Button" />
                            <asp:Button ID="Button_SaveAs1" CssClass="btn-flat-border" runat="server" Text="DBに上書保存" OnClick="SaveAsButton_Click_1" PostBackUrl="#Buppin_Button" />
                            <asp:Button ID="BtnBackA1" CssClass="btn-flat-border" runat="server" Text="閉じる" OnClick="ResetButton_Click" CausesValidation="False" PostBackUrl="#Buppin_Button" />
                            <asp:Label ID="lbl_SaveResult1" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="Panel3" runat="server" CssClass="noprint"><a name="Diligence_Button"></a>
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
                           <asp:TextBox ID="TextBox_Notification_ps" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" TextMode="MultiLine" Rows="3" MaxLength="196" Height="100px" Width="415px" Style="resize: none" ToolTip="全角306文字以内" Text="なし"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="Button2" CssClass="btn-flat-border" runat="server" Text="印刷ビュー" OnClick="Button2_Click" PostBackUrl="#Diligence_Button" />
                            <asp:Button ID="Button_Deligence_Reset" CssClass="btn-flat-border" runat="server" Text="初期化" OnClick="Button_Deligence_Reset_Click" CausesValidation="False" PostBackUrl="#Diligence_Button" />
                            <asp:Button ID="Button_Save2" CssClass="btn-flat-border" runat="server" Text="DBに新規保存" OnClick="SaveButton_Click_2" PostBackUrl="#Diligence_Button" />
                            <asp:Button ID="Button_SaveAs2" CssClass="btn-flat-border" runat="server" Text="DBに上書保存" OnClick="SaveAsButton_Click_2" PostBackUrl="#Diligence_Button" />
                            <asp:Button ID="btnBack_A2" CssClass="btn-flat-border" runat="server" Text="閉じる" OnClick="ResetButton_Click" CausesValidation="False" PostBackUrl="#Diligence_Button" />
                            <asp:Label ID="lbl_SaveResult2" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>





                <asp:Panel ID="Panel6" runat="server" CssClass="noprint">
                <table id="meisai">
                    <tr>
                        <td class="title">
                            <p>日付*</p>
                        </td>
                        <td class="text">
                            <asp:TextBox ID="TextBox_Tatekae_Date" runat="server" CssClass="textbox" Width="415px" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" placeholder="例：１月１日"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_Tatekae_Date"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>出張先*</p>
                        </td>
                        <td class="text">
                            <asp:TextBox ID="TextBox_Tatekae_WPlace" runat="server" CssClass="textbox" Width="415px" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" placeholder="例：m2m　出張していない場合は「なし」"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_Tatekae_WPlace"></asp:RequiredFieldValidator>
                        </td>
                    </tr>

                    <tr>
                        <td class="title">
                            <p>交通機関*</p>
                        </td>
                        <td class="text">
                            <asp:TextBox ID="TextBox_Tatekae_TUse" runat="server" CssClass="textbox" Width="415px" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" placeholder="例：電車　交通機関を利用していない場合は「徒歩」"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_Tatekae_TUse"></asp:RequiredFieldValidator>
                        </td>

                    </tr>
                    <tr>
                        <td class="title">
                            <p>乗車駅*</p>
                        </td>
                        <td class="text">
                            <asp:TextBox ID="TextBox_Tatekae_TIn" runat="server" CssClass="textbox" Width="415px" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" placeholder="例：西新宿駅　ない場合は「なし」" Text=""></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_Tatekae_TIn"></asp:RequiredFieldValidator>
                        </td>

                    </tr>
                    <tr>
                        <td class="title">
                            <p>降車駅*</p>
                        </td>
                        <td class="text">
                            <asp:TextBox ID="TextBox_Tatekae_TOut" runat="server" CssClass="textbox" Width="415px" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" placeholder="例：西新宿駅　ない場合は「なし」" Text=""></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_Tatekae_TOut"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>交通費（税込）*</p>
                        </td>
                        <td class="text">
                            \<asp:TextBox ID="TextBox_Tatekae_TWaste" runat="server" CssClass="textbox" Width="128px" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="0"></asp:TextBox>-
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_Tatekae_TWaste"></asp:RequiredFieldValidator>
                        </td>
                    </tr>

                    <tr>
                        <td class="title">
                            <p>宿泊場所*</p>
                        </td>
                        <td class="text">
                            <asp:TextBox ID="TextBox_Tatekae_Place" runat="server" CssClass="textbox" Width="415px" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" placeholder="例：〇〇宿　ない場合は「なし」" ></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_Tatekae_Place"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>宿泊金額（税込）*</p>
                        </td>
                        <td class="text">
                            \<asp:TextBox ID="TextBox_Tatekae_PWaste" runat="server" CssClass="textbox" Width="128px" ValidateRequestMode="Disabled" ToolTip="全角49文字以内" Text="0"></asp:TextBox>-
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_Tatekae_PWaste"></asp:RequiredFieldValidator>
                        </td>
                    </tr>



                    <tr>
                        <td class="title">
                            <p>領収証*</p>
                        </td>
                        <td class="text">
                            <asp:CheckBox ID="CheckBox_Tatekae_Receipt" runat="server" Text="あり" />
                        </td>
                        <td>
                        </td>
                    </tr>



                    <tr>
                        <td class="title">
                            <p>備考</p>
                        </td>
                            <td class="text">
                                <asp:TextBox ID="TextBox_Tatekae_ps" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" TextMode="MultiLine" Rows="3" MaxLength="196" Height="100px" Width="415px" Style="resize: none" ToolTip="全角306文字以内" Text="なし"></asp:TextBox>
                            </td>
                    </tr>

                    <tr>
                        <td class="title">
                            <p>定期券代（税込）*</p>
                        </td>
                        <td class="text">
                            \<asp:TextBox ID="TextBox_Tatekae_Teiki" runat="server" CssClass="textbox" Width="128px" ValidateRequestMode="Disabled" ToolTip="全角49文字以内" Text="0"></asp:TextBox>-
                            <asp:Button ID="Button5" CssClass="btn-flat-border" runat="server" Text="反映" OnClick="Change_Text_T_Teiki" CausesValidation="False" PostBackUrl="#Tatekae_Button" />
                        </td>

                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_Tatekae_Teiki"></asp:RequiredFieldValidator>
                        </td>
                   </tr>


                    
                    <tr>
                        <td colspan="3"><a name ="Tatekae_Button"></a>
                            <asp:Button ID="Button3" CssClass="btn-flat-border" runat="server" Text="挿入" OnClick="Button3_Click" PostBackUrl="#Tatekae_Button" />
                            <asp:Button ID="Button6" CssClass="btn-flat-border" runat="server" Text="初期化" OnClick="Button4_Click" CausesValidation="False" PostBackUrl="#Tatekae_Button" />
                            <asp:Button ID="Button8" CssClass="btn-flat-border" runat="server" Text="UNDO" OnClick="Button_Undo" CausesValidation="False" PostBackUrl="#Tatekae_Button" />
                            <asp:Button ID="Button7" CssClass="btn-flat-border" runat="server" Text="印刷ビュー" OnClick="Button5_Click" CausesValidation="False" PostBackUrl="#Tatekae_Button" />
                            <asp:Button ID="Button_Save3" CssClass="btn-flat-border" runat="server" Text="DBに新規保存" OnClick="SaveButton_Click_3" CausesValidation="False" PostBackUrl="#Tatekae_Button" />
                            <asp:Button ID="Button_SaveAs3" CssClass="btn-flat-border" runat="server" Text="DBに上書保存" OnClick="SaveAsButton_Click_3" CausesValidation="False" PostBackUrl="#Tatekae_Button" />
                            <asp:Button ID="Button4" CssClass="btn-flat-border" runat="server" Text="閉じる" OnClick="ResetButton_Click" CausesValidation="False" PostBackUrl="#Tatekae_Button" />
                            <asp:Label ID="lbl_SaveResult3" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            </div><%-- Wrap ここまでプリントしない --%>



































            <div id ="PrintWrap1"><%-- PrintWrap ここからプリントする --%>

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
                             <td colspan="3" class="sd">
                                  <asp:Label ID="date" runat="server" Text="Select Date"></asp:Label>
                             </td>
                    </tr>
                    <tr>
                        <td id="name" colspan="4">
                            <asp:Label ID="name1" runat="server" Text="No Name"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="Syacho">
                           <p class="SyachoText">社長</p>
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
                        <td colspan="4">
                            <p>上記の通り、申請致します。</p>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
          </div>

            <%-- lllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll --%>


          <div id ="PrintWrap2">
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
                        <td colspan="4" id="name2">
                            <asp:Label ID="lblDiligenceUser" runat="server" Text="No Name"></asp:Label><asp:Label ID="Label13" runat="server" Text="（印）"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                        </td>
                        <td class="Syacho">
                                  <p class="SyachoText">社長</p>
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
                        <td colspan="3" runat="server" class="zone">
                            <asp:Label ID="lblDiligenceClassification2" runat="server" Text="項目を選択してください"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="naiyou">
                            <p>日時</p>
                        </td>
                        <td colspan="3" class="nakami">
                            <div class ="sd"">
                            <asp:Label ID="lblDiligenceDateA1" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lblDiligenceDateA2" runat="server" Text=""></asp:Label>から<br />
                            <asp:Label ID="lblDiligenceDateB1" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lblDiligenceDateB2" runat="server" Text=""></asp:Label>まで
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="naiyou">
                            <p>理由</p>
                        </td>
                        <td colspan="3" class="zone">
                           <asp:Label ID="Label_Diligence_because" runat="server" Text="" ValidateRequestMode="Disabled" Height="100px" Width="415px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="naiyou">
                            <p>備考</p>
                        </td>
                        <td colspan="3" class="zone">
                           <asp:Label ID="Label_Diligence_ps" runat="server" Text="" ValidateRequestMode="Disabled" Height="100px" Width="415px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td id="foot" colspan="4">
                            <p>
                                上記の通り、申請いたします。
                            </p>
                        </td>
                    </tr>
                </table>

            </asp:Panel>
         </div>


                          <%-- lllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll --%>


          <div id ="PrintWrap3">
               <asp:Panel ID="Panel7" runat="server">
                <table id="TatekaeTodoke">
                    <tr>
                        <td colspan="3" id="Tatekaetop">
                            交通費・宿泊費精算書
                        </td>
                    </tr>
                    <tr>
                        <td id="date3">
                            <p>提出日：</p>
                        </td>
                        <td class="sd">
                            <asp:Label ID="lblTatekaeDate" runat="server" Text=""></asp:Label>
                        </td>
                        <td id="name3">
                            氏名（<asp:Label ID="lblTatekaeName" runat="server" Text="　　　　　　　　　"></asp:Label>）
                        </td>
                    </tr>
                    </table>


              <table id="TatekaeTable">
                  <tr class="naiyou"><td class="nakami">日付</td><td class="nakami">出張先</td><td class="nakami">交通機関</td><td class="nakami">乗車駅</td><td class="nakami">降車駅</td><td class="nakami">交通費</td><td class="nakami">宿泊場所</td><td class="nakami">宿泊費</td><td class="nakami">領収証</td><td class="nakami">備考</td></tr>
                  <asp:Label ID="lblTatekaeResult" runat="server" ValidateRequestMode="Disabled"></asp:Label>               
                  <tr class="naiyou"><td colspan="2" class="naiyou"></td><td  colspan="3" class="nakami-b">交通費計</td><td  class="naiyou-b"><asp:Label ID="lblTatekae_Koutuuhi" runat="server" ValidateRequestMode="Disabled" Text="\0-"></asp:Label></td><td class="nakami-b">宿泊費計</td><td  class="naiyou-b"><asp:Label ID="lblTatekae_Shukuhakuhi" runat="server" ValidateRequestMode="Disabled" Text="\0-"></asp:Label></td><td colspan="2" class="naiyou"></td></tr>
                  <tr class="naiyou"><td colspan="10" class="naiyou"></td></tr>
                  <tr class="naiyou"><td class="nakami" colspan="4">交通費＋宿泊費</td><td class="naiyou-b"><asp:Label ID="lblTatekae_Result1" runat="server" ValidateRequestMode="Disabled" Text="\0-"></asp:Label></td><td colspan="5" class="naiyou"></td></tr>
                  <tr class="naiyou"><td class="nakami" colspan="4">定期券代</td><td class="naiyou-b"><asp:Label ID="lblTatekae_Result2" runat="server" ValidateRequestMode="Disabled" Text="\0-"></asp:Label></td><td colspan="5" class="naiyou"></td></tr>
                  <tr class="naiyou"><td class="nakami" colspan="4">立替金合計</td><td class="naiyou-b"><asp:Label ID="lblTatekae_Result3" runat="server" ValidateRequestMode="Disabled" Text="\0-"></asp:Label></td><td colspan="5" class="naiyou"></td></tr>
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

                     </div><%-- PrintWrap ここまでプリントする（ボタンは個別にプリントしない） --%>

                                          <%-- lllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll --%>

<a name="Print_Button"></a>
       <asp:Panel ID="Panel_Print" runat="server" CssClass="noprint">
             <div id="fotter">
                <p class="noprint">
                    <input type="button" class="btn-flat-border" value="印刷" onclick="window.print();" />
                    <asp:Button ID="Btn_BackMaster" CssClass="btn-flat-border" runat="server" Text="閉じる" OnClick="BackButton_Click" CausesValidation="False" PostBackUrl="#Print_Button" />
                    ※印刷ビューに表示されているものが印刷されます。
                </p>
             </div>
       </asp:Panel>





                                          <%-- lllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll --%>

    </form>
</body>
</html>
