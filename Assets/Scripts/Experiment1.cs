using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;
using System.Reflection;
using UnityEngine.InputSystem.LowLevel;
using Valve.VR;

public class Experiment1 : MonoBehaviour
{
    //ステート用変数
    State currentState = State.Idle;
    private State _nextState;

    //現在実験中のパターンがパターン１ならtrue
    //private bool isPattern1 = true;

    //オブジェクト宣言
    public GameObject lefthand;
    public GameObject righthand;
    //GameObject cube;
    public GameObject cube;
    private UnityEngine.Component component;
    //Text変化用
    public UnityEngine.UI.Text questionnaireText;
    public SaveCsvScript saveCsvScript;

    //InteractUIボタンが押されてるのかを判定するためのIuiという関数にSteamVR_Actions.default_InteractUIを固定
    private SteamVR_Action_Boolean Iui = SteamVR_Actions.default_InteractUI;
    private SteamVR_Action_Boolean Tpb = SteamVR_Actions.default_TrackPadButton;
    //結果の格納用Boolean型関数interacrtui
    private Boolean interacrtuiRight;
    private Boolean interacrtuiLeft;
    private Boolean trackPadButtonRight;
    private Boolean trackPadButtonLeft;

    // 初期位置
    private Vector3 initialCubePosition = new Vector3(0, 0.90f, 3.1f);

    //階段法の終了判定を行う折り返し回数を記録する変数
    private int orikaeshiNum_Low = 0;
    private int orikaeshiNum_High = 0;
    //orikaeshiNumの片方が5に到達した際にlow_or_highのどちらかをブロックするための変数
    private int block_Low_High = 2;

    private float startExperiment; //実験の開始時間
    private float startAnswer; // 計測の開始時間を格納する変数

    //基本的に値は小数点を扱わないように1000倍して操作する
    [Header("ゲインの上限値と下限値と比較値(1000倍)")]
    [SerializeField] private const int LowGainValue = 400;
    [SerializeField] private const int HighGainValue = 1600;
    [SerializeField] private const int RefGainValue = 1000;

    private const string Reference = "reference";
    private const string Comparison = "comparison";


    //質問に回答した際に変動する値(定数)
    [Header("ゲインの変化量(1000倍)")]
    [SerializeField] private int ChangeValue = 100;

    private int gain_Low;
    private int gain_High;

    //質問に回答した際に変動する値(この変数の値は基本的に操作しない)
    private int APValue;

    //アンケートの回答をYes→true、No→falseで記録する変数
    //private bool answer = true;
    private string answer;


    [Header("パターン1かパターン2のどちらを実験するかをチェック)")]
    [SerializeField] private bool pattern1 = false;
    [SerializeField] private bool pattern2 = false;

    [Header("手のサイズ小と大の値（基準値1.0）)")]
    private Vector3 HandSizeLow = new Vector3(0.75f, 0.75f, 0.75f);
    private Vector3 HandSizeHigh = new Vector3(1.25f, 1.25f, 1.25f);
    private Vector3 HandSizeRef = new Vector3(1.0f, 1.0f, 1.0f);

    //0なら基準値とLowの比較を先に行う
    private int low_or_high = 0;

    //0なら基準値から先に値をセットする
    private int ref_or_com = 0;

    //セット内何回目かを記録
    private int startCount = 0;

    //実験中に実際に使う変数（�@か�Cの数値がAに、�Aか�Bの数値がBに入る）
    private Vector3 handSize_A = Vector3.zero;
    private Vector3 handSize_B = Vector3.zero;
    private int gain_A = 0;
    private int gain_B = 0;


    //今が�Aを比較しているかどうかを記録する変数
    //des→�A、Asc→�B
    private string preAnswerDes;
    private bool isFirstDes = true;
    private string preAnswerAsc;
    private bool isFirstAsc = true;
    private bool isContinue_high = true;
    private bool isContinue_low = true;
    private bool permitAdd;

    List<float> answerList = new List<float>();

    enum State
    {
        Idle,
        Set,
        Start,
        Lift1,
        Set2,
        Lift2,
        Answer,
        End
    }

    void ChangeState(State newState)
    {
        _nextState = newState;
    }


    // Start is called before the first frame update
    void Start()
    {
        //初期設定
        if (pattern1 && !pattern2)
        {
            handSize_A = HandSizeLow;
        }
        else if (!pattern1 && pattern2)
        {
            handSize_A = HandSizeHigh;
        }
        if (pattern1 ^ pattern2 == false)
        {
            //エラー吐いて止まるように
            Debug.LogError("インスペクターから実験条件を片方だけ選択してチェックしてください（両方チェックもダメ）");
            UnityEditor.EditorApplication.isPlaying = false;

        }
        ValueInit();
    }

    //変数の初期化
    void ValueInit()
    {
        gain_Low = LowGainValue;
        gain_High = HighGainValue;
        APValue = ChangeValue;

        gain_A = RefGainValue;
        handSize_B = HandSizeRef;

        // インタラクタブルコンポーネントの取得
        component = cube.GetComponent<Valve.VR.InteractionSystem.Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        //Stateに応じた挙動
        StateFunction();
    }

    IEnumerator DelayMethod(float delay, State newState)
    {
        //delay秒待つ
        yield return new WaitForSeconds(delay);
        ChangeState(newState);
    }


    void StateFunction()
    {
        //現在のStateに応じたUpdate処理を行う
        switch (currentState)
        {
            case State.Idle:
                //Enter押すと実験開始
                //if (Input.GetKeyDown(KeyCode.Return))
                //trackPadButtonRight = Tpb.GetState(SteamVR_Input_Sources.RightHand);
                questionnaireText.fontSize = 80;
                questionnaireText.text = "Before start\n準備ができたらトラックパッドのボタンを押してください\nPress the TrackPadButton when you are ready";

                //左のトラックパッドボタンでCubeの位置を初期位置にリセット
                if (Tpb.GetStateDown(SteamVR_Input_Sources.LeftHand))
                {
                    SetCubePosition(initialCubePosition);
                }

                if (Tpb.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    ChangeState(State.Set);
                }
                break;
            case State.Set:
                
            case State.Start:
                //必要なら待機時間を設けて、その後Lift1に遷移
                if (Tpb.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    ChangeState(State.Lift1);
                    //StartCoroutine(DelayMethod(1.0f, State.Lift1));
                }
                break;
            case State.Lift1:
                //左のトラックパッドボタンでCubeの位置を初期位置にリセット
                if (Tpb.GetStateDown(SteamVR_Input_Sources.LeftHand))
                {
                    SetCubePosition(initialCubePosition);
                }
                //持ち上げが終わったらLift2へ遷移
                if (Tpb.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    ChangeState(State.Set2);
                }
                break;

            case State.Set2:
                //必要なら待機時間を設けて、その後Startに遷移
                break;
            case State.Lift2:
                //左のトラックパッドボタンでCubeの位置を初期位置にリセット
                if (Tpb.GetStateDown(SteamVR_Input_Sources.LeftHand))
                {
                    SetCubePosition(initialCubePosition);
                }
                //持ち上げが終わったらLift1へ遷移
                if (Tpb.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    ChangeState(State.Answer);
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    ChangeState(State.Lift1);
                }
                break;
            case State.Answer:
                //アンケート回答受付
                //回答に応じたゲインの変更や適切なステートへの遷移を行う

                //アンケート表示
                //アンケート回答されたらSetに戻る
                //結果をGetStateで取得してinteracrtuiに格納
                //SteamVR_Input_Sources.機器名（今回は左コントローラ）
                interacrtuiRight = Iui.GetState(SteamVR_Input_Sources.RightHand);
                interacrtuiLeft = Iui.GetState(SteamVR_Input_Sources.LeftHand);

                //if (Input.GetKeyDown(KeyCode.Y))
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    ChangeState(State.Lift2);
                }
                if (interacrtuiLeft)
                {
                    //回答をもとにゲインの変更を行う
                    if (ref_or_com == 0)
                    {
                        answer = Reference;
                        saveCsvScript.SaveData(gain_B.ToString(), answer, low_or_high.ToString(), ref_or_com.ToString(), orikaeshiNum_High.ToString(), orikaeshiNum_Low.ToString());

                    }
                    else
                    {
                        answer = Comparison;
                        saveCsvScript.SaveData(gain_B.ToString(), answer, low_or_high.ToString(), ref_or_com.ToString(), orikaeshiNum_High.ToString(), orikaeshiNum_Low.ToString());
                        //saveCsvScript.SetAnswerData(genericStickyPlane.GetVisualIndicator(), conditionAHeight);
                    }
                    ChangeGain(answer);
                }
                //else if(Input.GetKeyDown(KeyCode.N))
                else if (interacrtuiRight)
                {
                    if (ref_or_com == 0)
                    {
                        answer = Comparison;
                        saveCsvScript.SaveData(gain_B.ToString(), answer, low_or_high.ToString(), ref_or_com.ToString(), orikaeshiNum_High.ToString(), orikaeshiNum_Low.ToString());

                    }
                    else
                    {
                        answer = Reference;
                        saveCsvScript.SaveData(gain_B.ToString(), answer, low_or_high.ToString(), ref_or_com.ToString(), orikaeshiNum_High.ToString(), orikaeshiNum_Low.ToString());
                        //saveCsvScript.SetAnswerData(genericStickyPlane.GetVisualIndicator(), conditionBHeight);
                    }
                    //回答をもとにゲインの変更を行う
                    ChangeGain(answer);
                }
                break;
            case State.End:

                //他のステートに遷移しないようにする（このステートに入った時点で実験終了）
                break;
        }

        // ステートが切り替わったら
        if (currentState != _nextState)
        {
            // 終了処理を呼び出し（切り替わり時に一度のみ呼び出し）
            switch (currentState)
            {
                case State.Idle:

                    break;
                case State.Set:
                    //比較1回目の条件を決定

                    Debug.Log("StartCount: " + startCount);
                    if (block_Low_High == 2)
                    {
                        if (startCount % 2 == 0)
                        {
                            low_or_high = UnityEngine.Random.Range(0, 2);
                        }
                        else if (startCount % 2 == 1)
                        {
                            if (low_or_high == 0)
                            {
                                low_or_high = 1;
                            }
                            else if (low_or_high == 1)
                            {
                                low_or_high = 0;
                            }
                        }

                    }
                    else if (block_Low_High == 0)
                    {
                        low_or_high = 1;
                    }
                    else if (block_Low_High == 1)
                    {
                        low_or_high = 0;
                    }
                    //開始前処理
                    cube.SetActive(false);
                    lefthand.SetActive(false);
                    righthand.SetActive(false);

                    //low_or_high = 1;
                    Debug.Log("<color=yellow>block_Low_High: </color>" + block_Low_High);
                    Debug.Log("low_or_high: " + low_or_high);
                    break;
                case State.Start:
                    //持ち上げ１，２回目の各条件を決定（その後Lift1に遷移）
                    ref_or_com = UnityEngine.Random.Range(0, 2);
                    //ref_or_com = 1;
                    Debug.Log("ref_or_com: " + ref_or_com);
                    break;
                case State.Lift1:
                    //オブジェクトを消すなどのLift1の終了処理を記述（その後Lift2へ遷移）
                    //実装
                    cube.SetActive(false);
                    lefthand.SetActive(false);
                    righthand.SetActive(false);
                    break;
                case State.Set2:
                    //

                    break;
                case State.Lift2:
                    //オブジェクトを消すなどのLift2の終了処理を記述（その後Answerへ遷移）
                    cube.SetActive(false);
                    lefthand.SetActive(false);
                    righthand.SetActive(false);

                    break;
                case State.Answer:
                    //回答を保存

                    break;
                case State.End:

                    break;
            }

            // 次のステートに遷移する
            //ステートの開始処理を行う（切り替わり時に一度のみ呼び出し）
            currentState = _nextState;
            switch (currentState)
            {
                case State.Idle:

                    break;
                case State.Set:
                    questionnaireText.fontSize = 100;
                    questionnaireText.text = "Set";
                    //必要なら待機時間を設けて、その後Startに遷移
                    StartCoroutine(DelayMethod(0.5f,State.Start));
                    break;
                case State.Start:
                    questionnaireText.fontSize = 80;
                    questionnaireText.text = "準備ができたらトラックパッドのボタンを押してください\nPress the TrackPadButton when you are ready";
                    if (low_or_high == 0)
                    {
                        gain_B = gain_Low;
                    }

                    //�Bから先に比較する場合
                    else if (low_or_high == 1)
                    {
                        gain_B = gain_High;
                    }
/*
                    //�Aから先に比較する場合
                    if (low_or_high == 0)
                    {
                        isDes = false;
                        if (startCount % 2 == 0)
                        {
                            startCount++;
                            gain_B = gain_Low;
                        }
                        else
                        {
                            startCount = 0;
                            gain_B = gain_High;
                        }
                    }

                    //�Bから先に比較する場合
                    else if (low_or_high == 1)
                    {
                        isDes = true;
                        if (startCount % 2 == 0)
                        {
                            startCount++;
                            gain_B = gain_High;
                        }
                        else
                        {
                            startCount = 0;
                            gain_B = gain_Low;
                        }
                    }*/
                    Debug.Log("StartCount: " + startCount);
                    break;
                case State.Lift1:
                    //1回目の持ち上げ条件を適用する
                    questionnaireText.fontSize = 100;
                    questionnaireText.text = "Lift1";
                    SetCubePosition(initialCubePosition);

                    //キューブをアクティブにする
                    cube.SetActive(true);
                    lefthand.SetActive(true);
                    righthand.SetActive(true);
                    if (ref_or_com == 0)
                    {
                        //�@か�Cの条件のうちこの段階で選ばれているものをオブジェクトと手に適用する（これは関数を作る）
                        SetGainAndHandSize(handSize_A, gain_A);
                    }
                    else
                    {
                        //�Aか�Bの条件のうちこの段階で選ばれているものをオブジェクトと手に適用する（これは関数を作る）
                        SetGainAndHandSize(handSize_B, gain_B);
                    }
                    break;
                case State.Set2:
                    questionnaireText.fontSize = 100;
                    questionnaireText.text = "Preparing for Lift2";
                    //必要なら待機時間を設けて、その後Startに遷移
                    StartCoroutine(DelayMethod(0.5f, State.Lift2));
                    break;
                case State.Lift2:
                    questionnaireText.fontSize = 100;
                    questionnaireText.text = "Lift2";
                    SetCubePosition(initialCubePosition);
                    //キューブとハンドをアクティブにする
                    cube.SetActive(true);
                    lefthand.SetActive(true);
                    righthand.SetActive(true);

                    //2回目の持ち上げ条件を適用する
                    if (ref_or_com == 0)
                    {
                        //�Aか�Bの条件のうちこの段階で選ばれているものをオブジェクトと手に適用する（これは関数を作る）
                        SetGainAndHandSize(handSize_B, gain_B);
                    }
                    else
                    {
                        //�@か�Cの条件のうちこの段階で選ばれているものをオブジェクトと手に適用する（これは関数を作る）
                        SetGainAndHandSize(handSize_A, gain_A);
                    }
                    break;
                case State.Answer:
                    //質問を表示
                    SetCubePosition(initialCubePosition);
                    questionnaireText.fontSize = 80;
                    questionnaireText.text = "Which object is heavier?? \ncondition 1 → left trigger \ncondition 2 → right trigger";
                    Debug.Log("gain_B" + gain_B);
                    startCount++;
                    startAnswer = Time.time; // 開始時間を格納

                    break;
                case State.End:
                    //終了処理
                    questionnaireText.fontSize = 100;
                    questionnaireText.text = "End";
                    Debug.Log("EndSave");
                    saveCsvScript.EndSave();
                    break;
            }
        }
    }

    private void SetGainAndHandSize(Vector3 handSize, int gain)
    {
        //手とオブジェクトにhandSizeとgainを適用する
        //手の処理
        GameObject[] hands = GameObject.FindGameObjectsWithTag("SizeHand");
        foreach (GameObject hand in hands)
        {
            hand.transform.localScale = handSize;
        }
        //Gainの処理
        MethodInfo methodInfo = component.GetType().GetMethod("VariableChanger");
        float inputGain = IntToFloat(gain);
        object[] parameters = new object[] { inputGain };
        methodInfo.Invoke(component, parameters);
    }


    //ゲイン変更処理
    void ChangeGain(string answer)
    {
        //desかascのどちらのゲインを変更するかを判定
        if (low_or_high == 1)
        {
            ProcessGain(answer, ref gain_High);

        }
        else
        {
            ProcessGain(answer, ref gain_Low);

        }
        //記録
        //実験条件を切り替え(ゲイン処理が終わってから切り替えに変更)
        //SendCSV();
        SwitchCondition();
    }

    //ゲイン変更に必要な処理をすべて関数化
    //引数：回答(yes=true,no=false), 変更したいゲインの変数(参照渡しする)
    void ProcessGain(string ans, ref int gain)
    {
        //回答がYesならゲインを加算、Noなら減算
        //呼び出された際の条件がAscかDesかを確認して対応する値のみ変更
        if (ans == Reference)
        {
            gain -= APValue;
        }
        else if (ans == Comparison)
        {
            gain += APValue;
        }
        //Debug.Log("Gain : "+gain);

        //折り返しがあるかどうかの確認と処理
        CheckTurnBack(ans, ref gain);

        //現在の値をリスト末尾に追加
        if (permitAdd)
        {
            RecordList(gain);
        }
        else
        {
            permitAdd = true;
        }

        //現在の値を表示(デバッグ用)
        //Debug.Log((isDes ? "des" : "asc") + " - SlideGain: " + gain);
    }

    //折り返しの有無と回数、フラグの処理を行う
    //引数：回答(yes=true,no=false), ゲインの変数
    void CheckTurnBack(string ans, ref int gain)
    {
        //もしゲインが上限/下限値をオーバーしていた場合、値を上限/下限に固定してカウント加算
        if (gain < LowGainValue)
        {
            gain = LowGainValue;
            CountProcess(gain);
        }
        else if (gain > HighGainValue)
        {
            gain = HighGainValue;
            CountProcess(gain);
        }

        //回答が前回と違うかを確認して、違う場合には折り返し回数の増加とpreAnswerの変更、終了条件の確認と処理を行う
        if (low_or_high == 1)
        {
            if (isFirstDes)
            {
                //1回目の処理
                preAnswerDes = ans;
                isFirstDes = false;
                //Debug.Log("preAnswerDesA" + preAnswerDes);
                //Debug.Log("Trial:1 Gain = 1.0/0.25");
            }
            if (ans != preAnswerDes)
            {
                CountProcess(gain);
                //preAnswerを変更
                preAnswerDes = ans;
                //Debug.Log("preAnswerDesB" + preAnswerDes);
            }
        }
        else if (low_or_high == 0)
        {
            if (isFirstAsc)
            {
                //1回目の処理
                preAnswerAsc = ans;
                isFirstAsc = false;
            }
            if (ans != preAnswerAsc)
            {
                CountProcess(gain);
                //preAnswerを変更
                preAnswerAsc = ans;
            }
        }

    }

    //折り返しが発生した際の処理を行う
    //引数：ゲインの値
    void CountProcess(int x)
    {
        float gain = IntToFloat(x);
        //折り返し地点でのゲインの値をdataに格納
        string data = gain.ToString();
        if (low_or_high == 1)
        {
            //三項演算子とインクリメントを用いてdesの折り返しを+1して継続条件を満たしているかの確認と処理を行う
            isContinue_high = (++orikaeshiNum_High > 4) ? false : true;
            permitAdd = isContinue_high;
            Debug.Log("<color=blue>desNum: </color>" + orikaeshiNum_High);
            if (!isContinue_high)
            {
                block_Low_High = 1;
            }
        }
        else
        {
            //三項演算子とインクリメントを用いてAscの折り返しを+1して継続条件を満たしているかの確認と処理を行う
            isContinue_low = (++orikaeshiNum_Low > 4) ? false : true;
            permitAdd = isContinue_low;
            Debug.Log("<color=red>ascNum: </color>" + orikaeshiNum_Low);
            if (!isContinue_low)
            {
                block_Low_High = 0;
            }

        }
    }
    


    //フラグの値から適切な条件に切り替えを行う関数
    void SwitchCondition()
    {
        //Debug.Log("確認用");
        if (!isContinue_low && !isContinue_high)
        {
            //Debug.Log(isContinue);
            ChangeState(State.End);
            Debug.Log("終了");
            return;
        }
        else
        {
            ChangeState(State.Set);
            Debug.Log("継続");
        }
    }

    void SetCubePosition(Vector3 position)
    {
        cube.transform.localPosition = position;

        // Rigidbody コンポーネントを取得
        Rigidbody rb = cube.GetComponent<Rigidbody>();

        // Rigidbody が存在する場合、速度と角速度をリセット
        if (rb != null)
        {
            rb.velocity = Vector3.zero;       // 速度をリセット
            rb.angularVelocity = Vector3.zero; // 角速度をリセット
        }
    }



    void RecordList(int x)
    {
        float y = IntToFloat(x);
        //answerList.Add(y);
    }


    void SendCSV()
    {
        float nowData = answerList[0];

        //先頭データを削除
        answerList.RemoveAt(0);
    }



    float IntToFloat(int a)
    {
        return (float)a / 1000.0f;
    }

}
