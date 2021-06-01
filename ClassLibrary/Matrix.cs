using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhereEver.ClassLibrary
{
    public class Matrix
    {


        /// <summary>
        /// 参考：
        /// @oskats1987(2020年05月01日)
        /// 「C#で、ディープラーニング　～誤差逆伝播法～」:
        /// https://qiita.com/oskats1987/items/69d25eb3b08cdcd8495a
        /// 2021年６月１日アクセス.
        /// 
        /// 本：
        /// 『ゼロから作る　Deep Learning(オライリー)』　版元調査中
        /// </summary>
        public class Mat
        {
            //プロパティR,Cは、行列の行と列の数を示す

            private int r = 0;
            public int R
            {
                get { return r; }
            }
            private int c = 0;
            public int C
            {
                get { return c; }
            }
            private bool err = false;
            public bool Err
            {
                get { return err; }
            }
            private double[][] matrix_data;
            public double[][] Matrix_data    //行列データを取得、設定
            {
                get
                {
                    double[][] a = new double[2][];
                    a[0] = new double[] { 0, 0 };
                    a[1] = new double[] { 0, 0 };
                    if (err) return a;
                    else return matrix_data;
                }
                set
                {
                    matrix_data = value;
                }
            }
            public double[][] Zero_matrix    //同じ形の、全ての要素が0の行列を示す
            {
                get
                {
                    double[][] zm = new double[this.r][];
                    for (int i = 0; i < this.r; i++)
                    {
                        zm[i] = new double[this.c];
                        for (int j = 0; j < this.c; j++)
                        {
                            zm[i][j] = 0;
                        }
                    }
                    return zm;
                }
            }
            public double[][] T      //転置行列を取得
            {
                get
                {
                    double[][] t = new double[this.c][];
                    for (int i = 0; i < this.c; i++)
                    {
                        t[i] = new double[this.r];
                        for (int j = 0; j < this.r; j++)
                        {
                            t[i][j] = matrix_data[j][i];
                        }
                    }
                    return t;
                }
            }
            public Mat(params double[][] vs)
            {
                int len = vs[0].Length;

                for (int i = 0; i < vs.Length; i++)
                {
                    if (i != 0 && len != vs[i].Length)
                    {
                        err = true;
                    }
                }
                if (!err)
                {
                    r = vs.Length;
                    c = vs[0].Length;
                    matrix_data = vs;
                }
            }

            private double[][] sigmoid_out;   //シグモイド関数実行時の結果を一時保存する
            public double[][] Sigmoid()   //シグモイド関数
            {
                double[][] sig = new double[1][];
                sig[0] = new double[this.c];

                for (int i = 0; i < this.c; i++)
                {
                    sig[0][i] = 1 / (1 + System.Math.Exp(-this.matrix_data[0][i]));
                }

                sigmoid_out = sig;

                return sig;
            }
            public double[][] Sigmoid_backword(double[][] dout)  //誤差逆伝播時のシグモイド関数の微分
            {
                double[][] sig = new double[1][];
                sig[0] = new double[this.c];

                for (int i = 0; i < this.c; i++)
                {
                    sig[0][i] = dout[0][i] * (1.0 - this.sigmoid_out[0][i]) * this.sigmoid_out[0][i];
                }

                return sig;
            }
            public double[][] Softmax()   //ソフトマックス関数
            {
                double[][] sm = new double[1][];
                sm[0] = new double[this.c];

                double m = this.matrix_data[0].Max();

                double[] exp_a = new double[this.c];
                for (int i = 0; i < this.c; i++)
                {
                    exp_a[i] = System.Math.Exp(this.matrix_data[0][i] - m);
                }

                double sum = 0.0;
                for (int i = 0; i < this.c; i++)
                {
                    sum = sum + exp_a[i];
                }

                for (int i = 0; i < this.c; i++)
                {
                    sm[0][i] = exp_a[i] / sum;
                }

                return sm;
            }
            public double Cross_etp_err(Mat t)   //公差エントロピ誤差
            {
                double delta = 0.0000001;
                double sum = 0.0;
                for (int i = 0; i < this.c; i++)
                {
                    sum = sum + t.matrix_data[0][i] * System.Math.Log(this.matrix_data[0][i] + delta);
                }

                return -sum;
            }
            public double[][] Numerical_gradient(System.Func<double> loss)  //勾配を求める
            {
                double h = 0.0001;
                double[][] grad = new double[this.r][];
                double tmp_val = 0.0;
                double fxh1 = 0.0;
                double fxh2 = 0.0;

                for (int i = 0; i < this.r; i++)
                {
                    grad[i] = new double[this.c];
                    for (int j = 0; j < this.c; j++)
                    {
                        tmp_val = this.matrix_data[i][j];
                        this.matrix_data[i][j] = tmp_val + h;
                        fxh1 = loss();

                        this.matrix_data[i][j] = tmp_val - h;
                        fxh2 = loss();

                        grad[i][j] = (fxh1 - fxh2) / (2 * h);
                        this.matrix_data[i][j] = tmp_val;
                    }
                }

                return grad;
            }
            //以下　演算子オーバーロード
            public static double[][] operator +(Mat p1, Mat p2)
            {
                double[][] d = new double[p1.R][];

                if (p1.C == p2.C && p1.R == p2.R)
                {
                    for (int i = 0; i < p1.R; i++)
                    {
                        d[i] = new double[p1.C];
                        for (int j = 0; j < p1.C; j++)
                        {
                            d[i][j] = p1.Matrix_data[i][j] + p2.Matrix_data[i][j];
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < p1.R; k++)
                    {
                        d[k] = new double[2] { 0, 0 };
                    }
                }

                return d;
            }
            public static double[][] operator +(double[][] p1, Mat p2)
            {
                double[][] d = new double[p1.Length][];

                if (p1[0].Length == p2.C && p1.Length == p2.R)
                {
                    for (int i = 0; i < p1.Length; i++)
                    {
                        d[i] = new double[p1[0].Length];
                        for (int j = 0; j < p1[0].Length; j++)
                        {
                            d[i][j] = p1[i][j] + p2.Matrix_data[i][j];
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < p1.Length; k++)
                    {
                        d[k] = new double[2] { 0, 0 };
                    }
                }

                return d;
            }
            public static double[][] operator +(Mat p1, double[][] p2)
            {
                double[][] d = new double[p1.R][];

                if (p1.C == p2[0].Length && p1.R == p2.Length)
                {
                    for (int i = 0; i < p1.R; i++)
                    {
                        d[i] = new double[p1.C];
                        for (int j = 0; j < p1.C; j++)
                        {
                            d[i][j] = p1.Matrix_data[i][j] + p2[i][j];
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < p1.R; k++)
                    {
                        d[k] = new double[2] { 0, 0 };
                    }
                }

                return d;
            }
            public static double[][] operator +(double p1, Mat p2)
            {
                double[][] d = new double[p2.R][];
                for (int i = 0; i < p2.R; i++)
                {
                    d[i] = new double[p2.C];
                    for (int j = 0; j < p2.C; j++)
                    {
                        d[i][j] = p2.Matrix_data[i][j] + p1;
                    }
                }

                return d;
            }
            public static double[][] operator +(Mat p1, double p2)
            {
                double[][] d = new double[p1.R][];
                for (int i = 0; i < p1.R; i++)
                {
                    d[i] = new double[p1.C];
                    for (int j = 0; j < p1.C; j++)
                    {
                        d[i][j] = p1.Matrix_data[i][j] + p2;
                    }
                }

                return d;
            }
            public static double[][] operator -(Mat p1, Mat p2)
            {
                double[][] d = new double[p1.R][];

                if (p1.C == p2.C && p1.R == p2.R)
                {
                    for (int i = 0; i < p1.R; i++)
                    {
                        d[i] = new double[p1.C];
                        for (int j = 0; j < p1.C; j++)
                        {
                            d[i][j] = p1.Matrix_data[i][j] - p2.Matrix_data[i][j];
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < p1.R; k++)
                    {
                        d[k] = new double[2] { 0, 0 };
                    }
                }

                return d;
            }
            public static double[][] operator -(double[][] p1, Mat p2)
            {
                double[][] d = new double[p1.Length][];

                if (p1[0].Length == p2.C && p1.Length == p2.R)
                {
                    for (int i = 0; i < p1.Length; i++)
                    {
                        d[i] = new double[p1[0].Length];
                        for (int j = 0; j < p1[0].Length; j++)
                        {
                            d[i][j] = p1[i][j] - p2.Matrix_data[i][j];
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < p1.Length; k++)
                    {
                        d[k] = new double[2] { 0, 0 };
                    }
                }

                return d;
            }
            public static double[][] operator -(Mat p1, double[][] p2)
            {
                double[][] d = new double[p1.R][];

                if (p1.C == p2[0].Length && p1.R == p2.Length)
                {
                    for (int i = 0; i < p1.R; i++)
                    {
                        d[i] = new double[p1.C];
                        for (int j = 0; j < p1.C; j++)
                        {
                            d[i][j] = p1.Matrix_data[i][j] - p2[i][j];
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < p1.R; k++)
                    {
                        d[k] = new double[2] { 0, 0 };
                    }
                }

                return d;
            }

            public static double[][] operator -(double p1, Mat p2)
            {
                double[][] d = new double[p2.R][];
                for (int i = 0; i < p2.R; i++)
                {
                    d[i] = new double[p2.C];
                    for (int j = 0; j < p2.C; j++)
                    {
                        d[i][j] = p1 - p2.Matrix_data[i][j];
                    }
                }

                return d;
            }
            public static double[][] operator -(Mat p1, double p2)
            {
                double[][] d = new double[p1.R][];
                for (int i = 0; i < p1.R; i++)
                {
                    d[i] = new double[p1.C];
                    for (int j = 0; j < p1.C; j++)
                    {
                        d[i][j] = p1.Matrix_data[i][j] - p2;
                    }
                }

                return d;
            }
            public static double[][] operator *(Mat p1, Mat p2)
            {
                double[][] d = new double[p1.R][];

                if (p1.C == p2.C && p1.R == p2.R)
                {
                    for (int i = 0; i < p1.R; i++)
                    {
                        d[i] = new double[p1.C];
                        for (int j = 0; j < p1.C; j++)
                        {
                            d[i][j] = p1.Matrix_data[i][j] * p2.Matrix_data[i][j];
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < p1.R; k++)
                    {
                        d[k] = new double[2] { 0, 0 };
                    }
                }

                return d;
            }
            public static double[][] operator *(double[][] p1, Mat p2)
            {
                double[][] d = new double[p1.Length][];

                if (p1[0].Length == p2.C && p1.Length == p2.R)
                {
                    for (int i = 0; i < p1.Length; i++)
                    {
                        d[i] = new double[p1[0].Length];
                        for (int j = 0; j < p1[0].Length; j++)
                        {
                            d[i][j] = p1[i][j] * p2.Matrix_data[i][j];
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < p1.Length; k++)
                    {
                        d[k] = new double[2] { 0, 0 };
                    }
                }

                return d;
            }
            public static double[][] operator *(Mat p1, double[][] p2)
            {
                double[][] d = new double[p1.R][];

                if (p1.C == p2[0].Length && p1.R == p2.Length)
                {
                    for (int i = 0; i < p1.R; i++)
                    {
                        d[i] = new double[p1.C];
                        for (int j = 0; j < p1.C; j++)
                        {
                            d[i][j] = p1.Matrix_data[i][j] * p2[i][j];
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < p1.R; k++)
                    {
                        d[k] = new double[2] { 0, 0 };
                    }
                }

                return d;
            }

            public static double[][] operator *(double p1, Mat p2)
            {
                double[][] d = new double[p2.R][];
                for (int i = 0; i < p2.R; i++)
                {
                    d[i] = new double[p2.C];
                    for (int j = 0; j < p2.C; j++)
                    {
                        d[i][j] = p1 * p2.Matrix_data[i][j];
                    }
                }

                return d;
            }
            public static double[][] operator *(Mat p1, double p2)
            {
                double[][] d = new double[p1.R][];
                for (int i = 0; i < p1.R; i++)
                {
                    d[i] = new double[p1.C];
                    for (int j = 0; j < p1.C; j++)
                    {
                        d[i][j] = p1.Matrix_data[i][j] * p2;
                    }
                }

                return d;
            }
            public static double[][] operator /(Mat p1, Mat p2)
            {
                double[][] d = new double[p1.R][];

                if (p1.C == p2.C && p1.R == p2.R)
                {
                    for (int i = 0; i < p1.R; i++)
                    {
                        d[i] = new double[p1.C];
                        for (int j = 0; j < p1.C; j++)
                        {
                            d[i][j] = p1.Matrix_data[i][j] / p2.Matrix_data[i][j];
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < p1.R; k++)
                    {
                        d[k] = new double[2] { 0, 0 };
                    }
                }

                return d;
            }
            public static double[][] operator /(double[][] p1, Mat p2)
            {
                double[][] d = new double[p1.Length][];

                if (p1[0].Length == p2.C && p1.Length == p2.R)
                {
                    for (int i = 0; i < p1.Length; i++)
                    {
                        d[i] = new double[p1[0].Length];
                        for (int j = 0; j < p1[0].Length; j++)
                        {
                            d[i][j] = p1[i][j] / p2.Matrix_data[i][j];
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < p1.Length; k++)
                    {
                        d[k] = new double[2] { 0, 0 };
                    }
                }

                return d;
            }
            public static double[][] operator /(Mat p1, double[][] p2)
            {
                double[][] d = new double[p1.R][];

                if (p1.C == p2[0].Length && p1.R == p2.Length)
                {
                    for (int i = 0; i < p1.R; i++)
                    {
                        d[i] = new double[p1.C];
                        for (int j = 0; j < p1.C; j++)
                        {
                            d[i][j] = p1.Matrix_data[i][j] / p2[i][j];
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < p1.R; k++)
                    {
                        d[k] = new double[2] { 0, 0 };
                    }
                }

                return d;
            }

            public static double[][] operator /(double p1, Mat p2)
            {
                double[][] d = new double[p2.R][];
                for (int i = 0; i < p2.R; i++)
                {
                    d[i] = new double[p2.C];
                    for (int j = 0; j < p2.C; j++)
                    {
                        d[i][j] = p1 / p2.Matrix_data[i][j];
                    }
                }

                return d;
            }
            public static double[][] operator /(Mat p1, double p2)
            {
                double[][] d = new double[p1.R][];
                for (int i = 0; i < p1.R; i++)
                {
                    d[i] = new double[p1.C];
                    for (int j = 0; j < p1.C; j++)
                    {
                        d[i][j] = p1.Matrix_data[i][j] / p2;
                    }
                }

                return d;
            }
            public static double[][] dot(Mat p1, Mat p2)    //行列のドット積
            {
                double[][] d = new double[p1.R][];
                double temp = 0;

                if (p1.C == p2.R)
                {
                    for (int i = 0; i < p1.R; i++)
                    {
                        d[i] = new double[p2.C];
                        for (int j = 0; j < p2.C; j++)
                        {
                            for (int a = 0; a < p1.C; a++)
                            {
                                temp = temp + p1.Matrix_data[i][a] * p2.Matrix_data[a][j];
                            }
                            d[i][j] = temp;
                            temp = 0.0;
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < p1.R; k++)
                    {
                        d[k] = new double[2] { 0, 0 };
                    }
                }

                return d;
            }
            public static double Sum(Mat mat)     //1次元のMatクラスの、全ての和
            {
                double s = 0.0;

                for (int i = 0; i < mat.c; i++)
                {
                    s = mat.Matrix_data[0][i] + s;
                }

                return s;
            }
        }





        public class Result
        {
            private string result = "";

            public void SetResult(string str)
            {
                result = str;
                return;
            }

            public string GetResult()
            {
                return result;
            }

        }

        public class Setting
        {

            //学習率
            private double rate = 0.1f;
            public void SetRate(double d)
            {
                rate = d;
                return;
            }

            public double GetRate()
            {
                return rate;
            }


            //誤差逆伝播 1=true 0=false
            private int deeplearning = 1;
            public void SetDeep(int d)
            {
                deeplearning = d;
                return;
            }

            public int GetDeep()
            {
                return deeplearning;
            }


            //学習回数
            private int learn = 10;
            public void SetLearn(int l)
            {
                learn = l;
                return;
            }

            public int GetLearn()
            {
                return learn;
            }


        }



    }
}