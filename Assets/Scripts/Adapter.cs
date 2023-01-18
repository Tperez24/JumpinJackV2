using System;
using UnityEngine.InputSystem;

public static class Adapter
{
    public static InputAction GetAction(int playerId, PlayerController controller,ActionType action)
    {
        if (playerId == 0)
        {
            return action switch
            {
                ActionType.Fire => controller.Gamepad.FireButton,
                ActionType.Jump => controller.Gamepad.Jump,
                _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
            };
        }
        
        else
            return action switch
            {
                ActionType.Fire => controller.Gamepad.FireButton1,
                ActionType.Jump => controller.Gamepad.Jump1,
                _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
            };
    }
    
    public enum ActionType
    {
        Jump,
        Fire
        
    }
}