using PlayerComponents;
using UnityEngine;

namespace States
{
        public class State
        {
                protected readonly StateMachine StateMachine;
                protected readonly Player Player;

                protected State(StateMachine stateMachine,Player player)
                {
                        StateMachine = stateMachine;
                        Player = player;
                }

                protected void PreparePunch()
                {
                        Player.SetVelocity(Vector3.zero);
                        StateMachine.canPunch = false;
                }

                protected void MoveAndGravity(bool enable)
                {
                        StateMachine.canMove = enable;
                        Player.GetRigidBody().useGravity = enable;
                }

                public virtual void DoAction() {}
                public virtual void ExitState() {}
        }
}