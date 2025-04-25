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
        Instantiate(head, transform.position, Quaternion.identity);
        Instantiate(ghost, transform.position, Quaternion.identity);
    }
    private void ReSpawn()
    {
        sceneFader.FadeToScene("Dirtmouth");
    }
}
