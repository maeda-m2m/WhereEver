using System;
using System.Collections.Generic;
using static System.Web.HttpUtility;
using System.IO;
using ZXing;
using System.Drawing;
using System.Web;


namespace WhereEver.ClassLibrary
{
    public class QRCodeClass
    {

        //This script includes the work that is 'ZXing 0.16.6 (created by Michael Jahn)' distributed in the Apache License 2.0.
        //このスクリプトは、 Apache 2.0ライセンスで配布されている製作物「ZXing 0.16.6 (製作者：Michael Jahn)」が含まれています。
        //http://www.apache.org/licenses/LICENSE-2.0


        //下記QRCodeClassのスクリプトの参照元：
        //「【ZXing.Net】C#でQRコードの読取」@satorimon
        //https://qiita.com/satorimon/items/7b7b70410398ee6fd1a4
        //（2021年6月11日アクセス）.

        public static string SetQRCode(HttpPostedFile postedFile)
        {

        //filename = HtmlEncode(filename);

        //QRコードの映った画像ファイルからビットマップを生成する

        //var img = new Bitmap(filename);
        //とすると、imgをDisposeするまで
        //プロセスが読み込んだビットマップファイルを掴んだままになってしまいます。
        //filestreamで読み込むとこの現象を避けられます。
        //System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        //System.Drawing.Image img = System.Drawing.Image.FromStream(fs); //ASP.Netを使うなら、直接fsにSystem.IO.FileStreamを放り込んでもよい。
        //fs.Close();


            byte[] buffer = new byte[postedFile.InputStream.Length];
            postedFile.InputStream.Read(buffer, 0, (int)postedFile.InputStream.Length);

            // TODO:Bitmap生成（使用不可）
            //Bitmap img = (System.Drawing.Bitmap)imgconv.ConvertFrom(buffer);

            // TODO:ファイルを保存 using System.Drawing.Imaging;
            //bitmap.Save(@"C:\img_save\test.jpg", ImageFormat.Jpeg);

            // C#ではFileStreamではなくMemoryStreamを使用する。
            System.IO.MemoryStream mms2 = new System.IO.MemoryStream(buffer);
            Bitmap img = new Bitmap(mms2);
            mms2.Close();

        // QRコードの解析
        ZXing.BarcodeReader reader = new ZXing.BarcodeReader();

        //ZXingに渡すのはBitmap
        ZXing.Result result = reader.Decode(new Bitmap(img));

        //これでQRコードのテキストが読める

        string text;

            if (result == null)
            {

                text = "";

            }
            else
            {

                text = result.Text;

            }

            //因みにresult.BarcodeFormatでコード種類が取れます。
            //QRコードならZXing.BarcodeFormat.QR_CODEのはずです。
            return text;
        }

        public static string GetQRCode(string encodetext, int width, int height)
        {

            if (encodetext == null || encodetext == "")
            {
                return null;
            }

            //encodetext = HtmlEncode(encodetext);

            // QRコードを生成するエンコーダ
            ZXing.QrCode.QRCodeWriter encoder = new ZXing.QrCode.QRCodeWriter();
            //ここでQRコードのオプションが色々指定できるようです。
            Dictionary<EncodeHintType, object> encodeHints = new Dictionary<EncodeHintType, object>()
            {
                { EncodeHintType.CHARACTER_SET, "shift_jis" },
                { EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.M }
            };

            BarcodeWriter writer = new BarcodeWriter();
            //このimageを画像として吐き出します。

            //p1=encodetext エンコードする文字列
            //p2=ZXing.BarcodeFormat.QR_COD ここで指定を変えればバーコードなども作れる模様
            //p3=width
            //p4=height
            System.Drawing.Bitmap bImage = writer.Write(encoder.encode(encodetext, ZXing.BarcodeFormat.QR_CODE, width, height, encodeHints));

            //ASP.NetではSystem.Drawingの使用は非推奨のため、Class内でstring（imgタグ）に変換する。
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] byteImage = ms.ToArray();
            string SigBase64 = Convert.ToBase64String(byteImage); //Get Base64
            return HtmlDecode("<img width=\"" + width + "px\" height=\"" + height + "px\" src=\"data:image/png;base64," + SigBase64 + "\" />");
        }




    }
}