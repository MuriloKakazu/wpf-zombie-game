using System;
using System.Collections.Generic;
using System.Timers;
using ZombieGame.Game.Enums;
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
        /// Direção a qual o personagem está voltado
        /// </summary>
        public Vector FacingDirection { get; set; }
        /// <summary>
        /// Retorna se o personagem está correndo
        /// </summary>
        public bool IsSprinting { get; set; }
        /// <summary>
        /// Retorna se o personagem está atacando/atirando
        /// </summary>
        public bool IsFiring { get; set; }
        /// <summary>
        /// Retorna se o personagem pertence a um jogador
        /// </summary>
        public bool IsAPlayer { get { return Tag == Tags.Player; } }
        #endregion

        #region Methods
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">Nome do personagem</param>
        /// <param name="tag">Tag do personagem</param>
        public Character(string name, Tags tag) : base(name, tag)
        {
            Weapon = new Weapon();
            FacingDirection = Vector.Zero;
            Characters.Add(this);
        }

        /// <summary>
        /// Retorna um conjunto de personagens no raio especificado
        /// </summary>
        /// <param name="radius">Raio de procura</param>
        /// <returns>Entity(Array)</returns>
        public Character[] GetNearbyCharacters(float radius)
        {
            List<Character> characters = new List<Character>();
            foreach (var c in Characters)
                if ((c.RigidBody.CenterPoint + RigidBody.CenterPoint).Magnitude <= radius)
                    characters.Add(c);
            return characters.ToArray();
        }

        #endregion
    }
}
