using System;

namespace ZombieGame.Physics
{
    public struct Vector
    {
        public static Vector Zero { get { return new Vector(); } }
        public static Vector Up { get { return new Vector(0, 1, 0); } }
        public static Vector Down { get { return new Vector(0, -1, 0); } }
        public static Vector Left { get { return new Vector(-1, 0, 0); } }
        public static Vector Right { get { return new Vector(1, 0, 0); } }

        public static float Distance(Vector v1, Vector v2)
        {
            return (float)Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + 
                                    Math.Pow(v1.Y - v2.Y, 2) + 
                                    Math.Pow(v1.Z - v2.Z, 2));
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public float Magnitude
        {
            get
            {
                return (float)Math.Sqrt(Math.Pow(X, 2) + 
                                        Math.Pow(Y, 2) + 
                                        Math.Pow(Z, 2));
            }
        }
        public Vector Normalized
        {
            get
            {
                var mag = Magnitude;

                if (Magnitude == 0)
                    return Vector.Zero;
                else
                    return new Vector(X / mag, Y / mag, Z / mag);
            }
        }
        public bool IsNormalized { get { return Magnitude == 1; } }

        public Vector(float x = 0, float y = 0, float z = 0) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Set(float x, float y, float z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Normalize()
        {
            var normalized = Normalized;
            Set(normalized.X, normalized.Y, normalized.Z);
        }
    }
}
