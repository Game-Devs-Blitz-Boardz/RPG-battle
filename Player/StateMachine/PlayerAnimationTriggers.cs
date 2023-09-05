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

                EnemyStats _target = hit.GetComponent<EnemyStats>();
                
                player.stats.DoDamage(_target);

                Inventory.instance.GetEquipment(EquipmentType.Weapon).Effect(_target.transform);

                ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                if (weaponData != null) {
                    weaponData.Effect(_target.transform);
                }

            }
        }
    }

    private void ThrowSword() {
        SkillManager.instance.sword.CreateSword();
    }
}
