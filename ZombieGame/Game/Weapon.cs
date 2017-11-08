using System.Xml.Serialization;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public class Weapon
    {
        #region Properties
        /// <summary>
        /// Nome da arma
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Dano da arma por segundo
        /// </summary>
        public float DPS { get { return Projectile.HitDamage * FireRate / 60; } }
        /// <summary>
        /// Taxa de disparo de projéteis da arma por minuto
        /// </summary>
        public float FireRate { get; set; }
        /// <summary>
        /// Quantia de munição da arma
        /// </summary>
        public int Ammo { get; set; }
        /// <summary>
        /// O tempo para recarregar a arma
        /// </summary>
        public float ReloadTime { get; set; }
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
        [XmlIgnore]
        public bool IsCoolingDown { get; set; }
        /// <summary>
        /// Diferença de tempo desde o último disparo, em segundos
        /// </summary>
        [XmlIgnore]
        public float DeltaT { get; set; }
        /// <summary>
        /// Tipo da arma
        /// </summary>
        public WeaponTypes WeaponType { get; set; }
        /// <summary>
        /// Tipos de projéteis aceitos pela arma
        /// </summary>
        public ProjectileTypes[] AcceptedProjectileTypes { get; set; }
        /// <summary>
        /// Projétil atual da arma
        /// </summary>
        [XmlIgnore]
        public Projectile Projectile { get; protected set; }
        #endregion

        #region Methods
        /// <summary>
        /// ctor
        /// </summary>
        public Weapon()
        {
            Projectile = new Projectile();
        }

        /// <summary>
        /// Redefine o projétil atual da arma
        /// </summary>
        /// <param name="p">Novo projétil</param>
        public void SetProjectile(Projectile p)
        {
            Projectile = p;
        }

        /// <summary>
        /// Inicia o processo de resfriamento da arma
        /// </summary>
        public void StartCoolDown()
        {
            Time.InternalTimer.Elapsed += UpdateTimer_Elapsed;
            IsCoolingDown = true;
        }

        /// <summary>
        /// Evento a ser disparado quando o intervalo do timer de atualização é decorrido
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito do evento</param>
        protected virtual void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DeltaT += Time.Delta;

            if (DeltaT >= CoolDownTime)
            {
                IsCoolingDown = false;
                DeltaT = 0;
                Time.InternalTimer.Elapsed -= UpdateTimer_Elapsed;
            }
        }

        /// <summary>
        /// Destrói o objeto, liberando memória
        /// </summary>
        public void Destroy()
        {
            Time.InternalTimer.Elapsed -= UpdateTimer_Elapsed;
            Projectile.Destroy();
        }

        /// <summary>
        /// Retorna um clone profundo da instância atual
        /// </summary>
        /// <returns>Weapon</returns>
        public Weapon Clone()
        {
            var copy = new Weapon()
            {
                AcceptedProjectileTypes = AcceptedProjectileTypes,
                Ammo = Ammo,
                DeltaT = DeltaT,
                FireRate = FireRate,
                IsCoolingDown = IsCoolingDown,
                Name = Name,
                Projectile = Projectile.Clone(),
                ReloadTime = ReloadTime,
                WeaponType = WeaponType
            };
            return copy;
        }
        #endregion
    }
}
