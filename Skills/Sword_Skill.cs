using UnityEngine;
using UnityEngine.UI;

public enum SwordType {
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{

    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")]
    [SerializeField] private UI_SkillTreeSlot bounceUnlockButton;
    [SerializeField] private int amountOfBounce;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce info")]
    [SerializeField] private UI_SkillTreeSlot pierceUnlockButton;
    [SerializeField] private int amountOfPierce;
    [SerializeField] private float pierceGravity;

    [Header("Spin info")]
    [SerializeField] private UI_SkillTreeSlot spinUnlockButton;
    [SerializeField] private float hitCooldown = 0.35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;

    [Header("Skill info")]
    [SerializeField] private UI_SkillTreeSlot swordUnlockButton;
    public bool swordUnlocked {get; private set;}
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    [Header("Passive skills")]
    [SerializeField] private UI_SkillTreeSlot timeStopUnlockButton;
    public bool timeStopUnlocked {get; private set;}
    [SerializeField] private UI_SkillTreeSlot vulnerableUnlockButton;
    public bool vulnerableUnlocked {get; private set;}

    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;
    
    private GameObject[] dots;

    protected override void Start() {
        base.Start();

        GenerateDots();
        SetupGravity();

        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounceSword);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierceSword);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpinSword);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnerable);

    }

    private void SetupGravity() {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if (swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }

    protected override void Update() {
        if (Input.GetKeyUp(KeyCode.Mouse1)) {
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }   

        if (Input.GetKey(KeyCode.Mouse1)) {
            // DotsActive(true);
            for (int i = 0; i<dots.Length; i++) {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        } 

    }

    public void CreateSword() {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, Quaternion.identity);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        if (swordType == SwordType.Bounce) {
            newSwordScript.SetupBounce(true, amountOfBounce, bounceSpeed);
        } else if (swordType == SwordType.Pierce) {
            newSwordScript.SetupPierce(amountOfPierce);
        } else if (swordType == SwordType.Spin) {
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);
        }

        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);
    }

#region Unlock region

    private void UnlockTimeStop() {
        if (timeStopUnlockButton.unlocked) {
            timeStopUnlocked = true;
        }
    }

    private void UnlockVulnerable() {
        if (vulnerableUnlockButton.unlocked) {
            vulnerableUnlocked = true;
        }
    }

    private void UnlockSword() {
        if (swordUnlockButton.unlocked) {
            swordType = SwordType.Regular;
            swordUnlocked = true;
        }
    }

    private void UnlockBounceSword() {
        if (bounceUnlockButton.unlocked) {
            swordType = SwordType.Bounce;
        }
    }

    private void UnlockPierceSword() {
        if (pierceUnlockButton.unlocked) {
            swordType = SwordType.Pierce;
        }
    }

    private void UnlockSpinSword() {
        if (spinUnlockButton.unlocked) {
            swordType = SwordType.Spin;
        }
    }

#endregion

#region Aim region

    public Vector2 AimDirection() {
        Vector2 playerPosition =player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool _isActive) {
        for (int i = 0; i<dots.Length; i++) {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots() {
 
        dots = new GameObject[numberOfDots];
        for (int i = 0; i< numberOfDots; i++) {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }

    }

    private Vector2 DotsPosition(float _t) {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y)
            * _t + 0.5f * (Physics2D.gravity * swordGravity) * _t * _t;

        return position;
    }
#endregion

}
