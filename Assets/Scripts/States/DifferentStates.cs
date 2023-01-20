using PlayerComponents;
using UnityEngine;

namespace States
{
    public class OnGroundState : State
    {
        public OnGroundState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            StateMachine.canJump = true;
            MoveAndGravity(true);
            
            Player.RotatePlayer();
            
            Player.SetAnimationBool("OnGround",true);
            Player.SetAnimationBool("Recovery",false);

            var velocity = Player.GetVelocity();
            var multiplier = Player.GetData().recoveryMultiplier;
            Player.SetVelocity(new Vector2(velocity.x * multiplier,velocity.y * multiplier));
        }
    }

    public class OnRecoveryState : State
    {
        public OnRecoveryState(StateMachine stateMachine, Player player) : base(stateMachine, player) { }

        public override void DoAction()
        {
            Player.SetAnimationBool("OnGround",false);
            if (Physics.Raycast(Player.transform.position,Vector3.down,out var hit,1f) && Player.IsOnGround(hit.collider.gameObject))
            {
                StateMachine.canJump = true;
                Player.SetAnimationBool("OnGround",true);
            }
            
            Player.RotatePlayer();
            
            Player.SetAnimationBool("Recovery",true);
            Player.SetAnimationBool("Launch",false);
            
            Player.launchPunchParticle.SetActive(false);
            
            Player.EnableFistCollider(false);
            MoveAndGravity(true);
            
            var velocity = Player.GetVelocity();
            var multiplier = Player.GetData().recoveryMultiplier;
            Player.SetVelocity(new Vector2(velocity.x * multiplier,velocity.y * multiplier));
        }
    }
    
    public class OnAir : State
    {
        public OnAir(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            if (!StateMachine.canJump) return;
            
            Player.RotatePlayer();
            
            Player.SetAnimationBool("OnGround",false);
            Player.SetAnimationBool("Recovery",false);
            Player.SetAnimationTrigger("OnAir");
            
            Player.SetForceToFist(0);
            
            MoveAndGravity(true);
            Player.SetVelocity(new Vector2(Player.GetVelocity().x,0));
            Player.AddForce(Vector3.up * Player.GetData().jumpForce, ForceMode.Impulse, () => { });
            StateMachine.canJump = !StateMachine.canJump;
        }
    }
    public class OnChargingPunchGroundState : State
    {
        public OnChargingPunchGroundState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            if (!StateMachine.canPunch) return;
            Player.SetAnimationBool("IsCharging",true);
            Player.SetVelocity(new Vector2(0,-1.5f));
            MoveAndGravity(false);

            StateMachine.isCharging = true;
            Player.StartGrowingSprite();
        
            StateMachine.SetActualTime(Time.time);
        }

        public override void ExitState() => StateMachine.ChangeState(StateType.OnLaunchPunchGround);
    }
    
    public class OnLaunchPunchGroundState : State
    {
        public OnLaunchPunchGroundState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            if (!StateMachine.canPunch) return;

            StateMachine.canJump = false;
            
            Player.SetAnimationBool("IsCharging",false);
            Player.SetAnimationBool("Launch",true);
            
            Player.EnableFistCollider(true);
            
            var duration = Time.time - StateMachine.GetActualTime();
            
            Player.SetForceToFist(Player.GetForceOnTime(duration));
            
            StateMachine.canMove = false;
            PreparePunch();
            Player.Punch(Player.GetForceOnTime(duration), Player.GetRecoveryTime(duration));
            
            StateMachine.isCharging = false;
            Player.ResizeSprite();
        }
    }
    public class HitStunState : State
    {
        public HitStunState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            Player.ResizeSprite();
            
            Player.RotatePlayer();
            StateMachine.canJump = false;
            StateMachine.canMove = false;
            StateMachine.canPunch = false;
            Player.EnableFistCollider(false);
            
            Player.SetAnimationTrigger("Hit");
            Player.SetAnimationBool("IsCharging",false);
        }

        public override void ExitState()
        {
            StateMachine.canPunch = true;
            StateMachine.ChangeState(StateType.OnRecovery);
        }
    }
    
    public class OnChargingPunchAirState : State
    {
        public OnChargingPunchAirState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            if (!StateMachine.canPunch) return;
            
            Player.SetAnimationBool("IsCharging",true);
            
            Player.SetVelocity(new Vector2(0, -1.5f));
            MoveAndGravity(false);
            
            StateMachine.isCharging = true;
            Player.StartGrowingSprite();

            StateMachine.SetActualTime(Time.time);
        }

        public override void ExitState() => StateMachine.ChangeState(StateType.OnLaunchPunchAir);
    }
    
    public class OnLaunchPunchAirState : State
    {
        public OnLaunchPunchAirState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            if (!StateMachine.canPunch) return;
            
            Player.SetAnimationBool("IsCharging",false);
            Player.SetAnimationBool("Launch",true);
            
            StateMachine.canJump = false;
            
            Player.EnableFistCollider(true);
            
            var duration = Time.time - StateMachine.GetActualTime();
            
            Player.SetForceToFist(Player.GetForceOnTime(duration));
            
            StateMachine.canMove = false;
            PreparePunch();
            Player.Punch(Player.GetForceOnTime(duration),Player.GetRecoveryTime(duration));
            
            StateMachine.isCharging = false;
            Player.ResizeSprite();
        }
    }
    
    public class OnDeathState : State
    {
        public OnDeathState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            //Ocultar malla, reposicionar, esperar x segundos
            Player.HideMesh();
            Player.ReturnToSpawn();
            Player.ShowMesh();
            Player.SetInmortal(true);
            Player.SetVelocity(Vector2.zero);
            StateMachine.canMove = false;
            StateMachine.isCharging = false;
            StateMachine.canJump = false;
            Player.SetAnimationBool("IsCharging",StateMachine.isCharging);
            Player.SetAnimationTrigger("Dead");
        }

        public override void ExitState()
        {
            Player.SetInmortal(false);
            StateMachine.canMove = true;
            StateMachine.canJump = true;
            StateMachine.ChangeState(StateType.OnGround);
        }
    }
}