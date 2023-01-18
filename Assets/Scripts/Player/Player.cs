using System;
using System.Collections;
using System.Linq;
using DefaultNamespace;
using Others;
using States;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody playerRb;
    
    public GameObject pointer;
    public int playerIndex;
    public MeshRenderer mRenderer;
    public TextMeshProUGUI txtMeshPro;

    private Vector2 _direction;
    private GameData _data;
    private StateMachine _stateMachine;
    
    private bool InThisState(StateType state) => _stateMachine.GetCurrentState() == state;
    
    public void ApplyForce()
    {
        if (InThisState(StateType.OnChargingPunchAir) || InThisState(StateType.OnChargingPunchGround))
            _stateMachine.ExitState();
    }
    
    public void StartCharging()
    {
        if(!_stateMachine.canPunch) return;
        if(InThisState(StateType.OnAir) || InThisState(StateType.OnLaunchPunchAir) || InThisState(StateType.OnRecovery)) _stateMachine.ChangeState(StateType.OnChargingPunchAir);
        if(InThisState(StateType.OnGround) || InThisState(StateType.OnLaunchPunchGround) || InThisState(StateType.OnRecovery)) _stateMachine.ChangeState(StateType.OnChargingPunchGround);
    }

    public void Move(Vector2 direction)
    {
        if (InThisState(StateType.OnChargingPunchAir) || InThisState(StateType.OnChargingPunchGround))
        {
            MovePointer(direction);
        }

        if (InThisState(StateType.OnGround) || InThisState(StateType.OnAir) || InThisState(StateType.OnRecovery))
        {
            _direction = direction;
        }
    }

    private void MovePlayer(Vector2 direction)
    {
        var position = transform.position;
        var pos = new Vector2(position.x + (-direction.x * _data.moveMultiplier), position.y);
        playerRb.MovePosition(pos);
    }

    private void MovePointer(Vector2 direction)
    {
        var dir = new Vector3(-direction.x, direction.y, 0);
        var finalDir = transform.position + dir;
        pointer.transform.position = new Vector3(finalDir.x,finalDir.y + 0.75f, finalDir.z);
    }

    public void SetData(GameData data)
    {
        TryGetComponent(out _stateMachine);
        _data = data;
    }

    public void Jump()
    {
        if(InThisState(StateType.OnGround) || InThisState(StateType.OnRecovery)) _stateMachine.ChangeState(StateType.OnAir);
    }

    public void Punch(float force,float recoveryDuration)
    {
        var pointerPos = GetPointerPos();
        var ownPos = GetOwnPos();
        var dir = GetDir(pointerPos, ownPos);
        var distance = Vector2.Distance(pointerPos, ownPos);
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, dir, out hit,distance) && IsOnGround(hit.collider.gameObject))
        {
            if (IsInAngle(Vector3.Dot(-transform.up, dir)))
            {
                SetVelocity(Vector3.zero);
                AddForce(-dir * _data.bounceForce,ForceMode.Impulse,EndCooldownLaunch);
                StartRecovery(recoveryDuration);
                return;
            }
        }
        
        AddForce(dir  *  force,ForceMode.Impulse,EndCooldownLaunch);
        StartRecovery(recoveryDuration);
    }

    private void StartRecovery(float recoveryDuration)
    {
        StartCoroutine(StartCooldown(() =>
        {
            if(InThisState(StateType.OnLaunchPunchAir) || InThisState(StateType.OnLaunchPunchGround))
                _stateMachine.ChangeState(StateType.OnRecovery);
        },recoveryDuration));
    }
    public bool IsInAngle(float angle) => (angle > _data.dotAngle);
    public Vector3 GetOwnPos() => transform.position + new Vector3(0, 0.75f, 0);
    public Vector3 GetPointerPos() => pointer.transform.position;
    public Vector3 GetDir(Vector3 pointerPos, Vector3 ownPos) => (pointerPos - ownPos).normalized;

    private void OnCollisionEnter(Collision collision)
    {
        //TODO on ground aqui
        if (IsOnGround(collision.gameObject) && !InThisState(StateType.OnChargingPunchAir)) _stateMachine.ChangeState(StateType.OnGround);
        if (collision.gameObject.CompareTag(TagNames.Player)) playerRb.useGravity = true;
    }

    public bool IsOnGround(GameObject go) => go.CompareTag(TagNames.Ground);

    private void OnDrawGizmos()
    {
        var position = GetOwnPos();
        var dir = GetDir(GetPointerPos(), position);
        Gizmos.DrawRay(position,dir);
    }

    public void SetMaterial(Material mat) => mRenderer.material = mat;
    
    private void Update()
    {
        txtMeshPro.text = _stateMachine.GetCurrentState().ToString();

        if (!_stateMachine.canMove)
            return;

        MovePlayer(_direction);
    }

    public void SetVelocity(Vector2 newVelocity) => playerRb.velocity = newVelocity;
    
    public void AddForce(Vector2 dir,ForceMode mode,Action action)
    {
        playerRb.AddForce(dir,mode);
        StartCoroutine(StartCooldown(action,_data.punchCooldown));
    }

    public static IEnumerator StartCooldown(Action onCooldownEnd,float time)
    {
        yield return new WaitForSecondsRealtime(time);
        onCooldownEnd?.Invoke();
    }

    private void EndCooldownLaunch()
    {
        _stateMachine.canPunch = true;
    }

    public float GetForceOnTime(float duration)
    {
        foreach (var forceDictionary in _data.forces.Where(forceDictionary => duration <= forceDictionary.time))
            return forceDictionary.forces;

        return _data.forces.Last().forces;
    }
    
    public float GetRecoveryTime(float duration)
    {
        foreach (var forceDictionary in _data.forces.Where(forceDictionary => duration <= forceDictionary.time))
            return forceDictionary.recoveryTime;

        return _data.forces.Last().recoveryTime;
    }
    
    public Rigidbody GetRigidBody() => playerRb;
    public GameData GetData() => _data;
}