using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q2dataStock : MonoBehaviour
{
    int trialNumber = 1;
    string condition = "Start";
    string gain = "Start";
    string answer = "Start";
    public saveCsv SampleSaveCsvScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(condition != "Start" && gain != "Start" && answer != "Start")
        {
            //Debug.Log("Q1dataStockcheck");
            SampleSaveCsvScript.SaveDataQ2(trialNumber.ToString(), condition, gain, answer);
            Debug.Log("Q2DataSend");
            condition = "Start";
            gain = "Start";
            answer = "Start";
            trialNumber ++;
        }
    }

    public void SetCondition(bool isHigh)
    {
        if(isHigh == true)
        {
            condition = "Descending";
            //Debug.Log("condDesOK");
        }
        else{
            condition = "Asending";
            //Debug.Log("condition"+condition);            
        }

    }
    public void SetGain(float x)
    {
        gain = x.ToString();
        //Debug.Log("Gain"+gain);
    }
    public void SetAnswer(string s)
    {
        answer = s;
        //Debug.Log("Answer"+answer);
    }

    //初期値1でsavedataQ1の呼び出し後に値を+1 = TrialNumber

}

