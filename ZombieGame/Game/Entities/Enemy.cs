using System;
using System.Collections.Generic;
using ZombieGame.Audio;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Interfaces;
using ZombieGame.Game.Prefabs.Entities;
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
        public virtual EnemyType Type { get; protected set; }
        /// <summary>
        /// Retorna a pontuação que o inimigo dará ao morrer
        /// </summary>
        public virtual float DeathPoints { get; protected set; }
        /// <summary>
        /// Quantia de dinheiro que o inimigo dará ao morrer
        /// </summary>
        public virtual float MoneyDrop { get; protected set; }
        /// <summary>
        /// Chave de efeito sonoro de morte do inimigo
        /// </summary>
        public virtual string DeathSFXKey { get; set; }
        /// <summary>
        /// Chave de efeito sonoro de dano do inimigo
        /// </summary>
        public virtual string HitSFXKey { get; set; }
        /// <summary>
        /// Retorna se o inimigo explode ao morrer
        /// </summary>
        public virtual bool ExplodesOnDeath { get; set; }
        /// <summary>
        /// Raio de explosão do inimigo
        /// </summary>
        public virtual float ExplosionRadius { get; set; }
        /// <summary>
        /// Chave do efeito sonoro de explosão
        /// </summary>
        public virtual string ExplosionSFXKey { get; protected set; }
        /// <summary>
        /// Retorna se o inimigo está explodindo
        /// </summary>
        public bool IsExploding { get; protected set; }
        /// <summary>
        /// Dano do inimigo ao colidir com o jogador
        /// </summary>
        public virtual float HitDamage { get; protected set; }
        #endregion

        #region Methods
        /// <summary>
        /// Retorna todos os inimigos ativos
        /// </summary>
        /// <returns></returns>
        public new static Enemy[] Instances
        {
            get { return Enemies.ToArray(); }
        }

        /// <summary>
        /// Retorna uma instância de inimigo a partir de uma interface
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Enemy Mount(ISerializableEnemy source)
        {
            Enemy e = new Enemy()
            {
                Name = source.Name,
                MaxHealth = source.Health * GameMaster.DifficultyBonus,
                Health = source.Health * GameMaster.DifficultyBonus,
                DeathPoints = source.DeathPoints,
                Hash = Guid.NewGuid(),
                MoneyDrop = source.MoneyDrop,
                Type = source.Type,
                DeathSFXKey = source.DeathSFXKey,
                HitSFXKey = source.HitSFXKey,
                ExplodesOnDeath = source.ExplodesOnDeath,
                ExplosionRadius = source.ExplosionRadius,
                ExplosionSFXKey = source.ExplosionSFXKey,
                HitDamage = source.HitDamage * GameMaster.DifficultyBonus,
            };
            e.RigidBody.SetMass(source.Mass);
            e.RigidBody.SpeedMultiplier = source.SpeedMultiplier + 0.25f * GameMaster.DifficultyBonus;
            e.RigidBody.Resize(source.Size);
            e.Sprite.Uri = IO.GlobalPaths.EnemySprites + source.SpriteFileName;
            e.LoadSprite(e.Sprite.Uri);
            return e;
        }

        /// <summary>
        /// ctor
        /// </summary>
        public Enemy() : this(EnemyType.Undefined)
        {

        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="type">O tipo de inimigo</param>
        public Enemy(EnemyType type) : base(type.ToString(), Tag.Enemy)
        {
            Type = type;
            RigidBody.UseRotation = true;
            Enemies.Add(this);
        }

        /// <summary>
        /// Método a ser chamado pelo timer de baixa frequência
        /// </summary>
        protected override void Update()
        {
            if (!IsStunned)
                Chase(GetNearestPlayer(), 10 * RigidBody.SpeedMultiplier);
            else
                RigidBody.SetVelocity(Vector.Zero);
            base.Update();
        }

        /// <summary>
        /// Faz com que o inimigo persiga um alvo
        /// </summary>
        /// <param name="target">Alvo</param>
        /// <param name="speedMagnitude">Magnitude da velocidade de perseguição</param>
        protected virtual void Chase(Entity target, float speedMagnitude)
        {
            if (target == null)
                return;

            if (RigidBody.Acceleration.Magnitude > 0)
                RigidBody.Acceleration.Approximate(Vector.Zero, 10);
            RigidBody.SetForce(RigidBody.Force / 1.1f);
            RigidBody.SetVelocity(RigidBody.CenterPoint.PointedAt(target.RigidBody.CenterPoint).Opposite.Normalized * speedMagnitude);
            RigidBody.PointAt(RigidBody.CenterPoint.PointedAt(target.RigidBody.CenterPoint).Opposite);
        }

        /// <summary>
        /// Aplica um ano ao inimigo
        /// </summary>
        /// <param name="damager"></param>
        /// <param name="quantity"></param>
        public override void Damage(Entity damager, float quantity)
        {
            SoundPlayer.Instance.Play(SoundTrack.GetAnyWithKey(HitSFXKey));
            base.Damage(damager, quantity);
            GameMaster.Score += quantity;
        }

        /// <summary>
        /// Mata o inimigo
        /// </summary>
        /// <param name="silent"></param>
        protected override void Kill(bool silent = false)
        {
            if (!silent)
            {
                SoundPlayer.Instance.Play(SoundTrack.GetAnyWithKey(DeathSFXKey));
                GameMaster.Score += DeathPoints * GameMaster.DifficultyBonus;
                GameMaster.Money += MoneyDrop * GameMaster.DifficultyBonus;
                if (ExplodesOnDeath && !IsExploding)
                    Explode();
            }
            base.Kill();
        }

        /// <summary>
        /// Explode o projétil
        /// </summary>
        /// <param name="pos">Posição da explosão</param>
        public virtual void Explode(bool preventCascadeEffect = false)
        {
            if (!preventCascadeEffect)
                SoundPlayer.Instance.Play(SoundTrack.GetAnyWithKey(ExplosionSFXKey));

            var pos = RigidBody.CenterPoint;
            IsExploding = true;

            Explosion.Create(pos, ExplosionRadius * 2);

            if (!preventCascadeEffect)
            {
                var targets = Character.GetNearbyCharacters(pos, ExplosionRadius / 2, 5);
                if (targets != null)
                {
                    foreach (var t in targets)
                    {
                        t.Damage(damager: this, quantity: HitDamage);

                        t.RigidBody.PointAt(pos - t.RigidBody.CenterPoint);
                        //t.RigidBody.AddForce(t.RigidBody.CenterPoint.PointedAt(RigidBody.CenterPoint).Normalized * (ExplosionRadius * 5));
                        if (t.IsEnemy)
                        {
                            var e = (Enemy)t;
                            if (e.ExplodesOnDeath && !e.IsExploding) e.Explode(preventCascadeEffect: true);
                            else e.Damage(damager: this, quantity: HitDamage * 5);
                        }
                    }
                }
                //var nearbyProjectiles = Projectile.GetNearbyProjectiles(ExplosionRadius / 2, 3);
                //if (nearbyProjectiles != null)
                //{
                //    foreach (var p in nearbyProjectiles)
                //        if (p.IsExplosive && !p.IsExploding)
                //            p.Explode(preventCascadeEffect: true);
                //}
            }

            MarkAsNoLongerNeeded();
        }

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
                {
                    //colliderChar.RigidBody.AddForce(e.CollisionDirection.Opposite.Normalized * 1000);
                    if (ExplodesOnDeath && !IsExploding)
                        Kill();
                    else if (!IsExploding)
                        colliderChar.Damage(damager: this, quantity: HitDamage);
                }
            }
        }

        /// <summary>
        /// Evento a ser disparado quando a entidade mantém-se em colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected override void OnCollisionStay(object sender, CollisionEventArgs e)
        {
            if (e.Collider.IsCamera)
                return;

            if (e.Collider.Tag != Tag.Projectile && e.Collider.Tag != Tag.Wall)
            {
                RigidBody.AddForce(e.CollisionDirection.Normalized * RigidBody.Momentum.Magnitude);
                if (e.Collider.IsPlayer)
                {
                    var colliderChar = Character.FromHashCode(e.Collider.Hash);
                    if (colliderChar != null)
                    {
                        colliderChar.Damage(damager: this, quantity: HitDamage / 10);
                    }
                }
            }
            if (e.Collider.IsEnemy)
            {
                //RigidBody.AddForce(e.CollisionDirection.Normalized * 5);
            }
        }

        /// <summary>
        /// Evento a ser disparado quando a entidade sai de uma colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected override void OnCollisionLeave(object sender, CollisionEventArgs e)
        {

        }
        #endregion
    }
}
