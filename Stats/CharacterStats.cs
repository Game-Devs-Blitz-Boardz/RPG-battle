using UnityEngine;
using System.Collections;

public enum StatType {
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armour,
    evasion,
    magicRes,
    fireDamage,
    iceDamage,
    lightningDamage,
}


public class CharacterStats : MonoBehaviour
{

    private EntityFX fx;

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

    [SerializeField] private float ailmentsDuration = 4f;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    [SerializeField] private GameObject shockStrikePrefab;
    private int igniteDamage;
    private int shockDamage;
    public int currentHealth;

    public System.Action onHealthChanged;
    public bool isDead {get; private set;}
    private bool isVulnerable;


    protected virtual void Start() {
        fx = GetComponent<EntityFX>();

        critPower.SetDefaultValue(150);
        currentHealth = GetMaxhealthValue();
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

        if (isIgnited)
            ApplyIgniteDamage();

    }

    public void MakeVulnerableFor(float _duration) {
        StartCoroutine(VulnerableCoroutine(_duration));
    }

    private IEnumerator VulnerableCoroutine(float _duration) {
        isVulnerable = true;
        yield return new WaitForSeconds(_duration);
        isVulnerable = false;
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify) {

        StartCoroutine(StartModCoroutine(_modifier, _duration, _statToModify));

    }

    private IEnumerator StartModCoroutine(int _modifier, float _duration, Stat _statToModify) {

        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);

    }

    public virtual void DoDamage(CharacterStats _targetStats) {

        if(CanAvoidAttack(_targetStats)) return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit()) {
            totalDamage = CalucalateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmour(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(_targetStats); // remove if you down want to apply magic hit on primary attack
    }

#region Magical Damage and ailements

    private void ApplyIgniteDamage() {
        if (igniteDamageTimer < 0) {

            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0 && !isDead) {
                Die();
            }
            
            igniteDamageTimer = igniteDamageCooldown;
        }
    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats) {

        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicaldamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();

        totalMagicaldamage = CheckTargetResistance(_targetStats, totalMagicaldamage);
        _targetStats.TakeDamage(totalMagicaldamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0) return;


        AttempToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightningDamage);

    }

    private void AttempToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage) {

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

        if (canApplyShock) _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightningDamage * 0.1f));

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public virtual void ApplyAilments(bool _ignite, bool _chill, bool _shock) {

        bool canApplyIgnite = !isIgnited && !isShocked && !isChilled;
        bool canApplyChill = !isChilled && !isShocked && !isIgnited;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite) {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;

            fx.IgniteFXFor(ailmentsDuration);
        }

        if (_chill && canApplyChill) {
            isChilled = _chill;
            chilledTimer = ailmentsDuration;

            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);

            fx.ChillFXFor(ailmentsDuration);
        }

        if (_shock && canApplyShock) {

            if (!isShocked) {
                ApplyShock(_shock);
            } else {

                if (GetComponent<Player>() != null) return;

                HitNearestTargetWithShockStrike();

            }


        }

    }

    public void ApplyShock(bool _shock) {

        if (isShocked) return;
        isShocked = _shock;
        shockedTimer = ailmentsDuration;

        fx.ShockFXFor(ailmentsDuration);
    }

    public void HitNearestTargetWithShockStrike() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach(var hit in colliders) {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) < 1) {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance) {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
                closestEnemy = transform;
        }

        if (closestEnemy != null) {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ThunderStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    public void SetupIgniteDamage(int _damage) {
        igniteDamage = _damage;
    }

    public void SetupShockStrikeDamage(int _damage) {
        shockDamage = _damage;
    }
#endregion

    public virtual void TakeDamage(int _damage) {
        DecreaseHealthBy(_damage);

        GetComponent<Entity>().Damage();
        fx.StartCoroutine("FlashFX");

        if (currentHealth < 0 && !isDead) {
            Die();
        }
    }

    protected virtual void DecreaseHealthBy(int _damage) {

        if (isVulnerable) {
            _damage = Mathf.RoundToInt(_damage * 1.1f);
        }

        currentHealth -= _damage;

        if (onHealthChanged != null) {
            onHealthChanged();
        }
    }

    public virtual void IncreaseHealthBy(int _amount) {
        currentHealth += _amount;

        if (currentHealth > GetMaxhealthValue()) {
            currentHealth = GetMaxhealthValue();
        }

        if (onHealthChanged != null) {
            onHealthChanged();
        }
    }

    protected virtual void Die() {
        isDead = true;
    }

#region Stat Calculations
    private int CheckTargetArmour(CharacterStats _targetStats, int _totalDamage) {

        if (_targetStats.isChilled) {
            _totalDamage -= Mathf.RoundToInt(_targetStats.armour.GetValue() * 0.8f);
        } else {
            _totalDamage -= _targetStats.armour.GetValue();
        }

        _totalDamage = Mathf.Clamp(_totalDamage, 0, int.MaxValue);
        return _totalDamage;
    }

    private int CheckTargetResistance(CharacterStats _targetStats, int _totalMagicaldamage) {

        _totalMagicaldamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        _totalMagicaldamage = Mathf.Clamp(_totalMagicaldamage, 0, int.MaxValue);
        return _totalMagicaldamage;

    }

    public virtual void OnEvasion() {

    }

    private bool CanAvoidAttack(CharacterStats _targetStats) {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked) totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion) {
            _targetStats.OnEvasion();
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

    public int GetMaxhealthValue() {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
#endregion

    public Stat GetStat(StatType _statType) {

        if (_statType == StatType.strength) return strength;
        else if (_statType == StatType.agility) return agility;
        else if (_statType == StatType.intelligence) return intelligence;
        else if (_statType == StatType.vitality) return vitality;
        else if (_statType == StatType.damage) return damage;
        else if (_statType == StatType.critChance) return critChance;
        else if (_statType == StatType.critPower) return critPower;
        else if (_statType == StatType.health) return maxHealth;
        else if (_statType == StatType.armour) return armour;
        else if (_statType == StatType.evasion) return evasion;
        else if (_statType == StatType.magicRes) return magicResistance;
        else if (_statType == StatType.fireDamage) return fireDamage;
        else if (_statType == StatType.iceDamage) return iceDamage;
        else if (_statType == StatType.lightningDamage) return lightningDamage;

        return null;

    }

}
