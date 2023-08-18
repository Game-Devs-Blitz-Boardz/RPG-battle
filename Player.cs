using UnityEngine;

public class Player : MonoBehaviour {

    #region Components
    public Animator anim { get; private set;}
    #endregion

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    #endregion

    void Awake() {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
    }

    void Start() {
        anim = GetComponentInChildren<Animator>();

        stateMachine.Initialize(idleState);
    }

    void Update() {
        stateMachine.currentState.Update();
    }

}
