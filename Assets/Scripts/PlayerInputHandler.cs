using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerController _controller;
    private Player mover;
    private InputAction movePointer;
    
    public void Initialize()
    {
        playerInput = GetComponent<PlayerInput>();
        var movers = FindObjectsOfType<Player>();
        var index = playerInput.playerIndex;
        mover = movers.FirstOrDefault(m => m.GetPlayerIndex() == index);

        movePointer = _controller.Gamepad.Joystick;
        movePointer.performed += OnMovePointer;
    }

    public void SetController(PlayerController controller) => _controller = controller;
    
    public void OnMovePointer(CallbackContext context)
    {
        if(mover != null)
            mover.MovePointer(context);
    }

}
