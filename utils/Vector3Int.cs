using System;

namespace AoC2021.utils
{
    public class Vector3Int
    {
        public int x;
        public int y;
        public int z;

        public Vector3Int(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3Int operator +(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3Int operator -(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3Int operator *(Vector3Int a, int b)
        {
            return new Vector3Int(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3Int operator *(int a, Vector3Int b)
        {
            return new Vector3Int(a * b.x, a * b.y, a * b.z);
        }

        public static Vector3Int operator /(Vector3Int a, int b)
        {
            return new Vector3Int(a.x / b, a.y / b, a.z / b);
        }

        public static Vector3Int operator /(int a, Vector3Int b)
        {
            return new Vector3Int(a / b.x, a / b.y, a / b.z);
        }

        public static bool operator ==(Vector3Int a, Vector3Int b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(Vector3Int a, Vector3Int b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }

        public float Length()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        public Vector3Int Shift(int x, int y, int z)
        {
            return new Vector3Int(this.x + x, this.y + y, this.z + z);
        }

        public int ManhattanDistance(Vector3Int other)
        {
            return Math.Abs(x - other.x) + Math.Abs(y - other.y) + Math.Abs(z - other.z);
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3Int vector &&
                   x == vector.x &&
                   y == vector.y &&
                   z == vector.z;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }

        public static Vector3Int Zero()
        {
            return new Vector3Int(0, 0, 0);
        }

        public static Vector3Int One()
        {
            return new Vector3Int(1, 1, 1);
        }

        public static Vector3Int Up()
        {
            return new Vector3Int(0, 1, 0);
        }

        public static Vector3Int Down()
        {
            return new Vector3Int(0, -1, 0);
        }

        public static Vector3Int Left()
        {
            return new Vector3Int(-1, 0, 0);
        }

        public static Vector3Int Right()
        {
            return new Vector3Int(1, 0, 0);
        }

        public static Vector3Int Forward()
        {
            return new Vector3Int(0, 0, 1);
        }

        public static Vector3Int Backward()
        {
            return new Vector3Int(0, 0, -1);
        }

        public static Vector3Int operator -(Vector3Int a)
        {
            return new Vector3Int(-a.x, -a.y, -a.z);
        }
    }
}