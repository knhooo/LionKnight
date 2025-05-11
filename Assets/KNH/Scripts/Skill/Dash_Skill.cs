using UnityEngine;

public class Dash_Skill : Skill
{
    private void Start()
    {
        cooldown = PlayerManager.instance.player.playerData.dash_coolTime;
    }

    public override void UseSkill()
    {
        base.UseSkill();
    }
}
