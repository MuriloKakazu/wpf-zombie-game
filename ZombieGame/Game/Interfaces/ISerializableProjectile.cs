﻿using ZombieGame.Game.Enums;
using ZombieGame.Physics;

namespace ZombieGame.Game.Interfaces
{
    public interface ISerializableProjectile
    {
        /// <summary>
        /// Nome do projétil
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Retorna se o projétil atordoa com o impacto
        /// </summary>
        bool IsStunner { get; set; }
        /// <summary>
        /// Retorna se o projétil explode com o impacto
        /// </summary>
        bool IsExplosive { get; set; }
        /// <summary>
        /// Dano de impacto do projétil
        /// </summary>
        float HitDamage { get; set; }
        /// <summary>
        /// Tipo do projétil
        /// </summary>
        ProjectileTypes Type { get; set; }
        /// <summary>
        /// Módulo da velocidade do projétil
        /// </summary>
        float SpeedMagnitude { get; set; }
        /// <summary>
        /// Coeficiente de ação do projétil sobre um corpo
        /// </summary>
        float KnockbackMagnitude { get; set; }
        /// <summary>
        /// Tempo em milisegundos que o projétil atordoa seu alvo
        /// </summary>
        float StunTimeMs { get; set; }
        /// <summary>
        /// Nome do arquivo de sprite do projétil
        /// </summary>
        string SpriteFileName { get; set; }
        /// <summary>
        /// Massa do projétil
        /// </summary>
        float Mass { get; set; }
        /// <summary>
        /// Tamanho do projétil
        /// </summary>
        Vector Size { get; set; }

        /// <summary>
        /// Monta uma instância de projétil
        /// </summary>
        /// <returns>Projectile</returns>
        Projectile Mount();
    }
}