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

        if (enemy.IsPlayerDetected()) {

            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance) {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }

        } else {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7) {
                stateMachine.ChangeState(enemy.idleState);
            }
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

    bool CanAttack() {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown) {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        Debug.Log("Attack is on cooldown");
        return false;
    }
}
