using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game;

namespace ZombieGame.Physics
{
    public class RigidBody
    {
        protected List<Vector> AppliedForces { get; set; }
        public Bounds Bounds { get; set; }
        public float Mass { get; set; }
        public float SpeedMultiplier { get; set; }
        public float MaxSpeedX { get; set; }
        public float MaxSpeedY { get; set; }
        public Vector Position { get { return Bounds.Position; } set { Bounds.Position = value; } }
        public Vector Size { get { return Bounds.Size; } }
        public Vector[] Forces { get { return AppliedForces.ToArray(); } }
        public Vector Force {
            get
            {
                Vector sum = Vector.Zero;
                foreach (var v in Forces)
                    sum += v;
                return sum;
            }
        }
        public Vector Velocity { get; protected set; }
        public Vector Acceleration { get; protected set; }
        public float Rotation { get; set; }
        public float Drag { get; set; }
        public bool UseRotation { get; set; }
        public bool UseGravity { get; set; }
        public bool CanMoveLeft { get; set; }
        public bool CanMoveRight { get; set; }
        public bool CanMoveUp { get; set; }
        public bool CanMoveDown { get; set; }

        public RigidBody()
        {
            AppliedForces = new List<Vector>();
            Mass = 1;
            Bounds = new Bounds(Vector.Zero, new Vector(50, 50));
            Acceleration = Vector.Zero;
            Velocity = Vector.Zero;
            UseGravity = true;
            Drag = 1f;
            MaxSpeedX = 200;
            MaxSpeedY = 1000;
        }

        public void AddForce(Vector f)
        {
            AppliedForces.Add(f);
        }

        public void AddVelocity(Vector v)
        {
            if (Math.Abs(Velocity.X + v.X) < MaxSpeedX)
                Velocity.X += v.X;
            if (Math.Abs(Velocity.Y + v.Y) < MaxSpeedY)
                Velocity.Y += v.Y;
        }

        public void Update()
        {
            for (int i = 0; i < AppliedForces.Count; i++)
            {
                AppliedForces[i].Approximate(Vector.Zero, Drag);
                if (AppliedForces[i].Magnitude == 0)
                    AppliedForces.RemoveAt(i);
            }

            Acceleration = Velocity / Time.Delta;
            Position += Velocity * Time.Delta;

            if (Position.Y < 0)
                Position.Y = 0;

            Velocity.Approximate(Vector.Zero, Drag);
        }
    }
}
