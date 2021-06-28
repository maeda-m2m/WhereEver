using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//参考：https://tech-blog.link-u.co.jp/2018/08/27/q-learning/


namespace WhereEver.ClassLibrary
{
    public class DQLClass
    {

        /* コマンドプロンプトでMain処理するときはこれを使用する
        class Program
        {           
            static void Main(string[] args)
            {
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                //var ql = new QLearning();
                var rb = new Seisan();
                rb.MainProcess(3000000);
                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                Console.WriteLine($"　{sw.ElapsedMilliseconds}ms");
                Console.Write("nEndn");
                Console.Read();
            }
        }
        */

        /// <summary>
        /// Q-Learningを実行します（出力はコメントアウトしてあります。好きにつけて下さい）
        /// 処理回数が1000000万回と非常に多いため、実行テストは保留にしてあります。TASK実行周辺が怪しいです。
        /// ここに書いたようなフルスクラッチをしなくても、PythonのライブラリをC#でラップしたほうが簡単です。
        /// いずれにせよ今積まれているGPUではTASKにしてもふっとびそうですので、むやみに実行しないで下さい。
        /// </summary>
        public void SetQLeearning(){
            var rb = new QLearning.Seisan();
            rb.MainProcess(3000000);
        }


        //基幹部の処理はどのQLでもほぼ同じ
        public class QLearning
        {
            public double[,] qValues;
            public double alpha;
            public double gamma;

            public QLearning(int sSize, int aSize, int fillValue, double alpha = 0.01, double gamma = 0.8)
            {
                this.alpha = alpha;
                this.gamma = gamma;
                this.qValues = new double[sSize, aSize];
                for (int i = 0; i < sSize; i++)
                {
                    for (int j = 0; j < aSize; j++)
                    {
                        this.qValues[i, j] = fillValue;
                    }
                }
            }

            public void updateQ(int situationNo, int nextSituation, int actionNo, double reward, List<int> unselectableActions = null, List<int> nextUnselectableActions = null)
            {
                int maxIndex = -1;      //初期値このまま
                double maxQ = -10000000;    //試行回数（負の値）デフォルトで-1000000回

                this.qValues[situationNo, actionNo] = (1 - this.alpha) * this.qValues[situationNo, actionNo]
                    + this.alpha * (reward + this.gamma * serachMaxAndArgmax(nextSituation, ref maxIndex, ref maxQ, nextUnselectableActions));
            }

            public int SelectActionByGreedy(int situationNo, List<int> unselectableActions = null)
            {
                unselectableActions = unselectableActions ?? new List<int>();
                int maxIndex = -1;      //初期値このまま
                double maxQ = -10000000;    //試行回数（負の値）デフォルトで-1000000回
                this.serachMaxAndArgmax(situationNo, ref maxIndex, ref maxQ, unselectableActions);
                return maxIndex;
            }

            public int SelectActionByEGreedy(double epsilon, int situationNo, List<int> unselectableActions = null)
            {
                Random r = new Random();
                if (r.NextDouble() < epsilon)
                {
                    int action = -1;
                    do
                    {
                        action = r.Next(this.qValues.GetLength(1));
                    } while (unselectableActions.Contains(action));
                    return action;
                }
                else
                {
                    return this.SelectActionByGreedy(situationNo, unselectableActions);
                }
            }

            private double serachMaxAndArgmax(int situationNo, ref int maxIndex, ref double maxQ, List<int> unselectableActions = null)
            {
                unselectableActions = unselectableActions ?? new List<int>();
                for (int j = 0; j < this.qValues.GetLength(1); j++)
                {

                    if (unselectableActions.Contains(j))
                    {
                        continue;
                    }
                    else if (this.qValues[situationNo, j] > maxQ)
                    {
                        maxIndex = j;
                        maxQ = this.qValues[situationNo, j];
                    }
                }

                return maxQ;
            }


            public void PrintQValues()
            {
                var rowCount = this.qValues.GetLength(0);
                var colCount = this.qValues.GetLength(1);
                for (int row = 0; row < rowCount; row++)
                {
                    for (int col = 0; col < colCount; col++) {
                        //Console.Write(String.Format("{0}t", this.qValues[row, col]));
                        //Console.WriteLine();
                    }
                }
            }
            //-----------------------------------------------------

            public abstract class Task
            {
                public abstract int MainProcess(int nEpisodes);
                public abstract int GetSituationSize();
                public abstract int GetActionSize();
                public abstract List<int> GetUnselectableActions(int situation);
                public abstract double CalcActionResult(int situationNo, ref int nextSituation, int actionNo);
            }

            //報酬
            public class Seisan : Task
            {
                //public const int PLAYER_DEATH = 27;
                //public const int ENEMY_DEATH = 28;

                //public const double PLAYER_DEATH_REWARD = -1000;
                //public const double ENEMY_DEATH_REWARD = 1000;

                public const int TASK_LIMIT_OVER = -2;  //8h労働ぶんを超えたらアウト
                public const int TASK_COMPLETE = -1;    //完遂

                public const double TASK_COMPLETE_REWARD = 1000;  //Reward １日ぶんのタスクが処理できた
                public const double TASK_LIMIT_OVER_REWARD = -1000; //Penalty(-Reward) タスクが１日で処理しきれなかった
                public const double TASK_ZAN_REWARD = -1500;      //Penalty(-Reward) 残業をした
                public const double TASK_LOSS_REWARD = -250; //Penalty(-Reward) タスクが処理できる余力を残したまま１日が終了した　余力n×定数a

                public const double MP_CONSUMPTION_REWARD_RATE = 0;

                //public Character player;
                //public Character enemy;
                //public List<Character> enemyList;

                //-----------------------------------------------------

                //定義
                private const int timeLimits = 8; //480min = 8h * 60min
                private const int myMaxTasks = 10;
                private static int myHoldTasks = 0;
                private static int myRequiredTaskCost = 0;
                private Dictionary<int, Action> actions;

                protected int GetMyHoldTasks()
                {
                    return myHoldTasks;
                }
                protected int GetMyMaxTasks()
                {
                    return myMaxTasks;
                }
                protected int GetMyTasks()
                {
                    Random rand = new Random();
                    myRequiredTaskCost = rand.Next(0,4);
                    return myRequiredTaskCost;
                }
                protected Dictionary<int, Action> SetActions()
                {
                    Dictionary<int, Action> actions = new Dictionary<int, Action>();
                        actions.Add(ActionTable.easyTask.id, ActionTable.easyTask);
                        actions.Add(ActionTable.normalTask.id, ActionTable.normalTask);
                        actions.Add(ActionTable.heaveyTask.id, ActionTable.heaveyTask);
                    return actions;
                }
                public Dictionary<int, Action> GetActions()
                {
                    return this.actions;
                }

                //行動種類
                public class Action
                {
                    public int id = 0;
                    public int wasteTimes = 0;
                    public int holdTasks = 0;

                    public bool taskSW = true;
                    public bool zan = false;

                    public Action(int id, int wasteTimes, int holdTasks, ref int count)
                    {
                        if (this.wasteTimes + wasteTimes > timeLimits)
                        {

                            if (this.wasteTimes + wasteTimes <= timeLimits + 2)//minなら+120
                            {
                                //残業なら間に合う
                                zan = true;
                            }
                            else
                            {

                                //間に合わない
                                zan = false;
                                taskSW = false;
                            }
                        }

                        //共通
                        this.id = id;
                        this.wasteTimes += wasteTimes;
                        this.holdTasks += holdTasks;

                        myRequiredTaskCost = wasteTimes;
                        myHoldTasks = holdTasks;


                        ++count;
                    }
                }

                //行動内容
                static class ActionTable
                {
                    public static int count;
                    public static Action easyTask;
                    public static Action normalTask;
                    public static Action heaveyTask;

                    static ActionTable()
                    {
                        count = 0;
                        easyTask = new Action(count, 60, 1, ref count);
                        normalTask = new Action(count, 120, 2, ref count);
                        heaveyTask = new Action(count, 180, 3, ref count);
                    }
                }

                public override int MainProcess(int nEpisodes)
                {
                    //this.player = new Player();
                    //this.SetEnemyList();

                    var q = new QLearning(
                        this.GetSituationSize(),
                        this.GetActionSize(),
                        0
                    );
                    int situationNo;
                    int nextSituationNo;
                    int e = 0;
                    while (e < nEpisodes)
                    {
                        //初期化
                        //this.enemy = this.SelectRandomEnemy();
                        //this.enemy = this.enemyList[e % 3];
                        //player.RestoreHp();
                        //player.RestoreMp();
                        //enemy.RestoreHp();
                        //enemy.RestoreMp();
                        do
                        {
                            situationNo = this.GetSituationNo();
                            nextSituationNo = -1;
                            var unselectable = this.GetUnselectableActions(situationNo);
                            int actionNo = q.SelectActionByEGreedy(0.05, situationNo, unselectable);

                            double reward = this.CalcActionResult(situationNo, ref nextSituationNo, actionNo);
                            var nextUnselectable = this.GetUnselectableActions(nextSituationNo);
                            q.updateQ(situationNo, nextSituationNo, actionNo, reward, unselectable, nextUnselectable);

                        } while (nextSituationNo != TASK_LIMIT_OVER && nextSituationNo != TASK_COMPLETE);

                        if (e % 1000 == 0)
                        {
                            //結果表示１
                            //Console.WriteLine();
                            //Console.Write(String.Format("Progress {0:f4}%", (double)100 * e / nEpisodes));
                            //Console.WriteLine();
                            //this.PrintQValuesWithParams(q);
                        }
                        e++;
                    }

                    //結果表示２
                    //Console.WriteLine();
                    //this.PrintQValuesWithParams(q);

                    return 0;
                }


                //------------------------------------------------------


                //シチュエーションのカウントを定義（オーバーライド）全部で何通りの「状態」があるのか？
                public override int GetSituationSize()
                {
                    /*
                    int enemyKindCount = 3;
                    int hpSituationCount = 3;
                    int mpSituationCount = 3;
                    //自分死亡(PLAYER_DEATH)と相手死亡(ENEMY_DEATH)状態も足す
                    return enemyKindCount * hpSituationCount * mpSituationCount + 2;
                    */
                    return 90;   //=rewuiredTask種別3通り * holdTask状態5通り * timeLimit6通り
                }





                //----------------------------------------------------
                //評価関数
                //----------------------------------------------------
                public int GetSituationNo()
                {
                    //return 9 * (this.enemy.GetId() - 1) + 3 * this.GetHpSituation() + this.GetMpSituation();
                    return this.GetHoldTaskSituation();   //乗算して重み付けし、それぞれ加算して評価を出す。GetSituationIndexByParamsやGetParamsBySituationIndexと同じ評価関数にする必要あり。
                }

                private int GetSituationIndexByParams(int holdTaskSituation) //(int charactterId, int hpSituation, int mpSituation)
                {
                    //return 9 * (charactterId - 1) + 3 * hpSituation + mpSituation;
                    return holdTaskSituation;   //乗算して重み付けし、それぞれ加算して評価を出す。GetSituationNoやGetParamsBySituationIndexと同じ評価関数にする必要あり。
                }

                //上記２つをもとに評価関数からもとに戻す関数をつくる
                private void GetParamsBySituationIndex(int situationId, int holdTaskSituation)    //(int situationId, ref int characterId, ref int hpSituation, ref int mpSituation)
                {
                    //characterId = situationId / 9;
                    //hpSituation = (situationId - 9 * characterId) / 3;
                    //mpSituation = situationId - 9 * characterId - 3 * hpSituation;
                    //++characterId;

                    holdTaskSituation = situationId * holdTaskSituation;
                }
                //-----------------------------------------------------
                //行動条件をパターン分け
                private int GetHoldTaskSituation()
                {
                    if (this.GetMyHoldTasks() > 8)
                    {
                    }


                        if (this.GetMyHoldTasks() >= 10)
                    {
                        //過多
                        return 6;
                    }
                    else if (this.GetMyHoldTasks() >= 9)
                    {
                        //多量
                        return 5;
                    }
                    else if (this.GetMyHoldTasks() >= 7)
                    {
                        //標準
                        return 4;
                    }
                    else if (this.GetMyHoldTasks() >= 3)
                    {
                        //少数
                        return 2;
                    }
                    else if (this.GetMyHoldTasks() >= 1)
                    {
                        //過少
                        return 1;
                    }
                    else
                    {
                        //なし
                        return 0;
                    }
                }
                //----------------------------------------------------


                //生産タスク最大値に要改変
                public override int GetActionSize()
                {
                    return this.GetActions().Count;
                }


                //要改造
                public override List<int> GetUnselectableActions(int situation)
                {
                    if (situation == TASK_LIMIT_OVER)
                    {
                        return new List<int>();
                    }

                    if (situation == TASK_COMPLETE)
                    {
                        return new List<int>();
                    }

                    List<int> unselectableActions = new List<int>();

                    foreach (KeyValuePair<int, Action> a in this.GetActions())
                    {
                        //TASK処理に必要な時間は足りているか？
                        if (timeLimits - a.Value.wasteTimes <= this.GetMyHoldTasks() + this.GetMyTasks())  //1hあたりの既存タスクと現在タスクを足したタスク量が、残り時間(タスク/h)以上
                        {
                            unselectableActions.Add(a.Value.id);
                        }
                    }

                    return unselectableActions;
                }



                //結果を取得する処理
                public override double CalcActionResult(int situationNo, ref int nextSituation, int actionNo)
                {

                    //時間切れ　報酬（罰）
                    if (situationNo == TASK_LIMIT_OVER)
                    {
                        nextSituation = TASK_LIMIT_OVER;
                        return TASK_LIMIT_OVER_REWARD;
                    }

                    //タスク完遂　報酬（賞）
                    if (situationNo == TASK_COMPLETE)
                    {
                        nextSituation = TASK_COMPLETE;
                        return TASK_COMPLETE_REWARD;
                    }


                    /* memo
                    TASK_COMPLETE_REWARD = 1000f;  //Reward １日ぶんのタスクが処理できた
                    TASK_LIMIT_OVER_REWARD = -1000f; //Penalty(-Reward) タスクが１日で処理しきれなかった
                    TASK_ZAN_REWARD = -1500f;      //Penalty(-Reward) 残業をした
                    TASK_LOSS_REWARD = -250f; //Penalty(-Reward) タスクが処理できる余力を残したまま１日が終了した　余力n×定数a                     
                     */


                    Action playerAction = this.GetActions()[actionNo];  //疑問点：Q値ごとに、actionNoで分類できているか？

                    //何もしなくても時間は経過していく
                    playerAction.wasteTimes += 1;


                    if (playerAction.wasteTimes > timeLimits)
                    {
                        //時間切れ（条件分岐）

                        if (playerAction.wasteTimes > timeLimits + 2)
                        {
                            //残業時間オーバー（失敗）
                            nextSituation = TASK_LIMIT_OVER;
                            return TASK_LIMIT_OVER_REWARD + TASK_ZAN_REWARD;
                        }
                    }

                    if (myMaxTasks == playerAction.holdTasks)
                    {
                        //タスク完遂
                        if (playerAction.wasteTimes > timeLimits + 2)
                        {
                            //残業時間オーバー（完遂）
                            nextSituation = TASK_COMPLETE;
                            return TASK_COMPLETE_REWARD + TASK_ZAN_REWARD;
                        }
                        else
                        {
                            //配分成功
                            nextSituation = TASK_COMPLETE;
                            return TASK_COMPLETE_REWARD;
                        }
                    }
                    else if (myMaxTasks > playerAction.holdTasks)
                    {
                        //オーバータスク
                        if (playerAction.wasteTimes > timeLimits + 2)
                        {
                            //残業時間オーバー（失敗）
                            nextSituation = TASK_LIMIT_OVER;
                            return TASK_LIMIT_OVER_REWARD + TASK_ZAN_REWARD;
                        }
                        else
                        {
                            //配分失敗
                            nextSituation = TASK_LIMIT_OVER;
                            return TASK_LIMIT_OVER_REWARD;
                        }

                    }

                    /* コピー元の処理

                    if (situationNo == PLAYER_DEATH)
                    {
                        nextSituation = PLAYER_DEATH;
                        return PLAYER_DEATH_REWARD;
                    }
                    if (situationNo == ENEMY_DEATH)
                    {
                        nextSituation = ENEMY_DEATH;
                        return ENEMY_DEATH_REWARD;
                    }

                    int playerHp = this.player.GetHp();
                    int playerMp = this.player.GetMp();
                    int playerMpOrg = playerMp;
                    int enemyHp = this.enemy.GetHp();
                    int enemyMp = this.enemy.GetMp();

                    Action playerAction = this.player.GetActions()[actionNo];
                    string playerAttackAttribute = playerAction.attribute;
                    enemyHp -= (int)(playerAction.hpDamage * this.enemy.GetWeekness()[playerAttackAttribute]);
                    playerHp += this.player.GetActions()[actionNo].hpHeal;
                    if (playerHp > this.player.GetMaxHp())
                        playerHp = this.player.GetMaxHp();
                    playerMp -= this.player.GetActions()[actionNo].mpCost;
                    if (enemyHp <= 0)
                    {
                        nextSituation = ENEMY_DEATH;
                        return ENEMY_DEATH_REWARD;
                    }
                    Action enemyAction = this.SelectRandomAction(enemy);
                    string enemyAttackAttribute = enemyAction.attribute;
                    playerHp -= (int)(enemyAction.hpDamage * this.player.GetWeekness()[enemyAttackAttribute]);
                    enemyHp += enemyAction.hpHeal;
                    if (enemyHp > this.enemy.GetMaxHp())
                        enemyHp = this.enemy.GetMaxHp();
                    enemyMp -= enemyAction.mpCost;
                    if (playerHp <= 0)
                    {
                        nextSituation = PLAYER_DEATH;
                        return PLAYER_DEATH_REWARD;
                    }
                    this.player.SetHp(playerHp);
                    this.player.SetMp(playerMp);
                    this.enemy.SetHp(enemyHp);
                    this.enemy.SetMp(enemyMp);


                    nextSituation = this.GetSituationNo();

                    return (playerMp - playerMpOrg) * MP_CONSUMPTION_REWARD_RATE;

                    */


                    nextSituation = this.GetSituationNo();
                    return 0f;//仮
                }



                /*
                public void PrintQValuesWithParams(QLearning q)
                {
                    var rowCount = q.qValues.GetLength(0);
                    var colCount = q.qValues.GetLength(1);

                    int charaId = -1;
                    int hpSituation = -1;
                    int mpSituation = -1;

                    for (int row = 0; row < rowCount; row++)
                    {
                        if (row >= PLAYER_DEATH)
                            continue;
                    }

                    this.GetParamsBySituationIndex(row, ref charaId, ref hpSituation, ref mpSituation);

                    //Console.Write(String.Format("{0}t{1}t{2}", this.enemyList[charaId - 1], hpSituation, mpSituation));
                    //Console.WriteLine();

                    //for (int col = 0; col < colCount; col++){
                    //Console.Write(String.Format("{0}t", q.qValues[row, col]));
                    //Console.WriteLine();
                    //}
                }
                */
            }
        }

    }
}

