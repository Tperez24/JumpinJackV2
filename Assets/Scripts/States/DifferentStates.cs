namespace States
{
    public class OnGroundState : State
    {
        //Movimiento normal
        public OnGroundState(StateMachine stateMachine) : base(stateMachine) { }
    }
    
    public class OnAir : State
    {
        //Movimiento leve
        public OnAir(StateMachine stateMachine) : base(stateMachine) { }
    }
    public class OnChargingPunchGroundState : State
    {
        //Quitar movimiento
        //Al lanzarlo quitar friccion durante x segundos
        //reiniciar
        public OnChargingPunchGroundState(StateMachine stateMachine) : base(stateMachine) { }
    }
    public class HitStunState : State
    {
        //desactivar gravedad x segundos
        //Lanzar en direccion de golpe
        //Jugador pierde control
        //Reiniciar inercias
        public HitStunState(StateMachine stateMachine) : base(stateMachine) { }
    }
    
    public class OnChargingPunchAirState : State
    {
        //Quitar gravedad y aplicar fuerza hacia abajo lenta
        //No nos podemos mover
        //Añadir fuerza y reiniciar gravedad
        //Cooldown
        public OnChargingPunchAirState(StateMachine stateMachine) : base(stateMachine) { }
    }
}