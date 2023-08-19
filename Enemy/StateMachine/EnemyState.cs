using UnityEngine;

public class EnemyState
{

    protected EnemyStateMachine stateMachine;
    protected Enemy enemy;

    string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public EnemyState(EnemyStateMachine _stateMachine, Enemy _enemy, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.enemy = _enemy;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter() {
        triggerCalled = false;
        enemy.anim.SetBool(animBoolName, true);
    }

    public virtual void Update() {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit() {
        enemy.anim.SetBool(animBoolName, false);
    }

}
