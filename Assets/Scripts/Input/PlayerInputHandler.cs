using PlayerComponents;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;
    private PlayerController _controller;
    private Player _player;
    private InputAction _movePointer,_fireButton,_jumpButton;

    public void Initialize(int playerIndex)
    {
        _playerInput = GetComponent<PlayerInput>();
        _player = GetComponentInParent<Player>();

        _movePointer = _controller.Gamepad.Joystick;
        _jumpButton = Adapter.GetAction(playerIndex, _controller, Adapter.ActionType.Jump);
        _fireButton = Adapter.GetAction(playerIndex, _controller, Adapter.ActionType.Fire);

        _movePointer.performed += OnMovePointer;
        _fireButton.started += StartFire;
        _fireButton.performed += Fire;
        _jumpButton.performed += Jump;
    }

    private void Jump(CallbackContext context)
    {
        if (IsActualDevice(context)) return;
        _player.Jump();
    }

    private void StartFire(CallbackContext context)
    {
        if (IsActualDevice(context)) return;
        _player.StartCharging();
    }

    private void Fire(CallbackContext context)
    {
        if (IsActualDevice(context)) return;
        _player.ApplyForce();
    }

    public void SetController(PlayerController controller) => _controller = controller;

    private void OnMovePointer(CallbackContext context)
    {
        if (IsActualDevice(context)) return;
        _player.Move(context.ReadValue<Vector2>());
    }

    private bool IsActualDevice(CallbackContext context) =>
        _player == null || _player.playerIndex != context.control.device.deviceId;
}
