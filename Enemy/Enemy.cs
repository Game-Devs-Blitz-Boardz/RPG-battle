using UnityEngine;

public class Enemy : Entity
{

    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;

    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake() {
        base.Awake();

        stateMachine = new EnemyStateMachine();
    }

    protected override void Update() {
        base.Update();

        stateMachine.currentState.Update();

    }

    public virtual RaycastHit2D IsPlayerDetected() {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);
    }

}
