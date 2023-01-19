using PlayerComponents;
using UnityEngine;

namespace States
{
    public class OnGroundState : State
    {
        public OnGroundState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            stateMachine.canJump = true;
            MoveAndGravity(true);
            
            player.RotatePlayer();
            
            player.SetAnimationBool("OnGround",true);
            player.SetAnimationBool("Recovery",false);

            var velocity = player.GetVelocity();
            var multiplier = player.GetData().recoveryMultiplier;
            player.SetVelocity(new Vector2(velocity.x * multiplier,velocity.y * multiplier));
        }

        public override void ExitState()
        {
            
        }
    }

    public class OnRecoveryState : State
    {
        public OnRecoveryState(StateMachine stateMachine, Player player) : base(stateMachine, player) { }

        public override void DoAction()
        { 
            RaycastHit hit;
            player.SetAnimationBool("OnGround",false);
            if (Physics.Raycast(player.transform.position,Vector3.down,out hit,1f) && Player.IsOnGround(hit.collider.gameObject))
            {
                stateMachine.canJump = true;
                player.SetAnimationBool("OnGround",true);
            }
            
            player.RotatePlayer();
            
            player.SetAnimationBool("Recovery",true);
            player.SetAnimationBool("Launch",false);
            
            
            player.EnableFistCollider(false);
            MoveAndGravity(true);
            
            var velocity = player.GetVelocity();
            var multiplier = player.GetData().recoveryMultiplier;
            player.SetVelocity(new Vector2(velocity.x * multiplier,velocity.y * multiplier));
        }
    }
    
    public class OnAir : State
    {
        public OnAir(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            //Movimiento leve
            if (!stateMachine.canJump) return;
            
            player.RotatePlayer();
            
            player.SetAnimationBool("OnGround",false);
            player.SetAnimationBool("Recovery",false);
            player.SetAnimationTrigger("OnAir");
            
            player.SetForceToFist(0);
            
            MoveAndGravity(true);
            player.SetVelocity(new Vector2(player.GetVelocity().x,0));
            player.AddForce(Vector3.up * player.GetData().jumpForce, ForceMode.Impulse, () => { });
            stateMachine.canJump = !stateMachine.canJump;
        }

        public override void ExitState()
        {
            
        }
    }
    public class OnChargingPunchGroundState : State
    {
        public OnChargingPunchGroundState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            if (!stateMachine.canPunch) return;
            player.SetAnimationBool("IsCharging",true);
            player.SetVelocity(new Vector2(0,-1.5f));
            MoveAndGravity(false);

            stateMachine.isCharging = true;
            player.StartGrowingSprite();
        
            stateMachine.SetActualTime(Time.time);
        }

        public override void ExitState()
        {
            stateMachine.ChangeState(StateType.OnLaunchPunchGround);
        }
    }
    
    public class OnLaunchPunchGroundState : State
    {
        public OnLaunchPunchGroundState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            if (!stateMachine.canPunch) return;
            
            player.SetAnimationBool("IsCharging",false);
            player.SetAnimationBool("Launch",true);
            
            player.EnableFistCollider(true);
            
            var duration = Time.time - stateMachine.GetActualTime();
            
            player.SetForceToFist(player.GetForceOnTime(duration));
            
            stateMachine.canMove = false;
            PreparePunch();
            player.Punch(player.GetForceOnTime(duration), player.GetRecoveryTime(duration));
            
            stateMachine.isCharging = false;
            player.ResizeSprite();
        }

        public override void ExitState()
        {
        }
    }
    public class HitStunState : State
    {
        public HitStunState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            player.ResizeSprite();
            
            player.RotatePlayer();
            player.GetRigidBody().useGravity = true;
            stateMachine.canJump = false;
            stateMachine.canMove = false;
            stateMachine.canPunch = false;
            player.EnableFistCollider(false);
            
            player.SetAnimationTrigger("Hit");
            player.SetAnimationBool("IsCharging",false);
        }

        public override void ExitState()
        {
            stateMachine.canPunch = true;
            stateMachine.ChangeState(StateType.OnRecovery);
        }
    }
    
    public class OnChargingPunchAirState : State
    {
        public OnChargingPunchAirState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            if (!stateMachine.canPunch) return;
            
            player.SetAnimationBool("IsCharging",true);
            
            player.SetVelocity(new Vector2(0, -1.5f));
            MoveAndGravity(false);
            
            stateMachine.isCharging = true;
            player.StartGrowingSprite();

            stateMachine.SetActualTime(Time.time);
        }

        public override void ExitState()
        {
            stateMachine.ChangeState(StateType.OnLaunchPunchAir);
        }
    }
    
    public class OnLaunchPunchAirState : State
    {
        public OnLaunchPunchAirState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            if (!stateMachine.canPunch) return;
            
            player.SetAnimationBool("IsCharging",false);
            player.SetAnimationBool("Launch",true);
            
            player.EnableFistCollider(true);
            
            var duration = Time.time - stateMachine.GetActualTime();
            
            player.SetForceToFist(player.GetForceOnTime(duration));
            
            stateMachine.canMove = false;
            PreparePunch();
            player.Punch(player.GetForceOnTime(duration),player.GetRecoveryTime(duration));
            
            stateMachine.isCharging = false;
            player.ResizeSprite();
        }

        public override void ExitState()
        {
        }
    }
    
    public class OnDeathState : State
    {
        public OnDeathState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            //Ocultar malla, reposicionar, esperar x segundos
            player.HideMesh();
            player.ReturnToSpawn();
            player.ShowMesh();
            player.SetInmortal(true);
            player.SetVelocity(Vector2.zero);
            stateMachine.canMove = false;
            stateMachine.canJump = false;
            player.SetAnimationTrigger("Dead");
        }

        public override void ExitState()
        {
            player.SetInmortal(false);
            stateMachine.canMove = true;
            stateMachine.canJump = true;
            stateMachine.ChangeState(StateType.OnGround);
        }
    }
}