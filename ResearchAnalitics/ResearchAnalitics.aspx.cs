using System;
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





        }



        protected void RunMatrix()
        {
            string learn = HtmlEncode(TextBox_Learn.Text);
            string rate = HtmlEncode(TextBox_Rate.Text);
            int.TryParse(learn, System.Globalization.NumberStyles.Currency, null, out int value1);
            double.TryParse(rate, System.Globalization.NumberStyles.Currency, null, out double value2);

            int isdeep;
            if (CheckBox_deep.Checked)
            {
                isdeep = 1;
            }
            else
            {
                isdeep = 0;
            }

            //※本来はタスクかコンソールで行うほうがよい。
            TextBox_DeepResult.Text = Master(isdeep, value1, value2);
        }


        //----------------------------------------------------
        //テスト用配列（読み取り専用）
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

                private static Mat A2 = new Mat(
                new double[] { 0.0, 0.0 }
                );

                private static Mat Z2 = new Mat(
                new double[] { 0.0, 0.0 }
                );

                private static Mat A3 = new Mat(
                new double[] { 0.0, 0.0, 0.0, 0.0 }
                );

                private static Mat Y = new Mat(
                new double[] { 0.0, 0.0, 0.0, 0.0 }
                );

        //----------------------------------------------------

        static double Loss()     //交差エントロピー誤差を求める
        {
            A1.Matrix_data = Mat.dot(X, W1) + B1;
            Z1.Matrix_data = A1.Sigmoid();
            A2.Matrix_data = Mat.dot(Z1, W2) + B2;
            Z2.Matrix_data = A2.Sigmoid();
            A3.Matrix_data = Mat.dot(Z2, W3) + B3;
            Y.Matrix_data = A3.Softmax();

            return Y.Cross_etp_err(T);
        }
        static Mat Loss_forward()     //誤差逆伝播時に使用する　交差エントロピー誤差とソフトマックス関数の微分
        {
            Mat dx = new Mat(
                new double[] { 0.0, 0.0, 0.0, 0.0 }
                );

            dx.Matrix_data = Y - T;
            dx.Matrix_data = dx / 2;

            return dx;
        }

        protected static new string Master(int deeplearning = 1, int learn = 30, double rate = 0.1)
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

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < learn; i++)
            {
                if (deeplearning == 0)            //誤差逆伝播法を使用しない場合
                {
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
                }
                else                     //誤差逆伝播法を使用する場合
                {
                    Loss();
                    Mat affine3 = Loss_forward();
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

                Loss();            //パラメータ更新後、最終的な出力を得、表示。

                //Result result = new Result();
                //result.SetResult(i.ToString() + ":" + Y.Matrix_data[0][0].ToString() + "==" + Y.Matrix_data[0][2].ToString());

                sb.Append(@i);
                sb.Append(":");
                sb.Append(Y.Matrix_data[0][0].ToString());
                sb.Append("==");
                sb.Append(Y.Matrix_data[0][2].ToString());
                sb.Append("\r\f");

            }
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
    }
}







