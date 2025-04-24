using UnityEngine;

public class Focus_Skill : Skill
{
    public void UseFocusSkill()
    {
        Debug.Log("체력 회복!");
        //체력 회복
        player.mp -= 50;//영혼 감소
        //체력 회복
        if (player.hp + 10 < player.maxHp) player.hp = player.maxHp;
        else player.hp += 10;
        Debug.Log("Hp" + player.hp);
        Debug.Log("Mp" + player.mp);
    }
}
