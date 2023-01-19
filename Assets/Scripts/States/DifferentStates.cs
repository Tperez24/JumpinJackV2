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
            stateMachine.canMove = true;
            player.GetRigidBody().useGravity = true;
            
            player.SetVelocity(new Vector2(player.GetRigidBody().velocity.x * player.GetData().recoveryMultiplier,player.GetRigidBody().velocity.y * player.GetData().recoveryMultiplier));
            //Movimiento normal
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

            if (Physics.Raycast(player.transform.position,Vector3.down,out hit,1f) && Player.IsOnGround(hit.collider.gameObject))
            {
                stateMachine.canJump = true;
            }

            player.SetAnimationTrigger("Recovery");
            
            stateMachine.canMove = true;
            player.EnableFistCollider(false);
            player.GetRigidBody().useGravity = true;
            
            player.SetVelocity(new Vector2(player.GetRigidBody().velocity.x * player.GetData().recoveryMultiplier,player.GetRigidBody().velocity.y * player.GetData().recoveryMultiplier));
        }
    }
    
    public class OnAir : State
    {
        public OnAir(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            //Movimiento leve
            if (!stateMachine.canJump) return;
            
            player.SetForceToFist(0);
            
            stateMachine.canMove = true;
            player.GetRigidBody().useGravity = true;
            player.SetVelocity(new Vector2(player.GetRigidBody().velocity.x,0));
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
            stateMachine.canMove = false;
            player.SetVelocity(new Vector2(0,-1.5f));
            player.GetRigidBody().useGravity = false;

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
            player.SetAnimationTrigger("Launch");
            
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
    public class HitStunState : State
    {
        public HitStunState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            player.GetRigidBody().useGravity = true;
            stateMachine.canJump = false;
            stateMachine.canMove = false;
            stateMachine.canPunch = false;
            player.EnableFistCollider(false);
            
            player.SetAnimationTrigger("Hit");
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
            
            stateMachine.canMove = false;
            player.SetVelocity(new Vector2(0, -1.5f));
            player.GetRigidBody().useGravity = false;
            
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
            player.SetAnimationTrigger("Launch");
            
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
        }

        public override void ExitState()
        {
            //Habilitar malla
        }
    }
}