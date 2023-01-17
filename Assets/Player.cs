using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody playerRb;
    public float force = 0.2f;
    public GameObject pointer;
    public int playerIndex = 0;
    
    public void ApplyForce(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Muevo al jugador " + playerIndex);
        
        var dir = (pointer.transform.position - transform.position);
        
        playerRb.AddForce(dir  *  force,ForceMode.Impulse);
    }

    public void MovePointer(Vector2 direction)
    {
        var dir = new Vector3(direction.x, direction.y, 0);
        pointer.transform.position = transform.position + dir;
    }
}