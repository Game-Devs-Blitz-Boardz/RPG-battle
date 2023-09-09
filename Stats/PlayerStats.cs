using UnityEngine;

public class PlayerStats : CharacterStats
{

    private Player player;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

    }

    protected override void Die()
    {
        base.Die();
        player.Die();

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        ItemData_Equipment currentArmour = Inventory.instance.GetEquipment(EquipmentType.Armour);

        if (currentArmour != null) {
            currentArmour.Effect(player.transform);
        }

    }

    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDoDamage(CharacterStats _targetStats, float _attackMultiplier) {

        if(CanAvoidAttack(_targetStats)) return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (_attackMultiplier > 0) {
            totalDamage = Mathf.RoundToInt(_attackMultiplier * totalDamage);
        } else {
            _targetStats.TakeDamage(damage.GetValue());
        }

        if (CanCrit()) {
            totalDamage = CalucalateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmour(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(_targetStats); // remove if you down want to apply magic hit on primary attack

    }   

}
