using UnityEngine;

public class SkeletonBattleState : EnemyState
{

    Transform player;
    Enemy_Skeleton enemy;
    int moveDir;

    public SkeletonBattleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Enemy_Skeleton _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player").transform;

    }

    public override void Update() {
        base.Update();

        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance) {
            Debug.Log("Attack");
            enemy.ZeroVelocity();
            return;
        }

        if (player.position.x > enemy.transform.position.x) {
            moveDir = 1;
        } else {
            moveDir = -1;
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
