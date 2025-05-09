using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class saveloadSelect : MonoBehaviour
{
    public GameObject creat;
    public Text[] slotText;
    public Text newmapname;

    bool[] savefile = new bool[4];

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            if (File.Exists(UIdataManage.instance.path + $"{i}"))
            {
                savefile[i] = true;
                UIdataManage.instance.curSlot = i;
                UIdataManage.instance.LoadData();
                slotText[i].text = UIdataManage.instance.curPlayer.name;
            }
            else
            {
                slotText[i].text = "New Game";
            }
        }
        UIdataManage.instance.DataClear();
    }

    public void Slot(int number)
    {
        UIdataManage.instance.curSlot = number;

        if (savefile[number])
        {
            UIdataManage.instance.LoadData();
            Loadgame();
        }
    }

    public void Loadgame()
    {
        if (!savefile[UIdataManage.instance.curSlot])
        {
            UIdataManage.instance.curPlayer.mapname = newmapname.text;
            UIdataManage.instance.SaveData(); 
        }
        SceneManager.LoadScene(1);
    }
}