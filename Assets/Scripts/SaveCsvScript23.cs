using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveCsvScript23 : MonoBehaviour
{
    private StreamWriter sw, swB;
    int[,] answerData = { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } };



    void Start()
    {
        sw = new StreamWriter(@"SaveExperimentData23.csv", true, Encoding.GetEncoding("Shift_JIS"));
        string[] s1 = { "セット数", "基準HandSize", "基準HeavyGain", "HandSize_Com","HeavyGain_Com", "より重かった方", "そのHand", "そのHeavy", "refA_or_refB","ref_or_com" };
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);

        swB = new StreamWriter(@"SaveExperimentAnalyzeData23.csv", true, Encoding.GetEncoding("Shift_JIS"));
        string[] s3 = { "HeavyGain", "0.4", "0.6", "0.8", "1.25", "1.5", "1.75" };
        string s4 = string.Join(",", s3);
        swB.WriteLine(s4);
    }

    //外部から保存したいデータを引数にして呼び出し
    public void SaveDataEx23(string txt1, string txt2, string txt3, string txt4, string txt5, string txt6, string txt7, string txt8, string txt9, string txt10)
    {
        string[] s1 = { txt1, txt2, txt3, txt4, txt5, txt6, txt7, txt8, txt9, txt10};
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);
    }

    public void SetAnswerData23(int pseudoGain_Ref, float comparison)
    {
        int i = 0;
        if (pseudoGain_Ref == 800)
        {
            i = 0;
        }
        else if (pseudoGain_Ref == 1200)
        {
            i = 1;
        }
        
        float[] array = { 0.40f, 0.60f, 0.80f, 1.25f, 1.50f, 1.75f };
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
        string[] answerStringA = { "800", answerData[0, 0].ToString(), answerData[0, 1].ToString(), answerData[0, 2].ToString(), answerData[0, 3].ToString(), answerData[0, 4].ToString(), answerData[0, 5].ToString() };
        string[] answerStringB = { "1200", answerData[1, 0].ToString(), answerData[1, 1].ToString(), answerData[1, 2].ToString(), answerData[1, 3].ToString(), answerData[1, 4].ToString(), answerData[1, 5].ToString() };
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