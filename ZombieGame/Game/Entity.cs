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

        public Entity()
        {
            Tag = Tags.Undefined;
            RigidBody = new RigidBody();
            Entities.Add(this);
            GameMaster.UpdateTimer.Elapsed += UpdateTimer_Elapsed;
        }

        private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Update();
            RigidBody.Update();
        }

        private void Update()
        {
            CheckCollision();
        }

        private void CheckCollision()
        {
            int groundCol = 0;
            foreach (var e in Entities)
                if (this.RigidBody.Bounds.IntersectsWith(e.RigidBody.Bounds) && e.Tag == Tags.Ground)
                    groundCol++;
            if (groundCol > 0)
                IsGrounded = true;
            else
                IsGrounded = false;
        }

        public void Destroy()
        {

        }
    }
}
