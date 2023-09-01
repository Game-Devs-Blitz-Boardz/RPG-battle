using UnityEngine;

public class Blackhole_Skill : Skill
{

    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    Blackhole_Skill_Controller currentBlackhole;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    protected override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);

        currentBlackhole = newBlackhole.GetComponent<Blackhole_Skill_Controller>();

        currentBlackhole.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown, blackholeDuration);
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();
    }

    public bool BlackHoleFinished() {

        if (!currentBlackhole) return false;

        if (currentBlackhole.playerCanExitState) {
            currentBlackhole = null;
            return true;
        } 

        return false;
        
    }

    public float GetBlackholeRadius() {
        return maxSize / 2;
    }

}
