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

public class Ex2 : MonoBehaviour
{

    //�X�e�[�g�p�ϐ�
    State currentState = State.Idle;

    private State _nextState;

    //���Z�b�g�ڂ����L�^(�ő�12���)
    private int setCount = 1;

    //�Z�b�g������ڂ����L�^(�ő�6���)
    private int comCount = 0;



    //���ݎ������̃p�^�[�����p�^�[���P�Ȃ�true
    //private bool isPattern1 = true;

    //�I�u�W�F�N�g�錾
    public GameObject lefthand;
    public GameObject righthand;

    /*public Renderer lefthands;
    public Renderer righthands;*/

    //GameObject cube;
    public GameObject cubeRef;
    public GameObject cube025;
    public GameObject cube050;
    public GameObject cube075;
    public GameObject cube125;
    public GameObject cube150;
    public GameObject cube175;
    /*public GameObject cube140;
    public GameObject cube180;
    public GameObject cube220;*/
    private UnityEngine.Component component;
    //Text�ω��p
    public UnityEngine.UI.Text questionnaireText;
    public SaveCsvScript2 saveCsvScript;

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

    //0.25
    private Vector3 rightHoverPosition025 = new Vector3(0.0278f, 0.026f, -0.150f);
    private Vector3 leftHoverPosition025 = new Vector3(-0.0278f, 0.026f, -0.150f);
    private Vector3 hoverScale025 = new Vector3(0.3f, 0.3f, 0.3f);

    //0.50
    private Vector3 rightHoverPosition050 = new Vector3(0.0245f, 0.0164f, -0.1416f);
    private Vector3 leftHoverPosition050 = new Vector3(-0.0245f, 0.0164f, -0.1416f);
    private Vector3 hoverScale050 = new Vector3(0.4f, 0.4f, 0.4f);

    //0.75
    private Vector3 rightHoverPosition075 = new Vector3(0.0265f,-0.0138f,-0.1141f);
    private Vector3 leftHoverPosition075 = new Vector3(-0.0265f, -0.0138f, -0.1141f);
    private Vector3 hoverScale075 = new Vector3(0.5f, 0.5f, 0.5f);



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




    [SerializeField] private const int RefGainValue = 1000;

    private const string Reference = "reference";
    private const string Comparison = "comparison";

    //�A���P�[�g�̉񓚂�Yes��true�ANo��false�ŋL�^����ϐ�
    //private bool answer = true;
    private string answer;

    [Header("��̃T�C�Y�̒l")]
    public List<float> handSizes = new List<float> { 0.25f, 0.50f, 0.75f, 1.25f, 1.50f, 1.75f };
    private List<float> shuffledHandSizes;
    // �V���b�t���p�̃J�E���g
    //private int currentIndex = 0; 
    //�@�n���h�T�C�Y�̊
    private float handSizeRef = 1.0f;
    private float handSizeCom = 1.0f;

    //0�Ȃ��l�����ɒl���Z�b�g����
    private int ref_or_com = 0;

    

    //�������Ɏ��ۂɎg���ϐ��i�@���C�̐��l��A�ɁA�A���B�̐��l��B�ɓ���j
    private Vector3 handSize_A = Vector3.zero;
    private Vector3 handSize_B = Vector3.zero;
    private int gain_A = 1000;
    private int gain_B = 1000;

    List<float> answerList = new List<float>();

    IEnumerator DelayMethod(float delay, State newState)
    {
        //delay�b�҂�
        yield return new WaitForSeconds(delay);
        ChangeState(newState);
    }
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
        /*questionnaireText.fontSize = 80;
        questionnaireText.text = "Before start\n�������ł�����E�̃g���b�N�p�b�h�̃{�^���������Ă�������\nPress the right TrackPadButton when you are ready";
        */
        questionnaireText.text = "";
        ValueInit();
    }

    //�ϐ��̏�����
    void ValueInit()
    {
        
        // �C���^���N�^�u���R���|�[�l���g�̎擾
        component = cubeRef.GetComponent<Valve.VR.InteractionSystem.Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        StateFunction();
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
                /*questionnaireText.fontSize = 80;
                questionnaireText.text = "Before start\n�������ł�����E�̃g���b�N�p�b�h�̃{�^���������Ă�������\nPress the right TrackPadButton when you are ready";
*/
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
                if (Tpb.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    ChangeState(State.Lift1);
                    //StartCoroutine(DelayMethod(1.0f, State.Lift1));
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
            case State.Lift2:
                //���̃g���b�N�p�b�h�{�^����Cube�̈ʒu�������ʒu�Ƀ��Z�b�g
                if (Tpb.GetStateDown(SteamVR_Input_Sources.LeftHand))
                {
                    SetCubePosition(initialCubePosition);
                }
                //�����グ���I�������Answer�֑J��
                if (Tpb.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    ChangeState(State.Answer);
                }
                //�~�X�����Ƃ��̖߂邽�߂̋@�\
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    //Debug.Log("��"); 
                    ChangeState(State.Lift1);
                    
                }
                break;
            case State.Answer:
                //�A���P�[�g�񓚎�t
                //�A���P�[�g�\��
                //�A���P�[�g�񓚂��ꂽ��Set�ɖ߂�
                //���ʂ�GetState�Ŏ擾����interacrtui�Ɋi�[
                //SteamVR_Input_Sources.�@�햼
                interacrtuiRight = Iui.GetState(SteamVR_Input_Sources.RightHand);
                interacrtuiLeft = Iui.GetState(SteamVR_Input_Sources.LeftHand);

                //savedata�ɓ����p�̕ϐ��@���܂�C�ɂ��Ȃ���
                string stSetCount = setCount.ToString();
                string stComCount = (comCount + 1).ToString();
                string stHandSizeRef = handSizeRef.ToString();
                string stHandSizeCom = handSizeCom.ToString();

                //�~�X�����Ƃ��̖߂邽�߂̋@�\
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (comCount != 0)
                    {
                        comCount--;
                    }
                    else if (comCount == 0)
                    {
                        comCount = 5;
                        setCount--;
                    }
                    //Debug.Log("��");
                    ChangeState(State.Lift2);
                }

                //�񓚂̋L�^
                if (interacrtuiLeft)
                {
                    if (ref_or_com == 0)
                    {
                        answer = Reference;
                        saveCsvScript.SaveDataEx2(stSetCount + " - (" + stComCount + ")", stHandSizeRef, stHandSizeCom, ref_or_com.ToString(), answer, stHandSizeRef);
                        CheckEnd();
                    }
                    else
                    {
                        answer = Comparison;
                        saveCsvScript.SaveDataEx2(stSetCount + " - (" + stComCount + ")", stHandSizeCom, stHandSizeRef, ref_or_com.ToString(), answer, stHandSizeCom); 
                        CheckEnd();
                    }
                }
                //
                else if (interacrtuiRight)
                {
                    if (ref_or_com == 0)
                    {
                        answer = Comparison;
                        saveCsvScript.SaveDataEx2(stSetCount + " - (" + stComCount + ")", stHandSizeRef, stHandSizeCom, ref_or_com.ToString(), answer, stHandSizeCom);
                        CheckEnd();
                    }
                    else
                    {
                        answer = Reference;
                        saveCsvScript.SaveDataEx2(stSetCount + " - (" + stComCount + ")", stHandSizeCom, stHandSizeRef, ref_or_com.ToString(), answer, stHandSizeRef);
                        CheckEnd();
                    }
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
                    //�J�n�O����
                    cubeRef.SetActive(false);
                    cube025.SetActive(false);
                    cube050.SetActive(false);
                    cube075.SetActive(false);
                    cube125.SetActive(false);
                    cube150.SetActive(false);
                    cube175.SetActive(false);
                    /*
                                        cube140.SetActive(false);
                                        cube180.SetActive(false);
                                        cube220.SetActive(false);*/
                    SetGainAndHandSize(0.0f, gain_A, rightHoverPosition, leftHoverPosition, hoverScale);

                    break;
                case State.Start:
                    //�����グ�P�C�Q��ڂ̊e����������i���̌�Lift1�ɑJ�ځj
                    ref_or_com = UnityEngine.Random.Range(0, 2);
                    //ref_or_com = 1;
                    Debug.Log("ref_or_com: " + ref_or_com);
                    break;
                case State.Lift1:
                    //�I�u�W�F�N�g�������Ȃǂ�Lift1�̏I���������L�q�i���̌�Lift2�֑J�ځj
                    //����
                    cubeRef.SetActive(false);
                    cube025.SetActive(false);
                    cube050.SetActive(false);
                    cube075.SetActive(false);
                    cube125.SetActive(false);
                    cube150.SetActive(false);
                    cube175.SetActive(false);
                    /*cube140.SetActive(false);
                    cube180.SetActive(false);
                    cube220.SetActive(false);*/
                    SetGainAndHandSize(0.0f, gain_A, rightHoverPosition, leftHoverPosition, hoverScale);
                    break;
                case State.Set2:

                    break;
                case State.Lift2:
                    //�I�u�W�F�N�g�������Ȃǂ�Lift2�̏I���������L�q�i���̌�Answer�֑J�ځj
                    cubeRef.SetActive(false);
                    cube025.SetActive(false);
                    cube050.SetActive(false);
                    cube075.SetActive(false);
                    cube125.SetActive(false);
                    cube150.SetActive(false);
                    cube175.SetActive(false);
                    /*cube140.SetActive(false);
                    cube180.SetActive(false);
                    cube220.SetActive(false);*/
                    SetGainAndHandSize(0.0f, gain_A, rightHoverPosition, leftHoverPosition, hoverScale);

                    break;
                case State.Answer:
                    if (comCount != 5)
                    {
                        comCount++;
                    }
                    else if (comCount == 5)
                    {
                        comCount = 0;
                        setCount++;
                    }
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
                    cube025.SetActive(false);
                    cube050.SetActive(false);
                    cube075.SetActive(false);
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
                    if (comCount == 0)
                    {
                        ShuffleHandSizes();
                    }
                    handSizeCom = shuffledHandSizes[comCount];

                    Debug.Log(handSizeCom);
                    //�K�v�Ȃ�ҋ@���Ԃ�݂��āA���̌�Start�ɑJ��
                    StartCoroutine(DelayMethod(0.5f, State.Start));
                    break;
                case State.Start:
                    questionnaireText.fontSize = 80;
                    questionnaireText.text = "�������ł�����E�̃g���b�N�p�b�h�̃{�^���������Ă�������\nPress the right TrackPadButton when you are ready";

                    Debug.Log("ExperimentCount: " + setCount + "-" + comCount+1);
                    break;
                case State.Lift1:
                    //1��ڂ̎����グ������K�p����
                    questionnaireText.fontSize = 100;
                    questionnaireText.text = setCount + "-" + (comCount+1) + "\nLift1";
                    //lefthand.SetActive(true);
                    //righthand.SetActive(true);
                    //lefthands.enabled = true;
                    //righthands.enabled = true;
                    if (ref_or_com == 0)
                    {
                        //�L���[�u���A�N�e�B�u�ɂ���
                        rightHoverPosition = initialRightHoverPosition;
                        leftHoverPosition = initialLeftHoverPosition;
                        hoverScale = initialHoverScale;
                        cubeRef.SetActive(true);
                        SetCubePosition(initialCubePosition);
                        //�@���C�̏����̂������̒i�K�őI�΂�Ă�����̂��I�u�W�F�N�g�Ǝ�ɓK�p����i����͊֐������j
                        SetGainAndHandSize(handSizeRef, gain_A, rightHoverPosition, leftHoverPosition, hoverScale);
                    }
                    else
                    {
                        if (handSizeCom == 0.25f)
                        {
                            rightHoverPosition = rightHoverPosition025;
                            leftHoverPosition = leftHoverPosition025;
                            hoverScale = hoverScale025;
                            cube025.SetActive(true);
                        }
                        else if (handSizeCom == 0.50f)
                        {
                            rightHoverPosition = rightHoverPosition050;
                            leftHoverPosition = leftHoverPosition050;
                            hoverScale = hoverScale050;
                            cube050.SetActive(true);
                        }
                        else if (handSizeCom == 0.75f)
                        {
                            rightHoverPosition = rightHoverPosition075;
                            leftHoverPosition = leftHoverPosition075;
                            hoverScale = hoverScale075;
                            cube075.SetActive(true);
                        }
                        else if (handSizeCom == 1.25f)
                        {
                            rightHoverPosition = rightHoverPosition125;
                            leftHoverPosition = leftHoverPosition125;
                            hoverScale = hoverScale125;
                            cube125.SetActive(true);
                        }
                        else if(handSizeCom == 1.50f)
                        {
                            rightHoverPosition = rightHoverPosition150;
                            leftHoverPosition = leftHoverPosition150;
                            hoverScale = hoverScale150;
                            cube150.SetActive(true);
                        }
                        else if(handSizeCom == 1.75f)
                        {
                            rightHoverPosition = rightHoverPosition175;
                            leftHoverPosition = leftHoverPosition175;
                            hoverScale = hoverScale175;
                            cube175.SetActive(true);
                        }
                        /*else if (handSizeCom == 1.40f)
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
                        //�A���B�̏����̂������̒i�K�őI�΂�Ă�����̂��I�u�W�F�N�g�Ǝ�ɓK�p����i����͊֐������j
                        SetGainAndHandSize(handSizeCom, gain_B, rightHoverPosition, leftHoverPosition, hoverScale);
                    }
                    break;
                case State.Set2:
                    questionnaireText.fontSize = 100;
                    questionnaireText.text = "Lift2�̏�����\nPreparing for Lift2";
                    //�K�v�Ȃ�ҋ@���Ԃ�݂��āA���̌�Start�ɑJ��
                    StartCoroutine(DelayMethod(3.0f, State.Lift2));
                    break;
                case State.Lift2:
                    questionnaireText.fontSize = 100;
                    questionnaireText.text = setCount + "-" + (comCount + 1) + "\nLift2";

                    //lefthand.SetActive(true);
                    //righthand.SetActive(true);
                    //lefthands.enabled = true;
                    //righthands.enabled = true;



                    //2��ڂ̎����グ������K�p����
                    if (ref_or_com == 0)
                    {
                        //�L���[�u�ƃn���h���A�N�e�B�u�ɂ���
                        if (handSizeCom == 0.25f)
                        {
                            rightHoverPosition = rightHoverPosition025;
                            leftHoverPosition = leftHoverPosition025;
                            hoverScale = hoverScale025;
                            cube025.SetActive(true);
                        }
                        else if (handSizeCom == 0.50f)
                        {
                            rightHoverPosition = rightHoverPosition050;
                            leftHoverPosition = leftHoverPosition050;
                            hoverScale = hoverScale050;
                            cube050.SetActive(true);
                        }
                        else if (handSizeCom == 0.75f)
                        {
                            rightHoverPosition = rightHoverPosition075;
                            leftHoverPosition = leftHoverPosition075;
                            hoverScale = hoverScale075;
                            cube075.SetActive(true);
                        }
                        else if (handSizeCom == 1.25f)
                        {
                            rightHoverPosition = rightHoverPosition125;
                            leftHoverPosition = leftHoverPosition125;
                            hoverScale = hoverScale125;
                            cube125.SetActive(true);
                        }
                        else if (handSizeCom == 1.50f)
                        {
                            rightHoverPosition = rightHoverPosition150;
                            leftHoverPosition = leftHoverPosition150;
                            hoverScale = hoverScale150;
                            cube150.SetActive(true);
                        }
                        else if (handSizeCom == 1.75f)
                        {
                            rightHoverPosition = rightHoverPosition175;
                            leftHoverPosition = leftHoverPosition175;
                            hoverScale = hoverScale175;
                            cube175.SetActive(true);
                        }
                        /*else if (handSizeCom == 1.40f)
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
                        //�A���B�̏����̂������̒i�K�őI�΂�Ă�����̂��I�u�W�F�N�g�Ǝ�ɓK�p����i����͊֐������j
                        SetGainAndHandSize(handSizeCom, gain_B, rightHoverPosition, leftHoverPosition, hoverScale);
                    }
                    else
                    {
                        rightHoverPosition = initialRightHoverPosition;
                        leftHoverPosition = initialLeftHoverPosition;
                        hoverScale = initialHoverScale;
                        cubeRef.SetActive(true);
                        SetCubePosition(initialCubePosition);
                        //�@���C�̏����̂������̒i�K�őI�΂�Ă�����̂��I�u�W�F�N�g�Ǝ�ɓK�p����i����͊֐������j
                        SetGainAndHandSize(handSizeRef, gain_A, rightHoverPosition, leftHoverPosition, hoverScale);
                    }
                    break;
                case State.Answer:
                    //�����\��
                    SetCubePosition(initialCubePosition);
                    questionnaireText.fontSize = 80;
                    questionnaireText.text = "�ǂ���̕��̂̕����d���������ł����H\nWhich object is heavier?? \nFirst object �� left trigger \nLast object �� right trigger";
                    break;
                case State.End:
                    //�I������
                    questionnaireText.fontSize = 100;
                    questionnaireText.text = "��ڂ̎����I���ł�\nEnd";
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
            hand.transform.localScale = new Vector3(handSize,handSize,handSize);
        }
        //Gain�̏���
        MethodInfo methodInfo = component.GetType().GetMethod("VariableChanger");
        float inputGain = IntToFloat(gain);
        object[] parameters = new object[] { inputGain };
        methodInfo.Invoke(component, parameters);

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
            leftFingerCollider.transform.localScale = new Vector3(-1.0f* (handSize), handSize, handSize);
        }

        //HoverPoint����
        GameObject rightHoverPoint = GameObject.FindGameObjectWithTag("RightHoverPoint");
        rightHoverPoint.transform.localPosition = rightHoverPos;
        rightHoverPoint.transform.localScale = HoverScl;

        GameObject leftHoverPoint = GameObject.FindGameObjectWithTag("LeftHoverPoint");
        leftHoverPoint.transform.localPosition = leftHoverPos;
        leftHoverPoint.transform.localScale = HoverScl;

    }

    void ShuffleHandSizes()
    {
        // ���̃��X�g���R�s�[���ăV���b�t��
        shuffledHandSizes = new List<float>(handSizes);
        for (int i = 0; i < shuffledHandSizes.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, shuffledHandSizes.Count);
            // ���݂̗v�f�ƃ����_���Ȉʒu�̗v�f������
            float temp = shuffledHandSizes[i];
            shuffledHandSizes[i] = shuffledHandSizes[randomIndex];
            shuffledHandSizes[randomIndex] = temp;
        }
        // currentIndex = 0; // �C���f�b�N�X�����Z�b�g
        Debug.Log("Shuffled HandSizes: " + string.Join(", ", shuffledHandSizes));
    }

    //�I�������𖞂����Ă��邩���m�F����
    void CheckEnd()
    {
        if (setCount == 12 && comCount == 5)
        {
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

    float IntToFloat(int a)
    {
        return (float)a / 1000.0f;
    }


    void SetCubePosition(Vector3 position)
    {
        cubeRef.transform.localPosition = position;
        cube025.transform.localPosition = position;
        cube050.transform.localPosition = position;
        cube075.transform.localPosition = position;
        cube125.transform.localPosition = position;
        cube150.transform.localPosition = position;
        cube175.transform.localPosition = position;
        /*cube140.transform.localPosition = position;
        cube180.transform.localPosition = position;
        cube220.transform.localPosition = position;*/

        // Rigidbody �R���|�[�l���g���擾
        Rigidbody rb = cubeRef.GetComponent<Rigidbody>();
        Rigidbody rb025 = cube025.GetComponent<Rigidbody>();
        Rigidbody rb050 = cube050.GetComponent<Rigidbody>();
        Rigidbody rb075 = cube075.GetComponent<Rigidbody>();
        Rigidbody rb125 = cube125.GetComponent<Rigidbody>();
        Rigidbody rb150 = cube150.GetComponent<Rigidbody>();
        Rigidbody rb175 = cube175.GetComponent<Rigidbody>();
        /*Rigidbody rb140 = cube140.GetComponent<Rigidbody>();
        Rigidbody rb180 = cube180.GetComponent<Rigidbody>();
        Rigidbody rb220 = cube220.GetComponent<Rigidbody>();*/

        // Rigidbody �����݂���ꍇ�A���x�Ɗp���x�����Z�b�g
        if (rb != null)
        {
            rb.velocity = Vector3.zero;       // ���x�����Z�b�g
            rb.angularVelocity = Vector3.zero; // �p���x�����Z�b�g
        }
        else if (rb025 != null)
        {
            rb025.velocity = Vector3.zero;       // ���x�����Z�b�g
            rb025.angularVelocity = Vector3.zero; // �p���x�����Z�b�g
        }
        else if (rb050 != null)
        {
            rb050.velocity = Vector3.zero;       // ���x�����Z�b�g
            rb050.angularVelocity = Vector3.zero; // �p���x�����Z�b�g
        }
        else if (rb075 != null)
        {
            rb075.velocity = Vector3.zero;       // ���x�����Z�b�g
            rb075.angularVelocity = Vector3.zero; // �p���x�����Z�b�g
        }
        else if (rb125 != null)
        {
            rb125.velocity = Vector3.zero;       // ���x�����Z�b�g
            rb125.angularVelocity = Vector3.zero; // �p���x�����Z�b�g
        }
        else if (rb150 != null)
        {
            rb150.velocity = Vector3.zero;       // ���x�����Z�b�g
            rb150.angularVelocity = Vector3.zero; // �p���x�����Z�b�g
        }
        else if (rb175 != null)
        {
            rb175.velocity = Vector3.zero;       // ���x�����Z�b�g
            rb175.angularVelocity = Vector3.zero; // �p���x�����Z�b�g
        }

        /*else if (rb140 != null)
        {
            rb140.velocity = Vector3.zero;       // ���x�����Z�b�g
            rb140.angularVelocity = Vector3.zero; // �p���x�����Z�b�g
        }
        else if (rb180 != null)
        {
            rb180.velocity = Vector3.zero;       // ���x�����Z�b�g
            rb180.angularVelocity = Vector3.zero; // �p���x�����Z�b�g
        }
        else if (rb220 != null)
        {
            rb220.velocity = Vector3.zero;       // ���x�����Z�b�g
            rb220.angularVelocity = Vector3.zero; // �p���x�����Z�b�g
        }*/
    }
}
