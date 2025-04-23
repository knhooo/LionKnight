using UnityEngine;

public class Geo : AddForceObject
{
    protected override void OnEnable()
    {
        rb.linearVelocity = Vector2.zero;
        base.OnEnable();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            // GameManager.instance.player.money++;
            gameObject.SetActive(false);
        }
    }
}
