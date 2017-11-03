using System;

namespace ZombieGame.Physics
{
    public class Vector
    {
        #region Static Properties
        public static Vector Zero { get { return new Vector(); } }
        public static Vector Up { get { return new Vector(0, 1, 0); } }
        public static Vector Down { get { return new Vector(0, -1, 0); } }
        public static Vector Left { get { return new Vector(-1, 0, 0); } }
        public static Vector Right { get { return new Vector(1, 0, 0); } }
        public static Vector EarthGravity { get { return Vector.Down * 9.807f; } }
        #endregion

        #region Operators
        public static Vector operator *(Vector v, float c)
        {
            return new Vector(v.X * c, v.Y * c, v.Z * c);
        }
        public static Vector operator *(Vector v1, Vector v2)
        {
            return new Vector(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }
        public static Vector operator /(Vector v, float c)
        {
            return new Vector(v.X / c, v.Y / c, v.Z / c);
        }
        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        public static Vector operator -(Vector v1)
        {
            return new Vector(-v1.X, -v1.Y, -v1.Z);
        }
        public static bool operator >(Vector v1, Vector v2)
        {
            return Math.Round(v1.Magnitude, 2) > Math.Round(v2.Magnitude, 2); // We will consider 0.995 and 1.005 = 1
        }
        public static bool operator <(Vector v1, Vector v2)
        {
            return Math.Round(v1.Magnitude, 2) < Math.Round(v2.Magnitude, 2); // We will consider 0.995 and 1.005 = 1
        }
        public static bool operator >=(Vector v1, Vector v2)
        {
            return Math.Round(v1.Magnitude, 2) >= Math.Round(v2.Magnitude, 2); // We will consider 0.995 and 1.005 = 1
        }
        public static bool operator <=(Vector v1, Vector v2)
        {
            return Math.Round(v1.Magnitude, 2) <= Math.Round(v2.Magnitude, 2); // We will consider 0.995 and 1.005 = 1
        }
        public static bool operator ==(Vector v1, Vector v2)
        {
            return Math.Round(v1.X, 2) == Math.Round(v2.X, 2) ^
                   Math.Round(v1.Y, 2) == Math.Round(v2.Y, 2) ^
                   Math.Round(v1.Z, 2) == Math.Round(v2.Z, 2); // We will consider 0.995 and 1.005 = 1

        }
        public static bool operator !=(Vector v1, Vector v2)
        {
            return Math.Round(v1.X, 2) != Math.Round(v2.X, 2) ||
                   Math.Round(v1.Y, 2) != Math.Round(v2.Y, 2) ||
                   Math.Round(v1.Z, 2) != Math.Round(v2.Z, 2); // We will consider 0.995 and 1.005 = 1
        }
        #endregion

        #region Properties
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
        public bool IsNormalized { get { return Math.Round(Magnitude, 2) == 1; } } // We will consider 0.995 and 1.005 = 1
        #endregion

        #region Methods
        public Vector(float x = 0, float y = 0, float z = 0)
        {
            Set(x, y, z);
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

        public void Approximate(Vector v, float offset)
        {
            if (Math.Abs(X - v.X) <= offset)
                X = v.X;
            else if (Math.Abs(X - v.X) != 0)
            {
                if (X > v.X)
                    X -= offset;
                else if (X < v.X)
                    X += offset;
            }

            if (Math.Abs(Y - v.Y) <= offset)
                Y = v.Y;
            else if (Math.Abs(Y - v.Y) != 0)
            {
                if (Y > v.Y)
                    Y -= offset;
                else if (Y < v.Y)
                    Y += offset;
            }

            if (Math.Abs(Z - v.Z) <= offset)
                Z = v.Z;
            else if (Math.Abs(Z - v.Z) != 0)
            {
                if (Z > v.Z)
                    Z -= offset;
                else if (Z < v.Z)
                    Z += offset;
            }
        }
        #endregion
    }
}
