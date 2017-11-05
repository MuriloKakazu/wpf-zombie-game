using ZombieGame.Game.Enums;
using ZombieGame.Game.Prefabs.Weapons;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public abstract class Weapon
    {
        #region Properties
        /// <summary>
        /// Dano da arma por projétil
        /// </summary>
        public float Damage { get { return Projectile.OfType(ProjectileType).HitDamage; } }
        /// <summary>
        /// Taxa de disparo de projéteis da arma por minuto
        /// </summary>
        public float FireRate { get; protected set; }
        /// <summary>
        /// Quantia de munição da arma
        /// </summary>
        public int Ammo { get; set; }
        /// <summary>
        /// O tempo para recarregar a arma
        /// </summary>
        public float ReloadTime { get; set; }
        /// <summary>
        /// Módulo da velocidade do projétil da arma
        /// </summary>
        public float BulletVelocity { get { return Projectile.OfType(ProjectileType).SpeedMagnitude; } }
        /// <summary>
        /// Tempo de espera entre cada disparo de projéteis, em segundos
        /// </summary>
        public float CoolDownTime
        {
            get
            {
                if (FireRate > 1000)
                    return 60 / 1000;
                else if (FireRate < 30)
                    return 60 / 30;
                return 60 / FireRate;
            }
        }
        /// <summary>
        /// Retorna se a arma está em tempo de espera entre os disparos
        /// </summary>
        public bool IsCoolingDown { get; protected set; }
        /// <summary>
        /// Diferença de tempo desde o último disparo, em segundos
        /// </summary>
        public float DeltaT { get; protected set; }
        /// <summary>
        /// Tipo da arma
        /// </summary>
        public WeaponTypes Type { get; set; }
        /// <summary>
        /// Tipo de projétil da arma
        /// </summary>
        public ProjectileTypes ProjectileType { get; set; }
        #endregion

        #region Methods
        public Weapon(WeaponTypes type, ProjectileTypes pType)
        {
            Type = type;
            ProjectileType = pType;
            GameMaster.UpdateTimer.Elapsed += UpdateTimer_Elapsed;
        }

        public void StartCoolDown()
        {
            IsCoolingDown = true;
        }

        protected virtual void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DeltaT += Time.Delta;

            if (DeltaT >= CoolDownTime)
            {
                IsCoolingDown = false;
                DeltaT = 0;
            }
        }

        public void Destroy()
        {
            GameMaster.UpdateTimer.Elapsed -= UpdateTimer_Elapsed;
        }
        #endregion
    }
}
