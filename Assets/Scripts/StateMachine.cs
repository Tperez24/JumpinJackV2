using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State initialState;
    public float checkExitRate;

    private State _currentState;

    private void Awake()
    {
        _currentState = initialState;
        _currentState.enabled = true;
        
        InvokeRepeating(nameof(Check), 0, checkExitRate);
    }

    private void OnDestroy() => CancelInvoke(nameof(Check));

    private void Check() => _currentState.CheckExit();

    public void ChangeState(State newState)
    {
        _currentState.enabled = false;
        _currentState = newState;
        _currentState.enabled = true;
    }
}
