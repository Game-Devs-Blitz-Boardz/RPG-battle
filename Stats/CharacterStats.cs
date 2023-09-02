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

    public bool isIgnited; // does damage over time
    public bool isChilled; // decrease enemy armour by 20%
    public bool isShocked; // miss 20% of attacks

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;


    [SerializeField] private int currentHealth;


    protected virtual void Start() {
        critPower.SetDefaultValue(150);

        currentHealth = maxHealth.GetValue();
    }

    protected virtual void Update() {

        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0) {
            isIgnited = false;
        }

        if (chilledTimer < 0) {
            isChilled = false;
        }

        if (shockedTimer < 0) {
            isShocked= false;
        }

        if (igniteDamageTimer < 0 && isIgnited) {
            Debug.Log("Ignite Damage: " + igniteDamage);

            currentHealth -= igniteDamage;

            if (currentHealth < 0) {
                Die();
            }
            
            igniteDamageTimer = igniteDamageCooldown;
        }

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

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0) return;

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while(!canApplyChill && !canApplyIgnite && !canApplyShock) {
            
            if (Random.value < .3f && _fireDamage > 0) {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            
            if (Random.value < .5f && _iceDamage > 0) {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && _lightningDamage > 0) {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

        }

        if (canApplyIgnite) _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);

    }

    private static int CheckTargetResistance(CharacterStats _targetStats, int _totalMagicaldamage) {

        _totalMagicaldamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        _totalMagicaldamage = Mathf.Clamp(_totalMagicaldamage, 0, int.MaxValue);
        return _totalMagicaldamage;

    }

    public virtual void ApplyAilments(bool _ignite, bool _chill, bool _shock) {

        if (isIgnited || isChilled || isShocked) return;

        if (_ignite) {
            isIgnited = _ignite;
            ignitedTimer = 2f;
        }

        if (_chill) {
            isChilled = _chill;
            chilledTimer = 2f;
        }

        if (_shock) {
            isShocked = _shock;
            shockedTimer = 2f;
        }

        isChilled = _chill;
        isShocked = _shock;

    }

    public void SetupIgniteDamage(int _damage) {
        igniteDamage = _damage;
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

        if (_targetStats.isChilled) {
            _totalDamage -= Mathf.RoundToInt(_targetStats.armour.GetValue() * 0.8f);
        } else {
            _totalDamage -= _targetStats.armour.GetValue();
        }

        _totalDamage = Mathf.Clamp(_totalDamage, 0, int.MaxValue);
        return _totalDamage;
    }

    private bool CanAvoidAttack(CharacterStats _targetStats) {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked) totalEvasion += 20;

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
