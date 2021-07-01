using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhereEver.ClassLibrary
{
    public class XorShift
    {
        /// <summary>
        ///  uintの乱数を高速生成します。
        /// 【呼び出し方】：
        ///  XorShift.Xorshift xorshift = new XorShift.Xorshift();
        ///  xorshift.Next();
        /// 【参考】：擬似乱数生成アルゴリズム「Xorshift」をC#で実装
        ///  @加賀
        ///  投稿日：2015年5月12日 更新日：2020年1月22日
        ///  https://developers.wonderpla.net/entry/2015/05/12/112650
        ///  2021年7月01日アクセス.
        /// </summary>
        public class Xorshift
        {
            // 内部メモリ  
            private UInt32 x;
            private UInt32 y;
            private UInt32 z;
            private UInt32 w;

            public Xorshift() : this((UInt64)DateTime.Now.Ticks)
            {
            }

            public Xorshift(UInt64 seed)
            {
                SetSeed(seed);
            }

            /// <summary>  
            /// シード値を設定  
            /// </summary>  
            /// <param name="seed">シード値</param>  
            public void SetSeed(UInt64 seed)
            {
                // x,y,z,wがすべて0にならないようにする  
                x = 521288629u;
                y = (UInt32)(seed >> 32) & 0xFFFFFFFF;
                z = (UInt32)(seed & 0xFFFFFFFF);
                w = x ^ z;
            }

            /// <summary>  
            /// 乱数を取得  
            /// </summary>  
            /// <returns>乱数</returns>  
            public UInt32 Next()
            {
                UInt32 t = x ^ (x << 11);
                x = y;
                y = z;
                z = w;
                w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
                return w;
            }


        }

    }
}