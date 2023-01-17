using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class State : MonoBehaviour
{
        protected StateMachine StateMachine;

        private void Awake()
        {
                StateMachine = GetComponent<StateMachine>();
        }
        
        public virtual void CheckExit() {}
}