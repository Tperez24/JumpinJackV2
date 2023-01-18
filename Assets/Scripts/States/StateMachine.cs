using System;
using UnityEngine;

namespace States
{
    public class StateMachine : MonoBehaviour
    {
        public State initialState;
        public float checkExitRate;

        private State _currentState;

        private void Awake()
        {
            _currentState = initialState;
            //_currentState.enabled = true;
        
            InvokeRepeating(nameof(Check), 0, checkExitRate);
        }

        private void OnDestroy() => CancelInvoke(nameof(Check));

        private void Check() => _currentState.CheckExit();

        public void ChangeState(State newState)
        {
            //_currentState.enabled = false;
            _currentState = newState;
            //_currentState.enabled = true;
        }

        private State GetState(StateType state)
        {
            return state switch
            {
                StateType.OnAir => new OnAir(this),
                StateType.OnGround => new OnGroundState(this),
                StateType.OnHitStun => new HitStunState(this),
                StateType.OnChargingPunchAir => new OnChargingPunchAirState(this),
                StateType.OnChargingPunchGround => new OnChargingPunchGroundState(this),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }

    public enum StateType
    {
        OnGround,
        OnAir,
        OnChargingPunchGround,
        OnChargingPunchAir,
        OnHitStun
    }
}