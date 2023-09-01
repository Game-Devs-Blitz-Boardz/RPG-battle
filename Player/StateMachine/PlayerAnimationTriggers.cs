using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{

    private Player player => GetComponentInParent<Player>();

    private void AnimTrigger() {
        player.AnimTrigger();
    }

    private void AttackTrigger() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(var hit in colliders) {
            if (hit.GetComponent<Enemy>() != null) {
                hit.GetComponent<CharacterStats>().TakeDamage(player.stats.damage.GetValue());

                Debug.Log(player.stats.damage.GetValue());
            }
        }
    }

    private void ThrowSword() {
        SkillManager.instance.sword.CreateSword();
    }
}
