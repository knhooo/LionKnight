using UnityEngine;

public class ShadowStateMachine
{
    public ShadowState currentState { get; private set; }
    //초기화
    public void Initialize(ShadowState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(ShadowState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
        //Debug.Log(currentState);
    }
}
