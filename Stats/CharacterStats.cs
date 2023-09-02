using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stat strength; // 1 point increase damage by 1 and crit. power by 1%
    public Stat agility; // 1 point increase evasion by 1% and crit. chance by 1 %
    public Stat intelligence; // 1 point increase magic damage by 1 and magic resistance by 3
    public Stat vitality; // 1 point increase health by 3 or 5 points

    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armour;
    public Stat evasion;

    public Stat damage;

    [SerializeField] private int currentHealth;


    protected virtual void Start() {
        currentHealth = maxHealth.GetValue();
    }

    public virtual void DoDamage(CharacterStats _targetStats) {

        if(CanAvoidAttack(_targetStats)) return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        totalDamage = CheckTargetArmour(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _damage) {
        currentHealth -= _damage;

        Debug.Log(_damage);

        if (currentHealth < 0) {
            Die();
        }
    }

    protected virtual void Die() {

    }

    private int CheckTargetArmour(CharacterStats _targetStats, int _totalDamage) {
        _totalDamage -= _targetStats.armour.GetValue();
        _totalDamage = Mathf.Clamp(_totalDamage, 0, int.MaxValue);
        return _totalDamage;
    }

    private bool CanAvoidAttack(CharacterStats _targetStats) {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion) {
            Debug.Log("attack avoided");
            return true;
        }

        return false;
    }

}
