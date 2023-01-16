using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scenes
{
    public class Test : MonoBehaviour
    {
        private PlayerController _controller;
        private InputAction _teclaG;
        public Rigidbody playerRb;
        public int force;
        
        private void Start()
        {
            _controller = new PlayerController();
            _controller.Enable();
            _teclaG = _controller.PlayerMap.Newaction;
            _teclaG.performed += PulsoG;
        }

        private void OnDisable()
        {
            _teclaG.performed -= PulsoG;
        }

        private void PulsoG(InputAction.CallbackContext context)
        {
            Debug.Log("Pulso G" + context.action);
        }

        private void Update()
        {
            var click = Mouse.current;

            if (click.leftButton.isPressed)
            {
                playerRb.AddForce(click.position.ReadValue().normalized  *  force,ForceMode.Impulse);
            }
        }
    }
}