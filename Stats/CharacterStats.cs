using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stat strength; // 1 point increase damage by 1 and crit. power by 1%
    public Stat agility; // 1 point increase evasion by 1% and crit. chance by 1 %
    public Stat intelligence; // 1 point increase magic damage by 1 and magic resistance by 3
    public Stat vitality; // 1 point increase health by 3 or 5 points

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower; //default value 150%

    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armour;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;


    [SerializeField] private int currentHealth;


    protected virtual void Start() {
        critPower.SetDefaultValue(150);

        currentHealth = maxHealth.GetValue();
    }

    public virtual void DoDamage(CharacterStats _targetStats) {

        if(CanAvoidAttack(_targetStats)) return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit()) {
            totalDamage = CalucalateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmour(_targetStats, totalDamage);

        // _targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(_targetStats);
    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats) {

        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicaldamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();

        totalMagicaldamage = CheckTargetResistance(_targetStats, totalMagicaldamage);
        _targetStats.TakeDamage(totalMagicaldamage);

    }

    private static int CheckTargetResistance(CharacterStats _targetStats, int _totalMagicaldamage) {

        _totalMagicaldamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        _totalMagicaldamage = Mathf.Clamp(_totalMagicaldamage, 0, int.MaxValue);
        return _totalMagicaldamage;

    }

    public virtual void ApplyAilments(bool _ignite, bool _chill, bool _shock) {

        if (isIgnited || isChilled || isShocked) return;

        isIgnited = _ignite;
        isChilled = _chill;
        isShocked = _shock;

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

    private bool CanCrit() {

        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance) {
            return true;
        }

        return false;

    }

    private int CalucalateCriticalDamage(int _damage) {

        float totalCriticalPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;

        float criticalDamage = _damage * totalCriticalPower;

        return Mathf.RoundToInt(criticalDamage);

    }
    

}
