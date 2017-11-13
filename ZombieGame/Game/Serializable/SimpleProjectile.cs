using System;
using System.Xml.Serialization;
using ZombieGame.Game.Entities;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Interfaces;
using ZombieGame.Physics;

namespace ZombieGame.Game.Serializable
{
    [Serializable]
    public sealed class SimpleProjectile : ISerializableProjectile
    {
        /// <summary>
        /// Nome do projétil
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Retorna se o projétil atordoa com o impacto
        /// </summary>
        public bool IsStunner { get; set; }
        /// <summary>
        /// Retorna se o projétil explode com o impacto
        /// </summary>
        public bool IsExplosive { get; set; }
        /// <summary>
        /// Dano de impacto do projétil
        /// </summary>
        public float HitDamage { get; set; }
        /// <summary>
        /// Tipo do projétil
        /// </summary>
        public ProjectileTypes Type { get; set; }
        /// <summary>
        /// Módulo da velocidade do projétil
        /// </summary>
        public float SpeedMagnitude { get; set; }
        /// <summary>
        /// Coeficiente de ação do projétil sobre um corpo
        /// </summary>
        public float KnockbackMagnitude { get; set; }
        /// <summary>
        /// Tempo em milisegundos que o projétil atordoa seu alvo
        /// </summary>
        public float StunTimeMs { get; set; }
        /// <summary>
        /// Nome do arquivo de sprite do projétil
        /// </summary>
        public string SpriteFileName { get; set; }
        /// <summary>
        /// Massa do projétil
        /// </summary>
        public float Mass { get; set; }
        /// <summary>
        /// Tamanho do projétil
        /// </summary>
        public Vector Size { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public SimpleProjectile() { }

        /// <summary>
        /// Monta uma instância de projétil
        /// </summary>
        /// <returns>Projectile</returns>
        public Projectile Mount()
        {
            return Projectile.Mount(this);
        }
    }
}
