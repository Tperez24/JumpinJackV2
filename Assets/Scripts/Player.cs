using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody playerRb;
    public float force = 0.2f;
    public GameObject pointer;
    public int playerIndex;
    public float jumpForce;

    private float _actualTime;
    private bool _canJump;
    private GameData _data;
    
    public void ApplyForce()
    {
        Debug.Log("Muevo al jugador " + playerIndex);
        
        force = GetForceOnTime(Time.time - _actualTime);

        var dir = (pointer.transform.position - transform.position);
        
        playerRb.AddForce(dir  *  force,ForceMode.VelocityChange);
    }

    private float GetForceOnTime(float duration)
    {
        foreach (var forceDictionary in _data.forces.Where(forceDictionary => duration <= forceDictionary.time))
            return forceDictionary.forces;

        return 10;
    }

    public void StartCharging() => _actualTime =  Time.time;

    public void MovePointer(Vector2 direction)
    {
        var dir = new Vector3(direction.x, direction.y, 0);
        pointer.transform.position = transform.position + dir;
    }

    public void SetData(GameData data) => _data = data;

    public void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);
    }
}