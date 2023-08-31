using UnityEngine;
using System.Collections.Generic;

public class Sword_Skill_Controller : MonoBehaviour
{


    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    private float freezeTimeDuration;
    private float returnSpeed = 12f;

    [Header("Pierce info")]
    private int amountOfPierce;

    [Header("Bounce info")]
    private float bounceSpeed= 20f;
    private bool isBouncing;
    private int amountOfBounce;
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;

    private float spinDirection;


    private void Awake() {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void Update() {
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning) {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1) {
                player.ClearTheSword();
            }
        }
        
        BounceLogic();
        SpinLogic();

    }

    private void StopWhenSpinning() {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic() {

        if (isBouncing && enemyTarget.Count > 0) {

            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f) {

                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                amountOfBounce--;

                if (amountOfBounce <= 0) {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count) {
                    targetIndex = 0;
                }
            }

        }
    }

    private void SpinLogic() {

        if (isSpinning) {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped) {
                StopWhenSpinning();
            }

            if (wasStopped) {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0) {
                    isSpinning = false;
                    isReturning = true;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0) {
                    hitTimer = hitCooldown;
                }

                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                foreach(var hit in colliders) {
                    if (hit.GetComponent<Enemy>() != null) {
                        SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (isReturning) return;

        if (other.GetComponent<Enemy>() != null) {

            Enemy enemy = other.GetComponent<Enemy>();
            SwordSkillDamage(enemy);

        }

        SetupTargetForBounce(other);

        StuckInto(other);

    }

    private void SwordSkillDamage(Enemy enemy) {
        enemy.Damage();
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
    }

    private void SetupTargetForBounce(Collider2D other) {
        if (other.GetComponent<Enemy>() != null) {

            if (isBouncing && enemyTarget.Count <= 0) {

                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach(var hit in colliders) {
                    if (hit.GetComponent<Enemy>() != null) {
                        enemyTarget.Add(hit.transform);
                    }
                }

            }

        }
    }

    public void SetupSword(Vector2 _dir, float gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed) {

        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;

        rb.velocity = _dir;
        rb.gravityScale = gravityScale;

        if (amountOfPierce <= 0) 
            anim.SetBool("Rotation", true);

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 7f);

    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounces, float _bounceSpeed) {
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounces;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int _amountOfPierce) {
        amountOfPierce = _amountOfPierce;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown) {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    public void ReturnSword() {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
    }

    private void StuckInto(Collider2D other) {

        if (amountOfPierce > 0 && other.GetComponent<Enemy>() != null) {
            amountOfPierce--;
            return;
        }

        if (isSpinning) {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0) return;

        anim.SetBool("Rotation", false);
        transform.parent = other.transform;
    }

    public void DestroyMe() {
        Destroy(gameObject);
    }

}
