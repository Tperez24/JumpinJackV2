using UnityEngine;
using UnityEngine.InputSystem;

public class Creator : MonoBehaviour
{
    public PlayerInputManager playerInputManager;
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
        inputHandler.SetController(_controller);
        inputHandler.Initialize();
    }
}
