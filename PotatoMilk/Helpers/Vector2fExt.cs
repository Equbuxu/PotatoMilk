﻿using SFML.System;
using System;

namespace PotatoMilk.Helpers
{
    public static class Vector2fExt
    {
        public static float Dot(this Vector2f vector, Vector2f other) => vector.X * other.X + vector.Y * other.Y;
        public static Vector2f TurnRight90(this Vector2f vector) => new Vector2f(vector.Y, -vector.X);
        public static Vector2f TurnLeft90(this Vector2f vector) => new Vector2f(-vector.Y, vector.X);
        public static Vector2f Norm(this Vector2f vector) => vector / vector.Length();
        public static float Length(this Vector2f vector) => (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        public static float LengthSquared(this Vector2f vector) => vector.X * vector.X + vector.Y * vector.Y;
        public static Vector2f Perp(this Vector2f vector, Vector2f direction)
        {
            var ccw = new Vector2f(-vector.Y, vector.X);
            var cw = new Vector2f(vector.Y, -vector.X);
            return cw.Dot(direction) < ccw.Dot(direction) ? ccw : cw;
        }
    }
}
