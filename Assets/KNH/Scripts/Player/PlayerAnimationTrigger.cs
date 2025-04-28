using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private SceneFader sceneFader => FindAnyObjectByType<SceneFader>();

    [SerializeField] private GameObject head;
    [SerializeField] private GameObject ghost;

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);


        //foreach (var hit in colliders)
        //{
        //    if (hit.GetComponent<Enemy>() != null)
        //        hit.GetComponent<Enemy>().Damage();
        //}
    }

    private void FocusTrigger()
    {
        SkillManager.instance.focus.UseFocusSkill();
    }

    private void DieAnimation()
    {
        Debug.Log(player.transform.position);
        GameObject obj1 = Instantiate(head, player.headPos.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Rigidbody2D rb = obj1.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(-2f, 3f), ForceMode2D.Impulse);
        GameObject obj2 = Instantiate(ghost, player.headPos.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
    }
    private void ReSpawn()
    {
        sceneFader.FadeToScene("Dirtmouth");
    }
}
