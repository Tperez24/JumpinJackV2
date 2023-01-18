using System;
using System.Collections;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody playerRb;
    public float force = 0.2f;
    public GameObject pointer;
    public int playerIndex;

    private float _actualTime;
    private bool _canJump = true, _canPunch = true;
    private GameData _data;
    
    public void ApplyForce()
    {
        Debug.Log("Muevo al jugador " + playerIndex);
        
        force = GetForceOnTime(Time.time - _actualTime);
        
        Punch();
    }

    private float GetForceOnTime(float duration)
    {
        foreach (var forceDictionary in _data.forces.Where(forceDictionary => duration <= forceDictionary.time))
            return forceDictionary.forces;

        return 10;
    }

    public void StartCharging()
    {
        if (!_canPunch) return;
        playerRb.velocity = new Vector2(0,-1.5f);
        playerRb.useGravity = false;
        
        _actualTime = Time.time;
    }

    public void MovePointer(Vector2 direction)
    {
        var dir = new Vector3(-direction.x, direction.y, 0);
        var finalDir = transform.position + dir;
        pointer.transform.position = new Vector3(finalDir.x,finalDir.y + 0.75f, finalDir.z);
    }

    public void SetData(GameData data) => _data = data;

    public void Jump()
    {
        if (!_canJump) return;
        playerRb.velocity = new Vector2(playerRb.velocity.x,0);
        AddForce(Vector3.up * _data.jumpForce, ForceMode.Impulse, () => { });
        _canJump = !_canJump;
    }

    private void Punch()
    {
        if (!_canPunch) return;

        playerRb.useGravity = true;
        playerRb.velocity = Vector3.zero;
        _canPunch = false;
        var pointerPos = pointer.transform.position;
        var ownPos = transform.position + new Vector3(0,0.75f,0);
        var dir = (pointerPos -ownPos).normalized;
        var distance = Vector2.Distance(pointerPos, ownPos);
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, dir, out hit,distance) && hit.collider.gameObject.CompareTag(TagNames.Ground))
        {
            var dot = Vector3.Dot(-transform.up, dir);
           //si angulo es -45 o 45
           if (dot > 0.707)
            {
                playerRb.velocity = Vector3.zero;
                AddForce(-dir * _data.bounceForce,ForceMode.VelocityChange,() => _canPunch = true);
                return;
            }
        }
        
        AddForce(dir  *  force,ForceMode.VelocityChange,() => _canPunch = true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagNames.Ground)) _canJump = true;
        if (collision.gameObject.CompareTag(TagNames.Player)) playerRb.useGravity = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagNames.Ground)) _canJump = false;
    }

    private void OnDrawGizmos()
    {
        var position = transform.position + new Vector3(0,0.75f,0);
        var dir = (pointer.transform.position -position).normalized;
        Gizmos.DrawRay(position,dir);
    }

    private void AddForce(Vector2 dir,ForceMode mode,Action action)
    {
        playerRb.AddForce(dir,mode);
        StartCoroutine(StartCooldown(action,_data.punchCooldown));
    }

    private static IEnumerator StartCooldown(Action onCooldownEnd,float time)
    {
        Debug.Log("Empiezo cooldown");
        yield return new WaitForSecondsRealtime(time);
        Debug.Log("Acabo cooldown");
        onCooldownEnd?.Invoke();
    }
    
}