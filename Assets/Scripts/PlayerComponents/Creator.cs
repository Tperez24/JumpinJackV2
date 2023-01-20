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
        public GameObject playerToInstantiate;
    
        private PlayerController _controller;
        private int _playerIndex;


        public void Initialize()
        {
            _controller = new PlayerController();
            _controller.Enable();
        
            var p1 = PlayerInput.Instantiate(playerToInstantiate, controlScheme: "Gamepad", pairWithDevice: Gamepad.all[0]);
            var p2 = PlayerInput.Instantiate(playerToInstantiate, controlScheme: "Gamepad",  pairWithDevice: Gamepad.all[1]);
            
            InitializePlayer(p1, Gamepad.all[0].device.deviceId);
            InitializePlayer(p2, Gamepad.all[1].device.deviceId);
        }

        private void InitializePlayer(Component player, int device)
        {
            var playerBehaviour = player.gameObject.GetComponentInParent<Player>();
            var inputHandler = player.gameObject.GetComponent<PlayerInputHandler>();

            playerBehaviour.playerIndex = device;
            playerBehaviour.SetData(data);

            inputHandler.SetController(_controller);
            inputHandler.Initialize(_playerIndex);

            if (_playerIndex == 0) SetPlayer(playerBehaviour, firstPlayerSpawn.position, firstPlayerMaterial, "Blue");
            else SetPlayer(playerBehaviour, secondPlayerSpawn.position, secondPlayerMaterial, "Red");

            _playerIndex++;
        }

        private static void SetPlayer(Player player, Vector3 pos, Material mat, string playerName)
        {
            player.gameObject.transform.position = pos;
            player.SetMaterial(mat);
            player.SetSpawnPoint(pos);
            player.SetPlayerName(playerName);
        }
    }
}
