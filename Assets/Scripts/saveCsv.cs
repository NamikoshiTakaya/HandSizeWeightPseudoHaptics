using UnityEngine;
using System.IO;
using System.Text;

public class saveCsv : MonoBehaviour
{
    private StreamWriter sw;
    private StreamWriter sw2;
    private StreamWriter sw3;
    private StreamWriter sw4;
    private StreamWriter sw5;
    private bool isClosed = false;


    void Awake()
    {
        sw = new StreamWriter(@"Q1.csv", true, Encoding.GetEncoding("Shift_JIS"));
        string[] s1 = { "TrialNumber", "Condition", "Gain", "Anser" };
        string st = string.Join(",", s1);
        sw.WriteLine(st);


        sw2 = new StreamWriter(@"Q2.csv", true, Encoding.GetEncoding("Shift_JIS"));
        string[] s2 = { "TrialNumber", "Condition", "Gain", "VAS" };
        string st2 = string.Join(",", s2);
        sw2.WriteLine(st2);

        sw3 = new StreamWriter(@"HandTracking.csv", true, Encoding.GetEncoding("Shift_JIS"));
        string[] s4 = { "Time", "TrialNumber", "Hand Tracking" };
        string s5 = string.Join(",", s4);
        sw3.WriteLine(s5);

        sw4 = new StreamWriter(@"EyeTracking.csv", true, Encoding.GetEncoding("Shift_JIS"));
        string[] s6 = { "Time", "TrialNumber", "Eye Tracking" };
        string s7 = string.Join(",", s6);
        sw4.WriteLine(s7);

        sw5 = new StreamWriter(@"Q1TurningGain.csv", true, Encoding.GetEncoding("Shift_JIS"));
        string[] s8 = { "Condition", "Frist", "Second", "Third", "Fourth", "Fifth" };
        string s9 = string.Join(",", s8);
        sw5.WriteLine(s9);

    }

    public void SaveDataQ1(string txt1, string txt2, string txt3, string txt4)
    {
        string[] s1 = { txt1, txt2, txt3, txt4 };
        string st = string.Join(",", s1);
        //Debug.Log(st);
        //Debug.Log("savedataQ1");
        sw.WriteLine(st);
    }

    public void SaveDataQ2(string txt1, string txt2, string txt3, string txt4)
    {
        string[] s2 = { txt1, txt2, txt3, txt4 };
        string st2 = string.Join(",", s2);
        //Debug.Log("savedataQ2");
        sw2.WriteLine(st2);
    }

    public void SaveDataHT(string txt1, string txt2, string txt3)
    {
        string[] s4 = { txt1, txt2, txt3 };
        string s5 = string.Join(",", s4);
        //Debug.Log("savedataHT");
        sw3.WriteLine(s5);
    }

    public void SaveDataET(string txt1, string txt2, string txt3, string txt4, string txt5)
    {
        string[] s6 = { txt1, txt2, txt3, txt4, txt5 };
        string s7 = string.Join(",", s6);
        //Debug.Log("savedataET");
        sw4.WriteLine(s7);
    }

    public void SaveDataTurningGain(string s)
    {
        //string s8 = string.Join(",", s);
        //Debug.Log("SaveDataTurningGain");
        sw5.WriteLine(s);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            sw.Close();
            sw2.Close();
            sw3.Close();
            sw4.Close();
            sw5.Close();
            isClosed = true;
        }

    }
}

