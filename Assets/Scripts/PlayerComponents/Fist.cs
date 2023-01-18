using System;
using DefaultNamespace;
using UnityEngine;

namespace PlayerComponents
    {
        public class Fist : MonoBehaviour
        {
            public GameObject parent;
            private float _force;

            public void SetForce(float force) => _force = force;

            private void OnCollisionEnter(Collision collision)
            {
                if (collision.gameObject.CompareTag(TagNames.Player))
                {
                    if (parent == collision.gameObject) return;
                    collision.gameObject.SendMessageUpwards("ApplyPunchForce",(_force,(collision.gameObject.transform.position - transform.position).normalized));
                }
            }
        }
    }
