using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
   
    protected Enemy_Skeleton enemy;

    protected Transform player;

    public SkeletonGroundedState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Enemy_Skeleton _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

    }

    public override void Update() {
        base.Update();

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2) {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
