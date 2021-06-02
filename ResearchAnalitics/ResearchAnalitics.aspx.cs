﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using static System.Web.HttpUtility;
using static WhereEver.ClassLibrary.Matrix;

namespace WhereEver.ResearchAnalitics
{
    public partial class ResearchAnalitics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            TextBox_Soukan_id.Text = SessionManager.User.M_User.id;



        }



        protected void RunMatrix()
        {
            // yの値は入力値xの値と未知数のw, bの値からなっており、
            // 出力値と目標値の差の二乗の合計の値が最小になるようにw, bの値を求める（最小二乗法＆勾配降下法）。
            // default X={ 6.3, 10.4, 11.1, 16.4 }

            string text_X1 = HtmlEncode(TextBox_X1.Text);
            double.TryParse(text_X1, System.Globalization.NumberStyles.Currency, null, out double X_1);
            string text_X2 = HtmlEncode(TextBox_X2.Text);
            double.TryParse(text_X2, System.Globalization.NumberStyles.Currency, null, out double X_2);
            string text_X3 = HtmlEncode(TextBox_X3.Text);
            double.TryParse(text_X3, System.Globalization.NumberStyles.Currency, null, out double X_3);
            string text_X4 = HtmlEncode(TextBox_X4.Text);
            double.TryParse(text_X4, System.Globalization.NumberStyles.Currency, null, out double X_4);

            string text_W1_1_1 = HtmlEncode(TextBox_W1_1_1.Text);
            double.TryParse(text_W1_1_1, System.Globalization.NumberStyles.Currency, null, out double W1_1_1);
            string text_W1_1_2 = HtmlEncode(TextBox_W1_1_2.Text);
            double.TryParse(text_W1_1_2, System.Globalization.NumberStyles.Currency, null, out double W1_1_2);
            string text_W1_1_3 = HtmlEncode(TextBox_W1_1_3.Text);
            double.TryParse(text_W1_1_3, System.Globalization.NumberStyles.Currency, null, out double W1_1_3);
            string text_W1_1_4 = HtmlEncode(TextBox_W1_1_4.Text);
            double.TryParse(text_W1_1_4, System.Globalization.NumberStyles.Currency, null, out double W1_1_4);
            string text_W1_1_5 = HtmlEncode(TextBox_W1_1_5.Text);
            double.TryParse(text_W1_1_5, System.Globalization.NumberStyles.Currency, null, out double W1_1_5);

            string text_W1_2_1 = HtmlEncode(TextBox_W1_2_1.Text);
            double.TryParse(text_W1_2_1, System.Globalization.NumberStyles.Currency, null, out double W1_2_1);
            string text_W1_2_2 = HtmlEncode(TextBox_W1_2_2.Text);
            double.TryParse(text_W1_2_2, System.Globalization.NumberStyles.Currency, null, out double W1_2_2);
            string text_W1_2_3 = HtmlEncode(TextBox_W1_2_3.Text);
            double.TryParse(text_W1_2_3, System.Globalization.NumberStyles.Currency, null, out double W1_2_3);
            string text_W1_2_4 = HtmlEncode(TextBox_W1_2_4.Text);
            double.TryParse(text_W1_2_4, System.Globalization.NumberStyles.Currency, null, out double W1_2_4);
            string text_W1_2_5 = HtmlEncode(TextBox_W1_2_5.Text);
            double.TryParse(text_W1_2_5, System.Globalization.NumberStyles.Currency, null, out double W1_2_5);

            string text_W1_3_1 = HtmlEncode(TextBox_W1_3_1.Text);
            double.TryParse(text_W1_3_1, System.Globalization.NumberStyles.Currency, null, out double W1_3_1);
            string text_W1_3_2 = HtmlEncode(TextBox_W1_3_2.Text);
            double.TryParse(text_W1_3_2, System.Globalization.NumberStyles.Currency, null, out double W1_3_2);
            string text_W1_3_3 = HtmlEncode(TextBox_W1_3_3.Text);
            double.TryParse(text_W1_3_3, System.Globalization.NumberStyles.Currency, null, out double W1_3_3);
            string text_W1_3_4 = HtmlEncode(TextBox_W1_3_4.Text);
            double.TryParse(text_W1_3_4, System.Globalization.NumberStyles.Currency, null, out double W1_3_4);
            string text_W1_3_5 = HtmlEncode(TextBox_W1_3_5.Text);
            double.TryParse(text_W1_3_5, System.Globalization.NumberStyles.Currency, null, out double W1_3_5);

            string text_W1_4_1 = HtmlEncode(TextBox_W1_4_1.Text);
            double.TryParse(text_W1_4_1, System.Globalization.NumberStyles.Currency, null, out double W1_4_1);
            string text_W1_4_2 = HtmlEncode(TextBox_W1_4_2.Text);
            double.TryParse(text_W1_4_2, System.Globalization.NumberStyles.Currency, null, out double W1_4_2);
            string text_W1_4_3 = HtmlEncode(TextBox_W1_4_3.Text);
            double.TryParse(text_W1_4_3, System.Globalization.NumberStyles.Currency, null, out double W1_4_3);
            string text_W1_4_4 = HtmlEncode(TextBox_W1_4_4.Text);
            double.TryParse(text_W1_4_4, System.Globalization.NumberStyles.Currency, null, out double W1_4_4);
            string text_W1_4_5 = HtmlEncode(TextBox_W1_4_5.Text);
            double.TryParse(text_W1_4_5, System.Globalization.NumberStyles.Currency, null, out double W1_4_5);

            string text_B1_1 = HtmlEncode(TextBox_B1_1.Text);
            double.TryParse(text_B1_1, System.Globalization.NumberStyles.Currency, null, out double B1_1);
            string text_B1_2 = HtmlEncode(TextBox_B1_2.Text);
            double.TryParse(text_B1_2, System.Globalization.NumberStyles.Currency, null, out double B1_2);
            string text_B1_3 = HtmlEncode(TextBox_B1_3.Text);
            double.TryParse(text_B1_3, System.Globalization.NumberStyles.Currency, null, out double B1_3);
            string text_B1_4 = HtmlEncode(TextBox_B1_4.Text);
            double.TryParse(text_B1_4, System.Globalization.NumberStyles.Currency, null, out double B1_4);
            string text_B1_5 = HtmlEncode(TextBox_B1_5.Text);
            double.TryParse(text_B1_5, System.Globalization.NumberStyles.Currency, null, out double B1_5);

            string text_W2_1_1 = HtmlEncode(TextBox_W2_1_1.Text);
            double.TryParse(text_W2_1_1, System.Globalization.NumberStyles.Currency, null, out double W2_1_1);
            string text_W2_1_2 = HtmlEncode(TextBox_W2_1_2.Text);
            double.TryParse(text_W2_1_2, System.Globalization.NumberStyles.Currency, null, out double W2_1_2);

            string text_W2_2_1 = HtmlEncode(TextBox_W2_2_1.Text);
            double.TryParse(text_W2_2_1, System.Globalization.NumberStyles.Currency, null, out double W2_2_1);
            string text_W2_2_2 = HtmlEncode(TextBox_W2_2_2.Text);
            double.TryParse(text_W2_2_2, System.Globalization.NumberStyles.Currency, null, out double W2_2_2);

            string text_W2_3_1 = HtmlEncode(TextBox_W2_3_1.Text);
            double.TryParse(text_W2_3_1, System.Globalization.NumberStyles.Currency, null, out double W2_3_1);
            string text_W2_3_2 = HtmlEncode(TextBox_W2_3_2.Text);
            double.TryParse(text_W2_3_2, System.Globalization.NumberStyles.Currency, null, out double W2_3_2);

            string text_W2_4_1 = HtmlEncode(TextBox_W2_4_1.Text);
            double.TryParse(text_W2_4_1, System.Globalization.NumberStyles.Currency, null, out double W2_4_1);
            string text_W2_4_2 = HtmlEncode(TextBox_W2_4_2.Text);
            double.TryParse(text_W2_4_2, System.Globalization.NumberStyles.Currency, null, out double W2_4_2);

            string text_W2_5_1 = HtmlEncode(TextBox_W2_5_1.Text);
            double.TryParse(text_W2_5_1, System.Globalization.NumberStyles.Currency, null, out double W2_5_1);
            string text_W2_5_2 = HtmlEncode(TextBox_W2_5_2.Text);
            double.TryParse(text_W2_5_2, System.Globalization.NumberStyles.Currency, null, out double W2_5_2);

            string text_B2_1 = HtmlEncode(TextBox_B2_1.Text);
            double.TryParse(text_B2_1, System.Globalization.NumberStyles.Currency, null, out double B2_1);
            string text_B2_2 = HtmlEncode(TextBox_B2_2.Text);
            double.TryParse(text_B2_2, System.Globalization.NumberStyles.Currency, null, out double B2_2);

            string text_W3_1_1 = HtmlEncode(TextBox_W3_1_1.Text);
            double.TryParse(text_W3_1_1, System.Globalization.NumberStyles.Currency, null, out double W3_1_1);
            string text_W3_1_2 = HtmlEncode(TextBox_W3_1_2.Text);
            double.TryParse(text_W3_1_2, System.Globalization.NumberStyles.Currency, null, out double W3_1_2);
            string text_W3_1_3 = HtmlEncode(TextBox_W3_1_3.Text);
            double.TryParse(text_W3_1_3, System.Globalization.NumberStyles.Currency, null, out double W3_1_3);
            string text_W3_1_4 = HtmlEncode(TextBox_W3_1_4.Text);
            double.TryParse(text_W3_1_4, System.Globalization.NumberStyles.Currency, null, out double W3_1_4);

            string text_W3_2_1 = HtmlEncode(TextBox_W3_2_1.Text);
            double.TryParse(text_W3_2_1, System.Globalization.NumberStyles.Currency, null, out double W3_2_1);
            string text_W3_2_2 = HtmlEncode(TextBox_W3_2_2.Text);
            double.TryParse(text_W3_2_2, System.Globalization.NumberStyles.Currency, null, out double W3_2_2);
            string text_W3_2_3 = HtmlEncode(TextBox_W3_2_3.Text);
            double.TryParse(text_W3_2_3, System.Globalization.NumberStyles.Currency, null, out double W3_2_3);
            string text_W3_2_4 = HtmlEncode(TextBox_W3_2_4.Text);
            double.TryParse(text_W3_2_4, System.Globalization.NumberStyles.Currency, null, out double W3_2_4);

            string text_B3_1 = HtmlEncode(TextBox_B3_1.Text);
            double.TryParse(text_B3_1, System.Globalization.NumberStyles.Currency, null, out double B3_1);
            string text_B3_2 = HtmlEncode(TextBox_B3_2.Text);
            double.TryParse(text_B3_2, System.Globalization.NumberStyles.Currency, null, out double B3_2);
            string text_B3_3 = HtmlEncode(TextBox_B3_3.Text);
            double.TryParse(text_B3_3, System.Globalization.NumberStyles.Currency, null, out double B3_3);
            string text_B3_4 = HtmlEncode(TextBox_B3_4.Text);
            double.TryParse(text_B3_4, System.Globalization.NumberStyles.Currency, null, out double B3_4);

            string text_T = HtmlEncode(TextBox_T.Text);
            double.TryParse(text_T, System.Globalization.NumberStyles.Currency, null, out double T_3);

            // 入力値 X
            Mat X = new Mat(
                new double[] { X_1, X_2, X_3, X_4 }
                );

                //---------------------------------------------------------------------------------------------------------
                // 未知数 w, b
                // ここに求めたい値を入れます。

                Mat W1 = new Mat(
                new double[] { W1_1_1, W1_1_2, W1_1_3, W1_1_4, W1_1_5 },
                new double[] { W1_2_1, W1_2_2, W1_2_3, W1_2_4, W1_2_5 },
                new double[] { W1_3_1, W1_3_2, W1_3_3, W1_3_4, W1_3_5 },
                new double[] { W1_4_1, W1_4_2, W1_4_3, W1_4_4, W1_4_5 }
                );

                Mat B1 = new Mat(
                new double[] { B1_1, B1_2, B1_3, B1_4, B1_5 }
                );

                Mat W2 = new Mat(
                new double[] { W2_1_1, W2_1_2 },
                new double[] { W2_2_1, W2_2_2 },
                new double[] { W2_3_1, W2_3_2 },
                new double[] { W2_4_1, W2_4_2 },
                new double[] { W2_5_1, W2_5_2 }
                );

                Mat B2 = new Mat(
                new double[] { B2_1, B2_2 }
                );

                Mat W3 = new Mat(
                new double[] { W3_1_1, W3_1_2, W3_1_3, W3_1_4 },
                new double[] { W3_2_1, W3_2_2, W3_2_3, W3_2_4 }
                );

                Mat B3 = new Mat(
                new double[] { B3_1, B3_2, B3_3, B3_4 }
                );

                //---------------------------------------------------------------------------------------------------------
                // 以下はconstやreadonlyにしてもよいが、一応代入できるようにしておく

                // 教師信号？（配列Tの左から３番目？）デフォルトは{0,0,1,0}
                // 「教師信号は、以前は0や1を使っていましたが、シグモイド関数では重みが発散しないように0.1や0.9を代わりに用いることが多いです。」
                // 参考：https://rightcode.co.jp/blog/information-technology/back-propagation-algorithm-implementation
                Mat T = new Mat(
                new double[] { 0, 0, T_3, 0 }
                );

                Mat A1 = new Mat(
                new double[] { 0.0, 0.0, 0.0, 0.0, 0.0 }
                );

                Mat Z1 = new Mat(
                new double[] { 0.0, 0.0, 0.0, 0.0, 0.0 }
                );

                Mat A2 = new Mat(
                new double[] { 0.0, 0.0 }
                );

                Mat Z2 = new Mat(
                new double[] { 0.0, 0.0 }
                );

                Mat A3 = new Mat(
                new double[] { 0.0, 0.0, 0.0, 0.0 }
                );

                Mat Y = new Mat(
                new double[] { 0.0, 0.0, 0.0, 0.0 }
                );

            string learn = HtmlEncode(TextBox_Learn.Text);
            string rate = HtmlEncode(TextBox_Rate.Text);
            int.TryParse(learn, System.Globalization.NumberStyles.Currency, null, out int value1);
            double.TryParse(rate, System.Globalization.NumberStyles.Currency, null, out double value2);

            int isdeep = 1;

            //※本来はタスクかコンソールで行うほうがよい。
            TextBox_DeepResult.Text = Master(X, W1, B1, W2, B2, W3, B3, T, A1, Z1, A2, Z2, A3, Y, isdeep, value1, value2);
        }


        //----------------------------------------------------
        //テスト用配列（読み取り専用）
/*
                private static readonly Mat X = new Mat(
                new double[] { 6.3, 10.4, 11.1, 16.4 }
                );

                private static readonly Mat W1 = new Mat(
                new double[] { 4.0, 2.0, 6.0, 1.0, 5.0 },
                new double[] { 1.0, 3.0, 7.0, 2.0, 1.0 },
                new double[] { 4.0, 2.0, 6.0, 1.0, 5.0 },
                new double[] { 1.0, 3.0, 7.0, 2.0, 1.0 }
                );

                private static readonly Mat B1 = new Mat(
                new double[] { 2.0, 6.0, 4.0, 4.0, 1.0 }
                );

                private static readonly Mat W2 = new Mat(
                new double[] { 3.0, 5.0 },
                new double[] { 6.0, 3.0 },
                new double[] { 1.0, 5.0 },
                new double[] { 3.0, 5.0 },
                new double[] { 6.0, 3.0 }
                );

                private static readonly Mat B2 = new Mat(
                new double[] { 3.0, 6.0 }
                );

                private static readonly Mat W3 = new Mat(
                new double[] { 7.0, 5.0, 7.0, 6.0 },
                new double[] { 4.0, 2.0, 4.0, 2.0 }
                );

                private static readonly Mat B3 = new Mat(
                new double[] { 4.0, 6.0, 5.0, 4.0 }
                );

                private static readonly Mat T = new Mat(
                new double[] { 0, 0, 1, 0 }
                );

                private static readonly Mat A1 = new Mat(
                new double[] { 0.0, 0.0, 0.0, 0.0, 0.0 }
                );

                private static readonly Mat Z1 = new Mat(
                new double[] { 0.0, 0.0, 0.0, 0.0, 0.0 }
                );

                private static readonly Mat A2 = new Mat(
                new double[] { 0.0, 0.0 }
                );

                private static readonly Mat Z2 = new Mat(
                new double[] { 0.0, 0.0 }
                );

                private static readonly Mat A3 = new Mat(
                new double[] { 0.0, 0.0, 0.0, 0.0 }
                );

                private static readonly Mat Y = new Mat(
                new double[] { 0.0, 0.0, 0.0, 0.0 }
                );
*/
        //----------------------------------------------------

        static double Loss(Mat X, Mat W1, Mat B1, Mat W2, Mat B2, Mat W3, Mat B3, Mat T, Mat A1, Mat Z1, Mat A2, Mat Z2, Mat A3, Mat Y)     //交差エントロピー誤差を求める
        {
            A1.Matrix_data = Mat.dot(X, W1) + B1;
            Z1.Matrix_data = A1.Sigmoid();
            A2.Matrix_data = Mat.dot(Z1, W2) + B2;
            Z2.Matrix_data = A2.Sigmoid();
            A3.Matrix_data = Mat.dot(Z2, W3) + B3;
            Y.Matrix_data = A3.Softmax();

            return Y.Cross_etp_err(T);
        }
        static Mat Loss_forward(Mat Y, Mat T)     //誤差逆伝播時に使用する　交差エントロピー誤差とソフトマックス関数の微分
        {
            Mat dx = new Mat(
                new double[] { 0.0, 0.0, 0.0, 0.0 }
                );

            dx.Matrix_data = Y - T;
            dx.Matrix_data = dx / 2;

            return dx;
        }

        protected static new string Master(Mat X, Mat W1, Mat B1, Mat W2, Mat B2, Mat W3, Mat B3, Mat T, Mat A1, Mat Z1, Mat A2, Mat Z2, Mat A3, Mat Y, int deeplearning = 1, int learn = 30, double rate = 0.1)
        {
            //Setting setting = new Setting();
            //double rate = setting.GetRate();     //学習率 0.1f

            Mat W1_grad = new Mat(W1.Zero_matrix),
                B1_grad = new Mat(B1.Zero_matrix),
                W2_grad = new Mat(W2.Zero_matrix),
                B2_grad = new Mat(B2.Zero_matrix),
                W3_grad = new Mat(W3.Zero_matrix),
                B3_grad = new Mat(B3.Zero_matrix);

            //Console.WriteLine("誤差逆伝播：しない　0　、する　1");
            //int deeplearning = int.Parse(Console.ReadLine());
            //Console.WriteLine("学習回数");
            //int learn = int.Parse(Console.ReadLine());

            //int deeplearning = setting.GetDeep();
            //int learn = setting.GetLearn();

            double result_Y = 0.0;
            double result_Accuracy = 0.0;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < learn; i++)
            {
                if (deeplearning == 0)            //誤差逆伝播法を使用しない場合
                {
                    /* Lossをメソッドグループに改造したため使用不可
                    W1_grad.Matrix_data = W1.Numerical_gradient(Loss);
                    B1_grad.Matrix_data = B1.Numerical_gradient(Loss);
                    W2_grad.Matrix_data = W2.Numerical_gradient(Loss);
                    B2_grad.Matrix_data = B2.Numerical_gradient(Loss);
                    W3_grad.Matrix_data = W3.Numerical_gradient(Loss);
                    B3_grad.Matrix_data = B3.Numerical_gradient(Loss);

                    W1.Matrix_data = W1 - (rate * W1_grad);
                    B1.Matrix_data = B1 - (rate * B1_grad);
                    W2.Matrix_data = W2 - (rate * W2_grad);
                    B2.Matrix_data = B2 - (rate * B2_grad);
                    W3.Matrix_data = W3 - (rate * W3_grad);
                    B3.Matrix_data = B3 - (rate * B3_grad);
                    */
                }
                else                     //誤差逆伝播法を使用する場合
                {
                    Loss(X, W1, B1, W2, B2, W3, B3, T, A1, Z1, A2, Z2, A3, Y);
                    Mat affine3 = Loss_forward(Y, T);
                    Mat affine3_dx = new Mat(Mat.dot(affine3, new Mat(W3.T)));   //癖のあるプログラムなので、Matクラスの設計を考え直す必要あり。
                    W3_grad.Matrix_data = Mat.dot(new Mat(Z2.T), affine3);
                    B3_grad.Matrix_data = affine3.Matrix_data;

                    Mat sigmoid_2 = new Mat(A2.Sigmoid_backword(affine3_dx.Matrix_data));

                    Mat affine2 = sigmoid_2;
                    Mat affine2_dx = new Mat(Mat.dot(affine2, new Mat(W2.T)));
                    W2_grad.Matrix_data = Mat.dot(new Mat(Z1.T), affine2);
                    B2_grad.Matrix_data = affine2.Matrix_data;

                    Mat sigmoid_1 = new Mat(A1.Sigmoid_backword(affine2_dx.Matrix_data));

                    Mat affine1 = sigmoid_1;
                    Mat affine1_dx = new Mat(Mat.dot(affine1, new Mat(W1.T)));
                    W1_grad.Matrix_data = Mat.dot(new Mat(X.T), affine1);
                    B1_grad.Matrix_data = affine1.Matrix_data;

                    W1.Matrix_data = W1 - (rate * W1_grad);
                    B1.Matrix_data = B1 - (rate * B1_grad);
                    W2.Matrix_data = W2 - (rate * W2_grad);
                    B2.Matrix_data = B2 - (rate * B2_grad);
                    W3.Matrix_data = W3 - (rate * W3_grad);
                    B3.Matrix_data = B3 - (rate * B3_grad);
                }

                Loss(X, W1, B1, W2, B2, W3, B3, T, A1, Z1, A2, Z2, A3, Y);            //パラメータ更新後、最終的な出力を得、表示。

                //Result result = new Result();
                //result.SetResult(i.ToString() + ":" + Y.Matrix_data[0][0].ToString() + "==" + Y.Matrix_data[0][2].ToString());

                sb.Append("[epoch: ");
                sb.Append(@i);
                sb.Append("]>> 出力値Y:");
                sb.Append(Y.Matrix_data[0][0].ToString());  //出力値 Y
                sb.Append("; ");
                sb.Append("信頼度：約");
                sb.Append(string.Format("{0:0.0%}", Y.Matrix_data[0][2]));
                sb.Append("(Accuracy = ");
                sb.Append(Y.Matrix_data[0][2].ToString());  //Accuracy？
                sb.Append("%)");

                //信頼度が最も高い値を保存
                if (Y.Matrix_data[0][2] > result_Accuracy)
                {
                    result_Accuracy = Y.Matrix_data[0][2];
                    result_Y = Y.Matrix_data[0][0];
                    sb.Append("<NEW RECORD>");
                }
                sb.Append(";");
                sb.Append("\r\f");
                //--------------------
                sb.Append("X1:");
                sb.Append(X.Matrix_data[0][0].ToString());  //固定値
                sb.Append(", ");
                sb.Append("X2:");
                sb.Append(X.Matrix_data[0][1].ToString());  //固定値
                sb.Append(", ");
                sb.Append("X3:");
                sb.Append(X.Matrix_data[0][2].ToString());  //固定値
                sb.Append(", ");
                sb.Append("X4:");
                sb.Append(X.Matrix_data[0][3].ToString());  //固定値
                sb.Append(", ");
                sb.Append("W1:");
                sb.Append(W1.Matrix_data[0][0].ToString());  //固定値
                sb.Append(", ");
                sb.Append("B1:");
                sb.Append(B1.Matrix_data[0][0].ToString());  //固定値
                sb.Append(", ");
                sb.Append("W2:");
                sb.Append(W2.Matrix_data[0][0].ToString());
                sb.Append(", ");
                sb.Append("B2:");
                sb.Append(B2.Matrix_data[0][0].ToString());
                sb.Append(", ");
                sb.Append("W3:");
                sb.Append(W3.Matrix_data[0][0].ToString());
                sb.Append(", ");
                sb.Append("B3:");
                sb.Append(B3.Matrix_data[0][0].ToString());
                sb.Append(", ");
                sb.Append("T:");
                sb.Append(T.Matrix_data[0][0].ToString());  //固定値
                sb.Append(";");
                sb.Append("\r\f");

            }
            //演算結果----------------------------------------------
            sb.Append("[Result]>>");
            sb.Append(result_Y);
            sb.Append("(");
            sb.Append(result_Accuracy);
            sb.Append("%);");
            return sb.ToString();
        }

        protected void Push_Deep(object sender, EventArgs e)
        {
            RunMatrix();
        }

        protected void Push_DP(object sender, EventArgs e)
        {
            if (Panel_DP.Visible)
            {
                Panel_DP.Visible = false;
            }
            else
            {
                Panel_DP.Visible = true;
            }
        }



        /// <summary>
        /// 要素[]x, []yより、相関係数rを求めます。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected double Soukan(double []x, double []y)
        {
            //x基準
            int number = x.Count();

            //共分散(Sxy): Σ(データx - データxの平均)(データy - データyの平均) /n -1
            double s_xy = S_xy(x, y, number, 1);

            //xの標準偏差: √(Σ(データx - データxの平均)^2 /n -1)
            //yの標準偏差: √(Σ(データy - データyの平均)^2 /n -1)
            double h = Hensa(x, y, number, 1);

            //相関係数(r): 共分散 / xの標準偏差 * yの標準偏差
            double r = s_xy / h;

            return r;   //相関係数r
        }

        /// <summary>
        /// 要素[]x, []yの共分散を求めます。
        /// Σ(データx - データxの平均)(データy - データyの平均) /n -1
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="n"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        protected double S_xy(double []x, double []y, int n, int i)
        {
            double sum = 0; //合計
            for (int k = i; k <= n; k++)
            {
                sum += (x[k] * k - x.Average()) * (y[k] - y.Average()) / x.Count(); //x基準　nullはないものとみなす
            }

            return sum;
        }

        /// <summary>
        /// 要素[]x, []yの標準偏差を求めます。
        /// √(Σ(データx - データxの平均)^2 /n -1) * √(Σ(データy - データyの平均)^2 /n -1)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="n"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        protected double Hensa(double []x, double []y, int n, int i)
        {
            double sum = 0; //合計
            for (int k = i; k <= n; k++)
            {
                sum += Math.Sqrt((x[k] * k - x.Average())*(x[k] * k - x.Average()) / x.Count()) * Math.Sqrt((y[k] - y.Average()) *(y[k] - y.Average()) / y.Count()); //x基準とy基準　nullはないものとみなす
            }

            return sum;
        }



    }
}






