using System;
using System.Collections.Generic;
using System.Timers;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Prefabs.Weapons;
using ZombieGame.Physics;

namespace ZombieGame.Game
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
        public Weapon Weapon { get; set; }
        /// <summary>
        /// Retorna se o personagem está correndo
        /// </summary>
        public bool IsSprinting { get; set; }
        /// <summary>
        /// Retorna se o personagem está atacando/atirando
        /// </summary>
        public bool IsFiring { get; set; }
        /// <summary>
        /// Retorna se o personagem está vivo
        /// </summary>
        public bool IsAlive { get { return Health > 0; } }
        /// <summary>
        /// Retorna a quantidade de dinheiro do personagem
        /// </summary>
        public int Money { get; set; }
        /// <summary>
        /// Retorna o nível do personagem
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// Retorna a saúde do personagem
        /// </summary>
        public float Health { get; protected set; }
        /// <summary>
        /// Retorna a experiência adquirida pelo personagem
        /// </summary>
        public int Experience { get; set; }
        /// <summary>
        /// Retorna se o personagem está atordoado
        /// </summary>
        public bool IsStunned { get; protected set; }
        /// <summary>
        /// Tempo que o personagem passou atordoado em ms
        /// </summary>
        protected float TimeStunned { get; set; }
        /// <summary>
        /// Tempo que o atordoamento deve durar em ms
        /// </summary>
        protected float TargetStunTime { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">Nome do personagem</param>
        /// <param name="tag">Tag do personagem</param>
        public Character(string name, Tags tag) : base(name, tag)
        {
            Health = 50;
            Level = 1;
            Weapon = new RPG();
            Characters.Add(this);
        }

        /// <summary>
        /// Atira um projétil na direção indicada
        /// </summary>
        /// <param name="direction">Direção do tiro</param>
        public virtual void ShootAt(Vector direction)
        {
            Weapon.StartCoolDown();
            Projectile p = Projectile.OfType(Weapon.ProjectileType);
            p.Owner = this;
            p.Launch(direction);
        }

        /// <summary>
        /// Diminui pontos de saúde do personagem
        /// </summary>
        /// <param name="damager">Origem do dano</param>
        /// <param name="quantity">Quantia de dano</param>
        public virtual void Damage(Character damager, float quantity)
        {
            Health -= quantity;
            if (!IsAlive)
                Kill(killer: damager);
        }

        /// <summary>
        /// Define a vida do personagem
        /// </summary>
        /// <param name="quantity">Valor da saúde</param>
        public virtual void SetHealth(float quantity)
        {
            Health = quantity;
            if (!IsAlive)
                Kill();
        }

        /// <summary>
        /// Aumenta pontos de saúde do personagem
        /// </summary>
        /// <param name="quantity">Valor a ser adicionado</param>
        public virtual void AddHealth(float quantity)
        {
            Health += quantity;
            if (!IsAlive)
                Kill();
        }

        /// <summary>
        /// Mata o personagem
        /// </summary>
        /// <param name="killer">Entidade que matou o personagem</param>
        public virtual void Kill(Character killer)
        {
            if (!this.IsPlayer)
                Console.WriteLine("{0} was killed by {1}", this.Name, killer.Name);

            Health = 0;
            if (!IsPlayer)
                Destroy();
        }

        /// <summary>
        /// Mata o personagem
        /// </summary>
        protected virtual void Kill()
        {
            Health = 0;
            if (!IsPlayer)
                Destroy();
        }

        /// <summary>
        /// Atordoa o personagem por um tempo limitado
        /// </summary>
        /// <param name="timeMs">Tempo, em milisegundos, que o personagem estará atordoado</param>
        public virtual void Stun(float timeMs)
        {
            IsStunned = true;
            TimeStunned = 0;
            TargetStunTime = timeMs;
        }

        /// <summary>
        /// Método que aciona funções que precisam ser disparadas constantemente
        /// </summary>
        protected override void Update()
        {
            base.Update();
            if (IsStunned)
            {
                TimeStunned += (float)Time.Delta * 1000;
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
        public Character[] GetNearbyCharacters(float radius)
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
        /// Retorna um conjunto de personagens no raio especificado
        /// </summary>
        /// <param name="radius">Raio de procura</param>
        /// <returns>Character(Array)</returns>
        public static Character[] GetNearbyCharacters(Vector pos, float radius)
        {
            try
            {
                List<Character> characters = new List<Character>();
                foreach (var c in Characters.ToArray())
                    if ((c.RigidBody.CenterPoint - pos).Magnitude <= radius)
                        characters.Add(c);
                return characters.ToArray();
            }
            catch { return null; }
        }

        /// <summary>
        /// Retorna uma instância de personagem de acordo com seu código de identificação
        /// </summary>
        /// <param name="hash">Código de identificação do personagem</param>
        /// <returns>Character</returns>
        public static Character FromHashCode(Guid hash)
        {
            foreach (Character c in Characters.ToArray())
                if (c.Hash == hash)
                    return c;
            return null;
        }

        /// <summary>
        /// Destrói o personagem
        /// </summary>
        public override void Destroy()
        {
            if (IsAlive)
                Kill();
            Weapon.Destroy();
            Characters.Remove(this);
            base.Destroy();
        }
        #endregion
    }
}
