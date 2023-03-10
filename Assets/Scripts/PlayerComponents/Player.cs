using System;
using System.Collections;
using System.Linq;
using Others;
using States;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerComponents
{
    public class Player : MonoBehaviour
    {
        public Rigidbody playerRb;

        private Vector3 _spawnPoint;
        private Vector3 _deathPos;

        public GameObject pointer;
        public int playerIndex;
        public SkinnedMeshRenderer mRenderer;
        public SpriteRenderer punchSprite;
        public Fist fist;
        public Animator animator;
        public Collider capsule, foot;

        public GameObject chargeParticle,
            deathParticle,
            hitPunchParticle,
            hitPunchGround,
            runParticle,
            launchPunchParticle,
            jumpParticle;
            

        private Vector2 _direction,_lastPunchDirection;
        private GameData _data;
        private StateMachine _stateMachine;
        private Vector3 _initialScale;
        private int _life;
        private string _name;
        private Material _ownMat;
        private MeshRenderer _fistRenderer;
        private MaterialPropertyBlock _materialPropertyBlock;
        private readonly int _intensity = Shader.PropertyToID("_Intensity");
        private readonly int _dissolve = Shader.PropertyToID("_Dissolve");
        private readonly int _attenuation = Shader.PropertyToID("_Attenuation");

        public AudioSource audioSource;
        public AudioClip dieAudio,
            hitAudio,
            chargeAudio,
            jumpAudio,
            finalChargeAudio,
            launchAudio;

        public static readonly UnityEvent<string,int> OnLifeLost = new();

        #region Unity Lifecycle

        private void OnCollisionEnter(Collision collision)
        {
            if (InThisState(StateType.OnLaunchPunchAir))
            {
                EnableFistCollider(false);
            }
            
            if (IsOnGround(collision.gameObject) && !InThisState(StateType.OnChargingPunchAir) &&
                !InThisState(StateType.OnHitStun) && !InThisState(StateType.OnGround) &&
                !InThisState(StateType.OnDeath))
            {
                _stateMachine.ChangeState(StateType.OnGround);
                launchPunchParticle.SetActive(false);
            }

            
            if (collision.gameObject.CompareTag(TagNames.Player)) playerRb.useGravity = true;
        }
        
        private void OnDrawGizmos()
        {
            var position = GetOwnPos();
            var dir = GetDir(GetPointerPos(), position);
            Gizmos.DrawRay(position,dir);
        }
        
        private void FixedUpdate()
        {
            if (!InThisState(StateType.OnHitStun))
            {
                switch (playerRb.velocity.x)
                {
                    case < 0: FlipPlayer(new Vector3(1,1,1),false); break;
                    case > 0: FlipPlayer(new Vector3(1,1,-1),true); break;
                }
            }
            
            if (!_stateMachine.canMove)
                return;

            MovePlayer(_direction);
            
            if(InThisState(StateType.OnGround) && playerRb.velocity.x != 0 && !runParticle.activeSelf) runParticle.SetActive(true);
            if(!InThisState(StateType.OnGround) && runParticle.activeSelf) runParticle.SetActive(false);
            if(InThisState(StateType.OnGround) && runParticle.activeSelf && playerRb.velocity.x == 0) runParticle.SetActive(false);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(TagNames.DeathParticles)) _deathPos = transform.position;

            if (!other.gameObject.CompareTag(TagNames.DeathWall)) return;
            
            OnLifeLost?.Invoke(_name,_life);
            _life++;
            _direction = Vector2.zero;
            playerRb.velocity = _direction;
            SetAnimatorFloat("XPosition",0);
            chargeParticle.SetActive(false);
            PlayOneShot(dieAudio);
            var ps = Instantiate(deathParticle, _deathPos, quaternion.identity);
            Destroy(ps,2);
            _stateMachine.ChangeState(StateType.OnDeath);
            StartCoroutine(StartCooldown(() =>
            {
                if (_life == 3)
                {
                    Destroy(gameObject);
                    return;
                }
                _stateMachine.ExitState();
            }, 1.5f));
        }

        #endregion

        #region Getters/Setters

        private bool InThisState(StateType state) => _stateMachine.GetCurrentState() == state;
        private bool IsInAngle(float angle) => (angle > _data.dotAngle);
        private Vector3 GetOwnPos() => transform.position + new Vector3(0, 0.75f, 0);
        private Vector3 GetPointerPos() => pointer.transform.position;
        private static Vector3 GetDir(Vector3 pointerPos, Vector3 ownPos) => (pointerPos - ownPos).normalized;
        public static bool IsOnGround(GameObject go) => go.CompareTag(TagNames.Ground);
        public void SetVelocity(Vector2 newVelocity) => playerRb.velocity = newVelocity;
        public Vector3 GetVelocity() => playerRb.velocity;
        public void SetForceToFist(float fistForce) => fist.SetForce(fistForce);
    
        public Rigidbody GetRigidBody() => playerRb;
        public GameData GetData() => _data;
        private void PlayOneShot(AudioClip audioClip) => audioSource.PlayOneShot(audioClip);
        
        public void SetInmortal(bool b)
        {
            capsule.enabled = !b;
            foot.enabled = b;
        }

        public void SetPlayerName(string newName) => _name = newName;
        
        public void SetData(GameData data)
        {
            _initialScale = punchSprite.transform.localScale;
            TryGetComponent(out _stateMachine);
            _data = data;
        }
        
        public Vector2 GetLastPunchDirection() => _lastPunchDirection;

        public void EnableFistCollider(bool enable) => fist.GetCollider().enabled = enable;

        public void SetAnimationTrigger(string trigger) => animator.SetTrigger(trigger);
        public void SetAnimationBool(string trigger,bool b) => animator.SetBool(trigger,b);

        private void SetAnimatorFloat(string trigger, float value) => animator.SetFloat(trigger, value);

        public void SetSpawnPoint(Vector3 point) => _spawnPoint = point;
        public void ReturnToSpawn() => transform.position = _spawnPoint;

        #endregion

        #region Utility Methods

        public void ApplyForce()
        {
            if (InThisState(StateType.OnChargingPunchAir) || InThisState(StateType.OnChargingPunchGround))
                _stateMachine.ExitState();
        }
    
        public void StartCharging()
        {
            if(!_stateMachine.canPunch) return;
            if (InThisState(StateType.OnAir) || InThisState(StateType.OnLaunchPunchAir) ||
                InThisState(StateType.OnRecovery))
            {
                _stateMachine.ChangeState(StateType.OnChargingPunchAir);
                chargeParticle.SetActive(true);
            }

            if (InThisState(StateType.OnGround) || InThisState(StateType.OnLaunchPunchGround) ||
                InThisState(StateType.OnRecovery))
            {
                _stateMachine.ChangeState(StateType.OnChargingPunchGround);
                chargeParticle.SetActive(true);
            }

            audioSource.clip = chargeAudio;
            audioSource.loop = true;
            audioSource.Play();
        }

        public void Move(Vector2 direction)
        {
            if (InThisState(StateType.OnChargingPunchAir) || InThisState(StateType.OnChargingPunchGround))
                MovePointer(direction);

            if (InThisState(StateType.OnGround) || InThisState(StateType.OnAir) || InThisState(StateType.OnRecovery))
                _direction = direction;
        }

        private void MovePlayer(Vector2 direction)
        {
            SetAnimatorFloat("XPosition",playerRb.velocity.x);

            playerRb.velocity = new Vector3 (-direction.x * _data.moveMultiplier, playerRb.velocity.y, direction.y * _data.moveMultiplier);
        }

        private void FlipPlayer(Vector3 scale, bool flip)
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

        public void Jump()
        {
            if (!InThisState(StateType.OnGround) && !InThisState(StateType.OnRecovery)) return;
            
            var jumpVfx = Instantiate(jumpParticle, transform.position, Quaternion.identity);
            jumpVfx.transform.localEulerAngles = new Vector3(-90, 0, 0);
            PlayOneShot(jumpAudio);
            Destroy(jumpVfx,1);
            _stateMachine.ChangeState(StateType.OnAir);
        }

        public void Punch(float force,float recoveryDuration)
        {
            PlaySoundPunch();
            
            chargeParticle.SetActive(false);
            launchPunchParticle.SetActive(true);

            var pointerPos = GetPointerPos();
            var ownPos = GetOwnPos();
            var normalizedDir = GetDir(pointerPos,  new Vector3(0, 0.75f, 0) + animator.transform.position);
            var distance = Vector2.Distance(pointerPos, ownPos);
            
            _lastPunchDirection = normalizedDir;

            if (Physics.Raycast(transform.position, normalizedDir, out var hit,distance) && IsOnGround(hit.collider.gameObject))
            {
                if (IsInAngle(Vector3.Dot(-transform.up, normalizedDir)))
                {
                    Instantiate(hitPunchGround, hit.point,quaternion.identity);
                    SetVelocity(Vector3.zero);
                    AddForce(-normalizedDir * _data.bounceForce,ForceMode.Impulse,EndCooldownLaunch);
                    StartRecovery(recoveryDuration);
                    return;
                }
            }

            var angle = 0f;
            if (_direction.x != 0) angle = Mathf.Rad2Deg * Mathf.Atan((normalizedDir.y / normalizedDir.x));

            if (angle is < 35 or > 60 && (!(angle <= -35) || !(angle >= -60)))
            {
                animator.gameObject.transform.eulerAngles = new Vector3(angle, -90, 0);
            }

            AddForce(normalizedDir  *  force,ForceMode.Impulse,EndCooldownLaunch);
            StartCoroutine(StartCooldown(() => SetAnimationBool("Launch", false),recoveryDuration));
            StartRecovery(recoveryDuration);
        }

        private void PlaySoundPunch()
        {
            audioSource.Stop();
            audioSource.loop = false;
            PlayOneShot(finalChargeAudio);
            audioSource.clip = launchAudio;
            audioSource.Play();
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
        
        public void SetMaterial(Material mat,Material fistMat)
        {
            mRenderer.material = mat;
            _ownMat = mat;
            _fistRenderer = fist.GetComponent<MeshRenderer>();
            _fistRenderer.material = fistMat;
            _materialPropertyBlock = new MaterialPropertyBlock();
        }

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
            StartCoroutine(ChangePropertyMaterial(0.5f, 0, 1, _attenuation, _fistRenderer));
            StartCoroutine(ChangePropertyMaterial(0.5f, 0, 1, _attenuation, mRenderer));
            StartCoroutine(StartCooldown(() => StartCoroutine(ChangePropertyMaterial(0.5f, 1, 0, _attenuation, _fistRenderer)),0.5f));
            StartCoroutine(StartCooldown(() => StartCoroutine(ChangePropertyMaterial(0.5f, 1, 0, _attenuation, mRenderer)),0.5f));
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

        public void ResizeSprite()
        {
            punchSprite.transform.localScale = _initialScale;
            var color = punchSprite.color;
            color = new Color(color.r, color.g, color.b, 0);
            punchSprite.color = color;
        }

        public void ApplyPunchForce(Vector3 direction)
        {
            playerRb.useGravity = false;
            
            launchPunchParticle.SetActive(false);
            
            StartCoroutine(ChangePropertyMaterial(0.1f, 0,0.75f, _intensity,mRenderer));
            StartCoroutine(StartCooldown(() => StartCoroutine(ChangePropertyMaterial(0.1f, 0.75f,0, _intensity,mRenderer)),0.1f));
            
            Instantiate(hitPunchParticle, transform.position + new Vector3(0,0.75f,0) , quaternion.identity);
            chargeParticle.SetActive(false);
            PlayOneShot(hitAudio);
            
            if (direction.x < 0) FlipPlayer(new Vector3(1,1,-1), true);
            else FlipPlayer(new Vector3(1, 1, 1), false);
            
            _stateMachine.ChangeState(StateType.OnHitStun);
            StartCoroutine(StartCooldown(() => _stateMachine.ExitState(),1f));
        }
        
        public void RotatePlayer() => animator.gameObject.transform.eulerAngles = new Vector3(0, -90, 0);

        public void HideMesh()
        {
            mRenderer.enabled = false;
            fist.gameObject.SetActive(false);
        }

        public void ShowMesh()
        {
            if (_life == 3) return;
                mRenderer.enabled = true;
            fist.gameObject.SetActive(true);
            StartCoroutine(ChangePropertyMaterial(1.5f, 1,0, _dissolve,mRenderer));
            StartCoroutine(ChangePropertyMaterial(1.5f, 1,0, _dissolve,_fistRenderer));
            //Corutina material
        }

        private IEnumerator ChangePropertyMaterial(float time,float min, float max,int property,Renderer rend)
        {

            var t = 0f;
            var lerp = new Vector2(min, max);

            while (t < 1)
            {
                t += Time.deltaTime / time;

                var constructionAmount = Mathf.Lerp(lerp.x, lerp.y, t);
                _materialPropertyBlock.SetFloat(property, constructionAmount);
                rend.SetPropertyBlock(_materialPropertyBlock, 0);
                
                yield return null;
            }
            _materialPropertyBlock.SetFloat(property, lerp.y);
        }

        public void StartAppearCoroutine()
        {
            StartCoroutine(StartCooldown(() =>
            {
                ShowMesh();
                SetInmortal(true);
                SetVelocity(Vector2.zero);
                _stateMachine.canMove = false;
                _stateMachine.isCharging = false;
                _stateMachine.canJump = false;
                SetAnimationBool("IsCharging",_stateMachine.isCharging);
                SetAnimationTrigger("Dead");
                launchPunchParticle.SetActive(false);
                SetInmortal(false);
            },1.5f) );
        }

        #endregion
    }
}