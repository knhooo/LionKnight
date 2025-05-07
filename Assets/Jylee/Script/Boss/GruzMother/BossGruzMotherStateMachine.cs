using UnityEngine;

public class BossGruzMotherStateMachine
{
    public BossGruzMotherState currentState { get; private set; }

    public void Initalize(BossGruzMotherState state)
    {
        currentState = state;
        currentState.Enter();
    }

    public void ChangeState(BossGruzMotherState state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }
}
