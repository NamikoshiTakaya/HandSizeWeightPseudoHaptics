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
using Valve.Newtonsoft.Json.Linq;

public class Ex22 : MonoBehaviour
{

    //ステート用変数
    State currentState = State.Idle;

    private State _nextState;

    //何セット目かを記録(最大12回目)
    private int setCount = 1;

    //セット内何回目かを記録(最大6回目)
    private int comCount = 0;



    //現在実験中のパターンがパターン１ならtrue
    //private bool isPattern1 = true;

    //オブジェクト宣言
    public GameObject lefthand;
    public GameObject righthand;

    /*public Renderer lefthands;
    public Renderer righthands;*/

    //GameObject cube;
    public GameObject cubeRef;
    public GameObject cube040;
    public GameObject cube060;
    public GameObject cube080;
    public GameObject cube125;
    public GameObject cube150;
    public GameObject cube175;
    /*public GameObject cube140;
    public GameObject cube180;
    public GameObject cube220;*/
    private UnityEngine.Component component;
    private UnityEngine.Component component040;
    private UnityEngine.Component component060;
    private UnityEngine.Component component080;
    private UnityEngine.Component component125;
    private UnityEngine.Component component150;
    private UnityEngine.Component component175;
    //Text変化用
    public UnityEngine.UI.Text questionnaireText;
    public SaveCsvScript22 saveCsvScript;

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

    private Vector3 rightHoverPosition = new Vector3(0.0227f, -0.0138f, -0.1141f);
    private Vector3 leftHoverPosition = new Vector3(-0.0227f, -0.0138f, -0.1141f);
    private Vector3 hoverScale = new Vector3(0.6f, 0.6f, 0.6f);

    //0.40
    private Vector3 rightHoverPosition040 = new Vector3(0.0269f, 0.0164f, -0.142f);
    private Vector3 leftHoverPosition040 = new Vector3(-0.0269f, 0.0164f, -0.142f);
    private Vector3 hoverScale040 = new Vector3(0.35f, 0.35f, 0.35f);

    //0.60
    private Vector3 rightHoverPosition060 = new Vector3(0.0245f, 0.0109f, -0.1358f);
    private Vector3 leftHoverPosition060 = new Vector3(-0.0245f, 0.0109f, -0.1358f);
    private Vector3 hoverScale060 = new Vector3(0.45f, 0.45f, 0.45f);

    //0.80
    private Vector3 rightHoverPosition080 = new Vector3(0.0265f,-0.0138f,-0.1141f);
    private Vector3 leftHoverPosition080 = new Vector3(-0.0265f, -0.0138f, -0.1141f);
    private Vector3 hoverScale080 = new Vector3(0.5f, 0.5f, 0.5f);

    //1.00
    private Vector3 initialRightHoverPosition = new Vector3(0.0227f, -0.0138f, -0.1141f);
    private Vector3 initialLeftHoverPosition = new Vector3(-0.0227f, -0.0138f, -0.1141f);
    private Vector3 initialHoverScale = new Vector3(0.6f, 0.6f, 0.6f);

    //1.25
    private Vector3 rightHoverPosition125 = new Vector3(0.0165f, -0.0197f, -0.0913f);
    private Vector3 leftHoverPosition125 = new Vector3(-0.0165f, -0.0197f, -0.0913f);
    private Vector3 hoverScale125 = new Vector3(0.9f, 0.9f, 0.9f);

    //1.50
    private Vector3 rightHoverPosition150 = new Vector3(0.015f, -0.0144f, -0.1004f);
    private Vector3 leftHoverPosition150 = new Vector3(-0.015f, -0.0144f, -0.1004f);
    private Vector3 hoverScale150 = new Vector3(1.1f, 1.1f, 1.1f);

    //1.75
    private Vector3 rightHoverPosition175 = new Vector3(0.0115f, -0.0347f, -0.0764f);
    private Vector3 leftHoverPosition175 = new Vector3(-0.0115f, -0.0347f, -0.0764f);
    private Vector3 hoverScale175 = new Vector3(1.3f, 1.3f, 1.3f);


/*
    //0.25
    private Vector3 rightHoverPosition025 = new Vector3(0.0278f, 0.026f, -0.150f);
    private Vector3 leftHoverPosition025 = new Vector3(-0.0278f, 0.026f, -0.150f);
    private Vector3 hoverScale025 = new Vector3(0.3f, 0.3f, 0.3f);

    //0.50
    private Vector3 rightHoverPosition050 = new Vector3(0.0245f, 0.0164f, -0.1416f);
    private Vector3 leftHoverPosition050 = new Vector3(-0.0245f, 0.0164f, -0.1416f);
    private Vector3 hoverScale050 = new Vector3(0.4f, 0.4f, 0.4f);

    //0.75
    private Vector3 rightHoverPosition075 = new Vector3(0.0265f, -0.0138f, -0.1141f);
    private Vector3 leftHoverPosition075 = new Vector3(-0.0265f, -0.0138f, -0.1141f);
    private Vector3 hoverScale075 = new Vector3(0.5f, 0.5f, 0.5f);

    //1.40
    private Vector3 rightHoverPosition140 = new Vector3(0.015f, -0.0144f, -0.1004f);
    private Vector3 leftHoverPosition140 = new Vector3(-0.015f, -0.0144f, -0.1004f);
    private Vector3 hoverScale140 = new Vector3(1.1f, 1.1f, 1.1f);


    //1.80
    private Vector3 rightHoverPosition180 = new Vector3(0.0115f, -0.0347f, -0.0764f);
    private Vector3 leftHoverPosition180 = new Vector3(-0.0115f, -0.0347f, -0.0764f);
    private Vector3 hoverScale180 = new Vector3(1.3f, 1.3f, 1.3f);

    //2.20
    private Vector3 rightHoverPosition220 = new Vector3(0.008f, -0.051f, -0.065f);
    private Vector3 leftHoverPosition220 = new Vector3(-0.008f, -0.051f, -0.065f);
    private Vector3 hoverScale220 = new Vector3(1.5f, 1.5f, 1.5f);

*/


    [SerializeField] private const int RefGainValue = 1000;

    private const string Reference = "基準/Ref";
    private const string Comparison = "比較/Com";

    //アンケートの回答をYes→true、No→falseで記録する変数
    //private bool answer = true;
    private string answer;

    [Header("手のサイズの値")]
    public float[] handSizes = { 0.40f, 0.60f, 0.80f, 1.25f, 1.50f, 1.75f };
    public int[] heavyGains = { 800, 1000, 1200 };
    private List<float> shuffledHandSizes;

    [Header("左利きか？")]
    public bool leftCheck;

    [Header("set回数")]
    public int maxSet = 8;
    // 組み合わせリスト
    private List<(float handSize_Com, int heavyGain)> combinations = new List<(float, int)>();

    float combination_handSize = 1.0f;
    int combination_heavyGain = 1000;

    // シャッフル用のカウント
    /// <summary>
    /// private int currentIndex = 0; 
    /// </summary>
    //　ハンドサイズの基準
    private float handSizeRef = 1.0f;
    private float handSizeCom = 1.0f;

    //0なら基準値から先に値をセットする
    private int ref_or_com = 0;

    

    //実験中に実際に使う変数（①か④の数値がAに、②か③の数値がBに入る）
    private Vector3 handSize_A = Vector3.zero;
    private Vector3 handSize_B = Vector3.zero;
    private int gain_Ref = 1000;

    //遅延用
    const float ReadyTime = 1f;
    private float readyTime = 0f;
    private int tmpR = 0;
    

    List<float> answerList = new List<float>();

    IEnumerator DelayMethod(float delay, State newState)
    {
        //delay秒待つ
        yield return new WaitForSeconds(delay);
        ChangeState(newState);
    }
    enum State
    {
        Idle,
        Set,
        Start,
        Ready1,
        Lift1,
        Set2,
        Ready2,
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
        /*questionnaireText.fontSize = 80;
        questionnaireText.text = "Before start\n準備ができたら右のトラックパッドのボタンを押してください\nPress the right TrackPadButton when you are ready";
        */
        questionnaireText.text = "";
        // 組み合わせを生成
        foreach (float handSize_Com in handSizes)
        {
            foreach (int heavyGain in heavyGains)
            {
                combinations.Add((handSize_Com, heavyGain));
            }
        }
        if (leftCheck)
        {
            righthand.transform.localScale = Vector3.zero;
        }
        else if (!leftCheck)
        {
            lefthand.transform.localScale = Vector3.zero;
        }

        ValueInit();
    }

    //変数の初期化
    void ValueInit()
    {
        
        // インタラクタブルコンポーネントの取得
        component = cubeRef.GetComponent<Valve.VR.InteractionSystem.Interactable>();
        component040 = cube040.GetComponent<Valve.VR.InteractionSystem.Interactable>();
        component060 = cube060.GetComponent<Valve.VR.InteractionSystem.Interactable>();
        component080 = cube080.GetComponent<Valve.VR.InteractionSystem.Interactable>();
        component125 = cube125.GetComponent<Valve.VR.InteractionSystem.Interactable>();
        component150 = cube150.GetComponent<Valve.VR.InteractionSystem.Interactable>();
        component175 = cube175.GetComponent<Valve.VR.InteractionSystem.Interactable>();

    }

    // Update is called once per frame
    void Update()
    {
        StateFunction();
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
                questionnaireText.text = "実験前\nBefore start";

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
                /*if (Tpb.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    ChangeState(State.Ready1);
                    //StartCoroutine(DelayMethod(0.5f, State.Ready1));
                }*/
                
                break;
            case State.Ready1:
                readyTime -= Time.deltaTime;
                tmpR = (int)readyTime;
                questionnaireText.fontSize = 80;
                questionnaireText.text = "Lift1の準備中\nPreparing for Lift1\n";
                if (readyTime <= 0)
                {
                    ChangeState(State.Lift1);
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
            case State.Ready2:
                readyTime -= Time.deltaTime;
                tmpR = (int)readyTime;
                questionnaireText.fontSize = 80;
                questionnaireText.text = "Lift2の準備中\nPreparing for Lift2\n";
                if (readyTime <= 0)
                {
                    ChangeState(State.Lift2);
                }

                break;
            case State.Lift2:
                //左のトラックパッドボタンでCubeの位置を初期位置にリセット
                if (Tpb.GetStateDown(SteamVR_Input_Sources.LeftHand))
                {
                    SetCubePosition(initialCubePosition);
                }
                //持ち上げが終わったらAnswerへ遷移
                if (Tpb.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    ChangeState(State.Answer);
                }
                //ミスしたときの戻るための機能
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    //Debug.Log("←"); 
                    ChangeState(State.Lift1);
                    
                }
                break;
            case State.Answer:
                //アンケート回答受付
                //アンケート表示
                //アンケート回答されたらSetに戻る
                //結果をGetStateで取得してinteracrtuiに格納
                //SteamVR_Input_Sources.機器名
                interacrtuiRight = Iui.GetState(SteamVR_Input_Sources.RightHand);
                interacrtuiLeft = Iui.GetState(SteamVR_Input_Sources.LeftHand);

                //savedataに入れる用の変数　あまり気にしないで
                string stSetCount = setCount.ToString();
                string stComCount = (comCount + 1).ToString();
                string stHandSizeRef = handSizeRef.ToString();
                string stHandSizeCom = combination_handSize.ToString();
                string stHeavyGainRef = gain_Ref.ToString();
                string stHeavyGainCom = combination_heavyGain.ToString();

                /*Debug.Log("stSetCount" + stSetCount);
                Debug.Log("stComCount" + stComCount);
                Debug.Log("stHandSizeRef" + stHandSizeRef);
                Debug.Log("stHeavyGainRef" + stHeavyGainRef);
                Debug.Log("stHandSizeCom" + stHandSizeCom);
                Debug.Log("stHeavyGainCom" + stHeavyGainCom);*/

                //ミスしたときの戻るための機能
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (comCount != 0)
                    {
                        comCount--;
                    }
                    else if (comCount == 0)
                    {
                        comCount = 17;
                        setCount--;
                    }
                    //Debug.Log("←");
                    ChangeState(State.Lift2);
                }

                //回答の記録
                if (interacrtuiLeft)
                {
                    if (ref_or_com == 0)
                    {
                        answer = Reference;
                        saveCsvScript.SaveDataEx22(stSetCount + " - (" + stComCount + ")", stHandSizeCom, stHeavyGainCom, answer, stHandSizeRef, stHeavyGainRef, ref_or_com.ToString());
                        CheckEnd();
                    }
                    else
                    {
                        answer = Comparison;
                        saveCsvScript.SaveDataEx22(stSetCount + " - (" + stComCount + ")", stHandSizeCom, stHeavyGainCom,  answer, stHandSizeCom, stHeavyGainCom, ref_or_com.ToString());
                        saveCsvScript.SetAnswerData22(combination_heavyGain, combination_handSize);
                        CheckEnd();
                    }
                }
                //
                else if (interacrtuiRight)
                {
                    if (ref_or_com == 0)
                    {
                        answer = Comparison;
                        saveCsvScript.SaveDataEx22(stSetCount + " - (" + stComCount + ")", stHandSizeCom, stHeavyGainCom, answer, stHandSizeCom, stHeavyGainCom, ref_or_com.ToString());
                        saveCsvScript.SetAnswerData22(combination_heavyGain, combination_handSize);
                        CheckEnd();
                    }
                    else
                    {
                        answer = Reference;
                        saveCsvScript.SaveDataEx22(stSetCount + " - (" + stComCount + ")", stHandSizeCom, stHeavyGainCom, answer, stHandSizeRef, stHeavyGainRef, ref_or_com.ToString());
                        
                        CheckEnd();
                    }
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
                    //開始前処理
                    HideAllCubes();
                    SetGainAndHandSize(0.0f, gain_Ref, rightHoverPosition, leftHoverPosition, hoverScale);


                    break;
                case State.Start:
                    
                    break;
                case State.Ready1:

                    break;
                case State.Lift1:
                    //オブジェクトを消すなどのLift1の終了処理を記述（その後Lift2へ遷移）
                    //実装
                    HideAllCubes();
                    SetGainAndHandSize(0.0f, gain_Ref, rightHoverPosition, leftHoverPosition, hoverScale);
                    break;
                case State.Set2:

                    break;
                case State.Ready2:

                    break;
                case State.Lift2:
                    //オブジェクトを消すなどのLift2の終了処理を記述（その後Answerへ遷移）
                    HideAllCubes();
                    SetGainAndHandSize(0.0f, gain_Ref, rightHoverPosition, leftHoverPosition, hoverScale);

                    break;
                case State.Answer:
                    if (comCount != 17)
                    {
                        comCount++;
                    }
                    else if (comCount == 17)
                    {
                        comCount = 0;
                        setCount++;
                    }
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
                    cube040.SetActive(false);
                    cube060.SetActive(false);
                    cube080.SetActive(false);
                    cube125.SetActive(false);
                    cube150.SetActive(false);
                    cube175.SetActive(false);
                    /*cube140.SetActive(false);
                    cube180.SetActive(false);
                    cube220.SetActive(false);*/
                    break;
                case State.Set:
                    /*questionnaireText.fontSize = 100;
                    questionnaireText.text = "Set";*/
                    Debug.Log("comCouonnt " + comCount);
                    if (comCount == 0)
                    {
                        //ShuffleHandSizes();
                        Debug.Log("シャッフルされたよ");
                        ShuffleCombination(combinations);
                    }
                    combination_handSize = combinations[comCount].handSize_Com;
                    combination_heavyGain = combinations[comCount].heavyGain;
                    //handSizeCom = shuffledHandSizes[comCount];
                    //Debug.Log(handSizeCom);
                    Debug.Log("List contents: " + string.Join(",, ", combinations));
                    Debug.Log(combination_handSize);
                    Debug.Log(combination_heavyGain);

                    //必要なら待機時間を設けて、その後Startに遷移
                    //StartCoroutine(DelayMethod(0.5f, State.Start));
                    ChangeState(State.Start);
                    break;
                case State.Start:
                    questionnaireText.fontSize = 80;
                    //questionnaireText.text = "準備ができたら右のトラックパッドのボタンを押してください\nPress the right TrackPadButton when you are ready";

                    Debug.Log("ExperimentCount: " + setCount + "-" + comCount);
                    //持ち上げ１，２回目の各条件を決定（その後Lift1に遷移）
                    ref_or_com = UnityEngine.Random.Range(0, 2);
                    //ref_or_com = 1;
                    Debug.Log("ref_or_com: " + ref_or_com);
                    ChangeState(State.Ready1);

                    break;
                case State.Ready1:
                    readyTime = ReadyTime;

                    break;
                case State.Lift1:
                    //1回目の持ち上げ条件を適用する
                    questionnaireText.fontSize = 100;
                    questionnaireText.text = setCount + "-" + (comCount+1) + "\nLift1";
                    //lefthand.SetActive(true);
                    //righthand.SetActive(true);
                    //lefthands.enabled = true;
                    //righthands.enabled = true;
                    if (ref_or_com == 0)
                    {
                        //キューブをアクティブにする
                        rightHoverPosition = initialRightHoverPosition;
                        leftHoverPosition = initialLeftHoverPosition;
                        hoverScale = initialHoverScale;
                        cubeRef.SetActive(true);
                        SetCubePosition(initialCubePosition);
                        //①か④の条件のうちこの段階で選ばれているものをオブジェクトと手に適用する（これは関数を作る）
                        SetGainAndHandSize(handSizeRef, gain_Ref, rightHoverPosition, leftHoverPosition, hoverScale);
                    }
                    else
                    {
                        if (combination_handSize == 0.40f)
                        {
                            rightHoverPosition = rightHoverPosition040;
                            leftHoverPosition = leftHoverPosition040;
                            hoverScale = hoverScale040;
                            cube040.SetActive(true);
                        }
                        else if (combination_handSize == 0.60f)
                        {
                            rightHoverPosition = rightHoverPosition060;
                            leftHoverPosition = leftHoverPosition060;
                            hoverScale = hoverScale060;
                            cube060.SetActive(true);
                        }
                        else if (combination_handSize == 0.80f)
                        {
                            rightHoverPosition = rightHoverPosition080;
                            leftHoverPosition = leftHoverPosition080;
                            hoverScale = hoverScale080;
                            cube080.SetActive(true);
                        }
                        else if (combination_handSize == 1.25f)
                        {
                            rightHoverPosition = rightHoverPosition125;
                            leftHoverPosition = leftHoverPosition125;
                            hoverScale = hoverScale125;
                            cube125.SetActive(true);
                        }
                        else if(combination_handSize == 1.50f)
                        {
                            rightHoverPosition = rightHoverPosition150;
                            leftHoverPosition = leftHoverPosition150;
                            hoverScale = hoverScale150;
                            cube150.SetActive(true);
                        }
                        else if(combination_handSize == 1.75f)
                        {
                            rightHoverPosition = rightHoverPosition175;
                            leftHoverPosition = leftHoverPosition175;
                            hoverScale = hoverScale175;
                            cube175.SetActive(true);
                        }
                        /*
                        if (combination_handSize == 0.25f)
                        {
                            rightHoverPosition = rightHoverPosition025;
                            leftHoverPosition = leftHoverPosition025;
                            hoverScale = hoverScale025;
                            cube025.SetActive(true);
                        }
                        else if (combination_handSize == 0.50f)
                        {
                            rightHoverPosition = rightHoverPosition050;
                            leftHoverPosition = leftHoverPosition050;
                            hoverScale = hoverScale050;
                            cube050.SetActive(true);
                        }
                        else if (combination_handSize == 0.75f)
                        {
                            rightHoverPosition = rightHoverPosition075;
                            leftHoverPosition = leftHoverPosition075;
                            hoverScale = hoverScale075;
                            cube075.SetActive(true);
                        }
                        else if (handSizeCom == 1.40f)
                        {
                            rightHoverPosition = rightHoverPosition140;
                            leftHoverPosition = leftHoverPosition140;
                            hoverScale = hoverScale140;
                            cube140.SetActive(true);
                        }
                        else if (handSizeCom == 1.80f)
                        {
                            rightHoverPosition = rightHoverPosition180;
                            leftHoverPosition = leftHoverPosition180;
                            hoverScale = hoverScale180;
                            cube180.SetActive(true);
                        }
                        else if (handSizeCom == 2.20f)
                        {
                            rightHoverPosition = rightHoverPosition220;
                            leftHoverPosition = leftHoverPosition220;
                            hoverScale = hoverScale220;
                            cube220.SetActive(true);
                        }*/
                        SetCubePosition(initialCubePosition);
                        //②か③の条件のうちこの段階で選ばれているものをオブジェクトと手に適用する（これは関数を作る）
                        SetGainAndHandSize(combination_handSize, combination_heavyGain, rightHoverPosition, leftHoverPosition, hoverScale);
                    }
                    break;
                case State.Set2:
                    questionnaireText.fontSize = 100;
                    //questionnaireText.text = "Lift2の準備中\nPreparing for Lift2";
                    //必要なら待機時間を設けて、その後Startに遷移
                    //StartCoroutine(DelayMethod(0.5f, State.Ready2));
                    ChangeState(State.Ready2);
                    //StartCoroutine(DelayMethod(3.0f, State.Lift2));
                    break;
                case State.Ready2:
                    readyTime = ReadyTime;




                    break;
                case State.Lift2:
                    questionnaireText.fontSize = 100;
                    questionnaireText.text = setCount + "-" + (comCount + 1) + "\nLift2";

                    //lefthand.SetActive(true);
                    //righthand.SetActive(true);
                    //lefthands.enabled = true;
                    //righthands.enabled = true;



                    //2回目の持ち上げ条件を適用する
                    if (ref_or_com == 0)
                    {
                        //キューブとハンドをアクティブにする
                        if (combination_handSize == 0.40f)
                        {
                            rightHoverPosition = rightHoverPosition040;
                            leftHoverPosition = leftHoverPosition040;
                            hoverScale = hoverScale040;
                            cube040.SetActive(true);
                        }
                        else if (combination_handSize == 0.60f)
                        {
                            rightHoverPosition = rightHoverPosition060;
                            leftHoverPosition = leftHoverPosition060;
                            hoverScale = hoverScale060;
                            cube060.SetActive(true);
                        }
                        else if (combination_handSize == 0.80f)
                        {
                            rightHoverPosition = rightHoverPosition080;
                            leftHoverPosition = leftHoverPosition080;
                            hoverScale = hoverScale080;
                            cube080.SetActive(true);
                        }
                        else if (combination_handSize == 1.25f)
                        {
                            rightHoverPosition = rightHoverPosition125;
                            leftHoverPosition = leftHoverPosition125;
                            hoverScale = hoverScale125;
                            cube125.SetActive(true);
                        }
                        else if (combination_handSize == 1.50f)
                        {
                            rightHoverPosition = rightHoverPosition150;
                            leftHoverPosition = leftHoverPosition150;
                            hoverScale = hoverScale150;
                            cube150.SetActive(true);
                        }
                        else if (combination_handSize == 1.75f)
                        {
                            rightHoverPosition = rightHoverPosition175;
                            leftHoverPosition = leftHoverPosition175;
                            hoverScale = hoverScale175;
                            cube175.SetActive(true);
                        }
                        /*
                        if (combination_handSize == 0.25f)
                        {
                            rightHoverPosition = rightHoverPosition025;
                            leftHoverPosition = leftHoverPosition025;
                            hoverScale = hoverScale025;
                            cube025.SetActive(true);
                        }
                        else if (combination_handSize == 0.50f)
                        {
                            rightHoverPosition = rightHoverPosition050;
                            leftHoverPosition = leftHoverPosition050;
                            hoverScale = hoverScale050;
                            cube050.SetActive(true);
                        }
                        else if (combination_handSize == 0.75f)
                        {
                            rightHoverPosition = rightHoverPosition075;
                            leftHoverPosition = leftHoverPosition075;
                            hoverScale = hoverScale075;
                            cube075.SetActive(true);
                        }
                        else if (handSizeCom == 1.40f)
                        {
                            rightHoverPosition = rightHoverPosition140;
                            leftHoverPosition = leftHoverPosition140;
                            hoverScale = hoverScale140;
                            cube140.SetActive(true);
                        }
                        else if (handSizeCom == 1.80f)
                        {
                            rightHoverPosition = rightHoverPosition180;
                            leftHoverPosition = leftHoverPosition180;
                            hoverScale = hoverScale180;
                            cube180.SetActive(true);
                        }
                        else if (handSizeCom == 2.20f)
                        {
                            rightHoverPosition = rightHoverPosition220;
                            leftHoverPosition = leftHoverPosition220;
                            hoverScale = hoverScale220;
                            cube220.SetActive(true);
                        }*/

                        SetCubePosition(initialCubePosition);
                        //②か③の条件のうちこの段階で選ばれているものをオブジェクトと手に適用する（これは関数を作る）
                        SetGainAndHandSize(combination_handSize, combination_heavyGain, rightHoverPosition, leftHoverPosition, hoverScale);
                    }
                    else
                    {
                        rightHoverPosition = initialRightHoverPosition;
                        leftHoverPosition = initialLeftHoverPosition;
                        hoverScale = initialHoverScale;
                        cubeRef.SetActive(true);
                        SetCubePosition(initialCubePosition);
                        //①か④の条件のうちこの段階で選ばれているものをオブジェクトと手に適用する（これは関数を作る）
                        SetGainAndHandSize(handSizeRef, gain_Ref, rightHoverPosition, leftHoverPosition, hoverScale);
                    }
                    break;
                case State.Answer:
                    //質問を表示
                    SetCubePosition(initialCubePosition);
                    questionnaireText.fontSize = 70;
                    questionnaireText.text = "どちらの物体の方が重かったですか？\n最初の物体 → 左トリガー\n最後の物体 → 右トリガー\nWhich object is heavier? \nFirst object → left trigger \nLast object → right trigger";
                    break;
                case State.End:
                    //終了処理
                    questionnaireText.fontSize = 100;
                    questionnaireText.text = "一つ目の実験終了です\nEnd";
                    Debug.Log("EndSave");
                    saveCsvScript.EndSave();
                    break;
            }
        }

    }

    private void SetGainAndHandSize(float handSize, int gain, Vector3 rightHoverPos, Vector3 leftHoverPos, Vector3 HoverScl)
    {
        //手とオブジェクトにhandSizeとgainを適用する
        //手の処理
        GameObject[] hands = GameObject.FindGameObjectsWithTag("SizeHand");
        foreach (GameObject hand in hands)
        {
            hand.transform.localScale = new Vector3(handSize,handSize,handSize);
        }
        //Gainの処理
        MethodInfo methodInfo = component.GetType().GetMethod("VariableChanger");
        MethodInfo methodInfo040 = component040.GetType().GetMethod("VariableChanger");
        MethodInfo methodInfo060 = component060.GetType().GetMethod("VariableChanger");
        MethodInfo methodInfo080 = component080.GetType().GetMethod("VariableChanger");
        MethodInfo methodInfo125 = component125.GetType().GetMethod("VariableChanger");
        MethodInfo methodInfo150 = component150.GetType().GetMethod("VariableChanger");
        MethodInfo methodInfo175 = component175.GetType().GetMethod("VariableChanger");

        //ここかえる！！！
        float inputGain = IntToFloat(gain);
        object[] parameters = new object[] { inputGain };
        methodInfo.Invoke(component, parameters);
        methodInfo040.Invoke(component040, parameters);
        methodInfo060.Invoke(component060, parameters);
        methodInfo080.Invoke(component080, parameters);
        methodInfo125.Invoke(component125, parameters);
        methodInfo150.Invoke(component150, parameters);
        methodInfo175.Invoke(component175, parameters);


        //ColliderScale
        GameObject[] handColliders = GameObject.FindGameObjectsWithTag("Collider");
        foreach (GameObject handCollider in handColliders)
        {
            handCollider.transform.localScale = new Vector3(handSize, handSize, handSize);
        }
        //左手の指だけ逆転のため、個別対応
        GameObject[] leftFingerColliders = GameObject.FindGameObjectsWithTag("LeftFingerCollider");
        foreach (GameObject leftFingerCollider in leftFingerColliders)
        {
            leftFingerCollider.transform.localScale = new Vector3(-1.0f* (handSize), handSize, handSize);
        }

        //HoverPoint調整
        GameObject rightHoverPoint = GameObject.FindGameObjectWithTag("RightHoverPoint");
        rightHoverPoint.transform.localPosition = rightHoverPos;
        rightHoverPoint.transform.localScale = HoverScl;

        GameObject leftHoverPoint = GameObject.FindGameObjectWithTag("LeftHoverPoint");
        leftHoverPoint.transform.localPosition = leftHoverPos;
        leftHoverPoint.transform.localScale = HoverScl;

    }

    void ShuffleHandSizes()
    {
        // 元のリストをコピーしてシャッフル
        shuffledHandSizes = new List<float>(handSizes);
        for (int i = 0; i < shuffledHandSizes.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, shuffledHandSizes.Count);
            // 現在の要素とランダムな位置の要素を交換
            float temp = shuffledHandSizes[i];
            shuffledHandSizes[i] = shuffledHandSizes[randomIndex];
            shuffledHandSizes[randomIndex] = temp;
        }
        //currentIndex = 0; // インデックスをリセット
        Debug.Log("Shuffled HandSizes: " + string.Join(", ", shuffledHandSizes));
    }

    //終了条件を満たしているかを確認する
    void CheckEnd()
    {
        if (setCount == maxSet && comCount == 17)
        {
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

    float IntToFloat(int a)
    {
        return (float)a / 1000.0f;
    }


    void SetCubePosition(Vector3 position)
    {
        cubeRef.transform.localPosition = position;
        cube040.transform.localPosition = position;
        cube060.transform.localPosition = position;
        cube080.transform.localPosition = position;
        cube125.transform.localPosition = position;
        cube150.transform.localPosition = position;
        cube175.transform.localPosition = position;
        /*cube140.transform.localPosition = position;
        cube180.transform.localPosition = position;
        cube220.transform.localPosition = position;*/

        // Rigidbody コンポーネントを取得
        Rigidbody rb = cubeRef.GetComponent<Rigidbody>();
        Rigidbody rb040 = cube040.GetComponent<Rigidbody>();
        Rigidbody rb060 = cube060.GetComponent<Rigidbody>();
        Rigidbody rb080 = cube080.GetComponent<Rigidbody>();
        Rigidbody rb125 = cube125.GetComponent<Rigidbody>();
        Rigidbody rb150 = cube150.GetComponent<Rigidbody>();
        Rigidbody rb175 = cube175.GetComponent<Rigidbody>();
        /*Rigidbody rb140 = cube140.GetComponent<Rigidbody>();
        Rigidbody rb180 = cube180.GetComponent<Rigidbody>();
        Rigidbody rb220 = cube220.GetComponent<Rigidbody>();*/

        // Rigidbody が存在する場合、速度と角速度をリセット
        if (rb != null)
        {
            rb.velocity = Vector3.zero;       // 速度をリセット
            rb.angularVelocity = Vector3.zero; // 角速度をリセット
        }
        else if (rb040 != null)
        {
            rb040.velocity = Vector3.zero;       // 速度をリセット
            rb040.angularVelocity = Vector3.zero; // 角速度をリセット
        }
        else if (rb060 != null)
        {
            rb060.velocity = Vector3.zero;       // 速度をリセット
            rb060.angularVelocity = Vector3.zero; // 角速度をリセット
        }
        else if (rb080 != null)
        {
            rb080.velocity = Vector3.zero;       // 速度をリセット
            rb080.angularVelocity = Vector3.zero; // 角速度をリセット
        }
        else if (rb125 != null)
        {
            rb125.velocity = Vector3.zero;       // 速度をリセット
            rb125.angularVelocity = Vector3.zero; // 角速度をリセット
        }
        else if (rb150 != null)
        {
            rb150.velocity = Vector3.zero;       // 速度をリセット
            rb150.angularVelocity = Vector3.zero; // 角速度をリセット
        }
        else if (rb175 != null)
        {
            rb175.velocity = Vector3.zero;       // 速度をリセット
            rb175.angularVelocity = Vector3.zero; // 角速度をリセット
        }

        /*else if (rb140 != null)
        {
            rb140.velocity = Vector3.zero;       // 速度をリセット
            rb140.angularVelocity = Vector3.zero; // 角速度をリセット
        }
        else if (rb180 != null)
        {
            rb180.velocity = Vector3.zero;       // 速度をリセット
            rb180.angularVelocity = Vector3.zero; // 角速度をリセット
        }
        else if (rb220 != null)
        {
            rb220.velocity = Vector3.zero;       // 速度をリセット
            rb220.angularVelocity = Vector3.zero; // 角速度をリセット
        }*/
    }
    void HideAllCubes()
    {
        cubeRef.SetActive(false);
        cube040.SetActive(false);
        cube060.SetActive(false);
        cube080.SetActive(false);
        cube125.SetActive(false);
        cube150.SetActive(false);
        cube175.SetActive(false);
        /* cube140.SetActive(false);
        cube180.SetActive(false);
        cube220.SetActive(false); */
    }

    // 組み合わせをシャッフルする関数
    void ShuffleCombination<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

}
