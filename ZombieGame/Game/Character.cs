using System;
using System.Timers;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public class Character : Entity
    {
        #region Properties
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
        /// ctor
        /// </summary>
        /// <param name="name">Nome do personagem</param>
        /// <param name="tag">Tag do personagem</param>
        public Character(string name, Tags tag) : base(name, tag)
        {
            Weapon = new Weapon();
            FacingDirection = Vector.Zero;
        }

        #endregion
    }
}
