using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveCsvScript3 : MonoBehaviour
{
    private StreamWriter sw, swB;
    int[,] answerData = { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } };



    void Start()
    {
        sw = new StreamWriter(@"SaveExperimentData3.csv", true, Encoding.GetEncoding("Shift_JIS"));
        string[] s1 = { "set", "Gain", "MoreHeavy", "low_or_high", "ref_or_com", "des", "asc" }; 
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);

        swB = new StreamWriter(@"SaveExperimentAnalyzeData3.csv", true, Encoding.GetEncoding("Shift_JIS"));
        string[] s3 = { "pattern",  "First", "Second", "Third", "Fourth", "Fifth" };
        string s4 = string.Join(",", s3);
        swB.WriteLine(s4);
    }

    //外部から保存したいデータを引数にして呼び出し
    public void SaveDataEx3(string txt1, string txt2, string txt3, string txt4, string txt5, string txt6, string txt7)
    {
        string[] s1 = { txt1, txt2, txt3, txt4, txt5, txt6, txt7};
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);
    }

    public void SetAnswerData3(int pattern,int lowHigh, int orikaeshiNum, int pseudoGain)
    {
        int i = 0;
        if (pattern == 0) 
        {
            if (lowHigh == 0)
            {
                i = 0;
            }
            else if (lowHigh == 1)
            {
                i = 1;
            }
        }
        else if (pattern == 1)
        {
            if (lowHigh == 0)
            {
                i = 2;
            }
            else if(lowHigh == 1)
            {
                i = 3;
            }
        }

        float[] array = { 1, 2, 3, 4, 5 };
        for (int j = 0; j < array.Length; j++)
        {
            if (array[j] == orikaeshiNum)
            {
                answerData[i, j] = pseudoGain;
                break;
            }
        }
    }

    public void WriteAnswer()
    {
        string[] answerStringA = { "1Small-Asc", answerData[0, 0].ToString(), answerData[0, 1].ToString(), answerData[0, 2].ToString(), answerData[0, 3].ToString(), answerData[0, 4].ToString() };
        string[] answerStringB = { "1Small-Des", answerData[1, 0].ToString(), answerData[1, 1].ToString(), answerData[1, 2].ToString(), answerData[1, 3].ToString(), answerData[1, 4].ToString() };
        string[] answerStringC = { "2Big-Asc", answerData[2, 0].ToString(), answerData[2, 1].ToString(), answerData[2, 2].ToString(), answerData[2, 3].ToString(), answerData[2, 4].ToString() };
        string[] answerStringD = { "2Big-Des", answerData[3, 0].ToString(), answerData[3, 1].ToString(), answerData[3, 2].ToString(), answerData[3, 3].ToString(), answerData[3, 4].ToString() };
        string sAnswer = string.Join(",", answerStringA);
        swB.WriteLine(sAnswer);
        string sAnswerB = string.Join(",", answerStringB);
        swB.WriteLine(sAnswerB);
        string sAnswerC = string.Join(",", answerStringC);
        swB.WriteLine(sAnswerC);
        string sAnswerD = string.Join(",", answerStringD);
        swB.WriteLine(sAnswerD);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            WriteAnswer();
            sw.Close();
            swB.Close();
        }

    }

    public void EndSave()
    {
        WriteAnswer();
        sw.Close();
        swB.Close();
    }
}