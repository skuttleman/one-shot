using System;
using UnityEngine;

namespace Game.Utils
{
    public static class Vectors
    {
        public static Vector3 Upgrade(Vector2 vector)
        {
            return new Vector3(vector.x, vector.y, 0f);
        }

        public static float Angle(Vector2 origin, Vector2 position)
        {
            Vector2 difference = origin - position;
            return Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg + 90f;
        }
    }
}
