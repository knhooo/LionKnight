using UnityEngine;

public class Focus_Skill : Skill
{
    public void UseFocusSkill()
    {
        Debug.Log("체력 회복!");
        //체력 회복, 마나 소모
        player.SetHPandMP(10,-player.playerData.soul_cost);
        player.soundClip.PlayerSoundOneShot(2);
    }
}
