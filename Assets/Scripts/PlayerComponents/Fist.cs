using Others;
using UnityEngine;

namespace PlayerComponents
    {
        public class Fist : MonoBehaviour
        {
            public Player player;
            private float _force;
            private Collider _ownCollider;

            private void Awake() => TryGetComponent(out _ownCollider);

            public Collider GetCollider() => _ownCollider;
            
            public void SetForce(float force) => _force = force;

            private void OnCollisionEnter(Collision collision)
            {
                if (!collision.gameObject.CompareTag(TagNames.Player)) return;
                
                if (player.gameObject == collision.gameObject) return;
                collision.gameObject.GetComponent<Player>().ApplyPunchForce(player.GetLastPunchDirection());
            }
        }
    }
