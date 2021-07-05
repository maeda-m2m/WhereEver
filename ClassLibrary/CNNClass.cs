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

        // やること：

        // 数字１字(150px*150px)の描かれた画像データをbitに直して、入力されたbitとAND演算する。
        // ReLU関数（かSoftMax）により、一致率が高い数字１字を演算する。
        // 部分認識はbitの仕様上、素のC#では難しいため、このような仕様をとる。


        //画像縮小エラーフラグ trueなら画像縮小エラー
        private static bool errorFlag = false;


        public static string SetCNNFile(HttpPostedFile postedFile)
        {
            //init
            CNNClass.errorFlag = false;

            //PNG形式などの画像をBMP形式に変換
            byte[] buffer = new byte[postedFile.InputStream.Length];
            postedFile.InputStream.Read(buffer, 0, (int)postedFile.InputStream.Length);

            // C#ではFileStreamではなくMemoryStreamを使用する。
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(buffer);
            Bitmap bitmap = new Bitmap(memoryStream);

            //インデックスの範囲外にあるとコンパイルエラーになるため、セーフティーをつける。縦横比はなるべく同じにする。
            const int width = 128;
            const int height = width;
            if(bitmap.Width > 16 && bitmap.Height > 16)
            {
                //BMPを高精度バイキュービック法でリサイズ（モノクロ8bps以外はカラー扱いされる）
                bitmap = CNNClass.ResizeBitmap(bitmap, width, height, System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);
            }
            else
            {
                return "エラー：画像サイズが小さすぎます。縦16*横16px以上にして下さい。";
            }

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
            StringBuilder sb2 = new StringBuilder();

            if (CNNClass.errorFlag == true)
            {
                sb.Append("警告：画像サイズの縮尺比が不正です。正常に縮小できませんでした。\r\f");
            }


            //0(-1)と1(+1)の合計値SUM
            List<int> sum = new List<int>();
            //List Index
            int sumId = 0;
            sum.Add(0);
            sum.Add(0);
            sum.Add(0);
            sum.Add(0);
            sum.Add(0);
            sum.Add(0);
            sum.Add(0);
            sum.Add(0);
            sum.Add(0);
            sum.Add(0);
            sum.Add(0);
            sum.Add(0);
            sum.Add(0);
            sum.Add(0);
            sum.Add(0);
            sum.Add(0);

            //DataBase
            List<int> sum0 = new List<int>();
            sum0.Add(-128);
            sum0.Add(-128);
            sum0.Add(-118);
            sum0.Add(-118);
            sum0.Add(-120);
            sum0.Add(-122);
            sum0.Add(-128);
            sum0.Add(-122);
            sum0.Add(-122);
            sum0.Add(-128);
            sum0.Add(-120);
            sum0.Add(-124);
            sum0.Add(-124);
            sum0.Add(-114);
            sum0.Add(-128);
            sum0.Add(-128);

            List<int> sum1 = new List<int>();
            sum1.Add(-128);
            sum1.Add(-128);
            sum1.Add(-122);
            sum1.Add(-124);
            sum1.Add(-128);
            sum1.Add(-128);
            sum1.Add(-128);
            sum1.Add(-128);
            sum1.Add(-112);
            sum1.Add(-126);
            sum1.Add(-128);
            sum1.Add(-128);
            sum1.Add(-128);
            sum1.Add(-128);
            sum1.Add(-128);
            sum1.Add(-128);

            List<int> sum2 = new List<int>();
            sum2.Add(-128);
            sum2.Add(-128);
            sum2.Add(-120);
            sum2.Add(-122);
            sum2.Add(-124);
            sum2.Add(-124);
            sum2.Add(-124);
            sum2.Add(-124);
            sum2.Add(-126);
            sum2.Add(-124);
            sum2.Add(-124);
            sum2.Add(-118);
            sum2.Add(-110);
            sum2.Add(-128);
            sum2.Add(-128);
            sum2.Add(-128);

            List<int> sum3 = new List<int>();
            sum3.Add(-128);
            sum3.Add(-128);
            sum3.Add(-118);
            sum3.Add(-128);
            sum3.Add(-122);
            sum3.Add(-124);
            sum3.Add(-126);
            sum3.Add(-120);
            sum3.Add(-106);
            sum3.Add(-128);
            sum3.Add(-114);
            sum3.Add(-112);
            sum3.Add(-120);
            sum3.Add(-124);
            sum3.Add(-114);
            sum3.Add(-128);

            List<int> sum4 = new List<int>();
            sum4.Add(-128);
            sum4.Add(-128);
            sum4.Add(-124);
            sum4.Add(-122);
            sum4.Add(-124);
            sum4.Add(-126);
            sum4.Add(-112);
            sum4.Add(-128);
            sum4.Add(-128);
            sum4.Add(-128);
            sum4.Add(-118);
            sum4.Add(-128);
            sum4.Add(-128);
            sum4.Add(-128);
            sum4.Add(-128);
            sum4.Add(-128);

            List<int> sum5 = new List<int>();
            sum5.Add(-128);
            sum5.Add(-128);
            sum5.Add(-128);
            sum5.Add(-118);
            sum5.Add(-128);
            sum5.Add(-124);
            sum5.Add(-126);
            sum5.Add(-124);
            sum5.Add(-120);
            sum5.Add(-116);
            sum5.Add(-112);
            sum5.Add(-116);
            sum5.Add(-120);
            sum5.Add(-128);
            sum5.Add(-128);
            sum5.Add(-128);

            List<int> sum6 = new List<int>();
            sum6.Add(-128);
            sum6.Add(-128);
            sum6.Add(-126);
            sum6.Add(-126);
            sum6.Add(-122);
            sum6.Add(-126);
            sum6.Add(-128);
            sum6.Add(-122);
            sum6.Add(-128);
            sum6.Add(-114);
            sum6.Add(-128);
            sum6.Add(-112);
            sum6.Add(-110);
            sum6.Add(-122);
            sum6.Add(-116);
            sum6.Add(-128);

            List<int> sum7 = new List<int>();
            sum7.Add(-128);
            sum7.Add(-128);
            sum7.Add(-128);
            sum7.Add(-118);
            sum7.Add(-122);
            sum7.Add(-122);
            sum7.Add(-124);
            sum7.Add(-128);
            sum7.Add(-126);
            sum7.Add(-126);
            sum7.Add(-128);
            sum7.Add(-128);
            sum7.Add(-120);
            sum7.Add(-120);
            sum7.Add(-128);
            sum7.Add(-128);

            List<int> sum8 = new List<int>();
            sum8.Add(-128);
            sum8.Add(-128);
            sum8.Add(-128);
            sum8.Add(-124);
            sum8.Add(-122);
            sum8.Add(-128);
            sum8.Add(-120);
            sum8.Add(-122);
            sum8.Add(-120);
            sum8.Add(-124);
            sum8.Add(-122);
            sum8.Add(-122);
            sum8.Add(-114);
            sum8.Add(-128);
            sum8.Add(-128);
            sum8.Add(-128);

            List<int> sum9 = new List<int>();
            sum9.Add(-128);
            sum9.Add(-128);
            sum9.Add(-120);
            sum9.Add(-120);
            sum9.Add(-128);
            sum9.Add(-126);
            sum9.Add(-108);
            sum9.Add(-128);
            sum9.Add(-122);
            sum9.Add(-128);
            sum9.Add(-124);
            sum9.Add(-128);
            sum9.Add(-124);
            sum9.Add(-120);
            sum9.Add(-128);
            sum9.Add(-128);

            //0～9
            List<int> sumAnswer = new List<int>();
            sumAnswer.Add(0);
            sumAnswer.Add(0);
            sumAnswer.Add(0);
            sumAnswer.Add(0);
            sumAnswer.Add(0);
            sumAnswer.Add(0);
            sumAnswer.Add(0);
            sumAnswer.Add(0);
            sumAnswer.Add(0);
            sumAnswer.Add(0);

            //init
            int sumMax = 0;

            //256bit
            for (int i = 0; i < sp.Length; i++)
            {
                //初期化
                int num = 0;

                /*
                if (i > 0)
                {
                    //例えば、縦16*横16なら64bit、if((i) % 8 == 0)にする（小さすぎて差が出ない）。32*32なら=128bit、64*64なら=512bit(*2 /8で横長)、128*128なら=2048bit(/2 /8で縦長)、いずれも√で余りが出る。√1024なら32になる。
                    if ((i) % (sp.Length /2 / 8) == 0)
                    {
                        sb.Append("\r\f");
                    }
                }
                */

                if (i > 0)
                {
                    if ((i) % (sp.Length /2 /8) == 0)
                    {
                        sumId += 1;
                    }
                }


                    num = Convert.ToInt32(sp[i], 16);      //RGB256(モノクロ変換後)

                //num += Convert.ToInt32(sp[i], 16);      //R
                //i++;
                //num += Convert.ToInt32(sp[i], 16);      //G
                //i++;
                //num += Convert.ToInt32(sp[i], 16);      //B
                //i++;
                //num += Convert.ToInt32(sp[i], 16);    //輝度


                //RGB256モノクロ形式
                //FFに近ければ白
                //00に近ければ黒
                //256 /2 = 128
                if (num > 128)
                {
                    //sb.Append("□");
                    //sb2.Append("0");
                    sum[sumId] -= 1;
                }
                else
                {
                    //sb.Append("■");
                    //sb2.Append("1");
                    sum[sumId] += 1;
                }

                /*
                //R+G+B(+輝度)形式
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
                */

            }

            //-----------------------
            sb.Append("\r\f");
            sb.Append(sb2.ToString());
            sb.Append("\r\f");

            for (int k =0; k < sum.Count; k++)
            {
                sb.Append("SUM[");
                sb.Append(k);
                sb.Append("] = ");
                sb.Append(sum[k]);
                sb.Append("\r\f");

                //差の絶対値で上書き（変数の使いまわし）
                sum0[k] = Math.Abs(sum[k] - sum0[k]);
                sum1[k] = Math.Abs(sum[k] - sum1[k]);
                sum2[k] = Math.Abs(sum[k] - sum2[k]);
                sum3[k] = Math.Abs(sum[k] - sum3[k]);
                sum4[k] = Math.Abs(sum[k] - sum4[k]);
                sum5[k] = Math.Abs(sum[k] - sum5[k]);
                sum6[k] = Math.Abs(sum[k] - sum6[k]);
                sum7[k] = Math.Abs(sum[k] - sum7[k]);
                sum8[k] = Math.Abs(sum[k] - sum8[k]);
                sum9[k] = Math.Abs(sum[k] - sum9[k]);

                //最小の差を算出
                sumMax = Math.Min(sum0[k], sum1[k]);
                sumMax = Math.Min(sumMax, sum2[k]);
                sumMax = Math.Min(sumMax, sum3[k]);
                sumMax = Math.Min(sumMax, sum4[k]);
                sumMax = Math.Min(sumMax, sum5[k]);
                sumMax = Math.Min(sumMax, sum6[k]);
                sumMax = Math.Min(sumMax, sum7[k]);
                sumMax = Math.Min(sumMax, sum8[k]);
                sumMax = Math.Min(sumMax, sum9[k]);
                sumMax = Math.Min(sumMax, sum0[k]);

                if (sumMax >= sum0[k])
                {
                    sumAnswer[0] += 1;
                }
                if (sumMax >= sum1[k])
                {
                    sumAnswer[1] += 1;
                }
                if (sumMax >= sum2[k])
                {
                    sumAnswer[2] += 1;
                }
                if (sumMax >= sum3[k])
                {
                    sumAnswer[3] += 1;
                }
                if (sumMax >= sum4[k])
                {
                    sumAnswer[4] += 1;
                }
                if (sumMax >= sum5[k])
                {
                    sumAnswer[5] += 1;
                }
                if (sumMax >= sum6[k])
                {
                    sumAnswer[6] += 1;
                }
                if (sumMax >= sum7[k])
                {
                    sumAnswer[7] += 1;
                }
                if (sumMax >= sum8[k])
                {
                    sumAnswer[8] += 1;
                }
                if (sumMax >= sum9[k])
                {
                    sumAnswer[9] += 1;
                }



            }

            //最大点数計算
            sumMax = Math.Max(sumAnswer[9], sumAnswer[0]);
            sumMax = Math.Max(sumMax, sumAnswer[1]);
            sumMax = Math.Max(sumMax, sumAnswer[2]);
            sumMax = Math.Max(sumMax, sumAnswer[3]);
            sumMax = Math.Max(sumMax, sumAnswer[4]);
            sumMax = Math.Max(sumMax, sumAnswer[5]);
            sumMax = Math.Max(sumMax, sumAnswer[6]);
            sumMax = Math.Max(sumMax, sumAnswer[7]);
            sumMax = Math.Max(sumMax, sumAnswer[8]);
            sumMax = Math.Max(sumMax, sumAnswer[9]);

            sb.Append("\r\f");
            sb.Append("Result_Num = ");

            for(int p=0; p<10; p++)
            {
                if (sumMax <= sumAnswer[p])
                {
                    sb.Append(p);
                    sb.Append("(");
                    sb.Append(sumAnswer[p]);
                    sb.Append("pts.)");
                    sb.Append("\r\f");
                }
            }


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




        /// <summary>
        /// Bitmap画像データのリサイズ
        /// ソースコード参照元：https://imagingsolution.net/program/csharp/rezise_bitmap_data/
        /// </summary>
        /// <param name="original">元のBitmapクラスオブジェクト</param>
        /// <param name="width">リサイズ後の幅</param>
        /// <param name="height">リサイズ後の高さ</param>
        /// <param name="interpolationMode">補間モード</param>
        /// <returns>リサイズされたBitmap</returns>
        private static Bitmap ResizeBitmap(Bitmap original, int width, int height, System.Drawing.Drawing2D.InterpolationMode interpolationMode)
        {
            Bitmap bmpResize;
            Bitmap bmpResizeColor;
            Graphics graphics = null;

            try
            {
                System.Drawing.Imaging.PixelFormat pf = original.PixelFormat;

                if (original.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                {
                    // モノクロの時は仮に24bitとする
                    pf = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
                }

                bmpResizeColor = new Bitmap(width, height, pf);
                var dstRect = new RectangleF(0, 0, width, height);
                var srcRect = new RectangleF(-0.5f, -0.5f, original.Width, original.Height);
                graphics = Graphics.FromImage(bmpResizeColor);
                graphics.Clear(Color.Transparent);
                graphics.InterpolationMode = interpolationMode;
                graphics.DrawImage(original, dstRect, srcRect, GraphicsUnit.Pixel);

            }
            finally
            {
                if (graphics != null)
                {
                    graphics.Dispose();
                }
            }

            if (original.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            {
                // モノクロ画像のとき、24bit→8bitへ変換

                // モノクロBitmapを確保
                bmpResize = new Bitmap(
                    bmpResizeColor.Width,
                    bmpResizeColor.Height,
                    System.Drawing.Imaging.PixelFormat.Format8bppIndexed
                    );

                var pal = bmpResize.Palette;
                for (int i = 0; i < bmpResize.Palette.Entries.Length; i++)
                {
                    try
                    {
                        pal.Entries[i] = original.Palette.Entries[i];
                    }
                    catch
                    {
                        //サイズによっては境界外になることがある。その場合は無視。
                        CNNClass.errorFlag = true;
                    }
                }
                bmpResize.Palette = pal;

                // カラー画像のポインタへアクセス
                var bmpDataColor = bmpResizeColor.LockBits(
                        new Rectangle(0, 0, bmpResizeColor.Width, bmpResizeColor.Height),
                        System.Drawing.Imaging.ImageLockMode.ReadWrite,
                        bmpResizeColor.PixelFormat
                        );

                // モノクロ画像のポインタへアクセス
                var bmpDataMono = bmpResize.LockBits(
                        new Rectangle(0, 0, bmpResize.Width, bmpResize.Height),
                        System.Drawing.Imaging.ImageLockMode.ReadWrite,
                        bmpResize.PixelFormat
                        );

                int colorStride = bmpDataColor.Stride;
                int monoStride = bmpDataMono.Stride;

                //byte配列型ポインタを使用しないように改造 (byte*)→(byte)
                //unsafe
                //{
                byte[] pColor = BitConverter.GetBytes((byte)bmpDataColor.Scan0);
                byte[] pMono = BitConverter.GetBytes((byte)bmpDataMono.Scan0);
                for (int y = 0; y < bmpDataColor.Height; y++)
                {
                    for (int x = 0; x < bmpDataColor.Width; x++)
                    {
                        try
                        {
                            // R,G,B同じ値のため、Bの値を代表してモノクロデータへ代入
                            pMono[x + y * monoStride] = pColor[x * 3 + y * colorStride];
                        }
                        catch
                        {
                            //サイズによっては境界外になることがある。その場合は無視。
                            CNNClass.errorFlag = true;
                        }
                    }
                }
                //}

                bmpResize.UnlockBits(bmpDataMono);
                bmpResizeColor.UnlockBits(bmpDataColor);

                //　解放
                bmpResizeColor.Dispose();
            }
            else
            {
                // カラー画像のとき
                bmpResize = bmpResizeColor;
            }

            return bmpResize;
        }



    }
}