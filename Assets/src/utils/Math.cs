using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    public static class Math
    {
        public static bool NonZero(float value)
        {
            return Mathf.Abs(value) > Mathf.Epsilon;
        }

        public static float Angle(Vector2 origin, Vector2 position) {
            Vector2 difference = origin - position;
            return Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg + 90f;
        }

        public static Vector3 upgrade(Vector2 vector)
        {
            return new Vector3(vector.x, vector.y, 0f);
        }
    }
}
