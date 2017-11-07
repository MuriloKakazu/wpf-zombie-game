using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;
using ZombieGame.Physics.Events;

namespace ZombieGame.Game.Prefabs.OtherEntities
{
    public sealed class Camera : Entity
    {
        public Camera() : base("Camera", Tags.Camera)
        {
            RigidBody.SetPosition(Vector.Zero);
            RigidBody.Resize(Vector.WindowSize);
            RigidBody.Freeze();
            RigidBody.UseRotation = false;
            RigidBody.IgnoreCollisions = false;
        }

        protected override void OnCollisionLeave(object sender, CollisionEventArgs e)
        {
            if (e.Collider.Tag != Tags.Wall && !e.Collider.IsPlayer)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Destroying object: {0}", e.Collider.Name);
                Console.ResetColor();
                e.Collider.Destroy();
            }
            else if (e.Collider.IsPlayer)
                e.Collider.RigidBody.SetPosition(new Vector(500, -500));
        }

        protected override void OnCollisionEnter(object sender, CollisionEventArgs e)
        {
            
        }

        protected override void OnCollisionStay(object sender, CollisionEventArgs e)
        {
            
        }
    }
}
