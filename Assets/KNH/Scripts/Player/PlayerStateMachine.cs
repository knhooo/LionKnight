using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }

    //초기화
    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
        Debug.Log(currentState);
    }
}
