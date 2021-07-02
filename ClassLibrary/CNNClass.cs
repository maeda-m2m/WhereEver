using System;
using System.Collections.Generic;
using static System.Web.HttpUtility;
using System.Linq;
using System.Drawing;
using System.Web;
using System.Drawing.Imaging;
using System.Text;

namespace WhereEver.ClassLibrary
{
    public class CNNClass
    {


        public static string SetCNNFile(HttpPostedFile postedFile)
        {
            //PNG形式などの画像をBMP形式に変換
            byte[] buffer = new byte[postedFile.InputStream.Length];
            postedFile.InputStream.Read(buffer, 0, (int)postedFile.InputStream.Length);

            // C#ではFileStreamではなくMemoryStreamを使用する。
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(buffer);
            Bitmap bitmap = new Bitmap(memoryStream);

            //高さと横幅を保存
            //int bitmap_width = bitmap.Width;
            //int bitmap_height = bitmap.Height;

            //Binary取得
            //return ToBinaryByFixed(bitmap, 1).ToString();
            string str = HtmlEncode(ConvertBinaryToString(bitmap, 1));

            //必ず実行「後」にCloseする。そうしないとメモリ不足でエラーになる。
            memoryStream.Close();


            //150px*150pxなら、150px*4bit+150px*4bit=3000bit、区切り文字"-"が2999個になる。
            string[] sp = str.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);  //空白文字をトリム              
            StringBuilder sb = new StringBuilder();

            //256bit
            for (int i = 0; i < sp.Length; i++)
            {
                int num = 0;

                if (i > 0)
                {
                    if (i % (sp.Length / 3 / 10 -1) == 0)
                    {
                        sb.Append("\r\f");
                    }
                }


                num += Convert.ToInt32(sp[i], 16);      //R
                i++;
                num += Convert.ToInt32(sp[i], 16);      //G
                i++;
                num += Convert.ToInt32(sp[i], 16);      //B
                //i++;
                //num += Convert.ToInt32(sp[i], 16);    //輝度

                //FFに近ければ白
                //00に近ければ黒
                //256*3=768 /2=384
                if (num > 384)
                {
                    sb.Append("□");
                }
                else
                {
                    sb.Append("■");
                }

            }

            //-----------------------
            sb.Append("\r\f");
            sb.Append(str);

            //stringになったbyte配列を返す
            return sb.ToString();
        }

        //参照元：
        //https://nakatsudotnet.blog.fc2.com/blog-entry-19.html
        //ソース参照元(GitHub)：
        //https://github.com/MatsukichiNakaya/ColorToBinary


        /// <summary>
        /// 輝度計算
        /// </summary>
        /// <param name="r">赤</param>
        /// <param name="g">緑</param>
        /// <param name="b">青</param>
        /// <returns>輝度</returns>
        /// <remarks>
        /// 引数3つは、順不同でもよい。
        /// 単純にMaxとMinを取得しているだけなので。
        /// </remarks>
        private static Single GetBrightness(byte r, byte g, byte b)
        {
            Single max = r;
            Single min = r;

            if (max < g) { max = g; }
            if (max < b) { max = b; }

            if (g < min) { min = g; }
            if (b < min) { min = b; }

            return ((max / 255.0f) + (min / 255.0f)) / 2;
        }

        /// <summary>
        /// 2値化画像に変換(固定閾値法)
        /// </summary>
        /// <param name="bmpBase">画像データ</param>
        /// <param name="threshold">2値化閾値</param>
        /// <returns>画像データ</returns>
        public static Bitmap ToBinaryByFixed(Bitmap bmpBase, int threshold)
        {
            // 指定の閾値を使用して2値化を行う
            return ConvertBinary(bmpBase, threshold, (rgb, th, sz, st) => BinaryFixedConvert(rgb, th, sz, st));
        }

        /// <summary>
        /// 2値化画像に変換(オーダー法)
        /// </summary>
        /// <param name="bmpBase">元となる画像</param>
        /// <returns>画像データ</returns>
        public static Bitmap ToBinaryByOrdered(Bitmap bmpBase)
        {
            return ConvertBinary(bmpBase, 0, (rgb, th, sz, st) => BinaryOrderedConvert(rgb, sz, st));
        }

        /// <summary>
        /// 2値化画像に変換(差分法)
        /// </summary>
        /// <param name="bmpBase">元となる画像</param>
        /// <param name="threshold">閾値</param>
        /// <returns>画像データ</returns>
        public static Bitmap ToBinaryByDiff(Bitmap bmpBase, Int32 threshold)
        {
            return ConvertBinary(bmpBase, threshold, (rgb, th, sz, st) => BinaryDiffConvert(rgb, th, sz, st));
        }

        //--------------------------------------------------------------------------------------

        /// <summary>
        /// 固定閾値法の変換処理
        /// くっきりとした線を取得するため、輪郭線や機械学習などに向いています。
        /// </summary>
        /// <param name="rgbValues">色データ</param>
        /// <param name="threshold">閾値</param>
        /// <param name="bmpSize">画像サイズ</param>
        /// <param name="stride">画像読み込み幅</param>
        /// <returns>変換色データ</returns>
        private static byte[] BinaryFixedConvert(byte[] rgbValues, int threshold, Size bmpSize, int stride)
        {
            float br = 0f;
            float th = (threshold / 255.0f);
            // 2値化
            int c = 0;
            int pos = 0;
            byte[] result = new byte[stride * bmpSize.Height];

            for (int r = 0; r < bmpSize.Height; r++)
            {
                for (c = 0; c < bmpSize.Width; c++)
                {
                    br = GetBrightness(rgbValues[(r * bmpSize.Width * 4 + c * 4) + 2],  // r
                                       rgbValues[(r * bmpSize.Width * 4 + c * 4) + 1],  // g
                                       rgbValues[(r * bmpSize.Width * 4 + c * 4) + 0]); // b
                                                                                        // 閾値チェック
                    if (th < br)
                    {   // 色設定
                        pos = (c >> 3) + stride * r;
                        result[pos] |= (byte)(0x80 >> (c & 0x7));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// オーダードディザリングの変換処理
        /// ぼんやりした白黒画像を生成します。
        /// </summary>
        /// <param name="rgbValues">色データ</param>
        /// <param name="bmpSize">画像サイズ</param>
        /// <param name="stride">画像読み込み幅</param>
        /// <returns>変換色データ</returns>
        private static byte[] BinaryOrderedConvert(byte[] rgbValues, Size bmpSize, int stride)
        {
            var br = 0f;
            // 閾値マップを作成する
            var thMap = new float[4][]
            {
        new float[4] {1f/17f, 9f/17f, 3f/17f, 11f/17f},
        new float[4] {13f/17f, 5f/17f, 15f/17f, 7f/17f},
        new float[4] {4f/17f, 12f/17f, 2f/17f, 10f/17f},
        new float[4] {16f/17f, 8f/17f, 14f/17f, 6f/17f },
            };
            var c = 0;
            var pos = 0;
            var result = new byte[stride * bmpSize.Height];

            for (int r = 0; r < bmpSize.Height; r++)
            {
                for (c = 0; c < bmpSize.Width; c++)
                {
                    br = GetBrightness(rgbValues[(r * bmpSize.Width * 4 + c * 4) + 2],  // r
                                       rgbValues[(r * bmpSize.Width * 4 + c * 4) + 1],  // g
                                       rgbValues[(r * bmpSize.Width * 4 + c * 4) + 0]); // b
                    if (thMap[r % 4][c % 4] <= br)
                    {
                        // 色設定
                        pos = (c >> 3) + stride * r;
                        result[pos] |= (byte)(0x80 >> (c & 0x7));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 差分法の変換処理
        /// ぼんやりした白黒画像その２を生成します。
        /// </summary>
        /// <param name="rgbValues">色データ</param>
        /// <param name="threshold">閾値</param>
        /// <param name="bmpSize">画像サイズ</param>
        /// <param name="stride">画像読み込み幅</param>
        /// <returns>変換色データ</returns>
        private static Byte[] BinaryDiffConvert(byte[] rgbValues, int threshold, Size bmpSize, int stride)
        {
            var err = 0.0f;
            var th = threshold / 255.0f;
            var c = 0;
            var pos = 0;
            var result = new byte[stride * bmpSize.Height];
            //現在の行と次の行の誤差を記憶する配列
            var errors = new float[2][] {
                    new float[bmpSize.Width + 1],
                    new float[bmpSize.Width + 1]
                 };

            for (int r = 0; r < bmpSize.Height; r++)
            {
                for (c = 0; c < bmpSize.Width; c++)
                {
                    err = GetBrightness(rgbValues[(r * bmpSize.Width * 4 + c * 4) + 2], // r
                                        rgbValues[(r * bmpSize.Width * 4 + c * 4) + 1], // g
                                        rgbValues[(r * bmpSize.Width * 4 + c * 4) + 0]) // b
                                        + errors[0][c];
                    if (th <= err)
                    {
                        // 色設定
                        pos = (c >> 3) + stride * r;
                        result[pos] |= (byte)(0x80 >> (c & 0x7));
                        //誤差を計算（黒くした時の誤差はerr-0なので、そのまま）
                        err -= 1f;
                    }

                    //誤差を振り分ける
                    errors[0][c + 1] += err * 7f / 16f;
                    if (c > 0)
                    {
                        errors[1][c - 1] += err * 3f / 16f;
                    }
                    errors[1][c] += err * 5f / 16f;
                    errors[1][c + 1] += err * 1f / 16f;
                }
                //誤差を記憶した配列を入れ替える
                errors[0] = errors[1];
                errors[1] = new float[errors[0].Length];
            }
            return result;
        }

        //----------------------------------------------------------------------------------

        /// <summary>
        /// 2値化用変換関数
        /// </summary>
        /// <param name="bmpBase">元となる画像</param>
        /// <param name="threshold">閾値</param>
        /// <param name="converter">変換式</param>
        /// <returns></returns>
        private static Bitmap ConvertBinary(Bitmap bmpBase, int threshold, Func<byte[], int, Size, int, byte[]> converter)
        {
            Rectangle rect = new Rectangle(0, 0, bmpBase.Width, bmpBase.Height);
            Bitmap result = new Bitmap(rect.Width, rect.Height, PixelFormat.Format1bppIndexed);

            byte[] rgbValues;
            // ベースのARGBデータ取得
            using (Bitmap bmp = bmpBase.Clone(rect, PixelFormat.Format32bppArgb))
            {
                BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);
                IntPtr ptr = bmpData.Scan0;
                int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
                rgbValues = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
                bmp.UnlockBits(bmpData);
            }

            // 変換後の2値データを作成
            BitmapData bmpResultData = result.LockBits(rect, ImageLockMode.WriteOnly, result.PixelFormat);
            // データをもとに計算
            byte[] resultValues = converter(rgbValues, threshold, result.Size, bmpResultData.Stride);
            // 計算結果を画像に反映させる
            IntPtr ptrRet = bmpResultData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(resultValues, 0, ptrRet, resultValues.Length);

            bmpBase.Dispose();
            result.UnlockBits(bmpResultData);
            return result;
        }


        /// <summary>
        /// 2値化用変換関数
        /// </summary>
        /// <param name="bmpBase">元となる画像</param>
        /// <param name="threshold">閾値</param>
        /// <param name="converter">変換式</param>
        /// <returns></returns>
        private static string ConvertBinaryToString(Bitmap bmpBase, int threshold)
        {
            Rectangle rect = new Rectangle(0, 0, bmpBase.Width, bmpBase.Height);
            Bitmap result = new Bitmap(rect.Width, rect.Height, PixelFormat.Format1bppIndexed);

            byte[] rgbValues;
            // ベースのARGBデータ取得
            using (Bitmap bmp = bmpBase.Clone(rect, PixelFormat.Format32bppArgb))
            {
                BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);
                IntPtr ptr = bmpData.Scan0;
                int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
                rgbValues = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
                bmp.UnlockBits(bmpData);
            }

            // 変換後の2値データを作成
            BitmapData bmpResultData = result.LockBits(rect, ImageLockMode.WriteOnly, result.PixelFormat);
            // データをもとに計算
            byte[] resultValues = BinaryFixedConvert(rgbValues, threshold, result.Size, bmpResultData.Stride);

            bmpBase.Dispose();
            result.UnlockBits(bmpResultData);

            return BitConverter.ToString(resultValues);
        }





    }
}