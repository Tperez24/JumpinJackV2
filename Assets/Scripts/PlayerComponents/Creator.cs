using Others;
using PlayerComponents;
using UnityEngine;
using UnityEngine.InputSystem;

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
       /* var player = Instantiate(playerToInstantiate, firstPlayerSpawn.position, Quaternion.identity);
        player.transform.SetParent(transform);
        var inputPlayer = player.GetComponentInChildren<PlayerInput>();*/
        InitializePlayer(p1, Gamepad.all[1].device.deviceId);
        InitializePlayer(p2, Gamepad.all[0].device.deviceId);
    }

    private void InitializePlayer(PlayerInput player, int device)
    {
        var playerBehaviour = player.gameObject.GetComponentInParent<Player>();
        var inputHandler = player.gameObject.GetComponent<PlayerInputHandler>();

        playerBehaviour.playerIndex = device;
        playerBehaviour.SetData(data);

        inputHandler.SetController(_controller);
        inputHandler.Initialize(_playerIndex);

        if (_playerIndex == 0)
        {
            playerBehaviour.gameObject.transform.position = firstPlayerSpawn.position;
            playerBehaviour.SetMaterial(firstPlayerMaterial);
            playerBehaviour.SetSpawnPoint(firstPlayerSpawn.position);
        }
        else
        {
            playerBehaviour.gameObject.transform.position = secondPlayerSpawn.position;
            playerBehaviour.SetMaterial(secondPlayerMaterial);
            playerBehaviour.SetSpawnPoint(secondPlayerSpawn.position);
        }
        
        _playerIndex++;
    }
}
