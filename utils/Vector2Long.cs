using System;

namespace AoC2021.utils
{
    public struct Vector2Long
    {
        public long x;
        public long y;

        public Vector2Long(long x, long y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2Long operator +(Vector2Long a, Vector2Long b)
        {
            return new Vector2Long(a.x + b.x, a.y + b.y);
        }

        public static Vector2Long operator -(Vector2Long a, Vector2Long b)
        {
            return new Vector2Long(a.x - b.x, a.y - b.y);
        }

        public static Vector2Long operator *(Vector2Long a, long b)
        {
            return new Vector2Long(a.x * b, a.y * b);
        }

        public static Vector2Long operator *(long a, Vector2Long b)
        {
            return new Vector2Long(a * b.x, a * b.y);
        }

        public static Vector2Long operator /(Vector2Long a, long b)
        {
            return new Vector2Long(a.x / b, a.y / b);
        }

        public static Vector2Long operator /(long a, Vector2Long b)
        {
            return new Vector2Long(a / b.x, a / b.y);
        }

        public static bool operator ==(Vector2Long a, Vector2Long b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Vector2Long a, Vector2Long b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2Long vector &&
                   x == vector.x &&
                   y == vector.y;
        }

        public bool Equals(Vector2Long other)
        {
            return x == other.x && y == other.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }
    }
}