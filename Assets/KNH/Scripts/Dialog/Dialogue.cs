using UnityEngine;

//인스펙터창에서 수정가능
[System.Serializable]
public class Dialogue
{

    [Tooltip("캐릭터 ID")]
    public int id;

    [Tooltip("캐릭터 이름")]
    public string name;

    [Tooltip("대사 내용")]
    public string[] contexts;
}

[System.Serializable]
public class DialogueEvent
{

    //이벤트 이름
    public string name;

    public Vector2 line;
    public Dialogue[] dialogues;

}
