﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Others
{
        [CreateAssetMenu(menuName = "Game Data")]
        public class GameData : ScriptableObject
        {
                public List<ForceDictionary> forces;
                public float dotAngle = 0.707f;

                public float jumpForce;
                public float punchCooldown;
                public float bounceForce;
        }

        [Serializable]
        public class ForceDictionary
        {
                public float time;
                public float forces;
        }
}