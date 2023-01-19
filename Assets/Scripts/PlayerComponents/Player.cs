using System;
using System.Collections;
using System.Linq;
using DefaultNamespace;
using Others;
using States;
using TMPro;
using UnityEngine;

namespace PlayerComponents
{
    public class Player : MonoBehaviour
    {
        public Rigidbody playerRb;
    
        public GameObject pointer;
        public int playerIndex;
        public SkinnedMeshRenderer mRenderer;
        public SpriteRenderer punchSprite;
        public TextMeshProUGUI txtMeshPro;
        public Fist fist;
        public Animator animator;

        private Vector2 _direction,_lastPunchDirection;
        private GameData _data;
        private StateMachine _stateMachine;
        private Vector3 _initialScale;
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
            SetAnimatorFloat("XPosition",playerRb.velocity.x);

            /*if (direction.x < 0) RotatePlayer(new Vector3(1,1,-1), true);
            else RotatePlayer(new Vector3(1, 1, 1), false);*/

            playerRb.velocity = new Vector3 (-direction.x * _data.moveMultiplier, playerRb.velocity.y, direction.y * _data.moveMultiplier);
        }

        private void RotatePlayer(Vector3 scale, bool flip)
        {
            animator.gameObject.transform.localScale = scale;
            punchSprite.flipY = flip;
        }

        private void MovePointer(Vector2 direction)
        {
            var dir = new Vector3(-direction.x, direction.y, 0);
            var position = transform.position;
            var finalDir = position + dir * 1.5f;
            pointer.transform.position = new Vector3(finalDir.x,finalDir.y + 0.75f, finalDir.z);
            pointer.transform.LookAt(transform);
            var color = punchSprite.color;
            punchSprite.color = new Color(color.r, color.g, color.b,
                Vector2.Distance(new Vector2(position.x,position.y + 0.75f), pointer.transform.position));
        }

        public void SetData(GameData data)
        {
            _initialScale = punchSprite.transform.localScale;
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

            /*if (dir.x > 0) RotatePlayer(new Vector3(1,1,-1), true);
            else RotatePlayer(new Vector3(1, 1, 1), false);*/
            
            _lastPunchDirection = dir;
            
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
            StartCoroutine(StartCooldown(() => SetAnimationBool("Launch", false),recoveryDuration));
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

        public void StartGrowingSprite()
        {
            StartCoroutine(RepeatLerp(punchSprite.transform.localScale, _data.punchScale,
                _data.forces.Last().time));
        }

        private IEnumerator RepeatLerp(Vector3 scaleFrom,Vector3 scaleTo,float time)
        {
            var i = 0f;
            var rate = (1 / time) * 2f;
            do
            {
                i += Time.deltaTime * rate;
                punchSprite.transform.localScale = Vector3.Lerp(scaleFrom, scaleTo, i);
                yield return null;
            
            } while (i < _data.forces.Last().time && _stateMachine.isCharging);
        }

        private bool IsInAngle(float angle) => (angle > _data.dotAngle);
        private Vector3 GetOwnPos() => transform.position + new Vector3(0, 0.75f, 0);
        private Vector3 GetPointerPos() => pointer.transform.position;
        private static Vector3 GetDir(Vector3 pointerPos, Vector3 ownPos) => (pointerPos - ownPos).normalized;

        private void OnCollisionEnter(Collision collision)
        {
            //TODO on ground aqui
            if (IsOnGround(collision.gameObject) && !InThisState(StateType.OnChargingPunchAir) && !InThisState(StateType.OnHitStun) && (!InThisState(StateType.OnGround))) _stateMachine.ChangeState(StateType.OnGround);
            if (collision.gameObject.CompareTag(TagNames.Player)) playerRb.useGravity = true;
        }

        public static bool IsOnGround(GameObject go) => go.CompareTag(TagNames.Ground);

        private void OnDrawGizmos()
        {
            var position = GetOwnPos();
            var dir = GetDir(GetPointerPos(), position);
            Gizmos.DrawRay(position,dir);
        }

        public void SetMaterial(Material mat) => mRenderer.material = mat;
    
        private void FixedUpdate()
        {
            txtMeshPro.text = _stateMachine.GetCurrentState().ToString();

            if (!InThisState(StateType.OnHitStun))
            {
                if(playerRb.velocity.x < 0) RotatePlayer(new Vector3(1,1,1),false);
                else if(playerRb.velocity.x > 0)
                {
                    RotatePlayer(new Vector3(1,1,-1),true);
                }
            }
            
            
            if (!_stateMachine.canMove)
                return;

            MovePlayer(_direction);
        }

        public void SetVelocity(Vector2 newVelocity) => playerRb.velocity = newVelocity;
        public Vector3 GetVelocity() => playerRb.velocity;
    
        public void AddForce(Vector2 dir,ForceMode mode,Action action)
        {
            playerRb.AddForce(dir,mode);
            StartCoroutine(StartCooldown(action,_data.punchCooldown));
        }

        private static IEnumerator StartCooldown(Action onCooldownEnd,float time)
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

        public void SetForceToFist(float fistForce) => fist.SetForce(fistForce);
    
        public Rigidbody GetRigidBody() => playerRb;
        public GameData GetData() => _data;

        public void ResizeSprite()
        {
            punchSprite.transform.localScale = _initialScale;
            var color = punchSprite.color;
            color = new Color(color.r, color.g, color.b, 0);
            punchSprite.color = color;
        }

        public void ApplyPunchForce(float force, Vector3 direction)
        {
            Debug.Log("Me golpiaste" + force + direction);
            
            if (direction.x < 0) RotatePlayer(new Vector3(1,1,-1), true);
            else RotatePlayer(new Vector3(1, 1, 1), false);
            
            _stateMachine.ChangeState(StateType.OnHitStun);
            StartCoroutine(StartCooldown(() => _stateMachine.ExitState(),1f));
        }
        
        public Vector2 GetLastPunchDirection() => _lastPunchDirection;

        public void EnableFistCollider(bool enable) => fist.GetCollider().enabled = enable;

        public void SetAnimationTrigger(string trigger) => animator.SetTrigger(trigger);
        public void SetAnimationBool(string trigger,bool b) => animator.SetBool(trigger,b);

        private void SetAnimatorFloat(string trigger, float value) => animator.SetFloat(trigger, value);
    }
}