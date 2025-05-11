using UnityEngine;

public class Howling_Skill : Skill
{
    [SerializeField] protected GameObject howlingPrefab;

    public override void UseSkill()
    {
        base.UseSkill();
    }

    public void UseHowlingSkill()
    {
        player.soundClip.PlayerSoundOneShot(14);

        Vector3 spawnPos = player.transform.position + new Vector3(0, 3f, 0);
        GameObject obj = Instantiate(howlingPrefab, spawnPos, Quaternion.identity);

        player.SetHPandMP(0, -player.playerData.soul_cost);
    }
}
