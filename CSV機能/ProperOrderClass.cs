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
            Mishukka = 0,       // ���o��
            ShukkaZumi = 1,     // �o�׍ς�
            MiUketuke = 2       // ����t
        }


        [Serializable]
        public class KensakuParam
        {
            // ���[�U�[�敪
            public EnumUserType UserType = EnumUserType.Shiiresaki;

            // �\���\���� 2012.04.05�ǉ�
            public int nHyoujiTukisu = 24; // �~�Y�m��t���̉ߋ�24�J�������{���\

            // �w�}��
            public Core.Type.NengappiKikan _SashizuBi = null;

            // �d����R�[�h
            public string _ShiiresakiCode = "";

            public List<string> lstTuikaShiiresakiCode = null; // �q���


            // ���Y�I�[�_�[No
            public string _SeisanOrderNo = "";
            // ����I�[�_�[No
            public string _HikitoriOrderNo = "";
            // ��
            public EnumChumonStatus ChumonStatus = EnumChumonStatus.None;

            public List<ProperOrderKey> lstProperOrderKey = new List<ProperOrderKey>();

            // �\�Z��                        
            public Core.Type.Nengetu ngYosanTuki = null;

            // �i��
            public string _Hinban = "";
            // �[�i�L��
            public string _NouhinKigou = null;
            // �׎󎖋Ə�
            public string _NiukeJigyousho = null;

            // �Ĕ����t���O
            public EnumYesNo SaiHacchuFlg = EnumYesNo.None;


            public EnumMsgStatus MsgStatus = EnumMsgStatus.None;

            public ViewDataset.VIEW_ProperOrderHinmokuDataTable
                getVIEW_ProperOrderHinmokuDataTable(SqlConnection sqlConn)
            {
                SqlDataAdapter da = new SqlDataAdapter("", sqlConn);

                Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
                GetWhereT_ProperOrder(this, w, da.SelectCommand);
                string strWhereT_ProperOrder = (string.IsNullOrEmpty(w.WhereText)) ? "" : " where " + w.WhereText;


                // ���̃f�[�^�̃O���[�v���̃L�[�ɑ΂��Č���
                Core.Sql.WhereGenerator wXXX = new Core.Sql.WhereGenerator();

                // ���Ə�(�󔒂���)
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


                // ������
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
                        // ���o��
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
                        // �o�׍ς�
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

            //�����t���O���������擾�ł��Ȃ��ׁA�e�[�u����ύX����
            public DataTable getT_ProperOrderDataTable(SqlConnection sqlConn)
            {
                //�����t���O�𐳂����擾�ł���悤�ɃN�G���ύX 20160920
                //�\�Z�i�e�[�u���ύX���ɂ͂����̒l�ɂ��ύX�������邱��
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


                // ���Ə�
                if (!string.IsNullOrEmpty(this._NiukeJigyousho))
                {
                    w.Add(string.Format("T_ProperOrder.NiukeJigyoushoCode = '{0}'", this._NiukeJigyousho));
                }


                // ��
                switch (this.ChumonStatus)
                {
                    case EnumChumonStatus.MiUketuke:
                        w.Add(@"not exists (select * from T_ProperUketsuke where 
T_ProperUketsuke.SashizuBi=T_ProperOrder.SashizuBi and 
T_ProperUketsuke.SeisanOrderNo=T_ProperOrder.SeisanOrderNo and 
T_ProperUketsuke.HikitoriOrderNo=T_ProperOrder.HikitoriOrderNo)");
                        break;
                    case EnumChumonStatus.Mishukka:
                        // ���o��
                        w.Add("T_ProperOrder.KanryouFlg =0 AND T_ProperOrder.Suuryou > T_ProperOrder.ShukkaSuu ");
                        break;
                    case EnumChumonStatus.ShukkaZumi:
                        w.Add("((T_ProperOrder.KanryouFlg=9 or T_ProperOrder.KanryouFlg=8) or T_ProperOrder.Suuryou=T_ProperOrder.ShukkaSuu)");
                        break;
                }

                if (!string.IsNullOrEmpty(w.WhereText))
                    da.SelectCommand.CommandText += " where " + w.WhereText;

                //�����t���O���������擾�ł��Ȃ��ׁA�e�[�u����ύX����
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

                // ���Ə�(�󔒂���)
                if (null != this._NiukeJigyousho)
                {
                    w.Add(string.Format("T_ProperOrder.NiukeJigyoushoCode = '{0}'", this._NiukeJigyousho));
                }

                // ��
                switch (this.ChumonStatus)
                {
                    case EnumChumonStatus.MiUketuke:
                        w.Add(@"not exists (select * from T_ProperUketsuke where 
T_ProperUketsuke.SashizuBi=T_ProperOrder.SashizuBi and 
T_ProperUketsuke.SeisanOrderNo=T_ProperOrder.SeisanOrderNo and 
T_ProperUketsuke.HikitoriOrderNo=T_ProperOrder.HikitoriOrderNo)");
                        break;
                    case EnumChumonStatus.Mishukka:
                        // ���o��
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
            /// VIEW_ProperOrderKeyDataTable ��VIEW_ProperOrderHinmokuDataTable�ŃO���[�v������O��T_ProperOrder�ɑ΂��Đݒ�ł��鋤�ʂ̏���
            /// </summary>
            /// <param name="k"></param>
            /// <param name="w"></param>
            /// <param name="cmd"></param>
            private void GetWhereT_ProperOrder(KensakuParam k, Core.Sql.WhereGenerator w, SqlCommand cmd)
            {
                // �~�Y�m��t���\���Ώۊ��� 2012.04.05�ǉ�
                if (0 < k.nHyoujiTukisu)
                {
                    // ���c�ł����Ă��\���Ώۊ��Ԃ�ݒ肷��B
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


                // �w�}��
                if (k._SashizuBi != null)
                {
                    w.Add(Core.Type.NengappiKikan.GenerateSQL(k._SashizuBi, false, "T_ProperOrder.SashizuBi"));
                }

                // �d����R�[�h(�w�肠��)

                if (k._ShiiresakiCode != "")
                {
                    w.Add(string.Format("T_ProperOrder.ShiiresakiCode = @ShiiresakiCode"));
                    cmd.Parameters.AddWithValue("@ShiiresakiCode", k._ShiiresakiCode);
                }
                // �d����R�[�h(����)
                if (k.lstTuikaShiiresakiCode != null && 0 < k.lstTuikaShiiresakiCode.Count)
                {
                    w.Add(string.Format("T_ProperOrder.ShiiresakiCode in ('{0}')", string.Join("','", k.lstTuikaShiiresakiCode.ToArray())));
                }


                // ���Y�I�[�_�[No
                if (!string.IsNullOrEmpty(k._SeisanOrderNo))
                {
                    w.Add(string.Format("T_ProperOrder.SeisanOrderNo LIKE @SeisanOrderNo"));
                    cmd.Parameters.AddWithValue("@SeisanOrderNo", "%" + k._SeisanOrderNo + "%");
                }
                // ����I�[�_�[No
                if (!string.IsNullOrEmpty(k._HikitoriOrderNo))
                {
                    w.Add(string.Format("T_ProperOrder.HikitoriOrderNo LIKE @HikitoriOrderNo"));
                    cmd.Parameters.AddWithValue("@HikitoriOrderNo", k._HikitoriOrderNo + "%");
                }


                // �\�Z��
                if (null != k.ngYosanTuki)
                {
                    w.Add(string.Format("T_ProperOrder.YosanTsuki='{0}'", k.ngYosanTuki.ToYYYYMM()));
                }

                // �i��
                if (k._Hinban != "")
                {
                    w.Add(string.Format("LTRIM(T_ProperOrder.Hinban) LIKE @Hinban"));
                    cmd.Parameters.AddWithValue("@Hinban", k._Hinban + "%");
                }
                // �[�i�L��(�󔒂����肠����)
                if (null != k._NouhinKigou)
                {
                    w.Add(string.Format("T_ProperOrder.NouhinKigou = '{0}'", k._NouhinKigou.Trim()));
                }

                // �Ĕ����t���O(�����P��(SashizuBi+SeisanOrderNo+HikitoriOrderNo)�Őݒ肳������)
                switch (this.SaiHacchuFlg)
                {
                    case EnumYesNo.Yes:
                        // ����
                        w.Add("T_ProperOrder.SaiHacchuFlg <> ''");
                        break;
                    case EnumYesNo.No:
                        // �Ȃ�
                        w.Add("T_ProperOrder.SaiHacchuFlg = ''");
                        break;
                }


                // ���b�Z�[�W(���b�Z�[�W�͒����P��(SashizuBi+SeisanOrderNo+HikitoriOrderNo)�Őݒ肳���)
                string strForShiiresaki = "";
                if (this.UserType == EnumUserType.Shiiresaki)
                {
                    // �{�\�Z�i�f�[�^�͐e/�q��ƂŌ�������邪�A���b�Z�[�W�̓��O�C�����Ă���d����Ō�������ׁA���̏������K�v
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
                        // ��M
                        if (this.UserType == EnumUserType.Shiiresaki)
                            w.Add(strMizuno2Torihikisaki);
                        else
                            w.Add(strTorihikisaki2Mizuno);
                        break;
                    case EnumMsgStatus.Shoushin:
                        // ���M
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
        /// ���������i���ʏ��i�ʔ����ꗗ�t�H�[���p�j
        /// </summary>
        public class KensakuParam_HacchuIchiran
        {
            // ���[�U�[�敪
            public byte _UserKubun = 0;
            // �d����R�[�h
            public string _ShiiresakiCode = "";
            // �d����R�[�h
            public string[] _SCodeAry = null;
            // ����
            public string _Bumon = "";
            // �i��
            public string _Hinban = "";
            // �\���N��(From)
            public string _From = "";
            // �\���N��(To)
            public string _To = "";
        }
        

        /// <summary>
        /// �\�Z�i��1�I�[�_�[������̎�L�[(�e�[�u���̎�L�[�Ƃ͈قȂ�)
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
        /// �\�Z�i�f�[�^�S�s�擾(���ꉽ�H�H�H�H)
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

            // �i�Ԏw�肠��
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
        /// <param name="strNouhinKigou">�󔒂�����</param>
        /// <param name="strNiukeBumonCode">�󔒂�����</param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static ViewDataset.VIEW_ProperOrderDataTable
            getVIEW_ProperOrderDataTable(ProperOrderKey key, string strHinban,
            Core.Type.Nengetu ngYosanTuki, string strNouhinKigou, string strNiukeJigyoushoCode, Core.Sql.RowNumberInfo r, SqlConnection sqlConn, ref int nCount)
        {

            nCount = 0;
            //�����t���O���������擾�ł��Ȃ��ׁA�ύX���� 20160920
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
        /// �o�ד��͂���ۂ̓��͑Ώۍ���
        /// ���Ə��R�[�h�̏����������ꍇ������B
        /// �E���[�ς݂��擾���Ă���B
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
SELECT                  TOP (100) PERCENT Base.Hinban, Base.Size, Base.Kakaku, Base.Suuryou, Base.ShukkaSuu, �i�ԃT�C�Y�ʂ̍ŏ��sNo.HinbanSizeBetuSuuryou, 
�i�ԃT�C�Y�ʂ̍ŏ��sNo.HinbanSizeBetuShukkaSuu
FROM                     (SELECT                  Hinban, Size, Kakaku, SUM(Suuryou) AS Suuryou, SUM(ShukkaSuu) AS ShukkaSuu
FROM                     dbo.T_ProperOrder
WHERE (SashizuBi = @SashizuBi) AND (SeisanOrderNo = @SeisanOrderNo) AND (HikitoriOrderNo = @HikitoriOrderNo) {0}  
GROUP BY          Hinban, Size, Kakaku) AS Base LEFT OUTER JOIN
(SELECT                  Hinban, Size, MIN(RowNo) AS MinRowNo, SUM(Suuryou) AS HinbanSizeBetuSuuryou, SUM(ShukkaSuu) 
AS HinbanSizeBetuShukkaSuu
FROM                     dbo.T_ProperOrder AS T_ProperOrder_1
WHERE                   (SashizuBi = @SashizuBi) AND (SeisanOrderNo = @SeisanOrderNo) AND (HikitoriOrderNo = @HikitoriOrderNo)
GROUP BY          Hinban, Size) AS �i�ԃT�C�Y�ʂ̍ŏ��sNo ON Base.Hinban = �i�ԃT�C�Y�ʂ̍ŏ��sNo.Hinban AND 
Base.Size = �i�ԃT�C�Y�ʂ̍ŏ��sNo.Size

LEFT OUTER JOIN
(SELECT                  Hinban, Size, MIN(RowNo) AS MinRowNo, SUM(Suuryou) AS HinbanSizeBetuSuuryou, SUM(ShukkaSuu) 
AS HinbanSizeBetuShukkaSuu
FROM                     dbo.T_ProperOrder AS T_ProperOrder_1
WHERE                   (SashizuBi = @SashizuBi) AND (SeisanOrderNo = @SeisanOrderNo) AND (HikitoriOrderNo = @HikitoriOrderNo) AND (NiukeJigyoushoCode = @NiukeJigyoushoCode)
GROUP BY          Hinban, Size) AS ���Ə��ʂ̍ŏ��sNo ON Base.Hinban = ���Ə��ʂ̍ŏ��sNo.Hinban AND 
Base.Size = ���Ə��ʂ̍ŏ��sNo.Size";

            if (!string.IsNullOrEmpty(strNiukeJigyoushoCode))
            {
                da.SelectCommand.CommandText += " ORDER BY ���Ə��ʂ̍ŏ��sNo.MinRowNo ";
            }
            else
            {
                da.SelectCommand.CommandText += " ORDER BY �i�ԃT�C�Y�ʂ̍ŏ��sNo.MinRowNo ";
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

            // �[�i�L��(�󔒂����肦��)
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

        //�����t���O���������擾�ł��Ȃ��ׁA�e�[�u����ύX���� T_ProperOrderDataTable �� DataTable
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


            // ��
            switch (s)
            {
                case EnumChumonStatus.MiUketuke:
                    w.Add(@"not exists (select * from T_ProperUketsuke where 
T_ProperUketsuke.SashizuBi=T_ProperOrder.SashizuBi and 
T_ProperUketsuke.SeisanOrderNo=T_ProperOrder.SeisanOrderNo and 
T_ProperUketsuke.HikitoriOrderNo=T_ProperOrder.HikitoriOrderNo)");
                    break;
                case EnumChumonStatus.Mishukka:
                    // ���o��
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


            // ��
            switch (s)
            {
                case EnumChumonStatus.MiUketuke:
                    w.Add(@"not exists (select * from T_ProperUketsuke where 
T_ProperUketsuke.SashizuBi=T_ProperOrder.SashizuBi and 
T_ProperUketsuke.SeisanOrderNo=T_ProperOrder.SeisanOrderNo and 
T_ProperUketsuke.HikitoriOrderNo=T_ProperOrder.HikitoriOrderNo)");
                    break;
                case EnumChumonStatus.Mishukka:
                    // ���o��
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
            // �~�Y�m��t���\���Ώۊ��� 2012.04.05�ǉ�
            if (0 < k.nHyoujiTukisu)
            {
                // ���c�ł����Ă��\���Ώۊ��Ԃ�ݒ肷��B
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


            // ��
            switch (s)
            {
                case EnumChumonStatus.MiUketuke:
                    w.Add(@"not exists (select * from T_ProperUketsuke where 
T_ProperUketsuke.SashizuBi=T_ProperOrder.SashizuBi and 
T_ProperUketsuke.SeisanOrderNo=T_ProperOrder.SeisanOrderNo and 
T_ProperUketsuke.HikitoriOrderNo=T_ProperOrder.HikitoriOrderNo)");
                    break;
                case EnumChumonStatus.Mishukka:
                    // ���o��
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
        /// <param name="strNouhinKigou">�[�i�L���͋󔒂�����</param>
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

            // ��
            switch (s)
            {
                case EnumChumonStatus.MiUketuke:
                    w.Add(@"not exists (select * from T_ProperUketsuke where 
T_ProperUketsuke.SashizuBi=T_ProperOrder.SashizuBi and 
T_ProperUketsuke.SeisanOrderNo=T_ProperOrder.SeisanOrderNo and 
T_ProperUketsuke.HikitoriOrderNo=T_ProperOrder.HikitoriOrderNo)");
                    break;
                case EnumChumonStatus.Mishukka:
                    // ���o��
                    w.Add("T_ProperOrder.KanryouFlg =0 AND T_ProperOrder.Suuryou > T_ProperOrder.ShukkaSuu ");
                    break;
                case EnumChumonStatus.ShukkaZumi:
                    w.Add("(T_ProperOrder.KanryouFlg=1 or T_ProperOrder.Suuryou=T_ProperOrder.ShukkaSuu)");
                    break;
            }

            //�[�i�L���̃`�F�b�N�͊O��_2012-12-05ꎓ�
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
        /// ���c�̗\�Z���ꗗ�擾
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

            // ��
            switch (s)
            {
                case EnumChumonStatus.MiUketuke:
                    w.Add(@"not exists (select * from T_ProperUketsuke where 
T_ProperUketsuke.SashizuBi=T_ProperOrder.SashizuBi and 
T_ProperUketsuke.SeisanOrderNo=T_ProperOrder.SeisanOrderNo and 
T_ProperUketsuke.HikitoriOrderNo=T_ProperOrder.HikitoriOrderNo)");
                    break;
                case EnumChumonStatus.Mishukka:
                    // ���o��
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
            // �~�Y�m��t���\���Ώۊ��� 2012.04.05�ǉ�
            if (0 < k.nHyoujiTukisu)
            {
                // ���c�ł����Ă��\���Ώۊ��Ԃ�ݒ肷��B
                w.Add(string.Format("CAST(SashizuBi AS DATETIME) >= CAST('{0}' AS DATETIME)", DateTime.Today.AddMonths((-1) * k.nHyoujiTukisu)));
            }


            //// �[�i�L��(�󔒂����肦��)
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
                public string NiukeJigyoushoCode { get; set; }  // �׎󎖋Ə��̎w�肪��������NULL or ��
                public string Hinban { get; set; }
                public string Size { get; set; }
                public bool NiukeJigyoushoCodeHikiateAll { get; set; } // True:�w��̎��Ə��̔[���c�ɑS���������Ă� / False:�������ĉ\�ȕ������������Ă�B(�A�b�v���[�h�o�^���������Č���̎��Ə��̎w�肪�����ׁj

                public MeisaiKey()
                {
                    NiukeJigyoushoCodeHikiateAll = true;
                }
            }

            public MeisaiKey Key = new MeisaiKey();


        }

        public class TourokuData : NouhinDataClass.TourokuDataBase
        {
            public string ShukkaNiukeJigyoushoCode = null; // �o�בΏۂƂȂ鎖�Ə��R�[�h(��ʂ���̓o�^�̏ꍇ�̂�)
            public Core.Type.Nengetu ngHonNouhinTuki = null;    // ���i�敪�����[�i�̎�����
            public string invoiceNo = null;
            public string UnsouGyoushaCode = null;
            public string UnsouGyoushaMei = null;
            public string KogutiSu = null;

            public List<MeisaiData> lstMeisai = new List<MeisaiData>();
        }


        // ���[�i����
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

        // ���[�i����
        internal static void DenpyouTouroku(bool bUpload, bool bShusei, ProperOrderKey pk, Core.Type.Nengetu ngYosanTuki, string strNouhinKigou,
            TourokuData data, SqlTransaction t, out List<NouhinDataClass.DenpyouKey> lstKey, out List<NouhinDataClass.DenpyouKey> lstKeyShukkaAri)
        {
            lstKeyShukkaAri = new List<NouhinDataClass.DenpyouKey>();
            lstKey = new List<NouhinDataClass.DenpyouKey>();

            int nPageSize = NouhinDataClass.GetDenpyouGyouCount(data.SouhinKubun); // ���i�敪�ɂ���ēo�^�ł���s�����قȂ�B

            // �o�א� = 0 && �����t���O = 9(����)�̃f�[�^�͕ʂ̓Ɨ������`�[�œo�^����B��ʏ�͓`�[���s�����Ńf�[�^�̂ݓo�^����(���M������))
            List<MeisaiData> lstShukka = new List<MeisaiData>();            
            List<MeisaiData> lstZeroKanryou = new List<MeisaiData>();
            List<MeisaiData> lstShukka_KeigenZeiritsu = new List<MeisaiData>(); // �y���ŗ��͕ʓ`�[�Ŕ��s �ǉ� 20190821 M0458
            List<MeisaiData> lstZeroKanryou_KeigenZeiritsu = new List<MeisaiData>(); // �y���ŗ��͕ʓ`�[�Ŕ��s �ǉ� 20190821 M0458
            //�y���ŗ��p�@10��1���@�ȍ~�ɏ���������悤��
            int nSashizuBi = int.Parse(DateTime.Now.ToShortDateString().Replace("/", ""));
            int nZeiritsuHenkouBi = 20191001;
            for (int i = 0; i < data.lstMeisai.Count; i++) {
                if (0 == data.lstMeisai[i].ShukkaSu && data.lstMeisai[i].KanryouFlag)
                {
                    // �y���ŗ��̔����ǉ��@20190821 M0458
                    int.TryParse(pk.SashizuBi.Replace("/", ""), out nSashizuBi);
                    if (data.lstMeisai[i].KeigenZeiritsu == "R8K" && nSashizuBi >= nZeiritsuHenkouBi) // 2019�N10�����珈������
                        lstZeroKanryou_KeigenZeiritsu.Add(data.lstMeisai[i]);
                    else
                        lstZeroKanryou.Add(data.lstMeisai[i]);
                }
                else
                {
                    // �y���ŗ��̔����ǉ��@20190821 M0458
                    int.TryParse(pk.SashizuBi.Replace("/", ""), out nSashizuBi);
                    if (data.lstMeisai[i].KeigenZeiritsu == "R8K" && nSashizuBi >= nZeiritsuHenkouBi) // 2019�N10�����珈������
                        lstShukka_KeigenZeiritsu.Add(data.lstMeisai[i]);
                    else
                        lstShukka.Add(data.lstMeisai[i]);
                }
            }

            List<List<MeisaiData>> lstDenpyou = new List<List<MeisaiData>>();   // �o�ׂ���f�[�^�A����0&�����f�[�^�̏��̏o�͂���

            // �o�׃f�[�^����
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

            // �o�׃f�[�^����@�y���ŗ��@�ǉ��@20190821�@M0458
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

            // �o�א�=0&�����t���O��9�̃f�[�^
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

            // �o�א�=0&�����t���O��9�̃f�[�^�@�y���ŗ��@�ǉ��@20190821�@M0458
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

            // �y�[�W�P�ʂō쐬
            List<MeisaiData> lstOrg = data.lstMeisai;
            for (int i = 0; i < lstDenpyou.Count; i++)
            {
                data.lstMeisai = lstDenpyou[i]; // �ꎞ�I�ɐݒ肷��B
                NouhinDataClass.DenpyouKey key = DenpyouTouroku(bShusei, 0, pk, ngYosanTuki, strNouhinKigou, data, t);
                lstKey.Add(key);

                if (i < nShukkaAriDenpyouCount)
                    lstKeyShukkaAri.Add(key);
            }
            data.lstMeisai = lstOrg;    // �{�֐�������Ɏg�p���邩������Ȃ��̂Ō��ɖ߂�

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


            // �ő�̔��sNo�擾
            SqlCommand cmdGetMaxHakkouNo = new SqlCommand("", t.Connection);
            cmdGetMaxHakkouNo.Transaction = t;
            cmdGetMaxHakkouNo.CommandText = @"SELECT MAX(HakkouNo) AS HacchuNo FROM T_NouhinHeader WHERE (NouhinmotoShiiresakiCode = @s) AND (YYMM = @YYMM) ";
            cmdGetMaxHakkouNo.Parameters.AddWithValue("@s", strShiiresakiCode);
            cmdGetMaxHakkouNo.Parameters.AddWithValue("@YYMM", int.Parse(data.dtHakkouBi.ToString("yyMM")));


            // �\�Z�i�e�[�u�����璍�c���擾(1������1�[�i�L���Ȃ̂Ŕ[�i�L���̏����͕s�v�Ǝv��)
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

            // �{�����̍ŏ��̕i�ԁA�T�C�Y�擾�i�P�s�ڂ��ǉ��i�Ԃ̏ꍇ�̃_�~�[�f�[�^�j
            SqlDataAdapter daGetFirstHinbanSize = new SqlDataAdapter("", t.Connection);
            daGetFirstHinbanSize.SelectCommand.CommandText = @"SELECT Hinban, Size FROM T_ProperOrder WHERE (RowNo = 1) AND (SashizuBi = @z) AND (SeisanOrderNo = @s) AND (HikitoriOrderNo = @h)";
            daGetFirstHinbanSize.SelectCommand.Parameters.AddWithValue("@z", key.SashizuBi);
            daGetFirstHinbanSize.SelectCommand.Parameters.AddWithValue("@s", key.SeisanOrderNo);
            daGetFirstHinbanSize.SelectCommand.Parameters.AddWithValue("@h", key.HikitoriOrderNo);
            daGetFirstHinbanSize.SelectCommand.Transaction = t;


            // ����
            SqlDataAdapter daM = new SqlDataAdapter("select * from T_NouhinMeisai", t.Connection);
            daM.SelectCommand.Transaction = t;
            daM.InsertCommand = new SqlCommandBuilder(daM).GetInsertCommand();
            daM.InsertCommand.Transaction = t;

            // �g���[���[
            SqlDataAdapter daTrailer = new SqlDataAdapter("", t.Connection);
            daTrailer.SelectCommand.CommandText = "select * from T_NouhinTrailer";
            daTrailer.SelectCommand.Transaction = t;
            daTrailer.InsertCommand = new SqlCommandBuilder(daTrailer).GetInsertCommand();
            daTrailer.InsertCommand.Transaction = t;


            // �\�Z�i�������ď��
            SqlDataAdapter daHikiate = new SqlDataAdapter("select * from T_ProperOrdrer_NouhinHikiate", t.Connection);
            daHikiate.SelectCommand.Transaction = t;
            daHikiate.InsertCommand = new SqlCommandBuilder(daHikiate).GetInsertCommand();
            daHikiate.InsertCommand.Transaction = t;



            // �����t���O�̃Z�b�g(�\�Z�� + �i�� + �T�C�Y�P��):���[�ɑ΂��Ă̓Z�b�g���Ȃ�
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

            // �\�Z�� + �i�� + �T�C�Y�P�ʂ̏o�׎c���擾
            // ���S���o�ׂōŏI���M�̔[�i�f�[�^�Ɋ����t���O���Z�b�g����B
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

            // �\�Z�� + �i�� + �T�C�Y�P�ʂŁA�����M�̔[�i�f�[�^�擾
            // ���ꕔ���[�Ŋ������́A�ŏI���M�Ώۂ̔[�i�f�[�^�Ɋ����t���O�������Ă��邩�`�F�b�N����B
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
            //�\�Z�i�f�[�^�̖��ׂ��擾
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
                    //2013.04.08 ���� 
                    //99999 �܂ŗ����� 80001�֖߂��悤�C��

                    // 90000�ȉ��ōő�̔��sNo�擾
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
                // �`�[�C������YYMM���ύX�Ȃ��A���sNo�ɕύX�������ꍇ�͂���
                // ���sNo�͎w�肳��Ă���
            }

            // ----- �w�b�_�[ ----
            MizunoDataSet.T_NouhinHeaderDataTable dtHeader = new MizunoDataSet.T_NouhinHeaderDataTable();
            MizunoDataSet.T_NouhinHeaderRow drHeader = dtHeader.NewT_NouhinHeaderRow();
            NouhinDataClass.InitT_NouhinHeaderRow(drHeader);
            
            // ���sNo
            drHeader.HakkouNo = nHakkouNo;
            // �`�[�敪
            drHeader.DenpyouKubun = DenpyouKubun._26;
            // �d����R�[�h
            drHeader.NouhinmotoShiiresakiCode = strShiiresakiCode;
            // �o�ד��͉�ʂőI�����ꂽ�[�i�L��
            drHeader.NouhinKigou = data.NouhinKigou;

            // �׎󎖋Ə��R�[�h                
            drHeader.NiukeBumon = data.NiukeJigyoushoCode;
            drHeader.NiukeBumonMei = data.NiukeJigyoushoMei;
            // �ۊǏꏊ
            drHeader.NiukeBasho = data.HokanBasho;
            drHeader.NiukeBashoMei = data.NiukeBashoMei;
            //invoiceNo
            drHeader.InvoiceNo = data.invoiceNo;

            //2014/02/14 ����
            //�^���Ǝ҃R�[�h
            drHeader.UnsouGyoushaCode = data.UnsouGyoushaCode;

            //�^���ƎҖ�
            drHeader.UnsouGyoushaMei = data.UnsouGyoushaMei;

            //����
            drHeader.KogutiSu = data.KogutiSu;



            //2013/12/04 ����
            //�ً}�������l
            drHeader.KinkyuChokusouBikou = data.KinkyuChokusouBikou;

            // �\�Z��
            drHeader.YosanTsuki = ngYosanTuki.ToYYYYMM().ToString();
            // �\�Z�i�I�[�_���̔[�i�L��
            drHeader.OrderNouhinKigou = (string.IsNullOrEmpty(strNouhinKigou))? "" : strNouhinKigou;

            // �����
            drHeader.HanbaitenKigyouRyakuMei = data.HanbaitenKigyouRyakuMei;
            // ������������
            
            drHeader.HakkouHiduke = data.dtHakkouBi.ToString("yyMMdd");
            
            // �������ɂ͎w�}����ݒ肷��B(yyMMdd)
            string strSashizuBi = key.SashizuBi.Replace("/", "");
            if (strSashizuBi.Length == 8)
            {
                drHeader.HakkouHiduke = strSashizuBi.Substring(2, 6);
            }

            // �o�ד��t                
            drHeader.ShukkaHiduke = data.dtHakkouBi.ToString("yyMMdd");
            // �o�ד�
            drHeader.ShukkaBi = data.dtHakkouBi;


            // YYMM
            drHeader.YYMM = int.Parse(data.dtHakkouBi.ToString("yyMM"));

            // ���i�敪
            drHeader.SouhinKubun = ((int)data.SouhinKubun).ToString();


            // �w�}��
            drHeader.SashizuBi = key.SashizuBi;
            // ���Y�I�[�_�[No
            drHeader.SeisanOrderNo = key.SeisanOrderNo;
            // ����I�[�_�[No
            drHeader.HikitoriOrderNo = key.HikitoriOrderNo;
            // �ޗ��I�[�_�[No
            drHeader.ZairyouOrderNo = "";
            // �ʒ��I�[�_�[No
            drHeader.OrderKanriNo = "";

            // ���M�t���O
            //2014/02/14 ���� �ǉ� �����No�ȂǓ��͂���Ă��Ȃ��ꍇ�͑��M�t���O=�������Ƃ���B
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
            
            // �[�i��
            drHeader.HonNouhinTsuki = (null == data.ngHonNouhinTuki) ? "" : data.ngHonNouhinTuki.ToString("yyyyMM");

            /*
            // �o�׎��Ə�
            // ���\�Z�i�̂ݎg�p�B�o�׎��̍i�荞�݂Ŏw�肵�����Ə��B
            // ���C�����ɂ��̎��Ə��̏o�א��𑝌�����ׂɋL�^���Ă����K�v������B
            drHeader.ShukkaNiukeBumon = (null == strNiukeBumonCode) ? "" : strNiukeBumonCode;
            */
            if (string.IsNullOrEmpty(data.ShukkaNiukeJigyoushoCode))
                drHeader.SetShukkaNiukeBumonNull();
            else
                drHeader.ShukkaNiukeBumon = data.ShukkaNiukeJigyoushoCode;


            // SK�S���Җ�
            drHeader.SKTantouMei = data.SKTantoushaMei;
            // �c�ƒS���Җ�
            drHeader.EigyouTantouMei = data.EigyouTantoushaMei;

            // �X��                
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
            List<string> lstKanryouFlgHinbanSize = new List<string>();  // ���񊮗��t���O���Z�b�g����i�ԃT�C�Y
            MizunoDataSet.T_ProperOrdrer_NouhinHikiateDataTable dtHikiate = new MizunoDataSet.T_ProperOrdrer_NouhinHikiateDataTable();


            for (int i = 0; i < data.lstMeisai.Count; i++)
            {
                MeisaiData m = data.lstMeisai[i];
                MizunoDataSet.T_NouhinMeisaiRow dr = dtM.NewT_NouhinMeisaiRow();
                NouhinDataClass.InitT_NouhinMeisaiRow(dr);
                dr.HakkouNo = nHakkouNo;
                dr.ShiiresakiCode = drHeader.NouhinmotoShiiresakiCode;
                dr.YYMM = drHeader.YYMM;

                if (0 == i && m.TuikaHinban && m.Key.Hinban.StartsWith("ĸ����"))
                {
                    // 1�s�ڂ�����i��(�^������ĸ����-XX)�ɂȂ��Ă��܂��ꍇ�͔����f�[�^��1�s�ڂ̕i�ԁA�T�C�Y�A�o�א� = 0�̃_�~�[�f�[�^��1�s�ڂɓ����
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
                    // �_�~�[�f�[�^�̈�
                    drDummy.RowNo = MeisaiRowNo.Dummy;
                    dtM.Rows.Add(drDummy);
                    nGyouNo = 2;
                }

                //2013-01-09 ꎓ��ύX-----
                if (NouhinDataClass.NOUHINDATA_MAX_GYOU_COUNT < nGyouNo)
                {
                    // �s�ԍ���9�ȏ�̎�
                    nGyouNo = 1; // �߂�
                    nEdaban++;
                }
                //-----------------------

                dr.GyouNo = nGyouNo;
                dr.Edaban = nEdaban;

                dr.Hinban = m.Key.Hinban;

                if (m.TuikaHinban)
                {
                    dr.HinbanTsuikaFlg = HinbanTsuikaFlg.Tsuika; // ��Fĸ����-03
                    dr.KanryouFlg = KanryouFlg.Kanryou; // �ǉ��i�Ԃ͊����t���O=9
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

                    // ���ۂ̗\�Z�i���R�[�h�̏o�א����Z�b�g
                    daProper.SelectCommand.CommandText = "select * from T_ProperOrder where SashizuBi=@z and SeisanOrderNo=@s and HikitoriOrderNo=@h and YosanTsuki=@y and Hinban=@Hinban and Size=@Size and Suuryou>ShukkaSuu";

                    if (!bShusei)
                        daProper.SelectCommand.CommandText += " and KanryouFlg=0";  // �C�����͊����t���O�͍l�����Ȃ��i100�̒�����90001��50�A90002�Ŏc��45�Ŋ����t���O�𓯂��o�ד�8/30�œo�^������A90001�̏o�ד���8/31�ɂ��čŏI�`�[�ɂ������ɖ��ƂȂ����ׁj

                    daProper.SelectCommand.Parameters["@Hinban"].Value = m.Key.Hinban;
                    daProper.SelectCommand.Parameters["@Size"].Value = m.Key.Size;

                    if (daProper.SelectCommand.Parameters.Contains("@NiukeJigyoushoCode"))
                        daProper.SelectCommand.Parameters.RemoveAt("@NiukeJigyoushoCode");

                    if (m.Key.NiukeJigyoushoCodeHikiateAll)
                    {
                        // �w��̎��Ə��̒������炵���������ĂȂ��B�S�Ĉ������ĂĂȂ���Δ[���c�Ȃ��ŃG���[�ɂ���ꍇ
                        if (!string.IsNullOrEmpty(m.Key.NiukeJigyoushoCode))
                        {
                            // ���Ə��w�肪����ꍇ�͂��̎��Ə��̃f�[�^����������Ă�
                            daProper.SelectCommand.CommandText += " and NiukeJigyoushoCode=@NiukeJigyoushoCode";
                            daProper.SelectCommand.Parameters.AddWithValue("@NiukeJigyoushoCode", m.Key.NiukeJigyoushoCode);
                        }
                    }
                    dtProper.Clear();
                    daProper.Fill(dtProper);
                    
                    if (!bShusei)
                    {
                        // �C���̎��́A���Ɋ����t���O�����̓`�[�ɃZ�b�g����Ă���ꍇ������̂Ń`�F�b�N�ΏۂƂ��Ȃ�
                        if (0 == dtProper.Count)
                            throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�̔[���c�͂���܂���B", m.Key.Hinban, m.Key.Size));
                    }


                    // �o�א�����������
                    int nShukkaSu = m.ShukkaSu;
                    dtProper.DefaultView.Sort = "RowNo";    // RowNo�̏��������̂����������

                    if (!m.Key.NiukeJigyoushoCodeHikiateAll && !string.IsNullOrEmpty(m.Key.NiukeJigyoushoCode))
                    {
                        //���Ə��̑I���������ꍇ�A���͉�ʂŐݒ肵���f�[�^�Ō������|����
                        //�`�[�f�[�^�A�b�v���[�h���ɂ�����ɓ��邽�ߐ�Ƀ`�F�b�N���s���Ă��� �����ꍇ�͒ʏ�̏�����������
                        dtProper.DefaultView.RowFilter =
                            string.Format("NiukeJigyoushoCode='{0}' AND HokanBasho='{1}'", drHeader.NiukeBumon, drHeader.NiukeBasho);

                        if (dtProper.DefaultView.Count == 0)
                        {
                            // �w��̎��Ə�����u��Ɂv�������Ă��镪�����������Ă�
                            dtProper.DefaultView.RowFilter = string.Format("NiukeJigyoushoCode='{0}'", m.Key.NiukeJigyoushoCode);
                        }
                    }
                    else
                    {
                        // ���͉�ʂŎw�肵�����Ə��E�ۊǏꏊ���A�g���̂��̂ƈꏏ�Ȃ�A
                        // ��������������s�� 20160825 ���O�ݒ肪�Ȃ��ꍇ�̂�
                        //���Ə��̑I���������ꍇ�A���͉�ʂŐݒ肵���f�[�^�Ō������|����
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
                        // �������ď��
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
                        //if (drP.ShukkaSuu == drP.Suuryou) drP.KanryouFlg = true;  // �������t���O�̓Z�b�g���Ȃ��i�����t���O�͐���>�o�א��̎��������I�������邽�߂ɃZ�b������j
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
                            // �������ď��
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
                            
                            //if (drP.ShukkaSuu == drP.Suuryou) drP.KanryouFlg = true;  // �������t���O�̓Z�b�g���Ȃ��i�����t���O�͐���>�o�א��̎��������I�������邽�߂ɃZ�b������j
                            if (0 == nShukkaSu) break;
                        }
                    }

                    if (0 < nShukkaSu)
                        throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�̏o�א����o�׎c���𒴂��Ă��܂��B", m.Key.Hinban, m.Key.Size));

                    daProper.Update(dtProper);


                    // �����t���O
                    dr.KanryouFlg = KanryouFlg.MiKanryou;   // �����t���O�͌�Őݒ肷��̂Ŗ������ɂ��Ă���
                    // �{���ׂɂ́A����i��+�T�C�Y�̃f�[�^���������݂���P�[�X������B�i����i�ԃT�C�Y�ŉ��i���قȂ�ꍇ�j
                    // �����t���O�́A�i��+�T�C�Y�P�ʂŐݒ肷�邪�A�������R�[�h�ɐݒ�o�����A�Ō�̃��R�[�h�ɑ΂��Đݒ肷��K�v����B

                }
                dr.Size = m.Key.Size;
                dr.LotNo = "";
                dr.Tani = "";

                // ����P��
                // 2013.01.24 1/17�ɔ[�i�f�[�^��TorihikiTanka���Œ蒷�łȂ��������A�\�Z�i�̕��ɂ����l�̏C�����K�v�������B
                dr.TorihikiTanka = m.TourokuKakaku.ToString();
                //dr.TorihikiTanka = m.TourokuKakaku.ToString("000000.0").Replace(".", "");
                // �[�i��
                dr.NouhinSuu = m.ShukkaSu;
                dr.NouhinKubun = (m.NouhinKubun == NouhinDataClass.EnumNouhinKubun.TujouNouhin)? "" : ((int)m.NouhinKubun).ToString();
                // �Z�ʉ��i
                dr.YuuduuKakaku = m.YuuduuKakaku.ToString("0000000");
                // �Еt
                dr.Sekiduke = m.Sekizuke.ToString("0000");

                // �E�v
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


            // ���[�Ŋ����t���O���Z�b�g�����i��+�T�C�Y���擾����B
            List<string> lstKannouHinbanSize = new List<string>();
            for (int i = 0; i < lstHinbanSizeJigyousyo.Count; i++)
            {
                string strHinbanSizeJigyousyo = lstHinbanSizeJigyousyo[i];
                string strHinban = strHinbanSizeJigyousyo.Split('\t')[0];
                string strSize = strHinbanSizeJigyousyo.Split('\t')[1];

                cmdGetZansu.Parameters["@Hinban"].Value = strHinban;
                cmdGetZansu.Parameters["@Size"].Value = strSize;

                //�ǉ� 20160920 ���Ə����I������Ă���ꍇ�͂��̎��Ə����܂�Ō������s��
                //if (!string.IsNullOrEmpty(data.ShukkaNiukeJigyoushoCode))
                //{
                //    cmdGetZansu.CommandText += " AND (NiukeJigyoushoCode = @njcode)";
                //    cmdGetZansu.Parameters.AddWithValue("@njcode", data.ShukkaNiukeJigyoushoCode);
                //}
                //else //�����ꍇ�̓R�}���h�������
                //    cmdGetZansu.CommandText = cmdGetZansu.CommandText.Replace(" AND (NiukeJigyoushoCode = @njcode)", "");

                int nZansu = Convert.ToInt32(cmdGetZansu.ExecuteScalar());
                if (0 > nZansu) throw new Exception(strHinban + "�̎c�����}�C�i�X�ł��B");
                if (0 == nZansu) 
                {
                    if (!lstKanryouFlgHinbanSize.Contains(strHinbanSizeJigyousyo))
                        lstKanryouFlgHinbanSize.Add(strHinbanSizeJigyousyo);
                    if (!lstKannouHinbanSize.Contains(strHinbanSizeJigyousyo))
                        lstKannouHinbanSize.Add(strHinbanSizeJigyousyo);
                }
            }
            
            // �Ō�̕i��+�T�C�Y�ɑ΂��Ċ����t���O���Z�b�g�i�{�[�i�f�[�^���ɓ���i�ԃT�C�Y���܂܂��P�[�X���L��ׁj
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
                drLast.KanryouFlg = KanryouFlg.Kanryou; // �Ō�̃��R�[�h�ɑ΂��Đݒ肷��B

                //�\�Z�i�����Ə��I������Ă��Ȃ��󋵂Ȃ犮���t���O��8�ɐݒ� 20160920
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

            // ----- �g���[���[ -----
            MizunoDataSet.T_NouhinTrailerDataTable dtTrailer = new MizunoDataSet.T_NouhinTrailerDataTable();
            MizunoDataSet.T_NouhinTrailerRow drTrailer = dtTrailer.NewT_NouhinTrailerRow();
            NouhinDataClass.InitT_NouhinTrailerRow(drTrailer);

            drTrailer.YYMM = drHeader.YYMM;
            drTrailer.HakkouNo = nHakkouNo;
            //�d����R�[�h
            drTrailer.ShiiresakiCode = strShiiresakiCode;

            // �׎�S���҃R�[�h
            drTrailer.EigyouTantoushaCode = data.EigyouTantoushaCode;
            // �w�Z���A�`�[����
            drTrailer.TeamMei = data.TeamMei;
            // �X��
            drTrailer.TenMei = data.TenMei;
            // �o�א�X�֔ԍ�
            drTrailer.ShukkaSakiYubinBangou = "";

            // �o�א�Z��
            drTrailer.ShukkaSakiJyusho = "";

            // �o�א�TEL
            drTrailer.ShukkaSakiTel = "";

            // ���Y���S���҃R�[�h
            drTrailer.SKTantoushaCode = data.SKTantoushaCode;
            // �^�����@
            drTrailer.UnsouHouhou = "";

            // �`�[���s��            
            drTrailer.NouhinShoHiduke = data.dtHakkouBi.ToString("yyMMdd");

            // �\��(����No)
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


            // ----- �����t���O�̃`�F�b�N -----
            for (int i = 0; i < lstHinbanSizeJigyousyo.Count; i++)
            {
                string strHinbanSizeJigyousyo = lstHinbanSizeJigyousyo[i];
                string strHinban = strHinbanSizeJigyousyo.Split('\t')[0];
                string strSize = strHinbanSizeJigyousyo.Split('\t')[1];
                string strJigyousyo = strHinbanSizeJigyousyo.Split('\t')[2];

                // �����M�f�[�^�擾
                /// 2014/05/26 �g�����U�N�V�������̓f�[�^�x�[�X����̎擾�͕s��
                /// 
                //daGetMisoushin.SelectCommand.Parameters["@Hinban"].Value = strHinban;
                //daGetMisoushin.SelectCommand.Parameters["@Size"].Value = strSize;
                //dtM.Clear();
                //daGetMisoushin.Fill(dtM);
                //if (0 == dtM.Count) continue;

                //DataView dv = new DataView(dtM);

                //dv.Sort = "ShukkaBi, YYMM, HakkouNo, Edaban, GyouNo";  // ���̏��ԂōŌ�̃��R�[�h���ŏI�[�i�f�[�^(�Ō�Ƀ~�Y�m�ɑ��M����f�[�^)�B1�`�[���ɓ���i�ԃT�C�Y���o�����邱�Ƃ�����i���i���قȂ�j�ׁA�}�ԁA�sNo�̏����Ŗ��׏��ōŌ�Ɋ����t���O���Z�b�g�����悤�l�����Ă���B


                DataView dv = new DataView(dtM);
                dv.RowFilter = string.Format("Hinban='{0}' AND Size='{1}'", strHinban, strSize);

                if (0 == dv.Count) continue;

                dv.Sort = "YYMM, HakkouNo, Edaban, GyouNo"; 


                if (lstKannouHinbanSize.Contains(strHinbanSizeJigyousyo))
                {
                    dv.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND NouhinSuu>0", strHinban, strSize);   // �[�i��=0�̓`�[������ׁA�[�i��>0�ōŌ�̖��ׂɊ����t���O���Z�b�g�������B(�`�[�C���Ŕ[�i��=0�̓`�[(����`�[)�̏o�ד����Ō�i�Ō�̓`�[�ɂȂ邱�Ƃ�����j)
                    if (0 == dv.Count)
                        dv.RowFilter = null;    // �[�i��>0�̓`�[�������A�[�i����0�̓`�[�Ɋ����t���O���Z�b�g���Ȃ��Ƃ����Ȃ��B�i��{�I�ɂ��̃P�[�X�͖����͂��j

                    // �S���o�׍ς݂Ȃ̂ŁA�Ō�̖����M�̔[�i�f�[�^�Ɋ����t���O�i9�j���Z�b�g����B
                    for (int k = 0; k < dv.Count; k++)
                    {
                        MizunoDataSet.T_NouhinMeisaiRow dr = dv[k].Row as MizunoDataSet.T_NouhinMeisaiRow;
                        //�ǉ� �\�Z�i�����Ə��I������Ă��Ȃ��󋵂Ȃ犮���t���O��8�ɐݒ� 20160920
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
                    // �o�׎c���������ԂŊ����̏ꍇ�A�������ꂩ�̔[�i�f�[�^�Ɋ����t���O�������Ă���ہA�ŏI�[�i�f�[�^�ɂɊ����t���O�������Ă��邩�`�F�b�N //�ǉ� 20160920
                    dv.RowFilter = string.Format("(KanryouFlg='{0}' OR KanryouFlg='{1}')", KanryouFlg.Kanryou, KanryouFlg.JigyoushoKanryou);
                    if (0 == dv.Count) continue;


                    // �ȉ������t���O�����݂���ꍇ
                    MizunoDataSet.T_NouhinMeisaiRow drKanryouFlg = dv[0].Row as MizunoDataSet.T_NouhinMeisaiRow;
                    dv.RowFilter = null;
                    MizunoDataSet.T_NouhinMeisaiRow drLast = dv[dv.Count - 1].Row as MizunoDataSet.T_NouhinMeisaiRow;
                    if (drLast.YYMM == drKanryouFlg.YYMM && drLast.HakkouNo == drKanryouFlg.HakkouNo)
                    {
                        // �Ō�Ɋ����t���O���Z�b�g����Ă���̂�OK
                    }
                    else
                    {
                        if (drKanryouFlg.YYMM == dk.YYMM && drKanryouFlg.HakkouNo == dk.HakkouNo)
                        {
                            // ���g�Ɋ����t���O�������Ă�����
                            throw new Exception(string.Format("�����t���O��ݒ�ł��܂���B(�i��:{0}, �T�C�Y:{1}�̊����t���O�͓`�[���s�N��{2:yyyy/MM}�A���sNo.{3}�ɐݒ肵�Ă��������B",
                                strHinban, (string.IsNullOrEmpty(strSize)) ? "�Ȃ�" : strSize, 
                                new Core.Type.Nengetu(drLast.YYMM).ToDateTime(), drLast.HakkouNo));
                        }
                        else
                        {
                            // ���̓`�[�Ɋ����t���O�������Ă�����
                            string strMsg = "";
                            if (drLast.YYMM == dk.YYMM && drLast.HakkouNo == dk.HakkouNo)
                                strMsg = "�����t���O�͖{�`�[�Őݒ肷��K�v������܂��B";
                            else
                                strMsg = string.Format("�����t���O�͓`�[���s�N��{0:yyyy/MM}�A���sNo.{1}�ɐݒ肷��K�v������܂��B", 
                                    new Core.Type.Nengetu(drLast.YYMM).ToDateTime(), drLast.HakkouNo);

                            throw new Exception(string.Format("�`�[���s�N��{0:yyyy/MM}�A���sNo.{1}�ŕi��{2}�A�T�C�Y:{3}�̊����t���O���O���ĉ������B{4}",
                                new Core.Type.Nengetu(drKanryouFlg.YYMM).ToDateTime(), drKanryouFlg.HakkouNo, strHinban, (string.IsNullOrEmpty(strSize)) ? "�Ȃ�" : strSize, strMsg));
                        }
                    }
                }

                // ����i�ԁA�������Ə����̃I�[�_�[���A���Ə����w�肵�Ă̏o�ד��͂̏ꍇ�A
                // �����Ə�
                // ���ۂ̗\�Z�i���R�[�h�̏o�א����Z�b�g
                
                daProper.SelectCommand.CommandText = "select * from T_ProperOrder where SashizuBi=@z and SeisanOrderNo=@s and HikitoriOrderNo=@h and YosanTsuki=@y and Hinban=@Hinban and Size=@Size and Suuryou>ShukkaSuu";

                if (!bShusei)
                    daProper.SelectCommand.CommandText += " and KanryouFlg=0";  // �C�����͊����t���O�͍l�����Ȃ��i100�̒�����90001��50�A90002�Ŏc��45�Ŋ����t���O�𓯂��o�ד�8/30�œo�^������A90001�̏o�ד���8/31�ɂ��čŏI�`�[�ɂ������ɖ��ƂȂ����ׁj

                daProper.SelectCommand.Parameters["@Hinban"].Value = strHinban;
                daProper.SelectCommand.Parameters["@Size"].Value = strSize;

                //if (daProper.SelectCommand.Parameters.Contains("@NiukeJigyoushoCode"))
                //    daProper.SelectCommand.Parameters.RemoveAt("@NiukeJigyoushoCode");


                dtProper.Clear();
                daProper.Fill(dtProper);
                
                //���莖�Ə���I�������ꍇ�ł������Ƃ���ׁA�ȉ���for���͔p�~ 20160920
                //for (int k = 0; k < dtProper.DefaultView.Count; k++)
                //{
                //    MizunoDataSet.T_ProperOrderRow drP = dtProper.DefaultView[k].Row as MizunoDataSet.T_ProperOrderRow;

                //    //�C�����͔��ʂ��Ȃ���ǉ� 20160624
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

            // �o�׎c���`�F�b�N(�p�t�H�[�}���X�������̂Ńo�O�̃`�F�b�N���ς񂾂牺�L�R�[�h�͍폜���邱��)

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
            //            // ���f�[�^�̒��c���قŃ`�F�b�N�Ɉ����������Ă��܂��ׁA����o�^�����A�C�e�������`�F�b�N���邱�Ƃɂ����B
            //            if (data.lstMeisai[m].Key.Hinban == dr.Hinban && data.lstMeisai[m].Key.Size == dr.Size)
            //            { 
            //                lstError.Add(string.Format("�i��{0}�A�T�C�Y{1}�ŏo�א��̍��ق�����܂��B�o�א�={2}, �[�i��={3}",
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

            // ����
            SqlDataAdapter daM = new SqlDataAdapter("", c);
            daM.SelectCommand.CommandText = @"
SELECT                  dbo.T_NouhinMeisai.*
FROM                     dbo.T_NouhinMeisai
WHERE                   (ShiiresakiCode = @s) AND (YYMM = @y) AND (HakkouNo = @h)";

            daM.SelectCommand.Parameters.AddWithValue("@h", key.HakkouNo);
            daM.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            daM.SelectCommand.Parameters.AddWithValue("@y", key.YYMM);
            daM.UpdateCommand = new SqlCommandBuilder(daM).GetUpdateCommand();


            // �o�׈������ď��
            SqlDataAdapter daHikiate = new SqlDataAdapter("", c);
            daHikiate.SelectCommand.CommandText = @"
SELECT                  dbo.T_ProperOrdrer_NouhinHikiate.*
FROM                     dbo.T_ProperOrdrer_NouhinHikiate
WHERE                   (HakkouNo = @h) AND (YYMM = @y) AND (ShiiresakiCode = @s)";
            daHikiate.SelectCommand.Parameters.AddWithValue("@h", key.HakkouNo);
            daHikiate.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            daHikiate.SelectCommand.Parameters.AddWithValue("@y", key.YYMM);



            // �i��+�T�C�Y�P�ʂŎ擾���[�i�L���𒊏o�����Ƃ��Ă������A�P�����ɕ����̔[�i�L�����o�����邱�Ƃ͖����̂ŁA��������O�����B
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



            // �����t���O�̃��Z�b�g(�\�Z�� + �i�� + �T�C�Y�P��)
            SqlCommand cmdSetKanryouFlgOff = new SqlCommand("", c);
            cmdSetKanryouFlgOff.CommandText = "update T_ProperOrder set KanryouFlg=0 where SashizuBi=@z and SeisanOrderNo=@s and HikitoriOrderNo=@h and YosanTsuki=@y and Hinban=@Hinban and Size=@Size";


            // �֘A�f�[�^�폜
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


            // �[�i���� �폜
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
                    return new Core.Error("�Y���̃f�[�^������܂���B");
                MizunoDataSet.T_NouhinHeaderRow drHeader = dtHeader[0];
                if (drHeader.SoushinFlg == SoushinFlg.SOUSHINZUMI)
                    return new Core.Error("���M�ς݂̈׏C���ł��܂���B");


                daProper.SelectCommand.Parameters["@z"].Value = drHeader.SashizuBi;
                daProper.SelectCommand.Parameters["@s"].Value = drHeader.SeisanOrderNo;
                daProper.SelectCommand.Parameters["@h"].Value = drHeader.HikitoriOrderNo;
                daProper.SelectCommand.Parameters["@y"].Value = int.Parse(drHeader.YosanTsuki);
                //daProper.SelectCommand.Parameters["@n"].Value = drHeader.OrderNouhinKigou;  // drHeader.NouhinKigou�łȂ��_�ɒ���(�[�i�L����1������1�ł��邪�A�o�דo�^���A���[�U���[�i�L����ύX���ēo�^�ł���BdrHeader.OrderNouhinKigou�͒����f�[�^���̔[�i�L���ł���B)������o�׃A�C�e���Ŕ[�i�L���������o�����邱�Ƃ͖����̂ō폜

              
                MizunoDataSet.T_NouhinMeisaiDataTable dtM = new MizunoDataSet.T_NouhinMeisaiDataTable();
                List<string> lstKanryouFlgHinbanSize = new List<string>();  // �����t���O���Z�b�g����Ă���i�ԃT�C�Y
                List<string> lstKannouHinbanSize = new List<string>();  // ���[���Ă���i��+�T�C�Y

                MizunoDataSet.T_ProperOrderDataTable dtProper = new MizunoDataSet.T_ProperOrderDataTable();

                daM.Fill(dtM);

                // �������ď��
                MizunoDataSet.T_ProperOrdrer_NouhinHikiateDataTable dtHikiate = new MizunoDataSet.T_ProperOrdrer_NouhinHikiateDataTable();
                daHikiate.Fill(dtHikiate);

                for (int i = 0; i < dtM.Count; i++) {
                    MizunoDataSet.T_NouhinMeisaiRow dr = dtM[i];
                    
                    if (dr.HinbanTsuikaFlg == HinbanTsuikaFlg.Tsuika) continue;
                    if (0 > dr.RowNo) continue;   // ������ǉ��i�Ԃ��邢�̓_�~�[���R�[�h�̏���

                    string strHinbanSize = string.Format("{0}\t{1}", dr.Hinban, dr.Size);
                    //���Ə������t���O��ǉ� 20160920
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
                        // �����Ńf�[�^���Ȃ��̂͂�������
                        if (0 == dtProper.Count) { 
                            throw new Exception(string.Format("�i��{0}�A�T�C�Y{2}�̒����f�[�^������܂���B", dr.Hinban, dr.Size));
                        }
                        int nChumonSu = Convert.ToInt32(dtProper.Compute("SUM(Suuryou)", null));
                        int nShukkaSu = Convert.ToInt32(dtProper.Compute("SUM(ShukkaSuu)", null));
                        if (nChumonSu == nShukkaSu) lstKannouHinbanSize.Add(strHinbanSize); // ���[���Ă����i��+�T�C�Y
                    }

                    if (0 == dtHikiate.Count)
                    {
                        // �������ď�񂪖�����

                        daProper.SelectCommand.CommandText = "select * from T_ProperOrder where SashizuBi=@z and SeisanOrderNo=@s and HikitoriOrderNo=@h and YosanTsuki=@y and Hinban=@Hinban and Size=@Size";
                        daProper.SelectCommand.Parameters["@Hinban"].Value = dr.Hinban;
                        daProper.SelectCommand.Parameters["@Size"].Value = dr.Size;


                        dtProper.Clear();
                        daProper.Fill(dtProper);
                        DataView dv = new DataView(dtProper);
                        // dv.RowFilter = "ShukkaSuu>0";�@����ʖځA���[�v����ShukkaSuu���X�V����̂�NG
                        dv.Sort = "RowNo DESC"; // �o�^���̈������Ă�RowNo�̎Ⴂ���̂���������Ă�̂ō��͋t
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
                            // ���ɖ߂��Ȃ�
                            throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�̏o�א������Z�b�g�ł��܂���ł����B���ِ�={2}��", dr.Hinban, dr.Size, nShukkaZumiSu));
                        }

                        daProper.Update(dtProper);
                    }
                }


                if (0 < dtHikiate.Count) 
                {
                    // �������ď�񂪗L��ꍇ�͂��̏������Ƃɏo�א��̌��c���s���i�����f�[�^�����[���Ă��܂��Έ������ăf�[�^�͍폜���Ăn�j�j
                    daProperGetRow.SelectCommand.Parameters["@z"].Value = drHeader.SashizuBi;
                    daProperGetRow.SelectCommand.Parameters["@s"].Value = drHeader.SeisanOrderNo;
                    daProperGetRow.SelectCommand.Parameters["@h"].Value = drHeader.HikitoriOrderNo;

                    for (int i = 0; i < dtHikiate.Count; i++) {
                        daProperGetRow.SelectCommand.Parameters["@r"].Value = dtHikiate[i].RowNo;
                        dtProper.Clear();
                        daProperGetRow.Fill(dtProper);
                        if (0 == dtProper.Count)
                            throw new Exception(string.Format("RowNo={0}�̒����f�[�^������܂���B", dtHikiate[i].RowNo));
                        dtProper[0].ShukkaSuu -= dtHikiate[i].HikiateSu;
                        if (0 > dtProper[0].ShukkaSuu)
                            throw new Exception(string.Format("RowNo={0}�̒����f�[�^�ň������Č�̏o�א����}�C�i�X�ł��B", dtHikiate[i].RowNo));
                        daProperGetRow.Update(dtProper);
                    }
                }


           
                // �����t���O�̃I�t�i�[�i�f�[�^�Ɋ����t���O�������Ă����i��+�T�C�Y�̃t���O�𗎂Ƃ��j
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

                // �����t���O�̗����Ă���ʂ̖����M�[�i�f�[�^�Ŋ��[���Ă�����̂́A���̓`�[�ł����Ă������t���O���I�t�ɂ���(�ȉ��̗���Q��)
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
                //���Ə������t���O��ǉ� 20160920
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

                    // ���[�i�f�[�^�Ŋ����t���O�������Ă���ꍇ������B
                    // �Ⴆ�΁A���钍���̕i�ԃT�C�Y�̒������F100��
                    // �@���sNO.90001��60�o��
                    // �A���sNO.90002��40�o�ׁi�����ł��̔[�i�f�[�^�Ɋ����t���O�����j
                    // �B���sNO.90001��60��50�ɕύX�iT_ProperOrder�̊����t���O�̓I�t�ɂȂ邪�A90002�̔[�i�f�[�^�Ɋ����t���O���c���Ă��܂��j
                    // ���{���A�����t���O��T_Proper�Ɣ[�i�f�[�^�̗����ێ����Ă���_�����i�s�������N����j�ŁA�����t���O�͓Ɨ����ĕێ����ׂ�
                    dtM.Clear();
                    daGetOtherNouhinMeisai.SelectCommand.Parameters["@Hinban"].Value = strHinban;
                    daGetOtherNouhinMeisai.SelectCommand.Parameters["@Size"].Value = strSize;
                    daGetOtherNouhinMeisai.Fill(dtM);
                    for (int m = 0; m < dtM.Count; m++)
                        dtM[m].KanryouFlg = KanryouFlg.MiKanryou;   // ���R�P�����Ȃ��͂�����
                    daM.Update(dtM);
                }

                int nHakkouNo = (int.Parse(data.dtHakkouBi.ToString("yyMM")) == drHeader.YYMM)? drHeader.HakkouNo : 0;  // YYMM���ς��Δ��sNo�͍ēx�̔�
                ProperOrderKey pk = new ProperOrderKey(drHeader.SashizuBi, drHeader.SeisanOrderNo, drHeader.HikitoriOrderNo);

                // �[�i�f�[�^���폜
                cmdDeleteNouhinData.ExecuteScalar();

                newKey = DenpyouTouroku(true, nHakkouNo, pk, new Core.Type.Nengetu(int.Parse(drHeader.YosanTsuki)), drHeader.OrderNouhinKigou, data, t);

                // 2013/03/22 ����
                // ����FLG=False AND �o�א�=0 �̃f�[�^�͍폜����B(�`�[�֕\���������Ȃ�)
                cmdDeleteNouhinData2.ExecuteScalar();

                // 2013/04/02 ����
                // ��L�폜�����ɂ�� T_NouhinMeisai.GyouNo �ɃY������������ꍇ������̂ŁA�U�蒼��
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
            public string strNiukeJigyoushoCode = null; // �󔒂��L�蓾��
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
        /// ������t����o�^
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
        
        //�\�Z�i�ꊇ��t
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
            da.SelectCommand.CommandText = "select * from T_ProperOrder where (SeisanOrderNo=@o or HikitoriOrderNo=@o) and ShiiresakiCode=@s AND Hokanbasho != '' AND NiukeJigyoushoCode = @j"; //��Hokanbasho��ǉ� 20160626 JigyoushoCode��ǉ� 20170623
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














