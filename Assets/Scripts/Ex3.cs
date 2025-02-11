using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Valve.VR;

public class Ex3 : MonoBehaviour
{
    //�X�e�[�g�p�ϐ�
    State currentState = State.Idle;
    private State _nextState;

    //���ݎ������̃p�^�[�����p�^�[���P�Ȃ�true
    //private bool isPattern1 = true;

    //�I�u�W�F�N�g�錾
    public GameObject lefthand;
    public GameObject righthand;
    //GameObject cube;
    public GameObject cubeRef;
    public GameObject cubeSmall;
    public GameObject cubeBig;


    private UnityEngine.Component component;
    private UnityEngine.Component componentSmall;
    private UnityEngine.Component componentBig;

    //Text�ω��p
    public UnityEngine.UI.Text questionnaireText;
    public SaveCsvScript3 saveCsvScript;

    //InteractUI�{�^����������Ă�̂��𔻒肷�邽�߂�Iui�Ƃ����֐���SteamVR_Actions.default_InteractUI���Œ�
    private SteamVR_Action_Boolean Iui = SteamVR_Actions.default_InteractUI;
    private SteamVR_Action_Boolean Tpb = SteamVR_Actions.default_TrackPadButton;
    //���ʂ̊i�[�pBoolean�^�֐�interacrtui
    private Boolean interacrtuiRight;
    private Boolean interacrtuiLeft;
    private Boolean trackPadButtonRight;
    private Boolean trackPadButtonLeft;

    // �����ʒu
    private Vector3 initialCubePosition = new Vector3(0, 0.90f, 3.1f);

    private Vector3 rightHoverPosition = new Vector3(0.0227f, -0.0138f, -0.1141f);
    private Vector3 leftHoverPosition = new Vector3(-0.0227f, -0.0138f, -0.1141f);
    private Vector3 hoverScale = new Vector3(0.6f, 0.6f, 0.6f);

    //1.00
    private Vector3 initialRightHoverPosition = new Vector3(0.0227f, -0.0138f, -0.1141f);
    private Vector3 initialLeftHoverPosition = new Vector3(-0.0227f, -0.0138f, -0.1141f);
    private Vector3 initialHoverScale = new Vector3(0.6f, 0.6f, 0.6f);

    //0.60
    private Vector3 rightHoverPosition060 = new Vector3(0.0245f, 0.0109f, -0.1358f);
    private Vector3 leftHoverPosition060 = new Vector3(-0.0245f, 0.0109f, -0.1358f);
    private Vector3 hoverScale060 = new Vector3(0.45f, 0.45f, 0.45f);

    //1.50
    private Vector3 rightHoverPosition150 = new Vector3(0.015f, -0.0144f, -0.1004f);
    private Vector3 leftHoverPosition150 = new Vector3(-0.015f, -0.0144f, -0.1004f);
    private Vector3 hoverScale150 = new Vector3(1.1f, 1.1f, 1.1f);


    //�K�i�@�̏I��������s���܂�Ԃ��񐔂��L�^����ϐ�
    private int orikaeshiNum_Low = 0;
    private int orikaeshiNum_High = 0;
    //orikaeshiNum�̕Е���5�ɓ��B�����ۂ�low_or_high�̂ǂ��炩���u���b�N���邽�߂̕ϐ�
    private int block_Low_High = 2;

    private float startExperiment; //�����̊J�n����
    private float startAnswer; // �v���̊J�n���Ԃ��i�[����ϐ�

    //��{�I�ɒl�͏����_������Ȃ��悤��1000�{���đ��삷��
    [Header("�Q�C���̏���l�Ɖ����l�Ɣ�r�l(1000�{)")]
    [SerializeField] private const int LowGainValue = 400;
    [SerializeField] private const int HighGainValue = 1600;
    [SerializeField] private const int RefGainValue = 1000;

    private const string Reference = "�/Ref";
    private const string Comparison = "��r/Com";


    //����ɉ񓚂����ۂɕϓ�����l(�萔)
    [Header("�Q�C���̕ω���(1000�{)")]
    [SerializeField] private int ChangeValue = 100;

    private int gain_Low;
    private int gain_High;

    //����ɉ񓚂����ۂɕϓ�����l(���̕ϐ��̒l�͊�{�I�ɑ��삵�Ȃ�)
    private int APValue;

    //�A���P�[�g�̉񓚂�Yes��true�ANo��false�ŋL�^����ϐ�
    //private bool answer = true;
    private string answer;


    [Header("�p�^�[��1���p�^�[��2�̂ǂ�����������邩���`�F�b�N)")]
    [SerializeField] private bool pattern1 = false;
    [SerializeField] private bool pattern2 = false;
    //private int pattern = 0;

    [Header("��̃T�C�Y���Ƒ�̒l�i��l1.0�j)")]
    public float HandSizeLow = 0.50f;
    public float HandSizeHigh = 1.80f;
    public float HandSizeRef = 1.0f;

    [Header("���������H")]
    public bool leftCheck;

    //0�Ȃ��l��Low�̔�r���ɍs��
    private int low_or_high = 0;

    //0�Ȃ��l�����ɒl���Z�b�g����
    private int ref_or_com = 0;

    private int setCount = 0;
    //�Z�b�g������ڂ����L�^
    private int startCount = 0;

    //�������Ɏ��ۂɎg���ϐ��i�@���C�̐��l��A�ɁA�A���B�̐��l��B�ɓ���j
    private float handSize_A = 0.0f;
    private float handSize_B = 0.0f;
    private int gain_A = 0;
    private int gain_B = 0;

    //�x���p
    const float ReadyTime = 1f;
    private float readyTime = 0f;
    private int tmpR = 0;


    //�����A���r���Ă��邩�ǂ������L�^����ϐ�
    //des���A�AAsc���B
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
        //�����ݒ�
        if (pattern1 && !pattern2)
        {
            handSize_A = HandSizeLow;
            //pattern = 0;
        }
        else if (!pattern1 && pattern2)
        {
            handSize_A = HandSizeHigh;
            //pattern = 1;
        }
        if (pattern1 ^ pattern2 == false)
        {
            //�G���[�f���Ď~�܂�悤��
            Debug.LogError("�C���X�y�N�^�[�������������Е������I�����ă`�F�b�N���Ă��������i�����`�F�b�N���_���j");
            UnityEditor.EditorApplication.isPlaying = false;

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

    //�ϐ��̏�����
    void ValueInit()
    {
        gain_Low = LowGainValue;
        gain_High = HighGainValue;
        APValue = ChangeValue;

        gain_A = RefGainValue;
        handSize_B = HandSizeRef;

        // �C���^���N�^�u���R���|�[�l���g�̎擾
        component = cubeRef.GetComponent<Valve.VR.InteractionSystem.Interactable>();
        componentSmall = cubeSmall.GetComponent<Valve.VR.InteractionSystem.Interactable>();
        componentBig = cubeBig.GetComponent<Valve.VR.InteractionSystem.Interactable>();

    }

    // Update is called once per frame
    void Update()
    {
        //State�ɉ���������
        StateFunction();
    }

    IEnumerator DelayMethod(float delay, State newState)
    {
        //delay�b�҂�
        yield return new WaitForSeconds(delay);
        ChangeState(newState);
    }


    void StateFunction()
    {
        //���݂�State�ɉ�����Update�������s��
        switch (currentState)
        {
            case State.Idle:
                //Enter�����Ǝ����J�n
                //if (Input.GetKeyDown(KeyCode.Return))
                //trackPadButtonRight = Tpb.GetState(SteamVR_Input_Sources.RightHand);
                questionnaireText.fontSize = 80;
                questionnaireText.text = "�����O\nBefore start";

                //���̃g���b�N�p�b�h�{�^����Cube�̈ʒu�������ʒu�Ƀ��Z�b�g
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
                //�K�v�Ȃ�ҋ@���Ԃ�݂��āA���̌�Lift1�ɑJ��
                /*if (Tpb.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    ChangeState(State.Lift1);
                    //StartCoroutine(DelayMethod(1.0f, State.Lift1));
                }*/
                break;
            case State.Ready1:
                readyTime -= Time.deltaTime;
                tmpR = (int)readyTime;
                questionnaireText.fontSize = 80;
                questionnaireText.text = "Lift1�̏�����\nPreparing for Lift1\n";
                if (readyTime <= 0)
                {
                    ChangeState(State.Lift1);
                }

                break;
            case State.Lift1:
                //���̃g���b�N�p�b�h�{�^����Cube�̈ʒu�������ʒu�Ƀ��Z�b�g
                if (Tpb.GetStateDown(SteamVR_Input_Sources.LeftHand))
                {
                    SetCubePosition(initialCubePosition);
                }
                //�����グ���I�������Lift2�֑J��
                if (Tpb.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    ChangeState(State.Set2);
                }
                break;

            case State.Set2:
                //�K�v�Ȃ�ҋ@���Ԃ�݂��āA���̌�Start�ɑJ��
                break;
            case State.Ready2:
                readyTime -= Time.deltaTime;
                tmpR = (int)readyTime;
                questionnaireText.fontSize = 80;
                questionnaireText.text = "Lift2�̏�����\nPreparing for Lift2\n";
                if (readyTime <= 0)
                {
                    ChangeState(State.Lift2);
                }
                break;

            case State.Lift2:
                //���̃g���b�N�p�b�h�{�^����Cube�̈ʒu�������ʒu�Ƀ��Z�b�g
                if (Tpb.GetStateDown(SteamVR_Input_Sources.LeftHand))
                {
                    SetCubePosition(initialCubePosition);
                }
                //�����グ���I�������Lift1�֑J��
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
                //�A���P�[�g�񓚎�t
                //�񓚂ɉ������Q�C���̕ύX��K�؂ȃX�e�[�g�ւ̑J�ڂ��s��

                //�A���P�[�g�\��
                //�A���P�[�g�񓚂��ꂽ��Set�ɖ߂�
                //���ʂ�GetState�Ŏ擾����interacrtui�Ɋi�[
                //SteamVR_Input_Sources.�@�햼�i����͍��R���g���[���j
                interacrtuiRight = Iui.GetState(SteamVR_Input_Sources.RightHand);
                interacrtuiLeft = Iui.GetState(SteamVR_Input_Sources.LeftHand);

                //savedata�ɓ����p�̕ϐ��@���܂�C�ɂ��Ȃ���
                string stSetCount = setCount.ToString();

                //if (Input.GetKeyDown(KeyCode.Y))
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    ChangeState(State.Lift2);
                }
                if (interacrtuiLeft)
                {
                    //�񓚂����ƂɃQ�C���̕ύX���s��
                    if (ref_or_com == 0)
                    {
                        answer = Reference;
                        saveCsvScript.SaveDataEx3(stSetCount, gain_B.ToString(), answer, low_or_high.ToString(), ref_or_com.ToString(), orikaeshiNum_High.ToString(), orikaeshiNum_Low.ToString());

                    }
                    else
                    {
                        answer = Comparison;
                        saveCsvScript.SaveDataEx3(stSetCount, gain_B.ToString(), answer, low_or_high.ToString(), ref_or_com.ToString(), orikaeshiNum_High.ToString(), orikaeshiNum_Low.ToString());
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
                        saveCsvScript.SaveDataEx3(stSetCount, gain_B.ToString(), answer, low_or_high.ToString(), ref_or_com.ToString(), orikaeshiNum_High.ToString(), orikaeshiNum_Low.ToString());

                    }
                    else
                    {
                        answer = Reference;
                        saveCsvScript.SaveDataEx3(stSetCount, gain_B.ToString(), answer, low_or_high.ToString(), ref_or_com.ToString(), orikaeshiNum_High.ToString(), orikaeshiNum_Low.ToString());
                        //saveCsvScript.SetAnswerData(genericStickyPlane.GetVisualIndicator(), conditionBHeight);
                    }
                    //�񓚂����ƂɃQ�C���̕ύX���s��
                    ChangeGain(answer);
                }
                break;
            case State.End:

                //���̃X�e�[�g�ɑJ�ڂ��Ȃ��悤�ɂ���i���̃X�e�[�g�ɓ��������_�Ŏ����I���j
                break;
        }

        // �X�e�[�g���؂�ւ������
        if (currentState != _nextState)
        {
            // �I���������Ăяo���i�؂�ւ�莞�Ɉ�x�̂݌Ăяo���j
            switch (currentState)
            {
                case State.Idle:

                    break;
                case State.Set:
                    //��r1��ڂ̏���������
                    setCount++;
                    Debug.Log("setCount + setCount");

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
                    //�J�n�O����
                    cubeRef.SetActive(false);
                    cubeSmall.SetActive(false);
                    cubeBig.SetActive(false);

                    //lefthand.SetActive(false);
                    //righthand.SetActive(false);
                    //�����������
                    SetGainAndHandSize(0.0f, gain_A, rightHoverPosition, leftHoverPosition, hoverScale);
                    //low_or_high = 1;
                    Debug.Log("<color=yellow>block_Low_High: </color>" + block_Low_High);
                    Debug.Log("low_or_high: " + low_or_high);
                    break;
                case State.Start:
                    //�����グ�P�C�Q��ڂ̊e����������i���̌�Lift1�ɑJ�ځj
                    //ref_or_com = UnityEngine.Random.Range(0, 2);
                    //ref_or_com = 1;
                    
                    break;
                case State.Ready1:

                    break;
                case State.Lift1:
                    //�I�u�W�F�N�g�������Ȃǂ�Lift1�̏I���������L�q�i���̌�Lift2�֑J�ځj
                    //����
                    cubeRef.SetActive(false);
                    cubeSmall.SetActive(false);
                    cubeBig.SetActive(false);

                    //�����������
                    SetGainAndHandSize(0.0f, gain_A, rightHoverPosition, leftHoverPosition, hoverScale);
                    break;
                case State.Set2:
                    //

                    break;
                case State.Ready2:

                    break;
                case State.Lift2:
                    //�I�u�W�F�N�g�������Ȃǂ�Lift2�̏I���������L�q�i���̌�Answer�֑J�ځj
                    cubeRef.SetActive(false);
                    cubeSmall.SetActive(false);
                    cubeBig.SetActive(false);

                    //�����������
                    SetGainAndHandSize(0.0f, gain_A, rightHoverPosition, leftHoverPosition, hoverScale);

                    break;
                case State.Answer:
                    //�񓚂�ۑ�
                    
                    break;
                case State.End:

                    break;
            }

            // ���̃X�e�[�g�ɑJ�ڂ���
            //�X�e�[�g�̊J�n�������s���i�؂�ւ�莞�Ɉ�x�̂݌Ăяo���j
            currentState = _nextState;
            switch (currentState)
            {
                case State.Idle:
                    cubeSmall.SetActive(false);
                    cubeBig.SetActive(false);

                    break;
                case State.Set:
                    questionnaireText.fontSize = 100;
                    questionnaireText.text = "Set";
                    //�K�v�Ȃ�ҋ@���Ԃ�݂��āA���̌�Start�ɑJ��
                    StartCoroutine(DelayMethod(0.5f, State.Start));
                    break;

                case State.Start:
                    questionnaireText.fontSize = 80;
                    //questionnaireText.text = "�������ł�����g���b�N�p�b�h�̃{�^���������Ă�������\nPress the TrackPadButton when you are ready";
                    if (low_or_high == 0)
                    {
                        gain_B = gain_Low;
                    }

                    //�B�����ɔ�r����ꍇ
                    else if (low_or_high == 1)
                    {
                        gain_B = gain_High;
                    }
                    ref_or_com = UnityEngine.Random.Range(0, 2);
                    Debug.Log("ref_or_com: " + ref_or_com);
                    Debug.Log("StartCount: " + startCount);
                    ChangeState(State.Ready1);

                    break;
                case State.Ready1:
                    readyTime = ReadyTime;

                    break;
                case State.Lift1:
                    //1��ڂ̎����グ������K�p����
                    questionnaireText.fontSize = 100;
                    questionnaireText.text = "Lift1";

                    //lefthand.SetActive(true);
                    //righthand.SetActive(true);
                    if (ref_or_com == 0)
                    {
                        if (pattern1)
                        {
                            rightHoverPosition = rightHoverPosition060;
                            leftHoverPosition = leftHoverPosition060;
                            hoverScale = hoverScale060;
                            cubeSmall.SetActive(true);

                        }
                        else if (pattern2)
                        {
                            rightHoverPosition = rightHoverPosition150;
                            leftHoverPosition = leftHoverPosition150;
                            hoverScale = hoverScale150;
                            cubeBig.SetActive(true);
                        }
                        
                        SetCubePosition(initialCubePosition);
                        //�@���C�̏����̂������̒i�K�őI�΂�Ă�����̂��I�u�W�F�N�g�Ǝ�ɓK�p����i����͊֐������j
                        SetGainAndHandSize(handSize_A, gain_A, rightHoverPosition, leftHoverPosition, hoverScale);
                    }
                    else
                    {
                        //�L���[�u���A�N�e�B�u�ɂ���
                        rightHoverPosition = initialRightHoverPosition;
                        leftHoverPosition = initialLeftHoverPosition;
                        hoverScale = initialHoverScale;
                        cubeRef.SetActive(true);
                        SetCubePosition(initialCubePosition);
                        //�A���B�̏����̂������̒i�K�őI�΂�Ă�����̂��I�u�W�F�N�g�Ǝ�ɓK�p����i����͊֐������j
                        SetGainAndHandSize(handSize_B, gain_B, rightHoverPosition, leftHoverPosition, hoverScale);
                    }
                    break;
                case State.Set2:
                    questionnaireText.fontSize = 100;
                    //questionnaireText.text = "Preparing for Lift2";
                    //�K�v�Ȃ�ҋ@���Ԃ�݂��āA���̌�Start�ɑJ��
                    //StartCoroutine(DelayMethod(0.5f, State.Lift2));
                    ChangeState(State.Ready2);
                    break;
                case State.Ready2:
                    readyTime = ReadyTime;

                    break;
                case State.Lift2:
                    questionnaireText.fontSize = 100;
                    questionnaireText.text = "Lift2";
                    

                    //2��ڂ̎����グ������K�p����
                    if (ref_or_com == 0)
                    {
                        //�L���[�u���A�N�e�B�u�ɂ���
                        rightHoverPosition = initialRightHoverPosition;
                        leftHoverPosition = initialLeftHoverPosition;
                        hoverScale = initialHoverScale;
                        cubeRef.SetActive(true);

                        SetCubePosition(initialCubePosition);
                        //�A���B�̏����̂������̒i�K�őI�΂�Ă�����̂��I�u�W�F�N�g�Ǝ�ɓK�p����i����͊֐������j
                        SetGainAndHandSize(handSize_B, gain_B, rightHoverPosition, leftHoverPosition, hoverScale);
                    }
                    else
                    {
                        if (pattern1)
                        {
                            rightHoverPosition = rightHoverPosition060;
                            leftHoverPosition = leftHoverPosition060;
                            hoverScale = hoverScale060;
                            cubeSmall.SetActive(true);

                        }
                        else if (pattern2)
                        {
                            rightHoverPosition = rightHoverPosition150;
                            leftHoverPosition = leftHoverPosition150;
                            hoverScale = hoverScale150;
                            cubeBig.SetActive(true);
                        }
                        
                        SetCubePosition(initialCubePosition);
                        //�@���C�̏����̂������̒i�K�őI�΂�Ă�����̂��I�u�W�F�N�g�Ǝ�ɓK�p����i����͊֐������j
                        SetGainAndHandSize(handSize_A, gain_A, rightHoverPosition, leftHoverPosition, hoverScale);
                    }
                    break;
                case State.Answer:
                    //�����\��
                    SetCubePosition(initialCubePosition);
                    questionnaireText.fontSize = 80;
                    questionnaireText.text = "�ǂ���̕��̂̕����d�������ł����H\n�ŏ��̕��� �� ���g���K�[\n�Ō�̕��� �� �E�g���K�[\nWhich object is heavier? \nFirst object �� left trigger \nLast object �� right trigger";

                    Debug.Log("gain_B" + gain_B);
                    startCount++;
                    startAnswer = Time.time; // �J�n���Ԃ��i�[

                    break;
                case State.End:
                    //�I������
                    questionnaireText.fontSize = 100;
                    questionnaireText.text = "�����I���ł�\nEnd";
                    Debug.Log("EndSave");
                    saveCsvScript.EndSave();
                    break;
            }
        }
    }

    private void SetGainAndHandSize(float handSize, int gain, Vector3 rightHoverPos, Vector3 leftHoverPos, Vector3 HoverScl)
    {
        //��ƃI�u�W�F�N�g��handSize��gain��K�p����
        //��̏���
        GameObject[] hands = GameObject.FindGameObjectsWithTag("SizeHand");
        foreach (GameObject hand in hands)
        {
            hand.transform.localScale = new Vector3(handSize, handSize, handSize);
        }
        //Gain�̏���
        MethodInfo methodInfo = component.GetType().GetMethod("VariableChanger");
        MethodInfo methodInfoSmall = componentSmall.GetType().GetMethod("VariableChanger");
        MethodInfo methodInfoBig = componentBig.GetType().GetMethod("VariableChanger");
        float inputGain = IntToFloat(gain);
        object[] parameters = new object[] { inputGain };
        methodInfo.Invoke(component, parameters);
        methodInfoSmall.Invoke(componentSmall, parameters);
        methodInfoBig.Invoke(componentBig, parameters);



        //ColliderScale
        GameObject[] handColliders = GameObject.FindGameObjectsWithTag("Collider");
        foreach (GameObject handCollider in handColliders)
        {
            handCollider.transform.localScale = new Vector3(handSize, handSize, handSize);
        }
        //����̎w�����t�]�̂��߁A�ʑΉ�
        GameObject[] leftFingerColliders = GameObject.FindGameObjectsWithTag("LeftFingerCollider");
        foreach (GameObject leftFingerCollider in leftFingerColliders)
        {
            leftFingerCollider.transform.localScale = new Vector3(-1.0f * (handSize), handSize, handSize);
        }

        //HoverPoint����
        GameObject rightHoverPoint = GameObject.FindGameObjectWithTag("RightHoverPoint");
        rightHoverPoint.transform.localPosition = rightHoverPos;
        rightHoverPoint.transform.localScale = HoverScl;

        GameObject leftHoverPoint = GameObject.FindGameObjectWithTag("LeftHoverPoint");
        leftHoverPoint.transform.localPosition = leftHoverPos;
        leftHoverPoint.transform.localScale = HoverScl;

    }


    //�Q�C���ύX����
    void ChangeGain(string answer)
    {
        //des��asc�̂ǂ���̃Q�C����ύX���邩�𔻒�
        if (low_or_high == 1)
        {
            ProcessGain(answer, ref gain_High);

        }
        else
        {
            ProcessGain(answer, ref gain_Low);

        }
        //�L�^
        //����������؂�ւ�(�Q�C���������I����Ă���؂�ւ��ɕύX)
        //SendCSV();
        SwitchCondition();
    }

    //�Q�C���ύX�ɕK�v�ȏ��������ׂĊ֐���
    //�����F��(yes=true,no=false), �ύX�������Q�C���̕ϐ�(�Q�Ɠn������)
    void ProcessGain(string ans, ref int gain)
    {
        //�񓚂�Yes�Ȃ�Q�C�������Z�ANo�Ȃ猸�Z
        //�Ăяo���ꂽ�ۂ̏�����Asc��Des�����m�F���đΉ�����l�̂ݕύX
        if (ans == Reference)
        {
            gain -= APValue;
        }
        else if (ans == Comparison)
        {
            gain += APValue;
        }
        //Debug.Log("Gain : "+gain);

        //�܂�Ԃ������邩�ǂ����̊m�F�Ə���
        CheckTurnBack(ans, ref gain);

        //���݂̒l�����X�g�����ɒǉ�
        if (permitAdd)
        {
            RecordList(gain);
        }
        else
        {
            permitAdd = true;
        }

        //���݂̒l��\��(�f�o�b�O�p)
        //Debug.Log((isDes ? "des" : "asc") + " - SlideGain: " + gain);
    }

    //�܂�Ԃ��̗L���Ɖ񐔁A�t���O�̏������s��
    //�����F��(yes=true,no=false), �Q�C���̕ϐ�
    void CheckTurnBack(string ans, ref int gain)
    {
        //�����Q�C�������/�����l���I�[�o�[���Ă����ꍇ�A�l�����/�����ɌŒ肵�ăJ�E���g���Z
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

        //�񓚂��O��ƈႤ�����m�F���āA�Ⴄ�ꍇ�ɂ͐܂�Ԃ��񐔂̑�����preAnswer�̕ύX�A�I�������̊m�F�Ə������s��
        if (low_or_high == 1)
        {
            if (isFirstDes)
            {
                //1��ڂ̏���
                preAnswerDes = ans;
                isFirstDes = false;
                //Debug.Log("preAnswerDesA" + preAnswerDes);
                //Debug.Log("Trial:1 Gain = 1.0/0.25");
            }
            if (ans != preAnswerDes)
            {
                CountProcess(gain);
                //preAnswer��ύX
                preAnswerDes = ans;
                //Debug.Log("preAnswerDesB" + preAnswerDes);
            }
        }
        else if (low_or_high == 0)
        {
            if (isFirstAsc)
            {
                //1��ڂ̏���
                preAnswerAsc = ans;
                isFirstAsc = false;
            }
            if (ans != preAnswerAsc)
            {
                CountProcess(gain);
                //preAnswer��ύX
                preAnswerAsc = ans;
            }
        }

    }

    //�܂�Ԃ������������ۂ̏������s��
    //�����F�Q�C���̒l
    void CountProcess(int x)
    {
        float gain = IntToFloat(x);
        //�܂�Ԃ��n�_�ł̃Q�C���̒l��data�Ɋi�[
        string data = gain.ToString();
        if (low_or_high == 1)
        {
            //�O�����Z�q�ƃC���N�������g��p����des�̐܂�Ԃ���+1���Čp�������𖞂����Ă��邩�̊m�F�Ə������s��
            isContinue_high = (++orikaeshiNum_High > 4) ? false : true;
            //saveCsvScript.SetAnswerData3(pattern,low_or_high,orikaeshiNum_High, x);
            permitAdd = isContinue_high;
            Debug.Log("<color=blue>desNum: </color>" + orikaeshiNum_High);
            if (!isContinue_high)
            {
                block_Low_High = 1;
            }
        }
        else
        {
            //�O�����Z�q�ƃC���N�������g��p����Asc�̐܂�Ԃ���+1���Čp�������𖞂����Ă��邩�̊m�F�Ə������s��
            isContinue_low = (++orikaeshiNum_Low > 4) ? false : true;
            //saveCsvScript.SetAnswerData3(pattern, low_or_high, orikaeshiNum_Low, x);
            permitAdd = isContinue_low;
            Debug.Log("<color=red>ascNum: </color>" + orikaeshiNum_Low);
            if (!isContinue_low)
            {
                block_Low_High = 0;
            }

        }
    }



    //�t���O�̒l����K�؂ȏ����ɐ؂�ւ����s���֐�
    void SwitchCondition()
    {
        //Debug.Log("�m�F�p");
        if (!isContinue_low && !isContinue_high)
        {
            //Debug.Log(isContinue);
            ChangeState(State.End);
            Debug.Log("�I��");
            return;
        }
        else
        {
            ChangeState(State.Set);
            Debug.Log("�p��");
        }
    }

    void SetCubePosition(Vector3 position)
    {
        cubeRef.transform.localPosition = position;
        cubeSmall.transform.localPosition = position;
        cubeBig.transform.localPosition = position;

        // Rigidbody �R���|�[�l���g���擾
        Rigidbody rb = cubeRef.GetComponent<Rigidbody>();
        Rigidbody rbSmall = cubeSmall.GetComponent<Rigidbody>();
        Rigidbody rbBig = cubeBig.GetComponent<Rigidbody>();

        // Rigidbody �����݂���ꍇ�A���x�Ɗp���x�����Z�b�g
        if (rb != null)
        {
            rb.velocity = Vector3.zero;       // ���x�����Z�b�g
            rb.angularVelocity = Vector3.zero; // �p���x�����Z�b�g
        }
        else if (rbSmall != null)
        {
            rbSmall.velocity = Vector3.zero;       // ���x�����Z�b�g
            rbSmall.angularVelocity = Vector3.zero; // �p���x�����Z�b�g
        }
        else if (rbBig != null)
        {
            rbBig.velocity = Vector3.zero;       // ���x�����Z�b�g
            rbBig.angularVelocity = Vector3.zero; // �p���x�����Z�b�g
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

        //�擪�f�[�^���폜
        answerList.RemoveAt(0);
    }



    float IntToFloat(int a)
    {
        return (float)a / 1000.0f;
    }

}
