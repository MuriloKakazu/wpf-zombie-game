using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;
using ZombieGame.Physics.Events;
using static ZombieGame.Game.GameMaster;

namespace ZombieGame.Game
{
    public abstract class Enemy : Character
    {
        /// <summary>
        /// Retorna o tipo de inimigo
        /// </summary>
        public EnemyTypes Type { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="type">O tipo de inimigo</param>
        public Enemy(EnemyTypes type) : base(type.ToString(), Tags.Enemy)
        {
            Type = type;
            RigidBody.UseRotation = true;
            SetMoney();
            SetHealth();
            SetExperience();
        }

        protected override void Update()
        {
            base.Update();

            Chase(GetNearestPlayer(), 250);
        }

        protected override void OnCollisionEnter(object sender, CollisionEventArgs e)
        {
            if (e.Collider.Tag == Tags.Player)
            {
                var colliderChar = Character.FromHashCode(e.Collider.Hash);
                if (colliderChar != null)
                {
                    colliderChar.Damage(10);
                    colliderChar.RigidBody.AddForce(e.CollisionDirection.Opposite.Normalized * 2000);
                    RigidBody.AddForce(e.CollisionDirection.Normalized * 2000);
                    //RigidBody.AddVelocity(e.CollisionDirection.Normalized * e.Collider.RigidBody.Velocity.Magnitude);
                }
            }
        }

        protected override void OnCollisionStay(object sender, CollisionEventArgs e)
        {

        }

        protected override void OnCollisionLeave(object sender, CollisionEventArgs e)
        {
            
        }

        /// <summary>
        /// O método para definir quando de dinheiro o inimigo dropará.
        /// <para>Se trata de um valor definido entre um range</para>
        /// </summary>
        private void SetMoney()
        {
            int maxMoney = default(int), minMoney = default(int), avgLvls;
            if (Player2.IsPlaying)
                avgLvls = (Player1.Character.Level + Player2.Character.Level) / 2;
            else
                avgLvls = Player1.Character.Level;

            switch (Type)
            {
                case EnemyTypes.Zombie:
                    maxMoney = 20 * avgLvls + 10 * Level;
                    minMoney = 5 * avgLvls + 10 * Level;
                    break;
                case EnemyTypes.Runner:
                    maxMoney = 50 * avgLvls + 20 * Level;
                    minMoney = 20 * avgLvls + 15 * Level;
                    break;
                case EnemyTypes.Tanker:
                    maxMoney = 100 * avgLvls + 30 * Level;
                    minMoney = 40 * avgLvls + 20 * Level;
                    break;
                case EnemyTypes.Boss:
                    maxMoney = 1000 * avgLvls + 300 * Level;
                    minMoney = 400 * avgLvls + 200 * Level;
                    break;
            }

            Money = new Random().Next(minMoney, maxMoney);
        }

        /// <summary>
        /// O método para definir a vida do inimigo.
        /// </summary>
        private void SetHealth()
        {
            switch (Type)
            {
                case EnemyTypes.Zombie:
                    Health = 50 * Level;
                    break;
                case EnemyTypes.Runner:
                    Health = 30 * Level;
                    break;
                case EnemyTypes.Tanker:
                    Health = 100 * Level + 100;
                    break;
                case EnemyTypes.Boss:
                    Health = 400 * Level + 200;
                    break;
            }
        }

        /// <summary>
        /// O método para definir a experiência que o inimigo dará ao morrer.
        /// </summary>
        private void SetExperience()
        {
            switch (Type)
            {
                case EnemyTypes.Zombie:
                    Experience = 25 * Level;
                    break;
                case EnemyTypes.Runner:
                    Experience = 35 * Level;
                    break;
                case EnemyTypes.Tanker:
                    Experience = 40 * Level;
                    break;
                case EnemyTypes.Boss:
                    Experience = 20 * Level + 10 * Level * Level;
                    break;
            }
        }

    }
}
