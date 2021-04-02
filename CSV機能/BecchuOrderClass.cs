using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MizunoDAL;

namespace MizunoDAL
{
    public class BecchuOrderClass
    {
        public enum EnumBecchuKubun
        {
            None,
            Becchu,
            Becchu2,
            DS, // �_�C�A�����h�X�^�[
            SP  // �X�y�N�g��
        }

        public static string sKyuHacchuPrefix = "-OLD";

        /// <summary>
        /// �ʒ��i�L�����Z���f�[�^NewRow��Ԃ�
        /// </summary>
        /// <returns></returns>
        public static MizunoDataSet.T_BecchuCancelRow
            newT_BecchuCancelRow()
        {
            return new MizunoDataSet.T_BecchuCancelDataTable().NewT_BecchuCancelRow();
        }

        /// <summary>
        /// �L�[���̔��l��NewRow��Ԃ�
        /// </summary>
        /// <returns></returns>
        public static MizunoDataSet.T_BecchuBikouRow
            newT_BecchuBikouRow()
        {
            return new MizunoDataSet.T_BecchuBikouDataTable().NewT_BecchuBikouRow();
        }

        /// <summary>
        /// �ʒ��i�̎�L�[
        /// </summary>
        [Serializable]
        public class BecchuOrderKey
        {
            public BecchuOrderKey()
            {
            }
            public BecchuOrderKey(string strUketsukeBi, string strOrderKanriNo, string strShiiresakiCode)
            {
                this.UketsukeBi = strUketsukeBi;
                this.OrderKanriNo = strOrderKanriNo;
                this.ShiiresakiCode = strShiiresakiCode;
            }
            public BecchuOrderKey(string strKeyAry)
            {
                string[] strAry = strKeyAry.Split('_');
                this.UketsukeBi = strAry[0];
                this.OrderKanriNo = strAry[1];
                this.ShiiresakiCode = strAry[2];
            }
            public override string ToString()
            {
                return this.UketsukeBi + "_" + this.OrderKanriNo + "_" + this.ShiiresakiCode;
            }
            public string UketsukeBi
            {
                get;
                set;
            }
            public string OrderKanriNo
            {
                get;
                set;
            }
            public string ShiiresakiCode
            {
                get;
                set;
            }
        }

        //�}�[�N�Ή��ǉ� 20151119
        public class MarkKey
        {
            public MarkKey()
            {
            }
            public MarkKey(string strSashizuBi, string strSashizuNo)
            {
                this.SashizuBi = strSashizuBi;
                this.SashizuNo = strSashizuNo;
            }
            public MarkKey(string strKeyAry)
            {
                string[] strAry = strKeyAry.Split('_');
                this.SashizuBi = strAry[0];
                this.SashizuNo = strAry[1];
            }
            public override string ToString()
            {
                {
                    return this.SashizuBi + "_" + this.SashizuNo;
                }
            }
            public string SashizuBi
            {
                get;
                set;
            }
            public string SashizuNo
            {
                get;
                set;
            }
        }




        /// <summary>
        /// ��������
        /// </summary>
        public class KensakuParam
        {
            // �\���\����
            public string _HyoujiKikan = "";
            // �[�����F��(�S�� = 0�A�����F = 1�A���F�ς� = 2)
            public byte _NoukiShounin = 0;
            // ���[�U�[�敪(�~�Y�m = 1�A���Ӑ� = 2�A�d���� = 3)
            //public byte _userkubun = 0;
            // ��ږ�
            public string _ShumokuCode = "";
            // �i��
            public string _Hinban = "";

            public Core.Type.NengappiKikan objMizunoUketukeBi = null;
            public Core.Type.NengappiKikan objKoujouShukkaYoteiBi = null;

            // �I�[�_�[�Ǘ�No
            public string _OrderKanriNo = "";
            // ��(���o�� = 0 /�o�׍ς� = 1 / �L�����Z�� = 2 /�S�� = 3)
            public byte _KanryouFlg = 3;
            // �ʒ��T�C�g�I�[�_�[No
            public string _BecchuSiteOrderNo = "";
            // �`�[����
            public string _TeamMei = "";
            // ���q�l��
            public string _OkyakusamaMei = "";
            // �}�[�N���H�L��
            public string _MarkKakou = "";
            // �n��
            public string _Chiku = "";
            // �c�ƒS����(�~�Y�m�S����)
            public string _EigyouTantousha = "";
            // �[�i�L��
            public string _NouhinKigou = "";
            // �d���於
            public string _ShiiresakiMei = "";
            // ���Y�Ǘ��S����
            public string _SKTantousha = "";

            // ���b�Z�[�W
            public byte _Msg = 0;
            // ��ЃR�[�h(���b�Z�[�W�����������Ɋ܂܂�Ă����ꍇ�̂ݎg�p����)
            public string _KaishaCode = "";

            // �d����R�[�h
            public string _SCode = "";
            // ���Ӑ�R�[�h
            public string _TCode = "";

            // �d����R�[�h(�K�w������׎g�p)
            public string[] _SCodeAry = null;
            // ���Ӑ�R�[�h(�K�w������׎g�p)
            public string[] _TCodeAry = null;

            // �\�[�g��
            public System.Collections.ArrayList _ColAry = new System.Collections.ArrayList();
            // �\�[�g�����~��
            public System.Collections.ArrayList _AscAry = new System.Collections.ArrayList();
        }




        /// <summary>
        /// SP�f�[�^1�s�擾
        /// </summary>
        /// <param name="OrderKanriNo"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static MizunoDataSet.T_SPRow
            getT_SPRow(string strOrderKanriNo, string strShiireCode, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText =
                "SELECT *  FROM  T_BecchuKeyInfo INNER JOIN T_SP ON T_BecchuKeyInfo.OrderKanriNo = T_SP.OrderKanriNo WHERE T_BecchuKeyInfo.OrderKanriNo = @OrderKanriNo AND T_BecchuKeyInfo.ShiiresakiCode = @ShiiresakiCode";

            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", strOrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", strShiireCode);

            MizunoDataSet.T_SPDataTable dt = new MizunoDataSet.T_SPDataTable();
            da.Fill(dt);
            if (1 == dt.Rows.Count)
                return (MizunoDataSet.T_SPRow)dt.Rows[0];
            else
                return null;
        }

        /// <summary>
        /// SP2�f�[�^1�s�擾 20200106�ǉ�
        /// </summary>
        /// <param name="OrderKanriNo"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static MizunoDataSet.T_SP2Row
            getT_SP2Row(string strOrderKanriNo, string strShiireCode, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText =
                "SELECT *  FROM  T_BecchuKeyInfo INNER JOIN T_SP2 ON T_BecchuKeyInfo.OrderKanriNo = T_SP2.OrderKanriNo WHERE T_BecchuKeyInfo.OrderKanriNo = @OrderKanriNo AND T_BecchuKeyInfo.ShiiresakiCode = @ShiiresakiCode";

            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", strOrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", strShiireCode);

            MizunoDataSet.T_SP2DataTable dt = new MizunoDataSet.T_SP2DataTable();
            da.Fill(dt);
            if (1 == dt.Rows.Count)
                return (MizunoDataSet.T_SP2Row)dt.Rows[0];
            else
                return null;
        }

        /// <summary>
        /// SP3�f�[�^1�s�擾 20200106�ǉ�
        /// </summary>
        /// <param name="OrderKanriNo"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static MizunoDataSet.T_SP3Row
            getT_SP3Row(string strOrderKanriNo, string strShiireCode, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText =
                "SELECT *  FROM  T_BecchuKeyInfo INNER JOIN T_SP3 ON T_BecchuKeyInfo.OrderKanriNo = T_SP3.OrderKanriNo WHERE T_BecchuKeyInfo.OrderKanriNo = @OrderKanriNo AND T_BecchuKeyInfo.ShiiresakiCode = @ShiiresakiCode";

            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", strOrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", strShiireCode);

            MizunoDataSet.T_SP3DataTable dt = new MizunoDataSet.T_SP3DataTable();
            da.Fill(dt);
            if (1 == dt.Rows.Count)
                return (MizunoDataSet.T_SP3Row)dt.Rows[0];
            else
                return null;
        }

        /// <summary>
        /// SP4�f�[�^1�s�擾 20200428�ǉ�
        /// </summary>
        /// <param name="OrderKanriNo"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static MizunoDataSet.T_SP4Row
            getT_SP4Row(string strOrderKanriNo, string strShiireCode, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText =
                "SELECT *  FROM  T_BecchuKeyInfo INNER JOIN T_SP4 ON T_BecchuKeyInfo.OrderKanriNo = T_SP4.OrderKanriNo WHERE T_BecchuKeyInfo.OrderKanriNo = @OrderKanriNo AND T_BecchuKeyInfo.ShiiresakiCode = @ShiiresakiCode";

            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", strOrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", strShiireCode);

            MizunoDataSet.T_SP4DataTable dt = new MizunoDataSet.T_SP4DataTable();
            da.Fill(dt);
            if (1 == dt.Rows.Count)
                return (MizunoDataSet.T_SP4Row)dt.Rows[0];
            else
                return null;
        }

        public static MizunoDataSet.T_SPRow
            getT_SPRow(string strOrderKanriNo, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "select * from T_SP where OrderKanriNo=@OrderKanriNo";

            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", strOrderKanriNo);

            MizunoDataSet.T_SPDataTable dt = new MizunoDataSet.T_SPDataTable();
            da.Fill(dt);
            if (1 == dt.Rows.Count)
                return dt[0];
            else
                return null;
        }

        /// <summary>
        /// DS�f�[�^1�s�擾
        /// </summary>
        /// <param name="OrderKanriNo"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static MizunoDataSet.T_DSRow
            getT_DSRow(string OrderKanriNo, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT * FROM T_DS WHERE OrderKanriNo = @OrderKanriNo";
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", OrderKanriNo);
            MizunoDataSet.T_DSDataTable dt = new MizunoDataSet.T_DSDataTable();
            da.Fill(dt);
            if (1 == dt.Rows.Count)
                return (MizunoDataSet.T_DSRow)dt.Rows[0];
            else
                return null;
        }

        /// <summary>
        /// DS2�f�[�^1�s�擾
        /// </summary>
        /// <param name="OrderKanriNo"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static MizunoDataSet.T_DS2Row
            getT_DS2Row(string OrderKanriNo, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT * FROM T_DS2 WHERE OrderKanriNo = @OrderKanriNo";
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", OrderKanriNo);
            MizunoDataSet.T_DS2DataTable dt = new MizunoDataSet.T_DS2DataTable();
            da.Fill(dt);
            if (1 == dt.Rows.Count)
                return (MizunoDataSet.T_DS2Row)dt.Rows[0];
            else
                return null;
        }


        //---2012-10-18 ꎓ��ǉ�
        public static MizunoDataSet.T_DS2Row get_DSData(string OrderKanriNo, SqlConnection sql)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText = "SELECT * FROM T_DS2 WHERE OrderKanriNo = @OrderKanriNo";
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", OrderKanriNo);
            MizunoDataSet.T_DS2DataTable dt = new MizunoDataSet.T_DS2DataTable();
            da.Fill(dt);
            if (1 == dt.Rows.Count)
                return (MizunoDataSet.T_DS2Row)dt.Rows[0];
            else
                return null;
        }


        public static MizunoDataSet.T_BecchuRow
            getT_BecchuRow(BecchuOrderKey key, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT * FROM T_Becchu WHERE ShiiresakiCode = @s and SashizuBi=@z and SashizuNo=@n";
            da.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);

            MizunoDataSet.T_BecchuDataTable dt = new MizunoDataSet.T_BecchuDataTable();
            da.Fill(dt);
            if (1 == dt.Rows.Count)
                return dt[0];
            else
                return null;
        }

        public static MizunoDataSet.T_Becchu2Row
            getT_Becchu2Row(BecchuOrderKey key, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT * FROM T_Becchu2 WHERE ShiiresakiCode = @s and SashizuBi=@z and SashizuNo=@n";
            da.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);

            MizunoDataSet.T_Becchu2DataTable dt = new MizunoDataSet.T_Becchu2DataTable();
            da.Fill(dt);
            if (1 == dt.Rows.Count)
                return dt[0];
            else
                return null;
        }


        /// <summary>
        /// �ʒ��i���L�����Z������
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static Core.Error BecchuOrderCancel(BecchuOrderKey key, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT * FROM T_BecchuCancel";
            da.InsertCommand = (new SqlCommandBuilder(da)).GetInsertCommand();
            MizunoDataSet.T_BecchuCancelDataTable dt = new MizunoDataSet.T_BecchuCancelDataTable();
            MizunoDataSet.T_BecchuCancelRow drNew = dt.NewT_BecchuCancelRow();
            try
            {
                drNew.MizunoUketsukeBi = key.UketsukeBi;
                drNew.OrderKanriNo = key.OrderKanriNo;
                drNew.CancelBi = DateTime.Today.ToString("yyyy/MM/dd");
                dt.Rows.Add(drNew);
                da.Update(dt);
                return null;
            }
            catch (Exception e)
            {
                return new Core.Error(e);
            }
        }

        /// <summary>
        /// �L�����Z���f�[�^���擾����
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static bool IsCancel
            (string strMizunoUketsukeBi, string strOrderKanriNo, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_BecchuCancel WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND "
                + "OrderKanriNo = @OrderKanriNo";

            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", strMizunoUketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", strOrderKanriNo);

            MizunoDataSet.T_BecchuCancelDataTable dt = new MizunoDataSet.T_BecchuCancelDataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// �L�����Z���f�[�^���擾����
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static MizunoDataSet.T_BecchuCancelRow getT_BecchuCancelRow
            (string strMizunoUketsukeBi, string strOrderKanriNo, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_BecchuCancel WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND "
                + "OrderKanriNo = @OrderKanriNo";

            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", strMizunoUketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", strOrderKanriNo);

            MizunoDataSet.T_BecchuCancelDataTable dt = new MizunoDataSet.T_BecchuCancelDataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return (MizunoDataSet.T_BecchuCancelRow)dt.Rows[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// �d�����v���i��o�^����
        /// </summary>
        /// <param name="MizunoUketsukeBi"></param>
        /// <param name="OrderKanriNo"></param>
        /// <param name="ShiiresakiCode"></param>
        /// <param name="nGoukeiKakaku"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static LibError T_BecchuKeyInfo_Update_Kakaku(string MizunoUketsukeBi, string OrderKanriNo,
            string ShiiresakiCode, int nGoukeiKakaku, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT * FROM T_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", MizunoUketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", ShiiresakiCode);
            da.UpdateCommand = (new SqlCommandBuilder(da)).GetUpdateCommand();
            MizunoDataSet.T_BecchuKeyInfoDataTable dt = new MizunoDataSet.T_BecchuKeyInfoDataTable();
            da.Fill(dt);
            if (1 != dt.Rows.Count)
                return new LibError();
            MizunoDataSet.T_BecchuKeyInfoRow drThis = (MizunoDataSet.T_BecchuKeyInfoRow)dt.Rows[0];
            try
            {
                drThis.GoukeiKakaku = nGoukeiKakaku;
                da.Update(dt);
                return null;
            }
            catch (Exception e)
            {
                return new LibError(e);
            }
        }

        /// <summary>
        /// �d�����v���i��o�^���� �d����p
        /// </summary>
        /// <param name="MizunoUketsukeBi"></param>
        /// <param name="OrderKanriNo"></param>
        /// <param name="ShiiresakiCode"></param>
        /// <param name="nGoukeiKakaku"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static LibError T_BecchuKeyInfo_Update_Kakaku_S(string MizunoUketsukeBi, string OrderKanriNo,
            string ShiiresakiCode, int nGoukeiKakaku, string loginID, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT * FROM T_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND "
                + "OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", MizunoUketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", ShiiresakiCode);
            da.UpdateCommand = (new SqlCommandBuilder(da)).GetUpdateCommand();
            MizunoDataSet.T_BecchuKeyInfoDataTable dt = new MizunoDataSet.T_BecchuKeyInfoDataTable();
            da.Fill(dt);
            if (1 != dt.Rows.Count)
                return new LibError();
            MizunoDataSet.T_BecchuKeyInfoRow drThis = (MizunoDataSet.T_BecchuKeyInfoRow)dt.Rows[0];
            try
            {
                drThis.GoukeiKakaku = nGoukeiKakaku;
                drThis.KakakuInputID = loginID;
                da.Update(dt);
                return null;
            }
            catch (Exception e)
            {
                return new LibError(e);
            }
        }



        /// <summary>
        /// ���i���ח\�����o�^����
        /// </summary>
        /// <param name="MizunoUketsukeBi"></param>
        /// <param name="OrderKanriNo"></param>
        /// <param name="ShiiresakiCode"></param>
        /// <param name="nGoukeiKakaku"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static LibError T_BecchuKeyInfo_Update_NyukaYoteiBi(BecchuOrderKey key, DateTime dtNyukaYoteiBi, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT * FROM T_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND "
                + "OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", key.UketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", key.OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", key.ShiiresakiCode);
            da.UpdateCommand = (new SqlCommandBuilder(da)).GetUpdateCommand();
            MizunoDataSet.T_BecchuKeyInfoDataTable dt = new MizunoDataSet.T_BecchuKeyInfoDataTable();
            da.Fill(dt);
            if (1 != dt.Rows.Count)
                return new LibError();
            MizunoDataSet.T_BecchuKeyInfoRow drThis = (MizunoDataSet.T_BecchuKeyInfoRow)dt.Rows[0];
            try
            {
                drThis.ShouhinNyukaYoteiBi = dtNyukaYoteiBi.ToString("yyyy/MM/dd");
                da.Update(dt);
                return null;
            }
            catch (Exception e)
            {
                return new LibError(e);
            }
        }

        /// <summary>
        /// �����No��o�^����
        /// </summary>
        /// <param name="MizunoUketsukeBi"></param>
        /// <param name="OrderKanriNo"></param>
        /// <param name="ShiiresakiCode"></param>
        /// <param name="nGoukeiKakaku"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static LibError T_BecchuKeyInfo_Update_OkuriJyouNo(string MizunoUketsukeBi, string OrderKanriNo,
            string ShiiresakiCode, string OkuriJyouNo, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT * FROM T_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND "
                + "OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", MizunoUketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", ShiiresakiCode);
            da.UpdateCommand = (new SqlCommandBuilder(da)).GetUpdateCommand();
            MizunoDataSet.T_BecchuKeyInfoDataTable dt = new MizunoDataSet.T_BecchuKeyInfoDataTable();
            da.Fill(dt);
            if (1 != dt.Rows.Count)
                return new LibError();
            MizunoDataSet.T_BecchuKeyInfoRow drThis = (MizunoDataSet.T_BecchuKeyInfoRow)dt.Rows[0];
            try
            {
                drThis.OkuriJyouNo = OkuriJyouNo;
                da.Update(dt);
                return null;
            }
            catch (Exception e)
            {
                return new LibError(e);
            }
        }


        /// <summary>
        /// �^���ƎҖ���o�^����
        /// </summary>
        /// <param name="MizunoUketsukeBi"></param>
        /// <param name="OrderKanriNo"></param>
        /// <param name="ShiiresakiCode"></param>
        /// <param name="nGoukeiKakaku"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static LibError T_BecchuKeyInfo_Update_UnsouGyoushaMei(string MizunoUketsukeBi, string OrderKanriNo,
            string ShiiresakiCode, string UnsouGyoushaMei, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND "
                + "OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", MizunoUketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", ShiiresakiCode);
            da.UpdateCommand = (new SqlCommandBuilder(da)).GetUpdateCommand();
            MizunoDataSet.T_BecchuKeyInfoDataTable dt = new MizunoDataSet.T_BecchuKeyInfoDataTable();
            da.Fill(dt);
            if (1 != dt.Rows.Count)
                return new LibError();
            MizunoDataSet.T_BecchuKeyInfoRow drThis = (MizunoDataSet.T_BecchuKeyInfoRow)dt.Rows[0];
            try
            {
                drThis.UnsouGyoushaMei = UnsouGyoushaMei;
                da.Update(dt);
                return null;
            }
            catch (Exception e)
            {
                return new LibError(e);
            }
        }

        /// <summary>
        /// �����t���O��True�ɍX�V
        /// </summary>
        /// <param name="MizunoUketsukeBi"></param>
        /// <param name="OrderKanriNo"></param>
        /// <param name="ShiiresakiCode"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static LibError T_BecchuKeyInfo_Update_KanryouFlg(string MizunoUketsukeBi, string OrderKanriNo,
            string ShiiresakiCode, bool bKanryou, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND "
                + "OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", MizunoUketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", ShiiresakiCode);
            da.UpdateCommand = (new SqlCommandBuilder(da)).GetUpdateCommand();
            MizunoDataSet.T_BecchuKeyInfoDataTable dt = new MizunoDataSet.T_BecchuKeyInfoDataTable();
            da.Fill(dt);
            if (1 != dt.Rows.Count)
                return new LibError();
            MizunoDataSet.T_BecchuKeyInfoRow drThis = (MizunoDataSet.T_BecchuKeyInfoRow)dt.Rows[0];
            try
            {
                drThis.KanryouFlg = bKanryou;
                da.Update(dt);
                return null;
            }
            catch (Exception e)
            {
                return new LibError(e);
            }
        }



        /// <summary>
        /// �ʒ��i�̏o�׃f�[�^���擾����
        /// </summary>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static MizunoDataSet.T_BecchuShukkaInfoDataTable getT_BecchuShukkaInfoDataTable
            (string MizunoUketsukeBi, string OrderKanriNo, string strShiiresakiCode, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT * FROM T_BecchuShukkaInfo WHERE (SashizuBi = @MizunoUketsukeBi) AND "
                + "(SashizuNo = @OrderKanriNo) AND (ShiiresakiCode = @ShiiresakiCode)";

            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", MizunoUketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", strShiiresakiCode);

            MizunoDataSet.T_BecchuShukkaInfoDataTable dt = new MizunoDataSet.T_BecchuShukkaInfoDataTable();
            da.Fill(dt);
            return dt;
        }

        /// <summary>
        /// �ʒ��i�̏o�׃f�[�^���擾����(�i�ԁA�T�C�Y�����t��)
        /// </summary>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static MizunoDataSet.T_BecchuShukkaInfoDataTable getT_BecchuShukkaInfoDataTable
            (string MizunoUketsukeBi, string OrderKanriNo, string strShiiresakiCode,
            string strHinban, string strSize, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_BecchuShukkaInfo WHERE (SashizuBi = @MizunoUketsukeBi) AND "
                + "(SashizuNo = @OrderKanriNo) AND (ShiiresakiCode = @ShiiresakiCode) AND "
                + "(Hinban = @Hinban) AND (Size = @Size)";

            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", MizunoUketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", strShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@Hinban", strHinban);
            da.SelectCommand.Parameters.AddWithValue("@Size", strSize);

            MizunoDataSet.T_BecchuShukkaInfoDataTable dt = new MizunoDataSet.T_BecchuShukkaInfoDataTable();
            da.Fill(dt);
            return dt;
        }




        /// <summary>
        /// �e�L�[���ɑ΂�����l��o�^����
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static Core.Error SaveBikou(BecchuOrderKey key, EnumUserType type, string strSeisanKubun, string strBikou, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"SELECT * FROM T_BecchuBikou WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND 
            ShiiresakiCode = @ShiiresakiCode AND UserKubun = @UserKubun AND SeisanKubun = @SeisanKubun";

            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", key.UketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", key.OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", key.ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@UserKubun", (byte)type);

            if (type == EnumUserType.Mizuno)
                da.SelectCommand.Parameters.AddWithValue("@SeisanKubun", strSeisanKubun);
            else
                da.SelectCommand.Parameters.AddWithValue("@SeisanKubun", "");

            da.InsertCommand = (new SqlCommandBuilder(da)).GetInsertCommand();
            da.UpdateCommand = (new SqlCommandBuilder(da)).GetUpdateCommand();

            try
            {
                sqlConn.Open();

                MizunoDataSet.T_BecchuBikouDataTable dt = new MizunoDataSet.T_BecchuBikouDataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    // �}��
                    MizunoDataSet.T_BecchuBikouRow drNew = dt.NewT_BecchuBikouRow();
                    drNew.MizunoUketsukeBi = key.UketsukeBi;
                    drNew.OrderKanriNo = key.OrderKanriNo;
                    drNew.ShiiresakiCode = key.ShiiresakiCode;
                    drNew.UserKubun = (byte)type;
                    if (type == EnumUserType.Mizuno)
                        drNew.SeisanKubun = strSeisanKubun;
                    else
                        drNew.SeisanKubun = "";
                    drNew.Bikou = strBikou;
                    dt.Rows.Add(drNew);
                }
                else if (dt.Rows.Count == 1)
                {
                    // �X�V
                    MizunoDataSet.T_BecchuBikouRow drThis = (MizunoDataSet.T_BecchuBikouRow)dt.Rows[0];
                    drThis.Bikou = strBikou;
                }
                else
                {
                    return new Core.Error("�G���[");
                }
                da.Update(dt);
                return null;
            }
            catch (Exception e)
            {
                return new LibError(e);
            }
            finally
            {
                sqlConn.Close();
            }
        }

        /*
         /// <summary>
        /// �m�F����o�^����
        /// </summary>
        /// <param name="MizunoUketsukeBi"></param>
        /// <param name="OrderKanriNo"></param>
        /// <param name="ShiiresakiCode"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static LibError T_BecchuKeyInfo_Update_KakuninBi
            (string MizunoUketsukeBi, string OrderKanriNo, string ShiiresakiCode, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = 
                "SELECT * FROM T_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND "
                + "OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", MizunoUketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", ShiiresakiCode);
            da.UpdateCommand = (new SqlCommandBuilder(da)).GetUpdateCommand();
            MizunoDataSet.T_BecchuKeyInfoDataTable dt = new MizunoDataSet.T_BecchuKeyInfoDataTable();
            da.Fill(dt);
            if (1 != dt.Rows.Count)
                return new LibError("�G���[");
            MizunoDataSet.T_BecchuKeyInfoRow drThis = (MizunoDataSet.T_BecchuKeyInfoRow)dt.Rows[0];
            try
            {
                // �m�F�������o�^�̏ꍇ
                if (drThis.IsKakuninBiNull())
                {                    
                    drThis.KakuninBi = DateTime.Now;
                    da.Update(dt);
                }                
                return null;
            }
            catch (Exception e)
            {
                return new LibError(e);
            }
        }
        */

        public static Core.Error SetKakuninBi(BecchuOrderKey key, string strUserID, SqlConnection sqlConn)
        {
            List<BecchuOrderKey> lstKey = new List<BecchuOrderKey>();
            lstKey.Add(key);
            return SetKakuninBi(lstKey, strUserID, sqlConn);
        }


        /// <summary>
        /// �d����̎�t����
        /// </summary>
        /// <param name="lstKey"></param>
        /// <param name="strUserID"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static Core.Error SetKakuninBi(List<BecchuOrderKey> lstKey, string strUserID, SqlConnection sqlConn)
        {
            SqlCommand cmd = new SqlCommand("", sqlConn);
            cmd.CommandText = "update T_BecchuKeyInfo set KakuninBi=@dt, UketsukeID=@UketsukeID where MizunoUketsukeBi=@m and ShiiresakiCode=@s and OrderKanriNo=@n and KakuninBi is null";
            cmd.Parameters.AddWithValue("@dt", DateTime.Now);
            cmd.Parameters.AddWithValue("@UketsukeID", strUserID);
            cmd.Parameters.AddWithValue("@s", "");
            cmd.Parameters.AddWithValue("@m", "");
            cmd.Parameters.AddWithValue("@n", "");

            SqlTransaction t = null;

            try
            {
                sqlConn.Open();
                t = sqlConn.BeginTransaction();
                cmd.Transaction = t;
                for (int i = 0; i < lstKey.Count; i++)
                {
                    cmd.Parameters["@m"].Value = lstKey[i].UketsukeBi;
                    cmd.Parameters["@s"].Value = lstKey[i].ShiiresakiCode;
                    cmd.Parameters["@n"].Value = lstKey[i].OrderKanriNo;
                    int nRet = cmd.ExecuteNonQuery();
                }
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
            }
        }





        /// <summary>
        /// �m�F��,��t��ID��o�^����@�d����p
        /// </summary>
        /// <param name="MizunoUketsukeBi"></param>
        /// <param name="OrderKanriNo"></param>
        /// <param name="ShiiresakiCode"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static LibError T_BecchuKeyInfo_Update_KakuninBi_S_Kill
            (string MizunoUketsukeBi, string OrderKanriNo, string ShiiresakiCode, string loginID, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND "
                + "OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", MizunoUketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", ShiiresakiCode);
            da.UpdateCommand = (new SqlCommandBuilder(da)).GetUpdateCommand();
            MizunoDataSet.T_BecchuKeyInfoDataTable dt = new MizunoDataSet.T_BecchuKeyInfoDataTable();
            da.Fill(dt);
            if (1 != dt.Rows.Count)
                return new LibError("�G���[");
            MizunoDataSet.T_BecchuKeyInfoRow drThis = (MizunoDataSet.T_BecchuKeyInfoRow)dt.Rows[0];
            try
            {
                // �m�F�������o�^�̏ꍇ
                if (drThis.IsKakuninBiNull())
                {
                    drThis.KakuninBi = DateTime.Now;
                    drThis.UketsukeID = loginID;
                    da.Update(dt);
                }
                return null;
            }
            catch (Exception e)
            {
                return new LibError(e);
            }
        }



        /// <summary>
        /// �w��I�[�_�[�Ǘ�No�ɕR�t�����}�[�N�������擾(���j�t�H�[���ɂ���}�[�N�Ȃ�)
        /// </summary>
        /// <param name="strOrderKanriNo"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static ViewDataset.VIEW_BecchuKeyInfoDataTable
            getVIEW_BecchuKeyInfoDataTable4MarkOrder(string strOrderKanriNo, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT * 
FROM                     VIEW_BecchuKeyInfo

WITH(NOLOCK)

WHERE                   (ShumokuCode LIKE N'M%' OR
ShumokuCode = N'SE') AND (([51OrderNo] = @OrderKanriNo) or (BecchuSpectraNo = @OrderKanriNo) or (P51OrderNo = @OrderKanriNo))";

            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", strOrderKanriNo);
            da.SelectCommand.CommandTimeout = 0;

            ViewDataset.VIEW_BecchuKeyInfoDataTable dt = new ViewDataset.VIEW_BecchuKeyInfoDataTable();
            da.Fill(dt);
            return dt;
        }

        public static ViewDataset.VIEW_BecchuKeyInfoDataTable
            getVIEW_BecchuKeyInfoDataTableM2(string sqlcmd, Core.Sql.RowNumberInfo r, SqlConnection sqlConn, ref int nCount)
        {
            SqlDataAdapter da = null;

            if (sqlcmd == "")
            {
                da = new SqlDataAdapter("select * from VIEW_BecchuKeyInfo where ShumokuCode = 'M2' ", sqlConn);
            }
            else
            {
                da = new SqlDataAdapter("select * from VIEW_BecchuKeyInfo where ShumokuCode = 'M2' " + sqlcmd, sqlConn);
            }

            ViewDataset.VIEW_BecchuKeyInfoDataTable dt = new ViewDataset.VIEW_BecchuKeyInfoDataTable();

            if (null != r)
            {
                r.LoadData(da.SelectCommand, sqlConn, dt, ref nCount);
            }
            else
            {
                da.Fill(dt);
                nCount = dt.Count;
            }
            return dt;
        }





        public static ViewDataset.VIEW_BecchuKeyInfoDataTable
            getVIEW_BecchuKeyInfoDataTable(string strOrderKanriNo, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"SELECT *  FROM VIEW_BecchuKeyInfo WHERE OrderKanriNo = @OrderKanriNo";
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", strOrderKanriNo);
            ViewDataset.VIEW_BecchuKeyInfoDataTable dt = new ViewDataset.VIEW_BecchuKeyInfoDataTable();
            da.Fill(dt);
            return dt;
        }


        public static ViewDataset.VIEW_BecchuKeyInfoRow
             getVIEW_BecchuKeyInfoRow(BecchuOrderKey key, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            //da.SelectCommand.CommandText = "SELECT * FROM VIEW_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            da.SelectCommand.CommandText = "SELECT * FROM VIEW_BecchuKeyInfo WITH(NOLOCK) WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode ";

            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", key.UketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", key.OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", key.ShiiresakiCode);

            ViewDataset.VIEW_BecchuKeyInfoDataTable dt = new ViewDataset.VIEW_BecchuKeyInfoDataTable();
            // 2014/05/30 �ꎞ���u
            da.SelectCommand.CommandTimeout = 60000;

            da.Fill(dt);
            if (1 == dt.Count) return dt[0];
            return null;
        }

        public static ViewDataset.VIEW_BecchuKeyInfo_PerFormanceRow
             getVIEW_BecchuKeyInfo_PerFormanceRow(BecchuOrderKey key, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT * FROM VIEW_BecchuKeyInfo_PerFormance AS VIEw_BecchuKeyInfo WITH(NOLOCK) WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode ";
            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", key.UketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", key.OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", key.ShiiresakiCode);

            ViewDataset.VIEW_BecchuKeyInfo_PerFormanceDataTable dt = new ViewDataset.VIEW_BecchuKeyInfo_PerFormanceDataTable();
            da.SelectCommand.CommandTimeout = 600000;

            da.Fill(dt);
            if (1 == dt.Count) return dt[0];
            return null;
        }

        public static MizunoDataSet.M_TsuikaHinbanDataTable
            getM_TsuikaHinbanDataTable(string strShumokuCode, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT * FROM M_TsuikaHinban WHERE ShumokuCode = @ShumokuCode ";
            da.SelectCommand.Parameters.AddWithValue("@ShumokuCode", strShumokuCode);
            MizunoDataSet.M_TsuikaHinbanDataTable dt = new MizunoDataSet.M_TsuikaHinbanDataTable();
            da.Fill(dt);
            return dt;
        }

        public static MizunoDataSet.M_TsuikaHinbanDataTable
            getM_TsuikaHinbanProfitDataTable(string ProfitCenterCode, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT * FROM M_TsuikaHinban";
            //da.SelectCommand.Parameters.AddWithValue("@ShumokuCode", ProfitCenterCode);
            da.SelectCommand.CommandText += " where Hinban like '" + ProfitCenterCode + "%'";
            MizunoDataSet.M_TsuikaHinbanDataTable dt = new MizunoDataSet.M_TsuikaHinbanDataTable();
            da.Fill(dt);
            return dt;
        }


        /// <summary>
        /// ��ڃR�[�h���擾
        /// </summary>
        /// <param name="MizunoUketsukeBi"></param>
        /// <param name="OrderKanriNo"></param>
        /// <param name="ShiiresakiCode"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static string getShumokuCode(string MizunoUketsukeBi, string OrderKanriNo,
                                                    string ShiiresakiCode, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", MizunoUketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", ShiiresakiCode);
            MizunoDataSet.T_BecchuKeyInfoDataTable dt = new MizunoDataSet.T_BecchuKeyInfoDataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                return "";
            }
            else
            {
                return dt[0].ShumokuCode;
            }
        }

        /// <summary>
        /// �L�[�����擾
        /// </summary>
        /// <param name="MizunoUketsukeBi"></param>
        /// <param name="OrderKanriNo"></param>
        /// <param name="ShiiresakiCode"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static MizunoDataSet.T_BecchuKeyInfoRow getT_BecchuKeyInfoRow
            (string OrderKanriNo, string ShiiresakiCode, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText =
                "SELECT TOP 1 * FROM T_BecchuKeyInfo WHERE OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode ORDER BY TourokuBi DESC";
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", ShiiresakiCode);
            MizunoDataSet.T_BecchuKeyInfoDataTable dt = new MizunoDataSet.T_BecchuKeyInfoDataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                return dt[0];
            }
            else
            {
                return null;
            }
        }

        public static MizunoDataSet.T_BecchuKeyInfoRow getT_BecchuKeyInfoRow
            (BecchuOrderKey k, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", k.UketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", k.OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", k.ShiiresakiCode);
            MizunoDataSet.T_BecchuKeyInfoDataTable dt = new MizunoDataSet.T_BecchuKeyInfoDataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
                return dt[0] as MizunoDataSet.T_BecchuKeyInfoRow;
            return null;
        }






        public enum EnumMsgStatus
        {
            None = 0, Jushin = 1, Shoushin = 2, Rireki = 3
        }

        public enum EnumNoukiKaitouStatus
        {
            None = 0, MiKaitou = 1, MiShounin = 2, ShouninZumi = 3
        }


        // ��(0=���o��,1=�o�׍�,2=�L�����Z��,3=�S��,4=����t��)
        public enum EnumChumonStatus
        {
            None = 3, MiShukka = 0, ShukkaZumi = 1, Cancel = 2, MiUketuke = 4,KyuChumonBun = 5
        }


        [Serializable]
        public class Kensaku
        {
            public EnumUserType UserType = EnumUserType.Shiiresaki;

            // �\���\����
            public int nHyoujiTukisu = 12; // �d����̓~�Y�m��t���̉ߋ�12�J�������{���\
            // �[�����F��(�S�� = 0�A�����F = 1�A���F�ς� = 2)
            public EnumNoukiKaitouStatus NoukiKaitouStatus = EnumNoukiKaitouStatus.None;

            // ��ږ�
            public string _ShumokuCode = "";


            // �i��
            public Core.Sql.FilterItem objHinban = null;

            public Core.Type.NengappiKikan objMizunoUketukeBi = null;
            public Core.Type.NengappiKikan objKoujouShukkaYoteiBi = null;

            public List<BecchuOrderKey> lstBecchuOrderKey = null;

            // �I�[�_�[�Ǘ�No
            public string _OrderKanriNo = "";


            // ��(���o�� = 0 /�o�׍ς� = 1 / �L�����Z�� = 2 /�S�� = 3)
            public EnumChumonStatus ChumonStatus = EnumChumonStatus.None;
            // �ʒ��T�C�g�I�[�_�[No
            public string _BecchuSiteOrderNo = "";
            // �`�[����
            public string _TeamMei = "";
            // ���q�l��
            public string _OkyakusamaMei = "";
            // �}�[�N���H�L��
            public string _MarkKakou = "";
            // �n��
            public string _Chiku = "";
            // �c�ƒS����(�~�Y�m�S����)
            public string _EigyouTantousha = "";
            // �[�i�L��
            public string _NouhinKigou = "";
            // �d���於
            public string _ShiiresakiMei = "";
            // ���Y�Ǘ��S����
            public string _SKTantousha = "";

            // ���b�Z�[�W
            public EnumMsgStatus MsgStatus = EnumMsgStatus.None;

            // ��ЃR�[�h(���b�Z�[�W�����������Ɋ܂܂�Ă����ꍇ�̂ݎg�p����)
            public string _KaishaCode = "";

            // �d����R�[�h
            public string _SCode = "";

            // ���Ӑ�R�[�h
            public string _TCode = "";

            // �d����R�[�h(�K�w������׎g�p)
            public string[] _SCodeAry = null;
            // ���Ӑ�R�[�h(�K�w������׎g�p)
            public string[] _TCodeAry = null;

            public void SetWhere(Core.Sql.WhereGenerator w, SqlCommand cmd)
            {
                SetWhere(this, w, cmd);
            }

            //DS2 �_�E�����[�h��p �֐�
            public void SetWhere2(Core.Sql.WhereGenerator w, SqlCommand cmd)
            {
                SetWhere2(this, w, cmd);
            }

            /// <summary>
            /// Where�����쐬
            /// </summary>
            /// <param name="k"></param>
            /// <param name="cmd"></param>
            /// <returns></returns>
            private static void SetWhere(Kensaku k, Core.Sql.WhereGenerator w, SqlCommand cmd)
            {
                if (k.UserType == EnumUserType.Tokuisaki)
                {
                    w.Add("M_Shumoku_TokuisakiKubun=1");    // ��ڃ}�X�^�Ō��J���Ă����ڂ������擾
                }

                // �~�Y�m��t���\���Ώۊ���
                if (0 < k.nHyoujiTukisu)
                {
                    /*
                    string strKanryou = "((case isdate(MizunoUketsukeBi) when 1 then cast(MizunoUketsukeBi as datetime) else null end ) >= CAST(@uuu as datetime) and (KanryouFlg =1 or not CancelBi is null))";
                    string strChuzan = "(KanryouFlg =0 AND CancelBi is null)";
                    w.Add(string.Format("({0} or {1})", strKanryou, strChuzan));
                    cmd.Parameters.AddWithValue("@uuu", Core.Type.Nengetu.Today.AddMonth(-k.nHyoujiKikanMonths).ToDateTime(1));
                 */
                    // ���c�ł����Ă��\���Ώۊ��Ԃ�ݒ肷��B
                    w.Add("(case isdate(VIEW_BecchuKeyInfo.MizunoUketsukeBi) when 1 then cast(VIEW_BecchuKeyInfo.MizunoUketsukeBi as datetime) else null end ) >= CAST(@uuu as datetime)");
                    string ca = Core.Type.Nengetu.Today.AddMonth(-k.nHyoujiTukisu).ToDateTime(1).ToString();
                    cmd.Parameters.AddWithValue("@uuu", Core.Type.Nengetu.Today.AddMonth(-k.nHyoujiTukisu).ToDateTime(1));
                    string a = Core.Type.Nengetu.Today.AddMonth(-k.nHyoujiTukisu).ToDateTime(1).ToString();
                }

                // ���Ӑ�R�[�h
                if (k._TCode != "")
                {
                    w.Add(string.Format("VIEW_BecchuKeyInfo.TokuisakiCode LIKE @TokuisakiCode"));
                    cmd.Parameters.AddWithValue("@TokuisakiCode", k._TCode + "%");
                }
                // �d����R�[�h
                if (!string.IsNullOrEmpty(k._SCode))
                {
                    w.Add("VIEW_BecchuKeyInfo.ShiiresakiCode = @ShiiresakiCode");
                    cmd.Parameters.AddWithValue("@ShiiresakiCode", k._SCode);
                }
                // ���Ӑ�R�[�h
                if (k._TCodeAry != null)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < k._TCodeAry.Length; i++)
                    {
                        if (sb.Length > 0) sb.Append(" OR ");
                        sb.Append("VIEW_BecchuKeyInfo.TokuisakiCode = @TokuisakiCode" + i);
                        cmd.Parameters.AddWithValue("@TokuisakiCode" + i, k._TCodeAry[i]);
                    }
                    if (sb.Length > 0)
                        w.Add("(" + sb.ToString() + ")");
                }

                // �d����R�[�h
                if (k._SCodeAry != null)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < k._SCodeAry.Length; i++)
                    {
                        if (sb.Length > 0) sb.Append(" OR ");
                        sb.Append("VIEW_BecchuKeyInfo.ShiiresakiCode = @ShiiresakiCode" + i);
                        cmd.Parameters.AddWithValue("@ShiiresakiCode" + i, k._SCodeAry[i]);
                    }
                    if (sb.Length > 0)
                        w.Add("(" + sb.ToString() + ")");
                }

                // ��ږ�
                if (k._ShumokuCode != "")
                {
                    w.Add(string.Format("VIEW_BecchuKeyInfo.ShumokuCode LIKE @ShumokuCode"));
                    cmd.Parameters.AddWithValue("@ShumokuCode", "%" + k._ShumokuCode + "%");
                }

                // �i��
                if (null != k.objHinban)
                {
                    // �ʒ�1
                    string strB1 = string.Format(@"
select MizunoUketsukeBi, OrderKanriNo, ShiiresakiCode from T_BecchuKeyInfo where  
exists (SELECT * FROM T_Becchu where T_Becchu.SashizuBi=T_BecchuKeyInfo.MizunoUketsukeBi and 
T_Becchu.SashizuNo=T_BecchuKeyInfo.OrderKanriNo and 
T_Becchu.ShiiresakiCode=T_BecchuKeyInfo.ShiiresakiCode and ({0} or {1}))", k.objHinban.GetFilterText("ltrim(Hinban1)", "@bh1", cmd),
                                                                         k.objHinban.GetFilterText("ltrim(Hinban2)", "@bh2", cmd));

                    // �ʒ��Q
                    string strB2 = string.Format(@"
select MizunoUketsukeBi, OrderKanriNo, ShiiresakiCode  from T_BecchuKeyInfo where  
exists (SELECT * FROM T_Becchu2Repeat where T_Becchu2Repeat.SashizuBi = T_BecchuKeyInfo.MizunoUketsukeBi and 
T_Becchu2Repeat.SashizuNo=T_BecchuKeyInfo.OrderKanriNo and 
T_Becchu2Repeat.ShiiresakiCode=T_BecchuKeyInfo.ShiiresakiCode and {0})", k.objHinban.GetFilterText("ltrim(Hinban)", "@b2h", cmd));

                    // �c�r
                    string strDS = string.Format(@"
select MizunoUketsukeBi, OrderKanriNo, ShiiresakiCode  from T_BecchuKeyInfo where  
exists (SELECT * FROM T_DS where T_DS.OrderKanriNo = T_BecchuKeyInfo.OrderKanriNo and ({0} or {1}))"
                        , k.objHinban.GetFilterText("ltrim(PantsHinban)", "@ds1", cmd),
                        k.objHinban.GetFilterText("ltrim(ShirtsHinban)", "@ds2", cmd));

                    // SP
                    string strSP1 = string.Format(@"
select MizunoUketsukeBi, OrderKanriNo, ShiiresakiCode  from T_BecchuKeyInfo where  
exists (SELECT * FROM T_SP where T_SP.OrderKanriNo = T_BecchuKeyInfo.OrderKanriNo and 
T_SP.ShiiresakiCode1 = T_BecchuKeyInfo.ShiiresakiCode and {0})", k.objHinban.GetFilterText("ltrim(T_SP.Hinban1)", "@sp1", cmd));

                    string strSP2 = string.Format(@"
select MizunoUketsukeBi, OrderKanriNo, ShiiresakiCode  from T_BecchuKeyInfo where  
exists (SELECT * FROM T_SP where T_SP.OrderKanriNo = T_BecchuKeyInfo.OrderKanriNo and 
T_SP.ShiiresakiCode1 = T_BecchuKeyInfo.ShiiresakiCode and {0})", k.objHinban.GetFilterText("ltrim(T_SP.Hinban2)", "@sp2", cmd));


                    string str = string.Format(@"
exists (select * from ({0} union all {1} union all {2} union all {3} union all {4}) T_A where T_A.MizunoUketsukeBi=VIEW_BecchuKeyInfo.MizunoUketsukeBi and 
T_A.OrderKanriNo=VIEW_BecchuKeyInfo.OrderKanriNo and T_A.ShiiresakiCode=VIEW_BecchuKeyInfo.ShiiresakiCode)", strB1, strB2, strDS, strSP1, strSP2);

                    w.Add(str);

                }


                // �I�[�_�[��t��
                if (null != k.objMizunoUketukeBi)
                {
                    w.Add(k.objMizunoUketukeBi.GenerateSQLAsDateTime("(case isdate(VIEW_BecchuKeyInfo.MizunoUketsukeBi) when 1 then cast(VIEW_BecchuKeyInfo.MizunoUketsukeBi as datetime) else null end )"));
                }

                // �H��o�ח\����@11-02-12
                if (null != k.objKoujouShukkaYoteiBi)
                {
                    w.Add(k.objKoujouShukkaYoteiBi.GenerateSQLAsDateTime("(case isdate(VIEW_BecchuKeyInfo.KoujyouShukkaYoteiBi) when 1 then cast(VIEW_BecchuKeyInfo.KoujyouShukkaYoteiBi as datetime) else null end )"));
                }

                // �I�[�_�[�Ǘ�No
                if (!string.IsNullOrEmpty(k._OrderKanriNo))
                {
                    // �D �I�[�_�[�Ǘ�No�A�ʒ��T�C�g���ް���@�O����v�����Ԉ�v�����ɕύX10-09-27 yamada
                    w.Add(string.Format("VIEW_BecchuKeyInfo.OrderKanriNo LIKE @OrderKanriNo"));
                    cmd.Parameters.AddWithValue("@OrderKanriNo", "%" + k._OrderKanriNo + "%");
                }

                if (null != k.lstBecchuOrderKey && 0 < k.lstBecchuOrderKey.Count)
                {
                    string[] str = new string[k.lstBecchuOrderKey.Count];

                    string ca = "";

                    for (int i = 0; i < k.lstBecchuOrderKey.Count; i++)
                    {
                        str[i] = string.Format("(VIEW_BecchuKeyInfo.MizunoUketsukeBi=@u{0} and VIEW_BecchuKeyInfo.OrderKanriNo=@o{0} and VIEW_BecchuKeyInfo.ShiiresakiCode=@s{0})", i);
                        ca += string.Format("(VIEW_BecchuKeyInfo.MizunoUketsukeBi='{0}' and VIEW_BecchuKeyInfo.OrderKanriNo='{1}' and VIEW_BecchuKeyInfo.ShiiresakiCode='{2}')", k.lstBecchuOrderKey[i].UketsukeBi, k.lstBecchuOrderKey[i].OrderKanriNo, k.lstBecchuOrderKey[i].ShiiresakiCode);
                        if (i + 1 < k.lstBecchuOrderKey.Count)
                        {
                            ca += " OR ";
                        }

                        cmd.Parameters.AddWithValue("@u" + i.ToString(), k.lstBecchuOrderKey[i].UketsukeBi);
                        cmd.Parameters.AddWithValue("@o" + i.ToString(), k.lstBecchuOrderKey[i].OrderKanriNo);
                        cmd.Parameters.AddWithValue("@s" + i.ToString(), k.lstBecchuOrderKey[i].ShiiresakiCode);
                    }
                    w.Add("(" + string.Join(" OR ", str) + ")");
                }


                // ��(0=���o��,1=�o�׍�,2=�L�����Z��,3=�S��,4=����t��)
                switch (k.ChumonStatus)
                {
                    case EnumChumonStatus.Cancel:
                        // �L�����Z��
                        w.Add("VIEW_BecchuKeyInfo.CancelBi IS NOT NULL");
                        break;
                    case EnumChumonStatus.MiShukka:
                        //2013/11/22 ���� �o�׍ς݂Ȃ̂Ɂu���o�ׁv�ňꗗ�ɕ\������Ă���f�[�^������B�Ƃ̘A�����󂯁A�C���B
                        w.Add("(VIEW_BecchuKeyInfo.KanryouFlg =0 AND CancelBi is null)");

                        //w.Add("((VIEW_BecchuKeyInfo.T_NouhinMeisaiKanryouFlgMax = 0 OR VIEW_BecchuKeyInfo.T_NouhinMeisaiKanryouFlgMax is NULL) AND CancelBi is null)");
                        //w.Add(@"((VIEW_BecchuKeyInfo.T_NouhinMeisaiKanryouFlgMax = 0 OR VIEW_BecchuKeyInfo.T_NouhinMeisaiKanryouFlgMax is NULL) AND CancelBi is null) AND (VIEW_BecchuKeyInfo.KanryouFlg =0 AND CancelBi is null)");
                        break;
                    case EnumChumonStatus.ShukkaZumi:
                        w.Add("(VIEW_BecchuKeyInfo.KanryouFlg =1 AND CancelBi is null)");

                        //w.Add("(VIEW_BecchuKeyInfo.T_NouhinMeisaiKanryouFlgMax = 9 AND CancelBi is null)");
//                        w.Add(@"(
//                                    (VIEW_BecchuKeyInfo.KanryouFlg =1 AND CancelBi is null) OR 
//                                    ((VIEW_BecchuKeyInfo.T_NouhinMeisaiKanryouFlgMax = 0 OR VIEW_BecchuKeyInfo.T_NouhinMeisaiKanryouFlgMax is NULL) AND VIEW_BecchuKeyInfo.KanryouFlg =1 AND CancelBi is null) 
//                                )");
                        //w.Add(@"((VIEW_BecchuKeyInfo.T_NouhinMeisaiKanryouFlgMax =  OR VIEW_BecchuKeyInfo.T_NouhinMeisaiKanryouFlgMax is NULL) AND CancelBi is null) AND (VIEW_BecchuKeyInfo.KanryouFlg =0 AND CancelBi is null)");
                        break;
                    case EnumChumonStatus.MiUketuke:
                        w.Add("(VIEW_BecchuKeyInfo.KakuninBi is null AND CancelBi is null)");
                        break;
                    case EnumChumonStatus.KyuChumonBun:
                        w.Add(string.Format("(VIEW_BecchuKeyInfo.OrderKanriNo LIKE '%{0}')", sKyuHacchuPrefix));
                        break;
                    case EnumChumonStatus.None:
                        break;
                }
                if (k.ChumonStatus != EnumChumonStatus.KyuChumonBun && k.ChumonStatus != EnumChumonStatus.None)
                {
                    w.Add(string.Format("(VIEW_BecchuKeyInfo.OrderKanriNo NOT LIKE '%{0}')",sKyuHacchuPrefix));
                }


                // �ʒ��T�C�g�I�[�_�[No
                if (!string.IsNullOrEmpty(k._BecchuSiteOrderNo))
                {
                    // �D �I�[�_�[�Ǘ�No�A�ʒ��T�C�g���ް���@�O����v�����Ԉ�v�����ɕύX10-09-27
                    w.Add(string.Format("VIEW_BecchuKeyInfo.BecchuSiteOrderNo LIKE @BecchuSiteOrderNo"));
                    cmd.Parameters.AddWithValue("@BecchuSiteOrderNo", "%" + k._BecchuSiteOrderNo + "%");
                }
                // �`�[����
                if (!string.IsNullOrEmpty(k._TeamMei))
                {
                    w.Add(string.Format("VIEW_BecchuKeyInfo.TeamMei LIKE @TeamMei"));
                    cmd.Parameters.AddWithValue("@TeamMei", "%" + k._TeamMei + "%");
                }
                // ���q�l��
                if (!string.IsNullOrEmpty(k._OkyakusamaMei))
                {
                    w.Add(string.Format("VIEW_BecchuKeyInfo.OkyakusamaMei LIKE @OkyakusamaMei"));
                    cmd.Parameters.AddWithValue("@OkyakusamaMei", "%" + k._OkyakusamaMei + "%");
                }
                // �}�[�N���H�L��
                if (k._MarkKakou != "")
                {
                    if (k._MarkKakou == MarkKakouKubun.Nashi)
                    {
                        // �����F�u�����v
                        w.Add(string.Format("VIEW_BecchuKeyInfo.MarkKakou = '" + k._MarkKakou + "' "));
                    }
                    else
                    {
                        // �L��F�u���i��}�[�N���H�v�u�����i�}�[�N���H�v�u�L��v�u�L�v    
                        w.Add(string.Format("VIEW_BecchuKeyInfo.MarkKakou != '" + MarkKakouKubun.Nashi + "' "));
                    }
                }
                // �n��
                if (!string.IsNullOrEmpty(k._Chiku))
                {
                    w.Add(string.Format("VIEW_BecchuKeyInfo.Chiku LIKE @Chiku"));
                    cmd.Parameters.AddWithValue("@Chiku", k._Chiku + "%");
                }
                // �c�ƒS����(�~�Y�m�S����)
                if (!string.IsNullOrEmpty(k._EigyouTantousha))
                {
                    w.Add(string.Format("VIEW_BecchuKeyInfo.MizunoTantousha LIKE @MizunoTantousha"));
                    cmd.Parameters.AddWithValue("@MizunoTantousha", "%" + k._EigyouTantousha + "%");
                }
                // �[�i�L��
                if (!string.IsNullOrEmpty(k._NouhinKigou))
                {
                    w.Add(string.Format("VIEW_BecchuKeyInfo.NouhinKigou = '{0}'", k._NouhinKigou));
                }
                // �d���於
                if (!string.IsNullOrEmpty(k._ShiiresakiMei))
                {
                    w.Add(string.Format("VIEW_BecchuKeyInfo.ShiiresakiMei LIKE @ShiiresakiMei"));
                    cmd.Parameters.AddWithValue("@ShiiresakiMei", "%" + k._ShiiresakiMei + "%");
                }
                // ���Y�Ǘ��S����
                if (!string.IsNullOrEmpty(k._SKTantousha))
                {
                    w.Add(string.Format("VIEW_BecchuKeyInfo.SKTantousha LIKE @SKTantousha"));
                    cmd.Parameters.AddWithValue("@SKTantousha", "%" + k._SKTantousha + "%");
                }

                // �[�����F��
                switch (k.NoukiKaitouStatus)
                {
                    case EnumNoukiKaitouStatus.MiKaitou:
                        // ����
                        // �ŏ�����KoujyouShukkaYoteiBi�ɒl���Z�b�g����Ă���i��V�X�e�����Ŕ[���o�^�j�̃f�[�^������H�H
                        w.Add("VIEW_BecchuKeyInfo.KoujyouShukkaYoteiBi='' AND VIEW_BecchuKeyInfo_LatestNoukiKaitou_ShouninFlg is null");
                        break;
                    case EnumNoukiKaitouStatus.MiShounin:
                        w.Add("VIEW_BecchuKeyInfo_LatestNoukiKaitou_ShouninFlg = 0");
                        break;
                    case EnumNoukiKaitouStatus.ShouninZumi:
                        w.Add("VIEW_BecchuKeyInfo_LatestNoukiKaitou_ShouninFlg = 1");
                        break;
                }


                // ���b�Z�[�W
                switch (k.MsgStatus)
                {
                    case EnumMsgStatus.Rireki:
                        if (k.UserType == EnumUserType.Shiiresaki)
                        {
                            w.Add("(VIEW_BecchuKeyInfo.ShiiresakiCode = @msg and not ToLoginID is null)");
                            cmd.Parameters.AddWithValue("@msg", k._SCode);  // ���b�Z�[�W�̌����̓��O�C�����Ă���d����̂�
                        }
                        else
                            w.Add("not ToLoginID is null");
                        break;
                    case EnumMsgStatus.Shoushin:
                        // ���M
                        if (k.UserType == EnumUserType.Shiiresaki)
                        {
                            w.Add("(FromKaishaCode<>'0' and ToLoginID is null and ShiiresakiCode = @msg)");
                            cmd.Parameters.AddWithValue("@msg", k._SCode);  // ���b�Z�[�W�̌����̓��O�C�����Ă���d����̂�
                        }
                        else if (k.UserType == EnumUserType.Mizuno)
                            w.Add("(FromKaishaCode='0' and ToLoginID is null)");
                        break;
                    case EnumMsgStatus.Jushin:
                        // ���M
                        if (k.UserType == EnumUserType.Shiiresaki)
                        {
                            w.Add("(FromKaishaCode='0' and ToLoginID is null and ShiiresakiCode = @msg)");
                            cmd.Parameters.AddWithValue("@msg", k._SCode);  // ���b�Z�[�W�̌����̓��O�C�����Ă���d����̂�
                        }
                        else if (k.UserType == EnumUserType.Mizuno)
                            w.Add("(FromKaishaCode<>'0' and ToLoginID is null)");
                        break;
                }



            }


            /// <summary>
            /// Where�����쐬  DS2 �_�E�����[�h��p �֐�
            /// </summary>
            /// <param name="k"></param>
            /// <param name="cmd"></param>
            /// <returns></returns>
            private static void SetWhere2(Kensaku k, Core.Sql.WhereGenerator w, SqlCommand cmd)
            {
                //2013.04.24 ����
                //DS2 �_�E�����[�h���A�i�ԃu�����N�E����0�̖��ׂ͏o�͂��Ȃ����̂Ƃ���B
                w.Add("Hinban != ''");
                w.Add("Suryou > 0");


                if (k.UserType == EnumUserType.Tokuisaki)
                {
                    w.Add("M_Shumoku_TokuisakiKubun=1");    // ��ڃ}�X�^�Ō��J���Ă����ڂ������擾
                }

                // �~�Y�m��t���\���Ώۊ���
                if (0 < k.nHyoujiTukisu)
                {
                    /*
                    string strKanryou = "((case isdate(MizunoUketsukeBi) when 1 then cast(MizunoUketsukeBi as datetime) else null end ) >= CAST(@uuu as datetime) and (KanryouFlg =1 or not CancelBi is null))";
                    string strChuzan = "(KanryouFlg =0 AND CancelBi is null)";
                    w.Add(string.Format("({0} or {1})", strKanryou, strChuzan));
                    cmd.Parameters.AddWithValue("@uuu", Core.Type.Nengetu.Today.AddMonth(-k.nHyoujiKikanMonths).ToDateTime(1));
                 */
                    // ���c�ł����Ă��\���Ώۊ��Ԃ�ݒ肷��B
                    w.Add("(case isdate(MizunoUketsukeBi) when 1 then cast(MizunoUketsukeBi as datetime) else null end ) >= CAST(@uuu as datetime)");
                    cmd.Parameters.AddWithValue("@uuu", Core.Type.Nengetu.Today.AddMonth(-k.nHyoujiTukisu).ToDateTime(1));
                }

                // ���Ӑ�R�[�h
                if (k._TCode != "")
                {
                    w.Add(string.Format("TokuisakiCode LIKE @TokuisakiCode"));
                    cmd.Parameters.AddWithValue("@TokuisakiCode", k._TCode + "%");
                }
                // �d����R�[�h
                if (!string.IsNullOrEmpty(k._SCode))
                {
                    w.Add("ShiiresakiCode = @ShiiresakiCode");
                    cmd.Parameters.AddWithValue("@ShiiresakiCode", k._SCode);
                }
                // ���Ӑ�R�[�h
                if (k._TCodeAry != null)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < k._TCodeAry.Length; i++)
                    {
                        if (sb.Length > 0) sb.Append(" OR ");
                        sb.Append("TokuisakiCode = @TokuisakiCode" + i);
                        cmd.Parameters.AddWithValue("@TokuisakiCode" + i, k._TCodeAry[i]);
                    }
                    if (sb.Length > 0)
                        w.Add("(" + sb.ToString() + ")");
                }

                // �d����R�[�h
                if (k._SCodeAry != null)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < k._SCodeAry.Length; i++)
                    {
                        if (sb.Length > 0) sb.Append(" OR ");
                        sb.Append("ShiiresakiCode = @ShiiresakiCode" + i);
                        cmd.Parameters.AddWithValue("@ShiiresakiCode" + i, k._SCodeAry[i]);
                    }
                    if (sb.Length > 0)
                        w.Add("(" + sb.ToString() + ")");
                }

                // ��ږ�
                if (k._ShumokuCode != "")
                {
                    w.Add(string.Format("ShumokuCode LIKE @ShumokuCode"));
                    cmd.Parameters.AddWithValue("@ShumokuCode", "%" + k._ShumokuCode + "%");
                }

                // �i��
                if (null != k.objHinban)
                {
                    // �ʒ�1
                    string strB1 = string.Format(@"
select MizunoUketsukeBi, OrderKanriNo, ShiiresakiCode from T_BecchuKeyInfo where  
exists (SELECT * FROM T_Becchu where T_Becchu.SashizuBi=T_BecchuKeyInfo.MizunoUketsukeBi and 
T_Becchu.SashizuNo=T_BecchuKeyInfo.OrderKanriNo and 
T_Becchu.ShiiresakiCode=T_BecchuKeyInfo.ShiiresakiCode and ({0} or {1}))", k.objHinban.GetFilterText("ltrim(Hinban1)", "@bh1", cmd),
                                                                         k.objHinban.GetFilterText("ltrim(Hinban2)", "@bh2", cmd));

                    // �ʒ��Q
                    string strB2 = string.Format(@"
select MizunoUketsukeBi, OrderKanriNo, ShiiresakiCode  from T_BecchuKeyInfo where  
exists (SELECT * FROM T_Becchu2Repeat where T_Becchu2Repeat.SashizuBi = T_BecchuKeyInfo.MizunoUketsukeBi and 
T_Becchu2Repeat.SashizuNo=T_BecchuKeyInfo.OrderKanriNo and 
T_Becchu2Repeat.ShiiresakiCode=T_BecchuKeyInfo.ShiiresakiCode and {0})", k.objHinban.GetFilterText("ltrim(Hinban)", "@b2h", cmd));

                    // �c�r
                    string strDS = string.Format(@"
select MizunoUketsukeBi, OrderKanriNo, ShiiresakiCode  from T_BecchuKeyInfo where  
exists (SELECT * FROM T_DS where T_DS.OrderKanriNo = T_BecchuKeyInfo.OrderKanriNo and ({0} or {1}))"
                        , k.objHinban.GetFilterText("ltrim(PantsHinban)", "@ds1", cmd),
                        k.objHinban.GetFilterText("ltrim(ShirtsHinban)", "@ds2", cmd));

                    // SP
                    string strSP1 = string.Format(@"
select MizunoUketsukeBi, OrderKanriNo, ShiiresakiCode  from T_BecchuKeyInfo where  
exists (SELECT * FROM T_SP where T_SP.OrderKanriNo = T_BecchuKeyInfo.OrderKanriNo and 
T_SP.ShiiresakiCode1 = T_BecchuKeyInfo.ShiiresakiCode and {0})", k.objHinban.GetFilterText("ltrim(T_SP.Hinban1)", "@sp1", cmd));

                    string strSP2 = string.Format(@"
select MizunoUketsukeBi, OrderKanriNo, ShiiresakiCode  from T_BecchuKeyInfo where  
exists (SELECT * FROM T_SP where T_SP.OrderKanriNo = T_BecchuKeyInfo.OrderKanriNo and 
T_SP.ShiiresakiCode1 = T_BecchuKeyInfo.ShiiresakiCode and {0})", k.objHinban.GetFilterText("ltrim(T_SP.Hinban2)", "@sp2", cmd));


                    string str = string.Format(@"
exists (select * from ({0} union all {1} union all {2} union all {3} union all {4}) T_A where T_A.MizunoUketsukeBi=MizunoUketsukeBi and 
T_A.OrderKanriNo=OrderKanriNo and T_A.ShiiresakiCode=ShiiresakiCode)", strB1, strB2, strDS, strSP1, strSP2);

                    w.Add(str);

                }


                // �I�[�_�[��t��
                if (null != k.objMizunoUketukeBi)
                {
                    w.Add(k.objMizunoUketukeBi.GenerateSQLAsDateTime("(case isdate(MizunoUketsukeBi) when 1 then cast(MizunoUketsukeBi as datetime) else null end )"));
                }

                // �H��o�ח\����@11-02-12
                if (null != k.objKoujouShukkaYoteiBi)
                {
                    w.Add(k.objKoujouShukkaYoteiBi.GenerateSQLAsDateTime("(case isdate(KoujyouShukkaYoteiBi) when 1 then cast(KoujyouShukkaYoteiBi as datetime) else null end )"));
                }

                // �I�[�_�[�Ǘ�No
                if (!string.IsNullOrEmpty(k._OrderKanriNo))
                {
                    // �D �I�[�_�[�Ǘ�No�A�ʒ��T�C�g���ް���@�O����v�����Ԉ�v�����ɕύX10-09-27 yamada
                    w.Add(string.Format("OrderKanriNo LIKE @OrderKanriNo"));
                    cmd.Parameters.AddWithValue("@OrderKanriNo", "%" + k._OrderKanriNo + "%");
                }

                if (null != k.lstBecchuOrderKey && 0 < k.lstBecchuOrderKey.Count)
                {
                    string[] str = new string[k.lstBecchuOrderKey.Count];
                    for (int i = 0; i < k.lstBecchuOrderKey.Count; i++)
                    {
                        str[i] = string.Format("(MizunoUketsukeBi=@u{0} and OrderKanriNo=@o{0} and ShiiresakiCode=@s{0})", i);
                        cmd.Parameters.AddWithValue("@u" + i.ToString(), k.lstBecchuOrderKey[i].UketsukeBi);
                        cmd.Parameters.AddWithValue("@o" + i.ToString(), k.lstBecchuOrderKey[i].OrderKanriNo);
                        cmd.Parameters.AddWithValue("@s" + i.ToString(), k.lstBecchuOrderKey[i].ShiiresakiCode);
                    }
                    w.Add("(" + string.Join(" OR ", str) + ")");
                }


                // ��(0=���o��,1=�o�׍�,2=�L�����Z��,3=�S��,4=����t��)
                switch (k.ChumonStatus)
                {
                    case EnumChumonStatus.Cancel:
                        // �L�����Z��
                        w.Add("CancelBi IS NOT NULL");
                        break;
                    case EnumChumonStatus.MiShukka:
                        w.Add("((KanryouFlg =0 or KanryouFlg is null) AND CancelBi is null)");
                        break;
                    case EnumChumonStatus.ShukkaZumi:
                        w.Add("(KanryouFlg =1 AND CancelBi is null)");
                        break;
                    case EnumChumonStatus.MiUketuke:
                        w.Add("(KakuninBi is null AND CancelBi is null)");
                        break;
                    case EnumChumonStatus.KyuChumonBun:
                        w.Add(string.Format("(OrderKanriNo LIKE '%{0}')", sKyuHacchuPrefix));
                        break;
                    case EnumChumonStatus.None:
                        break;
                }
                if (k.ChumonStatus != EnumChumonStatus.KyuChumonBun && k.ChumonStatus != EnumChumonStatus.None)
                {
                    w.Add(string.Format("(OrderKanriNo NOT LIKE '%{0}')", sKyuHacchuPrefix));
                }


                // �ʒ��T�C�g�I�[�_�[No
                if (!string.IsNullOrEmpty(k._BecchuSiteOrderNo))
                {
                    // �D �I�[�_�[�Ǘ�No�A�ʒ��T�C�g���ް���@�O����v�����Ԉ�v�����ɕύX10-09-27
                    w.Add(string.Format("BecchuSiteOrderNo LIKE @BecchuSiteOrderNo"));
                    cmd.Parameters.AddWithValue("@BecchuSiteOrderNo", "%" + k._BecchuSiteOrderNo + "%");
                }
                // �`�[����
                if (!string.IsNullOrEmpty(k._TeamMei))
                {
                    w.Add(string.Format("TeamMei LIKE @TeamMei"));
                    cmd.Parameters.AddWithValue("@TeamMei", "%" + k._TeamMei + "%");
                }
                // ���q�l��
                if (!string.IsNullOrEmpty(k._OkyakusamaMei))
                {
                    w.Add(string.Format("OkyakusamaMei LIKE @OkyakusamaMei"));
                    cmd.Parameters.AddWithValue("@OkyakusamaMei", "%" + k._OkyakusamaMei + "%");
                }
                // �}�[�N���H�L��
                if (k._MarkKakou != "")
                {
                    if (k._MarkKakou == MarkKakouKubun.Nashi)
                    {
                        // �����F�u�����v
                        w.Add(string.Format("MarkKakou = '" + k._MarkKakou + "' "));
                    }
                    else
                    {
                        // �L��F�u���i��}�[�N���H�v�u�����i�}�[�N���H�v�u�L��v�u�L�v    
                        w.Add(string.Format("MarkKakou != '" + MarkKakouKubun.Nashi + "' "));
                    }
                }
                // �n��
                if (!string.IsNullOrEmpty(k._Chiku))
                {
                    w.Add(string.Format("Chiku LIKE @Chiku"));
                    cmd.Parameters.AddWithValue("@Chiku", k._Chiku + "%");
                }
                // �c�ƒS����(�~�Y�m�S����)
                if (!string.IsNullOrEmpty(k._EigyouTantousha))
                {
                    w.Add(string.Format("MizunoTantousha LIKE @MizunoTantousha"));
                    cmd.Parameters.AddWithValue("@MizunoTantousha", "%" + k._EigyouTantousha + "%");
                }
                // �[�i�L��
                if (!string.IsNullOrEmpty(k._NouhinKigou))
                {
                    w.Add(string.Format("NouhinKigou = '{0}'", k._NouhinKigou));
                }
                // �d���於
                if (!string.IsNullOrEmpty(k._ShiiresakiMei))
                {
                    w.Add(string.Format("ShiiresakiMei LIKE @ShiiresakiMei"));
                    cmd.Parameters.AddWithValue("@ShiiresakiMei", "%" + k._ShiiresakiMei + "%");
                }
                // ���Y�Ǘ��S����
                if (!string.IsNullOrEmpty(k._SKTantousha))
                {
                    w.Add(string.Format("SKTantousha LIKE @SKTantousha"));
                    cmd.Parameters.AddWithValue("@SKTantousha", "%" + k._SKTantousha + "%");
                }

                // �[�����F��
                switch (k.NoukiKaitouStatus)
                {
                    case EnumNoukiKaitouStatus.MiKaitou:
                        // ����
                        // �ŏ�����KoujyouShukkaYoteiBi�ɒl���Z�b�g����Ă���i��V�X�e�����Ŕ[���o�^�j�̃f�[�^������H�H
                        w.Add("KoujyouShukkaYoteiBi='' AND VIEW_BecchuKeyInfo_LatestNoukiKaitou_ShouninFlg is null");
                        break;
                    case EnumNoukiKaitouStatus.MiShounin:
                        w.Add("VIEW_BecchuKeyInfo_LatestNoukiKaitou_ShouninFlg = 0");
                        break;
                    case EnumNoukiKaitouStatus.ShouninZumi:
                        w.Add("VIEW_BecchuKeyInfo_LatestNoukiKaitou_ShouninFlg = 1");
                        break;
                }


                // ���b�Z�[�W
                switch (k.MsgStatus)
                {
                    case EnumMsgStatus.Rireki:
                        if (k.UserType == EnumUserType.Shiiresaki)
                        {
                            w.Add("(ShiiresakiCode = @msg and not ToLoginID is null)");
                            cmd.Parameters.AddWithValue("@msg", k._SCode);  // ���b�Z�[�W�̌����̓��O�C�����Ă���d����̂�
                        }
                        else
                            w.Add("not ToLoginID is null");
                        break;
                    case EnumMsgStatus.Shoushin:
                        // ���M
                        if (k.UserType == EnumUserType.Shiiresaki)
                        {
                            w.Add("(FromKaishaCode<>'0' and ToLoginID is null and ShiiresakiCode = @msg)");
                            cmd.Parameters.AddWithValue("@msg", k._SCode);  // ���b�Z�[�W�̌����̓��O�C�����Ă���d����̂�
                        }
                        else if (k.UserType == EnumUserType.Mizuno)
                            w.Add("(FromKaishaCode='0' and ToLoginID is null)");
                        break;
                    case EnumMsgStatus.Jushin:
                        // ���M
                        if (k.UserType == EnumUserType.Shiiresaki)
                        {
                            w.Add("(FromKaishaCode='0' and ToLoginID is null and ShiiresakiCode = @msg)");
                            cmd.Parameters.AddWithValue("@msg", k._SCode);  // ���b�Z�[�W�̌����̓��O�C�����Ă���d����̂�
                        }
                        else if (k.UserType == EnumUserType.Mizuno)
                            w.Add("(FromKaishaCode<>'0' and ToLoginID is null)");
                        break;
                }



            }

        }



        public static ViewDataset.VIEW_BecchuKeyInfoDataTable
            getVIEW_BecchuKeyInfoDataTable(Kensaku p, Core.Sql.RowNumberInfo r, SqlConnection sqlConn, ref int nCount)
        {
            SqlDataAdapter da = new SqlDataAdapter(@"select VIEW_BecchuKeyInfo.* from VIEW_BecchuKeyInfo WITH(NOLOCK) ", sqlConn);
            da.SelectCommand.CommandTimeout = 600000;

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
            p.SetWhere(w, da.SelectCommand);

            if (!string.IsNullOrEmpty(w.WhereText))
            {
                da.SelectCommand.CommandText += " where " + w.WhereText + " AND ShumokuCode != 'KA' AND ShumokuCode != 'KB' AND ShumokuCode != 'M2'";
            }
            else 
            {
                da.SelectCommand.CommandText += " where ShumokuCode != 'KA' AND ShumokuCode != 'KB' AND ShumokuCode != 'M2'";
            }

            ViewDataset.VIEW_BecchuKeyInfoDataTable dt = new ViewDataset.VIEW_BecchuKeyInfoDataTable();
            if (null != r)
            {
                r.LoadData(da.SelectCommand, sqlConn, dt, ref nCount);
            }
            else
            {
                da.Fill(dt);
                nCount = dt.Count;
            }
            return dt;
        }

        //�p�t�H�[�}���X���P�p�̃v���O���� 20160310 �܂������ɐ����������邾���Ȃ񂾂��ǂ�
        public static ViewDataset.VIEW_BecchuKeyInfo_PerFormanceDataTable
            getVIEW_BecchuKeyInfo_PerFormanceDataTable(Kensaku p, Core.Sql.RowNumberInfo r, int nPageSize, SqlConnection sqlConn, ref int nCount)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM VIEW_BecchuKeyInfo_PerFormance AS VIEW_BecchuKeyInfo WITH(NOLOCK) ", sqlConn);
            da.SelectCommand.CommandTimeout = 300;//5��

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
            p.SetWhere(w, da.SelectCommand);

            if (!string.IsNullOrEmpty(w.WhereText))
            {
                da.SelectCommand.CommandText += " where " + w.WhereText + " AND ShumokuCode != 'KA' AND ShumokuCode != 'KB' AND ShumokuCode != 'M2'";
            }
            else
            {
                da.SelectCommand.CommandText += " where ShumokuCode != 'KA' AND ShumokuCode != 'KB' AND ShumokuCode != 'M2'";
            }

            //�������擾
            int nMax = 0;
            int nFrom = 0;
            int nTo = 0;
            string strSortText = "";
            string strS = "";
            da.SelectCommand.CommandText = da.SelectCommand.CommandText.Replace("*", string.Format("COUNT(*) AS Cnt", nPageSize));
            DataTable dtCount = new DataTable();
            da.Fill(dtCount);
            nMax = int.Parse(dtCount.Rows[0]["Cnt"].ToString());
            
            //�͈͌���
            if (null != r)
            {
                nFrom = r.nStartNumber;
                nTo = r.nEndNumber;
                strSortText = r.strOverText;
            }
            else
            {
                nTo = nPageSize;
            }

            if (strSortText != "") strS = strSortText; else strS = "MizunoUketsukeBi DESC,OrderKanriNo";

            //VIEW_BecchuKeyInfo�̍��ڂ�ύX�����ꍇ�͂�����ɂ����f�����肢���܂��B 
            da.SelectCommand.CommandText =
                @"SELECT * FROM
                (" +
                da.SelectCommand.CommandText.Replace("COUNT(*) AS Cnt","*,ROW_NUMBER() OVER(" + strS + ") AS RN ")
                + string.Format(@") as t
                where t.RN BETWEEN {0} AND {1}", nFrom, nTo);

            if (strSortText != "") da.SelectCommand.CommandText += " " + strSortText;

            ViewDataset.VIEW_BecchuKeyInfo_PerFormanceDataTable dt = new ViewDataset.VIEW_BecchuKeyInfo_PerFormanceDataTable();
            da.Fill(dt);
            nCount = dt.Count;

            nCount = nMax;

            return dt;
        }


        public static int GetVIEW_BecchuKeyInfo_DataCount(Kensaku p, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from VIEW_BecchuKeyInfo", sqlConn);

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
            p.SetWhere(w, da.SelectCommand);

            if (!string.IsNullOrEmpty(w.WhereText))
                da.SelectCommand.CommandText += " where " + w.WhereText;

            int nCount = 0;
            Core.Sql.MiscClass.GetRecordCount(da.SelectCommand, sqlConn, ref nCount);
            return nCount;
        }


        public static QueryDataset.Q_ShiiresakiCodeMeiDataTable
            getQ_ShiiresakiCodeMeiDataTable(int nHyoujiTukisu, EnumChumonStatus s, string strShumokuCode, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT        DISTINCT          T_A.ShiiresakiCode, ISNULL(dbo.M_Shiiresaki.RyakuMei, dbo.M_Shiiresaki.ShiiresakiMei) AS ShiiresakiMei
FROM                     (SELECT                  ShiiresakiCode, MAX(ShiiresakiMei) AS ShiiresakiMei
FROM                     dbo.T_BecchuKeyInfo {0} 
GROUP BY          ShiiresakiCode) AS T_A LEFT OUTER JOIN
dbo.M_Shiiresaki ON T_A.ShiiresakiCode = dbo.M_Shiiresaki.ShiiresakiCode";

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();

            // ��(0=���o��,1=�o�׍�,2=�L�����Z��,3=�S��,4=����t��)
            switch (s)
            {
                case EnumChumonStatus.Cancel:
                    // �L�����Z��
                    w.Add("exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo)");
                    break;
                case EnumChumonStatus.MiShukka:
                    w.Add("(KanryouFlg =0 AND not exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo))");
                    break;
                case EnumChumonStatus.ShukkaZumi:
                    w.Add("(KanryouFlg =1 AND not exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo))");
                    break;
                case EnumChumonStatus.MiUketuke:
                    w.Add("(KakuninBi is null AND not exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo))");
                    break;
            }

            // �~�Y�m��t���\���Ώۊ���
            if (0 < nHyoujiTukisu)
            {
                w.Add("(case isdate(MizunoUketsukeBi) when 1 then cast(MizunoUketsukeBi as datetime) else null end ) >= CAST(@uuu as datetime)");
                da.SelectCommand.Parameters.AddWithValue("@uuu", Core.Type.Nengetu.Today.AddMonth(-nHyoujiTukisu).ToDateTime(1));
            }


            if (!string.IsNullOrEmpty(strShumokuCode))
            {
                w.Add("ShumokuCode = @ShumokuCode"); ;
                da.SelectCommand.Parameters.AddWithValue("@ShumokuCode", strShumokuCode);
            }

            string strWhere = (string.IsNullOrEmpty(w.WhereText)) ? "" : " where " + w.WhereText;

            da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, strWhere);
            da.SelectCommand.CommandTimeout = 600000;
            QueryDataset.Q_ShiiresakiCodeMeiDataTable dt = new QueryDataset.Q_ShiiresakiCodeMeiDataTable();
            da.Fill(dt);
            return dt;
        }



        public static QueryDataset.Q_ShumokuDataTable
            getQ_ShumokuDataTable(int nHyoujiTukisu, EnumChumonStatus s, string[] lstShiiresakiCode, SqlConnection sqlConn)
        {

            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT                  dbo.T_BecchuKeyInfo.ShumokuCode, dbo.M_Shumoku.ShumokuMei
FROM                     dbo.T_BecchuKeyInfo LEFT OUTER JOIN
                                  dbo.M_Shumoku ON dbo.T_BecchuKeyInfo.ShumokuCode = dbo.M_Shumoku.ShumokuCode
 {0}
GROUP BY          dbo.T_BecchuKeyInfo.ShumokuCode, dbo.M_Shumoku.ShumokuMei";

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();

            // ��(0=���o��,1=�o�׍�,2=�L�����Z��,3=�S��,4=����t��)
            switch (s)
            {
                case EnumChumonStatus.Cancel:
                    // �L�����Z��
                    w.Add("exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo)");
                    break;
                case EnumChumonStatus.MiShukka:
                    w.Add("(KanryouFlg =0 AND not exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo))");
                    break;
                case EnumChumonStatus.ShukkaZumi:
                    w.Add("(KanryouFlg =1 AND not exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo))");
                    break;
                case EnumChumonStatus.MiUketuke:
                    w.Add("(KakuninBi is null AND not exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo))");
                    break;
            }

            if (null != lstShiiresakiCode && 0 < lstShiiresakiCode.Length)
            {
                w.Add(string.Format("T_BecchuKeyInfo.ShiiresakiCode in ('{0}')", string.Join("','", lstShiiresakiCode)));
            }


            // �~�Y�m��t���\���Ώۊ���
            if (0 < nHyoujiTukisu)
            {
                w.Add("(case isdate(MizunoUketsukeBi) when 1 then cast(MizunoUketsukeBi as datetime) else null end ) >= CAST(@uuu as datetime)");
                da.SelectCommand.Parameters.AddWithValue("@uuu", Core.Type.Nengetu.Today.AddMonth(-nHyoujiTukisu).ToDateTime(1));
            }

            w.Add(" dbo.T_BecchuKeyInfo.ShumokuCode != 'KA' and dbo.T_BecchuKeyInfo.ShumokuCode != 'KB' ");



            string strWhere = (string.IsNullOrEmpty(w.WhereText)) ? "" : " where " + w.WhereText;

            da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, strWhere);
            da.SelectCommand.CommandTimeout = 600000;
            QueryDataset.Q_ShumokuDataTable dt = new QueryDataset.Q_ShumokuDataTable();
            da.Fill(dt);
            return dt;
        }


        public static QueryDataset.Q_TokuisakiDataTable
            getQ_TokuisakiDataTable(bool bTokusakiMasterOnly, int nHyoujiTukisu, EnumChumonStatus s, string strShiiresakiCode, string strShumokuCode, SqlConnection sqlConn)
        {

            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);


            if (bTokusakiMasterOnly)
            {
                // ���Ӑ�}�X�^�ɓo�^����Ă�����̂���
                da.SelectCommand.CommandText = @"
SELECT                  T_A.TokuisakiCode, ISNULL(dbo.M_TokuisakiMei.TokuisakiMei, T_A.TokuisakiMei) AS TokuisakiMei
FROM                     (SELECT                  TokuisakiCode, MAX(TokuisakiMei) AS TokuisakiMei
FROM                     dbo.T_BecchuKeyInfo {0} 
GROUP BY          TokuisakiCode) AS T_A INNER JOIN
dbo.M_TokuisakiMei ON T_A.TokuisakiCode = dbo.M_TokuisakiMei.TokuisakiCode";
            }
            else
            {
                da.SelectCommand.CommandText = @"
SELECT                  T_A.TokuisakiCode, ISNULL(dbo.M_TokuisakiMei.TokuisakiMei, T_A.TokuisakiMei) AS TokuisakiMei
FROM                     (SELECT                  TokuisakiCode, MAX(TokuisakiMei) AS TokuisakiMei
FROM                     dbo.T_BecchuKeyInfo {0} 
GROUP BY          TokuisakiCode) AS T_A LEFT OUTER JOIN
dbo.M_TokuisakiMei ON T_A.TokuisakiCode = dbo.M_TokuisakiMei.TokuisakiCode";
            }


            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();


            // ��(0=���o��,1=�o�׍�,2=�L�����Z��,3=�S��,4=����t��)
            switch (s)
            {
                case EnumChumonStatus.Cancel:
                    // �L�����Z��
                    w.Add("exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo)");
                    break;
                case EnumChumonStatus.MiShukka:
                    w.Add("(KanryouFlg =0 AND not exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo))");
                    break;
                case EnumChumonStatus.ShukkaZumi:
                    w.Add("(KanryouFlg =1 AND not exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo))");
                    break;
                case EnumChumonStatus.MiUketuke:
                    w.Add("(KakuninBi is null AND not exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo))");
                    break;
            }

            if (!string.IsNullOrEmpty(strShiiresakiCode))
            {
                w.Add("T_BecchuKeyInfo.ShiiresakiCode = @ShiiresakiCode"); ;
                da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", strShiiresakiCode);
            }

            if (!string.IsNullOrEmpty(strShumokuCode))
            {
                w.Add("ShumokuCode = @ShumokuCode"); ;
                da.SelectCommand.Parameters.AddWithValue("@ShumokuCode", strShumokuCode);
            }

            // �~�Y�m��t���\���Ώۊ���
            if (0 < nHyoujiTukisu)
            {
                w.Add("(case isdate(MizunoUketsukeBi) when 1 then cast(MizunoUketsukeBi as datetime) else null end ) >= CAST(@uuu as datetime)");
                da.SelectCommand.Parameters.AddWithValue("@uuu", Core.Type.Nengetu.Today.AddMonth(-nHyoujiTukisu).ToDateTime(1));
            }


            string strWhere = (string.IsNullOrEmpty(w.WhereText)) ? "" : " where " + w.WhereText;

            da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, strWhere);

            QueryDataset.Q_TokuisakiDataTable dt = new QueryDataset.Q_TokuisakiDataTable();
            da.Fill(dt);
            return dt;
        }



        public static List<string>
            GetNouhinKigou(int nHyoujiTukisu, EnumChumonStatus s, string strShiiresakiCode, string strShumokuCode, SqlConnection sqlConn)
        {

            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT                  NouhinKigou
FROM                     dbo.T_BecchuKeyInfo {0} 
GROUP BY          NouhinKigou
ORDER BY          NouhinKigou
";


            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();

            // ��(0=���o��,1=�o�׍�,2=�L�����Z��,3=�S��,4=����t��)
            switch (s)
            {
                case EnumChumonStatus.Cancel:
                    // �L�����Z��
                    w.Add("exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo)");
                    break;
                case EnumChumonStatus.MiShukka:
                    w.Add("(KanryouFlg =0 AND not exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo))");
                    break;
                case EnumChumonStatus.ShukkaZumi:
                    w.Add("(KanryouFlg =1 AND not exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo))");
                    break;
                case EnumChumonStatus.MiUketuke:
                    w.Add("(KakuninBi is null AND not exists (select * from T_BecchuCancel where T_BecchuCancel.MizunoUketsukeBi=T_BecchuKeyInfo.MizunoUketsukeBi and T_BecchuCancel.OrderKanriNo=T_BecchuKeyInfo.OrderKanriNo))");
                    break;
            }

            if (!string.IsNullOrEmpty(strShiiresakiCode))
            {
                w.Add("T_BecchuKeyInfo.ShiiresakiCode = @ShiiresakiCode"); ;
                da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", strShiiresakiCode);
            }

            if (!string.IsNullOrEmpty(strShumokuCode))
            {
                w.Add("ShumokuCode = @ShumokuCode"); ;
                da.SelectCommand.Parameters.AddWithValue("@ShumokuCode", strShumokuCode);
            }

            // �~�Y�m��t���\���Ώۊ���
            if (0 < nHyoujiTukisu)
            {
                w.Add("(case isdate(MizunoUketsukeBi) when 1 then cast(MizunoUketsukeBi as datetime) else null end ) >= CAST(@uuu as datetime)");
                da.SelectCommand.Parameters.AddWithValue("@uuu", Core.Type.Nengetu.Today.AddMonth(-nHyoujiTukisu).ToDateTime(1));
            }


            string strWhere = (string.IsNullOrEmpty(w.WhereText)) ? "" : " where " + w.WhereText;

            da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, strWhere);
            da.SelectCommand.CommandTimeout = 600000;
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<string> lst = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst.Add((string)dt.Rows[i][0]);
            }

            return lst;
        }


        public static MizunoDataSet.T_Pdf_HenkouDataTable
            getT_Pdf_HenkouDataTableByOrderKanriNo(Kensaku p, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
            p.SetWhere(w, da.SelectCommand);

            string strWhere = (string.IsNullOrEmpty(w.WhereText)) ? "" : " where " + w.WhereText;

            da.SelectCommand.CommandText = string.Format(
                "select * from T_Pdf_Henkou where OrderNo in (select OrderKanriNo from VIEW_BecchuKeyInfo {0})", strWhere);

            MizunoDataSet.T_Pdf_HenkouDataTable dt = new MizunoDataSet.T_Pdf_HenkouDataTable();
            da.Fill(dt);
            return dt;
        }

        public static MizunoDataSet.T_PdfDataTable
            getT_PdfDataTableByOrderKanriNo(Kensaku p, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
            p.SetWhere(w, da.SelectCommand);

            string strWhere = (string.IsNullOrEmpty(w.WhereText)) ? "" : " where " + w.WhereText;

            da.SelectCommand.CommandText = string.Format(
                "select * from T_Pdf where OrderNo in (select OrderKanriNo from VIEW_BecchuKeyInfo {0})", strWhere);

            MizunoDataSet.T_PdfDataTable dt = new MizunoDataSet.T_PdfDataTable();
            da.Fill(dt);
            return dt;
        }


        public static MizunoDataSet.T_TenpuDataTable
            getT_TenpuDataTableByOrderKanriNo(Kensaku p, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
            p.SetWhere(w, da.SelectCommand);

            string strWhere = (string.IsNullOrEmpty(w.WhereText)) ? "" : " where " + w.WhereText;

            da.SelectCommand.CommandText = string.Format(
                "select * from T_Tenpu where T_Tenpu.OrderKanriNo in (select OrderKanriNo from VIEW_BecchuKeyInfo {0})", strWhere);

            MizunoDataSet.T_TenpuDataTable dt = new MizunoDataSet.T_TenpuDataTable();
            da.Fill(dt);
            return dt;
        }


        public static ViewDataset.VIEW_Becchu2_ShukkaMeisaiDataTable
            getVIEW_Becchu2_ShukkaMeisaiDataTable(BecchuOrderKey key, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = "select * from VIEW_Becchu2_ShukkaMeisai WHERE SashizuBi=@z and SashizuNo=@n and ShiiresakiCode=@s";
            da.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);

            ViewDataset.VIEW_Becchu2_ShukkaMeisaiDataTable dt = new ViewDataset.VIEW_Becchu2_ShukkaMeisaiDataTable();
            da.Fill(dt);
            return dt;
        }


        public static ViewDataset.VIEW_Becchu_ShukkaMeisaiDataTable
            getVIEW_Becchu_ShukkaMeisaiDataTable(BecchuOrderKey key, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = "select * from VIEW_Becchu_ShukkaMeisai WHERE SashizuBi=@z and SashizuNo=@n and ShiiresakiCode=@s order by HinbanNo ASC,RowNo";
            da.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);

            ViewDataset.VIEW_Becchu_ShukkaMeisaiDataTable dt = new ViewDataset.VIEW_Becchu_ShukkaMeisaiDataTable();
            da.Fill(dt);
            return dt;
        }

        public static ViewDataset.VIEW_DS_ShukkaMeisaiDataTable
            getVIEW_DS_ShukkaMeisaiDataTable(string strOrderKanriNo, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = "select * from VIEW_DS_ShukkaMeisai WHERE OrderKanriNo=@n";
            da.SelectCommand.Parameters.AddWithValue("@n", strOrderKanriNo);
            da.SelectCommand.CommandTimeout = 600000;

            ViewDataset.VIEW_DS_ShukkaMeisaiDataTable dt = new ViewDataset.VIEW_DS_ShukkaMeisaiDataTable();
            da.Fill(dt);
            return dt;
        }

        public static ViewDataset.VIEW_DS2_ShukkaMeisaiDataTable
            getVIEW_DS2_ShukkaMeisaiDataTable(string strOrderKanriNo, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);
            
            //da.SelectCommand.CommandText = "select * from VIEW_DS2_ShukkaMeisai WHERE OrderKanriNo=@n";
            //da.SelectCommand.CommandText = "select OrderKanriNo,JyougeKubun,Hinban,Size,RowNo,Suryou,ShukkaSuu,isnull(KanryouFlg,0) as KanryouFlg  from VIEW_DS2_ShukkaMeisai WHERE OrderKanriNo=@n";
            da.SelectCommand.CommandText = "select OrderKanriNo,JyougeKubun,Hinban,Size,RowNo,Suryou,ShukkaSuu,isnull(KanryouFlg,0) as KanryouFlg,JanCode,LotNo  from VIEW_DS2_ShukkaMeisai WHERE OrderKanriNo=@n";
            
            da.SelectCommand.Parameters.AddWithValue("@n", strOrderKanriNo);
            da.SelectCommand.CommandTimeout = 600000;

            ViewDataset.VIEW_DS2_ShukkaMeisaiDataTable dt = new ViewDataset.VIEW_DS2_ShukkaMeisaiDataTable();
            da.Fill(dt);
            return dt;
        }

        public static ViewDataset.VIEW_SP_ShukkaMeisaiDataTable
            getVIEW_SP_ShukkaMeisaiDataTable(BecchuOrderClass.BecchuOrderKey key, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = "select * from VIEW_SP_ShukkaMeisai WHERE ShiiresakiCode=@s and OrderKanriNo=@n";
            da.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
            da.SelectCommand.CommandTimeout = 600000;

            ViewDataset.VIEW_SP_ShukkaMeisaiDataTable dt = new ViewDataset.VIEW_SP_ShukkaMeisaiDataTable();
            da.Fill(dt);
            return dt;
        }


        public static MizunoDataSet.T_BecchuBikouRow
            getT_BecchuBikouRow(BecchuOrderKey key, EnumUserType type, string strSeisanKubun, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = "select * from T_BecchuBikou WHERE MizunoUketsukeBi=@z and OrderKanriNo=@n and ShiiresakiCode=@s and UserKubun=@u and SeisanKubun=@sk";
            da.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@u", (byte)type);
            da.SelectCommand.Parameters.AddWithValue("@sk", strSeisanKubun);

            MizunoDataSet.T_BecchuBikouDataTable dt = new MizunoDataSet.T_BecchuBikouDataTable();
            da.Fill(dt);
            if (1 == dt.Count) return dt[0];
            return null;
        }


        public class MeisaiData : NouhinDataClass.MeisaiDataCommon
        {
            public class MeisaiKey
            {
                public string Hinban { get; set; }
                public string Size { get; set; }
                public int RowNo { get; set; }  // ����͕ʒ�2�̎��̂ݎg�p
                public string LotNo { get; set; }
                //public decimal? Kakaku { get; set; }    // ���g�p(�{���A�i��+�T�C�Y+���i�P�ʂŖ��ׂ͓o�^���������A�i��+�T�C�Y�ňقȂ鉿�i�ƂȂ�f�[�^�������̂Ŏg�p���Ȃ�)
            }

            public MeisaiKey Key = new MeisaiKey();


            public MeisaiData Copy()
            {
                MeisaiData m = new MeisaiData();
                NouhinDataClass.MeisaiDataCommon c = (NouhinDataClass.MeisaiDataCommon)m;
                this.Copy(ref c);
                m.Key.Hinban = this.Key.Hinban;
                m.Key.Size = this.Key.Size;
                m.Key.RowNo = this.Key.RowNo;
                return m;
            }
        }

        public class TourokuData : NouhinDataClass.TourokuDataBase
        {
            public List<MeisaiData> lstMeisai = new List<MeisaiData>();
        }

        public static Core.Error
            DenpyouTouroku(bool bUpload, BecchuOrderKey b_bey, TourokuData data, SqlConnection c, out List<NouhinDataClass.DenpyouKey> lstKey, out List<NouhinDataClass.DenpyouKey> lstKeyShukkaAri)
        {
            lstKey = null;
            lstKeyShukkaAri = null;
            SqlTransaction t = null;
            try
            {
                c.Open();
                t = c.BeginTransaction();
                DenpyouTouroku(bUpload, false, b_bey, data, t, out lstKey, out lstKeyShukkaAri);
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

        internal static void DenpyouTouroku(bool bUpload, bool bShusei,BecchuOrderKey b_bey, TourokuData data, SqlTransaction t, out List<NouhinDataClass.DenpyouKey> lstKey, out List<NouhinDataClass.DenpyouKey> lstKeyShukkaAri)
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
            for (int i = 0; i < data.lstMeisai.Count; i++)
            {
                if (0 == data.lstMeisai[i].ShukkaSu && data.lstMeisai[i].KanryouFlag)
                {
                    // �y���ŗ��̔����ǉ��@20190821 M0458
                    int.TryParse(b_bey.UketsukeBi.Replace("/", ""), out nSashizuBi);
                    if (data.lstMeisai[i].KeigenZeiritsu == "R8K" && nSashizuBi >= nZeiritsuHenkouBi) // 2019�N10�����珈������
                        lstZeroKanryou_KeigenZeiritsu.Add(data.lstMeisai[i]);
                    else
                        lstZeroKanryou.Add(data.lstMeisai[i]);
                }
                else
                {
                    // �y���ŗ��̔����ǉ��@20190821 M0458
                    int.TryParse(b_bey.UketsukeBi.Replace("/", ""), out nSashizuBi);
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
                data.lstMeisai = lstDenpyou[i];
                NouhinDataClass.DenpyouKey key = DenpyouTouroku(bUpload, bShusei, 0, b_bey, data, t);
                lstKey.Add(key);
                if (i < nShukkaAriDenpyouCount)
                    lstKeyShukkaAri.Add(key);
            }
            data.lstMeisai = lstOrg;
        }

        private static NouhinDataClass.DenpyouKey DenpyouTouroku(bool bUpload, bool bShusei, int nHakkouNo, BecchuOrderKey key, TourokuData data, SqlTransaction t)
        {
            int nZenShukkaSuu = 0;

            SqlDataAdapter daKey = new SqlDataAdapter("", t.Connection);
            daKey.SelectCommand.CommandText = "SELECT * FROM VIEW_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            daKey.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", key.UketsukeBi);
            daKey.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", key.OrderKanriNo);
            daKey.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", key.ShiiresakiCode);
            daKey.SelectCommand.Transaction = t;
            ViewDataset.VIEW_BecchuKeyInfoDataTable dtKey = new ViewDataset.VIEW_BecchuKeyInfoDataTable();
            daKey.Fill(dtKey);
            if (0 == dtKey.Count) throw new Exception("�L�[���̎擾�Ɏ��s���܂����B");
            ViewDataset.VIEW_BecchuKeyInfoRow drKey = dtKey[0];

            //if (drKey.KanryouFlg) throw new Exception("���̒����͊������Ă��܂��B");  2011/8/6 ���c=0�Ŋ����������i�ԓ���ǉ��o�ׂł���悤�ɂ���׃R�����g�A�E�g

            if (!drKey.IsCancelBiNull())
                throw new Exception(string.Format("�I�[�_�[No{0}�̓L�����Z���ς݂ł�", key.OrderKanriNo));


            EnumBecchuKubun kubun = EnumBecchuKubun.Becchu;

            if (!drKey.IsT_Becchu_SashizuNoNull())
                kubun = EnumBecchuKubun.Becchu;
            else if (!drKey.IsT_Becchu2_SashizuNoNull())
                kubun = EnumBecchuKubun.Becchu2;
            else if (!drKey.IsT_DS_OrderKanriNoNull())
                kubun = EnumBecchuKubun.DS;
            else if (!drKey.IsT_SP_OrderKanriNoNull())
                kubun = EnumBecchuKubun.SP;
            else
                throw new Exception("�ʒ��i�̃f�[�^��������܂���ł����B");



            // �ő�̔��sNo�擾
            SqlCommand cmdGetMaxHakkouNo = new SqlCommand("", t.Connection);
            cmdGetMaxHakkouNo.Transaction = t;
            cmdGetMaxHakkouNo.CommandText = @"SELECT MAX(HakkouNo) AS HacchuNo FROM T_NouhinHeader WHERE (NouhinmotoShiiresakiCode = @s) AND (YYMM = @YYMM) ";
            cmdGetMaxHakkouNo.Parameters.AddWithValue("@s", drKey.ShiiresakiCode);
            cmdGetMaxHakkouNo.Parameters.AddWithValue("@YYMM", int.Parse(data.dtHakkouBi.ToString("yyMM")));

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


            // �o�׏��
            SqlDataAdapter daShukkaInfo = new SqlDataAdapter("", t.Connection);
            daShukkaInfo.SelectCommand.CommandText = "select * from T_BecchuShukkaInfo where SashizuBi=@z and SashizuNo=@n and ShiiresakiCode=@s";
            daShukkaInfo.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
            daShukkaInfo.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
            daShukkaInfo.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            daShukkaInfo.SelectCommand.Transaction = t;
            daShukkaInfo.InsertCommand = new SqlCommandBuilder(daShukkaInfo).GetInsertCommand();
            daShukkaInfo.UpdateCommand = new SqlCommandBuilder(daShukkaInfo).GetUpdateCommand();
            MizunoDataSet.T_BecchuShukkaInfoDataTable dtShukkaInfo = new MizunoDataSet.T_BecchuShukkaInfoDataTable();
            daShukkaInfo.Fill(dtShukkaInfo);

            string ShukkaSakiYubinBangou = "";
            string ShukkaSakiJyusho = "";
            string ShukkaSakiTel = "";
            //string SouhinKubun = ""; //DS���i�敪�擾 20160210 �ǉ��@���R���񂲈˗� 20160215 �ǉ��@���R���񂲈˗��@4��5�ɂ��鏈���L�����Z��

            if (kubun == EnumBecchuKubun.Becchu)
            {
                // BS�i�o�א�Z���ATEL�擾�j
                SqlDataAdapter daBs = new SqlDataAdapter("", t.Connection);
                daBs.SelectCommand.CommandText = "select * from T_Becchu where SashizuBi=@z and SashizuNo=@n and ShiiresakiCode=@s";
                daBs.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
                daBs.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
                daBs.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
                daBs.SelectCommand.Transaction = t;

                MizunoDataSet.T_BecchuDataTable dtBs = new MizunoDataSet.T_BecchuDataTable();

                daBs.Fill(dtBs);

                if (dtBs.Count > 0)
                {
                    ShukkaSakiYubinBangou = dtBs[0].IsShukkaSakiYubinBangouNull() ? "" : dtBs[0].ShukkaSakiYubinBangou;
                    ShukkaSakiJyusho = dtBs[0].IsShukkaSakiJyushoNull() ? "" : dtBs[0].ShukkaSakiJyusho;
                    ShukkaSakiTel = dtBs[0].IsShukkaSakiTelNull() ? "" : dtBs[0].ShukkaSakiTel;
                }
            }
            else if (kubun == EnumBecchuKubun.Becchu2)
            {
                // BS2�i�o�א�Z���ATEL�擾�j
                SqlDataAdapter daBs2 = new SqlDataAdapter("", t.Connection);
                daBs2.SelectCommand.CommandText = "select * from T_Becchu2 where SashizuBi=@z and SashizuNo=@n and ShiiresakiCode=@s";
                daBs2.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
                daBs2.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
                daBs2.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
                daBs2.SelectCommand.Transaction = t;

                MizunoDataSet.T_Becchu2DataTable dtBs2 = new MizunoDataSet.T_Becchu2DataTable();

                daBs2.Fill(dtBs2);

                if (dtBs2.Count > 0)
                {
                    ShukkaSakiYubinBangou = dtBs2[0].IsShukkaSakiYubinBangouNull() ? "" : dtBs2[0].ShukkaSakiYubinBangou;
                    ShukkaSakiJyusho = dtBs2[0].ShukkaSakiJyusho;
                    ShukkaSakiTel = dtBs2[0].ShukkaSakiTel;
                }
            }
            //else if (kubun == EnumBecchuKubun.DS) //20160210 �ǉ��@���R���񂲈˗��@�����̈����ɂ��� 20160215 �ǉ��@���R���񂲈˗��@4��5�ɖ߂������̓L�����Z��
            //{
            //    // DS2 (���i�敪�A�擾)
            //    SqlDataAdapter daDs2 = new SqlDataAdapter("", t.Connection);
            //    daDs2.SelectCommand.CommandText = "select * from T_DS2 where OrderKanriNo=@n";
            //    daDs2.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
            //    daDs2.SelectCommand.Transaction = t;

            //    MizunoDataSet.T_DS2DataTable dtDs2 = new MizunoDataSet.T_DS2DataTable();

            //    daDs2.Fill(dtDs2);

            //    if (dtDs2.Count > 0)
            //    {
            //        if (!dtDs2[0].IsTyokusouKubunNull())
            //        {
            //            if (((int)data.SouhinKubun) == 4 &&
            //                ((int)data.SouhinKubun).ToString() != dtDs2[0].TyokusouKubun.ToString())
            //                SouhinKubun = dtDs2[0].TyokusouKubun.ToString();
            //        }
            //    }
            //}

            // �����M�̔[�i�f�[�^�擾(�ǉ��i�Ԃ͏���)
            // ���ꕔ���[�Ŋ������́A�ŏI���M�Ώۂ̔[�i�f�[�^�Ɋ����t���O�������Ă��邩�`�F�b�N����B
            // T_NouhinMeisai.RowNo = -3 �͒ǉ��i�Ԃ̈Ӗ�
            // �ʒ��i�Œǉ��i�Ԃ�T_NouhinMeisai.HinbanTsuikaFlg = "1"���Z�b�g����郋�[���ł��邪�A���d�l�ł�"0"���Z�b�g����Ă���A
            // T_NouhinMeisai.RowNo = -3�ŋ�ʂ��Ă����B�����i�Ԃƒǉ��i�Ԃ�����œo�^�ł���P�[�X������A�ǂꂪ�����̕i�Ԃ��ǉ��̕i�Ԃ����ʂł����A
            // ���c���ɍ��قŏo�����Ƃ̑Ή��������悤�Bby ykuri 2011/7/5
            SqlDataAdapter daGetMisoushin = new SqlDataAdapter("", t.Connection);
//            daGetMisoushin.SelectCommand.CommandText = @"
//SELECT                  dbo.T_NouhinMeisai.*, dbo.T_NouhinHeader.ShukkaBi
//FROM                     dbo.T_NouhinHeader INNER JOIN
//dbo.T_NouhinMeisai ON dbo.T_NouhinHeader.HakkouNo = dbo.T_NouhinMeisai.HakkouNo AND 
//dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = dbo.T_NouhinMeisai.ShiiresakiCode AND 
//dbo.T_NouhinHeader.YYMM = dbo.T_NouhinMeisai.YYMM
//WHERE (dbo.T_NouhinHeader.SoushinFlg = 0) AND (dbo.T_NouhinMeisai.HinbanTsuikaFlg = N'0' AND T_NouhinMeisai.RowNo>=0) AND  
//(dbo.T_NouhinHeader.SashizuBi = @z) AND  (dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = @s) and 
//(dbo.T_NouhinHeader.OrderKanriNo = @n) AND 
//(dbo.T_NouhinMeisai.Hinban = @Hinban) AND (dbo.T_NouhinMeisai.Size = @Size) AND (dbo.T_NouhinMeisai.LotNo = @LotNo)";
//            daGetMisoushin.SelectCommand.CommandText = @"
//SELECT                  dbo.T_NouhinMeisai.*, dbo.T_NouhinHeader.ShukkaBi
//FROM                     dbo.T_NouhinHeader INNER JOIN
//dbo.T_NouhinMeisai ON dbo.T_NouhinHeader.HakkouNo = dbo.T_NouhinMeisai.HakkouNo AND 
//dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = dbo.T_NouhinMeisai.ShiiresakiCode AND 
//dbo.T_NouhinHeader.YYMM = dbo.T_NouhinMeisai.YYMM
//WHERE (dbo.T_NouhinHeader.SoushinFlg = 0) AND (dbo.T_NouhinMeisai.HinbanTsuikaFlg = N'0' AND T_NouhinMeisai.RowNo>=0) AND  
//(dbo.T_NouhinHeader.SashizuBi = @z) AND  (dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = @s) and 
//(dbo.T_NouhinHeader.OrderKanriNo = @n) 
//AND (dbo.T_NouhinMeisai.Hinban = @Hinban) AND (dbo.T_NouhinMeisai.Size = @Size) AND (dbo.T_NouhinMeisai.LotNo = @LotNo)";

            daGetMisoushin.SelectCommand.CommandText = @"
SELECT                  dbo.T_NouhinMeisai.*, dbo.T_NouhinHeader.ShukkaBi
FROM                     dbo.T_NouhinHeader INNER JOIN
dbo.T_NouhinMeisai ON dbo.T_NouhinHeader.HakkouNo = dbo.T_NouhinMeisai.HakkouNo AND 
dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = dbo.T_NouhinMeisai.ShiiresakiCode AND 
dbo.T_NouhinHeader.YYMM = dbo.T_NouhinMeisai.YYMM
WHERE (dbo.T_NouhinHeader.SoushinFlg IN ('0','1','2')) AND (dbo.T_NouhinMeisai.HinbanTsuikaFlg = N'0' AND T_NouhinMeisai.RowNo>=0) AND  
(dbo.T_NouhinHeader.SashizuBi = @z) AND  (dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = @s) and 
(dbo.T_NouhinHeader.OrderKanriNo = @n) 
AND (dbo.T_NouhinMeisai.Hinban = @Hinban) AND (dbo.T_NouhinMeisai.Size = @Size) AND (dbo.T_NouhinMeisai.LotNo = @LotNo)";

            daGetMisoushin.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            daGetMisoushin.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
            daGetMisoushin.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
            daGetMisoushin.SelectCommand.Parameters.AddWithValue("@Hinban", "");
            daGetMisoushin.SelectCommand.Parameters.AddWithValue("@Size", "");
            daGetMisoushin.SelectCommand.Parameters.AddWithValue("@LotNo", "");
            daGetMisoushin.SelectCommand.Transaction = t;

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
                    cmdGetMaxHakkouNo2.CommandText = @"SELECT MAX(HakkouNo) AS HacchuNo FROM T_NouhinHeader WHERE (NouhinmotoShiiresakiCode = @s) AND (YYMM = @YYMM) AND HakkouNo < 90000 ";
                    cmdGetMaxHakkouNo2.Parameters.AddWithValue("@s", drKey.ShiiresakiCode);
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
            else
            {
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
            drHeader.NouhinmotoShiiresakiCode = key.ShiiresakiCode;
            // �o�ד��͉�ʂőI�����ꂽ�[�i�L��
            drHeader.NouhinKigou = data.NouhinKigou;

            // ���Ə��R�[�h                
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


            //2013/11/07 ����
            //�ً}�������l
            drHeader.KinkyuChokusouBikou = data.KinkyuChokusouBikou;

            // �\�Z��
            drHeader.YosanTsuki = "";
            // �I�[�_���̔[�i�L��
            drHeader.OrderNouhinKigou = drKey.NouhinKigou;

            // �����
            drHeader.HanbaitenKigyouRyakuMei = data.HanbaitenKigyouRyakuMei;

            drHeader.SetShukkaNiukeBumonNull();


            // DS/SP�̎��������ɂ͎w�}No��ݒ肷��B(yyMMdd)
            // ���s���t
            drHeader.HakkouHiduke = "";
            if (!drKey.IsT_SP_OrderKanriNoNull() || !drKey.IsT_DS_OrderKanriNoNull())
            {
                if (key.OrderKanriNo.Length > 5)
                {
                    // 1�`6���܂�
                    drHeader.HakkouHiduke = key.OrderKanriNo.Substring(0, 6);
                }
            }
            else
            {
                // ������������
                string str = key.UketsukeBi.Replace("/", "");
                if (str.Length == 8)
                {
                    drHeader.HakkouHiduke = str.Substring(2, 6);
                }
            }

            // �o�ד��t                
            drHeader.ShukkaHiduke = data.dtHakkouBi.ToString("yyMMdd");
            // �o�ד�
            drHeader.ShukkaBi = data.dtHakkouBi;


            // YYMM
            drHeader.YYMM = int.Parse(data.dtHakkouBi.ToString("yyMM"));

            // ���i�敪
            drHeader.SouhinKubun = ((int)data.SouhinKubun).ToString();
            //���i�敪�̕t���ւ��@����5�œ��͎���4��������5�ɕύX���� 20160210 �ǉ��@���R���񂲈˗� 20160215 �ǉ��@���R���񂲈˗� 4��5�ɂ��鏈���L�����Z��
            //if (SouhinKubun != "") drHeader.SouhinKubun = SouhinKubun;

            // �w�}��
            drHeader.SashizuBi = key.UketsukeBi;
            // ���Y�I�[�_�[No
            drHeader.SeisanOrderNo = "";
            // ����I�[�_�[No
            drHeader.HikitoriOrderNo = "";
            // �ޗ��I�[�_�[No
            drHeader.ZairyouOrderNo = "";
            // �ʒ��I�[�_�[No
            drHeader.OrderKanriNo = key.OrderKanriNo;
            
            // ���M�t���O
            //2014/02/14 ���� �ǉ� �����No�ȂǓ��͂���Ă��Ȃ��ꍇ�͑��M�t���O=�������Ƃ���B
            //if (key.ShiiresakiCode.Substring(0, 2) == "29")
            //{
            //    //** �d���悪�C�O�̏ꍇ
            //    drHeader.SoushinFlg = SoushinFlg.NONE;
            //}
            //else
            //{
            //    //** �d���悪�����̏ꍇ
            if ((data.SouhinKubun == EnumSouhinKubun.Chokusou || data.SouhinKubun == EnumSouhinKubun.Nituu_GMS)
                && (data.invoiceNo == "" || (data.UnsouGyoushaCode == "" && data.UnsouGyoushaMei == "") || data.KogutiSu == ""))
            {
                drHeader.SoushinFlg = SoushinFlg.MIKAKUTEI;
            }
            else
            {
                drHeader.SoushinFlg = SoushinFlg.NONE;
            }
            //}
            
            
            // �[�i��
            drHeader.HonNouhinTsuki = "";


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

            drHeader.TourokuBi = DateTime.Now;
            dtHeader.Rows.Add(drHeader);


            // ----- ���� -----
            SqlDataAdapter daBecchu = new SqlDataAdapter("", t.Connection);
            SqlDataAdapter daBecchu2 = new SqlDataAdapter("", t.Connection);
            SqlDataAdapter daDS = new SqlDataAdapter("", t.Connection);
            //SqlDataAdapter daDS2 = new SqlDataAdapter("", t.Connection);
            SqlDataAdapter daSP = new SqlDataAdapter("", t.Connection);
            ViewDataset.VIEW_Becchu_ShukkaMeisaiDataTable dtBecchu = new ViewDataset.VIEW_Becchu_ShukkaMeisaiDataTable();
            ViewDataset.VIEW_Becchu2_ShukkaMeisaiDataTable dtBecchu2 = new ViewDataset.VIEW_Becchu2_ShukkaMeisaiDataTable();
            //ViewDataset.VIEW_DS_ShukkaMeisaiDataTable dtDS = new ViewDataset.VIEW_DS_ShukkaMeisaiDataTable();
            ViewDataset.VIEW_DS2_ShukkaMeisaiDataTable dtDS = new ViewDataset.VIEW_DS2_ShukkaMeisaiDataTable();
            ViewDataset.VIEW_SP_ShukkaMeisaiDataTable dtSP = new ViewDataset.VIEW_SP_ShukkaMeisaiDataTable();

            switch (kubun)
            {
                case EnumBecchuKubun.Becchu:
                    // VIEW_BecchuMeisai
                    daBecchu.SelectCommand.CommandText = "select * from VIEW_Becchu_ShukkaMeisai WHERE SashizuBi=@z and SashizuNo=@n and ShiiresakiCode=@s";
                    daBecchu.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
                    daBecchu.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
                    daBecchu.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
                    daBecchu.SelectCommand.Transaction = t;
                    daBecchu.Fill(dtBecchu);
                    dtBecchu.DefaultView.Sort = "HinbanNo, RowNo";
                    break;
                case EnumBecchuKubun.Becchu2:
                    // VIEW_Becchu2_ShukkaMeisai
                    daBecchu2.SelectCommand.CommandText = "select * from VIEW_Becchu2_ShukkaMeisai WHERE SashizuBi=@z and SashizuNo=@n and ShiiresakiCode=@s";
                    daBecchu2.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
                    daBecchu2.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
                    daBecchu2.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
                    daBecchu2.SelectCommand.Transaction = t;
                    daBecchu2.Fill(dtBecchu2);
                    dtBecchu2.DefaultView.Sort = "RowNo";
                    break;
                case EnumBecchuKubun.DS:
                    // DS
                    daDS.SelectCommand.CommandText = "select * from VIEW_DS2_ShukkaMeisai WHERE OrderKanriNo=@n";
                    daDS.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
                    daDS.SelectCommand.Transaction = t;
                    daDS.Fill(dtDS);
                    dtDS.DefaultView.Sort = "RowNo";
                    break;
                case EnumBecchuKubun.SP:
                    // SP
                    daSP.SelectCommand.CommandText = "select * from VIEW_SP_ShukkaMeisai WHERE OrderKanriNo=@n";
                    daSP.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
                    daSP.SelectCommand.Transaction = t;
                    daSP.Fill(dtSP);
                    dtSP.DefaultView.Sort = "No, MeisaiNo";
                    break;
            }

            int nGyouNo = 1;
            int nEdaban = 0;
            List<string> lstHinbanSizeLotNoRowNo = new List<string>();
            List<string> lstKanryouFlgHinbanSizeLotNoRowNo = new List<string>();  // �����t���O���Z�b�g����i�ԃT�C�Y
            MizunoDataSet.T_NouhinMeisaiDataTable dtM = new MizunoDataSet.T_NouhinMeisaiDataTable();

            //2014/12/16 �ǋL
            List<string> lstTorikeshiHinbanSizeNoRowNo = new List<string>();

            // 2013.03.02 �R�����g�A�E�g
            //bool bLotHikiate = false;

            for (int i = 0; i < data.lstMeisai.Count; i++)
            {
                MeisaiData m = data.lstMeisai[i];
                MizunoDataSet.T_NouhinMeisaiRow dr = dtM.NewT_NouhinMeisaiRow();
                NouhinDataClass.InitT_NouhinMeisaiRow(dr);
                dr.HakkouNo = nHakkouNo;
                dr.ShiiresakiCode = drHeader.NouhinmotoShiiresakiCode;
                dr.YYMM = drHeader.YYMM;

                // 2012-01-09 ꎓ��R�����g
                if (NouhinDataClass.NOUHINDATA_MAX_GYOU_COUNT < nGyouNo)
                {
                    // �s�ԍ���9�ȏ�̎�
                    nGyouNo = 1; // �߂�
                    nEdaban++;
                }

                dr.GyouNo = nGyouNo;
                dr.Edaban = nEdaban;

                int nRowNo = 0;

                dr.Hinban = m.Key.Hinban;
                dr.LotNo = m.Key.LotNo;
                dr.Tani = "";

                dr.HinbanTsuikaFlg = HinbanTsuikaFlg.Normal;

                string strHinbanSizeLotNoRowNo = string.Format("{0}\t{1}\t{2}\t{3}", m.Key.Hinban, m.Key.Size, m.Key.LotNo,m.Key.RowNo);
                if (!lstHinbanSizeLotNoRowNo.Contains(strHinbanSizeLotNoRowNo)) lstHinbanSizeLotNoRowNo.Add(strHinbanSizeLotNoRowNo);

                if (m.KanryouFlag)
                {
                    if (!lstKanryouFlgHinbanSizeLotNoRowNo.Contains(strHinbanSizeLotNoRowNo))
                        lstKanryouFlgHinbanSizeLotNoRowNo.Add(strHinbanSizeLotNoRowNo);
                }

                int nShukkaSu = m.ShukkaSu;

                // �i�Ԃ��ݒ肳��Ă��Ȃ��i�Ԃ͋����I�ɓ`�[���s�s�Ƃ��� 20200423 M0501 �ǉ��@����ς肢��Ȃ����Ă�!!! �X�{���񂩂� 20200424
                //if (dr.Hinban == "")
                //{
                //    throw new Exception("�i�ԂȂ��œ`�[���s�ł��܂���B");
                //}
                // ���ʂ�0�̕i�Ԃ͋����I�ɓ`�[���s�s�Ƃ��� 20200423 M0501 �ǉ�
                //if (nShukkaSu == 0)
                //{
                //    throw new Exception("����0�œ`�[���s�ł��܂���B");
                //}

                switch (kubun)
                {
                    // ����ʂ���̓o�^�̏ꍇ��UPLOAD����̓o�^�̏ꍇ�Ƃ��ɕi�ԁA�T�C�Y�A���b�gNo�ŕR�Â���OK

                    case EnumBecchuKubun.Becchu:
                        dtBecchu.DefaultView.RowFilter = string.Format(" Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", m.Key.Hinban.Replace("'", "''"), m.Key.Size.Replace("'", "''"), m.Key.LotNo.Replace("'", "''"));
                        if (!bShusei) dtBecchu.DefaultView.RowFilter += " AND (KanryouFlg is null or KanryouFlg=False)";

                        if (0 == dtBecchu.DefaultView.Count) throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�A���b�gNo.{2}�̔[���c������܂���B", m.Key.Hinban, m.Key.Size, m.Key.LotNo));

                        for (int y = 0; y < dtBecchu.DefaultView.Count; y++)
                        {
                            ViewDataset.VIEW_Becchu_ShukkaMeisaiRow d = dtBecchu.DefaultView[y].Row as ViewDataset.VIEW_Becchu_ShukkaMeisaiRow;
                            if (d.Suryou < d.ShukkaSuu) throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�A���b�gNo.{2}�ŏo�א��s����", m.Key.Hinban, m.Key.Size, m.Key.LotNo));
                            if (d.Suryou == d.ShukkaSuu) continue;
                            int nZan = (int)d.Suryou - d.ShukkaSuu;
                            int nHikiate = (nZan < nShukkaSu) ? nZan : nShukkaSu;
                            nZenShukkaSuu = d.ShukkaSuu + nShukkaSu;
                            d.ShukkaSuu += nHikiate;    // �f�[�^�Z�b�g�ō쐬�����ShukkaSuu��ReadOnly�ɂȂ�̂͂Ȃ��H�H
                            nShukkaSu -= nHikiate;
                            if (0 == nShukkaSu) break;
                        }
                        break;
                    case EnumBecchuKubun.Becchu2:
                        // ����ʂ���̓o�^�̏ꍇ�͕i�ԁA�T�C�Y�ARowNo�ŕR�t����
                        //   UPLOAD����̓o�^�̏ꍇ
                        //   ���b�gNo�����݂���@���i�ԁA�T�C�Y�A���b�gNo�ŕR�t����
                        //   ���b�gNo�����݂��Ȃ����i�ԁA�T�C�Y�ARowNo�ŕR�t����

                        if (bUpload)
                        {
                            dtBecchu2.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'",
                                m.Key.Hinban.Replace("'", "''"), m.Key.Size.Replace("'", "''"), m.Key.LotNo.Replace("'", "''"));

                            if (dtBecchu2.DefaultView.Count > 1)
                            {
                                // �i�ԁA�T�C�Y�A���b�gNo�ň������Ă��ł��Ȃ���΁A�ʒ��i����No�܂ŏ����ɉ�����
                                dtBecchu2.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}' AND RowNo = {3}",
                                    m.Key.Hinban.Replace("'", "''"), m.Key.Size.Replace("'", "''"), m.Key.LotNo.Replace("'", "''"), m.Key.RowNo);

                                if (dtBecchu2.DefaultView.Count > 1)
                                {
                                    throw new Exception(string.Format("�����ł��Ȃ����ׂ����݂��܂��B�i�i��{0}�A�T�C�Y{1}�A���b�gNo.{2}�A�ʒ��i����No.{3})", m.Key.Hinban, m.Key.Size, m.Key.LotNo, m.Key.RowNo));
                                }
                            }

                            if (0 == dtBecchu2.DefaultView.Count)
                                throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�A�sNo.{2}�̔[���c������܂���B", m.Key.Hinban, m.Key.Size, m.Key.RowNo));

                            /* 2013.03.02
                            if (!string.IsNullOrEmpty(m.Key.LotNo))
                            {
                                dtBecchu2.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'",
                                    m.Key.Hinban.Replace("'", "''"), m.Key.Size.Replace("'", "''"), m.Key.LotNo.Replace("'", "''"));

                                bLotHikiate = true;
                            }
                            else if (m.Key.RowNo > 0)
                            {
                                nRowNo = m.Key.RowNo;
                                dtBecchu2.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND RowNo={2}",
                                    m.Key.Hinban.Replace("'", "''"), m.Key.Size.Replace("'", "''"), m.Key.RowNo);
                            }
                            else
                            {
                                throw new Exception(string.Format("���b�gNo.�܂��͍sNo����͂��Ă��������B�i�i��{0}�A�T�C�Y{1}�j", m.Key.Hinban, m.Key.Size));
                            }
                            if (!bShusei) dtBecchu2.DefaultView.RowFilter += " AND (KanryouFlg is null or KanryouFlg=False)";

                            if (2 <= dtBecchu2.DefaultView.Count)
                                throw new Exception(string.Format("�����ł��Ȃ����ׂ����݂��܂��B�i�i��{0}�A�T�C�Y{1}�A���b�gNo.{2})", m.Key.Hinban, m.Key.Size, m.Key.LotNo));

                            if (0 == dtBecchu2.DefaultView.Count)
                                throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�A�sNo.{2}�̔[���c������܂���B", m.Key.Hinban, m.Key.Size, m.Key.RowNo));
                             */
                        }
                        else
                        {
                            nRowNo = m.Key.RowNo;
                            dtBecchu2.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND RowNo={2}",
                                m.Key.Hinban.Replace("'", "''"), m.Key.Size.Replace("'", "''"), m.Key.RowNo);
                            if (!bShusei) dtBecchu2.DefaultView.RowFilter += " AND (KanryouFlg is null or KanryouFlg=False)";

                            if (2 <= dtBecchu2.DefaultView.Count) throw new Exception("2���ȏ�擾����邱�Ƃ͖���");

                            if (0 == dtBecchu2.DefaultView.Count)
                                throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�A�sNo.{2}�̔[���c������܂���B", m.Key.Hinban, m.Key.Size, m.Key.RowNo));
                        }

                        if (1 == dtBecchu2.DefaultView.Count)
                        {
                            ViewDataset.VIEW_Becchu2_ShukkaMeisaiRow d = dtBecchu2.DefaultView[0].Row as ViewDataset.VIEW_Becchu2_ShukkaMeisaiRow;
                            if (d.Suuryou < d.ShukkaSuu) throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�ŏo�א��s����", m.Key.Hinban, m.Key.Size));
                            if (d.Suuryou > d.ShukkaSuu)
                            {
                                int nZan = d.Suuryou - d.ShukkaSuu;
                                int nHikiate = (nZan < nShukkaSu) ? nZan : nShukkaSu;
                                nZenShukkaSuu = d.ShukkaSuu + nShukkaSu;
                                d.ShukkaSuu += nHikiate;
                                nShukkaSu -= nHikiate;
                            }
                        }

                        break;
                    case EnumBecchuKubun.DS:
                        dtDS.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}'", m.Key.Hinban.Replace("'", "''"), m.Key.Size.Replace("'", "''"));
                        if (!bShusei) dtDS.DefaultView.RowFilter += " AND (KanryouFlg is null or KanryouFlg=False)";

                        if (0 == dtDS.DefaultView.Count)
                        {
                            ////**** ���C���i�Ԃ̉\�����l��
                            //bool bErr = false;

                            ////** �V���c
                            //dtDS.DefaultView.RowFilter = string.Format("SyatuStyleHinban='{0}'", m.Key.Hinban.Replace("'", "''"));
                            //if (dtDS.DefaultView.Count == 0)
                            //{
                            //    bErr = true;
                            //}


                            ////** �p���c
                            //dtDS.DefaultView.RowFilter = string.Format("PantuStyleHinban='{0}'", m.Key.Hinban.Replace("'", "''"));
                            //if (dtDS.DefaultView.Count == 0)
                            //{
                            //    bErr = true;
                            //}

                            //if (bErr == true)
                            //{
                            //    throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�̔[���c������܂���B", m.Key.Hinban, m.Key.Size));
                            //}
                        }


                        for (int y = 0; y < dtDS.DefaultView.Count; y++)
                        {
                            //ꎓ��ύX 2012-11-07
                            //ViewDataset.VIEW_DS_ShukkaMeisaiRow d = dtDS.DefaultView[y].Row as ViewDataset.VIEW_DS_ShukkaMeisaiRow;
                            ViewDataset.VIEW_DS2_ShukkaMeisaiRow d = dtDS.DefaultView[y].Row as ViewDataset.VIEW_DS2_ShukkaMeisaiRow;
                            if (d.Suryou < d.ShukkaSuu) throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�ŏo�א��s����", m.Key.Hinban, m.Key.Size));
                            if (d.Suryou == d.ShukkaSuu) continue;

                            int nZan = d.Suryou - d.ShukkaSuu;
                            nZenShukkaSuu = d.ShukkaSuu + nShukkaSu;

                            int nHikiate = (nZan < nShukkaSu) ? nZan : nShukkaSu;
                            d.ShukkaSuu += nHikiate;
                            nShukkaSu -= nHikiate;
                            if (0 == nShukkaSu) break;
                        }
                        break;
                    case EnumBecchuKubun.SP:
                        //20200325 LotNo�ǉ� LotNo���Ȃ����ƂŁA�`�[�f�[�^�A�b�v���[�h����LotNo�̈Ⴂ�ɂ��G���[��������������
                        dtSP.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", m.Key.Hinban.Replace("'", "''"), m.Key.Size.Replace("'", "''"), m.Key.LotNo.Replace("'", "''"));
                        if (!bShusei) dtSP.DefaultView.RowFilter += " AND (KanryouFlg is null or KanryouFlg=False)";
                        if (0 == dtSP.DefaultView.Count) throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�A���b�gNo{2}�̔[���c������܂���B", m.Key.Hinban, m.Key.Size, m.Key.LotNo));
                        for (int y = 0; y < dtSP.DefaultView.Count; y++)
                        {
                            ViewDataset.VIEW_SP_ShukkaMeisaiRow d = dtSP.DefaultView[y].Row as ViewDataset.VIEW_SP_ShukkaMeisaiRow;
                            if (d.Suuryou < d.ShukkaSuu) throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�A���b�gNo{2}�ŏo�א��s����", m.Key.Hinban, m.Key.Size, m.Key.LotNo));
                            if (d.Suuryou == d.ShukkaSuu) continue;
                            int nZan = d.Suuryou - d.ShukkaSuu;
                            int nHikiate = (nZan < nShukkaSu) ? nZan : nShukkaSu;
                            nZenShukkaSuu = d.ShukkaSuu + nShukkaSu;
                            d.ShukkaSuu += nHikiate;

                            nShukkaSu -= nHikiate;
                            if (0 == nShukkaSu) break;
                        }
                        break;

                }

                if (0 < nShukkaSu)
                {
                    // �������Ăł��Ȃ���
                    string noLotNoMsg = string.IsNullOrEmpty(m.Key.LotNo) ? "" : "�A���b�gNo" + m.Key.LotNo;
                    throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}{2}�͏o�א����������𒴂��Ă��܂��B", m.Key.Hinban, m.Key.Size, noLotNoMsg));
                }

                // ���o�׏����擾
                MizunoDataSet.T_BecchuShukkaInfoRow drShukkaInfo = null;

                if (bUpload)
                {
                    switch (kubun)
                    {
                        case EnumBecchuKubun.Becchu:
                        case EnumBecchuKubun.DS:
                        case EnumBecchuKubun.SP:
                            {
                                drShukkaInfo = dtShukkaInfo.FindBySashizuBiSashizuNoShiiresakiCodeHinbanSizeLotNoRowNo(key.UketsukeBi, key.OrderKanriNo, key.ShiiresakiCode, m.Key.Hinban, m.Key.Size, m.Key.LotNo, nRowNo);
                            }
                            break;
                        case EnumBecchuKubun.Becchu2:
                            {
                                nRowNo = m.Key.RowNo;

                                //RowNo�Ȃ����� 2�s�ڈȍ~����RowNo�����Ⴄ�ꍇ�͒ʂ��Ă��܂��̂ł́c�H
                                dtShukkaInfo.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'",
                                    m.Key.Hinban.Replace("'", "''"), m.Key.Size.Replace("'", "''"), m.Key.LotNo.Replace("'", "''"));

                                if (dtShukkaInfo.DefaultView.Count > 1)
                                {
                                    //RowNo���茟��
                                    dtShukkaInfo.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}' AND RowNo={3}",
                                    m.Key.Hinban.Replace("'", "''"), m.Key.Size.Replace("'", "''"), m.Key.LotNo.Replace("'", "''"), m.Key.RowNo);

                                    //���ꂪ����Ӗ����킩��Ȃ� RowNo���Ⴄ���ׂ�3�s�ȏ�o�^���悤�Ƃ����ꍇ�ɃG���[�ɂȂ� 20161021
                                    //if (dtShukkaInfo.DefaultView.Count != 1)
                                    //{
                                    //    throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�A���b�gNo.{2}�A�ʒ��i����No.{3}�ł͈������ł��܂���B", m.Key.Hinban, m.Key.Size, m.Key.LotNo, m.Key.RowNo));
                                    //}
                                }

                                if (dtShukkaInfo.DefaultView.Count == 1)
                                {
                                    //RowNo���Ȃ����A�ȑO�o�^�������̂�����ꍇ�͏����� 20161021
                                    if (dtShukkaInfo.DefaultView[0].Row["RowNo"].ToString() == "0" ||
                                        dtShukkaInfo.DefaultView[0].Row["RowNo"].ToString() == nRowNo.ToString())
                                        drShukkaInfo = (MizunoDataSet.T_BecchuShukkaInfoRow)dtShukkaInfo.DefaultView[0].Row;
                                }
                            }
                            break;
                    }
                }
                else
                {
                    drShukkaInfo = dtShukkaInfo.FindBySashizuBiSashizuNoShiiresakiCodeHinbanSizeLotNoRowNo(key.UketsukeBi, key.OrderKanriNo, key.ShiiresakiCode, m.Key.Hinban, m.Key.Size, m.Key.LotNo, nRowNo);
                }

                if (null == drShukkaInfo)
                {
                    drShukkaInfo = dtShukkaInfo.NewT_BecchuShukkaInfoRow();
                    drShukkaInfo.SashizuBi = key.UketsukeBi;
                    drShukkaInfo.ShiiresakiCode = key.ShiiresakiCode;
                    drShukkaInfo.SashizuNo = key.OrderKanriNo;
                    drShukkaInfo.Hinban = m.Key.Hinban;
                    drShukkaInfo.Size = m.Key.Size;
                    drShukkaInfo.LotNo = m.Key.LotNo;
                    drShukkaInfo.RowNo = nRowNo;

                    if (kubun == EnumBecchuKubun.Becchu2)
                        drShukkaInfo.KanryouFlg = m.KanryouFlag;    // BS2�̏ꍇ�͕i�ԃT�C�Y�P�ʂŊ����t���O���Z�b�g���ꂸ�A���גP�ʂׁ̈A�����ŃZ�b�g���Ă���
                    else
                        drShukkaInfo.KanryouFlg = false;    //�@�����t���O�͌�ŃZ�b�g����B


                    if (kubun == EnumBecchuKubun.Becchu)
                    {
                        drShukkaInfo.ShukkaSuu = m.ShukkaSu;
                    }
                    else
                    {
                        drShukkaInfo.ShukkaSuu = nZenShukkaSuu;
                    }
                    dtShukkaInfo.Rows.Add(drShukkaInfo);
                }
                else
                {
                    if (!bShusei)
                    {
                        if (drShukkaInfo.KanryouFlg)
                            throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�͏o�ׂ��������Ă��܂��B", m.Key.Hinban, m.Key.Size));
                    }
                    drShukkaInfo.ShukkaSuu += m.ShukkaSu;
                    //drShukkaInfo.ShukkaSuu = nZenShukkaSuu;
                    if (kubun == EnumBecchuKubun.Becchu2)
                        drShukkaInfo.KanryouFlg = m.KanryouFlag;  // BS2�̏ꍇ�͕i�ԃT�C�Y�P�ʂŊ����t���O���Z�b�g���ꂸ�A���גP�ʂׁ̈A�����ŃZ�b�g���Ă���
                }

                // �����t���O
                dr.KanryouFlg = KanryouFlg.MiKanryou;   // �����t���O�͌�Őݒ肷��̂Ŗ������ɂ��Ă���
                // �{���ׂɂ́A����i��+�T�C�Y�̃f�[�^���������݂���P�[�X������B�i����i�ԃT�C�Y�ŉ��i���قȂ�ꍇ�j
                // �����t���O�́A�i��+�T�C�Y�P�ʂŐݒ肷�邪�A�������R�[�h�ɐݒ�o�����A�Ō�̃��R�[�h�ɑ΂��Đݒ肷��K�v����B

                dr.Size = m.Key.Size;

                dr.LotNo = m.Key.LotNo;

                // ����P��
                // 2013.01.17 �g��l�̈˗��ŁA����P���͓��͍��ڂł͂Ȃ����߁A�����`�F�b�N���͂������B
                // �@�@�@�@�@ ���̉e���ɂ��A���L�C�����s�����B
                dr.TorihikiTanka = m.TourokuKakaku.ToString();
                //dr.TorihikiTanka = m.TourokuKakaku.ToString("000000.0").Replace(".", "");
                // �[�i��
                dr.NouhinSuu = m.ShukkaSu;
                dr.NouhinKubun = (m.NouhinKubun == NouhinDataClass.EnumNouhinKubun.TujouNouhin) ? "" : ((int)m.NouhinKubun).ToString();


                // 2014/12/16 �ǉ�
                if (m.ShukkaSu == 0 && m.KanryouFlag == false)
                {
                    lstTorikeshiHinbanSizeNoRowNo.Add(lstHinbanSizeLotNoRowNo[i]);
                }

                // �Z�ʉ��i
                dr.YuuduuKakaku = m.YuuduuKakaku.ToString("0000000");
                // �Еt(2011/10/24 ���c�ے��̎w���Œǉ� by ykuri)
                dr.Sekiduke = m.Sekizuke.ToString("0000");

                // �E�v
                dr.ShouhinRyakuMei = m.Tekiyou.Trim();

                dr.RowNo = nRowNo;

                dr.FreeKoumoku1 = string.IsNullOrEmpty(m.Free1) ? "" : m.Free1;
                dr.FreeKoumoku2 = string.IsNullOrEmpty(m.Free2) ? "" : m.Free2;
                dr.FreeKoumoku3 = string.IsNullOrEmpty(m.Free3) ? "" : m.Free3;

                dr.Bikou = string.IsNullOrEmpty(m.Bikou) ? "" : m.Bikou;    // 31�����l



                dtM.Rows.Add(dr);

                nGyouNo++;

            }

            List<string> lstKannouHinbanSizeLotNo = new List<string>();


            if (kubun == EnumBecchuKubun.Becchu2)
            {
                // BS2�Ɍ����ẮA����i�ԃT�C�Y���u���b�gNo���u�����N�v�̖��ׂ�����A���ꂼ��Ŋ����t���O���Z�b�g�������B
                // �e�i�ԃT�C�Y�̑S���ׂ������i���[�j�����^�C�~���O�Ŕ[�i�f�[�^�ɂ͊����t���O�i9�j���Z�b�g����B
                DataView dvBS2 = new DataView(dtBecchu2);
                for (int i = 0; i < lstHinbanSizeLotNoRowNo.Count; i++)
                {
                    string strHinban = lstHinbanSizeLotNoRowNo[i].Split('\t')[0].Replace("'", "''");
                    string strSize = lstHinbanSizeLotNoRowNo[i].Split('\t')[1].Replace("'", "''");
                    string strLotNo = lstHinbanSizeLotNoRowNo[i].Split('\t')[2].Replace("'", "''");
                    dvBS2.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", strHinban, strSize, strLotNo);
                    int nMishukka = 0;
                    if (0 == dvBS2.Count) continue;
                    for (int k = 0; k < dvBS2.Count; k++)
                    {
                        ViewDataset.VIEW_Becchu2_ShukkaMeisaiRow dr = dvBS2[k].Row as ViewDataset.VIEW_Becchu2_ShukkaMeisaiRow;
                        if (dr.Suuryou <= dr.ShukkaSuu) continue;

                        MizunoDataSet.T_BecchuShukkaInfoRow drShukkaInfo = null;
                        if (bUpload)
                        {
                            dtShukkaInfo.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'",
                                dr.Hinban.Replace("'", "''"), dr.Size.Replace("'", "''"), dr.LotNo.Replace("'", "''"));

                            if (dtShukkaInfo.DefaultView.Count > 1)
                            {
                                dtShukkaInfo.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}' AND RowNo={3}",
                                    dr.Hinban.Replace("'", "''"), dr.Size.Replace("'", "''"), dr.LotNo.Replace("'", "''"), dr.RowNo);

                                //���ꂪ����Ӗ����킩��Ȃ� RowNo���Ⴄ���ׂ�3�s�ȏ�o�^���悤�Ƃ����ꍇ�ɃG���[�ɂȂ� 20161021
                                //if (dtShukkaInfo.DefaultView.Count != 1)
                                //{
                                //    throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�A���b�gNo.{2}�A�ʒ��i����No.{3}�ł͈������ł��܂���B", dr.Hinban, dr.Size, dr.LotNo, dr.RowNo));
                                //}
                            }

                            if (dtShukkaInfo.DefaultView.Count == 1)
                            {
                                drShukkaInfo = (MizunoDataSet.T_BecchuShukkaInfoRow)dtShukkaInfo.DefaultView[0].Row;
                            }
                        }
                        else
                        {
                            drShukkaInfo = dtShukkaInfo.FindBySashizuBiSashizuNoShiiresakiCodeHinbanSizeLotNoRowNo(key.UketsukeBi, key.OrderKanriNo, key.ShiiresakiCode, dr.Hinban, dr.Size, dr.LotNo, dr.RowNo);
                        }

                        if (null != drShukkaInfo && drShukkaInfo.KanryouFlg) continue;  // MizunoDataSet.T_BecchuShukkaInfoRow�̊����t���O�͐�ɃZ�b�g����Ă���B

                        nMishukka++;
                    }
                    if (0 == nMishukka)
                        lstKannouHinbanSizeLotNo.Add(lstHinbanSizeLotNoRowNo[i]);  // �{�i�ԃT�C�Y���S�Ċ����������͏o�׍ς݂Ŋ��[�Ƃ���B
                }
            }
            else
            {
                // �S���o�ׂŊ����t���O���Z�b�g�o����i��+�T�C�Y���擾
                for (int i = 0; i < lstHinbanSizeLotNoRowNo.Count; i++)
                {
                    string strHinban = lstHinbanSizeLotNoRowNo[i].Split('\t')[0].Replace("'", "''");
                    string strSize = lstHinbanSizeLotNoRowNo[i].Split('\t')[1].Replace("'", "''");
                    string strLotNo = lstHinbanSizeLotNoRowNo[i].Split('\t')[2].Replace("'", "''");
                    int nChumonSu = -1;
                    int nShukkaSu = 0;
                    switch (kubun)
                    {
                        case EnumBecchuKubun.Becchu:
                            nChumonSu = Convert.ToInt32(dtBecchu.Compute("SUM(Suryou)", string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", strHinban, strSize, strLotNo)));
                            nShukkaSu = Convert.ToInt32(dtBecchu.Compute("SUM(ShukkaSuu)", string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", strHinban, strSize, strLotNo)));
                            break;
                        //case EnumBecchuKubun.Becchu2:
                        //nChumonSu = Convert.ToInt32(dtBecchu2.Compute("SUM(Suuryou)", string.Format("Hinban='{0}' AND Size='{1}'", strHinban, strSize)));
                        //nShukkaSu = Convert.ToInt32(dtBecchu2.Compute("SUM(ShukkaSuu)", string.Format("Hinban='{0}' AND Size='{1}'", strHinban, strSize)));

                        //  break;
                        case EnumBecchuKubun.DS:
                            nChumonSu = Convert.ToInt32(dtDS.Compute("SUM(Suryou)", string.Format("Hinban='{0}' AND Size='{1}'", strHinban, strSize)));
                            nShukkaSu = Convert.ToInt32(dtDS.Compute("SUM(ShukkaSuu)", string.Format("Hinban='{0}' AND Size='{1}'", strHinban, strSize)));
                            break;
                        case EnumBecchuKubun.SP:
                            //20200325 LotNo�ǉ� LotNo���Ȃ����ƂŁA�`�[�f�[�^�A�b�v���[�h����LotNo�̈Ⴂ�ɂ��G���[��������������
                            nChumonSu = Convert.ToInt32(dtSP.Compute("SUM(Suuryou)", string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", strHinban, strSize, strLotNo)));
                            nShukkaSu = Convert.ToInt32(dtSP.Compute("SUM(ShukkaSuu)", string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", strHinban, strSize, strLotNo)));
                            break;

                    }
                    if (nChumonSu == nShukkaSu)
                    {
                        if (!lstKanryouFlgHinbanSizeLotNoRowNo.Contains(lstHinbanSizeLotNoRowNo[i]))
                            lstKanryouFlgHinbanSizeLotNoRowNo.Add(lstHinbanSizeLotNoRowNo[i]);
                        if (!lstKannouHinbanSizeLotNo.Contains(lstHinbanSizeLotNoRowNo[i]))
                            lstKannouHinbanSizeLotNo.Add(lstHinbanSizeLotNoRowNo[i]);
                    }
                }


                // �����t���O���Z�b�g����B����i�ԃT�C�Y�������܂܂��P�[�X������̂ŁA���ׂ̍Ō�ɑ΂��Ċ����t���O���Z�b�g����B
                for (int i = 0; i < lstKanryouFlgHinbanSizeLotNoRowNo.Count; i++)
                {
                    string strHinban = lstKanryouFlgHinbanSizeLotNoRowNo[i].Split('\t')[0];
                    string strSize = lstKanryouFlgHinbanSizeLotNoRowNo[i].Split('\t')[1];
                    string strLotNo = lstKanryouFlgHinbanSizeLotNoRowNo[i].Split('\t')[2];
                    dtM.DefaultView.RowFilter = string.Format("Hinban='{0}' and Size='{1}' and LotNo='{2}'", strHinban.Replace("'", "''"), strSize.Replace("'", "''"), strLotNo.Replace("'", "''"));
                    dtM.DefaultView.Sort = "Edaban, GyouNo";

                    MizunoDataSet.T_NouhinMeisaiRow drLast = dtM.DefaultView[dtM.DefaultView.Count - 1].Row as MizunoDataSet.T_NouhinMeisaiRow; // ���Ƀ\�[�g���Ȃ��Ă����̖���
                    drLast.KanryouFlg = KanryouFlg.Kanryou; // �Ō�̃��R�[�h�ɑ΂��Đݒ肷��B


                    // �o�׏��ɂ������t���O���Z�b�g����B(�ʒ�2�ȊO�́A�i��+�T�C�Y�łP�����Y������B�ʒ�2��RowNo�P�ʂ����i��+�T�C�Y�P�ʂŊ����t���O���Z�b�g����̂ŁA�S�ĂɑΉ�����B)
                    // �債�������łȂ��̂ŁA�P���Ƀ��[�v�Ō���
                    for (int k = 0; k < dtShukkaInfo.Count; k++)
                    {
                        MizunoDataSet.T_BecchuShukkaInfoRow drShukkaInfo = dtShukkaInfo[k];
                        if (drShukkaInfo.Hinban == strHinban && drShukkaInfo.Size == strSize && drShukkaInfo.LotNo == strLotNo)
                        {
                            drShukkaInfo.KanryouFlg = true;
                        }
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
            drTrailer.ShiiresakiCode = key.ShiiresakiCode;

            // �׎�S���҃R�[�h
            drTrailer.EigyouTantoushaCode = data.EigyouTantoushaCode;
            // �w�Z���A�`�[����
            drTrailer.TeamMei = data.TeamMei;
            // ���Y���S���҃R�[�h
            drTrailer.SKTantoushaCode = data.SKTantoushaCode;
            // �X��
            drTrailer.TenMei = data.TenMei;

            // �o�א�X�֔ԍ�
            drTrailer.ShukkaSakiYubinBangou = ShukkaSakiYubinBangou;

            // �o�א�Z��
            drTrailer.ShukkaSakiJyusho = ShukkaSakiJyusho;

            // �o�א�TEL
            drTrailer.ShukkaSakiTel = ShukkaSakiTel;

            // �^�����@
            drTrailer.UnsouHouhou = "";

            // �`�[���s��            
            drTrailer.NouhinShoHiduke = data.dtHakkouBi.ToString("yyMMdd");

            // �\��(����No)
            // DS�ASP�̏ꍇ
            if (kubun == EnumBecchuKubun.DS || kubun == EnumBecchuKubun.SP)
            {
                if (key.OrderKanriNo.Length > 6)
                {
                    // 7�`14���܂�
                    drTrailer.Yobi2 = key.OrderKanriNo.Substring(6, key.OrderKanriNo.Length - 6);
                }
                else
                {
                    // �S��
                    drTrailer.Yobi2 = key.OrderKanriNo;
                }
            }
            else
            {
                drTrailer.Yobi2 = key.OrderKanriNo;
            }
            dtTrailer.Rows.Add(drTrailer);

            SqlDataAdapter daHeader = new SqlDataAdapter("select * from T_NouhinHeader", t.Connection);
            daHeader.SelectCommand.Transaction = t;
            daHeader.InsertCommand = new SqlCommandBuilder(daHeader).GetInsertCommand();
            daHeader.InsertCommand.Transaction = t;
            daHeader.Update(dtHeader);


            daM.Update(dtM);
            daTrailer.Update(dtTrailer);
            daShukkaInfo.Update(dtShukkaInfo);

            NouhinDataClass.DenpyouKey dk = new NouhinDataClass.DenpyouKey(key.ShiiresakiCode, int.Parse(data.dtHakkouBi.ToString("yyMM")), nHakkouNo);
            MizunoDataSet.T_NouhinMeisaiDataTable dtMisoushin = new MizunoDataSet.T_NouhinMeisaiDataTable();

            // ----- �����M���̊����t���O�̃`�F�b�N -----
            for (int i = 0; i < lstHinbanSizeLotNoRowNo.Count; i++)
            {
                string strHinbanSizeLotNo = lstHinbanSizeLotNoRowNo[i];
                string strHinban = strHinbanSizeLotNo.Split('\t')[0];
                string strSize = strHinbanSizeLotNo.Split('\t')[1];
                string strLotNo = strHinbanSizeLotNo.Split('\t')[2];

                // �����M�f�[�^�擾

                /// 2014/05/26 �g�����U�N�V�������̓f�[�^�x�[�X����̎擾�͕s��
                /// 
                daGetMisoushin.SelectCommand.Parameters["@Hinban"].Value = strHinban;
                daGetMisoushin.SelectCommand.Parameters["@Size"].Value = strSize;
                daGetMisoushin.SelectCommand.Parameters["@LotNo"].Value = strLotNo;
                dtMisoushin.Clear();
                daGetMisoushin.Fill(dtMisoushin);
                if (0 == dtMisoushin.Count) continue;
                DataView dv = new DataView(dtMisoushin);

                /// ���׃e�[�u���ɂ́uShukkaBi�v�������ׁA�G���[
                //dv.Sort = "ShukkaBi, YYMM, HakkouNo, Edaban, GyouNo";  // // ���̏��ԂōŌ�̃��R�[�h���ŏI�[�i�f�[�^(�Ō�Ƀ~�Y�m�ɑ��M����f�[�^)�B1�`�[���ɓ���i�ԃT�C�Y���o�����邱�Ƃ�����i���i���قȂ�j�ׁA�}�ԁA�sNo�̏����Ŗ��׏��ōŌ�Ɋ����t���O���Z�b�g�����悤�l�����Ă���B
                dv.Sort = "YYMM, HakkouNo, Edaban, GyouNo";  // // ���̏��ԂōŌ�̃��R�[�h���ŏI�[�i�f�[�^(�Ō�Ƀ~�Y�m�ɑ��M����f�[�^)�B1�`�[���ɓ���i�ԃT�C�Y���o�����邱�Ƃ�����i���i���قȂ�j�ׁA�}�ԁA�sNo�̏����Ŗ��׏��ōŌ�Ɋ����t���O���Z�b�g�����悤�l�����Ă���B

                //DataView dv = new DataView(dtM);
                //dv.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", strHinban, strSize, strLotNo);

                //if (0 == dv.Count) continue;

                //dv.Sort = "YYMM, HakkouNo, Edaban, GyouNo"; 


                if (lstKannouHinbanSizeLotNo.Contains(strHinbanSizeLotNo))
                {
                    // �S���o�׍ς݂Ȃ̂ŁA�Ō�̔[�i�f�[�^�Ɋ����t���O�i9�j���Z�b�g����B
                    /* �`�[�C�����A�����t���O���Z�b�g����BS2���ׂƈقȂ�ꏊ�Ɋ����t���O���Z�b�g����Ă��܂��׎~�߂�B
                    dv.RowFilter = "NouhinSuu>0";   // �[�i��=0�̓`�[������ׁA�[�i��>0�ōŌ�̖��ׂɊ����t���O���Z�b�g�������B(�`�[�C���Ŕ[�i��=0�̓`�[(����`�[)�̏o�ד����Ō�i�Ō�̓`�[�ɂȂ邱�Ƃ�����j)
                    if (0 == dv.Count)
                        dv.RowFilter = null;    // �[�i��>0�̓`�[�������A�[�i����0�̓`�[�Ɋ����t���O���Z�b�g���Ȃ��Ƃ����Ȃ��B�i��{�I�ɂ��̃P�[�X�͖����͂��j
                    */

                    for (int k = 0; k < dv.Count; k++)
                    {
                        MizunoDataSet.T_NouhinMeisaiRow dr = dv[k].Row as MizunoDataSet.T_NouhinMeisaiRow;
                        dr.KanryouFlg = (dv.Count - 1 == k) ? KanryouFlg.Kanryou : KanryouFlg.MiKanryou;
                    }
                    daM.Update(dtMisoushin);
                }
                else
                {
                    // �o�׎c���������ԂŊ����̏ꍇ�A�������ꂩ�̔[�i�f�[�^�Ɋ����t���O�������Ă���ہA�ŏI�[�i�f�[�^�ɂɊ����t���O�������Ă��邩�`�F�b�N
                    dv.RowFilter = string.Format("KanryouFlg='{0}'", KanryouFlg.Kanryou);
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
                                strHinban, string.IsNullOrEmpty(strSize) ? "�Ȃ�" : strSize,
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
                                new Core.Type.Nengetu(drKanryouFlg.YYMM).ToDateTime(),
                                drKanryouFlg.HakkouNo, strHinban, string.IsNullOrEmpty(strSize) ? "�Ȃ�" : strSize, strMsg));
                        }
                    }


                }


                // 2014/12/18 �ǋL�@M0056
                // �`�[������A���Y�f�[�^�̕��[���ɗ����Ă��銮���t���O��Q������
                if (lstTorikeshiHinbanSizeNoRowNo.Contains(strHinbanSizeLotNo))
                {
                    for (int k = 0; k < dv.Count; k++)
                    {
                        MizunoDataSet.T_NouhinMeisaiRow dr = dv[k].Row as MizunoDataSet.T_NouhinMeisaiRow;
                        if (dr.KanryouFlg == "9")
                        {
                            dr.KanryouFlg = "0";
                        }
                    }
                    daM.Update(dtMisoushin);
                }
                
            }

            // 2014/12/16 �ǋL�@M0056
            //for (int i = 0; i < lstHinbanSizeLotNo.Count; i++)
            //{
            //    string strHinbanSizeLotNo = lstHinbanSizeLotNo[i];
            //    string strHinban = strHinbanSizeLotNo.Split('\t')[0];
            //    string strSize = strHinbanSizeLotNo.Split('\t')[1];
            //    string strLotNo = strHinbanSizeLotNo.Split('\t')[2];

            //    // �����M�f�[�^�擾

            //     //2014/05/26 �g�����U�N�V�������̓f�[�^�x�[�X����̎擾�͕s��
                 
            //    daGetMisoushin.SelectCommand.Parameters["@Hinban"].Value = strHinban;
            //    daGetMisoushin.SelectCommand.Parameters["@Size"].Value = strSize;
            //    daGetMisoushin.SelectCommand.Parameters["@LotNo"].Value = strLotNo;
            //    dtMisoushin.Clear();
            //    daGetMisoushin.Fill(dtMisoushin);
            //    if (0 == dtMisoushin.Count) continue;
            //    DataView dv = new DataView(dtMisoushin);

            //    dv.Sort = "YYMM, HakkouNo, Edaban, GyouNo";  // // ���̏��ԂōŌ�̃��R�[�h���ŏI�[�i�f�[�^(�Ō�Ƀ~�Y�m�ɑ��M����f�[�^)�B1�`�[���ɓ���i�ԃT�C�Y���o�����邱�Ƃ�����i���i���قȂ�j�ׁA�}�ԁA�sNo�̏����Ŗ��׏��ōŌ�Ɋ����t���O���Z�b�g�����悤�l�����Ă���B
                
                
            //}

            // �ēx�����_�̏o�׏󋵎擾���Ċe�����P�ʂŁA�������邢�͊��[���Ă����T_BecchuKeyInfo�̊����t���O���Z�b�g����B
            bool bKanryou = false;
            switch (kubun)
            {
                case EnumBecchuKubun.Becchu:
                    dtBecchu.Clear();
                    daBecchu.Fill(dtBecchu);
                    dtBecchu.DefaultView.RowFilter = "(KanryouFlg is null or KanryouFlg=False) and Suryou>ShukkaSuu";
                    bKanryou = (0 == dtBecchu.DefaultView.Count);
                    break;
                case EnumBecchuKubun.Becchu2:
                    dtBecchu2.Clear();
                    daBecchu2.Fill(dtBecchu2);
                    dtBecchu2.DefaultView.RowFilter = "(KanryouFlg is null or KanryouFlg=False) and Suuryou>ShukkaSuu";
                    bKanryou = (0 == dtBecchu2.DefaultView.Count);
                    break;
                case EnumBecchuKubun.DS:
                    dtDS.Clear();
                    daDS.Fill(dtDS);
                    dtDS.DefaultView.RowFilter = "(KanryouFlg is null or KanryouFlg=False) and Suryou>ShukkaSuu";
                    bKanryou = (0 == dtDS.DefaultView.Count);
                    break;
                case EnumBecchuKubun.SP:
                    dtSP.Clear();
                    daSP.Fill(dtSP);
                    dtSP.DefaultView.RowFilter = string.Format("(KanryouFlg is null or KanryouFlg=False) and Suuryou>ShukkaSuu and SHiiresakiCode = '{0}'",key.ShiiresakiCode);
                    bKanryou = (0 == dtSP.DefaultView.Count);
                    break;
            }

            SqlDataAdapter daT_BecchuKeyInfo = new SqlDataAdapter("", t.Connection);
            daT_BecchuKeyInfo.SelectCommand.CommandText = "SELECT * FROM T_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            daT_BecchuKeyInfo.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", key.UketsukeBi);
            daT_BecchuKeyInfo.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", key.OrderKanriNo);
            daT_BecchuKeyInfo.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", key.ShiiresakiCode);
            daT_BecchuKeyInfo.SelectCommand.Transaction = t;
            daT_BecchuKeyInfo.UpdateCommand = new SqlCommandBuilder(daT_BecchuKeyInfo).GetUpdateCommand();
            daT_BecchuKeyInfo.UpdateCommand.Transaction = t;
            MizunoDataSet.T_BecchuKeyInfoDataTable T_BecchuKeyInfo = new MizunoDataSet.T_BecchuKeyInfoDataTable();
            daT_BecchuKeyInfo.Fill(T_BecchuKeyInfo);
            T_BecchuKeyInfo[0].KanryouFlg = bKanryou;
            daT_BecchuKeyInfo.Update(T_BecchuKeyInfo);



            // �c���`�F�b�N(���S�̊m�F������Ή��L�R�[�h�͕s�v)
            //SqlDataAdapter daCheck = new SqlDataAdapter("", t.Connection);
            //daCheck.SelectCommand.CommandText = "select * from VIEW_BecchuChuzanCheckList where (SashizuBi = @z) AND (SashizuNo = @n) AND (ShiiresakiCode = @s)";
            //daCheck.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
            //daCheck.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
            //daCheck.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            //daCheck.SelectCommand.Transaction = t;

            //DataTable dtCheck = new DataTable();
            //daCheck.Fill(dtCheck);
            //dtCheck.DefaultView.RowFilter = "ShukkaSuu<>NouhinSu";
            //if (0 < dtCheck.DefaultView.Count)
            //{
            //    string[] str = new string[dtCheck.DefaultView.Count];
            //    for (int i = 0; i < dtCheck.DefaultView.Count; i++)
            //    {
            //        string strHinban = Convert.ToString(dtCheck.DefaultView[i]["Hinban"]);
            //        string strSize = Convert.ToString(dtCheck.DefaultView[i]["Size"]);
            //        int nShukkaSuu = Convert.ToInt32(dtCheck.DefaultView[i]["ShukkaSuu"]);
            //        int nNouhinSuu = Convert.ToInt32(dtCheck.DefaultView[i]["NouhinSu"]);
            //        str[i] = string.Format("�i��{0}�A�T�C�Y{1}�ŏo�א��̍��ق�����܂��B�o�א�={2}, �[�i��={3}", strHinban, strSize, nShukkaSuu, nNouhinSuu);
            //    }

            //    throw new Exception(string.Join("/", str));
            //}

            return dk;
        }




        /// <summary>
        /// �`�[�C���o�^
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="c"></param>
        /// <param name="newKey"></param>
        /// <returns></returns>
        public static Core.Error DenpyouShuseiTouroku(NouhinDataClass.DenpyouKey key, TourokuData data, SqlConnection c, out NouhinDataClass.DenpyouKey newKey)
        {
            newKey = null;

            SqlDataAdapter daKey = new SqlDataAdapter("", c);
            daKey.SelectCommand.CommandText = "SELECT * FROM VIEW_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            daKey.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", "");
            daKey.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", "");
            daKey.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", "");

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


            // �o�׏��
            SqlDataAdapter daShukkaInfo = new SqlDataAdapter("", c);
            daShukkaInfo.SelectCommand.CommandText = "select * from T_BecchuShukkaInfo where SashizuBi=@z and SashizuNo=@n and ShiiresakiCode=@s";


            // �����t���O�̗����Ă���ʂ̖����M�[�i�f�[�^
            SqlDataAdapter daGetOtherNouhinMeisai = new SqlDataAdapter("", c);
            daGetOtherNouhinMeisai.SelectCommand.CommandText = @"
SELECT                  dbo.T_NouhinMeisai.*
FROM                     dbo.T_NouhinMeisai INNER JOIN
dbo.T_NouhinHeader ON dbo.T_NouhinMeisai.HakkouNo = dbo.T_NouhinHeader.HakkouNo AND 
dbo.T_NouhinMeisai.ShiiresakiCode = dbo.T_NouhinHeader.NouhinmotoShiiresakiCode AND 
dbo.T_NouhinMeisai.YYMM = dbo.T_NouhinHeader.YYMM
WHERE                   (dbo.T_NouhinHeader.SashizuBi = @z) AND (dbo.T_NouhinHeader.OrderKanriNo = @n) AND 
(dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = @s) AND (dbo.T_NouhinMeisai.Hinban = @Hinban) AND (dbo.T_NouhinMeisai.Size = @Size) AND (dbo.T_NouhinMeisai.LotNo = @LotNo) AND 
(dbo.T_NouhinMeisai.HinbanTsuikaFlg = N'0') AND (dbo.T_NouhinMeisai.RowNo >= 0) AND (dbo.T_NouhinHeader.SoushinFlg = 0) AND 
(dbo.T_NouhinMeisai.KanryouFlg = N'9') AND 
NOT (dbo.T_NouhinHeader.HakkouNo = @HakkouNo AND dbo.T_NouhinHeader.YYMM = @YYMM)";
            daGetOtherNouhinMeisai.SelectCommand.Parameters.AddWithValue("@z", "");
            daGetOtherNouhinMeisai.SelectCommand.Parameters.AddWithValue("@n", "");
            daGetOtherNouhinMeisai.SelectCommand.Parameters.AddWithValue("@s", "");
            daGetOtherNouhinMeisai.SelectCommand.Parameters.AddWithValue("@Hinban", "");
            daGetOtherNouhinMeisai.SelectCommand.Parameters.AddWithValue("@Size", "");
            daGetOtherNouhinMeisai.SelectCommand.Parameters.AddWithValue("@LotNo", "");
            daGetOtherNouhinMeisai.SelectCommand.Parameters.AddWithValue("@HakkouNo", key.HakkouNo);
            daGetOtherNouhinMeisai.SelectCommand.Parameters.AddWithValue("@YYMM", key.YYMM);


            // �[�i�f�[�^�폜
            SqlCommand cmdDeleteNouhinData = new SqlCommand("", c);
            cmdDeleteNouhinData.CommandText = @"
delete T_NouhinHeader where (HakkouNo = @h) AND (NouhinmotoShiiresakiCode = @s) AND (YYMM = @y);
delete T_NouhinMeisai where (HakkouNo = @h) AND (ShiiresakiCode = @s) AND (YYMM = @y);
delete T_NouhinMeisaiOption where (HakkouNo = @h) AND (ShiiresakiCode = @s) AND (YYMM = @y);
delete T_NouhinTrailer where (HakkouNo = @h) AND (ShiiresakiCode = @s) AND (YYMM = @y);
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

                daKey.SelectCommand.Transaction = t;
                daHeader.SelectCommand.Transaction = t;
                daM.SelectCommand.Transaction = daM.UpdateCommand.Transaction = t;
                cmdDeleteNouhinData.Transaction = t;
                cmdDeleteNouhinData2.Transaction = t;


                MizunoDataSet.T_NouhinHeaderDataTable dtHeader = new MizunoDataSet.T_NouhinHeaderDataTable();
                daHeader.Fill(dtHeader);
                if (0 == dtHeader.Count)
                    return new Core.Error("�Y���̃f�[�^������܂���B");
                MizunoDataSet.T_NouhinHeaderRow drHeader = dtHeader[0];
                if (drHeader.SoushinFlg == SoushinFlg.SOUSHINZUMI)
                    return new Core.Error("���M�ς݂̈׏C���ł��܂���B");

                daKey.SelectCommand.Parameters["@MizunoUketsukeBi"].Value = drHeader.SashizuBi;
                daKey.SelectCommand.Parameters["@OrderKanriNo"].Value = drHeader.OrderKanriNo;
                daKey.SelectCommand.Parameters["@ShiiresakiCode"].Value = drHeader.NouhinmotoShiiresakiCode;
                ViewDataset.VIEW_BecchuKeyInfoDataTable dtKey = new ViewDataset.VIEW_BecchuKeyInfoDataTable();
                daKey.Fill(dtKey);
                if (0 == dtKey.Count) throw new Exception("�L�[���̎擾�Ɏ��s���܂����B");
                ViewDataset.VIEW_BecchuKeyInfoRow drKey = dtKey[0];

                EnumBecchuKubun kubun = EnumBecchuKubun.Becchu;

                if (!drKey.IsT_Becchu_SashizuNoNull())
                    kubun = EnumBecchuKubun.Becchu;
                else if (!drKey.IsT_Becchu2_SashizuNoNull())
                    kubun = EnumBecchuKubun.Becchu2;
                else if (!drKey.IsT_DS_OrderKanriNoNull())
                    kubun = EnumBecchuKubun.DS;
                else if (!drKey.IsT_SP_OrderKanriNoNull())
                    kubun = EnumBecchuKubun.SP;
                else
                    throw new Exception("�ʒ��i�̃f�[�^��������܂���ł����B");

                daShukkaInfo.SelectCommand.Parameters.AddWithValue("@z", drHeader.SashizuBi);
                daShukkaInfo.SelectCommand.Parameters.AddWithValue("@n", drHeader.OrderKanriNo);
                daShukkaInfo.SelectCommand.Parameters.AddWithValue("@s", drHeader.NouhinmotoShiiresakiCode);
                daShukkaInfo.SelectCommand.Transaction = t;
                daShukkaInfo.UpdateCommand = new SqlCommandBuilder(daShukkaInfo).GetUpdateCommand();
                daShukkaInfo.DeleteCommand = new SqlCommandBuilder(daShukkaInfo).GetDeleteCommand();
                daShukkaInfo.UpdateCommand.Transaction = daShukkaInfo.DeleteCommand.Transaction = t;
                MizunoDataSet.T_BecchuShukkaInfoDataTable dtShukkaInfo = new MizunoDataSet.T_BecchuShukkaInfoDataTable();
                daShukkaInfo.Fill(dtShukkaInfo);


                ViewDataset.VIEW_Becchu_ShukkaMeisaiDataTable dtBecchu = new ViewDataset.VIEW_Becchu_ShukkaMeisaiDataTable();
                ViewDataset.VIEW_Becchu2_ShukkaMeisaiDataTable dtBecchu2 = new ViewDataset.VIEW_Becchu2_ShukkaMeisaiDataTable();
                ViewDataset.VIEW_DS2_ShukkaMeisaiDataTable dtDS = new ViewDataset.VIEW_DS2_ShukkaMeisaiDataTable();
//                ViewDataset.VIEW_DS_ShukkaMeisaiDataTable dtDS = new ViewDataset.VIEW_DS_ShukkaMeisaiDataTable();
                ViewDataset.VIEW_SP_ShukkaMeisaiDataTable dtSP = new ViewDataset.VIEW_SP_ShukkaMeisaiDataTable();


                switch (kubun)
                {
                    case EnumBecchuKubun.Becchu:
                        // VIEW_Becchu_ShukkaMeisai
                        SqlDataAdapter daBecchu = new SqlDataAdapter("", c);
                        daBecchu.SelectCommand.CommandText = "select * from VIEW_Becchu_ShukkaMeisai WHERE SashizuBi=@z and SashizuNo=@n and ShiiresakiCode=@s";
                        daBecchu.SelectCommand.Parameters.AddWithValue("@z", drHeader.SashizuBi);
                        daBecchu.SelectCommand.Parameters.AddWithValue("@n", drHeader.OrderKanriNo);
                        daBecchu.SelectCommand.Parameters.AddWithValue("@s", drHeader.NouhinmotoShiiresakiCode);
                        daBecchu.SelectCommand.Transaction = t;
                        daBecchu.Fill(dtBecchu);
                        break;
                    case EnumBecchuKubun.Becchu2:
                        // VIEW_Becchu2_ShukkaMeisai
                        SqlDataAdapter daBecchu2 = new SqlDataAdapter("", c);
                        daBecchu2.SelectCommand.CommandText = "select * from VIEW_Becchu2_ShukkaMeisai WHERE SashizuBi=@z and SashizuNo=@n and ShiiresakiCode=@s";
                        daBecchu2.SelectCommand.Parameters.AddWithValue("@z", drHeader.SashizuBi);
                        daBecchu2.SelectCommand.Parameters.AddWithValue("@n", drHeader.OrderKanriNo);
                        daBecchu2.SelectCommand.Parameters.AddWithValue("@s", drHeader.NouhinmotoShiiresakiCode);
                        daBecchu2.SelectCommand.Transaction = t;
                        daBecchu2.Fill(dtBecchu2);
                        break;
                    case EnumBecchuKubun.DS:
                        // DS
                        SqlDataAdapter daDS = new SqlDataAdapter("", c);
                        daDS.SelectCommand.CommandText = "select * from VIEW_DS2_ShukkaMeisai WHERE OrderKanriNo=@n";
                        daDS.SelectCommand.Parameters.AddWithValue("@n", drHeader.OrderKanriNo);
                        daDS.SelectCommand.Transaction = t;
                        daDS.Fill(dtDS);
                        break;
                    case EnumBecchuKubun.SP:
                        // SP
                        SqlDataAdapter daSP = new SqlDataAdapter("", c);
                        daSP.SelectCommand.CommandText = "select * from VIEW_SP_ShukkaMeisai WHERE OrderKanriNo=@n";
                        daSP.SelectCommand.Parameters.AddWithValue("@n", drHeader.OrderKanriNo);
                        daSP.SelectCommand.Transaction = t;
                        daSP.Fill(dtSP);
                        break;
                }


                MizunoDataSet.T_NouhinMeisaiDataTable dtM = new MizunoDataSet.T_NouhinMeisaiDataTable();
                Dictionary<string, int> tblHinbanSizeLotNoNouhinSu = new Dictionary<string, int>();
                List<string> lstKanryouFlgHinbanSizeLotNo = new List<string>();  // �����t���O���Z�b�g����Ă���i�ԃT�C�Y

                List<string> lstKannou_HinbanSizeLotNo = new List<string>();  // �S���o�ׂ̊��[�ƂȂ��Ă����i�ԁA�T�C�Y�i�{�o�ׂ����������邱�ƂŊ����t���O���I�t�ɂȂ�B�����M�̑��̓`�[�̊����t���O�̃��Z�b�g���s���K�v������j

                
                daM.Fill(dtM);


                for (int i = 0; i < dtM.Count; i++)
                {
                    MizunoDataSet.T_NouhinMeisaiRow dr = dtM[i];

                    string strHinbanSizeLotNo = string.Format("{0}\t{1}\t{2}", dr.Hinban, dr.Size, dr.LotNo);

                    if (!tblHinbanSizeLotNoNouhinSu.ContainsKey(strHinbanSizeLotNo))
                        tblHinbanSizeLotNoNouhinSu.Add(strHinbanSizeLotNo, (int)dr.NouhinSuu);
                    else
                        tblHinbanSizeLotNoNouhinSu[strHinbanSizeLotNo] += (int)dr.NouhinSuu;

                    if (dr.KanryouFlg == KanryouFlg.Kanryou)
                    {
                        // �{�[�i�f�[�^��Ŋ����t���O�̗����Ă���i��+�T�C�Y���擾(�ʒ��i)
                        // ����Ŋ����t���O���I�t�ɂ���B
                        if (!lstKanryouFlgHinbanSizeLotNo.Contains(strHinbanSizeLotNo))
                            lstKanryouFlgHinbanSizeLotNo.Add(strHinbanSizeLotNo);
                    }

                    int nNouhinSu = (int)dr.NouhinSuu;
                    if (0 == nNouhinSu) continue;   // ���ʂɕω����������̂̓X�L�b�v

                    int nChumonSu = 0;
                    int nShukkaSu = 0;
                    switch (kubun)
                    {
                        case EnumBecchuKubun.Becchu:
                            // VIEW_Becchu_ShukkaMeisai
                            nChumonSu = Convert.ToInt32(dtBecchu.Compute("SUM(Suryou)", string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", dr.Hinban, dr.Size, dr.LotNo)));
                            nShukkaSu = Convert.ToInt32(dtBecchu.Compute("SUM(ShukkaSuu)", string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", dr.Hinban, dr.Size, dr.LotNo)));
                            break;
                        case EnumBecchuKubun.Becchu2:
                            // VIEW_Becchu2_ShukkaMeisai
                            nChumonSu = Convert.ToInt32(dtBecchu2.Compute("SUM(Suuryou)", string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", dr.Hinban, dr.Size, dr.LotNo)));
                            nShukkaSu = Convert.ToInt32(dtBecchu2.Compute("SUM(ShukkaSuu)", string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", dr.Hinban, dr.Size, dr.LotNo)));
                            break;
                        case EnumBecchuKubun.DS:
                            // DS
                            nChumonSu = Convert.ToInt32(dtDS.Compute("SUM(Suryou)", string.Format("Hinban='{0}' AND Size='{1}'", dr.Hinban, dr.Size)));
                            nShukkaSu = Convert.ToInt32(dtDS.Compute("SUM(ShukkaSuu)", string.Format("Hinban='{0}' AND Size='{1}'", dr.Hinban, dr.Size)));
                            break;
                        case EnumBecchuKubun.SP:
                            // SP
                            //20200325 LotNo�ǉ� LotNo���Ȃ����ƂŁA�`�[�f�[�^�A�b�v���[�h����LotNo�̈Ⴂ�ɂ��G���[��������������
                            nChumonSu = Convert.ToInt32(dtSP.Compute("SUM(Suuryou)", string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", dr.Hinban, dr.Size, dr.LotNo)));
                            nShukkaSu = Convert.ToInt32(dtSP.Compute("SUM(ShukkaSuu)", string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", dr.Hinban, dr.Size, dr.LotNo)));
                            break;
                    }

                    if (nChumonSu == nShukkaSu)
                    {
                        // �{�i��+�T�C�Y�͊��[��Ԃł�����
                        if (!lstKannou_HinbanSizeLotNo.Contains(strHinbanSizeLotNo))
                            lstKannou_HinbanSizeLotNo.Add(strHinbanSizeLotNo);
                    }

                    

                    // �o�א����o�^����Ă���T_BecchuShukkaInfo�́A�ʒ�2�́ARowNo�ň������ĉ\�ŁA����ȊO��RowNo=0�ň������Ă�
                    MizunoDataSet.T_BecchuShukkaInfoRow drShukkaInfo =
                        dtShukkaInfo.FindBySashizuBiSashizuNoShiiresakiCodeHinbanSizeLotNoRowNo(drHeader.SashizuBi,
                        drHeader.OrderKanriNo, drHeader.NouhinmotoShiiresakiCode, dr.Hinban, dr.Size, dr.LotNo, (kubun == EnumBecchuKubun.Becchu2) ? dr.RowNo : 0);
                    if (null == drShukkaInfo)
                    {
                        throw new Exception(string.Format("�o�׏��f�[�^������܂���B�i�ԁF{0}�A�T�C�Y�F{1}�ARowNo:{2}", dr.Hinban, dr.Size, (kubun == EnumBecchuKubun.Becchu2) ? dr.RowNo : 0));
                    }
                    drShukkaInfo.ShukkaSuu -= (int)dr.NouhinSuu;  // �[�i�������炷
                    if (0 > drShukkaInfo.ShukkaSuu) throw new Exception("�o�א����}�C�i�X�ɂȂ�܂����B");
                    if (0 == drShukkaInfo.ShukkaSuu) drShukkaInfo.KanryouFlg = false;   // ����͖{�[�i�Ŋ��[���Ă����P�[�X�Ō�Ŋ����t���O��Flase�ɂȂ邪��芸���������ł�����Ă����B
                }

                daM.Update(dtM);


                // �����t���O�������Ă����i�ԁA�T�C�Y�̊����t���O���I�t�ɂ���B
                // T_BecchuShukkaInfo�ŕʒ�2��RowNo�P�ʂŊ����t���O���Z�b�g�����\���ł��邪�A�����t���O�́A�i��+�T�C�Y�P�ʂŃZ�b�g�����悤�ɂ����i2011/7/6 ����Ƀ��[���Ŋm�F�ς݁j
                // ���]���āA����i�ԃT�C�Y������ꍇ�́A�SRowNo�ɑ΂��ē����l�̊����t���O���Z�b�g���邱�ƂɂȂ�B
                for (int i = 0; i < lstKanryouFlgHinbanSizeLotNo.Count; i++)
                {
                    string strHinban = lstKanryouFlgHinbanSizeLotNo[i].Split('\t')[0];
                    string strSize = lstKanryouFlgHinbanSizeLotNo[i].Split('\t')[1];
                    string strLotNo = lstKanryouFlgHinbanSizeLotNo[i].Split('\t')[2];

                    dtShukkaInfo.DefaultView.RowFilter = string.Format("Hinban='{0}' and Size='{1}' and LotNo='{2}' AND RowNo>=0", strHinban.Replace("'", "''"), strSize.Replace("'", "''"), strLotNo.Replace("'", "''"));
                    // �ʒ�2�ȊO�͂P�����q�b�g����͂�
                    switch (kubun)
                    {
                        case EnumBecchuKubun.Becchu:
                        case EnumBecchuKubun.DS:
                        case EnumBecchuKubun.SP:
                            //20200325 LotNo�ǉ� LotNo���Ȃ����ƂŁA�`�[�f�[�^�A�b�v���[�h����LotNo�̈Ⴂ�ɂ��G���[��������������
                            if (1 != dtShukkaInfo.DefaultView.Count) throw new Exception(string.Format("�o�׏��f�[�^������܂���B�i�ԁF{0}�A�T�C�Y�F{1}�A���b�gNo�F{2}", strHinban, strSize, strLotNo));
                            break;
                    }

                    for (int y = 0; y < dtShukkaInfo.DefaultView.Count; y++)
                    {
                        MizunoDataSet.T_BecchuShukkaInfoRow drShukkaInfo = dtShukkaInfo.DefaultView[y].Row as MizunoDataSet.T_BecchuShukkaInfoRow;
                        drShukkaInfo.KanryouFlg = false;
                    }
                }



                // ���[�Ŋ����t���O�������Ă����i��+�T�C�Y�̃t���O���I�t�i���̖����M�[�i�f�[�^�ɂ��e������j
                daGetOtherNouhinMeisai.SelectCommand.Parameters["@z"].Value = drKey.MizunoUketsukeBi;
                daGetOtherNouhinMeisai.SelectCommand.Parameters["@n"].Value = drKey.OrderKanriNo;
                daGetOtherNouhinMeisai.SelectCommand.Parameters["@s"].Value = drKey.ShiiresakiCode;
                daGetOtherNouhinMeisai.SelectCommand.Transaction = t;
                for (int i = 0; i < lstKannou_HinbanSizeLotNo.Count; i++)
                {
                    string strHinban = lstKannou_HinbanSizeLotNo[i].Split('\t')[0];
                    string strSize = lstKannou_HinbanSizeLotNo[i].Split('\t')[1];
                    string strLotNo = lstKannou_HinbanSizeLotNo[i].Split('\t')[2];

                    dtShukkaInfo.DefaultView.RowFilter = string.Format("Hinban='{0}' and Size='{1}' and LotNo='{2}' AND RowNo>=0", strHinban.Replace("'", "''"), strSize.Replace("'", "''"), strLotNo.Replace("'", "''"));
                    // �ʒ�2�ȊO�͂P�����q�b�g����͂�
                    switch (kubun)
                    {
                        case EnumBecchuKubun.Becchu:
                        case EnumBecchuKubun.DS:
                        case EnumBecchuKubun.SP:
                            //20200325 LotNo�ǉ� LotNo���Ȃ����ƂŁA�`�[�f�[�^�A�b�v���[�h����LotNo�̈Ⴂ�ɂ��G���[��������������
                            if (1 != dtShukkaInfo.DefaultView.Count) throw new Exception(string.Format("�o�׏��f�[�^������܂���B�i�ԁF{0}�A�T�C�Y�F{1}�A���b�gNo�F{2}", strHinban, strSize, strLotNo));
                            break;
                    }

                    for (int y = 0; y < dtShukkaInfo.DefaultView.Count; y++)
                    {
                        MizunoDataSet.T_BecchuShukkaInfoRow drShukkaInfo = dtShukkaInfo.DefaultView[y].Row as MizunoDataSet.T_BecchuShukkaInfoRow;
                        drShukkaInfo.KanryouFlg = false;
                    }

                    // ���꒍���A�i�ԁA�T�C�Y�̑��̓`�[�Ŋ����t���O���t���Ă���ꍇ������̂ł�����폜����B
                    daGetOtherNouhinMeisai.SelectCommand.Parameters["@Hinban"].Value = strHinban;
                    daGetOtherNouhinMeisai.SelectCommand.Parameters["@Size"].Value = strSize;
                    daGetOtherNouhinMeisai.SelectCommand.Parameters["@LotNo"].Value = strLotNo;
                    dtM.Clear();
                    daGetOtherNouhinMeisai.Fill(dtM);
                    for (int m = 0; m < dtM.Count; m++)
                        dtM[m].KanryouFlg = KanryouFlg.MiKanryou;
                    daM.Update(dtM);
                }

                //
                //List<string> lstTorikeshi = new List<string>(); // ����0�̖��׈ꗗ

                //// ����̏ꍇ�A���̖����M�f�[�^�̊����t���O��OFF�ɂ���B
                //daGetOtherNouhinMeisai.SelectCommand.Parameters["@z"].Value = drKey.MizunoUketsukeBi;
                //daGetOtherNouhinMeisai.SelectCommand.Parameters["@n"].Value = drKey.OrderKanriNo;
                //daGetOtherNouhinMeisai.SelectCommand.Parameters["@s"].Value = drKey.ShiiresakiCode;
                //daGetOtherNouhinMeisai.SelectCommand.Transaction = t;

                //for (int i = 0; i < data.lstMeisai.Count; i++)
                //{
                //    data.lstMeisai[i].

                //    if (nShukkaSu == 0)
                //    {
                //        if (!lstTorikeshi.Contains(strHinbanSizeLotNo))
                //            lstTorikeshi.Add(strHinbanSizeLotNo);

                //    }

                //}



                
                //for (int i = 0; i < lstKannou_HinbanSizeLotNo.Count; i++)
                //{
                //    string strHinban = lstKannou_HinbanSizeLotNo[i].Split('\t')[0];
                //    string strSize = lstKannou_HinbanSizeLotNo[i].Split('\t')[1];
                //    string strLotNo = lstKannou_HinbanSizeLotNo[i].Split('\t')[2];

                //    dtShukkaInfo.DefaultView.RowFilter = string.Format("Hinban='{0}' and Size='{1}' and LotNo='{2}' AND RowNo>=0", strHinban.Replace("'", "''"), strSize.Replace("'", "''"), strLotNo.Replace("'", "''"));
                //    // �ʒ�2�ȊO�͂P�����q�b�g����͂�
                //    switch (kubun)
                //    {
                //        case EnumBecchuKubun.Becchu:
                //        case EnumBecchuKubun.DS:
                //        case EnumBecchuKubun.SP:
                //            if (1 != dtShukkaInfo.DefaultView.Count) throw new Exception(string.Format("�o�׏��f�[�^������܂���B�i�ԁF{0}�A�T�C�Y�F{1}", strHinban, strSize));
                //            break;
                //    }

                //    for (int y = 0; y < dtShukkaInfo.DefaultView.Count; y++)
                //    {
                //        MizunoDataSet.T_BecchuShukkaInfoRow drShukkaInfo = dtShukkaInfo.DefaultView[y].Row as MizunoDataSet.T_BecchuShukkaInfoRow;
                //        drShukkaInfo.KanryouFlg = false;
                //    }

                //    // ���꒍���A�i�ԁA�T�C�Y�̑��̓`�[�Ŋ����t���O���t���Ă���ꍇ������̂ł�����폜����B
                //    daGetOtherNouhinMeisai.SelectCommand.Parameters["@Hinban"].Value = strHinban;
                //    daGetOtherNouhinMeisai.SelectCommand.Parameters["@Size"].Value = strSize;
                //    daGetOtherNouhinMeisai.SelectCommand.Parameters["@LotNo"].Value = strLotNo;
                //    dtM.Clear();
                //    daGetOtherNouhinMeisai.Fill(dtM);
                //    for (int m = 0; m < dtM.Count; m++)
                //        dtM[m].KanryouFlg = KanryouFlg.MiKanryou;
                //    daM.Update(dtM);
                //}



                // �����t���O���I�t�ɂ���B
                SqlDataAdapter daT_BecchuKeyInfo = new SqlDataAdapter("", t.Connection);
                daT_BecchuKeyInfo.SelectCommand.CommandText = "SELECT * FROM T_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
                daT_BecchuKeyInfo.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", drHeader.SashizuBi);
                daT_BecchuKeyInfo.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", drHeader.OrderKanriNo);
                daT_BecchuKeyInfo.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", drHeader.NouhinmotoShiiresakiCode);
                daT_BecchuKeyInfo.SelectCommand.Transaction = t;
                daT_BecchuKeyInfo.UpdateCommand = new SqlCommandBuilder(daT_BecchuKeyInfo).GetUpdateCommand();
                daT_BecchuKeyInfo.UpdateCommand.Transaction = t;
                MizunoDataSet.T_BecchuKeyInfoDataTable dtBecchuKey = new MizunoDataSet.T_BecchuKeyInfoDataTable();
                daT_BecchuKeyInfo.Fill(dtBecchuKey);
                if (0 == dtBecchuKey.Count) throw new Exception("�L�[��񂪂���܂���B");
                dtBecchuKey[0].KanryouFlg = false;
                daT_BecchuKeyInfo.Update(dtBecchuKey);


                // �s�v�ȏo�׏����폜
                for (int i = dtShukkaInfo.Count - 1; i >= 0; i--)
                {
                    if (!dtShukkaInfo[i].KanryouFlg && 0 == dtShukkaInfo[i].ShukkaSuu) dtShukkaInfo[i].Delete();
                }
                daShukkaInfo.Update(dtShukkaInfo);


                int nHakkouNo = (int.Parse(data.dtHakkouBi.ToString("yyMM")) == drHeader.YYMM) ? drHeader.HakkouNo : 0;  // YYMM���ς��Δ��sNo�͍ēx�̔�

                // �[�i�f�[�^���폜
                cmdDeleteNouhinData.ExecuteScalar();

                // DB���� INSERT�EUPDATE
                newKey = DenpyouTouroku(false, true, nHakkouNo, new BecchuOrderKey(drKey.MizunoUketsukeBi, drKey.OrderKanriNo, drKey.ShiiresakiCode), data, t);

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
    dbo.T_NouhinMeisai.ShiiresakiCode = @NouhinmotoShiiresakiCode AND
    dbo.T_NouhinMeisai.YYMM = @YYMM
ORDER BY
    Edaban,GyouNo
";

                daGyouNo.SelectCommand.Parameters.AddWithValue("@HakkouNo", key.HakkouNo);
                daGyouNo.SelectCommand.Parameters.AddWithValue("@NouhinmotoShiiresakiCode", key.ShiiresakiCode);
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




//                string s = key.ShiiresakiCode;
//                int ym = key.YYMM;
//                string o = dt[i]["HachuNo"].ToString();
//                //string h = 
//                string l = dt[i]["LotNo"].ToString();
//                string hn = dt[i]["HakkouNo"].ToString();

//                // ����
//                SqlDataAdapter daGetNH = new SqlDataAdapter("", sqlConn);
//                daGetNH.SelectCommand.CommandText = string.Format(@"SELECT T_NouhinMeisai.* 
//FROM
//    dbo.T_NouhinMeisai 
//WHERE                  
//    dbo.T_NouhinMeisai.HakkouNo = @HakkouNo AND 
//    dbo.T_NouhinMeisai.ShiiresakiCode = @NouhinmotoShiiresakiCode AND
//    dbo.T_NouhinMeisai.YYMM = @YYMM
//ORDER BY
//    Edaban,GyouNo", s, y, o, h, l, hn);

//                daGetNH.SelectCommand.Transaction = t;
//                daGetNH.Fill(dtM);



//                DataView dvM = dtM.DefaultView;

//                dvM.RowFilter = "KanryouFlg = '0'";
//                dvM.Sort = "HakkouNo DESC";





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




        public static int GetDataCount(Kensaku p, EnumBecchuKubun k, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = "select * from VIEW_BecchuKeyInfo";
            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
            p.SetWhere(w, da.SelectCommand);

            // �ʒ��敪
            switch (k)
            {
                case EnumBecchuKubun.Becchu:
                    w.Add("VIEW_BecchuKeyInfo.T_Becchu_SashizuNo IS NOT NULL");
                    break;
                case EnumBecchuKubun.Becchu2:
                    w.Add("VIEW_BecchuKeyInfo.T_Becchu2_SashizuNo IS NOT NULL");
                    break;
                case EnumBecchuKubun.DS:
                    //2013.03.28 �����y�����z�����Ō��Ă���uT_DS_OrderKanriNo�v�́uT_DS2�v��JOIN���Ă���̂�EnumBecchuKubun.DS�Ƃ͂Ȃ��Ă��邪�f�[�^��DS2�̃f�[�^���Q�Ƃ��Ă���B
                    w.Add("VIEW_BecchuKeyInfo.T_DS_OrderKanriNo IS NOT NULL");
                    break;
                case EnumBecchuKubun.SP:
                    w.Add("VIEW_BecchuKeyInfo.T_SP_OrderKanriNo IS NOT NULL");
                    break;
            }

            if (!string.IsNullOrEmpty(w.WhereText))
                da.SelectCommand.CommandText += " where " + w.WhereText;

            int nCount = 0;
            Core.Error ret = Core.Sql.MiscClass.GetRecordCount(da.SelectCommand, c, ref nCount);
            return nCount;
        }

        public class ShumokuCodeMei
        {
            public string ShumokuCode { get; set; }
            public string ShumokuMei { get; set; }
        }


        /// <summary>
        /// ���Ӑ�̌����Ώێ�ڎ擾
        /// </summary>
        /// <param name="strTokuisakiCodes"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static ShumokuCodeMei[] GetShumokuCodeMei4Tokuisaki(string[] strTokuisakiCodes, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = @"
SELECT                  TOP (100) PERCENT dbo.T_BecchuKeyInfo.ShumokuCode, dbo.M_Shumoku.ShumokuMei
FROM                     dbo.T_BecchuKeyInfo INNER JOIN
dbo.M_Shumoku ON dbo.T_BecchuKeyInfo.ShumokuCode = dbo.M_Shumoku.ShumokuCode
WHERE                   (dbo.M_Shumoku.TokuisakiKubun = 1) AND (dbo.T_BecchuKeyInfo.TokuisakiCode in ('{0}'))
GROUP BY          dbo.T_BecchuKeyInfo.ShumokuCode, dbo.M_Shumoku.ShumokuMei
ORDER BY           dbo.M_Shumoku.ShumokuMei";

            da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, string.Join("','", strTokuisakiCodes));

            DataTable dt = new DataTable();

            da.Fill(dt);

            ShumokuCodeMei[] lst = new ShumokuCodeMei[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst[i] = new ShumokuCodeMei();
                lst[i].ShumokuCode = Convert.ToString(dt.Rows[i][0]);
                lst[i].ShumokuMei = Convert.ToString(dt.Rows[i][1]);
            }
            return lst;
        }



        public static ViewDataset.VIEW_Becchu_DownloadDataTable
            getVIEW_Becchu_DownloadDataTable(Kensaku p, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = @"

SELECT
    dbo.VIEW_BecchuKeyInfo.ShumokuCode,
    dbo.VIEW_BecchuKeyInfo.ShumokuMei,
    dbo.VIEW_BecchuKeyInfo.TokuisakiCode,
    dbo.VIEW_BecchuKeyInfo.TokuisakiMei,
    dbo.VIEW_BecchuKeyInfo.BunruiMei,
    dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi,
    dbo.VIEW_BecchuKeyInfo.OrderKanriNo,
    dbo.VIEW_BecchuKeyInfo.TeamMei,
    dbo.VIEW_BecchuKeyInfo.OkyakusamaMei,
    dbo.VIEW_BecchuKeyInfo.ShinkiTsuika,
    dbo.VIEW_BecchuKeyInfo.MarkKakou,
    dbo.VIEW_BecchuKeyInfo.KoujyouShukkaYoteiBi,
    dbo.VIEW_BecchuKeyInfo.NiukeJigyoushoCode,
    dbo.VIEW_BecchuKeyInfo.HokanBasho,
    dbo.VIEW_BecchuKeyInfo.KoujyouShukkaShijiBi,
    dbo.VIEW_BecchuKeyInfo.Eigyousho,
    dbo.VIEW_BecchuKeyInfo.Chiku,
    dbo.VIEW_BecchuKeyInfo.MizunoTantousha,
    dbo.VIEW_BecchuKeyInfo.NouhinKigou,
    dbo.VIEW_BecchuKeyInfo.ShiiresakiCode,
    dbo.VIEW_BecchuKeyInfo.ShiiresakiMei,
    dbo.VIEW_BecchuKeyInfo.BecchuSiteOrderNo,
    dbo.VIEW_BecchuKeyInfo.[51OrderNo],
    dbo.VIEW_BecchuKeyInfo.BecchuSpectraNo,
    dbo.VIEW_BecchuKeyInfo.P51OrderNo,
    dbo.VIEW_BecchuKeyInfo.OkuriJyouNo,
    dbo.VIEW_BecchuKeyInfo.UnsouGyoushaMei,
    dbo.VIEW_BecchuKeyInfo.SKTantousha,
    dbo.VIEW_BecchuKeyInfo.CancelBi,
    dbo.VIEW_Becchu_ShukkaMeisai.Hinban,
    dbo.VIEW_Becchu_ShukkaMeisai.Size,
    dbo.VIEW_Becchu_ShukkaMeisai.LotNo,
    dbo.VIEW_Becchu_ShukkaMeisai.Kakaku,
    dbo.VIEW_Becchu_ShukkaMeisai.YuduKakaku,
    dbo.VIEW_Becchu_ShukkaMeisai.Suryou AS Suuryou,
    dbo.VIEW_Becchu_ShukkaMeisai.RowNo,
    dbo.VIEW_Becchu_ShukkaMeisai.ShukkaSuu,
    dbo.VIEW_Becchu_ShukkaMeisai.KanryouFlg,
    dbo.VIEW_Becchu_ShukkaMeisai.ShiireUntinHacchuNo,
    dbo.VIEW_Becchu_ShukkaMeisai.JanCode,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_SeisanBumonMei,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_TantoushaMei,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_KibouNouki,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Genshu,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_DaihyouOrderCode,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_TenMei,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_EigyouBumon,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_GakunouHin,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_GakunouKubun,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_TenMeiKana,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_GakkouTeamMeiKana,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Color,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_ColorNo,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_RepeatZenkaiSashizuBi,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_RepeatZenkaiSashizuNo,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_GoukeiSuuryou,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_MarkKakouKubun,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_MarkKakouNoShinki,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_MarkKakouNoZenkai,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_SaidanHinMarking,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_SaidanHinMarkingKakousaki,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_SeihinMarking,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_SeihinMarkingKakouSaki,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Bikou1,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Bikou2,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Bikou3,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Bikou4,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Bikou5,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Bikou6,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Bikou7,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Bikou8,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Bikou9,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Bikou10,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_ChokusouFlag,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_TukaCode,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_SizeBikouRan,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_SaisunHyou,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku1,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku2,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku3,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku4,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku5,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku6,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku7,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku8,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku9,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku10,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku11,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku12,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku13,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku14,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku15,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku16,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku17,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku18,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku19,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku20,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku1,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku2,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku3,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku4,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku5,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku6,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku7,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku8,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku9,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku10,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku11,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku12,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku13,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku14,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku15,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku16,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku17,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku18,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku19,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku20,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_BesshiAri,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_BesshiNo,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_OkurisakiKanaMei,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_EigyouTantoushaCode,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_SKTantoushaCode,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_JyuyouRank,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1Hinban,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1Size,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1Suuryou,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1LotNo,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1ShiireKingaku,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2Hinban,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2Size,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2Suuryou,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2LotNo,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2ShiireKingaku,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3Hinban,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3Size,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3Suuryou,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3LotNo,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3ShiireKingaku,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_OrderJyoukyou,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_EigyouMemoRan,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_EigyouTantoushaCode,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_NSSBikouRan,
    '' AS T_Becchu2_MarkingKakouSaki,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_ShiireUntinHacchuNo AS ShiireUntinHacchuNo2,
    dbo.VIEW_BecchuKeyInfo.ShiyouBi,
    dbo.VIEW_BecchuKeyInfo.ShiyouMokuteki,
    dbo.VIEW_BecchuKeyInfo.Okurisaki,
    dbo.VIEW_BecchuKeyInfo.ShukkaSakiYubinBangou AS ShukkaSakiYubinBangou2,
    dbo.VIEW_BecchuKeyInfo.ShukkaSakiJyusho,
    dbo.VIEW_BecchuKeyInfo.ShukkaSakiTel,
    dbo.VIEW_BecchuKeyInfo.TenMeiKana2,
    dbo.VIEW_BecchuKeyInfo.GakkouTeamMeiKana2,
    dbo.VIEW_BecchuKeyInfo.OrderBikou
FROM
    dbo.VIEW_BecchuKeyInfo
    INNER JOIN
        dbo.VIEW_Becchu_ShukkaMeisai
    ON  dbo.VIEW_BecchuKeyInfo.ShiiresakiCode = dbo.VIEW_Becchu_ShukkaMeisai.ShiiresakiCode
    AND dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi = dbo.VIEW_Becchu_ShukkaMeisai.SashizuBi
    AND dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.VIEW_Becchu_ShukkaMeisai.SashizuNo
    LEFT OUTER JOIN
        dbo.T_Mark
    ON  dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.T_Mark.SashizuNo
    AND dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi = dbo.T_Mark.SashizuBi

            ";

            //dbo.VIEW_BecchuKeyInfo.T_Becchu_NiukeJigyoshoCode,dbo.VIEW_BecchuKeyInfo.T_Becchu_HokanBasho,
            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
            p.SetWhere(w, da.SelectCommand);


            if (!string.IsNullOrEmpty(w.WhereText))
                da.SelectCommand.CommandText += " where " + w.WhereText + " AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KA' AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KB' AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'M2'";
            else
                da.SelectCommand.CommandText += " where dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KA' AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KB' AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'M2'";

            //20170224 �ǉ� ����̎�ڃR�[�h�ł̓}�[�N���܂œo�^����Ă��Ȃ��Əo�͂���Ȃ��悤�ɂ���
            da.SelectCommand.CommandText += @" AND (dbo.VIEW_BecchuKeyInfo.ShumokuCode NOT IN ('QS', 'QR', 'QT', 'QB', 'QV', 'QA', 'SI', 'EZ', 'SE') OR
                (dbo.VIEW_BecchuKeyInfo.ShumokuCode IN ('QS', 'QR', 'QT', 'QB', 'QV', 'QA', 'SI', 'EZ', 'SE') AND 
                dbo.T_Mark.SashizuNo IS NOT NULL))";

            //QT,QP,QA,SI,SE�͕ʃ����[�X

            da.SelectCommand.CommandText += @" ORDER BY dbo.VIEW_BecchuKeyInfo.ShiiresakiCode,
                                                        VIEW_BecchuKeyInfo.OrderKanriNo,
                                                        dbo.view_becchu_shukkameisai.HinbanNo,
                                                        dbo.VIEW_Becchu_ShukkaMeisai.Hinban,
                                                        dbo.VIEW_Becchu_ShukkaMeisai.LotNo,
                                                        dbo.VIEW_Becchu_ShukkaMeisai.RowNo ";
            //���\�[�g��HinbanNo��ǉ� 20160125 ���삳��v�]

            ViewDataset.VIEW_Becchu_DownloadDataTable dt = new ViewDataset.VIEW_Becchu_DownloadDataTable();

            da.SelectCommand.CommandTimeout += 300;

            da.Fill(dt);

            return dt;
        }

        public static ViewDataset.VIEW_Becchu_DownloadDataTable
            getVIEW_Becchu2_DownloadDataTable(Kensaku p, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = @"

SELECT
    dbo.VIEW_BecchuKeyInfo.ShumokuCode,
    dbo.VIEW_BecchuKeyInfo.ShumokuMei,
    dbo.VIEW_BecchuKeyInfo.TokuisakiCode,
    dbo.VIEW_BecchuKeyInfo.TokuisakiMei,
    dbo.VIEW_BecchuKeyInfo.BunruiMei,
    dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi,
    dbo.VIEW_BecchuKeyInfo.OrderKanriNo,
    dbo.VIEW_BecchuKeyInfo.TeamMei,
    dbo.VIEW_BecchuKeyInfo.OkyakusamaMei,
    dbo.VIEW_BecchuKeyInfo.NiukeJigyoushoCode,
    dbo.VIEW_BecchuKeyInfo.HokanBasho,
    dbo.VIEW_BecchuKeyInfo.ShinkiTsuika,
    dbo.VIEW_BecchuKeyInfo.MarkKakou,
    dbo.VIEW_BecchuKeyInfo.KoujyouShukkaYoteiBi,
    dbo.VIEW_BecchuKeyInfo.KoujyouShukkaShijiBi,
    dbo.VIEW_BecchuKeyInfo.Eigyousho,
    dbo.VIEW_BecchuKeyInfo.Chiku,
    dbo.VIEW_BecchuKeyInfo.MizunoTantousha,
    dbo.VIEW_BecchuKeyInfo.NouhinKigou,
    dbo.VIEW_BecchuKeyInfo.ShiiresakiCode,
    dbo.VIEW_BecchuKeyInfo.ShiiresakiMei,
    dbo.VIEW_BecchuKeyInfo.BecchuSiteOrderNo,
    dbo.VIEW_BecchuKeyInfo.[51OrderNo],
    dbo.VIEW_BecchuKeyInfo.BecchuSpectraNo,
    dbo.VIEW_BecchuKeyInfo.P51OrderNo,
    dbo.VIEW_BecchuKeyInfo.OkuriJyouNo,
    dbo.VIEW_BecchuKeyInfo.UnsouGyoushaMei,
    dbo.VIEW_BecchuKeyInfo.SKTantousha,
    dbo.VIEW_BecchuKeyInfo.CancelBi,
    dbo.T_Becchu2Repeat.RowNo,
    dbo.T_Becchu2Repeat.Hinban,
    dbo.T_Becchu2Repeat.Size,
    dbo.T_Becchu2Repeat.LotNo,
    dbo.T_Becchu2Repeat.Suuryou,
    case dbo.T_Becchu2Repeat.Kakaku
        when '' then convert (decimal (10, 2), '0.00')
        else dbo.T_Becchu2Repeat.Kakaku
    end as Kakaku,
    --dbo.T_Becchu2Repeat.Kakaku,
    --dbo.T_Becchu2Repeat.YuduKakaku,
    0 AS YuduKakaku,
    ISNULL (dbo.T_BecchuShukkaInfo.ShukkaSuu, 0) AS ShukkaSuu,
    dbo.T_BecchuShukkaInfo.KanryouFlg,
    T_Becchu2.ShiireUntinHacchuNo,
    dbo.T_Becchu2Repeat.JanCode,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_SeisanBumonMei,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_TantoushaMei,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_KibouNouki,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Genshu,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_DaihyouOrderCode,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_TenMei,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_EigyouBumon as T_Becchu_EigyouBumon,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_GakunouHin,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_GakunouKubun,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_TenMeiKana,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_GakkouTeamMeiKana,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Color,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_ColorNo,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_RepeatZenkaiSashizuBi as T_Becchu_RepeatZenkaiSashizuBi,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_RepeatZenkaiSashizuNo as T_Becchu_RepeatZenkaiSashizuNo,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_GoukeiSuuryou,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_MarkKakouKubun,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_MarkKakouNoShinki,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_MarkKakouNoZenkai,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_SaidanHinMarking,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_SaidanHinMarkingKakousaki,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_SeihinMarking,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_SeihinMarkingKakouSaki,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Bikou1 as T_Becchu_Bikou1,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Bikou2 as T_Becchu_Bikou2,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Bikou3 as T_Becchu_Bikou3,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Bikou4,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Bikou5,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Bikou6,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Bikou7,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Bikou8,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Bikou9,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Bikou10,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_ChokusouFlag,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_TukaCode,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_SizeBikouRan as T_Becchu_SizeBikouRan,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_SaisunHyou,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku1,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku2,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku3,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku4,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku5,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku6,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku7,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku8,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku9,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku10,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku11,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku12,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku13,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku14,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku15,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku16,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku17,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku18,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku19,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_Kakaku20,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku1,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku2,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku3,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku4,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku5,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku6,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku7,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku8,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku9,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku10,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku11,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku12,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku13,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku14,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku15,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku16,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku17,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku18,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku19,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_YuduKakaku20,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_BesshiAriNasi as T_Becchu_BesshiAri,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_BesshiNo as T_Becchu_BesshiNo,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_OkurisakiKanaMei as T_Becchu_OkurisakiKanaMei,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_EigyouTantoushaCode,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_SKTantoushaCode as T_Becchu_SKTantoushaCode,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_JyuyouRank,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1Hinban,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1Size,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1Suuryou,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1LotNo,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika1ShiireKingaku,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2Hinban,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2Size,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2Suuryou,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2LotNo,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika2ShiireKingaku,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3Hinban,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3Size,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3Suuryou,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3LotNo,
    dbo.VIEW_BecchuKeyInfo.T_Becchu_Tuika3ShiireKingaku,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_OrderJyoukyou,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_EigyouMemoRan,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_EigyouTantoushaCode,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_NSSBikouRan,
    dbo.VIEW_BecchuKeyInfo.T_Becchu2_MarkingKakouSaki,
    dbo.T_Becchu2.ShiireUntinHacchuNo AS ShiireUntinHacchuNo2,
    dbo.VIEW_BecchuKeyInfo.ShiyouBi,
    dbo.VIEW_BecchuKeyInfo.ShiyouMokuteki,
    dbo.VIEW_BecchuKeyInfo.Okurisaki,
    dbo.VIEW_BecchuKeyInfo.ShukkaSakiYubinBangou AS ShukkaSakiYubinBangou2,
    dbo.VIEW_BecchuKeyInfo.ShukkaSakiJyusho,
    dbo.VIEW_BecchuKeyInfo.ShukkaSakiTel,
    dbo.VIEW_BecchuKeyInfo.TenMeiKana2,
    dbo.VIEW_BecchuKeyInfo.GakkouTeamMeiKana2,
    dbo.VIEW_BecchuKeyInfo.OrderBikou
FROM
    dbo.VIEW_BecchuKeyInfo
    INNER JOIN
        dbo.T_Becchu2Repeat
    ON  dbo.VIEW_BecchuKeyInfo.ShiiresakiCode = dbo.T_Becchu2Repeat.ShiiresakiCode
    AND dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi = dbo.T_Becchu2Repeat.SashizuBi
    AND dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.T_Becchu2Repeat.SashizuNo
    LEFT OUTER JOIN
        dbo.T_BecchuShukkaInfo
    ON  dbo.T_Becchu2Repeat.RowNo = dbo.T_BecchuShukkaInfo.RowNo
    AND dbo.T_Becchu2Repeat.Size = dbo.T_BecchuShukkaInfo.Size
    AND dbo.T_Becchu2Repeat.Hinban = dbo.T_BecchuShukkaInfo.Hinban
    AND dbo.T_Becchu2Repeat.LotNo = dbo.T_BecchuShukkaInfo.LotNo
    AND dbo.T_Becchu2Repeat.SashizuBi = dbo.T_BecchuShukkaInfo.SashizuBi
    AND dbo.T_Becchu2Repeat.SashizuNo = dbo.T_BecchuShukkaInfo.SashizuNo
    AND dbo.T_Becchu2Repeat.ShiiresakiCode = dbo.T_BecchuShukkaInfo.ShiiresakiCode
    left join
        T_Becchu2
    ON  dbo.VIEW_BecchuKeyInfo.ShiiresakiCode = dbo.T_Becchu2.ShiiresakiCode
    AND dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi = dbo.T_Becchu2.SashizuBi
    AND dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.T_Becchu2.SashizuNo
    LEFT OUTER JOIN
        dbo.T_Mark
    ON  dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.T_Mark.SashizuNo
    AND dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi = dbo.T_Mark.SashizuBi
            ";

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
            p.SetWhere(w, da.SelectCommand);

            if (!string.IsNullOrEmpty(w.WhereText))
                da.SelectCommand.CommandText += " where " + w.WhereText + " AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KA' AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KB' AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'M2'";
            else
                da.SelectCommand.CommandText += " where dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KA' AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KB' AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'M2'";

            //20170224 �ǉ� ����̎�ڃR�[�h�ł̓}�[�N���܂œo�^����Ă��Ȃ��Əo�͂���Ȃ��悤�ɂ���
            da.SelectCommand.CommandText += @" AND (dbo.VIEW_BecchuKeyInfo.ShumokuCode NOT IN ('QS', 'QR', 'QT', 'QB', 'QV', 'QA', 'SI', 'EZ', 'SE') OR
                (dbo.VIEW_BecchuKeyInfo.ShumokuCode IN ('QS', 'QR', 'QT', 'QB', 'QV', 'QA', 'SI', 'EZ', 'SE') AND 
                dbo.T_Mark.SashizuNo IS NOT NULL))";

            //QT,QP,QA,SI,SE�͕ʃ����[�X

            da.SelectCommand.CommandText += @" ORDER BY dbo.VIEW_BecchuKeyInfo.ShiiresakiCode,
                                                        VIEW_BecchuKeyInfo.OrderKanriNo,
                                                        dbo.T_Becchu2Repeat.Hinban,
                                                        dbo.T_Becchu2Repeat.LotNo,
                                                        dbo.T_Becchu2Repeat.RowNo ";
            da.SelectCommand.CommandTimeout = 600000;
            ViewDataset.VIEW_Becchu_DownloadDataTable dt = new ViewDataset.VIEW_Becchu_DownloadDataTable();
            da.Fill(dt);

            return dt;
        }



        public static ViewDataset.VIEW_DS_DownloadDataTable
            getVIEW_DS_DownloadDataTable(Kensaku p, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = @"
SELECT                  dbo.VIEW_BecchuKeyInfo.ShumokuCode, dbo.VIEW_BecchuKeyInfo.ShumokuMei, dbo.VIEW_BecchuKeyInfo.TokuisakiCode, 
                                  dbo.VIEW_BecchuKeyInfo.TokuisakiMei, dbo.VIEW_BecchuKeyInfo.BunruiMei, dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi, 
                                  dbo.VIEW_BecchuKeyInfo.OrderKanriNo, dbo.VIEW_BecchuKeyInfo.TeamMei, dbo.VIEW_BecchuKeyInfo.OkyakusamaMei, 
                                  dbo.VIEW_BecchuKeyInfo.ShinkiTsuika, dbo.VIEW_BecchuKeyInfo.MarkKakou, dbo.VIEW_BecchuKeyInfo.KoujyouShukkaYoteiBi, 
                                  dbo.VIEW_BecchuKeyInfo.KoujyouShukkaShijiBi, dbo.VIEW_BecchuKeyInfo.Eigyousho, dbo.VIEW_BecchuKeyInfo.Chiku, 
                                  dbo.VIEW_BecchuKeyInfo.MizunoTantousha, dbo.VIEW_BecchuKeyInfo.NouhinKigou, dbo.VIEW_BecchuKeyInfo.ShiiresakiCode, 
                                  dbo.VIEW_BecchuKeyInfo.ShiiresakiMei, dbo.VIEW_BecchuKeyInfo.BecchuSiteOrderNo, dbo.VIEW_BecchuKeyInfo.[51OrderNo], 
                                  dbo.VIEW_BecchuKeyInfo.BecchuSpectraNo, dbo.VIEW_BecchuKeyInfo.P51OrderNo, dbo.VIEW_BecchuKeyInfo.OkuriJyouNo, 
                                  dbo.VIEW_BecchuKeyInfo.UnsouGyoushaMei, dbo.VIEW_BecchuKeyInfo.SKTantousha, dbo.VIEW_BecchuKeyInfo.CancelBi, 
                                  dbo.VIEW_DS_ShukkaMeisai.JyougeKubun, dbo.T_DS.SStyleCode, dbo.T_DS.SStyleMei, dbo.T_DS.KubiColorCode, dbo.T_DS.KubiHaishoku, 
                                  dbo.T_DS.KubiColorMei, dbo.T_DS.KataWakiColorCode, dbo.T_DS.KataWakiHaishoku, dbo.T_DS.KataWakiColorMei, dbo.T_DS.SodeColorCode, 
                                  dbo.T_DS.SodeHaishoku, dbo.T_DS.SodeColorMei, dbo.T_DS.MaetateColorCode, dbo.T_DS.MaetateHaishoku, dbo.T_DS.MaetateColorMei, 
                                  dbo.T_DS.SusoColorCode, dbo.T_DS.SusoHaishoku, dbo.T_DS.SusoColorMei, dbo.T_DS.PStyleCode, dbo.T_DS.PStyleMei, dbo.T_DS.WakiColorCode, 
                                  dbo.T_DS.WakiHaishoku, dbo.T_DS.WakiColorMei, dbo.T_DS.WakiPadKakouCode, dbo.T_DS.WakiPadKakouMei, dbo.T_DS.BeltColorCode, 
                                  dbo.T_DS.BeltHaishoku, dbo.T_DS.BeltColorMei, dbo.VIEW_DS_ShukkaMeisai.Hinban, dbo.VIEW_DS_ShukkaMeisai.Size, 
                                  dbo.VIEW_DS_ShukkaMeisai.Suryou, dbo.VIEW_DS_ShukkaMeisai.ShukkaSuu, dbo.VIEW_DS_ShukkaMeisai.KanryouFlg
FROM                     dbo.VIEW_BecchuKeyInfo INNER JOIN
                                  dbo.T_DS ON dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.T_DS.OrderKanriNo INNER JOIN
                                  dbo.VIEW_DS_ShukkaMeisai ON dbo.T_DS.OrderKanriNo = dbo.VIEW_DS_ShukkaMeisai.OrderKanriNo";


            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
            p.SetWhere(w, da.SelectCommand);


            if (!string.IsNullOrEmpty(w.WhereText))
                da.SelectCommand.CommandText += " where " + w.WhereText;
            


            ViewDataset.VIEW_DS_DownloadDataTable dt = new ViewDataset.VIEW_DS_DownloadDataTable();
            da.Fill(dt);

            return dt;
        }

        public static ViewDataset.VIEW_DS2_DownloadDataTable getVIEW_DS2_DownloadDataTable(Kensaku p, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);
            da.SelectCommand.CommandText = @"
            SELECT VIEW_DS2_Download.*
            FROM VIEW_DS2_Download LEFT OUTER JOIN
                T_Mark ON VIEW_DS2_Download.MizunoUketsukeBi = T_Mark.SashizuBi AND
                    VIEW_DS2_Download.OrderKanriNo = T_Mark.SashizuNo
            ";

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
            //p.SetWhere(w, da.SelectCommand);
            p.SetWhere2(w, da.SelectCommand);

            //if (!string.IsNullOrEmpty(w.WhereText))
            //    da.SelectCommand.CommandText += " where " + w.WhereText;

            if (!string.IsNullOrEmpty(w.WhereText))
                da.SelectCommand.CommandText += " where " + w.WhereText + " AND dbo.VIEW_DS2_Download.ShumokuCode != 'KA' AND dbo.VIEW_DS2_Download.ShumokuCode != 'KB' AND dbo.VIEW_DS2_Download.ShumokuCode != 'M2'";
            else
                da.SelectCommand.CommandText += " where dbo.VIEW_DS2_Download.ShumokuCode != 'KA' AND dbo.VIEW_DS2_Download.ShumokuCode != 'KB' AND dbo.VIEW_DS2_Download.ShumokuCode != 'M2'";

            //20170224 �ǉ� ����̎�ڃR�[�h�ł̓}�[�N���܂œo�^����Ă��Ȃ��Əo�͂���Ȃ��悤�ɂ���
            da.SelectCommand.CommandText += @" AND (dbo.VIEW_DS2_Download.ShumokuCode NOT IN ('QS', 'QR', 'QT', 'QB', 'QV', 'QA', 'SI', 'EZ', 'SE') OR
                (dbo.VIEW_DS2_Download.ShumokuCode IN ('QS', 'QR', 'QT', 'QB', 'QV', 'QA', 'SI', 'EZ', 'SE') AND 
                dbo.T_Mark.SashizuNo IS NOT NULL))";

            //QT,QP,QA,SI,SE�͕ʃ����[�X

            da.SelectCommand.CommandTimeout = 600000;

            ViewDataset.VIEW_DS2_DownloadDataTable dt = new ViewDataset.VIEW_DS2_DownloadDataTable();
            da.Fill(dt);

            return dt;
        }


        /// �O�ÒJ�C�� 2014/05/23
        public static ViewDataset.VIEW_SP_DownloadDataTable
            getVIEW_SP_DownloadDataTable(Kensaku p, SqlConnection c)
        {
            SqlDataAdapter da = new SqlDataAdapter("", c);

            /// SELECT * FROM VIEW_SP_Download �ɂ��Ȃ��̂�
            /// �����Ō������ڂ�}�������Ă���ׁB
            /// 
            /// 2014/09/01 ����l �\�[�g���w�� �I�[�_�[�Ǘ�No�A�i�ԁA�T�C�Y
            da.SelectCommand.CommandText = @"
SELECT
    T_A.ShumokuCode,
    T_A.ShumokuMei,
    T_A.TokuisakiCode,
    T_A.TokuisakiMei,
    T_A.BunruiMei,
    T_A.MizunoUketsukeBi,
    T_A.OrderKanriNo,
    T_A.TeamMei,
    T_A.OkyakusamaMei,
    T_A.ShinkiTsuika,
    T_A.MarkKakou,
    T_A.KoujyouShukkaYoteiBi,
    T_A.KoujyouShukkaShijiBi,
    T_A.Eigyousho,
    T_A.Chiku,
    T_A.MizunoTantousha,
    T_A.NouhinKigou,
    T_A.ShiiresakiCode,
    T_A.ShiiresakiMei,
    T_A.BecchuSiteOrderNo,
    T_A.[51OrderNo],
    T_A.BecchuSpectraNo,
    T_A.P51OrderNo,
    T_A.OkuriJyouNo,
    T_A.UnsouGyoushaMei,
    T_A.SKTantousha,
    T_A.CancelBi,
    T_A.SeizouSashizuNo,
    T_A.Hinban,
    T_A.Hinmei,
    T_A.Size,
    T_A.Suuryou,
    T_A.Kingaku,
    T_A.BuiAColorCode,
    T_A.BuiAColorMei,
    T_A.BuiBColorCode,
    T_A.BuiBColorMei,
    T_A.BuiCColorCode,
    T_A.BuiCColorMei,
    T_A.BuiDColorCode,
    T_A.BuiDColorMei,
    T_A.BuiEColorCode,
    T_A.BuiEColorMei,
    T_A.MarkShotaiCode,
    T_A.MarkShotaiShousai,
    T_A.MarkColorCode,
    T_A.MarkColorShousai,
    T_A.MarkIchiCode,
    T_A.MarkIchiShousai,
    T_A.MarkHouhouCode,
    T_A.MarkHouhouShousai,
    T_A.MarkMoji,
    T_A.T_SP_MarkKakou,
    T_A.OkurisakiMei,
    T_A.MarkHinban,
    ISNULL (dbo.T_BecchuShukkaInfo.ShukkaSuu, 0) AS ShukkaSuu,
    dbo.T_BecchuShukkaInfo.KanryouFlg,
    T_A.NiukeJigyoushoCode,
    T_A.HokanBasho,
    dbo.T_BecchuShukkaInfo.RowNo,
    T_A.ShiireUntinHacchuNo,
    T_A.ShukkasakiYubinBangou,
    T_A.ShukkasakiJusho,
    T_A.ShukkasakiTel,
    T_A.JanCode,
    T_A.SeisanTantou,
    T_A.YoukyuTani,
    T_A.TyokusouKubun,
    T_A.MizunoTantoushaCode,
    T_A.SyukkaSakiMei,
    T_A.ZenkaiOrderKanriNo,
    T_A.SozaiHinban1,
    T_A.SozaiYoushaku1,
    T_A.SozaiHinban2,
    T_A.SozaiYoushaku2,
    T_A.SozaiHinban3,
    T_A.SozaiYoushaku3,
    T_A.SozaiHinban4,
    T_A.SozaiYoushaku4,
    T_A.SozaiHinban5,
    T_A.SozaiYoushaku5,
    T_A.Tuika1_Hinban,
    T_A.Tuika1_Size,
    T_A.Tuika1_Suryou,
    T_A.Tuika1_LotNo,
    T_A.Tuika1_ShiireKingaku,
    T_A.Tuika2_Hinban,
    T_A.Tuika2_Size,
    T_A.Tuika2_Suryou,
    T_A.Tuika2_LotNo,
    T_A.Tuika2_ShiireKingaku,
    T_A.Tuika3_Hinban,
    T_A.Tuika3_Size,
    T_A.Tuika3_Suryou,
    T_A.Tuika3_LotNo,
    T_A.Tuika3_ShiireKingaku,
    T_A.OrderBikou,
    T_A.TukaCode,
    T_A.EigyouTantoushaCode,
	T_A.SKTantoushaCode,
	T_A.TermMei,
	T_A.MarkKakouNoteNo,
	T_A.SyatuA_SozaiHinbanMei,
	T_A.SyatuB_SozaiHinbanMei,
	T_A.SyatuC_SozaiHinbanMei,
	T_A.SyatuD_SozaiHInbanMei,
	T_A.SyatuE_SozaiHinbanMei,
	T_A.PantuA_SozaiHinbanMei,
	T_A.PantuB_SozaiHinbanMei,
	T_A.PantuC_SozaiHinbanMei,
	T_A.PantuD_SozaiHinbanMei,
	T_A.PantuE_SozaiHinbanMei,
	T_A.KeihiShukkaKubun,
	T_A.PadKakouKubun,
	T_A.PadHinban_display,
	T_A.PadKakouRyakusyou,
	T_A.PadKakouBikou,
	T_A.MuneMarkNaiyou,
	T_A.MuneMarkColorCodeA,
	T_A.MuneMarkColorCodeB,
	T_A.MuneMarkColorCodeC,
	T_A.MuneMarkColorCodeD,
	T_A.MuneMarkColorNameA,
	T_A.MuneMarkColorNameB,
	T_A.MuneMarkColorNameC,
	T_A.MuneMarkColorNameD,
	T_A.UniformNumberSyotai,
	T_A.UniformColorCodeA,
	T_A.UniformColorCodeB,
	T_A.UniformColorCodeC,
	T_A.UniformColorNameA,
	T_A.UniformColorNameB,
	T_A.UniformColorNameC,
	T_A.LeftSleeveSyotai,
	T_A.LeftSleeveNaiyou,
	T_A.LeftSleeveColorCodeA,
	T_A.LeftSleeveColorCodeB,
	T_A.LeftSleeveColorNameA,
	T_A.LeftSleeveColorNameB,
	T_A.MizunoLambertMark,
	T_A.SizeNumberName,
	T_A.MuneNumberPosition,
	T_A.MuneNumberSyotai,
	T_A.MuneNumberKakouHouhou,
	T_A.BackNamePosition,
	T_A.BackNameSyotai,
	T_A.BackNameKakouHouhou,
	T_A.RightSleevePosition,
	T_A.RightSleeveSyotai,
	T_A.RightSleeveKakouHouhou,
	T_A.RightSleeveNaiyou,
	T_A.OriginalMarkMunePosition,
	T_A.OriginalMarkMuneKakouHouhou,
	T_A.OriginalMarkRightSleevePosition,
	T_A.OriginalMarkRightSleeveKakouHouhou,
	T_A.OriginalMarkLeftSleevePosition,
	T_A.OriginalMarkLeftSleeveKakouHouhou,
	T_A.StripePitchNaiyou,
	T_A.PantuSizeNumber,
	T_A.KijiHinban,
	T_A.LineKataHinmoku,
	T_A.LineKataName,
	T_A.LinePattern,
	T_A.LineType1ColorCode,
	T_A.LineType1ColorName,
	T_A.LineType1ReverseColor,
	T_A.LineType2ColorCode,
	T_A.LineType2ColorName,
	T_A.LineType2ReverseColor,
	T_A.LineType3ColorCode,
	T_A.LineType3ColorName,
	T_A.LineType3ReverseColor,
	T_A.PipingColorCode,
	T_A.PipingColorName,
	T_A.StripeKata,
	T_A.Stripe1ColorCode,
	T_A.Stripe1ColorName,
	T_A.Stripe2ColorCode,
	T_A.Stripe2ColorName,
	T_A.RapierName,
	T_A.SmallCollar,
	T_A.SmallCollarBackColorCode,
	T_A.SmallCollarBackColorName,
	T_A.SideCut,
	T_A.DFCut,
	T_A.DummyOpen,
	T_A.PadKakouHinban,
	T_A.PadKakouBikou2,
	T_A.ButtonSelectNaiyou,
	T_A.Button1ColorCode,
	T_A.Button1ColorName,
	T_A.Button2ColorCode,
	T_A.Button2ColorName,
	T_A.RapierNameSyotai,
	T_A.RapierNameColorCode,
	T_A.RapierNameColorName,
	T_A.RapierNameMoji,
	T_A.Zekken,
	T_A.ZekkenKubun,
	T_A.ZekkenKasyo,
	T_A.ZekkenNumber,
	T_A.TermMeiZenMuneMarkingIchi,
	T_A.TermMeiZenMuneSyotai,
	T_A.TermMeiZenMuneDesignMoji,
	T_A.TermMeiZenMuneColorA,
	T_A.TermMeiZenMuneColorB,
	T_A.TermMeiZenMuneColorC,
	T_A.TermMeiZenMuneColorD,
	T_A.TermMeiZenMuneColorSu,
	T_A.TermMeiZenMuneSize,
	T_A.TermMeiZenMuneKakouHouhou,
	T_A.TermMeiZenMuneKiji,
	T_A.TermMeiRightMuneMarkingIchi,
	T_A.TermMeiRightMuneSyotai,
	T_A.TermMeiRightMuneDesignMoji,
	T_A.TermMeiRightMuneColorA,
	T_A.TermMeiRightMuneColorB,
	T_A.TermMeiRightMuneColorC,
	T_A.TermMeiRightMuneColorD,
	T_A.TermMeiRightMuneColorSu,
	T_A.TermMeiRightMuneSize,
	T_A.TermMeiRightMuneKakouHouhou,
	T_A.TermMeiRightMuneKiji,
	T_A.TermMeiLeftMuneMarkingIchi,
	T_A.TermMeiLeftMuneSyotai,
	T_A.TermMeiLeftMuneDesignMoji,
	T_A.TermMeiLeftMuneColorA,
	T_A.TermMeiLeftMuneColorB,
	T_A.TermMeiLeftMuneColorC,
	T_A.TermMeiLeftMuneColorD,
	T_A.TermMeiLeftMuneColorSu,
	T_A.TermMeiLeftMuneSize,
	T_A.TermMeiLeftMuneKakouHouhou,
	T_A.TermMeiLeftMuneKiji,
	T_A.TermMeiBackMarkingIchi,
	T_A.TermMeiBackSyotai,
	T_A.TermMeiBackDesignMoji,
	T_A.TermMeiBackColorA,
	T_A.TermMeiBackColorB,
	T_A.TermMeiBackColorC,
	T_A.TermMeiBackColorD,
	T_A.TermMeiBackColorSu,
	T_A.TermMeiBackSize,
	T_A.TermMeiBackKakouHouhou,
	T_A.TermMeiBackKiji,
	T_A.TermMeiRightSleeveMarkingIchi,
	T_A.TermMeiRightSleeveSyotai,
	T_A.TermMeiRightSleeveDesignMoji,
	T_A.TermMeiRightSleeveColorA,
	T_A.TermMeiRightSleeveColorB,
	T_A.TermMeiRightSleeveColorC,
	T_A.TermMeiRightSleeveColorD,
	T_A.TermMeiRightSleeveColorSu,
	T_A.TermMeiRightSleeveSize,
	T_A.TermMeiRightSleeveKakouHouhou,
	T_A.TermMeiRightSleeveKiji,
	T_A.TermMeiLeftSleeveMarkingIchi,
	T_A.TermMeiLeftSleeveSyotai,
	T_A.TermMeiLeftSleeveDesignMoji,
	T_A.TermMeiLeftSleeveColorA,
	T_A.TermMeiLeftSleeveColorB,
	T_A.TermMeiLeftSleeveColorC,
	T_A.TermMeiLeftSleeveColorD,
	T_A.TermMeiLeftSleeveColorSu,
	T_A.TermMeiLeftSleeveSize,
	T_A.TermMeiLeftSleeveKakouHouhou,
	T_A.TermMeiLeftSleeveKiji,
	T_A.TermMeiUnderCollarMarkingIchi,
	T_A.TermMeiUnderCollarSyotai,
	T_A.TermMeiUnderCollarDesignMoji,
	T_A.TermMeiUnderCollarColorA,
	T_A.TermMeiUnderCollarColorB,
	T_A.TermMeiUnderCollarColorC,
	T_A.TermMeiUnderCollarColorD,
	T_A.TermMeiUnderCollarColorSu,
	T_A.TermMeiUnderCollarSize,
	T_A.TermMeiUnderCollarKakouHouhou,
	T_A.TermMeiUnderCollarKiji,
	T_A.TermMeiUnderUniformMarkingIchi,
	T_A.TermMeiUnderUniformSyotai,
	T_A.TermMeiUnderUniformDesignMoji,
	T_A.TermMeiUnderUniformColorA,
	T_A.TermMeiUnderUniformColorB,
	T_A.TermMeiUnderUniformColorC,
	T_A.TermMeiUnderUniformColorD,
	T_A.TermMeiUnderUniformColorSu,
	T_A.TermMeiUnderUniformSize,
	T_A.TermMeiUnderUniformKakouHouhou,
	T_A.TermMeiUnderUniformKiji,
	T_A.TodoufukenMeiLeftSleeveMarkingIchi,
	T_A.TodoufukenMeiLeftSleeveSyotai,
	T_A.TodoufukenMeiLeftSleeveDesignMoji,
	T_A.TodoufukenMeiLeftSleeveColorA,
	T_A.TodoufukenMeiLeftSleeveColorB,
	T_A.TodoufukenMeiLeftSleeveColorC,
	T_A.TodoufukenMeiLeftSleeveColorD,
	T_A.TodoufukenMeiLeftSleeveColorSu,
	T_A.TodoufukenMeiLeftSleeveSize,
	T_A.TodoufukenMeiLeftSleeveKakouHouhou,
	T_A.TodoufukenMeiLeftSleeveKiji,
	T_A.TodoufukenMeiBackMarkingIchi,
	T_A.TodoufukenMeiBackSyotai,
	T_A.TodoufukenMeiBackDesignMoji,
	T_A.TodoufukenMeiBackColorA,
	T_A.TodoufukenMeiBackColorB,
	T_A.TodoufukenMeiBackColorC,
	T_A.TodoufukenMeiBackColorD,
	T_A.TodoufukenMeiBackColorSu,
	T_A.TodoufukenMeiBackSize,
	T_A.TodoufukenMeiBackKakouHouhou,
	T_A.TodoufukenMeiBackKiji,
	T_A.FrontNumberIchiAMuneCenterMarkingIchi,
	T_A.FrontNumberIchiAMuneCenterSyotai,
	T_A.FrontNumberIchiAMuneCenterDesignMoji,
	T_A.FrontNumberIchiAMuneCenterColorA,
	T_A.FrontNumberIchiAMuneCenterColorB,
	T_A.FrontNumberIchiAMuneCenterColorC,
	T_A.FrontNumberIchiAMuneCenterColorD,
	T_A.FrontNumberIchiAMuneCenterColorSu,
	T_A.FrontNumberIchiAMuneCenterSize,
	T_A.FrontNumberIchiAMuneCenterKakouHouhou,
	T_A.FrontNumberIchiAMuneCenterKiji,
	T_A.FrontNumberIchiBMarkingIchi,
	T_A.FrontNumberIchiBSyotai,
	T_A.FrontNumberIchiBDesignMoji,
	T_A.FrontNumberIchiBColorA,
	T_A.FrontNumberIchiBColorB,
	T_A.FrontNumberIchiBColorC,
	T_A.FrontNumberIchiBColorD,
	T_A.FrontNumberIchiBColorSu,
	T_A.FrontNumberIchiBSize,
	T_A.FrontNumberIchiBKakouHouhou,
	T_A.FrontNumberIchiBKiji,
	T_A.FrontNumberIchiCMarkingIchi,
	T_A.FrontNumberIchiCSyotai,
	T_A.FrontNumberIchiCDesignMoji,
	T_A.FrontNumberIchiCColorA,
	T_A.FrontNumberIchiCColorB,
	T_A.FrontNumberIchiCColorC,
	T_A.FrontNumberIchiCColorD,
	T_A.FrontNumberIchiCColorSu,
	T_A.FrontNumberIchiCSize,
	T_A.FrontNumberIchiCKakouHouhou,
	T_A.FrontNumberIchiCKiji,
	T_A.FrontNumberIchiDMarkingIchi,
	T_A.FrontNumberIchiDSyotai,
	T_A.FrontNumberIchiDDesignMoji,
	T_A.FrontNumberIchiDColorA,
	T_A.FrontNumberIchiDColorB,
	T_A.FrontNumberIchiDColorC,
	T_A.FrontNumberIchiDColorD,
	T_A.FrontNumberIchiDColorSu,
	T_A.FrontNumberIchiDSize,
	T_A.FrontNumberIchiDKakouHouhou,
	T_A.FrontNumberIchiDKiji,
	T_A.MuneNumberRightMarkingIchi,
	T_A.MuneNumberRightSyotai,
	T_A.MuneNumberRightDesignMoji,
	T_A.MuneNumberRightColorA,
	T_A.MuneNumberRightColorB,
	T_A.MuneNumberRightColorC,
	T_A.MuneNumberRightColorD,
	T_A.MuneNumberRightColorSu,
	T_A.MuneNumberRightSize,
	T_A.MuneNumberRightKakouHouhou,
	T_A.MuneNumberRightKiji,
	T_A.MuneNumberLeftMarkingIchi,
	T_A.MuneNumberLeftSyotai,
	T_A.MuneNumberLeftDesignMoji,
	T_A.MuneNumberLeftColorA,
	T_A.MuneNumberLeftColorB,
	T_A.MuneNumberLeftColorC,
	T_A.MuneNumberLeftColorD,
	T_A.MuneNumberLeftColorSu,
	T_A.MuneNumberLeftSize,
	T_A.MuneNumberLeftKakouHouhou,
	T_A.MuneNumberLeftKiji,
	T_A.UniformMarkingIchi,
	T_A.UniformSyotai,
	T_A.UniformDesignMoji,
	T_A.UniformColorA,
	T_A.UniformColorB,
	T_A.UniformColorC,
	T_A.UniformColorD,
	T_A.UniformColorSu,
	T_A.UniformSize,
	T_A.UniformKakouHouhou,
	T_A.UniformKiji,
	T_A.BackNameMarkingIchi,
	T_A.BackNameSyotai2,
	T_A.BackNameDesignMoji,
	T_A.BackNameColorA,
	T_A.BackNameColorB,
	T_A.BackNameColorC,
	T_A.BackNameColorD,
	T_A.BackNameColorSu,
	T_A.BackNameSize,
	T_A.BackNameKakouHouhou2,
	T_A.BackNameKiji,
	T_A.PantuNumberMarkingIchi,
	T_A.PantuNumberSyotai,
	T_A.PantuNumberDesignMoji,
	T_A.PantuNumberColorA,
	T_A.PantuNumberColorB,
	T_A.PantuNumberColorC,
	T_A.PantuNumberColorD,
	T_A.PantuNumberColorSu,
	T_A.PantuNumberSize,
	T_A.PantuNumberKakouHouhou,
	T_A.PantuNumberKiji,
	T_A.WaterMarkMuneMarkingIchi,
	T_A.WaterMarkMuneSyotai,
	T_A.WaterMarkMuneDesignMoji,
	T_A.WaterMarkMuneColorA,
	T_A.WaterMarkMuneColorB,
	T_A.WaterMarkMuneColorC,
	T_A.WaterMarkMuneColorD,
	T_A.WaterMarkMuneColorSu,
	T_A.WaterMarkMuneSize,
	T_A.WaterMarkMuneKakouHouhou,
	T_A.WaterMarkMuneKiji,
	T_A.WaterMarkBackMarkingIchi,
	T_A.WaterMarkBackSyotai,
	T_A.WaterMarkBackDesignMoji,
	T_A.WaterMarkBackColorA,
	T_A.WaterMarkBackColorB,
	T_A.WaterMarkBackColorC,
	T_A.WaterMarkBackColorD,
	T_A.WaterMarkBackColorSu,
	T_A.WaterMarkBackSize,
	T_A.WaterMarkBackKakouHouhou,
	T_A.WaterMarkBackKiji,
	T_A.CaptainMarkTopSize,
	T_A.CaptainMarkTopNumber,
	T_A.CaptainMarkUnderSize,
	T_A.CaptainMarkUnderNumber,
	T_A.OriginalMarkBackHinban,
	T_A.OriginalMarkBackSuryou,
	T_A.OriginalMarkBackKingaku,
	T_A.OriginalMarkBackIchi,
	T_A.OriginalMarkBackKakouHouhou,
	T_A.OriginalMarkBackColorCodeA,
	T_A.OriginalMarkBackColorNameA,
	T_A.OriginalMarkBackColorCodeB,
	T_A.OriginalMarkBackColorNameB,
	T_A.OriginalMarkBackColorCodeC,
	T_A.OriginalMarkBackColorNameC,
	T_A.OriginalMarkBackColorCodeD,
	T_A.OriginalMarkBackColorNameD,
	T_A.SyatuBuiACode,
	T_A.SyatuBuiAName,
	T_A.SyatuBuiAColor1,
	T_A.SyatuBuiAColor2,
	T_A.SyatuBuiAColor3,
	T_A.SyatuBuiAColor4,
	T_A.SyatuBuiAColor5,
	T_A.SyatuBuiBCode,
	T_A.SyatuBuiBName,
	T_A.SyatuBuiBColor1,
	T_A.SyatuBuiBColor2,
	T_A.SyatuBuiBColor3,
	T_A.SyatuBuiBColor4,
	T_A.SyatuBuiBColor5,
	T_A.SyatuBuiCCode,
	T_A.SyatuBuiCName,
	T_A.SyatuBuiCColor1,
	T_A.SyatuBuiCColor2,
	T_A.SyatuBuiCColor3,
	T_A.SyatuBuiCColor4,
	T_A.SyatuBuiCColor5,
	T_A.SyatuBuiDCode,
	T_A.SyatuBuiDName,
	T_A.SyatuBuiDColor1,
	T_A.SyatuBuiDColor2,
	T_A.SyatuBuiDColor3,
	T_A.SyatuBuiDColor4,
	T_A.SyatuBuiDColor5,
	T_A.SyatuBuiECode,
	T_A.SyatuBuiEName,
	T_A.SyatuBuiEColor1,
	T_A.SyatuBuiEColor2,
	T_A.SyatuBuiEColor3,
	T_A.SyatuBuiEColor4,
	T_A.SyatuBuiEColor5,
    T_A.SyatuBuiFCode,
    T_A.SyatuBuiFName,
    T_A.SyatuBuiFColor1,
    T_A.SyatuBuiFColor2,
    T_A.SyatuBuiFColor3,
    T_A.SyatuBuiFColor4,
    T_A.SyatuBuiFColor5,
    T_A.SyatuBuiGCode,
    T_A.SyatuBuiGName,
    T_A.SyatuBuiGColor1,
    T_A.SyatuBuiGColor2,
    T_A.SyatuBuiGColor3,
    T_A.SyatuBuiGColor4,
    T_A.SyatuBuiGColor5,
    T_A.PantuBuiACode,
    T_A.PantuBuiAName,
    T_A.PantuBuiAColor1,
    T_A.PantuBuiAColor2,
    T_A.PantuBuiAColor3,
    T_A.PantuBuiAColor4,
    T_A.PantuBuiAColor5,
    T_A.PantuBuiBCode,
    T_A.PantuBuiBName,
    T_A.PantuBuiBColor1,
    T_A.PantuBuiBColor2,
    T_A.PantuBuiBColor3,
    T_A.PantuBuiBColor4,
    T_A.PantuBuiBColor5,
    T_A.PantuBuiCCode,
    T_A.PantuBuiCName,
    T_A.PantuBuiCColor1,
    T_A.PantuBuiCColor2,
    T_A.PantuBuiCColor3,
    T_A.PantuBuiCColor4,
    T_A.PantuBuiCColor5,
	T_A.SimulatorURL,
    T_A.SimulatorID,
    T_A.ZekkenMuneNumberOptionPosition,
    T_A.ZekkenMuneNumberOptionSyotai,
    T_A.ZekkenMuneNumberOptionKakouHouhou,
    T_A.ZekkenBackNumberOptionPosition,
    T_A.ZekkenBackNumberOptionSyotai,
    T_A.ZekkenBackNumberOptionKakouHouhou,
    T_A.LeftSleeveOptionPosition,
    T_A.LeftSleeveOptionSyotai,
    T_A.LeftSleeveOptionKakouHouhou,
    T_A.LeftSleeveOptionMarkNaiyou,
    T_A.ZenMuneOptionPosition,
    T_A.ZenMuneOptionSyotai,
    T_A.ZenMuneOptionKakouHouhou,
    T_A.ZenMuneOptionMarkNaiyou,
    T_A.LeftMuneOptionPosition,
    T_A.LeftMuneOptionSyotai,
    T_A.LeftMuneOptionKakouHouhou,
    T_A.LeftMuneOptionMarkNaiyou,
    T_A.BackAOptionPosition,
    T_A.BackAOptionSyotai,
    T_A.BackAOptionKakouHouhou,
    T_A.BackAOptionMarkNaiyou,
    T_A.BackBOptionPosition,
    T_A.BackBOptionSyotai,
    T_A.BackBOptionKakouHouhou,
    T_A.BackBOptionMarkNaiyou,
    T_A.BackCOptionPosition,
    T_A.BackCOptionSyotai,
    T_A.BackCOptionKakouHouhou,
    T_A.BackCOptionMarkNaiyou,
    T_A.BackDOptionPosition,
    T_A.BackDOptionSyotai,
    T_A.BackDOptionKakouHouhou,
    T_A.BackDOptionMarkNaiyou,
    T_A.BackUnderCollarOptionPosition,
    T_A.BackUnderCollarOptionSyotai,
    T_A.BackUnderCollarOptionKakouHouhou,
    T_A.BackUnderCollarOptionMarkNaiyou,
    T_A.OriginalMarkLeftPosition,
    T_A.OriginalMarkRightMunePosition,
    T_A.OriginalMarkBackAPosition,
    T_A.OriginalMarkBackBPosition,
    T_A.OriginalMarkBackCPosition,
    T_A.OriginalMarkBackDPosition,
    T_A.OriginalMarkBackUnderCollarPosition,
    T_A.OriginalMarkPantuRightHemPosition,
    T_A.SyatuBuiHCode,
    T_A.SyatuBuiHName,
    T_A.SyatuBuiHColor1,
    T_A.SyatuBuiHColor2,
    T_A.SyatuBuiHColor3,
    T_A.SyatuBuiHColor4,
    T_A.SyatuBuiHColor5,
    T_A.SyatuBuiICode,
    T_A.SyatuBuiIName,
    T_A.SyatuBuiIColor1,
    T_A.SyatuBuiIColor2,
    T_A.SyatuBuiIColor3,
    T_A.SyatuBuiIColor4,
    T_A.SyatuBuiIColor5,
    T_A.SyatuBuiJCode,
    T_A.SyatuBuiJName,
    T_A.SyatuBuiJColor1,
    T_A.SyatuBuiJColor2,
    T_A.SyatuBuiJColor3,
    T_A.SyatuBuiJColor4,
    T_A.SyatuBuiJColor5,
    T_A.PantuBuiDCode,
    T_A.PantuBuiDName,
    T_A.PantuBuiDColor1,
    T_A.PantuBuiDColor2,
    T_A.PantuBuiDColor3,
    T_A.PantuBuiDColor4,
    T_A.PantuBuiDColor5,
    T_A.PantuBuiECode,
    T_A.PantuBuiEName,
    T_A.PantuBuiEColor1,
    T_A.PantuBuiEColor2,
    T_A.PantuBuiEColor3,
    T_A.PantuBuiEColor4,
    T_A.PantuBuiEColor5,
    T_A.PantuBuiFCode,
    T_A.PantuBuiFName,
    T_A.PantuBuiFColor1,
    T_A.PantuBuiFColor2,
    T_A.PantuBuiFColor3,
    T_A.PantuBuiFColor4,
    T_A.PantuBuiFColor5,
    T_A.PantuBuiGCode,
    T_A.PantuBuiGName,
    T_A.PantuBuiGColor1,
    T_A.PantuBuiGColor2,
    T_A.PantuBuiGColor3,
    T_A.PantuBuiGColor4,
    T_A.PantuBuiGColor5,
    T_A.PantuBuiHCode,
    T_A.PantuBuiHName,
    T_A.PantuBuiHColor1,
    T_A.PantuBuiHColor2,
    T_A.PantuBuiHColor3,
    T_A.PantuBuiHColor4,
    T_A.PantuBuiHColor5,
    T_A.PantuBuiICode,
    T_A.PantuBuiIName,
    T_A.PantuBuiIColor1,
    T_A.PantuBuiIColor2,
    T_A.PantuBuiIColor3,
    T_A.PantuBuiIColor4,
    T_A.PantuBuiIColor5,
    T_A.PantuBuiJCode,
    T_A.PantuBuiJName,
    T_A.PantuBuiJColor1,
    T_A.PantuBuiJColor2,
    T_A.PantuBuiJColor3,
    T_A.PantuBuiJColor4,
    T_A.PantuBuiJColor5,
    T_A.Back1MarkingIchi,
    T_A.Back1Syotai,
    T_A.Back1DesignMoji,
    T_A.Back1ColorA,
    T_A.Back1ColorB,
    T_A.Back1ColorC,
    T_A.Back1ColorD,
    T_A.Back1ColorSu,
    T_A.Back1Size,
    T_A.Back1KakouHouhou,
    T_A.Back1Kiji,
    T_A.Back2MarkingIchi,
    T_A.Back2Syotai,
    T_A.Back2DesignMoji,
    T_A.Back2ColorA,
    T_A.Back2ColorB,
    T_A.Back2ColorC,
    T_A.Back2ColorD,
    T_A.Back2ColorSu,
    T_A.Back2Size,
    T_A.Back2KakouHouhou,
    T_A.Back2Kiji,
    T_A.Back3MarkingIchi,
    T_A.Back3Syotai,
    T_A.Back3DesignMoji,
    T_A.Back3ColorA,
    T_A.Back3ColorB,
    T_A.Back3ColorC,
    T_A.Back3ColorD,
    T_A.Back3ColorSu,
    T_A.Back3Size,
    T_A.Back3KakouHouhou,
    T_A.Back3Kiji
FROM
    (
        SELECT
            dbo.VIEW_BecchuKeyInfo.ShumokuCode,
            dbo.VIEW_BecchuKeyInfo.ShumokuMei,
            dbo.VIEW_BecchuKeyInfo.TokuisakiCode,
            dbo.VIEW_BecchuKeyInfo.TokuisakiMei,
            dbo.VIEW_BecchuKeyInfo.BunruiMei,
            dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi,
            dbo.VIEW_BecchuKeyInfo.OrderKanriNo,
            dbo.VIEW_BecchuKeyInfo.TeamMei,
            dbo.VIEW_BecchuKeyInfo.OkyakusamaMei,
            dbo.VIEW_BecchuKeyInfo.ShinkiTsuika,
            dbo.VIEW_BecchuKeyInfo.MarkKakou,
            dbo.VIEW_BecchuKeyInfo.KoujyouShukkaYoteiBi,
            dbo.VIEW_BecchuKeyInfo.KoujyouShukkaShijiBi,
            dbo.VIEW_BecchuKeyInfo.Eigyousho,
            dbo.VIEW_BecchuKeyInfo.Chiku,
            dbo.VIEW_BecchuKeyInfo.MizunoTantousha,
            dbo.VIEW_BecchuKeyInfo.NouhinKigou,
            dbo.VIEW_BecchuKeyInfo.ShiiresakiCode,
            dbo.VIEW_BecchuKeyInfo.ShiiresakiMei,
            dbo.VIEW_BecchuKeyInfo.BecchuSiteOrderNo,
            dbo.VIEW_BecchuKeyInfo.[51OrderNo],
            dbo.VIEW_BecchuKeyInfo.BecchuSpectraNo,
            dbo.VIEW_BecchuKeyInfo.P51OrderNo,
            dbo.VIEW_BecchuKeyInfo.OkuriJyouNo,
            dbo.VIEW_BecchuKeyInfo.UnsouGyoushaMei,
            dbo.VIEW_BecchuKeyInfo.SKTantousha,
            dbo.VIEW_BecchuKeyInfo.CancelBi,
            dbo.VIEW_SPMeisai.NiukeHokanBasho AS HokanBasho,
            dbo.VIEW_SPMeisai.NiukeJigyoushoCode,
            dbo.VIEW_SPMeisai.SeizouSashizuNo,
            dbo.VIEW_SPMeisai.Hinban,
            dbo.VIEW_SPMeisai.Hinmei,
            dbo.VIEW_SPMeisai.Size,
            dbo.VIEW_SPMeisai.Suuryou,
            dbo.VIEW_SPMeisai.Kingaku,
            dbo.VIEW_SPMeisai.BuiAColorCode,
            dbo.VIEW_SPMeisai.BuiAColorMei,
            dbo.VIEW_SPMeisai.BuiBColorCode,
            dbo.VIEW_SPMeisai.BuiBColorMei,
            dbo.VIEW_SPMeisai.BuiCColorCode,
            dbo.VIEW_SPMeisai.BuiCColorMei,
            dbo.VIEW_SPMeisai.BuiDColorCode,
            dbo.VIEW_SPMeisai.BuiDColorMei,
            dbo.VIEW_SPMeisai.BuiEColorCode,
            dbo.VIEW_SPMeisai.BuiEColorMei,
            dbo.VIEW_SPMeisai.MarkShotaiCode,
            dbo.VIEW_SPMeisai.MarkShotaiShousai,
            dbo.VIEW_SPMeisai.MarkColorCode,
            dbo.VIEW_SPMeisai.MarkColorShousai,
            dbo.VIEW_SPMeisai.MarkIchiCode,
            dbo.VIEW_SPMeisai.MarkIchiShousai,
            dbo.VIEW_SPMeisai.MarkHouhouCode,
            dbo.VIEW_SPMeisai.MarkHouhouShousai,
            dbo.VIEW_SPMeisai.MarkMoji,
            dbo.VIEW_SPMeisai.MarkKakou AS T_SP_MarkKakou,
            dbo.VIEW_SPMeisai.OkurisakiMei,
            dbo.VIEW_SPMeisai.MarkHinban,
            VIEW_SPMeisai.ShiireUntinHacchuNo,
            VIEW_SPMeisai.ShukkasakiYubinBangou,
            VIEW_SPMeisai.ShukkasakiJusho,
            VIEW_SPMeisai.ShukkasakiTel,
            VIEW_SPMeisai.JanCode,
            dbo.VIEW_BecchuKeyInfo.ZenkaiOrderKanriNo,
            dbo.VIEW_SPMeisai.SozaiHinban1,
            dbo.VIEW_SPMeisai.SozaiYoushaku1,
            dbo.VIEW_SPMeisai.SozaiHinban2,
            dbo.VIEW_SPMeisai.SozaiYoushaku2,
            dbo.VIEW_SPMeisai.SozaiHinban3,
            dbo.VIEW_SPMeisai.SozaiYoushaku3,
            dbo.VIEW_SPMeisai.SozaiHinban4,
            dbo.VIEW_SPMeisai.SozaiYoushaku4,
            dbo.VIEW_SPMeisai.SozaiHinban5,
            dbo.VIEW_SPMeisai.SozaiYoushaku5,
            dbo.VIEW_SPMeisai.Tuika1_Hinban,
            dbo.VIEW_SPMeisai.Tuika1_Size,
            dbo.VIEW_SPMeisai.Tuika1_Suryou,
            dbo.VIEW_SPMeisai.Tuika1_LotNo,
            dbo.VIEW_SPMeisai.Tuika1_ShiireKingaku,
            dbo.VIEW_SPMeisai.Tuika2_Hinban,
            dbo.VIEW_SPMeisai.Tuika2_Size,
            dbo.VIEW_SPMeisai.Tuika2_Suryou,
            dbo.VIEW_SPMeisai.Tuika2_LotNo,
            dbo.VIEW_SPMeisai.Tuika2_ShiireKingaku,
            dbo.VIEW_SPMeisai.Tuika3_Hinban,
            dbo.VIEW_SPMeisai.Tuika3_Size,
            dbo.VIEW_SPMeisai.Tuika3_Suryou,
            dbo.VIEW_SPMeisai.Tuika3_LotNo,
            dbo.VIEW_SPMeisai.Tuika3_ShiireKingaku,
            dbo.VIEW_BecchuKeyInfo.SeisanTantou,
            dbo.VIEW_BecchuKeyInfo.YoukyuTani,
            dbo.VIEW_SPMeisai.TyokusouKubun,
            dbo.VIEW_BecchuKeyInfo.MizunoTantoushaCode,
            dbo.VIEW_SPMeisai.SyukkaSakiMei,
            dbo.VIEW_SPMeisai.DLSort,
            dbo.VIEW_BecchuKeyInfo.OrderBikou,
            dbo.VIEW_SPMeisai.TukaCode,
			dbo.T_SP.EigyouTantoushaCode,
			dbo.T_SP.SKTantoushaCode,
			dbo.T_SP.TermMei,
			dbo.T_SP.MarkKakouNoteNo,
			dbo.T_SP.SyatuA_SozaiHinbanMei,
			dbo.T_SP.SyatuB_SozaiHinbanMei,
			dbo.T_SP.SyatuC_SozaiHinbanMei,
			dbo.T_SP.SyatuD_SozaiHInbanMei,
			dbo.T_SP.SyatuE_SozaiHinbanMei,
			dbo.T_SP.PantuA_SozaiHinbanMei,
			dbo.T_SP.PantuB_SozaiHinbanMei,
			dbo.T_SP.PantuC_SozaiHinbanMei,
			dbo.T_SP.PantuD_SozaiHinbanMei,
			dbo.T_SP.PantuE_SozaiHinbanMei,
			dbo.T_SP.KeihiShukkaKubun,
			dbo.T_SP.PadKakouKubun,
			dbo.T_SP.PadHinban_display,
			dbo.T_SP.PadKakouRyakusyou,
			dbo.T_SP.PadKakouBikou,
			dbo.T_SP.MuneMarkNaiyou,
			dbo.T_SP.MuneMarkColorCodeA,
			dbo.T_SP.MuneMarkColorCodeB,
			dbo.T_SP.MuneMarkColorCodeC,
			dbo.T_SP.MuneMarkColorCodeD,
			dbo.T_SP.MuneMarkColorNameA,
			dbo.T_SP.MuneMarkColorNameB,
			dbo.T_SP.MuneMarkColorNameC,
			dbo.T_SP.MuneMarkColorNameD,
			dbo.T_SP.UniformNumberSyotai,
			dbo.T_SP.UniformColorCodeA,
			dbo.T_SP.UniformColorCodeB,
			dbo.T_SP.UniformColorCodeC,
			dbo.T_SP.UniformColorNameA,
			dbo.T_SP.UniformColorNameB,
			dbo.T_SP.UniformColorNameC,
			dbo.T_SP.LeftSleeveSyotai,
			dbo.T_SP.LeftSleeveNaiyou,
			dbo.T_SP.LeftSleeveColorCodeA,
			dbo.T_SP.LeftSleeveColorCodeB,
			dbo.T_SP.LeftSleeveColorNameA,
			dbo.T_SP.LeftSleeveColorNameB,
			dbo.T_SP.MizunoLambertMark,
			dbo.T_SP.SizeNumberName,
			dbo.T_SP.MuneNumberPosition,
			dbo.T_SP.MuneNumberSyotai,
			dbo.T_SP.MuneNumberKakouHouhou,
			dbo.T_SP.BackNamePosition,
			dbo.T_SP.BackNameSyotai,
			dbo.T_SP.BackNameKakouHouhou,
			dbo.T_SP.RightSleevePosition,
			dbo.T_SP.RightSleeveSyotai,
			dbo.T_SP.RightSleeveKakouHouhou,
			dbo.T_SP.RightSleeveNaiyou,
			dbo.T_SP.OriginalMarkMunePosition,
			dbo.T_SP.OriginalMarkMuneKakouHouhou,
			dbo.T_SP.OriginalMarkRightSleevePosition,
			dbo.T_SP.OriginalMarkRightSleeveKakouHouhou,
			dbo.T_SP.OriginalMarkLeftSleevePosition,
			dbo.T_SP.OriginalMarkLeftSleeveKakouHouhou,
			dbo.T_SP2.StripePitchNaiyou,
			dbo.T_SP2.PantuSizeNumber,
			dbo.T_SP2.KijiHinban,
			dbo.T_SP2.LineKataHinmoku,
			dbo.T_SP2.LineKataName,
			dbo.T_SP2.LinePattern,
			dbo.T_SP2.LineType1ColorCode,
			dbo.T_SP2.LineType1ColorName,
			dbo.T_SP2.LineType1ReverseColor,
			dbo.T_SP2.LineType2ColorCode,
			dbo.T_SP2.LineType2ColorName,
			dbo.T_SP2.LineType2ReverseColor,
			dbo.T_SP2.LineType3ColorCode,
			dbo.T_SP2.LineType3ColorName,
			dbo.T_SP2.LineType3ReverseColor,
			dbo.T_SP2.PipingColorCode,
			dbo.T_SP2.PipingColorName,
			dbo.T_SP2.StripeKata,
			dbo.T_SP2.Stripe1ColorCode,
			dbo.T_SP2.Stripe1ColorName,
			dbo.T_SP2.Stripe2ColorCode,
			dbo.T_SP2.Stripe2ColorName,
			dbo.T_SP2.RapierName,
			dbo.T_SP2.SmallCollar,
			dbo.T_SP2.SmallCollarBackColorCode,
			dbo.T_SP2.SmallCollarBackColorName,
			dbo.T_SP2.SideCut,
			dbo.T_SP2.DFCut,
			dbo.T_SP2.DummyOpen,
			dbo.T_SP2.PadKakouHinban,
			dbo.T_SP2.PadKakouBikou2,
			dbo.T_SP2.ButtonSelectNaiyou,
			dbo.T_SP2.Button1ColorCode,
			dbo.T_SP2.Button1ColorName,
			dbo.T_SP2.Button2ColorCode,
			dbo.T_SP2.Button2ColorName,
			dbo.T_SP2.RapierNameSyotai,
			dbo.T_SP2.RapierNameColorCode,
			dbo.T_SP2.RapierNameColorName,
			dbo.T_SP2.RapierNameMoji,
			dbo.T_SP2.Zekken,
			dbo.T_SP2.ZekkenKubun,
			dbo.T_SP2.ZekkenKasyo,
			dbo.T_SP2.ZekkenNumber,
			dbo.T_SP2.TermMeiZenMuneMarkingIchi,
			dbo.T_SP2.TermMeiZenMuneSyotai,
			dbo.T_SP2.TermMeiZenMuneDesignMoji,
			dbo.T_SP2.TermMeiZenMuneColorA,
			dbo.T_SP2.TermMeiZenMuneColorB,
			dbo.T_SP2.TermMeiZenMuneColorC,
			dbo.T_SP2.TermMeiZenMuneColorD,
			dbo.T_SP2.TermMeiZenMuneColorSu,
			dbo.T_SP2.TermMeiZenMuneSize,
			dbo.T_SP2.TermMeiZenMuneKakouHouhou,
			dbo.T_SP2.TermMeiZenMuneKiji,
			dbo.T_SP2.TermMeiRightMuneMarkingIchi,
			dbo.T_SP2.TermMeiRightMuneSyotai,
			dbo.T_SP2.TermMeiRightMuneDesignMoji,
			dbo.T_SP2.TermMeiRightMuneColorA,
			dbo.T_SP2.TermMeiRightMuneColorB,
			dbo.T_SP2.TermMeiRightMuneColorC,
			dbo.T_SP2.TermMeiRightMuneColorD,
			dbo.T_SP2.TermMeiRightMuneColorSu,
			dbo.T_SP2.TermMeiRightMuneSize,
			dbo.T_SP2.TermMeiRightMuneKakouHouhou,
			dbo.T_SP2.TermMeiRightMuneKiji,
			dbo.T_SP2.TermMeiLeftMuneMarkingIchi,
			dbo.T_SP2.TermMeiLeftMuneSyotai,
			dbo.T_SP2.TermMeiLeftMuneDesignMoji,
			dbo.T_SP2.TermMeiLeftMuneColorA,
			dbo.T_SP2.TermMeiLeftMuneColorB,
			dbo.T_SP2.TermMeiLeftMuneColorC,
			dbo.T_SP2.TermMeiLeftMuneColorD,
			dbo.T_SP2.TermMeiLeftMuneColorSu,
			dbo.T_SP2.TermMeiLeftMuneSize,
			dbo.T_SP2.TermMeiLeftMuneKakouHouhou,
			dbo.T_SP2.TermMeiLeftMuneKiji,
			dbo.T_SP2.TermMeiBackMarkingIchi,
			dbo.T_SP2.TermMeiBackSyotai,
			dbo.T_SP2.TermMeiBackDesignMoji,
			dbo.T_SP2.TermMeiBackColorA,
			dbo.T_SP2.TermMeiBackColorB,
			dbo.T_SP2.TermMeiBackColorC,
			dbo.T_SP2.TermMeiBackColorD,
			dbo.T_SP2.TermMeiBackColorSu,
			dbo.T_SP2.TermMeiBackSize,
			dbo.T_SP2.TermMeiBackKakouHouhou,
			dbo.T_SP2.TermMeiBackKiji,
			dbo.T_SP2.TermMeiRightSleeveMarkingIchi,
			dbo.T_SP2.TermMeiRightSleeveSyotai,
			dbo.T_SP2.TermMeiRightSleeveDesignMoji,
			dbo.T_SP2.TermMeiRightSleeveColorA,
			dbo.T_SP2.TermMeiRightSleeveColorB,
			dbo.T_SP2.TermMeiRightSleeveColorC,
			dbo.T_SP2.TermMeiRightSleeveColorD,
			dbo.T_SP2.TermMeiRightSleeveColorSu,
			dbo.T_SP2.TermMeiRightSleeveSize,
			dbo.T_SP2.TermMeiRightSleeveKakouHouhou,
			dbo.T_SP2.TermMeiRightSleeveKiji,
			dbo.T_SP2.TermMeiLeftSleeveMarkingIchi,
			dbo.T_SP2.TermMeiLeftSleeveSyotai,
			dbo.T_SP2.TermMeiLeftSleeveDesignMoji,
			dbo.T_SP2.TermMeiLeftSleeveColorA,
			dbo.T_SP2.TermMeiLeftSleeveColorB,
			dbo.T_SP2.TermMeiLeftSleeveColorC,
			dbo.T_SP2.TermMeiLeftSleeveColorD,
			dbo.T_SP2.TermMeiLeftSleeveColorSu,
			dbo.T_SP2.TermMeiLeftSleeveSize,
			dbo.T_SP2.TermMeiLeftSleeveKakouHouhou,
			dbo.T_SP2.TermMeiLeftSleeveKiji,
			dbo.T_SP2.TermMeiUnderCollarMarkingIchi,
			dbo.T_SP2.TermMeiUnderCollarSyotai,
			dbo.T_SP2.TermMeiUnderCollarDesignMoji,
			dbo.T_SP2.TermMeiUnderCollarColorA,
			dbo.T_SP2.TermMeiUnderCollarColorB,
			dbo.T_SP2.TermMeiUnderCollarColorC,
			dbo.T_SP2.TermMeiUnderCollarColorD,
			dbo.T_SP2.TermMeiUnderCollarColorSu,
			dbo.T_SP2.TermMeiUnderCollarSize,
			dbo.T_SP2.TermMeiUnderCollarKakouHouhou,
			dbo.T_SP2.TermMeiUnderCollarKiji,
			dbo.T_SP2.TermMeiUnderUniformMarkingIchi,
			dbo.T_SP2.TermMeiUnderUniformSyotai,
			dbo.T_SP2.TermMeiUnderUniformDesignMoji,
			dbo.T_SP2.TermMeiUnderUniformColorA,
			dbo.T_SP2.TermMeiUnderUniformColorB,
			dbo.T_SP2.TermMeiUnderUniformColorC,
			dbo.T_SP2.TermMeiUnderUniformColorD,
			dbo.T_SP2.TermMeiUnderUniformColorSu,
			dbo.T_SP2.TermMeiUnderUniformSize,
			dbo.T_SP2.TermMeiUnderUniformKakouHouhou,
			dbo.T_SP2.TermMeiUnderUniformKiji,
			dbo.T_SP2.TodoufukenMeiLeftSleeveMarkingIchi,
			dbo.T_SP2.TodoufukenMeiLeftSleeveSyotai,
			dbo.T_SP2.TodoufukenMeiLeftSleeveDesignMoji,
			dbo.T_SP2.TodoufukenMeiLeftSleeveColorA,
			dbo.T_SP2.TodoufukenMeiLeftSleeveColorB,
			dbo.T_SP2.TodoufukenMeiLeftSleeveColorC,
			dbo.T_SP2.TodoufukenMeiLeftSleeveColorD,
			dbo.T_SP2.TodoufukenMeiLeftSleeveColorSu,
			dbo.T_SP2.TodoufukenMeiLeftSleeveSize,
			dbo.T_SP2.TodoufukenMeiLeftSleeveKakouHouhou,
			dbo.T_SP2.TodoufukenMeiLeftSleeveKiji,
			dbo.T_SP2.TodoufukenMeiBackMarkingIchi,
			dbo.T_SP2.TodoufukenMeiBackSyotai,
			dbo.T_SP2.TodoufukenMeiBackDesignMoji,
			dbo.T_SP2.TodoufukenMeiBackColorA,
			dbo.T_SP2.TodoufukenMeiBackColorB,
			dbo.T_SP2.TodoufukenMeiBackColorC,
			dbo.T_SP2.TodoufukenMeiBackColorD,
			dbo.T_SP2.TodoufukenMeiBackColorSu,
			dbo.T_SP2.TodoufukenMeiBackSize,
			dbo.T_SP2.TodoufukenMeiBackKakouHouhou,
			dbo.T_SP2.TodoufukenMeiBackKiji,
			dbo.T_SP2.FrontNumberIchiAMuneCenterMarkingIchi,
			dbo.T_SP2.FrontNumberIchiAMuneCenterSyotai,
			dbo.T_SP2.FrontNumberIchiAMuneCenterDesignMoji,
			dbo.T_SP2.FrontNumberIchiAMuneCenterColorA,
			dbo.T_SP2.FrontNumberIchiAMuneCenterColorB,
			dbo.T_SP2.FrontNumberIchiAMuneCenterColorC,
			dbo.T_SP2.FrontNumberIchiAMuneCenterColorD,
			dbo.T_SP2.FrontNumberIchiAMuneCenterColorSu,
			dbo.T_SP2.FrontNumberIchiAMuneCenterSize,
			dbo.T_SP2.FrontNumberIchiAMuneCenterKakouHouhou,
			dbo.T_SP2.FrontNumberIchiAMuneCenterKiji,
			dbo.T_SP2.FrontNumberIchiBMarkingIchi,
			dbo.T_SP2.FrontNumberIchiBSyotai,
			dbo.T_SP2.FrontNumberIchiBDesignMoji,
			dbo.T_SP2.FrontNumberIchiBColorA,
			dbo.T_SP2.FrontNumberIchiBColorB,
			dbo.T_SP2.FrontNumberIchiBColorC,
			dbo.T_SP2.FrontNumberIchiBColorD,
			dbo.T_SP2.FrontNumberIchiBColorSu,
			dbo.T_SP2.FrontNumberIchiBSize,
			dbo.T_SP2.FrontNumberIchiBKakouHouhou,
			dbo.T_SP2.FrontNumberIchiBKiji,
			dbo.T_SP2.FrontNumberIchiCMarkingIchi,
			dbo.T_SP2.FrontNumberIchiCSyotai,
			dbo.T_SP2.FrontNumberIchiCDesignMoji,
			dbo.T_SP2.FrontNumberIchiCColorA,
			dbo.T_SP2.FrontNumberIchiCColorB,
			dbo.T_SP2.FrontNumberIchiCColorC,
			dbo.T_SP2.FrontNumberIchiCColorD,
			dbo.T_SP2.FrontNumberIchiCColorSu,
			dbo.T_SP2.FrontNumberIchiCSize,
			dbo.T_SP2.FrontNumberIchiCKakouHouhou,
			dbo.T_SP2.FrontNumberIchiCKiji,
			dbo.T_SP2.FrontNumberIchiDMarkingIchi,
			dbo.T_SP2.FrontNumberIchiDSyotai,
			dbo.T_SP2.FrontNumberIchiDDesignMoji,
			dbo.T_SP2.FrontNumberIchiDColorA,
			dbo.T_SP2.FrontNumberIchiDColorB,
			dbo.T_SP2.FrontNumberIchiDColorC,
			dbo.T_SP2.FrontNumberIchiDColorD,
			dbo.T_SP2.FrontNumberIchiDColorSu,
			dbo.T_SP2.FrontNumberIchiDSize,
			dbo.T_SP2.FrontNumberIchiDKakouHouhou,
			dbo.T_SP2.FrontNumberIchiDKiji,
			dbo.T_SP3.MuneNumberRightMarkingIchi,
			dbo.T_SP3.MuneNumberRightSyotai,
			dbo.T_SP3.MuneNumberRightDesignMoji,
			dbo.T_SP3.MuneNumberRightColorA,
			dbo.T_SP3.MuneNumberRightColorB,
			dbo.T_SP3.MuneNumberRightColorC,
			dbo.T_SP3.MuneNumberRightColorD,
			dbo.T_SP3.MuneNumberRightColorSu,
			dbo.T_SP3.MuneNumberRightSize,
			dbo.T_SP3.MuneNumberRightKakouHouhou,
			dbo.T_SP3.MuneNumberRightKiji,
			dbo.T_SP3.MuneNumberLeftMarkingIchi,
			dbo.T_SP3.MuneNumberLeftSyotai,
			dbo.T_SP3.MuneNumberLeftDesignMoji,
			dbo.T_SP3.MuneNumberLeftColorA,
			dbo.T_SP3.MuneNumberLeftColorB,
			dbo.T_SP3.MuneNumberLeftColorC,
			dbo.T_SP3.MuneNumberLeftColorD,
			dbo.T_SP3.MuneNumberLeftColorSu,
			dbo.T_SP3.MuneNumberLeftSize,
			dbo.T_SP3.MuneNumberLeftKakouHouhou,
			dbo.T_SP3.MuneNumberLeftKiji,
			dbo.T_SP3.UniformMarkingIchi,
			dbo.T_SP3.UniformSyotai,
			dbo.T_SP3.UniformDesignMoji,
			dbo.T_SP3.UniformColorA,
			dbo.T_SP3.UniformColorB,
			dbo.T_SP3.UniformColorC,
			dbo.T_SP3.UniformColorD,
			dbo.T_SP3.UniformColorSu,
			dbo.T_SP3.UniformSize,
			dbo.T_SP3.UniformKakouHouhou,
			dbo.T_SP3.UniformKiji,
			dbo.T_SP3.BackNameMarkingIchi,
			dbo.T_SP3.BackNameSyotai2,
			dbo.T_SP3.BackNameDesignMoji,
			dbo.T_SP3.BackNameColorA,
			dbo.T_SP3.BackNameColorB,
			dbo.T_SP3.BackNameColorC,
			dbo.T_SP3.BackNameColorD,
			dbo.T_SP3.BackNameColorSu,
			dbo.T_SP3.BackNameSize,
			dbo.T_SP3.BackNameKakouHouhou2,
			dbo.T_SP3.BackNameKiji,
			dbo.T_SP3.PantuNumberMarkingIchi,
			dbo.T_SP3.PantuNumberSyotai,
			dbo.T_SP3.PantuNumberDesignMoji,
			dbo.T_SP3.PantuNumberColorA,
			dbo.T_SP3.PantuNumberColorB,
			dbo.T_SP3.PantuNumberColorC,
			dbo.T_SP3.PantuNumberColorD,
			dbo.T_SP3.PantuNumberColorSu,
			dbo.T_SP3.PantuNumberSize,
			dbo.T_SP3.PantuNumberKakouHouhou,
			dbo.T_SP3.PantuNumberKiji,
			dbo.T_SP3.WaterMarkMuneMarkingIchi,
			dbo.T_SP3.WaterMarkMuneSyotai,
			dbo.T_SP3.WaterMarkMuneDesignMoji,
			dbo.T_SP3.WaterMarkMuneColorA,
			dbo.T_SP3.WaterMarkMuneColorB,
			dbo.T_SP3.WaterMarkMuneColorC,
			dbo.T_SP3.WaterMarkMuneColorD,
			dbo.T_SP3.WaterMarkMuneColorSu,
			dbo.T_SP3.WaterMarkMuneSize,
			dbo.T_SP3.WaterMarkMuneKakouHouhou,
			dbo.T_SP3.WaterMarkMuneKiji,
			dbo.T_SP3.WaterMarkBackMarkingIchi,
			dbo.T_SP3.WaterMarkBackSyotai,
			dbo.T_SP3.WaterMarkBackDesignMoji,
			dbo.T_SP3.WaterMarkBackColorA,
			dbo.T_SP3.WaterMarkBackColorB,
			dbo.T_SP3.WaterMarkBackColorC,
			dbo.T_SP3.WaterMarkBackColorD,
			dbo.T_SP3.WaterMarkBackColorSu,
			dbo.T_SP3.WaterMarkBackSize,
			dbo.T_SP3.WaterMarkBackKakouHouhou,
			dbo.T_SP3.WaterMarkBackKiji,
			dbo.T_SP3.CaptainMarkTopSize,
			dbo.T_SP3.CaptainMarkTopNumber,
			dbo.T_SP3.CaptainMarkUnderSize,
			dbo.T_SP3.CaptainMarkUnderNumber,
			dbo.T_SP3.OriginalMarkBackHinban,
			dbo.T_SP3.OriginalMarkBackSuryou,
			dbo.T_SP3.OriginalMarkBackKingaku,
			dbo.T_SP3.OriginalMarkBackIchi,
			dbo.T_SP3.OriginalMarkBackKakouHouhou,
			dbo.T_SP3.OriginalMarkBackColorCodeA,
			dbo.T_SP3.OriginalMarkBackColorNameA,
			dbo.T_SP3.OriginalMarkBackColorCodeB,
			dbo.T_SP3.OriginalMarkBackColorNameB,
			dbo.T_SP3.OriginalMarkBackColorCodeC,
			dbo.T_SP3.OriginalMarkBackColorNameC,
			dbo.T_SP3.OriginalMarkBackColorCodeD,
			dbo.T_SP3.OriginalMarkBackColorNameD,
			dbo.T_SP3.SyatuBuiACode,
			dbo.T_SP3.SyatuBuiAName,
			dbo.T_SP3.SyatuBuiAColor1,
			dbo.T_SP3.SyatuBuiAColor2,
			dbo.T_SP3.SyatuBuiAColor3,
			dbo.T_SP3.SyatuBuiAColor4,
			dbo.T_SP3.SyatuBuiAColor5,
			dbo.T_SP3.SyatuBuiBCode,
			dbo.T_SP3.SyatuBuiBName,
			dbo.T_SP3.SyatuBuiBColor1,
			dbo.T_SP3.SyatuBuiBColor2,
			dbo.T_SP3.SyatuBuiBColor3,
			dbo.T_SP3.SyatuBuiBColor4,
			dbo.T_SP3.SyatuBuiBColor5,
			dbo.T_SP3.SyatuBuiCCode,
			dbo.T_SP3.SyatuBuiCName,
			dbo.T_SP3.SyatuBuiCColor1,
			dbo.T_SP3.SyatuBuiCColor2,
			dbo.T_SP3.SyatuBuiCColor3,
			dbo.T_SP3.SyatuBuiCColor4,
			dbo.T_SP3.SyatuBuiCColor5,
			dbo.T_SP3.SyatuBuiDCode,
			dbo.T_SP3.SyatuBuiDName,
			dbo.T_SP3.SyatuBuiDColor1,
			dbo.T_SP3.SyatuBuiDColor2,
			dbo.T_SP3.SyatuBuiDColor3,
			dbo.T_SP3.SyatuBuiDColor4,
			dbo.T_SP3.SyatuBuiDColor5,
			dbo.T_SP3.SyatuBuiECode,
			dbo.T_SP3.SyatuBuiEName,
			dbo.T_SP3.SyatuBuiEColor1,
			dbo.T_SP3.SyatuBuiEColor2,
			dbo.T_SP3.SyatuBuiEColor3,
			dbo.T_SP3.SyatuBuiEColor4,
			dbo.T_SP3.SyatuBuiEColor5,
            dbo.T_SP3.SyatuBuiFCode,
            dbo.T_SP3.SyatuBuiFName,
            dbo.T_SP3.SyatuBuiFColor1,
            dbo.T_SP3.SyatuBuiFColor2,
            dbo.T_SP3.SyatuBuiFColor3,
            dbo.T_SP3.SyatuBuiFColor4,
            dbo.T_SP3.SyatuBuiFColor5,
            dbo.T_SP3.SyatuBuiGCode,
            dbo.T_SP3.SyatuBuiGName,
            dbo.T_SP3.SyatuBuiGColor1,
            dbo.T_SP3.SyatuBuiGColor2,
            dbo.T_SP3.SyatuBuiGColor3,
            dbo.T_SP3.SyatuBuiGColor4,
            dbo.T_SP3.SyatuBuiGColor5,
            dbo.T_SP3.PantuBuiACode,
            dbo.T_SP3.PantuBuiAName,
            dbo.T_SP3.PantuBuiAColor1,
            dbo.T_SP3.PantuBuiAColor2,
            dbo.T_SP3.PantuBuiAColor3,
            dbo.T_SP3.PantuBuiAColor4,
            dbo.T_SP3.PantuBuiAColor5,
            dbo.T_SP3.PantuBuiBCode,
            dbo.T_SP3.PantuBuiBName,
            dbo.T_SP3.PantuBuiBColor1,
            dbo.T_SP3.PantuBuiBColor2,
            dbo.T_SP3.PantuBuiBColor3,
            dbo.T_SP3.PantuBuiBColor4,
            dbo.T_SP3.PantuBuiBColor5,
            dbo.T_SP3.PantuBuiCCode,
            dbo.T_SP3.PantuBuiCName,
            dbo.T_SP3.PantuBuiCColor1,
            dbo.T_SP3.PantuBuiCColor2,
            dbo.T_SP3.PantuBuiCColor3,
            dbo.T_SP3.PantuBuiCColor4,
            dbo.T_SP3.PantuBuiCColor5,
			dbo.T_SP3.SimulatorURL,
            dbo.T_SP4.SimulatorID,
            dbo.T_SP4.ZekkenMuneNumberOptionPosition,
            dbo.T_SP4.ZekkenMuneNumberOptionSyotai,
            dbo.T_SP4.ZekkenMuneNumberOptionKakouHouhou,
            dbo.T_SP4.ZekkenBackNumberOptionPosition,
            dbo.T_SP4.ZekkenBackNumberOptionSyotai,
            dbo.T_SP4.ZekkenBackNumberOptionKakouHouhou,
            dbo.T_SP4.LeftSleeveOptionPosition,
            dbo.T_SP4.LeftSleeveOptionSyotai,
            dbo.T_SP4.LeftSleeveOptionKakouHouhou,
            dbo.T_SP4.LeftSleeveOptionMarkNaiyou,
            dbo.T_SP4.ZenMuneOptionPosition,
            dbo.T_SP4.ZenMuneOptionSyotai,
            dbo.T_SP4.ZenMuneOptionKakouHouhou,
            dbo.T_SP4.ZenMuneOptionMarkNaiyou,
            dbo.T_SP4.LeftMuneOptionPosition,
            dbo.T_SP4.LeftMuneOptionSyotai,
            dbo.T_SP4.LeftMuneOptionKakouHouhou,
            dbo.T_SP4.LeftMuneOptionMarkNaiyou,
            dbo.T_SP4.BackAOptionPosition,
            dbo.T_SP4.BackAOptionSyotai,
            dbo.T_SP4.BackAOptionKakouHouhou,
            dbo.T_SP4.BackAOptionMarkNaiyou,
            dbo.T_SP4.BackBOptionPosition,
            dbo.T_SP4.BackBOptionSyotai,
            dbo.T_SP4.BackBOptionKakouHouhou,
            dbo.T_SP4.BackBOptionMarkNaiyou,
            dbo.T_SP4.BackCOptionPosition,
            dbo.T_SP4.BackCOptionSyotai,
            dbo.T_SP4.BackCOptionKakouHouhou,
            dbo.T_SP4.BackCOptionMarkNaiyou,
            dbo.T_SP4.BackDOptionPosition,
            dbo.T_SP4.BackDOptionSyotai,
            dbo.T_SP4.BackDOptionKakouHouhou,
            dbo.T_SP4.BackDOptionMarkNaiyou,
            dbo.T_SP4.BackUnderCollarOptionPosition,
            dbo.T_SP4.BackUnderCollarOptionSyotai,
            dbo.T_SP4.BackUnderCollarOptionKakouHouhou,
            dbo.T_SP4.BackUnderCollarOptionMarkNaiyou,
            dbo.T_SP4.OriginalMarkLeftPosition,
            dbo.T_SP4.OriginalMarkRightMunePosition,
            dbo.T_SP4.OriginalMarkBackAPosition,
            dbo.T_SP4.OriginalMarkBackBPosition,
            dbo.T_SP4.OriginalMarkBackCPosition,
            dbo.T_SP4.OriginalMarkBackDPosition,
            dbo.T_SP4.OriginalMarkBackUnderCollarPosition,
            dbo.T_SP4.OriginalMarkPantuRightHemPosition,
            dbo.T_SP4.SyatuBuiHCode,
            dbo.T_SP4.SyatuBuiHName,
            dbo.T_SP4.SyatuBuiHColor1,
            dbo.T_SP4.SyatuBuiHColor2,
            dbo.T_SP4.SyatuBuiHColor3,
            dbo.T_SP4.SyatuBuiHColor4,
            dbo.T_SP4.SyatuBuiHColor5,
            dbo.T_SP4.SyatuBuiICode,
            dbo.T_SP4.SyatuBuiIName,
            dbo.T_SP4.SyatuBuiIColor1,
            dbo.T_SP4.SyatuBuiIColor2,
            dbo.T_SP4.SyatuBuiIColor3,
            dbo.T_SP4.SyatuBuiIColor4,
            dbo.T_SP4.SyatuBuiIColor5,
            dbo.T_SP4.SyatuBuiJCode,
            dbo.T_SP4.SyatuBuiJName,
            dbo.T_SP4.SyatuBuiJColor1,
            dbo.T_SP4.SyatuBuiJColor2,
            dbo.T_SP4.SyatuBuiJColor3,
            dbo.T_SP4.SyatuBuiJColor4,
            dbo.T_SP4.SyatuBuiJColor5,
            dbo.T_SP4.PantuBuiDCode,
            dbo.T_SP4.PantuBuiDName,
            dbo.T_SP4.PantuBuiDColor1,
            dbo.T_SP4.PantuBuiDColor2,
            dbo.T_SP4.PantuBuiDColor3,
            dbo.T_SP4.PantuBuiDColor4,
            dbo.T_SP4.PantuBuiDColor5,
            dbo.T_SP4.PantuBuiECode,
            dbo.T_SP4.PantuBuiEName,
            dbo.T_SP4.PantuBuiEColor1,
            dbo.T_SP4.PantuBuiEColor2,
            dbo.T_SP4.PantuBuiEColor3,
            dbo.T_SP4.PantuBuiEColor4,
            dbo.T_SP4.PantuBuiEColor5,
            dbo.T_SP4.PantuBuiFCode,
            dbo.T_SP4.PantuBuiFName,
            dbo.T_SP4.PantuBuiFColor1,
            dbo.T_SP4.PantuBuiFColor2,
            dbo.T_SP4.PantuBuiFColor3,
            dbo.T_SP4.PantuBuiFColor4,
            dbo.T_SP4.PantuBuiFColor5,
            dbo.T_SP4.PantuBuiGCode,
            dbo.T_SP4.PantuBuiGName,
            dbo.T_SP4.PantuBuiGColor1,
            dbo.T_SP4.PantuBuiGColor2,
            dbo.T_SP4.PantuBuiGColor3,
            dbo.T_SP4.PantuBuiGColor4,
            dbo.T_SP4.PantuBuiGColor5,
            dbo.T_SP4.PantuBuiHCode,
            dbo.T_SP4.PantuBuiHName,
            dbo.T_SP4.PantuBuiHColor1,
            dbo.T_SP4.PantuBuiHColor2,
            dbo.T_SP4.PantuBuiHColor3,
            dbo.T_SP4.PantuBuiHColor4,
            dbo.T_SP4.PantuBuiHColor5,
            dbo.T_SP4.PantuBuiICode,
            dbo.T_SP4.PantuBuiIName,
            dbo.T_SP4.PantuBuiIColor1,
            dbo.T_SP4.PantuBuiIColor2,
            dbo.T_SP4.PantuBuiIColor3,
            dbo.T_SP4.PantuBuiIColor4,
            dbo.T_SP4.PantuBuiIColor5,
            dbo.T_SP4.PantuBuiJCode,
            dbo.T_SP4.PantuBuiJName,
            dbo.T_SP4.PantuBuiJColor1,
            dbo.T_SP4.PantuBuiJColor2,
            dbo.T_SP4.PantuBuiJColor3,
            dbo.T_SP4.PantuBuiJColor4,
            dbo.T_SP4.PantuBuiJColor5,
            dbo.T_SP4.Back1MarkingIchi,
            dbo.T_SP4.Back1Syotai,
            dbo.T_SP4.Back1DesignMoji,
            dbo.T_SP4.Back1ColorA,
            dbo.T_SP4.Back1ColorB,
            dbo.T_SP4.Back1ColorC,
            dbo.T_SP4.Back1ColorD,
            dbo.T_SP4.Back1ColorSu,
            dbo.T_SP4.Back1Size,
            dbo.T_SP4.Back1KakouHouhou,
            dbo.T_SP4.Back1Kiji,
            dbo.T_SP4.Back2MarkingIchi,
            dbo.T_SP4.Back2Syotai,
            dbo.T_SP4.Back2DesignMoji,
            dbo.T_SP4.Back2ColorA,
            dbo.T_SP4.Back2ColorB,
            dbo.T_SP4.Back2ColorC,
            dbo.T_SP4.Back2ColorD,
            dbo.T_SP4.Back2ColorSu,
            dbo.T_SP4.Back2Size,
            dbo.T_SP4.Back2KakouHouhou,
            dbo.T_SP4.Back2Kiji,
            dbo.T_SP4.Back3MarkingIchi,
            dbo.T_SP4.Back3Syotai,
            dbo.T_SP4.Back3DesignMoji,
            dbo.T_SP4.Back3ColorA,
            dbo.T_SP4.Back3ColorB,
            dbo.T_SP4.Back3ColorC,
            dbo.T_SP4.Back3ColorD,
            dbo.T_SP4.Back3ColorSu,
            dbo.T_SP4.Back3Size,
            dbo.T_SP4.Back3KakouHouhou,
            dbo.T_SP4.Back3Kiji
        FROM
            dbo.VIEW_BecchuKeyInfo
            INNER JOIN
                dbo.VIEW_SPMeisai
            ON  dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.VIEW_SPMeisai.OrderKanriNo
            AND dbo.VIEW_BecchuKeyInfo.ShiiresakiCode = dbo.VIEW_SPMeisai.ShiiresakiCode 
            LEFT OUTER JOIN
                dbo.T_Mark
            ON  dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.T_Mark.SashizuNo
            AND dbo.VIEW_BecchuKeyInfo.MizunoUketsukeBi = dbo.T_Mark.SashizuBi
            LEFT OUTER JOIN
				dbo.T_SP
			ON	dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.T_SP.OrderKanriNo
            LEFT OUTER JOIN
				dbo.T_SP2
			ON	dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.T_SP2.OrderKanriNo
			LEFT OUTER JOIN
				dbo.T_SP3
			ON	dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.T_SP3.OrderKanriNo
            LEFT OUTER JOIN
                dbo.T_SP4
            ON  dbo.VIEW_BecchuKeyInfo.OrderKanriNo = dbo.T_SP4.OrderKanriNo
        {0}
    ) AS T_A
    LEFT OUTER JOIN
        dbo.T_BecchuShukkaInfo
    ON  T_A.Size = dbo.T_BecchuShukkaInfo.Size
    AND T_A.Hinban = dbo.T_BecchuShukkaInfo.Hinban
    AND T_A.OrderKanriNo = dbo.T_BecchuShukkaInfo.SashizuNo
    AND T_A.ShiiresakiCode = dbo.T_BecchuShukkaInfo.ShiiresakiCode
    AND T_A.MizunoUketsukeBi = dbo.T_BecchuShukkaInfo.SashizuBi
WHERE
    (dbo.T_BecchuShukkaInfo.RowNo IS NULL)
OR  (dbo.T_BecchuShukkaInfo.RowNo = 0)
OR  (dbo.T_BecchuShukkaInfo.RowNo = 0)

ORDER BY OrderKanriNo, Hinban ,DLSort
            ";

            Core.Sql.WhereGenerator w = new Core.Sql.WhereGenerator();
            p.SetWhere(w, da.SelectCommand);


            string strWhere = "";
            //if (!string.IsNullOrEmpty(w.WhereText))
            //    strWhere = " where " + w.WhereText;

            if (!string.IsNullOrEmpty(w.WhereText))
                strWhere += " where " + w.WhereText + " AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KA' AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KB' AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'M2'";
            else
                strWhere += " where dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KA' AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'KB' AND dbo.VIEW_BecchuKeyInfo.ShumokuCode != 'M2'";

            //20170224 �ǉ� ����̎�ڃR�[�h�ł̓}�[�N���܂œo�^����Ă��Ȃ��Əo�͂���Ȃ��悤�ɂ���
            strWhere += @" AND (dbo.VIEW_BecchuKeyInfo.ShumokuCode NOT IN ('QS', 'QR', 'QT', 'QB', 'QV', 'QA', 'SI', 'EZ', 'SE') OR
                (dbo.VIEW_BecchuKeyInfo.ShumokuCode IN ('QS', 'QR', 'QT', 'QB', 'QV', 'QA', 'SI', 'EZ', 'SE') AND 
                dbo.T_Mark.SashizuNo IS NOT NULL))";

            //QT,QP,QA,SI,SE�͕ʃ����[�X

            da.SelectCommand.CommandTimeout = 100;

            da.SelectCommand.CommandText = string.Format(da.SelectCommand.CommandText, strWhere);

            ViewDataset.VIEW_SP_DownloadDataTable dt = new ViewDataset.VIEW_SP_DownloadDataTable();
            da.Fill(dt);

            return dt;
        }


        public static Core.Error NoukiKaitouTouroku(EnumUserType touroku_user_type,
            BecchuOrderClass.BecchuOrderKey key, DateTime dtNouki, string strUserID, SqlConnection sqlConn,
            ref bool? bJidouShounin, ref int nNewKaitouNo, ref bool? bMailSendFlg)
        {
            SqlTransaction t = null;
            try
            {

                sqlConn.Open();
                t = sqlConn.BeginTransaction();
                NoukiKaitouReg reg = new NoukiKaitouReg(t);
                NoukiKaitouTouroku(touroku_user_type, key, dtNouki, strUserID, reg, true, t, ref bJidouShounin, ref nNewKaitouNo, ref bMailSendFlg);
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
            }
        }




        public class NoukiKaitouData
        {
            public BecchuOrderClass.BecchuOrderKey BecchuOrderKey
            {
                get;
                set;
            }
            public DateTime Nouki { get; set; }
        }


        /// <summary>
        /// �񓚔[���f�[�^�o�^ 10-09-01
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="updCnt"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static Core.Error NoukiKaitou_Upload
            (EnumUserType touroku_user_type, List<NoukiKaitouData> lst, string strUserID, SqlConnection sqlConn)
        {
            SqlTransaction t = null;
            int i = 0;
            try
            {
                sqlConn.Open();
                t = sqlConn.BeginTransaction();

                NoukiKaitouReg reg = new NoukiKaitouReg(t);
                bool? bJidouShounin = false;
                int nNewKaitouNo = 0;
                bool? bMailSendFlg = false;

                for (i = 0; i < lst.Count; i++)
                {
                    NoukiKaitouTouroku(touroku_user_type, lst[i].BecchuOrderKey, lst[i].Nouki, strUserID, reg, false, t, ref bJidouShounin, ref nNewKaitouNo, ref bMailSendFlg);
                }

                t.Commit();
                return null;
            }
            catch (Exception e)
            {
                if (null != t) t.Rollback();
                return new Core.Error(string.Format("{0}�s��-{1}", i + 1, e.Message));
            }
            finally
            {
                sqlConn.Close();
            }
        }


        private class NoukiKaitouReg
        {
            SqlDataAdapter daKeyInfo = null;
            SqlDataAdapter daKeyInfo2 = null;
            SqlDataAdapter daNouki = null;
            SqlDataAdapter daNouki2 = null;

            SqlCommand cmdDelMiShounin = null;
            SqlCommand cmdDelMiShounin2 = null;

            public MizunoDataSet.T_BecchuKeyInfoDataTable dtKeyInfo = new MizunoDataSet.T_BecchuKeyInfoDataTable();
            public MizunoDataSet.T_NoukiKaitouDataTable dtNouki = new MizunoDataSet.T_NoukiKaitouDataTable();

            public NoukiKaitouReg(SqlTransaction t)
            {
                daKeyInfo = new SqlDataAdapter("", t.Connection);
                daKeyInfo.SelectCommand.CommandText = "SELECT * FROM T_BecchuKeyInfo WHERE CAST(MizunoUketsukeBi as date) = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode and isdate(MizunoUketsukeBi)=1";
                daKeyInfo.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", DateTime.Today);
                daKeyInfo.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", "");
                daKeyInfo.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", "");
                daKeyInfo.SelectCommand.Transaction = t;
                daKeyInfo.UpdateCommand = (new SqlCommandBuilder(daKeyInfo)).GetUpdateCommand();
                daKeyInfo.UpdateCommand.Transaction = t;

                daKeyInfo2 = new SqlDataAdapter("", t.Connection);
                daKeyInfo2.SelectCommand.CommandText = "SELECT * FROM T_BecchuKeyInfo where MizunoUketsukeBi= @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
                daKeyInfo2.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", "");
                daKeyInfo2.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", "");
                daKeyInfo2.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", "");
                daKeyInfo2.SelectCommand.Transaction = t;
                daKeyInfo2.UpdateCommand = (new SqlCommandBuilder(daKeyInfo2)).GetUpdateCommand();
                daKeyInfo2.UpdateCommand.Transaction = t;

                daNouki = new SqlDataAdapter("", t.Connection);
                daNouki.SelectCommand.CommandText = "SELECT * FROM T_NoukiKaitou WHERE CAST(MizunoUketsukeBi as date) = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode and isdate(MizunoUketsukeBi)=1";
                daNouki.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", DateTime.Today);
                daNouki.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", "");
                daNouki.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", "");
                daNouki.SelectCommand.Transaction = t;
                daNouki.InsertCommand = (new SqlCommandBuilder(daNouki)).GetInsertCommand();
                daNouki.UpdateCommand = (new SqlCommandBuilder(daNouki)).GetUpdateCommand();
                daNouki.DeleteCommand = (new SqlCommandBuilder(daNouki)).GetDeleteCommand();
                daNouki.InsertCommand.Transaction = daNouki.UpdateCommand.Transaction = daNouki.DeleteCommand.Transaction = t;

                daNouki2 = new SqlDataAdapter("", t.Connection);
                daNouki2.SelectCommand.CommandText = "SELECT * FROM T_NoukiKaitou WHERE MizunoUketsukeBi=@MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
                daNouki2.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", "");
                daNouki2.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", "");
                daNouki2.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", "");
                daNouki2.SelectCommand.Transaction = t;
                daNouki2.InsertCommand = (new SqlCommandBuilder(daNouki2)).GetInsertCommand();
                daNouki2.UpdateCommand = (new SqlCommandBuilder(daNouki2)).GetUpdateCommand();
                daNouki2.DeleteCommand = (new SqlCommandBuilder(daNouki2)).GetDeleteCommand();
                daNouki2.InsertCommand.Transaction = daNouki2.UpdateCommand.Transaction = daNouki2.DeleteCommand.Transaction = t;

                cmdDelMiShounin = new SqlCommand("", t.Connection);
                cmdDelMiShounin.CommandText = "delete T_NoukiKaitou WHERE CAST(MizunoUketsukeBi as date) = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode and isdate(MizunoUketsukeBi)=1 and ShouninFlg=0";
                cmdDelMiShounin.Parameters.AddWithValue("@MizunoUketsukeBi", DateTime.Today);
                cmdDelMiShounin.Parameters.AddWithValue("@OrderKanriNo", "");
                cmdDelMiShounin.Parameters.AddWithValue("@ShiiresakiCode", "");
                cmdDelMiShounin.Transaction = t;

                cmdDelMiShounin2 = new SqlCommand("", t.Connection);
                cmdDelMiShounin2.CommandText = "delete T_NoukiKaitou WHERE MizunoUketsukeBi=@MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode and ShouninFlg=0";
                cmdDelMiShounin2.Parameters.AddWithValue("@MizunoUketsukeBi", "");
                cmdDelMiShounin2.Parameters.AddWithValue("@OrderKanriNo", "");
                cmdDelMiShounin2.Parameters.AddWithValue("@ShiiresakiCode", "");
                cmdDelMiShounin2.Transaction = t;
            }

            public BecchuOrderKey BecchuOrderKey { get; set; }

            public int DeleteMiShouninMoukiKaitou()
            {
                if (BecchuOrderKey.UketsukeBi.Contains("/"))
                {
                    cmdDelMiShounin.Parameters["@MizunoUketsukeBi"].Value = DateTime.Parse(BecchuOrderKey.UketsukeBi);
                    cmdDelMiShounin.Parameters["@OrderKanriNo"].Value = BecchuOrderKey.OrderKanriNo;
                    cmdDelMiShounin.Parameters["@ShiiresakiCode"].Value = BecchuOrderKey.ShiiresakiCode;
                    return cmdDelMiShounin.ExecuteNonQuery();
                }
                else
                {
                    cmdDelMiShounin2.Parameters["@MizunoUketsukeBi"].Value = BecchuOrderKey.UketsukeBi;
                    cmdDelMiShounin2.Parameters["@OrderKanriNo"].Value = BecchuOrderKey.OrderKanriNo;
                    cmdDelMiShounin2.Parameters["@ShiiresakiCode"].Value = BecchuOrderKey.ShiiresakiCode;
                    return cmdDelMiShounin2.ExecuteNonQuery();
                }
            }

            public void LoadKeyInfo()
            {
                dtKeyInfo.Clear();

                if (BecchuOrderKey.UketsukeBi.Contains("/"))
                {
                    daKeyInfo.SelectCommand.Parameters["@MizunoUketsukeBi"].Value = DateTime.Parse(BecchuOrderKey.UketsukeBi);
                    daKeyInfo.SelectCommand.Parameters["@OrderKanriNo"].Value = BecchuOrderKey.OrderKanriNo;
                    daKeyInfo.SelectCommand.Parameters["@ShiiresakiCode"].Value = BecchuOrderKey.ShiiresakiCode;
                    daKeyInfo.Fill(dtKeyInfo);
                }
                else
                {
                    
                    string MizunoUketukeBi = int.Parse(BecchuOrderKey.UketsukeBi).ToString("0000/00/00");
                    daKeyInfo2.SelectCommand.Parameters["@MizunoUketsukeBi"].Value = MizunoUketukeBi;
                    //daKeyInfo2.SelectCommand.Parameters["@MizunoUketsukeBi"].Value = BecchuOrderKey.UketsukeBi;
                    daKeyInfo2.SelectCommand.Parameters["@OrderKanriNo"].Value = BecchuOrderKey.OrderKanriNo;
                    daKeyInfo2.SelectCommand.Parameters["@ShiiresakiCode"].Value = BecchuOrderKey.ShiiresakiCode;
                    daKeyInfo2.Fill(dtKeyInfo);
                }
            }

            public void LoadNoukiKaitou()
            {
                dtNouki.Clear();

                if (BecchuOrderKey.UketsukeBi.Contains("/"))
                {
                    daNouki.SelectCommand.Parameters["@MizunoUketsukeBi"].Value = DateTime.Parse(BecchuOrderKey.UketsukeBi);
                    daNouki.SelectCommand.Parameters["@OrderKanriNo"].Value = BecchuOrderKey.OrderKanriNo;
                    daNouki.SelectCommand.Parameters["@ShiiresakiCode"].Value = BecchuOrderKey.ShiiresakiCode;
                    daNouki.Fill(dtNouki);
                }
                else
                {
                    daNouki2.SelectCommand.Parameters["@MizunoUketsukeBi"].Value = BecchuOrderKey.UketsukeBi;
                    daNouki2.SelectCommand.Parameters["@OrderKanriNo"].Value = BecchuOrderKey.OrderKanriNo;
                    daNouki2.SelectCommand.Parameters["@ShiiresakiCode"].Value = BecchuOrderKey.ShiiresakiCode;
                    daNouki2.Fill(dtNouki);
                }
            }


            public void UpdateKey()
            {
                if (BecchuOrderKey.UketsukeBi.Contains("/"))
                    daKeyInfo.Update(dtKeyInfo);
                else
                    daKeyInfo2.Update(dtKeyInfo);
            }

            public void UpdateNouki()
            {
                if (BecchuOrderKey.UketsukeBi.Contains("/"))
                    daNouki.Update(dtNouki);
                else
                    daNouki2.Update(dtNouki);
            }
        }


        private static void NoukiKaitouTouroku(EnumUserType touroku_user_type,
            BecchuOrderClass.BecchuOrderKey key, DateTime dtNouki, string strUserID, NoukiKaitouReg reg, bool bErrorIfSameNouki, SqlTransaction t,
            ref bool? bJidouShounin, ref int nNewKaitouNo, ref bool? bMailSendFlg)
        {
            nNewKaitouNo = 0;

            if (dtNouki < DateTime.Today)
            {
                throw new Exception("�{���ȍ~�̓��t����͂��ĉ������B");
            }

            reg.BecchuOrderKey = key;
            reg.LoadKeyInfo();
            if (1 != reg.dtKeyInfo.Count)
                throw new Exception("�����f�[�^������܂���B" + key.OrderKanriNo);

            MizunoDataSet.T_BecchuKeyInfoRow drKey = reg.dtKeyInfo[0];

            if (touroku_user_type == EnumUserType.Shiiresaki)
            {
                // �d����̊m�F���o�^
                if (drKey.IsKakuninBiNull())
                    drKey.KakuninBi = DateTime.Now;
            }

            int nKaitouNouki = new Core.Type.Nengappi(dtNouki).ToYYYYMMDD();

            // �[���񓚎擾
            reg.LoadNoukiKaitou();
            reg.dtNouki.DefaultView.Sort = "KaitouNo DESC";
            if (bErrorIfSameNouki)
            {
                if (0 < reg.dtNouki.Count)
                {
                    MizunoDataSet.T_NoukiKaitouRow drLast = reg.dtNouki.DefaultView[0].Row as MizunoDataSet.T_NoukiKaitouRow;
                    if (Core.Type.Nengappi.Parse(drLast.Nouki).ToYYYYMMDD() == nKaitouNouki)
                        throw new Exception("���t�ɕύX������܂���B");
                }
            }

            // �����F�̔[���񓚂��폜����
            int nCount = reg.DeleteMiShouninMoukiKaitou();
            if (0 < nCount)
            {
                reg.LoadNoukiKaitou();  // �f�[�^���ă��[�h
            }

            // �o�׎w����
            int nShijiBi = 0;
            if ("" != drKey.KoujyouShukkaShijiBi.Trim())
            {
                try
                {
                    nShijiBi = Core.Type.Nengappi.Parse(drKey.KoujyouShukkaShijiBi).ToYYYYMMDD();
                }
                catch
                {
                    throw new Exception("�H��o�׎w�������s���Ȓl�ł��B" + drKey.KoujyouShukkaShijiBi);
                }
            }

            // �������F�t���O
            bJidouShounin = false;

            if (touroku_user_type == EnumUserType.Mizuno)
            {
                bJidouShounin = true;   // �~�Y�m����̓o�^�͎������F��
            }
            else
            {
                // �H��o�׎w�������Ȃ� or �o�׎w���� > �񓚔[��
                if (nShijiBi == 0 || nShijiBi > nKaitouNouki)
                {
                    bJidouShounin = true;
                    bMailSendFlg = false;
                }
                else if (0 == reg.dtNouki.Count)
                {
                    //--2015/08/06 �~�Y�m����̗v�]�ɂ�莩�����F����悤�ɕύX
                    bJidouShounin = true;
                    // ����
                    if (nShijiBi > 0 && nKaitouNouki <= nShijiBi)
                    {
                        // �񓚔[�����H��o�׎w�����ȓ��ł���Ύ������F
                        bMailSendFlg = false;
                    }
                    else
                    {
                        // �������F&���[�����M 2015/08/06
                        bMailSendFlg = true;
                    }
                }
                else
                {
                    // �ύX��
                    if (0 < reg.dtNouki.Count)
                    {
                        reg.dtNouki.DefaultView.Sort = "KaitouNo DESC";
                        int nHenkouMaeNouki = Core.Type.Nengappi.Parse((reg.dtNouki.DefaultView[0].Row as MizunoDataSet.T_NoukiKaitouRow).Nouki).ToYYYYMMDD();
                        //�������F�Ƃ���׃R�����g�A�E�g 2015/08/06
                        //if (nKaitouNouki <= nHenkouMaeNouki)
                        //{
                        //    // �񓚔[�����ύX�O�̏��F���ꂽ�[���ȓ��ł���Ύ������F
                        //    bJidouShounin = true;
                        //}
                        //�������F 2015/08/06
                        bJidouShounin = true;
                    }
                }
            }

            if (bJidouShounin.Value)
            {
                // �������F
                drKey.KoujyouShukkaYoteiBi = dtNouki.ToString("yyyy/MM/dd");
                // ���F�ς݂Ŗ����M�̃f�[�^�𑗐M�ς݂ɂ���
                for (int i = 0; i < reg.dtNouki.Count; i++)
                {
                    if (reg.dtNouki[i].ShouninFlg && reg.dtNouki[i].SoushinZumiFlg)
                        reg.dtNouki[i].SoushinZumiFlg = true;
                }
            }

            MizunoDataSet.T_NoukiKaitouRow dr = reg.dtNouki.NewT_NoukiKaitouRow();

            dr.MizunoUketsukeBi = reg.dtKeyInfo[0].MizunoUketsukeBi;
            dr.OrderKanriNo = reg.dtKeyInfo[0].OrderKanriNo;
            dr.ShiiresakiCode = reg.dtKeyInfo[0].ShiiresakiCode;

            nNewKaitouNo = 1;
            if (0 < reg.dtNouki.DefaultView.Count)
            {
                nNewKaitouNo = (reg.dtNouki.DefaultView[0].Row as MizunoDataSet.T_NoukiKaitouRow).KaitouNo + 1;
            }
            dr.KaitouNo = nNewKaitouNo;

            dr.SoushinZumiFlg = false;  // �����M��
            dr.ShouninFlg = bJidouShounin.Value;  // ��
            dr.TourokuBi = DateTime.Now;
            dr.Nouki = dtNouki.ToString("yyyy/MM/dd");
            dr.TourokuKaishaCode = (touroku_user_type == EnumUserType.Mizuno) ? "0" : reg.dtKeyInfo[0].ShiiresakiCode;
            dr.TourokushaID = strUserID;
            dr.ShouninshaID = (touroku_user_type == EnumUserType.Mizuno) ? strUserID : "";

            if (bJidouShounin.Value)
                dr.ShouninBi = DateTime.Now;

            reg.dtNouki.Rows.Add(dr);

            reg.UpdateKey();
            reg.UpdateNouki();

        }

        /// <summary>
        /// �E�[���񓚂����F����
        /// �E���F�����No��菬�����A�����F�̔[���񓚂��폜���� 
        /// </summary>
        /// <param name="MizunoUketsukeBi"></param>
        /// <param name="OrderKanriNo"></param>
        /// <param name="ShiiresakiCode"></param>
        /// <param name="KaitouNo"></param>
        /// <param name="sqlConn"></param>
        public static Core.Error NoukiKaitou_Shounin(BecchuOrderClass.BecchuOrderKey key, int KaitouNo, string ShouninshaID, SqlConnection sqlConn)
        {
            // �H��o�ח\����̍X�V
            SqlDataAdapter daKeyInfo = new SqlDataAdapter("", sqlConn);
            daKeyInfo.SelectCommand.CommandText =
                "SELECT * FROM T_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi "
                + "AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            daKeyInfo.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", key.UketsukeBi);
            daKeyInfo.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", key.OrderKanriNo);
            daKeyInfo.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", key.ShiiresakiCode);
            daKeyInfo.UpdateCommand = (new SqlCommandBuilder(daKeyInfo)).GetUpdateCommand();
            MizunoDataSet.T_BecchuKeyInfoDataTable dtKeyInfo = new MizunoDataSet.T_BecchuKeyInfoDataTable();
            daKeyInfo.Fill(dtKeyInfo);
            if (1 != dtKeyInfo.Rows.Count)
                return new LibError("�G���[");
            MizunoDataSet.T_BecchuKeyInfoRow drThisKeyInfo = (MizunoDataSet.T_BecchuKeyInfoRow)dtKeyInfo.Rows[0];

            // �[���񓚂����F����
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_NoukiKaitou WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND "
                + "OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode AND KaitouNo = @KaitouNo";
            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", key.UketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", key.OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", key.ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@KaitouNo", KaitouNo);
            da.UpdateCommand = (new SqlCommandBuilder(da)).GetUpdateCommand();
            MizunoDataSet.T_NoukiKaitouDataTable dt = new MizunoDataSet.T_NoukiKaitouDataTable();
            da.Fill(dt);
            if (1 != dt.Rows.Count)
                return new LibError("�G���[");
            MizunoDataSet.T_NoukiKaitouRow drThis = (MizunoDataSet.T_NoukiKaitouRow)dt.Rows[0];

            SqlTransaction sqlTran = null;
            try
            {
                sqlConn.Open();
                sqlTran = sqlConn.BeginTransaction();

                // �ݒ�
                daKeyInfo.UpdateCommand.Transaction = sqlTran;
                da.UpdateCommand.Transaction = sqlTran;

                // �L�[���̍H��o�ח\������X�V
                drThisKeyInfo.KoujyouShukkaYoteiBi = drThis.Nouki;
                daKeyInfo.Update(dtKeyInfo);

                // ���F����
                drThis.ShouninFlg = true;
                drThis.ShouninshaID = ShouninshaID;
                drThis.ShouninBi = DateTime.Now;
                da.Update(dt);

                sqlTran.Commit();
                return null;
            }
            catch (Exception e)
            {
                return new Core.Error(e);
            }
            finally
            {
                sqlConn.Close();
            }
        }

        /// <summary>
        /// �����F�̍ŐV�̔[���񓚂�1�s�擾����        
        /// </summary>
        /// <param name="MizunoUketsukeBi"></param>
        /// <param name="OrderKanriNo"></param>
        /// <param name="ShiiresakiCode"></param>
        /// <param name="KaitouNo"></param>
        /// <param name="sqlConn"></param>
        /// <returns></returns>
        public static MizunoDataSet.T_NoukiKaitouRow getT_NoukiKaitouRow
            (string MizunoUketsukeBi, string OrderKanriNo, string ShiiresakiCode, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText =
                "SELECT  TOP 1 MizunoUketsukeBi, OrderKanriNo, ShiiresakiCode, KaitouNo, Nouki, "
                + "TourokuBi, ShouninFlg, ShouninshaID, ShouninBi, SoushinZumiFlg "

                + "FROM T_NoukiKaitou WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND "
                + "OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode AND ShouninFlg = 0 "

                + "ORDER BY KaitouNo DESC ";

            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", MizunoUketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", ShiiresakiCode);
            MizunoDataSet.T_NoukiKaitouDataTable dt = new MizunoDataSet.T_NoukiKaitouDataTable();
            da.Fill(dt);
            if (1 == dt.Rows.Count)
                return (MizunoDataSet.T_NoukiKaitouRow)dt.Rows[0];
            else
                return null;
        }


        public static MizunoDataSet.T_NoukiKaitouDataTable
            getT_NoukiKaitouDataTable(BecchuOrderClass.BecchuOrderKey key, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT                  dbo.T_NoukiKaitou.*
FROM                     dbo.T_NoukiKaitou
WHERE                   (MizunoUketsukeBi = @MizunoUketsukeBi) AND (OrderKanriNo = @OrderKanriNo) AND (ShiiresakiCode = @ShiiresakiCode)";

            da.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", key.UketsukeBi);
            da.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", key.OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", key.ShiiresakiCode);
            MizunoDataSet.T_NoukiKaitouDataTable dt = new MizunoDataSet.T_NoukiKaitouDataTable();
            da.Fill(dt);
            return dt;
        }

        public static bool? IsSeihinOrder(string uketsukeBi, string orderKanriNo, string shiiresakiCode, SqlConnection sqlCon)
        {
            // ���i�I�[�_�[��������True
            // �ޗ��I�[�_�[��������False
            // �ǂ���ł��Ȃ����Null��Ԃ�
            string sql =
                @"
                SELECT 
                CASE 
                WHEN (SELECT COUNT(*) FROM T_BecchuKeyInfo 
					                  WHERE T_BecchuKeyInfo.MizunoUketsukeBi = @uketsukeBi AND 
					                  T_BecchuKeyInfo.OrderKanriNo = @orderKanriNo AND 
					                  T_BecchuKeyInfo.ShiiresakiCode = @shiiresakiCode) > 0 THEN 1
                WHEN (SELECT COUNT(*) FROM T_ProperOrder 
					                  WHERE T_ProperOrder.SashizuBi = @uketsukeBi AND 
					                  (T_ProperOrder.SeisanOrderNo = @orderKanriNo OR  T_ProperOrder.HikitoriOrderNo = @orderKanriNo) AND 
					                  T_ProperOrder.ShiiresakiCode = @shiiresakiCode) > 0 THEN 1
                WHEN (SELECT COUNT(*) FROM T_ZairyouOrder 
					                  WHERE T_ZairyouOrder.SashizuBi = @uketsukeBi AND 
					                  (T_ZairyouOrder.SeisanOrderNo = @orderKanriNo OR  T_ZairyouOrder.HikitoriOrderNo = @orderKanriNo) AND 
					                  T_ZairyouOrder.ShiiresakiCode = @shiiresakiCode) > 0 THEN 0
                ELSE NULL END AS IsSeihin
                ";

            SqlCommand cmd = new SqlCommand(sql, sqlCon);
            cmd.Parameters.AddWithValue("@uketsukeBi", uketsukeBi);
            cmd.Parameters.AddWithValue("@orderKanriNo", orderKanriNo);
            cmd.Parameters.AddWithValue("@shiiresakiCode", shiiresakiCode);

            try
            {
                sqlCon.Open();

                object objIsSeihinOrder = cmd.ExecuteScalar();
                if (objIsSeihinOrder == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToBoolean(objIsSeihinOrder);
            }
            finally
            {
                if (sqlCon != null) { sqlCon.Close(); }
            }
        }





        public static Core.Error KakakuShuusei(bool SeiZai, 
                                                bool ks, 
                                                bool bUpload, 
                                                BecchuOrderKey b_bey, 
                                                TourokuData data, 
                                                SqlConnection c, 
                                                out List<NouhinDataClass.DenpyouKey> lstKey, 
                                                out List<NouhinDataClass.DenpyouKey> lstKeyShukkaAri
                                                )
        {
            lstKey = null;
            lstKeyShukkaAri = null;
            SqlTransaction t = null;
            try
            {
                c.Open();
                t = c.BeginTransaction();

                //**** DB����
                KakakuShuusei2(SeiZai, ks, bUpload, false, b_bey, data, t, out lstKey, out lstKeyShukkaAri);
                
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


        internal static void KakakuShuusei2(bool SeiZai, 
                                            bool ks, 
                                            bool bUpload, 
                                            bool bShusei,
                                            BecchuOrderKey b_bey, 
                                            TourokuData data, 
                                            SqlTransaction t, 
                                            out List<NouhinDataClass.DenpyouKey> lstKey, 
                                            out List<NouhinDataClass.DenpyouKey> lstKeyShukkaAri
                                            )
        {
            lstKeyShukkaAri = new List<NouhinDataClass.DenpyouKey>();
            lstKey = new List<NouhinDataClass.DenpyouKey>();

            int nPageSize = NouhinDataClass.GetDenpyouGyouCount(data.SouhinKubun); // ���i�敪�ɂ���ēo�^�ł���s�����قȂ�B

            // �o�א� = 0 && �����t���O = 9(����)�̃f�[�^�͕ʂ̓Ɨ������`�[�œo�^����B��ʏ�͓`�[���s�����Ńf�[�^�̂ݓo�^����(���M������))
            List<MeisaiData> lstShukka = new List<MeisaiData>();
            List<MeisaiData> lstZeroKanryou = new List<MeisaiData>();
            for (int i = 0; i < data.lstMeisai.Count; i++)
            {
                if (0 == data.lstMeisai[i].ShukkaSu && data.lstMeisai[i].KanryouFlag)
                    lstZeroKanryou.Add(data.lstMeisai[i]);
                else
                    lstShukka.Add(data.lstMeisai[i]);
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


            // �y�[�W�P�ʂō쐬
            List<MeisaiData> lstOrg = data.lstMeisai;
            for (int i = 0; i < lstDenpyou.Count; i++)
            {
                data.lstMeisai = lstDenpyou[i];

                //**** DB����
                NouhinDataClass.DenpyouKey key = KakakuShuusei3(SeiZai, ks, bUpload, bShusei, b_bey, data, t);
                
                lstKey.Add(key);
                if (i < nShukkaAriDenpyouCount)
                    lstKeyShukkaAri.Add(key);
            }
            data.lstMeisai = lstOrg;
        }



        private static NouhinDataClass.DenpyouKey KakakuShuusei3(bool SeiZai, 
                                                                    bool ks, 
                                                                    bool bUpload, 
                                                                    bool bShusei, 
                                                                    BecchuOrderKey key, 
                                                                    TourokuData data, 
                                                                    SqlTransaction t
                                                                    )
        {
            //string SashizuBi = key.SashizuBi;
            //string HikitoriOrderNo = key.HikitoriOrderNo;
            MeisaiData md = data.lstMeisai[0];
            string Hinban = md.Key.Hinban;
            bool KakakuSyuuseiFlg = false;
            //bool Huku = false;

            SqlDataAdapter da = new SqlDataAdapter("", t.Connection);
            da.SelectCommand.CommandText = @"
select RowNo,Hinban 
from T_Becchu2Repeat 
where 
    SashizuNo=@sn and 
    ShiiresakiCode=@sc and 
    SashizuBi=@sb 
    --and 
    --Hinban=@hb
";
            da.SelectCommand.Parameters.AddWithValue("@sn", key.OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@sc", key.ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@sb", key.UketsukeBi);
            //da.SelectCommand.Parameters.AddWithValue("@hb", Hinban);
            da.SelectCommand.Transaction = t;

            DataTable dt = new DataTable();
            da.Fill(dt);

            int[] RowNo = new int[dt.Rows.Count];
            string[] LotNo = new string[dt.Rows.Count];
            int[] Kakaku = new int[dt.Rows.Count];
            int[] Suuryou = new int[dt.Rows.Count];
            string[] Size = new string[dt.Rows.Count];
            int[] ShukkaSuu = new int[dt.Rows.Count];

            string[] Hinban2 = new string[dt.Rows.Count];

            for (int count = 0; count < dt.Rows.Count; count++)
            {
                RowNo[count] = (int)dt.Rows[count][0];
                Hinban2[count] = dt.Rows[count][1].ToString();

                string k = key.UketsukeBi;
                SqlDataAdapter lotda = new SqlDataAdapter("", t.Connection);
                lotda.SelectCommand.CommandText = @"
select LotNo,Kakaku,Suuryou,Size 
from T_Becchu2Repeat 
where 
    SashizuNo=@sn and 
    ShiiresakiCode=@sc and 
    SashizuBi=@sb and 
    Hinban=@hb and 
    RowNo=@rn
";
                lotda.SelectCommand.Parameters.AddWithValue("@sn", key.OrderKanriNo);
                lotda.SelectCommand.Parameters.AddWithValue("@sc", key.ShiiresakiCode);
                lotda.SelectCommand.Parameters.AddWithValue("@sb", key.UketsukeBi);
                lotda.SelectCommand.Parameters.AddWithValue("@hb", Hinban2[count]);
                lotda.SelectCommand.Parameters.AddWithValue("@rn", RowNo[count]);
                lotda.SelectCommand.Transaction = t;
                DataTable lotdt = new DataTable();
                lotda.Fill(lotdt);

                SqlDataAdapter shuda = new SqlDataAdapter("", t.Connection);
                shuda.SelectCommand.CommandText = " select ShukkaSuu from T_BecchuShukkaInfo where SashizuBi=@sb and SashizuNo=@sn and ShiiresakiCode=@sc and Hinban=@hb and RowNo=@rn ";
                shuda.SelectCommand.Parameters.AddWithValue("@sb", key.UketsukeBi);
                shuda.SelectCommand.Parameters.AddWithValue("@sn", key.OrderKanriNo);
                shuda.SelectCommand.Parameters.AddWithValue("@sc", key.ShiiresakiCode);
                shuda.SelectCommand.Parameters.AddWithValue("@hb", Hinban);
                shuda.SelectCommand.Parameters.AddWithValue("@rn", RowNo[count]);
                shuda.SelectCommand.Transaction = t;
                DataTable shudt = new DataTable();
                shuda.Fill(shudt);


                try
                {
                    LotNo[count] = lotdt.Rows[0][0].ToString();
                }
                catch
                {
                    LotNo[count] = "";
                }

                try
                {
                    Kakaku[count] = int.Parse(lotdt.Rows[0][1].ToString());
                }
                catch
                {
                    Kakaku[count] = 0;
                }

                try
                {
                    Suuryou[count] = (int)lotdt.Rows[0][2];
                }
                catch
                {
                    Suuryou[count] = 0;
                }

                try
                {
                    Size[count] = lotdt.Rows[0][3].ToString();
                }
                catch
                {
                    Size[count] = "";
                }

                try
                {
                    ShukkaSuu[count] = (int)shudt.Rows[0][0];
                }
                catch
                {
                    ShukkaSuu[count] = 0;
                }



            }






            SqlDataAdapter daKey = new SqlDataAdapter("", t.Connection);
            daKey.SelectCommand.CommandText = "SELECT * FROM VIEW_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            daKey.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", key.UketsukeBi);
            daKey.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", key.OrderKanriNo);
            daKey.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", key.ShiiresakiCode);
            daKey.SelectCommand.Transaction = t;
            ViewDataset.VIEW_BecchuKeyInfoDataTable dtKey = new ViewDataset.VIEW_BecchuKeyInfoDataTable();
            daKey.Fill(dtKey);
            if (0 == dtKey.Count) throw new Exception("�L�[���̎擾�Ɏ��s���܂����B");
            ViewDataset.VIEW_BecchuKeyInfoRow drKey = dtKey[0];

            //if (drKey.KanryouFlg) throw new Exception("���̒����͊������Ă��܂��B");  2011/8/6 ���c=0�Ŋ����������i�ԓ���ǉ��o�ׂł���悤�ɂ���׃R�����g�A�E�g

            //if (!drKey.IsCancelBiNull())
            //    throw new Exception(string.Format("�I�[�_�[No{0}�̓L�����Z���ς݂ł�", key.OrderKanriNo));


            EnumBecchuKubun kubun = EnumBecchuKubun.Becchu;

            if (!drKey.IsT_Becchu_SashizuNoNull())
                kubun = EnumBecchuKubun.Becchu;
            else if (!drKey.IsT_Becchu2_SashizuNoNull())
                kubun = EnumBecchuKubun.Becchu2;
            else if (!drKey.IsT_DS_OrderKanriNoNull())
                kubun = EnumBecchuKubun.DS;
            else if (!drKey.IsT_SP_OrderKanriNoNull())
                kubun = EnumBecchuKubun.SP;
            else
                throw new Exception("�ʒ��i�̃f�[�^��������܂���ł����B");

            // 2013.02.27 �`�[���s�N���͓������t�ł悢�̂ł́H
            /*
            SqlCommand cmdGetyymm = new SqlCommand("", t.Connection);
            cmdGetyymm.Transaction = t;
            cmdGetyymm.CommandText = @"SELECT YYMM FROM T_NouhinHeader WHERE (NouhinmotoShiiresakiCode = @s) AND (SeisanOrderNo=@o or HikitoriOrderNo=@o or ZairyouOrderNo=@o or OrderKanriNo=@o) AND (KAflg='KA' or KAflg='KB')";
            cmdGetyymm.Parameters.AddWithValue("@o", key.OrderKanriNo);
            cmdGetyymm.Parameters.AddWithValue("@s", drKey.ShiiresakiCode);
            object objy = cmdGetyymm.ExecuteScalar();
            string strYYMM = "";
            string dmyYYMM = "";
            try
            {
                strYYMM = objy.ToString();
            }
            catch { }
             */
            string strYYMM = DateTime.Now.ToString("yyMM");


            // 2013.02.27 
            // �ʒ��f�[�^���牿�i�C���f�[�^������Ă��鎖�ɂȂ����̂ŁASeisanOrderNo�AHikitoriOrderNo�����ɍő�̔��sNo���擾���鎖�͂Ȃ��͂��B
            /*
            // �ő�̔��sNo�擾
            SqlCommand cmdGetMaxHakkouNo = new SqlCommand("", t.Connection);
            cmdGetMaxHakkouNo.Transaction = t;
            cmdGetMaxHakkouNo.CommandText = @"SELECT MAX(HakkouNo) AS HacchuNo FROM T_NouhinHeader WHERE (NouhinmotoShiiresakiCode = @s) AND YYMM=@YYMM AND (SeisanOrderNo=@o or HikitoriOrderNo=@o or ZairyouOrderNo=@o or OrderKanriNo=@o)";
            //cmdGetMaxHakkouNo.CommandText = @"SELECT MAX(HakkouNo) AS HacchuNo FROM T_NouhinHeader WHERE (NouhinmotoShiiresakiCode = @s) AND YYMM=@YYMM ";
            cmdGetMaxHakkouNo.Parameters.AddWithValue("@s", drKey.ShiiresakiCode);
            cmdGetMaxHakkouNo.Parameters.AddWithValue("@o",key.OrderKanriNo);
            cmdGetMaxHakkouNo.Parameters.AddWithValue("@YYMM", Convert.ToInt32(strYYMM));
             */

            SqlCommand cmdGetMaxHakkouNo = new SqlCommand("", t.Connection);
            cmdGetMaxHakkouNo.Transaction = t;
            cmdGetMaxHakkouNo.CommandText = @"SELECT MAX(HakkouNo) AS HacchuNo FROM T_NouhinHeader WHERE (NouhinmotoShiiresakiCode = @s) AND YYMM=@YYMM ";
            cmdGetMaxHakkouNo.Parameters.AddWithValue("@s", drKey.ShiiresakiCode);
            cmdGetMaxHakkouNo.Parameters.AddWithValue("@YYMM", Convert.ToInt32(strYYMM));

            
            object objHakkouNo = cmdGetMaxHakkouNo.ExecuteScalar();


            if (null == objHakkouNo || System.DBNull.Value == objHakkouNo)
            {
                objHakkouNo = cmdGetMaxHakkouNo.ExecuteScalar();
            }

            int nHakkouNo;

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
                cmdGetMaxHakkouNo2.Parameters.AddWithValue("@s", drKey.ShiiresakiCode);
                cmdGetMaxHakkouNo2.Parameters.AddWithValue("@YYMM", Convert.ToInt32(strYYMM));

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
                // ?? Huku = true;
                nHakkouNo = Convert.ToInt32(objHakkouNo);
                nHakkouNo++;
            }

            // 2013.02.27 �ȉ��̏����̈Ӗ����s��
            /*
            SqlCommand cmdGetSOrderNo = new SqlCommand("", t.Connection);
            cmdGetSOrderNo.Transaction = t;
            cmdGetSOrderNo.CommandText = @"select SeisanOrderNo FROM T_NouhinHeader  where (NouhinmotoShiiresakiCode = @s) AND (HakkouNo=@hn) AND (SeisanOrderNo=@o or HikitoriOrderNo=@o or ZairyouOrderNo=@o or OrderKanriNo=@o) AND YYMM=@YYMM";
            cmdGetSOrderNo.Parameters.AddWithValue("@s", drKey.ShiiresakiCode);
            cmdGetSOrderNo.Parameters.AddWithValue("@o", key.OrderKanriNo);
            cmdGetSOrderNo.Parameters.AddWithValue("@YYMM", strYYMM);
            cmdGetSOrderNo.Parameters.AddWithValue("@hn", nHakkouNo);

            SqlCommand cmdGetHOrderNo = new SqlCommand("", t.Connection);
            cmdGetHOrderNo.Transaction = t;
            cmdGetHOrderNo.CommandText = @"select HikitoriOrderNo FROM T_NouhinHeader  where (NouhinmotoShiiresakiCode = @s) AND (HakkouNo=@hn) AND (SeisanOrderNo=@o or HikitoriOrderNo=@o or ZairyouOrderNo=@o or OrderKanriNo=@o) AND YYMM=@YYMM";
            cmdGetHOrderNo.Parameters.AddWithValue("@s", drKey.ShiiresakiCode);
            cmdGetHOrderNo.Parameters.AddWithValue("@o", key.OrderKanriNo);
            cmdGetHOrderNo.Parameters.AddWithValue("@YYMM", strYYMM);
            cmdGetHOrderNo.Parameters.AddWithValue("@hn", nHakkouNo);

            SqlCommand cmdZGetHOrderNo = new SqlCommand("", t.Connection);
            cmdZGetHOrderNo.Transaction = t;
            cmdZGetHOrderNo.CommandText = @"select ZairyouOrderNo FROM T_NouhinHeader  where (NouhinmotoShiiresakiCode = @s) AND (HakkouNo=@hn) AND (SeisanOrderNo=@o or HikitoriOrderNo=@o or ZairyouOrderNo=@o or OrderKanriNo=@o) AND YYMM=@YYMM";
            cmdZGetHOrderNo.Parameters.AddWithValue("@s", drKey.ShiiresakiCode);
            cmdZGetHOrderNo.Parameters.AddWithValue("@o", key.OrderKanriNo);
            cmdZGetHOrderNo.Parameters.AddWithValue("@YYMM", strYYMM);
            cmdZGetHOrderNo.Parameters.AddWithValue("@hn", nHakkouNo);

            SqlCommand cmdGetOrderNo = new SqlCommand("", t.Connection);
            cmdGetOrderNo.Transaction = t;
            cmdGetOrderNo.CommandText = @"select OrderKanriNo FROM T_NouhinHeader  where (NouhinmotoShiiresakiCode = @s) AND (HakkouNo=@hn) AND (SeisanOrderNo=@o or HikitoriOrderNo=@o or ZairyouOrderNo=@o or OrderKanriNo=@o) AND YYMM=@YYMM";
            cmdGetOrderNo.Parameters.AddWithValue("@s", drKey.ShiiresakiCode);
            cmdGetOrderNo.Parameters.AddWithValue("@o", key.OrderKanriNo);
            cmdGetOrderNo.Parameters.AddWithValue("@YYMM", strYYMM);
            cmdGetOrderNo.Parameters.AddWithValue("@hn", nHakkouNo);

            SqlCommand cmdGetShukkaHiduke = new SqlCommand("", t.Connection);
            cmdGetShukkaHiduke.Transaction = t;
            cmdGetShukkaHiduke.CommandText = @"select ShukkaHiduke FROM T_NouhinHeader  where (NouhinmotoShiiresakiCode = @s) AND (HakkouNo=@hn) AND (SeisanOrderNo=@o or HikitoriOrderNo=@o or ZairyouOrderNo=@o or OrderKanriNo=@o) AND YYMM=@YYMM";
            cmdGetShukkaHiduke.Parameters.AddWithValue("@s", drKey.ShiiresakiCode);
            cmdGetShukkaHiduke.Parameters.AddWithValue("@o", key.OrderKanriNo);
            cmdGetShukkaHiduke.Parameters.AddWithValue("@YYMM", strYYMM);
            cmdGetShukkaHiduke.Parameters.AddWithValue("@hn", nHakkouNo);

            SqlCommand cmdGetShukkaBi = new SqlCommand("", t.Connection);
            cmdGetShukkaBi.Transaction = t;
            cmdGetShukkaBi.CommandText = @"select ShukkaBi FROM T_NouhinHeader  where (NouhinmotoShiiresakiCode = @s) AND (HakkouNo=@hn) AND (SeisanOrderNo=@o or HikitoriOrderNo=@o or ZairyouOrderNo=@o or OrderKanriNo=@o) AND YYMM=@YYMM";
            cmdGetShukkaBi.Parameters.AddWithValue("@s", drKey.ShiiresakiCode);
            cmdGetShukkaBi.Parameters.AddWithValue("@o", key.OrderKanriNo);
            cmdGetShukkaBi.Parameters.AddWithValue("@YYMM", strYYMM);
            cmdGetShukkaBi.Parameters.AddWithValue("@hn", nHakkouNo);

            SqlCommand cmdGetYYMM = new SqlCommand("", t.Connection);
            cmdGetYYMM.Transaction = t;
            cmdGetYYMM.CommandText = @"select HakkouHiduke FROM V_KakakuShuusei3  where (ShiiresakiCode = @s) AND OrderKanriNo=@o";
            cmdGetYYMM.Parameters.AddWithValue("@s", drKey.ShiiresakiCode);
            cmdGetYYMM.Parameters.AddWithValue("@o", key.OrderKanriNo);
            //cmdGetOrderNo.Parameters.AddWithValue("@YYMM", int.Parse(data.dtHakkouBi.ToString("yyMM")));
            //cmdGetYYMM.Parameters.AddWithValue("@hn", nHakkouNo);

            SqlCommand cmdGetYYMM2 = new SqlCommand("", t.Connection);
            cmdGetYYMM2.Transaction = t;
            cmdGetYYMM2.CommandText = @"select SashizuBi FROM V_KakakuShuusei3  where (ShiiresakiCode = @s) AND OrderKanriNo=@o";
            cmdGetYYMM2.Parameters.AddWithValue("@s", drKey.ShiiresakiCode);
            cmdGetYYMM2.Parameters.AddWithValue("@o", key.OrderKanriNo);
            */

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


            // �o�׏��
            SqlDataAdapter daShukkaInfo = new SqlDataAdapter("", t.Connection);
            daShukkaInfo.SelectCommand.CommandText = "select * from T_BecchuShukkaInfo where SashizuBi=@z and SashizuNo=@n and ShiiresakiCode=@s";
            daShukkaInfo.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
            daShukkaInfo.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
            daShukkaInfo.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            daShukkaInfo.SelectCommand.Transaction = t;
            daShukkaInfo.InsertCommand = new SqlCommandBuilder(daShukkaInfo).GetInsertCommand();
            daShukkaInfo.UpdateCommand = new SqlCommandBuilder(daShukkaInfo).GetUpdateCommand();
            MizunoDataSet.T_BecchuShukkaInfoDataTable dtShukkaInfo = new MizunoDataSet.T_BecchuShukkaInfoDataTable();
            daShukkaInfo.Fill(dtShukkaInfo);

            string ShukkaSakiYubinBangou = "";
            string ShukkaSakiJyusho = "";
            string ShukkaSakiTel = "";

            if (kubun == EnumBecchuKubun.Becchu)
            {
                // BS�i�o�א�Z���ATEL�擾�j
                SqlDataAdapter daBs = new SqlDataAdapter("", t.Connection);
                daBs.SelectCommand.CommandText = "select * from T_Becchu where SashizuBi=@z and SashizuNo=@n and ShiiresakiCode=@s";
                daBs.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
                daBs.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
                daBs.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
                daBs.SelectCommand.Transaction = t;

                MizunoDataSet.T_BecchuDataTable dtBs = new MizunoDataSet.T_BecchuDataTable();

                daBs.Fill(dtBs);

                if (dtBs.Count > 0)
                {
                    ShukkaSakiYubinBangou = dtBs[0].IsShukkaSakiYubinBangouNull() ? "" : dtBs[0].ShukkaSakiYubinBangou;
                    ShukkaSakiJyusho = dtBs[0].IsShukkaSakiJyushoNull() ? "" : dtBs[0].ShukkaSakiJyusho;
                    ShukkaSakiTel = dtBs[0].IsShukkaSakiTelNull() ? "" : dtBs[0].ShukkaSakiTel;
                }
            }
            else if (kubun == EnumBecchuKubun.Becchu2)
            {
                // BS2�i�o�א�Z���ATEL�擾�j
                SqlDataAdapter daBs2 = new SqlDataAdapter("", t.Connection);
                daBs2.SelectCommand.CommandText = "select * from T_Becchu2 where SashizuBi=@z and SashizuNo=@n and ShiiresakiCode=@s";
                daBs2.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
                daBs2.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
                daBs2.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
                daBs2.SelectCommand.Transaction = t;

                MizunoDataSet.T_Becchu2DataTable dtBs2 = new MizunoDataSet.T_Becchu2DataTable();

                daBs2.Fill(dtBs2);

                if (dtBs2.Count > 0)
                {
                    ShukkaSakiYubinBangou = dtBs2[0].IsShukkaSakiYubinBangouNull() ? "" : dtBs2[0].ShukkaSakiYubinBangou;
                    ShukkaSakiJyusho = dtBs2[0].ShukkaSakiJyusho;
                    ShukkaSakiTel = dtBs2[0].ShukkaSakiTel;
                }
            }

            // �����M�̔[�i�f�[�^�擾(�ǉ��i�Ԃ͏���)
            // ���ꕔ���[�Ŋ������́A�ŏI���M�Ώۂ̔[�i�f�[�^�Ɋ����t���O�������Ă��邩�`�F�b�N����B
            // T_NouhinMeisai.RowNo = -3 �͒ǉ��i�Ԃ̈Ӗ�
            // �ʒ��i�Œǉ��i�Ԃ�T_NouhinMeisai.HinbanTsuikaFlg = "1"���Z�b�g����郋�[���ł��邪�A���d�l�ł�"0"���Z�b�g����Ă���A
            // T_NouhinMeisai.RowNo = -3�ŋ�ʂ��Ă����B�����i�Ԃƒǉ��i�Ԃ�����œo�^�ł���P�[�X������A�ǂꂪ�����̕i�Ԃ��ǉ��̕i�Ԃ����ʂł����A
            // ���c���ɍ��قŏo�����Ƃ̑Ή��������悤�Bby ykuri 2011/7/5
            SqlDataAdapter daGetMisoushin = new SqlDataAdapter("", t.Connection);
//            daGetMisoushin.SelectCommand.CommandText = @"
//SELECT                  dbo.T_NouhinMeisai.*, dbo.T_NouhinHeader.ShukkaBi
//FROM                     dbo.T_NouhinHeader INNER JOIN
//dbo.T_NouhinMeisai ON dbo.T_NouhinHeader.HakkouNo = dbo.T_NouhinMeisai.HakkouNo AND 
//dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = dbo.T_NouhinMeisai.ShiiresakiCode AND 
//dbo.T_NouhinHeader.YYMM = dbo.T_NouhinMeisai.YYMM
//WHERE (dbo.T_NouhinHeader.SoushinFlg = 0) AND (dbo.T_NouhinMeisai.HinbanTsuikaFlg = N'0' AND T_NouhinMeisai.RowNo>=0) AND  
//(dbo.T_NouhinHeader.SashizuBi = @z) AND  (dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = @s) and 
//(dbo.T_NouhinHeader.OrderKanriNo = @n) AND 
//(dbo.T_NouhinMeisai.Hinban = @Hinban) AND (dbo.T_NouhinMeisai.Size = @Size) AND (dbo.T_NouhinMeisai.LotNo = @LotNo)";
            daGetMisoushin.SelectCommand.CommandText = @"
SELECT  dbo.T_NouhinMeisai.*, dbo.T_NouhinHeader.ShukkaBi
FROM    dbo.T_NouhinHeader INNER JOIN
            dbo.T_NouhinMeisai ON dbo.T_NouhinHeader.HakkouNo = dbo.T_NouhinMeisai.HakkouNo AND 
            dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = dbo.T_NouhinMeisai.ShiiresakiCode AND 
            dbo.T_NouhinHeader.YYMM = dbo.T_NouhinMeisai.YYMM
WHERE   (dbo.T_NouhinHeader.SoushinFlg = 0) AND (dbo.T_NouhinMeisai.HinbanTsuikaFlg = N'0' AND T_NouhinMeisai.RowNo>=0) AND  
        (dbo.T_NouhinHeader.SashizuBi = @z) AND  (dbo.T_NouhinHeader.NouhinmotoShiiresakiCode = @s) and 
        (dbo.T_NouhinHeader.OrderKanriNo = @n) 
        --AND (dbo.T_NouhinMeisai.Hinban = @Hinban) AND (dbo.T_NouhinMeisai.Size = @Size) AND (dbo.T_NouhinMeisai.LotNo = @LotNo)";

            daGetMisoushin.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            daGetMisoushin.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
            daGetMisoushin.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
            //daGetMisoushin.SelectCommand.Parameters.AddWithValue("@Hinban", "");
            //daGetMisoushin.SelectCommand.Parameters.AddWithValue("@Size", "");
            //daGetMisoushin.SelectCommand.Parameters.AddWithValue("@LotNo", "");
            daGetMisoushin.SelectCommand.Transaction = t;


            string YM = "";
            string YM2 = "";
            string shukka = "";
            string shukkaBi = "";

            /* 2013.02.27
            object objSeisanOrderNo = cmdGetSOrderNo.ExecuteScalar();
            object objHikitoriOrderNo = cmdGetHOrderNo.ExecuteScalar();
            object objZairyouOrderNo = cmdZGetHOrderNo.ExecuteScalar();
            object objOrderKanriNo = cmdGetOrderNo.ExecuteScalar();
            object objYYMM = cmdGetYYMM.ExecuteScalar();
            object objYYMM2 = cmdGetYYMM2.ExecuteScalar();
            object objShukkaHiduke = cmdGetShukkaHiduke.ExecuteScalar();
            object objShukkaBi = cmdGetShukkaBi.ExecuteScalar();
            try
            {
                YM = objYYMM.ToString();
                YM2 = objYYMM2.ToString();
                shukka = objShukkaHiduke.ToString();
                shukkaBi = objShukkaBi.ToString();
                YM = YM.Remove(4);
                shukka = shukka.Remove(4);
            }
            catch
            {
            }


            string a = "";
            try
            {
                a = objSeisanOrderNo.ToString();
            }
            catch
            {
                objSeisanOrderNo = "";
            }
            try
            {
                a = objHikitoriOrderNo.ToString();
            }
            catch
            {
                objHikitoriOrderNo = "";
            }
            try
            {
                a = objZairyouOrderNo.ToString();
            }
            catch
            {
                objZairyouOrderNo = "";
            }
            try
            {
                a = objOrderKanriNo.ToString();
            }
            catch
            {
                objOrderKanriNo = "";
            }
            */


            /* 2013.02.27 �����̏������Ӗ����s��
            if (key.OrderKanriNo == objSeisanOrderNo.ToString())
            {
                KakakuSyuuseiFlg = true;
            }
            else if (key.OrderKanriNo == objHikitoriOrderNo.ToString())
            {
                KakakuSyuuseiFlg = true;
            }
            else if (key.OrderKanriNo == objZairyouOrderNo.ToString())
            {
                KakakuSyuuseiFlg = true;
            }
            else if (key.OrderKanriNo == objOrderKanriNo.ToString())
            {
                KakakuSyuuseiFlg = true;
            }




            if (!KakakuSyuuseiFlg)
            {
                Huku = false;

            }
            else
            {

                data.dtHakkouBi = DateTime.Parse(shukkaBi);

            }
            */




            // ----- �w�b�_�[ ----
            MizunoDataSet.T_NouhinHeaderDataTable dtHeader = new MizunoDataSet.T_NouhinHeaderDataTable();
            MizunoDataSet.T_NouhinHeaderRow drHeader = dtHeader.NewT_NouhinHeaderRow();
            NouhinDataClass.InitT_NouhinHeaderRow(drHeader);

            // ���sNo
            drHeader.HakkouNo = nHakkouNo;
            // �`�[�敪
            drHeader.DenpyouKubun = DenpyouKubun._26;
            // �d����R�[�h
            drHeader.NouhinmotoShiiresakiCode = key.ShiiresakiCode;
            // �o�ד��͉�ʂőI�����ꂽ�[�i�L��
            drHeader.NouhinKigou = data.NouhinKigou;
            //drHeader.NouhinKigou = drKey.NouhinKigou;
            // ���Ə��R�[�h                
            //drHeader.NiukeBumon = data.NiukeJigyoushoCode;
            drHeader.NiukeBumon = data.NiukeJigyoushoCode;
            drHeader.NiukeBumonMei = data.NiukeJigyoushoMei;
            // �ۊǏꏊ
            drHeader.NiukeBasho = data.HokanBasho;
            //drHeader.NiukeBasho = "nb";
            drHeader.NiukeBashoMei = data.NiukeBashoMei;

            // �\�Z��
            drHeader.YosanTsuki = "";
            // �I�[�_���̔[�i�L��
            drHeader.OrderNouhinKigou = drKey.NouhinKigou;

            // �����
            drHeader.HanbaitenKigyouRyakuMei = data.HanbaitenKigyouRyakuMei;

            drHeader.SetShukkaNiukeBumonNull();
            drHeader.KAflg = drKey.ShumokuCode;
            //drHeader.KAflg = "KA";

            // DS/SP�̎��������ɂ͎w�}No��ݒ肷��B(yyMMdd)
            // ���s���t
            drHeader.HakkouHiduke = "";
            if (!drKey.IsT_SP_OrderKanriNoNull() || !drKey.IsT_DS_OrderKanriNoNull())
            {
                if (key.OrderKanriNo.Length > 5)
                {
                    // 1�`6���܂�
                    drHeader.HakkouHiduke = key.OrderKanriNo.Substring(0, 6);
                }
            }
            else
            {
                // ������������
                string str = key.UketsukeBi.Replace("/", "");
                string HakkouDay = DateTime.Now.ToString("yyyyMMdd");
                //if (str.Length == 8)
                //{
                    //drHeader.HakkouHiduke = str.Substring(2, 6);
                drHeader.HakkouHiduke = HakkouDay.Substring(2, 6);
                //}
            }

            // �o�ד��t                
            //drHeader.ShukkaHiduke = data.dtHakkouBi.ToString("yyMMdd");
            if (shukka.Replace(" ", "") != "")
            {
                drHeader.ShukkaHiduke = shukka;
            }
            else
            {
                drHeader.ShukkaHiduke = (DateTime.Now).ToString("yyMMdd");
            }
            // �o�ד�
            drHeader.ShukkaBi = data.dtHakkouBi;

            /* 2013.02.27 Huku���ĂȂ�
            // YYMM
            if (!Huku)
            {
                drHeader.YYMM = int.Parse(data.dtHakkouBi.ToString("yyMM"));
            }
            else
            {
                drHeader.YYMM = int.Parse(YM);
            }
            */
            drHeader.YYMM = Convert.ToInt32(strYYMM);

            // ���i�敪
            drHeader.SouhinKubun = ((int)data.SouhinKubun).ToString();


            // �w�}��
            drHeader.SashizuBi = key.UketsukeBi;
            // ���Y�I�[�_�[No
            drHeader.SeisanOrderNo = "";
            // ����I�[�_�[No
            drHeader.HikitoriOrderNo = "";

            drHeader.OrderKanriNo = key.OrderKanriNo;
            // ���M�t���O
            drHeader.SoushinFlg = SoushinFlg.NONE;
            // �[�i��
            drHeader.HonNouhinTsuki = "";
            drHeader.ZairyouOrderNo = "";

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

            //2013/12/18 �����ǉ�
            drHeader.KinkyuChokusouBikou = data.KinkyuChokusouBikou;

            drHeader.TourokuBi = DateTime.Now;
            dtHeader.Rows.Add(drHeader);


            // ----- ���� -----
            SqlDataAdapter daBecchu = new SqlDataAdapter("", t.Connection);
            SqlDataAdapter daBecchu2 = new SqlDataAdapter("", t.Connection);
            SqlDataAdapter daDS = new SqlDataAdapter("", t.Connection);
            SqlDataAdapter daSP = new SqlDataAdapter("", t.Connection);
            ViewDataset.VIEW_Becchu_ShukkaMeisaiDataTable dtBecchu = new ViewDataset.VIEW_Becchu_ShukkaMeisaiDataTable();
            ViewDataset.VIEW_Becchu2_ShukkaMeisaiDataTable dtBecchu2 = new ViewDataset.VIEW_Becchu2_ShukkaMeisaiDataTable();
            ViewDataset.VIEW_DS_ShukkaMeisaiDataTable dtDS = new ViewDataset.VIEW_DS_ShukkaMeisaiDataTable();
            ViewDataset.VIEW_SP_ShukkaMeisaiDataTable dtSP = new ViewDataset.VIEW_SP_ShukkaMeisaiDataTable();

            switch (kubun)
            {
                case EnumBecchuKubun.Becchu:
                    // VIEW_BecchuMeisai
                    daBecchu.SelectCommand.CommandText = "select * from VIEW_Becchu_ShukkaMeisai WHERE SashizuBi=@z and SashizuNo=@n and ShiiresakiCode=@s";
                    daBecchu.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
                    daBecchu.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
                    daBecchu.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
                    daBecchu.SelectCommand.Transaction = t;
                    daBecchu.Fill(dtBecchu);
                    dtBecchu.DefaultView.Sort = "HinbanNo, RowNo";
                    break;
                case EnumBecchuKubun.Becchu2:
                    // VIEW_Becchu2_ShukkaMeisai
                    daBecchu2.SelectCommand.CommandText = "select * from VIEW_Becchu2_ShukkaMeisai WHERE SashizuBi=@z and SashizuNo=@n and ShiiresakiCode=@s";
                    daBecchu2.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
                    daBecchu2.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
                    daBecchu2.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
                    daBecchu2.SelectCommand.Transaction = t;
                    daBecchu2.Fill(dtBecchu2);
                    dtBecchu2.DefaultView.Sort = "RowNo";
                    break;
                case EnumBecchuKubun.DS:
                    // DS
                    daDS.SelectCommand.CommandText = "select * from VIEW_DS_ShukkaMeisai WHERE OrderKanriNo=@n";
                    daDS.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
                    daDS.SelectCommand.Transaction = t;
                    daDS.Fill(dtDS);
                    dtDS.DefaultView.Sort = "RowNo";
                    break;
                case EnumBecchuKubun.SP:
                    // SP
                    daSP.SelectCommand.CommandText = "select * from VIEW_SP_ShukkaMeisai WHERE OrderKanriNo=@n";
                    daSP.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
                    daSP.SelectCommand.Transaction = t;
                    daSP.Fill(dtSP);
                    dtSP.DefaultView.Sort = "No, MeisaiNo";
                    break;
            }

            int nGyouNo = 1;
            int nEdaban = 0;
            List<string> lstHinbanSizeLotNo = new List<string>();
            List<string> lstKanryouFlgHinbanSizeLotNo = new List<string>();  // �����t���O���Z�b�g����i�ԃT�C�Y
            MizunoDataSet.T_NouhinMeisaiDataTable dtM = new MizunoDataSet.T_NouhinMeisaiDataTable();

            bool bLotHikiate = false;

            //for (int i = 0; i < data.lstMeisai.Count; i++)
            //{

            //for (int i = 0; i < dt.Rows.Count; i++)   //2013.06.19 ����
            for (int i = 0; i < data.lstMeisai.Count; i++)
            {
                MeisaiData m = data.lstMeisai[i];
                MizunoDataSet.T_NouhinMeisaiRow dr = dtM.NewT_NouhinMeisaiRow();
                NouhinDataClass.InitT_NouhinMeisaiRow(dr);
                dr.HakkouNo = nHakkouNo;
                dr.ShiiresakiCode = drHeader.NouhinmotoShiiresakiCode;
                dr.YYMM = drHeader.YYMM;

                if (NouhinDataClass.NOUHINDATA_MAX_GYOU_COUNT < nGyouNo)
                {
                    // �s�ԍ���9�ȏ�̎�
                    nGyouNo = 1; // �߂�
                    nEdaban++;
                }

                dr.GyouNo = nGyouNo;
                dr.Edaban = nEdaban;

                int nRowNo = 0;


                dr.Hinban = m.Key.Hinban;
                dr.LotNo = LotNo[i];    //2013.06.19 ����
                //dr.LotNo = m.Key.LotNo;

                dr.Tani = "";

                dr.HinbanTsuikaFlg = HinbanTsuikaFlg.Normal;

                string strHinbanSizeLotNo = string.Format("{0}\t{1}\t{2}", m.Key.Hinban, m.Key.Size, m.Key.LotNo);
                if (!lstHinbanSizeLotNo.Contains(strHinbanSizeLotNo)) lstHinbanSizeLotNo.Add(strHinbanSizeLotNo);

                if (m.KanryouFlag)
                {
                    if (!lstKanryouFlgHinbanSizeLotNo.Contains(strHinbanSizeLotNo))
                        lstKanryouFlgHinbanSizeLotNo.Add(strHinbanSizeLotNo);
                }

                //int nShukkaSu = m.ShukkaSu;
                int nShukkaSu = Suuryou[i];   //2013.06.19 ����

                switch (kubun)
                {
                    // ����ʂ���̓o�^�̏ꍇ��UPLOAD����̓o�^�̏ꍇ�Ƃ��ɕi�ԁA�T�C�Y�A���b�gNo�ŕR�Â���OK

                    case EnumBecchuKubun.Becchu:
                        dtBecchu.DefaultView.RowFilter = string.Format(" Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", m.Key.Hinban.Replace("'", "''"), m.Key.Size.Replace("'", "''"), m.Key.LotNo.Replace("'", "''"));
                        if (!bShusei) dtBecchu.DefaultView.RowFilter += " AND (KanryouFlg is null or KanryouFlg=False)";

                        if (0 == dtBecchu.DefaultView.Count) throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�A���b�gNo.{2}�̔[���c������܂���B", m.Key.Hinban, m.Key.Size, m.Key.LotNo));

                        for (int y = 0; y < dtBecchu.DefaultView.Count; y++)
                        {
                            ViewDataset.VIEW_Becchu_ShukkaMeisaiRow d = dtBecchu.DefaultView[y].Row as ViewDataset.VIEW_Becchu_ShukkaMeisaiRow;
                            if (d.Suryou < d.ShukkaSuu) throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�A���b�gNo.{2}�ŏo�א��s����", m.Key.Hinban, m.Key.Size, m.Key.LotNo));
                            if (d.Suryou == d.ShukkaSuu) continue;
                            int nZan = (int)d.Suryou - d.ShukkaSuu;
                            int nHikiate = (nZan < nShukkaSu) ? nZan : nShukkaSu;
                            d.ShukkaSuu += nHikiate;    // �f�[�^�Z�b�g�ō쐬�����ShukkaSuu��ReadOnly�ɂȂ�̂͂Ȃ��H�H
                            nShukkaSu -= nHikiate;
                            if (0 == nShukkaSu) break;
                        }
                        break;
                    case EnumBecchuKubun.Becchu2:
                        // ����ʂ���̓o�^�̏ꍇ�͕i�ԁA�T�C�Y�ARowNo�ŕR�t����
                        //   UPLOAD����̓o�^�̏ꍇ
                        //   ���b�gNo�����݂���@���i�ԁA�T�C�Y�A���b�gNo�ŕR�t����
                        //   ���b�gNo�����݂��Ȃ����i�ԁA�T�C�Y�ARowNo�ŕR�t����

                        if (bUpload)
                        {
                            if (!string.IsNullOrEmpty(m.Key.LotNo))
                            {
                                dtBecchu2.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'",
                                    m.Key.Hinban.Replace("'", "''"), m.Key.Size.Replace("'", "''"), m.Key.LotNo.Replace("'", "''"));

                                bLotHikiate = true;
                            }
                            else if (m.Key.RowNo > 0)
                            {
                                nRowNo = m.Key.RowNo;
                                dtBecchu2.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND RowNo={2}",
                                    m.Key.Hinban.Replace("'", "''"), m.Key.Size.Replace("'", "''"), m.Key.RowNo);
                            }
                            else
                            {
                                throw new Exception(string.Format("���b�gNo.�܂��͍sNo����͂��Ă��������B�i�i��{0}�A�T�C�Y{1}�j", m.Key.Hinban, m.Key.Size));
                            }
                            if (!bShusei) dtBecchu2.DefaultView.RowFilter += " AND (KanryouFlg is null or KanryouFlg=False)";

                            if (2 <= dtBecchu2.DefaultView.Count)
                                throw new Exception(string.Format("�����ł��Ȃ����ׂ����݂��܂��B�i�i��{0}�A�T�C�Y{1}�A���b�gNo.{2})", m.Key.Hinban, m.Key.Size, m.Key.LotNo));

                            if (0 == dtBecchu2.DefaultView.Count)
                                throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�A�sNo.{2}�̔[���c������܂���B", m.Key.Hinban, m.Key.Size, m.Key.RowNo));
                        }
                        else
                        {
                            nRowNo = RowNo[i];
                            //dtBecchu2.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND RowNo={2}",
                            //    m.Key.Hinban.Replace("'", "''"), m.Key.Size.Replace("'", "''"), m.Key.RowNo);

                            dtBecchu2.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND RowNo={2}",
                                m.Key.Hinban.Replace("'", "''"), Size[i].Replace("'", "''"), RowNo[i]);

                            //-----2012-6-22 ꎓ��R�����g
                            //if (!bShusei) dtBecchu2.DefaultView.RowFilter += " AND (KanryouFlg is null or KanryouFlg=False)";

                            //if (2 <= dtBecchu2.DefaultView.Count) throw new Exception("2���ȏ�擾����邱�Ƃ͖���");


                            //if (0 == dtBecchu2.DefaultView.Count)
                            //    throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�A�sNo.{2}�̔[���c������܂���B", m.Key.Hinban, m.Key.Size, m.Key.RowNo));
                            //---------------------------
                        }

                        if (1 == dtBecchu2.DefaultView.Count)
                        {
                            ViewDataset.VIEW_Becchu2_ShukkaMeisaiRow d = dtBecchu2.DefaultView[0].Row as ViewDataset.VIEW_Becchu2_ShukkaMeisaiRow;
                            if (d.Suuryou < d.ShukkaSuu) throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�ŏo�א��s����", m.Key.Hinban, m.Key.Size));
                            if (d.Suuryou > d.ShukkaSuu)
                            {
                                int nZan = d.Suuryou - d.ShukkaSuu;
                                int nHikiate = (nZan < nShukkaSu) ? nZan : nShukkaSu;
                                d.ShukkaSuu += nHikiate;
                                nShukkaSu -= nHikiate;
                            }
                        }

                        break;
                    case EnumBecchuKubun.DS:
                        dtDS.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}'", m.Key.Hinban.Replace("'", "''"), m.Key.Size.Replace("'", "''"));
                        if (!bShusei) dtDS.DefaultView.RowFilter += " AND (KanryouFlg is null or KanryouFlg=False)";
                        if (0 == dtDS.DefaultView.Count) throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�̔[���c������܂���B", m.Key.Hinban, m.Key.Size));
                        for (int y = 0; y < dtDS.DefaultView.Count; y++)
                        {
                            ViewDataset.VIEW_DS_ShukkaMeisaiRow d = dtDS.DefaultView[y].Row as ViewDataset.VIEW_DS_ShukkaMeisaiRow;
                            if (d.Suryou < d.ShukkaSuu) throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�ŏo�א��s����", m.Key.Hinban, m.Key.Size));
                            if (d.Suryou == d.ShukkaSuu) continue;
                            int nZan = d.Suryou - d.ShukkaSuu;
                            int nHikiate = (nZan < nShukkaSu) ? nZan : nShukkaSu;
                            d.ShukkaSuu += nHikiate;
                            nShukkaSu -= nHikiate;
                            if (0 == nShukkaSu) break;
                        }
                        break;
                    case EnumBecchuKubun.SP:
                        //20200325 LotNo�ǉ� LotNo���Ȃ����ƂŁA�`�[�f�[�^�A�b�v���[�h����LotNo�̈Ⴂ�ɂ��G���[��������������
                        dtSP.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", m.Key.Hinban.Replace("'", "''"), m.Key.Size.Replace("'", "''"), m.Key.LotNo.Replace("'", "''"));
                        if (!bShusei) dtSP.DefaultView.RowFilter += " AND (KanryouFlg is null or KanryouFlg=False)";
                        if (0 == dtSP.DefaultView.Count) throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�A���b�gNo{2}�̔[���c������܂���B", m.Key.Hinban, m.Key.Size, m.Key.LotNo));
                        for (int y = 0; y < dtSP.DefaultView.Count; y++)
                        {
                            ViewDataset.VIEW_SP_ShukkaMeisaiRow d = dtSP.DefaultView[y].Row as ViewDataset.VIEW_SP_ShukkaMeisaiRow;
                            //20200325 LotNo�ǉ� LotNo���Ȃ����ƂŁA�`�[�f�[�^�A�b�v���[�h����LotNo�̈Ⴂ�ɂ��G���[��������������
                            if (d.Suuryou < d.ShukkaSuu) throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�A���b�gNo{2}�ŏo�א��s����", m.Key.Hinban, m.Key.Size, m.Key.LotNo));
                            if (d.Suuryou == d.ShukkaSuu) continue;
                            int nZan = d.Suuryou - d.ShukkaSuu;
                            int nHikiate = (nZan < nShukkaSu) ? nZan : nShukkaSu;
                            d.ShukkaSuu += nHikiate;
                            nShukkaSu -= nHikiate;
                            if (0 == nShukkaSu) break;
                        }
                        break;

                }

                //if (0 < nShukkaSu)
                //{
                //    // �������Ăł��Ȃ���
                //    string noLotNoMsg = string.IsNullOrEmpty(m.Key.LotNo) ? "" : "�A���b�gNo" + m.Key.LotNo;
                //    throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}{2}�͏o�א����������𒴂��Ă��܂��B", m.Key.Hinban, m.Key.Size, noLotNoMsg));
                //}

                // ���o�׏����擾
                MizunoDataSet.T_BecchuShukkaInfoRow drShukkaInfo = null;

                if (bUpload)
                {
                    switch (kubun)
                    {
                        case EnumBecchuKubun.Becchu:
                        case EnumBecchuKubun.DS:
                        case EnumBecchuKubun.SP:
                            {
                                drShukkaInfo = dtShukkaInfo.FindBySashizuBiSashizuNoShiiresakiCodeHinbanSizeLotNoRowNo(key.UketsukeBi, key.OrderKanriNo, key.ShiiresakiCode, m.Key.Hinban, m.Key.Size, m.Key.LotNo, nRowNo);
                            }
                            break;
                        case EnumBecchuKubun.Becchu2:
                            {
                                nRowNo = RowNo[i];

                                if (bLotHikiate)
                                {
                                    //dtShukkaInfo.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'",
                                    //    m.Key.Hinban.Replace("'", "''"), m.Key.Size.Replace("'", "''"), m.Key.LotNo.Replace("'", "''"));

                                    dtShukkaInfo.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'",
                                        m.Key.Hinban.Replace("'", "''"), Size[i].Replace("'", "''"), LotNo[i].Replace("'", "''"));

                                    if (dtShukkaInfo.DefaultView.Count != 1)
                                    {
                                        throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�A���b�gNo.{2}�ł͈������ł��܂���B", m.Key.Hinban, Size[i], LotNo[i]));
                                    }
                                }
                                else
                                {
                                    dtShukkaInfo.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND RowNo={2}",
                                        m.Key.Hinban.Replace("'", "''"), Size[i].Replace("'", "''"), LotNo[i]);
                                }

                                if (dtShukkaInfo.DefaultView.Count == 1)
                                {
                                    drShukkaInfo = (MizunoDataSet.T_BecchuShukkaInfoRow)dtShukkaInfo.DefaultView[0].Row;
                                }
                            }
                            break;
                    }
                }
                else
                {
                    drShukkaInfo = dtShukkaInfo.FindBySashizuBiSashizuNoShiiresakiCodeHinbanSizeLotNoRowNo(key.UketsukeBi, key.OrderKanriNo, key.ShiiresakiCode, m.Key.Hinban, Size[i], LotNo[i], RowNo[i]);
                }

                if (null == drShukkaInfo)
                {
                    drShukkaInfo = dtShukkaInfo.NewT_BecchuShukkaInfoRow();
                    drShukkaInfo.SashizuBi = key.UketsukeBi;
                    drShukkaInfo.ShiiresakiCode = key.ShiiresakiCode;
                    drShukkaInfo.SashizuNo = key.OrderKanriNo;
                    drShukkaInfo.Hinban = m.Key.Hinban;

                    //drShukkaInfo.Size = m.Key.Size;
                    //drShukkaInfo.LotNo = m.Key.LotNo;
                    //drShukkaInfo.RowNo = nRowNo;

                    drShukkaInfo.Size = Size[i];
                    drShukkaInfo.LotNo = LotNo[i];
                    drShukkaInfo.RowNo = RowNo[i];

                    if (kubun == EnumBecchuKubun.Becchu2)
                        drShukkaInfo.KanryouFlg = m.KanryouFlag;    // BS2�̏ꍇ�͕i�ԃT�C�Y�P�ʂŊ����t���O���Z�b�g���ꂸ�A���גP�ʂׁ̈A�����ŃZ�b�g���Ă���
                    else
                        drShukkaInfo.KanryouFlg = false;    //�@�����t���O�͌�ŃZ�b�g����B

                    //drShukkaInfo.ShukkaSuu = m.ShukkaSu;
                    drShukkaInfo.ShukkaSuu = Suuryou[i];

                    //----ꎓ��ǉ�
                    //
                    drShukkaInfo.LotNo = LotNo[i];

                    //---
                    dtShukkaInfo.Rows.Add(drShukkaInfo);
                }
                else
                {
                    if (!bShusei)
                    {
                        if (drShukkaInfo.KanryouFlg)
                            throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�͏o�ׂ��������Ă��܂��B", m.Key.Hinban, m.Key.Size));
                    }
                    drShukkaInfo.ShukkaSuu += m.ShukkaSu;
                    if (kubun == EnumBecchuKubun.Becchu2)
                        drShukkaInfo.KanryouFlg = m.KanryouFlag;  // BS2�̏ꍇ�͕i�ԃT�C�Y�P�ʂŊ����t���O���Z�b�g���ꂸ�A���גP�ʂׁ̈A�����ŃZ�b�g���Ă���
                }
                // �����t���O
                dr.KanryouFlg = KanryouFlg.MiKanryou;   // �����t���O�͌�Őݒ肷��̂Ŗ������ɂ��Ă���
                // �{���ׂɂ́A����i��+�T�C�Y�̃f�[�^���������݂���P�[�X������B�i����i�ԃT�C�Y�ŉ��i���قȂ�ꍇ�j
                // �����t���O�́A�i��+�T�C�Y�P�ʂŐݒ肷�邪�A�������R�[�h�ɐݒ�o�����A�Ō�̃��R�[�h�ɑ΂��Đݒ肷��K�v����B

                dr.Size = Size[i];

                dr.LotNo = LotNo[i];

                // ����P��(�����_���ʂ��܂܂��̂Œ���)
                //dr.TorihikiTanka = m.TourokuKakaku.ToString("000000.0").Replace(".", "");
                dr.TorihikiTanka = Kakaku[i].ToString();
                // �[�i��
                //dr.NouhinSuu = m.ShukkaSu;
                //dr.NouhinSuu = Suuryou[i];
                dr.NouhinSuu = decimal.Parse(Suuryou[i].ToString().Replace(".00", ""));

                dr.NouhinKubun = (m.NouhinKubun == NouhinDataClass.EnumNouhinKubun.TujouNouhin) ? "" : ((int)m.NouhinKubun).ToString();
                // �Z�ʉ��i
                dr.YuuduuKakaku = m.YuuduuKakaku.ToString("0000000");
                // �Еt(2011/10/24 ���c�ے��̎w���Œǉ� by ykuri)
                dr.Sekiduke = m.Sekizuke.ToString("0000");

                // �E�v
                //---ꎓ��ύX
                if (m.Tekiyou != null)
                {
                    dr.ShouhinRyakuMei = m.Tekiyou.Trim();
                    //dr.ShouhinRyakuMei = Kakaku[i].ToString();
                }
                else
                {
                    dr.ShouhinRyakuMei = "";
                }
                //dr.RowNo = nRowNo;
                //dr.LotNo = m.Key.LotNo;
                dr.RowNo = RowNo[i];
                dr.LotNo = LotNo[i];
                dr.FreeKoumoku1 = string.IsNullOrEmpty(m.Free1) ? "" : m.Free1;
                dr.FreeKoumoku2 = string.IsNullOrEmpty(m.Free2) ? "" : m.Free2;
                dr.FreeKoumoku3 = string.IsNullOrEmpty(m.Free3) ? "" : m.Free3;

                dr.Bikou = string.IsNullOrEmpty(m.Bikou) ? "" : m.Bikou;    // 31�����l
                dtM.Rows.Add(dr);
                nGyouNo++;

            }

            List<string> lstKannouHinbanSizeLotNo = new List<string>();


            if (kubun == EnumBecchuKubun.Becchu2)
            {
                // BS2�Ɍ����ẮA����i�ԃT�C�Y���u���b�gNo���u�����N�v�̖��ׂ�����A���ꂼ��Ŋ����t���O���Z�b�g�������B
                // �e�i�ԃT�C�Y�̑S���ׂ������i���[�j�����^�C�~���O�Ŕ[�i�f�[�^�ɂ͊����t���O�i9�j���Z�b�g����B
                DataView dvBS2 = new DataView(dtBecchu2);
                for (int i = 0; i < lstHinbanSizeLotNo.Count; i++)
                {
                    string strHinban = lstHinbanSizeLotNo[i].Split('\t')[0].Replace("'", "''");
                    string strSize = lstHinbanSizeLotNo[i].Split('\t')[1].Replace("'", "''");
                    string strLotNo = lstHinbanSizeLotNo[i].Split('\t')[2].Replace("'", "''");
                    dvBS2.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", strHinban, strSize, strLotNo);
                    int nMishukka = 0;
                    if (0 == dvBS2.Count) continue;
                    for (int k = 0; k < dvBS2.Count; k++)
                    {
                        ViewDataset.VIEW_Becchu2_ShukkaMeisaiRow dr = dvBS2[k].Row as ViewDataset.VIEW_Becchu2_ShukkaMeisaiRow;
                        if (dr.Suuryou <= dr.ShukkaSuu) continue;

                        MizunoDataSet.T_BecchuShukkaInfoRow drShukkaInfo = null;
                        if (bUpload)
                        {
                            if (bLotHikiate)
                            {
                                dtShukkaInfo.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'",
                                    dr.Hinban.Replace("'", "''"), dr.Size.Replace("'", "''"), dr.LotNo.Replace("'", "''"));

                                if (dtShukkaInfo.DefaultView.Count != 1)
                                {
                                    throw new Exception(string.Format("�i��{0}�A�T�C�Y{1}�A���b�gNo.{2}�ł͈������ł��܂���B", dr.Hinban, dr.Size, dr.LotNo));
                                }
                            }
                            else
                            {
                                dtShukkaInfo.DefaultView.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND RowNo={2}",
                                    dr.Hinban.Replace("'", "''"), dr.Size.Replace("'", "''"), dr.RowNo);
                            }

                            if (dtShukkaInfo.DefaultView.Count == 1)
                            {
                                drShukkaInfo = (MizunoDataSet.T_BecchuShukkaInfoRow)dtShukkaInfo.DefaultView[0].Row;
                            }
                        }
                        else
                        {
                            drShukkaInfo = dtShukkaInfo.FindBySashizuBiSashizuNoShiiresakiCodeHinbanSizeLotNoRowNo(key.UketsukeBi, key.OrderKanriNo, key.ShiiresakiCode, dr.Hinban, dr.Size, dr.LotNo, dr.RowNo);
                        }

                        if (null != drShukkaInfo && drShukkaInfo.KanryouFlg) continue;  // MizunoDataSet.T_BecchuShukkaInfoRow�̊����t���O�͐�ɃZ�b�g����Ă���B

                        nMishukka++;
                    }
                    if (0 == nMishukka)
                        lstKannouHinbanSizeLotNo.Add(lstHinbanSizeLotNo[i]);  // �{�i�ԃT�C�Y���S�Ċ����������͏o�׍ς݂Ŋ��[�Ƃ���B
                }
            }
            else
            {
                // �S���o�ׂŊ����t���O���Z�b�g�o����i��+�T�C�Y���擾
                for (int i = 0; i < lstHinbanSizeLotNo.Count; i++)
                {
                    string strHinban = lstHinbanSizeLotNo[i].Split('\t')[0].Replace("'", "''");
                    string strSize = lstHinbanSizeLotNo[i].Split('\t')[1].Replace("'", "''");
                    string strLotNo = lstHinbanSizeLotNo[i].Split('\t')[2].Replace("'", "''");
                    int nChumonSu = -1;
                    int nShukkaSu = 0;
                    switch (kubun)
                    {
                        case EnumBecchuKubun.Becchu:
                            nChumonSu = Convert.ToInt32(dtBecchu.Compute("SUM(Suryou)", string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", strHinban, strSize, strLotNo)));
                            nShukkaSu = Convert.ToInt32(dtBecchu.Compute("SUM(ShukkaSuu)", string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", strHinban, strSize, strLotNo)));
                            break;
                        case EnumBecchuKubun.Becchu2:
                            nChumonSu = Convert.ToInt32(dtBecchu2.Compute("SUM(Suuryou)", string.Format("Hinban='{0}' AND Size='{1}'", strHinban, strSize)));
                            nShukkaSu = Convert.ToInt32(dtBecchu2.Compute("SUM(ShukkaSuu)", string.Format("Hinban='{0}' AND Size='{1}'", strHinban, strSize)));

                            break;
                        case EnumBecchuKubun.DS:
                            nChumonSu = Convert.ToInt32(dtDS.Compute("SUM(Suryou)", string.Format("Hinban='{0}' AND Size='{1}'", strHinban, strSize)));
                            nShukkaSu = Convert.ToInt32(dtDS.Compute("SUM(ShukkaSuu)", string.Format("Hinban='{0}' AND Size='{1}'", strHinban, strSize)));
                            break;
                        case EnumBecchuKubun.SP:
                            //20200325 LotNo�ǉ� LotNo���Ȃ����ƂŁA�`�[�f�[�^�A�b�v���[�h����LotNo�̈Ⴂ�ɂ��G���[��������������
                            nChumonSu = Convert.ToInt32(dtSP.Compute("SUM(Suuryou)", string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", strHinban, strSize, strLotNo)));
                            nShukkaSu = Convert.ToInt32(dtSP.Compute("SUM(ShukkaSuu)", string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", strHinban, strSize, strLotNo)));
                            break;

                    }
                    if (nChumonSu == nShukkaSu)
                    {
                        if (!lstKanryouFlgHinbanSizeLotNo.Contains(lstHinbanSizeLotNo[i]))
                            lstKanryouFlgHinbanSizeLotNo.Add(lstHinbanSizeLotNo[i]);
                        if (!lstKannouHinbanSizeLotNo.Contains(lstHinbanSizeLotNo[i]))
                            lstKannouHinbanSizeLotNo.Add(lstHinbanSizeLotNo[i]);
                    }
                }


                // �����t���O���Z�b�g����B����i�ԃT�C�Y�������܂܂��P�[�X������̂ŁA���ׂ̍Ō�ɑ΂��Ċ����t���O���Z�b�g����B
                for (int i = 0; i < lstKanryouFlgHinbanSizeLotNo.Count; i++)
                {
                    string strHinban = lstKanryouFlgHinbanSizeLotNo[i].Split('\t')[0];
                    string strSize = lstKanryouFlgHinbanSizeLotNo[i].Split('\t')[1];
                    string strLotNo = lstKanryouFlgHinbanSizeLotNo[i].Split('\t')[2];
                    dtM.DefaultView.RowFilter = string.Format("Hinban='{0}' and Size='{1}' and LotNo='{2}'", strHinban.Replace("'", "''"), strSize.Replace("'", "''"), strLotNo.Replace("'", "''"));
                    dtM.DefaultView.Sort = "Edaban, GyouNo";

                    MizunoDataSet.T_NouhinMeisaiRow drLast = dtM.DefaultView[dtM.DefaultView.Count - 1].Row as MizunoDataSet.T_NouhinMeisaiRow; // ���Ƀ\�[�g���Ȃ��Ă����̖���
                    drLast.KanryouFlg = KanryouFlg.Kanryou; // �Ō�̃��R�[�h�ɑ΂��Đݒ肷��B


                    // �o�׏��ɂ������t���O���Z�b�g����B(�ʒ�2�ȊO�́A�i��+�T�C�Y�łP�����Y������B�ʒ�2��RowNo�P�ʂ����i��+�T�C�Y�P�ʂŊ����t���O���Z�b�g����̂ŁA�S�ĂɑΉ�����B)
                    // �債�������łȂ��̂ŁA�P���Ƀ��[�v�Ō���
                    for (int k = 0; k < dtShukkaInfo.Count; k++)
                    {
                        MizunoDataSet.T_BecchuShukkaInfoRow drShukkaInfo = dtShukkaInfo[k];
                        if (drShukkaInfo.Hinban == strHinban && drShukkaInfo.Size == strSize && drShukkaInfo.LotNo == strLotNo)
                        {
                            drShukkaInfo.KanryouFlg = true;
                        }
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
            drTrailer.ShiiresakiCode = key.ShiiresakiCode;

            // �׎�S���҃R�[�h
            drTrailer.EigyouTantoushaCode = data.EigyouTantoushaCode;
            // �w�Z���A�`�[����
            drTrailer.TeamMei = data.TeamMei;
            // ���Y���S���҃R�[�h
            drTrailer.SKTantoushaCode = data.SKTantoushaCode;
            // �X��
            drTrailer.TenMei = data.TenMei;



            // �o�א�X�֔ԍ�
            drTrailer.ShukkaSakiYubinBangou = ShukkaSakiYubinBangou;

            // �o�א�Z��
            drTrailer.ShukkaSakiJyusho = ShukkaSakiJyusho;

            // �o�א�TEL
            drTrailer.ShukkaSakiTel = ShukkaSakiTel;

            // �^�����@
            drTrailer.UnsouHouhou = "";

            // �`�[���s��            
            //drTrailer.NouhinShoHiduke = data.dtHakkouBi.ToString("yyMMdd");
            drTrailer.NouhinShoHiduke = YM;

            // �\��(����No)
            // DS�ASP�̏ꍇ
            if (kubun == EnumBecchuKubun.DS || kubun == EnumBecchuKubun.SP)
            {
                if (key.OrderKanriNo.Length > 6)
                {
                    // 7�`14���܂�
                    drTrailer.Yobi2 = key.OrderKanriNo.Substring(6, key.OrderKanriNo.Length - 6);
                }
                else
                {
                    // �S��
                    drTrailer.Yobi2 = key.OrderKanriNo;
                }
            }
            else
            {
                drTrailer.Yobi2 = key.OrderKanriNo;
            }
            dtTrailer.Rows.Add(drTrailer);

            SqlDataAdapter daHeader = new SqlDataAdapter("select * from T_NouhinHeader", t.Connection);
            daHeader.SelectCommand.Transaction = t;
            daHeader.InsertCommand = new SqlCommandBuilder(daHeader).GetInsertCommand();
            daHeader.InsertCommand.Transaction = t;




            //if (!Huku)
            //{
                daHeader.Update(dtHeader);


                daM.Update(dtM);
                daTrailer.Update(dtTrailer);
                daShukkaInfo.Update(dtShukkaInfo);
            //}
            NouhinDataClass.DenpyouKey dk;

            //if (strYYMM == "")
            //{
            //    dk = new NouhinDataClass.DenpyouKey(key.ShiiresakiCode, int.Parse(data.dtHakkouBi.ToString("yyMM")), nHakkouNo);
            //}
            //else
            //{
                dk = new NouhinDataClass.DenpyouKey(key.ShiiresakiCode, int.Parse(strYYMM), nHakkouNo);
            //}

            

            // ----- �����M���̊����t���O�̃`�F�b�N -----
            for (int i = 0; i < lstHinbanSizeLotNo.Count; i++)
            {
                string strHinbanSizeLotNo = lstHinbanSizeLotNo[i];
                string strHinban = strHinbanSizeLotNo.Split('\t')[0];
                string strSize = strHinbanSizeLotNo.Split('\t')[1];
                string strLotNo = strHinbanSizeLotNo.Split('\t')[2];

                // �����M�f�[�^�擾

                /// 2014/05/26 �g�����U�N�V�������̓f�[�^�x�[�X����̎擾�͕s��
                /// 
                //daGetMisoushin.SelectCommand.Parameters["@Hinban"].Value = strHinban;
                //daGetMisoushin.SelectCommand.Parameters["@Size"].Value = strSize;
                //daGetMisoushin.SelectCommand.Parameters["@LotNo"].Value = strLotNo;
                //dtM.Clear();
                //daGetMisoushin.Fill(dtM);
                //if (0 == dtM.Count) continue;
                //DataView dv = new DataView(dtM);

                //dv.Sort = "ShukkaBi, YYMM, HakkouNo, Edaban, GyouNo";  // // ���̏��ԂōŌ�̃��R�[�h���ŏI�[�i�f�[�^(�Ō�Ƀ~�Y�m�ɑ��M����f�[�^)�B1�`�[���ɓ���i�ԃT�C�Y���o�����邱�Ƃ�����i���i���قȂ�j�ׁA�}�ԁA�sNo�̏����Ŗ��׏��ōŌ�Ɋ����t���O���Z�b�g�����悤�l�����Ă���B

                DataView dv = new DataView(dtM);
                dv.RowFilter = string.Format("Hinban='{0}' AND Size='{1}' AND LotNo='{2}'", strHinban, strSize, strLotNo);

                if (0 == dv.Count) continue;

                dv.Sort = "YYMM, HakkouNo, Edaban, GyouNo"; 


                if (lstKannouHinbanSizeLotNo.Contains(strHinbanSizeLotNo))
                {
                    // �S���o�׍ς݂Ȃ̂ŁA�Ō�̔[�i�f�[�^�Ɋ����t���O�i9�j���Z�b�g����B
                    /* �`�[�C�����A�����t���O���Z�b�g����BS2���ׂƈقȂ�ꏊ�Ɋ����t���O���Z�b�g����Ă��܂��׎~�߂�B
                    dv.RowFilter = "NouhinSuu>0";   // �[�i��=0�̓`�[������ׁA�[�i��>0�ōŌ�̖��ׂɊ����t���O���Z�b�g�������B(�`�[�C���Ŕ[�i��=0�̓`�[(����`�[)�̏o�ד����Ō�i�Ō�̓`�[�ɂȂ邱�Ƃ�����j)
                    if (0 == dv.Count)
                        dv.RowFilter = null;    // �[�i��>0�̓`�[�������A�[�i����0�̓`�[�Ɋ����t���O���Z�b�g���Ȃ��Ƃ����Ȃ��B�i��{�I�ɂ��̃P�[�X�͖����͂��j
                    */

                    for (int k = 0; k < dv.Count; k++)
                    {
                        MizunoDataSet.T_NouhinMeisaiRow dr = dv[k].Row as MizunoDataSet.T_NouhinMeisaiRow;
                        dr.KanryouFlg = (dv.Count - 1 == k) ? KanryouFlg.Kanryou : KanryouFlg.MiKanryou;
                    }
                    daM.Update(dtM);
                }
                else
                {
                    // �o�׎c���������ԂŊ����̏ꍇ�A�������ꂩ�̔[�i�f�[�^�Ɋ����t���O�������Ă���ہA�ŏI�[�i�f�[�^�ɂɊ����t���O�������Ă��邩�`�F�b�N
                    dv.RowFilter = string.Format("KanryouFlg='{0}'", KanryouFlg.Kanryou);
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
                                strHinban, string.IsNullOrEmpty(strSize) ? "�Ȃ�" : strSize,
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
                                new Core.Type.Nengetu(drKanryouFlg.YYMM).ToDateTime(),
                                drKanryouFlg.HakkouNo, strHinban, string.IsNullOrEmpty(strSize) ? "�Ȃ�" : strSize, strMsg));
                        }
                    }


                }
            }


            // �ēx�����_�̏o�׏󋵎擾���Ċe�����P�ʂŁA�������邢�͊��[���Ă����T_BecchuKeyInfo�̊����t���O���Z�b�g����B
            bool bKanryou = false;
            switch (kubun)
            {
                case EnumBecchuKubun.Becchu:
                    dtBecchu.Clear();
                    daBecchu.Fill(dtBecchu);
                    dtBecchu.DefaultView.RowFilter = "(KanryouFlg is null or KanryouFlg=False) and Suryou>ShukkaSuu";
                    bKanryou = (0 == dtBecchu.DefaultView.Count);
                    break;
                case EnumBecchuKubun.Becchu2:
                    dtBecchu2.Clear();
                    daBecchu2.Fill(dtBecchu2);
                    dtBecchu2.DefaultView.RowFilter = "(KanryouFlg is null or KanryouFlg=False) and Suuryou>ShukkaSuu";
                    bKanryou = (0 == dtBecchu2.DefaultView.Count);
                    break;
                case EnumBecchuKubun.DS:
                    dtDS.Clear();
                    daDS.Fill(dtDS);
                    dtDS.DefaultView.RowFilter = "(KanryouFlg is null or KanryouFlg=False) and Suryou>ShukkaSuu";
                    bKanryou = (0 == dtDS.DefaultView.Count);
                    break;
                case EnumBecchuKubun.SP:
                    dtSP.Clear();
                    daSP.Fill(dtSP);
                    dtSP.DefaultView.RowFilter = "(KanryouFlg is null or KanryouFlg=False) and Suuryou>ShukkaSuu";
                    bKanryou = (0 == dtSP.DefaultView.Count);
                    break;
            }

            SqlDataAdapter daT_BecchuKeyInfo = new SqlDataAdapter("", t.Connection);
            daT_BecchuKeyInfo.SelectCommand.CommandText = "SELECT * FROM T_BecchuKeyInfo WHERE MizunoUketsukeBi = @MizunoUketsukeBi AND OrderKanriNo = @OrderKanriNo AND ShiiresakiCode = @ShiiresakiCode";
            daT_BecchuKeyInfo.SelectCommand.Parameters.AddWithValue("@MizunoUketsukeBi", key.UketsukeBi);
            daT_BecchuKeyInfo.SelectCommand.Parameters.AddWithValue("@OrderKanriNo", key.OrderKanriNo);
            daT_BecchuKeyInfo.SelectCommand.Parameters.AddWithValue("@ShiiresakiCode", key.ShiiresakiCode);
            daT_BecchuKeyInfo.SelectCommand.Transaction = t;
            daT_BecchuKeyInfo.UpdateCommand = new SqlCommandBuilder(daT_BecchuKeyInfo).GetUpdateCommand();
            daT_BecchuKeyInfo.UpdateCommand.Transaction = t;
            MizunoDataSet.T_BecchuKeyInfoDataTable T_BecchuKeyInfo = new MizunoDataSet.T_BecchuKeyInfoDataTable();
            daT_BecchuKeyInfo.Fill(T_BecchuKeyInfo);
            T_BecchuKeyInfo[0].KanryouFlg = bKanryou;
            daT_BecchuKeyInfo.Update(T_BecchuKeyInfo);



            //// �c���`�F�b�N(���S�̊m�F������Ή��L�R�[�h�͕s�v)
            //SqlDataAdapter daCheck = new SqlDataAdapter("", t.Connection);
            //daCheck.SelectCommand.CommandText = "select * from VIEW_BecchuChuzanCheckList where (SashizuBi = @z) AND (SashizuNo = @n) AND (ShiiresakiCode = @s)";
            //daCheck.SelectCommand.Parameters.AddWithValue("@z", key.UketsukeBi);
            //daCheck.SelectCommand.Parameters.AddWithValue("@n", key.OrderKanriNo);
            //daCheck.SelectCommand.Parameters.AddWithValue("@s", key.ShiiresakiCode);
            //daCheck.SelectCommand.Transaction = t;

            //DataTable dtCheck = new DataTable();
            //daCheck.Fill(dtCheck);
            //dtCheck.DefaultView.RowFilter = "ShukkaSuu<>NouhinSu";
            //if (0 < dtCheck.DefaultView.Count)
            //{
            //    string[] str = new string[dtCheck.DefaultView.Count];
            //    for (int i = 0; i < dtCheck.DefaultView.Count; i++)
            //    {
            //        string strHinban = Convert.ToString(dtCheck.DefaultView[i]["Hinban"]);
            //        string strSize = Convert.ToString(dtCheck.DefaultView[i]["Size"]);
            //        int nShukkaSuu = Convert.ToInt32(dtCheck.DefaultView[i]["ShukkaSuu"]);
            //        int nNouhinSuu = Convert.ToInt32(dtCheck.DefaultView[i]["NouhinSu"]);
            //        str[i] = string.Format("�i��{0}�A�T�C�Y{1}�ŏo�א��̍��ق�����܂��B�o�א�={2}, �[�i��={3}", strHinban, strSize, nShukkaSuu, nNouhinSuu);
            //    }

            //    throw new Exception(string.Join("/", str));
            //}

            return dk;
        }

        public static int get_SeihinDtCount(SqlConnection sql, string SashizuBi,string Hinban,string ShiiresakiCode ,string OrderKanriNo)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText = "select RowNo from T_Becchu2Repeat where SashizuNo=@sn and ShiiresakiCode=@sc and SashizuBi=@sb and Hinban=@hb";
            da.SelectCommand.Parameters.AddWithValue("@sn", OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@sc", ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@sb", SashizuBi);
            da.SelectCommand.Parameters.AddWithValue("@hb", Hinban);
            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt.Rows.Count;


        }

        public static MizunoDataSet.T_Becchu2RepeatDataTable get_SeihinDtCount2(SqlConnection sql, string SashizuBi, string ShiiresakiCode, string OrderKanriNo)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText = "select * from T_Becchu2Repeat where SashizuNo=@sn and ShiiresakiCode=@sc and SashizuBi=@sb ";
            da.SelectCommand.Parameters.AddWithValue("@sn", OrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@sc", ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@sb", SashizuBi);
            MizunoDataSet.T_Becchu2RepeatDataTable dt = new MizunoDataSet.T_Becchu2RepeatDataTable();
            da.Fill(dt);

            return dt;


        }


        public static string get_TukaCode(string ShiiresakiCode, string SashizuBi, string SashizuNo, SqlConnection sql)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText = "select TukaCode from T_Becchu where ShiiresakiCode=@sc and SashizuBi=@sb and SashizuNo=@sn";
            //MizunoDataSet.T_BecchuDataTable dt = new MizunoDataSet.T_BecchuDataTable();
            DataTable dt = new DataTable();
            da.SelectCommand.Parameters.AddWithValue("@sc", ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@sb", SashizuBi);
            da.SelectCommand.Parameters.AddWithValue("@sn", SashizuNo);
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

        public static string get_TukaCode2(string ShiiresakiCode, string SashizuBi, string SashizuNo, SqlConnection sql)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText = "select TukaCode from T_Becchu2 where ShiiresakiCode=@sc and SashizuBi=@sb and SashizuNo=@sn";
            DataTable dt = new DataTable();
            //MizunoDataSet.T_Becchu2DataTable dt = new MizunoDataSet.T_Becchu2DataTable();
            da.SelectCommand.Parameters.AddWithValue("@sc", ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@sb", SashizuBi);
            da.SelectCommand.Parameters.AddWithValue("@sn", SashizuNo);
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

        public static MizunoDataSet.T_DS2DataTable get_DS2datatable(string OrderNo, SqlConnection sql)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText = "select * from T_DS2 where OrderKanriNo=@o";
            da.SelectCommand.Parameters.AddWithValue("@o", OrderNo);
            MizunoDataSet.T_DS2DataTable dt = new MizunoDataSet.T_DS2DataTable();
            da.Fill(dt);
            return dt;
        }


//        public static ViewDataset.V_KakakuShuusei6DataTable getV_KakakuShuusei6DataTable(string sOrderKanriNo,
//                                                                                        string SashizuBi,
//                                                                                        string ShiiresakiCode,
//                                                                                        string Hinban,
//                                                                                        string ShumokuCode,
//                                                                                        SqlConnection sqlConn
//                                                                                        )
//        {
//            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
//            da.SelectCommand.CommandText = @"
//SELECT * 
//FROM 
//    V_KakakuShuusei6 
//WHERE 
//    ShiiresakiCode=@s and 
//    SashizuBi=@z and 
//    OrderKanriNo=@n and 
//    (
//        ZHinban = @h or
//        BecchuHinban = @h or
//        Becchu2Hinban = @h 
//    ) and
//    Shumoku=@smk and
//    HakkouHiduke is null       --�����s�f�[�^�𒊏o
//";
//            da.SelectCommand.Parameters.AddWithValue("@s", ShiiresakiCode);
//            da.SelectCommand.Parameters.AddWithValue("@z", SashizuBi);
//            da.SelectCommand.Parameters.AddWithValue("@n", sOrderKanriNo);
//            da.SelectCommand.Parameters.AddWithValue("@h", Hinban);
//            da.SelectCommand.Parameters.AddWithValue("@smk", ShumokuCode);

//            ViewDataset.V_KakakuShuusei6DataTable dt = new ViewDataset.V_KakakuShuusei6DataTable();
//            da.Fill(dt);
//            if (dt.Rows.Count > 0)
//                return dt;
//            else
//                return null;
//        }

        public static ViewDataset.V_KakakuShuusei6DataTable getV_KakakuShuusei6DataTable(string sOrderKanriNo,
                                                                                string SashizuBi,
                                                                                string ShiiresakiCode,
                                                                                string ShumokuCode,
                                                                                SqlConnection sqlConn
                                                                                )
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"
SELECT * 
FROM 
    V_KakakuShuusei6 
WHERE 
    ShiiresakiCode=@s and 
    SashizuBi=@z and 
    OrderKanriNo=@n and 
    Shumoku=@smk and
    HakkouHiduke is null       --�����s�f�[�^�𒊏o
";
            da.SelectCommand.Parameters.AddWithValue("@s", ShiiresakiCode);
            da.SelectCommand.Parameters.AddWithValue("@z", SashizuBi);
            da.SelectCommand.Parameters.AddWithValue("@n", sOrderKanriNo);
            da.SelectCommand.Parameters.AddWithValue("@smk", ShumokuCode);

            ViewDataset.V_KakakuShuusei6DataTable dt = new ViewDataset.V_KakakuShuusei6DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
                return dt;
            else
                return null;
        }




        public static bool bChokusouOrder(BecchuOrderClass.BecchuOrderKey VsBecchuOrderKey,string ChokusouJigyoushoCode,string ChokusouHokanBasho, SqlConnection c)
        {
            bool bChokusou = false;

            if (VsBecchuOrderKey != null)
            {
                ViewDataset.VIEW_BecchuKeyInfoRow dr = BecchuOrderClass.getVIEW_BecchuKeyInfoRow(VsBecchuOrderKey, c);
                if (dr != null)
                {
                    if (!dr.IsT_Becchu_SashizuNoNull())
                    {
                        MizunoDataSet.T_BecchuRow drBS = BecchuOrderClass.getT_BecchuRow(VsBecchuOrderKey, c);
                        if (drBS != null)
                        {
                            if (drBS.ChokusouFlag == 1)
                            {
                                //�����̏ꍇ
                                bChokusou = true;
                            }
                        }

                    }
                    else if (!dr.IsT_Becchu2_SashizuNoNull())
                    {
                        MizunoDataSet.T_Becchu2Row drBS2 = BecchuOrderClass.getT_Becchu2Row(VsBecchuOrderKey, c);

                        if (drBS2 != null)
                        {
                            if (drBS2.ChokusouFlag == 1)
                            {
                                //�����̏ꍇ
                                bChokusou = true;
                            }
                        }

                    }
                    else if (!dr.IsT_DS_OrderKanriNoNull())
                    {
                        MizunoDataSet.T_DS2Row drDS2 = BecchuOrderClass.getT_DS2Row(VsBecchuOrderKey.OrderKanriNo, c);
                        if (drDS2 != null)
                        {
                            //�d�l�ύX�̈׃R�����g�A�E�g 20160128 ���R�l���˗�
                            ////�udrDS2.TeemName�v �Ƃ����̂� �u�׎�ۊǏꏊ�v�̎��B
                            //string[] aryChoku = ChokusouJigyoushoCode.Split(',');

                            ////���Ə���CONFIG�Őݒ肳��Ă��钼���悩�`�F�b�N
                            //bool bJigyousho = false;
                            //for (int i = 0; i < aryChoku.Length; i++)
                            //{
                            //    if (drDS2.JigyosyoCode == aryChoku[i])
                            //    {
                            //        bJigyousho = true;
                            //        break;
                            //    }
                            //}

                            //if (drDS2.IsTeemNameNull() == false)
                            //{
                            //    if (bJigyousho == true &&
                            //        drDS2.TeemName == ChokusouHokanBasho
                            //        )
                            //    {
                            //        //�����̏ꍇ
                            //        bChokusou = true;
                            //    }
                            //}

                            //�d�l�ύX�@20160128�@���R�l���˗�
                            //���������̕ύX�@���i�敪=4�̏ꍇ�A����
                            //�d�l�ύX  20160210  ���R�l���˗�
                            //���������̒ǉ��@���i�敪=5�̏ꍇ���A����
                            if (drDS2.TyokusouKubun.ToString() == SouhinKubun.Chokusou ||
                                drDS2.TyokusouKubun.ToString() == SouhinKubun.SeisanBumon)
                            {
                                //�����̏ꍇ
                                bChokusou = true;
                            }
                        }

                    }
                    else if (!dr.IsT_SP_OrderKanriNoNull())
                    {
                        MizunoDataSet.T_SPRow drSP = BecchuOrderClass.getT_SPRow(VsBecchuOrderKey.OrderKanriNo, c);
                        if (drSP != null)
                        {
                            //�d�l�ύX�̈׃R�����g�A�E�g 20160128 ���R�l���˗�
                            //string[] aryChoku = ChokusouJigyoushoCode.Split(',');

                            ////���Ə���CONFIG�Őݒ肳��Ă��钼���悩�`�F�b�N
                            //bool bJigyousho = false;
                            //for (int i = 0; i < aryChoku.Length; i++)
                            //{
                            //    if (drSP.NiukeJigyoushoCode == aryChoku[i])
                            //    {
                            //        bJigyousho = true;
                            //        break;
                            //    }
                            //}

                            //if (drSP.IsNiukeHokanBashoNull() == false)
                            //{
                            //    if (bJigyousho == true &&
                            //        drSP.NiukeHokanBasho == ChokusouHokanBasho
                            //        )
                            //    {
                            //        //�����̏ꍇ
                            //        bChokusou = true;
                            //    }
                            //}

                            //�d�l�ύX�@20160128�@���R�l���˗�
                            //���������̕ύX�@�V���c�����敪=1 or �p���c�����敪=1 �̏ꍇ�A����
                            if (drSP.SyatuTyokusouKubun == "1" || drSP.PantuTyokusouKubun == "1")
                            {
                                //�����̏ꍇ
                                bChokusou = true;
                            }
                        }
                    }
                }
            }



            return bChokusou;
        }

        public static bool bChokusouOrderForZairyou(ZairyouOrderClass.ZairyouOrderKey VsZairyouOrderKey, string ChokusouJigyoushoCode, string ChokusouHokanBasho, SqlConnection c)
        {
            bool bChokusou = false;

            //if (VsZairyouOrderKey != null)
            //{
            //    //ViewDataset.VIEW_BecchuKeyInfoRow dr = BecchuOrderClass.getVIEW_BecchuKeyInfoRow(VsZairyouOrderKey, c);
            //    ViewDataset.VIEW_ZairyouOrderKeyRow dr = ZairyouOrderClass.getVIEW_ZairyouOrderKeyRow(VsZairyouOrderKey, "", c);

            //    dr.
            //    if (!dr.IsT_Becchu_SashizuNoNull())
            //    {
            //        MizunoDataSet.T_BecchuRow drBS = BecchuOrderClass.getT_BecchuRow(VsZairyouOrderKey, c);
            //        if (drBS != null)
            //        {
            //            if (drBS.ChokusouFlag == 1)
            //            {
            //                //�����̏ꍇ
            //                bChokusou = true;
            //            }
            //        }

            //    }
            //    else if (!dr.IsT_Becchu2_SashizuNoNull())
            //    {
            //        MizunoDataSet.T_Becchu2Row drBS2 = BecchuOrderClass.getT_Becchu2Row(VsZairyouOrderKey, c);

            //        if (drBS2 != null)
            //        {
            //            if (drBS2.ChokusouFlag == 1)
            //            {
            //                //�����̏ꍇ
            //                bChokusou = true;
            //            }
            //        }

            //    }
            //    else if (!dr.IsT_DS_OrderKanriNoNull())
            //    {
            //        MizunoDataSet.T_DS2Row drDS2 = BecchuOrderClass.getT_DS2Row(VsZairyouOrderKey.OrderKanriNo, c);
            //        if (drDS2 != null)
            //        {
            //            //�udrDS2.TeemName�v �Ƃ����̂� �u�׎�ۊǏꏊ�v�̎��B
            //            string[] aryChoku = ChokusouJigyoushoCode.Split(',');

            //            //���Ə���CONFIG�Őݒ肳��Ă��钼���悩�`�F�b�N
            //            bool bJigyousho = false;
            //            for (int i = 0; i < aryChoku.Length; i++)
            //            {
            //                if (drDS2.JigyosyoCode == aryChoku[i])
            //                {
            //                    bJigyousho = true;
            //                    break;
            //                }
            //            }

            //            if (bJigyousho == true &&
            //                drDS2.TeemName == ChokusouHokanBasho
            //                )
            //            {
            //                //�����̏ꍇ
            //                bChokusou = true;
            //            }
            //        }

            //    }
            //    else if (!dr.IsT_SP_OrderKanriNoNull())
            //    {
            //        MizunoDataSet.T_SPRow drSP = BecchuOrderClass.getT_SPRow(VsZairyouOrderKey.OrderKanriNo, c);
            //        if (drSP != null)
            //        {
            //            string[] aryChoku = ChokusouJigyoushoCode.Split(',');

            //            //���Ə���CONFIG�Őݒ肳��Ă��钼���悩�`�F�b�N
            //            bool bJigyousho = false;
            //            for (int i = 0; i < aryChoku.Length; i++)
            //            {
            //                if (drSP.NiukeJigyoushoCode == aryChoku[i])
            //                {
            //                    bJigyousho = true;
            //                    break;
            //                }
            //            }


            //            if (bJigyousho == true &&
            //                drSP.NiukeHokanBasho == ChokusouHokanBasho
            //                )
            //            {
            //                //�����̏ꍇ
            //                bChokusou = true;
            //            }
            //        }
            //    }
            //}

            return bChokusou;
        }



        public static MizunoDataSet.T_SP_PdfDataTable T_SPDataTable(string strWhere, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = @"SELECT *
                                             FROM   T_BecchuKeyInfo AS B INNER JOIN T_SP AS S ON B.OrderKanriNo = S.OrderKanriNo ";
            if (strWhere != "")
            {
                da.SelectCommand.CommandText += " WHERE " + strWhere;
            }

            MizunoDataSet.T_SP_PdfDataTable dt = new MizunoDataSet.T_SP_PdfDataTable();
            da.Fill(dt);
            return dt;
        }

        public static DataTable GetT_BecchuKeyInfo_Shiiresaki(string sqlcmd, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = string.Format(@"SELECT TOP (100) PERCENT dbo.T_BecchuKeyInfo.ShiiresakiCode, dbo.M_Shiiresaki.ShiiresakiMei
                                             FROM dbo.T_BecchuKeyInfo INNER JOIN
                                                  dbo.M_Shiiresaki ON dbo.T_BecchuKeyInfo.ShiiresakiCode = dbo.M_Shiiresaki.ShiiresakiCode
                                             {0} 
                                             GROUP BY dbo.T_BecchuKeyInfo.ShiiresakiCode, dbo.M_Shiiresaki.ShiiresakiMei, dbo.T_BecchuKeyInfo.ShumokuCode
                                             ORDER BY dbo.T_BecchuKeyInfo.ShiiresakiCode", sqlcmd);

            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static int GetT_BecchuKeyInfo_ShiiresakiCount(string sqlcmd, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = string.Format(@"SELECT TOP (100) PERCENT COUNT(dbo.T_BecchuKeyInfo.ShiiresakiCode) as cnt 
                                             FROM dbo.T_BecchuKeyInfo 
                                             {0} 
                                             ", sqlcmd);

            DataTable dt = new DataTable();
            da.Fill(dt);
            return int.Parse(dt.Rows[0]["cnt"].ToString());
        }

        //�}�[�N���擾
        public static ViewDataset.VIEW_MarkDataTable GetVIEW_MarkDataTable(List<string> strKeyAry, SqlConnection sqlConn)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConn);
            da.SelectCommand.CommandText = "SELECT * FROM VIEW_Mark";
            ViewDataset.VIEW_MarkDataTable dt = new ViewDataset.VIEW_MarkDataTable();

            string strKey = "";
            for (int i = 0; i < strKeyAry.Count; i++)
            {
                string[] key = strKeyAry[i].Split(',');
                if (strKey != "") strKey += " OR ";
                strKey += string.Format("(SashizuBi = '{0}' AND SashizuNo = '{1}')", key[0], key[1]);
            }

            if(strKey != "")
                da.SelectCommand.CommandText += string.Format(" WHERE {0}", strKey);

            da.Fill(dt);

            return dt;
        }
    }
}