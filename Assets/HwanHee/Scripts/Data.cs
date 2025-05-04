
// 저장
// - 플레이어
// - 상점
// - 사슴정거장 Lift


public class PlayerData
{
    public string fromSceneName;
    public string toSceneName;
    public int money = 0;     //지오
    public int lostMoney = 0; //잃어버린 지오
    public float hp = 0;      //체력
    public float maxHp = 50;  //최대체력
    public float mp = 0;      //영혼
    public float maxMp = 100; //최대영혼
    public int lastDeathLocation = 0;//최근에 죽은 장소
    public bool isShadowAlive = false;//그림자 존재 여부
}

public class LiftData
{
    public bool isLiftUp = true;
}