using System;
using System.Collections.Generic;
using TMPro;
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
                public float moveMultiplier;
                public float recoveryMultiplier;
                public Vector3 punchScale;
                public float gameTime = 300;

                public List<string> winText;
        }

        [Serializable]
        public class ForceDictionary
        {
                public float time;
                public float recoveryTime;
                public float forces;
        }
}