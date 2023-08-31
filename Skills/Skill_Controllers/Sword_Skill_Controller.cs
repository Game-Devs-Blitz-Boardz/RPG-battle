using UnityEngine;
using System.Collections.Generic;

public class Sword_Skill_Controller : MonoBehaviour
{

    [SerializeField] private float returnSpeed = 12f;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    public float bounceSpeed= 20f;
    public bool isBouncing = true;
    public int amountOfBounce = 4;
    public List<Transform> enemyTarget;
    private int targetIndex;

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

        if (isBouncing && enemyTarget.Count > 0) {

            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f) {
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

    public void SetupSword(Vector2 _dir, float gravityScale, Player _player) {

        player = _player;

        rb.velocity = _dir;
        rb.gravityScale = gravityScale;

        anim.SetBool("Rotation", true);
    }

    public void ReturnSword() {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (isReturning) return;

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

        StuckInto(other);

    }

    private void StuckInto(Collider2D other) {

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0) return;

        anim.SetBool("Rotation", false);
        transform.parent = other.transform;
    }

}
