public class SkeletonIdleState : EnemyState
{

    Enemy_Skeleton enemy;

    public SkeletonIdleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Enemy_Skeleton _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update() {
        base.Update();

        if (stateTimer < 0) {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
   
}
