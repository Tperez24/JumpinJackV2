using InputScripts;
using Others;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerComponents
{
    public class Creator : MonoBehaviour
    {
        public GameData data;
        public Transform firstPlayerSpawn, secondPlayerSpawn;
        public Material firstPlayerMaterial, secondPlayerMaterial;
        public Material firstPlayerMaterialFist, secondPlayerMaterialFist;
        public GameObject playerToInstantiate;
    
        private PlayerController _controller;
        private int _playerIndex;


        public void Initialize(InputDevice device1, InputDevice device2)
        {
            _controller = new PlayerController();
            _controller.Enable();
        
            var p1 = PlayerInput.Instantiate(playerToInstantiate, controlScheme: "Gamepad", pairWithDevice: device1);
            var p2 = PlayerInput.Instantiate(playerToInstantiate, controlScheme: "Gamepad",  pairWithDevice: device2);
            
            InitializePlayer(p1, device1.deviceId);
            InitializePlayer(p2, device2.deviceId);
        }

        private void InitializePlayer(Component player, int device)
        {
            var playerBehaviour = player.gameObject.GetComponentInParent<Player>();
            var inputHandler = player.gameObject.GetComponent<PlayerInputHandler>();

            playerBehaviour.playerIndex = device;
            playerBehaviour.SetData(data);

            inputHandler.SetController(_controller);
            inputHandler.Initialize(_playerIndex);

            if (_playerIndex == 0)
            {
                SetPlayer(playerBehaviour, firstPlayerSpawn.position, firstPlayerMaterial, "Blue",firstPlayerMaterialFist);
            }
            else
            {
                SetPlayer(playerBehaviour, secondPlayerSpawn.position, secondPlayerMaterial, "Red",secondPlayerMaterialFist);
            }

            _playerIndex++;
        }

        private static void SetPlayer(Player player, Vector3 pos, Material mat, string playerName,
            Material playerMaterialFist)
        {
            player.gameObject.transform.position = pos;
            player.SetMaterial(mat,playerMaterialFist);
            player.SetSpawnPoint(pos);
            player.SetPlayerName(playerName);
        }
    }
}
