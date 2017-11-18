﻿using System;
using System.Collections.Generic;
using ZombieGame.Audio;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;

namespace ZombieGame.Game.Entities
{
    public class Character : Entity
    {
        #region Properties
        /// <summary>
        /// Lista estática copntendo todos os personagens ativos
        /// </summary>
        protected static List<Character> Characters = new List<Character>();

        /// <summary>
        /// Arma do personagem
        /// </summary>
        public virtual Weapon Weapon { get; protected set; }
        /// <summary>
        /// Retorna se o personagem está correndo
        /// </summary>
        public virtual bool IsSprinting { get; set; }
        /// <summary>
        /// Retorna se o personagem está atacando/atirando
        /// </summary>
        public virtual bool IsFiring { get; set; }
        /// <summary>
        /// Retorna se o personagem está vivo
        /// </summary>
        public virtual bool IsAlive { get { return Health > 0; } }
        /// <summary>
        /// Retorna o nível do personagem
        /// </summary>
        public virtual int Level { get; protected set; }
        /// <summary>
        /// Retorna a saúde do personagem
        /// </summary>
        public virtual float Health { get; protected set; }
        /// <summary>
        /// Retorna a experiência adquirida pelo personagem
        /// </summary>
        public virtual int Experience { get; protected set; }
        /// <summary>
        /// Retorna se o personagem está atordoado
        /// </summary>
        public virtual bool IsStunned { get; protected set; }
        /// <summary>
        /// Tempo que o personagem passou atordoado em ms
        /// </summary>
        protected virtual float TimeStunned { get; set; }
        /// <summary>
        /// Tempo que o atordoamento deve durar em ms
        /// </summary>
        protected virtual float TargetStunTime { get; set; }
        protected virtual bool HasWeapon { get { return Weapon != null; } }
        #endregion

        #region Methods
        /// <summary>
        /// Retorna todos os personagens ativos
        /// </summary>
        /// <returns>Character(Array)</returns>
        public new static Character[] Instances
        {
            get { return Characters.ToArray(); }
        }

        /// <summary>
        /// Retorna um conjunto de personagens no raio especificado
        /// </summary>
        /// <param name="radius">Raio de procura</param>
        /// <returns>Character(Array)</returns>
        public static Character[] GetNearbyCharacters(Vector pos, float radius, int maxValue)
        {
            try
            {
                List<Character> characters = new List<Character>();
                foreach (var c in Characters.ToArray())
                    if ((c.RigidBody.CenterPoint - pos).Magnitude <= radius && characters.Count < maxValue)
                        characters.Add(c);
                return characters.ToArray();
            }
            catch { return null; }
        }

        /// <summary>
        /// Retorna uma instância de personagem de acordo com o Hashcode
        /// </summary>
        /// <param name="hash">Código de identificação do objeto</param>
        /// <returns>Character</returns>
        public static Character FromHashCode(Guid hash)
        {
            foreach (Character c in Characters.ToArray())
                if (c.Hash == hash)
                    return c;
            return null;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">Nome do personagem</param>
        /// <param name="tag">Tag do personagem</param>
        public Character(string name, Tags tag) : base(name, tag)
        {
            //Health = 50;
            //Level = 1;
            if (tag == Tags.Player)
            {
                Weapon = Database.Weapons[0].Mount();
                Weapon.SetProjectile(Database.Projectiles[7].Mount());
                SetZIndex(ZIndexes.Player);
            }
            else
                SetZIndex(ZIndexes.Enemy);
            Characters.Add(this);
            Show();
        }

        /// <summary>
        /// Atira em uma direção
        /// </summary>
        /// <param name="direction">Direção</param>
        public virtual void ShootAt(Vector direction)
        {
            Weapon.StartCoolDown();
            SoundPlayer.Instance.Play(SoundTrack.GetAnyWithKey(Weapon.SoundFXKey));
            var db = Database.Weapons;

            if (Weapon.Type == WeaponTypes.Shotgun)
            {
                Random r = new Random();
                for (int i = -5; i < 5; i++)
                {
                    Projectile p = Weapon.Projectile.Clone();
                    p.Owner = this;
                    if (i <= 0)
                        p.Launch(new Vector(direction.X - r.NextDouble() * 0.5, direction.Y - r.NextDouble() * 0.5));
                    else
                        p.Launch(new Vector(direction.X + r.NextDouble() * 0.5, direction.Y + r.NextDouble() * 0.5));
                }
            }
            else
            {
                Projectile p = Weapon.Projectile.Clone();
                p.Owner = this;
                p.Launch(direction);
            }
        }

        /// <summary>
        /// Retira pontos da saúde do personagem
        /// </summary>
        /// <param name="damager">Personagem que causou o dano</param>
        /// <param name="quantity">Quantidade de dano</param>
        public virtual void Damage(Entity damager, float quantity)
        {
            Health -= quantity;
            if (!IsAlive)
                Kill(killer: damager);
        }

        /// <summary>
        /// Define a vida do personagem
        /// </summary>
        /// <param name="quantity">Quantia</param>
        public virtual void SetHealth(float quantity)
        {
            Health = quantity;
            if (!IsAlive)
                Kill();
        }

        /// <summary>
        /// Adiciona pontos à saúde do personagem
        /// </summary>
        /// <param name="quantity">Quantia</param>
        public virtual void AddHealth(float quantity)
        {
            Health += quantity;
            if (!IsAlive)
                Kill();
        }

        /// <summary>
        /// Mata o personagem
        /// </summary>
        /// <param name="killer">Personagem que matou</param>
        public virtual void Kill(Entity killer)
        {
            if (!this.IsPlayer)
                Console.WriteLine("{0} was killed by {1}", this.Name, killer.Name);

            Kill();
        }

        /// <summary>
        /// Mata o personagem
        /// </summary>
        protected virtual void Kill()
        {
            Health = 0;
            if (!IsPlayer)
                MarkAsNoLongerNeeded();
        }

        /// <summary>
        /// Atordoa o personagem
        /// </summary>
        /// <param name="timeMs">Tempo em milisegundos</param>
        public virtual void Stun(float timeMs)
        {
            IsStunned = true;
            TimeStunned = 0;
            TargetStunTime = timeMs;
        }

        /// <summary>
        /// Método que aciona funções que precisam ser disparadas constantemente
        /// </summary>
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (IsStunned)
            {
                TimeStunned += Time.Delta * 1000;
                if (TimeStunned >= TargetStunTime)
                {
                    IsStunned = false;
                }
            }
        }

        /// <summary>
        /// Retorna um conjunto de personagens no raio especificado
        /// </summary>
        /// <param name="radius">Raio de procura</param>
        /// <returns>Character(Array)</returns>
        public virtual Character[] GetNearbyCharacters(float radius)
        {
            try
            {
                List<Character> characters = new List<Character>();
                foreach (var c in Characters.ToArray())
                    if ((c.RigidBody.CenterPoint + RigidBody.CenterPoint).Magnitude <= radius)
                        characters.Add(c);
                return characters.ToArray();
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Destrói o personagem
        /// </summary>
        public override void Destroy()
        {
            if (IsAlive)
                Kill();
            if (HasWeapon)
                Weapon.Destroy();
            Characters.Remove(this);
            base.Destroy();
        }
        #endregion
    }
}