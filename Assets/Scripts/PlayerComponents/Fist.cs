﻿using System;
using DefaultNamespace;
using UnityEngine;

namespace PlayerComponents
    {
        public class Fist : MonoBehaviour
        {
            public Player player;
            private float _force;

            public void SetForce(float force) => _force = force;

            private void OnCollisionEnter(Collision collision)
            {
                if (collision.gameObject.CompareTag(TagNames.Player))
                {
                    if (player.gameObject == collision.gameObject) return;
                    collision.gameObject.GetComponent<Player>().ApplyPunchForce(_force,player.GetLastPunchDirection());
                }
            }
        }
    }
