using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody playerRb;
    public float force;
    public GameObject pointer;
    public int playerIndex = 0;
    
    public void MovePlayer(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Muevo al jugador ");
    }

    public void MovePointer(InputAction.CallbackContext callbackContext)
    {
        var value = callbackContext.ReadValue<Vector2>();
        if(value == Vector2.zero) return;
        var direction = new Vector3(value.x, value.y, 0);
        pointer.transform.position = transform.position + direction;
        Debug.Log(callbackContext.ReadValue<Vector2>());
    }

    public int GetPlayerIndex() => playerIndex;

    private void Update()
    {
        var click = Mouse.current;

        if (click.leftButton.isPressed)
        {
            playerRb.AddForce(click.position.ReadValue().normalized  *  force,ForceMode.Impulse);
            Debug.Log(click.position.ReadValue().normalized);
        }
    }
}