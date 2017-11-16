﻿using System;
using System.Xml.Serialization;
using ZombieGame.Game.Entities;
using ZombieGame.Game.Enums;
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
        public Character Character { get; protected set; }
        /// <summary>
        /// Retorna se o jogador está ativo
        /// </summary>
        public bool IsPlaying { get; protected set; }
        #endregion

        #region Methods
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="playerNumber">Número do jogador</param>
        public Player(int playerNumber, string name)
        {
            PlayerNumber = playerNumber;
            Character = new Character(name, Tags.Player);
            Character.SetHealth(100);
            Character.LoadSprite(GlobalPaths.CharacterSprites + "player" + playerNumber + ".png");
            Time.HighPriorityTimer.Elapsed += UpdateTimer_Elapsed;
        }

        /// <summary>
        /// Evento a ser disparado quando o timer de atualização é decorrido
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações do evento</param>
        private void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Update();
        }

        /// <summary>
        /// Método que aciona funções que precisam ser disparadas constantemente
        /// </summary>
        public void Update()
        {
            if (!Character.IsStunned)
            {
                Character.IsSprinting = Convert.ToBoolean(Input.GetAxis(AxisTypes.Sprint, PlayerNumber));
                Character.IsFiring = Convert.ToBoolean(Input.GetAxis(AxisTypes.Fire, PlayerNumber));
                var x = Input.GetAxis(AxisTypes.Horizontal, PlayerNumber);
                var y = Input.GetAxis(AxisTypes.Vertical, PlayerNumber);
                var r = new Vector(x, y);
                var speedMult = 20f;
                if (Character.IsSprinting)
                    speedMult += 10;
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

                if (Character.IsFiring && !Character.Weapon.IsCoolingDown)
                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        Character.ShootAt(Character.RigidBody.Front);
                    });
            }
            else
            {
                Character.RigidBody.SetVelocity(Vector.Zero);
            }
        }
        #endregion
    }
}
