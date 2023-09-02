using UnityEngine;

public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    private void AnimTrigger() {
        enemy.AnimFinishTrigger();
    }

    private void AttackTrigger() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach(var hit in colliders) {
            if (hit.GetComponent<Player>() != null) {

                PlayerStats _target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(_target);

            }
        }
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
