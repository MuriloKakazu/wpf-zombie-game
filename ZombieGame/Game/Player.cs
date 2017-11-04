using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;
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
        /// Personagem do jogador
        /// </summary>
        public Character Character { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="playerNumber">Número do jogador</param>
        public Player(int playerNumber)
        {
            PlayerNumber = playerNumber;
            Character = new Character(string.Format("Player{0}", playerNumber.ToString()), Tags.Player);
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
            var v1 = new Vector(x);
            var v2 = new Vector(0, y);
            if (x != 0 && y != 0)
            {
                if (x < 0 && y < 0)
                {
                    x = (float)Math.Cos(v1.AngleBetween(v2)) * x;
                    y = (float)Math.Cos(v1.AngleBetween(v2)) * y;
                }
                else if (x > 0 && y > 0)
                {
                    x = (float)Math.Cos(v2.AngleBetween(v1)) * x;
                    y = (float)Math.Cos(v2.AngleBetween(v1)) * y;
                }
                else if (x < 0 && y > 0)
                {
                    x = (float)Math.Cos(v1.AngleBetween(v2)) * x;
                    y = (float)Math.Cos(v1.AngleBetween(v2)) * y;
                }
                else if (x > 0 && y < 0)
                {
                    x = (float)Math.Cos(v2.AngleBetween(v1)) * x;
                    y = (float)Math.Cos(v2.AngleBetween(v1)) * y;
                }
            }
            var r = new Vector(x, y);
            var speedMult = 150f;
            if (Character.IsSprinting)
                speedMult += 100;
            Character.RigidBody.SetVelocity(r * speedMult);
            if (r.Magnitude != 0)
                Character.FacingDirection = r.Normalized;
        }
    }
}
