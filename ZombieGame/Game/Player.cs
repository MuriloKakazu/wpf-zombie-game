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
            Character.RigidBody.Stop();
            Character.IsSprinting = Convert.ToBoolean(Input.GetAxis(AxisTypes.Sprint, PlayerNumber));
            Character.IsFiring = Convert.ToBoolean(Input.GetAxis(AxisTypes.Fire, PlayerNumber));
            if (Character.IsSprinting)
            {
                Character.RigidBody.AddVelocity(new Vector(Input.GetAxis(AxisTypes.Horizontal, PlayerNumber)) * 500);
                Character.RigidBody.AddVelocity(new Vector(0, Input.GetAxis(AxisTypes.Vertical, PlayerNumber)) * 500);
            }
            else
            {
                Character.RigidBody.AddVelocity(new Vector(Input.GetAxis(AxisTypes.Horizontal, PlayerNumber)) * 250);
                Character.RigidBody.AddVelocity(new Vector(0, Input.GetAxis(AxisTypes.Vertical, PlayerNumber)) * 250);
            }
        }
    }
}
