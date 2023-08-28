using UnityEngine;

public class SkeletonAttackState : EnemyState
{

    Enemy_Skeleton enemy;

    public SkeletonAttackState (EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Enemy_Skeleton _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemy.ZeroVelocity();

        if (triggerCalled) {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

}
