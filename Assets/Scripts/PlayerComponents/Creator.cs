using Others;
using PlayerComponents;
using UnityEngine;
using UnityEngine.InputSystem;

public class Creator : MonoBehaviour
{
    public PlayerInputManager playerInputManager;
    public GameData data;
    public Transform firstPlayerSpawn, secondPlayerSpawn;
    public Material firstPlayerMaterial, secondPlayerMaterial;
    
    private PlayerController _controller;
    private int _playerIndex;


    private void Start()
    {
        _controller = new PlayerController();
        _controller.Enable();

        playerInputManager.onPlayerJoined += InitializePlayer;
    }

    private void InitializePlayer(PlayerInput player)
    {
        var playerBehaviour = player.gameObject.GetComponentInParent<Player>();
        var inputHandler = player.gameObject.GetComponent<PlayerInputHandler>();

        playerBehaviour.playerIndex = player.devices[0].deviceId;
        playerBehaviour.SetData(data);
        
        inputHandler.SetController(_controller);
        inputHandler.Initialize();

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
