using UnityEngine;

namespace States
{
    public class OnGroundState : State
    {
        public OnGroundState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            stateMachine.canJump = true;
            //Movimiento normal
        }

        public override void ExitState()
        {
            
        }
    }
    
    public class OnAir : State
    {
        public OnAir(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            //Movimiento leve
            if (!stateMachine.canJump) return;
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
            //Quitar movimiento
            //Al lanzarlo quitar friccion durante x segundos
            if (!stateMachine.canPunch) return;
            player.SetVelocity(new Vector2(0,0));
            player.GetRigidBody().useGravity = false;
        
            stateMachine.SetActualTime(Time.time);
        }

        public override void ExitState()
        {
            //reiniciar
            stateMachine.ChangeState(StateType.OnLaunchPunchGround);
        }
    }
    
    public class OnLaunchPunchGroundState : State
    {
        public OnLaunchPunchGroundState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            //Quitar movimiento
            //Al lanzarlo quitar friccion durante x segundos
            if (!stateMachine.canPunch) return;
            PreparePunch();
            player.Punch(player.GetForceOnTime(Time.time - stateMachine.GetActualTime()));
            
        }

        public override void ExitState()
        {
            //reiniciar
        }
    }
    public class HitStunState : State
    {
        public HitStunState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            //desactivar gravedad x segundos
            //Lanzar en direccion de golpe
            //Jugador pierde control
        }

        public override void ExitState()
        {
            //Reiniciar inercias
        }
    }
    
    public class OnChargingPunchAirState : State
    {
        public OnChargingPunchAirState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            //Quitar gravedad y aplicar fuerza hacia abajo lenta
            //No nos podemos mover
            if (!stateMachine.canPunch) return;
            player.SetVelocity(new Vector2(0, -1.5f));
            player.GetRigidBody().useGravity = false;

            stateMachine.SetActualTime(Time.time);
        }

        public override void ExitState()
        {
            //Añadir fuerza y reiniciar gravedad
            //Cooldown
            stateMachine.ChangeState(StateType.OnLaunchPunchAir);
        }
    }
    
    public class OnLaunchPunchAirState : State
    {
        public OnLaunchPunchAirState(StateMachine stateMachine,Player player) : base(stateMachine,player) { }

        public override void DoAction()
        {
            //Quitar gravedad y aplicar fuerza hacia abajo lenta
            //No nos podemos mover
            if (!stateMachine.canPunch) return;
            
            PreparePunch();
            player.Punch(player.GetForceOnTime(Time.time - stateMachine.GetActualTime()));
        }

        public override void ExitState()
        {
            //Añadir fuerza y reiniciar gravedad
            //Cooldown
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