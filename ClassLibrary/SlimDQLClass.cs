using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WhereEver.ClassLibrary
{
    public class SlimDQLClass
    {



        /*
            Q-LearningではQ値を、「その状態での即時的に得られる報酬の価値」と「見積もられた次の状態の価値」の和と定義しています。
            そして次の状態の価値を見積もるのに、現在推定されている価値の最大値を用いるのがQ値の特徴です。

            状態State：現在どういう状況になっているかを表すもの。ゲームの場合、「敵が目の前にいる」という情報も状態になります
            行動Action：実際に起こす行動を表すもの。ゲームの場合、「ジャンプする」といったことが行動に当たります
            報酬Reward：とある状態においてある行動を起こした結果、どのくらいの利益が得られるかの値。ゲームの場合、クリアしたときです

            状態行動価値Q：Q値と呼ばれる。ある状態Sにおいて、ある行動Aを行ったときの価値のこと

            引用元：
            https://www.atmarkit.co.jp/ait/articles/1903/05/news008.html
         */


        //------------------------------------------[コンフィグ]------------------------------------------//
        private static readonly StringBuilder sb = new StringBuilder();  //GetQLeearningでreturnする文字列に使用します。
        //private const int cMainProcess = 3000000;   //メインプロセスの待機フレームです。Main処理でのみ使用します。
        private const int cX = 5;    //X方向のパターン数(0～n)
        private const int cY = 5;    //Y方向のパターン数(0～n)
        private const int cState = cX + 1 * cY + 1;    //State 状態のパターン数(0～n)
        private const int cAction = 4;   //Action 行動のパターン数(0～n)
        private const int cMaxIndex =  cState + 1 * cAction + 1;    //最大index数(0～n, n > 0)

        private const int cMaxQ = 1000;  //試行回数 深層学習なら1000000回、テストなら1000回程度が推奨です。
        private const double cAlpha = 0.01f;    //α 学習率 1に近いほど急激にQ値を更新して学習します。高すぎると過学習のおそれがあります。
        private const double cGammma = 0.8f;    //γ 割引率(0～1) 0なら目先の報酬を重視します。パラメータと報酬はγに左右されます。
        private static bool cDictionarySW = true;  //Q関数を用いた辞書型AIを使用するか？（初期検索時はfalseにしたほうが局所解に陥る確率が低くなります）

        private static List<double> QMasterTable;   //QTable    (double)Q値を複数格納するマスターテーブル。indexで呼び出しと書き込みができます。初期値は全て0扱いです。
        private static List<double> RMasterTable;   //RTable    (double)Reward値を複数格納マスターテーブル。indexで呼び出しができます。書き込みは事前に設定しておきます。
        //------------------------------------------------------------------------------------------------//

        /// <summary>
        /// SlimSQLを実行し、出力結果をstring形式で得ます。
        /// テスト用のため、値はデータベースに保存されません。
        /// </summary>
        /// <param name="b">Q辞書ON/OFF</param>
        /// <param name="episodeId">通常は0</param>
        /// <returns>string</returns>
        public string GetSlimDQL(bool b = true, int episodeId = 0)
        {
            //Config Q辞書 ON/OFF
            cDictionarySW = b;

            //実行
            this.SetEpisode(episodeId);

            return sb.ToString();
        }

        /// <summary>
        /// 状態Stateと行動Actionから現在のインデックスindexを取得します。
        /// </summary>
        /// <param name="state"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        protected int GetIndex(int state, int action)
        {
            //最大値と最小値でトリム
            state = Math.Min(cState, state);
            action = Math.Min(cAction, action);
            state = Math.Max(0, state);
            action = Math.Max(0, action);

            return state + 1 * action + 1;
        }

        //------------------------------------------[初期化処理]------------------------------------------//

        /// <summary>
        /// QMasterTableとRMasterTableを初期化します。
        /// </summary>
        /// <param name="initValue"></param>
        protected void SetQInit(double initValue = 0f)
        {
            //init
            QMasterTable = new List<double>();
            RMasterTable = new List<double>();

            for (int k = 0; k < cMaxIndex; k++)
            {
                QMasterTable.Add((double)initValue);
                RMasterTable.Add((double)initValue);
            }

            //報酬を格納するRMasterTableを初期設定します。
            this.SetReward();

        }

        /// <summary>
        /// 報酬Rewardを定義します。
        /// </summary>
        protected void SetReward()
        {
            //定数 Reward UserDataBase
            RMasterTable[0] = -1f;  //行動ごとに微小の罰
            RMasterTable[1] = -1f;
            RMasterTable[2] = -1f;
            RMasterTable[3] = -1f;
            RMasterTable[4] = -1f;
            RMasterTable[5] = -1f;
            RMasterTable[6] = -1f;
            RMasterTable[7] = -1f;
            RMasterTable[8] = -1f;
            RMasterTable[9] = -1f;
            RMasterTable[10] = -1f;
            RMasterTable[11] = -1f;
            RMasterTable[12] = -1f;
            RMasterTable[13] = -1f;
            RMasterTable[14] = -1f;
            RMasterTable[15] = -1f;
            RMasterTable[16] = -1f;
            RMasterTable[17] = -1f;
            RMasterTable[18] = -1f;
            RMasterTable[19] = -1f;
            RMasterTable[20] = -1f;
            RMasterTable[21] = -1f;
            RMasterTable[22] = -1f;
            RMasterTable[23] = -1f;
            RMasterTable[24] = -1f;
            RMasterTable[25] = -1f;
            RMasterTable[26] = -1f;
            RMasterTable[27] = -1f;
            RMasterTable[28] = -1f;
            RMasterTable[29] = -1f;
            RMasterTable[30] = -1f;
            RMasterTable[31] = -1f;
            RMasterTable[32] = -1f;
            RMasterTable[33] = -1f;
            RMasterTable[34] = -1f;
            RMasterTable[35] = -1f;
            RMasterTable[36] = 100f;    //目標（ゴール地点）

        }

        //------------------------------------------------------------------------------------------------//


        //------------------------------------------------------------------------------------------------//
        //  α省略仕様：
        //  Q(状態、アクション)=R(状態、アクション）+ γ ×Max(Q(次の状態、すべてのアクション))
        //
        //  標準仕様：
        //  Q(状態、アクション)=(1-α) Q(状態、アクション) +  α(R(状態、アクション + γ ×Max(Q(次の状態、すべてのアクション)))
        //
        //  Sarsaの場合：Max Qではなく１つずつ全部アクションを更新する。重い。
        //
        //  モンテカルロ法：プラスの報酬を獲得するまでQMasterTableを一切更新しない。成果主義。
        //
        //  ある程度Q値を取得したところで非線形関数近似を行い、
        //  すべてのQ値をプロットしなくても関数でQ値を推定できるようにします。
        //  この関数近似にニューラルネットワーク技術を適用し、近似精度を向上させたものをDQNと呼びます。 
        //
        //  参照元：
        //  https://products.sint.co.jp/aisia/blog/vol1-12
        //
        //  状態（列）、アクション（行）はList<double>から参照するローカル変数int indexで実装します。
        //  例えば、状態が6つあればアクションも6つあることになります。このとき、QTableのセルの数は6*6=36通りです。
        //  index=状態（列）[0～5]*アクション（行）[0～5]で実装できます。
        //  複数Setしたい場合はList<int>化して下さい。
        //------------------------------------------------------------------------------------------------//

        /// <summary>
        /// 指定したIndexのmaxQ値よりも新しいQ値のほうが大きければ更新します。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="Q"></param>
        protected void SetMaxQ(int index, double Q, int state)
        {
            //index 0～max
            if (index < QMasterTable.Count)  // ex: 0<0_false, 0<1_true
            {
                //指定したindexになるまで0fを追加
                int loop = QMasterTable.Count - index;
                for (int i = 0; i < loop; i++)
                {
                    QMasterTable.Add(0f);
                }
            }

            //報酬
            double reward = (double)GetReward(state);

            //文字列出力
            sb.Append("[reward=");
            sb.Append(reward);
            sb.Append("]");

            //Qは過去最大値か？
            if (Q > QMasterTable[index])
            {
                //Q値を更新
                QMasterTable[index] = (1 - cAlpha) * Q + cAlpha * (reward + Q * cGammma);
                //文字列出力
                sb.Append("[QMasterTable[");
                sb.Append(index);
                sb.Append("]=");
                sb.Append(QMasterTable[index]);
                sb.Append("];");
            }

            return;
        }

        /// <summary>
        /// 指定したList<int>に含まれる全indexの最大値、MaxQ値*Gammmaを返します。
        /// </summary>
        /// <paramref name="QTableIndex"/></param>
        /// <returns>double MaxQValue * Gammma</returns>
        protected double GetMaxQ(List<int>QTableIndex)
        {
            //宣言と初期化
            double maxQ = 0f;

            for (int i = 0; i < QTableIndex.Count; i++)
            {
                int index = QTableIndex[i];

                //index 0～max
                if (index < QMasterTable.Count)  // ex: 0<0_false, 0<1_true
                {
                    //QTableにindexが見つからなければ0f扱い
                    maxQ = Math.Max(maxQ, 0f);
                }

                //indexのQValueを返す
                maxQ = Math.Max(maxQ, QMasterTable[index]);
            }

            //最終的な R(状態, アクション) + maxQ値 * Gammmaを返す（maxQの中に全部入っています）
            return maxQ;
        }


        /// <summary>
        /// 報酬Rewardを返すプロシージャです。
        /// </summary>
        /// <param name="state">状態State</param>
        /// <returns></returns>
        protected double GetReward(int state)
        {
            double reward = 0f;

            //報酬をセット
            reward = (double)RMasterTable[state];

            //----------------------------------------
            //報酬を細かく弄りたい場合はここに記述する
            //----------------------------------------

            //報酬を返す
            return (double)reward;
        }


        /// <summary>
        /// エピソード(0～n)をcMaxQ回実行します。
        /// </summary>
        /// <param name="startEpisodeNo"></param>
        protected void SetEpisode(int startEpisodeNo = 0)
        {
            //startEpisodeNo >= 0
            startEpisodeNo = Math.Max(startEpisodeNo, 0);

            if (startEpisodeNo == 0)
            {
                //最初の１回のみQMasterTableを初期化
                this.SetQInit();
            }


            //データがほしい状態とアクションを決めてindexを確定させる。
            for (int i=0; i<cMaxQ + startEpisodeNo; i++){

                //------------------------[EPISODE i]------------------------//
                sb.Append("[EPISODE:");
                sb.Append(i);
                sb.Append("]");

                //エピソードの状態をリセット
                int state = this.SetInitState();
                int action = 0;

                int saveState = 0;
                int index = 0;
                double maxQ = 0f;

                //１エピソードにつき最大100回まで試行
                const int loopmax = 100;
                for (int m = 0; m < loopmax; m++)
                {

                    //-----------------------------------------------------------------
                    //宣言と初期化
                    List<int> stateList = new List<int>(); //状態リスト
                    double perQ = 0f;   // Q確率

                    //シード値をランダムにセット & 0x0000FFFF は桁あふれ防止の切り捨て処理
                    Random random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
                    //-----------------------------------------------------------------


                    if (cDictionarySW)
                    {
                        //------------------------------------------------------------------------
                        //ある程度QMasterTableが埋まったら、QMasterTableをもとに決定するとよい。
                        index = this.GetIndex(state, action);   //前回のactionは保存されている。初期状態は不動のため初期値の0でよい。
                                                                    //初期化
                        stateList = new List<int>();
                        stateList.Add(index);
                        maxQ = this.GetMaxQ(stateList);
                        if (QMasterTable.Count > 0)
                        {
                            if (QMasterTable.Max() != 0)
                            {
                                //最大値で割るとパーセンテージが出る
                                perQ = (double)(QMasterTable[index] / QMasterTable.Max());
                            }
                        }
                        //初期化
                        stateList = new List<int>();
                        //stateをaction方向(0:不動、1:左、2:下、3:右、4:上)に進める（変更可能）
                        saveState = state;
                        if (action == 0)
                        {
                            //不動
                        }
                        else if (action == 1)
                        {
                            state -= cX;
                        }
                        else if (action == 2)
                        {
                            state += cY;
                        }
                        else if (action == 3)
                        {
                            state += cX;
                        }
                        else if (action == 4)
                        {
                            state -= cY;
                        }

                        //はみだし
                        if (state > cState || state < 0)
                        {
                            state = saveState;
                            action = 0; //不動に変更
                        }
                        //------------------------------------------------------------------------
                    }
                    else
                    {
                        //------------------------------------------------------------------------
                        //↓はランダム。初期段階のテスト中は局所解を避けるためにランダムでよい。
                        //------------------------------------------------------------------------
                        //  今回のactionを決定（ランダム）
                        action = random.Next(0, cAction);
                        //------------------------------------------------------------------------
                    }


                    //------------------------------------------------------------------------
                    //今回Action実行
                    //stateをaction方向(0:不動、1:左、2:下、3:右、4:上)に進める（変更可能）
                    saveState = state;
                    if (action == 0)
                    {
                        //不動
                    }
                    else if (action == 1)
                    {
                        state -= cX;
                    }
                    else if (action == 2)
                    {
                        state += cY;
                    }
                    else if (action == 3)
                    {
                        state += cX;
                    }
                    else if (action == 4)
                    {
                        state -= cY;
                    }

                    //はみだし
                    if(state > cState || state < 0)
                    {
                        state = saveState;
                        action = 0;
                    }

                    //------------------------------------------------------------------------

                    //今回のAction後に「次のActionで取得可能なmaxQ」をとる
                    for (int p=0; p<cAction; p++)
                    {
                        //※選択不可能なp(=action)があればここで条件分岐し、falseならcontinueする。

                        index = this.GetIndex(state, p);
                        stateList.Add(index);
                        maxQ = this.GetMaxQ(stateList);

                        if (QMasterTable.Count > 0)
                        {

                            if (QMasterTable.Max() != 0)
                            {
                                //最大値で割るとパーセンテージが出る
                                perQ = (double)(maxQ / QMasterTable.Max());
                            }
                        }
                    }


                    /*
                    //最大Q値を参照する それによって行動を決める    0～100の乱数→0f%～100f%に対応
                    if (random.Next(0, 101) < maxQ)
                    {
                        //YOU CAN SET ACTION A IN HIGH MAX_Q
                    }
                    else
                    {
                        //YOU CAN SET ACTION IN LOW MAX_Q
                    }
                    */

                    //-----------------------------------------------------------------

                    //文字列出力
                    sb.Append("[S=");
                    sb.Append(state);
                    sb.Append("][A=");
                    sb.Append(action);
                    sb.Append("][I=");
                    sb.Append(index);
                    sb.Append("][maxQ=");
                    sb.Append(maxQ);
                    sb.Append("][Q確率=");
                    sb.Append(perQ);
                    sb.Append("%];");

                    //現在のQ値を検証 報酬の獲得
                    this.SetMaxQ(index, maxQ, state);

                    //文字列出力（改行）
                    sb.Append("\r\f");

                    //------------------------------

                    //終了判定
                    if (this.GetIsEnd(state))
                    {
                        //強制終了
                        break;
                    }
                }

                //-----------------------------------------------------------//
                //                         EPSODE i END
                //-----------------------------------------------------------//
                //文字列出力（改行）
                sb.Append("[EPISODE ");
                sb.Append(i);
                sb.Append("END]\r\f");
            }
            sb.Append("[---LEARNING COMPLETED---]\r\f");
        }


        /// <summary>
        /// 状態Stateをリセットして返すプロシージャです。各エピソード開始時に必ず実行します。
        /// </summary>
        /// <returns>int state</returns>
        protected int SetInitState()
        {
            //---------------------------
            //初期Stateの設定はここで行う（変更可）
            //---------------------------

            return 0;
        }

        /// <summary>
        /// 状態Stateから終了かどうかを返すプロシージャです。trueなら終了（ゴール）します。
        /// </summary>
        /// <returns>bool</returns>
        protected bool GetIsEnd(int state)
        {
            //stateが最大値に達したら強制終了（変更可）
            if (state == cState) {
                return true;
            }
            else
            {
                return false;
            }
        }





    }
}