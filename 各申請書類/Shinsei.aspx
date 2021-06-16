<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Shinsei.aspx.cs" Inherits="WhereEver.Calender" %>

<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="application/json; charset=utf-8" />
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
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

          <div class="panel">
          <p><asp:Button ID="Button_list_open" CssClass="btn-flat-border" runat="server" Text="リストを開く" OnClick="Button_Datalist_Open_Click" CausesValidation="False" /></p>
          <p><asp:Label ID="lblTop_00" runat="server" Text="各種申請書類を作成または管理できます。"></asp:Label></p>
          </div>

         </asp:Panel>

          <asp:Panel ID="Panel0" runat="server" CssClass="noprint">
              <div class="panel">

                       <p><asp:Button ID="Button_list_close" CssClass="btn-flat-border" runat="server" Text="リストを閉じる" OnClick="Button_Datalist_Close_Click" CausesValidation="False" />
                       <asp:Button ID="Button_reload" CssClass="btn-flat-border" runat="server" Text="リスト手動更新" OnClick="Button_reload_Click" CausesValidation="False" /></p>

                       <p><asp:Label ID="lblTop_0" runat="server" Text="各種申請書類を作成または管理できます。"></asp:Label></p>

            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="id,uid" DataSourceID="SqlDataSource1" CssClass="DGTable" OnRowCommand="grid_RowCommand" AllowPaging="True" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="id" ReadOnly="True" SortExpression="id" HeaderStyle-CssClass="none" ItemStyle-CssClass="none" />
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
                <HeaderStyle BackColor="#1E1E1E" ForeColor="White" />
                <RowStyle BackColor="Gray" ForeColor="White" />
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

                       <p><asp:CheckBox ID="CheckBox_is_del_pop" runat="server" Text="削除時に確認する" Checked="True" OnCheckedChanged="SetDelPop" AutoPostBack="True" /></p>
                  </div>
            </asp:Panel>


            <%-- 疑似モーダルポップアップ --%>
<asp:Panel ID="Panel_del_pop" runat="server" CssClass="noprint">
    <div class="cautionWrap">
        <p>※最終確認※</p>
        <p>削除uid: <asp:Label ID="lbldeluid" runat="server" Text="null"></asp:Label></p>
        <p>本当に削除しますか？（一度消すと元に戻せません！）</p>
        <asp:Button id="btnDelete" CssClass="btn-flat-border" runat="server" text="Delete" OnClick="Button_del_pop_delete" />
        <asp:Button id="btnCancel" CssClass="btn-flat-border" runat="server" text="Cancel" OnClick="Button_del_pop_cancel" />
    </div>
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
            <div class="panel">
                <table id="sinsei">
                    <tr>
                            <td class="title">
                                申請書類                             
                            </td>
                            <td>
                                <a id="master"></a>
                                <asp:Button ID="Button_Master_A" CssClass="btn-flat-border" runat="server" Text="物品購入" OnClick="Push_Master_A" CausesValidation="False" PostBackUrl="#master" />
                                <asp:Button ID="Button_Master_B" CssClass="btn-flat-border" runat="server" Text="勤怠関連" OnClick="Push_Master_B" CausesValidation="False" PostBackUrl="#master" />
                                <asp:Button ID="Button_Master_C" CssClass="btn-flat-border" runat="server" Text="立替金明細表" OnClick="Push_Master_C" CausesValidation="False" PostBackUrl="#master" />
                                <asp:Button ID="Button_Master_D" CssClass="btn-flat-border" runat="server" Text="週報" OnClick="Push_Master_D" CausesValidation="False" PostBackUrl="#master" />
                                <asp:Button ID="Button_Money" runat="server" CssClass="btn-flat-border" Text="財務諸表" OnClick="btnMoney_Click" />
                            </td>
                        </tr>
                </table>
                <table class="none">
                    <tr>
                            <td class="text">
                                <asp:DropDownList ID="DropDownList1" runat="server" OnTextChanged="DropDownList_Master_SelectionChanged" AutoPostBack="True" CausesValidation="False" Visible="false" >
                                    <asp:ListItem>【申請書類を選択】</asp:ListItem>
                                    <asp:ListItem>物品購入申請</asp:ListItem>
                                    <asp:ListItem>勤怠関連申請</asp:ListItem>
                                    <asp:ListItem>立替金明細表申請</asp:ListItem>
                                    <asp:ListItem>週報</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="Button_Master" CssClass="btn-flat-border" runat="server" Text="申請" OnClick="DropDownList_Master_SelectionChanged" CausesValidation="False" Visible="false" />
                            </td>
                    </tr>
                </table>
            </div>
            </asp:Panel>

            <asp:Panel ID="Panel2" runat="server" CssClass="noprint"><a name="Buppin_Button"></a>
                <div class="panel">
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
                            <asp:TextBox ID="TextBox_howMany" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" ToolTip="全角49文字以内" placeholder="例：1"></asp:TextBox>点
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
                                <asp:TextBox ID="TextBox_ps" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" TextMode="MultiLine" Rows="3" MaxLength="196" Height="100px" Width="415px" Style="resize: none" ToolTip="全角306文字以内" Text=""></asp:TextBox>
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
              </div>
            </asp:Panel>

            <asp:Panel ID="Panel3" runat="server" CssClass="noprint"><a id="Diligence_Button"></a>
            <div class="panel">
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
                                        <asp:Calendar ID="Calendar1" runat="server" OnSelectionChanged="Calendar1_SelectionChanged">
                                            <DayHeaderStyle BackColor="Black" />
                                            <DayStyle BackColor="#1E1E1E" ForeColor="White" />
                                            <OtherMonthDayStyle BackColor="Gray" />
                                            <SelectedDayStyle BackColor="AliceBlue" ForeColor="Blue" />
                                            <TitleStyle ForeColor="Black" />
                                            <TodayDayStyle ForeColor="Red" />
                                            <WeekendDayStyle BackColor="Black" />
                                          </asp:Calendar>
                                        <asp:Label ID="lblSelectedDateA1" runat="server" Text=""></asp:Label>
        
                                          <asp:DropDownList ID="DropDownList_A_Time" runat="server" OnTextChanged="DropDownList_A_Time_SelectionChanged">
                                            <asp:ListItem>9:00</asp:ListItem>
                                            <asp:ListItem>9:30</asp:ListItem>
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
                                        </asp:DropDownList>から
                                      </td>
                                      <td>
                                        <asp:Calendar ID="Calendar2" runat="server" OnSelectionChanged="Calendar2_SelectionChanged">
                                            <DayHeaderStyle BackColor="Black" />
                                            <DayStyle BackColor="#1E1E1E" ForeColor="White" />
                                            <OtherMonthDayStyle ForeColor="Gray" />
                                            <SelectedDayStyle BackColor="AliceBlue" ForeColor="Blue" />
                                            <TitleStyle ForeColor="Black" />
                                            <TodayDayStyle ForeColor="Red" />
                                            <WeekendDayStyle BackColor="Black" />
                                          </asp:Calendar>
                                        <asp:Label ID="lblSelectedDateB1" runat="server" Text=""></asp:Label>

                                        <asp:DropDownList ID="DropDownList_B_Time" runat="server" OnTextChanged="DropDownList_B_Time_SelectionChanged">
                                            <asp:ListItem>9:00</asp:ListItem>
                                            <asp:ListItem>9:30</asp:ListItem>
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
                                        </asp:DropDownList>まで
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
                           <asp:TextBox ID="TextBox_Notification_ps" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" TextMode="MultiLine" Rows="3" MaxLength="196" Height="100px" Width="415px" Style="resize: none" ToolTip="全角306文字以内" Text=""></asp:TextBox>
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
            </div>
            </asp:Panel>





                <asp:Panel ID="Panel6" runat="server" CssClass="noprint">
                <div class="panel">
                <table id="meisai">
                    <tr>
                        <td class="title">
                            <p>日付*</p>
                        </td>
                        <td class="text">
                            <asp:Calendar ID="Calendar3" runat="server" OnSelectionChanged="Calendar3_SelectionChanged">
                                <DayHeaderStyle BackColor="Black" ForeColor="White" />
                                <DayStyle BackColor="#1E1E1E" ForeColor="White" />
                                <OtherMonthDayStyle ForeColor="Gray" />
                                <SelectedDayStyle BackColor="AliceBlue" ForeColor="Blue" />
                                <TitleStyle ForeColor="Black" />
                                <TodayDayStyle ForeColor="Red" />
                                <WeekendDayStyle BackColor="Black" />
                            </asp:Calendar>
                            <asp:TextBox ID="TextBox_Tatekae_Date" runat="server" CssClass="textbox" Width="180px" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" placeholder="例：1月1日　1/1でもOK"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_Tatekae_Date"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <p>出張先</p>
                        </td>
                        <td class="text">
                            <asp:TextBox ID="TextBox_Tatekae_WPlace" runat="server" CssClass="textbox" Width="415px" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" placeholder="例：株式会社エム・ツー・エム" Text=""></asp:TextBox>
                        </td>
                        <td>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_Tatekae_WPlace"></asp:RequiredFieldValidator> --%>
                        </td>
                    </tr>

                    <tr>
                        <td class="title">
                            <p>交通機関*</p>
                        </td>
                        <td class="text">
                            <asp:DropDownList ID="DropDownList_Way" runat="server" AutoPostBack="true" OnTextChanged=" DropDownList_C_SelectionChanged">
                                <asp:ListItem Value="【選択補助】"></asp:ListItem>
                                <asp:ListItem Value="徒歩"></asp:ListItem>
                                <asp:ListItem Value="自転車"></asp:ListItem>
                                <asp:ListItem Value="電車"></asp:ListItem>
                                <asp:ListItem Value="新幹線"></asp:ListItem>
                                <asp:ListItem Value="社用車"></asp:ListItem>
                                <asp:ListItem Value="タクシー"></asp:ListItem>
                                <asp:ListItem Value="バス"></asp:ListItem>
                                <asp:ListItem Value="飛行機"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="TextBox_Tatekae_TUse" runat="server" CssClass="textbox" Width="415px" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" placeholder="例：電車　交通機関を利用していない場合は「徒歩」" Text="徒歩"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_Tatekae_TUse"></asp:RequiredFieldValidator>
                        </td>

                    </tr>
                    <tr>
                        <td class="title">
                            <p>乗車駅</p>
                        </td>
                        <td class="text">
                            <asp:TextBox ID="TextBox_Tatekae_TIn" runat="server" CssClass="textbox" Width="415px" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" placeholder="例：西新宿駅" Text=""></asp:TextBox>
                        </td>
                        <td>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_Tatekae_TIn"></asp:RequiredFieldValidator> --%>
                        </td>

                    </tr>
                    <tr>
                        <td class="title">
                            <p>降車駅</p>
                        </td>
                        <td class="text">
                            <asp:TextBox ID="TextBox_Tatekae_TOut" runat="server" CssClass="textbox" Width="415px" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" placeholder="例：西新宿駅" Text=""></asp:TextBox>
                        </td>
                        <td>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_Tatekae_TOut"></asp:RequiredFieldValidator> --%>
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
                            <p>宿泊場所</p>
                        </td>
                        <td class="text">
                            <asp:TextBox ID="TextBox_Tatekae_Place" runat="server" CssClass="textbox" Width="415px" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" placeholder="例：〇〇宿" ></asp:TextBox>
                        </td>
                        <td>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="※必須入力です" ForeColor="Red" ControlToValidate="TextBox_Tatekae_Place"></asp:RequiredFieldValidator> --%>
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
                                <asp:TextBox ID="TextBox_Tatekae_ps" runat="server" CssClass="textbox" ValidateRequestMode="Disabled" TextMode="MultiLine" Rows="3" MaxLength="196" Height="100px" Width="415px" Style="resize: none" ToolTip="全角306文字以内" Text=""></asp:TextBox>
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
                        <td colspan="3"><a id ="Tatekae_Button"></a>
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
            </div>
            </asp:Panel>


            <asp:Panel ID="Panel_WeeklyUI" runat="server" DefaultButton="Button_SaveWeeklyUI">
             <div class="panel">
                 <p>週報登録</p>
                <a id ="Weekly_Button"></a>
                <p><asp:DropDownList ID="DropDownList_WeeklyDate" runat="server" AutoPostBack="True" ValidateRequestMode="Disabled" OnSelectedIndexChanged="DropDownList_WeeklyChanged" >
                    <asp:ListItem>先週</asp:ListItem>
                    <asp:ListItem>今週</asp:ListItem>
                    <asp:ListItem>来週</asp:ListItem>
                    </asp:DropDownList></p>
                <asp:Button ID="Button_SaveWeeklyUI" CssClass="btn-flat-border" runat="server" Text="DBに新規保存" OnClick="Button_SaveWeeklyUI_Click" CausesValidation="False" PostBackUrl="#Weekly_Button" />
                <asp:Button ID="Button_WeeklyClose" CssClass="btn-flat-border" runat="server" Text="閉じる" OnClick="ResetButton_Click" CausesValidation="False" PostBackUrl="#Weekly_Button" />
                <asp:Button ID="Button_WeeklyPrint" CssClass="btn-flat-border" runat="server" Text="印刷ビュー" OnClick="WeeklyPrint_Click" CausesValidation="False" PostBackUrl="#Tatekae_Button" />
                <p><asp:Label ID="Label_WeeklyUI_SaveTest" runat="server" Text="チェックボックスの結果をここでテスト表示します。"></asp:Label></p>
             </div>
            </asp:Panel>


            </div><%-- Wrap ここまでプリントしない --%>































                <asp:Panel ID="Panel_Weekly" runat="server">
                    <table id="WeeklyTable">
                        <tr>
                            <td colspan="20" class="left">
                                <asp:Label ID="Label_Weekly_year" runat="server" Text="yyyy年"></asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="Label_Weekly_name1" runat="server" Text="M2M ASP"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="3" class="dashrow">
                            </td>
                            <td rowspan="3" class="company">
                                担当会社/団体
                            </td>
                            <td rowspan="3" class="work">
                                主要業務内容
                            </td>
                            <td colspan="3">
                                <asp:Label ID="Label_Weekly_date1" runat="server" Text="M月d日"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="Label_Weekly_date2" runat="server" Text="M月d日"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="Label_Weekly_date3" runat="server" Text="M月d日"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="Label_Weekly_date4" runat="server" Text="M月d日"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="Label_Weekly_date5" runat="server" Text="M月d日"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="Label_Weekly_date6" runat="server" Text="M月d日"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="Label_Weekly_date7" runat="server" Text="M月d日"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                （月）
                            </td>
                            <td colspan="3">
                                （火）
                            </td>
                            <td colspan="3">
                                （水）
                            </td>
                            <td colspan="3">
                                （木）
                            </td>
                            <td colspan="3">
                                （金）
                            </td>
                            <td colspan="3">
                                （土）
                            </td>
                            <td colspan="3">
                                （日）
                            </td>
                        </tr>
                        <tr>
                            <td class="time">
                                午前
                            </td>
                            <td class="time">
                                午後
                            </td>
                            <td class="time">
                                時間外
                            </td>
                            <td class="time">
                                午前
                            </td>
                            <td class="time">
                                午後
                            </td>
                            <td class="time">
                                時間外
                            </td>
                            <td class="time">
                                午前
                            </td>
                            <td class="time">
                                午後
                            </td>
                            <td class="time">
                                時間外
                            </td>
                            <td class="time">
                                午前
                            </td>
                            <td class="time">
                                午後
                            </td>
                            <td class="time">
                                時間外
                            </td>
                            <td class="time">
                                午前
                            </td>
                            <td class="time">
                                午後
                            </td>
                            <td class="time">
                                時間外
                            </td>
                            <td class="time">
                                午前
                            </td>
                            <td class="time">
                                午後
                            </td>
                            <td class="time">
                                時間外
                            </td>
                            <td class="time">
                                午前
                            </td>
                            <td class="time">
                                午後
                            </td>
                            <td class="time">
                                時間外
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="12">
                                開<br />発<br />/<br />運<br />用
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly1" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly2" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly1" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly2" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly3" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly4" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly5" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly6" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly7" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly8" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly9" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly10" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly11" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly12" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly13" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly14" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly15" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly16" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly17" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly18" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly19" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly20" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly21" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly3" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly4" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly22" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly23" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly24" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly25" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly26" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly27" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly28" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly29" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly30" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly31" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly32" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly33" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly34" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly35" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly36" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly37" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly38" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly39" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly40" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly41" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly42" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly5" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly6" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly43" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly44" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly45" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly46" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly47" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly48" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly49" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly50" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly51" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly52" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly53" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly54" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly55" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly56" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly57" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly58" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly59" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly60" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly61" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly62" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly63" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly7" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly8" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly64" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly65" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly66" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly67" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly68" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly69" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly70" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly71" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly72" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly73" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly74" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly75" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly76" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly77" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly78" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly79" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly80" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly81" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly82" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly83" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly84" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly9" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly10" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly85" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly86" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly87" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly88" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly89" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly90" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly91" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly92" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly93" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly94" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly95" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly96" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly97" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly98" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly99" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly100" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly101" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly102" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly103" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly104" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly105" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly11" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly12" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly106" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly107" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly108" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly109" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly110" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly111" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly112" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly113" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly114" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly115" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly116" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly117" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly118" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly119" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly120" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly121" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly122" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly123" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly124" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly125" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly126" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly13" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly14" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly127" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly128" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly129" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly130" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly131" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly132" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly133" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly134" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly135" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly136" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly137" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly138" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly139" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly140" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly141" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly142" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly143" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly144" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly145" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly146" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly147" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly15" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly16" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly148" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly149" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly150" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly151" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly152" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly153" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly154" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly155" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly156" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly157" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly158" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly159" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly160" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly161" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly162" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly163" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly164" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly165" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly166" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly167" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly168" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly17" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly18" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly169" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly170" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly171" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly172" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly173" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly174" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly175" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly176" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly177" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly178" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly179" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly180" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly181" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly182" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly183" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly184" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly185" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly186" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly187" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly188" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly189" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly19" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly20" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly190" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly191" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly192" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly193" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly194" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly195" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly196" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly197" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly198" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly199" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly200" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly201" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly202" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly203" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly204" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly205" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly206" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly207" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly208" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly209" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly210" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly21" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly22" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly211" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly212" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly213" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly214" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly215" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly216" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly217" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly218" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly219" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly220" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly221" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly222" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly223" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly224" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly225" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly226" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly227" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly228" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly229" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly230" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly231" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly23" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly24" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly232" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly233" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly234" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly235" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly236" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly237" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly238" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly239" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly240" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly241" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly242" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly243" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly244" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly245" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly246" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly247" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly248" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly249" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly250" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly251" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly252" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="12">
                                W<br />e<br />b<br />訪<br />問<br />等
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly25" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly26" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly253" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly254" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly255" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly256" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly257" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly258" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly259" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly260" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly261" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly262" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly263" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly264" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly265" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly266" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly267" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly268" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly269" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly270" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly271" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly272" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly273" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly27" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly28" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly274" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly275" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly276" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly277" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly278" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly279" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly280" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly281" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly282" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly283" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly284" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly285" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly286" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly287" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly288" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly289" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly290" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly291" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly292" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly293" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly294" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly29" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly30" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly295" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly296" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly297" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly298" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly299" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly300" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly301" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly302" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly303" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly304" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly305" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly306" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly307" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly308" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly309" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly310" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly311" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly312" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly313" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly314" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly315" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly31" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly32" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly316" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly317" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly318" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly319" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly320" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly321" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly322" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly323" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly324" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly325" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly326" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly327" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly328" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly329" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly330" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly331" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly332" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly333" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly334" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly335" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly336" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly33" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly34" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly337" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly338" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly339" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly340" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly341" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly342" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly343" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly344" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly345" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly346" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly347" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly348" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly349" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly350" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly351" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly352" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly353" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly354" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly355" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly356" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly357" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly35" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly36" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly358" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly359" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly360" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly361" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly362" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly363" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly364" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly365" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly366" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly367" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly368" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly369" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly370" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly371" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly372" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly373" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly374" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly375" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly376" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly377" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly378" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly37" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly38" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly379" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly380" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly381" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly382" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly383" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly384" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly385" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly386" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly387" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly388" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly389" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly390" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly391" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly392" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly393" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly394" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly395" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly396" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly397" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly398" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly399" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly39" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly40" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly400" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly401" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly402" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly403" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly404" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly405" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly406" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly407" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly408" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly409" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly410" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly411" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly412" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly413" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly414" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly415" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly416" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly417" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly418" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly419" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly420" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly41" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly42" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly421" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly422" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly423" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly424" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly425" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly426" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly427" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly428" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly429" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly430" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly431" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly432" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly433" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly434" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly435" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly436" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly437" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly438" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly439" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly440" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly441" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly43" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly44" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly442" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly443" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly444" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly445" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly446" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly447" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly448" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly449" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly450" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly451" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly452" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly453" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly454" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly455" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly456" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly457" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly458" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly459" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly460" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly461" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly462" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly45" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly46" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly463" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly464" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly465" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly466" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly467" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly468" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly469" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly470" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly471" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly472" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly473" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly474" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly475" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly476" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly477" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly478" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly479" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly480" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly481" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly482" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly483" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly47" runat="server" CssClass="weeklytxt" Width="10em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_Weekly48" runat="server" CssClass="weeklytxt" Width="12em" ValidateRequestMode="Disabled" ToolTip="全角50文字以内" Text="" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly484" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly485" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly486" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly487" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly488" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly489" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly490" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly491" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly492" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly493" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly494" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly495" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly496" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly497" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly498" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly499" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly500" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly501" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly502" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly503" runat="server" CssClass="weeklyobj" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox_Weekly504" runat="server" CssClass="weeklyobj" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>



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
