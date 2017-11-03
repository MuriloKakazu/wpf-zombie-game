using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;
using ZombieGame.Physics.Events;

namespace ZombieGame.Game
{
    public class Entity
    {
        public static List<Entity> Entities = new List<Entity>();

        public string Name { get; set; }
        public Tags Tag { get; set; }
        public RigidBody RigidBody { get; set; }
        public bool IsGrounded { get; protected set; }

        public Entity(string name)
        {
            Name = name;
            Tag = Tags.Undefined;
            RigidBody = new RigidBody();
            Entities.Add(this);
            GameMaster.UpdateTimer.Elapsed += UpdateTimer_Elapsed;
        }

        public delegate void CollisionHandler(object sender, CollisionEventArgs e);
        public event CollisionHandler CollisionEnter;
        public event CollisionHandler CollisionLeave;

        protected virtual void OnCollisionEnter(CollisionEventArgs e)
        {
            Console.WriteLine("Collision with {0}", e.Collider.Name);
            CollisionEnter?.Invoke(this, e);
        }

        protected virtual void OnCollisionLeave(CollisionEventArgs e)
        {
            CollisionLeave?.Invoke(this, e);
        }

        protected virtual void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Update();
            RigidBody.Update();
        }

        protected void Update()
        {
            CheckCollision();
        }

        protected void CheckCollision()
        {
            foreach (var e in Entities)
            {
                Vector dir;
                if (this.RigidBody.Bounds.IntersectsWith(e.RigidBody.Bounds, out dir))
                    CollisionEnter?.Invoke(this, new CollisionEventArgs(e, dir));
            }
        }

        public void Destroy()
        {

        }
    }
}
