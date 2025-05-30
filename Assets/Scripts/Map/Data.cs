
// 저장
// - 플레이어
// - 상점
// - 사슴정거장 Lift


using UnityEngine;

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

    public float soul_amount = 10;//타격 시 획득 영혼 수치
    public int spell_Damage = 10;//주문 공격 피해량
    public float dash_coolTime = 0.6f;//대시 쿨타임
    public int attack_Damage = 10;//기본 공격 피해량
    public float soul_cost = 33;//영혼 소모량
}

public class LiftData
{
    public bool isLiftUp = true;
}

public class ShopData
{
    public bool[] isSolds = new bool[5];
}

public class BossDeadData
{
    public bool isDead = false;
}