using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControllerOnWinMenu : MonoBehaviour
{
   
    
        private InputAction _move,_accept;
        private GameObject _jugar, _salir;
        private bool _playButtonSelected, _exitButtonSelected;
        private void Start()
        {
            var controller = GetComponent<PlayerInput>();

            
                
            _jugar = GameObject.Find("Volver");
            _salir = GameObject.Find("Menu");
            
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
            Destroy(GameObject.Find("UIManager"));
            
            if (_playButtonSelected)
            {
                UnsuscribeInputs();
                SceneManager.LoadScene(1);
            }

            if (_exitButtonSelected)
            {
                UnsuscribeInputs();
                SceneManager.LoadScene(0);
            }
        }

        private void Select(InputAction.CallbackContext obj)
        {
            if (obj.ReadValue<Vector2>() == Vector2.left)
            {
                SelectPlay();
            }

            if (obj.ReadValue<Vector2>() == Vector2.right)
            {
                SelectExit();
            }
           
        }

        private void SelectPlay()
        {
            _jugar.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            _salir.gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            _playButtonSelected = true;
            _exitButtonSelected = false;
        }
    
        private void SelectExit()
        {
            _jugar.gameObject.transform.localScale = Vector3.one;
            _salir.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            _playButtonSelected = false;
            _exitButtonSelected = true;
        }
    }

