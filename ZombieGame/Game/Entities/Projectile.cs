﻿using System;
using System.Collections.Generic;
using System.Linq;
using ZombieGame.Audio;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Interfaces;
using ZombieGame.Physics;
using ZombieGame.Physics.Events;

namespace ZombieGame.Game.Entities
{
    public class Projectile : Entity
    {
        #region Properties
        /// <summary>
        /// Lista de todos os projéteis ativos
        /// </summary>
        private static List<Projectile> Projectiles = new List<Projectile>();

        /// <summary>
        /// Retorna se o projétil atordoa com o impacto
        /// </summary>
        public virtual bool IsStunner { get; protected set; }
        /// <summary>
        /// Retorna se o projétil explode com o impacto
        /// </summary>
        public virtual bool IsExplosive { get; protected set; }
        protected virtual bool IsExploding { get; set; }
        protected virtual float TravelDistance { get; set; }
        public virtual float ExplosionRadius { get; protected set; }
        /// <summary>
        /// Dano de impacto do projétil
        /// </summary>
        public virtual float HitDamage { get; set; }
        /// <summary>
        /// Entidade que invocou o projétil
        /// </summary>
        public virtual Character Owner { get; set; }
        /// <summary>
        /// Tipo do projétil
        /// </summary>
        public virtual ProjectileTypes Type { get; protected set; }
        /// <summary>
        /// O valor primitivo da velocidade
        /// </summary>
        private float baseSpeedMagnitude = 40;
        /// <summary>
        /// Módulo da velocidade do projétil
        /// </summary>
        public virtual float SpeedMagnitude
        {
            get { return baseSpeedMagnitude; }
            set
            {
                if (value < 25)
                    baseSpeedMagnitude = 25;
                else if (value > 150)
                    baseSpeedMagnitude = 150;
                else
                    baseSpeedMagnitude = value;
            }
        }
        /// <summary>
        /// Coeficiente de ação do projétil sobre um corpo
        /// </summary>
        public virtual float KnockbackMagnitude { get; protected set; }
        /// <summary>
        /// Tempo em milisegundos que o projétil atordoa seu alvo
        /// </summary>
        public virtual float StunTimeMs { get; protected set; }
        public virtual string ImpactSFXKey { get; protected set; }
        public virtual string ExplosionSFXKey { get; protected set; }
        public virtual Vector LaunchPosition { get; protected set; }
        #endregion

        #region Methods
        /// <summary>
        /// Retorna todos os projéteis ativos
        /// </summary>
        /// <returns></returns>
        public new static Projectile[] GetAllActive()
        {
            return Projectiles.ToArray();
        }

        /// <summary>
        /// Retorna uma instância do objeto Projectile
        /// </summary>
        /// <param name="hash">Código de identificação do objeto</param>
        /// <returns>Projectile</returns>
        public static Projectile FromHashCode(Guid hash)
        {
            try
            {
                foreach (Projectile p in Projectiles.ToArray())
                    if (p != null && p.Hash == hash)
                        return p;
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Monta uma instância de projétil
        /// </summary>
        /// <param name="source">Objeto serializável</param>
        /// <returns>Projectile</returns>
        public static Projectile Mount(ISerializableProjectile source)
        {
            var p = new Projectile()
            {
                Hash = Guid.NewGuid(),
                HitDamage = source.HitDamage,
                IsExplosive = source.IsExplosive,
                IsStunner = source.IsStunner,
                KnockbackMagnitude = source.KnockbackMagnitude,
                Name = source.Name,
                SpeedMagnitude = source.SpeedMagnitude,
                StunTimeMs = source.StunTimeMs,
                Tag = Tags.Projectile,
                Type = source.Type,
                ExplosionSFXKey = source.ExplosionSFXKey,
                ImpactSFXKey = source.ImpactSFXKey,
                TravelDistance = source.TravelDistance,
                ExplosionRadius = source.ExplosionRadius,
            };
            p.RigidBody.Resize(source.Size);
            p.Sprite.Uri = IO.GlobalPaths.ProjectileSprites + source.SpriteFileName;

            return p;
        }

        /// <summary>
        /// ctor
        /// </summary>
        protected Projectile() : this(ProjectileTypes.Undefined) { }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="type">Tipo de projétil</param>
        public Projectile(ProjectileTypes type) : base(type.ToString() + "Projectile", Tags.Projectile)
        {
            LaunchPosition = RigidBody.Position;
            SetZIndex(ZIndexes.Projectile);
            Projectiles.Add(this);
        }

        /// <summary>
        /// Lança o projétil em uma direção
        /// </summary>
        /// <param name="dir">Direção de lançamento</param>
        public virtual void Launch(Vector dir)
        {
            RigidBody.SetPosition(new Vector(Owner.RigidBody.CenterPoint.X - RigidBody.Size.X / 2, Owner.RigidBody.CenterPoint.Y + RigidBody.Size.Y / 2));
            LaunchPosition = RigidBody.Position;
            RigidBody.UseRotation = true;
            RigidBody.PointAt(dir);
            RigidBody.SetVelocity(dir.Normalized * SpeedMagnitude);
            Show();
        }

        /// <summary>
        /// Explode o projétil
        /// </summary>
        /// <param name="pos">Posição da explosão</param>
        protected virtual void Explode()
        {
            SoundPlayer.Instance.Play(SoundTrack.GetAnyWithKey(ExplosionSFXKey));

            var pos = RigidBody.CenterPoint;
            IsExploding = true;

            var targets = Character.GetNearbyCharacters(pos, ExplosionRadius, 4);
            var nearbyProjectiles = GetNearbyProjectiles(ExplosionRadius / 2);
            GameMaster.CurrentScene.SpawnExplosionAt(pos, ExplosionRadius * 2, applyPhysics: false); // We will override the physics

            if (targets != null)
            {
                foreach (var t in targets)
                {
                    if (!t.IsPlayer)
                    {
                        t.Stun(StunTimeMs);
                        t.Damage(damager: this.Owner, quantity: HitDamage);
                    }
                    else
                    {
                        t.Stun(StunTimeMs / 3);
                        t.Damage(damager: this.Owner, quantity: HitDamage / 10);
                    }

                    t.RigidBody.PointAt(pos - t.RigidBody.CenterPoint);
                    t.RigidBody.AddForce(t.RigidBody.CenterPoint.PointedAt(RigidBody.CenterPoint).Normalized * (ExplosionRadius * 5));

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0} exploded on {1}", Name, t.Name);
                    Console.ResetColor();
                }
            }
            if (nearbyProjectiles != null)
            {
                foreach (var p in nearbyProjectiles)
                    if (p.IsExplosive && !p.IsExploding)
                        p.Explode();
            }

            Destroy();
        }

        /// <summary>
        /// Destrói o projétil para liberar espaço na memória
        /// </summary>
        public override void Destroy()
        {
            base.Destroy();
            Projectiles.Remove(this);
        }

        /// <summary>
        /// Retorna um clone profundo da instância atual
        /// </summary>
        /// <returns>Weapon</returns>
        public Projectile Clone()
        {
            var copy = new Projectile()
            {
                Hash = Guid.NewGuid(),
                HitDamage = HitDamage,
                IsExplosive = IsExplosive,
                Collisions = Collisions,
                IsStunner = IsStunner,
                StunTimeMs = StunTimeMs,
                KnockbackMagnitude = KnockbackMagnitude,
                ExplosionSFXKey = ExplosionSFXKey,
                ImpactSFXKey = ImpactSFXKey,
                Name = Name,
                Owner = Owner,
                SpeedMagnitude = SpeedMagnitude,
                Sprite = Sprite,
                Tag = Tag,
                Type = Type,
                Visible = Visible,
                TravelDistance = TravelDistance,
                ExplosionRadius = ExplosionRadius,
                VisualControl = new Controls.VisualControl(),
            };
            copy.RigidBody.Resize(RigidBody.Size);
            if (copy.Visible)
            {
                System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().AddVisualComponent(VisualControl);
                UpdateVisualControl();
            }
            return copy;
        }

        protected override void Update()
        {
            CheckTravelDistance();
            base.Update();
        }

        protected virtual void CheckTravelDistance()
        {
            if (HasExceededTravelDistance)
            {
                if (IsExplosive && !IsExploding)
                    Explode();
                else
                    Destroy();
            }
        }

        protected virtual bool HasExceededTravelDistance
        {
            get
            {
                return RigidBody.Position.DistanceBetween(LaunchPosition) >= TravelDistance;
            }
        }

        /// <summary>
        /// Retorna um conjunto de projéteis no raio especificado
        /// </summary>
        /// <param name="radius">Raio de procura</param>
        /// <returns>Entity(Array)</returns>
        public virtual Projectile[] GetNearbyProjectiles(float radius)
        {
            try
            {
                List<Projectile> projectiles = new List<Projectile>();
                foreach (var e in Projectiles.ToArray())
                    if ((e.RigidBody.CenterPoint - RigidBody.CenterPoint).Magnitude <= radius && e.Hash != Hash)
                        projectiles.Add(e);
                return projectiles.ToArray();
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Evento a ser disparado quando o projétil entra em colisão com outro objeto
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações da colisão</param>
        protected override void OnCollisionEnter(object sender, CollisionEventArgs e)
        {
            if (e.Collider.IsCamera || e.Collider.IsWall)
                return;

            //if (e.Collider != Owner)
            //{
            //    Console.ForegroundColor = ConsoleColor.Cyan;
            //    Console.WriteLine("{0} collided with {1}", Name, e.Collider.Name);
            //    Console.ResetColor();
            //}

            var colliderChar = Character.FromHashCode(e.Collider.Hash);

            if (e.Collider != Owner)
            {
                if (colliderChar != null)
                {
                    if (!colliderChar.IsPlayer)
                    {
                        colliderChar.Damage(damager: this.Owner, quantity: HitDamage);
                        if (IsStunner)
                            colliderChar.Stun(StunTimeMs);
                    }
                    else
                    {
                        colliderChar.Damage(damager: this.Owner, quantity: HitDamage / 10);
                        if (IsStunner)
                            colliderChar.Stun(StunTimeMs / 3);
                    }
                    //colliderChar.RigidBody.AddVelocity(e.CollisionDirection.Opposite.Normalized * KnockbackMagnitude);
                    colliderChar.RigidBody.AddForce(e.CollisionDirection.Opposite.Normalized * KnockbackMagnitude * 10);
                    if (IsExplosive && !IsExploding)
                        Explode();

                    SoundPlayer.Instance.Play(SoundTrack.GetAnyWithKey(ImpactSFXKey));

                    Destroy();
                }
                else if (e.Collider.Tag == Tags.Projectile)
                {
                    var p = Projectile.FromHashCode(e.Collider.Hash);
                    if (p != null && p.Owner != Owner)
                    {
                        if (p.IsExplosive && !p.IsExploding)
                            p.Explode();
                        if (IsExplosive && !IsExploding)
                            Explode();

                        SoundPlayer.Instance.Play(SoundTrack.GetAnyWithKey(ImpactSFXKey));

                        p.Destroy();
                        Destroy();
                    }
                }
                else
                {
                    if (IsExplosive && !IsExploding)
                        Explode();

                    SoundPlayer.Instance.Play(SoundTrack.GetAnyWithKey(ImpactSFXKey));

                    Destroy();
                }
            }
            else
            {
                if (colliderChar != null)
                {
                    if (colliderChar.RigidBody.Force.Magnitude <= 300)
                        colliderChar.RigidBody.AddForce(e.Collider.RigidBody.Front.Opposite * 200);
                }
            }
        }

        /// <summary>
        /// Evento a ser disparado quando o projétil continua em colisão com outro objeto
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações da colisão</param>
        protected override void OnCollisionStay(object sender, CollisionEventArgs e)
        {
            // Override para garantir que nada seja feito
        }

        /// <summary>
        /// Evento a ser disparado quando o projétil sai de colisão com outro objeto
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações da colisão</param>
        protected override void OnCollisionLeave(object sender, CollisionEventArgs e)
        {
            // Override para garantir que nada seja feito
        }
        #endregion
    }
}
