using System.Collections.Generic;
using UnityEngine;

public class DialogParser : MonoBehaviour
{
    //딕셔너리<npc코드,대사>
    Dictionary<int, string[]> talkData;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
    }

    public Dialogue[] Parse(string _CSVFILEName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();//대사리스트 생성
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFILEName);//csv파일 가져옴

        //data = 한 줄씩 split
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length;)
        {
            //row[0] : id
            //row[1] : 이름
            //row[2] : 대사
            string[] row = data[i].Split(new char[] { '@' });

            Dialogue dialogue = new Dialogue();//대사 리스트 생성

            int strToInt = int.Parse(row[0]);
            dialogue.id = strToInt;

            dialogue.name = row[1];
            List<string> contextList = new List<string>();
            do
            {
                contextList.Add(row[2]);

                if (++i < data.Length)
                {
                    row = data[i].Split(new char[] { '@' });
                }
                else
                {
                    break;
                }
            } while (row[0].ToString() == "");

            dialogue.contexts = contextList.ToArray();
            dialogueList.Add(dialogue);
        }
        return dialogueList.ToArray();
    }
    void Start()
    {
        Parse("Dialogue");
    }
}
