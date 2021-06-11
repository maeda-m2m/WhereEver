using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using static System.Web.HttpUtility;


//----------------------------------------------------------------------------------------------
// 使用したパッケージ
// Json.NET is a popular high - performance JSON framework for .NET
// v13.0.1（2021/06/11現在最新安定版）
// 作成者：(C)James Newton-King
// MIT Licence
// https://opensource.org/licenses/mit-license.php
//
//MIT License

//        Copyright(C) 2013-2021 .NET Foundation and Contributors

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files(the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//----------------------------------------------------------------------------------------------




namespace WhereEver.ClassLibrary
{
    public static class CoinClass
    {
        public class Block
        {
            /// <summary>
            /// インデックス
            /// </summary>
            public int Index { get; set; }

            /// <summary>
            /// タイムスタンプ
            /// </summary>
            public DateTime Timestamp { get; set; }

            /// <summary>
            /// トランザクション（取引データ）
            /// </summary>
            public List<Transaction> Transactions { get; set; }

            /// <summary>
            /// ナンス
            /// </summary>
            public int Nonce { get; set; }

            /// <summary>
            /// 前のブロックのハッシュ
            /// </summary>
            public string PreviousHash { get; set; }

            public Block()
            {
                Transactions = new List<Transaction>();
            }
        }

        /// <summary>
        /// トランザクション
        /// </summary>
        public class Transaction
        {
            /// <summary>
            /// 量
            /// </summary>
            public int Amount { get; set; }

            /// <summary>
            /// 受信者
            /// </summary>
            public string Recipient { get; set; }

            /// <summary>
            /// 送信者
            /// </summary>
            public string Sender { get; set; }
        }

        public class BlockChain
        {
            /// <summary>
            /// ブロックチェーン
            /// </summary>
            private List<Block> blockChain = new List<Block>();

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public BlockChain()
            {
                //ジェネシスブロック生成（ベタ書き）List blockChainが空のときに実行
                blockChain.Add(new Block()
                {
                    Index = blockChain.Count,
                    Timestamp = DateTime.UtcNow,
                    Nonce = -1,
                    PreviousHash = "666"
                });
            }

            /// <summary>
            /// ブロック生成
            /// </summary>
            public string CreateBlock(int firstnonce, int limitnonce, string condition)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i <= limitnonce; i++)
                {

                    int nonce = CreateNonce(firstnonce, blockChain.Last().Nonce, limitnonce, i, condition);
                    if (nonce <= -1)
                    {
                        //取得完了or失敗
                        return sb.ToString();
                    }

                    blockChain.Add(new Block
                    {
                        Index = blockChain.Count,
                        Timestamp = DateTime.UtcNow,
                        Nonce = nonce,
                        PreviousHash = CreateHash(blockChain.Last())
                    });
                    //-------------------------------------------
                    if(sb.ToString() == "")
                    {
                        sb.Append("---Result(Condition(StartsWith): ");
                        sb.Append(condition);
                        sb.Append(")---");
                    }
                    else
                    {
                        sb.Append("------NEXT------");
                    }
                    sb.Append(";\r\f");
                    sb.Append("Coin_Index: ");
                    sb.Append(blockChain.Count);
                    sb.Append(";\r\f");
                    sb.Append("Timestamp(UTC): ");
                    sb.Append(DateTime.UtcNow);
                    sb.Append(";\r\f");
                    sb.Append("Nonce: ");
                    sb.Append(nonce);
                    sb.Append(";\r\f");
                    sb.Append("PreviousHash: ");
                    sb.Append(CreateHash(blockChain.Last()));
                    sb.Append(";\r\f");
                    sb.Append("------------");

                    //再度次のCoinを発行
                    i = nonce;
                    firstnonce = nonce + 1;

                }
                //本来ここにはこない
                return sb.ToString();
            }
        }



        /// <summary>
        /// ナンス生成
        /// </summary>
        /// <param name="previousNonce">前のブロックのナンス</param>
        /// <returns>ナンス</returns>
        private static int CreateNonce(int firstnonce, int previousNonce, int limitnonce, int counter, string condition)
        {
            int nonce = firstnonce;
            while (!CheckNonce(previousNonce, nonce, condition))
            {
                nonce++;

                counter++;
                if (counter >= limitnonce)
                {
                    return -1;
                }

            }
            return nonce;
        }

        /// <summary>
        /// ナンスチェック
        /// </summary>
        /// <param name="previousNonce">前のブロックのナンス</param>
        /// <param name="nonce">チェックするナンス</param>
        /// <param name="condition">コンディション</param>
        /// <returns></returns>
        private static bool CheckNonce(int previousNonce, int nonce, string condition)
        {
            return GetHash($"{previousNonce}{nonce}").StartsWith(condition);
        }

        /// <summary>
        /// ハッシュ値作成
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        private static string CreateHash(Block block)
        {
            return GetHash(JsonConvert.SerializeObject(block));
        }


        /// <summary>
        /// ハッシュ値取得
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string GetHash(string data)
        {
            byte[] hash = null;
            var bytes = Encoding.Unicode.GetBytes(data);

            using (var sha256 = new SHA256CryptoServiceProvider())
            {
                hash = sha256.ComputeHash(bytes);
            }

            return string.Join("", hash.Select(x => x.ToString("X")));
        }




    }
}