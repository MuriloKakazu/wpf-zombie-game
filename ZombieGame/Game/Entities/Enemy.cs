using System;
using System.Collections.Generic;
using ZombieGame.Audio;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Interfaces;
using ZombieGame.Physics;
using ZombieGame.Physics.Events;

namespace ZombieGame.Game.Entities
{
    public class Enemy : Character
    {
        #region Properties
        /// <summary>
        /// Lista estática de todos os inimigos ativos
        /// </summary>
        private static List<Enemy> Enemies = new List<Enemy>();

        /// <summary>
        /// Retorna o tipo de inimigo
        /// </summary>
        public virtual EnemyTypes Type { get; protected set; }
        /// <summary>
        /// Retorna a pontuação que o inimigo dará ao morrer
        /// </summary>
        public virtual float DeathPoints { get; protected set; }
        public virtual float MoneyDrop { get; protected set; }
        public virtual string DeathSFXKey { get; set; }
        public virtual string HitSFXKey { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Retorna todos os inimigos ativos
        /// </summary>
        /// <returns></returns>
        public new static Enemy[] GetAllActive()
        {
            return Enemies.ToArray();
        }

        public static Enemy Mount(ISerializableEnemy source)
        {
            Enemy e = new Enemy()
            {
                Name = source.Name,
                Health = source.Health * GameMaster.DifficultyBonus,
                DeathPoints = source.DeathPoints,
                Hash = Guid.NewGuid(),
                MoneyDrop = source.MoneyDrop,
                Type = source.Type,
                DeathSFXKey = source.DeathSFXKey,
                HitSFXKey = source.HitSFXKey
            };
            e.RigidBody.SetMass(source.Mass);
            e.RigidBody.SpeedMultiplier = source.SpeedMultiplier + 0.15f * GameMaster.DifficultyBonus;
            e.RigidBody.Resize(source.Size);
            e.Sprite.Uri = IO.GlobalPaths.EnemySprites + source.SpriteFileName;
            e.LoadSprite(e.Sprite.Uri);
            return e;
        }

        public Enemy() : this(EnemyTypes.Undefined)
        {

        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="type">O tipo de inimigo</param>
        public Enemy(EnemyTypes type) : base(type.ToString(), Tags.Enemy)
        {
            Type = type;
            RigidBody.UseRotation = true;
            Enemies.Add(this);
            Time.LowPriorityTimer.Elapsed += LowPriorityTimer_Elapsed;
        }

        private void LowPriorityTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!IsStunned)
                Chase(GetNearestPlayer(), 10 * RigidBody.SpeedMultiplier);
            else
                RigidBody.SetVelocity(Vector.Zero);
        }

        /// <summary>
        /// Faz com que o inimigo persiga um alvo
        /// </summary>
        /// <param name="target">Alvo</param>
        /// <param name="speedMagnitude">Magnitude da velocidade de perseguição</param>
        protected virtual void Chase(Entity target, float speedMagnitude)
        {
            if (RigidBody.Acceleration.Magnitude > 0)
                RigidBody.Acceleration.Approximate(Vector.Zero, 10);
            RigidBody.SetForce(RigidBody.Force / 1.1f);
            RigidBody.SetVelocity(RigidBody.CenterPoint.PointedAt(target.RigidBody.CenterPoint).Opposite.Normalized * speedMagnitude);
            RigidBody.PointAt(RigidBody.CenterPoint.PointedAt(target.RigidBody.CenterPoint).Opposite);
        }

        public override void Damage(Entity damager, float quantity)
        {
            SoundPlayer.Instance.Play(SoundTrack.GetAnyWithKey(HitSFXKey));
            base.Damage(damager, quantity);
        }

        protected override void Kill()
        {
            SoundPlayer.Instance.Play(SoundTrack.GetAnyWithKey(DeathSFXKey));
            GameMaster.Score += DeathPoints * GameMaster.DifficultyBonus;
            GameMaster.Money += MoneyDrop * GameMaster.DifficultyBonus;
            base.Kill();
        }

        ///// <summary>
        ///// Define quanto de dinheiro o inimigo dará ao morrer.
        ///// <para>Se trata de um valor definido entre um range</para>
        ///// </summary>
        //protected virtual void SetMoney()
        //{
        //    int maxMoney = default(int), minMoney = default(int), avgLvls = 0;

        //    foreach (var p in GameMaster.Players)
        //        avgLvls += p.Character.Level;

        //    avgLvls /= GameMaster.Players.Length;

        //    switch (Type)
        //    {
        //        case EnemyTypes.Walker:
        //            maxMoney = 20 * avgLvls + 10 * Level;
        //            minMoney = 5 * avgLvls + 10 * Level;
        //            break;
        //        case EnemyTypes.Boss:
        //            maxMoney = 1000 * avgLvls + 300 * Level;
        //            minMoney = 400 * avgLvls + 200 * Level;
        //            break;
        //    }

        //    Money = new Random().Next(minMoney, maxMoney);
        //}

        ///// <summary>
        ///// Define a vida do inimigo.
        ///// </summary>
        //protected virtual void SetHealth()
        //{
        //    switch (Type)
        //    {
        //        case EnemyTypes.Walker:
        //            Health = 50 * Level;
        //            break;
        //        case EnemyTypes.Boss:
        //            Health = 400 * Level + 200;
        //            break;
        //    }
        //}

        ///// <summary>
        ///// Define a experiência que o inimigo dará ao morrer.
        ///// </summary>
        //protected virtual void SetExperience()
        //{
        //    switch (Type)
        //    {
        //        case EnemyTypes.Walker:
        //            Experience = 25 * Level;
        //            break;
        //        case EnemyTypes.Boss:
        //            Experience = 20 * Level + 10 * Level * Level;
        //            break;
        //    }
        //}

        /// <summary>
        /// Destrói o objeto, liberando memória
        /// </summary>
        public override void Destroy()
        {
            Enemies.Remove(this);
            base.Destroy();
        }

        /// <summary>
        /// Evento a ser disparado quando a entidade entra em colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected override void OnCollisionEnter(object sender, CollisionEventArgs e)
        {
            if (e.Collider.IsPlayer)
            {
                var colliderChar = Character.FromHashCode(e.Collider.Hash);
                if (colliderChar != null)
                    colliderChar.RigidBody.AddForce(e.CollisionDirection.Opposite.Normalized * 1000);
            }
        }

        /// <summary>
        /// Evento a ser disparado quando a entidade mantém-se em colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected override void OnCollisionStay(object sender, CollisionEventArgs e)
        {
            //base.OnCollisionStay(sender, e);
            if (e.Collider.IsCamera)
                return;

            if (e.Collider.Tag != Tags.Projectile && e.Collider.Tag != Tags.Wall)
            {
                var colliderChar = Character.FromHashCode(e.Collider.Hash);
                if (colliderChar != null && colliderChar.IsPlayer)
                    colliderChar.Damage(damager: this, quantity: 1);

                //RigidBody.SetVelocity(e.CollisionDirection.Normalized * e.Collider.RigidBody.Velocity.Magnitude);
                //RigidBody.AddForce(e.CollisionDirection.Normalized * 5);
            }
            if (e.Collider.IsEnemy)
            {
                RigidBody.AddForce(e.CollisionDirection.Normalized * 5);
            }
        }

        /// <summary>
        /// Evento a ser disparado quando a entidade sai de uma colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected override void OnCollisionLeave(object sender, CollisionEventArgs e)
        {
            //if (e.Collider.IsEnemy)
            //    RigidBody.SetForce(Vector.Zero);
        }
        #endregion
    }
}
