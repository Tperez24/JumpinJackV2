using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenuScripts
{
    public class ControllerOnMainMenu : MonoBehaviour
    {
        private InputAction _move,_accept;
        private GameObject jugar, salir;
        private bool _playButtonSelected, _exitButtonSelected;
        private void Awake()
        {
            var controller = GetComponent<PlayerInput>();

            jugar = GameObject.Find("PlayBut");
            salir = GameObject.Find("ExitBut");
            
            _move = controller.actions.FindAction("Cruz");
            _accept = controller.actions.FindAction("Jump");

            SuscribeInputs();
        
            SelectPlay();
        }

        private void SuscribeInputs()
        {
            _move.performed += Select;
            _accept.performed += Accept;
        }

        private void UnsuscribeInputs()
        {
            _move.performed -= Select;
            _accept.performed -= Accept;
        }

        private void Accept(InputAction.CallbackContext obj)
        {
            if (_playButtonSelected)
            {
                UnsuscribeInputs();
                SceneManager.LoadScene(1);
            }

            if (_exitButtonSelected)
            {
                UnsuscribeInputs();
                Application.Quit();
            }
        }

        private void Select(InputAction.CallbackContext obj)
        {
            if (obj.ReadValue<Vector2>() == Vector2.up)
            {
                SelectPlay();
            }

            if (obj.ReadValue<Vector2>() == Vector2.down)
            {
                SelectExit();
            }
           
        }

        private void SelectPlay()
        {
            jugar.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            salir.gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            _playButtonSelected = true;
            _exitButtonSelected = false;
        }
    
        private void SelectExit()
        {
            jugar.gameObject.transform.localScale = Vector3.one;
            salir.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            _playButtonSelected = false;
            _exitButtonSelected = true;
        }
    }
}
