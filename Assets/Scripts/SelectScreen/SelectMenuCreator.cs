using System.Collections;
using System.Collections.Generic;
using PlayerComponents;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SelectScreen
{
    public class SelectMenuCreator : MonoBehaviour
    {
        public List<GameObject> gameObjectsToHide;
        public PlayerInputManager creator;
        public Transform canvas;

        public Transform player1, player2;
        private InputAction _start;
        private int _playerIndex;

        private InputDevice _device1, _device2;
        
        private void Start() => SubscribeInputs();
        private void OnDisable() => UnsubscribeInputs();

        private void SubscribeInputs() => creator.onPlayerJoined += Join;
        private void UnsubscribeInputs() => creator.onPlayerJoined -= Join;

        private void Join(PlayerInput obj)
        {
            switch (creator.playerCount)
            {
                case 1: 
                    SetPlayer(obj, Color.blue, player1.position);
                    _device1 = obj.devices[0].device;
                    break;
                case 2: 
                    SetPlayer(obj, Color.red, player2.position);
                    _device2 = obj.devices[0].device;

                    StartCoroutine(StartGame());
                    break;
            }
        }

        private void SetPlayer(Component obj, Color color, Vector3 pos)
        {
            obj.GetComponent<Image>().color = color;
            obj.transform.SetParent(canvas);
            obj.gameObject.transform.position = pos;
        }

        private IEnumerator StartGame()
        {
            Debug.Log("FadeScreen");
            var newScene = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
            do
            {
                yield return null;
            } while (newScene.progress != 1);
        
            var newCreator = GameObject.Find("Creator").GetComponent<Creator>();

            newCreator.Initialize(_device1, _device2);
     
            foreach (var go in gameObjectsToHide) go.SetActive(false);
        }
    }
}
