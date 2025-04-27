using UnityEngine;

public class Spirit_Skill : Skill
{
    [SerializeField] private GameObject spiritPrefab;
    [SerializeField] private float spiritSpeed = 20f;
    public override void UseSkill()
    {
        base.UseSkill();
        UseSpiritSkill();
    }

    public void UseSpiritSkill()
    {
        Vector3 spawnPos = player.transform.position + new Vector3(0, 0.5f, 0);
        GameObject obj = Instantiate(spiritPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(player.facingDir * spiritSpeed, 0f);

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.flipX = player.facingDir == -1;

        Debug.Log("Hp: " + player.hp + " MP: " + player.mp);
    }
}
