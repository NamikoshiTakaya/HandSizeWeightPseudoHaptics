using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Test : MonoBehaviour
{
    public GameObject cube;
    GameObject handsize;


    public bool pattern1;
    public bool pattern2;

    [Tooltip("Pseudo-Haptics")]
    public bool pseudoHaptics = true;

    //Gain変更確認用
    private Component component;

    //public float pseudoGain = 0.4f;

    void Start()
    {
        InitializeExperiment();
        component = cube.GetComponent<Valve.VR.InteractionSystem.Interactable>();
        //pseudoGain = cube.GetComponent<PseudoGain>();
    }

    void InitializeExperiment()
    {
        cube.transform.position = new Vector3(0, 0.95f, 3.25f);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //確認用

            Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            //leftHand.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            InitializeExperiment();
            //pseudoGain.pseudoHapticsGain = 0.4f;
            SizeChange();

            //component.pseudoHapticsGain = 0.4f;
            MethodInfo methodInfo = component.GetType().GetMethod("VariableChanger");
            object[] parameters = new object[] { 0.4f };
            methodInfo.Invoke(component, parameters);
        }
    }

    void SizeChange()
    {
        handsize = GameObject.FindGameObjectWithTag("SizeHand");
        handsize.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
    }
}










