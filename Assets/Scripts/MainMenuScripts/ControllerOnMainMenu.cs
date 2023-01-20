using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace MainMenuScripts
{
    public class ControllerOnMainMenu : MonoBehaviour
    {
        private InputAction _move,_accept;
        private GameObject _play, _exit;
        private bool _playButtonSelected, _exitButtonSelected;
        private void Awake()
        {
            var controller = GetComponent<PlayerInput>();

            _play = GameObject.Find("PlayBut");
            _exit = GameObject.Find("ExitBut");
            
            _move = controller.actions.FindAction("Cruz");
            _accept = controller.actions.FindAction("Jump");

            SubscribeInputs();
        
            SelectPlay();
        }

        private void SubscribeInputs()
        {
            _move.performed += Select;
            _accept.performed += Accept;
        }

        private void UnsubscribeInputs()
        {
            _move.performed -= Select;
            _accept.performed -= Accept;
        }

        private void Accept(InputAction.CallbackContext obj)
        {
            if (_playButtonSelected)
            {
                UnsubscribeInputs();
                SceneManager.LoadScene(1);
            }

            if (!_exitButtonSelected) return;
            
            UnsubscribeInputs();
            Application.Quit();
        }

        private void Select(InputAction.CallbackContext obj)
        {
            if (obj.ReadValue<Vector2>() == Vector2.up) SelectPlay();
            if (obj.ReadValue<Vector2>() == Vector2.down) SelectExit();
        }

        private void SelectPlay()
        {
            ScalePlay(new Vector3(1.5f, 1.5f, 1.5f));
            ScaleExit(new Vector3(0.8f, 0.8f, 0.8f));
            EnablePlay(true);
        }
    
        private void SelectExit()
        {
           ScalePlay(Vector3.one);
           ScaleExit(new Vector3(1.5f, 1.5f, 1.5f));
           EnablePlay(false);
        }

        private void ScalePlay(Vector3 scale) => _play.gameObject.transform.localScale = scale;
        private void ScaleExit(Vector3 scale) => _exit.gameObject.transform.localScale = scale;

        private void EnablePlay(bool enable)
        {
            _playButtonSelected = enable;
            _exitButtonSelected = !enable;
        }
    }
}
