using UnityEngine;

public class EnemyState
{

    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;

    string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public EnemyState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.enemyBase = _enemyBase;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter() {
        triggerCalled = false;
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Update() {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit() {
        enemyBase.anim.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimBoolName(animBoolName);
    }

    public virtual void AnimFinishTrigger() {
        triggerCalled = true;
    }

}
