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
                hit.GetComponent<Player>().Damage();
            }
        }
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
