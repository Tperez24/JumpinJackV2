using System;
using UnityEngine;

namespace States
{
    public class StateMachine : MonoBehaviour
    {
        public StateType initialState;

        private State _currentState;
        private Player _player;
        private StateType _currentType;
        private float _actualTime;
        
        public bool canPunch = true, canJump = true, canMove = true;

        private void Awake()
        {
            TryGetComponent(out _player);
            _currentState = GetState(initialState);
            //_currentState.enabled = true;
        }

        public void ChangeState(StateType state)
        {
            //_currentState.enabled = false;
            
            var newState = GetState(state);
            
            if(_currentState == newState) return;

            _currentType = state;
            _currentState = newState;
            _currentState.DoAction();
            //_currentState.enabled = true;
        }

        public void ExitState() => _currentState.ExitState();

        public StateType GetCurrentState() => _currentType;

        public float GetActualTime() => _actualTime;

        public void SetActualTime(float time) => _actualTime = time;

        private State GetState(StateType state)
        {
            return state switch
            {
                StateType.OnAir => new OnAir(this,_player),
                StateType.OnGround => new OnGroundState(this,_player),
                StateType.OnHitStun => new HitStunState(this,_player),
                StateType.OnLaunchPunchAir => new OnLaunchPunchAirState(this,_player),
                StateType.OnChargingPunchAir => new OnChargingPunchAirState(this,_player),
                StateType.OnChargingPunchGround => new OnChargingPunchGroundState(this,_player),
                StateType.OnLaunchPunchGround => new OnLaunchPunchGroundState(this,_player),
                StateType.OnDeath => new OnDeathState(this,_player),
                StateType.OnRecovery => new OnRecoveryState(this,_player),
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
        OnHitStun,
        OnDeath,
        OnLaunchPunchAir,
        OnLaunchPunchGround,
        OnRecovery
    }
}