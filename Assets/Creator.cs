using UnityEngine;
using UnityEngine.InputSystem;

public class Creator : MonoBehaviour
{
    public PlayerInputManager playerInputManager;
    private PlayerController _controller;

    private void Start()
    {
        _controller = new PlayerController();
        _controller.Enable();

        playerInputManager.onPlayerJoined += InitializePlayer;
    }

    private void InitializePlayer(PlayerInput player)
    {
        var playerBehaviour = player.gameObject.GetComponent<Player>();
        var inputHandler = player.gameObject.GetComponentInChildren<PlayerInputHandler>();

        inputHandler.SetController(_controller);
        inputHandler.Initialize();
    }
}
