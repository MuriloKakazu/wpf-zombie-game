using System;
using System.Xml.Serialization;
using ZombieGame.Audio;
using ZombieGame.Game.Entities;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Prefabs.Audio;
using ZombieGame.IO;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public class Player
    {
        #region Properties
        /// <summary>
        /// Número do jogador
        /// </summary>
        public int PlayerNumber { get; protected set; }
        /// <summary>
        /// Retorna se o jogador está sendo controlado por uma pessoa
        /// </summary>
        public bool IsHuman { get; protected set; }
        /// <summary>
        /// Personagem do jogador
        /// </summary>
        public Character Character { get; set; }
        /// <summary>
        /// Retorna se o jogador está ativo
        /// </summary>
        public bool IsPlaying { get; set; }
        #endregion

        #region Methods

        public static Player GetByCharacterHashcode(Guid hashcode)
        {
            foreach (var p in GameMaster.Players)
                if (p.Character != null && p.Character.Hash == hashcode)
                    return p;
            return null;
        }

        public Player()
        {
            
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="playerNumber">Número do jogador</param>
        public Player(int playerNumber, string name)
        {
            PlayerNumber = playerNumber;
            Character = new Character(name, Tags.Player);
            Character.MaxHealth = 100;
            Character.SetHealth(100);
            Character.LoadSprite(GlobalPaths.CharacterSprites + "player" + playerNumber + ".png");
            Time.HighFrequencyTimer.Elapsed += UpdateTimer_Elapsed;
        }

        /// <summary>
        /// Evento a ser disparado quando o timer de atualização é decorrido
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações do evento</param>
        private void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate { Update(); });
        }

        /// <summary>
        /// Método que aciona funções que precisam ser disparadas constantemente
        /// </summary>
        public void Update()
        {
            if (!Character.IsStunned && IsPlaying)
            {
                Character.IsSprinting = Convert.ToBoolean(Input.GetAxis(AxisTypes.Sprint, PlayerNumber));
                Character.IsFiring = Convert.ToBoolean(Input.GetAxis(AxisTypes.Fire, PlayerNumber));
                bool weaponReloadRequest = Convert.ToBoolean(Input.GetAxis(AxisTypes.Reload, PlayerNumber));
                var x = Input.GetAxis(AxisTypes.Horizontal, PlayerNumber);
                var y = Input.GetAxis(AxisTypes.Vertical, PlayerNumber);
                var r = new Vector(x, y);
                var speedMult = 20f;
                if (Character.IsSprinting)
                    speedMult += 8;
                if (x != 0 && y != 0)
                {
                    var mag = r.Magnitude;
                    r.X = (float)Math.Abs(Math.Sin(speedMult / mag)) * r.X;
                    r.Y = (float)Math.Abs(Math.Sin(speedMult / mag)) * r.Y;
                }

                Character.RigidBody.SetVelocity(r.Normalized * speedMult);
                if (r.Magnitude > 0)
                {
                    if (Character.RigidBody.Acceleration.Magnitude > 0)
                        Character.RigidBody.Acceleration.Approximate(Vector.Zero, 10);
                    Character.RigidBody.SetForce(Character.RigidBody.Force / 1.1f);
                    Character.RigidBody.PointAt(r);
                }

                if (Character.IsFiring && !Character.Weapon.IsCoolingDown && !Character.Weapon.IsReloading)
                {
                    if (Character.Weapon.Ammo > 0)
                    {
                        Character.Weapon.ShootAt(Character.RigidBody.Front);

                        if (Character.Weapon.Type == WeaponTypes.Shotgun || Character.Weapon.Type == WeaponTypes.RocketLauncher)
                            Character.Stun(250);
                    }
                    else
                        SoundPlayer.Instance.Play(new NoAmmo());
                }

                if (weaponReloadRequest && !Character.Weapon.IsReloading)
                {
                    Character.Weapon.Reload();
                    SoundPlayer.Instance.Play(new WeaponReload());
                }
            }
            else
            {
                Character.RigidBody.SetVelocity(Vector.Zero);
            }
        }
        #endregion
    }
}
