using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Core;
using Core.Type;
using Microsoft.VisualBasic;


namespace MizunoDAL
{
    public class NouhinDataClass
    {
        public const int NOUHINDATA_MAX_GYOU_COUNT = 8;

        public class DenpyouKey
        {
            public int HakkouNo
            {
                get;
                set;
            }
            public string ShiiresakiCode
            {
                get;
                set;
            }
            public int YYMM
            {
                get;
                set;
            }

            public DenpyouKey(string strShiiresakiCode, int nYYMM, int nHakkouNo)
            {
                this.ShiiresakiCode = strShiiresakiCode;
                this.YYMM = nYYMM;
                this.HakkouNo = nHakkouNo;
            }
            public DenpyouKey()
            {
            }

            public override string ToString()
            {
                return string.Format("{0}\t{1}\t{2}", ShiiresakiCode, YYMM, HakkouNo);
            }

            public static DenpyouKey Parse(string str)
            {
                string[] s = str.Split('\t');
                return new DenpyouKey(s[0], int.Parse(s[1]), int.Parse(s[2]));
            }
        }

        public enum EnumNouhinKubun
        {
            TujouNouhin = 0,        // 通常納品
            TokuisakiHimoduke = 1,        // 得意先紐付け
            OrderHimoduke = 2,       //  ｵｰﾀﾞｰ紐付け
            SekiZukeNyuko = 3, //       籍付入庫 
            SenkouNyuko = 6      // 先行入庫
        }

        internal class MeisaiData
        {
            public int LineCount
            {
                get
                {
                    return MeisaiOptionDatas.Count + 1; // 明細オプション数 + ヘッダー1件
                }
            }

            public string Data
            {
                get;
                set;
            }
            public List<string> MeisaiOptionDatas = new List<string>();

            public void Write(System.IO.StringWriter sw)
            {
                sw.WriteLine(Data);
                for (int i = 0; i < MeisaiOptionDatas.Count; i++)
                    sw.WriteLine(MeisaiOptionDatas[i]);
            }
        }

        internal class DenpyouData
        {
            public string HeaderData
            {
                get;
                set;
            }
            public List<MeisaiData> MeisaiDatas = new List<MeisaiData>();

            public string Trailer
            {
                get;
                set;
            }

            public int LineCount
            {
                get
                {
                    int nMeisaiDataCount = 0;
                    for (int i = 0; i < MeisaiDatas.Count; i++)
                    {
                        nMeisaiDataCount += MeisaiDatas[i].LineCount;
                    }
                    return 2 + nMeisaiDataCount;    // ヘッダ+トレーラー+明細
                }
            }

            public void Write(System.IO.StringWriter sw)
            {
                sw.WriteLine(HeaderData);
                for (int i = 0; i < MeisaiDatas.Count; i++)
                    MeisaiDatas[i].Write(sw);
                sw.WriteLine(Trailer);
            }
        }

        internal class ShiiresakiNouhinData
        {
            public List<DenpyouData> DenpyouDatas = new List<DenpyouData>();

            private string NouhinmotoShiiresakiCode
            {
                get;
                set;
            }

            private DateTime DataHiduke
            {
                get;
                set;
            }

            public void Write(System.IO.StringWriter sw)
            {
                int nLineCount = 0;
                for (int i = 0; i < DenpyouDatas.Count; i++)
                    nLineCount += this.DenpyouDatas[i].LineCount;
                nLineCount++;    // 納品ファイルヘッダ1行を追加


                // 納品ファイルヘッダー
                sw.Write("A");
                sw.Write("21");
                sw.Write(DataHiduke.ToString("yyMMdd"));
                sw.Write(DataHiduke.ToString("HHmmss"));
                sw.Write(DataHiduke.ToString("yyMMdd"));
                sw.Write("{0,-7}", NouhinmotoShiiresakiCode);

                sw.Write("{0,1}", "");//dr.Yobi1 = "";
                sw.Write("{0,6}", "");//dr.SaishuuKaishaCode = "";
                sw.Write("{0,2}", "");//dr.Yobi2 = "";
                sw.Write("{0,6}", "");//dr.ChokusetuKaishaCode = "";
                sw.Write("{0,2}", "");//dr.Yobi3 = "";

                // レコード長
                sw.Write("128");    // dr.RecodeChou = "128";
                // 仕入先毎の全納品データ行数(ファイルヘッダー(1行)を含む)
                sw.Write(nLineCount.ToString("000000"));

                // 伝票ヘッダーの行数
                sw.Write(DenpyouDatas.Count.ToString("00000"));
                sw.Write("00000");      // 予備4
                sw.Write("{0,64}", ""); //予備5
                sw.WriteLine("");

                for (int i = 0; i < DenpyouDatas.Count; i++)
                    DenpyouDatas[i].Write(sw);
            }


            public ShiiresakiNouhinData(string NouhinmotoShiiresakiCode, DateTime DataHiduke)
            {
                this.NouhinmotoShiiresakiCode = NouhinmotoShiiresakiCode;
                this.DataHiduke = DataHiduke;
            }
        }

        public static string getSize(SqlConnection sql ,string HakkouNo,string Edaban,string GyouNo,string ShiiresakiCode,string YYMM)
        {
            string Size = "";
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText = "select Size from T_NouhinMeisai where HakkouNo=@hn and Edaban=@eb and GyouNo=@gn and ShiiresakiCode=@sc and YYMM=@ym";
            da.SelectCommand.Parameters.AddWithValue("@hn", HakkouNo);
            da.SelectCommand.Parameters.AddWithValue("@eb", Edaban);
            da.SelectCommand.Parameters.AddWithValue("@gn", GyouNo);
            da.SelectCommand.Parameters.AddWithValue("@sc", ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@ym", YYMM);
            DataTable dt = new DataTable();
            da.Fill(dt);
            try
            {
                Size = dt.Rows[0][0].ToString();
            }
            catch
            {

            }
            return Size;
        }




        public static Core.Error CreateNouhinData(
            DateTime dtDataHiduke,
            Core.Type.NengappiKikan ShukkaBi,
            List<string> lstShiteiShiiresakiCode,
            byte? sFlag, // 未送信
            bool bSetSoushinFlg, // データ連携タスクの場合必ず[true]
            string strFilePath, int nLimitT_NouhinHeaderCount, SqlConnection sqlConn, out string strData)
        {
            strData = null;
            
            SqlConnection sql = sqlConn;
            MizunoDataSet.T_NouhinHeaderDataTable dtHeader = new MizunoDataSet.T_NouhinHeaderDataTable();

            SqlCommand setSoushinFlg = new SqlCommand("", sqlConn);
            setSoushinFlg.CommandText = "update T_NouhinHeader set SoushinFlg=@sf, ShuturyokuBi=@ShuturyokuBi where HakkouNo=@h and NouhinmotoShiiresakiCode=@s and YYMM=@y";
            setSoushinFlg.Parameters.AddWithValue("@sf", SoushinFlg.SOUSHINZUMI);
            setSoushinFlg.Parameters.Add("@s", SqlDbType.NVarChar);
            setSoushinFlg.Parameters.Add("@y", SqlDbType.Int);
            setSoushinFlg.Parameters.Add("@h", SqlDbType.Int);
            setSoushinFlg.Parameters.AddWithValue("@ShuturyokuBi", DateTime.Now);
            setSoushinFlg.CommandTimeout = 600000;

            //---------------------------------------------------------------------------------------
            //出力対象データを取得する
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);

            string ncount = "";
            if (nLimitT_NouhinHeaderCount != -1)
            {
                ncount = " TOP (3000) ";
            }
            da.SelectCommand.CommandText = "SELECT " + ncount + " * FROM VIEW_RCV_MPASNH ";

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
            if (null != ShukkaBi)
            {
                w.Add(ShukkaBi.GenerateSQLAsDateTime("ShukkaBi"));
            }

            if (sFlag != null)
            {
                if (bSetSoushinFlg)
                {
                    /// データ連携タスクの場合必ず[true]
                    /// 
                    if (sFlag != SoushinFlg.MISOUSHIN) //出荷確定こっち SHUKKA_KAKUTEI
                    {
                        w.Add("SoushinFlg=@SoushinFlg");
                        da.SelectCommand.Parameters.AddWithValue("@SoushinFlg", sFlag);
                        //da.SelectCommand.Parameters.AddWithValue("@SoushinFlg", 1);
                    }
                    else //納品データはこっち NOUHIN_DATA
                    {
                        w.Add("SoushinFlg IN (@NONE, @KAKUTEI, @MIKAKUTEI)");
                        da.SelectCommand.Parameters.AddWithValue("@NONE", SoushinFlg.NONE);
                        da.SelectCommand.Parameters.AddWithValue("@KAKUTEI", SoushinFlg.KAKUTEI);
                        da.SelectCommand.Parameters.AddWithValue("@MIKAKUTEI", SoushinFlg.MIKAKUTEI);
                        //da.SelectCommand.Parameters.AddWithValue("@KAKUTEI", SoushinFlg.KAKUTEI);
                    }
                }
                else if (!bSetSoushinFlg && sFlag == SoushinFlg.MISOUSHIN)
                    {
                        /// 2014/07/07 手動での納品データダウンロードの場合、[未送信]か否かを判断する
                        w.Add("SoushinFlg IN (@NONE, @KAKUTEI, @MIKAKUTEI)");
                        da.SelectCommand.Parameters.AddWithValue("@NONE", SoushinFlg.NONE);
                        da.SelectCommand.Parameters.AddWithValue("@KAKUTEI", SoushinFlg.KAKUTEI);
                        da.SelectCommand.Parameters.AddWithValue("@MIKAKUTEI", SoushinFlg.MIKAKUTEI);
                    }
            }

            if (null != lstShiteiShiiresakiCode && 0 < lstShiteiShiiresakiCode.Count)
            {
                w.Add(string.Format("NouhinmotoShiiresakiCode in ('{0}')", string.Join("','", lstShiteiShiiresakiCode.ToArray())));
            }

            //一時的に除外する
            //w.Add("NouhinmotoShiiresakiCode NOT IN ('20531403','22000905','29025102')");

            string whereText = w.WhereText;
            if (whereText != "")
            {
                da.SelectCommand.CommandText += " WHERE " + whereText;
            }

            da.SelectCommand.CommandText += " ORDER BY YYMM,NouhinmotoShiiresakiCode,HakkouNo,Edaban,GyouNo";

            ViewDataset.VIEW_RCV_MPASNHDataTable dt = new ViewDataset.VIEW_RCV_MPASNHDataTable();
            da.SelectCommand.CommandTimeout = 600000;
            da.Fill(dt);

            //---------------------------------------------------------------------------------------

            //-----------------------------------2014/11/28 M0056------------------------------------
            //明細更新用
            DataView dv = dt.DefaultView;

            // 明細
            SqlDataAdapter daM = new SqlDataAdapter("", sqlConn);
            daM.SelectCommand.CommandText = @"
            SELECT      dbo.T_NouhinMeisai.*
            FROM        dbo.T_NouhinMeisai
            WHERE       (ShiiresakiCode = @s) AND (YYMM = @y) AND (HakkouNo = @h)";

            daM.SelectCommand.Parameters.AddWithValue("@h", "");
            daM.SelectCommand.Parameters.AddWithValue("@s", "");
            daM.SelectCommand.Parameters.AddWithValue("@y", "");
            daM.UpdateCommand = new SqlCommandBuilder(daM).GetUpdateCommand();
            


            MizunoDataSet.T_NouhinMeisaiDataTable dtM = new MizunoDataSet.T_NouhinMeisaiDataTable();

            //---------------------------------------------------------------------------------------

            //-----------------------------------2016/06/10 M0156------------------------------------
            // 納品データの明細を取得
            SqlDataAdapter daGetNH = new SqlDataAdapter("", sqlConn);
            daGetNH.SelectCommand.CommandText = 
                @"SELECT dbo.T_NouhinMeisai.RecodeKubun, dbo.T_NouhinMeisai.DataKubun, 
                      dbo.T_NouhinMeisai.HakkouNo, dbo.T_NouhinMeisai.Yobi1, dbo.T_NouhinMeisai.Edaban, dbo.T_NouhinMeisai.GyouNo, 
                      dbo.T_NouhinMeisai.ShouhinBunrui, dbo.T_NouhinMeisai.ShouhinCode, dbo.T_NouhinMeisai.Hinban, dbo.T_NouhinMeisai.HinbanTsuikaFlg, 
                      
                      dbo.T_NouhinMeisai.Filler, dbo.T_NouhinMeisai.TorihikiTanka, dbo.T_NouhinMeisai.Joudai, dbo.T_NouhinMeisai.BrandRyakuMei, 
                      dbo.T_NouhinMeisai.ShouhinRyakuMei, dbo.T_NouhinMeisai.JanCode, dbo.T_NouhinMeisai.NouhinSuu, dbo.T_NouhinMeisai.Size, 
                      dbo.T_NouhinMeisai.Meisai, dbo.T_NouhinMeisai.NouhinKubun, dbo.T_NouhinMeisai.YuuduuKakaku, dbo.T_NouhinMeisai.Sekiduke, 
                      dbo.T_NouhinMeisai.KanryouFlg, dbo.T_NouhinMeisai.ShiiresakiCode, dbo.T_NouhinMeisai.YYMM, dbo.T_NouhinMeisai.RowNo, 
                      dbo.T_NouhinMeisai.FreeKoumoku1, dbo.T_NouhinMeisai.FreeKoumoku2, dbo.T_NouhinMeisai.FreeKoumoku3, dbo.T_NouhinMeisai.Bikou, 
                      dbo.T_NouhinMeisai.LotNo, dbo.T_NouhinMeisai.Tani,dbo.T_NouhinHeader.SoushinFlg  
                      FROM dbo.T_NouhinHeader INNER JOIN 
                      dbo.T_NouhinMeisai ON dbo.T_NouhinHeader.HakkouNo = dbo.T_NouhinMeisai.HakkouNo AND 
                      dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = dbo.T_NouhinMeisai.ShiiresakiCode AND 
                      dbo.T_NouhinHeader.YYMM = dbo.T_NouhinMeisai.YYMM
                      WHERE (dbo.T_NouhinMeisai.ShiiresakiCode = @s) AND (dbo.T_NouhinMeisai.YYMM = @y) AND 
                      (CASE WHEN dbo.T_NouhinHeader.SeisanOrderNo <> '' THEN dbo.T_NouhinHeader.SeisanOrderNo WHEN dbo.T_NouhinHeader.HikitoriOrderNo <> '' THEN
                       dbo.T_NouhinHeader.HikitoriOrderNo WHEN dbo.T_NouhinHeader.OrderKanriNo <> '' THEN dbo.T_NouhinHeader.OrderKanriNo WHEN dbo.T_NouhinHeader.ZairyouOrderNo
                       <> '' THEN dbo.T_NouhinHeader.ZairyouOrderNo ELSE '' END = @o) AND (dbo.T_NouhinMeisai.Hinban + dbo.T_NouhinMeisai.Size = @h) AND 
                      (dbo.T_NouhinMeisai.LotNo = @l) AND ((dbo.T_NouhinMeisai.HakkouNo = @hn) OR 
                       (dbo.T_NouhinHeader.InvoiceNo IS NULL OR RTRIM(dbo.T_NouhinHeader.InvoiceNo) = '' OR
                       dbo.T_NouhinHeader.UnsouGyoushaCode IS NULL OR RTRIM(dbo.T_NouhinHeader.UnsouGyoushaCode) = '' OR
                       dbo.T_NouhinHeader.UnsouGyoushaMei IS NULL OR RTRIM(dbo.T_NouhinHeader.UnsouGyoushaMei) = '' OR
                       dbo.T_NouhinHeader.KogutiSu IS NULL OR RTRIM(dbo.T_NouhinHeader.KogutiSu) = ''))
                       Order By HakkouNo DESC";
            daGetNH.SelectCommand.Parameters.AddWithValue("@s", "");
            daGetNH.SelectCommand.Parameters.AddWithValue("@y", "");
            daGetNH.SelectCommand.Parameters.AddWithValue("@o", "");
            daGetNH.SelectCommand.Parameters.AddWithValue("@h", "");
            daGetNH.SelectCommand.Parameters.AddWithValue("@l", "");
            daGetNH.SelectCommand.Parameters.AddWithValue("@hn", "");
            daGetNH.SelectCommand.CommandTimeout = 600000;

            //---------------------------------------------------------------------------------------

            //-----------------------------------2016/06/10 M0156------------------------------------
            //納品明細データログ保存用
            MizunoDataSet.T_NouhinHeader_HistoryDataTable dtHistory = new MizunoDataSet.T_NouhinHeader_HistoryDataTable();
            //---------------------------------------------------------------------------------------

            //-----------------------------------2016/09/20 M0237------------------------------------
            //予算品データの明細を取得
            SqlDataAdapter daGetYosan = new SqlDataAdapter("", sqlConn);
            daGetYosan.SelectCommand.CommandText =
                @"SELECT DISTINCT NiukeJigyoushoCode FROM T_ProperOrder
                  WHERE SashizuBi = @sb AND ShiiresakiCode = @s AND (HikitoriOrderNo = @o OR SeisanOrderNo = @o) AND (Hinban + Size = @h)";
            daGetYosan.SelectCommand.Parameters.AddWithValue("@sb", "");
            daGetYosan.SelectCommand.Parameters.AddWithValue("@s", "");
            daGetYosan.SelectCommand.Parameters.AddWithValue("@o", "");
            daGetYosan.SelectCommand.Parameters.AddWithValue("@h", "");
            daGetYosan.SelectCommand.CommandTimeout = 600000;
            DataTable dtYosan = new DataTable();
            //---------------------------------------------------------------------------------------

            SqlTransaction t = null;
            int erri = 0;
            try
            {
                sqlConn.Open();
                t = sqlConn.BeginTransaction();
                setSoushinFlg.Transaction = t;

                // 2014/12/01 M0056
                daM.SelectCommand.Transaction = t;
                daM.UpdateCommand.Transaction = t;

                // 追加 20160610
                daGetNH.SelectCommand.Transaction = t;
                // 追加 20160920
                daGetYosan.SelectCommand.Transaction = t;

                StringBuilder sb = new StringBuilder();
                string sHakkouNoBefore = "";
                string sHachuNoBefore = "";
                string sNouhinmotoShiiresakiCodeBefore = "";
                int iGyouNo = 1;
                string strSQL = ""; //ログ用 20160610

                for (int i = 0; i < dt.Count; i++)
                {
                    erri = i;

                    if (dt[i].HachuNo == "FS-A-S-5545Y")
                        {
                            string b = "";
                        }

                        //行No
                        if (sHakkouNoBefore != dt[i]["HakkouNo"].ToString() ||
                            sHachuNoBefore != dt[i]["HachuNo"].ToString() ||
                            sNouhinmotoShiiresakiCodeBefore != dt[i]["NouhinmotoShiiresakiCode"].ToString()
                           )
                        {
                            iGyouNo = 1;
                        }


                        /// 2014/04/22 猪川様御指摘
                        /// ・送品区分"3"か"4"
                        /// ・仕入先が国内
                        /// ・配送業者名＋送り状No＋個口数(送り状情報)の入力が無い(送り状情報が全部入力されていれば出力する)
                        /// 上記条件を満たすデータは対象外にする
                        /// 
                        /// 2014/05/16 吉川様御依頼
                        /// 「出荷打ち止め」(明細行数量「0」 AND KanryouFlg = 9)の場合、連携の対象とする
                        
                        //下の処理でも利用できるようにdrHは外で定義しておく20160610
                        int iCnt = 0;
                        ViewDataset.VIEW_NouhinHeaderRow drH = NouhinDataClass.getVIEW_NouhinHeaderRow(int.Parse(dt[i]["HakkouNo"].ToString()),
                                                                    dt[i]["NouhinmotoShiiresakiCode"].ToString(),
                                                                    int.Parse(dt[i]["YYMM"].ToString()), sqlConn, ref iCnt, t);

                        if (dt[i]["NouhinmotoShiiresakiCode"].ToString().Substring(0, 2) != "29")
                        {
                            //** 国内の場合
                            if (drH != null)
                            {
                                bool Is3or4 = ((EnumSouhinKubun)int.Parse(drH.SouhinKubun) == EnumSouhinKubun.Chokusou || (EnumSouhinKubun)int.Parse(drH.SouhinKubun) == EnumSouhinKubun.Nituu_GMS);
                                bool IsFullInput =
                                            ((!drH.IsInvoiceNoNull() && drH.InvoiceNo.Trim() != "")
                                            &&
                                            ((!drH.IsUnsouGyoushaCodeNull() && drH.UnsouGyoushaCode.Trim() != "") || (!drH.IsUnsouGyoushaMeiNull() && drH.UnsouGyoushaMei.Trim() != ""))
                                            &&
                                            (!drH.IsKogutiSuNull() && drH.KogutiSu.Trim() != ""));

                                bool IsClosedNH = ClosedNouhin(drH.HakkouNo, drH.NouhinmotoShiiresakiCode, drH.YYMM, sqlConn, t);


                                if (Is3or4 && !IsFullInput && !IsClosedNH)
                                {
                                    continue;
                                }
                                else
                                { 
                                    
                                }
                            }
                        }

                        //for (int j = 0; j < dt.Columns.Count-1; j++)
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Columns[j].ColumnName == "YYMM" ||
                                dt.Columns[j].ColumnName == "ShukkaBi" ||
                                dt.Columns[j].ColumnName == "SoushinFlg" ||
                                dt.Columns[j].ColumnName == "Edaban" ||
                                dt.Columns[j].ColumnName == "Size" ||
                                dt.Columns[j].ColumnName == "SeiZaiFlg" ||
                                dt.Columns[j].ColumnName == "Tani" ||
                                dt.Columns[j].ColumnName == "T_Becchu_TorikeshimaeOrderKanriNo" ||
                                dt.Columns[j].ColumnName == "T_Becchu2_TorikeshimaeOrderKanriNo"
                                )
                            {
                                continue;
                            }


                            if (dt[i].IsNull(dt.Columns[j].ColumnName))
                            {
                                sb.Append("");
                            }
                            else
                            {
                                //納品数
                                if (dt.Columns[j].ColumnName == "NouhinSuu")
                                {
                                    sb.Append(dt[i][j].ToString().Replace(".00", ""));
                                    sb.Append(",");
                                    continue;
                                }

                                //品目番号
                                if (dt.Columns[j].ColumnName == "HinmokuBangou")
                                {
                                    sb.Append(dt[i][j].ToString().Replace(" ", ""));
                                    sb.Append(",");
                                    continue;
                                }

                                //発行日付
                                if (dt.Columns[j].ColumnName == "HakkouHiduke")
                                {
                                    // 中身は[ShukkaHiduke]を取得している
                                    sb.Append("20" + dt[i][j - 5].ToString());
                                    sb.Append(",");
                                    continue;
                                }

                                //行No
                                //岡村修正 2013-05-07 
                                //齊藤追加 2013-01-09
                                if (dt.Columns[j].ColumnName == "GyouNo")
                                {
                                    //sb.Append(int.Parse(dt[i][j].ToString()) + (int.Parse(dt[i][8].ToString()) * 8));
                                    sb.Append(iGyouNo);
                                    sb.Append(",");
                                    continue;
                                }
                                //-------------------

                                //出荷日付
                                if (dt.Columns[j].ColumnName == "ShukkaHiduke")
                                {
                                    //----齊藤　発行日付を代入
                                    //sb.Append("20" + dt[i][j+5].ToString());
                                    sb.Append(",");
                                    continue;
                                }

                                //連携システムフラグ
                                // 2013.12.14 
                                if (j == 13)
                                {
                                    sb.Append(dt[i]["RenkeiSystemFlag"].ToString());
                                    sb.Append(",");
                                    continue;
                                }

                                // 完了フラグ
                                // 2014/11/28 ミズノ吉川様より
                                // 完了フラグ付き方のロジック変更の依頼あり。

                                
                                if (dt.Columns[j].ColumnName == "KanryouFlg")
                                {
                                    string KanryouFlg = dt[i]["KanryouFlg"].ToString();

                                    if ((dt[i]["KanryouFlg"].ToString() == "9" || dt[i]["KanryouFlg"].ToString() == "8"))
                                    {
                                        dtM.Clear();

                                        string s = dt[i]["NouhinmotoShiiresakiCode"].ToString();
                                        string y = dt[i]["YYMM"].ToString();
                                        string o = dt[i]["HachuNo"].ToString();
                                        string h = dt[i]["HinmokuBangou"].ToString();
                                        string l = dt[i]["LotNo"].ToString();
                                        string hn = dt[i]["HakkouNo"].ToString();

                                        daGetNH.SelectCommand.Parameters["@s"].Value = s;
                                        daGetNH.SelectCommand.Parameters["@y"].Value = y;
                                        daGetNH.SelectCommand.Parameters["@o"].Value = o;
                                        daGetNH.SelectCommand.Parameters["@h"].Value = h;
                                        daGetNH.SelectCommand.Parameters["@l"].Value = l;
                                        daGetNH.SelectCommand.Parameters["@hn"].Value = hn;

                                        daGetNH.Fill(dtM);

                                        //追加 予算品かつ事業所選択されていない状況なら完了フラグは8に設定 20160920
                                        if (drH.IsShukkaNiukeBumonNull())
                                        {
                                            dtYosan.Clear();

                                            daGetYosan.SelectCommand.Parameters["@sb"].Value = drH.SashizuBi;
                                            daGetYosan.SelectCommand.Parameters["@s"].Value = s;
                                            daGetYosan.SelectCommand.Parameters["@o"].Value = drH.OrderKanriNo;
                                            daGetYosan.SelectCommand.Parameters["@h"].Value = h;

                                            daGetYosan.Fill(dtYosan);

                                            if (dtYosan.Rows.Count > 1)
                                            {
                                                KanryouFlg = "8";
                                            }
                                        }

                                        DataView dvM = dtM.DefaultView;

                                        // 未連携分のデータに絞り込む
                                        dvM.RowFilter = string.Format("KanryouFlg = '0' AND SoushinFlg IN ('{0}','{1}') ", SoushinFlg.NONE, SoushinFlg.MIKAKUTEI);
                                        dvM.Sort = "HakkouNo DESC";

                                        // 未連携分が存在する場合、完了フラグの付け替えを行う
                                        if (dvM.Count > 0)
                                        {
                                            daM.SelectCommand.Parameters.AddWithValue("@h", dvM[0]["HakkouNo"].ToString());
                                            daM.SelectCommand.Parameters.AddWithValue("@s", dvM[0]["ShiiresakiCode"].ToString());
                                            daM.SelectCommand.Parameters.AddWithValue("@y", dvM[0]["YYMM"].ToString());
                                            string MHakkouNo = dvM[0]["HakkouNo"].ToString();

                                            for (int cnt = 0; dtM.Rows.Count > cnt; cnt++)
                                            {
                                                //dvM.RowFilter = "KanryouFlg = '0'";
                                                if (dvM.Count > 0 && MHakkouNo == dtM.Rows[cnt]["HakkouNo"].ToString())
                                                    dtM.Rows[cnt]["KanryouFlg"] = KanryouFlg;

                                                if (dt[i]["HakkouNo"].ToString() == dtM.Rows[cnt]["HakkouNo"].ToString())
                                                {
                                                    dtM.Rows[cnt]["KanryouFlg"] = "0";
                                                    KanryouFlg = "0";
                                                }

                                            }

                                            daM.Update(dtM);
                                        }

                                        // 他事業所向けの納品が完了しているか これを利用している箇所がないのでコメントアウト 20160610
                                        //dvM.RowFilter = string.Format("KanryouFlg = '9'", SoushinFlg.NONE, SoushinFlg.MIKAKUTEI);
                                        //dvM.Sort = "HakkouNo DESC";

                                    }

                                    sb.Append(KanryouFlg);
                                    sb.Append(",");
                                    continue;
                                }

                                //送品区分
                                if (j == 15)
                                {
                                    //if (dt[i][j].ToString() == "4")
                                    //{
                                    //    sb.Append("Y");

                                    //}
                                    //else
                                    //{
                                    sb.Append(dt[i].Tani);
                                    //}
                                    sb.Append(",");
                                    sb.Append(dt[i][j].ToString());
                                    sb.Append(",");
                                    continue;
                                }

                                //送信フラグ
                                if (j == 16)
                                {
                                    sb.Append(dt[i][j - 1].ToString());
                                    sb.Append(",");
                                    continue;
                                }

                                //緊急直送備考
                                if (dt.Columns[j].ColumnName == "KinkyuChokusouBikou")
                                {
                                    //2013/11/08 岡村 追加
                                    if (dt[i].IsNull("KinkyuChokusouBikou") == false)
                                    {
                                        sb.Append(dt[i]["KinkyuChokusouBikou"].ToString());
                                    }

                                    /// 2014/05/13 他の取得形式と合わせる為、コメントアウト解除
                                    sb.Append(",");
                                    continue;
                                }

                                //2014/02/17 岡村 追加
                                if (dt.Columns[j].ColumnName == "InvoiceNo")
                                {

                                    if (dt[i]["NouhinmotoShiiresakiCode"].ToString().Substring(0, 2) == "29")
                                    {
                                        //** 仕入先が海外の場合
                                        if (dt[i].IsNull("InvoiceNo") == false)
                                        {
                                            sb.Append(dt[i]["InvoiceNo"].ToString());
                                        }
                                        else
                                        {
                                            sb.Append("");
                                        }
                                    }
                                    else
                                    {
                                        //** 国内の場合
                                        //int iCnt = 0;
                                        //上で処理しているのでここでは不要 20160610
                                        //ViewDataset.VIEW_NouhinHeaderRow drH = NouhinDataClass.getVIEW_NouhinHeaderRow(int.Parse(dt[i]["HakkouNo"].ToString()),
                                        //                                        dt[i]["NouhinmotoShiiresakiCode"].ToString(),
                                        //                                        int.Parse(dt[i]["YYMM"].ToString()), sqlConn, ref iCnt, t);

                                        if (!drH.IsUnsouGyoushaCodeNull())
                                        {
                                            if (drH.UnsouGyoushaCode == "ZZ" || drH.UnsouGyoushaCode == "")
                                            {
                                                //運送業者コードなし
                                                //NULLの場合が有る
                                                if (!drH.IsUnsouGyoushaMeiNull() && !drH.IsInvoiceNoNull())
                                                {
                                                    if (drH.UnsouGyoushaMei.Length > 6)
                                                    {
                                                        sb.Append(drH.UnsouGyoushaMei.Substring(0, 6) + drH.InvoiceNo);
                                                    }
                                                    else
                                                    {
                                                        sb.Append(drH.UnsouGyoushaMei + drH.InvoiceNo);
                                                    }
                                                }
                                                else
                                                {
                                                    sb.Append("");
                                                }
                                            }
                                            else
                                            {
                                                //運送業者コードあり
                                                sb.Append(drH.UnsouGyoushaCode + ((!drH.IsInvoiceNoNull()) ? drH.InvoiceNo : ""));
                                            }
                                        }
                                        else
                                        {
                                            sb.Append("");
                                        }

                                        //if (dt[i].IsNull("InvoiceNo") == false)
                                        //{
                                        //    sb.Append(dt[i]["InvoiceNo"].ToString());
                                        //}
                                    }

                                    sb.Append(",");

                                    continue;
                                }

                                //else
                                //{
                                sb.Append(dt[i][j].ToString());
                                //}

                            }

                            if (j < 15)
                            {
                                sb.Append(",");
                            }
                            else
                            {
                            }
                        }


                        //2014/02/17 岡村 追加 取消前オーダー管理No
                        /// 2014/05/13 他の取得形式と合わせる為、「sb.Append(",");」をコメントアウト
                        if (dt[i].IsNull("T_Becchu_TorikeshimaeOrderKanriNo") == false)
                        {
                            //sb.Append(",");
                            sb.Append(dt[i]["T_Becchu_TorikeshimaeOrderKanriNo"]);
                        }
                        else if (dt[i].IsNull("T_Becchu2_TorikeshimaeOrderKanriNo") == false)
                        {
                            //sb.Append(",");
                            sb.Append(dt[i]["T_Becchu2_TorikeshimaeOrderKanriNo"]);
                        }
                        else
                        {
                            //sb.Append(",");
                        }

                        //既に一度出力されているかどうか確認
                        bool bShuturyokuBi = true;
                        if(drH != null) 
                            if (drH.IsShuturyokuBiNull()) bShuturyokuBi = false;

                        sb.Append("\r\n");
                        setSoushinFlg.Parameters["@s"].Value = dt[i].NouhinmotoShiiresakiCode;
                        setSoushinFlg.Parameters["@y"].Value = dt[i].YYMM;
                        setSoushinFlg.Parameters["@h"].Value = dt[i].HakkouNo;
                        setSoushinFlg.ExecuteNonQuery();

                        //一括更新に変更するのでコメントアウト 20160610
                        /// 2014/05/22 重複出力監視用テーブルに保存
                        //if (bSetSoushinFlg)
                        //{ ImportT_NouhinHeader_History(dt[i].NouhinmotoShiiresakiCode, dt[i].YYMM, dt[i].HakkouNo, iGyouNo, sFlag, sqlConn, t); }

                        //一括更新用SQL作成 20160610
                        if (bSetSoushinFlg)
                        {
                            //strSQL += string.Format("INSERT INTO T_NouhinHeader_History select *, GETDATE() , (CASE WHEN (SELECT COUNT(*) FROM T_NouhinHeader_History WHERE (NouhinmotoShiiresakiCode = '{0}') and (YYMM = '{1}') and (HakkouNo = '{2}') AND (ThisSoushinFlg = '{4}')) > 0 THEN  1 ELSE 0 END), '{3}', '{4}' from T_NouhinHeader where  (NouhinmotoShiiresakiCode='{0}') and (YYMM='{1}') and (HakkouNo='{2}') \n "
                            //    , dt[i].NouhinmotoShiiresakiCode    //仕入先コード
                            //    , dt[i].YYMM                        //YYMM
                            //    , dt[i].HakkouNo                    //発行No
                            //    , iGyouNo                           //行No
                            //    , sFlag);                           //出荷確定か未送信かの判定フラグ

                            //納品ヘッダー情報を取得
                            MizunoDataSet.T_NouhinHeaderRow drNouhinHeader = NouhinDataClass.getT_NouhinHeaderRow(dt[i].NouhinmotoShiiresakiCode, dt[i].YYMM, dt[i].HakkouNo, t);
                            if (drNouhinHeader == null) throw new Exception("ヘッダー情報がありません");

                            MizunoDataSet.T_NouhinHeader_HistoryRow drHistory = dtHistory.NewT_NouhinHeader_HistoryRow();
                            drHistory.ItemArray = drNouhinHeader.ItemArray;
                            drHistory.InsertDate = DateTime.Now;
                            //存在確認を行う ある場合はtrue無い場合はfalse 今まで明細が２以上だったら再出力としてみなしてたけど…それはおかしくない？
                            drHistory.SaisyutsuRyoku = bShuturyokuBi;
                            drHistory.RowNo = iGyouNo;
                            drHistory.ThisSoushinFlg = (byte)sFlag;
                            dtHistory.Rows.Add(drHistory);
                        }

                        //発行No 保持
                        sHakkouNoBefore = dt[i]["HakkouNo"].ToString();
                        sHachuNoBefore = dt[i]["HachuNo"].ToString();
                        sNouhinmotoShiiresakiCodeBefore = dt[i]["NouhinmotoShiiresakiCode"].ToString();

                        iGyouNo++;
                }

                // 重複出力監視用テール部に一括保存 20160610
                if (dtHistory.Count > 0)
                {
                    ImportT_NouhinHeader_HistoryNew(dtHistory, t);
                }

#if DEBUG
                //throw new Exception("");
#endif
                //System.IO.StreamWriter sw = new StreamWriter(strFilePath.Replace("\\\\","\\"), true, System.Text.Encoding.GetEncoding(932));
                System.IO.StreamWriter sw = new StreamWriter(strFilePath, true, System.Text.Encoding.GetEncoding(932));
                strData = sb.ToString();


                if (bSetSoushinFlg)
                sw.Write(strData);

                if (sw != null)
                {
                    sw.Close();
                }

                if (bSetSoushinFlg)
                {
                    t.Commit();


                    /// 重複チェック
                    try
                    {
                        CyoufukuCheck(sFlag, sqlConn, t);
                    }
                    catch (Exception chk)
                    {
                        strData = "";
                        strData = chk.Message;
                     }
                }

            }
            catch (Exception ex)
            {
                t.Rollback();
                return new Core.Error(ex);
            }
            finally
            {
                if (sqlConn != null) { sqlConn.Close(); }
            }

            return null;
        }


        /// <summary>
        /// 2014/05 抽出NHデータの重複チェック
        /// </summary>
        /// <param name="strShiiresakiCode"></param>
        /// <param name="nYYMM"></param>
        /// <param name="nHakkouNo"></param>
        /// <param name="sqlConn"></param>
        /// <param name="t"></param>
        private static void ImportT_NouhinHeader_History
            (string strShiiresakiCode, int nYYMM, int nHakkouNo, int nRowNo, byte? sFlag, SqlConnection sqlConn, SqlTransaction t)
        {
            SqlCommand cmd = new SqlCommand("", sqlConn);
            cmd.CommandText = @"INSERT INTO T_NouhinHeader_History
                                          select *, 
                                                 GETDATE() ,
                                                 (CASE WHEN (SELECT COUNT(*)
			                                                    FROM T_NouhinHeader_History
			                                                    WHERE (NouhinmotoShiiresakiCode = @s) 
                                                                  and (YYMM = @y) 
                                                                  and (HakkouNo = @h) 
                                                                  AND (ThisSoushinFlg = @f)) > 0 THEN  1
                                                    ELSE 0
                                                    END), 
                                                 @r,
                                                 @f
                                          from T_NouhinHeader
                                          where  (NouhinmotoShiiresakiCode=@s) and (YYMM=@y) and (HakkouNo=@h) ";
            cmd.Parameters.AddWithValue("@s", strShiiresakiCode);
            cmd.Parameters.AddWithValue("@y", nYYMM);
            cmd.Parameters.AddWithValue("@h", nHakkouNo);
            cmd.Parameters.AddWithValue("@r", nRowNo);
            cmd.Parameters.AddWithValue("@f", sFlag);

            if (t != null)
            {
                cmd.Transaction = t;
            }
            int nRirekiID = Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>
        /// 2016/06/10 抽出NHデータの重複チェック　パフォーマンス改善版
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="t"></param>
        private static void ImportT_NouhinHeader_HistoryNew
            (MizunoDataSet.T_NouhinHeader_HistoryDataTable dtHistory, SqlTransaction t)
        {
            SqlDataAdapter da = new SqlDataAdapter("", t.Connection);
            da.SelectCommand.CommandText = "SELECT * FROM T_NouhinHeader_History";
            da.SelectCommand.Transaction = t;
            da.InsertCommand = (new SqlCommandBuilder(da)).GetInsertCommand();
            da.InsertCommand.Transaction = t;
            da.InsertCommand.CommandTimeout = 600000;

            da.Update(dtHistory);
        }

        private static void CyoufukuCheck(byte? sFlag, SqlConnection sqlConn, SqlTransaction t)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"SELECT HakkouNo, NouhinmotoShiiresakiCode, OrderKanriNo
                                             FROM   T_NouhinHeader_History
                                             WHERE  (RowNo = 1) AND (SaisyutsuRyoku = 1) AND (ThisSoushinFlg = @f)
                                             GROUP BY HakkouNo, NouhinmotoShiiresakiCode, YYMM, OrderKanriNo ";
            da.SelectCommand.Parameters.AddWithValue("@f", sFlag);
            da.SelectCommand.CommandTimeout = 600000;
            if (t != null)
            {
                da.SelectCommand.Transaction = t;
            }
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append(string.Format("納品データ重複：{0}_{1}_{2}_{3}", dt.Rows[i][0], dt.Rows[i][1], dt.Rows[i][2], sFlag));
                    sb.Append("\r\n");
                }

                throw new Exception(sb.ToString());
            }
        }


        // ★納品処理
        /// <summary>
        /// 納品データ作成
        /// 【仕入先N:[ヘッダー(1行)]【伝票N:[ヘッダー(1行)]【明細:[ヘッダー(1行)]【明細オプション(N行)】】[トレーラー(1行)]】】
        /// </summary>
        /// <param name="dtDataHiduke"></param>
        /// <param name="ShukkaBi"></param>
        /// <param name="lstShiteiShiiresakiCode"></param>
        /// <param name="bMiSoushinOnly">
        /// true:指定出荷日”以前”の未送信分で作成
        /// false:指定出荷日のみのデータで作成
        /// </param>
        /// <param name="bSetSoushinFlg"></param>
        /// <param name="strFilePath"></param>
        /// <param name="nLimitT_NouhinHeaderCount">出力対象となるヘッダーの件数の制限</param>
        /// <param name="sqlConn"></param>
        /// <param name="strData"></param>
        /// <returns></returns>
        /// <remarks>
        /// ・データが無いとき、空のファイルを作成する。
        /// </remarks>
        public static Core.Error CreateNouhinData_OLD(
            DateTime dtDataHiduke, 
            Core.Type.NengappiKikan ShukkaBi, 
            List<string> lstShiteiShiiresakiCode,
            bool bMiSoushinOnly, bool bSetSoushinFlg, string strFilePath, int nLimitT_NouhinHeaderCount, SqlConnection sqlConn, out string strData)
        {
            strData = null;


            SqlDataAdapter daT_NouhinHeader = new SqlDataAdapter("", sqlConn);
            daT_NouhinHeader.SelectCommand.CommandText = "SELECT * FROM T_NouhinHeader ";

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
            if (null != ShukkaBi) {
                w.Add(ShukkaBi.GenerateSQLAsDateTime("ShukkaBi"));
            }
            if (bMiSoushinOnly) {
                w.Add("SoushinFlg=0");
            }
            if (null != lstShiteiShiiresakiCode && 0 < lstShiteiShiiresakiCode.Count) { 
                w.Add(string.Format("NouhinmotoShiiresakiCode in ('{0}')", string.Join("','", lstShiteiShiiresakiCode.ToArray())));
            }

            if (!string.IsNullOrEmpty(w.WhereText))
                daT_NouhinHeader.SelectCommand.CommandText += " where " + w.WhereText;
 


            MizunoDataSet.T_NouhinHeaderDataTable dtHeader = new MizunoDataSet.T_NouhinHeaderDataTable();

            SqlCommand setSoushinFlg = new SqlCommand("", sqlConn);
            setSoushinFlg.CommandText = "update T_NouhinHeader set SoushinFlg=1, ShuturyokuBi=@ShuturyokuBi where HakkouNo=@h and NouhinmotoShiiresakiCode=@s and YYMM=@y";
            setSoushinFlg.Parameters.AddWithValue("@s", "");
            setSoushinFlg.Parameters.AddWithValue("@y", 0);
            setSoushinFlg.Parameters.AddWithValue("@h", 0);
            setSoushinFlg.Parameters.AddWithValue("@ShuturyokuBi", DateTime.Now);

            // 明細レコード
            SqlDataAdapter daMeisai = new SqlDataAdapter("", sqlConn);
            daMeisai.SelectCommand.CommandText = "SELECT * FROM T_NouhinMeisai WHERE (ShiiresakiCode = @s) AND (YYMM = @y) AND (HakkouNo = @h)";
            daMeisai.SelectCommand.Parameters.AddWithValue("@s", "");
            daMeisai.SelectCommand.Parameters.AddWithValue("@y", 0);
            daMeisai.SelectCommand.Parameters.AddWithValue("@h", 0);

            MizunoDataSet.T_NouhinMeisaiDataTable dtMeisai = new MizunoDataSet.T_NouhinMeisaiDataTable();

            // 明細オプションレコード
            SqlDataAdapter daMeisaiOption = new SqlDataAdapter("", sqlConn);
            daMeisaiOption.SelectCommand.CommandText = @"
SELECT                  RecodeKubun, DataKubun, HakkouNo, Yobi1, Edaban, GyouNo, JanCode2, NouhinSuu2, Size2, Meisai2, NouhinKubun2, JanCode3, NouhinSuu3, Size3, 
Meisai3, NouhinKubun3, JanCode4, NouhinSuu4, Size4, Meisai4, NouhinKubun4, Sekiduke2, Sekiduke3, Sekiduke4, Yobi2, KanryouFlg2, KanryouFlg3, 
KanryouFlg4, HinbanTsuikaFlg2, HinbanTsuikaFlg3, HinbanTsuikaFlg4
FROM                     dbo.T_NouhinMeisaiOption
WHERE                   (HakkouNo = @h) AND (Edaban = @e) AND (GyouNo = @g) AND (ShiiresakiCode = @s) AND (YYMM = @y)";

            daMeisaiOption.SelectCommand.Parameters.AddWithValue("@s", "");
            daMeisaiOption.SelectCommand.Parameters.AddWithValue("@y", 0);
            daMeisaiOption.SelectCommand.Parameters.AddWithValue("@h", 0);
            daMeisaiOption.SelectCommand.Parameters.AddWithValue("@e", 0);
            daMeisaiOption.SelectCommand.Parameters.AddWithValue("@g", 0);
            DataTable dtMeisaiOption = new DataTable();


            // トレーラレコード取得
            SqlDataAdapter daTrailer = new SqlDataAdapter("", sqlConn);
            daTrailer.SelectCommand.CommandText = @"
SELECT                  RecodeKubun, DataKubun, HakkouNo, Yobi1, Edaban, GyouNo, EigyouTantoushaCode, TeamMei, TenMei, SKTantoushaCode, UnsouHouhou, Kosuu, 
UnchinKubun, Shogakari, UnChin, ShinaDai, ShouhiZei, NouhinShoHiduke, Yobi2
FROM                     dbo.T_NouhinTrailer
WHERE                   (HakkouNo = @h) AND (ShiiresakiCode = @s) AND (YYMM = @y)";

            daTrailer.SelectCommand.Parameters.AddWithValue("@s", "");
            daTrailer.SelectCommand.Parameters.AddWithValue("@y", 0);
            daTrailer.SelectCommand.Parameters.AddWithValue("@h", 0);
            DataTable dtTrailer = new DataTable();


            SqlTransaction t = null;
            System.IO.StreamWriter file = null;

            string[] strHeaderColumns = new string[]
            {
                "RecodeKubun",
"DataKubun",
"HakkouNo",
"Yobi1",
"Edaban",
"GyouNo",
"DenpyouKubun",
"TorihikisakiKaishaCode",
"TorihikisakiBusho",
"NouhinmotoShiiresakiCode",
"NouhinmotoYobi",
"HanbaitenKaishaCode",
"HanbaitenBusho",
"NouhinKigou",
"NiukeBasho",
"ShukkasakiYobi",
"NiukeBumon",
"HanbaitenKigyouRyakuMei",
"HanbaitenBushoRyakuMei",
"ShukkaSakiKigyouRyakuMei",
"ShukkaSakiBushoRyakuMei",
"HakkouHiduke",
"ShukkaHiduke",
"HacchuNo",
"HacchuKubun",
"R1",
"R2",
"R3",
"R4",
"SouhinKubun"
            };
            int[] nHeaderColumnLength = new int[]{
            1,
2,
5,
3,
2,
1,
3,
6,
4,
7,
3,
6,
4,
3,
2,
1,
4,
10,
10,
10,
10,
6,
6,
10,
4,
1,
1,
1,
1,
1            
            };

            
            // 明細23項目
            string[] strMeisaiColumns = new string[]{
            "RecodeKubun","DataKubun","HakkouNo","Yobi1","Edaban","GyouNo","ShouhinBunrui","ShouhinCode","Hinban","HinbanTsuikaFlg","Filler",
            "TorihikiTanka","Joudai","BrandRyakuMei","ShouhinRyakuMei","JanCode","NouhinSuu","Size","Meisai","NouhinKubun","YuuduuKakaku","Sekiduke","KanryouFlg"
            };
            int[] nMeisaiColumnLength = new int[]{
            1,
            2,
            5,
            3,
            2,
            1,
            4,
            13,
            10,
            1,
            7,
            7,
            7,
            10,
            15,
            13,
            5,
            5,
            4,
            1,
            7,
            4,
            1
            };

            System.Text.StringBuilder sbHeader = new StringBuilder();
            System.Text.StringBuilder sbMeisai = new StringBuilder();
            bool bExistFile = File.Exists(strFilePath);

            try
            {
                if (0 < nLimitT_NouhinHeaderCount)
                {
                    int nCount = 0;
                    Core.Error ret = Core.Sql.MiscClass.GetRecordCount(daT_NouhinHeader.SelectCommand, sqlConn, ref nCount);
                    if (null != ret) return ret;
                    if (nLimitT_NouhinHeaderCount <= nCount) {
                        return new Error(string.Format("該当の伝票の{0:N0}件あります。{1:N0}件を超えるデータは出力できません。", nCount, nLimitT_NouhinHeaderCount));
                    }
                }

                sqlConn.Open();
                t = sqlConn.BeginTransaction();
                daT_NouhinHeader.SelectCommand.Transaction = t;
                setSoushinFlg.Transaction = t;
                daMeisai.SelectCommand.Transaction = t;
                daMeisaiOption.SelectCommand.Transaction = t;
                daTrailer.SelectCommand.Transaction = t;

                List<string> lstShiiresakiCode = new List<string>();
                daT_NouhinHeader.Fill(dtHeader);
                // 事前に仕入先コードを列挙する(大した処理でない)
                dtHeader.DefaultView.Sort = "NouhinmotoShiiresakiCode";
                for (int i = 0; i < dtHeader.DefaultView.Count; i++)
                {
                    MizunoDataSet.T_NouhinHeaderRow dr = dtHeader.DefaultView[i].Row as MizunoDataSet.T_NouhinHeaderRow;
                    // 全角で仕入先コードの登録があるので半角へ変換する
                    dr.NouhinmotoShiiresakiCode = Microsoft.VisualBasic.Strings.StrConv(dr.NouhinmotoShiiresakiCode, Microsoft.VisualBasic.VbStrConv.Narrow, 0x0411);
                    if (!lstShiiresakiCode.Contains(dr.NouhinmotoShiiresakiCode))
                        lstShiiresakiCode.Add(dr.NouhinmotoShiiresakiCode);
                }

                dtHeader.DefaultView.Sort = "YYMM, HakkouNo";

                List<ShiiresakiNouhinData> NouhinData = new List<ShiiresakiNouhinData>();


                // 仕入先単位で出荷データを生成する
                for (int i = 0; i < lstShiiresakiCode.Count; i++)
                {
                    string strShiiresakiCode = lstShiiresakiCode[i];

                    if (null != lstShiteiShiiresakiCode && !lstShiteiShiiresakiCode.Contains(strShiiresakiCode)) 
                    {
                        // 指定の仕入先のみ取得
                        continue;
                    }

                    dtHeader.DefaultView.RowFilter = string.Format("NouhinmotoShiiresakiCode='{0}'", strShiiresakiCode);

                    ShiiresakiNouhinData data = new ShiiresakiNouhinData(strShiiresakiCode, dtDataHiduke);
                    NouhinData.Add(data);

                    // 伝票単位でデータ生成
                    for (int n = 0; n < dtHeader.DefaultView.Count; n++)
                    {
                        MizunoDataSet.T_NouhinHeaderRow drHeader = dtHeader.DefaultView[n].Row as MizunoDataSet.T_NouhinHeaderRow;
                        int nYYMM = Convert.ToInt32(drHeader["YYMM"]);
                        DenpyouData den = new DenpyouData();
                        data.DenpyouDatas.Add(den);

                        // ----- 伝票ヘッダー情報 -----
                        sbHeader.Length = 0;
                        if (drHeader.SouhinKubun == "6")
                        {
                            for (int p = 0; p < strHeaderColumns.Length; p++)
                            {
                                string strColumnName = strHeaderColumns[p];

                                switch (strColumnName.ToUpper()) 
                                {
                                    case "SHUKKAHIDUKE":
                                        // 出荷日付に本納品月をセット
                                        try
                                        {
                                            if (drHeader.HonNouhinTsuki.Length == 6)
                                            {
                                                // yyyyMMをyyMM01にしてセットする。(例：201002→100201)                                        
                                                sbHeader.Append(drHeader.HonNouhinTsuki.Substring(2, 4) + "01");
                                            }
                                            else
                                            {
                                                // エラーメール送信
                                                //SendMail("本納品月が6文字以外のデータが存在しています。\r\n確認して下さい。");
                                                sbHeader.Append("000000");
                                            }
                                        }
                                        catch
                                        {
                                            sbHeader.Append("000000");
                                        }
                                        break;
                                    default:
                                        {
                                            sbHeader.Append(string.Format("{0, -" + nHeaderColumnLength[p].ToString() + "}",
                                                Convert.ToString(drHeader[strColumnName]).TrimEnd()));
                                            break;
                                        }

                                }


                            }
                        }
                        else {
                            for (int p = 0; p < strHeaderColumns.Length; p++)
                            {
                                string strColumnName = strHeaderColumns[p];
                                sbHeader.Append(string.Format("{0, -" + nHeaderColumnLength[p].ToString() + "}",
                                    Convert.ToString(drHeader[strColumnName]).TrimEnd()));
                            }
                        }
                        den.HeaderData = sbHeader.ToString();

                        // ----- 明細 -----
                        daMeisai.SelectCommand.Parameters["@s"].Value = strShiiresakiCode;
                        daMeisai.SelectCommand.Parameters["@y"].Value = nYYMM;
                        daMeisai.SelectCommand.Parameters["@h"].Value = drHeader.HakkouNo;
                        dtMeisai.Clear();
                        daMeisai.Fill(dtMeisai);
                        if (0 == dtMeisai.Count)
                            throw new Exception(string.Format("明細データ (仕入先コード={0},YYMM={1},発行No={2})",
                                drHeader.NouhinmotoShiiresakiCode, nYYMM, drHeader.HakkouNo));

                        // 必ずこの順番 10-10-25　伝票と納品データの並び順を同じにする
                        dtMeisai.DefaultView.Sort = "Edaban, GyouNo";

                        for (int m = 0; m < dtMeisai.DefaultView.Count; m++)
                        {
                            MizunoDataSet.T_NouhinMeisaiRow drMeisai = dtMeisai.DefaultView[m].Row as MizunoDataSet.T_NouhinMeisaiRow;

                            drMeisai.ShouhinRyakuMei = string.Format("{0, -15}", drMeisai.ShouhinRyakuMei.Trim());

                            MeisaiData md = new MeisaiData();
                            den.MeisaiDatas.Add(md);
                            md.Data = "";
                            sbMeisai.Length = 0;
                            for (int p = 0; p < strMeisaiColumns.Length; p++)
                            {
                                string strColumnName = strMeisaiColumns[p];
                                switch (strColumnName) { 
                                    case "Edaban":
                                        sbMeisai.Append(string.Format("{0:00}", drMeisai.Edaban));
                                        break;
                                    case "NouhinSuu":
                                        sbMeisai.Append(string.Format("{0:00000}", drMeisai.NouhinSuu));    // 5桁0埋め
                                        break;
                                    default:
                                        {
                                            sbMeisai.Append(string.Format("{0, -" + nMeisaiColumnLength[p].ToString() + "}",
                                                Convert.ToString(drMeisai[strMeisaiColumns[p]]).TrimEnd()));
                                        }
                                        break;
                                }

                            }
                            md.Data = sbMeisai.ToString();

                            // ----- 明細オプション(レコードがない場合もある) -----
                            daMeisaiOption.SelectCommand.Parameters["@s"].Value = strShiiresakiCode;
                            daMeisaiOption.SelectCommand.Parameters["@y"].Value = nYYMM;
                            daMeisaiOption.SelectCommand.Parameters["@h"].Value = drHeader.HakkouNo;
                            daMeisaiOption.SelectCommand.Parameters["@e"].Value = drMeisai.Edaban;
                            daMeisaiOption.SelectCommand.Parameters["@g"].Value = drMeisai.GyouNo;
                            dtMeisaiOption.Clear();
                            daMeisaiOption.Fill(dtMeisaiOption);
                            for (int o = 0; o < dtMeisaiOption.Rows.Count; o++)
                            {
                                string strMeisaiOption = "";
                                DataRow drOp = dtMeisaiOption.Rows[o];
                                for (int p = 0; p < drMeisai.Table.Columns.Count; p++)
                                    strMeisaiOption += Convert.ToString(drOp[p]);
                                md.MeisaiOptionDatas.Add(strMeisaiOption);
                            }
                        }


                        // ----- トレーラー(1行だけ) -----
                        daTrailer.SelectCommand.Parameters["@s"].Value = strShiiresakiCode;
                        daTrailer.SelectCommand.Parameters["@y"].Value = nYYMM;
                        daTrailer.SelectCommand.Parameters["@h"].Value = drHeader.HakkouNo;
                        dtTrailer.Clear();
                        daTrailer.Fill(dtTrailer);
                        if (0 == dtTrailer.Rows.Count)
                        {
                            throw new Exception(string.Format("トレーラーレコードなし (仕入先コード={0},YYMM={1},発行No={2})",
                              drHeader.NouhinmotoShiiresakiCode, nYYMM, drHeader.HakkouNo));
                        }
                        den.Trailer = "";
                        for (int p = 0; p < dtTrailer.Columns.Count; p++)
                            den.Trailer += Convert.ToString(dtTrailer.Rows[0][p]);


                        // 送信済フラグ設定
                        if (bSetSoushinFlg)
                        {
                            setSoushinFlg.Parameters["@s"].Value = strShiiresakiCode;
                            setSoushinFlg.Parameters["@y"].Value = nYYMM;
                            setSoushinFlg.Parameters["@h"].Value = drHeader.HakkouNo;
                            setSoushinFlg.ExecuteNonQuery();
                        }
                    }

                }



                // ★前回のファイルに追記すること!!!!(append= true)
                System.IO.StringWriter sw = new System.IO.StringWriter();
                for (int i = 0; i < NouhinData.Count; i++)
                {
                    NouhinData[i].Write(sw);
                }

                if (!string.IsNullOrEmpty(strFilePath))
                {
                    // 納品データはシフトJISで
                    file = new System.IO.StreamWriter(strFilePath, true, System.Text.Encoding.GetEncoding(932));
                    file.Write(sw.ToString());
                }

                strData = sw.ToString();

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
                sqlConn.Close();
                if (null != file)
                    file.Close();

                if (bExistFile)
                    File.SetLastWriteTime(strFilePath, DateTime.Now);

            }

        }



        public static ViewDataset.VIEW_NouhinMeisaiDataTable
            getVIEW_NouhinMeisaiDataTable(NouhinDataClass.DenpyouKey key, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT * 
FROM                     dbo.VIEW_NouhinMeisai
WHERE                   (ShiiresakiCode = @s) AND (YYMM = @y) AND (HakkouNo = @h)";

            da.SelectCommand.Parameters.AddWithValue("@h", key.HakkouNo);
            da.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@y", key.YYMM);
            ViewDataset.VIEW_NouhinMeisaiDataTable dt = new ViewDataset.VIEW_NouhinMeisaiDataTable();
            da.Fill(dt);
            return dt;
        }


        public static ViewDataset.VIEW_ShiiresakiDataTable
            getVIEW_ShiiresakiDataTable(SqlConnection c)
        { 
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = @"
SELECT        DISTINCT          dbo.VIEW_Shiiresaki.*
FROM                     dbo.VIEW_Shiiresaki
WHERE                   (ShiiresakiCode IN
(SELECT                  NouhinmotoShiiresakiCode
FROM                     dbo.T_NouhinHeader
GROUP BY          NouhinmotoShiiresakiCode))

Order by ShiiresakiCode
";

            ViewDataset.VIEW_ShiiresakiDataTable dt = new ViewDataset.VIEW_ShiiresakiDataTable();
            da.Fill(dt);
            return dt;
        }



        /// <summary>
        /// 予算品で完了フラグの立っている納品データ取得(★★★★★ただし追加品番は除く★★★★★)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dkIgnore"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static MizunoDataSet.T_NouhinMeisaiDataTable
            getT_NouhinMeisaiDataTable4KanroyuFlg(ProperOrderClass.ProperOrderKey key, NouhinDataClass.DenpyouKey dkIgnore, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = @"
SELECT                  dbo.T_NouhinMeisai.*
FROM                     dbo.T_NouhinMeisai INNER JOIN
dbo.T_NouhinHeader ON dbo.T_NouhinMeisai.HakkouNo = dbo.T_NouhinHeader.HakkouNo AND 
dbo.T_NouhinMeisai.ShiiresakiCode = dbo.T_NouhinHeader.NouhinmotoShiiresakiCode AND 
dbo.T_NouhinMeisai.YYMM = dbo.T_NouhinHeader.YYMM
WHERE                   ((dbo.T_NouhinMeisai.KanryouFlg = N'9') OR (dbo.T_NouhinMeisai.KanryouFlg = N'8')) AND (dbo.T_NouhinHeader.SashizuBi = @z) AND (dbo.T_NouhinHeader.SeisanOrderNo = @s) AND 
(dbo.T_NouhinHeader.HikitoriOrderNo = @h) AND T_NouhinMeisai.HinbanTsuikaFlg='0' AND T_NouhinMeisai.RowNo>=0";
            //完了フラグに8を追加 20160920
            da.SelectCommand.Parameters.AddWithValue("@z", key.SashizuBi);
            da.SelectCommand.Parameters.AddWithValue("@s", key.SeisanOrderNo);
            da.SelectCommand.Parameters.AddWithValue("@h", key.HikitoriOrderNo);

            if (null != dkIgnore) { 
                da.SelectCommand.CommandText += " and NOT (dbo.T_NouhinHeader.HakkouNo = @HakkouNo AND dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = @s_code AND dbo.T_NouhinHeader.YYMM = @YYMM)";
                da.SelectCommand.Parameters.AddWithValue("@HakkouNo", dkIgnore.HakkouNo);
                da.SelectCommand.Parameters.AddWithValue("@s_code", dkIgnore.ShiiresakiCode);
                da.SelectCommand.Parameters.AddWithValue("@YYMM", dkIgnore.YYMM);
            }

            MizunoDataSet.T_NouhinMeisaiDataTable dt = new MizunoDataSet.T_NouhinMeisaiDataTable();
            da.Fill(dt);
            return dt;
        }

        /// <summary>
        /// 材料で完了フラグの立っている納品データ取得
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dkIgnore"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static MizunoDataSet.T_NouhinMeisaiDataTable
            getT_NouhinMeisaiDataTable4KanroyuFlg(ZairyouOrderClass.ZairyouOrderKey key, NouhinDataClass.DenpyouKey dkIgnore, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = @"
SELECT                  dbo.T_NouhinMeisai.*
FROM                     dbo.T_NouhinMeisai INNER JOIN
dbo.T_NouhinHeader ON dbo.T_NouhinMeisai.HakkouNo = dbo.T_NouhinHeader.HakkouNo AND 
dbo.T_NouhinMeisai.ShiiresakiCode = dbo.T_NouhinHeader.NouhinmotoShiiresakiCode AND 
dbo.T_NouhinMeisai.YYMM = dbo.T_NouhinHeader.YYMM
WHERE                   (dbo.T_NouhinMeisai.KanryouFlg = N'9') AND (dbo.T_NouhinHeader.SashizuBi = @z) AND 
(dbo.T_NouhinHeader.ZairyouOrderNo = @h) AND T_NouhinMeisai.HinbanTsuikaFlg='0' AND T_NouhinMeisai.RowNo>=0";

            da.SelectCommand.Parameters.AddWithValue("@z", key.SashizuBi);
            da.SelectCommand.Parameters.AddWithValue("@h", key.HikitoriOrderNo);

            if (null != dkIgnore)
            {
                da.SelectCommand.CommandText += " and NOT (dbo.T_NouhinHeader.HakkouNo = @HakkouNo AND dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = @s_code AND dbo.T_NouhinHeader.YYMM = @YYMM)";
                da.SelectCommand.Parameters.AddWithValue("@HakkouNo", dkIgnore.HakkouNo);
                da.SelectCommand.Parameters.AddWithValue("@s_code", dkIgnore.ShiiresakiCode);
                da.SelectCommand.Parameters.AddWithValue("@YYMM", dkIgnore.YYMM);
            }

            MizunoDataSet.T_NouhinMeisaiDataTable dt = new MizunoDataSet.T_NouhinMeisaiDataTable();
            da.Fill(dt);
            return dt;
        }


        /// <summary>
        /// 別注品で完了フラグの立っている納品データ取得（★★★★★ただし追加品番は除く★★★★★）
        /// 注文品番と同じ追加品番が登録できてしまう為、追加品番の完了フラグで注文品番が完了したと誤って判断してしまう不具合があるので注意
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dkIgnore"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static MizunoDataSet.T_NouhinMeisaiDataTable
            getT_NouhinMeisaiDataTable4KanroyuFlg(BecchuOrderClass.BecchuOrderKey key, NouhinDataClass.DenpyouKey dkIgnore, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = @"
SELECT                  dbo.T_NouhinMeisai.*
FROM                     dbo.T_NouhinMeisai INNER JOIN
dbo.T_NouhinHeader ON dbo.T_NouhinMeisai.HakkouNo = dbo.T_NouhinHeader.HakkouNo AND 
dbo.T_NouhinMeisai.ShiiresakiCode = dbo.T_NouhinHeader.NouhinmotoShiiresakiCode AND 
dbo.T_NouhinMeisai.YYMM = dbo.T_NouhinHeader.YYMM
WHERE                   (dbo.T_NouhinMeisai.KanryouFlg = N'9') AND (dbo.T_NouhinHeader.SashizuBi = @z) AND (dbo.T_NouhinHeader.OrderKanriNo = @OrderKanriNo) AND 
(dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = @s) AND T_NouhinMeisai.HinbanTsuikaFlg='0' AND T_NouhinMeisai.RowNo>=0";

            da.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", key.OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);

            if (null != dkIgnore)
            {
                da.SelectCommand.CommandText += " and NOT (dbo.T_NouhinHeader.HakkouNo = @HakkouNo AND dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = @s_code AND dbo.T_NouhinHeader.YYMM = @YYMM)";
                da.SelectCommand.Parameters.AddWithValue("@HakkouNo", dkIgnore.HakkouNo);
                da.SelectCommand.Parameters.AddWithValue("@s_code", dkIgnore.ShiiresakiCode);
                da.SelectCommand.Parameters.AddWithValue("@YYMM", dkIgnore.YYMM);
            }

            MizunoDataSet.T_NouhinMeisaiDataTable dt = new MizunoDataSet.T_NouhinMeisaiDataTable();
            da.Fill(dt);
            return dt;
        }




        /// <summary>
        /// 送品区分毎の伝票表示行数
        /// </summary>
        /// <param name="sk"></param>
        /// <returns></returns>
        public static int GetDenpyouGyouCount(EnumSouhinKubun sk)
        {
            /* jde対応 コメントアウト
            switch (sk)
            {
                case EnumSouhinKubun.DC:
                case EnumSouhinKubun.KariNouhin:
                    return 17;
                case EnumSouhinKubun.EigyoushoNouhin:
                case EnumSouhinKubun.SKNouhin:
                    return 10;
                case EnumSouhinKubun.NSS:
                case EnumSouhinKubun.Chokusou:
                    return 8;
            }
             */

            switch (sk)
            {
                case EnumSouhinKubun.DC:
                    return 17;
                case EnumSouhinKubun.NSS_Eigyousho:
                case EnumSouhinKubun.Nituu_GMS:
                case EnumSouhinKubun.Chokusou:
                case EnumSouhinKubun.SeisanBumon:
                    return 8;
            }
            return 0;
        }


        /*
        public static string GetDenpyouKubun(EnumSouhinKubun sk)
        {
            switch (sk)
            {
                case EnumSouhinKubun.DC:
                    return "26";
                //case EnumSouhinKubun.KariNouhin:
                //    return "265";
                case EnumSouhinKubun.None:
                    return "";
                default:
                    return "20";
            }
        }
        */


        /// <summary>
        /// 予算品、別注品で共通の伝票登録項目
        /// </summary>
        public class TourokuDataBase
        {
            public string Suuryou = "0";

            public string DenpyouKubun = "";    // アップロード時 20,26,265

            public EnumSouhinKubun SouhinKubun = EnumSouhinKubun.None;
            public string NouhinKigou; // 出荷入力画面で選択された納品記号
            // 荷受事業所コード   
            public string NiukeJigyoushoCode;
            public string NiukeJigyoushoMei;
            // 保管場所
            public string HokanBasho;
            public string NiukeBashoMei;
            // 送り先
            public string HanbaitenKigyouRyakuMei = "";

            //---invoiceNo
            public string invoiceNo = "";

            //2013/11/07 岡村
            public string KinkyuChokusouBikou = "";

            //2014/02/14 岡村 追加
            public string UnsouGyoushaCode = "";
            public string UnsouGyoushaMei = "";
            public string KogutiSu = "";
            public byte bSoushinFlg = SoushinFlg.NONE;

            public DateTime dtHakkouBi;

            // 生産部担当者コード
            public string SKTantoushaCode = "";
            // SK担当者名
            public string SKTantoushaMei = "";

            // 荷受担当者コード
            public string EigyouTantoushaCode = "";
            // 営業担当者名
            public string EigyouTantoushaMei = "";

            // 店名                
            public string TenMei = "";

            // 学校名、チーム名
            public string TeamMei = "";

            //事業所を絞り込んだ場合に値が入る
            public string NiukeFilter = "";

        }


        /// <summary>
        /// 予算品、別注品で共通の伝票登録明細項目
        /// </summary>
        public class MeisaiDataCommon
        {
            public decimal TourokuKakaku { get; set; }

            public bool TuikaHinban { get; set; }
            public int ShukkaSu { get; set; }
            public int Suuryou { get; set; }
            public decimal Suuryou2 { get; set; }
            public string Tani { get; set; }
            public bool KanryouFlag { get; set; }
            public EnumNouhinKubun NouhinKubun { get; set; }
            public int Sekizuke = 0;  // 籍付
            public int YuuduuKakaku { get; set; }
            public string Tekiyou { get; set; }
            public string Free1 { get; set; } // 自由項目
            public string Free2 { get; set; }
            public string Free3 { get; set; }
            public string Bikou { get; set; }
            public decimal ShukkaSu2 { get; set; }
            public string KeigenZeiritsu { get; set; } // 軽減税率 追加 20190821

            public MeisaiDataCommon()
            {
                this.YuuduuKakaku = 0;
            }

            protected void Copy(ref MeisaiDataCommon m)
            {
                m.TourokuKakaku = TourokuKakaku;
                m.Free1 = this.Free1;
                m.Free2 = this.Free2;
                m.Free3 = this.Free3;
                m.KanryouFlag = this.KanryouFlag;
                m.NouhinKubun = this.NouhinKubun;
                m.ShukkaSu = this.ShukkaSu;
                m.Tekiyou = this.Tekiyou;
                m.TourokuKakaku = this.TourokuKakaku;
                m.TuikaHinban = this.TuikaHinban;
                m.YuuduuKakaku = this.YuuduuKakaku;
                m.KeigenZeiritsu = this.KeigenZeiritsu; // 軽減税率 追加 20190821
            }
        }

        private enum ZairyouDataField : int
        {
            ShiiresakiCode=0,
            HacchuNo=1,
            LotNo=2,
            Hinban=3,
            Suuryou=4,
            KanryouFlg=5,
        }


        // データ項目
        private enum EnumDataField : int
        {
            ShiiresakiCode      = 0,
            HacchuBi            = 1,
            HacchuNo            = 2,
            // DenpyouKubun = 3,
            DenpyouHakkouBi     = 3,
            SouhinKubun         = 4,
            NiukeJigyoushoCode  = 5,
            HokanBasho          = 6,
            NouhinKigou         = 7,
            //HonNouhinTsuki = 9,
            GyouNo              = 8,
            LotNo               = 9,
            Hinban              = 10,
            Size                = 11,
            //ShiireKakaku    = 13,
            //YuuzuuKakaku    = 14,
            Suuryou             = 12,
            KanryouFlg          = 13,
            //NouhinKubun     = 17,
            Tekiyou             = 14,
            //Sekiduke        = 19,
            SeisanTantoushaMei  = 15,
            EigyouTantoushaMei  = 16,
            Okurisaki           = 17,
            TenMei              = 18,
            TeamMei             = 19,
            JiyuKoumoku1        = 20,
            JiyuKoumoku2        = 21,
            JiyuKoumoku3        = 22,
            YosanTuki           = 23,   // 予算品の予算月
            BecchuHin_RowNo     = 24, // 別注品の行No
            SPNo                = 25,
            UnsouGyoushaMei     = 26,
            OkuriJyouNo         = 27,
            INVOICE             = 28,
            KinkyuChokusouBikou = 29,
            UnsouGyoushaCD      = 30,   //2014/03/31 岡村 追加
            KogutiSu            = 31,   //2014/03/31 岡村 追加
            //GoukeiKakaku    = 33
        }


        public static string StrCnvToHankaku(string strArg)
        {
            return Strings.StrConv(strArg, VbStrConv.Narrow, 0x0411);
        }


        private class ZairyouOrderTourokuData
        {
            //public ProperOrderClass.TourokuData TourokuData { get; set; }
            //public ProperOrderClass.ProperOrderKey ProperOrderKey { get; set; }
            public ZairyouOrderClass.TourokuData TourokuData { get; set; }
            public ZairyouOrderClass.ZairyouOrderKey ZairyouOrderKey { get; set; }

            public Core.Type.Nengetu YosanTuki { get; set; }
            public string NouhinKigou { get; set; }

            SortedList<byte, ZairyouOrderClass.MeisaiData> _tblMeisaiDataWithGyouNo = new SortedList<byte, ZairyouOrderClass.MeisaiData>();
            //SortedList<byte, ProperOrderClass.MeisaiData> _tblMeisaiDataWithGyouNo = new SortedList<byte, ProperOrderClass.MeisaiData>();
            List<ZairyouOrderClass.MeisaiData> _lstMeisaiDataWithNoGyouNo = new List<ZairyouOrderClass.MeisaiData>();

            public bool AddMeisai(byte bGyouNo, ZairyouOrderClass.MeisaiData m)
            {
                if (0 == bGyouNo)
                {
                    _lstMeisaiDataWithNoGyouNo.Add(m);
                    return true;
                }
                else
                {

                    if (_tblMeisaiDataWithGyouNo.ContainsKey(bGyouNo)) return false;
                    _tblMeisaiDataWithGyouNo.Add(bGyouNo, m);
                    return true;
                }
            }

            /// <summary>
            /// 正しい明細順で格納する。
            /// </summary>
            public void SetMeisaiData()
            {

                int nMaxGyouNo = _tblMeisaiDataWithGyouNo.Keys[_tblMeisaiDataWithGyouNo.Count - 1];
                int nCount = _tblMeisaiDataWithGyouNo.Count + _lstMeisaiDataWithNoGyouNo.Count;
                if (nCount < nMaxGyouNo) nCount = nMaxGyouNo;
                nCount++;

                ZairyouOrderClass.MeisaiData[] array = new ZairyouOrderClass.MeisaiData[nCount];

                for (int i = 0; i < _tblMeisaiDataWithGyouNo.Count; i++)
                {
                    byte bGyouNo = _tblMeisaiDataWithGyouNo.Keys[i];
                    array[bGyouNo] = _tblMeisaiDataWithGyouNo[bGyouNo];
                }

                // 行Noが無いものは空いている若い行Noから順に格納
                for (int i = 0; i < _lstMeisaiDataWithNoGyouNo.Count; i++)
                {
                    for (int k = 0; k < array.Length; k++)
                    {
                        if (array[k] == null)
                        {
                            array[k] = _lstMeisaiDataWithNoGyouNo[i];
                            break;
                        }
                    }
                }

                TourokuData.lstMeisai.Clear();
                for (int i = 0; i < array.Length; i++)
                {
                    if (null != array[i])
                        TourokuData.lstMeisai.Add(array[i]);
                }
            }
        }

        private class ProperOrderTourokuData
        {
            public ProperOrderClass.TourokuData TourokuData { get; set; }
            public ProperOrderClass.ProperOrderKey ProperOrderKey { get; set; }
            public Core.Type.Nengetu YosanTuki { get; set; }
            public string NouhinKigou { get; set; }
            public string INVOICE { get; set; }

            SortedList<byte, ProperOrderClass.MeisaiData> _tblMeisaiDataWithGyouNo = new SortedList<byte, ProperOrderClass.MeisaiData>();
            List<ProperOrderClass.MeisaiData> _lstMeisaiDataWithNoGyouNo = new List<ProperOrderClass.MeisaiData>();

            public bool AddMeisai(byte bGyouNo, ProperOrderClass.MeisaiData m)
            {
                if (0 == bGyouNo)
                {
                    _lstMeisaiDataWithNoGyouNo.Add(m);
                    return true;
                }
                else
                {

                    if (_tblMeisaiDataWithGyouNo.ContainsKey(bGyouNo)) return false;
                    _tblMeisaiDataWithGyouNo.Add(bGyouNo, m);
                    return true;
                }
            }

            /// <summary>
            /// 正しい明細順で格納する。
            /// </summary>
            public void SetMeisaiData()
            {
                int nMaxGyouNo = _tblMeisaiDataWithGyouNo.Keys[_tblMeisaiDataWithGyouNo.Count - 1];
                int nCount = _tblMeisaiDataWithGyouNo.Count + _lstMeisaiDataWithNoGyouNo.Count;
                if (nCount < nMaxGyouNo) nCount = nMaxGyouNo;
                nCount++;

                ProperOrderClass.MeisaiData[] array = new ProperOrderClass.MeisaiData[nCount];

                for (int i = 0; i < _tblMeisaiDataWithGyouNo.Count; i++) {
                    byte bGyouNo = _tblMeisaiDataWithGyouNo.Keys[i];
                    array[bGyouNo] = _tblMeisaiDataWithGyouNo[bGyouNo];
                }

                // 行Noが無いものは空いている若い行Noから順に格納
                for (int i = 0; i < _lstMeisaiDataWithNoGyouNo.Count; i++) {
                    for (int k = 0; k < array.Length; k++) {
                        if (array[k] == null) {
                            array[k] = _lstMeisaiDataWithNoGyouNo[i];
                            break;
                        }
                    }
                }

                TourokuData.lstMeisai.Clear();
                for (int i = 0; i < array.Length; i++) {
                    if (null != array[i])
                        TourokuData.lstMeisai.Add(array[i]);    
                }
            }

        }


        private class BecchuOrderTourokuData
        {
            public BecchuOrderClass.TourokuData TourokuData { get; set; }
            public BecchuOrderClass.BecchuOrderKey BecchuOrderKey { get; set; }
            
            SortedList<byte, BecchuOrderClass.MeisaiData> _tblMeisaiDataWithGyouNo = new SortedList<byte, BecchuOrderClass.MeisaiData>();
            List<BecchuOrderClass.MeisaiData> _lstMeisaiDataWithNoGyouNo = new List<BecchuOrderClass.MeisaiData>();


            public string BecchuSpectraNo{get;set;}
            public string UnsouGyoushaMei{get;set;}           
            public string OkuriJyouNo{get;set;}
            public string INVOICE { get; set; }
            public string KinkyuChokusouBikou { get; set; }

            public string UnsouGyoushaCD { get; set; }  //2014/03/31 岡村 追加
            public string KogutiSu { get; set; }        //2014/03/31 岡村 追加   

            public string LotNo { get; set; }

            public int? GoukeiKakaku { get; set; }


            public bool AddMeisai(byte bGyouNo, BecchuOrderClass.MeisaiData m)
            {
                if (0 == bGyouNo)
                {
                    _lstMeisaiDataWithNoGyouNo.Add(m);
                    return true;
                }
                else
                {

                    if (_tblMeisaiDataWithGyouNo.ContainsKey(bGyouNo)) return false;
                    _tblMeisaiDataWithGyouNo.Add(bGyouNo, m);
                    return true;
                }
            }

            /// <summary>
            /// 正しい明細順で格納する。
            /// </summary>
            public void SetMeisaiData()
            {
                int nMaxGyouNo = _tblMeisaiDataWithGyouNo.Keys[_tblMeisaiDataWithGyouNo.Count - 1];
                int nCount = _tblMeisaiDataWithGyouNo.Count + _lstMeisaiDataWithNoGyouNo.Count;
                if (nCount < nMaxGyouNo) nCount = nMaxGyouNo;
                nCount++;

                BecchuOrderClass.MeisaiData[] array = new BecchuOrderClass.MeisaiData[nCount];

                for (int i = 0; i < _tblMeisaiDataWithGyouNo.Count; i++)
                {
                    byte bGyouNo = _tblMeisaiDataWithGyouNo.Keys[i];
                    array[bGyouNo] = _tblMeisaiDataWithGyouNo[bGyouNo];
                }

                // 行Noが無いものは空いている若い行Noから順に格納
                for (int i = 0; i < _lstMeisaiDataWithNoGyouNo.Count; i++)
                {
                    for (int k = 0; k < array.Length; k++)
                    {
                        if (array[k] == null)
                        {
                            array[k] = _lstMeisaiDataWithNoGyouNo[i];
                            break;
                        }
                    }
                }


                TourokuData.lstMeisai.Clear();
                for (int i = 0; i < array.Length; i++)
                {
                    if (null != array[i])
                        TourokuData.lstMeisai.Add(array[i]);
                }
            }
        }

        public static Core.Error UploadNouhnData(string strShiiresakiCode, System.IO.Stream stream,
            System.Text.Encoding enc, SqlConnection c, out List<string> lstErrorMsg,
            out List<NouhinDataClass.DenpyouKey> lstDenpyouKey, out List<NouhinDataClass.DenpyouKey> lstDenpyouKeyShukkaAri,
            string ChokusouJigyoushoCode,
            string ChokusouHokanBasho
            )
        {
            lstErrorMsg = new List<string>();
            lstDenpyouKey = new List<NouhinDataClass.DenpyouKey>();
            lstDenpyouKeyShukkaAri = new List<DenpyouKey>();

            // 予算品データ取得、注文番号は、生産オーダーNoか引取オーダーNoのどちらか
            SqlDataAdapter daProperKey = new SqlDataAdapter("", c);
            daProperKey.SelectCommand.CommandText = @"
SELECT                  dbo.T_ProperOrder.*
FROM                     dbo.T_ProperOrder
WHERE                   (SashizuBi = @z) AND ((SeisanOrderNo = @c AND ltrim(HikitoriOrderNo) = '') OR (ltrim(SeisanOrderNo)='' AND HikitoriOrderNo = @c))";
            daProperKey.SelectCommand.Parameters.AddWithValue("@z", "");
            daProperKey.SelectCommand.Parameters.AddWithValue("@c", "");

            // 予算月の件数取得
            SqlDataAdapter daGetYosanTuki = new SqlDataAdapter("", c);
            daGetYosanTuki.SelectCommand.CommandText = @"
SELECT                  MAX(YosanTsuki) AS YosanTsuki, COUNT(DISTINCT YosanTsuki) AS YosanTsukiCount
FROM                     dbo.T_ProperOrder
WHERE                   (SashizuBi = @z) AND (SeisanOrderNo = @s) AND (HikitoriOrderNo = @h)";
            daGetYosanTuki.SelectCommand.Parameters.AddWithValue("@z", "");
            daGetYosanTuki.SelectCommand.Parameters.AddWithValue("@s", "");
            daGetYosanTuki.SelectCommand.Parameters.AddWithValue("@h", "");

            // 別注品キー情報　MizunoUketsukeBiが文字列でいい加減なので日付で検索　yyyy/MM/ddの形式になっているとは限らない (2011/1/31のようなデータもある)
            SqlDataAdapter daBecchu = new SqlDataAdapter("", c);
            daBecchu.SelectCommand.CommandText = "SELECT * FROM VIEW_BecchuKeyInfo WHERE OrderKanriNo=@ChumonBangou AND ShiiresakiCode = @s and CONVERT(char(8),CAST(MizunoUketsukeBi as date), 112) = @MizunoUketsukeBi AND ISDATE(MizunoUketsukeBi) = 1";
            daBecchu.SelectCommand.Parameters.AddWithValue("@s", strShiiresakiCode);
            daBecchu.SelectCommand.Parameters.AddWithValue("@ChumonBangou", "");
            daBecchu.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", "");

            // 予算品の品番+サイズの明細取得
            SqlDataAdapter daProper = new SqlDataAdapter("", c);
            daProper.SelectCommand.CommandText = @"
SELECT                  dbo.T_ProperOrder.*
FROM                     dbo.T_ProperOrder
WHERE                   (SashizuBi = @z) AND SeisanOrderNo = @s AND HikitoriOrderNo = @h AND YosanTsuki=@y and (Ltrim(Hinban) = @Hinban) AND (Size = @Size)";
            daProper.SelectCommand.Parameters.AddWithValue("@z", "");
            daProper.SelectCommand.Parameters.AddWithValue("@s", "");
            daProper.SelectCommand.Parameters.AddWithValue("@h", "");
            daProper.SelectCommand.Parameters.AddWithValue("@y", 0);
            daProper.SelectCommand.Parameters.AddWithValue("@Hinban", "");
            daProper.SelectCommand.Parameters.AddWithValue("@Size", "");

            System.IO.StreamReader tabReader = null;
            Core.IO.CSVReader csvReader = null;

            System.IO.StreamReader check = new System.IO.StreamReader(stream, enc);
            string strCheck = check.ReadLine();  // CSVとタブ区切りの確認の為
            if (null == strCheck)
            {
                return new Core.Error("データがありません。");
            }
            bool bTab = (strCheck.Split('\t').Length > strCheck.Split(',').Length);   // これ良いの？
            stream.Position = 0;
            if (bTab)
                tabReader = new System.IO.StreamReader(stream, enc);
            else
                csvReader = new Core.IO.CSVReader(stream, enc);

            int[] nDataMaxLength = new int[]{
8,  // 1:仕入先ｺｰﾄﾞ
10,  // 2:指図日(ミズノ受付日)
40, // 3:ｵｰﾀﾞｰ№
//3,  
8,  // 4:伝票発行日
1,  // 5:送品区分
4,  // 6:荷受事業所ｺｰﾄﾞ
20,  // 7:保管場所ｺｰﾄﾞ
3,  // 8:納品記号
//6,  
2,  // 9行No
50, // 10:ロットNo
10, // 11:品番
5,  // 12:サイズ
//7,
//7,
5,  // 13:数量
1,  // 14:完了フラグ
//1,  
15, // 15:摘要
10,  // 16:生産担当者
10, // 17:営業担当者
10, // 18:送り先
10, // 19:店名
10, // 20:学校/チーム名
//20,     // 25項目目 ?
20, // 21:自由項目1
20, // 22:自由項目2
20, // 23:自由項目3
6,  // 24:予算品予算月(YYYYMM)
2,  // 25:別注2行No
14, // 26:スペクトラオーダーNo
15, // 27:運送業者名
15, // 28:送り状No
18, // 29:InvoiceNo
//10  
30, // 30:緊急直送備考
2,  // 31:運送業者CD    2014/03/31 岡村 追記
2,  // 32:個口数        2014/03/31 岡村 追記
            };

            string[] strFieldMei = new string[]{            
"仕入先ｺｰﾄﾞ",
"指図日(ミズノ受付日)",
"ｵｰﾀﾞｰ№",
//"伝区",
"伝票発行日",
"送品区分",
"荷受事業所ｺｰﾄﾞ",
"保管場所ｺｰﾄﾞ",
"納品記号",
//"本納品月",
"行No",
"ロットNo.",    // 追加
"品番",
"サイズ",
//"仕入価格",
//"融通価格",
"数量",
"完了フラグ",
//"納品区分",
"摘要",
//"籍付",
"生産担当者",
"営業担当者",
"送り先",
"店名",
"学校/チーム名",
"自由使用欄1",
"自由使用欄2",
"自由使用欄3",
"予算月",
"別注品明細No",
"ｽﾍﾟｸﾄﾗｵｰﾀﾞｰ№",
"運送業者名",
"送り状№",
//"仕入合計金額"
"INVOICE№",
"緊急直送備考",
"運送業者CD",
"個口数",
            };

            TsuikaHinban th = new TsuikaHinban();

            NouhinDataClass.TourokuDataBase denpyou = null;
            ProperOrderTourokuData pData = null;
            BecchuOrderTourokuData bData = null;

            List<object> lstTourokuData = new List<object>();
            
            DateTime dtNow = DateTime.Now;
            ViewDataset.VIEW_BecchuKeyInfoRow drBecchuKey = null;
            BecchuOrderClass.EnumBecchuKubun BecchuKubun = BecchuOrderClass.EnumBecchuKubun.None;
            ViewDataset.VIEW_Becchu_ShukkaMeisaiDataTable dtBecchu = null;
            ViewDataset.VIEW_Becchu2_ShukkaMeisaiDataTable dtBecchu2 = null;

            //2014/11/05 三津谷修正
            //ViewDataset.VIEW_DS_ShukkaMeisaiDataTable dtDS = null;
            ViewDataset.VIEW_DS2_ShukkaMeisaiDataTable dtDS = null;

            ViewDataset.VIEW_SP_ShukkaMeisaiDataTable dtSP = null;
            MizunoDataSet.M_TsuikaHinbanDataTable dtTuikaHinban4Becchu = null;  // 種目コード別の追加品番
            
            // 完了フラグの立っているデータ
            Dictionary<string, ProperOrderClass.TourokuData> tblProperKanroyuData = new Dictionary<string, ProperOrderClass.TourokuData>(); // キー=指図日_生産オーダーNo_引取オーダーNo_品番_サイズ
            Dictionary<string, BecchuOrderClass.TourokuData> tblBecchuKanroyuData = new Dictionary<string, BecchuOrderClass.TourokuData>();
            
            // 「数量0&完了フラグ = 9」のデータと、「数量1以上」のデータが1伝票内に共存してはいけない。
            int nSuryouZeroDataCount = 0;

            int nLine = 0;
            string[] str = null;
            string[] strPrevData = null;
            
            while (true)
            {
                try
                {
                    string strLine = null;

                    string[] strArray = null;

                    if (null != tabReader)
                    {
                        strLine = tabReader.ReadLine();
                        if (null == strLine) break;
                        strArray = strLine.Split('\t');
                    }
                    else
                    {
                        strArray = csvReader.GetCSVLine(ref strLine);
                        if (string.IsNullOrEmpty(strLine)) break;
                        if (null == strArray || 0 == strArray.Length) break;
                    }

                    nLine++;

                    //str = new string[30];
                    str = new string[32];

                    if (strArray.Length < 14)
                    {
                        throw new  Exception(string.Format("{0}行目の列数が14列に満たないため、登録できません。", nLine));
                    }

                    //for (int i = 0; i < 30; i++)
                    for (int i = 0; i < 32; i++)
                    { str[i] = ""; }

                    for (int i = 0; i < strArray.Length; i++)
                    { str[i] = strArray[i]; }

                    for (int i = 0; i < str.Length; i++)
                    {
                        string s = str[i].Trim();
                        // 桁数チェック
                        if (nDataMaxLength[i] < s.Length)
                        {
                            // 2013.02.28
                            throw new Exception(string.Format("{0}列目({1})が規定桁数を上回っています(規定桁数={2})", i + 1, strFieldMei[i], nDataMaxLength[i]));
                            // throw new Exception(string.Format("{0}列目({1})が規定桁数を上回っています(規定桁数={2})", i + 1, strFieldMei[i], s.Length));
                        }
                        // 必須項目チェック
                        if ("" == s)
                        {
                            switch ((EnumDataField)i)
                            {
                                case EnumDataField.ShiiresakiCode:
                                case EnumDataField.HacchuBi:
                                case EnumDataField.HacchuNo:
                                case EnumDataField.DenpyouHakkouBi:
                                case EnumDataField.SouhinKubun:
                                case EnumDataField.NiukeJigyoushoCode:
                                //case EnumDataField.HokanBasho:
                                //case EnumDataField.NouhinKigou:
                                case EnumDataField.Hinban:
                                case EnumDataField.Suuryou:
                                case EnumDataField.KanryouFlg:
                                case EnumDataField.GyouNo:
                                    throw new Exception(string.Format("{0}列目({1})は必須項目です", i + 1, strFieldMei[i]));
                            }
                        }
                        // 数値項目のチェック
                        if ("" != s)
                        {
                            str[i] = StrCnvToHankaku(s).Trim();
                            s = str[i];
                            switch ((EnumDataField)i)
                            {
                                case EnumDataField.ShiiresakiCode:
                                //case EnumDataField.HacchuBi:
                                case EnumDataField.DenpyouHakkouBi:
                                case EnumDataField.NiukeJigyoushoCode:
                                case EnumDataField.GyouNo:
                                case EnumDataField.Suuryou:
                                case EnumDataField.KanryouFlg:
                                case EnumDataField.BecchuHin_RowNo:
                                case EnumDataField.YosanTuki:
                                    try
                                    {
                                        int.Parse(s);
                                    }
                                    catch
                                    {
                                        throw new Exception(string.Format("{0}列目({1})は数値項目です", i + 1, strFieldMei[i]));
                                    }
                                    break;
                            }
                        }
                    }

                    // 仕入先コード
                    if (int.Parse(strShiiresakiCode) != int.Parse(str[(int)EnumDataField.ShiiresakiCode]))
                        throw new Exception("仕入先コードが正しくありません。");

                    // 行No
                    bool bNewDenpyou = false;
                    byte bGyouNo = 0;
                    string strRowNo = str[(int)EnumDataField.GyouNo];

                    if ("" != strRowNo)
                    {
                        if (!byte.TryParse(strRowNo, out bGyouNo) || 0 == bGyouNo)
                        {
                            throw new Exception("行Noを正しく入力してください。");
                        }

                        if (1 != bGyouNo && 0 == lstTourokuData.Count)
                        {
                            return new Error("行Noは1から入力して下さい。");
                        }

                        bNewDenpyou = (1 == bGyouNo);
                    }

                    //2013/08/30 岡村 追加
                    // InvoiceNo
                    string strInvoiceNo = str[(int)EnumDataField.INVOICE];

                    if ("" != strInvoiceNo)
                    {

                        if (strInvoiceNo.Contains(",") ||
                            strInvoiceNo.Contains("?") ||
                            strInvoiceNo.Contains("!") ||
                            strInvoiceNo.Contains("\"") ||
                            strInvoiceNo.Contains("'")
                            )
                            {
                                throw new Exception("INVOICE No.に使用不可文字が入力されています");
                            }
                    }

                    //29******の仕入先の場合INVOICE№欄は必須とする。
                    string sShiiresakiCode = str[(int)EnumDataField.ShiiresakiCode];

                    if (sShiiresakiCode.Substring(0, 2) == "29")
                    {
                        //** 仕入先が海外の場合

                        if (strInvoiceNo == "")
                        { throw new Exception("INVOICE No.を入力して下さい"); }
                    }

                    // 2013.02.28 YYYY/MM/DD書式でもアップロードできるように修正
                    DateTime dateSashizuBi = new DateTime();
                    bool isHidukeFormat = false;
                    string strHiduke = str[(int)EnumDataField.HacchuBi].Trim();

                    if (System.Text.RegularExpressions.Regex.IsMatch(strHiduke, @"\d{8}"))
                    {
                        string yyyy_mm_dd = strHiduke.Substring(0, 4) + "/" + strHiduke.Substring(4, 2) + "/" + strHiduke.Substring(6, 2);
                        isHidukeFormat = DateTime.TryParse(yyyy_mm_dd, out dateSashizuBi);
                    }

                    if (!isHidukeFormat && System.Text.RegularExpressions.Regex.IsMatch(strHiduke, @"\d{4}/[01]?\d{1}/[01]?\d{1}"))
                    {
                        isHidukeFormat = DateTime.TryParse(strHiduke, out dateSashizuBi);
                    }

                    if (!isHidukeFormat)
                    {
                        throw new Exception("指図日はYYYY/MM/DDまたはYYYYMMDDの日付書式で入力してください。");
                    }

                    string strSashizuBi = dateSashizuBi.ToString("yyyyMMdd");
                    string strChumonNo = str[(int)EnumDataField.HacchuNo].Trim();

                    if (bNewDenpyou)
                    {
                        // 新しい伝票の始まり
                        nSuryouZeroDataCount = 0;

                        // 予算品 or 別注品かの確認
                        pData = null; bData = null;  // リセット
                        daProperKey.SelectCommand.Parameters["@z"].Value = strSashizuBi;
                        daProperKey.SelectCommand.Parameters["@c"].Value = strChumonNo;

                        //緊急直送判定用 20160824
                        bool bKinkyuChokusou = false;

                        MizunoDataSet.T_ProperOrderDataTable dtProper = new MizunoDataSet.T_ProperOrderDataTable();
                        daProperKey.Fill(dtProper);
                        if (0 < dtProper.Count)
                        {
                            // 予算品
                            pData = new ProperOrderTourokuData();

                            pData.TourokuData = new ProperOrderClass.TourokuData();
                            pData.ProperOrderKey = new ProperOrderClass.ProperOrderKey(dtProper[0].SashizuBi, dtProper[0].SeisanOrderNo, dtProper[0].HikitoriOrderNo);

                            // 予算月取得
                            daGetYosanTuki.SelectCommand.Parameters["@z"].Value = pData.ProperOrderKey.SashizuBi;
                            daGetYosanTuki.SelectCommand.Parameters["@h"].Value = pData.ProperOrderKey.HikitoriOrderNo;
                            daGetYosanTuki.SelectCommand.Parameters["@s"].Value = pData.ProperOrderKey.SeisanOrderNo;
                            DataTable dtYosanTuki = new DataTable();
                            daGetYosanTuki.Fill(dtYosanTuki);
                            if (0 == dtYosanTuki.Rows.Count) return new Error("発注データが存在しません。");
                            int nYosanTuki = Convert.ToInt32(dtYosanTuki.Rows[0][0]);
                            int nYosanTukiCount = Convert.ToInt32(dtYosanTuki.Rows[0][1]);

                            if (1 < nYosanTukiCount)
                            {
                                // 本オーダーに予算月が複数ある場合
                                // 予算月を確認(1伝票に複数の予算月指定は不可)
                                string strYosanTuki = StrCnvToHankaku(str[(int)EnumDataField.YosanTuki].Trim());
                                if (6 != strYosanTuki.Length) throw new Exception("予算月を6桁(例:201106)で入力してください。");

                                try
                                {
                                    pData.YosanTuki = new Nengetu(int.Parse(strYosanTuki));
                                }
                                catch
                                {
                                    throw new Exception("予算月を正しく入力してください。");
                                }
                            }
                            else
                                pData.YosanTuki = new Nengetu(nYosanTuki);  // 予算月は1つで固定

                            //緊急直送専用処理 20160824
                            if (str[(int)EnumDataField.SouhinKubun].Trim() == "4")
                            {
                                int nError = 0;
                                //保管場所が「KKZK」ではない
                                if (str[(int)EnumDataField.HokanBasho] != "KKZK")
                                    nError = 1; //保管場所が違うだけ
                                //保管場所「KKZK」以外の事業所が設定されている
                                MizunoDataSet.M_NiukeBumonBashoDataTable dt =
                                    NiukeBumonClass.getM_NiukeBumonBashoSouhinKubun4DataTable(str[(int)EnumDataField.NiukeJigyoushoCode], c);
                                if (dt.Count == 0)
                                    if (nError != 1)
                                        nError = 2; //事業所が違うだけ
                                    else
                                        nError = 3; //保管場所と事業所が違う

                                string strdm = "";
                                for (int dm = 0; dm < 10000; dm++)
                                {
                                    strdm += dm.ToString();
                                }

                                if (nError != 0)
                                    switch (nError)
                                    {
                                        case 1:
                                            throw new Exception("送品区分が4：直送に変更されています。そのため荷受保管場所コードには「KKZK」しか入力できません。");
                                            break;
                                        case 2:
                                            throw new Exception(@"送品区分が4：直送に変更されています。そのため入力されている荷受事業所コードは設定できません。
事業所については、操作マニュアルダウンロード画面の緊急直送時出荷マニュアルをご確認下さい。");
                                            break;
                                        case 3:
                                            throw new Exception(@"送品区分が4：直送に変更されています。そのため入力されている荷受事業所コード・荷受保管場所コードは設定できません。
事業所については、操作マニュアルダウンロード画面の緊急直送時出荷マニュアルをご確認下さい。");
                                            break;
                                    }
                            }

                            //緊急直送専用処理 20161012
                            if (str[(int)EnumDataField.SouhinKubun].Trim() != "4")
                            {
                                if (str[(int)EnumDataField.HokanBasho] == "KKZK")
                                {
                                    throw new Exception(string.Format("オーダーNo：{0}の送品区分、事業所、保管場所の組み合わせが正しくありません", strChumonNo));
                                }
                            }

                            // INVOICE№
                            //pData.INVOICE = str[(int)EnumDataField.INVOICE].Trim();
                            //処理追加　送り状にデータが有る場合はInvoiceNoに送り状データを入れ、無い場合はInvoiceをInvoiceNoに入れる 20151217
                            if (str[(int)EnumDataField.OkuriJyouNo].Trim() != "")
                                pData.TourokuData.invoiceNo = str[(int)EnumDataField.OkuriJyouNo].Trim();
                            else
                                pData.TourokuData.invoiceNo = str[(int)EnumDataField.INVOICE].Trim();

                            //2016/03/04 追加　運送業者コード
                            pData.TourokuData.UnsouGyoushaCode = str[(int)EnumDataField.UnsouGyoushaCD].Trim();

                            //2016/03/04 追加　運送業者名
                            pData.TourokuData.UnsouGyoushaMei = str[(int)EnumDataField.UnsouGyoushaMei].Trim();

                            //2016/03/04 追加　小口数
                            pData.TourokuData.KogutiSu = str[(int)EnumDataField.KogutiSu].Trim();

                            lstTourokuData.Add(pData);
                            denpyou = pData.TourokuData;
                        }
                        else
                        {
                            // 別注品を検索
                            bData = new BecchuOrderTourokuData();

                            daBecchu.SelectCommand.Parameters["@ChumonBangou"].Value = strChumonNo;
                            daBecchu.SelectCommand.Parameters["@MizunoUketsukeBi"].Value = new Core.Type.Nengappi(int.Parse(strSashizuBi)).ToYYYYMMDD().ToString();
                            ViewDataset.VIEW_BecchuKeyInfoDataTable dtB = new ViewDataset.VIEW_BecchuKeyInfoDataTable();
                            daBecchu.Fill(dtB);
                            if (0 == dtB.Count) throw new Exception("発注データが存在しません。");
                            drBecchuKey = dtB[0];

                            //****緊急直送対応の場合は緊急直送備考を必須入力とする
                            //** 直送かどうかの判断
                            BecchuOrderClass.BecchuOrderKey VsBecchuOrderKey = new BecchuOrderClass.BecchuOrderKey(drBecchuKey.MizunoUketsukeBi,drBecchuKey.OrderKanriNo,drBecchuKey.ShiiresakiCode);

                            bool bChokusou = BecchuOrderClass.bChokusouOrder(VsBecchuOrderKey,
                                                            ChokusouJigyoushoCode,
                                                            ChokusouHokanBasho,
                                                            c
                                                            );

                            //直送 20160824
                            bKinkyuChokusou = bChokusou;

                            //直送時送品区分などは変更不可とする(出荷入力画面と同じ)　20170405 M0337 改めて反映 20180315
                            if (bChokusou)
                            {
                                if (str[(int)EnumDataField.SouhinKubun] != "4" ||
                                    drBecchuKey.NiukeJigyoushoCode != str[(int)EnumDataField.NiukeJigyoushoCode] ||
                                    drBecchuKey.HokanBasho != str[(int)EnumDataField.HokanBasho])
                                {
                                    throw new Exception("直送オーダーのため、送品区分・事業所・保管場所の変更はできません");
                                }
                            }

                            //** 緊急直送の条件 → 「直送オーダーではない AND 送品区分が直送(4)」
                            if (bChokusou == false && str[(int)EnumDataField.SouhinKubun].Trim() == "4")
                            {
                                //2013/12/04 岡村 猪川様からご連絡あり、一旦必須入力で無い状態として欲しいとの事
                                //if (str[(int)EnumDataField.KinkyuChokusouBikou].Trim() == "")
                                //{
                                //    throw new Exception(string.Format("オーダーNo{0}は緊急直送オーダーの為、緊急直送備考の入力が必要です。", strChumonNo));
                                //}

                                //緊急直送専用処理 20160824
                                int nError = 0;
                                //保管場所が「KKZK」ではない
                                if (str[(int)EnumDataField.HokanBasho] != "KKZK")
                                    nError = 1; //保管場所が違うだけ
                                //保管場所「KKZK」以外の事業所が設定されている
                                MizunoDataSet.M_NiukeBumonBashoDataTable dt =
                                    NiukeBumonClass.getM_NiukeBumonBashoSouhinKubun4DataTable(str[(int)EnumDataField.NiukeJigyoushoCode], c);
                                if (dt.Count == 0)
                                    if (nError != 1)
                                        nError = 2; //事業所が違うだけ
                                    else
                                        nError = 3; //保管場所と事業所が違う

                                if (nError != 0)
                                    switch (nError)
                                    {
                                        case 1:
                                            throw new Exception("送品区分が4：直送に変更されています。そのため荷受保管場所コードには「KKZK」しか入力できません。");
                                            break;
                                        case 2:
                                            throw new Exception(@"送品区分が4：直送に変更されています。そのため入力されている荷受事業所コードは設定できません。
事業所については、操作マニュアルダウンロード画面の緊急直送時出荷マニュアルをご確認下さい。");
                                            break;
                                        case 3:
                                            throw new Exception(@"送品区分が4：直送に変更されています。そのため入力されている荷受事業所コード・荷受保管場所コードは設定できません。
事業所については、操作マニュアルダウンロード画面の緊急直送時出荷マニュアルをご確認下さい。");
                                            break;
                                    }
                            }

                            //緊急直送専用処理 20161012
                            if (str[(int)EnumDataField.SouhinKubun].Trim() != "4")
                            {
                                if (str[(int)EnumDataField.HokanBasho] == "KKZK")
                                {
                                    throw new Exception(string.Format("オーダーNo：{0}の送品区分、事業所、保管場所の組み合わせが正しくありません", strChumonNo));
                                }
                            }


                            if (!drBecchuKey.IsCancelBiNull())
                                throw new Exception(string.Format("オーダーNo{0}はキャンセル済みです", strChumonNo));
                            if (drBecchuKey.KanryouFlg)
                                throw new Exception(string.Format("オーダーNo{0}は完了しています。", strChumonNo));

                            bData.BecchuOrderKey = new BecchuOrderClass.BecchuOrderKey(drBecchuKey.MizunoUketsukeBi, drBecchuKey.OrderKanriNo, drBecchuKey.ShiiresakiCode);

                            if (!drBecchuKey.IsT_Becchu_SashizuNoNull())
                            {
                                // 別注データ
                                BecchuKubun = BecchuOrderClass.EnumBecchuKubun.Becchu;
                                dtBecchu = BecchuOrderClass.getVIEW_Becchu_ShukkaMeisaiDataTable(bData.BecchuOrderKey, c);
                                dtBecchu.Columns.Add("HinbanTrim", typeof(string));
                                dtBecchu.Columns.Add("SizeTrim", typeof(string));
                                dtBecchu.Columns.Add("LotNoTrim", typeof(string));
                                for (int i = 0; i < dtBecchu.Count; i++)
                                {
                                    dtBecchu[i]["HinbanTrim"] = dtBecchu[i].Hinban.Trim();
                                    dtBecchu[i]["SizeTrim"] = dtBecchu[i].Size.Trim();
                                    dtBecchu[i]["LotNoTrim"] = dtBecchu[i].LotNo.Trim();
                                }

                                // 必須チェック 20160302 M0337 改めて反映 20180315
                                if (!drBecchuKey.IsHokanBashoNull())
                                {
                                    //  ※発注時に「DRCT」のデータはそのまま登録OK
                                    if (drBecchuKey.HokanBasho.Trim() != "DRCT" && str[(int)EnumDataField.HokanBasho] == "DRCT")
                                    {
                                        throw new Exception("発注内容で保管場所が設定されているので保管場所「DRCT」は選択出来ません");
                                    }

                                    //保管場所が設定されていたのに空白にするのはダメです。
                                    if (drBecchuKey.HokanBasho != "" && str[(int)EnumDataField.HokanBasho] == "")
                                    {
                                        throw new Exception("発注内容で保管場所が空白以外で設定されているので保管場所を空白以外で選択して下さい");
                                    }
                                }

                            }
                            else if (!drBecchuKey.IsT_Becchu2_SashizuNoNull())
                            {
                                // 別注2
                                BecchuKubun = BecchuOrderClass.EnumBecchuKubun.Becchu2;
                                dtBecchu2 = BecchuOrderClass.getVIEW_Becchu2_ShukkaMeisaiDataTable(bData.BecchuOrderKey, c);
                                dtBecchu2.Columns.Add("Zansu", typeof(decimal), "Suuryou-ShukkaSuu");
                                dtBecchu2.Columns.Add("HinbanTrim", typeof(string));
                                dtBecchu2.Columns.Add("SizeTrim", typeof(string));
                                dtBecchu2.Columns.Add("LotNoTrim", typeof(string));
                                for (int i = 0; i < dtBecchu2.Count; i++)
                                {
                                    dtBecchu2[i]["HinbanTrim"] = dtBecchu2[i].Hinban.Trim();
                                    dtBecchu2[i]["SizeTrim"] = dtBecchu2[i].Size.Trim();
                                    dtBecchu2[i]["LotNoTrim"] = dtBecchu2[i].LotNo.Trim();
                                }

                                // 必須チェック 20160302 M0337 改めて反映 20180315
                                if (!drBecchuKey.IsHokanBashoNull())
                                {
                                    //  ※発注時に「DRCT」のデータはそのまま登録OK
                                    if (drBecchuKey.HokanBasho.Trim() != "DRCT" && str[(int)EnumDataField.HokanBasho] == "DRCT")
                                    {
                                        throw new Exception("発注内容で保管場所が設定されているので保管場所「DRCT」は選択出来ません");
                                    }

                                    //保管場所が設定されていたのに空白にするのはダメです。
                                    if (drBecchuKey.HokanBasho != "" && str[(int)EnumDataField.HokanBasho] == "")
                                    {
                                        throw new Exception("発注内容で保管場所が空白以外で設定されているので保管場所を空白以外で選択して下さい");
                                    }
                                }
                            }
                            else if (!drBecchuKey.IsT_DS_OrderKanriNoNull())
                            {
                                // DS
                                BecchuKubun = BecchuOrderClass.EnumBecchuKubun.DS;
                                dtDS = BecchuOrderClass.getVIEW_DS2_ShukkaMeisaiDataTable(bData.BecchuOrderKey.OrderKanriNo, c);
                                dtDS.Columns.Add("HinbanTrim", typeof(string));
                                dtDS.Columns.Add("SizeTrim", typeof(string));
                                for (int i = 0; i < dtDS.Count; i++)
                                {
                                    dtDS[i]["HinbanTrim"] = dtDS[i].Hinban.Trim();
                                    dtDS[i]["SizeTrim"] = dtDS[i].Size.Trim();
                                }

                                // 必須チェック 20160302 M0337 改めて反映 20180315
                                if (!drBecchuKey.IsNiukeJigyoushoCodeNull())
                                {
                                    MizunoDataSet.T_DS2Row d = BecchuOrderClass.getT_DS2Row(drBecchuKey.OrderKanriNo, c);
                                    if (null != d)
                                    {
                                        if (!d.IsTeemNameNull())
                                        {
                                            //  ※発注時に「DRCT」のデータはそのまま登録OK
                                            if (d.TeemName.Trim() != "DRCT" && str[(int)EnumDataField.HokanBasho] == "DRCT")
                                            {
                                                throw new Exception("発注内容で保管場所が設定されているので保管場所「DRCT」は選択出来ません");
                                            }

                                            //保管場所が設定されていたのに空白にするのはダメです。
                                            if (d.TeemName != "" && str[(int)EnumDataField.HokanBasho] == "")
                                            {
                                                throw new Exception("発注内容で保管場所が空白以外で設定されているので保管場所を空白以外で選択して下さい");
                                            }
                                        }
                                    }
                                }
                            }
                            else if (!drBecchuKey.IsT_SP_OrderKanriNoNull())
                            {
                                // SP
                                BecchuKubun = BecchuOrderClass.EnumBecchuKubun.SP;
                                dtSP = BecchuOrderClass.getVIEW_SP_ShukkaMeisaiDataTable(bData.BecchuOrderKey, c);
                                dtSP.Columns.Add("HinbanTrim", typeof(string));
                                dtSP.Columns.Add("SizeTrim", typeof(string));
                                //20200325 LotNo追加 LotNoがないことで、伝票データアップロード時にLotNoの違いによるエラーが発生したため
                                dtSP.Columns.Add("LotNoTrim", typeof(string));
                                for (int i = 0; i < dtSP.Count; i++)
                                {
                                    dtSP[i]["HinbanTrim"] = dtSP[i].Hinban.Trim();
                                    dtSP[i]["SizeTrim"] = dtSP[i].Size.Trim();
                                    //20200325 LotNo追加 LotNoがないことで、伝票データアップロード時にLotNoの違いによるエラーが発生したため
                                    dtSP[i]["LotNoTrim"] = dtSP[i].LotNo.Trim();
                                }

                                // 必須チェック 20160302 M0337 改めて反映 20180315
                                if (!drBecchuKey.IsNiukeJigyoushoCodeNull())
                                {
                                    MizunoDataSet.T_SPRow d = BecchuOrderClass.getT_SPRow(drBecchuKey.OrderKanriNo, c);
                                    if (null != d)
                                    {
                                        if (!d.IsNiukeHokanBashoNull())
                                        {
                                            //  ※発注時に「DRCT」のデータはそのまま登録OK
                                            if (d.NiukeHokanBasho.Trim() != "DRCT" && str[(int)EnumDataField.HokanBasho] == "DRCT")
                                            {
                                                throw new Exception("発注内容で保管場所が設定されているので保管場所「DRCT」は選択出来ません");
                                            }

                                            //保管場所が設定されていたのに空白にするのはダメです。
                                            if (d.NiukeHokanBasho != "" && str[(int)EnumDataField.HokanBasho] == "")
                                            {
                                                throw new Exception("発注内容で保管場所が空白以外で設定されているので保管場所を空白以外で選択して下さい");
                                            }
                                        }
                                    }
                                }
                            }
                            else
                                throw new Exception("発注データが存在しません。");  // 予算品、別注品共にデータがない


                            // ｽﾍﾟｸﾄﾗｵｰﾀﾞｰ№
                            bData.BecchuSpectraNo = str[(int)EnumDataField.SPNo].Trim();
                            // 運送業者名
                            bData.UnsouGyoushaMei = str[(int)EnumDataField.UnsouGyoushaMei].Trim();
                            // 送り状№
                            bData.OkuriJyouNo = str[(int)EnumDataField.OkuriJyouNo].Trim();
                            // INVOICE№
                            //処理追加　送り状にデータが有る場合はInvoiceNoに送り状データを入れ、無い場合はInvoiceをInvoiceNoに入れる 20151217
                            if (str[(int)EnumDataField.OkuriJyouNo].Trim() != "")
                                bData.INVOICE = str[(int)EnumDataField.OkuriJyouNo].Trim();
                            else
                                bData.INVOICE = str[(int)EnumDataField.INVOICE].Trim();

                            //2013/11/08 岡村
                            // 緊急直送備考
                            bData.KinkyuChokusouBikou = str[(int)EnumDataField.KinkyuChokusouBikou].Trim();

                            //20160106 運送業者チェック機能追加
                            if (str[(int)EnumDataField.UnsouGyoushaCD].Trim() != "")
                            {
                                MizunoDataSet.M_UnsouGyoushaRow drU = UnsouGyoushaClass.getM_UnsouGyoushaRow(str[(int)EnumDataField.UnsouGyoushaCD].Trim(), c);
                                if (drU == null) throw new Exception("運送業者コードが運送業者マスタに存在しません。");
                            }
                            //2014/03/31 岡村 追加
                            bData.UnsouGyoushaCD = str[(int)EnumDataField.UnsouGyoushaCD].Trim();

                            //2014/03/31 岡村 追加
                            bData.KogutiSu = str[(int)EnumDataField.KogutiSu].Trim();

                            bData.TourokuData = new BecchuOrderClass.TourokuData();
                            lstTourokuData.Add(bData);
                            denpyou = bData.TourokuData;

                            // 当該種目コードに対応する追加品番取得
                            if (null != dtTuikaHinban4Becchu && 0 < dtTuikaHinban4Becchu.Count && dtTuikaHinban4Becchu[0].ShumokuCode == drBecchuKey.ShumokuCode)
                            {
                                // 前回取得データを再利用
                            }
                            else
                            {
                                dtTuikaHinban4Becchu = BecchuOrderClass.getM_TsuikaHinbanDataTable(drBecchuKey.ShumokuCode, c);
                                for (int i = 0; i < dtTuikaHinban4Becchu.Count; i++)
                                {
                                    dtTuikaHinban4Becchu[i].Hinban = dtTuikaHinban4Becchu[i].Hinban.Trim(); // 品番の頭に空白があるので
                                }
                            }
                        }


                        // 送品区分
                        try
                        {
                            denpyou.SouhinKubun = (EnumSouhinKubun)int.Parse(str[(int)EnumDataField.SouhinKubun]);
                        }
                        catch
                        {
                            throw new Exception("送品区分を正しく入力してください。");
                        }

                        // 伝票発行日
                        try
                        {
                            denpyou.dtHakkouBi = new Core.Type.Nengappi(int.Parse(str[(int)EnumDataField.DenpyouHakkouBi])).ToDateTime();
                        }
                        catch
                        {
                            throw new Exception("伝票発行日は8桁で入力してください。");
                        }
                        if (new Core.Type.Nengappi(denpyou.dtHakkouBi).ToYYYYMMDD() < Core.Type.Nengappi.Today.ToYYYYMMDD())
                            throw new Exception("伝票発行日は本日以降の日付を入力してください。");
                        else if (new Core.Type.Nengappi(denpyou.dtHakkouBi).ToYYYYMMDD() > Core.Type.Nengappi.Parse("2099/12/31").ToYYYYMMDD())
                            throw new Exception("伝票発行日は2099/12/31以前の日付を入力してください。");

                        denpyou.HokanBasho = str[(int)EnumDataField.HokanBasho];
                        denpyou.NiukeJigyoushoCode = str[(int)EnumDataField.NiukeJigyoushoCode];
                        //処理追加　送り状にデータが有る場合はInvoiceNoに送り状データを入れ、無い場合はInvoiceをInvoiceNoに入れる 20151217
                        if (str[(int)EnumDataField.OkuriJyouNo].Trim() != "")
                            denpyou.invoiceNo = str[(int)EnumDataField.OkuriJyouNo].Trim();
                        else
                            denpyou.invoiceNo = str[(int)EnumDataField.INVOICE];
                        denpyou.KinkyuChokusouBikou = str[(int)EnumDataField.KinkyuChokusouBikou];
                        denpyou.UnsouGyoushaCode = str[(int)EnumDataField.UnsouGyoushaCD];
                        denpyou.UnsouGyoushaMei = str[(int)EnumDataField.UnsouGyoushaMei];
                        denpyou.KogutiSu = str[(int)EnumDataField.KogutiSu];

                        MizunoDataSet.M_NiukeBumonBashoDataTable dtNiuke = NiukeBumonClass.getM_NiukeBumonBashoDataTable(denpyou.NiukeJigyoushoCode, denpyou.HokanBasho, c);
                        if (0 == dtNiuke.Count)
                        {
                            throw new Exception(string.Format("オーダーNo：{0}の送品区分、事業所、保管場所の組み合わせが正しくありません", strChumonNo));
                        }
                        denpyou.NiukeJigyoushoMei = dtNiuke[0].NiukeBumonMei;
                        denpyou.NiukeBashoMei = dtNiuke[0].NiukeBashoMei;
                        denpyou.NouhinKigou = StrCnvToHankaku(str[(int)EnumDataField.NouhinKigou]).ToUpper();   // 大文字で

                        // 納品記号は必須項目ではない為、不要？
                        //if (null == NouhinKigouClass.getM_NouhinKigouRow(denpyou.NouhinKigou, c))
                        //    throw new Exception(string.Format("オーダーNo：{0}の納品記号が正しくありません", strChumonNo));

                        //緊急直送は除外 20160824
                        if (!((str[(int)EnumDataField.SouhinKubun].Trim() == "4") &&
                            (0 < dtProper.Count) ||
                            (0 == dtProper.Count && !bKinkyuChokusou)))
                        {
                            if (!NiukeBumonClass.CheckBumonBasho(denpyou.SouhinKubun, denpyou.NiukeJigyoushoCode, denpyou.HokanBasho, denpyou.NouhinKigou, c))
                                throw new Exception(string.Format("オーダーNo：{0}の送品区分、事業所、保管場所の組み合わせが正しくありません", strChumonNo));
                        }

                        // 以下、半角チェックは済んでいる
                        denpyou.SKTantoushaCode = "";   // アップロード時はコードはブランク
                        denpyou.SKTantoushaMei = str[(int)EnumDataField.SeisanTantoushaMei];

                        denpyou.EigyouTantoushaCode = "";
                        denpyou.EigyouTantoushaMei = str[(int)EnumDataField.EigyouTantoushaMei];

                        denpyou.HanbaitenKigyouRyakuMei = str[(int)EnumDataField.Okurisaki];
                        denpyou.TenMei = str[(int)EnumDataField.TenMei];
                        denpyou.TeamMei = str[(int)EnumDataField.TeamMei];
                    }
                    else
                    {
                        // 2行目以降

                        // 伝票の共通項目が前行と同じかどうかチェック
                        for (int i = 0; i < str.Length; i++)
                        {
                            switch ((EnumDataField)i)
                            {
                                case EnumDataField.HacchuBi:
                                case EnumDataField.HacchuNo:
                                case EnumDataField.SouhinKubun:
                                case EnumDataField.NiukeJigyoushoCode:
                                case EnumDataField.HokanBasho:
                                case EnumDataField.SPNo:
                                case EnumDataField.UnsouGyoushaMei:
                                case EnumDataField.OkuriJyouNo:
                                case EnumDataField.SeisanTantoushaMei:
                                case EnumDataField.EigyouTantoushaMei:
                                case EnumDataField.Okurisaki:
                                case EnumDataField.TenMei:
                                case EnumDataField.TeamMei:
                                    if (!str[i].Equals(strPrevData[i]))
                                        throw new Exception(string.Format("{0}が{1}行目と異なっている為、行Noを1から開始して下さい", strFieldMei[i], nLine - 1));
                                    break;
                            }
                        }

                        object objTourokuData = lstTourokuData[lstTourokuData.Count - 1];   // 現在の伝票を取得
                        pData = null; bData = null;
                        if (objTourokuData is ProperOrderTourokuData)
                        {
                            // この伝票が予算品の出荷伝票の場合
                            int nCol = (int)EnumDataField.YosanTuki;
                            if (!str[nCol].Equals(strPrevData[nCol]))   // 予算月が全行と同じかﾁｪｯｸ
                                return new Error(string.Format("予算月が{0}行目と異なっている為、行Noを1から開始して下さい", nLine - 1));
                            pData = objTourokuData as ProperOrderTourokuData;
                            denpyou = pData.TourokuData;
                        }
                        else
                        {
                            bData = objTourokuData as BecchuOrderTourokuData;
                            denpyou = bData.TourokuData;
                        }
                    }

                    strPrevData = str;  // 確認



                    // ----- 明細 -----
                    NouhinDataClass.MeisaiDataCommon meisai = null;
                    ProperOrderClass.MeisaiData meisai_p = null;
                    BecchuOrderClass.MeisaiData meisai_b = null;
                    if (null != pData)
                        meisai = meisai_p = new ProperOrderClass.MeisaiData();
                    else
                        meisai = meisai_b = new BecchuOrderClass.MeisaiData();

                    meisai.Tekiyou = str[(int)EnumDataField.Tekiyou].Trim();
                    switch (meisai.NouhinKubun)
                    {
                        case EnumNouhinKubun.TokuisakiHimoduke:
                        case EnumNouhinKubun.OrderHimoduke:
                            // 摘要が必須のはずだけど
                            if ("" == meisai.Tekiyou)
                            {
                                throw new Exception("納品区分が1もしくは2の場合は摘要を入力してください。");
                            }
                            break;
                    }

                    meisai.Tekiyou = str[(int)EnumDataField.Tekiyou];
                    string strHinban = StrCnvToHankaku(str[(int)EnumDataField.Hinban].Trim());
                    string strSize = str[(int)EnumDataField.Size].Trim();
                    string strLotNo = str[(int)EnumDataField.LotNo].Trim();

                    string strTokushuHinbanCode = th.GetHinban(strHinban);  // 特殊品番の場合は、「運賃」とかが入力されている

                    meisai.TuikaHinban = !string.IsNullOrEmpty(strTokushuHinbanCode);
                    meisai.ShukkaSu = int.Parse(str[(int)EnumDataField.Suuryou]);

                    if (meisai.TuikaHinban)
                    {
                        // 追加品番の場合
                        if (null != pData)
                        {
                            meisai_p.Key.Hinban = "ﾄｸｼｭﾋﾝ-" + strTokushuHinbanCode;
                            meisai_p.Key.Size = "";
                            // 軽減税率　追加 20190821 M0458
                            MizunoDataSet.M_HinmokuRow drHinmoku = HinbanClass.getM_HinmokuRow(meisai_p.Key.Hinban, meisai_p.Key.Size, c);
                            string strKeigenZeiritsu = "";
                            if (drHinmoku != null)
                            {
                                if (!drHinmoku.IsKeigenZeiritsuFlgNull())
                                    strKeigenZeiritsu = drHinmoku.KeigenZeiritsuFlg.ToString();
                            }
                            meisai_p.KeigenZeiritsu = strKeigenZeiritsu;
                            pData.AddMeisai(bGyouNo, meisai_p);
                        }
                        else
                        {
                            meisai_b.Key.Hinban = "ﾄｸｼｭﾋﾝ-" + strTokushuHinbanCode;
                            meisai_b.Key.Size = "";
                            // 軽減税率　追加 20190821 M0458
                            MizunoDataSet.M_HinmokuRow drHinmoku = HinbanClass.getM_HinmokuRow(meisai_b.Key.Hinban, meisai_b.Key.Size, c);
                            string strKeigenZeiritsu = "";
                            if (drHinmoku != null)
                            {
                                if (!drHinmoku.IsKeigenZeiritsuFlgNull())
                                    strKeigenZeiritsu = drHinmoku.KeigenZeiritsuFlg.ToString();
                            }
                            meisai_b.KeigenZeiritsu = strKeigenZeiritsu;
                            bData.AddMeisai(bGyouNo, meisai_b);
                        }
                        meisai.KanryouFlag = true;  // 自動的に完了
                        if (0 >= meisai.ShukkaSu) throw new Exception("数量は1以上で入力して下さい");   // 追加品番で数量0は有り得ない
                    }
                    else
                    {
                        if (null != pData)
                        {
                            // 予算品の明細取得
                            daProper.SelectCommand.Parameters["@z"].Value = pData.ProperOrderKey.SashizuBi;
                            daProper.SelectCommand.Parameters["@s"].Value = pData.ProperOrderKey.SeisanOrderNo;
                            daProper.SelectCommand.Parameters["@h"].Value = pData.ProperOrderKey.HikitoriOrderNo;
                            daProper.SelectCommand.Parameters["@y"].Value = pData.YosanTuki.ToYYYYMM();
                            daProper.SelectCommand.Parameters["@Hinban"].Value = strHinban;
                            daProper.SelectCommand.Parameters["@Size"].Value = strSize;

                            MizunoDataSet.T_ProperOrderDataTable dtProper = new MizunoDataSet.T_ProperOrderDataTable();
                            daProper.Fill(dtProper);
                            if (0 == dtProper.Count) throw new Exception("発注データが存在しません。");
                            if (long.Parse(dtProper[0].ShiiresakiCode) != long.Parse(strShiiresakiCode))
                                throw new Exception("仕入先コードが正しくありません。");
                            meisai_p.Key.Hinban = dtProper[0].Hinban;   // DBの品番をセットする。（頭に空白があったりするので、DBの内容に合わせる為）
                            meisai_p.Key.Size = dtProper[0].Size;


                            int nZansu = 0;
                            for (int k = 0; k < dtProper.Count; k++)
                            {
                                MizunoDataSet.T_ProperOrderRow drP = dtProper[k];
                                if (drP.KanryouFlg) continue;
                                int n = drP.Suuryou - drP.ShukkaSuu;
                                if (0 >= n) continue;

                                if (string.IsNullOrEmpty(pData.NouhinKigou))
                                    pData.NouhinKigou = drP.NouhinKigou;
                                else
                                {
                                    if (pData.NouhinKigou != drP.NouhinKigou)
                                    {
                                        // 基本的に、注文内の品番+サイズで納品記号が異なるケースは無いが、念の為
                                        throw new Exception(string.Format("納品記号が一致しません。", strChumonNo));
                                    }
                                }

                                // 必須チェック 元々別の保管場所が設定されている状態で保管場所が「DRCT」の場合はエラー 20160302 M0337 改めて反映 20180315
                                if (dtProper[k].HokanBasho != "")
                                {
                                    //保管場所が設定されていたのに空白にするのはダメです。
                                    if (dtProper[k].HokanBasho.Trim() != "" && str[(int)EnumDataField.HokanBasho] == "")
                                    {
                                        throw new Exception("発注内容で保管場所が空白以外で設定されているので保管場所を空白以外で選択して下さい");
                                    }
                                }

                                // ☆同一品番、サイズで価格が異なるケースのチェックも追加した方が良いだろうか？

                                // 2013.02.28 アップロード時に納品データに仕入単価が登録されない不具合を修正
                                meisai_p.TourokuKakaku = drP.Kakaku;

                                nZansu += n;
                            }
                            if (0 == nZansu)
                                throw new Exception(string.Format("発注データが存在しません。予算品オーダーNo：{0}の注文は終了しています。", strChumonNo));

                            // 今回指定の事業所で引き当て可能なので事業所指定で登録する。
                            meisai_p.Key.NiukeJigyoushoCode = denpyou.NiukeJigyoushoCode;
                            meisai_p.Key.NiukeJigyoushoCodeHikiateAll = false;  // 同一品番サイズで1注文に複数の事業所があり、引き当て対象となる事業所が確定できないので、伝票の事業所から引き当てられる分だけ先に引き当てる。

                            // 軽減税率　追加 20190821 M0458
                            MizunoDataSet.M_HinmokuRow drHinmoku = HinbanClass.getM_HinmokuRow(meisai_p.Key.Hinban, meisai_p.Key.Size, c);
                            string strKeigenZeiritsu = "";
                            if (drHinmoku != null)
                            {
                                if (!drHinmoku.IsKeigenZeiritsuFlgNull())
                                    strKeigenZeiritsu = drHinmoku.KeigenZeiritsuFlg.ToString();
                            }
                            meisai_p.KeigenZeiritsu = strKeigenZeiritsu;

                            if (!pData.AddMeisai(bGyouNo, meisai_p))
                                throw new Exception("行Noが重複しています。");

                            // 完了フラグのチェック
                            string strKey = string.Format("{0}_{1}_{2}_{3}_{4}",
                                pData.ProperOrderKey.SashizuBi, pData.ProperOrderKey.HikitoriOrderNo, pData.ProperOrderKey.SeisanOrderNo, meisai_p.Key.Hinban, meisai_p.Key.Size);
                            ProperOrderClass.TourokuData pd = (tblProperKanroyuData.ContainsKey(strKey)) ? tblProperKanroyuData[strKey] : null;

                            if (meisai.KanryouFlag)
                            {
                                // 別の伝票で既に完了フラグがセットされていればエラー
                                if (null == pd)
                                {
                                    tblProperKanroyuData.Add(strKey, pData.TourokuData);
                                }
                                else
                                {
                                    if (pd != pData.TourokuData)
                                        throw new Exception(string.Format("予算品オーダーNo：{0}、品番:{1}、サイズ:{2}の完了フラグが別の伝票で既に登録されています。",
                                            strChumonNo, meisai_p.Key.Hinban, meisai_p.Key.Size));
                                }
                            }
                            else
                            {
                                // 当該品番+サイズで完了フラグが立っていても出荷日が後であればＯＫ
                                if (null != pd && pd != pData.TourokuData)
                                {
                                    if (pd.dtHakkouBi <= pData.TourokuData.dtHakkouBi)
                                    {
                                        throw new Exception(string.Format("予算品オーダーNo：{0}、品番:{1}、サイズ:{2}の完了フラグは伝票発行日{0:yyyy/MM/dd}の伝票で既にセットされています。",
                                            strChumonNo, meisai_p.Key.Hinban, meisai_p.Key.Size, pd.dtHakkouBi));
                                    }
                                }
                            }
                        }
                        else
                        {
                            bool bFoundChumonData = false;

                            switch (BecchuKubun)
                            {
                                case BecchuOrderClass.EnumBecchuKubun.Becchu:
                                    dtBecchu.DefaultView.RowFilter = string.Format("HinbanTrim='{0}' and SizeTrim='{1}' and LotNoTrim='{2}'", strHinban.Replace("'", "''"), strSize.Replace("'", "''"), strLotNo.Replace("'", "''"));
                                    bFoundChumonData = (0 < dtBecchu.DefaultView.Count);
                                    if (bFoundChumonData)
                                    {
                                        ViewDataset.VIEW_Becchu_ShukkaMeisaiRow drBecchu = dtBecchu.DefaultView[0].Row as ViewDataset.VIEW_Becchu_ShukkaMeisaiRow;
                                        meisai_b.Key.Hinban = drBecchu.Hinban;
                                        meisai_b.Key.Size = drBecchu.Size;
                                        meisai_b.Key.LotNo = drBecchu.LotNo;

                                        // 2013.02.28 アップロード時に納品データに仕入単価が登録されない不具合を修正
                                        decimal dTourokuKakaku = 0;
                                        if (!drBecchu.IsKakakuNull())
                                        {
                                            string strKakaku = drBecchu.Kakaku.Replace(",", "").Replace("\\", "");
                                            if (!decimal.TryParse(strKakaku, out dTourokuKakaku))
                                            {
                                                throw new Exception(
                                                    string.Format("別注品オーダーNo：{0}、品番：{1}、サイズ：{2}、ロットNo：{3}の価格が不正です。",
                                                    drBecchu.SashizuNo, drBecchu.Hinban, drBecchu.Size, drBecchu.LotNo));
                                            }
                                        }
                                        meisai.TourokuKakaku = dTourokuKakaku;
                                    }
                                    break;
                                case BecchuOrderClass.EnumBecchuKubun.Becchu2:
                                    {
                                        dtBecchu2.DefaultView.RowFilter = string.Format("HinbanTrim='{0}' AND SizeTrim='{1}' AND LotNoTrim='{2}' AND Suuryou>ShukkaSuu",
                                            strHinban.Replace("'", "''"), strSize.Replace("'", "''"), strLotNo.Replace("'", "''"));

                                        if (dtBecchu2.DefaultView.Count > 1)
                                        {
                                            // 行No
                                            try
                                            {
                                                meisai_b.Key.RowNo = int.Parse(StrCnvToHankaku(str[(int)EnumDataField.BecchuHin_RowNo].Trim()));
                                            }
                                            catch
                                            {
                                                throw new Exception(string.Format("別注品オーダーNo：{0}、品番：{1}、サイズ：{2}、ロットNo：{3}は引き当てができないため、別注品明細Noを追加してください。",
                                                    strChumonNo, strHinban, strSize, strLotNo));
                                            }

                                            dtBecchu2.DefaultView.RowFilter = string.Format("HinbanTrim='{0}' AND SizeTrim='{1}' AND LotNoTrim='{2}' AND RowNo={3} AND Suuryou>ShukkaSuu",
                                                strHinban.Replace("'", "''"), strSize.Replace("'", "''"), strLotNo.Replace("'", "''"), meisai_b.Key.RowNo);
                                        }

                                        /* 2013.02.28 
                                         * 下記ソースだと、ロットNoがブランクのときは必ず別注品明細Noが必要になり、不親切なので、上記ソースに修正
                                         * 品番、サイズ、ロットNoで引き当てができてない場合は、品番、サイズ、ロットNo、別注品明細Noで引き当てる。
                                        if (!string.IsNullOrEmpty(strLotNo))
                                        {
                                            dtBecchu2.DefaultView.RowFilter = string.Format("HinbanTrim='{0}' AND SizeTrim='{1}' AND LotNoTrim='{2}' AND Suuryou>ShukkaSuu",
                                                strHinban.Replace("'", "''"), strSize.Replace("'", "''"), strLotNo.Replace("'", "''"));
                                        }
                                        else
                                        {
                                            // 行No
                                            try
                                            {
                                                meisai_b.Key.RowNo = int.Parse(StrCnvToHankaku(str[(int)EnumDataField.BecchuHin_RowNo].Trim()));
                                            }
                                            catch
                                            {
                                                throw new Exception("ロットNo.を入力するか、別注品明細Noを正しく入力してください。");
                                            }

                                            dtBecchu2.DefaultView.RowFilter = string.Format("HinbanTrim='{0}' AND SizeTrim='{1}' AND RowNo={2} AND Suuryou>ShukkaSuu",
                                                strHinban.Replace("'", "''"), strSize.Replace("'", "''"), meisai_b.Key.RowNo);
                                        }
                                         */

                                        bFoundChumonData = (0 < dtBecchu2.DefaultView.Count);

                                        if (bFoundChumonData)
                                        {
                                            ViewDataset.VIEW_Becchu2_ShukkaMeisaiRow drBecchu2 = dtBecchu2.DefaultView[0].Row as ViewDataset.VIEW_Becchu2_ShukkaMeisaiRow;
                                            meisai_b.Key.Hinban = drBecchu2.Hinban;
                                            meisai_b.Key.Size = drBecchu2.Size;
                                            meisai_b.Key.LotNo = drBecchu2.LotNo;
                                            meisai_b.Key.RowNo = drBecchu2.RowNo;

                                            // 2013.02.28 アップロード時に納品データに仕入単価が登録されない不具合を修正
                                            decimal dTourokuKakaku = 0;
                                            string strKakaku = drBecchu2.Kakaku.Replace(",", "").Replace("\\", "");
                                            if (!decimal.TryParse(strKakaku, out dTourokuKakaku))
                                            {
                                                throw new Exception(
                                                    string.Format("別注品オーダーNo：{0}、品番：{1}、サイズ：{2}、ロットNo：{3}の価格が不正です。",
                                                    drBecchu2.SashizuNo, drBecchu2.Hinban, drBecchu2.Size, drBecchu2.LotNo));
                                            }
                                            meisai.TourokuKakaku = dTourokuKakaku;
                                        }
                                    }
                                    break;
                                case BecchuOrderClass.EnumBecchuKubun.DS:
                                    dtDS.DefaultView.RowFilter = string.Format("HinbanTrim='{0}' AND SizeTrim='{1}'", strHinban.Replace("'", "''"), strSize.Replace("'", "''"));
                                    bFoundChumonData = (0 < dtDS.DefaultView.Count);
                                    if (bFoundChumonData)
                                    {
                                        //meisai_b.Key.Hinban = (dtDS.DefaultView[0].Row as ViewDataset.VIEW_DS_ShukkaMeisaiRow).Hinban;
                                        //meisai_b.Key.Size = (dtDS.DefaultView[0].Row as ViewDataset.VIEW_DS_ShukkaMeisaiRow).Size;
                                        meisai_b.Key.Hinban = (dtDS.DefaultView[0].Row as ViewDataset.VIEW_DS2_ShukkaMeisaiRow).Hinban;
                                        meisai_b.Key.Size = (dtDS.DefaultView[0].Row as ViewDataset.VIEW_DS2_ShukkaMeisaiRow).Size;
                                        meisai_b.Key.LotNo = (dtDS.DefaultView[0].Row as ViewDataset.VIEW_DS2_ShukkaMeisaiRow).LotNo;
                                    }
                                    break;
                                case BecchuOrderClass.EnumBecchuKubun.SP:
                                    {
                                        //20200325 LotNo追加 LotNoがないことで、伝票データアップロード時にLotNoの違いによるエラーが発生したため
                                        dtSP.DefaultView.RowFilter = string.Format("HinbanTrim='{0}' AND SizeTrim='{1}' AND LotNoTrim='{2}'", strHinban.Replace("'", "''"), strSize.Replace("'", "''"), strLotNo.Replace("'", "''"));
                                        bFoundChumonData = (0 < dtSP.DefaultView.Count);
                                        if (bFoundChumonData)
                                        {
                                            ViewDataset.VIEW_SP_ShukkaMeisaiRow drSp = dtSP.DefaultView[0].Row as ViewDataset.VIEW_SP_ShukkaMeisaiRow;
                                            meisai_b.Key.Hinban = drSp.Hinban;
                                            meisai_b.Key.Size = drSp.Size;
                                            meisai_b.Key.LotNo = drSp.LotNo;
                                            // 2013.02.28 アップロード時に納品データに仕入単価が登録されない不具合を修正
                                            meisai.TourokuKakaku = drSp.IsKingakuNull() ? 0 : drSp.Kingaku;
                                        }
                                    }

                                    break;
                            }

                            if (!bFoundChumonData)
                            {
                                // 注文データ中になくても追加品番の可能性がある
                                if (null == dtTuikaHinban4Becchu.FindByHinbanShumokuCode(strHinban, drBecchuKey.ShumokuCode))
                                    throw new Exception("発注データ(追加品番)が存在しません。(品番=" + strHinban + ")");
                                else
                                {
                                    // 追加品番である
                                    meisai.TuikaHinban = true;
                                    meisai_b.Key.Hinban = strHinban;
                                    meisai_b.Key.Size = strSize;    // 追加品番（特殊品番でない）の場合はサイズ指定が可能

                                    meisai_b.Key.LotNo = strLotNo; // 2012/12/12 岡村追加
                                }
                            }

                            // 軽減税率　追加 20190821 M0458
                            MizunoDataSet.M_HinmokuRow drHinmoku = HinbanClass.getM_HinmokuRow(meisai_b.Key.Hinban, meisai_b.Key.Size, c);
                            string strKeigenZeiritsu = "";
                            if (drHinmoku != null)
                            {
                                if (!drHinmoku.IsKeigenZeiritsuFlgNull())
                                    strKeigenZeiritsu = drHinmoku.KeigenZeiritsuFlg.ToString();
                            }
                            meisai_b.KeigenZeiritsu = strKeigenZeiritsu;

                            if (!bData.AddMeisai(bGyouNo, meisai_b))
                                throw new Exception("行Noが重複しています。");
                        }



                        // 完了フラグ
                        string strKanryouFlg = str[(int)EnumDataField.KanryouFlg].Trim();
                        if ("0" != strKanryouFlg && "9" != strKanryouFlg) throw new Exception("完了フラグは0または9を入力してください");
                        meisai.KanryouFlag = (KanryouFlg.Kanryou == strKanryouFlg);

                        if (0 > meisai.ShukkaSu) throw new Exception("数量は0以上で入力して下さい");
                        if (KanryouFlg.MiKanryou == strKanryouFlg && 0 == meisai.ShukkaSu) throw new Exception("完了フラグが0の時は、数量を1以上で入力して下さい");
                        if (meisai.KanryouFlag && 0 == meisai.ShukkaSu) nSuryouZeroDataCount++;
                        // 「数量0、完了フラグ = 9」のデータと、「数量1以上」のデータが1伝票内に共存してはいけない。
                        if (0 < meisai.ShukkaSu && 0 < nSuryouZeroDataCount)
                        {
                            throw new Exception(string.Format("オーダーNo：{0}:数量=0、完了フラグ=9のデータは数量1以上のデータと伝票を分けて下さい", strChumonNo));
                        }

                        // 自由使用欄
                        meisai.Free1 = str[(int)EnumDataField.JiyuKoumoku1];
                        meisai.Free2 = str[(int)EnumDataField.JiyuKoumoku2];
                        meisai.Free3 = str[(int)EnumDataField.JiyuKoumoku3];
                        // 31桁備考
                        meisai.Bikou = "";

                    }


                }
                catch (Exception e)
                {
                    strPrevData = str;
                    lstErrorMsg.Add(string.Format("{0}行目 - {1}", nLine, e.Message));
                }

            }

            if (0 < lstErrorMsg.Count) return new Error("登録に失敗しました。");

            if (0 == lstTourokuData.Count) return new Error("登録するデータがありません。");




            // 完了フラグをオフにする。
            SqlDataAdapter daBki = new SqlDataAdapter("", c);
            daBki.SelectCommand.CommandText = "SELECT * FROM T_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            daBki.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", "");
            daBki.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", "");
            daBki.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", "");
            daBki.UpdateCommand = new SqlCommandBuilder(daBki).GetUpdateCommand();
            MizunoDataSet.T_BecchuKeyInfoDataTable dtBecchuKey = new MizunoDataSet.T_BecchuKeyInfoDataTable();



            SqlTransaction t = null;

            int nDenpyouIndex = 0;

            try
            {
                c.Open();
                t = c.BeginTransaction();

                daBki.SelectCommand.Transaction = daBki.UpdateCommand.Transaction = t;

                for (nDenpyouIndex = 0; nDenpyouIndex < lstTourokuData.Count; nDenpyouIndex++)
                {
                    List<NouhinDataClass.DenpyouKey> lst = null;
                    List<NouhinDataClass.DenpyouKey> lstShukkaAri = null;
                    if (lstTourokuData[nDenpyouIndex] is ProperOrderTourokuData)
                    {
                        // 予算品
                        ProperOrderTourokuData p = lstTourokuData[nDenpyouIndex] as ProperOrderTourokuData;
                        p.SetMeisaiData();

                        int nMaxGyouSu = GetDenpyouGyouCount(p.TourokuData.SouhinKubun);
                        if (nMaxGyouSu < p.TourokuData.lstMeisai.Count)
                        {
                            string strChumonBangou = (!string.IsNullOrEmpty(p.ProperOrderKey.HikitoriOrderNo)) ?
                                p.ProperOrderKey.HikitoriOrderNo : p.ProperOrderKey.SeisanOrderNo;
                            throw new Exception(string.Format("注文No.{0}が伝票作成可能な行数を超えています。(最大行数={1})",
                                strChumonBangou, nMaxGyouSu));
                        }

                        ProperOrderClass.DenpyouTouroku(true, false, p.ProperOrderKey, p.YosanTuki, p.NouhinKigou, p.TourokuData, t, out lst, out lstShukkaAri);
                    }
                    else
                    {
                        // 別注品
                        BecchuOrderTourokuData b = lstTourokuData[nDenpyouIndex] as BecchuOrderTourokuData;
                        b.SetMeisaiData();

                        int nMaxGyouSu = GetDenpyouGyouCount(b.TourokuData.SouhinKubun);
                        if (nMaxGyouSu < b.TourokuData.lstMeisai.Count)
                        {
                            throw new Exception(string.Format("注文No.{0}が伝票作成可能な行数を超えています。(最大行数={1})",
                                b.BecchuOrderKey.OrderKanriNo, nMaxGyouSu));
                        }

                        BecchuOrderClass.DenpyouTouroku(true, false, b.BecchuOrderKey, b.TourokuData, t, out lst, out lstShukkaAri);

                        daBki.SelectCommand.Parameters["@MizunoUketsukeBi"].Value = b.BecchuOrderKey.UketsukeBi;
                        daBki.SelectCommand.Parameters["@OrderKanriNo"].Value = b.BecchuOrderKey.OrderKanriNo;
                        daBki.SelectCommand.Parameters["@ShiiresakiCode"].Value = b.BecchuOrderKey.ShiiresakiCode;
                        dtBecchuKey.Clear();
                        daBki.Fill(dtBecchuKey);
                        if (0 == dtBecchuKey.Count) throw new Exception(string.Format("オーダーNo.{0}のキー情報がありません。", b.BecchuOrderKey.OrderKanriNo));

                        MizunoDataSet.T_BecchuKeyInfoRow dr = dtBecchuKey[0];

                        if (!string.IsNullOrEmpty(b.BecchuSpectraNo)) dr.BecchuSpectraNo = b.BecchuSpectraNo;   // これ正しいの？

                        if (!string.IsNullOrEmpty(b.OkuriJyouNo)) dr.OkuriJyouNo = b.OkuriJyouNo;
                        
                        if (!string.IsNullOrEmpty(b.UnsouGyoushaMei)) dr.UnsouGyoushaMei = b.UnsouGyoushaMei;
                        if (b.GoukeiKakaku.HasValue) dr.GoukeiKakaku = b.GoukeiKakaku.Value;
                        daBki.Update(dtBecchuKey);
                    }

                    if (0 < lst.Count) lstDenpyouKey.AddRange(lst);
                    if (null != lstShukkaAri && 0 < lstShukkaAri.Count) lstDenpyouKeyShukkaAri.AddRange(lstShukkaAri);
                }

                //一時的にコメントアウト 2013-02-04
                t.Commit();

                return null;
            }
            catch (Exception e)
            {
                if (null != t) t.Rollback();
                return new Core.Error(string.Format("{0}枚目の伝票 - {1}", nDenpyouIndex + 1, e.Message));
            }
            finally
            {
                c.Close();
            }

        }

        public static Core.Error ZUploadNouhnData(string strShiiresakiCode, System.IO.Stream stream,
            System.Text.Encoding enc, SqlConnection c, out List<string> lstErrorMsg,
            out List<NouhinDataClass.DenpyouKey> lstDenpyouKey, out List<NouhinDataClass.DenpyouKey> lstDenpyouKeyShukkaAri)
        {

            lstErrorMsg = new List<string>();
            lstDenpyouKey = new List<NouhinDataClass.DenpyouKey>();
            lstDenpyouKeyShukkaAri = new List<DenpyouKey>();
            
            //daZairyouKey.SelectCommand.Parameters.AddWithValue("@r", "");
            // 予算品データ取得、注文番号は、生産オーダーNoか引取オーダーNoのどちらか
//            SqlDataAdapter daProperKey = new SqlDataAdapter("", c);
//            daProperKey.SelectCommand.CommandText = @"
//SELECT                  dbo.T_ProperOrder.*
//FROM                     dbo.T_ProperOrder
//WHERE                   (SashizuBi = @z) AND ((SeisanOrderNo = @c AND ltrim(HikitoriOrderNo) = '') OR (ltrim(SeisanOrderNo)='' AND HikitoriOrderNo = @c))";
//            daProperKey.SelectCommand.Parameters.AddWithValue("@z", "");
//            daProperKey.SelectCommand.Parameters.AddWithValue("@c", "");
//            // 予算月の件数取得
//            SqlDataAdapter daGetYosanTuki = new SqlDataAdapter("", c);
//            daGetYosanTuki.SelectCommand.CommandText = @"
//SELECT                  MAX(YosanTsuki) AS YosanTsuki, COUNT(DISTINCT YosanTsuki) AS YosanTsukiCount
//FROM                     dbo.T_ProperOrder
//WHERE                   (SashizuBi = @z) AND (SeisanOrderNo = @s) AND (HikitoriOrderNo = @h)";
//            daGetYosanTuki.SelectCommand.Parameters.AddWithValue("@z", "");
//            daGetYosanTuki.SelectCommand.Parameters.AddWithValue("@s", "");
//            daGetYosanTuki.SelectCommand.Parameters.AddWithValue("@h", "");

//            // 別注品キー情報　MizunoUketsukeBiが文字列でいい加減なので日付で検索　yyyy/MM/ddの形式になっているとは限らない (2011/1/31のようなデータもある)
//            SqlDataAdapter daBecchu = new SqlDataAdapter("", c);
//            daBecchu.SelectCommand.CommandText = "SELECT * FROM VIEW_BecchuKeyInfo WHERE OrderKanriNo=@ChumonBangou AND ShiiresakiCode = @s and CONVERT(char(8),CAST(MizunoUketsukeBi as date), 112) = @MizunoUketsukeBi AND ISDATE(MizunoUketsukeBi) = 1";
//            daBecchu.SelectCommand.Parameters.AddWithValue("@s", strShiiresakiCode);
//            daBecchu.SelectCommand.Parameters.AddWithValue("@ChumonBangou", "");
//            daBecchu.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", "");

//            // 予算品の品番+サイズの明細取得
//            SqlDataAdapter daProper = new SqlDataAdapter("", c);
//            daProper.SelectCommand.CommandText = @"
//SELECT                  dbo.T_ProperOrder.*
//FROM                     dbo.T_ProperOrder
//WHERE                   (SashizuBi = @z) AND SeisanOrderNo = @s AND HikitoriOrderNo = @h AND YosanTsuki=@y and (Ltrim(Hinban) = @Hinban) AND (Size = @Size)";
//            daProper.SelectCommand.Parameters.AddWithValue("@z", "");
//            daProper.SelectCommand.Parameters.AddWithValue("@s", "");
//            daProper.SelectCommand.Parameters.AddWithValue("@h", "");
//            daProper.SelectCommand.Parameters.AddWithValue("@y", 0);
//            daProper.SelectCommand.Parameters.AddWithValue("@Hinban", "");
//            daProper.SelectCommand.Parameters.AddWithValue("@Size", "");

            // 材料2012-07-27 14:09
            SqlDataAdapter daZairyouKey = new SqlDataAdapter("", c);
            daZairyouKey.SelectCommand.CommandText = @"
select dbo.T_ZairyouOrder.* 
from   dbo.T_ZairyouOrder 
where  (SashizuBi = @z) AND 
HikitoriOrderNo = @c ";
            daZairyouKey.SelectCommand.Parameters.AddWithValue("@z", "");
            daZairyouKey.SelectCommand.Parameters.AddWithValue("@c", "");

            SqlDataAdapter daZairyou = new SqlDataAdapter("", c);
            daZairyou.SelectCommand.CommandText = @"
select dbo.T_ZairyouOrder.*
from   dbo.T_ZairyouOrder
where  (SashizuBi = @z) AND HikitoriOrderNo = @h AND 
(Ltrim(Hinban) = @Hinban) AND LotNo=@lot ";
            //"AND (Size = @Size)";
            daZairyou.SelectCommand.Parameters.AddWithValue("@z", "");
            daZairyou.SelectCommand.Parameters.AddWithValue("@h", "");
            daZairyou.SelectCommand.Parameters.AddWithValue("@Hinban", "");
            daZairyou.SelectCommand.Parameters.AddWithValue("@lot", "");

            //------ファイル読み込み
            System.IO.StreamReader tabReader = null;
            Core.IO.CSVReader csvReader = null;

            System.IO.StreamReader check = new System.IO.StreamReader(stream, enc);
            string strCheck = check.ReadLine();  // CSVとタブ区切りの確認の為
            if (null == strCheck)
            {
                return new Core.Error("データがありません。");
            }
            bool bTab = (strCheck.Split('\t').Length > strCheck.Split(',').Length);   // これ良いの？
            stream.Position = 0;
            if (bTab)
                tabReader = new System.IO.StreamReader(stream, enc);
            else
                csvReader = new Core.IO.CSVReader(stream, enc);

            int[] nDataMaxLength = new int[]{
8,  // 1:仕入先ｺｰﾄﾞ
8,  // 2:指図日(ミズノ受付日)
40, // 3:ｵｰﾀﾞｰ№
//3,  
8,  // 4:伝票発行日
1,  // 5:送品区分
4,  // 6:荷受事業所ｺｰﾄﾞ
20,  // 7:保管場所ｺｰﾄﾞ
3,  // 8:納品記号
//6,  
2,  // 9行No
50, // 10:ロットNo
10, // 11:品番
5,  // 12:サイズ
//7,
//7,
5,  // 13:数量
1,  // 14:完了フラグ
//1,  
15, // 15:摘要
10,  // 16:生産担当者
10, // 17:営業担当者
10, // 18:送り先
10, // 19:店名
10, // 20:学校/チーム名
//20,     // 25項目目 ?
20, // 21:自由項目1
20, // 22:自由項目2
20, // 23:自由項目3
6,  // 24:予算品予算月(YYYYMM)
2,  // 25:別注2行No
14, // 26:スペクトラオーダーNo
15, // 27:運送業者名
15, // 28:送り状No
//10  
        };

            string[] strFieldMei = new string[]{            
"仕入先ｺｰﾄﾞ",
"指図日(ミズノ受付日)",
"ｵｰﾀﾞｰ№",
//"伝区",
"伝票発行日",
"送品区分",
"荷受事業所ｺｰﾄﾞ",
"保管場所ｺｰﾄﾞ",
"納品記号",
//"本納品月",
"行No",
"ロットNo.",    // 追加
"品番",
"サイズ",
//"仕入価格",
//"融通価格",
"数量",
"完了フラグ",
//"納品区分",
"摘要",
//"籍付",
"生産担当者",
"営業担当者",
"送り先",
"店名",
"学校/チーム名",
"自由使用欄1",
"自由使用欄2",
"自由使用欄3",
"予算月",
"別注品明細No",
"ｽﾍﾟｸﾄﾗｵｰﾀﾞｰ№",
"運送業者名",
"送り状№",
//"仕入合計金額"
            };

            TsuikaHinban th = new TsuikaHinban();

            NouhinDataClass.TourokuDataBase denpyou = null;
            //ProperOrderTourokuData pData = null;
            //BecchuOrderTourokuData bData = null;
            ZairyouOrderTourokuData zData = null;
            
            List<object> lstTourokuData = new List<object>();

            DateTime dtNow = DateTime.Now;
            //ViewDataset.VIEW_BecchuKeyInfoRow drBecchuKey = null;
            //BecchuOrderClass.EnumBecchuKubun BecchuKubun = BecchuOrderClass.EnumBecchuKubun.None;
            //ViewDataset.VIEW_Becchu_ShukkaMeisaiDataTable dtBecchu = null;
            //ViewDataset.VIEW_Becchu2_ShukkaMeisaiDataTable dtBecchu2 = null;
            //ViewDataset.VIEW_DS_ShukkaMeisaiDataTable dtDS = null;
            //ViewDataset.VIEW_SP_ShukkaMeisaiDataTable dtSP = null;
            //MizunoDataSet.M_TsuikaHinbanDataTable dtTuikaHinban4Becchu = null;  // 種目コード別の追加品番

            // 完了フラグの立っているデータ
            //Dictionary<string, ProperOrderClass.TourokuData> tblProperKanroyuData = new Dictionary<string, ProperOrderClass.TourokuData>(); // キー=指図日_生産オーダーNo_引取オーダーNo_品番_サイズ
            //Dictionary<string, BecchuOrderClass.TourokuData> tblBecchuKanroyuData = new Dictionary<string, BecchuOrderClass.TourokuData>();
            Dictionary<string, ZairyouOrderClass.TourokuData> tblZairyouKanryouData = new Dictionary<string, ZairyouOrderClass.TourokuData>();

            // 「数量0&完了フラグ = 9」のデータと、「数量1以上」のデータが1伝票内に共存してはいけない。
            int nSuryouZeroDataCount = 0;

            int nLine = 0;
            string[] str = null;
            string[] strPrevData = null;

            while (true)
            {
                try
                {
                    string strLine = null;

                    string[] strArray = null;

                    if (null != tabReader)
                    {
                        strLine = tabReader.ReadLine();
                        if (null == strLine) break;
                        strArray = strLine.Split('\t');
                    }
                    else
                    {
                        strArray = csvReader.GetCSVLine(ref strLine);
                        if (string.IsNullOrEmpty(strLine)) break;
                        if (null == strArray || 0 == strArray.Length) break;
                    }

                    nLine++;

                    str = new string[28];

                    if (strArray.Length < 14)
                    {
                        throw new Exception(string.Format("{0}行目の列数が14列に満たないため、登録できません。", nLine));
                    }

                    for (int i = 0; i < 28; i++)
                    { str[i] = ""; }

                    for (int i = 0; i < strArray.Length; i++)
                    { str[i] = strArray[i]; }

                    for (int i = 0; i < str.Length; i++)
                    {
                        string s = str[i].Trim();
                        // 桁数チェック
                        if (nDataMaxLength[i] < s.Length)
                            throw new Exception(string.Format("{0}列目({1})が規定桁数を上回っています(規定桁数={2})", i + 1, strFieldMei[i], s.Length));
                        // 必須項目チェック
                        if ("" == s)
                        {
                            switch ((EnumDataField)i)
                            {
                                case EnumDataField.ShiiresakiCode:
                                case EnumDataField.HacchuBi:
                                case EnumDataField.HacchuNo:
                                case EnumDataField.DenpyouHakkouBi:
                                case EnumDataField.SouhinKubun:
                                case EnumDataField.NiukeJigyoushoCode:
                                //case EnumDataField.HokanBasho:
                                case EnumDataField.NouhinKigou:
                                case EnumDataField.Hinban:
                                case EnumDataField.Suuryou:
                                case EnumDataField.KanryouFlg:
                                    throw new Exception(string.Format("{0}列目({1})は必須項目です", i + 1, strFieldMei[i]));
                            }
                        }
                        // 数値項目のチェック
                        if ("" != s)
                        {
                            str[i] = StrCnvToHankaku(s).Trim();
                            s = str[i];
                            switch ((EnumDataField)i)
                            {
                                case EnumDataField.ShiiresakiCode:
                                case EnumDataField.HacchuBi:
                                case EnumDataField.DenpyouHakkouBi:
                                case EnumDataField.NiukeJigyoushoCode:
                                case EnumDataField.GyouNo:
                                case EnumDataField.KanryouFlg:
                                case EnumDataField.BecchuHin_RowNo:
                                case EnumDataField.YosanTuki:
                                    try
                                    {
                                        int.Parse(s);
                                    }
                                    catch
                                    {
                                        throw new Exception(string.Format("{0}列目({1})は数値項目です", i + 1, strFieldMei[i]));
                                    }
                                    break;
                                case EnumDataField.Suuryou:
                                    try
                                    {
                                        decimal.Parse(s);
                                    }
                                    catch
                                    {
                                        throw new Exception(string.Format("{0}列目({1})は数値項目です", i + 1, strFieldMei[i]));
                                    }
                                    break;
                            }
                        }
                    }

                    // 仕入先コード
                    if (int.Parse(strShiiresakiCode) != int.Parse(str[(int)EnumDataField.ShiiresakiCode]))
                        throw new Exception("仕入先コードが正しくありません。");

                    // 行No
                    bool bNewDenpyou = false;
                    byte bGyouNo = 0;
                    string strRowNo = str[(int)EnumDataField.GyouNo];
                    if ("" != strRowNo)
                    {
                        if (!byte.TryParse(strRowNo, out bGyouNo) || 0 == bGyouNo)
                        {
                            throw new Exception("行Noを正しく入力してください。");
                        }

                        if (1 != bGyouNo && 0 == lstTourokuData.Count)
                        {
                            return new Error("行Noは1から入力して下さい。");
                        }

                        bNewDenpyou = (1 == bGyouNo);
                    }

                    string strSashizuBi = str[(int)EnumDataField.HacchuBi].Trim();
                    string strChumonNo = str[(int)EnumDataField.HacchuNo].Trim();
                    string strZRowNo = str[(int)EnumDataField.GyouNo].Trim();

                    if (bNewDenpyou)
                    {
                        // 新しい伝票の始まり
                        nSuryouZeroDataCount = 0;

                        // 予算品 or 別注品かの確認
                        //pData = null; bData = null;
                        zData = null;// リセット
                        //daProperKey.SelectCommand.Parameters["@z"].Value = strSashizuBi;
                        //daProperKey.SelectCommand.Parameters["@c"].Value = strChumonNo;

                        daZairyouKey.SelectCommand.Parameters["@z"].Value = strSashizuBi;
                        daZairyouKey.SelectCommand.Parameters["@c"].Value = strChumonNo;
                        //daZairyouKey.SelectCommand.Parameters["@r"].Value = strZRowNo;

                        //MizunoDataSet.T_ProperOrderDataTable dtProper = new MizunoDataSet.T_ProperOrderDataTable();
                        MizunoDataSet.T_ZairyouOrderDataTable dtZairyou = new MizunoDataSet.T_ZairyouOrderDataTable();

                        //daProperKey.Fill(dtProper);
                        daZairyouKey.Fill(dtZairyou);
                        if (0 == dtZairyou.Count) throw new Exception("発注データが存在しません。");
                        //if (0 < dtProper.Count)
                        //{
                        if (0 < dtZairyou.Count)
                        {
                            // 予算品
                            //pData = new ProperOrderTourokuData();
                            zData = new ZairyouOrderTourokuData();
                            

                            //pData.TourokuData = new ProperOrderClass.TourokuData();
                           // pData.ProperOrderKey = new ProperOrderClass.ProperOrderKey(dtProper[0].SashizuBi, dtProper[0].SeisanOrderNo, dtProper[0].HikitoriOrderNo);

                            zData.TourokuData = new ZairyouOrderClass.TourokuData();
                            zData.ZairyouOrderKey = new ZairyouOrderClass.ZairyouOrderKey(dtZairyou[0].SashizuBi, dtZairyou[0].HikitoriOrderNo);
                            

                            // 予算月取得
                            //daGetYosanTuki.SelectCommand.Parameters["@z"].Value = pData.ProperOrderKey.SashizuBi;
                            //daGetYosanTuki.SelectCommand.Parameters["@h"].Value = pData.ProperOrderKey.HikitoriOrderNo;
                            //daGetYosanTuki.SelectCommand.Parameters["@s"].Value = pData.ProperOrderKey.SeisanOrderNo;
                            //DataTable dtYosanTuki = new DataTable();
                            //daGetYosanTuki.Fill(dtYosanTuki);
                            //if (0 == dtYosanTuki.Rows.Count) return new Error("発注データが存在しません。");
                            //int nYosanTuki = Convert.ToInt32(dtYosanTuki.Rows[0][0]);
                            //int nYosanTukiCount = Convert.ToInt32(dtYosanTuki.Rows[0][1]);

                            //if (1 < nYosanTukiCount)
                            //{
                            //    // 本オーダーに予算月が複数ある場合
                            //    // 予算月を確認(1伝票に複数の予算月指定は不可)
                            //    string strYosanTuki = StrCnvToHankaku(str[(int)EnumDataField.YosanTuki].Trim());
                            //    if (6 != strYosanTuki.Length) throw new Exception("予算月を6桁(例:201106)で入力してください。");

                            //    try
                            //    {
                            //        pData.YosanTuki = new Nengetu(int.Parse(strYosanTuki));
                            //    }
                            //    catch
                            //    {
                            //        throw new Exception("予算月を正しく入力してください。");
                            //    }
                            //}
                            //else
                            //    pData.YosanTuki = new Nengetu(nYosanTuki);  // 予算月は1つで固定
                            
                            lstTourokuData.Add(zData);
                            denpyou = zData.TourokuData;


                            //lstTourokuData.Add(pData);
                            //denpyou = pData.TourokuData;
                        }
                        //else
                        //{
                        //    // 別注品を検索
                        //    bData = new BecchuOrderTourokuData();

                        //    daBecchu.SelectCommand.Parameters["@ChumonBangou"].Value = strChumonNo;
                        //    daBecchu.SelectCommand.Parameters["@MizunoUketsukeBi"].Value = new Core.Type.Nengappi(int.Parse(strSashizuBi)).ToYYYYMMDD().ToString();
                        //    ViewDataset.VIEW_BecchuKeyInfoDataTable dtB = new ViewDataset.VIEW_BecchuKeyInfoDataTable();
                        //    daBecchu.Fill(dtB);
                        //    if (0 == dtB.Count) throw new Exception("発注データが存在しません。");
                        //    drBecchuKey = dtB[0];

                        //    if (!drBecchuKey.IsCancelBiNull())
                        //        throw new Exception(string.Format("オーダーNo{0}はキャンセル済みです", strChumonNo));
                        //    if (drBecchuKey.KanryouFlg)
                        //        throw new Exception(string.Format("オーダーNo{0}は完了しています。", strChumonNo));

                        //    bData.BecchuOrderKey = new BecchuOrderClass.BecchuOrderKey(drBecchuKey.MizunoUketsukeBi, drBecchuKey.OrderKanriNo, drBecchuKey.ShiiresakiCode);

                        //    if (!drBecchuKey.IsT_Becchu_SashizuNoNull())
                        //    {
                        //        // 別注データ
                        //        BecchuKubun = BecchuOrderClass.EnumBecchuKubun.Becchu;
                        //        dtBecchu = BecchuOrderClass.getVIEW_Becchu_ShukkaMeisaiDataTable(bData.BecchuOrderKey, c);
                        //        dtBecchu.Columns.Add("HinbanTrim", typeof(string));
                        //        dtBecchu.Columns.Add("SizeTrim", typeof(string));
                        //        dtBecchu.Columns.Add("LotNoTrim", typeof(string));
                        //        for (int i = 0; i < dtBecchu.Count; i++)
                        //        {
                        //            dtBecchu[i]["HinbanTrim"] = dtBecchu[i].Hinban.Trim();
                        //            dtBecchu[i]["SizeTrim"] = dtBecchu[i].Size.Trim();
                        //            dtBecchu[i]["LotNoTrim"] = dtBecchu[i].LotNo.Trim();
                        //        }
                        //    }
                        //    else if (!drBecchuKey.IsT_Becchu2_SashizuNoNull())
                        //    {
                        //        // 別注2
                        //        BecchuKubun = BecchuOrderClass.EnumBecchuKubun.Becchu2;
                        //        dtBecchu2 = BecchuOrderClass.getVIEW_Becchu2_ShukkaMeisaiDataTable(bData.BecchuOrderKey, c);
                        //        dtBecchu2.Columns.Add("Zansu", typeof(decimal), "Suuryou-ShukkaSuu");
                        //        dtBecchu2.Columns.Add("HinbanTrim", typeof(string));
                        //        dtBecchu2.Columns.Add("SizeTrim", typeof(string));
                        //        dtBecchu2.Columns.Add("LotNoTrim", typeof(string));
                        //        for (int i = 0; i < dtBecchu2.Count; i++)
                        //        {
                        //            dtBecchu2[i]["HinbanTrim"] = dtBecchu2[i].Hinban.Trim();
                        //            dtBecchu2[i]["SizeTrim"] = dtBecchu2[i].Size.Trim();
                        //            dtBecchu2[i]["LotNoTrim"] = dtBecchu2[i].LotNo.Trim();
                        //        }
                        //    }
                        //    else if (!drBecchuKey.IsT_DS_OrderKanriNoNull())
                        //    {
                        //        // DS
                        //        BecchuKubun = BecchuOrderClass.EnumBecchuKubun.DS;
                        //        dtDS = BecchuOrderClass.getVIEW_DS_ShukkaMeisaiDataTable(bData.BecchuOrderKey.OrderKanriNo, c);
                        //        dtDS.Columns.Add("HinbanTrim", typeof(string));
                        //        dtDS.Columns.Add("SizeTrim", typeof(string));
                        //        for (int i = 0; i < dtDS.Count; i++)
                        //        {
                        //            dtDS[i]["HinbanTrim"] = dtDS[i].Hinban.Trim();
                        //            dtDS[i]["SizeTrim"] = dtDS[i].Size.Trim();
                        //        }
                        //    }
                        //    else if (!drBecchuKey.IsT_SP_OrderKanriNoNull())
                        //    {
                        //        // SP
                        //        BecchuKubun = BecchuOrderClass.EnumBecchuKubun.SP;
                        //        dtSP = BecchuOrderClass.getVIEW_SP_ShukkaMeisaiDataTable(bData.BecchuOrderKey, c);
                        //        dtSP.Columns.Add("HinbanTrim", typeof(string));
                        //        dtSP.Columns.Add("SizeTrim", typeof(string));
                        //        for (int i = 0; i < dtSP.Count; i++)
                        //        {
                        //            dtSP[i]["HinbanTrim"] = dtSP[i].Hinban.Trim();
                        //            dtSP[i]["SizeTrim"] = dtSP[i].Size.Trim();
                        //        }
                        //    }
                        //    else
                        //        throw new Exception("発注データが存在しません。");  // 予算品、別注品共にデータがない
                            
                        //    // ｽﾍﾟｸﾄﾗｵｰﾀﾞｰ№
                        //    bData.BecchuSpectraNo = str[(int)EnumDataField.SPNo].Trim();
                        //    // 運送業者名
                        //    bData.UnsouGyoushaMei = str[(int)EnumDataField.UnsouGyoushaMei].Trim();
                        //    // 送り状№
                        //    bData.OkuriJyouNo = str[(int)EnumDataField.OkuriJyouNo].Trim();

                        //    bData.TourokuData = new BecchuOrderClass.TourokuData();

                        //    lstTourokuData.Add(bData);
                        //    denpyou = bData.TourokuData;

                        //    // 当該種目コードに対応する追加品番取得
                        //    if (null != dtTuikaHinban4Becchu && 0 < dtTuikaHinban4Becchu.Count && dtTuikaHinban4Becchu[0].ShumokuCode == drBecchuKey.ShumokuCode)
                        //    {
                        //        // 前回取得データを再利用
                        //    }
                        //    else
                        //    {
                        //        dtTuikaHinban4Becchu = BecchuOrderClass.getM_TsuikaHinbanDataTable(drBecchuKey.ShumokuCode, c);
                        //        for (int i = 0; i < dtTuikaHinban4Becchu.Count; i++)
                        //        {
                        //            dtTuikaHinban4Becchu[i].Hinban = dtTuikaHinban4Becchu[i].Hinban.Trim(); // 品番の頭に空白があるので
                        //        }
                        //    }
                        //}

                        // 送品区分
                        try
                        {
                            denpyou.SouhinKubun = (EnumSouhinKubun)int.Parse(str[(int)EnumDataField.SouhinKubun]);
                        }
                        catch
                        {
                            throw new Exception("送品区分を正しく入力してください。");
                        }

                        // 伝票発行日
                        try
                        {
                            denpyou.dtHakkouBi = new Core.Type.Nengappi(int.Parse(str[(int)EnumDataField.DenpyouHakkouBi])).ToDateTime();
                        }
                        catch
                        {
                            throw new Exception("伝票発行日は8桁で入力してください。");
                        }
                        if (new Core.Type.Nengappi(denpyou.dtHakkouBi).ToYYYYMMDD() < Core.Type.Nengappi.Today.ToYYYYMMDD())
                            throw new Exception("伝票発行日は本日以降の日付を入力してください。");
                        else if (new Core.Type.Nengappi(denpyou.dtHakkouBi).ToYYYYMMDD() > Core.Type.Nengappi.Parse("2099/12/31").ToYYMMDD())
                            throw new Exception("伝票発行日は2099/12/31以前の日付を入力してください。");

                        denpyou.HokanBasho = str[(int)EnumDataField.HokanBasho];
                        denpyou.NiukeJigyoushoCode = str[(int)EnumDataField.NiukeJigyoushoCode];
                        MizunoDataSet.M_NiukeBumonBashoDataTable dtNiuke = NiukeBumonClass.getM_NiukeBumonBashoDataTable(denpyou.NiukeJigyoushoCode, denpyou.HokanBasho, c);
                        if (0 == dtNiuke.Count)
                        {
                            throw new Exception(string.Format("オーダーNo：{0}の送品区分、事業所、保管場所の組み合わせが正しくありません", strChumonNo));
                        }
                        denpyou.NiukeJigyoushoMei = dtNiuke[0].NiukeBumonMei;
                        denpyou.NiukeBashoMei = dtNiuke[0].NiukeBashoMei;
                        denpyou.NouhinKigou = StrCnvToHankaku(str[(int)EnumDataField.NouhinKigou]).ToUpper();   // 大文字で
                        if (null == NouhinKigouClass.getM_NouhinKigouRow(denpyou.NouhinKigou, c))
                            throw new Exception(string.Format("オーダーNo：{0}の納品記号が正しくありません", strChumonNo));

                        if (!NiukeBumonClass.CheckBumonBasho(denpyou.SouhinKubun, denpyou.NiukeJigyoushoCode, denpyou.HokanBasho, denpyou.NouhinKigou, c))
                            throw new Exception(string.Format("オーダーNo：{0}の送品区分、事業所、保管場所の組み合わせが正しくありません", strChumonNo));

                        // 以下、半角チェックは済んでいる
                        denpyou.SKTantoushaCode = "";   // アップロード時はコードはブランク
                        denpyou.SKTantoushaMei = str[(int)EnumDataField.SeisanTantoushaMei];

                        denpyou.EigyouTantoushaCode = "";
                        denpyou.EigyouTantoushaMei = str[(int)EnumDataField.EigyouTantoushaMei];

                        denpyou.HanbaitenKigyouRyakuMei = str[(int)EnumDataField.Okurisaki];
                        denpyou.TenMei = str[(int)EnumDataField.TenMei];
                        denpyou.TeamMei = str[(int)EnumDataField.TeamMei];
                    }
                    else
                    {
                        // 2行目以降

                        // 伝票の共通項目が前行と同じかどうかチェック
                        for (int i = 0; i < str.Length; i++)
                        {
                            switch ((EnumDataField)i)
                            {
                                case EnumDataField.HacchuBi:
                                case EnumDataField.HacchuNo:
                                case EnumDataField.SouhinKubun:
                                case EnumDataField.NiukeJigyoushoCode:
                                case EnumDataField.HokanBasho:
                                case EnumDataField.SPNo:
                                case EnumDataField.UnsouGyoushaMei:
                                case EnumDataField.OkuriJyouNo:
                                case EnumDataField.SeisanTantoushaMei:
                                case EnumDataField.EigyouTantoushaMei:
                                case EnumDataField.Okurisaki:
                                case EnumDataField.TenMei:
                                case EnumDataField.TeamMei:
                                    if (!str[i].Equals(strPrevData[i]))
                                        throw new Exception(string.Format("{0}が{1}行目と異なっている為、行Noを1から開始して下さい", strFieldMei[i], nLine - 1));
                                    break;
                            }
                        }

                        object objTourokuData = lstTourokuData[lstTourokuData.Count - 1];  // 現在の伝票を取得
                        //pData = null; bData = null; 
                        zData = null;
                        //if (objTourokuData is ProperOrderTourokuData)
                        //{
                        //    // この伝票が予算品の出荷伝票の場合
                        //    int nCol = (int)EnumDataField.YosanTuki;
                        //    if (!str[nCol].Equals(strPrevData[nCol]))   // 予算月が全行と同じかﾁｪｯｸ
                        //        return new Error(string.Format("予算月が{0}行目と異なっている為、行Noを1から開始して下さい", nLine - 1));
                        //    pData = objTourokuData as ProperOrderTourokuData;
                        //    denpyou = pData.TourokuData;
                        //}
                        //else
                        //{
                        //    bData = objTourokuData as BecchuOrderTourokuData;
                        //    denpyou = bData.TourokuData;
                        //}

                        zData = objTourokuData as ZairyouOrderTourokuData;
                        denpyou = zData.TourokuData;
                        
                    }

                    strPrevData = str;  // 確認

                    // ----- 明細 -----
                    NouhinDataClass.MeisaiDataCommon meisai = null;
                    //ProperOrderClass.MeisaiData meisai_p = null;
                    //BecchuOrderClass.MeisaiData meisai_b = null;
                    ZairyouOrderClass.MeisaiData meisai_z = null;

                    //if (null != pData)
                    //    meisai = meisai_p = new ProperOrderClass.MeisaiData();
                    //else
                    //    meisai = meisai_b = new BecchuOrderClass.MeisaiData();

                    meisai = meisai_z = new ZairyouOrderClass.MeisaiData();

                    meisai.Tekiyou = str[(int)EnumDataField.Tekiyou].Trim();
                    switch (meisai.NouhinKubun)
                    {
                        case EnumNouhinKubun.TokuisakiHimoduke:
                        case EnumNouhinKubun.OrderHimoduke:
                            // 摘要が必須のはずだけど
                            if ("" == meisai.Tekiyou)
                            {
                                throw new Exception("納品区分が1もしくは2の場合は摘要を入力してください。");
                            }
                            break;
                    }

                    meisai.Tekiyou = str[(int)EnumDataField.Tekiyou];
                    string strHinban = StrCnvToHankaku(str[(int)EnumDataField.Hinban].Trim());
                    string strSize = str[(int)EnumDataField.Size].Trim();
                    string strLotNo = str[(int)EnumDataField.LotNo].Trim();

                    string strTokushuHinbanCode = th.GetHinban(strHinban);  // 特殊品番の場合は、「運賃」とかが入力されている

                    meisai.TuikaHinban = !string.IsNullOrEmpty(strTokushuHinbanCode);
                    meisai.ShukkaSu2 = decimal.Parse(str[(int)EnumDataField.Suuryou]);

                    if (meisai.TuikaHinban)
                    {
                        //// 追加品番の場合
                        //if (null != pData)
                        //{
                        //    meisai_p.Key.Hinban = "ﾄｸｼｭﾋﾝ-" + strTokushuHinbanCode;
                        //    meisai_p.Key.Size = "";
                        //    pData.AddMeisai(bGyouNo, meisai_p);
                        //}
                        //else
                        //{
                        //    meisai_b.Key.Hinban = "ﾄｸｼｭﾋﾝ-" + strTokushuHinbanCode;
                        //    meisai_b.Key.Size = "";
                        //   bData.AddMeisai(bGyouNo, meisai_b);
                        //}

                        meisai_z.Key.Hinban = "ﾄｸｼｭﾋﾝｰ" + strTokushuHinbanCode;
                        meisai_z.Key.Size = "";
                        meisai_z.Key.LotNo = strLotNo;
                        zData.AddMeisai(bGyouNo, meisai_z);

                        meisai.KanryouFlag = true;  // 自動的に完了
                        if (0 >= meisai.ShukkaSu) throw new Exception("数量は1以上で入力して下さい");   // 追加品番で数量0は有り得ない
                    }
                    else
                    {
                        if (null != zData)
                        {
                            // 予算品の明細取得
                            //daProper.SelectCommand.Parameters["@z"].Value = pData.ProperOrderKey.SashizuBi;
                            //daProper.SelectCommand.Parameters["@s"].Value = pData.ProperOrderKey.SeisanOrderNo;
                            //daProper.SelectCommand.Parameters["@h"].Value = pData.ProperOrderKey.HikitoriOrderNo;
                            //daProper.SelectCommand.Parameters["@y"].Value = pData.YosanTuki.ToYYYYMM();
                            //daProper.SelectCommand.Parameters["@Hinban"].Value = strHinban;
                            //daProper.SelectCommand.Parameters["@Size"].Value = strSize;

                            daZairyou.SelectCommand.Parameters["@z"].Value = zData.ZairyouOrderKey.SashizuBi;
                            daZairyou.SelectCommand.Parameters["@h"].Value = zData.ZairyouOrderKey.HikitoriOrderNo;
                            daZairyou.SelectCommand.Parameters["@Hinban"].Value = strHinban;
                            daZairyou.SelectCommand.Parameters["@lot"].Value = strLotNo;

                            //MizunoDataSet.T_ProperOrderDataTable dtProper = new MizunoDataSet.T_ProperOrderDataTable();
                            MizunoDataSet.T_ZairyouOrderDataTable dtZairyou = new MizunoDataSet.T_ZairyouOrderDataTable();

                            //daProper.Fill(dtProper);
                            daZairyou.Fill(dtZairyou);
                            if (0 == dtZairyou.Count) throw new Exception("発注データが存在しません。");
                            //if (long.Parse(dtProper[0].ShiiresakiCode) != long.Parse(strShiiresakiCode))
                            //    throw new Exception("仕入先コードが正しくありません。");
                            if (long.Parse(dtZairyou[0].ShiiresakiCode) != long.Parse(strShiiresakiCode))
                                throw new Exception("仕入先コードが正しくありません。");
                            //meisai_p.Key.Hinban = dtProper[0].Hinban;   // DBの品番をセットする。（頭に空白があったりするので、DBの内容に合わせる為）
                            //meisai_p.Key.Size = dtProper[0].Size;

                            meisai_z.Key.Hinban = dtZairyou[0].Hinban;   // DBの品番をセットする。（頭に空白があったりするので、DBの内容に合わせる為）
                            //meisai_p.Key.Size = dtZairyou[0].Size;
                            meisai_z.Key.Size = "";
                            meisai_z.Key.LotNo = strLotNo;

                            decimal nZansu = 0;
                            //for (int k = 0; k < dtProper.Count; k++)
                            //{
                            for (int k = 0; k < dtZairyou.Count; k++)
                            {
                                //MizunoDataSet.T_ProperOrderRow drP = dtProper[k];
                                MizunoDataSet.T_ZairyouOrderRow drZ = dtZairyou[k];
                                //if (drP.KanryouFlg) continue;
                                if (drZ.KanryouFlg) continue;

                                //int n = drP.Suuryou - drP.ShukkaSuu;

                                decimal n = drZ.Suuryou - drZ.ShukkaSuu;
                                if (0 >= n) continue;

                                if (string.IsNullOrEmpty(zData.NouhinKigou))
                                    zData.NouhinKigou = drZ.NouhinKigou;
                                //pData.NouhinKigou = drP.NouhinKigou;
                                else
                                {
                                    //if (pData.NouhinKigou != drP.NouhinKigou)
                                    //{
                                    if (zData.NouhinKigou != drZ.NouhinKigou)
                                    {
                                        // 基本的に、注文内の品番+サイズで納品記号が異なるケースは無いが、念の為
                                        throw new Exception(string.Format("納品記号が一致しません。", strChumonNo));
                                    }
                                }

                                nZansu += n;
                            }
                            if (0 == nZansu)
                                throw new Exception(string.Format("発注データが存在しません。材料品オーダーNo：{0}の注文は終了しています。", strChumonNo));

                            // 今回指定の事業所で引き当て可能なので事業所指定で登録する。
                            //meisai_p.Key.NiukeJigyoushoCode = denpyou.NiukeJigyoushoCode;
                            //meisai_p.Key.NiukeJigyoushoCodeHikiateAll = false;  // 同一品番サイズで1注文に複数の事業所があり、引き当て対象となる事業所が確定できないので、伝票の事業所から引き当てられる分だけ先に引き当てる。

                            meisai_z.Key.NiukeJigyoushoCode = denpyou.NiukeJigyoushoCode;
                            meisai_z.Key.NiukeJigyoushoCodeHikiateAll = false;


                            //if (!pData.AddMeisai(bGyouNo, meisai_p))
                            //    throw new Exception("行Noが重複しています。");

                            if (!zData.AddMeisai(bGyouNo, meisai_z))
                                throw new Exception("行Noが重複しています。");


                            // 完了フラグのチェック
                            //string strKey = string.Format("{0}_{1}_{2}_{3}_{4}",
                            //    pData.ProperOrderKey.SashizuBi, pData.ProperOrderKey.HikitoriOrderNo, pData.ProperOrderKey.SeisanOrderNo, meisai_p.Key.Hinban, meisai_p.Key.Size);
                            string strKey = string.Format("{0}_{1}_{2}_{3}_{4}",
                                zData.ZairyouOrderKey.SashizuBi, zData.ZairyouOrderKey.HikitoriOrderNo,"", meisai_z.Key.Hinban,meisai_z.Key.Size);

                            //ProperOrderClass.TourokuData pd = (tblProperKanroyuData.ContainsKey(strKey)) ? tblProperKanroyuData[strKey] : null;
                            ZairyouOrderClass.TourokuData zd = (tblZairyouKanryouData.ContainsKey(strKey)) ? tblZairyouKanryouData[strKey] : null;

                            if (meisai.KanryouFlag)
                            {
                                // 別の伝票で既に完了フラグがセットされていればエラー
                                //if (null == pd)
                                //{
                                if (null == zd)
                                {
                                    //tblProperKanroyuData.Add(strKey, pData.TourokuData);
                                    tblZairyouKanryouData.Add(strKey, zData.TourokuData);
                                }
                                else
                                {
                                    //if (pd != pData.TourokuData)
                                    //    throw new Exception(string.Format("予算品オーダーNo：{0}、品番:{1}、サイズ:{2}の完了フラグが別の伝票で既に登録されています。",
                                    //        strChumonNo, meisai_p.Key.Hinban, meisai_p.Key.Size));
                                    if (zd != zData.TourokuData)
                                        throw new Exception(string.Format("予算品オーダーNo：{0}、品番:{1}、サイズ:{2}の完了フラグが別の伝票で既に登録されています。",
                                            strChumonNo, meisai_z.Key.Hinban, meisai_z.Key.Size));
                                }
                            }
                            else
                            {
                                // 当該品番+サイズで完了フラグが立っていても出荷日が後であればＯＫ
                                //if (null != pd && pd != pData.TourokuData)
                                //{
                                //    if (pd.dtHakkouBi <= pData.TourokuData.dtHakkouBi)
                                //    {
                                //        throw new Exception(string.Format("予算品オーダーNo：{0}、品番:{1}、サイズ:{2}の完了フラグは伝票発行日{0:yyyy/MM/dd}の伝票で既にセットされています。",
                                //            strChumonNo, meisai_p.Key.Hinban, meisai_p.Key.Size, pd.dtHakkouBi));
                                //    }
                                //}
                                if (null != zd && zd != zData.TourokuData)
                                {
                                    if (zd.dtHakkouBi <= zData.TourokuData.dtHakkouBi)
                                    {
                                        throw new Exception(string.Format("予算品オーダーNo：{0}、品番:{1}、サイズ:{2}の完了フラグは伝票発行日{0:yyyy/MM/dd}の伝票で既にセットされています。",
                                            strChumonNo, meisai_z.Key.Hinban, meisai_z.Key.Size, zd.dtHakkouBi));
                                    }
                                }
                            }
                        }
                        else
                        {
                            //bool bFoundChumonData = false;

                            //switch (BecchuKubun)
                            //{
                            //    case BecchuOrderClass.EnumBecchuKubun.Becchu:
                            //        dtBecchu.DefaultView.RowFilter = string.Format("HinbanTrim='{0}' and SizeTrim='{1}' and LotNoTrim='{2}'", strHinban.Replace("'", "''"), strSize.Replace("'", "''"), strLotNo.Replace("'", "''"));
                            //        bFoundChumonData = (0 < dtBecchu.DefaultView.Count);
                            //        if (bFoundChumonData)
                            //        {
                            //            ViewDataset.VIEW_Becchu_ShukkaMeisaiRow drBecchu = dtBecchu.DefaultView[0].Row as ViewDataset.VIEW_Becchu_ShukkaMeisaiRow;
                            //            meisai_b.Key.Hinban = drBecchu.Hinban;
                            //            meisai_b.Key.Size = drBecchu.Size;
                            //            meisai_b.Key.LotNo = drBecchu.LotNo;
                            //        }
                            //        break;
                            //    case BecchuOrderClass.EnumBecchuKubun.Becchu2:
                            //        {
                            //            if (!string.IsNullOrEmpty(strLotNo))
                            //            {
                            //                dtBecchu2.DefaultView.RowFilter = string.Format("HinbanTrim='{0}' AND SizeTrim='{1}' AND LotNoTrim='{2}' AND Suuryou>ShukkaSuu",
                            //                    strHinban.Replace("'", "''"), strSize.Replace("'", "''"), strLotNo.Replace("'", "''"));
                            //            }
                            //            else
                            //            {
                            //                // 行No
                            //                try
                            //                {
                            //                    meisai_b.Key.RowNo = int.Parse(StrCnvToHankaku(str[(int)EnumDataField.BecchuHin_RowNo].Trim()));
                            //                }
                            //                catch
                            //                {
                            //                    throw new Exception("ロットNo.を入力するか、別注品明細Noを正しく入力してください。");
                            //                }

                            //                dtBecchu2.DefaultView.RowFilter = string.Format("HinbanTrim='{0}' AND SizeTrim='{1}' AND RowNo={2} AND Suuryou>ShukkaSuu",
                            //                    strHinban.Replace("'", "''"), strSize.Replace("'", "''"), meisai_b.Key.RowNo);
                            //            }

                            //            bFoundChumonData = (0 < dtBecchu2.DefaultView.Count);
                            //            if (bFoundChumonData)
                            //            {
                            //                ViewDataset.VIEW_Becchu2_ShukkaMeisaiRow drBecchu2 = dtBecchu2.DefaultView[0].Row as ViewDataset.VIEW_Becchu2_ShukkaMeisaiRow;
                            //                meisai_b.Key.Hinban = drBecchu2.Hinban;
                            //                meisai_b.Key.Size = drBecchu2.Size;
                            //                meisai_b.Key.LotNo = drBecchu2.LotNo;
                            //                meisai_b.Key.RowNo = drBecchu2.RowNo;
                            //            }
                            //        }
                            //        break;
                            //    case BecchuOrderClass.EnumBecchuKubun.DS:
                            //        dtDS.DefaultView.RowFilter = string.Format("HinbanTrim='{0}' AND SizeTrim='{1}'", strHinban.Replace("'", "''"), strSize.Replace("'", "''"));
                            //        bFoundChumonData = (0 < dtDS.DefaultView.Count);
                            //        if (bFoundChumonData)
                            //        {
                            //            meisai_b.Key.Hinban = (dtDS.DefaultView[0].Row as ViewDataset.VIEW_DS_ShukkaMeisaiRow).Hinban;
                            //            meisai_b.Key.Size = (dtDS.DefaultView[0].Row as ViewDataset.VIEW_DS_ShukkaMeisaiRow).Size;
                            //        }
                            //        break;
                            //    case BecchuOrderClass.EnumBecchuKubun.SP:
                            //        {
                            //            dtSP.DefaultView.RowFilter = string.Format("HinbanTrim='{0}' AND SizeTrim='{1}'", strHinban.Replace("'", "''"), strSize.Replace("'", "''"));
                            //            bFoundChumonData = (0 < dtSP.DefaultView.Count);
                            //            if (bFoundChumonData)
                            //            {
                            //                ViewDataset.VIEW_SP_ShukkaMeisaiRow drSp = dtSP.DefaultView[0].Row as ViewDataset.VIEW_SP_ShukkaMeisaiRow;
                            //                meisai_b.Key.Hinban = drSp.Hinban;
                            //                meisai_b.Key.Size = drSp.Size;
                            //            }
                            //        }

                            //        break;
                            //}

                            //if (!bFoundChumonData)
                            //{
                            //    // 注文データ中になくても追加品番の可能性がある
                            //    if (null == dtTuikaHinban4Becchu.FindByHinbanShumokuCode(strHinban, drBecchuKey.ShumokuCode))
                            //        throw new Exception("発注データ(追加品番)が存在しません。(品番=" + strHinban + ")");
                            //    else
                            //    {
                            //        // 追加品番である
                            //        meisai.TuikaHinban = true;
                            //        meisai_b.Key.Hinban = strHinban;
                            //        meisai_b.Key.Size = strSize;    // 追加品番（特殊品番でない）の場合はサイズ指定が可能
                            //    }
                            //}

                            //if (!bData.AddMeisai(bGyouNo, meisai_b))
                            //    throw new Exception("行Noが重複しています。");
                        }

                        // 完了フラグ
                        string strKanryouFlg = str[(int)EnumDataField.KanryouFlg].Trim();
                        if ("0" != strKanryouFlg && "9" != strKanryouFlg) throw new Exception("完了フラグは0または9を入力してください");
                        meisai.KanryouFlag = (KanryouFlg.Kanryou == strKanryouFlg);

                        if (0 > meisai.ShukkaSu) throw new Exception("数量は0以上で入力して下さい");
                        if (KanryouFlg.MiKanryou == strKanryouFlg && 0 == meisai.ShukkaSu) throw new Exception("完了フラグが0の時は、数量を1以上で入力して下さい");
                        if (meisai.KanryouFlag && 0 == meisai.ShukkaSu) nSuryouZeroDataCount++;
                        // 「数量0、完了フラグ = 9」のデータと、「数量1以上」のデータが1伝票内に共存してはいけない。
                        if (0 < meisai.ShukkaSu && 0 < nSuryouZeroDataCount)
                        {
                            throw new Exception(string.Format("オーダーNo：{0}:数量=0、完了フラグ=9のデータは数量1以上のデータと伝票を分けて下さい", strChumonNo));
                        }
                        

                        // 自由使用欄
                        meisai.Free1 = str[(int)EnumDataField.JiyuKoumoku1];
                        meisai.Free2 = str[(int)EnumDataField.JiyuKoumoku2];
                        meisai.Free3 = str[(int)EnumDataField.JiyuKoumoku3];
                        // 31桁備考
                        meisai.Bikou = "";
                    }
                }
                catch (Exception e)
                {
                    strPrevData = str;
                    lstErrorMsg.Add(string.Format("{0}行目 - {1}", nLine, e.Message));
                }
            }

            if (0 < lstErrorMsg.Count) return new Error("登録に失敗しました。");

            if (0 == lstTourokuData.Count) return new Error("登録するデータがありません。");

            // 完了フラグをオフにする。
            //SqlDataAdapter daBki = new SqlDataAdapter("", c);
            //daBki.SelectCommand.CommandText = "SELECT * FROM T_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            //daBki.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", "");
            //daBki.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", "");
            //daBki.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", "");
            //daBki.UpdateCommand = new SqlCommandBuilder(daBki).GetUpdateCommand();
            //MizunoDataSet.T_BecchuKeyInfoDataTable dtBecchuKey = new MizunoDataSet.T_BecchuKeyInfoDataTable();

            SqlTransaction t = null;

            int nDenpyouIndex = 0;
            Core.Error ret = null;
            try
            {
                //c.Open();
                //t = c.BeginTransaction();

                //daBki.SelectCommand.Transaction = daBki.UpdateCommand.Transaction = t;

                for (nDenpyouIndex = 0; nDenpyouIndex < lstTourokuData.Count; nDenpyouIndex++)
                {
                    List<NouhinDataClass.DenpyouKey> lst = null;
                    List<NouhinDataClass.DenpyouKey> lstShukkaAri = null;
                    //if (lstTourokuData[nDenpyouIndex] is ProperOrderTourokuData)
                    //{
                    if (lstTourokuData[nDenpyouIndex] is ZairyouOrderTourokuData)
                    {
                        // 予算品
                        //ProperOrderTourokuData p = lstTourokuData[nDenpyouIndex] as ProperOrderTourokuData;
                        //p.SetMeisaiData();
                        ZairyouOrderTourokuData z = lstTourokuData[nDenpyouIndex] as ZairyouOrderTourokuData;
                        z.SetMeisaiData();

                        //int nMaxGyouSu = GetDenpyouGyouCount(p.TourokuData.SouhinKubun);
                        //if (nMaxGyouSu < p.TourokuData.lstMeisai.Count)
                        //{
                        //    string strChumonBangou = (!string.IsNullOrEmpty(p.ProperOrderKey.HikitoriOrderNo)) ?
                        //        p.ProperOrderKey.HikitoriOrderNo : p.ProperOrderKey.SeisanOrderNo;
                        //    throw new Exception(string.Format("注文No.{0}が伝票作成可能な行数を超えています。(最大行数={1})",
                        //        strChumonBangou, nMaxGyouSu));
                        //}

                        int nMaxGyouSu = GetDenpyouGyouCount(z.TourokuData.SouhinKubun);
                        if (nMaxGyouSu < z.TourokuData.lstMeisai.Count)
                        {
                            string strChumonBangou = z.ZairyouOrderKey.HikitoriOrderNo;

                            throw new Exception(string.Format("注文No.{0}が伝票作成可能な行数を超えています。(最大行数={1})",
                                strChumonBangou, nMaxGyouSu));
                        }

                        //ProperOrderClass.DenpyouTouroku(true, false, p.ProperOrderKey, p.YosanTuki, p.NouhinKigou, p.TourokuData, t, out lst, out lstShukkaAri);
                        ret = ZairyouOrderClass.DenpyouTouroku(true, z.ZairyouOrderKey, z.NouhinKigou, z.TourokuData, c, out lst, out lstShukkaAri);
                    }
                    else
                    {
                        // 別注品
                        //BecchuOrderTourokuData b = lstTourokuData[nDenpyouIndex] as BecchuOrderTourokuData;
                        //b.SetMeisaiData();

                        //int nMaxGyouSu = GetDenpyouGyouCount(b.TourokuData.SouhinKubun);
                        //if (nMaxGyouSu < b.TourokuData.lstMeisai.Count)
                        //{
                        //    throw new Exception(string.Format("注文No.{0}が伝票作成可能な行数を超えています。(最大行数={1})",
                        //        b.BecchuOrderKey.OrderKanriNo, nMaxGyouSu));
                        //}

                        //BecchuOrderClass.DenpyouTouroku(true, false, b.BecchuOrderKey, b.TourokuData, t, out lst, out lstShukkaAri);

                        //daBki.SelectCommand.Parameters["@MizunoUketsukeBi"].Value = b.BecchuOrderKey.UketsukeBi;
                        //daBki.SelectCommand.Parameters["@OrderKanriNo"].Value = b.BecchuOrderKey.OrderKanriNo;
                        //daBki.SelectCommand.Parameters["@ShiiresakiCode"].Value = b.BecchuOrderKey.ShiiresakiCode;
                        //dtBecchuKey.Clear();
                        //daBki.Fill(dtBecchuKey);
                        //if (0 == dtBecchuKey.Count) throw new Exception(string.Format("オーダーNo.{0}のキー情報がありません。", b.BecchuOrderKey.OrderKanriNo));

                        //MizunoDataSet.T_BecchuKeyInfoRow dr = dtBecchuKey[0];

                        //if (!string.IsNullOrEmpty(b.BecchuSpectraNo)) dr.BecchuSpectraNo = b.BecchuSpectraNo;   // これ正しいの？

                        //if (!string.IsNullOrEmpty(b.OkuriJyouNo)) dr.OkuriJyouNo = b.OkuriJyouNo;
                        //if (!string.IsNullOrEmpty(b.UnsouGyoushaMei)) dr.UnsouGyoushaMei = b.UnsouGyoushaMei;
                        //if (b.GoukeiKakaku.HasValue) dr.GoukeiKakaku = b.GoukeiKakaku.Value;
                        //daBki.Update(dtBecchuKey);
                    }

                    if (ret == null)
                    {
                        if (0 < lst.Count) lstDenpyouKey.AddRange(lst);
                        if (null != lstShukkaAri && 0 < lstShukkaAri.Count) lstDenpyouKeyShukkaAri.AddRange(lstShukkaAri);
                    }
                    else
                    {
                        return new Core.Error(string.Format("{0}枚目の伝票 - {1}", nDenpyouIndex + 1,ret.Message));
                    }
                }

                //t.Commit();

                return null;
            }
            catch (Exception e)
            {
                //if (null != t) t.Rollback();
                return new Core.Error(string.Format("{0}枚目の伝票 - {1}", nDenpyouIndex + 1, e.Message));
            }
            finally
            {
                //c.Close();
            }
        }

        public static Core.Error ZUploadNouhnData2(string strShiiresakiCode, System.IO.Stream stream,
            System.Text.Encoding enc, SqlConnection c, out List<string> lstErrorMsg,
            out List<NouhinDataClass.DenpyouKey> lstDenpyouKey, out List<NouhinDataClass.DenpyouKey> lstDenpyouKeyShukkaAri)
        {

            lstErrorMsg = new List<string>();
            lstDenpyouKey = new List<NouhinDataClass.DenpyouKey>();
            lstDenpyouKeyShukkaAri = new List<DenpyouKey>();

            //齊藤追加 2012-08-22
            SqlDataAdapter daSashizuBi = new SqlDataAdapter("", c);
            daSashizuBi.SelectCommand.CommandText = "select * from T_ZairyouOrder where ShiiresakiCode=@sc and HikitoriOrderNo=@ho and Hinban=@hb and LotNo=@ln";
            daSashizuBi.SelectCommand.Parameters.AddWithValue("@sc", "");
            daSashizuBi.SelectCommand.Parameters.AddWithValue("@ho", "");
            daSashizuBi.SelectCommand.Parameters.AddWithValue("@hb", "");
            daSashizuBi.SelectCommand.Parameters.AddWithValue("@ln", "");

            // 材料2012-07-27 14:09
            SqlDataAdapter daZairyouKey = new SqlDataAdapter("", c);
            daZairyouKey.SelectCommand.CommandText = @"
select dbo.T_ZairyouOrder.* 
from   dbo.T_ZairyouOrder 
where  (SashizuBi = @z) AND 
HikitoriOrderNo = @c ";
            daZairyouKey.SelectCommand.Parameters.AddWithValue("@z", "");
            daZairyouKey.SelectCommand.Parameters.AddWithValue("@c", "");

            SqlDataAdapter daZairyou = new SqlDataAdapter("", c);
            daZairyou.SelectCommand.CommandText = @"
select dbo.T_ZairyouOrder.*
from   dbo.T_ZairyouOrder
where  (SashizuBi = @z) AND HikitoriOrderNo = @h AND 
(Ltrim(Hinban) = @Hinban) AND LotNo=@lot ";
            //"AND (Size = @Size)";
            daZairyou.SelectCommand.Parameters.AddWithValue("@z", "");
            daZairyou.SelectCommand.Parameters.AddWithValue("@h", "");
            daZairyou.SelectCommand.Parameters.AddWithValue("@Hinban", "");
            daZairyou.SelectCommand.Parameters.AddWithValue("@lot", "");

            //------ファイル読み込み
            System.IO.StreamReader tabReader = null;
            Core.IO.CSVReader csvReader = null;

            System.IO.StreamReader check = new System.IO.StreamReader(stream, enc);
            string strCheck = check.ReadLine();  // CSVとタブ区切りの確認の為
            if (null == strCheck)
            {
                return new Core.Error("データがありません。");
            }
            bool bTab = (strCheck.Split('\t').Length > strCheck.Split(',').Length);   // これ良いの？

            stream.Position = 0;
            if (bTab)
                tabReader = new System.IO.StreamReader(stream, enc);
            else
                csvReader = new Core.IO.CSVReader(stream, enc);

            int[] nDataMaxLength = new int[]{
8,  // 1:仕入先ｺｰﾄﾞ
8,  // 2:指図日(ミズノ受付日)
40, // 3:ｵｰﾀﾞｰ№
8,  // 4:伝票発行日
1,  // 5:送品区分
4,  // 6:荷受事業所ｺｰﾄﾞ
20,  // 7:保管場所ｺｰﾄﾞ
3,  // 8:納品記号
2,  // 9行No
50, // 10:ロットNo
20, // 11:品番
5,  // 12:サイズ
10,  // 13:数量
1,  // 14:完了フラグ
15, // 15:摘要
10,  // 16:生産担当者
10, // 17:営業担当者
10, // 18:送り先
10, // 19:店名
10, // 20:学校/チーム名
20, // 21:自由項目1
20, // 22:自由項目2
20, // 23:自由項目3
6,  // 24:予算品予算月(YYYYMM)
2,  // 25:別注2行No
14, // 26:スペクトラオーダーNo
15, // 27:運送業者名
15, // 28:送り状No
18, // 29:InvoiceNo     2014/10/28 三津谷 追記
30, // 30:緊急直送備考  2014/10/28 三津谷 追記
2,  // 31:運送業者CD    2014/10/28 三津谷 追記
2,  // 32:個口数        2014/10/28 三津谷 追記

        };

            string[] strFieldMei = new string[]{            
"仕入先ｺｰﾄﾞ",
"指図日(ミズノ受付日)",
"ｵｰﾀﾞｰ№",
"伝票発行日",
"送品区分",
"荷受事業所ｺｰﾄﾞ",
"保管場所ｺｰﾄﾞ",
"納品記号",
"行No",
"ロットNo.",    // 追加
"品番",
"サイズ",
"数量",
"完了フラグ",
"摘要",
"生産担当者",
"営業担当者",
"送り先",
"店名",
"学校/チーム名",
"自由使用欄1",
"自由使用欄2",
"自由使用欄3",
"予算月",
"別注品明細No",
"ｽﾍﾟｸﾄﾗｵｰﾀﾞｰ№",
"運送業者名",
"送り状№",
"INVOICE№",
"緊急直送備考",
"運送業者CD",
"個口数",
            };

            TsuikaHinban th = new TsuikaHinban();

            NouhinDataClass.TourokuDataBase denpyou = null;
            
            ZairyouOrderTourokuData zData = null;

            List<object> lstTourokuData = new List<object>();

            DateTime dtNow = DateTime.Now;

            Dictionary<string, ZairyouOrderClass.TourokuData> tblZairyouKanryouData = new Dictionary<string, ZairyouOrderClass.TourokuData>();

            // 「数量0&完了フラグ = 9」のデータと、「数量1以上」のデータが1伝票内に共存してはいけない。
            int nSuryouZeroDataCount = 0;

            int nLine = 0;
            string[] str = null;
            string[] strPrevData = null;

            string ShiiresakiCode = "";
            string HikitoriOrderNo = "";
            string LotNo = "";
            string HakkouBi = "";
            string SouhinKubun = "";
            string Hinban = "";
            decimal Suuryou = 0;
            string KanryouFLG = "";
            string Tekiyou = "";
            int RowCount = 0;

            //2014/10/28 三津谷追記

            string SashizuBi = "";
            string NiukeJigyoushoCode = "";
            string HokanBasho = "";
            string NouhinKigou = "";
            string RowNo = "";

            string Size = "";
            string SeisanTantousha = "";
            string EigyouTantousha = "";
            string OkurisakiMei = "";
            string TenMei = "";
            string TeamMei = "";
            string Free1 = "";
            string Free2 = "";
            string Free3 = "";
            string YosanTuki = "";
            string BecchuMeisaiNo = "";
            string SPOrderNo = "";
            string GyoushaMei = "";
            string OkurijyouNo = "";
            string InvoiceNo = "";
            string KinkyuTyokusouBikou = "";
            string UnsouGyousyaCode = "";
            int KogutiSu = 0;


            while (true)
            {
                RowCount++;
                try
                {
                    nLine++;
                    string strLine = null;

                    string[] strArray = null;

                    if (null != tabReader)
                    {
                        strLine = tabReader.ReadLine();
                        if (null == strLine) break;
                        strArray = strLine.Split('\t');
                    }
                    else
                    {
                        strArray = csvReader.GetCSVLine(ref strLine);
                        if (string.IsNullOrEmpty(strLine)) break;
                        if (null == strArray || 0 == strArray.Length) break;
                    }

                    str = new string[32];

                    if (strArray.Length < 8)
                    {
                        throw new Exception(string.Format("{0}行目の列数が8列に満たないため、登録できません。", nLine));
                    }

                    for (int i = 0; i < 32; i++)
                    { str[i] = ""; }

                    for (int i = 0; i < strArray.Length; i++)
                    { str[i] = strArray[i]; }

                    //ShiiresakiCode,HikitoriOrderNo,SouhinKubun,LotNo,Hinban,Suuryou,InvoiceNo以外はコメントアウト 20160711
                    ShiiresakiCode = str[0];
                    HikitoriOrderNo = strArray[2];
                    //HakkouBi = strArray[3];
                    SouhinKubun = str[4];
                    if (SouhinKubun != "2" && SouhinKubun != "4")
                    {
                        throw new Exception(string.Format("送品区分は2か4のみ、入力可能です。", nLine));
                    }
                    LotNo = strArray[9];
                    Hinban = strArray[10];
                    Suuryou = decimal.Parse(str[12]);
                    //KanryouFLG = strArray[13];
                    //Tekiyou = strArray[14];

                    //// 2014/10/28 三津谷追記

                    //SashizuBi = strArray[1];
                    //NiukeJigyoushoCode = strArray[5];
                    //HokanBasho = strArray[6];
                    //NouhinKigou = strArray[7];
                    //RowNo = strArray[8];

                    //Size = strArray[11];

                    //SeisanTantousha = strArray[15];
                    //EigyouTantousha = strArray[16];
                    //OkurisakiMei = strArray[17];
                    //TenMei = strArray[18];
                    //TeamMei = strArray[19];
                    //Free1 = strArray[20];
                    //Free2 = strArray[21];
                    //Free3 = strArray[22];
                    //YosanTuki = strArray[23];
                    //BecchuMeisaiNo = strArray[24];
                    //SPOrderNo = strArray[25];
                    //GyoushaMei = strArray[26];
                    //OkurijyouNo = strArray[27];
                    InvoiceNo = (str[27] != "") ? str[27] : str[28]; //送り状NoかInvoiceNoを入れる

                    string strInvoiceNo = InvoiceNo;

                    if ("" != strInvoiceNo)
                    {

                        if (strInvoiceNo.Contains(",") ||
                            strInvoiceNo.Contains("?") ||
                            strInvoiceNo.Contains("!") ||
                            strInvoiceNo.Contains("\"") ||
                            strInvoiceNo.Contains("'")
                            )
                        {
                            throw new Exception("INVOICE No.に使用不可文字が入力されています");
                        }
                    }

                    //29******の仕入先の場合INVOICE№欄は必須とする。
                    string sShiiresakiCode = ShiiresakiCode;

                    if (sShiiresakiCode.Substring(0, 2) == "29")
                    {
                        //** 仕入先が海外の場合

                        if (strInvoiceNo == "")
                        { throw new Exception("INVOICE No.を入力して下さい"); }
                    }

                    //必要ないのでコメントアウト 20160711
                    //KinkyuTyokusouBikou = strArray[29];
                    //UnsouGyousyaCode = strArray[30];
                    //if (strArray[31] != "")
                    //{ KogutiSu = int.Parse(strArray[31]); }

                    SqlDataAdapter daZairyou2 = new SqlDataAdapter("", c);
                    daZairyou2.SelectCommand.CommandText = @"
select dbo.T_ZairyouOrder.*
from   dbo.T_ZairyouOrder
where  (ShiiresakiCode = @z) AND HikitoriOrderNo = @h AND 
(Ltrim(Hinban) = @Hinban) AND LotNo=@lot ";
                    //"AND (Size = @Size)";
                    daZairyou2.SelectCommand.Parameters.AddWithValue("@z",ShiiresakiCode);
                    daZairyou2.SelectCommand.Parameters.AddWithValue("@h", HikitoriOrderNo);
                    daZairyou2.SelectCommand.Parameters.AddWithValue("@Hinban", Hinban);
                    daZairyou2.SelectCommand.Parameters.AddWithValue("@lot", LotNo);
                    MizunoDataSet.T_ZairyouOrderDataTable dtZairyou2 = new MizunoDataSet.T_ZairyouOrderDataTable();
                    daZairyou2.Fill(dtZairyou2);
                    MizunoDataSet.T_ZairyouOrderRow drZairyou2 = (MizunoDataSet.T_ZairyouOrderRow)dtZairyou2.Rows[0];

                    //for (int i = 0; i < strArray.Length; i++)
                    //{ str[i] = strArray[i]; }
                    //----値を入れていく by齊藤
                    //必要ないのでコメントアウト 20160711
                    //str[0] = ShiiresakiCode;
                    //str[1] = SashizuBi;
                    //str[2] = HikitoriOrderNo;
                    //str[3] = HakkouBi;
                    //str[4] = SouhinKubun;
                    //str[5] = NiukeJigyoushoCode;
                    //str[6] = HokanBasho;
                    //str[7] = NouhinKigou;
                    //str[8] = RowNo;
                    //str[9] = LotNo;
                    //str[10] = Hinban;
                    //str[11] = Size;
                    //str[12] = Suuryou.ToString();
                    //str[13] = KanryouFLG.ToString();
                    //str[14] = Tekiyou;
                    //str[15] = SeisanTantousha;
                    //str[16] = EigyouTantousha;
                    //str[17] = OkurisakiMei;
                    //str[18] = TenMei;
                    //str[19] = TeamMei;
                    //str[20] = Free1;
                    //str[21] = Free2;
                    //str[22] = Free3;
                    //str[23] = YosanTuki;
                    //str[24] = BecchuMeisaiNo;
                    //str[25] = SPOrderNo;
                    //str[26] = GyoushaMei;
                    //str[27] = OkurijyouNo;
                    //str[28] = InvoiceNo;
                    //str[29] = KinkyuTyokusouBikou;
                    //str[30] = UnsouGyousyaCode;
                    //str[31] = KogutiSu.ToString();
                    //--------

                    for (int i = 0; i < str.Length; i++)
                    {
                        string s = str[i].Trim();
                        // 桁数チェック
                        if (nDataMaxLength[i] < s.Length)
                            throw new Exception(string.Format("{0}列目({1})が規定桁数を上回っています(規定桁数={2})", i + 1, strFieldMei[i], nDataMaxLength[i]));
                        // 必須項目チェック
                        if ("" == s)
                        {
                            switch ((EnumDataField)i)
                            {
                                case EnumDataField.ShiiresakiCode:
                                case EnumDataField.HacchuBi:
                                case EnumDataField.HacchuNo:
                                case EnumDataField.DenpyouHakkouBi:
                                case EnumDataField.SouhinKubun:
                                case EnumDataField.NiukeJigyoushoCode:
                                //case EnumDataField.HokanBasho:
                                //case EnumDataField.NouhinKigou: これをコメントアウトしないと登録が出来ないデータがある 20160711
                                case EnumDataField.Hinban:
                                case EnumDataField.Suuryou:
                                case EnumDataField.KanryouFlg:
                                    throw new Exception(string.Format("{0}列目({1})は必須項目です", i + 1, strFieldMei[i]));
                            }
                        }
                        // 数値項目のチェック
                        if ("" != s)
                        {
                            str[i] = StrCnvToHankaku(s).Trim();
                            s = str[i];
                            switch ((EnumDataField)i)
                            {
                                case EnumDataField.ShiiresakiCode:
                                case EnumDataField.HacchuBi:
                                case EnumDataField.DenpyouHakkouBi:
                                case EnumDataField.NiukeJigyoushoCode:
                                case EnumDataField.GyouNo:
                                case EnumDataField.KanryouFlg:
                                case EnumDataField.BecchuHin_RowNo:
                                case EnumDataField.YosanTuki:
                                    try
                                    {
                                        int.Parse(s);
                                    }
                                    catch
                                    {
                                        throw new Exception(string.Format("{0}列目({1})は数値項目です", i + 1, strFieldMei[i]));
                                    }
                                    break;
                                case EnumDataField.Suuryou:
                                    try
                                    {
                                        decimal.Parse(s);
                                    }
                                    catch
                                    {
                                        throw new Exception(string.Format("{0}列目({1})は数値項目です", i + 1, strFieldMei[i]));
                                    }
                                    break;
                            }
                        }
                    }

                    // 仕入先コード
                    if (int.Parse(strShiiresakiCode) != int.Parse(str[(int)EnumDataField.ShiiresakiCode]))
                        throw new Exception("仕入先コードが正しくありません。");

                    // 行No
                    bool bNewDenpyou = false;
                    byte bGyouNo = 0;
                    string strRowNo = str[(int)EnumDataField.GyouNo];
                    if ("" != strRowNo)
                    {
                        //if (!byte.TryParse(strRowNo, out bGyouNo) || 0 == bGyouNo)
                        //{
                        //    throw new Exception("行Noを正しく入力してください。");
                        //}

                        if (!byte.TryParse(strRowNo, out bGyouNo) || 0 == bGyouNo)
                        {
                            throw new Exception("行Noを正しく入力してください。");
                        }

                        bNewDenpyou = (1 == bGyouNo);

                    }

                    string strSashizuBi = str[(int)EnumDataField.HacchuBi].Trim();
                    string strChumonNo = str[(int)EnumDataField.HacchuNo].Trim();
                    string strZRowNo = str[(int)EnumDataField.GyouNo].Trim();
                    
                    if (bNewDenpyou)
                    {
                        // 新しい伝票の始まり
                        nSuryouZeroDataCount = 0;

                        // 予算品 or 別注品かの確認
                        
                        zData = null;// リセット
                        

                        daZairyouKey.SelectCommand.Parameters["@z"].Value = strSashizuBi;
                        daZairyouKey.SelectCommand.Parameters["@c"].Value = strChumonNo;
                        
                        MizunoDataSet.T_ZairyouOrderDataTable dtZairyou = new MizunoDataSet.T_ZairyouOrderDataTable();

                        
                        daZairyouKey.Fill(dtZairyou);
                        if (0 == dtZairyou.Count) throw new Exception("発注データが存在しません。");
                        //if (0 < dtProper.Count)
                        //{
                        if (0 < dtZairyou.Count)
                        {
                            // 予算品
                            zData = new ZairyouOrderTourokuData();


                            
                            zData.TourokuData = new ZairyouOrderClass.TourokuData();
                            zData.ZairyouOrderKey = new ZairyouOrderClass.ZairyouOrderKey(dtZairyou[0].SashizuBi, dtZairyou[0].HikitoriOrderNo);

                            //直送時送品区分などは変更不可とする(出荷入力画面と同じ)　20170405 M0337 改めて反映 20180315
                            if (dtZairyou[0].UriwatashisakiCode != "")
                            {
                                if (str[(int)EnumDataField.SouhinKubun] != "4" ||
                                    dtZairyou[0].NiukeJigyoushoCode != str[(int)EnumDataField.NiukeJigyoushoCode] ||
                                    dtZairyou[0].HokanBasho != str[(int)EnumDataField.HokanBasho])
                                {
                                    throw new Exception("直送オーダーのため、送品区分・事業所・保管場所の変更はできません");
                                }
                            }

                            //緊急直送専用処理 20160824
                            //材料は緊急直送の特殊仕様は不要という事でコメントアウト　高山さん要望 20170213
                            /*if (str[(int)EnumDataField.SouhinKubun].Trim() == "4" && dtZairyou[0].UriwatashisakiCode == "")
                            {
                                int nError = 0;
                                //保管場所が「KKZK」ではない
                                if (str[(int)EnumDataField.HokanBasho] != "KKZK")
                                    nError = 1; //保管場所が違うだけ
                                //保管場所「KKZK」以外の事業所が設定されている
                                MizunoDataSet.M_NiukeBumonBashoDataTable dt =
                                    NiukeBumonClass.getM_NiukeBumonBashoSouhinKubun4DataTable(str[(int)EnumDataField.NiukeJigyoushoCode], c);
                                if (dt.Count == 0)
                                    if (nError != 1)
                                        nError = 2; //事業所が違うだけ
                                    else
                                        nError = 3; //保管場所と事業所が違う

                                if (nError != 0)
                                    switch (nError)
                                    {
                                        case 1:
                                            throw new Exception("送品区分が4：直送に変更されています。そのため荷受保管場所コードには「KKZK」しか入力できません。");
                                            break;
                                        case 2:
                                            throw new Exception(@"送品区分が4：直送に変更されています。そのため入力されている荷受事業所コードは設定できません。
事業所については、操作マニュアルダウンロード画面の緊急直送時出荷マニュアルをご確認下さい。");
                                            break;
                                        case 3:
                                            throw new Exception(@"送品区分が4：直送に変更されています。そのため入力されている荷受事業所コード・荷受保管場所コードは設定できません。
事業所については、操作マニュアルダウンロード画面の緊急直送時出荷マニュアルをご確認下さい。");
                                            break;
                                    }
                            }*/

                            // INVOICE№
                            //pData.INVOICE = str[(int)EnumDataField.INVOICE].Trim();
                            //処理追加　送り状にデータが有る場合はInvoiceNoに送り状データを入れ、無い場合はInvoiceをInvoiceNoに入れる 20151217
                            if (str[(int)EnumDataField.OkuriJyouNo].Trim() != "")
                                zData.TourokuData.invoiceNo = str[(int)EnumDataField.OkuriJyouNo].Trim();
                            else
                                zData.TourokuData.invoiceNo = str[(int)EnumDataField.INVOICE].Trim();

                            //2016/03/04 追加　運送業者コード
                            zData.TourokuData.UnsouGyoushaCode = str[(int)EnumDataField.UnsouGyoushaCD].Trim();

                            //2016/03/04 追加　運送業者名
                            zData.TourokuData.UnsouGyoushaMei = str[(int)EnumDataField.UnsouGyoushaMei].Trim();

                            //2016/03/04 追加　小口数
                            zData.TourokuData.KogutiSu = str[(int)EnumDataField.KogutiSu].Trim();

                            lstTourokuData.Add(zData);
                            denpyou = zData.TourokuData;
                        }
                        

                        // 送品区分
                        try
                        {
                            denpyou.SouhinKubun = (EnumSouhinKubun)int.Parse(str[(int)EnumDataField.SouhinKubun]);
                        }
                        catch
                        {
                            throw new Exception("送品区分を正しく入力してください。");
                        }

                        // 伝票発行日
                        try
                        {
                            denpyou.dtHakkouBi = new Core.Type.Nengappi(int.Parse(str[(int)EnumDataField.DenpyouHakkouBi])).ToDateTime();
                        }
                        catch
                        {
                            throw new Exception("伝票発行日は8桁で入力してください。");
                        }
                        if (new Core.Type.Nengappi(denpyou.dtHakkouBi).ToYYYYMMDD() < Core.Type.Nengappi.Today.ToYYYYMMDD())
                            throw new Exception("伝票発行日は本日以降の日付を入力してください。");
                        else if (new Core.Type.Nengappi(denpyou.dtHakkouBi).ToYYYYMMDD() > Core.Type.Nengappi.Parse("2099/12/31").ToYYYYMMDD())
                            throw new Exception("伝票発行日は2099/12/31以前の日付を入力してください。");

                        denpyou.HokanBasho = str[(int)EnumDataField.HokanBasho];
                        denpyou.NiukeJigyoushoCode = str[(int)EnumDataField.NiukeJigyoushoCode];
                        MizunoDataSet.M_NiukeBumonBashoDataTable dtNiuke = NiukeBumonClass.getM_NiukeBumonBashoDataTable(denpyou.NiukeJigyoushoCode, denpyou.HokanBasho, c);
                        if (0 == dtNiuke.Count)
                        {
                            throw new Exception(string.Format("オーダーNo：{0}の送品区分、事業所、保管場所の組み合わせが正しくありません", strChumonNo));
                        }
                        denpyou.NiukeJigyoushoMei = dtNiuke[0].NiukeBumonMei;
                        denpyou.NiukeBashoMei = dtNiuke[0].NiukeBashoMei;
                        if (str[(int)EnumDataField.NouhinKigou].ToString() != "") //納品記号に何も無ければ空白を登録 20160711
                        {
                            denpyou.NouhinKigou = StrCnvToHankaku(str[(int)EnumDataField.NouhinKigou]).ToUpper();   // 大文字で
                            if (null == NouhinKigouClass.getM_NouhinKigouRow(denpyou.NouhinKigou, c))
                                throw new Exception(string.Format("オーダーNo：{0}の納品記号が正しくありません", strChumonNo));
                        }
                        else
                            denpyou.NouhinKigou = "";

                        //緊急直送は除外 20160824
                        //材料は緊急直送の特殊仕様は不要という事でコメントアウト　高山さん要望 20170213
                        //if (!(str[(int)EnumDataField.SouhinKubun].Trim() == "4" && dtZairyou[0].UriwatashisakiCode == ""))
                        //{
                            if (!NiukeBumonClass.CheckBumonBasho(denpyou.SouhinKubun, denpyou.NiukeJigyoushoCode, denpyou.HokanBasho, denpyou.NouhinKigou, c))
                                throw new Exception(string.Format("オーダーNo：{0}の送品区分、事業所、保管場所の組み合わせが正しくありません", strChumonNo));
                        //}

                        // INVOICE№
                        //処理追加　送り状にデータが有る場合はInvoiceNoに送り状データを入れ、無い場合はInvoiceをInvoiceNoに入れる 20160307
                        if (str[(int)EnumDataField.OkuriJyouNo].Trim() != "")
                            denpyou.invoiceNo = str[(int)EnumDataField.OkuriJyouNo].Trim();
                        else
                            denpyou.invoiceNo = str[(int)EnumDataField.INVOICE].Trim();

                        //2016/03/07 追加　運送業者コード
                        denpyou.UnsouGyoushaCode = str[(int)EnumDataField.UnsouGyoushaCD].Trim();

                        //2016/03/07 追加　運送業者名
                        denpyou.UnsouGyoushaMei = str[(int)EnumDataField.UnsouGyoushaMei].Trim();

                        //2016/03/07 追加　小口数
                        denpyou.KogutiSu = str[(int)EnumDataField.KogutiSu].Trim();

                        // 以下、半角チェックは済んでいる
                        denpyou.SKTantoushaCode = "";   // アップロード時はコードはブランク
                        denpyou.SKTantoushaMei = str[(int)EnumDataField.SeisanTantoushaMei];

                        denpyou.EigyouTantoushaCode = "";
                        denpyou.EigyouTantoushaMei = str[(int)EnumDataField.EigyouTantoushaMei];

                        denpyou.HanbaitenKigyouRyakuMei = str[(int)EnumDataField.Okurisaki];
                        denpyou.TenMei = str[(int)EnumDataField.TenMei];
                        denpyou.TeamMei = str[(int)EnumDataField.TeamMei];
                    }
                    else
                    {
                        // 2行目以降

                        try
                        {
                            denpyou.dtHakkouBi = new Core.Type.Nengappi(int.Parse(str[(int)EnumDataField.DenpyouHakkouBi])).ToDateTime();
                        }
                        catch
                        {
                            throw new Exception("伝票発行日は8桁で入力してください。");
                        }
                        if (new Core.Type.Nengappi(denpyou.dtHakkouBi).ToYYYYMMDD() < Core.Type.Nengappi.Today.ToYYYYMMDD())
                            throw new Exception("伝票発行日は本日以降の日付を入力してください。");
                        else if (new Core.Type.Nengappi(denpyou.dtHakkouBi).ToYYYYMMDD() > Core.Type.Nengappi.Parse("2099/12/31").ToYYYYMMDD())
                            throw new Exception("伝票発行日は2099/12/31以前の日付を入力してください。");

                        // 伝票の共通項目が前行と同じかどうかチェック
                        for (int i = 0; i < str.Length; i++)
                        {
                            switch ((EnumDataField)i)
                            {
                                case EnumDataField.HacchuBi:
                                case EnumDataField.HacchuNo:
                                case EnumDataField.SouhinKubun:
                                case EnumDataField.NiukeJigyoushoCode:
                                case EnumDataField.HokanBasho:
                                case EnumDataField.SPNo:
                                case EnumDataField.UnsouGyoushaMei:
                                case EnumDataField.OkuriJyouNo:
                                case EnumDataField.SeisanTantoushaMei:
                                case EnumDataField.EigyouTantoushaMei:
                                case EnumDataField.Okurisaki:
                                case EnumDataField.TenMei:
                                case EnumDataField.TeamMei:
                                    if (!str[i].Equals(strPrevData[i]))
                                        throw new Exception(string.Format("{0}が{1}行目と異なっている為、行Noを1から開始して下さい", strFieldMei[i], nLine - 1));
                                    break;
                            }
                        }

                        object objTourokuData = lstTourokuData[lstTourokuData.Count - 1];  // 現在の伝票を取得
                        zData = null;
                        
                        zData = objTourokuData as ZairyouOrderTourokuData;
                        denpyou = zData.TourokuData;

                    }

                    strPrevData = str;  // 確認

                    // ----- 明細 -----
                    NouhinDataClass.MeisaiDataCommon meisai = null;
                    ZairyouOrderClass.MeisaiData meisai_z = null;
                    
                    meisai = meisai_z = new ZairyouOrderClass.MeisaiData();

                    meisai.Tekiyou = str[(int)EnumDataField.Tekiyou].Trim();
                    switch (meisai.NouhinKubun)
                    {
                        case EnumNouhinKubun.TokuisakiHimoduke:
                        case EnumNouhinKubun.OrderHimoduke:
                            // 摘要が必須のはずだけど
                            if ("" == meisai.Tekiyou)
                            {
                                throw new Exception("納品区分が1もしくは2の場合は摘要を入力してください。");
                            }
                            break;
                    }

                    meisai.Tekiyou = str[(int)EnumDataField.Tekiyou];
                    string strHinban = StrCnvToHankaku(str[(int)EnumDataField.Hinban].Trim());
                    string strSize = str[(int)EnumDataField.Size].Trim();
                    string strLotNo = str[(int)EnumDataField.LotNo].Trim();

                    string strTokushuHinbanCode = th.GetHinban(strHinban);  // 特殊品番の場合は、「運賃」とかが入力されている

                    meisai.TuikaHinban = !string.IsNullOrEmpty(strTokushuHinbanCode);
                    meisai.ShukkaSu2 = decimal.Parse(str[(int)EnumDataField.Suuryou]);

                    if (meisai.TuikaHinban)
                    {
                        meisai_z.Key.Hinban = "ﾄｸｼｭﾋﾝｰ" + strTokushuHinbanCode;
                        meisai_z.Key.Size = "";
                        meisai_z.Key.LotNo = strLotNo;
                        // 軽減税率　追加 20190821 M0458
                        MizunoDataSet.M_HinmokuRow drHinmoku = HinbanClass.getM_HinmokuRow(meisai_z.Key.Hinban, meisai_z.Key.Size, c);
                        string strKeigenZeiritsu = "";
                        if (drHinmoku != null)
                        {
                            if (!drHinmoku.IsKeigenZeiritsuFlgNull())
                                strKeigenZeiritsu = drHinmoku.KeigenZeiritsuFlg.ToString();
                        }
                        meisai_z.KeigenZeiritsu = strKeigenZeiritsu;

                        zData.AddMeisai(bGyouNo, meisai_z);

                        meisai.KanryouFlag = true;  // 自動的に完了
                        if (0 >= meisai.ShukkaSu2) throw new Exception("数量は1以上で入力して下さい");   // 追加品番で数量0は有り得ない
                    }
                    else
                    {
                        if (null != zData)
                        {

                            daZairyou.SelectCommand.Parameters["@z"].Value = zData.ZairyouOrderKey.SashizuBi;
                            daZairyou.SelectCommand.Parameters["@h"].Value = zData.ZairyouOrderKey.HikitoriOrderNo;
                            daZairyou.SelectCommand.Parameters["@Hinban"].Value = strHinban;
                            daZairyou.SelectCommand.Parameters["@lot"].Value = strLotNo;

                            MizunoDataSet.T_ZairyouOrderDataTable dtZairyou = new MizunoDataSet.T_ZairyouOrderDataTable();

                            daZairyou.Fill(dtZairyou);
                            if (0 == dtZairyou.Count) throw new Exception("発注データが存在しません。");
                            if (long.Parse(dtZairyou[0].ShiiresakiCode) != long.Parse(strShiiresakiCode))
                                throw new Exception("仕入先コードが正しくありません。");

                            meisai_z.Key.Hinban = dtZairyou[0].Hinban;   // DBの品番をセットする。（頭に空白があったりするので、DBの内容に合わせる為）
                            meisai_z.Key.Size = "";
                            meisai_z.Key.LotNo = strLotNo;

                            decimal nZansu = 0;
                            for (int k = 0; k < dtZairyou.Count; k++)
                            {
                                MizunoDataSet.T_ZairyouOrderRow drZ = dtZairyou[k];
                                if (drZ.KanryouFlg) continue;


                                decimal n = drZ.Suuryou - drZ.ShukkaSuu;
                                if (0 >= n) continue;

                                if (string.IsNullOrEmpty(zData.NouhinKigou))
                                    zData.NouhinKigou = drZ.NouhinKigou;
                                else
                                {
                                    if (zData.NouhinKigou != drZ.NouhinKigou)
                                    {
                                        // 基本的に、注文内の品番+サイズで納品記号が異なるケースは無いが、念の為
                                        throw new Exception(string.Format("納品記号が一致しません。", strChumonNo));
                                    }
                                }

                                // 必須チェック 元々別の保管場所が設定されている状態で保管場所が「DRCT」の場合はエラー 20160302 M0337 改めて反映 20180315
                                if (drZ.HokanBasho != "")
                                {
                                    //保管場所が設定されていたのに空白にするのはダメです。
                                    if (drZ.HokanBasho.Trim() != "" && str[(int)EnumDataField.HokanBasho] == "")
                                    {
                                        throw new Exception("発注内容で保管場所が空白以外で設定されているので保管場所を空白以外で選択して下さい");
                                    }
                                }

                                nZansu += n;
                            }
                            if (0 == nZansu)
                                throw new Exception(string.Format("発注データが存在しません。材料品オーダーNo：{0}の注文は終了しています。", strChumonNo));

                            
                            meisai_z.Key.NiukeJigyoushoCode = denpyou.NiukeJigyoushoCode;
                            meisai_z.Key.NiukeJigyoushoCodeHikiateAll = false;

                            // 軽減税率　追加 20190821 M0458
                            MizunoDataSet.M_HinmokuRow drHinmoku = HinbanClass.getM_HinmokuRow(meisai_z.Key.Hinban, meisai_z.Key.Size, c);
                            string strKeigenZeiritsu = "";
                            if (drHinmoku != null)
                            {
                                if (!drHinmoku.IsKeigenZeiritsuFlgNull())
                                    strKeigenZeiritsu = drHinmoku.KeigenZeiritsuFlg.ToString();
                            }
                            meisai_z.KeigenZeiritsu = strKeigenZeiritsu;

                            if (!zData.AddMeisai(bGyouNo, meisai_z))
                                throw new Exception("行Noが重複しています。");


                            // 完了フラグのチェック
                            string strKey = string.Format("{0}_{1}_{2}_{3}_{4}",
                                zData.ZairyouOrderKey.SashizuBi, zData.ZairyouOrderKey.HikitoriOrderNo, "", meisai_z.Key.Hinban, meisai_z.Key.Size);

                            ZairyouOrderClass.TourokuData zd = (tblZairyouKanryouData.ContainsKey(strKey)) ? tblZairyouKanryouData[strKey] : null;

                            if (meisai.KanryouFlag)
                            {
                                // 別の伝票で既に完了フラグがセットされていればエラー
                                if (null == zd)
                                {
                                    tblZairyouKanryouData.Add(strKey, zData.TourokuData);
                                }
                                else
                                {
                                    if (zd != zData.TourokuData)
                                        throw new Exception(string.Format("予算品オーダーNo：{0}、品番:{1}、サイズ:{2}の完了フラグが別の伝票で既に登録されています。",
                                            strChumonNo, meisai_z.Key.Hinban, meisai_z.Key.Size));
                                }
                            }
                            else
                            {
                                
                                if (null != zd && zd != zData.TourokuData)
                                {
                                    if (zd.dtHakkouBi <= zData.TourokuData.dtHakkouBi)
                                    {
                                        throw new Exception(string.Format("予算品オーダーNo：{0}、品番:{1}、サイズ:{2}の完了フラグは伝票発行日{0:yyyy/MM/dd}の伝票で既にセットされています。",
                                            strChumonNo, meisai_z.Key.Hinban, meisai_z.Key.Size, zd.dtHakkouBi));
                                    }
                                }
                            }
                        }
                        

                        // 完了フラグ
                        string strKanryouFlg = str[(int)EnumDataField.KanryouFlg].Trim();
                        if ("0" != strKanryouFlg && "9" != strKanryouFlg) throw new Exception("完了フラグは0または9を入力してください");
                        meisai.KanryouFlag = (KanryouFlg.Kanryou == strKanryouFlg);

                        if (0 > meisai.ShukkaSu2) throw new Exception("数量は0以上で入力して下さい");
                        if (KanryouFlg.MiKanryou == strKanryouFlg && 0 == meisai.ShukkaSu2) throw new Exception("完了フラグが0の時は、数量を1以上で入力して下さい");
                        if (meisai.KanryouFlag && 0 == meisai.ShukkaSu2) nSuryouZeroDataCount++;
                        // 「数量0、完了フラグ = 9」のデータと、「数量1以上」のデータが1伝票内に共存してはいけない。
                        if (0 < meisai.ShukkaSu2 && 0 < nSuryouZeroDataCount)
                        {
                            throw new Exception(string.Format("オーダーNo：{0}:数量=0、完了フラグ=9のデータは数量1以上のデータと伝票を分けて下さい", strChumonNo));
                        }


                        // 自由使用欄
                        meisai.Free1 = str[(int)EnumDataField.JiyuKoumoku1];
                        meisai.Free2 = str[(int)EnumDataField.JiyuKoumoku2];
                        meisai.Free3 = str[(int)EnumDataField.JiyuKoumoku3];
                        // 31桁備考
                        meisai.Bikou = "";
                    }
                }
                catch (Exception e)
                {
                    strPrevData = str;
                    lstErrorMsg.Add(string.Format("{0}行目 - {1}", nLine, e.Message));
                }
            }

            if (0 < lstErrorMsg.Count) return new Error("登録に失敗しました。");

            if (0 == lstTourokuData.Count) return new Error("登録するデータがありません。");

            
            SqlTransaction t = null;

            int nDenpyouIndex = 0;
            Core.Error ret = null;
            try
            {
                
                for (nDenpyouIndex = 0; nDenpyouIndex < lstTourokuData.Count; nDenpyouIndex++)
                {
                    List<NouhinDataClass.DenpyouKey> lst = null;
                    List<NouhinDataClass.DenpyouKey> lstShukkaAri = null;
                    if (lstTourokuData[nDenpyouIndex] is ZairyouOrderTourokuData)
                    {
                        // 予算品
                        ZairyouOrderTourokuData z = lstTourokuData[nDenpyouIndex] as ZairyouOrderTourokuData;
                        z.SetMeisaiData();                        

                        int nMaxGyouSu = GetDenpyouGyouCount(z.TourokuData.SouhinKubun);
                        if (nMaxGyouSu < z.TourokuData.lstMeisai.Count)
                        {
                            string strChumonBangou = z.ZairyouOrderKey.HikitoriOrderNo;

                            throw new Exception(string.Format("注文No.{0}が伝票作成可能な行数を超えています。(最大行数={1})",
                                strChumonBangou, nMaxGyouSu));
                        }

                        ret = ZairyouOrderClass.DenpyouTouroku(true, z.ZairyouOrderKey, z.NouhinKigou, z.TourokuData, c, out lst, out lstShukkaAri);
                    }
                    

                    if (ret == null)
                    {
                        if (0 < lst.Count) lstDenpyouKey.AddRange(lst);
                        if (null != lstShukkaAri && 0 < lstShukkaAri.Count) lstDenpyouKeyShukkaAri.AddRange(lstShukkaAri);
                    }
                    else
                    {
                        return new Core.Error(string.Format("{0}枚目の伝票 - {1}", nDenpyouIndex + 1, ret.Message));
                    }
                }

                //t.Commit();

                return null;
            }
            catch (Exception e)
            {
                //if (null != t) t.Rollback();
                return new Core.Error(string.Format("{0}枚目の伝票 - {1}", nDenpyouIndex + 1, e.Message));
            }
            finally
            {
                //c.Close();
            }
        }

        public static string get_ZHinban(SqlConnection sql, string ShiiresakiCode, string SashizuBi)
        {
            string Hinban = "";

            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText = "select Hinban from T_ZairyouOrder where ShiiresakiCode=@sc and SashizuBi=@sb";
            da.SelectCommand.Parameters.AddWithValue("@sc", ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@sb", SashizuBi);
            
            DataTable dt = new DataTable();
            da.Fill(dt);
            Hinban = dt.Rows[0][0].ToString();
            return Hinban;
        }

        public static string get_ZNiukeJigyoushoCode(SqlConnection sql, string ShiiresakiCode, string SashizuBi, string Hinban)
        {
            string Code = "";
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText = "select NiukeJigyoushoCode from T_ZairyouOrder where ShiiresakiCode=@sc and SashizuBi=@sb and Hinban=@hb";
            da.SelectCommand.Parameters.AddWithValue("@sc", ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@sb", SashizuBi);
            da.SelectCommand.Parameters.AddWithValue("@hb", Hinban);

            DataTable dt = new DataTable();
            da.Fill(dt);
            Code = dt.Rows[0][0].ToString();


            return Code;
        }

        public static string get_ZNiukeJigyoushoCode2(SqlConnection sql, string ShiiresakiCode, string SashizuBi, string Hinban ,string OrderNo)
        {
            string Code = "";
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            //da.SelectCommand.CommandText = "select NiukeJigyoushoCode from T_ZairyouOrder where ShiiresakiCode=@sc and SashizuBi=@sb and Hinban=@hb and HikitoriOrderNo=@hn";
            da.SelectCommand.CommandText = "select NiukeJigyoushoCode from T_ZairyouOrder where ShiiresakiCode=@sc and SashizuBi=@sb and HikitoriOrderNo=@hn";
            da.SelectCommand.Parameters.AddWithValue("@sc", ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@sb", SashizuBi);
            //da.SelectCommand.Parameters.AddWithValue("@hb", Hinban);
            da.SelectCommand.Parameters.AddWithValue("@hn", OrderNo);

            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                Code = dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
            return Code;
        }

        public static string get_HokanBasho(SqlConnection sql, string ShiiresakiCode, string SashizuBi, string Hinban)
        {
            string Basho = "";
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText = "select HokanBasho from T_ZairyouOrder where ShiiresakiCode=@sc and SashizuBi=@sb and Hinban=@hb";
            da.SelectCommand.Parameters.AddWithValue("@sc", ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@sb", SashizuBi);
            da.SelectCommand.Parameters.AddWithValue("@hb", Hinban);

            DataTable dt = new DataTable();
            da.Fill(dt);
            try
            {
                Basho = dt.Rows[0][0].ToString();
            }
            catch
            {
            }
            return Basho;
        }

        public static string get_HokanBasho(SqlConnection sql, string LotNo, string Hinban,string orderno,string sashizubi)
        {
            string Basho = "";
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText = "select HokanBasho from T_ZairyouOrder where LotNo=@sb and Hinban=@hb and HikitoriOrderNo=@hi and SashizuBi=@bi ";
            da.SelectCommand.Parameters.AddWithValue("@sb", LotNo);
            da.SelectCommand.Parameters.AddWithValue("@hb", Hinban);
            da.SelectCommand.Parameters.AddWithValue("@hi", orderno);
            da.SelectCommand.Parameters.AddWithValue("@bi", sashizubi);

            DataTable dt = new DataTable();
            da.Fill(dt);
            try
            {
                Basho = dt.Rows[0][0].ToString();
            }
            catch
            {
            }
            return Basho;
        }

        // ★納品処理
        // 予算品、別注品共に、現状のフォーマットでは引き当てる先が分らない
        // 予算品：予算月、納品記号、事業所、品番、サイズ、価格が登録時のキーでこのキーで出荷数引き当て
        // 別注品：品番、サイズ、価格、RowNo(別注2の場合のみ)
        // →予算品と別注品でフォーマットをわける
        public static Core.Error UploadNouhnData_OLD(string strShiiresakiCode, System.IO.Stream stream,
            System.Text.Encoding enc, SqlConnection c, out List<string> lstErrorMsg, 
            out List<NouhinDataClass.DenpyouKey> lstDenpyouKey, out List<NouhinDataClass.DenpyouKey> lstDenpyouKeyShukkaAri)
        {

            lstErrorMsg = new List<string>();
            lstDenpyouKey = new List<NouhinDataClass.DenpyouKey>();
            lstDenpyouKeyShukkaAri = new List<DenpyouKey>();

            // 予算品データ取得、注文番号は、生産オーダーNoか引取オーダーNoのどちらか
            SqlDataAdapter daProperKey = new SqlDataAdapter("", c);
            daProperKey.SelectCommand.CommandText = @"
SELECT                  dbo.T_ProperOrder.*
FROM                     dbo.T_ProperOrder
WHERE                   (SashizuBi = @z) AND ((SeisanOrderNo = @c AND ltrim(HikitoriOrderNo) = '') OR (ltrim(SeisanOrderNo)='' AND HikitoriOrderNo = @c))";
            daProperKey.SelectCommand.Parameters.AddWithValue("@z", "");
            daProperKey.SelectCommand.Parameters.AddWithValue("@c", "");

            // 予算月の件数取得
            SqlDataAdapter daGetYosanTuki = new SqlDataAdapter("", c);
            daGetYosanTuki.SelectCommand.CommandText = @"
SELECT                  MAX(YosanTsuki) AS YosanTsuki, COUNT(DISTINCT YosanTsuki) AS YosanTsukiCount
FROM                     dbo.T_ProperOrder
WHERE                   (SashizuBi = @z) AND (SeisanOrderNo = @s) AND (HikitoriOrderNo = @h)";
            daGetYosanTuki.SelectCommand.Parameters.AddWithValue("@z", "");
            daGetYosanTuki.SelectCommand.Parameters.AddWithValue("@s", "");
            daGetYosanTuki.SelectCommand.Parameters.AddWithValue("@h", "");


            // 別注品キー情報　MizunoUketsukeBiが文字列でいい加減なので日付で検索　yyyy/MM/ddの形式になっているとは限らない (2011/1/31のようなデータもある)
            SqlDataAdapter daBecchu = new SqlDataAdapter("", c);
            daBecchu.SelectCommand.CommandText = "SELECT * FROM VIEW_BecchuKeyInfo WHERE OrderKanriNo=@ChumonBangou AND ShiiresakiCode = @s and CONVERT(char(8),CAST(MizunoUketsukeBi as date), 112) = @MizunoUketsukeBi AND ISDATE(MizunoUketsukeBi) = 1";
            daBecchu.SelectCommand.Parameters.AddWithValue("@s", strShiiresakiCode);
            daBecchu.SelectCommand.Parameters.AddWithValue("@ChumonBangou", "");
            daBecchu.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", "");


            // 予算品の品番+サイズの明細取得
            SqlDataAdapter daProper = new SqlDataAdapter("", c);
            daProper.SelectCommand.CommandText = @"
SELECT                  dbo.T_ProperOrder.*
FROM                     dbo.T_ProperOrder
WHERE                   (SashizuBi = @z) AND SeisanOrderNo = @s AND HikitoriOrderNo = @h AND YosanTsuki=@y and (Ltrim(Hinban) = @Hinban) AND (Size = @Size)";
            daProper.SelectCommand.Parameters.AddWithValue("@z", "");
            daProper.SelectCommand.Parameters.AddWithValue("@s", "");
            daProper.SelectCommand.Parameters.AddWithValue("@h", "");
            daProper.SelectCommand.Parameters.AddWithValue("@y", 0);
            daProper.SelectCommand.Parameters.AddWithValue("@Hinban", "");
            daProper.SelectCommand.Parameters.AddWithValue("@Size", "");

            System.IO.StreamReader tabReader = null;
            Core.IO.CSVReader csvReader = null;

            System.IO.StreamReader check = new System.IO.StreamReader(stream, enc);
            string strCheck = check.ReadLine();  // CSVとタブ区切りの確認の為
            if (null == strCheck)
            {
                return new Core.Error("データがありません。");
            }
            bool bTab = (strCheck.Split('\t').Length > strCheck.Split(',').Length);   // これ良いの？
            stream.Position = 0;
            if (bTab)
                tabReader = new System.IO.StreamReader(stream, enc);
            else
                csvReader = new Core.IO.CSVReader(stream, enc);

            int[] nDataMaxLength = new int[]{
7,
8,
40,
3,
8,
1,
4,
2,
3,
6,
2,
10,
5,
7,
7,
5,
1,
1,
20,
4,
10,
10,
10,
10,
20,     // 25項目目
20,     // 自由項目1
20,     // 自由項目2
20,     // 自由項目3
6,       // 予算品予算月(YYYYMM)
2,       // 別注2行No
14,     // スペクトラオーダーNo
15,     // 運送
15,     // 送り状No
10      // 仕入金額合計
        };

            string[] strFieldMei = new string[]{            
"仕入先ｺｰﾄﾞ",
"指図日(ミズノ受付日)",
"ｵｰﾀﾞｰ№",
"伝区",
"伝票発行日",
"送品区分",
"荷受部門ｺｰﾄﾞ",
"荷受場所ｺｰﾄﾞ",
"納品記号",
"本納品月",
"行No",
"品番",
"サイズ",
"仕入価格",
"融通価格",
"数量",
"完了フラグ",
"納品区分",
"摘要",
"籍付",
"生産担当者",
"営業担当者",
"送り先",
"店名",
"学校/チーム名",
"自由使用欄1",
"自由使用欄2",
"自由使用欄3",
"予算月",
"別注品明細No",
"ｽﾍﾟｸﾄﾗｵｰﾀﾞｰ№",
"運送業者名",
"送り状№",
"仕入合計金額"
            };

            TsuikaHinban th = new TsuikaHinban();

            NouhinDataClass.TourokuDataBase denpyou = null;
            ProperOrderTourokuData pData = null;
            BecchuOrderTourokuData bData = null;

            List<object> lstTourokuData = new List<object>();


            DateTime dtNow = DateTime.Now;
            ViewDataset.VIEW_BecchuKeyInfoRow drBecchuKey = null;
            BecchuOrderClass.EnumBecchuKubun BecchuKubun = BecchuOrderClass.EnumBecchuKubun.None;
            ViewDataset.VIEW_Becchu_ShukkaMeisaiDataTable dtBecchu = null;
            ViewDataset.VIEW_Becchu2_ShukkaMeisaiDataTable dtBecchu2 = null;
            ViewDataset.VIEW_DS_ShukkaMeisaiDataTable dtDS = null;
            ViewDataset.VIEW_SP_ShukkaMeisaiDataTable dtSP = null;
            MizunoDataSet.M_TsuikaHinbanDataTable dtTuikaHinban4Becchu = null;  // 種目コード別の追加品番


            // 完了フラグの立っているデータ
            Dictionary<string, ProperOrderClass.TourokuData> tblProperKanroyuData = new Dictionary<string, ProperOrderClass.TourokuData>(); // キー=指図日_生産オーダーNo_引取オーダーNo_品番_サイズ
            Dictionary<string, BecchuOrderClass.TourokuData> tblBecchuKanroyuData = new Dictionary<string, BecchuOrderClass.TourokuData>();


            // 「数量0&完了フラグ = 9」のデータと、「数量1以上」のデータが1伝票内に共存してはいけない。
            int nSuryouZeroDataCount = 0;

            int nLine = 0;
            string[] str = null;
            string[] strPrevData = null;

            while (true)
            {
                try
                {
                    string strLine = null;

                    string[] strArray = null;

                    if (null != tabReader)
                    {
                        strLine = tabReader.ReadLine();
                        if (null == strLine) break;
                        strArray = strLine.Split('\t');
                    }
                    else
                    {
                        strArray = csvReader.GetCSVLine(ref strLine);
                        if (string.IsNullOrEmpty(strLine)) break;
                        if (null == strArray || 0 == strArray.Length) break;
                    }

                    str = new string[34];
                    if (strArray.Length == str.Length)
                        str = strArray;
                    else {
                        for (int i = 0; i < str.Length; i++) str[i] = "";

                        // 旧バージョンのフォーマットの場合
/*
【旧フォーマット1】26項目
"仕入先ｺｰﾄﾞ",
"指図日(ミズノ受付日)",
"ｵｰﾀﾞｰ№",
"伝区",
"伝票発行日",
"送品区分",
"荷受部門ｺｰﾄﾞ",
"荷受場所ｺｰﾄﾞ",
"納品記号",
"本納品月",
"行No",
"品番",
"サイズ",
"仕入価格",
"融通価格",
"数量",
"完了フラグ",
"納品区分",
"摘要",
"籍付",
"生産担当者",
"営業担当者",
"送り先",
"店名",
"学校/チーム名",
"ｽﾍﾟｸﾄﾗｵｰﾀﾞｰ№"

 【旧フォーマット2】29項目  運送業者名,送り状№,仕入合計金額の3項目が追加
 
"仕入先ｺｰﾄﾞ",
"指図日(ミズノ受付日)",
"ｵｰﾀﾞｰ№",
"伝区",
"伝票発行日",
"送品区分",
"荷受部門ｺｰﾄﾞ",
"荷受場所ｺｰﾄﾞ",
"納品記号",
"本納品月",
"行No",
"品番",
"サイズ",
"仕入価格",
"融通価格",
"数量",
"完了フラグ",
"納品区分",
"摘要",
"籍付",
"生産担当者",
"営業担当者",
"送り先",
"店名",
"学校/チーム名",
"ｽﾍﾟｸﾄﾗｵｰﾀﾞｰ№",
"運送業者名",
"送り状№",
"仕入合計金額"
 
 */
                        if (strArray.Length <= 26) 
                        {
                            // 【旧フォーマット1】
                            // "学校/チーム名"まで25項目読み込む
                            for (int i = 0; i < 25; i++)
                            {
                                if (i >= strArray.Length) break;
                                str[i] = strArray[i];
                            }
                            if (26 == strArray.Length)
                                str[(int)EnumDataField.SPNo] = strArray[25];
                        }
                        else if (strArray.Length <= 29)
                        {
                            //  【旧フォーマット2】
                            for (int i = 0; i < 25; i++)
                                str[i] = strArray[i];
                            str[(int)EnumDataField.SPNo] = strArray[25];
                            if (27 <= strArray.Length) str[(int)EnumDataField.UnsouGyoushaMei]  = strArray[26];
                            if (28 <= strArray.Length) str[(int)EnumDataField.OkuriJyouNo]      = strArray[27];
                            if (29 == strArray.Length) str[(int)EnumDataField.INVOICE]     = strArray[28];
                        }
                        else {
                            // 最新フォーマット34項目
                            for (int i = 0; i < strArray.Length; i++)
                                str[i] = strArray[i];
                        }
                    }

                    for (int i = 0; i < str.Length; i++)
                        str[i] = str[i].Trim();

                    nLine++;

                    for (int i = 0; i < str.Length; i++)
                    {
                        string s = str[i].Trim();
                        // 桁数チェック
                        if (nDataMaxLength[i] < s.Length)
                            throw new Exception(string.Format("{0}列目({1})が規定桁数を上回っています(規定桁数={2})", i + 1, strFieldMei[i], s.Length));
                        // 必須項目チェック
                        if ("" == s)
                        {
                            switch ((EnumDataField)i)
                            {
                                case EnumDataField.ShiiresakiCode:
                                case EnumDataField.HacchuBi:
                                case EnumDataField.HacchuNo:
                                //case EnumDataField.DenpyouKubun:
                                case EnumDataField.DenpyouHakkouBi:
                                case EnumDataField.SouhinKubun:
                                case EnumDataField.NiukeJigyoushoCode:
                                //case EnumDataField.HokanBasho:
                                case EnumDataField.NouhinKigou:
                                case EnumDataField.Hinban:
                                case EnumDataField.Suuryou:
                                case EnumDataField.KanryouFlg:
                                    throw new Exception(string.Format("{0}列目({1})は必須項目です", i + 1, strFieldMei[i]));
                            }
                        }
                        // 数値項目のチェック
                        if ("" != s)
                        {
                            str[i] = StrCnvToHankaku(s).Trim();
                            s = str[i]; 
                            switch ((EnumDataField)i)
                            {
                                case EnumDataField.ShiiresakiCode:
                                case EnumDataField.HacchuBi:
                                case EnumDataField.DenpyouHakkouBi:
                                case EnumDataField.NiukeJigyoushoCode:
                                //case EnumDataField.HonNouhinTsuki:
                                case EnumDataField.GyouNo:
                                //case EnumDataField.YuuzuuKakaku:
                                case EnumDataField.Suuryou:
                                case EnumDataField.KanryouFlg:
                                //case EnumDataField.Sekiduke:
                                //case EnumDataField.GoukeiKakaku:
                                case EnumDataField.BecchuHin_RowNo:
                                case EnumDataField.YosanTuki:
                                    try
                                    {
                                        int.Parse(s);
                                    }
                                    catch
                                    {
                                        throw new Exception(string.Format("{0}列目({1})は数値項目です", i + 1, strFieldMei[i]));
                                    }
                                    break;
                                //case EnumDataField.ShiireKakaku:
                                //    // 仕入価格
                                //    decimal dKakaku = 0;
                                //    try
                                //    {
                                //        dKakaku = decimal.Parse(s);
                                //    }
                                //    catch
                                //    {
                                //        throw new Exception(string.Format("{0}列目({1})は数値項目です", i + 1, strFieldMei[i]));
                                //    }
                                //    if (0 > dKakaku) throw new Exception("仕入価格を正しく入力してください。");
                                //    // 整数部チェック
                                //    if (decimal.Truncate(dKakaku) > 999999) throw new Exception("仕入価格の整数部は6桁以内で入力して下さい");
                                //    // 小数部チェック
                                //    if (1 < Core.Utility.Decimal.GetDecimalKetaSu(dKakaku)) throw new Exception("仕入価格の小数部は1桁以内で入力して下さい");

                                //    break;
                            }

                            // 半角チェック
                            switch ((EnumDataField)i)
                            {
                                case EnumDataField.SeisanTantoushaMei:
                                case EnumDataField.EigyouTantoushaMei:
                                case EnumDataField.Okurisaki:
                                case EnumDataField.TenMei:
                                case EnumDataField.TeamMei:
                                case EnumDataField.Tekiyou:     // 摘要
                                    str[i] = StrCnvToHankaku(str[i]);
                                    if (!Core.Utility.String.IsHankaku(str[i]))
                                    {
                                        throw new Exception(string.Format("{0}列目({1})は半角で入力してください。", i + 1, strFieldMei[i]));
                                    }
                                    break;
                            }
                        }
                    }

                    // 仕入先コード
                    if (int.Parse(strShiiresakiCode) != int.Parse(str[(int)EnumDataField.ShiiresakiCode]))
                        throw new Exception("仕入先コードが正しくありません。");

                    // 行No
                    bool bNewDenpyou = false;
                    byte bGyouNo = 0;
                    string strRowNo = str[(int)EnumDataField.GyouNo];
                    if ("" != strRowNo)
                    {
                        if (!byte.TryParse(strRowNo, out bGyouNo) || 0 == bGyouNo)
                        {
                            throw new Exception("行Noを正しく入力してください。");
                        }

                        if (1 != bGyouNo && 0 == lstTourokuData.Count)
                        {
                            return new Error("行Noは1から入力して下さい。");
                        }

                        bNewDenpyou = (1 == bGyouNo);
                    }

                    string strSashizuBi = str[(int)EnumDataField.HacchuBi].Trim();
                    string strChumonNo = str[(int)EnumDataField.HacchuNo].Trim();

                    if (bNewDenpyou)
                    {
                        // 新しい伝票の始まり
                        nSuryouZeroDataCount = 0;


                        // 予算品 or 別注品かの確認
                        pData = null; bData = null;  // リセット
                        daProperKey.SelectCommand.Parameters["@z"].Value = strSashizuBi;
                        daProperKey.SelectCommand.Parameters["@c"].Value = strChumonNo;

                        MizunoDataSet.T_ProperOrderDataTable dtProper = new MizunoDataSet.T_ProperOrderDataTable();
                        daProperKey.Fill(dtProper);
                        if (0 < dtProper.Count)
                        {
                            // 予算品
                            pData = new ProperOrderTourokuData();

                            pData.TourokuData = new ProperOrderClass.TourokuData();
                            pData.ProperOrderKey = new ProperOrderClass.ProperOrderKey(dtProper[0].SashizuBi, dtProper[0].SeisanOrderNo, dtProper[0].HikitoriOrderNo);

                            // 予算月取得
                            daGetYosanTuki.SelectCommand.Parameters["@z"].Value = pData.ProperOrderKey.SashizuBi;
                            daGetYosanTuki.SelectCommand.Parameters["@h"].Value = pData.ProperOrderKey.HikitoriOrderNo;
                            daGetYosanTuki.SelectCommand.Parameters["@s"].Value = pData.ProperOrderKey.SeisanOrderNo;
                            DataTable dtYosanTuki = new DataTable();
                            daGetYosanTuki.Fill(dtYosanTuki);
                            if (0 == dtYosanTuki.Rows.Count) return new Error("発注データが存在しません。");
                            int nYosanTuki = Convert.ToInt32(dtYosanTuki.Rows[0][0]);
                            int nYosanTukiCount = Convert.ToInt32(dtYosanTuki.Rows[0][1]);

                            if (1 < nYosanTukiCount)
                            {
                                // 本オーダーに予算月が複数ある場合
                                // 予算月を確認(1伝票に複数の予算月指定は不可)
                                string strYosanTuki = StrCnvToHankaku(str[(int)EnumDataField.YosanTuki].Trim());
                                if (6 != strYosanTuki.Length) throw new Exception("予算月を6桁(例:201106)で入力してください。");

                                try
                                {
                                    pData.YosanTuki = new Nengetu(int.Parse(strYosanTuki));
                                }
                                catch
                                {
                                    throw new Exception("予算月を正しく入力してください。");
                                }
                            }
                            else
                                pData.YosanTuki = new Nengetu(nYosanTuki);  // 予算月は1つで固定
                            

                            lstTourokuData.Add(pData);
                            denpyou = pData.TourokuData;
                        }
                        else
                        {
                            // 別注品を検索
                            bData = new BecchuOrderTourokuData();

                            daBecchu.SelectCommand.Parameters["@ChumonBangou"].Value = strChumonNo;
                            daBecchu.SelectCommand.Parameters["@MizunoUketsukeBi"].Value = new Core.Type.Nengappi(int.Parse(strSashizuBi)).ToYYYYMMDD().ToString();
                            ViewDataset.VIEW_BecchuKeyInfoDataTable dtB = new ViewDataset.VIEW_BecchuKeyInfoDataTable();
                            daBecchu.Fill(dtB);
                            if (0 == dtB.Count) throw new Exception("発注データが存在しません。");
                            drBecchuKey = dtB[0];

                            if (!drBecchuKey.IsCancelBiNull())
                                throw new Exception(string.Format("オーダーNo{0}はキャンセル済みです", strChumonNo));
                            if (drBecchuKey.KanryouFlg)
                                throw new Exception(string.Format("オーダーNo{0}は完了しています。", strChumonNo));

                            bData.BecchuOrderKey = new BecchuOrderClass.BecchuOrderKey(drBecchuKey.MizunoUketsukeBi, drBecchuKey.OrderKanriNo, drBecchuKey.ShiiresakiCode);

                            if (!drBecchuKey.IsT_Becchu_SashizuNoNull())
                            {
                                // 別注データ
                                BecchuKubun = BecchuOrderClass.EnumBecchuKubun.Becchu;
                                dtBecchu = BecchuOrderClass.getVIEW_Becchu_ShukkaMeisaiDataTable(bData.BecchuOrderKey, c);
                                dtBecchu.Columns.Add("HinbanTrim", typeof(string));
                                dtBecchu.Columns.Add("SizeTrim", typeof(string));
                                for (int i = 0; i < dtBecchu.Count; i++)
                                {
                                    dtBecchu[i]["HinbanTrim"] = dtBecchu[i].Hinban.Trim();
                                    dtBecchu[i]["SizeTrim"] = dtBecchu[i].Size.Trim();
                                }
                            }
                            else if (!drBecchuKey.IsT_Becchu2_SashizuNoNull())
                            {
                                // 別注2
                                BecchuKubun = BecchuOrderClass.EnumBecchuKubun.Becchu2;
                                dtBecchu2 = BecchuOrderClass.getVIEW_Becchu2_ShukkaMeisaiDataTable(bData.BecchuOrderKey, c);
                                dtBecchu2.Columns.Add("Zansu", typeof(decimal), "Suuryou-ShukkaSuu");
                                dtBecchu2.Columns.Add("HinbanTrim", typeof(string));
                                dtBecchu2.Columns.Add("SizeTrim", typeof(string));
                                for (int i = 0; i < dtBecchu2.Count; i++)
                                {
                                    dtBecchu2[i]["HinbanTrim"] = dtBecchu2[i].Hinban.Trim();
                                    dtBecchu2[i]["SizeTrim"] = dtBecchu2[i].Size.Trim();
                                }
                            }
                            else if (!drBecchuKey.IsT_DS_OrderKanriNoNull())
                            {
                                // DS
                                BecchuKubun = BecchuOrderClass.EnumBecchuKubun.DS;
                                dtDS = BecchuOrderClass.getVIEW_DS_ShukkaMeisaiDataTable(bData.BecchuOrderKey.OrderKanriNo, c);
                                dtDS.Columns.Add("HinbanTrim", typeof(string));
                                dtDS.Columns.Add("SizeTrim", typeof(string));
                                for (int i = 0; i < dtDS.Count; i++)
                                {
                                    dtDS[i]["HinbanTrim"] = dtDS[i].Hinban.Trim();
                                    dtDS[i]["SizeTrim"] = dtDS[i].Size.Trim();
                                }
                            }
                            else if (!drBecchuKey.IsT_SP_OrderKanriNoNull())
                            {
                                // SP
                                BecchuKubun = BecchuOrderClass.EnumBecchuKubun.SP;
                                dtSP = BecchuOrderClass.getVIEW_SP_ShukkaMeisaiDataTable(bData.BecchuOrderKey, c);
                                dtSP.Columns.Add("HinbanTrim", typeof(string));
                                dtSP.Columns.Add("SizeTrim", typeof(string));
                                for (int i = 0; i < dtSP.Count; i++)
                                {
                                    dtSP[i]["HinbanTrim"] = dtSP[i].Hinban.Trim();
                                    dtSP[i]["SizeTrim"] = dtSP[i].Size.Trim();
                                }
                            }
                            else
                                throw new Exception("発注データが存在しません。");  // 予算品、別注品共にデータがない


                            // ｽﾍﾟｸﾄﾗｵｰﾀﾞｰ№
                            bData.BecchuSpectraNo = str[(int)EnumDataField.SPNo].Trim();
                            // 運送業者名
                            bData.UnsouGyoushaMei = str[(int)EnumDataField.UnsouGyoushaMei].Trim();
                            // 送り状№
                            bData.OkuriJyouNo = str[(int)EnumDataField.OkuriJyouNo].Trim();
                            // INVOICE
                            bData.INVOICE = str[(int)EnumDataField.INVOICE].Trim();

                            bData.KinkyuChokusouBikou = str[(int)EnumDataField.KinkyuChokusouBikou].Trim();
                            //// 仕入合計金額
                            //string strGoukei = StrCnvToHankaku(str[(int)EnumDataField.GoukeiKakaku].Trim()).Replace(",", "");
                            //bData.GoukeiKakaku = null;
                            //if ("" != strGoukei) {
                            //    try
                            //    {
                            //        bData.GoukeiKakaku = int.Parse(strGoukei);
                            //        if (0 > bData.GoukeiKakaku.Value) throw new Exception("");
                            //    }
                            //    catch {
                            //        throw new Exception("仕入合計金額を正しく入力してください。");
                            //    }
                            //}

                            bData.TourokuData = new BecchuOrderClass.TourokuData();

                            lstTourokuData.Add(bData);
                            denpyou = bData.TourokuData;

                            // 当該種目コードに対応する追加品番取得
                            if (null != dtTuikaHinban4Becchu && 0 < dtTuikaHinban4Becchu.Count && dtTuikaHinban4Becchu[0].ShumokuCode == drBecchuKey.ShumokuCode) {
                                // 前回取得データを再利用
                            }
                            else {
                                dtTuikaHinban4Becchu = BecchuOrderClass.getM_TsuikaHinbanDataTable(drBecchuKey.ShumokuCode, c);
                                for (int i = 0; i < dtTuikaHinban4Becchu.Count; i++) {
                                    dtTuikaHinban4Becchu[i].Hinban = dtTuikaHinban4Becchu[i].Hinban.Trim(); // 品番の頭に空白があるので
                                }
                            }
                        }

                        
                        // 送品区分
                        try
                        {
                            denpyou.SouhinKubun = (EnumSouhinKubun)int.Parse(str[(int)EnumDataField.SouhinKubun]);
                        }
                        catch
                        {
                            throw new Exception("送品区分を正しく入力してください。");
                        }
                        // jde対応
                        //if (null != bData && denpyou.SouhinKubun == EnumSouhinKubun.KariNouhin)
                        //{
                        //    // 別注品の場合
                        //    throw new Exception(string.Format("オーダーNo：{0}の送品区分は6以外を入力してください。", strChumonNo));
                        //}

                        //// 伝票区分
                        //denpyou.DenpyouKubun = GetDenpyouKubun(denpyou.SouhinKubun);
                        //if (denpyou.DenpyouKubun != str[(int)EnumDataField.DenpyouKubun])
                        //    throw new Exception("伝票区分が正しくありません。");    // 送品区分で決定するのになぜ態々入力させる？

                        // 伝票発行日
                        try
                        {
                            denpyou.dtHakkouBi = new Core.Type.Nengappi(int.Parse(str[(int)EnumDataField.DenpyouHakkouBi])).ToDateTime();
                        }
                        catch
                        {
                            throw new Exception("伝票発行日は8桁で入力してください。");
                        }
                        if (new Core.Type.Nengappi(denpyou.dtHakkouBi).ToYYYYMMDD() < Core.Type.Nengappi.Today.ToYYYYMMDD())
                            throw new Exception("伝票発行日は本日以降の日付を入力してください。");
                        else if (new Core.Type.Nengappi(denpyou.dtHakkouBi).ToYYYYMMDD() > Core.Type.Nengappi.Parse("2099/12/31").ToYYMMDD())
                            throw new Exception("伝票発行日は2099/12/31以前の日付を入力してください。");

                        denpyou.HokanBasho = str[(int)EnumDataField.HokanBasho];
                        denpyou.NiukeJigyoushoCode = str[(int)EnumDataField.NiukeJigyoushoCode];
                        MizunoDataSet.M_NiukeBumonBashoDataTable dtNiuke = NiukeBumonClass.getM_NiukeBumonBashoDataTable(denpyou.NiukeJigyoushoCode, denpyou.HokanBasho, c);
                        if (0 == dtNiuke.Count)
                        {
                            throw new Exception(string.Format("オーダーNo：{0}の送品区分、事業所、保管場所の組み合わせが正しくありません", strChumonNo));
                        }
                        denpyou.NiukeJigyoushoMei = dtNiuke[0].NiukeBumonMei;
                        denpyou.NiukeBashoMei = dtNiuke[0].NiukeBashoMei;
                        denpyou.NouhinKigou = StrCnvToHankaku(str[(int)EnumDataField.NouhinKigou]).ToUpper();   // 大文字で
                        if (null == NouhinKigouClass.getM_NouhinKigouRow(denpyou.NouhinKigou, c))
                            throw new Exception(string.Format("オーダーNo：{0}の納品記号が正しくありません", strChumonNo));

                        if (!NiukeBumonClass.CheckBumonBasho(denpyou.SouhinKubun, denpyou.NiukeJigyoushoCode, denpyou.HokanBasho, denpyou.NouhinKigou, c))
                            throw new Exception(string.Format("オーダーNo：{0}の送品区分、事業所、保管場所の組み合わせが正しくありません", strChumonNo));

                        // jde対応
                        //if (null != pData && denpyou.SouhinKubun == EnumSouhinKubun.KariNouhin)
                        //{
                        //    // 本納品月
                        //    string strHonNouhinTsuki = str[(int)EnumDataField.HonNouhinTsuki];
                        //    if (6 != strHonNouhinTsuki.Length)
                        //        throw new Exception("本納品月を6桁で入力して下さい");
                        //    try
                        //    {
                        //        pData.TourokuData.ngHonNouhinTuki = new Core.Type.Nengetu(int.Parse(strHonNouhinTsuki));
                        //        if (pData.TourokuData.ngHonNouhinTuki.ToYYYYMM() < Core.Type.Nengetu.Today.ToYYYYMM() ||
                        //            pData.TourokuData.ngHonNouhinTuki.ToYYYYMM() > Core.Type.Nengetu.Today.AddMonth(12).ToYYYYMM())
                        //            throw new Exception(string.Format("本納品月を{0:yyyy/MM}～{1:yyyy/MM}の間で入力して下さい", DateTime.Today, DateTime.Today.AddMonths(12)));
                        //    }
                        //    catch
                        //    {
                        //        throw new Exception("本納品月を正しく入力して下さい");
                        //    }
                        //}

                        // 以下、半角チェックは済んでいる
                        denpyou.SKTantoushaCode = "";   // アップロード時はコードはブランク
                        denpyou.SKTantoushaMei = str[(int)EnumDataField.SeisanTantoushaMei];

                        denpyou.EigyouTantoushaCode = "";
                        denpyou.EigyouTantoushaMei = str[(int)EnumDataField.EigyouTantoushaMei];
                     
                        denpyou.HanbaitenKigyouRyakuMei = str[(int)EnumDataField.Okurisaki];
                        denpyou.TenMei = str[(int)EnumDataField.TenMei];
                        denpyou.TeamMei = str[(int)EnumDataField.TeamMei];
                    }
                    else
                    {
                        // 2行目以降
                                                
                        // 伝票の共通項目が前行と同じかどうかチェック
                        for (int i = 0; i < str.Length; i++)
                        {
                            switch ((EnumDataField)i)
                            {
                                case EnumDataField.HacchuBi:
                                case EnumDataField.HacchuNo:
                                //case EnumDataField.DenpyouKubun:
                                case EnumDataField.SouhinKubun:
                                case EnumDataField.HokanBasho:
                                case EnumDataField.NiukeJigyoushoCode:
                                //case EnumDataField.HonNouhinTsuki:
                                case EnumDataField.SPNo:
                                case EnumDataField.UnsouGyoushaMei:
                                case EnumDataField.OkuriJyouNo:
                                //case EnumDataField.GoukeiKakaku:
                                case EnumDataField.SeisanTantoushaMei:
                                case EnumDataField.EigyouTantoushaMei:
                                case EnumDataField.Okurisaki:
                                case EnumDataField.TenMei:
                                case EnumDataField.TeamMei:
                                    if (!str[i].Equals(strPrevData[i]))
                                        throw new Exception(string.Format("{0}が{1}行目と異なっている為、行Noを1から開始して下さい", strFieldMei[i], nLine - 1));
                                    break;
                            }
                        }

                        object objTourokuData = lstTourokuData[lstTourokuData.Count - 1];   // 現在の伝票を取得
                        pData = null; bData = null;
                        if (objTourokuData is ProperOrderTourokuData)
                        {
                            // この伝票が予算品の出荷伝票の場合
                            int nCol = (int)EnumDataField.YosanTuki;
                            if (!str[nCol].Equals(strPrevData[nCol]))   // 予算月が全行と同じかﾁｪｯｸ
                                return new Error(string.Format("予算月が{0}行目と異なっている為、行Noを1から開始して下さい", nLine - 1));
                            pData = objTourokuData as ProperOrderTourokuData;
                            denpyou = pData.TourokuData;
                        }
                        else
                        {
                            bData = objTourokuData as BecchuOrderTourokuData;
                            denpyou = bData.TourokuData;
                        }
                    }

                    strPrevData = str;  // 確認



                    // ----- 明細 -----
                    NouhinDataClass.MeisaiDataCommon meisai = null;
                    ProperOrderClass.MeisaiData meisai_p = null;
                    BecchuOrderClass.MeisaiData meisai_b = null;
                    if (null != pData)
                        meisai = meisai_p = new ProperOrderClass.MeisaiData();
                    else
                        meisai = meisai_b = new BecchuOrderClass.MeisaiData();


                    //// 納品区分
                    //string strNouhinKubun = str[(int)EnumDataField.NouhinKubun].Trim();
                    //if ("" == strNouhinKubun) meisai.NouhinKubun = EnumNouhinKubun.TujouNouhin;
                    //else {
                    //    try
                    //    {
                    //        meisai.NouhinKubun = (EnumNouhinKubun)int.Parse(strNouhinKubun);
                    //    }
                    //    catch {
                    //        throw new Exception("納品区分を正しく入力してください。");
                    //    }
                    //}
                    

                    meisai.Tekiyou = str[(int)EnumDataField.Tekiyou].Trim();
                    switch (meisai.NouhinKubun)
                    {
                        case EnumNouhinKubun.TokuisakiHimoduke:
                        case EnumNouhinKubun.OrderHimoduke:
                            // 摘要が必須のはずだけど
                            if ("" == meisai.Tekiyou)
                            {
                                throw new Exception("納品区分が1もしくは2の場合は摘要を入力してください。");
                            }
                            break;
                        case EnumNouhinKubun.SekiZukeNyuko:
                            // 籍付けの値入力は予算品のみ有効であったが、別注品でも入力可能とした（2011/10/24）
                            try
                            {
                                //meisai.Sekizuke = int.Parse(str[(int)EnumDataField.Sekiduke]);
                                if (meisai.Sekizuke <= 0 || meisai.Sekizuke > 9999) throw new Exception("");
                            }
                            catch
                            {
                                throw new Exception("籍付は整数4桁で入力して下さい");
                            }

                            /*
                            if (null != pData)
                            {
                                try
                                {
                                    meisai_p.Sekizuke = int.Parse(str[(int)EnumDataField.Sekiduke]);
                                    if (meisai_p.Sekizuke <= 0 || meisai_p.Sekizuke > 9999) throw new Exception("");
                                }
                                catch
                                {
                                    throw new Exception("籍付は整数4桁で入力して下さい");
                                }
                            }
                            */


                            break;
                    }

                    //// 価格
                    //string strKakaku = StrCnvToHankaku(str[(int)EnumDataField.ShiireKakaku].Trim());
                    //if ("" != strKakaku)
                    //{
                    //    meisai.TourokuKakaku = decimal.Parse(strKakaku);
                    //    if (0 > meisai.TourokuKakaku) throw new Exception("仕入価格を正しく入力してください。");
                    //}
                    //// 融通価格
                    //string strYuzuKakaku = StrCnvToHankaku(str[(int)EnumDataField.YuuzuuKakaku]).Trim();
                    //if ("" != strYuzuKakaku)
                    //{
                    //    try
                    //    {
                    //        meisai.YuuduuKakaku = int.Parse(strYuzuKakaku.Replace(",", ""));
                    //        if (0 > meisai.YuuduuKakaku) throw new Exception("");
                    //    }
                    //    catch
                    //    {
                    //        throw new Exception("融通価格を正しく入力してください。");
                    //    }
                    //}

                    meisai.Tekiyou = str[(int)EnumDataField.Tekiyou];
                    string strHinban = StrCnvToHankaku(str[(int)EnumDataField.Hinban].Trim());
                    string strSize = str[(int)EnumDataField.Size].Trim();

                    string strTokushuHinbanCode = th.GetHinban(strHinban);  // 特殊品番の場合は、「運賃」とかが入力されている

                    meisai.TuikaHinban = !string.IsNullOrEmpty(strTokushuHinbanCode);
                    meisai.ShukkaSu = int.Parse(str[(int)EnumDataField.Suuryou]);

                    if (meisai.TuikaHinban)
                    {
                        // 追加品番の場合
                        if (null != pData)
                        {
                            meisai_p.Key.Hinban = "ﾄｸｼｭﾋﾝ-" + strTokushuHinbanCode;
                            meisai_p.Key.Size = "";
                            pData.AddMeisai(bGyouNo, meisai_p);
                        }
                        else
                        {
                            meisai_b.Key.Hinban = "ﾄｸｼｭﾋﾝ-" + strTokushuHinbanCode;
                            meisai_b.Key.Size = "";
                            bData.AddMeisai(bGyouNo, meisai_b);
                        }
                        meisai.KanryouFlag = true;  // 自動的に完了
                        if (0 >= meisai.ShukkaSu) throw new Exception("数量は1以上で入力して下さい");   // 追加品番で数量0は有り得ない
                    }
                    else
                    {
                        if (null != pData)
                        {
                            // 予算品の明細取得
                            daProper.SelectCommand.Parameters["@z"].Value = pData.ProperOrderKey.SashizuBi;
                            daProper.SelectCommand.Parameters["@s"].Value = pData.ProperOrderKey.SeisanOrderNo;
                            daProper.SelectCommand.Parameters["@h"].Value = pData.ProperOrderKey.HikitoriOrderNo;
                            daProper.SelectCommand.Parameters["@y"].Value = pData.YosanTuki.ToYYYYMM();
                            daProper.SelectCommand.Parameters["@Hinban"].Value = strHinban;
                            daProper.SelectCommand.Parameters["@Size"].Value = strSize;
                            
                            MizunoDataSet.T_ProperOrderDataTable dtProper = new MizunoDataSet.T_ProperOrderDataTable();
                            daProper.Fill(dtProper);
                            if (0 == dtProper.Count) throw new Exception("発注データが存在しません。");
                            if (long.Parse(dtProper[0].ShiiresakiCode) != long.Parse(strShiiresakiCode))
                                throw new Exception("仕入先コードが正しくありません。");
                            meisai_p.Key.Hinban = dtProper[0].Hinban;   // DBの品番をセットする。（頭に空白があったりするので、DBの内容に合わせる為）
                            meisai_p.Key.Size = dtProper[0].Size;

                           
                            int nZansu = 0;
                            for (int k = 0; k < dtProper.Count; k++)
                            {
                                MizunoDataSet.T_ProperOrderRow drP = dtProper[k];
                                if (drP.KanryouFlg) continue;
                                int n = drP.Suuryou - drP.ShukkaSuu;
                                if (0 >= n) continue;
 
                                if (string.IsNullOrEmpty(pData.NouhinKigou))
                                    pData.NouhinKigou = drP.NouhinKigou;
                                else
                                {
                                    if (pData.NouhinKigou != drP.NouhinKigou) {
                                        // 基本的に、注文内の品番+サイズで納品記号が異なるケースは無いが、念の為
                                        throw new Exception(string.Format("納品記号が一致しません。", strChumonNo));
                                    }
                                }

                                nZansu += n;
                            }
                            if (0 == nZansu)
                                throw new Exception(string.Format("発注データが存在しません。予算品オーダーNo：{0}の注文は終了しています。", strChumonNo));

                            // 今回指定の事業所で引き当て可能なので事業所指定で登録する。
                            meisai_p.Key.NiukeJigyoushoCode = denpyou.NiukeJigyoushoCode;
                            meisai_p.Key.NiukeJigyoushoCodeHikiateAll = false;  // 同一品番サイズで1注文に複数の事業所があり、引き当て対象となる事業所が確定できないので、伝票の事業所から引き当てられる分だけ先に引き当てる。

                            if (!pData.AddMeisai(bGyouNo, meisai_p))
                                throw new Exception("行Noが重複しています。");

                            // 完了フラグのチェック
                            string strKey = string.Format("{0}_{1}_{2}_{3}_{4}",
                                pData.ProperOrderKey.SashizuBi, pData.ProperOrderKey.HikitoriOrderNo, pData.ProperOrderKey.SeisanOrderNo, meisai_p.Key.Hinban, meisai_p.Key.Size);
                            ProperOrderClass.TourokuData pd = (tblProperKanroyuData.ContainsKey(strKey))? tblProperKanroyuData[strKey] : null;

                            if (meisai.KanryouFlag)
                            {
                                // 別の伝票で既に完了フラグがセットされていればエラー
                                if (null == pd)
                                {
                                    tblProperKanroyuData.Add(strKey, pData.TourokuData);
                                }
                                else
                                {
                                    if (pd != pData.TourokuData)
                                        throw new Exception(string.Format("予算品オーダーNo：{0}、品番:{1}、サイズ:{2}の完了フラグが別の伝票で既に登録されています。",
                                            strChumonNo, meisai_p.Key.Hinban, meisai_p.Key.Size));
                                }
                            }
                            else
                            {
                                // 当該品番+サイズで完了フラグが立っていても出荷日が後であればＯＫ
                                if (null != pd && pd != pData.TourokuData)
                                {
                                    if (pd.dtHakkouBi <= pData.TourokuData.dtHakkouBi)
                                    {
                                        throw new Exception(string.Format("予算品オーダーNo：{0}、品番:{1}、サイズ:{2}の完了フラグは伝票発行日{0:yyyy/MM/dd}の伝票で既にセットされています。",
                                            strChumonNo, meisai_p.Key.Hinban, meisai_p.Key.Size, pd.dtHakkouBi));
                                    }
                                }
                            }
                        }
                        else
                        {
                            bool bFoundChumonData = false;

                            switch (BecchuKubun)
                            {
                                case BecchuOrderClass.EnumBecchuKubun.Becchu:
                                    dtBecchu.DefaultView.RowFilter = string.Format("HinbanTrim='{0}' and SizeTrim='{1}'", strHinban.Replace("'", "''"), strSize.Replace("'", "''"));
                                    bFoundChumonData = (0 < dtBecchu.DefaultView.Count);
                                    if (bFoundChumonData)
                                    {
                                        ViewDataset.VIEW_Becchu_ShukkaMeisaiRow drBecchu = dtBecchu.DefaultView[0].Row as ViewDataset.VIEW_Becchu_ShukkaMeisaiRow;
                                        meisai_b.Key.Hinban = drBecchu.Hinban;
                                        meisai_b.Key.Size = drBecchu.Size;
                                    }
                                    break;
                                case BecchuOrderClass.EnumBecchuKubun.Becchu2:
                                    {
                                        // 行No
                                        try
                                        {
                                            meisai_b.Key.RowNo = int.Parse(StrCnvToHankaku(str[(int)EnumDataField.BecchuHin_RowNo].Trim()));
                                            if (0 >= meisai_b.Key.RowNo) throw new Exception("");
                                        }   
                                        catch {
                                            throw new Exception("別注品明細Noを正しく入力してください。");
                                        }

                                        dtBecchu2.DefaultView.RowFilter = string.Format("HinbanTrim='{0}' AND SizeTrim='{1}' AND RowNo={2} AND Suuryou>ShukkaSuu",
                                            strHinban.Replace("'", "''"), strSize.Replace("'", "''"), meisai_b.Key.RowNo);
                                        bFoundChumonData = (0 < dtBecchu2.DefaultView.Count);
                                        if (bFoundChumonData)
                                        {
                                            ViewDataset.VIEW_Becchu2_ShukkaMeisaiRow drBecchu2 = dtBecchu2.DefaultView[0].Row as ViewDataset.VIEW_Becchu2_ShukkaMeisaiRow;
                                            meisai_b.Key.Hinban = drBecchu2.Hinban;
                                            meisai_b.Key.Size = drBecchu2.Size;
                                        }
                                    }
                                    break;
                                case BecchuOrderClass.EnumBecchuKubun.DS:
                                    dtDS.DefaultView.RowFilter = string.Format("HinbanTrim='{0}' AND SizeTrim='{1}'", strHinban.Replace("'", "''"), strSize.Replace("'", "''"));
                                    bFoundChumonData = (0 < dtDS.DefaultView.Count);
                                    if (bFoundChumonData)
                                    {
                                        meisai_b.Key.Hinban = (dtDS.DefaultView[0].Row as ViewDataset.VIEW_DS_ShukkaMeisaiRow).Hinban;
                                        meisai_b.Key.Size = (dtDS.DefaultView[0].Row as ViewDataset.VIEW_DS_ShukkaMeisaiRow).Size;
                                    }
                                    break;
                                case BecchuOrderClass.EnumBecchuKubun.SP:
                                    {
                                        dtSP.DefaultView.RowFilter = string.Format("HinbanTrim='{0}' AND SizeTrim='{1}'", strHinban.Replace("'", "''"), strSize.Replace("'", "''"));
                                        bFoundChumonData = (0 < dtSP.DefaultView.Count);
                                        if (bFoundChumonData)
                                        {
                                            ViewDataset.VIEW_SP_ShukkaMeisaiRow drSp = dtSP.DefaultView[0].Row as ViewDataset.VIEW_SP_ShukkaMeisaiRow;
                                            meisai_b.Key.Hinban = drSp.Hinban;
                                            meisai_b.Key.Size = drSp.Size;
                                        }
                                    }

                                    break;
                            }

                            if (!bFoundChumonData) { 
                                // 注文データ中になくても追加品番の可能性がある
                                if (null == dtTuikaHinban4Becchu.FindByHinbanShumokuCode(strHinban, drBecchuKey.ShumokuCode))
                                    throw new Exception("発注データ(追加品番)が存在しません。(品番=" + strHinban + ")");
                                else {
                                    // 追加品番である
                                    meisai.TuikaHinban = true;
                                    meisai_b.Key.Hinban = strHinban;
                                    meisai_b.Key.Size = strSize;    // 追加品番（特殊品番でない）の場合はサイズ指定が可能
                                }
                            }

                            if (!bData.AddMeisai(bGyouNo, meisai_b))
                                throw new Exception("行Noが重複しています。");
                        }



                        // 完了フラグ
                        string strKanryouFlg = str[(int)EnumDataField.KanryouFlg].Trim();
                        if ("0" != strKanryouFlg && "9" != strKanryouFlg) throw new Exception("完了フラグは0または9を入力してください");
                        meisai.KanryouFlag = (KanryouFlg.Kanryou == strKanryouFlg);

                        if (0 > meisai.ShukkaSu) throw new Exception("数量は0以上で入力して下さい");
                        if (KanryouFlg.MiKanryou == strKanryouFlg && 0 == meisai.ShukkaSu) throw new Exception("完了フラグが0の時は、数量を1以上で入力して下さい");
                        if (meisai.KanryouFlag && 0 == meisai.ShukkaSu) nSuryouZeroDataCount++;
                        // 「数量0、完了フラグ = 9」のデータと、「数量1以上」のデータが1伝票内に共存してはいけない。
                        if (0 < meisai.ShukkaSu && 0 < nSuryouZeroDataCount)
                        {
                            throw new Exception(string.Format("オーダーNo：{0}:数量=0、完了フラグ=9のデータは数量1以上のデータと伝票を分けて下さい", strChumonNo));
                        }


                        // 自由使用欄
                        meisai.Free1 = str[(int)EnumDataField.JiyuKoumoku1];
                        meisai.Free2 = str[(int)EnumDataField.JiyuKoumoku2];
                        meisai.Free3 = str[(int)EnumDataField.JiyuKoumoku3];
                        // 31桁備考
                        meisai.Bikou = "";

                    }


                }
                catch (Exception e)
                {
                    strPrevData = str;
                    lstErrorMsg.Add(string.Format("{0}行目 - {1}", nLine, e.Message));
                }

            }

            if (0 < lstErrorMsg.Count) return new Error("登録に失敗しました。");

            if (0 == lstTourokuData.Count) return new Error("登録するデータがありません。");




            // 完了フラグをオフにする。
            SqlDataAdapter daBki = new SqlDataAdapter("", c);
            daBki.SelectCommand.CommandText = "SELECT * FROM T_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            daBki.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", "");
            daBki.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", "");
            daBki.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", "");
            daBki.UpdateCommand = new SqlCommandBuilder(daBki).GetUpdateCommand();
            MizunoDataSet.T_BecchuKeyInfoDataTable dtBecchuKey = new MizunoDataSet.T_BecchuKeyInfoDataTable();



            SqlTransaction t = null;

            int nDenpyouIndex = 0;

            try
            {
                c.Open();
                t = c.BeginTransaction();

                daBki.SelectCommand.Transaction = daBki.UpdateCommand.Transaction = t;

                for (nDenpyouIndex = 0; nDenpyouIndex < lstTourokuData.Count; nDenpyouIndex++) 
                {
                    List<NouhinDataClass.DenpyouKey> lst = null;
                    List<NouhinDataClass.DenpyouKey> lstShukkaAri = null;
                    if (lstTourokuData[nDenpyouIndex] is ProperOrderTourokuData)
                    {
                        // 予算品
                        ProperOrderTourokuData p = lstTourokuData[nDenpyouIndex] as ProperOrderTourokuData;
                        p.SetMeisaiData();

                        int nMaxGyouSu = GetDenpyouGyouCount(p.TourokuData.SouhinKubun);
                        if (nMaxGyouSu < p.TourokuData.lstMeisai.Count)
                        {
                            string strChumonBangou = (!string.IsNullOrEmpty(p.ProperOrderKey.HikitoriOrderNo))? 
                                p.ProperOrderKey.HikitoriOrderNo : p.ProperOrderKey.SeisanOrderNo;
                            throw new Exception(string.Format("注文No.{0}が伝票作成可能な行数を超えています。(最大行数={1})",
                                strChumonBangou, nMaxGyouSu));
                        }

                        ProperOrderClass.DenpyouTouroku(true, false, p.ProperOrderKey, p.YosanTuki, p.NouhinKigou, p.TourokuData, t, out lst, out lstShukkaAri);
                    }
                    else {
                        // 別注品
                        BecchuOrderTourokuData b = lstTourokuData[nDenpyouIndex] as BecchuOrderTourokuData;
                        b.SetMeisaiData();

                        int nMaxGyouSu = GetDenpyouGyouCount(b.TourokuData.SouhinKubun);
                        if (nMaxGyouSu < b.TourokuData.lstMeisai.Count)
                        {
                            throw new Exception(string.Format("注文No.{0}が伝票作成可能な行数を超えています。(最大行数={1})",
                                b.BecchuOrderKey.OrderKanriNo, nMaxGyouSu));
                        }

                        BecchuOrderClass.DenpyouTouroku(true, false, b.BecchuOrderKey, b.TourokuData, t, out lst, out lstShukkaAri);

                        daBki.SelectCommand.Parameters["@MizunoUketsukeBi"].Value = b.BecchuOrderKey.UketsukeBi;
                        daBki.SelectCommand.Parameters["@OrderKanriNo"].Value = b.BecchuOrderKey.OrderKanriNo;
                        daBki.SelectCommand.Parameters["@ShiiresakiCode"].Value = b.BecchuOrderKey.ShiiresakiCode;
                        dtBecchuKey.Clear();
                        daBki.Fill(dtBecchuKey);
                        if (0 == dtBecchuKey.Count) throw new Exception(string.Format("オーダーNo.{0}のキー情報がありません。", b.BecchuOrderKey.OrderKanriNo));

                        MizunoDataSet.T_BecchuKeyInfoRow dr = dtBecchuKey[0];
                        
                        if (!string.IsNullOrEmpty(b.BecchuSpectraNo)) dr.BecchuSpectraNo = b.BecchuSpectraNo;   // これ正しいの？
                        
                        if (!string.IsNullOrEmpty(b.OkuriJyouNo)) dr.OkuriJyouNo = b.OkuriJyouNo;
                        if (!string.IsNullOrEmpty(b.UnsouGyoushaMei)) dr.UnsouGyoushaMei = b.UnsouGyoushaMei;
                        if (b.GoukeiKakaku.HasValue) dr.GoukeiKakaku = b.GoukeiKakaku.Value;
                        daBki.Update(dtBecchuKey);
                    }

                    if (0 < lst.Count) lstDenpyouKey.AddRange(lst);
                    if (null != lstShukkaAri && 0 < lstShukkaAri.Count) lstDenpyouKeyShukkaAri.AddRange(lstShukkaAri);
                }
                
                t.Commit();

                return null;
            }
            catch (Exception e)
            {
                if (null != t) t.Rollback();
                return new Core.Error(string.Format("{0}枚目の伝票 - {1}", nDenpyouIndex + 1, e.Message));
            }
            finally
            {
                c.Close();
            }

        }



        /// <summary>
        /// 伝票ヘッダーDataRow
        /// </summary>
        /// <returns></returns>
        public static void InitT_NouhinHeaderRow(MizunoDataSet.T_NouhinHeaderRow dr)
        {
            dr.RecodeKubun = "B";
            dr.DataKubun = "21";
            dr.HakkouNo = 0;
            dr.Yobi1 = "";
            dr.Edaban = "00";
            dr.GyouNo = "0";
            dr.DenpyouKubun = "";
            dr.TorihikisakiKaishaCode = "";
            dr.TorihikisakiBusho = "";
            dr.NouhinmotoShiiresakiCode = "";
            dr.NouhinmotoYobi = "";
            dr.HanbaitenKaishaCode = "";
            dr.HanbaitenBusho = "";
            dr.NouhinKigou = "";
            dr.NiukeBasho = "";
            dr.ShukkasakiYobi = "";
            dr.NiukeBumon = "";
            dr.HanbaitenKigyouRyakuMei = "";
            dr.HanbaitenBushoRyakuMei = "";
            dr.ShukkaSakiKigyouRyakuMei = "";
            dr.ShukkaSakiBushoRyakuMei = "";
            dr.HakkouHiduke = "000000";
            dr.ShukkaHiduke = "000000";
            dr.HacchuNo = "";
            dr.HacchuKubun = "";
            dr.R1 = "";
            dr.R2 = "";
            dr.R3 = "";
            dr.R4 = "";
            dr.SouhinKubun = "";
            dr.SashizuBi = "";
            dr.ShukkaBi = DateTime.Today;
            dr.SoushinFlg = SoushinFlg.NONE;
            dr.HonNouhinTsuki = "";
            dr.SKTantouMei = "";
            dr.EigyouTantouMei = "";
            dr.SetShuturyokuBiNull();
        }

        /// <summary>
        /// 伝票明細DataRow
        /// </summary>
        /// <returns></returns>
        public static void InitT_NouhinMeisaiRow(MizunoDataSet.T_NouhinMeisaiRow dr)
        {
            dr.RecodeKubun = "D";
            dr.DataKubun = "21";
            dr.HakkouNo = 0;
            dr.Yobi1 = "";
            dr.Edaban = 0;
            dr.GyouNo = 0;
            dr.ShouhinBunrui = "";
            dr.ShouhinCode = "";
            dr.Hinban = "";
            dr.HinbanTsuikaFlg = "0";
            dr.Filler = "";
            dr.TorihikiTanka = "0000000";
            dr.Joudai = "0000000";
            dr.BrandRyakuMei = "";
            dr.ShouhinRyakuMei = "";
            dr.JanCode = "0000000000000";
            dr.NouhinSuu = 0;
            dr.Size = "";
            dr.Meisai = "";
            dr.NouhinKubun = "";
            dr.YuuduuKakaku = "0000000";
            dr.Sekiduke = "0000";
            dr.KanryouFlg = "0";
            dr.ShiiresakiCode = "";
            dr.FreeKoumoku1 = "";
            dr.FreeKoumoku2 = "";
            dr.FreeKoumoku3 = "";
            dr.Bikou = "";
        }

        public static void InitT_NouhinTrailerRow(MizunoDataSet.T_NouhinTrailerRow dr)
        {
            dr.RecodeKubun = "F";
            dr.DataKubun = "21";
            dr.HakkouNo = 0;
            dr.Yobi1 = "";
            dr.Edaban = "00";
            dr.GyouNo = "9";
            dr.EigyouTantoushaCode = "";
            dr.TeamMei = "";
            dr.TenMei = "";
            dr.SKTantoushaCode = "";
            dr.UnsouHouhou = "";
            dr.Kosuu = "000";
            dr.UnchinKubun = "";
            dr.Shogakari = "000000";
            dr.UnChin = "000000";
            dr.ShinaDai = "000000000";
            dr.ShouhiZei = "0000000";
            dr.NouhinShoHiduke = "000000";
            dr.Yobi2 = "";
            dr.ShiiresakiCode = "";
        }

        public static MizunoDataSet.T_NouhinHeaderDataTable
            getT_NouhinHeaderDataTable(string strShiiresakiCode, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "select * from T_NouhinHeader where NouhinmotoShiiresakiCode=@s and YYMM>='1103'";

            da.SelectCommand.Parameters.AddWithValue("@s", strShiiresakiCode);

            MizunoDataSet.T_NouhinHeaderDataTable dt = new MizunoDataSet.T_NouhinHeaderDataTable();
            da.Fill(dt);
            return dt;
        }

//        /// <summary>
//        /// 過去指定月数分の納品伝票発行年月取得
//        /// </summary>
//        /// <param name="strShiiresakiCode"></param>
//        /// <param name="nMonths">過去当該月数分のデータが対象</param>
//        /// <param name="sqlConn"></param>
//        /// <returns></returns>
//        public static List<Core.Type.Nengetu> GetHakkouNengetu(string strShiiresakiCode, int nMonths, SqlConnection sqlConn)
//        {
//            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
//            da.SelectCommand.CommandText = @"
//SELECT                  TOP (100) PERCENT YYMM
//FROM                     dbo.T_NouhinHeader
//WHERE                   (NouhinmotoShiiresakiCode = @s) AND (CAST(YYMM AS int) >= @m)
//GROUP BY          YYMM
//ORDER BY           YYMM DESC";

//            da.SelectCommand.Parameters.AddWithValue("@s", strShiiresakiCode);
//            da.SelectCommand.Parameters.AddWithValue("@m", Core.Type.Nengetu.Today.AddMonth(-nMonths).ToYYMM());

//            DataTable dt = new DataTable();
//            da.Fill(dt);

//            List<Core.Type.Nengetu> lstTest = GetHakkouNengetuInCommon(strShiiresakiCode, nMonths, sqlConn);


//            List<Core.Type.Nengetu> lst = new List<Core.Type.Nengetu>();
//            for (int i = 0; i < dt.Rows.Count; i++)
//            {
//                lst.Add(new Core.Type.Nengetu(Convert.ToInt32(dt.Rows[i][0])));
//            }
//            return lst;
//        }

//        /// <summary>
//        /// 過去指定月数分の納品伝票発行年月取得
//        /// </summary>
//        /// <param name="strShiiresakiCode"></param>
//        /// <param name="nMonths">過去当該月数分のデータが対象</param>
//        /// <param name="sqlConn"></param>
//        /// <returns></returns>
//        public static List<Core.Type.Nengetu> GetHakkouNengetu2(int nMonths, SqlConnection sqlConn)
//        {
//            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
//            da.SelectCommand.CommandText = @"
//SELECT                  TOP (100) PERCENT YYMM
//FROM                     dbo.T_NouhinHeader
//WHERE                   (CAST(YYMM AS int) >= @m)
//GROUP BY          YYMM
//ORDER BY           YYMM DESC";

//            da.SelectCommand.Parameters.AddWithValue("@m", Core.Type.Nengetu.Today.AddMonth(-nMonths).ToYYMM());

//            DataTable dt = new DataTable();
//            da.Fill(dt);

//            List<Core.Type.Nengetu> lstTest = GetHakkouNengetuInCommon("", nMonths, sqlConn);



//            List<Core.Type.Nengetu> lst = new List<Core.Type.Nengetu>();
//            for (int i = 0; i < dt.Rows.Count; i++)
//            {
//                lst.Add(new Core.Type.Nengetu(Convert.ToInt32(dt.Rows[i][0])));
//            }
//            return lst;
//        }

        /// <summary>
        /// 2014/05/23 吉川様御要望
        /// 「発行年月」の基準を「発行年月」から「出荷日(ShukkaBi)」に変更する事に伴い修正
        /// </summary>
        /// <param name="strShiiresakiCode"></param>
        /// <param name="nMonths"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static List<Core.Type.Nengetu> GetHakkouNengetuInCommon(string strShiiresakiCode, int nMonths, SqlConnection sqlConn)
        {
            string strWhere = string.Format("(ShukkaBi >= DATEADD(MONTH, -{0}, GETDATE()))", nMonths);
            if (strShiiresakiCode != "")
            {
                strWhere += string.Format(" AND (NouhinmotoShiiresakiCode = '{0}') ", strShiiresakiCode);
            }

            string sql = string.Format(@"SELECT YYMM
                                         FROM(SELECT TOP(100)PERCENT SUBSTRING(REPLACE(CONVERT(NVARCHAR, ShukkaBi, 11), '/', ''), 1, 4)AS YYMM
                                              FROM   dbo.T_NouhinHeader
                                              WHERE  {0}
                                              ORDER BY ShukkaBi DESC) AS T_A
                                         GROUP BY YYMM
                                         ORDER BY YYMM DESC", strWhere);

            SqlDataAdapter da = new SqlDataAdapter(sql, sqlConn);

            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Core.Type.Nengetu> lst = new List<Core.Type.Nengetu>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst.Add(new Core.Type.Nengetu(Convert.ToInt32(dt.Rows[i][0])));
            }
            return lst;
        }





        [Serializable]
        public class NouhinDataKensaku
        {
            public string strShiiresakiCode;
            public Core.Type.Nengetu ngYYMM = null;             // 発行年月
            public Core.Type.NengappiKikan ShukkaBi = null;
            public Core.Sql.FilterItem objHinban = null;
            public Core.Sql.FilterItem objHakkouNo = null;      // 発行No
            public Core.Sql.FilterItem objChumonBangou = null;  // 予算品の時：引取オーダーNo ,別注品の時：オーダー管理No
            public Core.Sql.FilterItem objOkurijouNo = null;    // 2014/02/18 岡村 追加
            public string SoushinFlg = null;                    //2014/02/18 岡村修正 IN句にセットする。 SoushinFlg in (0,1)
            public DateTime? ShukkaBiFrom = null;
            public DateTime? ShukkaBiTo = null;
            public string strOkuriJou = null;                   //2014/02/18 岡村追加 「1：送品区分4で入力無し分のみ(出力されていないデータを抽出)」
            public string strSouhinKubun = null;                //2014/03/31 岡村追加  送品区分
            public DateTime? dOkurijouLessReleaseBi = null;     //2014/04/14 岡村追加　送り状機能、リリース日


            public int nYYMMMonths = 11; // 過去6ヶ月の納品年月分まで取得
            //public int nYYMMMonths = 12; // 過去6ヶ月の納品年月分まで取得 ※2013/08/09 岡村 一時的に8カ月閲覧可能とする。 ※2013/11/21  一時的に12カ月閲覧可能とする。


            /// <summary>
            /// [VIEW_NouhinHeader]と[T_NouhinHeader]を区別する
            /// </summary>
            public int nViewOrTable = 1;


            /// <summary>
            /// 2014/07/03 M0031,M0032につき追加
            /// </summary>
            public string strJigyousyo = null;
            public string strSyumoku = null;
            public string strHaccyuTantousya = null;



            public void Where4T_NouhinHeaderDataTable(Core.Sql.WhereGenerator w, SqlCommand cmd)
            {
                if (null != objHinban)
                {
                    if (nViewOrTable == 1)
                    {
                        // = 1→View
                        string str = string.Format(@"
exists (select * from T_NouhinMeisai where  
T_NouhinMeisai.HakkouNo=VIEW_NouhinHeader.HakkouNo and 
T_NouhinMeisai.ShiiresakiCode=VIEW_NouhinHeader.NouhinmotoShiiresakiCode and 
T_NouhinMeisai.YYMM=VIEW_NouhinHeader.YYMM and {0}
)", objHinban.GetFilterText("LTRIM(T_NouhinMeisai.Hinban)", "@Hinban", cmd));
                        w.Add(str);
                    }
                    else if (nViewOrTable == 2)
                    {
                        // = 2→Table
                        // 将来的にDBの品番の頭の空白は取り除くこと（LTRIM(T_NouhinMeisai.Hinban)でLtrimは不要）
                        string str = string.Format(@"
exists (select * from T_NouhinMeisai where  
T_NouhinMeisai.HakkouNo=T_NouhinHeader.HakkouNo and 
T_NouhinMeisai.ShiiresakiCode=T_NouhinHeader.NouhinmotoShiiresakiCode and 
T_NouhinMeisai.YYMM=T_NouhinHeader.YYMM and {0}
)", objHinban.GetFilterText("LTRIM(T_NouhinMeisai.Hinban)", "@Hinban", cmd));
                        w.Add(str);

                    }

//                    // 将来的にDBの品番の頭の空白は取り除くこと（LTRIM(T_NouhinMeisai.Hinban)でLtrimは不要）
//                    string str = string.Format(@"
//exists (select * from T_NouhinMeisai where  
//T_NouhinMeisai.HakkouNo=T_NouhinHeader.HakkouNo and 
//T_NouhinMeisai.ShiiresakiCode=T_NouhinHeader.NouhinmotoShiiresakiCode and 
//T_NouhinMeisai.YYMM=T_NouhinHeader.YYMM and {0}
//)", objHinban.GetFilterText("LTRIM(T_NouhinMeisai.Hinban)", "@Hinban", cmd));
//                    w.Add(str);
                }
                Where(w, cmd);
            }

            public void Where4VIEW_NouhinMeisai2DataTable(Core.Sql.WhereGenerator w, SqlCommand cmd)
            {


                if (null != objHinban)
                {
                    //2013/10/03 岡村 修正
                    //w.Add(objHinban.GetFilterText("Hinban", "@Hinban", cmd));
                    string sWhere = string.Format(@"
exists
(
select *
from T_NouhinMeisai
where 
T_NouhinMeisai.HakkouNo =VIEW_NouhinMeisai2.HakkouNo and
T_NouhinMeisai.Edaban  =VIEW_NouhinMeisai2.Edaban and	
--T_NouhinMeisai.GyouNo =VIEW_NouhinMeisai2.GyouNo and
T_NouhinMeisai.YYMM =VIEW_NouhinMeisai2.YYMM and
T_NouhinMeisai.ShiiresakiCode =VIEW_NouhinMeisai2.NouhinmotoShiiresakiCode and
{0}
)
", objHinban.GetFilterText("T_NouhinMeisai.Hinban", "@Hinban", cmd));
                    
                    w.Add(sWhere);
                }


//                if (null != objHinban)
//                {
//                    // 将来的にDBの品番の頭の空白は取り除くこと（LTRIM(T_NouhinMeisai.Hinban)でLtrimは不要）
//                    string str = string.Format(@"
//exists (select * from T_NouhinMeisai where  
//T_NouhinMeisai.HakkouNo=T_NouhinHeader.HakkouNo and 
//T_NouhinMeisai.ShiiresakiCode=T_NouhinHeader.NouhinmotoShiiresakiCode and 
//T_NouhinMeisai.YYMM=T_NouhinHeader.YYMM and {0}
//)", objHinban.GetFilterText("LTRIM(T_NouhinMeisai.Hinban)", "@Hinban", cmd));
//                    w.Add(str);
//                }


                /// 2014/07/03 M0031,M0032につき追加
                if (strJigyousyo != null)
                {
                    w.Add(string.Format("(NiukeBumon = '{0}')", strJigyousyo));
                }

                if (strSyumoku != null)
                {
                    if (strSyumoku.Trim() != "")
                    { w.Add(string.Format("(ShumokuCode = '{0}')", strSyumoku)); }
                    else
                    { w.Add("(ShumokuCode IS NULL)"); }
                }

                if (strHaccyuTantousya != null)
                {
                    w.Add(string.Format("(SKTantouMei LIKE '{0}%')", strHaccyuTantousya));
                }


                Where(w, cmd);
            }


            public void Where4VIEW_NouhinDataDownloadDataTable(Core.Sql.WhereGenerator w, SqlCommand cmd)
            {
                if (null != objHinban)
                {
                    // 将来的にDBの品番の頭の空白は取り除くこと（LTRIM(T_NouhinMeisai.Hinban)でLtrimは不要）
                    w.Add(objHinban.GetFilterText("LTRIM(Hinban)", "@Hinban", cmd));
                }
                Where(w, cmd);
            }

            private void Where(Core.Sql.WhereGenerator w, SqlCommand cmd)
            {
                if (!string.IsNullOrEmpty(strShiiresakiCode))
                {
                    w.Add("NouhinmotoShiiresakiCode=@s");
                    cmd.Parameters.AddWithValue("@s", strShiiresakiCode);
                }

                /// 2014/05/15 吉川様御依頼
                /// 「発行年月」の基準を「発行年月(YYMM)」から「出荷日(ShukkaBi)」に変更
                // 発行年月
                if (null != ngYYMM)
                {
                    DateTime dF = ngYYMM.ToDateTime();
                    DateTime dE = new DateTime(ngYYMM.Year, ngYYMM.Month, ngYYMM.DaysInMonth);
                    string strD = string.Format("('{0}' <= ShukkaBi AND ShukkaBi < '{1}')", dF, dE.AddDays(1));
                    w.Add(strD);

                    //w.Add("cast(YYMM as int)=@YYMM");
                    //cmd.Parameters.AddWithValue("@YYMM", ngYYMM.ToYYMM());
                }


                // 発行年月→過去6ヶ月の納品年月分まで取得
                if (0 < nYYMMMonths)
                {
                    w.Add("(CAST(YYMM AS int) >= @m)");
                    cmd.Parameters.AddWithValue("@m", Core.Type.Nengetu.Today.AddMonth(-nYYMMMonths).ToYYMM());
                }


                if (null != ShukkaBi)
                {
                    w.Add(ShukkaBi.GenerateSQLAsDateTime("ShukkaBi"));
                }

                if (null != objChumonBangou)
                {
                    string strYosan = objChumonBangou.GetFilterText("HikitoriOrderNo", "@HikitoriOrderNo", cmd);
                    string strBecchu = objChumonBangou.GetFilterText("OrderKanriNo", "@OrderKanriNo", cmd);

                    string strZairyou = objChumonBangou.GetFilterText("ZairyouOrderNo", "@ZairyouOrderNo", cmd);
                    string strSeisan = objChumonBangou.GetFilterText("SeisanOrderNo", "@SeisanOrderNo", cmd);

                    w.Add(string.Format("({0} or {1} or {2} or {3})", strYosan, strBecchu,strZairyou,strSeisan));
                }

                //送り状No
                if (null != objOkurijouNo)
                {
                    w.Add(objOkurijouNo.GetFilterText("InvoiceNo", "@InvoiceNo", cmd));
                }


                // 発行No
                if (null != objHakkouNo)
                    w.Add(objHakkouNo.GetFilterText("HakkouNo", "@HakkouNo", cmd));

                if (null != SoushinFlg)
                {
                    w.Add(string.Format("SoushinFlg in ({0})",SoushinFlg));
                    //cmd.Parameters.AddWithValue("@SoushinFlg", SoushinFlg);
                }

                if (null != ShukkaBiFrom)
                {
                    w.Add("ShukkaBi >= @ShukkaBiFrom");
                    cmd.Parameters.AddWithValue("@ShukkaBiFrom", ShukkaBiFrom);
                }

                if (null != ShukkaBiTo)
                {
                    w.Add("ShukkaBi <= @ShukkaBiTo");
                    cmd.Parameters.AddWithValue("@ShukkaBiTo", ShukkaBiTo);
                }

                //2014/02/17 岡村
                if (null != strOkuriJou && "0" != strOkuriJou)
                {
                    /// 2014/06/02 「出荷取消」及び「出荷打止」に対応すべく追加 
                    string strName = "";
                    switch (nViewOrTable)
                    {
                        case 2: // T_NouhinTable
                            { strName = "T_NouhinHeader"; }
                            break;
                        case 1: // VIEW_NouhinHeader
                            { strName = "VIEW_NouhinHeader"; }
                            break;
                        case 3: // VIEW_NouhinMeisai2
                            { strName = "VIEW_NouhinMeisai2"; }
                            break;
                        case 4: // VIEW_NouhinDataDownload
                            { strName = "VIEW_NouhinDataDownload"; }
                            break;
                    }


                    if (strOkuriJou == "1")
                    {
                        /// ・送品区分"3"か"4"
                        /// ・仕入先が国内
                        /// ・配送業者名＋送り状No＋個口数の入力が無い
                        /// ・≧2014/05/01
                        /// ・≦本日日付
                        /// 
                        /// 2014/05/10 吉川様　条件追加
                        /// 取消された伝票が表示されない様にする
                        /// →「SoushinFlg = 9 AND 明細行無し」が取消伝票
                        /// 2014/05/13 吉川様　条件追加
                        /// 「出荷打ち止め」(明細行数量「0」 AND SoushinFlg = 9)も表示されない様にする
                        //                        w.Add(string.Format(@"(SouhinKubun = '4' or SouhinKubun = '3') and 
                        //                                              ((SUBSTRING(NouhinmotoShiiresakiCode, 0, 3)) <> '29') and 
                        //                                              ((InvoiceNo = '' OR InvoiceNo IS NULL) or ((UnsouGyoushaCode = '' OR UnsouGyoushaCode IS NULL) and (UnsouGyoushaMei = '' OR UnsouGyoushaMei IS NULL)) or (KogutiSu = '' OR KogutiSu IS NULL)) and 
                        //                                              --SoushinFlg != 9 and 
                        //                                              --SoushinFlg != 0 and
                        //                                              ShukkaBi <= GETDATE() and
                        //                                              ShukkaBi >= '{0}'
                        //                                            ", dOkurijouLessReleaseBi));

                        w.Add(string.Format(@"(SouhinKubun = '4' or SouhinKubun = '3') and 
                                              ((SUBSTRING(NouhinmotoShiiresakiCode, 0, 3)) <> '29') and 
                                              ((InvoiceNo = '' OR InvoiceNo IS NULL) or ((UnsouGyoushaCode = '' OR UnsouGyoushaCode IS NULL) and (UnsouGyoushaMei = '' OR UnsouGyoushaMei IS NULL)) or (KogutiSu = '' OR KogutiSu IS NULL)) and 
                                              --SoushinFlg != 9 and 
                                              --SoushinFlg != 0 and
                                              ShukkaBi <= GETDATE() and
                                              ShukkaBi >= '{0}'
                                              -- 取消
                                              AND (SoushinFlg != 9 AND ISNULL((select COUNT(*) from T_NouhinMeisai AS TN 
								                                            where {1}.HakkouNo = TN.HakkouNo 
								                                              AND {1}.NouhinmotoShiiresakiCode = TN.ShiiresakiCode 
								                                              AND {1}.YYMM = TN.YYMM 
								                                            GROUP BY TN.HakkouNo), 0) > 0)
                                              -- 打ち止め
                                              AND (ISNULL((select COUNT(*) from T_NouhinMeisai AS TN 
								                           where {1}.HakkouNo = TN.HakkouNo 
								                             AND {1}.NouhinmotoShiiresakiCode = TN.ShiiresakiCode 
								                             AND {1}.YYMM = TN.YYMM
                                                             AND (TN.NouhinSuu = 0) AND (TN.KanryouFlg = 9)), 0) = 0)
                                            ", dOkurijouLessReleaseBi, strName));
                    }
                    else if (strOkuriJou == "2")
                    {
                        /// ・送品区分"3"か"4"
                        /// ・仕入先が国内
                        /// ・配送業者名＋送り状No＋個口数の入力が無い
                        /// ・≧2014/05/01
                        /// 
                        /// 2014/05/10 吉川様　条件追加
                        /// 取消された伝票が表示されない様にする
                        /// →「SoushinFlg = 9 AND 明細行無し」が取消伝票
                        /// 2014/05/13 吉川様　条件追加
                        /// 「出荷打ち止め」(明細行数量「0」 AND SoushinFlg = 9)も表示されない様にする
                        //                        w.Add(string.Format(@"(SouhinKubun = '4' or SouhinKubun = '3') and 
                        //                                              ((SUBSTRING(NouhinmotoShiiresakiCode, 0, 3)) <> '29') and 
                        //                                              ((InvoiceNo = '' OR InvoiceNo IS NULL) or ((UnsouGyoushaCode = '' OR UnsouGyoushaCode IS NULL) and (UnsouGyoushaMei = '' OR UnsouGyoushaMei IS NULL)) or (KogutiSu = '' OR KogutiSu IS NULL)) and 
                        //                                              --SoushinFlg != 9 and 
                        //                                              --SoushinFlg != 0 and
                        //                                              ----ShukkaBi <= GETDATE() and
                        //                                              ShukkaBi >= '{0}'
                        //                                            ", dOkurijouLessReleaseBi));

                        w.Add(string.Format(@"(SouhinKubun = '4' or SouhinKubun = '3') and 
                                              ((SUBSTRING(NouhinmotoShiiresakiCode, 0, 3)) <> '29') and 
                                              ((InvoiceNo = '' OR InvoiceNo IS NULL) or ((UnsouGyoushaCode = '' OR UnsouGyoushaCode IS NULL) and (UnsouGyoushaMei = '' OR UnsouGyoushaMei IS NULL)) or (KogutiSu = '' OR KogutiSu IS NULL)) and 
                                              --SoushinFlg != 9 and 
                                              --SoushinFlg != 0 and
                                              ----ShukkaBi <= GETDATE() and
                                              ShukkaBi >= '{0}'
                                              -- 取消
                                              AND (SoushinFlg != 9 AND ISNULL((select COUNT(*) from T_NouhinMeisai AS TN 
								                                            where {1}.HakkouNo = TN.HakkouNo 
								                                              AND {1}.NouhinmotoShiiresakiCode = TN.ShiiresakiCode 
								                                              AND {1}.YYMM = TN.YYMM 
								                                            GROUP BY TN.HakkouNo), 0) > 0)
                                              -- 打ち止め
                                              AND (ISNULL((select COUNT(*) from T_NouhinMeisai AS TN 
								                           where {1}.HakkouNo = TN.HakkouNo 
								                             AND {1}.NouhinmotoShiiresakiCode = TN.ShiiresakiCode 
								                             AND {1}.YYMM = TN.YYMM
                                                             AND (TN.NouhinSuu = 0) AND (TN.KanryouFlg = 9)), 0) = 0)
                                            ", dOkurijouLessReleaseBi, strName));
                    }

                }

                //2014/03/31 岡村 追加
                if (null != strSouhinKubun && "0" != strSouhinKubun)
                {
                    w.Add(string.Format("SouhinKubun = '{0}'",strSouhinKubun));
                }
            }
        }

        public static MizunoDataSet.T_NouhinHeaderDataTable getT_NouhinHeaderDataTable(NouhinDataKensaku p,
                                                                                        Core.Sql.RowNumberInfo r, 
                                                                                        SqlConnection sqlConn, 
                                                                                        ref int nTotalCount
                                                                                        )
        {
            nTotalCount = 0;
            SqlDataAdapter da = new SqlDataAdapter("select * from T_NouhinHeader", sqlConn);

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();

            p.nViewOrTable = 2;

            p.Where4T_NouhinHeaderDataTable(w, da.SelectCommand);

            if (!string.IsNullOrEmpty(w.WhereText))
                da.SelectCommand.CommandText += " where " + w.WhereText;

            MizunoDataSet.T_NouhinHeaderDataTable dt = new MizunoDataSet.T_NouhinHeaderDataTable();

            if (null != r)
                r.LoadData(da.SelectCommand, sqlConn, dt, ref nTotalCount);
            else
            {
                da.Fill(dt);
                nTotalCount = dt.Count;
            }
            return dt;
        }

        public static ViewDataset.VIEW_NouhinHeaderRow getVIEW_NouhinHeaderRow(int iHakkouNo,string sNouhinmotoShiiresakiCode,int iYYMM,
                                                                                SqlConnection sqlConn,
                                                                                ref int nTotalCount,
                                                                                SqlTransaction t
                                                                                )
        {
            nTotalCount = 0;
            SqlDataAdapter da = new SqlDataAdapter("select * from VIEW_NouhinHeader where HakkouNo=@h and NouhinmotoShiiresakiCode=@s and YYMM=@y", sqlConn);
            da.SelectCommand.Parameters.AddWithValue("@h", iHakkouNo);
            da.SelectCommand.Parameters.AddWithValue("@s", sNouhinmotoShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@y", iYYMM);
            da.SelectCommand.CommandTimeout = 600000;

            if (t != null)
            {
                da.SelectCommand.Transaction = t;
            }

            //Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
            //if (!string.IsNullOrEmpty(w.WhereText))
            //    da.SelectCommand.CommandText += " where " + w.WhereText;

            ViewDataset.VIEW_NouhinHeaderDataTable dt = new ViewDataset.VIEW_NouhinHeaderDataTable();

            //if (null != r)
            //    r.LoadData(da.SelectCommand, sqlConn, dt, ref nTotalCount);
            //else
            //{
                da.Fill(dt);
                nTotalCount = dt.Count;
            //}

            if (dt.Count == 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0] as ViewDataset.VIEW_NouhinHeaderRow;
            }

        }

        public static ViewDataset.VIEW_NouhinHeaderDataTable getVIEW_NouhinHeaderDataTable(NouhinDataKensaku p,
                                                                        Core.Sql.RowNumberInfo r,
                                                                        SqlConnection sqlConn,
                                                                        ref int nTotalCount
                                                                        )
        {
            nTotalCount = 0;
            SqlDataAdapter da = new SqlDataAdapter("select * from VIEW_NouhinHeader", sqlConn);

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();

            p.nViewOrTable = 1;

            p.Where4T_NouhinHeaderDataTable(w, da.SelectCommand);

            if (!string.IsNullOrEmpty(w.WhereText))
                da.SelectCommand.CommandText += " where " + w.WhereText;

            ViewDataset.VIEW_NouhinHeaderDataTable dt = new ViewDataset.VIEW_NouhinHeaderDataTable();

            if (null != r)
                r.LoadData(da.SelectCommand, sqlConn, dt, ref nTotalCount);
            else
            {
                da.Fill(dt);
                nTotalCount = dt.Count;
            }
            return dt;
        }


        public static ViewDataset.VIEW_NouhinMeisai2DataTable getVIEW_NouhinMeisai2DataTable(NouhinDataKensaku p,
                                                                                            Core.Sql.RowNumberInfo r,
                                                                                            SqlConnection sqlConn,
                                                                                            ref int nTotalCount
                                                                                            )
        {
            nTotalCount = 0;
            SqlDataAdapter da = new SqlDataAdapter("select * from VIEW_NouhinMeisai2", sqlConn);

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();

            /// 2014/06/02 「伝票取消」及び「伝票打止」への対応に伴い
            /// ミズノ側「伝票検索」への対応の為追加
            p.nViewOrTable = 3;


            p.Where4VIEW_NouhinMeisai2DataTable(w, da.SelectCommand);

            if (!string.IsNullOrEmpty(w.WhereText))
                da.SelectCommand.CommandText += " where " + w.WhereText;

            ViewDataset.VIEW_NouhinMeisai2DataTable dt = new ViewDataset.VIEW_NouhinMeisai2DataTable();

            if (null != r)
                r.LoadData(da.SelectCommand, sqlConn, dt, ref nTotalCount);
            else
            {
                da.Fill(dt);
                nTotalCount = dt.Count;
            }
            return dt;
        }


        public static ViewDataset.VIEW_NouhinDataDownloadDataTable
            getVIEW_NouhinDataDownloadDataTable(NouhinDataKensaku p, SqlConnection sqlConn)
        {
            // 20170217 ビューの完了フラグを書き換えたので変更 8⇒9 9⇒9 0⇒0
            SqlDataAdapter da = new SqlDataAdapter("select * from VIEW_NouhinDataDownload_ListID5 AS VIEW_NouhinDataDownload", sqlConn);

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();


            /// 2014/06/03 ダウンロードを追加
            p.nViewOrTable = 4;

            p.Where4VIEW_NouhinDataDownloadDataTable(w, da.SelectCommand);

            if (!string.IsNullOrEmpty(w.WhereText))
                da.SelectCommand.CommandText += " where " + w.WhereText;

            da.SelectCommand.CommandTimeout = 600;

            ViewDataset.VIEW_NouhinDataDownloadDataTable dt = new ViewDataset.VIEW_NouhinDataDownloadDataTable();
            da.Fill(dt);
            return dt;
        }

        public static MizunoDataSet.T_NouhinHeaderRow
            getT_NouhinHeaderRow(DenpyouKey key, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT                  dbo.T_NouhinHeader.*
FROM                     dbo.T_NouhinHeader
WHERE                   (HakkouNo = @h) AND (NouhinmotoShiiresakiCode = @s) AND (YYMM = @y)";

            da.SelectCommand.Parameters.AddWithValue("@h", key.HakkouNo);
            da.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@y", key.YYMM);
            MizunoDataSet.T_NouhinHeaderDataTable dt = new MizunoDataSet.T_NouhinHeaderDataTable();
            da.Fill(dt);
            if (1 == dt.Count)
                return dt[0];
            else
                return null;
        }

        /// <summary>
        /// 納品データ発行処理で使用 20160611
        /// </summary>
        /// <param name="strShiiresakiCode"></param>
        /// <param name="nYYMM"></param>
        /// <param name="nHakkouNo"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static MizunoDataSet.T_NouhinHeaderRow
            getT_NouhinHeaderRow(string strShiiresakiCode, int nYYMM, int nHakkouNo, SqlTransaction t)
        {
            SqlDataAdapter da = new SqlDataAdapter("", t.Connection);
            da.SelectCommand.CommandText = @"
SELECT dbo.T_NouhinHeader.*
FROM    dbo.T_NouhinHeader
WHERE  (HakkouNo = @h) AND (NouhinmotoShiiresakiCode = @s) AND (YYMM = @y)";
            da.SelectCommand.Parameters.AddWithValue("@h", nHakkouNo);
            da.SelectCommand.Parameters.AddWithValue("@s", strShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@y", nYYMM);
            da.SelectCommand.Transaction = t;
            da.SelectCommand.CommandTimeout = 600000;
            MizunoDataSet.T_NouhinHeaderDataTable dt = new MizunoDataSet.T_NouhinHeaderDataTable();
            da.Fill(dt);
            if (1 == dt.Count)
                return dt[0];
            else
                return null;
        }

        public static MizunoDataSet.T_PdfDataTable get_PDFInfo(string FileName, SqlConnection sql)
        {
            MizunoDataSet.T_PdfDataTable dt = new MizunoDataSet.T_PdfDataTable();
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText = "select * from T_Pdf where FileName=@f ";
            da.SelectCommand.Parameters.AddWithValue("@f", FileName);
            da.Fill(dt);
            return dt;
        }

        public static MizunoDataSet.T_NouhinTrailerRow
            getT_NouhinTrailerRow(DenpyouKey key, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT           * 
FROM                     dbo.T_NouhinTrailer
WHERE                   (HakkouNo = @h) AND (ShiiresakiCode = @s) AND (YYMM = @y)";

            da.SelectCommand.Parameters.AddWithValue("@h", key.HakkouNo);
            da.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@y", key.YYMM);
            MizunoDataSet.T_NouhinTrailerDataTable dt = new MizunoDataSet.T_NouhinTrailerDataTable();
            da.Fill(dt);
            if (1 == dt.Count)
                return dt[0];
            else
                return null;
        }

        public static MizunoDataSet.T_NouhinMeisaiDataTable
            getT_NouhinMeisaiDataTable(DenpyouKey key, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT                  dbo.T_NouhinMeisai.*
FROM                     dbo.T_NouhinMeisai
WHERE                   (ShiiresakiCode = @s) AND (YYMM = @y) AND (HakkouNo = @h)
ORDER BY Hinban,GyouNo ASC";

            da.SelectCommand.Parameters.AddWithValue("@h", key.HakkouNo);
            da.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@y", key.YYMM);
            MizunoDataSet.T_NouhinMeisaiDataTable dt = new MizunoDataSet.T_NouhinMeisaiDataTable();
            da.Fill(dt);
            return dt;
        }


        public static string GetSouhinKubunMei(EnumSouhinKubun k)
        {
            switch (k)
            {
                case EnumSouhinKubun.DC:
                    return "DC納品";
                case EnumSouhinKubun.NSS_Eigyousho:
                    return "NSS/各営/日通納品";
                case EnumSouhinKubun.Nituu_GMS:
                    return "営業倉庫納品";
                case EnumSouhinKubun.Chokusou:
                    return "直送";
                case EnumSouhinKubun.SeisanBumon:
                    return "調達部門納品";
            }

            /*
            switch (k)
            {
                case EnumSouhinKubun.DC:
                    return "DC納品";
                case EnumSouhinKubun.NSS:
                    return "NSS納品";
                case EnumSouhinKubun.EigyoushoNouhin:
                    return "営業所納品";
                case EnumSouhinKubun.Chokusou:
                    return "直送";
                case EnumSouhinKubun.SKNouhin:
                    return "発注部門納品";
                case EnumSouhinKubun.KariNouhin:
                    return "仮納品";
            }
             */
            return "";
        }

        public static void ShukkaKakutei(int HakkouNo, string NouhinmotoShiiresakiCode, int YYMM, SqlConnection sqlCon)
        {
            string sql = "SELECT * FROM T_NouhinHeader WHERE HakkouNo = @HakkouNo AND NouhinmotoShiiresakiCode = @NouhinmotoShiiresakiCode AND YYMM = @YYMM";
            SqlDataAdapter da = new SqlDataAdapter(sql, sqlCon);
            da.SelectCommand.Parameters.AddWithValue("@HakkouNo", HakkouNo);
            da.SelectCommand.Parameters.AddWithValue("@NouhinmotoShiiresakiCode", NouhinmotoShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@YYMM", YYMM);
            da.UpdateCommand = new SqlCommandBuilder(da).GetUpdateCommand();
            MizunoDataSet.T_NouhinHeaderDataTable dt = new MizunoDataSet.T_NouhinHeaderDataTable();
            da.Fill(dt);

            //20170131 追加　納品データ出力済みは登録させない
            if (!dt[0].IsShuturyokuBiNull())
            {
                throw new Exception("納品データは出力済です");
            }

            /// 2014/06/19 過去日付の場合、「出荷日」が変更されない様に修正
            /// 14005_変更管理表
            if (dt[0].ShukkaBi >= DateTime.Today)
            {
                dt[0].ShukkaBi = DateTime.Today;
                dt[0].ShukkaHiduke = DateTime.Today.ToString("yyMMdd");
            }
            

            /// 2014/04/29 未確定伝票ならば[出荷確定]としない
            /// ・送品区分[3]or[4]
            /// ・「運送業者」「送り状No」「個口数」のいずれか一つでも入力されていない
            if ((dt[0].SouhinKubun == "3" || dt[0].SouhinKubun == "4")
                && ((dt[0].IsInvoiceNoNull() || dt[0].InvoiceNo.Trim() == "")
                    ||
                    ((dt[0].IsUnsouGyoushaCodeNull() || dt[0].UnsouGyoushaCode.Trim() == "") && (dt[0].IsUnsouGyoushaMeiNull() || dt[0].UnsouGyoushaMei.Trim() == ""))
                    ||
                    (dt[0].IsKogutiSuNull() || dt[0].KogutiSu.Trim() == ""))
                )
            {
            }
            else
            { dt[0].SoushinFlg = SoushinFlg.KAKUTEI; }

            // 「出荷確定処理が行われているか否か」の切替が無かった為追加
            dt[0].ShukkaKakuteiShori = true;

            da.Update(dt);
        }

        public static List<DenpyouKey> ShukkaKakutei(MizunoDataSet.T_NouhinHeaderDataTable dt, SqlTransaction sqlTran)
        {
            List<DenpyouKey> lstSakihidukeDenpyo = new List<DenpyouKey>();

            string sql = "SELECT * FROM T_NouhinHeader WHERE HakkouNo = @HakkouNo AND NouhinmotoShiiresakiCode = @NouhinmotoShiiresakiCode AND YYMM = @YYMM";
            SqlDataAdapter da = new SqlDataAdapter(sql, sqlTran.Connection);
            da.SelectCommand.Transaction = sqlTran;
            da.SelectCommand.Parameters.Add("@HakkouNo", SqlDbType.Int);
            da.SelectCommand.Parameters.Add("@NouhinmotoShiiresakiCode", SqlDbType.NChar);
            da.SelectCommand.Parameters.Add("@YYMM", SqlDbType.Int);
            da.UpdateCommand = new SqlCommandBuilder(da).GetUpdateCommand();
            da.UpdateCommand.Transaction = sqlTran;

            MizunoDataSet.T_NouhinHeaderDataTable dtThis = new MizunoDataSet.T_NouhinHeaderDataTable();

            for (int i = 0; i < dt.Count; i++)
            {
                da.SelectCommand.Parameters["@HakkouNo"].Value = dt[i].HakkouNo;
                da.SelectCommand.Parameters["@NouhinmotoShiiresakiCode"].Value = dt[i].NouhinmotoShiiresakiCode;
                da.SelectCommand.Parameters["@YYMM"].Value = dt[i].YYMM;

                dtThis.Clear();
                da.Fill(dtThis);

                if (dtThis.Count == 0) { continue; }

                //20170131 追加 納品データ出力済みか確認
                if (!dtThis[0].IsShuturyokuBiNull())
                {
                    throw new Exception("納品データは出力済です");
                }

                // 2014/02/18 岡村
                //未確定伝票がどうか判定
                bool bMikakutei = false;
                if ((dtThis[0].SouhinKubun == "4" || dtThis[0].SouhinKubun == "3") &&
                        (
                            (dtThis[0].IsInvoiceNoNull() == true || dtThis[0].InvoiceNo == "") ||
                            (
                                (dtThis[0].IsUnsouGyoushaCodeNull() == true || dtThis[0].UnsouGyoushaCode == "") &&
                                (dtThis[0].IsUnsouGyoushaMeiNull() == true || dtThis[0].UnsouGyoushaMei == "")
                            ) ||
                            (dtThis[0].IsKogutiSuNull() == true || dtThis[0].KogutiSu == "")
                        )
                    )
                {
                    bMikakutei = true;
                }


                // 先日付の場合
                if (dtThis[0].ShukkaBi.Date >= DateTime.Today && bMikakutei == false)
                {
                    //lstSakihidukeDenpyo.Add(new DenpyouKey(dtThis[0].NouhinmotoShiiresakiCode, dtThis[0].YYMM, dtThis[0].HakkouNo));
                    dtThis[0].ShukkaBi = DateTime.Today;
                    dtThis[0].ShukkaHiduke = DateTime.Today.ToString("yyMMdd");
                }


                //2013.06.28　岡村
                //出荷日が過去の場合は伝票を発行しない仕様になっていたので修正
                lstSakihidukeDenpyo.Add(new DenpyouKey(dtThis[0].NouhinmotoShiiresakiCode, dtThis[0].YYMM, dtThis[0].HakkouNo));

                if (bMikakutei == false)
                {
                    dtThis[0].SoushinFlg = SoushinFlg.KAKUTEI;
                }

                //2014/02/18 岡村追加
                dtThis[0].ShukkaKakuteiShori = true;

                //else
                //{ 
                //    //未確定伝票(=送り状Noなどが未設定の状態)の場合
                //    dtThis[0].SoushinFlg = SoushinFlg.MIKAKUTEI_SHUKKASHORIZUMI;
                //}

                da.Update(dtThis);
            }

            return lstSakihidukeDenpyo;
        }

        public static QueryDataset.Q_ShiiresakiCodeMeiDataTable getQ_ShiiresakiCodeMeiDataTable(Core.Type.Nengetu ngYYMM, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
//            da.SelectCommand.CommandText = @"
//SELECT DISTINCT
//    T_A.ShiiresakiCode, 
//    ISNULL(dbo.M_Shiiresaki.RyakuMei, dbo.M_Shiiresaki.ShiiresakiMei) AS ShiiresakiMei
//FROM                     
//    (
//        SELECT
//            ShiiresakiCode, 
//            MAX(ShiiresakiMei) AS ShiiresakiMei
//        FROM
//            dbo.T_BecchuKeyInfo {0} 
//        GROUP BY          
//            ShiiresakiCode
//    ) AS T_A 
//LEFT OUTER JOIN
//dbo.M_Shiiresaki ON T_A.ShiiresakiCode = dbo.M_Shiiresaki.ShiiresakiCode";

            da.SelectCommand.CommandText = @"
SELECT DISTINCT
    NouhinmotoShiiresakiCode AS ShiiresakiCode, 
    ShiiresakiMei
FROM                     
    VIEW_NouhinHeader
{0}
";


            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();

            if (ngYYMM != null)
            {
                w.Add(string.Format("YYMM = {0}", ngYYMM.ToYYMM()));
            }

            //// 状況(0=未出荷,1=出荷済,2=キャンセル,3=全て,4=未受付分)
            //switch (s)
            //{
            //    case EnumChumonStatus.Cancel:
            //        // キャンセル
            //        w.Add("exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo)");
            //        break;
            //    case EnumChumonStatus.MiShukka:
            //        w.Add("(KanryouFlg =0 AND not exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo))");
            //        break;
            //    case EnumChumonStatus.ShukkaZumi:
            //        w.Add("(KanryouFlg =1 AND not exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo))");
            //        break;
            //    case EnumChumonStatus.MiUketuke:
            //        w.Add("(KakuninBi is null AND not exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo))");
            //        break;
            //}

            //// ミズノ受付日表示対象期間
            //if (0 < nHyoujiTukisu)
            //{
            //    w.Add("(case isdate(MizunoUketsukeBi) when 1 then cast(MizunoUketsukeBi as datetime) else null end ) >= CAST(@uuu as datetime)");
            //    da.SelectCommand.Parameters.AddWithValue("@uuu", Core.Type.Nengetu.Today.AddMonth(-nHyoujiTukisu).ToDateTime(1));
            //}


            //if (!string.IsNullOrEmpty(strShumokuCode))
            //{
            //    w.Add("ShumokuCode = @ShumokuCode"); ;
            //    da.SelectCommand.Parameters.AddWithValue("@ShumokuCode", strShumokuCode);
            //}

            string strWhere = (string.IsNullOrEmpty(w.WhereText)) ? "" : " where " + w.WhereText;

            da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, strWhere);

            QueryDataset.Q_ShiiresakiCodeMeiDataTable dt = new QueryDataset.Q_ShiiresakiCodeMeiDataTable();
            da.Fill(dt);
            return dt;
        }

        //T_NouhinHeader.SoushinFlg を 9 にUPDATEするのみの関数
        public static LibError UpdT_NouhinHeader(int iHakkouNo, string sNouhinmotoShiiresakiCode, int iYYMM, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT * 
FROM 
    T_NouhinHeader 
WHERE 
    HakkouNo = @h AND
    NouhinmotoShiiresakiCode = @n AND
    YYMM = @y
";
            da.SelectCommand.Parameters.AddWithValue("@h", iHakkouNo);
            da.SelectCommand.Parameters.AddWithValue("@n", sNouhinmotoShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@y", iYYMM);
            da.UpdateCommand = (new SqlCommandBuilder(da)).GetUpdateCommand();
            MizunoDataSet.T_NouhinHeaderDataTable dt = new MizunoDataSet.T_NouhinHeaderDataTable();
            da.Fill(dt);

            if (1 != dt.Rows.Count)
            {
                return new LibError("エラー");
            }

            try
            {
                MizunoDataSet.T_NouhinHeaderRow drThis = (MizunoDataSet.T_NouhinHeaderRow)dt.Rows[0];

                drThis.SoushinFlg = (int)SoushinFlg.SOUSHINZUMI;

                da.Update(dt);
                return null;
            }
            catch (Exception e)
            {
                return new LibError(e);
            }
        }


        //T_NouhinHeader「InvoiceNo」「UnsouGyoushaCode」「UnsouGyoushaMei」「KogutiSu」 をUPDATEするのみの関数
        public static LibError UpdT_NouhinHeader(int iHakkouNo, 
            string sNouhinmotoShiiresakiCode, 
            int iYYMM, 
            string sInvoiceNo,
            string sUnsouGyoushaCode,
            string sUnsouGyoushaMei,
            string sKogutiSu,
            SqlConnection sqlConn
            )
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT * 
FROM 
    T_NouhinHeader 
WHERE 
    HakkouNo = @h AND
    NouhinmotoShiiresakiCode = @n AND
    YYMM = @y
";
            da.SelectCommand.Parameters.AddWithValue("@h", iHakkouNo);
            da.SelectCommand.Parameters.AddWithValue("@n", sNouhinmotoShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@y", iYYMM);
            da.UpdateCommand = (new SqlCommandBuilder(da)).GetUpdateCommand();
            MizunoDataSet.T_NouhinHeaderDataTable dt = new MizunoDataSet.T_NouhinHeaderDataTable();
            da.Fill(dt);

            if (1 != dt.Rows.Count)
            {
                return new LibError("エラー");
            }

            try
            {
                MizunoDataSet.T_NouhinHeaderRow drThis = (MizunoDataSet.T_NouhinHeaderRow)dt.Rows[0];

                //** 送り状
                drThis.InvoiceNo = sInvoiceNo;


                //** 運送業者コード
                //** 運送業者名
                if (sUnsouGyoushaCode != "")
                {
                    //コードがある＝マスタから選択
                    drThis.UnsouGyoushaCode = sUnsouGyoushaCode;
                    drThis.UnsouGyoushaMei = "";
                }
                else
                {
                    //コードがない＝フリーでテキスト入力
                    drThis.UnsouGyoushaMei = sUnsouGyoushaMei;
                    drThis.UnsouGyoushaCode = "";
                }

                //** 個口数
                drThis.KogutiSu = sKogutiSu;

                if (drThis.SoushinFlg != SoushinFlg.SOUSHINZUMI)
                {
                    //「送り状No」「運送業者名」「個口数」すべて入力されていれば送信フラグをNONEに更新してCSVに出力する
                    if ((sUnsouGyoushaCode != "" || sUnsouGyoushaMei != "") && sInvoiceNo != "" && sUnsouGyoushaMei != "" && sKogutiSu != "")
                    {
                        drThis.SoushinFlg = SoushinFlg.NONE;
                    }
                    else if ((drThis.SouhinKubun == "3" || drThis.SouhinKubun == "4")
                            && (sUnsouGyoushaCode == "" || sUnsouGyoushaMei == "" || sInvoiceNo == "" || sKogutiSu == ""))
                    {
                        drThis.SoushinFlg = SoushinFlg.MIKAKUTEI;
                    }
                }
                

                da.Update(dt);
                return null;
            }
            catch (Exception e)
            {
                return new LibError(e);
            }
        }


        /// <summary>
        /// 2014/05/17 「出荷打ち止め」の判定　false→打止されていない、true→打止済み
        /// </summary>
        /// <param name="HakkouNo"></param>
        /// <param name="ShiiresakiCode"></param>
        /// <param name="YYMM"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static bool ClosedNouhin(int HakkouNo, string ShiiresakiCode, int YYMM, SqlConnection sqlConn, SqlTransaction t)
        {
            //完了フラグ8を追加 20160920
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"SELECT COUNT(*) AS Cnt
                                             FROM   T_NouhinMeisai AS TN with(nolock)
                                             WHERE  (TN.NouhinSuu = 0) AND ((TN.KanryouFlg = 9) OR (TN.KanryouFlg = 8))
                                               AND  (TN.HakkouNo = @HN) AND (TN.ShiiresakiCode = @CD) AND (TN.YYMM = @YM)";
            da.SelectCommand.Parameters.AddWithValue("@HN", HakkouNo);
            da.SelectCommand.Parameters.AddWithValue("@CD", ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@YM", YYMM);

            if (t != null)
            {
                da.SelectCommand.Transaction = t;
            }

            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            if (!dt.Rows[0].IsNull("Cnt") && int.Parse(dt.Rows[0]["Cnt"].ToString()) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        //伝票データ出力 20180926 追加
        public static Core.Error
            CREATE_VIEW_NouhinDataDownload_ShutsuryokuDataTable(SqlConnection sqlConn, string strFilePath, out string strData)
        {
            strData = null;
            DateTime dFrom = new DateTime();
            DateTime dTo = new DateTime();
            dFrom = DateTime.Parse(DateTime.Now.AddDays(-1).ToShortDateString() + " 21:00:00");
            dTo = DateTime.Parse(DateTime.Now.AddDays(1).ToShortDateString() + " 00:00:00");
            // 20170217 ビューの完了フラグを書き換えたので変更 8⇒9 9⇒9 0⇒0
            SqlDataAdapter da = new SqlDataAdapter(string.Format("SELECT * FROM VIEW_NouhinDataDownload_Shutsuryoku AS VIEW_NouhinDataDownload WHERE ShuturyokuBi BETWEEN '{0}' AND '{1}' ORDER BY YYMM, HakkouNo, Edaban, GyouNo", dFrom, dTo), sqlConn);

            da.SelectCommand.CommandTimeout = 600000;

            DataTable dt = new DataTable();
            da.Fill(dt);

            //出力データ登録処理を行う
            SqlCommand cmd = new SqlCommand("", sqlConn);
            cmd.CommandText = string.Format(@"INSERT INTO T_DenpyouShuturyoku
                                          SELECT DISTINCT HakkouNo ,NouhinmotoShiiresakiCode ,YYMM, GETDATE()
                                          FROM VIEW_NouhinDataDownload_Shutsuryoku AS VIEW_NouhinDataDownload WHERE ShuturyokuBi BETWEEN '{0}' AND '{1}' ", dFrom, dTo);
            cmd.CommandTimeout = 600000;

            SqlTransaction t = null;
            try
            {
                sqlConn.Open();
                t = sqlConn.BeginTransaction();
                cmd.Transaction = t;
                int nRet = cmd.ExecuteNonQuery();
                t.Commit();
            }
            catch (Exception e)
            {
                if (null != t) t.Rollback();
                return new Core.Error(e);
            }
            finally
            {
                sqlConn.Close();
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //出力日,枝番,YYMMは不要
                for (int c = 0; c < dt.Columns.Count - 3; c++)
                {
                    if (c != 0) sb.Append(",");
                    sb.Append(dt.Rows[i][c].ToString());
                }
                sb.Append("\r\n");
            }

            System.IO.StreamWriter sw = new StreamWriter(strFilePath, true, System.Text.Encoding.GetEncoding(932));
            strData = sb.ToString();

            sw.Write(strData);

            if (sw != null)
            {
                sw.Close();
            }

            return null;
        }

        //伝票データ出力 20190306 追加 上記のは以前のやつ M0439で対応しようとしたけどM0314でいいとのことでコメントアウト
//        public static Core.Error
//            CREATE_VIEW_NouhinDataDownload_ShutsuryokuDataTable(SqlConnection sqlConn, string strFilePath, out string strData)
//        {
//            strData = null;
//            DateTime dFrom = new DateTime();
//            dFrom = DateTime.Parse(DateTime.Now.ToShortDateString() + " 00:00:00");

//            //予算品を取得
//            SqlDataAdapter daProper = new SqlDataAdapter("", sqlConn);
//            daProper.SelectCommand.CommandText = @"
//            SELECT
//	            HikitoriOrderNo AS OrderKanriNo,
//	            NouhinKigou,
//	            SashizuBi AS MizunoUketsukeBi,
//	            Tantousha AS MizunoTantousha,
//	            Shouninsha AS SKTantousha,
//	            CAST(RowNo AS NVARCHAR) AS RowNo,
//	            Hinban,
//	            CAST(Size AS NVARCHAR) AS Size,
//	            CAST(Suuryou AS NVARCHAR) AS Suuryou,
//	            '' AS LotNo,
//	            NiukeJigyoushoCode,
//	            HokanBasho,
//	            ShiiresakiCode,
//	            '' AS ShiiresakiMei,
//	            '' AS TokuisakiCode,
//	            '' AS TokuisakiMei,
//	            '' AS Tuika1_Hinban,
//	            '' AS Tuika1_Size,
//	            '' AS Tuika1_Suryou,
//	            '' AS Tuika1_LotNo,
//	            '' AS Tuika2_Hinban,
//	            '' AS Tuika2_Size,
//	            '' AS Tuika2_Suryou,
//	            '' AS Tuika2_LotNo,
//	            '' AS Tuika3_Hinban,
//	            '' AS Tuika3_Size,
//	            '' AS Tuika3_Suryou,
//	            '' AS Tuika3_LotNo
//            FROM T_ProperOrder 
//            WHERE  CAST(SashizuBi AS DATETIME) = CAST(@uuu AS DATETIME)
//            ORDER BY ShiiresakiCode, HikitoriOrderNo, Hinban, RowNo";
//            daProper.SelectCommand.Parameters.AddWithValue("@uuu", dFrom);
//            daProper.SelectCommand.CommandTimeout = 600000;

//            //BSを取得
//            SqlDataAdapter daBS = new SqlDataAdapter("", sqlConn);
//            daBS.SelectCommand.CommandText = @"
//            SELECT
//	            dbo.VIEW_BecchuKeyInfo.OrderKanriNo,
//	            dbo.VIEW_BecchuKeyInfo.NouhinKigou,
//	            dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi,
//	            dbo.VIEW_BecchuKeyInfo.MizunoTantousha,
//	            dbo.VIEW_BecchuKeyInfo.SKTantousha,
//	            CAST(dbo.VIEW_Becchu_ShukkaMeisai.RowNo AS NVARCHAR) AS RowNo,
//	            dbo.VIEW_Becchu_ShukkaMeisai.Hinban,
//                CAST(dbo.VIEW_Becchu_ShukkaMeisai.Size AS NVARCHAR) AS Size,
//                CAST(dbo.VIEW_Becchu_ShukkaMeisai.Suryou AS NVARCHAR) AS Suuryou,
//                CAST(dbo.VIEW_Becchu_ShukkaMeisai.LotNo AS NVARCHAR) AS LotNo,
//	            dbo.VIEW_BecchuKeyInfo.NiukeJigyoushoCode,
//                dbo.VIEW_BecchuKeyInfo.HokanBasho,
//                dbo.VIEW_BecchuKeyInfo.ShiiresakiCode,
//                dbo.VIEW_BecchuKeyInfo.ShiiresakiMei,
//                dbo.VIEW_BecchuKeyInfo.TokuisakiCode,
//                dbo.VIEW_BecchuKeyInfo.TokuisakiMei,
//                dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1Hinban AS Tuika1_Hinban,
//                CAST(dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1Size AS NVARCHAR) AS Tuika1_Size,
//                CAST(dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1Suuryou AS NVARCHAR) AS Tuika1_Suryou,
//                dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1LotNo AS Tuika1_LotNo,
//                dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2Hinban AS Tuika2_Hinban,
//                CAST(dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2Size AS NVARCHAR) AS Tuika2_Size,
//                CAST(dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2Suuryou AS NVARCHAR) AS Tuika2_Suryou,
//                dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2LotNo AS Tuika2_LotNo,
//                dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3Hinban AS Tuika3_Hinban,
//                CAST(dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3Size AS NVARCHAR) AS Tuika3_Size,
//                CAST(dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3Suuryou AS NVARCHAR) AS Tuika3_Suryou,
//                dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3LotNo AS Tuika3_LotNo
//            FROM
//                dbo.VIEW_BecchuKeyInfo
//                INNER JOIN
//                    dbo.VIEW_Becchu_ShukkaMeisai
//                ON  dbo.VIEW_BecchuKeyInfo.ShiiresakiCode = dbo.VIEW_Becchu_ShukkaMeisai.ShiiresakiCode
//                AND dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi = dbo.VIEW_Becchu_ShukkaMeisai.SashizuBi
//                AND dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.VIEW_Becchu_ShukkaMeisai.SashizuNo
//                LEFT OUTER JOIN
//                    dbo.T_Mark
//                ON  dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.T_Mark.SashizuNo
//                AND dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi = dbo.T_Mark.SashizuBi
//            WHERE  (case isdate(VIEW_BecchuKeyInfo.MizunoUketsukeBi) when 1 then cast(VIEW_BecchuKeyInfo.MizunoUketsukeBi as datetime) else null end ) = CAST(@uuu as datetime)
//	            AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KA' 
//	            AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KB' 
//	            AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'M2' 
//	            AND (dbo.VIEW_BecchuKeyInfo.ShumokuCode NOT IN ('QS', 'QR', 'QT', 'QB', 'QV', 'QA', 'SI', 'EZ', 'SE') 
//	            OR (dbo.VIEW_BecchuKeyInfo.ShumokuCode IN ('QS', 'QR', 'QT', 'QB', 'QV', 'QA', 'SI', 'EZ', 'SE') AND dbo.T_Mark.SashizuNo IS NOT NULL)) 
//            ORDER BY dbo.VIEW_BecchuKeyInfo.ShiiresakiCode, VIEW_BecchuKeyInfo.OrderKanriNo, dbo.view_becchu_shukkameisai.HinbanNo, 
//	            dbo.VIEW_Becchu_ShukkaMeisai.Hinban, dbo.VIEW_Becchu_ShukkaMeisai.LotNo, dbo.VIEW_Becchu_ShukkaMeisai.RowNo";
//            daBS.SelectCommand.Parameters.AddWithValue("@uuu", dFrom);
//            daBS.SelectCommand.CommandTimeout = 600000;

//            //BS2を取得
//            SqlDataAdapter daBS2 = new SqlDataAdapter("", sqlConn);
//            daBS2.SelectCommand.CommandText = @"
//            SELECT
//	            dbo.VIEW_BecchuKeyInfo.OrderKanriNo,
//	            dbo.VIEW_BecchuKeyInfo.NouhinKigou,
//	            dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi,
//	            dbo.VIEW_BecchuKeyInfo.MizunoTantousha,
//	            dbo.VIEW_BecchuKeyInfo.SKTantousha,
//	            CAST(dbo.T_Becchu2Repeat.RowNo AS NVARCHAR) AS RowNo,
//                dbo.T_Becchu2Repeat.Hinban,
//                CAST(dbo.T_Becchu2Repeat.Size AS NVARCHAR) AS Size,
//                CAST(dbo.T_Becchu2Repeat.Suuryou AS NVARCHAR) AS Suuryou,
//                CAST(dbo.T_Becchu2Repeat.LotNo AS NVARCHAR) AS LotNo,
//                dbo.VIEW_BecchuKeyInfo.NiukeJigyoushoCode,
//                dbo.VIEW_BecchuKeyInfo.HokanBasho,
//                dbo.VIEW_BecchuKeyInfo.ShiiresakiCode,
//                dbo.VIEW_BecchuKeyInfo.ShiiresakiMei,
//                dbo.VIEW_BecchuKeyInfo.TokuisakiCode,
//                dbo.VIEW_BecchuKeyInfo.TokuisakiMei,
//                dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1Hinban AS Tuika1_Hinban,
//                CAST(dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1Size AS NVARCHAR) AS Tuika1_Size,
//                CAST(dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1Suuryou AS NVARCHAR) AS Tuika1_Suryou,
//                dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1LotNo AS Tuika1_LotNo,
//                dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2Hinban AS Tuika2_Hinban,
//                CAST(dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2Size AS NVARCHAR) AS Tuika2_Size,
//                CAST(dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2Suuryou AS NVARCHAR) AS Tuika2_Suryou,
//                dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2LotNo AS Tuika2_LotNo,
//                dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3Hinban AS Tuika3_Hinban,
//                CAST(dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3Size AS NVARCHAR) AS Tuika3_Size,
//                CAST(dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3Suuryou AS NVARCHAR) AS Tuika3_Suryou,
//                dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3LotNo AS Tuika3_LotNo
//            FROM
//                dbo.VIEW_BecchuKeyInfo
//                INNER JOIN
//                    dbo.T_Becchu2Repeat
//                ON  dbo.VIEW_BecchuKeyInfo.ShiiresakiCode = dbo.T_Becchu2Repeat.ShiiresakiCode
//                AND dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi = dbo.T_Becchu2Repeat.SashizuBi
//                AND dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.T_Becchu2Repeat.SashizuNo
//                LEFT OUTER JOIN
//                    dbo.T_BecchuShukkaInfo
//                ON  dbo.T_Becchu2Repeat.RowNo = dbo.T_BecchuShukkaInfo.RowNo
//                AND dbo.T_Becchu2Repeat.Size = dbo.T_BecchuShukkaInfo.Size
//                AND dbo.T_Becchu2Repeat.Hinban = dbo.T_BecchuShukkaInfo.Hinban
//                AND dbo.T_Becchu2Repeat.LotNo = dbo.T_BecchuShukkaInfo.LotNo
//                AND dbo.T_Becchu2Repeat.SashizuBi = dbo.T_BecchuShukkaInfo.SashizuBi
//                AND dbo.T_Becchu2Repeat.SashizuNo = dbo.T_BecchuShukkaInfo.SashizuNo
//                AND dbo.T_Becchu2Repeat.ShiiresakiCode = dbo.T_BecchuShukkaInfo.ShiiresakiCode
//                left join
//                    T_Becchu2
//                ON  dbo.VIEW_BecchuKeyInfo.ShiiresakiCode = dbo.T_Becchu2.ShiiresakiCode
//                AND dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi = dbo.T_Becchu2.SashizuBi
//                AND dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.T_Becchu2.SashizuNo
//                LEFT OUTER JOIN
//                    dbo.T_Mark
//                ON  dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.T_Mark.SashizuNo
//                AND dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi = dbo.T_Mark.SashizuBi
//            WHERE  (case isdate(VIEW_BecchuKeyInfo.MizunoUketsukeBi) when 1 then cast(VIEW_BecchuKeyInfo.MizunoUketsukeBi as datetime) else null end ) = CAST(@uuu as datetime)
//	            AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KA' 
//	            AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KB' 
//	            AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'M2' 
//	            AND (dbo.VIEW_BecchuKeyInfo.ShumokuCode NOT IN ('QS', 'QR', 'QT', 'QB', 'QV', 'QA', 'SI', 'EZ', 'SE') 
//	            OR (dbo.VIEW_BecchuKeyInfo.ShumokuCode IN ('QS', 'QR', 'QT', 'QB', 'QV', 'QA', 'SI', 'EZ', 'SE')
//	            AND dbo.T_Mark.SashizuNo IS NOT NULL)) 
//            ORDER BY dbo.VIEW_BecchuKeyInfo.ShiiresakiCode,VIEW_BecchuKeyInfo.OrderKanriNo,
//	            dbo.T_Becchu2Repeat.Hinban,dbo.T_Becchu2Repeat.LotNo,dbo.T_Becchu2Repeat.RowNo";
//            daBS2.SelectCommand.Parameters.AddWithValue("@uuu", dFrom);
//            daBS2.SelectCommand.CommandTimeout = 600000;

//            //DS2の取得
//            SqlDataAdapter daDS2 = new SqlDataAdapter("", sqlConn);
//            daDS2.SelectCommand.CommandText = @"
//            SELECT 
//                dbo.VIEW_DS2_Download.OrderKanriNo,
//	            dbo.VIEW_DS2_Download.NouhinKigou,
//	            dbo.VIEW_DS2_Download.MizunoUketsukeBi,
//	            dbo.VIEW_DS2_Download.MizunoTantousha,
//	            dbo.VIEW_DS2_Download.SKTantousha,
//	            '' AS RowNo,
//	            dbo.VIEW_DS2_Download.Hinban,
//	            CAST(dbo.VIEW_DS2_Download.Size AS NVARCHAR) AS Size,
//	            CAST(dbo.VIEW_DS2_Download.Suryou AS NVARCHAR) AS Suuryou,
//	            '' AS LotNo,
//	            dbo.VIEW_DS2_Download.NiukeJigyoushoCode,
//	            dbo.VIEW_DS2_Download.HokanBasho,
//	            dbo.VIEW_DS2_Download.ShiiresakiCode,
//	            dbo.VIEW_DS2_Download.ShiiresakiMei,
//	            dbo.VIEW_DS2_Download.TokuisakiCode,
//	            dbo.VIEW_DS2_Download.TokuisakiMei,
//	            dbo.VIEW_DS2_Download.Tuika1_Hinban,
//	            CAST(dbo.VIEW_DS2_Download.Tuika1_Size AS NVARCHAR) AS Tuika1_Size,
//	            CAST(dbo.VIEW_DS2_Download.Tuika1_Suryou AS NVARCHAR) AS Tuika1_Suryou,
//	            dbo.VIEW_DS2_Download.Tuika1_LotNo,
//	            dbo.VIEW_DS2_Download.Tuika2_Hinban,
//	            CAST(dbo.VIEW_DS2_Download.Tuika2_Size AS NVARCHAR) AS Tuika2_Size,
//	            CAST(dbo.VIEW_DS2_Download.Tuika2_Suryou AS NVARCHAR) AS Tuika2_Suryou,
//	            dbo.VIEW_DS2_Download.Tuika2_LotNo,
//	            dbo.VIEW_DS2_Download.Tuika3_Hinban,
//	            CAST(dbo.VIEW_DS2_Download.Tuika3_Size AS NVARCHAR) AS Tuika3_Size,
//	            CAST(dbo.VIEW_DS2_Download.Tuika3_Suryou AS NVARCHAR) AS Tuika3_Suryou,
//	            dbo.VIEW_DS2_Download.Tuika3_LotNo
//            FROM dbo.VIEW_DS2_Download LEFT OUTER JOIN
//                T_Mark ON dbo.VIEW_DS2_Download.MizunoUketsukeBi = T_Mark.SashizuBi AND
//                    dbo.VIEW_DS2_Download.OrderKanriNo = T_Mark.SashizuNo
//            WHERE Hinban != '' 
//	            AND Suryou > 0 
//	            AND (case isdate(dbo.VIEW_DS2_Download.MizunoUketsukeBi) when 1 then cast(dbo.VIEW_DS2_Download.MizunoUketsukeBi as datetime) else null end ) = CAST(@uuu as datetime) 
//	            AND dbo.VIEW_DS2_Download.ShumokuCode != 'KA' 
//	            AND dbo.VIEW_DS2_Download.ShumokuCode != 'KB' 
//	            AND dbo.VIEW_DS2_Download.ShumokuCode != 'M2' 
//	            AND (dbo.VIEW_DS2_Download.ShumokuCode NOT IN ('QS', 'QR', 'QT', 'QB', 'QV', 'QA', 'SI', 'EZ', 'SE') 
//	            OR (dbo.VIEW_DS2_Download.ShumokuCode IN ('QS', 'QR', 'QT', 'QB', 'QV', 'QA', 'SI', 'EZ', 'SE') 
//	            AND dbo.T_Mark.SashizuNo IS NOT NULL))
//            ORDER BY dbo.VIEW_DS2_Download.ShiiresakiCode,VIEW_DS2_Download.OrderKanriNo,
//	            dbo.VIEW_DS2_Download.Hinban";
//            daDS2.SelectCommand.Parameters.AddWithValue("@uuu", dFrom);
//            daDS2.SelectCommand.CommandTimeout = 600000;

//            //SPの取得
//            SqlDataAdapter daSP = new SqlDataAdapter("", sqlConn);
//            daSP.SelectCommand.CommandText = @"
//            SELECT
//	            dbo.VIEW_BecchuKeyInfo.OrderKanriNo,
//	            dbo.VIEW_BecchuKeyInfo.NouhinKigou,
//	            dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi,
//	            dbo.VIEW_BecchuKeyInfo.MizunoTantousha,
//	            dbo.VIEW_BecchuKeyInfo.SKTantousha,
//	            '' AS RowNo,
//	            dbo.VIEW_SPMeisai.Hinban,
//	            CAST(dbo.VIEW_SPMeisai.Size AS NVARCHAR) AS Size,
//                CAST(dbo.VIEW_SPMeisai.Suuryou AS NVARCHAR) AS Suuryou,
//                '' AS LotNo,
//                dbo.VIEW_SPMeisai.NiukeJigyoushoCode,
//                dbo.VIEW_SPMeisai.NiukeHokanBasho AS HokanBasho,
//                dbo.VIEW_BecchuKeyInfo.ShiiresakiCode,
//                dbo.VIEW_BecchuKeyInfo.ShiiresakiMei,
//	            dbo.VIEW_BecchuKeyInfo.TokuisakiCode,
//                dbo.VIEW_BecchuKeyInfo.TokuisakiMei,
//                dbo.VIEW_SPMeisai.Tuika1_Hinban,
//                CAST(dbo.VIEW_SPMeisai.Tuika1_Size AS NVARCHAR) AS Tuika1_Size,
//                CAST(dbo.VIEW_SPMeisai.Tuika1_Suryou AS NVARCHAR) AS Tuika1_Suryou,
//                dbo.VIEW_SPMeisai.Tuika1_LotNo,
//                dbo.VIEW_SPMeisai.Tuika2_Hinban,
//                CAST(dbo.VIEW_SPMeisai.Tuika2_Size AS NVARCHAR) Tuika2_Size,
//                CAST(dbo.VIEW_SPMeisai.Tuika2_Suryou AS NVARCHAR) Tuika2_Suryou,
//                dbo.VIEW_SPMeisai.Tuika2_LotNo,
//                dbo.VIEW_SPMeisai.Tuika3_Hinban,
//                CAST(dbo.VIEW_SPMeisai.Tuika3_Size AS NVARCHAR) AS Tuika3_Size,
//                CAST(dbo.VIEW_SPMeisai.Tuika3_Suryou AS NVARCHAR) AS Tuika3_Suryou,
//                dbo.VIEW_SPMeisai.Tuika3_LotNo
//            FROM
//                dbo.VIEW_BecchuKeyInfo
//                INNER JOIN
//                    dbo.VIEW_SPMeisai
//                ON  dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.VIEW_SPMeisai.OrderKanriNo
//                AND dbo.VIEW_BecchuKeyInfo.ShiiresakiCode = dbo.VIEW_SPMeisai.ShiiresakiCode 
//                LEFT OUTER JOIN
//                    dbo.T_Mark
//                ON  dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.T_Mark.SashizuNo
//                AND dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi = dbo.T_Mark.SashizuBi
//            WHERE (case isdate(VIEW_BecchuKeyInfo.MizunoUketsukeBi) when 1 then cast(VIEW_BecchuKeyInfo.MizunoUketsukeBi as datetime) else null end ) = CAST(@uuu as datetime)
//	            AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KA' 
//	            AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KB' 
//	            AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'M2' 
//	            AND (dbo.VIEW_BecchuKeyInfo.ShumokuCode NOT IN ('QS', 'QR', 'QT', 'QB', 'QV', 'QA', 'SI', 'EZ', 'SE') 
//	            OR (dbo.VIEW_BecchuKeyInfo.ShumokuCode IN ('QS', 'QR', 'QT', 'QB', 'QV', 'QA', 'SI', 'EZ', 'SE') 
//	            AND dbo.T_Mark.SashizuNo IS NOT NULL))
//            ORDER BY dbo.VIEW_BecchuKeyInfo.ShiiresakiCode,VIEW_BecchuKeyInfo.OrderKanriNo,
//	            dbo.VIEW_SPMeisai.Hinban";
//            daSP.SelectCommand.Parameters.AddWithValue("@uuu", dFrom);
//            daSP.SelectCommand.CommandTimeout = 600000;

//            //材料の取得
//            SqlDataAdapter daZairyou = new SqlDataAdapter("", sqlConn);
//            daZairyou.SelectCommand.CommandText = @"
//            SELECT 
//	            HikitoriOrderNo AS OrderKanriNo,
//	            NouhinKigou,
//	            SashizuBi AS MizunoUketsukeBi,
//	            Tantousha AS MizunoTantousha,
//	            Shouninsha AS SKTantousha,
//	            CAST(RowNo AS NVARCHAR) AS RowNo,
//	            Hinban,
//	            '' AS Size,
//	            CAST(CAST(Suuryou AS INT) AS NVARCHAR) AS Suuryou,
//	            CAST(LotNo AS NVARCHAR) AS LotNo,
//	            NiukeJigyoushoCode,
//	            HokanBasho,
//	            ShiiresakiCode,
//	            '' AS ShiiresakiMei,
//	            '' AS TokuisakiCode,
//	            '' AS TokuisakiMei,
//	            '' AS Tuika1_Hinban,
//	            '' AS Tuika1_Size,
//	            '' AS Tuika1_Suryou,
//	            '' AS Tuika1_LotNo,
//	            '' AS Tuika2_Hinban,
//	            '' AS Tuika2_Size,
//	            '' AS Tuika2_Suryou,
//	            '' AS Tuika2_LotNo,
//	            '' AS Tuika3_Hinban,
//	            '' AS Tuika3_Size,
//	            '' AS Tuika3_Suryou,
//	            '' AS Tuika3_LotNo
//            FROM T_ZairyouOrder 
//            WHERE  CAST(SashizuBi AS DATETIME) = CAST(@uuu AS DATETIME) 
//            ORDER BY ShiiresakiCode, HikitoriOrderNo, Hinban, LotNo, RowNo";
//            daZairyou.SelectCommand.Parameters.AddWithValue("@uuu", dFrom);
//            daZairyou.SelectCommand.CommandTimeout = 600000;

//            //テーブル宣言
//            DataTable dtMerge = new DataTable(); //全体をこのテーブルにまとめる
//            DataTable dtProper = new DataTable();
//            DataTable dtBS = new DataTable();
//            DataTable dtBS2 = new DataTable();
//            DataTable dtDS2 = new DataTable();
//            DataTable dtSP = new DataTable();
//            DataTable dtZairyou = new DataTable();

//            try
//            {
//                //検索　データ件数による処理時間の長さを確認する事
//                daProper.Fill(dtProper);
//                daBS.Fill(dtBS);
//                daBS2.Fill(dtBS2);
//                daDS2.Fill(dtDS2);
//                daSP.Fill(dtSP);
//                daZairyou.Fill(dtZairyou);

//                //全てを一つにまとめる 項目を全て合わせていないとエラーになるので注意
//                dtMerge = dtProper;
//                dtMerge.Merge(dtBS);
//                dtMerge.Merge(dtBS2);
//                dtMerge.Merge(dtDS2);
//                dtMerge.Merge(dtSP);
//                dtMerge.Merge(dtZairyou);

//                StringBuilder sb = new StringBuilder();
//                //書き込みデータ作成
//                for (int i = 0; i < dtMerge.Rows.Count; i++)
//                {
//                    for (int c = 0; c < dtMerge.Columns.Count; c++)
//                    {
//                        if (c != 0) sb.Append(",");
//                        sb.Append(dtMerge.Rows[i][c].ToString());
//                    }
//                    sb.Append("\r\n");
//                }

//                System.IO.StreamWriter sw = new StreamWriter(strFilePath, true, System.Text.Encoding.GetEncoding(932));
//                strData = sb.ToString();
//                //出力
//                sw.Write(strData);

//                if (sw != null)
//                {
//                    sw.Close();
//                }
//            }
//            catch (Exception ex)
//            {
//                return new LibError(ex);
//            }

//            return null;
//        }
            
    }
}
