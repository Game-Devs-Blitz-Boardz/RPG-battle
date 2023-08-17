using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public PlayerStateMachine stateMachine { get; private set; }
    
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }

    void Awake() {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "idle");
        moveState = new PlayerMoveState(this, stateMachine, "move");
    }

    void Start() {
        stateMachine.Initialize(idleState);
    }

    void Update() {
        stateMachine.currentState.Update();
    }

}
