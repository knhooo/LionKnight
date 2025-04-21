using UnityEngine;

public class BossGrimmStateMachine
{
    public BossGrimmState currentState { get; private set; }

    public void Initalize(BossGrimmState state)
    {
        currentState = state;
        currentState.Enter();
    }

    public void ChangeState(BossGrimmState state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }
}
