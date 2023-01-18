namespace States
{
    public class OnGroundState : State
    {
        public OnGroundState(StateMachine stateMachine) : base(stateMachine) { }

        public override void DoAction()
        {
            //Movimiento normal
        }

        public override void CheckExit()
        {
            base.CheckExit();
        }
    }
    
    public class OnAir : State
    {
        public OnAir(StateMachine stateMachine) : base(stateMachine) { }

        public override void DoAction()
        {
            //Movimiento leve
        }

        public override void CheckExit()
        {
            base.CheckExit();
        }
    }
    public class OnChargingPunchGroundState : State
    {
        public OnChargingPunchGroundState(StateMachine stateMachine) : base(stateMachine) { }

        public override void DoAction()
        {
            //Quitar movimiento
            //Al lanzarlo quitar friccion durante x segundos
        }

        public override void CheckExit()
        {
            //reiniciar
        }
    }
    public class HitStunState : State
    {
        public HitStunState(StateMachine stateMachine) : base(stateMachine) { }

        public override void DoAction()
        {
            //desactivar gravedad x segundos
            //Lanzar en direccion de golpe
            //Jugador pierde control
        }

        public override void CheckExit()
        {
            //Reiniciar inercias
        }
    }
    
    public class OnChargingPunchAirState : State
    {
        public OnChargingPunchAirState(StateMachine stateMachine) : base(stateMachine) { }

        public override void DoAction()
        {
            //Quitar gravedad y aplicar fuerza hacia abajo lenta
            //No nos podemos mover
        }

        public override void CheckExit()
        {
            //Añadir fuerza y reiniciar gravedad
            //Cooldown
        }
    }
}