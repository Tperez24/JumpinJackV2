//[RequireComponent(typeof(StateMachine))]
namespace States
{
        public class State //: MonoBehaviour
        {
                protected StateMachine stateMachine;

                /*private void Awake()
                {
                StateMachine = GetComponent<StateMachine>();
                }*/

                protected State(StateMachine stateMachine) => this.stateMachine = stateMachine;
                public virtual void DoAction() {}
                public virtual void CheckExit() {}
        }
}