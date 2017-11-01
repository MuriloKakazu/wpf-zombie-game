using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGame.Physics
{
    public struct RigidBody
    {
        public float Mass { get; set; }
        public float Speed { get; set; }
        public Vector Position { get; set; }
        public Vector Velocity { get; set; }
        public Vector Gravity { get; set; }
        public float Rotation { get; set; }
        public bool UseRotation { get; set; }
        public bool UseGravity { get; set; }
    }
}
