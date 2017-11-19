using System;
using System.Xml.Serialization;
using ZombieGame.Audio;
using ZombieGame.Game.Entities;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Interfaces;
using ZombieGame.Game.Prefabs.Audio;
using ZombieGame.Game.Prefabs.Entities;
using ZombieGame.Game.Serializable;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public class Weapon
    {
        #region Properties
        /// <summary>
        /// Nome da arma
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// Dano da arma por segundo
        /// </summary>
        public float DPS { get { return Projectile.HitDamage * FireRate / 60; } }
        /// <summary>
        /// Taxa de disparo de projéteis da arma por minuto
        /// </summary>
        public float FireRate { get; protected set; }
        /// <summary>
        /// Quantia de munição da arma
        /// </summary>
        public int MagSize { get; protected set; }
        public int Ammo { get; protected set; }
        /// <summary>
        /// O tempo para recarregar a arma em ms
        /// </summary>
        public float ReloadTime { get; protected set; }
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
        public bool IsReloading { get; protected set; }
        /// <summary>
        /// Diferença de tempo desde o último disparo, em segundos
        /// </summary>
        protected float DeltaT { get; set; }
        /// <summary>
        /// Tipo da arma
        /// </summary>
        public WeaponTypes Type { get; protected set; }
        /// <summary>
        /// Tipos de projéteis aceitos pela arma
        /// </summary>
        public ProjectileTypes[] AcceptedProjectileTypes { get; protected set; }
        /// <summary>
        /// Projétil atual da arma
        /// </summary>
        public Projectile Projectile { get; protected set; }
        public Character Owner { get; set; }
        public string SoundFXKey { get; protected set; }
        public bool HasProjectile { get { return Projectile != null; } }
        #endregion

        #region Methods
        /// <summary>
        /// Monta uma instância do objeto Weapon
        /// </summary>
        /// <returns>Projectile</returns>
        public static Weapon Mount(ISerializableWeapon source)
        {
            Weapon w = new Weapon()
            {
                AcceptedProjectileTypes = source.AcceptedProjectileTypes,
                MagSize = source.MagSize,
                Ammo = source.MagSize,
                Name = source.Name,
                ReloadTime = source.ReloadTime,
                FireRate = source.FireRate,
                Type = source.Type,
                SoundFXKey = source.SoundFXKey
            };
            return w;
        }

        /// <summary>
        /// ctor
        /// </summary>
        public Weapon()
        {
            Time.HighFrequencyTimer.Elapsed += HighFrequencyTimer_Elapsed;
        }

        /// <summary>
        /// Atira em uma direção
        /// </summary>
        /// <param name="direction">Direção</param>
        public virtual void ShootAt(Vector direction)
        {
            if (Projectile == null)
            {
                SoundPlayer.Instance.Play(new NoAmmo());
                Ammo = 0;
                return;
            }

            StartCoolDown();
            SoundPlayer.Instance.Play(SoundTrack.GetAnyWithKey(SoundFXKey));
            var db = Database.Weapons;
            Projectile p = Projectile.Clone();
            p.Owner = Owner;

            if (Type == WeaponTypes.Shotgun)
            {
                Random r = new Random();
                for (int i = -5; i < 5; i++)
                {
                    p = Projectile.Clone();
                    p.Owner = Owner;
                    if (i <= 0)
                        p.Launch(new Vector(direction.X - r.NextDouble() * 0.5, direction.Y - r.NextDouble() * 0.5));
                    else
                        p.Launch(new Vector(direction.X + r.NextDouble() * 0.5, direction.Y + r.NextDouble() * 0.5));
                }
            }
            else
                p.Launch(direction);

            Ammo--;
        }

        public void Reload()
        {
            IsReloading = true;
        }

        /// <summary>
        /// Redefine o projétil atual da arma
        /// </summary>
        /// <param name="p">Novo projétil</param>
        public void SetProjectile(Projectile p)
        {
            if (Projectile != null)
                Projectile.MarkAsNoLongerNeeded();
            
            Projectile = p;
        }

        /// <summary>
        /// Inicia o processo de resfriamento da arma
        /// </summary>
        public void StartCoolDown()
        {
            IsCoolingDown = true;
        }

        /// <summary>
        /// Evento a ser disparado quando o intervalo do timer de atualização é decorrido
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito do evento</param>
        protected virtual void HighFrequencyTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (IsCoolingDown)
            {
                DeltaT += Time.Delta;

                if (DeltaT >= CoolDownTime)
                {
                    IsCoolingDown = false;
                    DeltaT = 0;
                }
            }
            else if (IsReloading)
            {
                DeltaT += Time.Delta;

                if (DeltaT >= ReloadTime / 1000)
                {
                    IsReloading = false;
                    Ammo = MagSize;
                    DeltaT = 0;
                }
            }
        }

        /// <summary>
        /// Destrói o objeto, liberando memória
        /// </summary>
        public void Destroy()
        {
            Time.HighFrequencyTimer.Elapsed -= HighFrequencyTimer_Elapsed;
            if (HasProjectile)
                Projectile.MarkAsNoLongerNeeded();
        }

        /// <summary>
        /// Retorna um clone profundo da instância atual
        /// </summary>
        /// <returns>Weapon</returns>
        //public Weapon Clone()
        //{
        //    var copy = new Weapon()
        //    {
        //        AcceptedProjectileTypes = AcceptedProjectileTypes,
        //        Ammo = Ammo,
        //        DeltaT = DeltaT,
        //        FireRate = FireRate,
        //        IsCoolingDown = IsCoolingDown,
        //        Name = Name,
        //        Projectile = Projectile.Clone(),
        //        ReloadTime = ReloadTime,
        //        WeaponType = WeaponType
        //    };
        //    return copy;
        //}
        #endregion
    }
}
