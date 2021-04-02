using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MizunoDAL
{   
    public class ProperOrderClass
    {
        public enum EnumMsgStatus
        {
            None = 0, Jushin = 1, Shoushin = 2, Rireki = 3
        }

        public enum EnumChumonStatus
        {
            None = 3,
            Mishukka = 0,       // 未出荷
            ShukkaZumi = 1,     // 出荷済み
            MiUketuke = 2       // 未受付
        }


        [Serializable]
        public class KensakuParam
        {
            // ユーザー区分
            public EnumUserType UserType = EnumUserType.Shiiresaki;

            // 表示可能期間 2012.04.05追加
            public int nHyoujiTukisu = 24; // ミズノ受付日の過去24カ月分を閲覧可能

            // 指図日
            public Core.Type.NengappiKikan _SashizuBi = null;

            // 仕入先コード
            public string _ShiiresakiCode = "";

            public List<string> lstTuikaShiiresakiCode = null; // 子企業


            // 生産オーダーNo
            public string _SeisanOrderNo = "";
            // 引取オーダーNo
            public string _HikitoriOrderNo = "";
            // 状況
            public EnumChumonStatus ChumonStatus = EnumChumonStatus.None;

            public List<ProperOrderKey> lstProperOrderKey = new List<ProperOrderKey>();

            // 予算月                        
            public Core.Type.Nengetu ngYosanTuki = null;

            // 品番
            public string _Hinban = "";
            // 納品記号
            public string _NouhinKigou = null;
            // 荷受事業所
            public string _NiukeJigyousho = null;

            // 再発注フラグ
            public EnumYesNo SaiHacchuFlg = EnumYesNo.None;


            public EnumMsgStatus MsgStatus = EnumMsgStatus.None;

            public ViewDataset.VIEW_ProperOrderHinmokuDataTable
                getVIEW_ProperOrderHinmokuDataTable(SqlConnection sqlConn)
            {
                SqlDataAdapter da = new SqlDataAdapter("", sqlConn);

                Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
                GetWhereT_ProperOrder(this, w, da.SelectCommand);
                string strWhereT_ProperOrder = (string.IsNullOrEmpty(w.WhereText)) ? "" : " where " + w.WhereText;


                // このデータのグループ化のキーに対して検索
                Core.Sql.WhereGenerator wXXX = new Core.Sql.WhereGenerator();

                // 事業所(空白あり)
                if (null != this._NiukeJigyousho)
                {
                    wXXX.Add(@"exists (select * from T_ProperOrder where 
T_ProperOrder.SashizuBi=T_XXXXX.SashizuBi and 
T_ProperOrder.SeisanOrderNo=T_XXXXX.SeisanOrderNo and 
T_ProperOrder.HikitoriOrderNo=T_XXXXX.HikitoriOrderNo and 
T_ProperOrder.Hinban=T_XXXXX.Hinban and 
T_ProperOrder.YosanTsuki=T_XXXXX.YosanTsuki and
T_ProperOrder.NouhinKigou=T_XXXXX.NouhinKigou and
T_ProperOrder.NiukeJigyoushoCode=@NiukeJigyoushoCode)");
                    da.SelectCommand.Parameters.AddWithValue("@NiukeJigyoushoCode", this._NiukeJigyousho);
                }


                // 注文状況
                switch (this.ChumonStatus)
                {
                    case EnumChumonStatus.MiUketuke:
                        wXXX.Add(@"not exists (select * from T_ProperUketsuke where 
T_ProperUketsuke.SashizuBi=T_XXXXX.SashizuBi and 
T_ProperUketsuke.SeisanOrderNo=T_XXXXX.SeisanOrderNo and 
T_ProperUketsuke.HikitoriOrderNo=T_XXXXX.HikitoriOrderNo)
");
                        break;
                    case EnumChumonStatus.Mishukka:
                        // 未出荷
                        wXXX.Add(@"exists (select * from T_ProperOrder where 
T_ProperOrder.SashizuBi=T_XXXXX.SashizuBi and 
T_ProperOrder.SeisanOrderNo=T_XXXXX.SeisanOrderNo and 
T_ProperOrder.HikitoriOrderNo=T_XXXXX.HikitoriOrderNo and 
T_ProperOrder.Hinban=T_XXXXX.Hinban and 
T_ProperOrder.YosanTsuki=T_XXXXX.YosanTsuki and
T_ProperOrder.NouhinKigou=T_XXXXX.NouhinKigou and 
T_ProperOrder.KanryouFlg =0 AND T_ProperOrder.Suuryou > T_ProperOrder.ShukkaSuu)");
                        break;
                    case EnumChumonStatus.ShukkaZumi:
                        // 出荷済み
                        wXXX.Add(@"exists (select * from T_ProperOrder where 
T_ProperOrder.SashizuBi         =T_XXXXX.SashizuBi and 
T_ProperOrder.SeisanOrderNo     =T_XXXXX.SeisanOrderNo and 
T_ProperOrder.HikitoriOrderNo   =T_XXXXX.HikitoriOrderNo and 
T_ProperOrder.Hinban            =T_XXXXX.Hinban and 
T_ProperOrder.YosanTsuki        =T_XXXXX.YosanTsuki and
T_ProperOrder.NouhinKigou       =T_XXXXX.NouhinKigou and 
(T_ProperOrder.KanryouFlg=1 or T_ProperOrder.Suuryou=T_ProperOrder.ShukkaSuu))");
                        break;
                }

                da.SelectCommand.CommandText = string.Format(@"
select * from (
SELECT                  Base.SashizuBi, Base.ShiiresakiCode, Base.SeisanOrderNo, Base.HikitoriOrderNo, Base.YosanTsuki, Base.NouhinKigou, Base.Hinban, Base.Suuryou, 
Base.ShukkaSuu, Base.KanryouFlgCount, Base.RecordCount, dbo.VIEW_ProperOrder_SaishuShukkaBi.SaishuShukkaBi
FROM                     (SELECT                  SashizuBi, ShiiresakiCode, SeisanOrderNo, HikitoriOrderNo, YosanTsuki, NouhinKigou, Hinban, SUM(Suuryou) AS Suuryou, 
SUM(ShukkaSuu) AS ShukkaSuu, SUM(CASE KanryouFlg WHEN 1 THEN 1 ELSE 0 END) AS KanryouFlgCount, COUNT(*) 
AS RecordCount
FROM                     dbo.T_ProperOrder {0} 
GROUP BY          SashizuBi, ShiiresakiCode, SeisanOrderNo, HikitoriOrderNo, Hinban, YosanTsuki, NouhinKigou) AS Base LEFT OUTER JOIN
dbo.VIEW_ProperOrder_SaishuShukkaBi ON Base.SashizuBi = dbo.VIEW_ProperOrder_SaishuShukkaBi.SashizuBi AND 
Base.SeisanOrderNo = dbo.VIEW_ProperOrder_SaishuShukkaBi.SeisanOrderNo AND 
Base.HikitoriOrderNo = dbo.VIEW_ProperOrder_SaishuShukkaBi.HikitoriOrderNo AND Base.Hinban = dbo.VIEW_ProperOrder_SaishuShukkaBi.Hinban
) T_XXXXX {1}
", strWhereT_ProperOrder, (string.IsNullOrEmpty(wXXX.WhereText) ? "" : " where " + wXXX.WhereText));

                ViewDataset.VIEW_ProperOrderHinmokuDataTable dt = new ViewDataset.VIEW_ProperOrderHinmokuDataTable();

                da.Fill(dt);

                return dt;
            }

            //完了フラグが正しく取得できない為、テーブルを変更する
            public DataTable getT_ProperOrderDataTable(SqlConnection sqlConn)
            {
                //完了フラグを正しく取得できるようにクエリ変更 20160920
                //予算品テーブル変更時にはここの値にも変更を加えること
                SqlDataAdapter da = new SqlDataAdapter(
                    @"select
                        SashizuBi,ShiiresakiCode,SeisanOrderNo,HikitoriOrderNo,RowNo,YosanTsuki
                        ,HikitoriNouki,Hinban,Size,Spec,TukaCode,Kakaku,NihonShiireKakaku,Suuryou
                        ,NouhinKigou,NiukeJigyoushoCode,NiukeJigyoushoMei,HokanBasho,Sekiduke
                        ,DenpyouKubun,SaiHacchuFlg,TsuikaInfo1,TsuikaInfo2,Tantousha,Shouninsha
                        ,JanCode,ShukkaSuu
                        ,ISNULL(CASE (SELECT MAX(M.KanryouFlg) FROM T_NouhinHeader H INNER JOIN
	                        T_NouhinMeisai M ON M.YYMM = H.YYMM AND M.HakkouNo = H.HakkouNo AND
		                        M.ShiiresakiCode = H.NouhinMotoshiiresakiCode
                         WHERE H.SashizuBi = T_ProperOrder.SashizuBi AND H.NouhinmotoShiiresakiCode = T_ProperOrder.ShiiresakiCode AND
	                        (H.SeisanOrderNo = T_ProperOrder.SeisanOrderNo AND H.HikitoriOrderNo = T_ProperOrder.HikitoriOrderNo) AND
	                        M.Hinban = T_ProperOrder.Hinban AND M.Size = T_ProperOrder.Size) WHEN 8 THEN 9 WHEN 9 THEN 9 ELSE 0 END,0) AS KanryouFlg
                        ,TourokuBi
                      from T_ProperOrder", sqlConn);
                Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
                GetWhereT_ProperOrder(this, w, da.SelectCommand);


                // 事業所
                if (!string.IsNullOrEmpty(this._NiukeJigyousho))
                {
                    w.Add(string.Format("T_ProperOrder.NiukeJigyoushoCode = '{0}'", this._NiukeJigyousho));
                }


                // 状況
                switch (this.ChumonStatus)
                {
                    case EnumChumonStatus.MiUketuke:
                        w.Add(@"not exists (select * from T_ProperUketsuke where 
T_ProperUketsuke.SashizuBi=T_ProperOrder.SashizuBi and 
T_ProperUketsuke.SeisanOrderNo=T_ProperOrder.SeisanOrderNo and 
T_ProperUketsuke.HikitoriOrderNo=T_ProperOrder.HikitoriOrderNo)");
                        break;
                    case EnumChumonStatus.Mishukka:
                        // 未出荷
                        w.Add("T_ProperOrder.KanryouFlg =0 AND T_ProperOrder.Suuryou > T_ProperOrder.ShukkaSuu ");
                        break;
                    case EnumChumonStatus.ShukkaZumi:
                        w.Add("((T_ProperOrder.KanryouFlg=9 or T_ProperOrder.KanryouFlg=8) or T_ProperOrder.Suuryou=T_ProperOrder.ShukkaSuu)");
                        break;
                }

                if (!string.IsNullOrEmpty(w.WhereText))
                    da.SelectCommand.CommandText += " where " + w.WhereText;

                //完了フラグが正しく取得できない為、テーブルを変更する
                //MizunoDataSet.T_ProperOrderDataTable dt = new MizunoDataSet.T_ProperOrderDataTable();
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            public ViewDataset.VIEW_ProperOrderKeyDataTable
                getVIEW_ProperOrderKeyDataTable(Core.Sql.RowNumberInfo r, SqlConnection sqlConn, ref int nDataCount)
            {
                SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
                Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
                GetWhereT_ProperOrder(this, w, da.SelectCommand);

                // 事業所(空白あり)
                if (null != this._NiukeJigyousho)
                {
                    w.Add(string.Format("T_ProperOrder.NiukeJigyoushoCode = '{0}'", this._NiukeJigyousho));
                }

                // 状況
                switch (this.ChumonStatus)
                {
                    case EnumChumonStatus.MiUketuke:
                        w.Add(@"not exists (select * from T_ProperUketsuke where 
T_ProperUketsuke.SashizuBi=T_ProperOrder.SashizuBi and 
T_ProperUketsuke.SeisanOrderNo=T_ProperOrder.SeisanOrderNo and 
T_ProperUketsuke.HikitoriOrderNo=T_ProperOrder.HikitoriOrderNo)");
                        break;
                    case EnumChumonStatus.Mishukka:
                        // 未出荷
                        w.Add("(T_ProperOrder.KanryouFlg =0 or T_ProperOrder.KanryouFlg is null) AND T_ProperOrder.Suuryou > T_ProperOrder.ShukkaSuu ");
                        break;
                    case EnumChumonStatus.ShukkaZumi:
                        w.Add("(T_ProperOrder.KanryouFlg=1 or T_ProperOrder.Suuryou=T_ProperOrder.ShukkaSuu)");
                        break;
                }

                string strWhereT_ProperOrder = (string.IsNullOrEmpty(w.WhereText)) ? "" : " where " + w.WhereText;

                da.SelectCommand.CommandText = string.Format(@"
SELECT                  W_Base.SashizuBi, W_Base.SeisanOrderNo, W_Base.HikitoriOrderNo, W_Base.ShiiresakiCode, dbo.VIEW_Shiiresaki.ShiiresakiMei, 
dbo.VIEW_Shiiresaki.RyakuMei, dbo.VIEW_ProperOrder_LatestMsg.KaishaCode AS VIEW_ProperOrder_LatestMsg_KaishaCode, 
dbo.VIEW_ProperOrder_LatestMsg.OpenedKaishaCode AS VIEW_ProperOrder_LatestMsg_KaishaCode_OpenedKaishaCode, W_Base.SaiHacchuFlg, 
W_Base.Tantousha, W_Base.Shouninsha
FROM                     (SELECT                  SashizuBi, SeisanOrderNo, HikitoriOrderNo, ShiiresakiCode, MAX(SaiHacchuFlg) AS SaiHacchuFlg, MAX(Tantousha) AS Tantousha, 
MAX(Shouninsha) AS Shouninsha
FROM                     dbo.T_ProperOrder {0} 
GROUP BY          SashizuBi, SeisanOrderNo, HikitoriOrderNo, ShiiresakiCode) AS W_Base 

LEFT OUTER JOIN
                                      (SELECT                  ShiiresakiCode, SeisanOrderNo, HikitoriOrderNo
                                            FROM                     dbo.T_ProperOrder AS T_ProperOrder_1
                                            WHERE                   (KanryouFlg = 0 OR
                                                                              KanryouFlg IS NULL) AND (Suuryou > ShukkaSuu)
                                            GROUP BY          ShiiresakiCode, SeisanOrderNo, HikitoriOrderNo) AS W_Kanryou ON W_Kanryou.ShiiresakiCode = W_Base.ShiiresakiCode AND 
                                  W_Kanryou.SeisanOrderNo = W_Base.SeisanOrderNo AND W_Kanryou.HikitoriOrderNo = W_Base.HikitoriOrderNo 


LEFT OUTER JOIN
dbo.VIEW_ProperOrder_LatestMsg ON W_Base.SashizuBi = dbo.VIEW_ProperOrder_LatestMsg.SashizuBi AND 
W_Base.SeisanOrderNo = dbo.VIEW_ProperOrder_LatestMsg.SeisanOrderNo AND 
W_Base.HikitoriOrderNo = dbo.VIEW_ProperOrder_LatestMsg.HikitoriOrderNo LEFT OUTER JOIN
dbo.VIEW_Shiiresaki ON W_Base.ShiiresakiCode = dbo.VIEW_Shiiresaki.ShiiresakiCode", strWhereT_ProperOrder);

                if (this.ChumonStatus == EnumChumonStatus.ShukkaZumi)
                {
                    da.SelectCommand.CommandText += " WHERE  W_Kanryou.ShiiresakiCode IS NULL";
                }

                ViewDataset.VIEW_ProperOrderKeyDataTable dt = new ViewDataset.VIEW_ProperOrderKeyDataTable();
                r.LoadData(da.SelectCommand, sqlConn, dt, ref nDataCount);
                return dt;
            }


            /// <summary>
            /// VIEW_ProperOrderKeyDataTable とVIEW_ProperOrderHinmokuDataTableでグループ化する前のT_ProperOrderに対して設定できる共通の条件
            /// </summary>
            /// <param name="k"></param>
            /// <param name="w"></param>
            /// <param name="cmd"></param>
            private void GetWhereT_ProperOrder(KensakuParam k, Core.Sql.WhereGenerator w, SqlCommand cmd)
            {
                // ミズノ受付日表示対象期間 2012.04.05追加
                if (0 < k.nHyoujiTukisu)
                {
                    // 注残であっても表示対象期間を設定する。
                    w.Add("CAST(SashizuBi AS DATETIME) >= CAST(@uuu AS DATETIME)");
                    cmd.Parameters.AddWithValue("@uuu", DateTime.Today.AddMonths((-1) * k.nHyoujiTukisu));
                }

                if (null != lstProperOrderKey && 0 < lstProperOrderKey.Count)
                {
                    string[] str = new string[lstProperOrderKey.Count];
                    for (int i = 0; i < lstProperOrderKey.Count; i++)
                    {
                        str[i] = string.Format("(T_ProperOrder.SashizuBi=@sn{0} and T_ProperOrder.SeisanOrderNo=@so{0} and T_ProperOrder.HikitoriOrderNo=@ho{0})", i);
                        cmd.Parameters.AddWithValue(string.Format("@sn{0}", i), lstProperOrderKey[i].SashizuBi);
                        cmd.Parameters.AddWithValue(string.Format("@so{0}", i), lstProperOrderKey[i].SeisanOrderNo);
                        cmd.Parameters.AddWithValue(string.Format("@ho{0}", i), lstProperOrderKey[i].HikitoriOrderNo);
                    }
                    w.Add(string.Format("(({0}))", string.Join(") or (", str)));
                }


                // 指図日
                if (k._SashizuBi != null)
                {
                    w.Add(Core.Type.NengappiKikan.GenerateSQL(k._SashizuBi, false, "T_ProperOrder.SashizuBi"));
                }

                // 仕入先コード(指定あり)

                if (k._ShiiresakiCode != "")
                {
                    w.Add(string.Format("T_ProperOrder.ShiiresakiCode = @ShiiresakiCode"));
                    cmd.Parameters.AddWithValue("@ShiiresakiCode", k._ShiiresakiCode);
                }
                // 仕入先コード(複数)
                if (k.lstTuikaShiiresakiCode != null && 0 < k.lstTuikaShiiresakiCode.Count)
                {
                    w.Add(string.Format("T_ProperOrder.ShiiresakiCode in ('{0}')", string.Join("','", k.lstTuikaShiiresakiCode.ToArray())));
                }


                // 生産オーダーNo
                if (!string.IsNullOrEmpty(k._SeisanOrderNo))
                {
                    w.Add(string.Format("T_ProperOrder.SeisanOrderNo LIKE @SeisanOrderNo"));
                    cmd.Parameters.AddWithValue("@SeisanOrderNo", "%" + k._SeisanOrderNo + "%");
                }
                // 引取オーダーNo
                if (!string.IsNullOrEmpty(k._HikitoriOrderNo))
                {
                    w.Add(string.Format("T_ProperOrder.HikitoriOrderNo LIKE @HikitoriOrderNo"));
                    cmd.Parameters.AddWithValue("@HikitoriOrderNo", k._HikitoriOrderNo + "%");
                }


                // 予算月
                if (null != k.ngYosanTuki)
                {
                    w.Add(string.Format("T_ProperOrder.YosanTsuki='{0}'", k.ngYosanTuki.ToYYYYMM()));
                }

                // 品番
                if (k._Hinban != "")
                {
                    w.Add(string.Format("LTRIM(T_ProperOrder.Hinban) LIKE @Hinban"));
                    cmd.Parameters.AddWithValue("@Hinban", k._Hinban + "%");
                }
                // 納品記号(空白もありあえる)
                if (null != k._NouhinKigou)
                {
                    w.Add(string.Format("T_ProperOrder.NouhinKigou = '{0}'", k._NouhinKigou.Trim()));
                }

                // 再発注フラグ(注文単位(SashizuBi+SeisanOrderNo+HikitoriOrderNo)で設定されるもの)
                switch (this.SaiHacchuFlg)
                {
                    case EnumYesNo.Yes:
                        // あり
                        w.Add("T_ProperOrder.SaiHacchuFlg <> ''");
                        break;
                    case EnumYesNo.No:
                        // なし
                        w.Add("T_ProperOrder.SaiHacchuFlg = ''");
                        break;
                }


                // メッセージ(メッセージは注文単位(SashizuBi+SeisanOrderNo+HikitoriOrderNo)で設定される)
                string strForShiiresaki = "";
                if (this.UserType == EnumUserType.Shiiresaki)
                {
                    // 本予算品データは親/子企業で検索されるが、メッセージはログインしている仕入先で検索する為、この条件が必要
                    strForShiiresaki = string.Format(" and VIEW_ProperOrder_LatestMsg.ShiiresakiCode='{0}'", this._ShiiresakiCode);
                }
                string strMizuno2Torihikisaki = string.Format(@"exists (select * from VIEW_ProperOrder_LatestMsg where 
VIEW_ProperOrder_LatestMsg.SashizuBi=T_ProperOrder.SashizuBi and 
VIEW_ProperOrder_LatestMsg.SeisanOrderNo=T_ProperOrder.SeisanOrderNo and 
VIEW_ProperOrder_LatestMsg.HikitoriOrderNo=T_ProperOrder.HikitoriOrderNo and 
VIEW_ProperOrder_LatestMsg.OpenedKaishaCode is null and 
VIEW_ProperOrder_LatestMsg.KaishaCode='0' {0})", strForShiiresaki);

                string strTorihikisaki2Mizuno = string.Format(@"exists (select * from VIEW_ProperOrder_LatestMsg where 
VIEW_ProperOrder_LatestMsg.SashizuBi=T_ProperOrder.SashizuBi and 
VIEW_ProperOrder_LatestMsg.SeisanOrderNo=T_ProperOrder.SeisanOrderNo and 
VIEW_ProperOrder_LatestMsg.HikitoriOrderNo=T_ProperOrder.HikitoriOrderNo and 
VIEW_ProperOrder_LatestMsg.OpenedKaishaCode is null and 
VIEW_ProperOrder_LatestMsg.KaishaCode<>'0' {0})", strForShiiresaki);

                switch (this.MsgStatus)
                {
                    case EnumMsgStatus.Jushin:
                        // 受信
                        if (this.UserType == EnumUserType.Shiiresaki)
                            w.Add(strMizuno2Torihikisaki);
                        else
                            w.Add(strTorihikisaki2Mizuno);
                        break;
                    case EnumMsgStatus.Shoushin:
                        // 送信
                        if (this.UserType == EnumUserType.Shiiresaki)
                            w.Add(strTorihikisaki2Mizuno);
                        else
                            w.Add(strMizuno2Torihikisaki);
                        break;
                    case EnumMsgStatus.Rireki:
                        w.Add(string.Format(@"
exists (select * from VIEW_ProperOrder_LatestMsg where 
VIEW_ProperOrder_LatestMsg.SashizuBi=T_ProperOrder.SashizuBi and 
VIEW_ProperOrder_LatestMsg.SeisanOrderNo=T_ProperOrder.SeisanOrderNo and 
VIEW_ProperOrder_LatestMsg.HikitoriOrderNo=T_ProperOrder.HikitoriOrderNo and 
VIEW_ProperOrder_LatestMsg.OpenedKaishaCode is not null {0})", strForShiiresaki));
                        break;
                }


            }

        }


    
        /// <summary>
        /// 検索条件（月別商品別発注一覧フォーム用）
        /// </summary>
        public class KensakuParam_HacchuIchiran
        {
            // ユーザー区分
            public byte _UserKubun = 0;
            // 仕入先コード
            public string _ShiiresakiCode = "";
            // 仕入先コード
            public string[] _SCodeAry = null;
            // 部門
            public string _Bumon = "";
            // 品番
            public string _Hinban = "";
            // 表示年月(From)
            public string _From = "";
            // 表示年月(To)
            public string _To = "";
        }
        

        /// <summary>
        /// 予算品の1オーダーあたりの主キー(テーブルの主キーとは異なる)
        /// </summary>
        public class ProperOrderKey
        {
            public ProperOrderKey(string strSashizuBi, string strSeisanOrderNo, string strHikitoriOrderNo)
            {
                this.SashizuBi = strSashizuBi;
                this.SeisanOrderNo = strSeisanOrderNo;
                this.HikitoriOrderNo = strHikitoriOrderNo;     
            }
            public ProperOrderKey(string strKeyAry)
            {                
                string [] strAry = strKeyAry.Split('_');
                this.SashizuBi = strAry[0];
                this.SeisanOrderNo = strAry[1];
                this.HikitoriOrderNo = strAry[2];                
            }

            public ProperOrderKey()
            {
            }

            public override string ToString()
            {
                return this.SashizuBi + "_" + this.SeisanOrderNo + "_" + this.HikitoriOrderNo;
            }
            public string SashizuBi
            {
                get;
                set;
            }
            public string SeisanOrderNo
            {
                get;
                set;
            }
            public string HikitoriOrderNo
            {
                get;
                set;
            }

        }

        

        /// <summary>
        /// 予算品データ全行取得(これ何？？？？)
        /// </summary>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static MizunoDataSet.T_ProperOrderRow
            getT_ProperOrderRow(string strShiireCode, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT TOP(1) * FROM T_ProperOrder ";

            if (strShiireCode != "0")
            {
                da.SelectCommand.CommandText += " WHERE  (ShiiresakiCode = @ShiireCode) "; 
            }

            da.SelectCommand.CommandText += " ORDER BY YosanTsuki DESC ";
            da.SelectCommand.Parameters.AddWithValue("@ShiireCode", strShiireCode);
            MizunoDataSet.T_ProperOrderDataTable dt = new MizunoDataSet.T_ProperOrderDataTable();
           
            da.Fill(dt);
            if (1 == dt.Rows.Count)
                return (MizunoDataSet.T_ProperOrderRow)dt.Rows[0];
            else
                return null;
        }

        public static MizunoDataSet.T_ProperOrderRow
            getT_ProperOrderRow(string SashizuBi, string SeisanOrderNo, string HikitoriOrderNo, SqlTransaction sqlTran)
        {
            string sql = "SELECT * FROM T_ProperOrder WHERE SashizuBi = @SashizuBi AND SeisanOrderNo = @SeisanOrderNo AND HikitoriOrderNo = @HikitoriOrderNo";
            SqlDataAdapter da = new SqlDataAdapter(sql, sqlTran.Connection);
            da.SelectCommand.Transaction = sqlTran;
            da.SelectCommand.Parameters.AddWithValue("@SashizuBi", SashizuBi);
            da.SelectCommand.Parameters.AddWithValue("@SeisanOrderNo", SeisanOrderNo);
            da.SelectCommand.Parameters.AddWithValue("@HikitoriOrderNo", HikitoriOrderNo);
            MizunoDataSet.T_ProperOrderDataTable dt = new MizunoDataSet.T_ProperOrderDataTable();
            
            da.Fill(dt);
            
            if (dt.Count == 0) { return null; }

            return dt[0];
        }



        public static List<string> GetNiukeJigyoushoCode(ProperOrderKey key, string strHinban, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT NiukeJigyoushoCode FROM T_ProperOrder WHERE (SashizuBi = @SashizuBi) AND (SeisanOrderNo = @SeisanOrderNo) AND (HikitoriOrderNo = @HikitoriOrderNo) {0} 
Group by NiukeJigyoushoCode ORDER BY NiukeJigyoushoCode";

            da.SelectCommand.Parameters.AddWithValue("@SashizuBi", key.SashizuBi);
            da.SelectCommand.Parameters.AddWithValue("@SeisanOrderNo", key.SeisanOrderNo);
            da.SelectCommand.Parameters.AddWithValue("@HikitoriOrderNo", key.HikitoriOrderNo);

            // 品番指定あり
            if (!string.IsNullOrEmpty(strHinban))
            {
                da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, " AND Hinban = @Hinban");
                da.SelectCommand.Parameters.AddWithValue("@Hinban", strHinban);
            }
            else
                da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, "");

            DataTable dt = new DataTable();
            da.Fill(dt);
            List<string> lst = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++) {
                lst.Add(Convert.ToString(dt.Rows[i][0]));
            }
            return lst;
        }


        public static List<string> GetNouhinKigou(ProperOrderKey key, string strHinban, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT NouhinKigou FROM T_ProperOrder WHERE (SashizuBi = @SashizuBi) AND (SeisanOrderNo = @SeisanOrderNo) AND (HikitoriOrderNo = @HikitoriOrderNo) {0} 
Group by NouhinKigou ORDER BY NouhinKigou";


            da.SelectCommand.Parameters.AddWithValue("@SashizuBi", key.SashizuBi);
            da.SelectCommand.Parameters.AddWithValue("@SeisanOrderNo", key.SeisanOrderNo);
            da.SelectCommand.Parameters.AddWithValue("@HikitoriOrderNo", key.HikitoriOrderNo);
            

            if (!string.IsNullOrEmpty(strHinban))
            {
                da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, " AND Hinban = @Hinban");
                da.SelectCommand.Parameters.AddWithValue("@Hinban", strHinban);
            }
            else
                da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, "");

            DataTable dt = new DataTable();
            da.Fill(dt);
            List<string> lst = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst.Add(Convert.ToString(dt.Rows[i][0]));
            }
            return lst;
        }



        public static List<Core.Type.Nengetu> GetYosanTuki(ProperOrderKey key, string strHinban, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT YosanTsuki FROM T_ProperOrder WHERE (SashizuBi = @SashizuBi) AND (SeisanOrderNo = @SeisanOrderNo) AND (HikitoriOrderNo = @HikitoriOrderNo) {0} 
Group by YosanTsuki ORDER BY YosanTsuki";


            da.SelectCommand.Parameters.AddWithValue("@SashizuBi", key.SashizuBi);
            da.SelectCommand.Parameters.AddWithValue("@SeisanOrderNo", key.SeisanOrderNo);
            da.SelectCommand.Parameters.AddWithValue("@HikitoriOrderNo", key.HikitoriOrderNo);


            if (!string.IsNullOrEmpty(strHinban))
            {
                da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, " AND Hinban = @Hinban");
                da.SelectCommand.Parameters.AddWithValue("@Hinban", strHinban);
            }
            else
                da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, "");

            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Core.Type.Nengetu> lst = new List<Core.Type.Nengetu>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst.Add(new Core.Type.Nengetu(Convert.ToInt32(dt.Rows[i][0])));
            }
            return lst;
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ngYosanTuki"></param>
        /// <param name="strNouhinKigou">空白もある</param>
        /// <param name="strNiukeBumonCode">空白もある</param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static ViewDataset.VIEW_ProperOrderDataTable
            getVIEW_ProperOrderDataTable(ProperOrderKey key, string strHinban,
            Core.Type.Nengetu ngYosanTuki, string strNouhinKigou, string strNiukeJigyoushoCode, Core.Sql.RowNumberInfo r, SqlConnection sqlConn, ref int nCount)
        {

            nCount = 0;
            //完了フラグが正しく取得できない為、変更する 20160920
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
                select
                    SashizuBi,ShiiresakiCode,SeisanOrderNo,HikitoriOrderNo,RowNo
                    ,YosanTsuki,HikitoriNouki,Hinban,Size,Spec,Kakaku,Suuryou
                    ,NouhinKigou,NiukeJigyoushoCode,NiukeJigyoushoMei,HokanBasho
                    ,Sekiduke,DenpyouKubun,SaiHacchuFlg,TsuikaInfo1,TsuikaInfo2
                    ,Tantousha,Shouninsha,ShukkaSuu,
                    ISNULL((SELECT MAX(M.KanryouFlg) FROM T_NouhinHeader H INNER JOIN
                                T_NouhinMeisai M ON M.YYMM = H.YYMM AND M.HakkouNo = H.HakkouNo AND
                                    M.ShiiresakiCode = H.NouhinMotoshiiresakiCode
                             WHERE H.SashizuBi = VIEW_ProperOrder.SashizuBi AND H.NouhinmotoShiiresakiCode = VIEW_ProperOrder.ShiiresakiCode AND
                                (H.SeisanOrderNo = VIEW_ProperOrder.SeisanOrderNo OR H.HikitoriOrderNo = VIEW_ProperOrder.HikitoriOrderNo) AND
                                M.Hinban = VIEW_ProperOrder.Hinban AND M.Size = VIEW_ProperOrder.Size AND 
                                VIEW_ProperOrder.KanryouFlg = 1),0) AS KanryouFlg
                    ,SaishuShukkaBi,TukaCode
                from VIEW_ProperOrder WHERE (SashizuBi = @z) AND (SeisanOrderNo = @s) and (HikitoriOrderNo = @h)";

            da.SelectCommand.Parameters.AddWithValue("@z", key.SashizuBi);
            da.SelectCommand.Parameters.AddWithValue("@s", key.SeisanOrderNo);
            da.SelectCommand.Parameters.AddWithValue("@h", key.HikitoriOrderNo);

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();

            if (!string.IsNullOrEmpty(strHinban)) {
                w.Add("Hinban = @Hinban");
                da.SelectCommand.Parameters.AddWithValue("@Hinban", strHinban);
            }

            if (null != ngYosanTuki) {
                w.Add("YosanTsuki = @y");
                da.SelectCommand.Parameters.AddWithValue("@y", ngYosanTuki.ToYYYYMM());
            }

            if (null != strNouhinKigou) {
                w.Add("NouhinKigou = @NouhinKigou");
                da.SelectCommand.Parameters.AddWithValue("@NouhinKigou", strNouhinKigou);
            }

            if (null != strNiukeJigyoushoCode)
            {
                w.Add("NiukeJigyoushoCode = @NiukeJigyoushoCode");
                da.SelectCommand.Parameters.AddWithValue("@NiukeJigyoushoCode", strNiukeJigyoushoCode);
            }

            if (!string.IsNullOrEmpty(w.WhereText))
                da.SelectCommand.CommandText += " and " + w.WhereText;

            ViewDataset.VIEW_ProperOrderDataTable dt = new ViewDataset.VIEW_ProperOrderDataTable();
            if (null != r)
                r.LoadData(da.SelectCommand, sqlConn, dt, ref nCount);
            else
            {
                da.Fill(dt);
                nCount = dt.Count;
            }

            return dt;
        }






        /// <summary>
        /// 出荷入力する際の入力対象項目
        /// 事業所コードの条件が加わる場合もある。
        /// ・完納済みも取得している。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="strNiukeBumonCode"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static QueryDataset.Q_ProperOrderShukkaNyuryokuKeyDataTable
            getQ_ProperOrderShukkaNyuryokuKeyDataTable(ProperOrderKey key,
            Core.Type.Nengetu ngYosanTuki, string strHinban, string strNouhinKigou, string strNiukeJigyoushoCode, bool bMikanryouOnly, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT                  TOP (100) PERCENT Base.Hinban, Base.Size, Base.Kakaku, Base.Suuryou, Base.ShukkaSuu, 品番サイズ別の最小行No.HinbanSizeBetuSuuryou, 
品番サイズ別の最小行No.HinbanSizeBetuShukkaSuu
FROM                     (SELECT                  Hinban, Size, Kakaku, SUM(Suuryou) AS Suuryou, SUM(ShukkaSuu) AS ShukkaSuu
FROM                     dbo.T_ProperOrder
WHERE (SashizuBi = @SashizuBi) AND (SeisanOrderNo = @SeisanOrderNo) AND (HikitoriOrderNo = @HikitoriOrderNo) {0}  
GROUP BY          Hinban, Size, Kakaku) AS Base LEFT OUTER JOIN
(SELECT                  Hinban, Size, MIN(RowNo) AS MinRowNo, SUM(Suuryou) AS HinbanSizeBetuSuuryou, SUM(ShukkaSuu) 
AS HinbanSizeBetuShukkaSuu
FROM                     dbo.T_ProperOrder AS T_ProperOrder_1
WHERE                   (SashizuBi = @SashizuBi) AND (SeisanOrderNo = @SeisanOrderNo) AND (HikitoriOrderNo = @HikitoriOrderNo)
GROUP BY          Hinban, Size) AS 品番サイズ別の最小行No ON Base.Hinban = 品番サイズ別の最小行No.Hinban AND 
Base.Size = 品番サイズ別の最小行No.Size

LEFT OUTER JOIN
(SELECT                  Hinban, Size, MIN(RowNo) AS MinRowNo, SUM(Suuryou) AS HinbanSizeBetuSuuryou, SUM(ShukkaSuu) 
AS HinbanSizeBetuShukkaSuu
FROM                     dbo.T_ProperOrder AS T_ProperOrder_1
WHERE                   (SashizuBi = @SashizuBi) AND (SeisanOrderNo = @SeisanOrderNo) AND (HikitoriOrderNo = @HikitoriOrderNo) AND (NiukeJigyoushoCode = @NiukeJigyoushoCode)
GROUP BY          Hinban, Size) AS 事業所別の最小行No ON Base.Hinban = 事業所別の最小行No.Hinban AND 
Base.Size = 事業所別の最小行No.Size";

            if (!string.IsNullOrEmpty(strNiukeJigyoushoCode))
            {
                da.SelectCommand.CommandText += " ORDER BY 事業所別の最小行No.MinRowNo ";
            }
            else
            {
                da.SelectCommand.CommandText += " ORDER BY 品番サイズ別の最小行No.MinRowNo ";
            }



            da.SelectCommand.Parameters.AddWithValue("@SashizuBi", key.SashizuBi);
            da.SelectCommand.Parameters.AddWithValue("@SeisanOrderNo", key.SeisanOrderNo);
            da.SelectCommand.Parameters.AddWithValue("@HikitoriOrderNo", key.HikitoriOrderNo);

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();


            if (!string.IsNullOrEmpty(strHinban))
            {
                w.Add("Hinban = @Hinban");
                da.SelectCommand.Parameters.AddWithValue("@Hinban", strHinban);
            }


            if (null != ngYosanTuki)
            {
                w.Add("YosanTsuki = @y");
                da.SelectCommand.Parameters.AddWithValue("@y", ngYosanTuki.ToYYYYMM());
            }

            // 納品記号(空白もありえる)
            if (null != strNouhinKigou)
            {
                w.Add("NouhinKigou = @NouhinKigou");
                da.SelectCommand.Parameters.AddWithValue("@NouhinKigou", strNouhinKigou);
            }

            if (!string.IsNullOrEmpty(strNiukeJigyoushoCode))
            {
                w.Add("NiukeJigyoushoCode = @NiukeJigyoushoCode");
                da.SelectCommand.Parameters.AddWithValue("@NiukeJigyoushoCode", strNiukeJigyoushoCode);
            }
            else
            {
                da.SelectCommand.Parameters.AddWithValue("@NiukeJigyoushoCode", "");
            }

            if (bMikanryouOnly)
                w.Add("KanryouFlg=0 or KanryouFlg is null");


            da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, (string.IsNullOrEmpty(w.WhereText)) ? "" : " and " + w.WhereText);


            QueryDataset.Q_ProperOrderShukkaNyuryokuKeyDataTable dt = new QueryDataset.Q_ProperOrderShukkaNyuryokuKeyDataTable();
            da.Fill(dt);
            return dt;
        }



        public static int GetShukkaZansu(ProperOrderKey key, string strHinban, string strSize, SqlConnection c)
        {
            SqlCommand cmd = new SqlCommand("", c);
            cmd.CommandText = @"
SELECT                  SUM(Suuryou - ShukkaSuu) AS Expr1
FROM                     dbo.T_ProperOrder
WHERE                   (SashizuBi = @z) AND (SeisanOrderNo = @s) AND (HikitoriOrderNo = @h) AND (Hinban = @Hinban) AND (Size = @Size)";

            cmd.Parameters.AddWithValue("@z", key.SashizuBi);
            cmd.Parameters.AddWithValue("@s", key.SeisanOrderNo);
            cmd.Parameters.AddWithValue("@h", key.HikitoriOrderNo);
            cmd.Parameters.AddWithValue("@Hinban", strHinban);
            cmd.Parameters.AddWithValue("@Size", strSize);

            try
            {
                c.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception e)
            {

                return 0;
            }
            finally {
                c.Close();
            }
        }

        //完了フラグが正しく取得できない為、テーブルを変更する T_ProperOrderDataTable → DataTable
        public static DataTable getT_ProperOrderDataTable(KensakuParam p, SqlConnection sqlConn)
        {
            return p.getT_ProperOrderDataTable(sqlConn);
        }


        public static List<string>
            GetNouhinKigou(EnumChumonStatus s, string [] lstShiiresakiCode, SqlConnection sqlConn)
        {

            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT                  NouhinKigou
FROM                     dbo.T_ProperOrder {0} 
GROUP BY          NouhinKigou
ORDER BY NouhinKigou";


            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();


            // 状況
            switch (s)
            {
                case EnumChumonStatus.MiUketuke:
                    w.Add(@"not exists (select * from T_ProperUketsuke where 
T_ProperUketsuke.SashizuBi=T_ProperOrder.SashizuBi and 
T_ProperUketsuke.SeisanOrderNo=T_ProperOrder.SeisanOrderNo and 
T_ProperUketsuke.HikitoriOrderNo=T_ProperOrder.HikitoriOrderNo)");
                    break;
                case EnumChumonStatus.Mishukka:
                    // 未出荷
                    w.Add("T_ProperOrder.KanryouFlg =0 AND T_ProperOrder.Suuryou > T_ProperOrder.ShukkaSuu ");
                    break;
                case EnumChumonStatus.ShukkaZumi:
                    w.Add("(T_ProperOrder.KanryouFlg=1 or T_ProperOrder.Suuryou=T_ProperOrder.ShukkaSuu)");
                    break;
            }

            if (null != lstShiiresakiCode && 0 < lstShiiresakiCode.Length)
            {
                w.Add(string.Format("ShiiresakiCode in ('{0}')", string.Join("','", lstShiiresakiCode))); ;
            }

            string strWhere = (string.IsNullOrEmpty(w.WhereText)) ? "" : " where " + w.WhereText;

            da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, strWhere);

            DataTable dt = new DataTable();
            da.Fill(dt);

            List<string> lst = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst.Add((string)dt.Rows[i][0]);
            }

            return lst;
        }

        public static List<string>
    GetNiukeJigyoushoCode(EnumChumonStatus s, string strNouhinKigou, string[] lstShiiresakiCode,
    Core.Sql.FilterItem YosanTukiFromTo, SqlConnection sqlConn)
        {

            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT                  NiukeJigyoushoCode
FROM                     dbo.T_ProperOrder {0} 
GROUP BY          NiukeJigyoushoCode
ORDER BY          NiukeJigyoushoCode
";


            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();


            // 状況
            switch (s)
            {
                case EnumChumonStatus.MiUketuke:
                    w.Add(@"not exists (select * from T_ProperUketsuke where 
T_ProperUketsuke.SashizuBi=T_ProperOrder.SashizuBi and 
T_ProperUketsuke.SeisanOrderNo=T_ProperOrder.SeisanOrderNo and 
T_ProperUketsuke.HikitoriOrderNo=T_ProperOrder.HikitoriOrderNo)");
                    break;
                case EnumChumonStatus.Mishukka:
                    // 未出荷
                    w.Add("T_ProperOrder.KanryouFlg =0 AND T_ProperOrder.Suuryou > T_ProperOrder.ShukkaSuu ");
                    break;
                case EnumChumonStatus.ShukkaZumi:
                    w.Add("(T_ProperOrder.KanryouFlg=1 or T_ProperOrder.Suuryou=T_ProperOrder.ShukkaSuu)");
                    break;
            }

            if (null != lstShiiresakiCode && 0 < lstShiiresakiCode.Length)
            {
                w.Add(string.Format("ShiiresakiCode in ('{0}')", string.Join("','", lstShiiresakiCode))); ;
            }

            //if (null != strNouhinKigou)
            //{
            //    w.Add("NouhinKigou=@NouhinKigou");
            //    da.SelectCommand.Parameters.AddWithValue("@NouhinKigou", strNouhinKigou);
            //}


            if (null != YosanTukiFromTo)
            {
                w.Add(YosanTukiFromTo.GetFilterText("YosanTsuki", "@YosanTsuki", da.SelectCommand));
            }

            KensakuParam k = new KensakuParam();
            // ミズノ受付日表示対象期間 2012.04.05追加
            if (0 < k.nHyoujiTukisu)
            {
                // 注残であっても表示対象期間を設定する。
                w.Add(string.Format("CAST(SashizuBi AS DATETIME) >= CAST('{0}' AS DATETIME)", DateTime.Today.AddMonths((-1) * k.nHyoujiTukisu)));
            }



            string strWhere = (string.IsNullOrEmpty(w.WhereText)) ? "" : " where " + w.WhereText;

            da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, strWhere);

            DataTable dt = new DataTable();
            da.Fill(dt);

            List<string> lst = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst.Add((string)dt.Rows[i][0]);
            }

            return lst;
        }

        /*
        public static List<string>
            GetNiukeBumonCode(EnumChumonStatus s, string strNouhinKigou, string[] lstShiiresakiCode,
            Core.Sql.FilterItem YosanTukiFromTo, SqlConnection sqlConn)
        {

            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT                  NiukeBumonCode
FROM                     dbo.T_ProperOrder {0} 
GROUP BY          NiukeBumonCode
ORDER BY          NiukeBumonCode
";


            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();


            // 状況
            switch (s)
            {
                case EnumChumonStatus.MiUketuke:
                    w.Add(@"not exists (select * from T_ProperUketsuke where 
T_ProperUketsuke.SashizuBi=T_ProperOrder.SashizuBi and 
T_ProperUketsuke.SeisanOrderNo=T_ProperOrder.SeisanOrderNo and 
T_ProperUketsuke.HikitoriOrderNo=T_ProperOrder.HikitoriOrderNo)");
                    break;
                case EnumChumonStatus.Mishukka:
                    // 未出荷
                    w.Add("T_ProperOrder.KanryouFlg =0 AND T_ProperOrder.Suuryou > T_ProperOrder.ShukkaSuu ");
                    break;
                case EnumChumonStatus.ShukkaZumi:
                    w.Add("(T_ProperOrder.KanryouFlg=1 or T_ProperOrder.Suuryou=T_ProperOrder.ShukkaSuu)");
                    break;
            }

            if (null != lstShiiresakiCode && 0 < lstShiiresakiCode.Length)
            {
                w.Add(string.Format("ShiiresakiCode in ('{0}')", string.Join("','", lstShiiresakiCode))); ;
            }

            if (null != strNouhinKigou)
            {
                w.Add("NouhinKigou=@NouhinKigou");
                da.SelectCommand.Parameters.AddWithValue("@NouhinKigou", strNouhinKigou);
            }


            if (null != YosanTukiFromTo)
            {
                w.Add(YosanTukiFromTo.GetFilterText("YosanTsuki", "@YosanTsuki", da.SelectCommand));
            }


            string strWhere = (string.IsNullOrEmpty(w.WhereText)) ? "" : " where " + w.WhereText;

            da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, strWhere);

            DataTable dt = new DataTable();
            da.Fill(dt);

            List<string> lst = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst.Add((string)dt.Rows[i][0]);
            }

            return lst;
        }
        */




        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="strNouhinKigou">納品記号は空白もある</param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static QueryDataset.Q_ShiiresakiCodeMeiDataTable
            getQ_ShiiresakiCodeMeiDataTable(EnumChumonStatus s, string strNouhinKigou, 
            Core.Sql.FilterItem YosanTukiFromTo, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT          DISTINCT        T_A.ShiiresakiCode, dbo.VIEW_Shiiresaki.RyakuMei AS ShiiresakiMei
FROM                     (SELECT                  ShiiresakiCode
FROM                     dbo.T_ProperOrder {0} 
GROUP BY          ShiiresakiCode) AS T_A LEFT OUTER JOIN
dbo.VIEW_Shiiresaki ON T_A.ShiiresakiCode = dbo.VIEW_Shiiresaki.ShiiresakiCode";

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();

            // 状況
            switch (s)
            {
                case EnumChumonStatus.MiUketuke:
                    w.Add(@"not exists (select * from T_ProperUketsuke where 
T_ProperUketsuke.SashizuBi=T_ProperOrder.SashizuBi and 
T_ProperUketsuke.SeisanOrderNo=T_ProperOrder.SeisanOrderNo and 
T_ProperUketsuke.HikitoriOrderNo=T_ProperOrder.HikitoriOrderNo)");
                    break;
                case EnumChumonStatus.Mishukka:
                    // 未出荷
                    w.Add("T_ProperOrder.KanryouFlg =0 AND T_ProperOrder.Suuryou > T_ProperOrder.ShukkaSuu ");
                    break;
                case EnumChumonStatus.ShukkaZumi:
                    w.Add("(T_ProperOrder.KanryouFlg=1 or T_ProperOrder.Suuryou=T_ProperOrder.ShukkaSuu)");
                    break;
            }

            //納品記号のチェックは外す_2012-12-05齊藤
            //if (null != strNouhinKigou)
            //{
            //    w.Add("NouhinKigou=@NouhinKigou");
            //    da.SelectCommand.Parameters.AddWithValue("@NouhinKigou", strNouhinKigou);
            //}

            if (null != YosanTukiFromTo) {
                w.Add(YosanTukiFromTo.GetFilterText("YosanTsuki", "@YosanTsuki", da.SelectCommand));
            }


            string strWhere = (string.IsNullOrEmpty(w.WhereText)) ? "" : " where " + w.WhereText;

            da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, strWhere);


            QueryDataset.Q_ShiiresakiCodeMeiDataTable dt = new QueryDataset.Q_ShiiresakiCodeMeiDataTable();
            da.Fill(dt);
            return dt;
        }



        /// <summary>
        /// 注残の予算月一覧取得
        /// </summary>
        /// <param name="strShiiresakiCode"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static List<Core.Type.Nengetu> GetYosanTuki(EnumChumonStatus s, string strNouhinKigou, string[] lstShiiresakiCode, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT                  TOP (100) PERCENT YosanTsuki
FROM                     dbo.T_ProperOrder {0} 
GROUP BY          YosanTsuki
ORDER BY           YosanTsuki DESC";

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();

            // 状況
            switch (s)
            {
                case EnumChumonStatus.MiUketuke:
                    w.Add(@"not exists (select * from T_ProperUketsuke where 
T_ProperUketsuke.SashizuBi=T_ProperOrder.SashizuBi and 
T_ProperUketsuke.SeisanOrderNo=T_ProperOrder.SeisanOrderNo and 
T_ProperUketsuke.HikitoriOrderNo=T_ProperOrder.HikitoriOrderNo)");
                    break;
                case EnumChumonStatus.Mishukka:
                    // 未出荷
                    w.Add("T_ProperOrder.KanryouFlg =0 AND T_ProperOrder.Suuryou > T_ProperOrder.ShukkaSuu ");
                    break;
                case EnumChumonStatus.ShukkaZumi:
                    w.Add("(T_ProperOrder.KanryouFlg=1 or T_ProperOrder.Suuryou=T_ProperOrder.ShukkaSuu)");
                    break;
            }


            if (null != lstShiiresakiCode && 0 < lstShiiresakiCode.Length)
            {
                w.Add(string.Format("ShiiresakiCode in ('{0}')", string.Join("','", lstShiiresakiCode)));
            }

            KensakuParam k = new KensakuParam();
            // ミズノ受付日表示対象期間 2012.04.05追加
            if (0 < k.nHyoujiTukisu)
            {
                // 注残であっても表示対象期間を設定する。
                w.Add(string.Format("CAST(SashizuBi AS DATETIME) >= CAST('{0}' AS DATETIME)", DateTime.Today.AddMonths((-1) * k.nHyoujiTukisu)));
            }


            //// 納品記号(空白もありえる)
            //if (null != strNouhinKigou)
            //{
            //    w.Add("NouhinKigou = @NouhinKigou");
            //    da.SelectCommand.Parameters.AddWithValue("@NouhinKigou", strNouhinKigou);
            //}


            string strWhere = (string.IsNullOrEmpty(w.WhereText)) ? "" : " where " + w.WhereText;

            da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, strWhere);


            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Core.Type.Nengetu> lst = new List<Core.Type.Nengetu>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst.Add(new Core.Type.Nengetu(Convert.ToInt32(dt.Rows[i][0])));
            }


            return lst;
        }




        public static Core.Error 
            GetMaxMinYosanTuki(List<string> lstShiiresakiCode, SqlConnection c, out Core.Type.Nengetu ngMax, out Core.Type.Nengetu ngMin)
        {
            ngMax = ngMin = null;
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = @"
SELECT                  MAX(YosanTsuki) AS YosanTukiMAX, MIN(YosanTsuki) AS YosanTukiMIN
FROM                     dbo.T_ProperOrder";

            if (null != lstShiiresakiCode && 0 < lstShiiresakiCode.Count)
            {
                da.SelectCommand.CommandText += string.Format(" where ShiiresakiCode in ('{0}')", string.Join("','", lstShiiresakiCode.ToArray()));
            }

            try
            {
                c.Open();

                DataTable dt = new DataTable();
                da.Fill(dt);

                ngMax =new Core.Type.Nengetu(Convert.ToInt32(dt.Rows[0][0]));
                ngMin = new Core.Type.Nengetu(Convert.ToInt32(dt.Rows[0][1]));
                return null;
            }
            catch (Exception e)
            {
                return new Core.Error(e);
            }
            finally {
                c.Close();
            }


        }






        public static ViewDataset.VIEW_ProperOrderKeyRow
            getVIEW_ProperOrderKeyRow(ProperOrderKey key, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = "select * from VIEW_ProperOrderKey WHERE (SashizuBi = @z) AND (SeisanOrderNo = @s) and (HikitoriOrderNo = @h) ";

            da.SelectCommand.Parameters.AddWithValue("@z", key.SashizuBi);
            da.SelectCommand.Parameters.AddWithValue("@s", key.SeisanOrderNo);
            da.SelectCommand.Parameters.AddWithValue("@h", key.HikitoriOrderNo);
            ViewDataset.VIEW_ProperOrderKeyDataTable dt = new ViewDataset.VIEW_ProperOrderKeyDataTable();
            da.Fill(dt);
            if (1 == dt.Count) return dt[0];
            return null;
        }


        public class MeisaiData : NouhinDataClass.MeisaiDataCommon
        {
            public class MeisaiKey
            {
                public string NiukeJigyoushoCode { get; set; }  // 荷受事業所の指定が無い時はNULL or 空白
                public string Hinban { get; set; }
                public string Size { get; set; }
                public bool NiukeJigyoushoCodeHikiateAll { get; set; } // True:指定の事業所の納入残に全数引き当てる / False:引き当て可能な分だけ引き当てる。(アップロード登録時引き当て元先の事業所の指定が無い為）

                public MeisaiKey()
                {
                    NiukeJigyoushoCodeHikiateAll = true;
                }
            }

            public MeisaiKey Key = new MeisaiKey();


        }

        public class TourokuData : NouhinDataClass.TourokuDataBase
        {
            public string ShukkaNiukeJigyoushoCode = null; // 出荷対象となる事業所コード(画面からの登録の場合のみ)
            public Core.Type.Nengetu ngHonNouhinTuki = null;    // 送品区分＝仮納品の時だけ
            public string invoiceNo = null;
            public string UnsouGyoushaCode = null;
            public string UnsouGyoushaMei = null;
            public string KogutiSu = null;

            public List<MeisaiData> lstMeisai = new List<MeisaiData>();
        }


        // ★納品処理
        public static Core.Error DenpyouTouroku(bool bUpload, ProperOrderKey pk, Core.Type.Nengetu ngYosanTuki, string strNouhinKigou,
            TourokuData data, SqlConnection c, out List<NouhinDataClass.DenpyouKey> lstKey, out List<NouhinDataClass.DenpyouKey> lstKeyShukkaAri)
        {
            lstKey = null;
            lstKeyShukkaAri = null;
            SqlTransaction t = null;         
            try {
                c.Open();
                t = c.BeginTransaction();
                DenpyouTouroku(bUpload, false, pk, ngYosanTuki, strNouhinKigou, data, t, out lstKey, out lstKeyShukkaAri);
                t.Commit();
                return null;
            }
            catch (Exception e)
            {
                if (null != t) t.Rollback();
                return new Core.Error(e);
            }
            finally {
                c.Close();
            }
        }

        // ★納品処理
        internal static void DenpyouTouroku(bool bUpload, bool bShusei, ProperOrderKey pk, Core.Type.Nengetu ngYosanTuki, string strNouhinKigou,
            TourokuData data, SqlTransaction t, out List<NouhinDataClass.DenpyouKey> lstKey, out List<NouhinDataClass.DenpyouKey> lstKeyShukkaAri)
        {
            lstKeyShukkaAri = new List<NouhinDataClass.DenpyouKey>();
            lstKey = new List<NouhinDataClass.DenpyouKey>();

            int nPageSize = NouhinDataClass.GetDenpyouGyouCount(data.SouhinKubun); // 送品区分によって登録できる行数が異なる。

            // 出荷数 = 0 && 完了フラグ = 9(完了)のデータは別の独立した伝票で登録する。画面上は伝票発行無しでデータのみ登録する(送信もする))
            List<MeisaiData> lstShukka = new List<MeisaiData>();            
            List<MeisaiData> lstZeroKanryou = new List<MeisaiData>();
            List<MeisaiData> lstShukka_KeigenZeiritsu = new List<MeisaiData>(); // 軽減税率は別伝票で発行 追加 20190821 M0458
            List<MeisaiData> lstZeroKanryou_KeigenZeiritsu = new List<MeisaiData>(); // 軽減税率は別伝票で発行 追加 20190821 M0458
            //軽減税率用　10月1日　以降に処理をするように
            int nSashizuBi = int.Parse(DateTime.Now.ToShortDateString().Replace("/", ""));
            int nZeiritsuHenkouBi = 20191001;
            for (int i = 0; i < data.lstMeisai.Count; i++) {
                if (0 == data.lstMeisai[i].ShukkaSu && data.lstMeisai[i].KanryouFlag)
                {
                    // 軽減税率の判定を追加　20190821 M0458
                    int.TryParse(pk.SashizuBi.Replace("/", ""), out nSashizuBi);
                    if (data.lstMeisai[i].KeigenZeiritsu == "R8K" && nSashizuBi >= nZeiritsuHenkouBi) // 2019年10月から処理する
                        lstZeroKanryou_KeigenZeiritsu.Add(data.lstMeisai[i]);
                    else
                        lstZeroKanryou.Add(data.lstMeisai[i]);
                }
                else
                {
                    // 軽減税率の判定を追加　20190821 M0458
                    int.TryParse(pk.SashizuBi.Replace("/", ""), out nSashizuBi);
                    if (data.lstMeisai[i].KeigenZeiritsu == "R8K" && nSashizuBi >= nZeiritsuHenkouBi) // 2019年10月から処理する
                        lstShukka_KeigenZeiritsu.Add(data.lstMeisai[i]);
                    else
                        lstShukka.Add(data.lstMeisai[i]);
                }
            }

            List<List<MeisaiData>> lstDenpyou = new List<List<MeisaiData>>();   // 出荷ありデータ、数量0&完了データの順の出力する

            // 出荷データから
            if (0 < lstShukka.Count)
            {
                List<MeisaiData> m = new List<MeisaiData>();
                for (int i = 0; i < lstShukka.Count; i++)
                {
                    m.Add(lstShukka[i]);
                    if (m.Count == nPageSize)
                    {
                        lstDenpyou.Add(m);
                        m = new List<MeisaiData>();
                    }
                }
                if (0 < m.Count) lstDenpyou.Add(m);
            }

            // 出荷データから　軽減税率　追加　20190821　M0458
            if (0 < lstShukka_KeigenZeiritsu.Count)
            {
                List<MeisaiData> m = new List<MeisaiData>();
                for (int i = 0; i < lstShukka_KeigenZeiritsu.Count; i++)
                {
                    m.Add(lstShukka_KeigenZeiritsu[i]);
                    if (m.Count == nPageSize)
                    {
                        lstDenpyou.Add(m);
                        m = new List<MeisaiData>();
                    }
                }
                if (0 < m.Count) lstDenpyou.Add(m);
            }

            int nShukkaAriDenpyouCount = lstDenpyou.Count;

            // 出荷数=0&完了フラグ＝9のデータ
            if (0 < lstZeroKanryou.Count)
            {
                List<MeisaiData> m = new List<MeisaiData>();
                for (int i = 0; i < lstZeroKanryou.Count; i++)
                {
                    m.Add(lstZeroKanryou[i]);
                    if (m.Count == nPageSize)
                    {
                        lstDenpyou.Add(m);
                        m = new List<MeisaiData>();
                    }
                }
                if (0 < m.Count) lstDenpyou.Add(m);
            }

            // 出荷数=0&完了フラグ＝9のデータ　軽減税率　追加　20190821　M0458
            if (0 < lstZeroKanryou_KeigenZeiritsu.Count)
            {
                List<MeisaiData> m = new List<MeisaiData>();
                for (int i = 0; i < lstZeroKanryou_KeigenZeiritsu.Count; i++)
                {
                    m.Add(lstZeroKanryou_KeigenZeiritsu[i]);
                    if (m.Count == nPageSize)
                    {
                        lstDenpyou.Add(m);
                        m = new List<MeisaiData>();
                    }
                }
                if (0 < m.Count) lstDenpyou.Add(m);
            }

            // ページ単位で作成
            List<MeisaiData> lstOrg = data.lstMeisai;
            for (int i = 0; i < lstDenpyou.Count; i++)
            {
                data.lstMeisai = lstDenpyou[i]; // 一時的に設定する。
                NouhinDataClass.DenpyouKey key = DenpyouTouroku(bShusei, 0, pk, ngYosanTuki, strNouhinKigou, data, t);
                lstKey.Add(key);

                if (i < nShukkaAriDenpyouCount)
                    lstKeyShukkaAri.Add(key);
            }
            data.lstMeisai = lstOrg;    // 本関数処理後に使用するかもしれないので元に戻す

        }

        private static NouhinDataClass.DenpyouKey 
            DenpyouTouroku(bool bShusei, int nHakkouNo, 
            ProperOrderKey key, Core.Type.Nengetu ngYosanTuki, string strNouhinKigou, TourokuData data, SqlTransaction t)
        {
            SqlCommand cmdGetShiiresakiCode = new SqlCommand("", t.Connection);
            cmdGetShiiresakiCode.Transaction = t;
            cmdGetShiiresakiCode.CommandText = "select ShiiresakiCode from T_ProperOrder where SashizuBi=@z and SeisanOrderNo=@s and HikitoriOrderNo=@h";
            cmdGetShiiresakiCode.Parameters.AddWithValue("@z", key.SashizuBi);
            cmdGetShiiresakiCode.Parameters.AddWithValue("@s", key.SeisanOrderNo);
            cmdGetShiiresakiCode.Parameters.AddWithValue("@h", key.HikitoriOrderNo);
            string strShiiresakiCode = Convert.ToString(cmdGetShiiresakiCode.ExecuteScalar());


            // 最大の発行No取得
            SqlCommand cmdGetMaxHakkouNo = new SqlCommand("", t.Connection);
            cmdGetMaxHakkouNo.Transaction = t;
            cmdGetMaxHakkouNo.CommandText = @"SELECT MAX(HakkouNo) AS HacchuNo FROM T_NouhinHeader WHERE (NouhinmotoShiiresakiCode = @s) AND (YYMM = @YYMM) ";
            cmdGetMaxHakkouNo.Parameters.AddWithValue("@s", strShiiresakiCode);
            cmdGetMaxHakkouNo.Parameters.AddWithValue("@YYMM", int.Parse(data.dtHakkouBi.ToString("yyMM")));


            // 予算品テーブルから注残分取得(1注文＝1納品記号なので納品記号の条件は不要と思う)
            SqlDataAdapter daProper = new SqlDataAdapter("select * from T_ProperOrder where SashizuBi=@z and SeisanOrderNo=@s and HikitoriOrderNo=@h and YosanTsuki=@y and Hinban=@Hinban and Size=@Size and KanryouFlg=0 and Suuryou>ShukkaSuu", t.Connection);
            daProper.SelectCommand.Parameters.AddWithValue("@z", key.SashizuBi);
            daProper.SelectCommand.Parameters.AddWithValue("@s", key.SeisanOrderNo);
            daProper.SelectCommand.Parameters.AddWithValue("@h", key.HikitoriOrderNo);
            daProper.SelectCommand.Parameters.AddWithValue("@y", ngYosanTuki.ToYYYYMM());
            daProper.SelectCommand.Parameters.AddWithValue("@Hinban", "");
            daProper.SelectCommand.Parameters.AddWithValue("@Size", "");
            daProper.SelectCommand.Transaction = t;
            daProper.UpdateCommand = new SqlCommandBuilder(daProper).GetUpdateCommand();
            daProper.UpdateCommand.Transaction = t;

            // 本注文の最初の品番、サイズ取得（１行目が追加品番の場合のダミーデータ）
            SqlDataAdapter daGetFirstHinbanSize = new SqlDataAdapter("", t.Connection);
            daGetFirstHinbanSize.SelectCommand.CommandText = @"SELECT Hinban, Size FROM T_ProperOrder WHERE (RowNo = 1) AND (SashizuBi = @z) AND (SeisanOrderNo = @s) AND (HikitoriOrderNo = @h)";
            daGetFirstHinbanSize.SelectCommand.Parameters.AddWithValue("@z", key.SashizuBi);
            daGetFirstHinbanSize.SelectCommand.Parameters.AddWithValue("@s", key.SeisanOrderNo);
            daGetFirstHinbanSize.SelectCommand.Parameters.AddWithValue("@h", key.HikitoriOrderNo);
            daGetFirstHinbanSize.SelectCommand.Transaction = t;


            // 明細
            SqlDataAdapter daM = new SqlDataAdapter("select * from T_NouhinMeisai", t.Connection);
            daM.SelectCommand.Transaction = t;
            daM.InsertCommand = new SqlCommandBuilder(daM).GetInsertCommand();
            daM.InsertCommand.Transaction = t;

            // トレーラー
            SqlDataAdapter daTrailer = new SqlDataAdapter("", t.Connection);
            daTrailer.SelectCommand.CommandText = "select * from T_NouhinTrailer";
            daTrailer.SelectCommand.Transaction = t;
            daTrailer.InsertCommand = new SqlCommandBuilder(daTrailer).GetInsertCommand();
            daTrailer.InsertCommand.Transaction = t;


            // 予算品引き当て情報
            SqlDataAdapter daHikiate = new SqlDataAdapter("select * from T_ProperOrdrer_NouhinHikiate", t.Connection);
            daHikiate.SelectCommand.Transaction = t;
            daHikiate.InsertCommand = new SqlCommandBuilder(daHikiate).GetInsertCommand();
            daHikiate.InsertCommand.Transaction = t;



            // 完了フラグのセット(予算月 + 品番 + サイズ単位):完納に対してはセットしない
            SqlCommand cmdSetKanryouFlg = new SqlCommand("", t.Connection);
            cmdSetKanryouFlg.CommandText = "update T_ProperOrder set KanryouFlg=1 where SashizuBi=@z and SeisanOrderNo=@s and HikitoriOrderNo=@h and YosanTsuki=@y and Hinban=@Hinban and Size=@Size AND Suuryou>ShukkaSuu";
            cmdSetKanryouFlg.Parameters.AddWithValue("@z", key.SashizuBi);
            cmdSetKanryouFlg.Parameters.AddWithValue("@s", key.SeisanOrderNo);
            cmdSetKanryouFlg.Parameters.AddWithValue("@h", key.HikitoriOrderNo);
            cmdSetKanryouFlg.Parameters.AddWithValue("@y", ngYosanTuki.ToYYYYMM());
            cmdSetKanryouFlg.Parameters.AddWithValue("@Hinban", "");
            cmdSetKanryouFlg.Parameters.AddWithValue("@Size", "");
            if (data.NiukeFilter != "")
            {
                cmdSetKanryouFlg.CommandText += " and NiukeJigyoushoCode=@n";
                cmdSetKanryouFlg.Parameters.AddWithValue("@n","");
            }
            cmdSetKanryouFlg.Transaction = t;

            // 予算月 + 品番 + サイズ単位の出荷残数取得
            // →全数出荷で最終送信の納品データに完了フラグをセットする。
            SqlCommand cmdGetZansu = new SqlCommand("", t.Connection);
            cmdGetZansu.CommandText = @"
SELECT                  SUM(Suuryou - ShukkaSuu) AS Zansu
FROM                     dbo.T_ProperOrder
WHERE                   (SashizuBi = @z) AND (SeisanOrderNo = @s) AND 
(HikitoriOrderNo = @h) AND (YosanTsuki = @y) AND (Hinban = @Hinban) AND (Size = @Size)";
            cmdGetZansu.Parameters.AddWithValue("@z", key.SashizuBi);
            cmdGetZansu.Parameters.AddWithValue("@s", key.SeisanOrderNo);
            cmdGetZansu.Parameters.AddWithValue("@h", key.HikitoriOrderNo);
            cmdGetZansu.Parameters.AddWithValue("@y", ngYosanTuki.ToYYYYMM());
            cmdGetZansu.Parameters.AddWithValue("@Hinban", "");
            cmdGetZansu.Parameters.AddWithValue("@Size", "");
            cmdGetZansu.Transaction = t;

            // 予算月 + 品番 + サイズ単位で、未送信の納品データ取得
            // →一部未納で完了時は、最終送信対象の納品データに完了フラグが立っているかチェックする。
            SqlDataAdapter daGetMisoushin = new SqlDataAdapter("", t.Connection);
//            daGetMisoushin.SelectCommand.CommandText = @"
//SELECT                  dbo.T_NouhinMeisai.*, dbo.T_NouhinHeader.ShukkaBi
//FROM                     dbo.T_NouhinHeader INNER JOIN
//dbo.T_NouhinMeisai ON dbo.T_NouhinHeader.HakkouNo = dbo.T_NouhinMeisai.HakkouNo AND 
//dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = dbo.T_NouhinMeisai.ShiiresakiCode AND 
//dbo.T_NouhinHeader.YYMM = dbo.T_NouhinMeisai.YYMM
//WHERE                   (dbo.T_NouhinHeader.SoushinFlg = 0) AND (dbo.T_NouhinMeisai.HinbanTsuikaFlg = N'0') AND (dbo.T_NouhinHeader.SashizuBi = @z) AND 
//(dbo.T_NouhinHeader.SeisanOrderNo = @s) AND (dbo.T_NouhinHeader.HikitoriOrderNo = @h) AND (dbo.T_NouhinHeader.YosanTsuki = @y) AND
//(dbo.T_NouhinMeisai.Hinban = @Hinban) AND (dbo.T_NouhinMeisai.Size = @Size)";
            daGetMisoushin.SelectCommand.CommandText = @"
SELECT dbo.T_NouhinMeisai.*, dbo.T_NouhinHeader.ShukkaBi
FROM   dbo.T_NouhinHeader INNER JOIN
        dbo.T_NouhinMeisai ON dbo.T_NouhinHeader.HakkouNo = dbo.T_NouhinMeisai.HakkouNo AND 
        dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = dbo.T_NouhinMeisai.ShiiresakiCode AND 
        dbo.T_NouhinHeader.YYMM = dbo.T_NouhinMeisai.YYMM
WHERE  (dbo.T_NouhinHeader.SoushinFlg = 0) AND (dbo.T_NouhinMeisai.HinbanTsuikaFlg = N'0') AND (dbo.T_NouhinHeader.SashizuBi = @z) AND 
       (dbo.T_NouhinHeader.SeisanOrderNo = @s) AND (dbo.T_NouhinHeader.HikitoriOrderNo = @h) AND (dbo.T_NouhinHeader.YosanTsuki = @y)
        --AND (dbo.T_NouhinMeisai.Hinban = @Hinban) AND (dbo.T_NouhinMeisai.Size = @Size)";
            daGetMisoushin.SelectCommand.Parameters.AddWithValue("@z", key.SashizuBi);
            daGetMisoushin.SelectCommand.Parameters.AddWithValue("@s", key.SeisanOrderNo);
            daGetMisoushin.SelectCommand.Parameters.AddWithValue("@h", key.HikitoriOrderNo);
            daGetMisoushin.SelectCommand.Parameters.AddWithValue("@y", ngYosanTuki.ToYYYYMM());
            //daGetMisoushin.SelectCommand.Parameters.AddWithValue("@Hinban", "");
            //daGetMisoushin.SelectCommand.Parameters.AddWithValue("@Size", "");
            daGetMisoushin.SelectCommand.Transaction = t;


            //-----------------------------------2016/09/20 M0237------------------------------------
            //予算品データの明細を取得
            SqlDataAdapter daGetYosan = new SqlDataAdapter("", t.Connection);
            daGetYosan.SelectCommand.CommandText =
                @"SELECT DISTINCT NiukeJigyoushoCode FROM T_ProperOrder
                  WHERE SashizuBi = @sb AND ShiiresakiCode = @s AND (HikitoriOrderNo = @o OR SeisanOrderNo = @o) AND (Hinban + Size = @h)";
            daGetYosan.SelectCommand.Parameters.AddWithValue("@sb", "");
            daGetYosan.SelectCommand.Parameters.AddWithValue("@s", "");
            daGetYosan.SelectCommand.Parameters.AddWithValue("@o", "");
            daGetYosan.SelectCommand.Parameters.AddWithValue("@h", "");
            daGetYosan.SelectCommand.CommandTimeout = 600000;
            daGetYosan.SelectCommand.Transaction = t;
            DataTable dtYosan = new DataTable();
            //---------------------------------------------------------------------------------------

            if (0 >= nHakkouNo)
            {
                object objHakkouNo = cmdGetMaxHakkouNo.ExecuteScalar();
                if (null == objHakkouNo || System.DBNull.Value == objHakkouNo)
                {
                    nHakkouNo = 90001;
                }
                else if (Convert.ToInt32(objHakkouNo) == 99999)
                {
                    //2013.04.08 岡村 
                    //99999 まで来たら 80001へ戻すよう修正

                    // 90000以下で最大の発行No取得
                    SqlCommand cmdGetMaxHakkouNo2 = new SqlCommand("", t.Connection);
                    cmdGetMaxHakkouNo2.Transaction = t;
                    cmdGetMaxHakkouNo2.CommandText = @"SELECT MAX(HakkouNo) AS HacchuNo FROM T_NouhinHeader WHERE (NouhinmotoShiiresakiCode = @s) AND (YYMM = @YYMM) AND HakkouNo < 90000";
                    cmdGetMaxHakkouNo2.Parameters.AddWithValue("@s", strShiiresakiCode);
                    cmdGetMaxHakkouNo2.Parameters.AddWithValue("@YYMM", int.Parse(data.dtHakkouBi.ToString("yyMM")));


                    object objHakkouNo2 = cmdGetMaxHakkouNo2.ExecuteScalar();

                    if (null == objHakkouNo2 || System.DBNull.Value == objHakkouNo2)
                    {
                        nHakkouNo = 80001;
                    }
                    else
                    {
                        nHakkouNo = Convert.ToInt32(objHakkouNo2) + 1;
                    }

                }
                else
                {
                    nHakkouNo = Convert.ToInt32(objHakkouNo) + 1;
                }
            }
            else { 
                // 伝票修正時でYYMMが変更なく、発行Noに変更が無い場合はここ
                // 発行Noは指定されている
            }

            // ----- ヘッダー ----
            MizunoDataSet.T_NouhinHeaderDataTable dtHeader = new MizunoDataSet.T_NouhinHeaderDataTable();
            MizunoDataSet.T_NouhinHeaderRow drHeader = dtHeader.NewT_NouhinHeaderRow();
            NouhinDataClass.InitT_NouhinHeaderRow(drHeader);
            
            // 発行No
            drHeader.HakkouNo = nHakkouNo;
            // 伝票区分
            drHeader.DenpyouKubun = DenpyouKubun._26;
            // 仕入先コード
            drHeader.NouhinmotoShiiresakiCode = strShiiresakiCode;
            // 出荷入力画面で選択された納品記号
            drHeader.NouhinKigou = data.NouhinKigou;

            // 荷受事業所コード                
            drHeader.NiukeBumon = data.NiukeJigyoushoCode;
            drHeader.NiukeBumonMei = data.NiukeJigyoushoMei;
            // 保管場所
            drHeader.NiukeBasho = data.HokanBasho;
            drHeader.NiukeBashoMei = data.NiukeBashoMei;
            //invoiceNo
            drHeader.InvoiceNo = data.invoiceNo;

            //2014/02/14 岡村
            //運送業者コード
            drHeader.UnsouGyoushaCode = data.UnsouGyoushaCode;

            //運送業者名
            drHeader.UnsouGyoushaMei = data.UnsouGyoushaMei;

            //個口数
            drHeader.KogutiSu = data.KogutiSu;



            //2013/12/04 岡村
            //緊急直送備考
            drHeader.KinkyuChokusouBikou = data.KinkyuChokusouBikou;

            // 予算月
            drHeader.YosanTsuki = ngYosanTuki.ToYYYYMM().ToString();
            // 予算品オーダ内の納品記号
            drHeader.OrderNouhinKigou = (string.IsNullOrEmpty(strNouhinKigou))? "" : strNouhinKigou;

            // 送り先
            drHeader.HanbaitenKigyouRyakuMei = data.HanbaitenKigyouRyakuMei;
            // 発注日を入れる
            
            drHeader.HakkouHiduke = data.dtHakkouBi.ToString("yyMMdd");
            
            // 発注日には指図日を設定する。(yyMMdd)
            string strSashizuBi = key.SashizuBi.Replace("/", "");
            if (strSashizuBi.Length == 8)
            {
                drHeader.HakkouHiduke = strSashizuBi.Substring(2, 6);
            }

            // 出荷日付                
            drHeader.ShukkaHiduke = data.dtHakkouBi.ToString("yyMMdd");
            // 出荷日
            drHeader.ShukkaBi = data.dtHakkouBi;


            // YYMM
            drHeader.YYMM = int.Parse(data.dtHakkouBi.ToString("yyMM"));

            // 送品区分
            drHeader.SouhinKubun = ((int)data.SouhinKubun).ToString();


            // 指図日
            drHeader.SashizuBi = key.SashizuBi;
            // 生産オーダーNo
            drHeader.SeisanOrderNo = key.SeisanOrderNo;
            // 引取オーダーNo
            drHeader.HikitoriOrderNo = key.HikitoriOrderNo;
            // 材料オーダーNo
            drHeader.ZairyouOrderNo = "";
            // 別注オーダーNo
            drHeader.OrderKanriNo = "";

            // 送信フラグ
            //2014/02/14 岡村 追加 送り状Noなど入力されていない場合は送信フラグ=未完了とする。
            //drHeader.SoushinFlg = SoushinFlg.NONE;
            if ((data.SouhinKubun == EnumSouhinKubun.Chokusou || data.SouhinKubun == EnumSouhinKubun.Nituu_GMS)
                && ((data.UnsouGyoushaCode == "" && data.UnsouGyoushaMei == "") || data.KogutiSu == ""))
            {
                drHeader.SoushinFlg = SoushinFlg.MIKAKUTEI;
            }
            else
            {
                drHeader.SoushinFlg = SoushinFlg.NONE;
            }
            
            // 納品月
            drHeader.HonNouhinTsuki = (null == data.ngHonNouhinTuki) ? "" : data.ngHonNouhinTuki.ToString("yyyyMM");

            /*
            // 出荷事業所
            // ※予算品のみ使用。出荷時の絞り込みで指定した事業所。
            // ※修正時にこの事業所の出荷数を増減する為に記録しておく必要がある。
            drHeader.ShukkaNiukeBumon = (null == strNiukeBumonCode) ? "" : strNiukeBumonCode;
            */
            if (string.IsNullOrEmpty(data.ShukkaNiukeJigyoushoCode))
                drHeader.SetShukkaNiukeBumonNull();
            else
                drHeader.ShukkaNiukeBumon = data.ShukkaNiukeJigyoushoCode;


            // SK担当者名
            drHeader.SKTantouMei = data.SKTantoushaMei;
            // 営業担当者名
            drHeader.EigyouTantouMei = data.EigyouTantoushaMei;

            // 店名                
            if (data.TenMei.Length <= 6)
            {
                drHeader.HanbaitenKaishaCode = data.TenMei;
            }
            else
            {
                drHeader.HanbaitenKaishaCode = data.TenMei.Substring(0, 6);
                drHeader.HanbaitenBusho = data.TenMei.Substring(6, (data.TenMei.Length - 6));
            }

            // Invoice No,
            drHeader.InvoiceNo = data.invoiceNo;

            drHeader.TourokuBi = DateTime.Now;
            dtHeader.Rows.Add(drHeader);

            int nGyouNo = 1;
            int nEdaban = 0;
            MizunoDataSet.T_ProperOrderDataTable dtProper = new MizunoDataSet.T_ProperOrderDataTable();
            MizunoDataSet.T_NouhinMeisaiDataTable dtM = new MizunoDataSet.T_NouhinMeisaiDataTable();
            List<string> lstHinbanSizeJigyousyo = new List<string>();
            List<string> lstKanryouFlgHinbanSize = new List<string>();  // 今回完了フラグをセットする品番サイズ
            MizunoDataSet.T_ProperOrdrer_NouhinHikiateDataTable dtHikiate = new MizunoDataSet.T_ProperOrdrer_NouhinHikiateDataTable();


            for (int i = 0; i < data.lstMeisai.Count; i++)
            {
                MeisaiData m = data.lstMeisai[i];
                MizunoDataSet.T_NouhinMeisaiRow dr = dtM.NewT_NouhinMeisaiRow();
                NouhinDataClass.InitT_NouhinMeisaiRow(dr);
                dr.HakkouNo = nHakkouNo;
                dr.ShiiresakiCode = drHeader.NouhinmotoShiiresakiCode;
                dr.YYMM = drHeader.YYMM;

                if (0 == i && m.TuikaHinban && m.Key.Hinban.StartsWith("ﾄｸｼｭﾋﾝ"))
                {
                    // 1行目が特殊品番(運賃等のﾄｸｼｭﾋﾝ-XX)になってしまう場合は発注データの1行目の品番、サイズ、出荷数 = 0のダミーデータを1行目に入れる
                    DataTable dtHinbanSize = new DataTable();
                    daGetFirstHinbanSize.Fill(dtHinbanSize);
                    MizunoDataSet.T_NouhinMeisaiRow drDummy = dtM.NewT_NouhinMeisaiRow();
                    NouhinDataClass.InitT_NouhinMeisaiRow(drDummy);
                    drDummy.HakkouNo = nHakkouNo;
                    drDummy.ShiiresakiCode = drHeader.NouhinmotoShiiresakiCode;
                    drDummy.YYMM = drHeader.YYMM;
                    drDummy.Hinban = Convert.ToString(dtHinbanSize.Rows[0]["Hinban"]);
                    drDummy.Size = Convert.ToString(dtHinbanSize.Rows[0]["Size"]);
                    drDummy.Tani = "";
                    drDummy.LotNo = "";
                    drDummy.GyouNo = 1;
                    drDummy.Edaban = 0;
                    drDummy.NouhinSuu = 0;
                    // ダミーデータの印
                    drDummy.RowNo = MeisaiRowNo.Dummy;
                    dtM.Rows.Add(drDummy);
                    nGyouNo = 2;
                }

                //2013-01-09 齊藤変更-----
                if (NouhinDataClass.NOUHINDATA_MAX_GYOU_COUNT < nGyouNo)
                {
                    // 行番号が9以上の時
                    nGyouNo = 1; // 戻す
                    nEdaban++;
                }
                //-----------------------

                dr.GyouNo = nGyouNo;
                dr.Edaban = nEdaban;

                dr.Hinban = m.Key.Hinban;

                if (m.TuikaHinban)
                {
                    dr.HinbanTsuikaFlg = HinbanTsuikaFlg.Tsuika; // 例：ﾄｸｼｭﾋﾝ-03
                    dr.KanryouFlg = KanryouFlg.Kanryou; // 追加品番は完了フラグ=9
                }
                else
                {
                    dr.HinbanTsuikaFlg = HinbanTsuikaFlg.Normal;

                    string strKey = string.Format("{0}\t{1}\t{2}", m.Key.Hinban, m.Key.Size,m.Key.NiukeJigyoushoCode);
                    if (!lstHinbanSizeJigyousyo.Contains(strKey))
                        lstHinbanSizeJigyousyo.Add(strKey);
                    if (m.KanryouFlag)
                    {
                        if (!lstKanryouFlgHinbanSize.Contains(strKey))
                            lstKanryouFlgHinbanSize.Add(strKey);
                    }

                    // 実際の予算品レコードの出荷数をセット
                    daProper.SelectCommand.CommandText = "select * from T_ProperOrder where SashizuBi=@z and SeisanOrderNo=@s and HikitoriOrderNo=@h and YosanTsuki=@y and Hinban=@Hinban and Size=@Size and Suuryou>ShukkaSuu";

                    if (!bShusei)
                        daProper.SelectCommand.CommandText += " and KanryouFlg=0";  // 修正時は完了フラグは考慮しない（100個の注文で90001で50個、90002で残り45個で完了フラグを同じ出荷日8/30で登録した後、90001の出荷日を8/31にして最終伝票にした時に問題となった為）

                    daProper.SelectCommand.Parameters["@Hinban"].Value = m.Key.Hinban;
                    daProper.SelectCommand.Parameters["@Size"].Value = m.Key.Size;

                    if (daProper.SelectCommand.Parameters.Contains("@NiukeJigyoushoCode"))
                        daProper.SelectCommand.Parameters.RemoveAt("@NiukeJigyoushoCode");

                    if (m.Key.NiukeJigyoushoCodeHikiateAll)
                    {
                        // 指定の事業所の注文からしか引き当てない。全て引き当ててなければ納入残なしでエラーにする場合
                        if (!string.IsNullOrEmpty(m.Key.NiukeJigyoushoCode))
                        {
                            // 事業所指定がある場合はこの事業所のデータから引き当てる
                            daProper.SelectCommand.CommandText += " and NiukeJigyoushoCode=@NiukeJigyoushoCode";
                            daProper.SelectCommand.Parameters.AddWithValue("@NiukeJigyoushoCode", m.Key.NiukeJigyoushoCode);
                        }
                    }
                    dtProper.Clear();
                    daProper.Fill(dtProper);
                    
                    if (!bShusei)
                    {
                        // 修正の時は、既に完了フラグが他の伝票にセットされている場合があるのでチェック対象としない
                        if (0 == dtProper.Count)
                            throw new Exception(string.Format("品番{0}、サイズ{1}の納入残はありません。", m.Key.Hinban, m.Key.Size));
                    }


                    // 出荷数を引き当て
                    int nShukkaSu = m.ShukkaSu;
                    dtProper.DefaultView.Sort = "RowNo";    // RowNoの小さいものから引き当て

                    if (!m.Key.NiukeJigyoushoCodeHikiateAll && !string.IsNullOrEmpty(m.Key.NiukeJigyoushoCode))
                    {
                        //事業所の選択が無い場合、入力画面で設定したデータで検索を掛ける
                        //伝票データアップロード時にこちらに入るため先にチェックを行っておく 無い場合は通常の処理をさせる
                        dtProper.DefaultView.RowFilter =
                            string.Format("NiukeJigyoushoCode='{0}' AND HokanBasho='{1}'", drHeader.NiukeBumon, drHeader.NiukeBasho);

                        if (dtProper.DefaultView.Count == 0)
                        {
                            // 指定の事業所から「先に」引き当てられる分だけ引き当てる
                            dtProper.DefaultView.RowFilter = string.Format("NiukeJigyoushoCode='{0}'", m.Key.NiukeJigyoushoCode);
                        }
                    }
                    else
                    {
                        // 入力画面で指定した事業所・保管場所が連携時のものと一緒なら、
                        // そこから引当を行う 20160825 事前設定がない場合のみ
                        //事業所の選択が無い場合、入力画面で設定したデータで検索を掛ける
                        dtProper.DefaultView.RowFilter =
                            string.Format("NiukeJigyoushoCode='{0}' AND HokanBasho='{1}'", drHeader.NiukeBumon, drHeader.NiukeBasho);
                    }

                    for (int k = 0; k < dtProper.DefaultView.Count; k++)
                    {
                        MizunoDataSet.T_ProperOrderRow drP = dtProper.DefaultView[k].Row as MizunoDataSet.T_ProperOrderRow;
                        int nZan = drP.Suuryou - drP.ShukkaSuu;
                        if (0 >= nZan) continue;
                        int nHikiate = (nShukkaSu < nZan) ? nShukkaSu : nZan;
                        drP.ShukkaSuu += nHikiate;
                        nShukkaSu -= nHikiate;
                        // 引き当て情報
                        MizunoDataSet.T_ProperOrdrer_NouhinHikiateRow drHikiate = dtHikiate.NewT_ProperOrdrer_NouhinHikiateRow();
                        drHikiate.HakkouNo = nHakkouNo;
                        drHikiate.YYMM = drHeader.YYMM;
                        drHikiate.ShiiresakiCode = drHeader.NouhinmotoShiiresakiCode;
                        drHikiate.GyouNo = nGyouNo;
                        drHikiate.Edaban = nEdaban;
                        drHikiate.SashizuBi = key.SashizuBi;
                        drHikiate.SeisanOrderNo = key.SeisanOrderNo;
                        drHikiate.HikitoriOrderNo = key.HikitoriOrderNo;
                        drHikiate.RowNo = drP.RowNo;
                        drHikiate.HikiateSu = nHikiate;
                        dtHikiate.Rows.Add(drHikiate);
                        //if (drP.ShukkaSuu == drP.Suuryou) drP.KanryouFlg = true;  // ★完了フラグはセットしない（完了フラグは数量>出荷数の時注文を終了させるためにセッさせる）
                        if (0 == nShukkaSu) break;
                    }

                    dtProper.DefaultView.RowFilter = null;

                    if (0 < nShukkaSu)
                    {
                        
                        for (int k = 0; k < dtProper.DefaultView.Count; k++)
                        {

                            

                            MizunoDataSet.T_ProperOrderRow drP = dtProper.DefaultView[k].Row as MizunoDataSet.T_ProperOrderRow;
                            int nZan = drP.Suuryou - drP.ShukkaSuu;

                            
                            if (0 >= nZan) continue;
                            int nHikiate = (nShukkaSu < nZan) ? nShukkaSu : nZan;
                            drP.ShukkaSuu += nHikiate;
                            nShukkaSu -= nHikiate;
                            // 引き当て情報
                            MizunoDataSet.T_ProperOrdrer_NouhinHikiateRow drHikiate = dtHikiate.NewT_ProperOrdrer_NouhinHikiateRow();
                            drHikiate.HakkouNo = nHakkouNo;
                            drHikiate.YYMM = drHeader.YYMM;
                            drHikiate.ShiiresakiCode = drHeader.NouhinmotoShiiresakiCode;
                            drHikiate.GyouNo = nGyouNo;
                            drHikiate.Edaban = nEdaban;
                            drHikiate.SashizuBi = key.SashizuBi;
                            drHikiate.SeisanOrderNo = key.SeisanOrderNo;
                            drHikiate.HikitoriOrderNo = key.HikitoriOrderNo;
                            drHikiate.RowNo = drP.RowNo;
                            drHikiate.HikiateSu = nHikiate;
                            dtHikiate.Rows.Add(drHikiate);
                            
                            //if (drP.ShukkaSuu == drP.Suuryou) drP.KanryouFlg = true;  // ★完了フラグはセットしない（完了フラグは数量>出荷数の時注文を終了させるためにセッさせる）
                            if (0 == nShukkaSu) break;
                        }
                    }

                    if (0 < nShukkaSu)
                        throw new Exception(string.Format("品番{0}、サイズ{1}の出荷数が出荷残数を超えています。", m.Key.Hinban, m.Key.Size));

                    daProper.Update(dtProper);


                    // 完了フラグ
                    dr.KanryouFlg = KanryouFlg.MiKanryou;   // 完了フラグは後で設定するので未完了にしておく
                    // 本明細には、同一品番+サイズのデータが複数存在するケースがある。（同一品番サイズで価格が異なる場合）
                    // 完了フラグは、品番+サイズ単位で設定するが、複数レコードに設定出来ず、最後のレコードに対して設定する必要ある。

                }
                dr.Size = m.Key.Size;
                dr.LotNo = "";
                dr.Tani = "";

                // 取引単価
                // 2013.01.24 1/17に納品データのTorihikiTankaを固定長でなくしたが、予算品の方にも同様の修正が必要だった。
                dr.TorihikiTanka = m.TourokuKakaku.ToString();
                //dr.TorihikiTanka = m.TourokuKakaku.ToString("000000.0").Replace(".", "");
                // 納品数
                dr.NouhinSuu = m.ShukkaSu;
                dr.NouhinKubun = (m.NouhinKubun == NouhinDataClass.EnumNouhinKubun.TujouNouhin)? "" : ((int)m.NouhinKubun).ToString();
                // 融通価格
                dr.YuuduuKakaku = m.YuuduuKakaku.ToString("0000000");
                // 籍付
                dr.Sekiduke = m.Sekizuke.ToString("0000");

                // 摘要
                dr.ShouhinRyakuMei = (string.IsNullOrEmpty(m.Tekiyou))? "" : m.Tekiyou.Trim();

                dr.RowNo = 0;

                dr.FreeKoumoku1 = string.IsNullOrEmpty(m.Free1) ? "" : m.Free1;
                dr.FreeKoumoku2 = string.IsNullOrEmpty(m.Free2) ? "" : m.Free2;
                dr.FreeKoumoku3 = string.IsNullOrEmpty(m.Free3) ? "" : m.Free3;

                dr.Bikou = string.IsNullOrEmpty(m.Bikou) ? "" : m.Bikou;

                dtM.Rows.Add(dr);

                nGyouNo++;
            }


            daHikiate.Update(dtHikiate);


            // 完納で完了フラグがセットされる品番+サイズを取得する。
            List<string> lstKannouHinbanSize = new List<string>();
            for (int i = 0; i < lstHinbanSizeJigyousyo.Count; i++)
            {
                string strHinbanSizeJigyousyo = lstHinbanSizeJigyousyo[i];
                string strHinban = strHinbanSizeJigyousyo.Split('\t')[0];
                string strSize = strHinbanSizeJigyousyo.Split('\t')[1];

                cmdGetZansu.Parameters["@Hinban"].Value = strHinban;
                cmdGetZansu.Parameters["@Size"].Value = strSize;

                //追加 20160920 事業所が選択されている場合はその事業所も含んで検索を行う
                //if (!string.IsNullOrEmpty(data.ShukkaNiukeJigyoushoCode))
                //{
                //    cmdGetZansu.CommandText += " AND (NiukeJigyoushoCode = @njcode)";
                //    cmdGetZansu.Parameters.AddWithValue("@njcode", data.ShukkaNiukeJigyoushoCode);
                //}
                //else //無い場合はコマンドから消す
                //    cmdGetZansu.CommandText = cmdGetZansu.CommandText.Replace(" AND (NiukeJigyoushoCode = @njcode)", "");

                int nZansu = Convert.ToInt32(cmdGetZansu.ExecuteScalar());
                if (0 > nZansu) throw new Exception(strHinban + "の残数がマイナスです。");
                if (0 == nZansu) 
                {
                    if (!lstKanryouFlgHinbanSize.Contains(strHinbanSizeJigyousyo))
                        lstKanryouFlgHinbanSize.Add(strHinbanSizeJigyousyo);
                    if (!lstKannouHinbanSize.Contains(strHinbanSizeJigyousyo))
                        lstKannouHinbanSize.Add(strHinbanSizeJigyousyo);
                }
            }
            
            // 最後の品番+サイズに対して完了フラグをセット（本納品データ中に同一品番サイズが含まれるケースが有る為）
            for (int i = 0; i < lstKanryouFlgHinbanSize.Count; i++)
            {
                string strHinban = lstKanryouFlgHinbanSize[i].Split('\t')[0];
                string strSize = lstKanryouFlgHinbanSize[i].Split('\t')[1];

                cmdSetKanryouFlg.Parameters["@Hinban"].Value = strHinban;
                cmdSetKanryouFlg.Parameters["@Size"].Value = strSize;

                if (data.NiukeFilter != "")
                {
                    cmdSetKanryouFlg.Parameters["@n"].Value = data.NiukeFilter;
                }

                cmdSetKanryouFlg.ExecuteNonQuery();

                dtM.DefaultView.RowFilter = string.Format("Hinban='{0}' and Size='{1}'", strHinban, strSize);
                dtM.DefaultView.Sort = "Edaban, GyouNo";
                MizunoDataSet.T_NouhinMeisaiRow drLast = dtM.DefaultView[dtM.DefaultView.Count - 1].Row as MizunoDataSet.T_NouhinMeisaiRow;
                drLast.KanryouFlg = KanryouFlg.Kanryou; // 最後のレコードに対して設定する。

                //予算品かつ事業所選択されていない状況なら完了フラグは8に設定 20160920
                if (drHeader.IsShukkaNiukeBumonNull())
                {
                    dtYosan.Clear();
                    daGetYosan.SelectCommand.Parameters["@sb"].Value = drHeader.SashizuBi;
                    daGetYosan.SelectCommand.Parameters["@s"].Value = drHeader.NouhinmotoShiiresakiCode;
                    daGetYosan.SelectCommand.Parameters["@o"].Value = drHeader.OrderKanriNo;
                    daGetYosan.SelectCommand.Parameters["@h"].Value = strHinban + strSize;

                    daGetYosan.Fill(dtYosan);

                    if (dtYosan.Rows.Count > 1)
                    {
                        drLast.KanryouFlg = KanryouFlg.JigyoushoKanryou;
                    }
                }
            }

            // ----- トレーラー -----
            MizunoDataSet.T_NouhinTrailerDataTable dtTrailer = new MizunoDataSet.T_NouhinTrailerDataTable();
            MizunoDataSet.T_NouhinTrailerRow drTrailer = dtTrailer.NewT_NouhinTrailerRow();
            NouhinDataClass.InitT_NouhinTrailerRow(drTrailer);

            drTrailer.YYMM = drHeader.YYMM;
            drTrailer.HakkouNo = nHakkouNo;
            //仕入先コード
            drTrailer.ShiiresakiCode = strShiiresakiCode;

            // 荷受担当者コード
            drTrailer.EigyouTantoushaCode = data.EigyouTantoushaCode;
            // 学校名、チーム名
            drTrailer.TeamMei = data.TeamMei;
            // 店名
            drTrailer.TenMei = data.TenMei;
            // 出荷先郵便番号
            drTrailer.ShukkaSakiYubinBangou = "";

            // 出荷先住所
            drTrailer.ShukkaSakiJyusho = "";

            // 出荷先TEL
            drTrailer.ShukkaSakiTel = "";

            // 生産部担当者コード
            drTrailer.SKTantoushaCode = data.SKTantoushaCode;
            // 運送方法
            drTrailer.UnsouHouhou = "";

            // 伝票発行日            
            drTrailer.NouhinShoHiduke = data.dtHakkouBi.ToString("yyMMdd");

            // 予備(発注No)
            drTrailer.Yobi2 = (key.HikitoriOrderNo != "") ? key.HikitoriOrderNo : key.SeisanOrderNo;
            dtTrailer.Rows.Add(drTrailer);

            SqlDataAdapter daHeader = new SqlDataAdapter("select * from T_NouhinHeader", t.Connection);
            daHeader.SelectCommand.Transaction = t;
            daHeader.InsertCommand = new SqlCommandBuilder(daHeader).GetInsertCommand();
            daHeader.InsertCommand.Transaction = t;
            daHeader.Update(dtHeader);

            daProper.Update(dtProper);
            daM.Update(dtM);
            daTrailer.Update(dtTrailer);

            NouhinDataClass.DenpyouKey dk = new NouhinDataClass.DenpyouKey(strShiiresakiCode, int.Parse(data.dtHakkouBi.ToString("yyMM")), nHakkouNo);


            // ----- 完了フラグのチェック -----
            for (int i = 0; i < lstHinbanSizeJigyousyo.Count; i++)
            {
                string strHinbanSizeJigyousyo = lstHinbanSizeJigyousyo[i];
                string strHinban = strHinbanSizeJigyousyo.Split('\t')[0];
                string strSize = strHinbanSizeJigyousyo.Split('\t')[1];
                string strJigyousyo = strHinbanSizeJigyousyo.Split('\t')[2];

                // 未送信データ取得
                /// 2014/05/26 トランザクション中はデータベースからの取得は不可
                /// 
                //daGetMisoushin.SelectCommand.Parameters["@Hinban"].Value = strHinban;
                //daGetMisoushin.SelectCommand.Parameters["@Size"].Value = strSize;
                //dtM.Clear();
                //daGetMisoushin.Fill(dtM);
                //if (0 == dtM.Count) continue;

                //DataView dv = new DataView(dtM);

                //dv.Sort = "ShukkaBi, YYMM, HakkouNo, Edaban, GyouNo";  // この順番で最後のレコードが最終納品データ(最後にミズノに送信するデータ)。1伝票内に同一品番サイズが出現することもある（価格が異なる）為、枝番、行Noの昇順で明細順で最後に完了フラグがセットされるよう考慮している。


                DataView dv = new DataView(dtM);
                dv.RowFilter = string.Format("Hinban='{0}' AND Size='{1}'", strHinban, strSize);

                if (0 == dv.Count) continue;

                dv.Sort = "YYMM, HakkouNo, Edaban, GyouNo"; 


                if (lstKannouHinbanSize.Contains(strHinbanSizeJigyousyo))
                {
                    dv.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND NouhinSuu>0", strHinban, strSize);   // 納品数=0の伝票もある為、納品数>0で最後の明細に完了フラグをセットしたい。(伝票修正で納品数=0の伝票(取消伝票)の出荷日が最後（最後の伝票になることもある）)
                    if (0 == dv.Count)
                        dv.RowFilter = null;    // 納品数>0の伝票が無く、納品数＝0の伝票に完了フラグをセットしないといけない。（基本的にこのケースは無いはず）

                    // 全数出荷済みなので、最後の未送信の納品データに完了フラグ（9）をセットする。
                    for (int k = 0; k < dv.Count; k++)
                    {
                        MizunoDataSet.T_NouhinMeisaiRow dr = dv[k].Row as MizunoDataSet.T_NouhinMeisaiRow;
                        //追加 予算品かつ事業所選択されていない状況なら完了フラグは8に設定 20160920
                        if (dv.Count - 1 == k)
                        {
                            dr.KanryouFlg = KanryouFlg.Kanryou;
                            if (drHeader.IsShukkaNiukeBumonNull())
                            {
                                dtYosan.Clear();
                                daGetYosan.SelectCommand.Parameters["@sb"].Value = drHeader.SashizuBi;
                                daGetYosan.SelectCommand.Parameters["@s"].Value = drHeader.NouhinmotoShiiresakiCode;
                                daGetYosan.SelectCommand.Parameters["@o"].Value = drHeader.OrderKanriNo;
                                daGetYosan.SelectCommand.Parameters["@h"].Value = strHinban + strSize;

                                daGetYosan.Fill(dtYosan);

                                if (dtYosan.Rows.Count > 1)
                                {
                                    dr.KanryouFlg = KanryouFlg.JigyoushoKanryou;
                                }
                            }
                        }
                        else
                            dr.KanryouFlg = KanryouFlg.MiKanryou;
                    }
                    daM.Update(dtM);
                }
                else
                {
                    // 出荷残数がある状態で完了の場合、もし何れかの納品データに完了フラグが立っている際、最終納品データにに完了フラグが立っているかチェック //追加 20160920
                    dv.RowFilter = string.Format("(KanryouFlg='{0}' OR KanryouFlg='{1}')", KanryouFlg.Kanryou, KanryouFlg.JigyoushoKanryou);
                    if (0 == dv.Count) continue;


                    // 以下完了フラグが存在する場合
                    MizunoDataSet.T_NouhinMeisaiRow drKanryouFlg = dv[0].Row as MizunoDataSet.T_NouhinMeisaiRow;
                    dv.RowFilter = null;
                    MizunoDataSet.T_NouhinMeisaiRow drLast = dv[dv.Count - 1].Row as MizunoDataSet.T_NouhinMeisaiRow;
                    if (drLast.YYMM == drKanryouFlg.YYMM && drLast.HakkouNo == drKanryouFlg.HakkouNo)
                    {
                        // 最後に完了フラグがセットされているのでOK
                    }
                    else
                    {
                        if (drKanryouFlg.YYMM == dk.YYMM && drKanryouFlg.HakkouNo == dk.HakkouNo)
                        {
                            // 自身に完了フラグが立っていた時
                            throw new Exception(string.Format("完了フラグを設定できません。(品番:{0}, サイズ:{1}の完了フラグは伝票発行年月{2:yyyy/MM}、発行No.{3}に設定してください。",
                                strHinban, (string.IsNullOrEmpty(strSize)) ? "なし" : strSize, 
                                new Core.Type.Nengetu(drLast.YYMM).ToDateTime(), drLast.HakkouNo));
                        }
                        else
                        {
                            // 他の伝票に完了フラグが立っていた時
                            string strMsg = "";
                            if (drLast.YYMM == dk.YYMM && drLast.HakkouNo == dk.HakkouNo)
                                strMsg = "完了フラグは本伝票で設定する必要があります。";
                            else
                                strMsg = string.Format("完了フラグは伝票発行年月{0:yyyy/MM}、発行No.{1}に設定する必要があります。", 
                                    new Core.Type.Nengetu(drLast.YYMM).ToDateTime(), drLast.HakkouNo);

                            throw new Exception(string.Format("伝票発行年月{0:yyyy/MM}、発行No.{1}で品番{2}、サイズ:{3}の完了フラグを外して下さい。{4}",
                                new Core.Type.Nengetu(drKanryouFlg.YYMM).ToDateTime(), drKanryouFlg.HakkouNo, strHinban, (string.IsNullOrEmpty(strSize)) ? "なし" : strSize, strMsg));
                        }
                    }
                }

                // 同一品番、複数事業所宛のオーダー且つ、事業所を指定しての出荷入力の場合、
                // 他事業所
                // 実際の予算品レコードの出荷数をセット
                
                daProper.SelectCommand.CommandText = "select * from T_ProperOrder where SashizuBi=@z and SeisanOrderNo=@s and HikitoriOrderNo=@h and YosanTsuki=@y and Hinban=@Hinban and Size=@Size and Suuryou>ShukkaSuu";

                if (!bShusei)
                    daProper.SelectCommand.CommandText += " and KanryouFlg=0";  // 修正時は完了フラグは考慮しない（100個の注文で90001で50個、90002で残り45個で完了フラグを同じ出荷日8/30で登録した後、90001の出荷日を8/31にして最終伝票にした時に問題となった為）

                daProper.SelectCommand.Parameters["@Hinban"].Value = strHinban;
                daProper.SelectCommand.Parameters["@Size"].Value = strSize;

                //if (daProper.SelectCommand.Parameters.Contains("@NiukeJigyoushoCode"))
                //    daProper.SelectCommand.Parameters.RemoveAt("@NiukeJigyoushoCode");


                dtProper.Clear();
                daProper.Fill(dtProper);
                
                //特定事業所を選択した場合でも完了とする為、以下のfor文は廃止 20160920
                //for (int k = 0; k < dtProper.DefaultView.Count; k++)
                //{
                //    MizunoDataSet.T_ProperOrderRow drP = dtProper.DefaultView[k].Row as MizunoDataSet.T_ProperOrderRow;

                //    //修正時は判別しないを追加 20160624
                //    if (!bShusei && drP.NiukeJigyoushoCode != strJigyousyo)
                //    {
                //        if (drP.KanryouFlg != true || drP.ShukkaSuu < drP.Suuryou)
                //        {
                //            //DataView dv = new DataView(dtM);
                //            dv.RowFilter = string.Format("Hinban='{0}' AND Size='{1}'", drP.Hinban,drP.Size);

                //            if (0 == dv.Count) continue;

                //            dv.Sort = "YYMM, HakkouNo, Edaban, GyouNo";

                //            for (int k2 = 0; k2 < dv.Count; k2++)
                //            {
                //                MizunoDataSet.T_NouhinMeisaiRow dr = dv[k2].Row as MizunoDataSet.T_NouhinMeisaiRow;
                //                dr.KanryouFlg = KanryouFlg.MiKanryou;
                //            }
                //            daM.Update(dtM);
                //        }
                //    }
                //}
            }

            // 出荷残数チェック(パフォーマンスが悪いのでバグのチェックが済んだら下記コードは削除すること)

            SqlDataAdapter daCheck = new SqlDataAdapter("", t.Connection);
            daCheck.SelectCommand.CommandText = "select * from VIEW_ProperChuzanCheckList where (HikitoriOrderNo = @h) AND (SeisanOrderNo = @s) AND (SashizuBi = @z)";

            daCheck.SelectCommand.Parameters.AddWithValue("@z", key.SashizuBi);
            daCheck.SelectCommand.Parameters.AddWithValue("@s", key.SeisanOrderNo);
            daCheck.SelectCommand.Parameters.AddWithValue("@h", key.HikitoriOrderNo);
            daCheck.SelectCommand.Transaction = t;

            ViewDataset.VIEW_ProperChuzanCheckListDataTable dtCheck = new ViewDataset.VIEW_ProperChuzanCheckListDataTable();
            daCheck.Fill(dtCheck);
            dtCheck.DefaultView.RowFilter = "ShukkaSuu<>NouhinSu";

            //if (0 < dtCheck.DefaultView.Count) 
            //{
            //    List<string> lstError = new List<string>();
            //    for (int i = 0; i < dtCheck.DefaultView.Count; i++) {
            //        ViewDataset.VIEW_ProperChuzanCheckListRow dr = dtCheck.DefaultView[i].Row as ViewDataset.VIEW_ProperChuzanCheckListRow;

            //        for (int m = 0; m < data.lstMeisai.Count; m++) 
            //        {
            //            // 旧データの注残差異でチェックに引っかかってしまう為、今回登録したアイテムだけチェックすることにした。
            //            if (data.lstMeisai[m].Key.Hinban == dr.Hinban && data.lstMeisai[m].Key.Size == dr.Size)
            //            { 
            //                lstError.Add(string.Format("品番{0}、サイズ{1}で出荷数の差異があります。出荷数={2}, 納品数={3}",
            //                    dr.Hinban, dr.Size, dr.ShukkaSuu, dr.NouhinSu));
            //                break;
            //            }
            //        }
            //    }

            //    if (0 < lstError.Count)
            //        throw new Exception(string.Join("/", lstError.ToArray()));
            //}

            return dk;
        }





        public static Core.Error DenpyouShuseiTouroku(
            NouhinDataClass.DenpyouKey key, TourokuData data, SqlConnection c, out NouhinDataClass.DenpyouKey newKey)
        {
            newKey = null;

            SqlDataAdapter daHeader = new SqlDataAdapter("", c);
            daHeader.SelectCommand.CommandText = @"
SELECT                  dbo.T_NouhinHeader.*
FROM                     dbo.T_NouhinHeader
WHERE                   (HakkouNo = @h) AND (NouhinmotoShiiresakiCode = @s) AND (YYMM = @y)";

            daHeader.SelectCommand.Parameters.AddWithValue("@h", key.HakkouNo);
            daHeader.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            daHeader.SelectCommand.Parameters.AddWithValue("@y", key.YYMM);

            // 明細
            SqlDataAdapter daM = new SqlDataAdapter("", c);
            daM.SelectCommand.CommandText = @"
SELECT                  dbo.T_NouhinMeisai.*
FROM                     dbo.T_NouhinMeisai
WHERE                   (ShiiresakiCode = @s) AND (YYMM = @y) AND (HakkouNo = @h)";

            daM.SelectCommand.Parameters.AddWithValue("@h", key.HakkouNo);
            daM.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            daM.SelectCommand.Parameters.AddWithValue("@y", key.YYMM);
            daM.UpdateCommand = new SqlCommandBuilder(daM).GetUpdateCommand();


            // 出荷引き当て情報
            SqlDataAdapter daHikiate = new SqlDataAdapter("", c);
            daHikiate.SelectCommand.CommandText = @"
SELECT                  dbo.T_ProperOrdrer_NouhinHikiate.*
FROM                     dbo.T_ProperOrdrer_NouhinHikiate
WHERE                   (HakkouNo = @h) AND (YYMM = @y) AND (ShiiresakiCode = @s)";
            daHikiate.SelectCommand.Parameters.AddWithValue("@h", key.HakkouNo);
            daHikiate.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            daHikiate.SelectCommand.Parameters.AddWithValue("@y", key.YYMM);



            // 品番+サイズ単位で取得★納品記号を抽出条件としていたが、１注文に複数の納品記号が出現することは無いので、条件から外した。
            SqlDataAdapter daProper = new SqlDataAdapter("", c);
            daProper.SelectCommand.CommandText = "select * from T_ProperOrder where SashizuBi=@z and SeisanOrderNo=@s and HikitoriOrderNo=@h and YosanTsuki=@y and Hinban=@Hinban and Size=@Size";
            daProper.SelectCommand.Parameters.AddWithValue("@z", "");
            daProper.SelectCommand.Parameters.AddWithValue("@s", "");
            daProper.SelectCommand.Parameters.AddWithValue("@h", "");
            daProper.SelectCommand.Parameters.AddWithValue("@y", 0);
            daProper.SelectCommand.Parameters.AddWithValue("@Hinban", "");
            daProper.SelectCommand.Parameters.AddWithValue("@Size", "");
            daProper.UpdateCommand = new SqlCommandBuilder(daProper).GetUpdateCommand();


            SqlDataAdapter daProperGetRow = new SqlDataAdapter("", c);
            daProperGetRow.SelectCommand.CommandText = "select * from T_ProperOrder where SashizuBi=@z and SeisanOrderNo=@s and HikitoriOrderNo=@h and RowNo=@r";
            daProperGetRow.SelectCommand.Parameters.AddWithValue("@z", "");
            daProperGetRow.SelectCommand.Parameters.AddWithValue("@s", "");
            daProperGetRow.SelectCommand.Parameters.AddWithValue("@h", "");
            daProperGetRow.SelectCommand.Parameters.AddWithValue("@r", 0);
            daProperGetRow.UpdateCommand = new SqlCommandBuilder(daProperGetRow).GetUpdateCommand();



            // 完了フラグのリセット(予算月 + 品番 + サイズ単位)
            SqlCommand cmdSetKanryouFlgOff = new SqlCommand("", c);
            cmdSetKanryouFlgOff.CommandText = "update T_ProperOrder set KanryouFlg=0 where SashizuBi=@z and SeisanOrderNo=@s and HikitoriOrderNo=@h and YosanTsuki=@y and Hinban=@Hinban and Size=@Size";


            // 関連データ削除
            SqlCommand cmdDeleteNouhinData = new SqlCommand("", c);
            cmdDeleteNouhinData.CommandText = @"
delete T_NouhinHeader where (HakkouNo = @h) AND (NouhinmotoShiiresakiCode = @s) AND (YYMM = @y);
delete T_NouhinMeisai where (HakkouNo = @h) AND (ShiiresakiCode = @s) AND (YYMM = @y);
delete T_NouhinMeisaiOption where (HakkouNo = @h) AND (ShiiresakiCode = @s) AND (YYMM = @y);
delete T_NouhinTrailer where (HakkouNo = @h) AND (ShiiresakiCode = @s) AND (YYMM = @y);
delete T_ProperOrdrer_NouhinHikiate where (HakkouNo = @h) AND (ShiiresakiCode = @s) AND (YYMM = @y);
";
            cmdDeleteNouhinData.Parameters.AddWithValue("@h", key.HakkouNo);
            cmdDeleteNouhinData.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            cmdDeleteNouhinData.Parameters.AddWithValue("@y", key.YYMM);


            // 納品明細 削除
            SqlCommand cmdDeleteNouhinData2 = new SqlCommand("", c);
            cmdDeleteNouhinData2.CommandText = @"
DELETE FROM
	dbo.VIEW_NouhinMeisai
WHERE
	(ShiiresakiCode = @s) AND 
	(YYMM = @y) AND 
	(HakkouNo = @h) AND
	NouhinSuu = 0 AND
	KanryouFlg = 0
";
            cmdDeleteNouhinData2.Parameters.AddWithValue("@h", key.HakkouNo);
            cmdDeleteNouhinData2.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            cmdDeleteNouhinData2.Parameters.AddWithValue("@y", key.YYMM);

	

            SqlTransaction t = null;

            try
            {
                c.Open();
                t = c.BeginTransaction();

                daHeader.SelectCommand.Transaction = t;
                daM.SelectCommand.Transaction = daM.UpdateCommand.Transaction = t;
                daProper.SelectCommand.Transaction = daProper.UpdateCommand.Transaction = t;
                cmdSetKanryouFlgOff.Transaction = t;
                cmdDeleteNouhinData.Transaction = t;
                cmdDeleteNouhinData2.Transaction = t;
                daHikiate.SelectCommand.Transaction = t;
                daProperGetRow.SelectCommand.Transaction = daProperGetRow.UpdateCommand.Transaction = t;

                MizunoDataSet.T_NouhinHeaderDataTable dtHeader = new MizunoDataSet.T_NouhinHeaderDataTable();
                daHeader.Fill(dtHeader);
                if (0 == dtHeader.Count)
                    return new Core.Error("該当のデータがありません。");
                MizunoDataSet.T_NouhinHeaderRow drHeader = dtHeader[0];
                if (drHeader.SoushinFlg == SoushinFlg.SOUSHINZUMI)
                    return new Core.Error("送信済みの為修正できません。");


                daProper.SelectCommand.Parameters["@z"].Value = drHeader.SashizuBi;
                daProper.SelectCommand.Parameters["@s"].Value = drHeader.SeisanOrderNo;
                daProper.SelectCommand.Parameters["@h"].Value = drHeader.HikitoriOrderNo;
                daProper.SelectCommand.Parameters["@y"].Value = int.Parse(drHeader.YosanTsuki);
                //daProper.SelectCommand.Parameters["@n"].Value = drHeader.OrderNouhinKigou;  // drHeader.NouhinKigouでない点に注意(納品記号は1注文で1つであるが、出荷登録時、ユーザが納品記号を変更して登録できる。drHeader.OrderNouhinKigouは注文データ内の納品記号である。)→同一出荷アイテムで納品記号が複数出現することは無いので削除

              
                MizunoDataSet.T_NouhinMeisaiDataTable dtM = new MizunoDataSet.T_NouhinMeisaiDataTable();
                List<string> lstKanryouFlgHinbanSize = new List<string>();  // 完了フラグがセットされている品番サイズ
                List<string> lstKannouHinbanSize = new List<string>();  // 完納している品番+サイズ

                MizunoDataSet.T_ProperOrderDataTable dtProper = new MizunoDataSet.T_ProperOrderDataTable();

                daM.Fill(dtM);

                // 引き当て情報
                MizunoDataSet.T_ProperOrdrer_NouhinHikiateDataTable dtHikiate = new MizunoDataSet.T_ProperOrdrer_NouhinHikiateDataTable();
                daHikiate.Fill(dtHikiate);

                for (int i = 0; i < dtM.Count; i++) {
                    MizunoDataSet.T_NouhinMeisaiRow dr = dtM[i];
                    
                    if (dr.HinbanTsuikaFlg == HinbanTsuikaFlg.Tsuika) continue;
                    if (0 > dr.RowNo) continue;   // これも追加品番あるいはダミーレコードの条件

                    string strHinbanSize = string.Format("{0}\t{1}", dr.Hinban, dr.Size);
                    //事業所完了フラグを追加 20160920
                    if (dr.KanryouFlg == KanryouFlg.Kanryou || dr.KanryouFlg == KanryouFlg.JigyoushoKanryou)
                    {
                        if (!lstKanryouFlgHinbanSize.Contains(strHinbanSize))
                            lstKanryouFlgHinbanSize.Add(strHinbanSize);
                    }

                    if (0 == dr.NouhinSuu) continue;

                    if (!lstKannouHinbanSize.Contains(strHinbanSize))
                    {
                        daProper.SelectCommand.Parameters["@Hinban"].Value = dr.Hinban.TrimEnd();
                        daProper.SelectCommand.Parameters["@Size"].Value = dr.Size.TrimEnd();
                        dtProper.Clear();
                        daProper.Fill(dtProper);
                        // ここでデータがないのはおかしい
                        if (0 == dtProper.Count) { 
                            throw new Exception(string.Format("品番{0}、サイズ{2}の注文データがありません。", dr.Hinban, dr.Size));
                        }
                        int nChumonSu = Convert.ToInt32(dtProper.Compute("SUM(Suuryou)", null));
                        int nShukkaSu = Convert.ToInt32(dtProper.Compute("SUM(ShukkaSuu)", null));
                        if (nChumonSu == nShukkaSu) lstKannouHinbanSize.Add(strHinbanSize); // 完納していた品番+サイズ
                    }

                    if (0 == dtHikiate.Count)
                    {
                        // 引き当て情報が無い時

                        daProper.SelectCommand.CommandText = "select * from T_ProperOrder where SashizuBi=@z and SeisanOrderNo=@s and HikitoriOrderNo=@h and YosanTsuki=@y and Hinban=@Hinban and Size=@Size";
                        daProper.SelectCommand.Parameters["@Hinban"].Value = dr.Hinban;
                        daProper.SelectCommand.Parameters["@Size"].Value = dr.Size;


                        dtProper.Clear();
                        daProper.Fill(dtProper);
                        DataView dv = new DataView(dtProper);
                        // dv.RowFilter = "ShukkaSuu>0";　これ駄目、ループ中でShukkaSuuを更新するのでNG
                        dv.Sort = "RowNo DESC"; // 登録時の引き当てはRowNoの若いものから引き当てるので今は逆
                        int nShukkaZumiSu = (int)dr.NouhinSuu;
                        for (int k = 0; k < dv.Count; k++)
                        {
                            MizunoDataSet.T_ProperOrderRow drP = dtProper.DefaultView[k].Row as MizunoDataSet.T_ProperOrderRow;
                            if (drP.ShukkaSuu == 0) continue;
                            int nHikiate = (drP.ShukkaSuu > nShukkaZumiSu) ? nShukkaZumiSu : drP.ShukkaSuu;
                            drP.ShukkaSuu -= nHikiate;
                            nShukkaZumiSu -= nHikiate;
                            if (0 == nShukkaZumiSu) break;
                        }

                        if (0 != nShukkaZumiSu)
                        {
                            // 元に戻せない
                            throw new Exception(string.Format("品番{0}、サイズ{1}の出荷数をリセットできませんでした。差異数={2}個", dr.Hinban, dr.Size, nShukkaZumiSu));
                        }

                        daProper.Update(dtProper);
                    }
                }


                if (0 < dtHikiate.Count) 
                {
                    // 引き当て情報が有る場合はその情報をもとに出荷数の減残を行う（注文データが完納してしまえば引き当てデータは削除してＯＫ）
                    daProperGetRow.SelectCommand.Parameters["@z"].Value = drHeader.SashizuBi;
                    daProperGetRow.SelectCommand.Parameters["@s"].Value = drHeader.SeisanOrderNo;
                    daProperGetRow.SelectCommand.Parameters["@h"].Value = drHeader.HikitoriOrderNo;

                    for (int i = 0; i < dtHikiate.Count; i++) {
                        daProperGetRow.SelectCommand.Parameters["@r"].Value = dtHikiate[i].RowNo;
                        dtProper.Clear();
                        daProperGetRow.Fill(dtProper);
                        if (0 == dtProper.Count)
                            throw new Exception(string.Format("RowNo={0}の注文データがありません。", dtHikiate[i].RowNo));
                        dtProper[0].ShukkaSuu -= dtHikiate[i].HikiateSu;
                        if (0 > dtProper[0].ShukkaSuu)
                            throw new Exception(string.Format("RowNo={0}の注文データで引き当て後の出荷数がマイナスです。", dtHikiate[i].RowNo));
                        daProperGetRow.Update(dtProper);
                    }
                }


           
                // 完了フラグのオフ（納品データに完了フラグが立っていた品番+サイズのフラグを落とす）
                cmdSetKanryouFlgOff.Parameters.AddWithValue("@z", drHeader.SashizuBi);
                cmdSetKanryouFlgOff.Parameters.AddWithValue("@s",  drHeader.SeisanOrderNo);
                cmdSetKanryouFlgOff.Parameters.AddWithValue("@h", drHeader.HikitoriOrderNo);
                cmdSetKanryouFlgOff.Parameters.AddWithValue("@y", int.Parse(drHeader.YosanTsuki));
                cmdSetKanryouFlgOff.Parameters.AddWithValue("@Hinban", "");
                cmdSetKanryouFlgOff.Parameters.AddWithValue("@Size", "");
                for (int i = 0; i < lstKanryouFlgHinbanSize.Count; i++)
                {
                    string strHinban = lstKanryouFlgHinbanSize[i].Split('\t')[0];
                    string strSize = lstKanryouFlgHinbanSize[i].Split('\t')[1];
                    cmdSetKanryouFlgOff.Parameters["@Hinban"].Value = strHinban;
                    cmdSetKanryouFlgOff.Parameters["@Size"].Value = strSize;
                    cmdSetKanryouFlgOff.ExecuteNonQuery();
                }

                // 完了フラグの立っている別の未送信納品データで完納しているものは、他の伝票であっても完了フラグをオフにする(以下の例を参照)
                SqlDataAdapter daGetOtherNouhinMeisai = new SqlDataAdapter("", c);
                daGetOtherNouhinMeisai.SelectCommand.CommandText = @"
SELECT T_NouhinMeisai.* 
FROM                     dbo.T_NouhinHeader INNER JOIN
dbo.T_NouhinMeisai ON dbo.T_NouhinHeader.HakkouNo = dbo.T_NouhinMeisai.HakkouNo AND 
dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = dbo.T_NouhinMeisai.ShiiresakiCode AND 
dbo.T_NouhinHeader.YYMM = dbo.T_NouhinMeisai.YYMM
WHERE                   (dbo.T_NouhinMeisai.HinbanTsuikaFlg = N'0') AND (dbo.T_NouhinHeader.OrderKanriNo = N'') AND (dbo.T_NouhinHeader.YosanTsuki = @y) AND 
(dbo.T_NouhinHeader.SoushinFlg IN('0','1','2')) AND (dbo.T_NouhinMeisai.Hinban = @Hinban) AND (dbo.T_NouhinMeisai.Size = @Size) AND 
(dbo.T_NouhinHeader.SashizuBi = @z) AND (dbo.T_NouhinHeader.SeisanOrderNo = @s) AND (dbo.T_NouhinHeader.HikitoriOrderNo = @n) AND 
((dbo.T_NouhinMeisai.KanryouFlg = N'9') OR (dbo.T_NouhinMeisai.KanryouFlg = N'8')) AND NOT (dbo.T_NouhinHeader.HakkouNo = @HakkouNo AND dbo.T_NouhinHeader.YYMM = @YYMM)";
                //事業所完了フラグを追加 20160920
                daGetOtherNouhinMeisai.SelectCommand.Parameters.AddWithValue("@z", drHeader.SashizuBi);
                daGetOtherNouhinMeisai.SelectCommand.Parameters.AddWithValue("@s", drHeader.SeisanOrderNo);
                daGetOtherNouhinMeisai.SelectCommand.Parameters.AddWithValue("@n", drHeader.HikitoriOrderNo);
                daGetOtherNouhinMeisai.SelectCommand.Parameters.AddWithValue("@y", drHeader.YosanTsuki);
                daGetOtherNouhinMeisai.SelectCommand.Parameters.AddWithValue("@Hinban", "");
                daGetOtherNouhinMeisai.SelectCommand.Parameters.AddWithValue("@Size", "");
                daGetOtherNouhinMeisai.SelectCommand.Parameters.AddWithValue("@HakkouNo", key.HakkouNo);
                daGetOtherNouhinMeisai.SelectCommand.Parameters.AddWithValue("@YYMM", key.YYMM);
                daGetOtherNouhinMeisai.SelectCommand.Transaction = t;

                for (int i = 0; i < lstKannouHinbanSize.Count; i++)
                {
                    string strHinban = lstKannouHinbanSize[i].Split('\t')[0];
                    string strSize = lstKannouHinbanSize[i].Split('\t')[1];
                    cmdSetKanryouFlgOff.Parameters["@Hinban"].Value = strHinban;
                    cmdSetKanryouFlgOff.Parameters["@Size"].Value = strSize;
                    cmdSetKanryouFlgOff.ExecuteNonQuery();

                    // 他納品データで完了フラグが立っている場合がある。
                    // 例えば、ある注文の品番サイズの注文数：100個
                    // ①発行NO.90001で60個出荷
                    // ②発行NO.90002で40個出荷（ここでこの納品データに完了フラグが立つ）
                    // ③発行NO.90001を60個→50個に変更（T_ProperOrderの完了フラグはオフになるが、90002の納品データに完了フラグが残ってしまう）
                    // →本来、完了フラグがT_Properと納品データの両方保持している点が問題（不整合が起こる）で、完了フラグは独立して保持すべき
                    dtM.Clear();
                    daGetOtherNouhinMeisai.SelectCommand.Parameters["@Hinban"].Value = strHinban;
                    daGetOtherNouhinMeisai.SelectCommand.Parameters["@Size"].Value = strSize;
                    daGetOtherNouhinMeisai.Fill(dtM);
                    for (int m = 0; m < dtM.Count; m++)
                        dtM[m].KanryouFlg = KanryouFlg.MiKanryou;   // 当然１つしかないはずだが
                    daM.Update(dtM);
                }

                int nHakkouNo = (int.Parse(data.dtHakkouBi.ToString("yyMM")) == drHeader.YYMM)? drHeader.HakkouNo : 0;  // YYMMが変われば発行Noは再度採番
                ProperOrderKey pk = new ProperOrderKey(drHeader.SashizuBi, drHeader.SeisanOrderNo, drHeader.HikitoriOrderNo);

                // 納品データを削除
                cmdDeleteNouhinData.ExecuteScalar();

                newKey = DenpyouTouroku(true, nHakkouNo, pk, new Core.Type.Nengetu(int.Parse(drHeader.YosanTsuki)), drHeader.OrderNouhinKigou, data, t);

                // 2013/03/22 岡村
                // 完了FLG=False AND 出荷数=0 のデータは削除する。(伝票へ表示したくない)
                cmdDeleteNouhinData2.ExecuteScalar();

                // 2013/04/02 岡村
                // 上記削除処理により T_NouhinMeisai.GyouNo にズレが発生する場合があるので、振り直す
                SqlDataAdapter daGyouNo = new SqlDataAdapter("", c);
                daGyouNo.SelectCommand.CommandText = @"
SELECT T_NouhinMeisai.* 
FROM
    dbo.T_NouhinMeisai 
WHERE                  
    dbo.T_NouhinMeisai.HakkouNo = @HakkouNo AND 
    dbo.T_NouhinMeisai.ShiiresakiCode = @ShiiresakiCode AND
    dbo.T_NouhinMeisai.YYMM = @YYMM
ORDER BY
    Edaban,GyouNo
";

                daGyouNo.SelectCommand.Parameters.AddWithValue("@HakkouNo", key.HakkouNo);
                daGyouNo.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", key.ShiiresakiCode);
                daGyouNo.SelectCommand.Parameters.AddWithValue("@YYMM", key.YYMM);
                daGyouNo.SelectCommand.Transaction = t;
                daGyouNo.UpdateCommand = (new SqlCommandBuilder(daGyouNo)).GetUpdateCommand(); 
                MizunoDataSet.T_NouhinMeisaiDataTable dtGyou = new MizunoDataSet.T_NouhinMeisaiDataTable();
                daGyouNo.Fill(dtGyou);

                int iCurrentEdaban = 0;
                int iGyouNo = 1;
                for (int i = 0; i < dtGyou.Count; i++)
                {
                    if (i == 0 || int.Parse(dtGyou.Rows[i]["Edaban"].ToString()) != iCurrentEdaban)
                    {
                        iCurrentEdaban = int.Parse(dtGyou.Rows[i]["Edaban"].ToString());
                        iGyouNo = 1;
                    }

                    dtGyou.Rows[i]["GyouNo"] = iGyouNo;

                    iGyouNo++;
                }

                daGyouNo.Update(dtGyou);



                t.Commit();

                return null;
            }
            catch (Exception e)
            {
                if (null != t) t.Rollback();
                return new Core.Error(e);
            }
            finally
            {
                c.Close();
            }
        }




        public static MizunoDataSet.T_ProperOrdrer_NouhinHikiateDataTable
            getT_ProperOrdrer_NouhinHikiateDataTable(NouhinDataClass.DenpyouKey dk, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = @"
SELECT                  dbo.T_ProperOrdrer_NouhinHikiate.*
FROM                     dbo.T_ProperOrdrer_NouhinHikiate
WHERE                   (HakkouNo = @h) AND (ShiiresakiCode = @s) AND (YYMM = @y)";
            da.SelectCommand.Parameters.AddWithValue("@h", dk.HakkouNo);
            da.SelectCommand.Parameters.AddWithValue("@s", dk.ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@y", dk.YYMM);

            MizunoDataSet.T_ProperOrdrer_NouhinHikiateDataTable dt = new MizunoDataSet.T_ProperOrdrer_NouhinHikiateDataTable();
            da.Fill(dt);
            return dt;
        }




        [Serializable]
        public class VIEW_ProperOrderSummaryKensaku
        {
            public List<string> lstShiiresakiCode = null;
            public string strNiukeJigyoushoCode = null; // 空白も有り得る
            public Core.Sql.FilterItem objHinban = null;
            public Core.Type.Nengetu ngYosanTukiFrom = null;
            public Core.Type.Nengetu ngYosanTukiTo = null;
        }



        public static ViewDataset.VIEW_ProperOrderSummaryDataTable
            getVIEW_ProperOrderSummaryDataTable(VIEW_ProperOrderSummaryKensaku p, Core.Sql.RowNumberInfo r, SqlConnection c, ref int nCount)
        {
            nCount = 0;
            SqlDataAdapter da = new SqlDataAdapter("", c);

            List<string> lstYosanTuki = new List<string>();
            List<string> lstSuryou = new List<string>(); 
            List<string> lstShukkaSu = new List<string>();

            List<string> lstShukkaSuAndZansu = new List<string>();

            int nNo = 1;
            for (Core.Type.Nengetu ng = p.ngYosanTukiFrom; ng.ToYYYYMM() <= p.ngYosanTukiTo.ToYYYYMM(); ng = ng.AddMonth(1))
            {
                lstYosanTuki.Add(string.Format("[{0}]", ng.ToYYYYMM()));
                lstSuryou.Add(string.Format("[{0}] as Suryou{1}", ng.ToYYYYMM(), nNo));
                lstShukkaSu.Add(string.Format("[{0}] as ShukkaSu{1}", ng.ToYYYYMM(), nNo));
                //lstShukkaSuAndZansu.Add(string.Format("V_Shukka.ShukkaSu{0} as ShukkaSu{0}, V_Suryou.Suryou{0} - V_Shukka.ShukkaSu{0} AS ZanSu{0}", nNo));
                lstShukkaSuAndZansu.Add(string.Format("V_Shukka.ShukkaSu{0} as ShukkaSu{0}", nNo));
                nNo++;
            }


            string str = @"
SELECT                  V_Suryou.*, M_Size.OrderByNo as SizeOrderByNo, VIEW_Shiiresaki.RyakuMei as ShiiresakiMei, {0}
FROM                     
(SELECT                  ShiiresakiCode, NiukeJigyoushoCode, Hinban, Size, {1} 
FROM                     (SELECT ShiiresakiCode, NiukeJigyoushoCode, YosanTsuki, Hinban , Size, Suuryou FROM T_ProperOrder WHERE {4}) 
T PIVOT (SUM(Suuryou) FOR YosanTsuki IN ({3})) 
AS PIVOT_TABLE) AS V_Suryou INNER JOIN
(SELECT                  ShiiresakiCode, NiukeJigyoushoCode, Hinban, Size, {2}
FROM                     (SELECT ShiiresakiCode, NiukeJigyoushoCode, YosanTsuki, Hinban , Size, CASE KanryouFlg WHEN 0 THEN ShukkaSuu WHEN 1 THEN Suuryou END AS ShukkaSuu
FROM  T_ProperOrder WHERE {4}) T PIVOT (SUM(ShukkaSuu) FOR 
YosanTsuki IN ({3})) AS PIVOT_TABLE) as V_Shukka ON V_Suryou.ShiiresakiCode = V_Shukka.ShiiresakiCode AND 
V_Suryou.NiukeJigyoushoCode = V_Shukka.NiukeJigyoushoCode AND V_Suryou.Hinban = V_Shukka.Hinban AND 
V_Suryou.Size = V_Shukka.Size 
LEFT OUTER JOIN M_Size ON V_Suryou.Size = M_Size.Size 
LEFT OUTER JOIN VIEW_Shiiresaki ON V_Suryou.ShiiresakiCode = VIEW_Shiiresaki.ShiiresakiCode
";


            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
            if (null != p.lstShiiresakiCode && 0 < p.lstShiiresakiCode.Count) { 
                w.Add(string.Format("ShiiresakiCode in ('{0}')", string.Join("','", p.lstShiiresakiCode.ToArray())));
            }

            if (null != p.strNiukeJigyoushoCode)
            {
                w.Add("NiukeJigyoushoCode=@NiukeJigyoushoCode");
                da.SelectCommand.Parameters.AddWithValue("@NiukeJigyoushoCode", p.strNiukeJigyoushoCode);
            }

            if (null != p.objHinban) {
                w.Add(p.objHinban.GetFilterText("LTRIM(Hinban)", "@Hinban", da.SelectCommand));
            }

            w.Add(string.Format("(YosanTsuki>={0} AND YosanTsuki<={1})", p.ngYosanTukiFrom.ToYYYYMM(), p.ngYosanTukiTo.ToYYYYMM())); 


            da.SelectCommand.CommandText = string.Format(str,
                string.Join(",", lstShukkaSuAndZansu.ToArray()),
                string.Join(",", lstSuryou.ToArray()),
                string.Join(",", lstShukkaSu.ToArray()),
                string.Join(",", lstYosanTuki.ToArray()), w.WhereText);

            ViewDataset.VIEW_ProperOrderSummaryDataTable dt = new ViewDataset.VIEW_ProperOrderSummaryDataTable();

            if (null != r)
                r.LoadData(da.SelectCommand, c, dt, ref nCount);
            else
            {
                da.Fill(dt);
                nCount = dt.Count;
            }

            return dt;
        }



        public static MizunoDataSet.T_ProperUketsukeRow
            getT_ProperUketsukeRow(ProperOrderKey key, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT * FROM T_ProperUketsuke WHERE (SashizuBi = @SashizuBi) AND (SeisanOrderNo = @SeisanOrderNo) AND (HikitoriOrderNo = @HikitoriOrderNo) ";

            da.SelectCommand.Parameters.AddWithValue("@SashizuBi", key.SashizuBi);
            da.SelectCommand.Parameters.AddWithValue("@SeisanOrderNo", key.SeisanOrderNo);
            da.SelectCommand.Parameters.AddWithValue("@HikitoriOrderNo", key.HikitoriOrderNo);
            MizunoDataSet.T_ProperUketsukeDataTable dt = new MizunoDataSet.T_ProperUketsukeDataTable();
            da.Fill(dt);
            if (1 == dt.Count) return dt[0];
            return null;
        }


        /// <summary>
        /// 発注受付情報を登録
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="sqlConn"></param>
        public static Core.Error 
            T_ProperUketsuke_Insert(ProperOrderKey key, string LoginID, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT * FROM T_ProperUketsuke WHERE (SashizuBi = @SashizuBi) AND (SeisanOrderNo = @SeisanOrderNo) AND (HikitoriOrderNo = @HikitoriOrderNo) ";

            da.SelectCommand.Parameters.AddWithValue("@SashizuBi", key.SashizuBi);
            da.SelectCommand.Parameters.AddWithValue("@SeisanOrderNo", key.SeisanOrderNo);
            da.SelectCommand.Parameters.AddWithValue("@HikitoriOrderNo", key.HikitoriOrderNo);
            
            da.InsertCommand = (new SqlCommandBuilder(da)).GetInsertCommand();
            MizunoDataSet.T_ProperUketsukeDataTable dt = new MizunoDataSet.T_ProperUketsukeDataTable();
            da.Fill(dt);
            if (0 != dt.Count) return null;

            try
            {
                MizunoDataSet.T_ProperUketsukeRow dr = dt.NewT_ProperUketsukeRow();
                dr.SashizuBi = key.SashizuBi;
                dr.SeisanOrderNo = key.SeisanOrderNo;
                dr.HikitoriOrderNo = key.HikitoriOrderNo;
                dr.KakuninBi = DateTime.Now;
                dr.UketsukeID = LoginID;
                dt.Rows.Add(dr);
                da.Update(dt);

                return null;
            }
            catch (Exception e)
            {
                return new LibError(e);
            }
        }
        
        //予算品一括受付
        public static Core.Error
            Set_T_ProperUketsuke(List<ProperOrderKey> lstKey, string LoginID, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT * FROM T_ProperUketsuke WHERE (SashizuBi = @SashizuBi) AND (SeisanOrderNo = @SeisanOrderNo) AND (HikitoriOrderNo = @HikitoriOrderNo) ";

            da.SelectCommand.Parameters.AddWithValue("@SashizuBi", DateTime.Now);
            da.SelectCommand.Parameters.AddWithValue("@SeisanOrderNo", SqlDbType.NVarChar);
            da.SelectCommand.Parameters.AddWithValue("@HikitoriOrderNo", SqlDbType.NVarChar);
            da.InsertCommand = (new SqlCommandBuilder(da)).GetInsertCommand();
            MizunoDataSet.T_ProperUketsukeDataTable dt = new MizunoDataSet.T_ProperUketsukeDataTable();

            SqlTransaction t = null;

            try
            {
                sqlConn.Open();
                t = sqlConn.BeginTransaction();
                da.SelectCommand.Transaction =
                da.InsertCommand.Transaction = t;
                for (int i = 0; i < lstKey.Count; i++)
                {
                    dt = new MizunoDataSet.T_ProperUketsukeDataTable();
                    da.SelectCommand.Parameters["@SashizuBi"].Value = lstKey[i].SashizuBi;
                    da.SelectCommand.Parameters["@SeisanOrderNo"].Value = lstKey[i].SeisanOrderNo;
                    da.SelectCommand.Parameters["@HikitoriOrderNo"].Value = lstKey[i].HikitoriOrderNo;
                    da.Fill(dt);
                    if (dt.Count > 0) continue;

                    MizunoDataSet.T_ProperUketsukeRow dr = dt.NewT_ProperUketsukeRow();
                    dr.SashizuBi = lstKey[i].SashizuBi;
                    dr.SeisanOrderNo = lstKey[i].SeisanOrderNo;
                    dr.HikitoriOrderNo = lstKey[i].HikitoriOrderNo;
                    dr.KakuninBi = DateTime.Now;
                    dr.UketsukeID = LoginID;
                    dt.Rows.Add(dr);
                    da.Update(dt);
                }
                t.Commit();

                return null;
            }
            catch (Exception e)
            {
                if (null != t) t.Rollback();
                return new LibError(e);
            }
            finally
            {
                sqlConn.Close();
            }
        }

        public static string get_TukaCode(string OrderNo, string Hinban, string Size, SqlConnection sql)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            //MizunoDataSet.T_ProperOrderDataTable dt = new MizunoDataSet.T_ProperOrderDataTable();
            DataTable dt = new DataTable();
            da.SelectCommand.CommandText = "select TukaCode from T_ProperOrder where (SeisanOrderNo = @on OR HikitoriOrderNo=@on) AND Hinban=@hb AND Size=@rn";
            da.SelectCommand.Parameters.AddWithValue("@on", OrderNo);
            da.SelectCommand.Parameters.AddWithValue("@hb", Hinban);
            da.SelectCommand.Parameters.AddWithValue("@rn", Size);

            da.Fill(dt);
            try
            {
                return dt.Rows[0][0].ToString();
            }
            catch
            {
                return "";
            }
        }

        public static string get_Hokanbasyo_Proper(string OrderNo, string Hinban,string ShiiresakiCode, string JigyoushoCode, SqlConnection sql)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            MizunoDataSet.T_ProperOrderDataTable dt = new MizunoDataSet.T_ProperOrderDataTable();
            da.SelectCommand.CommandText = "select * from T_ProperOrder where (SeisanOrderNo=@o or HikitoriOrderNo=@o) and ShiiresakiCode=@s AND Hokanbasho != '' AND NiukeJigyoushoCode = @j"; //←Hokanbashoを追加 20160626 JigyoushoCodeを追加 20170623
            da.SelectCommand.Parameters.AddWithValue("@o", OrderNo);
            da.SelectCommand.Parameters.AddWithValue("@s", ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@j", JigyoushoCode);
            if (Hinban != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@h", Hinban);
                da.SelectCommand.CommandText += " and Hinban=@h ";
            }
            da.Fill(dt);
            if (dt.Count > 0)
            {
                return dt[0].HokanBasho;
            }
            else
            {
                return "";
            }
        }
    }
}














