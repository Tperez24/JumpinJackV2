//[RequireComponent(typeof(StateMachine))]

using UnityEngine;

namespace States
{
        public class State //: MonoBehaviour
        {
                protected StateMachine stateMachine;
                protected Player player;

                /*private void Awake()
                {
                StateMachine = GetComponent<StateMachine>();
                }*/

                protected State(StateMachine stateMachine,Player player)
                {
                        this.stateMachine = stateMachine;
                        this.player = player;
                }

                protected void PreparePunch()
                {
                        player.GetRigidBody().useGravity = true;
                        player.SetVelocity(Vector3.zero);
                        stateMachine.canPunch = false;
                }

                public virtual void DoAction() {}
                public virtual void ExitState() {}
        }
}