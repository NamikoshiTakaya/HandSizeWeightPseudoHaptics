using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveCsvScript : MonoBehaviour
{
    private StreamWriter sw, swB;
    int[,] answerData = { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } };



    void Start()
    {
        sw = new StreamWriter(@"SaveExperimentData.csv", true, Encoding.GetEncoding("Shift_JIS"));
        string[] s1 = { "Gain", "MoreHeavy", "low_or_high", "ref_or_com", "des", "asc"};
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);

        swB = new StreamWriter(@"SaveExperimentAnalyzeData.csv", true, Encoding.GetEncoding("Shift_JIS"));
        string[] s3 = { "visualIndicator", "0.4", "0.5", "0.6", "0.7", "0.8", "0.9", "1.0", "1.1", "1.2", "1.3", "1.4", "1.5", "1.6" };
        string s4 = string.Join(",", s3);
        swB.WriteLine(s4);
    }

    //外部から保存したいデータを引数にして呼び出し
    public void SaveData(string txt1, string txt2, string txt3, string txt4, string txt5, string txt6)
    {
        string[] s1 = { txt1, txt2, txt3, txt4, txt5, txt6};
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);
    }

    

    public void SetAnswerData(bool visualIndicator, float comparison)
    {
        int i = 0;
        if (visualIndicator)
        {
            i = 1;
        }
        float[] array = { 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f };
        for (int j = 0; j < array.Length; j++)
        {
            if (array[j] == comparison)
            {
                answerData[i, j] += 1;
                break;
            }
        }
    }

    public void WriteAnswer()
    {
        string[] answerStringA = { "False", answerData[0, 0].ToString(), answerData[0, 1].ToString(), answerData[0, 2].ToString(), answerData[0, 3].ToString(), answerData[0, 4].ToString(), answerData[0, 5].ToString() };
        string[] answerStringB = { "True", answerData[1, 0].ToString(), answerData[1, 1].ToString(), answerData[1, 2].ToString(), answerData[1, 3].ToString(), answerData[1, 4].ToString(), answerData[1, 5].ToString() };
        string sAnswer = string.Join(",", answerStringA);
        swB.WriteLine(sAnswer);
        string sAnswerB = string.Join(",", answerStringB);
        swB.WriteLine(sAnswerB);
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