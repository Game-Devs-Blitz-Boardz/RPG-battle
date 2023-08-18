using UnityEngine;

public class PlayerState : MonoBehaviour
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    string animBoolName;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        Debug.Log("In " + animBoolName);
        player.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        Debug.Log("Updated " + animBoolName);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
}
