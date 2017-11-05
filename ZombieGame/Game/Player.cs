using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;
using ZombieGame.IO;
using ZombieGame.IO.Serialization;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public class Player
    {
        /// <summary>
        /// Número do jogador
        /// </summary>
        public int PlayerNumber { get; set; }
        /// <summary>
        /// Retorna se o jogador está sendo controlado por uma pessoa
        /// </summary>
        public bool IsHuman { get; set; }
        /// <summary>
        /// Personagem do jogador
        /// </summary>
        public Character Character { get; set; }
        /// <summary>
        /// Retorna o noem de usuário do jogador
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Retorna se o jogador está ativo
        /// </summary>
        public bool IsPlaying { get; internal set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="playerNumber">Número do jogador</param>
        public Player(int playerNumber, string name)
        {
            PlayerNumber = playerNumber;
            Character = new Character(name, Tags.Player);
            Character.LoadSprite(GlobalPaths.CharacterSprites + "player" + playerNumber + ".png");
            GameMaster.UpdateTimer.Elapsed += UpdateTimer_Elapsed;
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
            Character.IsSprinting = Convert.ToBoolean(Input.GetAxis(AxisTypes.Sprint, PlayerNumber));
            Character.IsFiring = Convert.ToBoolean(Input.GetAxis(AxisTypes.Fire, PlayerNumber));
            var x = Input.GetAxis(AxisTypes.Horizontal, PlayerNumber);
            var y = Input.GetAxis(AxisTypes.Vertical, PlayerNumber);
            var r = new Vector(x, y);
            var speedMult = 150f;
            if (Character.IsSprinting)
                speedMult += 100;
            if (x != 0 && y != 0)
            {
                var mag = r.Magnitude;
                r.X = (float)Math.Abs(Math.Sin(speedMult / mag)) * r.X;
                r.Y = (float)Math.Abs(Math.Sin(speedMult / mag)) * r.Y;
            }
               
            if (Character.RigidBody.IsAccelerating)
            {
                Character.RigidBody.SetVelocity(r);
                Character.RigidBody.RemoveForce(Character.RigidBody.Force / 10f);
            }
            else
                Character.RigidBody.SetVelocity(r * speedMult);

            if (Character.IsFiring && !Character.Weapon.IsCoolingDown)
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    Character.LaunchProjectile();
                });
        }
    }
}
